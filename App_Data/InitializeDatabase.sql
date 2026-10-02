
-- Sublytic Database Initialization Script
-- SQL Server

IF DB_ID('SublyticDB') IS NULL
BEGIN
    CREATE DATABASE [SublyticDB];
END
GO

USE [SublyticDB];
GO

IF OBJECT_ID(N'[dbo].[Subscription]', N'U') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT IF EXISTS [FK_Subscription_Category];
    DROP TABLE [dbo].[Subscription];
END
GO

IF OBJECT_ID(N'[dbo].[Category]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Category];
END
GO

CREATE TABLE [dbo].[Category] (
    [CategoryId] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [ColorHex] NVARCHAR(7) NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_Category_IsActive] DEFAULT 1,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);
GO

CREATE TABLE [dbo].[Subscription] (
    [SubscriptionId] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [BillingCycle] INT NOT NULL,
    [StartDate] DATETIME2 NOT NULL,
    [NextRenewalDate] DATETIME2 NOT NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_Subscription_IsActive] DEFAULT 1,
    [AutoRenew] BIT NOT NULL CONSTRAINT [DF_Subscription_AutoRenew] DEFAULT 1,
    [CategoryId] INT NOT NULL,
    [WebsiteUrl] NVARCHAR(255) NULL,
    [AccountEmail] NVARCHAR(50) NULL,
    [Notes] NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_Subscription_CreatedAt] DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED ([SubscriptionId] ASC),
    CONSTRAINT [FK_Subscription_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId]) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX [IX_Subscription_CategoryId] ON [dbo].[Subscription]([CategoryId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Subscription_IsActive] ON [dbo].[Subscription]([IsActive] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Subscription_NextRenewalDate] ON [dbo].[Subscription]([NextRenewalDate] ASC);
GO

INSERT INTO [dbo].[Category] ([Name], [Description], [ColorHex], [IsActive]) VALUES
(N'Entertainment', N'Streaming services, music, movies', N'#667eea', 1),
(N'Productivity', N'Office tools, cloud storage, development', N'#f093fb', 1),
(N'Utilities', N'Cloud services, hosting, domain', N'#4facfe', 1),
(N'Fitness & Health', N'Gym memberships, health apps', N'#43e97b', 1),
(N'Education', N'Learning platforms, courses', N'#fa709a', 1),
(N'Lifestyle', N'Food delivery, beauty, subscription boxes', N'#ffd700', 1),
(N'News & Media', N'Newspapers, magazines, news sites', N'#00f2fe', 1),
(N'Software', N'Licensed software, SaaS tools', N'#a8edea', 1);
GO

SET IDENTITY_INSERT [dbo].[Subscription] ON;
GO

INSERT INTO [dbo].[Subscription] ([SubscriptionId], [Name], [Description], [Price], [BillingCycle], [StartDate], [NextRenewalDate], [IsActive], [AutoRenew], [CategoryId], [WebsiteUrl], [AccountEmail], [Notes], [CreatedAt], [UpdatedAt]) VALUES
(1, N'Netflix Premium', N'4K streaming subscription', 15.99, 1, DATEADD(YEAR, -2, DATEADD(DAY, 15, '2024-01-01')), DATEADD(DAY, 5, GETDATE()), 1, 1, 1, N'https://www.netflix.com', N'user@example.com', N'Family plan shared with 2 others', DATEADD(YEAR, -2, GETDATE()), DATEADD(MONTH, -1, GETDATE())),
(2, N'Spotify Premium', N'Ad-free music streaming', 9.99, 1, DATEADD(YEAR, -2, DATEADD(DAY, 20, '2024-03-01')), DATEADD(DAY, 12, GETDATE()), 1, 1, 1, N'https://www.spotify.com', N'user@example.com', N'Individual plan', DATEADD(YEAR, -2, DATEADD(MONTH, 2, GETDATE())), DATEADD(MONTH, -1, GETDATE())),
(3, N'Microsoft 365 Family', N'Office apps for up to 6 people', 99.99, 12, DATEADD(YEAR, -3, DATEADD(MONTH, -1, '2024-11-01')), DATEADD(DAY, 30, GETDATE()), 1, 1, 2, N'https://www.microsoft.com', N'user@example.com', N'Includes 1TB OneDrive per person', DATEADD(YEAR, -3, DATEADD(MONTH, -1, GETDATE())), DATEADD(MONTH, -2, GETDATE())),
(4, N'Adobe Creative Cloud', N'All apps creative suite', 54.99, 1, DATEADD(YEAR, -1, DATEADD(MONTH, 5, '2024-05-10')), DATEADD(DAY, 8, GETDATE()), 1, 1, 2, N'https://www.adobe.com', N'user@example.com', N'20+ creative apps', DATEADD(YEAR, -1, DATEADD(MONTH, 5, GETDATE())), DATEADD(MONTH, -1, GETDATE())),
(5, N'AWS Free Tier (Upgrade)', N'Cloud hosting services', 29.99, 1, DATEADD(YEAR, -1, DATEADD(MONTH, 8, '2024-02-01')), DATEADD(DAY, 3, GETDATE()), 1, 1, 3, N'https://aws.amazon.com', N'admin@example.com', N'EC2 + S3 usage', DATEADD(YEAR, -1, DATEADD(MONTH, 8, GETDATE())), DATEADD(DAY, -5, GETDATE())),
(6, N'Google Workspace', N'Business email and productivity', 72.00, 12, DATEADD(YEAR, -1, DATEADD(MONTH, 2, '2024-08-01')), DATEADD(DAY, 45, GETDATE()), 1, 1, 3, N'https://workspace.google.com', N'admin@example.com', N'3 users at $6/month each annual plan', DATEADD(YEAR, -1, DATEADD(MONTH, 2, GETDATE())), DATEADD(MONTH, -3, GETDATE())),
(7, N'Peloton Digital', N'Fitness classes app', 12.99, 1, DATEADD(MONTH, 9, '2024-01-05'), DATEADD(DAY, 15, GETDATE()), 1, 0, 4, N'https://www.peloton.com', N'user@example.com', N'Considering cancellation', DATEADD(MONTH, 9, GETDATE()), DATEADD(DAY, -10, GETDATE())),
(8, N'Skillshare Premium', N'Online learning platform', 168.00, 12, DATEADD(MONTH, 7, '2024-03-15'), DATEADD(DAY, 60, GETDATE()), 1, 1, 5, N'https://www.skillshare.com', N'user@example.com', N'Unlimited classes', DATEADD(MONTH, 7, GETDATE()), NULL),
(9, N'HelloFresh', N'Meal kit delivery', 59.99, 0, DATEADD(MONTH, 4, '2024-06-01'), DATEADD(DAY, 2, GETDATE()), 1, 1, 6, N'https://www.hellofresh.com', N'user@example.com', N'3 meals for 2 people', DATEADD(MONTH, 4, GETDATE()), DATEADD(DAY, -2, GETDATE())),
(10, N'The New York Times', N'Digital news subscription', 17.00, 1, DATEADD(YEAR, -1, DATEADD(MONTH, 1, '2024-09-01')), DATEADD(DAY, 22, GETDATE()), 1, 1, 7, N'https://www.nytimes.com', N'user@example.com', N'All digital access', DATEADD(YEAR, -1, DATEADD(MONTH, 1, GETDATE())), DATEADD(MONTH, -2, GETDATE())),
(11, N'Visual Studio Enterprise', N'Professional IDE license', 250.00, 1, DATEADD(YEAR, -1, '2024-10-01'), DATEADD(DAY, 1, GETDATE()), 1, 1, 8, N'https://visualstudio.microsoft.com', N'admin@example.com', N'Work expense reimbursed', DATEADD(YEAR, -1, GETDATE()), DATEADD(MONTH, -1, GETDATE())),
(12, N'Disney+', N'Family streaming', 7.99, 1, DATEADD(MONTH, 10, '2024-12-25'), DATEADD(DAY, 10, GETDATE()), 1, 1, 1, N'https://www.disneyplus.com', N'user@example.com', N'With Hulu bundle option', DATEADD(MONTH, 10, GETDATE()), NULL),
(13, N'Dropbox Plus', N'Cloud storage 2TB', 11.99, 1, DATEADD(YEAR, -2, DATEADD(MONTH, 9, '2024-07-10')), DATEADD(DAY, 18, GETDATE()), 0, 0, 2, N'https://www.dropbox.com', N'user@example.com', N'Switched to OneDrive', DATEADD(YEAR, -2, DATEADD(MONTH, 9, GETDATE())), DATEADD(MONTH, -4, GETDATE())),
(14, N'HBO Max', N'Premium streaming', 14.99, 1, DATEADD(YEAR, -1, DATEADD(MONTH, 6, '2024-04-01')), DATEADD(DAY, 25, GETDATE()), 1, 1, 1, N'https://www.hbomax.com', N'user@example.com', N'Ad-free tier', DATEADD(YEAR, -1, DATEADD(MONTH, 6, GETDATE())), DATEADD(MONTH, -1, GETDATE()));
GO

SET IDENTITY_INSERT [dbo].[Subscription] OFF;
GO

PRINT N'Sublytic database initialized successfully!';
PRINT N'Categories inserted: 8';
PRINT N'Subscriptions inserted: 14';
GO
