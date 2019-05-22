CREATE TABLE [dbo].[users] (
    [Id]               NVARCHAR (50)  NOT NULL,
    [email]            NVARCHAR (150) NOT NULL,
    [password]         TEXT           NOT NULL,
    [salt]             NVARCHAR (50)  NOT NULL,
    [logincode]        NVARCHAR (50)  NULL,
    [Name]             NVARCHAR (100) NULL,
    [location]         NVARCHAR (50)  NULL,
    [phone]            NVARCHAR (50)  NULL,
    [IsActivated]      BIT            NULL,
    [isAdmin]          BIT            NULL,
    [IsVerified]       BIT            NULL,
    [VerificationCode] NVARCHAR (50)  NULL,
    [passResetCode]    NVARCHAR (50)  NULL,
    CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_users]
    ON [dbo].[users]([email] ASC);

