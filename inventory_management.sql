-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 06, 2025 at 11:21 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `inventory_management`
--

-- --------------------------------------------------------

--
-- Table structure for table `tblaccounts`
--

CREATE TABLE `tblaccounts` (
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `usertype` varchar(50) NOT NULL,
  `status` varchar(50) NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `datecreated` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblaccounts`
--

INSERT INTO `tblaccounts` (`username`, `password`, `usertype`, `status`, `createdby`, `datecreated`) VALUES
('admin', '123456', 'ADMINISTRATOR', 'ACTIVE', 'admin', '09/04/2025'),
('staff', '123456', 'PHARMACIST', 'ACTIVE', 'admin', '04/09/2025'),
('rhenn', '123456', 'ADMINISTRATOR', 'ACTIVE', 'admin', '10/04/2025');

-- --------------------------------------------------------

--
-- Table structure for table `tbladjustment`
--

CREATE TABLE `tbladjustment` (
  `products` varchar(50) NOT NULL,
  `quantity` varchar(20) DEFAULT NULL,
  `unitprice` varchar(50) DEFAULT NULL,
  `reason` text NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `dateadjusted` varchar(20) NOT NULL,
  `timeadjusted` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tbladjustment`
--

INSERT INTO `tbladjustment` (`products`, `quantity`, `unitprice`, `reason`, `createdby`, `dateadjusted`, `timeadjusted`) VALUES
('Alaxan FR Capsule', '10', NULL, 'Added 10 from the other branch', 'admin', '09/26/2025', '16:47:13'),
('Alaxan FR Capsule', NULL, '12.00', 'Price Increase from 11 to 12', 'admin', '09/26/2025', '16:48:49'),
('Ascof Lagundi Syrup 60ml', '50', NULL, 'From other branch', 'admin', '10/02/2025', '18:43:01'),
('Biogesic Syrup 60ml', '5', NULL, 'From other branch', 'admin', '10/03/2025', '15:44:51'),
('Alaxan FR Capsule', '-5', NULL, 'Expired', 'admin', '10/06/2025', '11:21:23 am'),
('Ascof Lagundi Syrup 60ml', '3', NULL, 'From other branch', 'admin', '10/06/2025', '11:25:27 am');

-- --------------------------------------------------------

--
-- Table structure for table `tblhistory`
--

CREATE TABLE `tblhistory` (
  `products` varchar(50) NOT NULL,
  `unitcost` varchar(20) NOT NULL,
  `datecreated` varchar(30) NOT NULL,
  `createdby` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblhistory`
--

INSERT INTO `tblhistory` (`products`, `unitcost`, `datecreated`, `createdby`) VALUES
('Ceelin Syrup 120ml', '100', '10/03/2025 11:30:44 pm', 'admin'),
('Ceelin Syrup 120ml', '100', '10/03/2025 11:30:54 pm', 'admin'),
('Ceelin Syrup 120ml', '90', '10/03/2025 11:45:41 pm', 'admin'),
('Ceelin Syrup 120ml', '85', '10/03/2025 11:46:00 pm', 'admin'),
('Gaviscon Liquid 120ml', '120', '10/04/2025 12:37:32 pm', 'admin'),
('Gaviscon Liquid 120ml', '120', '10/04/2025 01:12:19 pm', 'admin'),
('Alaxan FR Capsule', '4', '10/06/2025 11:13:21 am', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `tbllogs`
--

CREATE TABLE `tbllogs` (
  `datelog` varchar(20) NOT NULL,
  `timelog` varchar(20) NOT NULL,
  `module` varchar(50) NOT NULL,
  `action` varchar(20) NOT NULL,
  `performedto` varchar(50) NOT NULL,
  `performedby` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tbllogs`
--

INSERT INTO `tbllogs` (`datelog`, `timelog`, `module`, `action`, `performedto`, `performedby`) VALUES
('10/06/2025', '04:59:32 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/06/2025-045932-pm | TOTAL: 12.0', 'admin'),
('10/06/2025', '4:59:44 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/06/2025-045932-pm - 1 items', 'admin'),
('10/06/2025', '16:59:51', 'SALES REPORT', 'DELETE REFUND', 'ORDER ID: order-10/06/2025-045932-pm', 'admin'),
('10/06/2025', '05:00:04 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/06/2025-050004-pm | TOTAL: 12.0', 'admin'),
('10/06/2025', '05:00:20 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/06/2025-050020-pm | TOTAL: 360.', 'admin'),
('10/06/2025', '5:00:35 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/06/2025-050004-pm - 1 items', 'admin'),
('10/06/2025', '5:00:49 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/06/2025-050020-pm - 1 items', 'admin'),
('10/06/2025', '17:01:17', 'SALES REPORT', 'DELETE ALL', 'Daily sales and refunds for 10/06/2025', 'admin'),
('10/06/2025', '05:01:35 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/06/2025-050135-pm | TOTAL: 90.0', 'admin'),
('10/06/2025', '05:02:12 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/06/2025-050212-pm | TOTAL: 316.', 'admin'),
('10/06/2025', '5:02:33 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/06/2025-050212-pm - 1 items', 'admin'),
('10/06/2025', '5:02:52 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/06/2025-050135-pm - 1 items', 'admin'),
('10/06/2025', '05:03:25 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/06/2025-050325-pm | TOTAL: 160.', 'admin'),
('10/06/2025', '17:19:44', 'SALES REPORT', 'EXPORT', 'CSV Export: Daily - 10/06/2025', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `tblproducts`
--

CREATE TABLE `tblproducts` (
  `products` varchar(50) NOT NULL,
  `description` text NOT NULL,
  `unitprice` varchar(20) NOT NULL,
  `currentstock` varchar(20) NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `datecreated` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblproducts`
--

INSERT INTO `tblproducts` (`products`, `description`, `unitprice`, `currentstock`, `createdby`, `datecreated`) VALUES
('Mucotuss Forte', '', '7.00', '223', 'admin', '11/09/2025'),
('Neozep Forte Tablet', '', '8.00', '99', 'admin', '09/18/2025'),
('Tuseran Forte Capsule', '', '14.00', '97', 'admin', '09/18/2025'),
('Biogesic 500mg Tablet', '', '7.00', '172', 'admin', '09/19/2025'),
('Bioflu Tablet', '', '9.00', '130', 'admin', '09/19/2025'),
('Alaxan FR Capsule', '', '12.00', '102', 'admin', '09/19/2025'),
('Decolgen Forte Tablet', '', '8.50', '177', 'admin', '09/19/2025'),
('Paracetamol 500mg Tablet (Generic)', '', '2.00', '498', 'admin', '09/19/2025'),
('Ibuprofen 400mg Tablet', '', '4.50', '299', 'admin', '09/19/2025'),
('Kremil-S Tablet', '', '10.00', '98', 'admin', '09/19/2025'),
('Ascof Lagundi Syrup 60ml', '', '90.00', '147', 'admin', '09/19/2025'),
('Ventolin Inhaler 100mcg', '', '350.00', '29', 'admin', '09/19/2025'),
('Cetirizine 10mg Tablet', '', '6.00', '196', 'admin', '09/19/2025'),
('Claritin 10mg Tablet', '', '35.00', '75', 'admin', '09/19/2025'),
('Myra-E 400IU Capsule', '', '12.00', '149', 'admin', '09/19/2025'),
('Enervon Tablet', '', '10.00', '98', 'admin', '09/19/2025'),
('Stresstabs Capsule', '', '18.00', '99', 'admin', '09/19/2025'),
('Centrum Advance Tablet', '', '22.00', '59', 'admin', '09/19/2025'),
('Ascorbic Acid 500mg Tablet (Generic)', '', '4.00', '261', 'admin', '09/19/2025'),
('Fern-C 500mg Capsule', '', '13.00', '78', 'admin', '09/19/2025'),
('Ceelin Syrup 120ml', '', '140.00', '59', 'admin', '09/19/2025'),
('Diatabs Capsule', '', '9.00', '98', 'admin', '09/19/2025'),
('Dulcolax 5mg Tablet', '', '12.00', '47', 'admin', '09/19/2025'),
('Gaviscon Liquid 120ml', '', '180.00', '31', 'admin', '09/19/2025'),
('Hydrite Oral Rehydration Salt Sachet', '', '8.00', '148', 'admin', '09/19/2025'),
('Imodium Capsule', '', '22.00', '69', 'admin', '09/19/2025'),
('Mefenamic Acid 500mg Tablet', '', '4.00', '249', 'admin', '09/19/2025'),
('Buscopan Tablet', '', '16.00', '116', 'admin', '09/19/2025'),
('Robitussin DM Syrup 60ml', '', '95.00', '30', 'admin', '09/19/2025'),
('Tempra Forte 60ml (Paracetamol)', '', '120.00', '40', 'admin', '09/19/2025'),
('Biogesic Syrup 60ml', '', '110.00', '78', 'admin', '09/19/2025');

-- --------------------------------------------------------

--
-- Table structure for table `tblpurchase_order`
--

CREATE TABLE `tblpurchase_order` (
  `products` varchar(50) NOT NULL,
  `quantity` varchar(20) NOT NULL,
  `unitcost` varchar(20) NOT NULL,
  `totalcost` varchar(20) NOT NULL,
  `status` varchar(50) NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `datecreated` varchar(20) NOT NULL,
  `timecreated` varchar(20) NOT NULL,
  `supplier` varchar(50) NOT NULL,
  `datereceived` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblpurchase_order`
--

INSERT INTO `tblpurchase_order` (`products`, `quantity`, `unitcost`, `totalcost`, `status`, `createdby`, `datecreated`, `timecreated`, `supplier`, `datereceived`) VALUES
('Ceelin Syrup 120ml', '1', '85', '85.00', 'Received', 'admin', '10/03/2025', '11:46:00 pm', 'Test', '10/04/2025'),
('Gaviscon Liquid 120ml', '10', '120', '1200.00', 'Received', 'admin', '10/04/2025', '12:37:32 pm', 'Test', '10/04/2025'),
('Gaviscon Liquid 120ml', '3', '120', '360.00', 'Received', 'admin', '10/04/2025', '01:12:19 pm', 'Test', '10/04/2025'),
('Alaxan FR Capsule', '3', '4', '12.00', 'Received', 'admin', '10/06/2025', '11:13:21 am', 'Test', '10/06/2025');

-- --------------------------------------------------------

--
-- Table structure for table `tblrefunds`
--

CREATE TABLE `tblrefunds` (
  `orderid` varchar(50) NOT NULL,
  `products` varchar(50) NOT NULL,
  `quantity` varchar(50) NOT NULL,
  `unitprice` varchar(50) NOT NULL,
  `reason` text NOT NULL,
  `daterefunded` varchar(50) NOT NULL,
  `timerefunded` varchar(50) NOT NULL,
  `refundedby` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblrefunds`
--

INSERT INTO `tblrefunds` (`orderid`, `products`, `quantity`, `unitprice`, `reason`, `daterefunded`, `timerefunded`, `refundedby`) VALUES
('order-10/06/2025-050212-pm', 'Biogesic Syrup 60ml', '1', '110.00', 'Wrong quantity', '10/06/2025', '5:02:33 pm', 'admin'),
('order-10/06/2025-050135-pm', 'Ascof Lagundi Syrup 60ml', '1', '90.00', 'Wrong product', '10/06/2025', '5:02:52 pm', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `tblsales`
--

CREATE TABLE `tblsales` (
  `orderid` varchar(50) NOT NULL,
  `products` varchar(50) NOT NULL,
  `quantity` varchar(50) NOT NULL,
  `payment` varchar(50) NOT NULL,
  `paymentchange` varchar(50) NOT NULL,
  `totalcost` varchar(50) NOT NULL,
  `discounted` varchar(20) NOT NULL,
  `datecreated` varchar(20) NOT NULL,
  `timecreated` varchar(20) NOT NULL,
  `createdby` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblsales`
--

INSERT INTO `tblsales` (`orderid`, `products`, `quantity`, `payment`, `paymentchange`, `totalcost`, `discounted`, `datecreated`, `timecreated`, `createdby`) VALUES
('order-10/06/2025-050212-pm', 'Bioflu Tablet', '6', '400.00', '84.00', '54.00', 'No', '10/06/2025', '05:02:12 pm', 'admin'),
('order-10/06/2025-050212-pm', 'Biogesic 500mg Tablet', '6', '400.00', '84.00', '42.00', 'No', '10/06/2025', '05:02:12 pm', 'admin'),
('order-10/06/2025-050212-pm', 'Biogesic Syrup 60ml', '1', '400.00', '84.00', '110.00', 'No', '10/06/2025', '05:02:12 pm', 'admin'),
('order-10/06/2025-050325-pm', 'Ceelin Syrup 120ml', '1', '200.00', '39.50', '140.00', 'No', '10/06/2025', '05:03:25 pm', 'admin'),
('order-10/06/2025-050325-pm', 'Decolgen Forte Tablet', '1', '200.00', '39.50', '8.50', 'No', '10/06/2025', '05:03:25 pm', 'admin'),
('order-10/06/2025-050325-pm', 'Dulcolax 5mg Tablet', '1', '200.00', '39.50', '12.00', 'No', '10/06/2025', '05:03:25 pm', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `tblsupplier`
--

CREATE TABLE `tblsupplier` (
  `supplier` varchar(50) NOT NULL,
  `description` text NOT NULL,
  `contactinfo` varchar(50) NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `datecreated` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblsupplier`
--

INSERT INTO `tblsupplier` (`supplier`, `description`, `contactinfo`, `createdby`, `datecreated`) VALUES
('Test', '', '123456', 'admin', '09-11-2025'),
('test2', '', '123456', 'admin', '09/18/2025');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
