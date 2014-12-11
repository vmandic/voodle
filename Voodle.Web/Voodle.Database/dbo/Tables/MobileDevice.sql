CREATE TABLE [dbo].[MobileDevice] (
    [ID]                              INT            IDENTITY (1, 1) NOT NULL,
    [UserID]                          INT            NOT NULL,
    [SmartphonePlatform]              NVARCHAR (MAX) NULL,
    [PushNotificationsRegistrationID] NVARCHAR (MAX) NULL,
    [DeviceID]                        NVARCHAR (MAX) NULL,
    [Active]                          BIT            NOT NULL,
    [ModifiedAt]                      DATETIME       NULL,
    [CreatedAt]                       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

