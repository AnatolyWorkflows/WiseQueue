SET NOCOUNT ON
DECLARE @TARGET_SCHEMA_VERSION INT;
DECLARE @TARGET_SCHEMA_DESCRIPTION NVARCHAR(400);

SET @TARGET_SCHEMA_VERSION = 1;
SET @TARGET_SCHEMA_DESCRIPTION = 'Will contains information about installation';

PRINT 'Installing WiseQueue SQL objects...';

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

BEGIN TRANSACTION

-- Create the database schema if it doesn't exists
IF NOT EXISTS (SELECT [schema_id] FROM [sys].[schemas] WHERE [name] = '#{WiseQueueSchema}')
BEGIN
    EXEC (N'CREATE SCHEMA [#{WiseQueueSchema}]');
    PRINT 'Created database schema [#{WiseQueueSchema}]';
END
ELSE
    PRINT 'Database schema [#{WiseQueueSchema}] already exists';
    
DECLARE @SCHEMA_ID int;
SELECT @SCHEMA_ID = [schema_id] FROM [sys].[schemas] WHERE [name] = '#{WiseQueueSchema}';


-- Create the [#{WiseQueueSchema}].Schema table if not exists
IF NOT EXISTS(SELECT [object_id] FROM [sys].[tables] 
    WHERE [name] = 'Schema' AND [schema_id] = @SCHEMA_ID)
BEGIN
    CREATE TABLE [#{WiseQueueSchema}].[Schema](
        [Version] [int] NOT NULL,
        [Description] [nvarchar](1000) NOT NULL,
        CONSTRAINT [PK_WiseQueue_Schema] PRIMARY KEY CLUSTERED ([Version] ASC)
    );
    PRINT 'Created table [#{WiseQueueSchema}].[Schema]';
END
ELSE
    PRINT 'Table [#{WiseQueueSchema}].[Schema] already exists';
    
DECLARE @CURRENT_SCHEMA_VERSION int;
SELECT @CURRENT_SCHEMA_VERSION = [Version] FROM [#{WiseQueueSchema}].[Schema];

PRINT 'Current WiseQueue schema version: ' + CASE @CURRENT_SCHEMA_VERSION WHEN NULL THEN 'none' ELSE CONVERT(nvarchar, @CURRENT_SCHEMA_VERSION) END;

IF @CURRENT_SCHEMA_VERSION IS NOT NULL AND @CURRENT_SCHEMA_VERSION > @TARGET_SCHEMA_VERSION
BEGIN
    ROLLBACK TRANSACTION;
    RAISERROR(N'WiseQueue current database schema version %d is newer than the configured SqlServerStorage schema version %d.', 11, 1,
        @CURRENT_SCHEMA_VERSION, @TARGET_SCHEMA_VERSION);
END

-- Create [#{WiseQueueSchema}] tables
IF @CURRENT_SCHEMA_VERSION IS NULL
BEGIN
    PRINT 'Installing schema version 1';

	-- Creating Queues table    
    CREATE TABLE [#{WiseQueueSchema}].[Queues](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](4000) NULL,
		CONSTRAINT [PK_Queue] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	-- Create servers table...
	CREATE TABLE [#{WiseQueueSchema}].[Servers](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](200) NOT NULL,
		[Description] [nvarchar](2000) NOT NULL,
		[ExpiredAt] [datetime] NOT NULL,
		CONSTRAINT [PK_Servers] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	-- Create servers and queues journal
	CREATE TABLE [#{WiseQueueSchema}].[ServerQueues](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[ServerId] [bigint] NOT NULL,
		[QeueuId] [bigint] NOT NULL,
		CONSTRAINT [PK_ServerQueues] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]	
	
	-- Create tasks table
	CREATE TABLE [#{WiseQueueSchema}].[Tasks](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[QueueId] [bigint] NOT NULL,
		[ServerId] [bigint] NULL,
		[State] [smallint] NOT NULL,
		[CompletedAt] [datetime] NULL,
		[ExecuteAt] [datetime] DEFAULT GETUTCDATE() NOT NULL,
		[RepeatCrashCount] [int] DEFAULT (3) NOT NULL,
		[InstanceType] [nvarchar](4000) NOT NULL,
		[Method] [nvarchar](4000) NOT NULL,
		[ParametersTypes] [nvarchar](4000) NOT NULL,
		[Arguments] [nvarchar](4000) NOT NULL,
		CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]	
	
	-- Different constrains
	/****** Object:  Default [DF_Tasks_ServerId]    Script Date: 06/06/2016 21:01:10 ******/
	ALTER TABLE [#{WiseQueueSchema}].[Tasks] ADD  CONSTRAINT [DF_Tasks_ServerId]  DEFAULT NULL FOR [ServerId]
	/****** Object:  ForeignKey [FK_ServerQueues_Queues]    Script Date: 06/06/2016 21:01:10 ******/
	ALTER TABLE [#{WiseQueueSchema}].[ServerQueues]  WITH CHECK ADD  CONSTRAINT [FK_ServerQueues_Queues] FOREIGN KEY([QeueuId])
		REFERENCES [#{WiseQueueSchema}].[Queues] ([Id])
	ALTER TABLE [#{WiseQueueSchema}].[ServerQueues] CHECK CONSTRAINT [FK_ServerQueues_Queues]

	/****** Object:  ForeignKey [FK_ServerQueues_Servers]    Script Date: 06/06/2016 21:01:10 ******/
	ALTER TABLE [#{WiseQueueSchema}].[ServerQueues]  WITH CHECK ADD  CONSTRAINT [FK_ServerQueues_Servers] FOREIGN KEY([ServerId])
		REFERENCES [#{WiseQueueSchema}].[Servers] ([Id])
	ALTER TABLE [#{WiseQueueSchema}].[ServerQueues] CHECK CONSTRAINT [FK_ServerQueues_Servers]
	
	/****** Object:  ForeignKey [FK_Tasks_Queues]    Script Date: 06/06/2016 21:01:10 ******/
	ALTER TABLE [#{WiseQueueSchema}].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Queues] FOREIGN KEY([QueueId])
		REFERENCES [#{WiseQueueSchema}].[Queues] ([Id])
	ALTER TABLE [#{WiseQueueSchema}].[Tasks] CHECK CONSTRAINT [FK_Tasks_Queues]

	/****** Object:  ForeignKey [FK_Tasks_Servers1]    Script Date: 06/06/2016 21:01:10 ******/
	ALTER TABLE [#{WiseQueueSchema}].[Tasks]  WITH NOCHECK ADD  CONSTRAINT [FK_Tasks_Servers1] FOREIGN KEY([ServerId])
		REFERENCES [#{WiseQueueSchema}].[Servers] ([Id])
	ALTER TABLE [#{WiseQueueSchema}].[Tasks] NOCHECK CONSTRAINT [FK_Tasks_Servers1]	

	/****** Object:  StoredProcedure [WiseQueues].[InsertTask]    Script Date: 7/11/2016 10:43:13 PM ******/
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	PRINT 'Creating stored procedures...'

	IF OBJECT_ID ( '[#{WiseQueueSchema}].InsertTask', 'P' ) IS NOT NULL   
		DROP PROCEDURE [#{WiseQueueSchema}].InsertTask;  


	EXEC sp_executesql N' CREATE PROCEDURE [#{WiseQueueSchema}].InsertTask 
		@QueueId bigint,
		@State smallint,  
		@InstanceType nvarchar(4000),
		@Method nvarchar(4000),
		@ParametersTypes nvarchar(4000),
		@Arguments nvarchar(4000)
	AS
	BEGIN
		-- SET TRANSACTION ISOLATION LEVEL READ COMMITTED  -- This level will be set in the initialization script.	

		-- Insert statements for procedure here
		INSERT INTO WiseQueues.Tasks 
			([QueueId], [State], [InstanceType], [Method], [ParametersTypes], [Arguments]) 
		VALUES 
			(@QueueId,  @State,  @InstanceType,  @Method,  @ParametersTypes,  @Arguments); 

		SELECT CAST(scope_identity() AS bigint);
	END'

	PRINT 'All stored procedures have been created.'

	SET @CURRENT_SCHEMA_VERSION = 1;   
	SET @TARGET_SCHEMA_DESCRIPTION = 'This is a first version of tables.'; 
END


UPDATE [#{WiseQueueSchema}].[Schema] SET [Version] = @CURRENT_SCHEMA_VERSION
IF @@ROWCOUNT = 0 
	INSERT INTO [#{WiseQueueSchema}].[Schema] ([Version], [Description]) VALUES (@CURRENT_SCHEMA_VERSION, @TARGET_SCHEMA_DESCRIPTION)        

PRINT 'WiseQueue database schema has been installed';

PRINT 'Inserting the default queue...';

IF NOT EXISTS (SELECT * FROM [#{WiseQueueSchema}].[Queues] WHERE [Name] = 'default')
	BEGIN
		INSERT INTO [#{WiseQueueSchema}].[Queues] ([Name], [Description])
		VALUES ('default', 'This is a default queue')
	END 

PRINT 'The default queue has been inserted';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

PRINT 'WiseQueue SQL objects have been installed';