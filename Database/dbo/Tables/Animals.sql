CREATE TABLE [dbo].[Animals] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (25) NOT NULL,
    [CommonName] NVARCHAR (25) NOT NULL,
    CONSTRAINT [PK_Animals] PRIMARY KEY CLUSTERED ([Id] ASC)
);

