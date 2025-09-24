-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 24, 2025 at 09:36 AM
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
  `quantity` varchar(20) NOT NULL,
  `reason` text NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `dateadjusted` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tbladjustment`
--

INSERT INTO `tbladjustment` (`products`, `quantity`, `reason`, `createdby`, `dateadjusted`) VALUES
('Mucotuss Forte', '10', '10 stocks were discarded due to expiration', 'admin', '09/17/2025');

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
('18/09/2025', '4:49 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Neozep', 'admin'),
('18/09/2025', '4:51 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Tuseran', 'admin'),
('09/18/2025', '4:51 pm', 'SUPPLIER MANAGEMENT', 'ADD', 'test2', 'admin'),
('09/18/2025', '5:58 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Neozep', 'admin'),
('18/09/2025', '5:59 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'neozep', 'admin'),
('18/09/2025', '6:15 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Mucotuss Forte', 'admin'),
('09/18/2025', '6:15 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Mucotuss Forte', 'admin'),
('09/18/2025', '6:16 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE ALL', 'ALL PENDING ORDERS', 'admin'),
('09/19/2025', '11:35:14', 'POS', 'PURCHASE', 'TOTAL: 37.00', 'admin'),
('09/19/2025', '5:19 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Neozep Forte Tablet', 'admin'),
('09/19/2025', '5:20 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Biogesic 500mg Tablet', 'admin'),
('09/19/2025', '5:20 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Bioflu Tablet', 'admin'),
('09/19/2025', '5:21 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('09/19/2025', '5:21 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Decolgen Forte Tablet', 'admin'),
('09/19/2025', '5:22 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Paracetamol 500mg Tablet (Generic)', 'admin'),
('09/19/2025', '5:23 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Ibuprofen 400mg Tablet', 'admin'),
('09/19/2025', '5:23 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Kremil-S Tablet', 'admin'),
('09/19/2025', '5:23 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Tuseran Forte Capsule', 'admin'),
('09/19/2025', '5:24 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Ascof Lagundi Syrup 60ml', 'admin'),
('09/19/2025', '5:25 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Ventolin Inhaler 100mcg', 'admin'),
('09/19/2025', '5:25 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Cetirizine 10mg Tablet', 'admin'),
('09/19/2025', '5:25 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Claritin 10mg Tablet', 'admin'),
('09/19/2025', '5:25 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Myra-E 400IU Capsule', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Enervon Tablet', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Ascof Lagundi Syrup 60ml', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Bioflu Tablet', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Biogesic 500mg Tablet', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Cetirizine 10mg Tablet', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Claritin 10mg Tablet', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Kremil-S Tablet', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Mucotuss Forte', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Myra-E 400IU Capsule', 'admin'),
('09/19/2025', '5:26 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Neozep Forte Tablet', 'admin'),
('09/19/2025', '5:27 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Paracetamol 500mg Tablet (Generic)', 'admin'),
('09/19/2025', '5:33 pm', 'PRODUCT MANAGEMENT', 'UPDATE', 'Ventolin Inhaler 100mcg', 'admin'),
('09/19/2025', '5:34 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Stresstabs Capsule', 'admin'),
('09/19/2025', '5:34 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Centrum Advance Tablet', 'admin'),
('09/19/2025', '5:35 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Ascorbic Acid 500mg Tablet (Generic)', 'admin'),
('09/19/2025', '5:35 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Fern-C 500mg Capsule', 'admin'),
('09/19/2025', '5:35 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Ceelin Syrup 120ml', 'admin'),
('09/19/2025', '5:36 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Diatabs Capsule', 'admin'),
('09/19/2025', '5:36 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Dulcolax 5mg Tablet', 'admin'),
('09/19/2025', '5:36 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Gaviscon Liquid 120ml', 'admin'),
('09/19/2025', '5:36 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Hydrite Oral Rehydration Salt Sachet', 'admin'),
('09/19/2025', '5:36 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Imodium Capsule', 'admin'),
('09/19/2025', '5:37 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Mefenamic Acid 500mg Tablet', 'admin'),
('09/19/2025', '5:37 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Buscopan Tablet', 'admin'),
('09/19/2025', '5:38 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Robitussin DM Syrup 60ml', 'admin'),
('09/19/2025', '5:38 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Tempra Forte 60ml (Paracetamol)', 'admin'),
('09/19/2025', '5:38 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'Biogesic Syrup 60ml', 'admin'),
('09/19/2025', '5:39 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', 'ALL RECORDS', 'admin'),
('19/09/2025', '5:40 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Biogesic 500mg Tablet', 'admin'),
('19/09/2025', '5:41 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Neozep Forte Tablet', 'admin'),
('09/19/2025', '17:44:28', 'POS', 'PURCHASE', 'TOTAL: 219.00', 'admin'),
('09/24/2025', '14:48:05', 'POS', 'PURCHASE', 'TOTAL: 84.00', 'admin'),
('09/24/2025', '2:49 pm', 'SALES REPORT', 'DELETE ALL', 'ALL SALES ON 09/24/2025', 'admin'),
('09/24/2025', '15:01:56', 'POS', 'PURCHASE', 'TOTAL: 9.60 | DISCOUNTED: Yes', 'admin'),
('09/24/2025', '15:03:54', 'POS', 'PURCHASE', 'TOTAL: 114.00 | DISCOUNTED: No', 'admin'),
('09/24/2025', '15:31:16', 'POS', 'PURCHASE', 'TOTAL: 272.00 | DISCOUNTED: Yes', 'admin');

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
('Mucotuss Forte', '', '7.00', '224', 'admin', '11/09/2025'),
('Neozep Forte Tablet', '', '8.00', '100', 'admin', '09/18/2025'),
('Tuseran Forte Capsule', '', '14.00', '100', 'admin', '09/18/2025'),
('Biogesic 500mg Tablet', '', '7.00', '200', 'admin', '09/19/2025'),
('Bioflu Tablet', '', '9.00', '150', 'admin', '09/19/2025'),
('Alaxan FR Capsule', '', '12.00', '113', 'admin', '09/19/2025'),
('Decolgen Forte Tablet', '', '8.50', '180', 'admin', '09/19/2025'),
('Paracetamol 500mg Tablet (Generic)', '', '2.00', '500', 'admin', '09/19/2025'),
('Ibuprofen 400mg Tablet', '', '4.50', '300', 'admin', '09/19/2025'),
('Kremil-S Tablet', '', '10.00', '100', 'admin', '09/19/2025'),
('Ascof Lagundi Syrup 60ml', '', '90.00', '45', 'admin', '09/19/2025'),
('Ventolin Inhaler 100mcg', '', '350.00', '30', 'admin', '09/19/2025'),
('Cetirizine 10mg Tablet', '', '6.00', '200', 'admin', '09/19/2025'),
('Claritin 10mg Tablet', '', '35.00', '80', 'admin', '09/19/2025'),
('Myra-E 400IU Capsule', '', '12.00', '150', 'admin', '09/19/2025'),
('Enervon Tablet', '', '10.00', '100', 'admin', '09/19/2025'),
('Stresstabs Capsule', '', '18.00', '100', 'admin', '09/19/2025'),
('Centrum Advance Tablet', '', '22.00', '60', 'admin', '09/19/2025'),
('Ascorbic Acid 500mg Tablet (Generic)', '', '3.00', '298', 'admin', '09/19/2025'),
('Fern-C 500mg Capsule', '', '13.00', '80', 'admin', '09/19/2025'),
('Ceelin Syrup 120ml', '', '140.00', '39', 'admin', '09/19/2025'),
('Diatabs Capsule', '', '9.00', '100', 'admin', '09/19/2025'),
('Dulcolax 5mg Tablet', '', '12.00', '50', 'admin', '09/19/2025'),
('Gaviscon Liquid 120ml', '', '180.00', '20', 'admin', '09/19/2025'),
('Hydrite Oral Rehydration Salt Sachet', '', '8.00', '150', 'admin', '09/19/2025'),
('Imodium Capsule', '', '22.00', '70', 'admin', '09/19/2025'),
('Mefenamic Acid 500mg Tablet', '', '4.00', '250', 'admin', '09/19/2025'),
('Buscopan Tablet', '', '16.00', '120', 'admin', '09/19/2025'),
('Robitussin DM Syrup 60ml', '', '95.00', '30', 'admin', '09/19/2025'),
('Tempra Forte 60ml (Paracetamol)', '', '120.00', '40', 'admin', '09/19/2025'),
('Biogesic Syrup 60ml', '', '110.00', '34', 'admin', '09/19/2025');

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
('Neozep Forte Tablet', '150', '6', '900.00', 'Pending', 'admin', '09/19/2025', 'Test', '');

-- --------------------------------------------------------

--
-- Table structure for table `tblsales`
--

CREATE TABLE `tblsales` (
  `products` varchar(50) NOT NULL,
  `quantity` varchar(50) NOT NULL,
  `totalcost` varchar(50) NOT NULL,
  `discounted` varchar(20) NOT NULL,
  `datecreated` varchar(20) NOT NULL,
  `timecreated` varchar(20) NOT NULL,
  `createdby` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblsales`
--

INSERT INTO `tblsales` (`products`, `quantity`, `totalcost`, `discounted`, `datecreated`, `timecreated`, `createdby`) VALUES
('Mucotuss Forte', '20', '140', '', '09/18/2025', '', 'admin'),
('Neozep', '20', '100', '', '09/18/2025', '10:47 am', 'admin'),
('Mucotuss Forte', '20', '140', '', '09/18/2025', '10:46 am', 'admin'),
('Tuseran', '20', '140', '', '09/19/2025', '10:46 am', 'admin'),
('Neozep', '20', '100', '', '09/19/2025', '10:47 am', 'admin'),
('Mucotuss Forte', '1', '7.00', '', '09/19/2025', '11:35:14', 'admin'),
('Neozep', '6', '30.00', '', '09/19/2025', '11:35:14', 'admin'),
('Ascof Lagundi Syrup 60ml', '2', '180.00', '', '09/19/2025', '17:44:28', 'admin'),
('Ascorbic Acid 500mg Tablet (Generic)', '1', '3.00', '', '09/19/2025', '17:44:28', 'admin'),
('Alaxan FR Capsule', '3', '36.00', '', '09/19/2025', '17:44:28', 'admin'),
('Alaxan FR Capsule', '1', '9.60', 'Yes', '09/24/2025', '15:01:56', 'admin'),
('Ascof Lagundi Syrup 60ml', '1', '90.00', 'No', '09/24/2025', '15:03:54', 'admin'),
('Alaxan FR Capsule', '2', '24.00', 'No', '09/24/2025', '15:03:54', 'admin'),
('Ascof Lagundi Syrup 60ml', '1', '72.00', 'Yes', '09/24/2025', '15:31:16', 'admin'),
('Biogesic Syrup 60ml', '1', '88.00', 'Yes', '09/24/2025', '15:31:16', 'admin'),
('Ceelin Syrup 120ml', '1', '112.00', 'Yes', '09/24/2025', '15:31:16', 'admin');

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
