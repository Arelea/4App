SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE AddBook
@in_Name NVARCHAR(100),
@in_Author NVARCHAR(100),
@in_Year int,
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
		Where [Name] = @in_Name AND Author = @in_Author AND [Year] = @in_Year)
		
		IF @loc_Check > 0
			Throw 51001, 'Такая книга уже существует!', 1;

		INSERT INTO Books
           ([Name]
           ,Author
           ,[Year]
		   ,Content
           )
		VALUES
           (@in_Name,
           @in_Author,
           @in_Year,
		   @in_Content)

		COMMIT TRANSACTION;

		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION; 
			Select @loc_EM = ERROR_MESSAGE();
			Throw 51001, @loc_EM, 1;
		END CATCH
END;
