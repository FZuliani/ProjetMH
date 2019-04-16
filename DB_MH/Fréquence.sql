CREATE TABLE [dbo].[Fréquence]
(
	[frequence] FLOAT NOT NULL PRIMARY KEY DEFAULT 440.00, 
    [Name English] VARCHAR(20) NOT NULL, 
    [Name French] VARCHAR(20) NULL,
    [carractère] IMAGE NULL
	
)
