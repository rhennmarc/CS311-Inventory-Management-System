-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 26, 2025 at 01:09 PM
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
('staff', '123456', 'PHARMACIST', 'ACTIVE', 'admin', '04/09/2025');

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
('Alaxan FR Capsule', NULL, '12.00', 'Price Increase from 11 to 12', 'admin', '09/26/2025', '16:48:49');

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
('09/25/2025', '11:06:52 pm', 'POS', 'PURCHASE', 'ORDER ID: order-09/25/2025-110652-pm | TOTAL: 408.', 'admin'),
('09/25/2025', '11:24:17 pm', 'POS', 'PURCHASE', 'ORDER ID: order-09/25/2025-112417-pm | TOTAL: 757.', 'admin'),
('09/25/2025', '11:26:41 pm', 'POS', 'PURCHASE', 'ORDER ID: order-09/25/2025-112641-pm | TOTAL: 377.', 'admin'),
('09/26/2025', '10:07:35 am', 'POS', 'PURCHASE', 'ORDER ID: order-09/26/2025-100735-am | TOTAL: 27.0', 'admin'),
('09/26/2025', '10:33:38', 'SALES REPORT', 'EXPORT', 'CSV Export: Daily - 09/26/2025', 'admin'),
('09/26/2025', '10:34:51', 'SALES REPORT', 'EXPORT', 'CSV Export: Weekly - 09/26/2025', 'admin'),
('09/26/2025', '10:43:09', 'SALES REPORT', 'EXPORT', 'CSV Export: Weekly - 09/26/2025', 'admin'),
('09/26/2025', '10:46:01', 'SALES REPORT', 'EXPORT', 'CSV Export: Weekly - 09/26/2025', 'admin'),
('09/26/2025', '11:11:14', 'PURCHASE ORDER MANAGEMENT', 'EXPORT', 'CSV Export: Test - All', 'admin'),
('09/26/2025', '1:40 pm', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Alaxan FR Capsule, Qty: +15, Stock: 104?1', 'admin'),
('09/26/2025', '1:45 pm', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Ascof Lagundi Syrup 60ml, Qty: +5, Stock:', 'admin'),
('09/26/2025', '1:47 pm', 'ADJUSTMENT MANAGEMENT', 'UPDATE ADJUSTMENT', 'Product: Ascof Lagundi Syrup 60ml, Old: +5, New: +', 'admin'),
('09/26/2025', '1:47 pm', 'ADJUSTMENT MANAGEMENT', 'UPDATE ADJUSTMENT', 'Product: Ascof Lagundi Syrup 60ml, Old: +10, New: ', 'admin'),
('09/26/2025', '1:47 pm', 'ADJUSTMENT MANAGEMENT', 'DELETE', 'Product: Mucotuss Forte, Qty: 10', 'admin'),
('09/26/2025', '2:16 pm', 'ADJUSTMENT MANAGEMENT', 'DELETE', 'Product: Alaxan FR Capsule, Qty: 15', 'admin'),
('09/26/2025', '2:16 pm', 'ADJUSTMENT MANAGEMENT', 'DELETE', 'Product: Ascof Lagundi Syrup 60ml, Qty: 10', 'admin'),
('09/26/2025', '2:18 pm', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Alaxan FR Capsule, Qty: +5, Stock: 119?12', 'admin'),
('09/26/2025', '2:18 pm', 'ADJUSTMENT MANAGEMENT', 'UPDATE ADJUSTMENT', 'Product: Alaxan FR Capsule, Old: +5, New: -5, Stoc', 'admin'),
('09/26/2025', '2:19 pm', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Ascof Lagundi Syrup 60ml, Qty: +13, Stock', 'admin'),
('09/26/2025', '2:21 pm', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Alaxan FR Capsule, Price: 12.00?11.00', 'admin'),
('09/26/2025', '2:36 pm', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Ascorbic Acid 500mg Tablet (Generic), Pri', 'admin'),
('09/26/2025', '2:38 pm', 'ADJUSTMENT MANAGEMENT', 'UPDATE PRICE', 'Product: Alaxan FR Capsule, Price: 11.00?11.00', 'admin'),
('09/26/2025', '4:47 pm', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Alaxan FR Capsule, Qty: +10, Stock: 114?1', 'admin'),
('09/26/2025', '4:48 pm', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Alaxan FR Capsule, Price: 11.00?13.00', 'admin'),
('09/26/2025', '4:50 pm', 'ADJUSTMENT MANAGEMENT', 'UPDATE PRICE', 'Product: Alaxan FR Capsule, Price: 13.00?12.00', 'admin'),
('09/26/2025', '04:55:31 pm', 'POS', 'PURCHASE', 'ORDER ID: order-09/26/2025-045531-pm | TOTAL: 106.', 'admin'),
('09/26/2025', '05:07:41 pm', 'POS', 'PURCHASE', 'ORDER ID: order-09/26/2025-050741-pm | TOTAL: 110.', 'admin'),
('09/26/2025', '17:08:18', 'SALES REPORT', 'EXPORT', 'CSV Export: Weekly - 09/26/2025', 'admin');

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
('Biogesic 500mg Tablet', '', '7.00', '191', 'admin', '09/19/2025'),
('Bioflu Tablet', '', '9.00', '141', 'admin', '09/19/2025'),
('Alaxan FR Capsule', '', '12.00', '122', 'admin', '09/19/2025'),
('Decolgen Forte Tablet', '', '8.50', '178', 'admin', '09/19/2025'),
('Paracetamol 500mg Tablet (Generic)', '', '2.00', '498', 'admin', '09/19/2025'),
('Ibuprofen 400mg Tablet', '', '4.50', '299', 'admin', '09/19/2025'),
('Kremil-S Tablet', '', '10.00', '98', 'admin', '09/19/2025'),
('Ascof Lagundi Syrup 60ml', '', '90.00', '59', 'admin', '09/19/2025'),
('Ventolin Inhaler 100mcg', '', '350.00', '29', 'admin', '09/19/2025'),
('Cetirizine 10mg Tablet', '', '6.00', '197', 'admin', '09/19/2025'),
('Claritin 10mg Tablet', '', '35.00', '75', 'admin', '09/19/2025'),
('Myra-E 400IU Capsule', '', '12.00', '149', 'admin', '09/19/2025'),
('Enervon Tablet', '', '10.00', '98', 'admin', '09/19/2025'),
('Stresstabs Capsule', '', '18.00', '99', 'admin', '09/19/2025'),
('Centrum Advance Tablet', '', '22.00', '59', 'admin', '09/19/2025'),
('Ascorbic Acid 500mg Tablet (Generic)', '', '4.00', '290', 'admin', '09/19/2025'),
('Fern-C 500mg Capsule', '', '13.00', '78', 'admin', '09/19/2025'),
('Ceelin Syrup 120ml', '', '140.00', '37', 'admin', '09/19/2025'),
('Diatabs Capsule', '', '9.00', '98', 'admin', '09/19/2025'),
('Dulcolax 5mg Tablet', '', '12.00', '48', 'admin', '09/19/2025'),
('Gaviscon Liquid 120ml', '', '180.00', '18', 'admin', '09/19/2025'),
('Hydrite Oral Rehydration Salt Sachet', '', '8.00', '148', 'admin', '09/19/2025'),
('Imodium Capsule', '', '22.00', '69', 'admin', '09/19/2025'),
('Mefenamic Acid 500mg Tablet', '', '4.00', '249', 'admin', '09/19/2025'),
('Buscopan Tablet', '', '16.00', '117', 'admin', '09/19/2025'),
('Robitussin DM Syrup 60ml', '', '95.00', '30', 'admin', '09/19/2025'),
('Tempra Forte 60ml (Paracetamol)', '', '120.00', '40', 'admin', '09/19/2025'),
('Biogesic Syrup 60ml', '', '110.00', '30', 'admin', '09/19/2025');

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
  `supplier` varchar(50) NOT NULL,
  `datereceived` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblpurchase_order`
--

INSERT INTO `tblpurchase_order` (`products`, `quantity`, `unitcost`, `totalcost`, `status`, `createdby`, `datecreated`, `supplier`, `datereceived`) VALUES
('Biogesic 500mg Tablet', '100', '5', '500.00', 'Pending', 'admin', '09/19/2025', 'Test', ''),
('Neozep Forte Tablet', '150', '6', '900.00', 'Pending', 'admin', '09/19/2025', 'Test', ''),
('Alaxan FR Capsule', '20', '7', '140.00', 'Pending', 'admin', '09/25/2025', 'Test', '');

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
('order-09/26/2025-050741-pm', 'Bioflu Tablet', '4', '500.00', '389.60', '28.80', 'Yes', '09/26/2025', '05:07:41 pm', 'admin');

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
