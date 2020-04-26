USE TestDatabase;
GO
CREATE PROCEDURE [dbo].[SaveCheque]
	@ChequeId uniqueidentifier,
	@Number nvarchar(50),
	@Summ money,
	@Discount money,
	@Articles nvarchar(MAX)
AS
INSERT INTO Cheques(ChequeId, Number, Summ, Discount, Articles) VALUES(@ChequeId, @Number, @Summ, @Discount, @Articles)
GO