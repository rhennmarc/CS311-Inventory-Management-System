-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 10, 2025 at 08:48 AM
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
('rhenn', '123456', 'ADMINISTRATOR', 'INACTIVE', 'admin', '10/04/2025');

-- --------------------------------------------------------

--
-- Table structure for table `tbladjustment`
--

CREATE TABLE `tbladjustment` (
  `products` varchar(50) NOT NULL,
  `quantity` varchar(20) DEFAULT NULL,
  `unitprice` varchar(20) DEFAULT NULL,
  `reason` text NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `dateadjusted` varchar(20) NOT NULL,
  `timeadjusted` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tbladjustment`
--

INSERT INTO `tbladjustment` (`products`, `quantity`, `unitprice`, `reason`, `createdby`, `dateadjusted`, `timeadjusted`) VALUES
('Alaxan FR Capsule', '5', NULL, 'From other branch', 'admin', '10/08/2025', '9:09:48 am'),
('Alaxan FR Capsule', NULL, '11.00', 'Market price increase from 10 to 11', 'admin', '10/08/2025', '12:11:17 pm'),
('Ascof Lagundi Syrup 60ml', NULL, '80.00', 'Price decrease from 90 to 80', 'admin', '10/08/2025', '12:12:00 pm'),
('Diatabs Capsule', '-20', NULL, 'wrong qty 170 -> 150', 'admin', '10/08/2025', '3:04:51 pm'),
('Gaviscon Liquid 120ml', '15', NULL, 'From another branch', 'admin', '10/10/2025', '12:30:42 pm');

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
('Ascorbic Acid 500mg Tablet (Generic)', '1.5', '10/08/2025 12:03:37 pm', 'admin'),
('Bioflu Tablet', '6', '10/08/2025 12:06:23 pm', 'admin'),
('Ascorbic Acid 500mg Tablet (Generic)', '1.5', '10/08/2025 12:07:08 pm', 'admin'),
('Ascorbic Acid 500mg Tablet (Generic)', '2', '10/08/2025 12:07:26 pm', 'admin'),
('Biogesic 500mg Tablet', '5', '10/08/2025 12:07:45 pm', 'admin'),
('Buscopan Tablet', '10', '10/08/2025 12:08:07 pm', 'admin'),
('Ascof Lagundi Syrup 60ml', '70', '10/08/2025 12:09:00 pm', 'admin'),
('Biogesic Syrup 60ml', '100', '10/08/2025 12:09:26 pm', 'admin'),
('Fern-C 500mg Capsule', '9', '10/08/2025 12:10:20 pm', 'admin'),
('Diatabs Capsule', '6', '10/08/2025 03:01:19 pm', 'admin'),
('Ascorbic Acid 500mg Tablet (Generic)', '2', '10/09/2025 02:15:11 pm', 'admin'),
('Bioflu Tablet', '7', '10/09/2025 02:15:29 pm', 'admin'),
('Biogesic 500mg Tablet', '4', '10/09/2025 02:15:41 pm', 'admin'),
('Gaviscon Liquid 120ml', '150', '10/10/2025 11:30:10 am', 'admin');

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
('10/06/2025', '17:19:44', 'SALES REPORT', 'EXPORT', 'CSV Export: Daily - 10/06/2025', 'admin'),
('10/07/2025', '1:13:03 pm', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Alaxan FR Capsule, Price: 12.00?11.00', 'admin'),
('10/07/2025', '1:13:32 pm', 'ADJUSTMENT MANAGEMENT', 'REMOVE ADJUSTMENT', 'Product: Alaxan FR Capsule, Qty: -1, Stock: 102?10', 'admin'),
('10/07/2025', '13:24:55', 'ADJUSTMENT MANAGEMENT', 'DELETE ALL', 'Daily adjustments for 10/07/2025', 'admin'),
('10/07/2025', '8:33 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'ABC', 'admin'),
('10/07/2025', '8:34 pm', 'PRODUCTS MANAGEMENT', 'DELETE', 'ABC', 'admin'),
('10/07/2025', '08:36:34 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/07/2025-083634-pm | TOTAL: 360.', 'admin'),
('10/07/2025', '08:37:07 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/07/2025-083707-pm | TOTAL: 550.', 'admin'),
('10/07/2025', '08:38:11 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:38:12 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:38:42 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:38:44 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:39:09 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:39:10 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '8:42:31 pm', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Ceelin Syrup 120ml, Price: 140.00?135.00', 'admin'),
('10/07/2025', '8:44:25 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/07/2025-083707-pm - 1 items', 'admin'),
('10/07/2025', '8:45:34 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/07/2025-083634-pm - 1 items', 'admin'),
('10/07/2025', '08:46:52 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'Updated unit cost for Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:46:54 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:47:37 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'Updated unit cost for Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '08:47:38 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '09:34:29 pm', 'PURCHASE ORDER MANAGEMENT', 'ERROR', 'Delete All Error: The given key was not present in', 'admin'),
('10/07/2025', '09:35:53 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '09:35:54 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '09:40:05 pm', 'PURCHASE ORDER MANAGEMENT', 'ERROR', 'Delete All Error: The given key was not present in', 'admin'),
('10/07/2025', '09:41:13 pm', 'PURCHASE ORDER MANAGEMENT', 'ERROR', 'Delete All Error: The given key was not present in', 'admin'),
('10/07/2025', '09:46:08 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'Updated unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '09:46:10 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '09:46:24 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'Updated unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '09:46:25 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:37:53 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:40:01 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:46:49 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:47:15 pm', 'PRODUCT HISTORY', 'DELETE ALL', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:47:28 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:47:32 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '10:47:35 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '10:47:49 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Ventolin Inhaler 100mcg', 'admin'),
('10/07/2025', '10:48:13 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:48:14 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:50:07 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '10:50:10 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:01 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'a', 'admin'),
('10/07/2025', '11:01 pm', 'PRODUCTS MANAGEMENT', 'DELETE', 'a', 'admin'),
('10/07/2025', '11:07:28 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:07:29 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:07:33 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:07:43 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:07:52 pm', 'UNIT COST HISTORY', 'HISTORY UPDATE', 'Updated unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:07:52 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:08:12 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:08:13 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:08:23 pm', 'UNIT COST HISTORY', 'HISTORY UPDATE', 'Updated unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:08:23 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:19:32 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '3 RECORDS (test2 - All - None)', 'admin'),
('10/07/2025', '11:52:09 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:52:10 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:53:20 pm', 'UNIT COST HISTORY', 'HISTORY UPDATE', 'Updated unit cost for Alaxan FR Capsule', 'admin'),
('10/07/2025', '11:53:20 pm', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/08/2025', '12:35:31 am', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Alaxan FR Capsule, Qty: +5, Stock: 101?10', 'admin'),
('10/08/2025', '12:35:55 am', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Alaxan FR Capsule, Price: 11.00?10.00', 'admin'),
('10/08/2025', '12:36:03 am', 'ADJUSTMENT MANAGEMENT', 'UPDATE PRICE', 'Product: Alaxan FR Capsule, Price: 10.00?10.00', 'admin'),
('10/08/2025', '08:19:35 am', 'POS', 'PURCHASE', 'ORDER ID: order-10/08/2025-081935-am | TOTAL: 104.', 'admin'),
('10/08/2025', '08:22:27 am', 'POS', 'PURCHASE', 'ORDER ID: order-10/08/2025-082227-am | TOTAL: 2264', 'admin'),
('10/08/2025', '8:25 am', 'PRODUCTS MANAGEMENT', 'ADD', 'abc', 'admin'),
('10/08/2025', '8:25 am', 'PRODUCTS MANAGEMENT', 'DELETE', 'abc', 'admin'),
('10/08/2025', '08:26:51 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Alaxan FR Capsule', 'admin'),
('10/08/2025', '08:26:53 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('10/08/2025', '08:27:39 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Alaxan FR Capsule', 'admin'),
('10/08/2025', '08:27:41 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Alaxan FR Capsule', 'admin'),
('10/08/2025', '08:27:52 am', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '2 RECORDS (test2 - All - None)', 'admin'),
('10/08/2025', '08:28:50 am', 'POS', 'PURCHASE', 'ORDER ID: order-10/08/2025-082850-am | TOTAL: 264.', 'admin'),
('10/08/2025', '09:09:30', 'ADJUSTMENT MANAGEMENT', 'DELETE ALL', 'All Records adjustments', 'admin'),
('10/08/2025', '9:09:50 am', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Alaxan FR Capsule, Qty: +5, Stock: 105?11', 'admin'),
('10/08/2025', '9:37 am', 'SUPPLIER MANAGEMENT', 'UPDATE', 'ABC Company', 'admin'),
('10/08/2025', '9:40 am', 'SUPPLIER MANAGEMENT', 'UPDATE', 'ABC Company', 'admin'),
('10/08/2025', '9:40 am', 'SUPPLIER MANAGEMENT', 'UPDATE', 'DEF Company', 'admin'),
('10/08/2025', '9:42 am', 'SUPPLIER MANAGEMENT', 'UPDATE', 'ABC Company', 'admin'),
('10/08/2025', '9:42 am', 'SUPPLIER MANAGEMENT', 'UPDATE', 'DEF Company', 'admin'),
('10/08/2025', '9:43 am', 'SUPPLIER MANAGEMENT', 'ADD', 'GHI Company', 'admin'),
('10/08/2025', '09:44:20 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Biogesic 500mg Tablet', 'admin'),
('10/08/2025', '09:44:22 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Biogesic 500mg Tablet', 'admin'),
('10/08/2025', '09:45:22 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Paracetamol 500mg Tablet (Generi', 'admin'),
('10/08/2025', '09:45:26 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Paracetamol 500mg Tablet (Generic)', 'admin'),
('10/08/2025', '09:45:39 am', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Biogesic 500mg Tablet', 'admin'),
('10/08/2025', '09:46:14 am', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '2 RECORDS (ABC Company - All - None)', 'admin'),
('10/08/2025', '09:49:15 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ibuprofen 400mg Tablet', 'admin'),
('10/08/2025', '09:49:21 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ibuprofen 400mg Tablet', 'admin'),
('10/08/2025', '09:49:40 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Kremil-S Tablet', 'admin'),
('10/08/2025', '09:49:43 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Kremil-S Tablet', 'admin'),
('10/08/2025', '09:49:58 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Decolgen Forte Tablet', 'admin'),
('10/08/2025', '09:50:01 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:04:31 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:04:33 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:05:03 am', 'UNIT COST HISTORY', 'HISTORY UPDATE', 'Updated unit cost for Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:05:03 am', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:05:40 am', 'UNIT COST HISTORY', 'HISTORY UPDATE', 'Updated unit cost for Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:05:40 am', 'PURCHASE ORDER MANAGEMENT', 'UPDATE', 'Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:06:49 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Decolgen Forte Tablet', 'admin'),
('10/08/2025', '10:06:51 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Decolgen Forte Tablet', 'admin'),
('10/08/2025', '12:02:56 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '4 RECORDS (ABC Company - All - None)', 'admin'),
('10/08/2025', '12:03:07 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '1 RECORDS (DEF Company - All - None)', 'admin'),
('10/08/2025', '12:03:37 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ascorbic Acid 500mg Tablet (Gene', 'admin'),
('10/08/2025', '12:03:38 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ascorbic Acid 500mg Tablet (Generic)', 'admin'),
('10/08/2025', '12:06:23 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Bioflu Tablet', 'admin'),
('10/08/2025', '12:06:25 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Bioflu Tablet', 'admin'),
('10/08/2025', '12:07:08 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ascorbic Acid 500mg Tablet (Gene', 'admin'),
('10/08/2025', '12:07:11 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ascorbic Acid 500mg Tablet (Generic)', 'admin'),
('10/08/2025', '12:07:26 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ascorbic Acid 500mg Tablet (Gene', 'admin'),
('10/08/2025', '12:07:28 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ascorbic Acid 500mg Tablet (Generic)', 'admin'),
('10/08/2025', '12:07:45 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Biogesic 500mg Tablet', 'admin'),
('10/08/2025', '12:07:46 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Biogesic 500mg Tablet', 'admin'),
('10/08/2025', '12:08:07 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Buscopan Tablet', 'admin'),
('10/08/2025', '12:08:09 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Buscopan Tablet', 'admin'),
('10/08/2025', '12:09:00 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ascof Lagundi Syrup 60ml', 'admin'),
('10/08/2025', '12:09:02 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ascof Lagundi Syrup 60ml', 'admin'),
('10/08/2025', '12:09:26 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Biogesic Syrup 60ml', 'admin'),
('10/08/2025', '12:09:27 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Biogesic Syrup 60ml', 'admin'),
('10/08/2025', '12:10:20 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Fern-C 500mg Capsule', 'admin'),
('10/08/2025', '12:10:22 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Fern-C 500mg Capsule', 'admin'),
('10/08/2025', '12:10:29 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Fern-C 500mg Capsule', 'admin'),
('10/08/2025', '12:11:18 pm', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Alaxan FR Capsule, Price: 10.00?11.00', 'admin'),
('10/08/2025', '12:12:01 pm', 'ADJUSTMENT MANAGEMENT', 'PRICE UPDATE', 'Product: Ascof Lagundi Syrup 60ml, Price: 90.00?80', 'admin'),
('10/08/2025', '12:12:45 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/08/2025-082850-am - 1 items', 'admin'),
('10/08/2025', '12:13:10 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/08/2025-121310-pm | TOTAL: 80.0', 'admin'),
('10/08/2025', '12:13:30 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/08/2025-121310-pm - 1 items', 'admin'),
('10/08/2025', '12:14:35 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/08/2025-121435-pm | TOTAL: 443.', 'admin'),
('10/08/2025', '2:01 pm', 'ACCOUNTS MANAGEMENT', 'UPDATE', 'rhenn', 'admin'),
('10/08/2025', '2:57 pm', 'PRODUCTS MANAGEMENT', 'ADD', 'oilment', 'admin'),
('10/08/2025', '2:57 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'oilment', 'admin'),
('10/08/2025', '03:01:19 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Diatabs Capsule', 'admin'),
('10/08/2025', '03:01:22 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Diatabs Capsule', 'admin'),
('10/08/2025', '03:02:30 pm', 'PURCHASE ORDER MANAGEMENT', 'RECEIVE', 'Diatabs Capsule', 'admin'),
('10/08/2025', '3:04:52 pm', 'ADJUSTMENT MANAGEMENT', 'REMOVE ADJUSTMENT', 'Product: Diatabs Capsule, Qty: -20, Stock: 170?150', 'admin'),
('10/08/2025', '03:07:06 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/08/2025-030706-pm | TOTAL: 76.0', 'admin'),
('10/08/2025', '3:08:20 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/08/2025-030706-pm - 1 items', 'admin'),
('10/08/2025', '15:09:13', 'SALES REPORT', 'EXPORT', 'CSV Export: Daily - 10/08/2025', 'admin'),
('10/09/2025', '1:48 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Alaxan FR Capsule', 'admin'),
('10/09/2025', '1:48 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Diatabs Capsule', 'admin'),
('10/09/2025', '1:48 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Fern-C 500mg Capsule', 'admin'),
('10/09/2025', '1:48 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Imodium Capsule', 'admin'),
('10/09/2025', '1:48 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Myra-E 400IU Capsule', 'admin'),
('10/09/2025', '1:48 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Stresstabs Capsule', 'admin'),
('10/09/2025', '1:49 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Tuseran Forte Capsule', 'admin'),
('10/09/2025', '1:49 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Ascorbic Acid 500mg Tablet (Generic)', 'admin'),
('10/09/2025', '1:49 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Bioflu Tablet', 'admin'),
('10/09/2025', '1:49 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Biogesic 500mg Tablet', 'admin'),
('10/09/2025', '1:50 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Buscopan Tablet', 'admin'),
('10/09/2025', '1:50 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Centrum Advance Tablet', 'admin'),
('10/09/2025', '1:50 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Cetirizine 10mg Tablet', 'admin'),
('10/09/2025', '1:50 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Claritin 10mg Tablet', 'admin'),
('10/09/2025', '1:50 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Decolgen Forte Tablet', 'admin'),
('10/09/2025', '1:51 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Dulcolax 5mg Tablet', 'admin'),
('10/09/2025', '1:51 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Enervon Tablet', 'admin'),
('10/09/2025', '1:51 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Ibuprofen 400mg Tablet', 'admin'),
('10/09/2025', '1:51 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Kremil-S Tablet', 'admin'),
('10/09/2025', '1:51 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Mefenamic Acid 500mg Tablet', 'admin'),
('10/09/2025', '1:51 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Neozep Forte Tablet', 'admin'),
('10/09/2025', '1:51 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Paracetamol 500mg Tablet (Generic)', 'admin'),
('10/09/2025', '1:52 pm', 'SUPPLIER MANAGEMENT', 'UPDATE', 'ABC Company 1', 'admin'),
('10/09/2025', '1:52 pm', 'SUPPLIER MANAGEMENT', 'UPDATE', 'ABC Company', 'admin'),
('10/09/2025', '1:56 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Ascof Lagundi Syrup 60ml', 'admin'),
('10/09/2025', '1:57 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Biogesic Syrup 60ml', 'admin'),
('10/09/2025', '1:57 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Ceelin Syrup 120ml', 'admin'),
('10/09/2025', '1:57 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Robitussin DM Syrup 60ml', 'admin'),
('10/09/2025', '1:58 pm', 'SUPPLIER MANAGEMENT', 'ADD', 'JKL', 'admin'),
('10/09/2025', '1:58 pm', 'SUPPLIER MANAGEMENT', 'UPDATE', 'JKL Company', 'admin'),
('10/09/2025', '1:58 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Gaviscon Liquid 120ml', 'admin'),
('10/09/2025', '1:58 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Hydrite Oral Rehydration Salt Sachet', 'admin'),
('10/09/2025', '1:59 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Mucotuss Forte', 'admin'),
('10/09/2025', '1:59 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'oilment', 'admin'),
('10/09/2025', '1:59 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Tempra Forte 60ml (Paracetamol)', 'admin'),
('10/09/2025', '1:59 pm', 'PRODUCTS MANAGEMENT', 'UPDATE', 'Ventolin Inhaler 100mcg', 'admin'),
('10/09/2025', '2:03 pm', 'SUPPLIER MANAGEMENT', 'CASCADE UPDATE', 'Updated supplier in products from JKL Company to J', 'admin'),
('10/09/2025', '2:03 pm', 'SUPPLIER MANAGEMENT', 'UPDATE', 'JKL Company 1', 'admin'),
('10/09/2025', '2:03 pm', 'SUPPLIER MANAGEMENT', 'CASCADE UPDATE', 'Updated supplier in products from JKL Company 1 to', 'admin'),
('10/09/2025', '2:03 pm', 'SUPPLIER MANAGEMENT', 'UPDATE', 'JKL Company', 'admin'),
('10/09/2025', '02:13:39 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '7 RECORDS (ABC Company - All - None)', 'admin'),
('10/09/2025', '02:13:48 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '2 RECORDS (DEF Company - All - None)', 'admin'),
('10/09/2025', '02:13:53 pm', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '1 RECORDS (GHI Company - All - None)', 'admin'),
('10/09/2025', '02:15:11 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Ascorbic Acid 500mg Tablet (Gene', 'admin'),
('10/09/2025', '02:15:13 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Ascorbic Acid 500mg Tablet (Generic)', 'admin'),
('10/09/2025', '02:15:29 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Bioflu Tablet', 'admin'),
('10/09/2025', '02:15:30 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Bioflu Tablet', 'admin'),
('10/09/2025', '02:15:41 pm', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Biogesic 500mg Tablet', 'admin'),
('10/09/2025', '02:15:42 pm', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Biogesic 500mg Tablet', 'admin'),
('10/09/2025', '03:16:52 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/09/2025-031652-pm | TOTAL: 136.', 'admin'),
('10/09/2025', '15:35:11', 'SALES REPORT', 'EXPORT', 'CSV Export: Daily - 10/09/2025', 'admin'),
('10/09/2025', '15:49:52', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-031652-pm - 1 items', 'admin'),
('10/09/2025', '03:54:03 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/09/2025-035403-pm | TOTAL: 272.', 'admin'),
('10/09/2025', '03:55:21 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/09/2025-035521-pm | TOTAL: 459.', 'admin'),
('10/09/2025', '16:00:01', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-035521-pm - 4 items', 'admin'),
('10/09/2025', '16:01:36', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-035521-pm - 1 items', 'admin'),
('10/09/2025', '17:31:32', 'SALES REPORT', 'DELETE ALL', 'Daily sales and refunds for 10/08/2025', 'admin'),
('10/09/2025', '05:45:12 pm', 'POS', 'PURCHASE', 'ORDER ID: order-10/09/2025-054512-pm | TOTAL: 274.', 'admin'),
('10/09/2025', '17:45:57', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-054512-pm - 1 items', 'admin'),
('10/09/2025', '06:03:25 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-054512-pm - 1 items', 'admin'),
('10/09/2025', '06:50:04 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-054512-pm - 1 items', 'admin'),
('10/09/2025', '07:03:33 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-054512-pm - 1 items', 'admin'),
('10/09/2025', '07:23:08 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-054512-pm - 1 items', 'admin'),
('10/09/2025', '07:38:54 pm', 'SALES', 'REFUND', 'ORDER ID: order-10/09/2025-054512-pm - 1 items', 'admin'),
('10/10/2025', '11:30:10 am', 'UNIT COST HISTORY', 'HISTORY LOG', 'New unit cost for Gaviscon Liquid 120ml', 'admin'),
('10/10/2025', '11:30:11 am', 'PURCHASE ORDER MANAGEMENT', 'ADD', 'Gaviscon Liquid 120ml', 'admin'),
('10/10/2025', '11:30:14 am', 'PURCHASE ORDER MANAGEMENT', 'DELETE ALL', '1 RECORDS (JKL Company - All - None)', 'admin'),
('10/10/2025', '12:30:42 pm', 'ADJUSTMENT MANAGEMENT', 'ADD ADJUSTMENT', 'Product: Gaviscon Liquid 120ml, Qty: +15, Stock: 1', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `tblproducts`
--

CREATE TABLE `tblproducts` (
  `products` varchar(50) NOT NULL,
  `description` text NOT NULL,
  `unitprice` varchar(20) NOT NULL,
  `currentstock` varchar(20) NOT NULL,
  `supplier` varchar(50) NOT NULL,
  `createdby` varchar(50) NOT NULL,
  `datecreated` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblproducts`
--

INSERT INTO `tblproducts` (`products`, `description`, `unitprice`, `currentstock`, `supplier`, `createdby`, `datecreated`) VALUES
('Mucotuss Forte', '', '7.00', '223', 'JKL Company', 'admin', '11/09/2025'),
('Neozep Forte Tablet', '', '8.00', '99', 'ABC Company', 'admin', '09/18/2025'),
('Tuseran Forte Capsule', '', '14.00', '97', 'GHI Company', 'admin', '09/18/2025'),
('Biogesic 500mg Tablet', '', '7.00', '239', 'ABC Company', 'admin', '09/19/2025'),
('Bioflu Tablet', '', '9.00', '125', 'ABC Company', 'admin', '09/19/2025'),
('Alaxan FR Capsule', '', '11.00', '106', 'GHI Company', 'admin', '09/19/2025'),
('Decolgen Forte Tablet', '', '8.50', '175', 'ABC Company', 'admin', '09/19/2025'),
('Paracetamol 500mg Tablet (Generic)', '', '2.00', '498', 'ABC Company', 'admin', '09/19/2025'),
('Ibuprofen 400mg Tablet', '', '4.50', '285', 'ABC Company', 'admin', '09/19/2025'),
('Kremil-S Tablet', '', '10.00', '98', 'ABC Company', 'admin', '09/19/2025'),
('Ascof Lagundi Syrup 60ml', '', '80.00', '137', 'DEF Company', 'admin', '09/19/2025'),
('Ventolin Inhaler 100mcg', '', '350.00', '29', 'JKL Company', 'admin', '09/19/2025'),
('Cetirizine 10mg Tablet', '', '6.00', '191', 'ABC Company', 'admin', '09/19/2025'),
('Claritin 10mg Tablet', '', '35.00', '73', 'ABC Company', 'admin', '09/19/2025'),
('Myra-E 400IU Capsule', '', '12.00', '149', 'GHI Company', 'admin', '09/19/2025'),
('Enervon Tablet', '', '10.00', '98', 'ABC Company', 'admin', '09/19/2025'),
('Stresstabs Capsule', '', '18.00', '99', 'GHI Company', 'admin', '09/19/2025'),
('Centrum Advance Tablet', '', '22.00', '58', 'ABC Company', 'admin', '09/19/2025'),
('Ascorbic Acid 500mg Tablet (Generic)', '', '4.00', '251', 'ABC Company', 'admin', '09/19/2025'),
('Fern-C 500mg Capsule', '', '13.00', '88', 'GHI Company', 'admin', '09/19/2025'),
('Ceelin Syrup 120ml', '', '135.00', '56', 'DEF Company', 'admin', '09/19/2025'),
('Diatabs Capsule', '', '9.00', '149', 'GHI Company', 'admin', '09/19/2025'),
('Dulcolax 5mg Tablet', '', '12.00', '45', 'ABC Company', 'admin', '09/19/2025'),
('Gaviscon Liquid 120ml', '', '180.00', '30', 'JKL Company', 'admin', '09/19/2025'),
('Hydrite Oral Rehydration Salt Sachet', '', '8.00', '147', 'JKL Company', 'admin', '09/19/2025'),
('Imodium Capsule', '', '22.00', '69', 'GHI Company', 'admin', '09/19/2025'),
('Mefenamic Acid 500mg Tablet', '', '4.00', '249', 'ABC Company', 'admin', '09/19/2025'),
('Buscopan Tablet', '', '16.00', '115', 'ABC Company', 'admin', '09/19/2025'),
('Robitussin DM Syrup 60ml', '', '95.00', '30', 'DEF Company', 'admin', '09/19/2025'),
('Tempra Forte 60ml (Paracetamol)', '', '120.00', '40', 'JKL Company', 'admin', '09/19/2025'),
('Biogesic Syrup 60ml', '', '110.00', '68', 'DEF Company', 'admin', '09/19/2025'),
('oilment', '', '20.00', '40', 'JKL Company', 'admin', '10/08/2025');

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
('Alaxan FR Capsule', '3', '4', '12.00', 'Received', 'admin', '10/06/2025', '11:13:21 am', 'Test', '10/06/2025'),
('Alaxan FR Capsule', '2', '6', '12.00', 'Pending', 'admin', '10/07/2025', '11:52:09 pm', 'Test', ''),
('Ascorbic Acid 500mg Tablet (Generic)', '30', '2', '60.00', 'Pending', 'admin', '10/09/2025', '02:15:11 pm', 'ABC Company', ''),
('Bioflu Tablet', '20', '7', '140.00', 'Pending', 'admin', '10/09/2025', '02:15:29 pm', 'ABC Company', ''),
('Biogesic 500mg Tablet', '40', '4', '160.00', 'Pending', 'admin', '10/09/2025', '02:15:41 pm', 'ABC Company', '');

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
('order-10/06/2025-050135-pm', 'Ascof Lagundi Syrup 60ml', '1', '90.00', 'Wrong product', '10/06/2025', '5:02:52 pm', 'admin'),
('order-10/07/2025-083707-pm', 'Biogesic Syrup 60ml', '1', '110.00', 'Wrong quantity', '10/07/2025', '8:44:25 pm', 'admin'),
('order-10/07/2025-083634-pm', 'Ascof Lagundi Syrup 60ml', '1', '57.60', 'Wrong quantity', '10/07/2025', '8:45:34 pm', 'admin'),
('order-10/09/2025-031652-pm', 'Ascof Lagundi Syrup 60ml', '1', '51.20', 'Wrong quantity', '10/09/2025', '15:49:52', 'admin'),
('order-10/09/2025-035521-pm', 'Biogesic Syrup 60ml', '1', '70.40', 'Wrong products', '10/09/2025', '16:00:01', 'admin'),
('order-10/09/2025-035521-pm', 'Buscopan Tablet', '1', '10.24', 'Wrong products', '10/09/2025', '16:00:01', 'admin'),
('order-10/09/2025-035521-pm', 'Centrum Advance Tablet', '1', '14.08', 'Wrong products', '10/09/2025', '16:00:01', 'admin'),
('order-10/09/2025-035521-pm', 'Cetirizine 10mg Tablet', '1', '3.84', 'Wrong products', '10/09/2025', '16:00:01', 'admin'),
('order-10/09/2025-035521-pm', 'Ascorbic Acid 500mg Tablet (Generic)', '1', '2.56', 'Wrong product', '10/09/2025', '16:01:36', 'admin'),
('order-10/09/2025-054512-pm', 'Ibuprofen 400mg Tablet', '1', '2.88', 'Wrong quantity', '10/09/2025', '17:45:57', 'admin'),
('order-10/09/2025-054512-pm', 'Ibuprofen 400mg Tablet', '1', '2.91', 'Wrong quantity', '10/09/2025', '06:03:25 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Ibuprofen 400mg Tablet', '1', '2.94', 'Wrong quantity', '10/09/2025', '06:50:04 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Ibuprofen 400mg Tablet', '1', '2.98', 'Wrong quantity', '10/09/2025', '07:03:33 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Ibuprofen 400mg Tablet', '1', '3.01', 'Wrong quantity', '10/09/2025', '07:23:08 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Ibuprofen 400mg Tablet', '1', '3.05', 'Wrong quantity', '10/09/2025', '07:38:54 pm', 'admin');

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
  `discountedname` varchar(50) DEFAULT NULL,
  `discountedid` varchar(50) DEFAULT NULL,
  `datecreated` varchar(20) NOT NULL,
  `timecreated` varchar(20) NOT NULL,
  `createdby` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblsales`
--

INSERT INTO `tblsales` (`orderid`, `products`, `quantity`, `payment`, `paymentchange`, `totalcost`, `discounted`, `discountedname`, `discountedid`, `datecreated`, `timecreated`, `createdby`) VALUES
('order-10/06/2025-050212-pm', 'Bioflu Tablet', '6', '400.00', '84.00', '54.00', 'No', '', '', '10/06/2025', '05:02:12 pm', 'admin'),
('order-10/06/2025-050212-pm', 'Biogesic 500mg Tablet', '6', '400.00', '84.00', '42.00', 'No', '', '', '10/06/2025', '05:02:12 pm', 'admin'),
('order-10/06/2025-050212-pm', 'Biogesic Syrup 60ml', '1', '400.00', '84.00', '110.00', 'No', '', '', '10/06/2025', '05:02:12 pm', 'admin'),
('order-10/06/2025-050325-pm', 'Ceelin Syrup 120ml', '1', '200.00', '39.50', '140.00', 'No', '', '', '10/06/2025', '05:03:25 pm', 'admin'),
('order-10/06/2025-050325-pm', 'Decolgen Forte Tablet', '1', '200.00', '39.50', '8.50', 'No', '', '', '10/06/2025', '05:03:25 pm', 'admin'),
('order-10/06/2025-050325-pm', 'Dulcolax 5mg Tablet', '1', '200.00', '39.50', '12.00', 'No', '', '', '10/06/2025', '05:03:25 pm', 'admin'),
('order-10/07/2025-083634-pm', 'Ascof Lagundi Syrup 60ml', '4', '400.00', '40.00', '302.40', 'Yes', '', '', '10/07/2025', '08:36:34 pm', 'admin'),
('order-10/07/2025-083707-pm', 'Biogesic Syrup 60ml', '4', '600.00', '50.00', '440.00', 'No', '', '', '10/07/2025', '08:37:07 pm', 'admin'),
('order-10/09/2025-031652-pm', 'Alaxan FR Capsule', '1', '150.00', '13.20', '8.80', 'Yes', 'Test', '1239756834381', '10/09/2025', '03:16:52 pm', 'admin'),
('order-10/09/2025-031652-pm', 'Ascof Lagundi Syrup 60ml', '1', '150.00', '13.20', '76.80', 'Yes', 'Test', '1239756834381', '10/09/2025', '03:16:52 pm', 'admin'),
('order-10/09/2025-035403-pm', 'Alaxan FR Capsule', '1', '300.00', '27.20', '8.80', 'Yes', 'Test', '123456', '10/09/2025', '03:54:03 pm', 'admin'),
('order-10/09/2025-035403-pm', 'Biogesic Syrup 60ml', '3', '300.00', '27.20', '264.00', 'Yes', 'Test', '123456', '10/09/2025', '03:54:03 pm', 'admin'),
('order-10/09/2025-035521-pm', 'Alaxan FR Capsule', '1', '500.00', '40.80', '8.80', 'Yes', 'ABCD', '1234', '10/09/2025', '03:55:21 pm', 'admin'),
('order-10/09/2025-035521-pm', 'Ascof Lagundi Syrup 60ml', '1', '500.00', '40.80', '64.00', 'Yes', 'ABCD', '1234', '10/09/2025', '03:55:21 pm', 'admin'),
('order-10/09/2025-035521-pm', 'Bioflu Tablet', '3', '500.00', '40.80', '21.60', 'Yes', 'ABCD', '1234', '10/09/2025', '03:55:21 pm', 'admin'),
('order-10/09/2025-035521-pm', 'Biogesic 500mg Tablet', '4', '500.00', '40.80', '22.40', 'Yes', 'ABCD', '1234', '10/09/2025', '03:55:21 pm', 'admin'),
('order-10/09/2025-035521-pm', 'Ceelin Syrup 120ml', '2', '500.00', '40.80', '216.00', 'Yes', 'ABCD', '1234', '10/09/2025', '03:55:21 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Claritin 10mg Tablet', '1', '300.00', '26.00', '28.00', 'Yes', 'RM', '123456', '10/09/2025', '05:45:12 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Decolgen Forte Tablet', '1', '300.00', '26.00', '6.80', 'Yes', 'RM', '123456', '10/09/2025', '05:45:12 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Diatabs Capsule', '1', '300.00', '26.00', '7.20', 'Yes', 'RM', '123456', '10/09/2025', '05:45:12 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Dulcolax 5mg Tablet', '1', '300.00', '26.00', '9.60', 'Yes', 'RM', '123456', '10/09/2025', '05:45:12 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Gaviscon Liquid 120ml', '1', '300.00', '26.00', '144.00', 'Yes', 'RM', '123456', '10/09/2025', '05:45:12 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Hydrite Oral Rehydration Salt Sachet', '1', '300.00', '26.00', '6.40', 'Yes', 'RM', '123456', '10/09/2025', '05:45:12 pm', 'admin'),
('order-10/09/2025-054512-pm', 'Ibuprofen 400mg Tablet', '14', '300.00', '26.00', '54.23', 'Yes', 'RM', '123456', '10/09/2025', '05:45:12 pm', 'admin');

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
('ABC Company', 'Tablet products', '028-123-4567', 'admin', '09-11-2025'),
('DEF Company', 'Syrup products', '0913256789', 'admin', '09/18/2025'),
('GHI Company', 'Capsule Products', '377534897', 'admin', '10/08/2025'),
('JKL Company', 'Other products', '09123456789', 'admin', '10/09/2025');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
