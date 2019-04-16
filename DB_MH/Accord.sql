CREATE TABLE [dbo].[Accord]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Note] FLOAT NOT NULL DEFAULT 440.00, 
    [Accord] NCHAR(13) NOT NULL DEFAULT '(0,0,0,0,0,0)',
	CONSTRAINT CK_Accord CHECK (Accord LIKE '(%,%,%,%,%,%)')

)
