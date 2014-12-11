CREATE TABLE [dbo].[User] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (100) NULL,
    [LastName]     NVARCHAR (100) NULL,
    [Username]     NVARCHAR (100) NOT NULL,
    [Password]     NVARCHAR (255) NULL,
    [Email]        NVARCHAR (255) NULL,
    [RoleID]       INT            NOT NULL,
    [Active]       BIT            NULL,
    [LastLoggedAt] DATETIME       NULL,
    [CreatedAt]    DATETIME       NULL,
    [ModifiedAt]   DATETIME       NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID])
);



