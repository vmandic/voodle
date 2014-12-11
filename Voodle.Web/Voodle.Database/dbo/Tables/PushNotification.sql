CREATE TABLE [dbo].[PushNotification] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [MobileDeviceID] INT            NOT NULL,
    [Message]        NVARCHAR (MAX) NULL,
    [Status]         INT            NOT NULL,
    [CreatedAt]      DATETIME       NOT NULL,
    [ModifiedAt]     DATETIME       NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.PushNotification] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbo.PushNotification_dbo.MobileDevice_MobileDeviceID] FOREIGN KEY ([MobileDeviceID]) REFERENCES [dbo].[MobileDevice] ([ID])
);

