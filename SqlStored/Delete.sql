SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE DeleteBook
@in_Id int



AS
BEGIN

Declare @loc_EM nvarchar(200),
@loc_Check int

	BEGIN TRANSACTION
	Set Transaction ISOLATION LEVEL
	READ COMMITTED

	BEGIN TRY 
		Select @loc_Check = (select count(Distinct [Name]) from Books 
		Where Id = @in_Id)

		IF @loc_Check = 0
			Throw 51001, 'Такой книги нету!', 1;

		Delete From Books 
		Where Id = @in_Id	
		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION; 
		Select @loc_EM = ERROR_MESSAGE();
		Throw 51001, @loc_EM, 1;
	END CATCH
END;
