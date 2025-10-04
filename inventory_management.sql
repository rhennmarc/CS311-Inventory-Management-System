-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 04, 2025 at 12:14 PM
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
('Biogesic Syrup 60ml', '5', NULL, 'From other branch', 'admin', '10/03/2025', '15:44:51');

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
('Gaviscon Liquid 120ml', '120', '10/04/2025 01:12:19 pm', 'admin');

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
('10/03/2025', '15:23:58', 'SALES', 'REFUND', 'ORDER ID: order-10/03/2025-032324-pm - 1 items', 'admin'),
('10/03/2025', '15:25:24', 'SALES REPORT', 'DELETE REFUND', 'ORDER ID: order-10/03/2025-032324-pm', 'admin'),
('10/03/2025', '15:25:28', 'SALES REPORT', 'DELETE', 'ORDER ID: order-10/03/2025-032324-pm', 'admin'),
('10/03/2025', '03:41:08 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/03/2025-034108-pm | TOTAL: 660.', 'admin'),
('10/03/2025', '3:41:25 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/03/2025-034108-pm - 1 items', 'admin'),
('03/10/2025', '3:43 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Biogesic Syrup 60ml', 'admin'),
('10/03/2025', '3:44 pm', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Biogesic Syrup 60ml, Qty: +5, Stock: 25?3', 'admin'),
('10/03/2025', '3:45 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Biogesic Syrup 60ml', 'admin'),
('10/03/2025', '5:55 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', 'ALL RECORDS', 'admin'),
('10/03/2025', '8:19 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '8:19 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '8:19 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '8:19 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '8:19 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '8:19 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '8:20 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE ALL', 'ALL PENDING ORDERS', 'admin'),
('10/03/2025', '10:19 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:19 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:19 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:20 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:21 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:32 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:32 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:32 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:44 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:44 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:44:57 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:44:58 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:45:21 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:45:22 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:45 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE ALL', 'ALL PENDING ORDERS', 'admin'),
('10/03/2025', '10:46:10 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:46:12 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '10:58 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:09:35 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:09:36 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:09:46 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:09:48 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:10:09 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:30:44 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:30:46 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:30:54 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:30:55 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:31:01 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:45:41 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:45:43 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:46:00 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:46:01 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('10/03/2025', '11:46:06 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ceelin Syrup 120ml', 'admin'),
('10/04/2025', '11:18:24 am', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Ceelin Syrup 120ml', 'admin'),
('10/04/2025', '12:37:32 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Gaviscon Liquid 120ml', 'admin'),
('10/04/2025', '12:37:34 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Gaviscon Liquid 120ml', 'admin'),
('10/04/2025', '01:03:59 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE ALL', 'ALL PENDING ORDERS', 'admin'),
('10/04/2025', '01:12:19 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Gaviscon Liquid 120ml', 'admin'),
('10/04/2025', '01:12:20 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Gaviscon Liquid 120ml', 'admin'),
('10/04/2025', '01:12:23 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE ALL', 'ALL PENDING ORDERS', 'admin');

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
('Biogesic 500mg Tablet', '', '7.00', '178', 'admin', '09/19/2025'),
('Bioflu Tablet', '', '9.00', '136', 'admin', '09/19/2025'),
('Alaxan FR Capsule', '', '12.00', '109', 'admin', '09/19/2025'),
('Decolgen Forte Tablet', '', '8.50', '178', 'admin', '09/19/2025'),
('Paracetamol 500mg Tablet (Generic)', '', '2.00', '498', 'admin', '09/19/2025'),
('Ibuprofen 400mg Tablet', '', '4.50', '299', 'admin', '09/19/2025'),
('Kremil-S Tablet', '', '10.00', '98', 'admin', '09/19/2025'),
('Ascof Lagundi Syrup 60ml', '', '90.00', '150', 'admin', '09/19/2025'),
('Ventolin Inhaler 100mcg', '', '350.00', '29', 'admin', '09/19/2025'),
('Cetirizine 10mg Tablet', '', '6.00', '196', 'admin', '09/19/2025'),
('Claritin 10mg Tablet', '', '35.00', '75', 'admin', '09/19/2025'),
('Myra-E 400IU Capsule', '', '12.00', '149', 'admin', '09/19/2025'),
('Enervon Tablet', '', '10.00', '98', 'admin', '09/19/2025'),
('Stresstabs Capsule', '', '18.00', '99', 'admin', '09/19/2025'),
('Centrum Advance Tablet', '', '22.00', '59', 'admin', '09/19/2025'),
('Ascorbic Acid 500mg Tablet (Generic)', '', '4.00', '261', 'admin', '09/19/2025'),
('Fern-C 500mg Capsule', '', '13.00', '78', 'admin', '09/19/2025'),
('Ceelin Syrup 120ml', '', '140.00', '61', 'admin', '09/19/2025'),
('Diatabs Capsule', '', '9.00', '98', 'admin', '09/19/2025'),
('Dulcolax 5mg Tablet', '', '12.00', '48', 'admin', '09/19/2025'),
('Gaviscon Liquid 120ml', '', '180.00', '31', 'admin', '09/19/2025'),
('Hydrite Oral Rehydration Salt Sachet', '', '8.00', '148', 'admin', '09/19/2025'),
('Imodium Capsule', '', '22.00', '69', 'admin', '09/19/2025'),
('Mefenamic Acid 500mg Tablet', '', '4.00', '249', 'admin', '09/19/2025'),
('Buscopan Tablet', '', '16.00', '116', 'admin', '09/19/2025'),
('Robitussin DM Syrup 60ml', '', '95.00', '30', 'admin', '09/19/2025'),
('Tempra Forte 60ml (Paracetamol)', '', '120.00', '40', 'admin', '09/19/2025'),
('Biogesic Syrup 60ml', '', '110.00', '80', 'admin', '09/19/2025');

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
('Gaviscon Liquid 120ml', '3', '120', '360.00', 'Received', 'admin', '10/04/2025', '01:12:19 pm', 'Test', '10/04/2025');

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
('order-09/30/2025-080348-pm', 'Ascof Lagundi Syrup 60ml', '2', '90.00', 'Wrong quantity', '09/30/2025', '20:44:53', 'admin'),
('order-09/30/2025-080348-pm', 'Alaxan FR Capsule', '1', '12.00', 'Wrong quantity', '09/30/2025', '20:44:53', 'admin'),
('order-10/02/2025-095000-am', 'Bioflu Tablet', '8', '9.00', 'Wrong order', '10/02/2025', '10:24:25', 'admin'),
('order-10/02/2025-112056-am', 'Ceelin Syrup 120ml', '2', '140.00', 'Wrong product', '10/02/2025', '11:23:28', 'admin'),
('order-10/02/2025-112056-am', 'Ceelin Syrup 120ml', '1', '140.00', 'Wrong product', '10/02/2025', '11:32:49', 'admin'),
('order-10/02/2025-114302-am', 'Ascorbic Acid 500mg Tablet (Generic)', '6', '4.00', 'Wrong quantity', '10/02/2025', '11:43:29', 'admin'),
('order-10/02/2025-115506-am', 'Ceelin Syrup 120ml', '1', '140.00', 'Wrong product', '10/02/2025', '11:55:32', 'admin'),
('order-10/02/2025-115633-am', 'Biogesic Syrup 60ml', '2', '110.00', 'Wrong quantity', '10/02/2025', '11:56:58', 'admin'),
('order-10/02/2025-115633-am', 'Biogesic Syrup 60ml', '1', '110.00', 'Wrong quantity', '10/02/2025', '12:07:38', 'admin'),
('order-10/02/2025-122239-pm', 'Ceelin Syrup 120ml', '1', '140.00', 'Wrong quantity', '10/02/2025', '12:23:04', 'admin'),
('order-10/02/2025-114302-am', 'Ascorbic Acid 500mg Tablet (Generic)', '2', '4.00', 'Wrong quantity', '10/02/2025', '13:12:48', 'admin'),
('order-10/02/2025-114302-am', 'Ascorbic Acid 500mg Tablet (Generic)', '2', '4.00', 'Wrong quantity', '10/02/2025', '13:42:02', 'admin'),
('order-10/02/2025-063516-pm', 'Ascof Lagundi Syrup 60ml', '1', '90.00', 'Wrong quantity', '10/02/2025', '18:36:38', 'admin'),
('order-10/03/2025-034108-pm', 'Ascof Lagundi Syrup 60ml', '3', '90.00', 'Wrong quantity', '10/03/2025', '3:41:25 pm', 'admin');

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
('order-09/25/2025-110417-pm', 'Alaxan FR Capsule', '1', '150.00', '32.00', '12.00', 'No', '09/25/2025', '11:04:17 pm', 'admin'),
('order-09/25/2025-110417-pm', 'Ascof Lagundi Syrup 60ml', '1', '150.00', '32.00', '90.00', 'No', '09/25/2025', '11:04:17 pm', 'admin'),
('order-09/25/2025-110417-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '150.00', '32.00', '3.00', 'No', '09/25/2025', '11:04:17 pm', 'admin'),
('order-09/25/2025-110417-pm', 'Biogesic 500mg Tablet', '1', '150.00', '32.00', '7.00', 'No', '09/25/2025', '11:04:17 pm', 'admin'),
('order-09/25/2025-110417-pm', 'Cetirizine 10mg Tablet', '1', '150.00', '32.00', '6.00', 'No', '09/25/2025', '11:04:17 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Claritin 10mg Tablet', '4', '420.00', '12.00', '112.00', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Decolgen Forte Tablet', '1', '420.00', '12.00', '6.80', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Diatabs Capsule', '1', '420.00', '12.00', '7.20', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Dulcolax 5mg Tablet', '1', '420.00', '12.00', '9.60', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Enervon Tablet', '1', '420.00', '12.00', '8.00', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Fern-C 500mg Capsule', '1', '420.00', '12.00', '10.40', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Gaviscon Liquid 120ml', '1', '420.00', '12.00', '144.00', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Hydrite Oral Rehydration Salt Sachet', '1', '420.00', '12.00', '6.40', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Ibuprofen 400mg Tablet', '1', '420.00', '12.00', '3.60', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Imodium Capsule', '1', '420.00', '12.00', '17.60', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Kremil-S Tablet', '1', '420.00', '12.00', '8.00', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Mefenamic Acid 500mg Tablet', '1', '420.00', '12.00', '3.20', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Myra-E 400IU Capsule', '1', '420.00', '12.00', '9.60', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Mucotuss Forte', '1', '420.00', '12.00', '5.60', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Neozep Forte Tablet', '1', '420.00', '12.00', '6.40', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Paracetamol 500mg Tablet (Generic)', '1', '420.00', '12.00', '1.60', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Stresstabs Capsule', '1', '420.00', '12.00', '14.40', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-110652-pm', 'Tuseran Forte Capsule', '3', '420.00', '12.00', '33.60', 'Yes', '09/25/2025', '11:06:52 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Alaxan FR Capsule', '1', '1000.00', '242.80', '9.60', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Ascof Lagundi Syrup 60ml', '1', '1000.00', '242.80', '72.00', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '3', '1000.00', '242.80', '7.20', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Bioflu Tablet', '1', '1000.00', '242.80', '7.20', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Biogesic 500mg Tablet', '3', '1000.00', '242.80', '16.80', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Biogesic Syrup 60ml', '3', '1000.00', '242.80', '264.00', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Buscopan Tablet', '2', '1000.00', '242.80', '25.60', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Ceelin Syrup 120ml', '1', '1000.00', '242.80', '112.00', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Centrum Advance Tablet', '1', '1000.00', '242.80', '17.60', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Cetirizine 10mg Tablet', '1', '1000.00', '242.80', '4.80', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Claritin 10mg Tablet', '1', '1000.00', '242.80', '28.00', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Decolgen Forte Tablet', '1', '1000.00', '242.80', '6.80', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Diatabs Capsule', '1', '1000.00', '242.80', '7.20', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Dulcolax 5mg Tablet', '1', '1000.00', '242.80', '9.60', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Enervon Tablet', '1', '1000.00', '242.80', '8.00', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Fern-C 500mg Capsule', '1', '1000.00', '242.80', '10.40', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Gaviscon Liquid 120ml', '1', '1000.00', '242.80', '144.00', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112417-pm', 'Hydrite Oral Rehydration Salt Sachet', '1', '1000.00', '242.80', '6.40', 'Yes', '09/25/2025', '11:24:17 pm', 'admin'),
('order-09/25/2025-112641-pm', 'Alaxan FR Capsule', '1', '400.00', '23.00', '12.00', 'No', '09/25/2025', '11:26:41 pm', 'admin'),
('order-09/25/2025-112641-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '400.00', '23.00', '3.00', 'No', '09/25/2025', '11:26:41 pm', 'admin'),
('order-09/25/2025-112641-pm', 'Ventolin Inhaler 100mcg', '1', '400.00', '23.00', '350.00', 'No', '09/25/2025', '11:26:41 pm', 'admin'),
('order-09/25/2025-112641-pm', 'Kremil-S Tablet', '1', '400.00', '23.00', '10.00', 'No', '09/25/2025', '11:26:41 pm', 'admin'),
('order-09/25/2025-112641-pm', 'Paracetamol 500mg Tablet (Generic)', '1', '400.00', '23.00', '2.00', 'No', '09/25/2025', '11:26:41 pm', 'admin'),
('order-09/26/2025-100735-am', 'Alaxan FR Capsule', '1', '30.00', '3.00', '12.00', 'No', '09/26/2025', '10:07:35 am', 'admin'),
('order-09/26/2025-100735-am', 'Bioflu Tablet', '1', '30.00', '3.00', '9.00', 'No', '09/26/2025', '10:07:35 am', 'admin'),
('order-09/26/2025-100735-am', 'Cetirizine 10mg Tablet', '1', '30.00', '3.00', '6.00', 'No', '09/26/2025', '10:07:35 am', 'admin'),
('order-09/26/2025-045531-pm', 'Alaxan FR Capsule', '1', '120.00', '14.00', '12.00', 'No', '09/26/2025', '04:55:31 pm', 'admin'),
('order-09/26/2025-045531-pm', 'Ascof Lagundi Syrup 60ml', '1', '120.00', '14.00', '90.00', 'No', '09/26/2025', '04:55:31 pm', 'admin'),
('order-09/26/2025-045531-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '120.00', '14.00', '4.00', 'No', '09/26/2025', '04:55:31 pm', 'admin'),
('order-09/26/2025-050741-pm', 'Alaxan FR Capsule', '1', '500.00', '389.60', '9.60', 'Yes', '09/26/2025', '05:07:41 pm', 'admin'),
('order-09/26/2025-050741-pm', 'Ascof Lagundi Syrup 60ml', '1', '500.00', '389.60', '72.00', 'Yes', '09/26/2025', '05:07:41 pm', 'admin'),
('order-09/26/2025-050741-pm', 'Bioflu Tablet', '4', '500.00', '389.60', '28.80', 'Yes', '09/26/2025', '05:07:41 pm', 'admin'),
('order-09/26/2025-071343-pm', 'Alaxan FR Capsule', '1', '120.00', '14.00', '12.00', 'No', '09/26/2025', '07:13:43 pm', 'admin'),
('order-09/26/2025-071343-pm', 'Ascof Lagundi Syrup 60ml', '1', '120.00', '14.00', '90.00', 'No', '09/26/2025', '07:13:43 pm', 'admin'),
('order-09/26/2025-071343-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '120.00', '14.00', '4.00', 'No', '09/26/2025', '07:13:43 pm', 'admin'),
('order-09/28/2025-081544-pm', 'Alaxan FR Capsule', '1', '30.00', '8.00', '12.00', 'No', '09/28/2025', '08:15:44 pm', 'admin'),
('order-09/28/2025-081544-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '30.00', '8.00', '4.00', 'No', '09/28/2025', '08:15:44 pm', 'admin'),
('order-09/28/2025-081544-pm', 'Cetirizine 10mg Tablet', '1', '30.00', '8.00', '6.00', 'No', '09/28/2025', '08:15:44 pm', 'admin'),
('order-09/28/2025-081609-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '200.00', '24.00', '4.00', 'No', '09/28/2025', '08:16:09 pm', 'admin'),
('order-09/28/2025-081609-pm', 'Bioflu Tablet', '1', '200.00', '24.00', '9.00', 'No', '09/28/2025', '08:16:09 pm', 'admin'),
('order-09/28/2025-081609-pm', 'Biogesic 500mg Tablet', '1', '200.00', '24.00', '7.00', 'No', '09/28/2025', '08:16:09 pm', 'admin'),
('order-09/28/2025-081609-pm', 'Buscopan Tablet', '1', '200.00', '24.00', '16.00', 'No', '09/28/2025', '08:16:09 pm', 'admin'),
('order-09/28/2025-081609-pm', 'Ceelin Syrup 120ml', '1', '200.00', '24.00', '140.00', 'No', '09/28/2025', '08:16:09 pm', 'admin'),
('order-09/30/2025-034332-pm', 'Alaxan FR Capsule', '1', '250.00', '34.00', '12.00', 'No', '09/30/2025', '03:43:32 pm', 'admin'),
('order-09/30/2025-034332-pm', 'Ascof Lagundi Syrup 60ml', '1', '250.00', '34.00', '90.00', 'No', '09/30/2025', '03:43:32 pm', 'admin'),
('order-09/30/2025-034332-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '250.00', '34.00', '4.00', 'No', '09/30/2025', '03:43:32 pm', 'admin'),
('order-09/30/2025-034332-pm', 'Biogesic Syrup 60ml', '1', '250.00', '34.00', '110.00', 'No', '09/30/2025', '03:43:32 pm', 'admin'),
('order-09/30/2025-034443-pm', 'Ceelin Syrup 120ml', '1', '300.00', '28.00', '112.00', 'Yes', '09/30/2025', '03:44:43 pm', 'admin'),
('order-09/30/2025-034443-pm', 'Biogesic Syrup 60ml', '1', '300.00', '28.00', '88.00', 'Yes', '09/30/2025', '03:44:43 pm', 'admin'),
('order-09/30/2025-034443-pm', 'Ascof Lagundi Syrup 60ml', '1', '300.00', '28.00', '72.00', 'Yes', '09/30/2025', '03:44:43 pm', 'admin'),
('order-09/30/2025-080348-pm', 'Alaxan FR Capsule', '3', '400.00', '78.00', '36.00', 'No', '09/30/2025', '08:03:48 pm', 'admin'),
('order-09/30/2025-080348-pm', 'Ascof Lagundi Syrup 60ml', '1', '400.00', '78.00', '90.00', 'No', '09/30/2025', '08:03:48 pm', 'admin'),
('order-09/30/2025-080348-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '400.00', '78.00', '4.00', 'No', '09/30/2025', '08:03:48 pm', 'admin'),
('order-10/02/2025-092816-am', 'Alaxan FR Capsule', '1', '150.00', '8.00', '12.00', 'No', '10/02/2025', '09:28:16 am', 'admin'),
('order-10/02/2025-092816-am', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '150.00', '8.00', '4.00', 'No', '10/02/2025', '09:28:16 am', 'admin'),
('order-10/02/2025-092816-am', 'Ascof Lagundi Syrup 60ml', '1', '150.00', '8.00', '90.00', 'No', '10/02/2025', '09:28:16 am', 'admin'),
('order-10/02/2025-092816-am', 'Bioflu Tablet', '4', '150.00', '8.00', '36.00', 'No', '10/02/2025', '09:28:16 am', 'admin'),
('order-10/02/2025-093939-am', 'Alaxan FR Capsule', '6', '150.00', '14.00', '72.00', 'No', '10/02/2025', '09:39:39 am', 'admin'),
('order-10/02/2025-093939-am', 'Ascorbic Acid 500mg Tablet (Generic)', '16', '150.00', '14.00', '64.00', 'No', '10/02/2025', '09:39:39 am', 'admin'),
('order-10/02/2025-095000-am', 'Biogesic 500mg Tablet', '12', '200.00', '44.00', '84.00', 'No', '10/02/2025', '09:50:00 am', 'admin'),
('order-10/02/2025-112056-am', 'Biogesic Syrup 60ml', '2', '700.00', '60.00', '220.00', 'No', '10/02/2025', '11:20:56 am', 'admin'),
('order-10/02/2025-114302-am', 'Ascorbic Acid 500mg Tablet (Generic)', '6', '100.00', '36.00', '24.00', 'No', '10/02/2025', '11:43:02 am', 'admin'),
('order-10/02/2025-115633-am', 'Biogesic Syrup 60ml', '1', '500.00', '60.00', '110.00', 'No', '10/02/2025', '11:56:33 am', 'admin'),
('order-10/02/2025-122239-pm', 'Ceelin Syrup 120ml', '1', '300.00', '20.00', '140.00', 'No', '10/02/2025', '12:22:39 pm', 'admin'),
('order-10/02/2025-063516-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '200.00', '16.00', '4.00', 'No', '10/02/2025', '06:35:16 pm', 'admin'),
('order-10/02/2025-063516-pm', 'Ascof Lagundi Syrup 60ml', '1', '200.00', '16.00', '90.00', 'No', '10/02/2025', '06:35:16 pm', 'admin'),
('order-10/03/2025-034108-pm', 'Alaxan FR Capsule', '10', '700.00', '40.00', '120.00', 'No', '10/03/2025', '03:41:08 pm', 'admin'),
('order-10/03/2025-034108-pm', 'Ascof Lagundi Syrup 60ml', '3', '700.00', '40.00', '270.00', 'No', '10/03/2025', '03:41:08 pm', 'admin');

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
