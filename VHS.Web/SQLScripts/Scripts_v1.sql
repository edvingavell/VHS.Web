/****** Object:  Table [dbo].[Address]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressId] [uniqueidentifier] NOT NULL,
	[RegistrationNumber] [nvarchar](50) NOT NULL,
	[Destination] [nvarchar](255) NOT NULL,
	[DateOfCreation] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Address] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Alarm]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alarm](
	[AlarmId] [uniqueidentifier] NOT NULL,
	[RegistrationNumber] [nvarchar](50) NOT NULL,
	[DateOfCreation] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
	[positionLatitude] [float] NULL,
	[positionLongitude] [float] NULL,
 CONSTRAINT [PK_Alarm] PRIMARY KEY CLUSTERED 
(
	[AlarmId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DrivingJournal]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DrivingJournal](
	[DrivingJournalId] [uniqueidentifier] NOT NULL,
	[RegistrationNumber] [nvarchar](50) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[StopTime] [datetime] NOT NULL,
	[DistanceInKm] [float] NOT NULL,
	[EnergyConsumptionInkWh] [float] NOT NULL,
	[AverageConsumptionInkWhPer100km] [float] NOT NULL,
	[AverageSpeedInKmPerHour] [float] NOT NULL,
	[DateOfCreation] [datetime] NOT NULL,
	[BuisnessTravel] [int] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Drivingjournal] PRIMARY KEY CLUSTERED 
(
	[DrivingJournalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [uniqueidentifier] NOT NULL,
	[RegistrationNumber] [nvarchar](50) NOT NULL,
	[BatteryStatus] [int] NOT NULL,
	[TripMeter] [float] NOT NULL,
	[EngineRunning] [int] NOT NULL,
	[LockStatus] [int] NOT NULL,
	[AlarmStatus] [int] NOT NULL,
	[DateOfCreation] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
	[TirePressures] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[RegistrationNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Table_1]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_1](
	[addressID] [nvarchar](255) NOT NULL,
	[regNumber] [nvarchar](255) NOT NULL,
	[dest] [nvarchar](255) NOT NULL,
	[dCreation] [nvarchar](255) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehiclePosition]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehiclePosition](
	[VehiclePositionId] [uniqueidentifier] NOT NULL,
	[RegistrationNumber] [nvarchar](50) NOT NULL,
	[PositionLatitude] [float] NULL,
	[PositionLongitude] [float] NULL,
	[Point] [geography] NULL,
	[DateOfCreation] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
	[PositionRadius] [float] NULL,
 CONSTRAINT [PK_VehiclePosition] PRIMARY KEY CLUSTERED 
(
	[RegistrationNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DrivingJournal]    Script Date: 2021-12-17 15:11:53 ******/
CREATE NONCLUSTERED INDEX [IX_DrivingJournal] ON [dbo].[DrivingJournal]
(
	[RegistrationNumber] ASC,
	[StartTime] ASC,
	[StopTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sAddress_GetAllByRegNo]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sAddress_GetAllByRegNo]
	@RegistrationNumber nvarchar (50)
AS
BEGIN
		SELECT 
			* 
		FROM 
			dbo.Address AS a
		WHERE
		a.RegistrationNumber = @RegistrationNumber
		ORDER BY a.DateOfCreation DESC
END
GO
/****** Object:  StoredProcedure [dbo].[sAddress_GetLastByRegNo]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sAddress_GetLastByRegNo]
	@RegistrationNumber nvarchar (50)
AS
BEGIN
		SELECT TOP 1 * 
		FROM dbo.Address AS a
		WHERE
		a.RegistrationNumber = @RegistrationNumber
		ORDER BY a.DateOfCreation DESC
END
GO
/****** Object:  StoredProcedure [dbo].[sAddress_Post]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sAddress_Post]
	@AddressId uniqueidentifier output,
	@RegistrationNumber nvarchar (50),
	@Destination nvarchar (255),
	@DateOfCreation datetime output
AS
BEGIN
	SET @AddressId = NEWID();
	SET @DateOfCreation = GETDATE();
	INSERT INTO
		dbo.Address
	VALUES 
	(
		@AddressId,
		@RegistrationNumber,
		@Destination,
		@DateOfCreation
	);

END
GO
/****** Object:  StoredProcedure [dbo].[sAlarm_Post]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sAlarm_Post]
	@AlarmId uniqueidentifier output,
	@RegistrationNumber nvarchar (50),
	@PositionLatitude float,
	@PositionLongitude float
AS
BEGIN
	SET @AlarmId = NEWID();

		INSERT INTO 
			dbo.Alarm
		VALUES
			(
				@AlarmId,
				@RegistrationNumber,
				GETDATE(),
				GETDATE(),
				@PositionLatitude,
				@PositionLongitude
			);
END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_DeleteByGuid]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_DeleteByGuid]
	@DrivingJournalId uniqueidentifier
AS
BEGIN
	DELETE FROM dbo.DrivingJournal WHERE @DrivingJournalId = DrivingJournalId
--"DELETE FROM " + "dbo.DrivingJournal" + " WHERE " + "DrivingJournalId" + " = '" + id + "'", con
END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_Get]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_Get]
    @RegistrationNumber nvarchar (50),
	@FilterGuid uniqueidentifier,
	@FilterDate datetime,
	@FilterStartTime datetime,
	@FilterStopTime datetime,
	@FilterBuisnessTravel bit
AS
BEGIN
	IF(@FilterGuid is null AND @FilterDate is null AND @FilterStartTime is null
		AND @FilterStopTime is null AND @FilterBuisnessTravel is null) 
	BEGIN
		SELECT
			d.DrivingJournalId,
			d.RegistrationNumber,
			d.StartTime,
			d.StopTime,
			d.DistanceInKm,
			d.EnergyConsumptionInkWh,
			d.AverageConsumptionInkWhPer100km,
			d.AverageSpeedInKmPerHour,
			d.DateOfCreation,
			CAST(d.BuisnessTravel AS bit),
			d.DateLastModified
		FROM dbo.DrivingJournal AS d
		WHERE d.RegistrationNumber = @RegistrationNumber
		ORDER BY   
		d.RegistrationNumber,
		d.StartTime,
		d.StopTime
    END 
    ELSE BEGIN
		DECLARE @SQL nvarchar (4000);
		SET @SQL = '
		SELECT
			d.DrivingJournalId,
			d.RegistrationNumber,
			d.StartTime,
			d.StopTime,
			d.DistanceInKm,
			d.EnergyConsumptionInkWh,
			d.AverageConsumptionInkWhPer100km,
			d.AverageSpeedInKmPerHour,
			d.DateOfCreation,
			CAST(d.BuisnessTravel AS bit),
			d.DateLastModified
		FROM dbo.DrivingJournal AS d
		WHERE
			d.RegistrationNumber = ''' + @RegistrationNumber +''' ';
		IF (NOT @FilterGuid IS NULL) BEGIN
			SET @SQL = @SQL + ' AND d.DrivingJournalId = ''' + CONVERT(nvarchar(50), @FilterGuid) + ''' ';
		END
		IF (NOT @FilterBuisnessTravel IS NULL) BEGIN
			SET @SQL = @SQL + ' AND d.BuisnessTravel = ' + CONVERT(nvarchar(50), @FilterBuisnessTravel) + ' ';
		END
		IF (NOT @FilterDate IS NULL) BEGIN
			DECLARE @Year int;
			DECLARE @Month int;
			DECLARE @Day int;

			SET @Year = YEAR(@FilterDate);
			SET @Month = MONTH(@FilterDate);
			SET @Day = DAY(@FilterDate);

			SET @SQL = @SQL + ' AND YEAR(d.StartTime) = ' + CONVERT(nvarchar(50), @Year) + ' ';
			SET @SQL = @SQL + ' AND MONTH(d.StartTime) = ' + CONVERT(nvarchar(50), @Month) + ' ';
			SET @SQL = @SQL + ' AND DAY(d.StartTime) = ' + CONVERT(nvarchar(50), @Day) + ' ';
		END
		IF (NOT @FilterStartTime IS NULL AND NOT @FilterStopTime IS NULL) BEGIN
			SET @SQL = @SQL + ' AND d.StartTime >= ''' + CONVERT(nvarchar(50), @FilterStartTime) + 
			 ''' AND d.StartTime <= ''' + CONVERT(nvarchar(50), @FilterStopTime) + ''' ';
		END
		SET @SQL = @SQL + '	ORDER BY           
			d.RegistrationNumber,
			d.StartTime,
			d.StopTime';
		EXECUTE sp_executesql @SQL
       END
END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_GetByGuid]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_GetByGuid]
	@DrivingJournalId uniqueidentifier
AS
BEGIN
	SELECT 
		DrivingJournalId = d.DrivingjournalId, 
		RegistrationNumber = d.RegistrationNumber, 
		StartTime = d.StartTime, 
		StopTime = d.StopTime,
		DistanceInKm = d.DistanceInKm,
		EnergyConsumptionInkWh = d.EnergyConsumptionInkWh,
		AverageConsumptionInkWhPer100km = d.AverageConsumptionInkWhPer100km,
		AverageSpeedInKmPerHour = d.AverageSpeedInKmPerHour,
		BuisnessTravel = CAST(d.BuisnessTravel AS bit),
		DateOfCreation = d.DateOfCreation,
		DateLastModified = d.DateLastModified
	FROM 
		dbo.DrivingJournal AS d
	WHERE
		d.DrivingJournalId = @DrivingJournalId
END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_GetByRegNo]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_GetByRegNo]
	@RegistrationNumber nvarchar (50)
AS
BEGIN

	SELECT 
		DrivingJournalId = d.DrivingjournalId, 
		RegistrationNumber = d.RegistrationNumber, 
		StartTime = d.StartTime, 
		StopTime = d.StopTime,
		DistanceInKm = d.DistanceInKm,
		EnergyConsumptionInkWh = d.EnergyConsumptionInkWh,
		AverageConsumptionInkWhPer100km = d.AverageConsumptionInkWhPer100km,
		AverageSpeedInKmPerHour = d.AverageSpeedInKmPerHour,
		BuisnessTravel = CAST(d.BuisnessTravel AS bit),
		DateOfCreation = d.DateOfCreation,
		DateLastModified = d.DateLastModified
	FROM 
		dbo.DrivingJournal AS d
	WHERE
		d.RegistrationNumber = @RegistrationNumber
	ORDER BY
		d.StartTime DESC;

END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_GetByRegNoAndBetweenDates]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_GetByRegNoAndBetweenDates]
	@RegistrationNumber nvarchar (50),
	@Start datetime,
	@End datetime
AS
BEGIN

	SELECT 
		DrivingJournalId = d.DrivingjournalId, 
		RegistrationNumber = d.RegistrationNumber, 
		StartTime = d.StartTime, 
		StopTime = d.StopTime,
		DistanceInKm = d.DistanceInKm,
		EnergyConsumptionInkWh = d.EnergyConsumptionInkWh,
		AverageConsumptionInkWhPer100km = d.AverageConsumptionInkWhPer100km,
		AverageSpeedInKmPerHour = d.AverageSpeedInKmPerHour,
		BuisnessTravel = CAST(d.BuisnessTravel AS bit),
		DateOfCreation = d.DateOfCreation,
		DateLastModified = d.DateLastModified
	FROM 
		dbo.DrivingJournal AS d
	WHERE
		d.RegistrationNumber = @RegistrationNumber
		AND d.StartTime >= @Start 
		AND d.StopTime <= @End

END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_GetByRegNoAndDate]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_GetByRegNoAndDate]
	@RegistrationNumber nvarchar (50),
	@SearchDate datetime
AS
BEGIN
	DECLARE @Year int;
	DECLARE @Month int;
	DECLARE @Day int;

	SET @Year = YEAR(@SearchDate);
	SET @Month = MONTH(@SearchDate);
	SET @Day = DAY(@SearchDate);

	SELECT 
		DrivingJournalId = d.DrivingjournalId, 
		RegistrationNumber = d.RegistrationNumber, 
		StartTime = d.StartTime, 
		StopTime = d.StopTime,
		DistanceInKm = d.DistanceInKm,
		EnergyConsumptionInkWh = d.EnergyConsumptionInkWh,
		AverageConsumptionInkWhPer100km = d.AverageConsumptionInkWhPer100km,
		AverageSpeedInKmPerHour = d.AverageSpeedInKmPerHour,
		BuisnessTravel = CAST(d.BuisnessTravel AS bit),
		DateOfCreation = d.DateOfCreation,
		DateLastModified = d.DateLastModified
	FROM 
		dbo.DrivingJournal AS d
	WHERE
		d.RegistrationNumber = @RegistrationNumber
		AND YEAR(d.StartTime) = @Year
		AND MONTH(d.StartTime) = @Month
		AND DAY(d.StartTime) = @Day

END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_PatchTravelTypeByGuid]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_PatchTravelTypeByGuid]
	@DrivingJournalId uniqueidentifier,
	@BuisnessTravel bit
AS
BEGIN
	UPDATE
		dbo.DrivingJournal
	SET
		BuisnessTravel = CAST(@BuisnessTravel AS int),
		DateLastModified = GETDATE()
	WHERE
		DrivingJournalId = @DrivingJournalId
END
GO
/****** Object:  StoredProcedure [dbo].[sDrivingJournal_Post]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sDrivingJournal_Post]
	@DrivingJournalId uniqueidentifier output,
	@RegistrationNumber nvarchar (50),
	@StartTime datetime,
	@StopTime datetime,
	@DistanceInKm float,
	@EnergyConsumptionInkWh float,
	@AverageConsumptionInkWhPer100km float,
	@AverageSpeedInKmPerHour float,
	@BuisnessTravel bit
AS
BEGIN
	SET @DrivingJournalId = NEWID();

		INSERT INTO 
			dbo.DrivingJournal
		VALUES
			(
				@DrivingJournalId,
				@RegistrationNumber,
				@StartTime,
				@StopTime,
				@DistanceInKm,
				@EnergyConsumptionInkWh,
				@AverageConsumptionInkWhPer100km,
				@AverageSpeedInKmPerHour,
				GETDATE(),
				CAST(@BuisnessTravel AS int),
				GETDATE()
			);
END
GO
/****** Object:  StoredProcedure [dbo].[sStatus_GetByRegNo]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sStatus_GetByRegNo]
	@RegistrationNumber nvarchar (50)
AS
BEGIN

	SELECT 
		StatusId = s.StatusId, 
		RegistrationNumber = s.RegistrationNumber, 
		BatteryStatus = s.BatteryStatus, 
		TripMeter = s.TripMeter,
		EngineRunning = s.EngineRunning,
		LockStatus = s.LockStatus,
		AlarmStatus = s.AlarmStatus,
		DateOfCreation = s.DateOfCreation,
		DateLastModified = s.DateLastModified,
		TirePressures = s.TirePressures,
		PositionLatitude = v.PositionLatitude,
		PositionLongitude = v.PositionLongitude,
		PositionRadius = v.PositionRadius
	FROM 
		dbo.Status AS s
		LEFT OUTER JOIN dbo.VehiclePosition AS v 
		ON s.RegistrationNumber = v.RegistrationNumber
	WHERE
		s.RegistrationNumber = @RegistrationNumber;

END
GO
/****** Object:  StoredProcedure [dbo].[sStatus_Post]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sStatus_Post]
	@StatusId uniqueidentifier output,
	@RegistrationNumber nvarchar (50),
	@BatteryStatus int,
	@TripMeter float,
	@EngineRunning int,
	@LockStatus int,
	@AlarmStatus int,
	@TirePressures nvarchar (50)
AS
BEGIN

	UPDATE 
		dbo.Status
	SET
		@StatusId = StatusId,
		BatteryStatus = @BatteryStatus,
		TripMeter =	@TripMeter,
		EngineRunning = @EngineRunning,
		LockStatus = @LockStatus,
		AlarmStatus = @AlarmStatus,
		DateLastModified = GETDATE(),
		TirePressures =	@TirePressures

	WHERE	
		RegistrationNumber = @RegistrationNumber;

	IF (@@ROWCOUNT = 0) BEGIN

	SET @StatusId = NEWID();

		INSERT INTO 
			dbo.Status
		VALUES
			(
				@StatusId,
				@RegistrationNumber,
				@BatteryStatus,
				@TripMeter,
				@EngineRunning,
				@LockStatus,
				@AlarmStatus,
				GETDATE(),
				GETDATE(),
				@TirePressures
			);
	END

END
GO
/****** Object:  StoredProcedure [dbo].[sVehiclePosition_GetDistance]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sVehiclePosition_GetDistance]
	@VehiclePositionId uniqueidentifier output,
	@RegistrationNumber nvarchar (50),
	@PositionLatitude float,
	@PositionLongitude float
AS
BEGIN
	DECLARE @Point1 geography;
	SET @Point1 = geography::Point(@PositionLatitude, @PositionLongitude, 4326)

	SET
		@VehiclePositionId = (SELECT v.VehiclePositionId
	FROM
		dbo.VehiclePosition AS v
		LEFT OUTER JOIN dbo.Status AS s
		ON s.RegistrationNumber = v.RegistrationNumber
	WHERE 
	v.RegistrationNumber = @RegistrationNumber 
	AND @Point1.STDistance(v.Point) <= v.PositionRadius
	AND s.EngineRunning = 0)
	
END
GO
/****** Object:  StoredProcedure [dbo].[sVehiclePosition_Post]    Script Date: 2021-12-17 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sVehiclePosition_Post]
	@VehiclePositionId uniqueidentifier,
	@RegistrationNumber nvarchar (50),
	@PositionLatitude float,
	@PositionLongitude float,
	@PositionRadius float
AS
BEGIN
	
	UPDATE 
		dbo.VehiclePosition
	SET
		PositionLatitude = @PositionLatitude,
		PositionLongitude = @PositionLongitude,
		Point = geography::Point(@PositionLatitude, @PositionLongitude, 4326),
		DateLastModified = GETDATE()

	WHERE	
		RegistrationNumber = @RegistrationNumber;

	IF (@@ROWCOUNT = 0) BEGIN

	DECLARE @Point geography;

	SET @Point = geography::Point(@PositionLatitude, @PositionLongitude, 4326);

		INSERT INTO 
			dbo.VehiclePosition
		VALUES
			(
				@VehiclePositionId,
				@RegistrationNumber,
				@PositionLatitude,
				@PositionLongitude,
				@Point,
				GETDATE(),
				GETDATE(),
				@PositionRadius
			);
	END

END
GO
