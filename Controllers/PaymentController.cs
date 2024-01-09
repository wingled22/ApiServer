using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using olappApi.Entities;
using olappApi.Model;

namespace olappApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly OlappContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(OlappContext context, ILogger<PaymentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private Transaction CreateTransaction(long loanID, decimal amt, long schedId, long clientID)
        {
            return new Transaction
            {
                LoanId = loanID,
                Amount = amt,
                PaymentDate = DateTime.Now.ToString("d"),
                ScheduleId = schedId,
                ClientId = clientID
            };
        }

        [HttpPost("PostPayment")]
        public IActionResult PostPayment(PaymentProcessingModel payment)
        {
            try
            {
                var stats = _context.Schedules.Where(q => q.Id == payment.SchedId).FirstOrDefault();

                if (stats == null)
                {
                    return Ok(new PaymentProcessingOutputMessage
                    {
                        Status = "Error",
                        Message = "Invalid input or missing payment information."
                    });
                }

                string errormessage = "";

                var ln = _context.Loans.Where(q => q.Id == stats.LoanId).FirstOrDefault();
                var clnt = _context.Clients.Where(q => q.Id == ln.ClientId).FirstOrDefault();


                decimal amtPaid = payment.Amount;
                decimal currentAmt = amtPaid;

                List<Transaction> transactions = new List<Transaction>();
                List<Schedule> schedules = _context.Schedules.Where(q => q.Id >= payment.SchedId).Where(q => q.LoanId == stats.LoanId).Where(q => q.Status != "Paid").ToList();

                bool okToSave = true;

                foreach (var item in schedules)
                {
                    if (currentAmt <= 0)
                    {
                        _logger.LogError("Amount sent is negative value");
                        break;
                    }

                    // if (schedules.Count() == 1 && item.Collectables < amtPaid)
                    // {
                    //     okToSave = false;
                    //     break;
                    // }


                    if (currentAmt < item.Collectables)
                    {
                        /*pay as a partial payment*/
                        decimal schedSummedPartialPayments = 0;
                        if (item.Status == "Partial")
                        {
                            var getTransactionBySchedId = _context.Transactions.Where(q => q.ScheduleId == item.Id).Select(s => (decimal)s.Amount).ToList();


                            if (getTransactionBySchedId.Count() == 0)
                            {
                                _logger.LogError("Partial payment summed to zero, data review needed", "Credits & Collection Management");
                                okToSave = false;
                                break;
                            }
                            schedSummedPartialPayments = getTransactionBySchedId.DefaultIfEmpty().Sum();

                            var testNum = ((decimal)item.Collectables - schedSummedPartialPayments) - currentAmt;
                            if (testNum <= 0)
                            {
                                item.Status = "Paid";
                            }
                            else
                            {
                                item.Status = "Partial";
                            }

                            transactions.Add(CreateTransaction((long)stats.LoanId, currentAmt, item.Id, clnt.Id));
                            currentAmt = currentAmt - ((decimal)item.Collectables - schedSummedPartialPayments);

                        }
                        else
                        {
                            item.Status = "Partial";
                            _context.SaveChanges();
                            transactions.Add(CreateTransaction((long)stats.LoanId, currentAmt, item.Id, clnt.Id));
                            currentAmt = currentAmt - ((decimal)item.Collectables - schedSummedPartialPayments);
                        }
                    }
                    else
                    {
                        /*pay as a full amt*/
                        decimal schedSummedPartialPayments = 0;

                        if (item.Status.ToLower() == "partial")
                        {
                            schedSummedPartialPayments = _context.Transactions.Where(q => q.ScheduleId == item.Id).Select(s => (decimal)s.Amount).Sum();
                            if (schedSummedPartialPayments == 0)
                            {
                                _logger.LogError("Partial payment summed to zero, data review needed");
                                okToSave = false;
                                break;
                            }

                            item.Status = "Paid";
                            transactions.Add(CreateTransaction((long)stats.LoanId, ((decimal)item.Collectables - schedSummedPartialPayments), item.Id, clnt.Id));
                            currentAmt = currentAmt - ((decimal)item.Collectables - schedSummedPartialPayments);
                        }
                        else
                        {
                            item.Status = "Paid";
                            transactions.Add(CreateTransaction((long)stats.LoanId, (decimal)item.Collectables, item.Id, clnt.Id));
                            currentAmt = currentAmt - (decimal)item.Collectables;

                        }

                    }
                }

                if (okToSave)
                {
                    _context.SaveChanges();
                    _context.Transactions.AddRange(transactions);
                    _context.SaveChanges();

                    var transIds = transactions.Select(s => (int)s.TransId).ToList();

                    var loan = _context.Loans.Where(q => q.Id == stats.LoanId).FirstOrDefault();
                    transactions = _context.Transactions.Where(q => q.LoanId == stats.LoanId).ToList();

                    if (loan != null || transactions != null)
                    {
                        decimal sumOfPayments = transactions.Select(s => (decimal)s.Amount).DefaultIfEmpty().Sum();
                        if (sumOfPayments == loan.LoanAmount)
                        {
                            loan.Status = "Paid";
                            _context.SaveChanges();
                        }
                    }

                    return Ok(new PaymentProcessingOutputMessage
                    {
                        Status = "Ok",
                        Message = "Payment Process Successfull"
                    });



                }
                else
                {
                    _logger.LogInformation("Try paying again but not in bulk.", "Credits & Collection Management");

                    return Ok(new PaymentProcessingOutputMessage
                    {
                        Status = "Error",
                        Message = "Payment processing failed. Please try again later."
                    });
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                return Ok(new PaymentProcessingOutputMessage
                {
                    Status = "Error",
                    Message = "An error occurred while processing the payment."
                });
            }
        }


        [HttpPost("PostPenalty")]
        public IActionResult PostPenalty(PenaltyPaymentProcessingModel payment)
        {
            try
            {

                var ln = _context.Loans.Where(q => q.Id == payment.LoanId).FirstOrDefault();

                if (ln == null)
                {
                    return Ok(new PaymentProcessingOutputMessage
                    {
                        Status = "Error",
                        Message = "Invalid input or missing payment information."
                    });
                }

                var clnt = _context.Clients.Where(q => q.Id == ln.ClientId).FirstOrDefault();


                decimal amtPaid = payment.Amount;
                decimal currentAmt = amtPaid;

                decimal pastPayment = _context.Transactions.Where(q => q.LoanId == ln.Id).Sum(s => (decimal?)s.Amount) ?? 0;

                decimal totalToPay = (decimal)(ln.LoanAmount + ln.TotalPenalty) - pastPayment;

                if (totalToPay < payment.Amount || payment.Amount == 0)
                {
                    return BadRequest(new PaymentProcessingOutputMessage
                    {
                        Status = "Error",
                        Message = "Invalid input or missing payment information."
                    });
                }

                if (totalToPay == payment.Amount)
                {
                    Transaction transaction = new Transaction
                    {
                        Amount = payment.Amount,
                        PaymentDate = DateTime.Now.ToString("d"),
                        LoanId = ln.Id
                    };

                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();

                    ln.Status = "Paid";
                    _context.Loans.Update(ln);
                    _context.SaveChanges();

                    List<Schedule> schedules = _context.Schedules.Where(x => x.LoanId == ln.Id).ToList();
                    foreach (var item in schedules)
                    {
                        item.Status = "Paid";
                    }
                    _context.Schedules.UpdateRange(schedules);
                    _context.SaveChanges();

                }
                else
                {
                    Transaction transaction = new Transaction
                    {
                        Amount = payment.Amount,
                        PaymentDate = DateTime.Now.ToString("d"),
                        LoanId = ln.Id
                    };

                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();

                }


                return Ok(new PaymentProcessingOutputMessage
                {
                    Status = "Ok",
                    Message = "Payment Process Successfull"
                });
            }
            catch (System.Exception)
            {

                return Ok(new PaymentProcessingOutputMessage
                {
                    Status = "Error",
                    Message = "Payment processing failed. Please try again later."
                });
            }
        }

    }
}