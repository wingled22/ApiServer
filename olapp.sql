-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Dec 29, 2023 at 02:04 PM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `olapp`
--

-- --------------------------------------------------------

--
-- Table structure for table `Client`
--

CREATE TABLE `Client` (
  `id` bigint(20) NOT NULL,
  `name` varchar(70) DEFAULT NULL,
  `gender` varchar(11) DEFAULT NULL,
  `email` varchar(70) DEFAULT NULL,
  `birthdate` varchar(70) DEFAULT NULL,
  `province` varchar(20) DEFAULT NULL,
  `municipal` varchar(20) DEFAULT NULL,
  `barangay` varchar(20) DEFAULT NULL,
  `purok` int(11) DEFAULT NULL,
  `address` varchar(200) DEFAULT NULL,
  `additionalAddressInfo` varchar(500) DEFAULT NULL,
  `city` varchar(11) DEFAULT NULL,
  `emailAddress` varchar(100) DEFAULT NULL,
  `contactNumber` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `Client`
--

INSERT INTO `Client` (`id`, `name`, `gender`, `email`, `birthdate`, `province`, `municipal`, `barangay`, `purok`, `address`, `additionalAddressInfo`, `city`, `emailAddress`, `contactNumber`) VALUES
(3, 'Full name', 'Female', 'test@email.com', '1991-12-05', '0722', NULL, '072211011', NULL, NULL, 'purok sampaguita', '072211', NULL, '091238753847'),
(8, 'Peter Griffin', 'Male', 'email2@test.com', '2023-12-05', '0722', NULL, '072211010', NULL, NULL, 'skina naay kan anan', '072211', NULL, '044545'),
(12, 'Crispoin', 'Male', 'testing@email.com', '2023-08-13', '0837', NULL, '083738007', NULL, NULL, 'esdfe', '083738', NULL, '234234'),
(13, 'Sample Client 12', 'Female', 'email3@test.com', '2023-11-28', '0722', NULL, '072221006', NULL, NULL, 'pap', '072221', NULL, '234324'),
(14, 'Shaq Manolo', 'Female', 'email4@test.com', '2023-12-14', '0722', NULL, '072211009', NULL, NULL, 'Victoe', '072211', NULL, '0234234');

-- --------------------------------------------------------

--
-- Table structure for table `Loan`
--

CREATE TABLE `Loan` (
  `Id` bigint(20) NOT NULL,
  `client_id` bigint(20) DEFAULT NULL,
  `type` varchar(255) DEFAULT NULL,
  `deductCBU` decimal(18,2) DEFAULT NULL,
  `deductInsurance` decimal(18,2) DEFAULT NULL,
  `loan_amount` decimal(18,2) DEFAULT NULL,
  `capital` decimal(18,2) DEFAULT NULL,
  `interest` decimal(18,2) DEFAULT NULL,
  `interested_amount` decimal(18,2) DEFAULT NULL,
  `loan_receivable` decimal(18,2) DEFAULT NULL,
  `no_payment` int(11) DEFAULT NULL,
  `status` varchar(255) DEFAULT NULL,
  `date_time` varchar(255) DEFAULT NULL,
  `due_Date` datetime DEFAULT NULL,
  `total_penalty` decimal(18,2) DEFAULT NULL,
  `added_interest` decimal(18,2) DEFAULT NULL,
  `other_fee` decimal(18,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `Loan`
--

INSERT INTO `Loan` (`Id`, `client_id`, `type`, `deductCBU`, `deductInsurance`, `loan_amount`, `capital`, `interest`, `interested_amount`, `loan_receivable`, `no_payment`, `status`, `date_time`, `due_Date`, `total_penalty`, `added_interest`, `other_fee`) VALUES
(3, 3, 'Daily', '100.00', '100.00', '6600.00', '6000.00', '10.00', '600.00', '5700.00', 60, 'Unpaid', '12/13/2023 11:23:35 PM', '2024-02-11 23:23:36', NULL, NULL, '100.00'),
(4, 8, 'Emergency', '100.00', '100.00', '11000.00', '10000.00', '10.00', '1000.00', '9700.00', 10, 'Unpaid', '12/20/2023 2:10:19 PM', '2024-02-28 14:10:19', NULL, NULL, '100.00'),
(5, 12, 'PO Cash', '100.00', '150.00', '11000.00', '10000.00', '10.00', '1000.00', '9550.00', 10, 'Unpaid', '12/26/2023 3:01:37 PM', NULL, NULL, NULL, '200.00'),
(6, 13, '', '100.00', '100.00', '5500.00', '5000.00', '10.00', '500.00', '4500.00', 10, 'Unpaid', '12/27/2023 10:49:42 PM', NULL, NULL, NULL, '300.00'),
(7, 14, 'Emergency', '200.00', '200.00', '11500.00', '10000.00', '15.00', '1500.00', '9300.00', 20, 'Unpaid', '12/27/2023 10:54:24 PM', '2024-05-15 22:54:24', NULL, NULL, '300.00');

-- --------------------------------------------------------

--
-- Table structure for table `Schedule`
--

CREATE TABLE `Schedule` (
  `Id` bigint(20) NOT NULL,
  `loan_id` bigint(20) DEFAULT NULL,
  `collectables` decimal(18,2) DEFAULT NULL,
  `date` datetime DEFAULT NULL,
  `status` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `Schedule`
--

INSERT INTO `Schedule` (`Id`, `loan_id`, `collectables`, `date`, `status`) VALUES
(1, 3, '110.00', '2023-12-14 23:24:31', 'Unpaid'),
(2, 3, '110.00', '2023-12-15 23:24:31', 'Unpaid'),
(3, 3, '110.00', '2023-12-16 23:24:31', 'Unpaid'),
(4, 3, '110.00', '2023-12-17 23:24:31', 'Unpaid'),
(5, 3, '110.00', '2023-12-18 23:24:31', 'Unpaid'),
(6, 3, '110.00', '2023-12-19 23:24:31', 'Unpaid'),
(7, 3, '110.00', '2023-12-20 23:24:31', 'Unpaid'),
(8, 3, '110.00', '2023-12-21 23:24:31', 'Unpaid'),
(9, 3, '110.00', '2023-12-22 23:24:31', 'Unpaid'),
(10, 3, '110.00', '2023-12-23 23:24:31', 'Unpaid'),
(11, 3, '110.00', '2023-12-24 23:24:31', 'Unpaid'),
(12, 3, '110.00', '2023-12-25 23:24:31', 'Unpaid'),
(13, 3, '110.00', '2023-12-26 23:24:31', 'Unpaid'),
(14, 3, '110.00', '2023-12-27 23:24:31', 'Unpaid'),
(15, 3, '110.00', '2023-12-28 23:24:31', 'Unpaid'),
(16, 3, '110.00', '2023-12-29 23:24:31', 'Unpaid'),
(17, 3, '110.00', '2023-12-30 23:24:31', 'Unpaid'),
(18, 3, '110.00', '2023-12-31 23:24:31', 'Unpaid'),
(19, 3, '110.00', '2024-01-01 23:24:31', 'Unpaid'),
(20, 3, '110.00', '2024-01-02 23:24:31', 'Unpaid'),
(21, 3, '110.00', '2024-01-03 23:24:31', 'Unpaid'),
(22, 3, '110.00', '2024-01-04 23:24:31', 'Unpaid'),
(23, 3, '110.00', '2024-01-05 23:24:31', 'Unpaid'),
(24, 3, '110.00', '2024-01-06 23:24:31', 'Unpaid'),
(25, 3, '110.00', '2024-01-07 23:24:31', 'Unpaid'),
(26, 3, '110.00', '2024-01-08 23:24:31', 'Unpaid'),
(27, 3, '110.00', '2024-01-09 23:24:31', 'Unpaid'),
(28, 3, '110.00', '2024-01-10 23:24:31', 'Unpaid'),
(29, 3, '110.00', '2024-01-11 23:24:31', 'Unpaid'),
(30, 3, '110.00', '2024-01-12 23:24:31', 'Unpaid'),
(31, 3, '110.00', '2024-01-13 23:24:31', 'Unpaid'),
(32, 3, '110.00', '2024-01-14 23:24:31', 'Unpaid'),
(33, 3, '110.00', '2024-01-15 23:24:31', 'Unpaid'),
(34, 3, '110.00', '2024-01-16 23:24:31', 'Unpaid'),
(35, 3, '110.00', '2024-01-17 23:24:31', 'Unpaid'),
(36, 3, '110.00', '2024-01-18 23:24:31', 'Unpaid'),
(37, 3, '110.00', '2024-01-19 23:24:31', 'Unpaid'),
(38, 3, '110.00', '2024-01-20 23:24:31', 'Unpaid'),
(39, 3, '110.00', '2024-01-21 23:24:31', 'Unpaid'),
(40, 3, '110.00', '2024-01-22 23:24:31', 'Unpaid'),
(41, 3, '110.00', '2024-01-23 23:24:31', 'Unpaid'),
(42, 3, '110.00', '2024-01-24 23:24:31', 'Unpaid'),
(43, 3, '110.00', '2024-01-25 23:24:31', 'Unpaid'),
(44, 3, '110.00', '2024-01-26 23:24:31', 'Unpaid'),
(45, 3, '110.00', '2024-01-27 23:24:31', 'Unpaid'),
(46, 3, '110.00', '2024-01-28 23:24:31', 'Unpaid'),
(47, 3, '110.00', '2024-01-29 23:24:31', 'Unpaid'),
(48, 3, '110.00', '2024-01-30 23:24:31', 'Unpaid'),
(49, 3, '110.00', '2024-01-31 23:24:31', 'Unpaid'),
(50, 3, '110.00', '2024-02-01 23:24:31', 'Unpaid'),
(51, 3, '110.00', '2024-02-02 23:24:31', 'Unpaid'),
(52, 3, '110.00', '2024-02-03 23:24:31', 'Unpaid'),
(53, 3, '110.00', '2024-02-04 23:24:31', 'Unpaid'),
(54, 3, '110.00', '2024-02-05 23:24:31', 'Unpaid'),
(55, 3, '110.00', '2024-02-06 23:24:31', 'Unpaid'),
(56, 3, '110.00', '2024-02-07 23:24:31', 'Unpaid'),
(57, 3, '110.00', '2024-02-08 23:24:31', 'Unpaid'),
(58, 3, '110.00', '2024-02-09 23:24:31', 'Unpaid'),
(59, 3, '110.00', '2024-02-10 23:24:31', 'Unpaid'),
(60, 3, '110.00', '2024-02-11 23:24:31', 'Unpaid'),
(61, 4, '1100.00', '2023-12-27 14:10:19', 'Unpaid'),
(62, 4, '1100.00', '2024-01-03 14:10:19', 'Unpaid'),
(63, 4, '1100.00', '2024-01-10 14:10:19', 'Unpaid'),
(64, 4, '1100.00', '2024-01-17 14:10:19', 'Unpaid'),
(65, 4, '1100.00', '2024-01-24 14:10:19', 'Unpaid'),
(66, 4, '1100.00', '2024-01-31 14:10:19', 'Unpaid'),
(67, 4, '1100.00', '2024-02-07 14:10:19', 'Unpaid'),
(68, 4, '1100.00', '2024-02-14 14:10:19', 'Unpaid'),
(69, 4, '1100.00', '2024-02-21 14:10:19', 'Unpaid'),
(70, 4, '1100.00', '2024-02-28 14:10:19', 'Unpaid'),
(71, 7, '575.00', '2024-01-03 22:54:24', 'Unpaid'),
(72, 7, '575.00', '2024-01-10 22:54:24', 'Unpaid'),
(73, 7, '575.00', '2024-01-17 22:54:24', 'Unpaid'),
(74, 7, '575.00', '2024-01-24 22:54:24', 'Unpaid'),
(75, 7, '575.00', '2024-01-31 22:54:24', 'Unpaid'),
(76, 7, '575.00', '2024-02-07 22:54:24', 'Unpaid'),
(77, 7, '575.00', '2024-02-14 22:54:24', 'Unpaid'),
(78, 7, '575.00', '2024-02-21 22:54:24', 'Unpaid'),
(79, 7, '575.00', '2024-02-28 22:54:24', 'Unpaid'),
(80, 7, '575.00', '2024-03-06 22:54:24', 'Unpaid'),
(81, 7, '575.00', '2024-03-13 22:54:24', 'Unpaid'),
(82, 7, '575.00', '2024-03-20 22:54:24', 'Unpaid'),
(83, 7, '575.00', '2024-03-27 22:54:24', 'Unpaid'),
(84, 7, '575.00', '2024-04-03 22:54:24', 'Unpaid'),
(85, 7, '575.00', '2024-04-10 22:54:24', 'Unpaid'),
(86, 7, '575.00', '2024-04-17 22:54:24', 'Unpaid'),
(87, 7, '575.00', '2024-04-24 22:54:24', 'Unpaid'),
(88, 7, '575.00', '2024-05-01 22:54:24', 'Unpaid'),
(89, 7, '575.00', '2024-05-08 22:54:24', 'Unpaid'),
(90, 7, '575.00', '2024-05-15 22:54:24', 'Unpaid');

-- --------------------------------------------------------

--
-- Table structure for table `Transaction`
--

CREATE TABLE `Transaction` (
  `trans_id` bigint(20) NOT NULL,
  `amount` decimal(18,2) DEFAULT NULL,
  `payment_date` varchar(255) DEFAULT NULL,
  `added_by` varchar(255) DEFAULT NULL,
  `client_id` bigint(20) DEFAULT NULL,
  `schedule_id` bigint(20) DEFAULT NULL,
  `loan_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `Client`
--
ALTER TABLE `Client`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `Loan`
--
ALTER TABLE `Loan`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `Schedule`
--
ALTER TABLE `Schedule`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `Transaction`
--
ALTER TABLE `Transaction`
  ADD PRIMARY KEY (`trans_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `Client`
--
ALTER TABLE `Client`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `Loan`
--
ALTER TABLE `Loan`
  MODIFY `Id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `Schedule`
--
ALTER TABLE `Schedule`
  MODIFY `Id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=91;

--
-- AUTO_INCREMENT for table `Transaction`
--
ALTER TABLE `Transaction`
  MODIFY `trans_id` bigint(20) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
