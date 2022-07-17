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

CREATE TABLE [genre] (
    [ID] int NOT NULL IDENTITY,
    [name] nvarchar(50) NOT NULL,
    [description] ntext NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_genre] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [keyword] (
    [ID] int NOT NULL IDENTITY,
    [keyword_name] nvarchar(100) NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_keyword] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [movie] (
    [ID] int NOT NULL IDENTITY,
    [title] nvarchar(200) NOT NULL,
    [overview] nvarchar(4000) NULL,
    [release_date] date NOT NULL,
    [poster_path] varchar(300) NULL,
    [backdrop_path] varchar(300) NULL,
    [imdb_average] float NULL,
    [views] bigint NULL,
    [duration] varchar(20) NULL,
    [trailer] varchar(1000) NOT NULL,
    [movie_status] bit NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [person] (
    [ID] int NOT NULL IDENTITY,
    [name] nvarchar(50) NOT NULL,
    [profile_path] varchar(300) NULL,
    [description] nvarchar(4000) NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    [popularity] float NULL,
    [birthday] date NULL,
    CONSTRAINT [PK_person] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [production_company] (
    [ID] int NOT NULL IDENTITY,
    [name] nvarchar(50) NOT NULL,
    [file_path] varchar(300) NULL,
    [description] ntext NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_production_company] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [production_country] (
    [ID] varchar(5) NOT NULL,
    [name] nvarchar(50) NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_production_country] PRIMARY KEY ([ID])
);
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

CREATE TABLE [movie_genre] (
    [ID] int NOT NULL IDENTITY,
    [genreID] int NOT NULL,
    [movieID] int NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_genre] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_genre_genre_ID] FOREIGN KEY ([genreID]) REFERENCES [genre] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_genre_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_keywords] (
    [ID] int NOT NULL IDENTITY,
    [movie_id] int NOT NULL,
    [keyword_id] int NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_keywords] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_keyword_id_keyword_ID] FOREIGN KEY ([keyword_id]) REFERENCES [keyword] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_keywords_movie_ID] FOREIGN KEY ([movie_id]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_source] (
    [ID] int NOT NULL IDENTITY,
    [movie_id] int NOT NULL,
    [link_source] varchar(1000) NOT NULL,
    [description] nvarchar(200) NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    [type] varchar(10) NULL,
    CONSTRAINT [PK_movie_source] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_source_movie_ID] FOREIGN KEY ([movie_id]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_cast] (
    [ID] int NOT NULL IDENTITY,
    [personID] int NOT NULL,
    [movieID] int NOT NULL,
    [character] nvarchar(100) NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_cast] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_cast_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_cast_person_ID] FOREIGN KEY ([personID]) REFERENCES [person] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_crew] (
    [ID] int NOT NULL IDENTITY,
    [personID] int NOT NULL,
    [movieID] int NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_crew] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_crew_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_crew_person_ID] FOREIGN KEY ([personID]) REFERENCES [person] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_company] (
    [ID] int NOT NULL IDENTITY,
    [companyID] int NOT NULL,
    [movieID] int NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_company] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_company_company_ID] FOREIGN KEY ([companyID]) REFERENCES [production_company] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_company_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_country] (
    [ID] int NOT NULL IDENTITY,
    [countryID] varchar(5) NOT NULL,
    [movieID] int NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_country] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_country_country_ID] FOREIGN KEY ([countryID]) REFERENCES [production_country] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_country_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION
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

CREATE TABLE [movie_favourite] (
    [ID] int NOT NULL IDENTITY,
    [movieID] int NOT NULL,
    [userID] nvarchar(450) NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_favourite] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_favourite_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_favourite_user_ID] FOREIGN KEY ([userID]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_review] (
    [ID] int NOT NULL IDENTITY,
    [movieID] int NOT NULL,
    [userID] nvarchar(450) NOT NULL,
    [rating] smallint NULL,
    [comment] nvarchar(300) NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_movie_review] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_review_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_review_user_ID] FOREIGN KEY ([userID]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
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

CREATE INDEX [IX_movie_cast_movieID] ON [movie_cast] ([movieID]);
GO

CREATE INDEX [IX_movie_cast_personID] ON [movie_cast] ([personID]);
GO

CREATE INDEX [IX_movie_company_companyID] ON [movie_company] ([companyID]);
GO

CREATE INDEX [IX_movie_company_movieID] ON [movie_company] ([movieID]);
GO

CREATE INDEX [IX_movie_country_countryID] ON [movie_country] ([countryID]);
GO

CREATE INDEX [IX_movie_country_movieID] ON [movie_country] ([movieID]);
GO

CREATE INDEX [IX_movie_crew_movieID] ON [movie_crew] ([movieID]);
GO

CREATE INDEX [IX_movie_crew_personID] ON [movie_crew] ([personID]);
GO

CREATE INDEX [IX_movie_favourite_movieID] ON [movie_favourite] ([movieID]);
GO

CREATE INDEX [IX_movie_favourite_userID] ON [movie_favourite] ([userID]);
GO

CREATE INDEX [IX_movie_genre_genreID] ON [movie_genre] ([genreID]);
GO

CREATE INDEX [IX_movie_genre_movieID] ON [movie_genre] ([movieID]);
GO

CREATE INDEX [IX_movie_keywords_keyword_id] ON [movie_keywords] ([keyword_id]);
GO

CREATE INDEX [IX_movie_keywords_movie_id] ON [movie_keywords] ([movie_id]);
GO

CREATE INDEX [IX_movie_review_movieID] ON [movie_review] ([movieID]);
GO

CREATE INDEX [IX_movie_review_userID] ON [movie_review] ([userID]);
GO

CREATE INDEX [IX_movie_source_movie_id] ON [movie_source] ([movie_id]);
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
VALUES (N'20220710070516_Movie247_V1', N'5.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Users] ADD [Image] nvarchar(100) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220710094855_Movie247_V2_UserImage', N'5.0.17');
GO

COMMIT;
GO

