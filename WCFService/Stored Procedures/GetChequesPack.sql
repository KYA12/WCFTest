USE TestDatabase;
GO
CREATE PROCEDURE [dbo].[GetChequesPack]
	@amount int
AS
BEGIN
	SELECT TOP (@amount) ChequeId as Id, ChequeNumber as Number, Discount, Summ, Articles
	FROM Cheques 
END;
GO