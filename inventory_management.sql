-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 19, 2025 at 05:51 AM
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
('09/19/2025', '11:35:14', 'POS', 'PURCHASE', 'TOTAL: 37.00', 'admin');

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
('Mucotuss Forte', '', '7', '224', 'admin', '11/09/2025'),
('Neozep', '', '5', '244', 'admin', '09/18/2025'),
('Tuseran', '', '7', '60', 'admin', '09/18/2025');

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
('Mucotuss Forte', '100', '3', '300.00', 'Received', 'admin', '09/15/2025', 'Test', '09/18/2025'),
('Neozep', '150', '5', '750.00', 'Received', 'admin', '09/18/2025', 'Test', '09/18/2025'),
('Tuseran', '60', '7', '420.00', 'Received', 'admin', '09/18/2025', 'Test', '09/18/2025'),
('neozep', '100', '5', '500.00', 'Received', 'admin', '09/18/2025', 'Test', '09/18/2025'),
('Mucotuss Forte', '20', '5', '100.00', 'Received', 'admin', '09/18/2025', 'Test', '09/18/2025');

-- --------------------------------------------------------

--
-- Table structure for table `tblsales`
--

CREATE TABLE `tblsales` (
  `products` varchar(50) NOT NULL,
  `quantity` varchar(50) NOT NULL,
  `totalcost` varchar(50) NOT NULL,
  `datecreated` varchar(20) NOT NULL,
  `timecreated` varchar(20) NOT NULL,
  `createdby` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblsales`
--

INSERT INTO `tblsales` (`products`, `quantity`, `totalcost`, `datecreated`, `timecreated`, `createdby`) VALUES
('Mucotuss Forte', '20', '140', '09/18/2025', '', 'admin'),
('Neozep', '20', '100', '09/18/2025', '10:47 am', 'admin'),
('Mucotuss Forte', '20', '140', '09/18/2025', '10:46 am', 'admin'),
('Tuseran', '20', '140', '09/19/2025', '10:46 am', 'admin'),
('Neozep', '20', '100', '09/19/2025', '10:47 am', 'admin'),
('Mucotuss Forte', '1', '7.00', '09/19/2025', '11:35:14', 'admin'),
('Neozep', '6', '30.00', '09/19/2025', '11:35:14', 'admin');

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
