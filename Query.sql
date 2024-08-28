CREATE OR ALTER PROCEDURE PR_AuctionsDemo_InsertData
@Title NVARCHAR(50),
@Description NVARCHAR(MAX),
@ImageUrl NVARCHAR(MAX),
@Link NVARCHAR(MAX),
@LotCount INT,
@StartDate INT,
@StartMonth NVARCHAR(50),
@StartYear INT,
@StartTime NVARCHAR(50),
@EndDate INT,
@EndMonth NVARCHAR(50),
@EndYear INT,
@EndTime NVARCHAR(50),
@Location NVARCHAR(100)
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
	VALUES
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
Select * FROM AuctionsDemo

CREATE OR ALTER PROCEDURE PR_GetDataByLink
@Link NVARCHAR(MAX)
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
@ImageUrl NVARCHAR(MAX),
@Link NVARCHAR(MAX),
@LotCount INT,
@StartDate INT,
@StartMonth NVARCHAR(50),
@StartYear INT,
@StartTime NVARCHAR(50),
@EndDate INT,
@EndMonth NVARCHAR(50),
@EndYear INT,
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