--Categories
CREATE TABLE [dbo].[Categories] (
    [CategoryID]   INT           NOT NULL,
    [CategoryCode] VARCHAR (20)  NOT NULL,
    [Name]         VARCHAR (255) NOT NULL,
    [Status]       CHAR (1)      NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    [CreateUser]   INT           NOT NULL,
    [AmendDate]    DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);

--Products
CREATE TABLE [dbo].[Products] (
    [ProductID]   INT             NOT NULL,
    [ProductCode] VARCHAR (20)    NOT NULL,
    [ProductName] VARCHAR (255)   NOT NULL,
    [Description] VARCHAR (500)   NULL,
    [CategoryID]  INT             NOT NULL,
    [Price]       DECIMAL (19, 4) NOT NULL,
    [Image]       VARCHAR (MAX)   NULL,
    [CreateDate]  DATETIME        NOT NULL,
    [CreateUser]  INT             NOT NULL,
    [AmendDate]   DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC)
);

--SyncQueue
CREATE TABLE [dbo].[SyncQueue] (
    [QueueID]    INT           IDENTITY (1, 1) NOT NULL,
    [UserName]   VARCHAR (20)  NOT NULL,
    [SyncURL]    VARCHAR (255) NOT NULL,
    [SyncData]   VARCHAR (255) NOT NULL,
    [Status]     CHAR (1)      NOT NULL,
    [CreateDate] DATETIME      NOT NULL,
    [CreateUser] INT           NOT NULL,
    [AmendDate]  DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([QueueID] ASC)
);

--Users
CREATE TABLE [dbo].[Users] (
    [UserID]     INT           NOT NULL,
    [UserName]   VARCHAR (255) NOT NULL,
    [Password]   VARCHAR (255) NOT NULL,
    [Status]     CHAR (1)      NULL,
    [CreateDate] DATETIME      NOT NULL,
    [CreateUser] INT           NOT NULL,
    [AmendDate]  DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);
