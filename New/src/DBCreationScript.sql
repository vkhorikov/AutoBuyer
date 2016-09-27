USE [master]
GO

CREATE DATABASE [AutoBuyer]
GO

USE [AutoBuyer]
GO

CREATE TABLE [dbo].[Buyer](
	[ItemID] [nvarchar](200) NOT NULL,
	[BuyerName] [nvarchar](200) NOT NULL,
	[MaximumPrice] [int] NOT NULL,
	[NumberToBuy] [int] NOT NULL,
	[CurrentPrice] [int] NOT NULL,
	[NumberInStock] [int] NOT NULL,
	[BoughtSoFar] [int] NOT NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Buyer] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
