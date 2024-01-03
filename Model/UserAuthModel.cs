using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olappApi.Model
{
    public class UserAuthModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
    }
}