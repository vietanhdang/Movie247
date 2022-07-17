using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movie247.Areas.Identity.Data;
using Movie247.Models;

namespace Movie247.Data
{
    public class Movie247Context : IdentityDbContext<Movie247User>
    {
        public Movie247Context()
        {
        }

        public Movie247Context(DbContextOptions<Movie247Context> options)
            : base(options)
        {
        }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieCast> MovieCasts { get; set; }
        public virtual DbSet<MovieCompany> MovieCompanies { get; set; }
        public virtual DbSet<MovieCountry> MovieCountries { get; set; }
        public virtual DbSet<MovieCrew> MovieCrews { get; set; }
        public virtual DbSet<MovieFavourite> MovieFavourites { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<MovieReview> MovieReviews { get; set; }
        public virtual DbSet<MovieSource> MovieSources { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<MovieComment> MovieComments { get; set; }
        public virtual DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public virtual DbSet<ProductionCountry> ProductionCountries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build().GetConnectionString("AppDbContext"));
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Genre>(entity =>
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


            builder.Entity<Movie>(entity =>
            {
                entity.ToTable("movie");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasIndex(e => e.Title);

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
                    .HasMaxLength(4000)
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

            builder.Entity<MovieCast>(entity =>
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

            builder.Entity<MovieCompany>(entity =>
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

            builder.Entity<MovieCountry>(entity =>
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

            builder.Entity<MovieCrew>(entity =>
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

            builder.Entity<MovieFavourite>(entity =>
            {
                entity.ToTable("movie_favourite");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieFavourites)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_favourite_movie_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MovieFavourites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_favourite_user_ID");

            });

            builder.Entity<MovieGenre>(entity =>
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


            builder.Entity<MovieReview>(entity =>
            {
                entity.ToTable("movie_review");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(300)
                    .HasColumnName("comment");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MovieId).HasColumnName("movieID");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userID");

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
            builder.Entity<MovieComment>(entity =>
            {
                entity.ToTable("movie_comment");


            });

            builder.Entity<MovieSource>(entity =>
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

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updateAt");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieSources)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_source_movie_ID");
            });

            builder.Entity<Person>(entity =>
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
                    .HasMaxLength(4000)
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

            builder.Entity<ProductionCompany>(entity =>
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

            builder.Entity<ProductionCountry>(entity =>
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
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }


    }
}
