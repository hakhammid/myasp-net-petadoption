-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 15, 2025 at 10:09 AM
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
-- Database: `petadoptiondb`
--

-- --------------------------------------------------------

--
-- Table structure for table `adoptionrequests`
--

CREATE TABLE `adoptionrequests` (
  `RequestID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `PetID` int(11) NOT NULL,
  `Message` varchar(500) NOT NULL,
  `DateRequested` datetime(6) NOT NULL,
  `Status` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `pets`
--

CREATE TABLE `pets` (
  `PetID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Breed` varchar(50) NOT NULL,
  `Age` int(11) NOT NULL,
  `Gender` varchar(10) NOT NULL,
  `Description` varchar(1000) NOT NULL,
  `ImagePath` varchar(500) DEFAULT NULL,
  `IsAdopted` tinyint(1) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pets`
--

INSERT INTO `pets` (`PetID`, `Name`, `Breed`, `Age`, `Gender`, `Description`, `ImagePath`, `IsAdopted`, `CreatedDate`) VALUES
(9, 'Fajin Hak', 'AsPin', 21, 'Male', 'Adik mag Warzoned at CodM ', 'uploads/pets/98a37ea4-effb-4dda-bd47-b73be967ea25_faijin pic.jpg', 0, '2025-06-15 04:12:08.361415');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `PasswordHash` longtext NOT NULL,
  `Role` varchar(20) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`UserID`, `Name`, `Email`, `PasswordHash`, `Role`, `CreatedDate`) VALUES
(1, 'Admin', 'admin@petadoption.com', '$2a$11$vhJa6hRJqCH30GjUKW33BOcjO1hv/H5vmhRqlPAART3c2VYkfNdr2', 'Admin', '2025-06-14 11:44:37.000000'),
(2, 'Hammid', 'hakhammid@gmail.com', '$2a$11$3o.TXGg34e6Re3LgNqH4SOv7wE2hPgjlePoXnN5DgxN7cy3LkWOny', 'User', '2025-06-14 13:03:23.000000'),
(3, 'adasd', 'test@gmail.com', '$2a$11$2B1QBU.7zuqZ1W4cD53Q3u9OQBQOdB6T7ZI37y8AVVMNwakm72G2W', 'User', '2025-06-14 13:07:42.752199'),
(4, 'Fajin Hak', 'hackfaijin12@gmail.com', '$2a$11$.q2bg7xsw.5BIYaKYCpeBOZuqV12JiC5IuaA6XwJNec0bMu4OzIXq', 'User', '2025-06-14 19:49:27.474422');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20250614034437_InitialCreate', '8.0.13');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `adoptionrequests`
--
ALTER TABLE `adoptionrequests`
  ADD PRIMARY KEY (`RequestID`),
  ADD KEY `IX_AdoptionRequests_PetID` (`PetID`),
  ADD KEY `IX_AdoptionRequests_UserID` (`UserID`);

--
-- Indexes for table `pets`
--
ALTER TABLE `pets`
  ADD PRIMARY KEY (`PetID`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `IX_Users_Email` (`Email`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `adoptionrequests`
--
ALTER TABLE `adoptionrequests`
  MODIFY `RequestID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `pets`
--
ALTER TABLE `pets`
  MODIFY `PetID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `adoptionrequests`
--
ALTER TABLE `adoptionrequests`
  ADD CONSTRAINT `FK_AdoptionRequests_Pets_PetID` FOREIGN KEY (`PetID`) REFERENCES `pets` (`PetID`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AdoptionRequests_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
