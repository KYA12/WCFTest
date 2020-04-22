USE [TestDatabase]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cheques](
	[cheque_id] [uniqueidentifier] NOT NULL,
	[cheque_number] [nvarchar](50) NOT NULL,
	[summ] [money] NOT NULL,
	[discount] [money] NULL,
	[articles] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[cheque_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Cheques] ADD  DEFAULT (newid()) FOR [cheque_id]
GO
