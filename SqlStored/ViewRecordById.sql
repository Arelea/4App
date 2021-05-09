USE [Mememe]
GO
/****** Object:  StoredProcedure [dbo].[ViewRecordById]    Script Date: 10.05.2021 1:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ViewRecordById]
@in_Id int
AS
BEGIN

Declare @loc_EM nvarchar(200)
	BEGIN TRANSACTION	
	BEGIN TRY 

		Select * 
		From Books 
		Where Id = @in_Id
		COMMIT TRANSACTION;

		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION; 
			Select @loc_EM = ERROR_MESSAGE();
			Throw 51001, @loc_EM, 1;
		END CATCH
END;
