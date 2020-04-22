USE TestDatabase;
GO
CREATE PROCEDURE [dbo].[GetChequesPack]
	@amount int
AS
BEGIN
	SELECT TOP (@amount) cheque_id as Id, cheque_number as Number, discount, summ, articles
	FROM Cheques 
END;
GO