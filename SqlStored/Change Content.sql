USE [Mememe]
GO
/****** Object:  StoredProcedure [dbo].[UpdateBook]    Script Date: 10.05.2021 3:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE ChangeContent
@in_Id int,
@in_Content XML


AS
BEGIN
	BEGIN TRANSACTION
	Set Transaction ISOLATION LEVEL
	READ COMMITTED

	Declare @loc_Check int,
	@loc_EM NVARCHAR(200)
	SET NOCOUNT ON;

	BEGIN TRY 

		Select @loc_Check = (select count(Distinct [Name]) from Books 
		Where Id = @in_Id)
		
		IF @loc_Check = 0
			Throw 51001, 'Книги, которую вы пытаетесь изменить, не существует!', 1;

		UPDATE Books
		SET Content = @in_Content		
		WHERE Id = @in_Id

		COMMIT TRANSACTION;

		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION; 
			Select @loc_EM = ERROR_MESSAGE();
			Throw 51001, @loc_EM, 1;
		END CATCH
END;
