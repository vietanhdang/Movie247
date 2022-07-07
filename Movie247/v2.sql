IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Roles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [Firstname] nvarchar(100) NULL,
    [LastName] nvarchar(100) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [RoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);
GO

CREATE INDEX [IX_UserLogins_UserId] ON [UserLogins] ([UserId]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220706175427_First-Create', N'5.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Genre] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_Genre] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Keyword] (
    [Id] int NOT NULL IDENTITY,
    [KeywordName] nvarchar(max) NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_Keyword] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Movie] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Overview] nvarchar(max) NULL,
    [ReleaseDate] datetime2 NOT NULL,
    [PosterPath] nvarchar(max) NULL,
    [BackdropPath] nvarchar(max) NULL,
    [ImdbAverage] float NULL,
    [Views] bigint NULL,
    [Duration] nvarchar(max) NULL,
    [Trailer] nvarchar(max) NULL,
    [MovieStatus] bit NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_Movie] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Person] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [ProfilePath] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    [Popularity] float NULL,
    [Birthday] datetime2 NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProductionCompany] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [FilePath] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_ProductionCompany] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProductionCountry] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_ProductionCountry] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [FavouriteMovies] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NULL,
    [MovieId] int NOT NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_FavouriteMovies] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FavouriteMovies_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FavouriteMovies_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [MovieGenre] (
    [Id] int NOT NULL IDENTITY,
    [GenreId] int NOT NULL,
    [MovieId] int NOT NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_MovieGenre] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovieGenre_Genre_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genre] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MovieGenre_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MovieKeyword] (
    [Id] int NOT NULL IDENTITY,
    [MovieId] int NOT NULL,
    [KeywordId] int NOT NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_MovieKeyword] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovieKeyword_Keyword_KeywordId] FOREIGN KEY ([KeywordId]) REFERENCES [Keyword] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MovieKeyword_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MovieSource] (
    [Id] int NOT NULL IDENTITY,
    [MovieId] int NOT NULL,
    [LinkSource] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_MovieSource] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovieSource_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MovieCast] (
    [Id] int NOT NULL IDENTITY,
    [PersonId] int NOT NULL,
    [MovieId] int NOT NULL,
    [Character] nvarchar(max) NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_MovieCast] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovieCast_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MovieCast_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MovieCrew] (
    [Id] int NOT NULL IDENTITY,
    [PersonId] int NOT NULL,
    [MovieId] int NOT NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_MovieCrew] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovieCrew_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MovieCrew_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MovieCompany] (
    [Id] int NOT NULL IDENTITY,
    [CompanyId] int NOT NULL,
    [MovieId] int NOT NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_MovieCompany] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovieCompany_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MovieCompany_ProductionCompany_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [ProductionCompany] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MovieCountry] (
    [Id] int NOT NULL IDENTITY,
    [CountryId] nvarchar(450) NULL,
    [MovieId] int NOT NULL,
    [CreateAt] datetime2 NULL,
    [UpdateAt] datetime2 NULL,
    CONSTRAINT [PK_MovieCountry] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovieCountry_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MovieCountry_ProductionCountry_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [ProductionCountry] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_FavouriteMovies_MovieId] ON [FavouriteMovies] ([MovieId]);
GO

CREATE INDEX [IX_FavouriteMovies_UserId] ON [FavouriteMovies] ([UserId]);
GO

CREATE INDEX [IX_MovieCast_MovieId] ON [MovieCast] ([MovieId]);
GO

CREATE INDEX [IX_MovieCast_PersonId] ON [MovieCast] ([PersonId]);
GO

CREATE INDEX [IX_MovieCompany_CompanyId] ON [MovieCompany] ([CompanyId]);
GO

CREATE INDEX [IX_MovieCompany_MovieId] ON [MovieCompany] ([MovieId]);
GO

CREATE INDEX [IX_MovieCountry_CountryId] ON [MovieCountry] ([CountryId]);
GO

CREATE INDEX [IX_MovieCountry_MovieId] ON [MovieCountry] ([MovieId]);
GO

CREATE INDEX [IX_MovieCrew_MovieId] ON [MovieCrew] ([MovieId]);
GO

CREATE INDEX [IX_MovieCrew_PersonId] ON [MovieCrew] ([PersonId]);
GO

CREATE INDEX [IX_MovieGenre_GenreId] ON [MovieGenre] ([GenreId]);
GO

CREATE INDEX [IX_MovieGenre_MovieId] ON [MovieGenre] ([MovieId]);
GO

CREATE INDEX [IX_MovieKeyword_KeywordId] ON [MovieKeyword] ([KeywordId]);
GO

CREATE INDEX [IX_MovieKeyword_MovieId] ON [MovieKeyword] ([MovieId]);
GO

CREATE INDEX [IX_MovieSource_MovieId] ON [MovieSource] ([MovieId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220707160530_Second-Create', N'5.0.17');
GO

COMMIT;
GO

