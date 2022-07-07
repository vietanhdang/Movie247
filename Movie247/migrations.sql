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
    [overview] ntext NULL,
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
    [description] ntext NULL,
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

CREATE TABLE [role] (
    [ID] int NOT NULL IDENTITY,
    [name] nvarchar(50) NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_role] PRIMARY KEY ([ID])
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

CREATE TABLE [user] (
    [ID] int NOT NULL IDENTITY,
    [userName] varchar(50) NOT NULL,
    [surName] nvarchar(50) NULL,
    [middleName] nvarchar(50) NULL,
    [name] nvarchar(50) NULL,
    [email] nvarchar(100) NOT NULL,
    [phoneNumber] char(11) NULL,
    [password] varchar(100) NOT NULL,
    [salt] varchar(100) NULL,
    [description] ntext NULL,
    [activated] bit NOT NULL,
    [roleId] int NOT NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    CONSTRAINT [PK_user] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_user_role_ID] FOREIGN KEY ([roleId]) REFERENCES [role] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [movie_review] (
    [ID] int NOT NULL IDENTITY,
    [movieID] int NOT NULL,
    [userID] int NOT NULL,
    [review] ntext NULL,
    [createAt] datetime NULL DEFAULT ((getdate())),
    [updateAt] datetime NULL,
    [vote] smallint NULL,
    CONSTRAINT [PK_movie_review] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_movie_review_movie_ID] FOREIGN KEY ([movieID]) REFERENCES [movie] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_movie_review_user_ID] FOREIGN KEY ([userID]) REFERENCES [user] ([ID]) ON DELETE NO ACTION
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

CREATE INDEX [IX_user_roleId] ON [user] ([roleId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220705133108_Init', N'5.0.17');
GO

COMMIT;
GO

