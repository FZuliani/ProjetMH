﻿/*
Deployment script for DB_MH

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "DB_MH"
:setvar DefaultFilePrefix "DB_MH"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL12.TECHNOBELSQL2014\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL12.TECHNOBELSQL2014\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Dropping [dbo].[FK_Accord_Note]...';


GO
ALTER TABLE [dbo].[Accord] DROP CONSTRAINT [FK_Accord_Note];


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
USE DB_MH
GO

INSERT INTO Accord(Accord,Note) VALUES 
('(X,3,2,0,1,0)', 261.63),
('(X,4,6,6,6,4)', 277.18),
('(X,X,0,2,3,2)', 293.66),
('(X,6,8,8,8,6)', 311.13),
('(0,2,2,1,0,0)', 329.63),
('(1,3,3,2,1,1)', 349.23),
('(2,4,4,3,2,2)', 369.99),
('(3,2,0,0,3,3)', 392.00),
('(4,6,6,5,4,4)', 415.30),
('(X,0,2,2,2,0)', 440.00),
('(X,1,3,3,3,1)', 466.16),
('(X,2,3,3,3,2)', 493.88),
('(X,3,5,5,X,X)', 523.25),
('(X,4,6,6,X,X)', 554.37),
('(X,5,7,7,X,X)', 587.33),
('(X,6,8,8,X,X)', 622.25),
('(0,2,2,0,0,0)', 659.26),
('(1,3,3,X,X,X)', 698.46),
('(2,4,4,X,X,X)', 739.99),
('(3,5,5,X,X,X)', 783.99),
('(4,6,6,X,X,X)', 830.61),
('(X,0,2,2,X,X)', 880.00),
('(X,1,3,3,X,X)', 932.33),
('(X,2,4,4,X,X)', 987.77),
('(X,3,2,2,1,0)', 1046.50),
('(X,4,6,6,6,6)', 1108.73),
('(X,X,0,2,0,2)', 1174.66),
('(X,X,1,3,1,3)', 1244.51),
('(0,2,2,1,2,0)', 1318.51),
('(X,X,3,2,3,1)', 1396.91),
('(X,X,4,3,4,2)', 1479.98),
('(3,2,0,0,0,0)', 1567.98),
('(X,X,6,5,6,4)', 1661.22),
('(X,0,2,2,2,2)', 1760.00),
('(X,1,3,3,3,3)', 1864.66),
('(X,2,1,1,0,2)', 1975.53),
('(X,3,2,3,1,0)', 2093.00),
('(X,4,6,4,6,4)', 2217.46),
('(X,X,0,2,1,2)', 2349.32),
('(X,X,1,3,2,3)', 2489.02),
('(0,2,0,2,0,0)', 2637.02),
('(1,3,1,2,1,1)', 2793.83),
('(2,4,2,3,2,2)', 2959.96),
('(3,2,0,0,0,1)', 3135.96),
('(4,6,4,5,4,4)', 3322.44),
('(X,0,2,0,2,0)', 3520.00),
('(X,1,3,1,3,1)', 3729.31),
('(X,2,1,2,0,2)', 3951.07),
('(X,3,2,3,3,1)', 8372.01),
('(X,4,3,4,4,X)', 8869.84),
('(X,X,0,2,1,3)', 9397.27),
('(X,6,5,6,6,X)', 9956.06),
('(0,2,0,1,0,2)', 10548.08),
('(1,3,1,2,1,3)', 11175.30),
('(2,1,2,3,2,4)', 11839.81),
('(3,5,3,4,3,5)', 12543.85),
('(4,6,4,5,4,6)', 13289.74),
('(X,0,2,4,2,3)', 14080.00),
('(X,1,0,1,1,X)', 14917.23),
('(X,2,1,2,2,X)', 15804.26);
GO

GO
PRINT N'Update complete.';


GO
