USE [Mememe]
GO
/****** Object:  StoredProcedure [dbo].[ShowBook]    Script Date: 10.05.2021 1:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ShowBook]

AS
BEGIN

Declare @loc_EM nvarchar(200)
	BEGIN TRANSACTION	
	BEGIN TRY 

		Select * 
		From Books 	
		COMMIT TRANSACTION;

		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION; 
			Select @loc_EM = ERROR_MESSAGE();
			Throw 51001, @loc_EM, 1;
		END CATCH
END;
