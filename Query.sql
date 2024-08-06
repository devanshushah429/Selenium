CREATE Procedure PR_AuctionsDemo_InsertData
@Title nvarchar(50),
@Description nvarchar(max),
@ImageUrl nvarchar(max),
@Link nvarchar(max),
@LotCount int,
@StartDate int,
@StartMonth nvarchar(50),
@StartYear int,
@StartTime nvarchar(50),
@EndDate int,
@EndMonth nvarchar(50),
@EndYear int,
@EndTime nvarchar(50),
@Location nvarchar(100)
AS
BEGIN
	INSERT INTO AuctionsDemo
	(
		Title
		,Description
		,ImageUrl
		,Link
		,LotCount
		,StartDate
		,StartMonth
		,StartYear
		,StartTime
		,EndDate
		,EndMonth
		,EndYear
		,EndTime
		,Location
		)
	Values
	(
		@Title
		,@Description
		,@ImageUrl
		,@Link
		,@LotCount
		,@StartDate
		,@StartMonth
		,@StartYear
		,@StartTime
		,@EndDate
		,@EndMonth
		,@EndYear
		,@EndTime
		,@Location
	)
END
Truncate table AuctionsDemo
SELECT * FROM AuctionsDemo

CREATE OR ALTER Procedure PR_GetDataByLink
@Link nvarchar(max)
AS
Begin
	SELECT
		Title
		,Description
		,ImageUrl
		,Link
		,LotCount
		,StartDate
		,StartMonth
		,StartYear
		,StartTime
		,EndDate
		,EndMonth
		,EndYear
		,EndTime
		,Location
	FROM AuctionsDemo
	WHERE Link = @Link
END

CREATE OR ALTER PROCEDURE PR_UpdateDataByLink
@Title NVARCHAR(50),
@Description NVARCHAR(MAX),
@ImageUrl NVARCHAR(max),
@Link NVARCHAR(max),
@LotCount int,
@StartDate int,
@StartMonth NVARCHAR(50),
@StartYear INT,
@StartTime NVARCHAR(50),
@EndDate INT,
@EndMonth NVARCHAR(50),
@EndYear int,
@EndTime NVARCHAR(50),
@Location NVARCHAR(100)
AS
BEGIN
	UPDATE AuctionsDemo 
	SET Title = @Title
		,Description = @Description
		,ImageUrl = @ImageUrl
		,LotCount = @LotCount
		,StartDate = @StartDate
		,StartMonth = @StartMonth
		,StartYear = @StartYear
		,StartTime = @StartTime
		,EndDate = @EndDate
		,EndMonth = @EndMonth
		,EndYear = @EndYear
		,EndTime = @EndTime
		,Location = @Location
	WHERE LINK = @Link
END