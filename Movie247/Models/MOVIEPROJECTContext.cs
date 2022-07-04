﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace Movie247.Models
{
    public partial class MOVIEPROJECTContext : DbContext
    {
        public MOVIEPROJECTContext()
        {
        }

        public MOVIEPROJECTContext(DbContextOptions<MOVIEPROJECTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieCast> MovieCasts { get; set; }
        public virtual DbSet<MovieCompany> MovieCompanies { get; set; }
        public virtual DbSet<MovieCountry> MovieCountries { get; set; }
        public virtual DbSet<MovieCrew> MovieCrews { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<MovieKeyword> MovieKeywords { get; set; }
        public virtual DbSet<MovieReview> MovieReviews { get; set; }
        public virtual DbSet<MovieSource> MovieSources { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public virtual DbSet<ProductionCountry> ProductionCountries { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build().GetConnectionString("AppDbContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");
            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("keyword");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.KeywordName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("keyword_name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movie");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BackdropPath)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("backdrop_path");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Duration)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("duration");

                entity.Property(e => e.ImdbAverage).HasColumnName("imdb_average");

                entity.Property(e => e.MovieStatus).HasColumnName("movie_status");

                entity.Property(e => e.Overview)
                    .HasColumnType("ntext")
                    .HasColumnName("overview");

                entity.Property(e => e.PosterPath)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("poster_path");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("date")
                    .HasColumnName("release_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.Trailer)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("trailer");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.Property(e => e.Views).HasColumnName("views");
            });

            modelBuilder.Entity<MovieCast>(entity =>
            {
                entity.ToTable("movie_cast");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Character)
                    .HasMaxLength(100)
                    .HasColumnName("character");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.PersonId).HasColumnName("personID");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieCasts)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_cast_movie_ID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.MovieCasts)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_cast_person_ID");
            });

            modelBuilder.Entity<MovieCompany>(entity =>
            {
                entity.ToTable("movie_company");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("companyID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.MovieCompanies)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_company_company_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieCompanies)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_company_movie_ID");
            });

            modelBuilder.Entity<MovieCountry>(entity =>
            {
                entity.ToTable("movie_country");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("countryID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.MovieCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_country_country_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieCountries)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_country_movie_ID");
            });

            modelBuilder.Entity<MovieCrew>(entity =>
            {
                entity.ToTable("movie_crew");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.PersonId).HasColumnName("personID");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieCrews)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_crew_movie_ID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.MovieCrews)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_crew_person_ID");
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.ToTable("movie_genre");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GenreId).HasColumnName("genreID");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MovieGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_genre_genre_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieGenres)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_genre_movie_ID");
            });

            modelBuilder.Entity<MovieKeyword>(entity =>
            {
                entity.ToTable("movie_keywords");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.MovieKeywords)
                    .HasForeignKey(d => d.KeywordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_keyword_id_keyword_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieKeywords)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_keywords_movie_ID");
            });

            modelBuilder.Entity<MovieReview>(entity =>
            {
                entity.ToTable("movie_review");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.Review)
                    .HasColumnType("ntext")
                    .HasColumnName("review");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Vote).HasColumnName("vote");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieReviews)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_review_movie_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MovieReviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_review_user_ID");
            });

            modelBuilder.Entity<MovieSource>(entity =>
            {
                entity.ToTable("movie_source");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.LinkSource)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("link_source");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieSources)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_source_movie_ID");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Popularity).HasColumnName("popularity");

                entity.Property(e => e.ProfilePath)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("profile_path");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");
            });

            modelBuilder.Entity<ProductionCompany>(entity =>
            {
                entity.ToTable("production_company");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("file_path");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");
            });

            modelBuilder.Entity<ProductionCountry>(entity =>
            {
                entity.ToTable("production_country");

                entity.Property(e => e.Id)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activated).HasColumnName("activated");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .HasColumnName("middleName");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber")
                    .IsFixedLength(true);

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.Salt)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("salt");

                entity.Property(e => e.SurName)
                    .HasMaxLength(50)
                    .HasColumnName("surName");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_role_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
