USE TestDatabase;
GO
CREATE PROCEDURE [dbo].[SaveCheque]
	@cheque_id uniqueidentifier,
	@cheque_number nvarchar(50),
	@summ money,
	@discount money,
	@articles nvarchar(MAX)
AS
INSERT INTO Cheques(cheque_id, cheque_number, summ, discount, articles) VALUES(@cheque_id, @cheque_number, @summ, @discount, @articles)
GO