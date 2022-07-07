﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movie247.Models;

namespace Movie247.Migrations
{
    [DbContext(typeof(MOVIEPROJECTContext))]
    partial class MOVIEPROJECTContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Movie247.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("ntext")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.ToTable("genre");
                });

            modelBuilder.Entity("Movie247.Models.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("KeywordName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("keyword_name");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.ToTable("keyword");
                });

            modelBuilder.Entity("Movie247.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BackdropPath")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("backdrop_path");

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Duration")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("duration");

                    b.Property<double?>("ImdbAverage")
                        .HasColumnType("float")
                        .HasColumnName("imdb_average");

                    b.Property<bool?>("MovieStatus")
                        .HasColumnType("bit")
                        .HasColumnName("movie_status");

                    b.Property<string>("Overview")
                        .HasColumnType("ntext")
                        .HasColumnName("overview");

                    b.Property<string>("PosterPath")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("poster_path");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("date")
                        .HasColumnName("release_date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("title");

                    b.Property<string>("Trailer")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("trailer");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.Property<long?>("Views")
                        .HasColumnType("bigint")
                        .HasColumnName("views");

                    b.HasKey("Id");

                    b.ToTable("movie");
                });

            modelBuilder.Entity("Movie247.Models.MovieCast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Character")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("character");

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movieID");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("personID");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("PersonId");

                    b.ToTable("movie_cast");
                });

            modelBuilder.Entity("Movie247.Models.MovieCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("companyID");

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movieID");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("MovieId");

                    b.ToTable("movie_company");
                });

            modelBuilder.Entity("Movie247.Models.MovieCountry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryId")
                        .IsRequired()
                        .HasMaxLength(5)
                        .IsUnicode(false)
                        .HasColumnType("varchar(5)")
                        .HasColumnName("countryID");

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movieID");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("MovieId");

                    b.ToTable("movie_country");
                });

            modelBuilder.Entity("Movie247.Models.MovieCrew", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movieID");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("personID");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("PersonId");

                    b.ToTable("movie_crew");
                });

            modelBuilder.Entity("Movie247.Models.MovieGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("GenreId")
                        .HasColumnType("int")
                        .HasColumnName("genreID");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movieID");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MovieId");

                    b.ToTable("movie_genre");
                });

            modelBuilder.Entity("Movie247.Models.MovieKeyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("KeywordId")
                        .HasColumnType("int")
                        .HasColumnName("keyword_id");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.HasIndex("KeywordId");

                    b.HasIndex("MovieId");

                    b.ToTable("movie_keywords");
                });

            modelBuilder.Entity("Movie247.Models.MovieSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("description");

                    b.Property<string>("LinkSource")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("link_source");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("movie_source");
                });

            modelBuilder.Entity("Movie247.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("date")
                        .HasColumnName("birthday");

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("ntext")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<double?>("Popularity")
                        .HasColumnType("float")
                        .HasColumnName("popularity");

                    b.Property<string>("ProfilePath")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("profile_path");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.ToTable("person");
                });

            modelBuilder.Entity("Movie247.Models.ProductionCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("ntext")
                        .HasColumnName("description");

                    b.Property<string>("FilePath")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("file_path");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.ToTable("production_company");
                });

            modelBuilder.Entity("Movie247.Models.ProductionCountry", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(5)
                        .IsUnicode(false)
                        .HasColumnType("varchar(5)")
                        .HasColumnName("ID");

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("createAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updateAt");

                    b.HasKey("Id");

                    b.ToTable("production_country");
                });

            modelBuilder.Entity("Movie247.Models.MovieCast", b =>
                {
                    b.HasOne("Movie247.Models.Movie", "Movie")
                        .WithMany("MovieCasts")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_cast_movie_ID")
                        .IsRequired();

                    b.HasOne("Movie247.Models.Person", "Person")
                        .WithMany("MovieCasts")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK_movie_cast_person_ID")
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Movie247.Models.MovieCompany", b =>
                {
                    b.HasOne("Movie247.Models.ProductionCompany", "Company")
                        .WithMany("MovieCompanies")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_movie_company_company_ID")
                        .IsRequired();

                    b.HasOne("Movie247.Models.Movie", "Movie")
                        .WithMany("MovieCompanies")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_company_movie_ID")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Movie247.Models.MovieCountry", b =>
                {
                    b.HasOne("Movie247.Models.ProductionCountry", "Country")
                        .WithMany("MovieCountries")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("FK_movie_country_country_ID")
                        .IsRequired();

                    b.HasOne("Movie247.Models.Movie", "Movie")
                        .WithMany("MovieCountries")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_country_movie_ID")
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Movie247.Models.MovieCrew", b =>
                {
                    b.HasOne("Movie247.Models.Movie", "Movie")
                        .WithMany("MovieCrews")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_crew_movie_ID")
                        .IsRequired();

                    b.HasOne("Movie247.Models.Person", "Person")
                        .WithMany("MovieCrews")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK_movie_crew_person_ID")
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Movie247.Models.MovieGenre", b =>
                {
                    b.HasOne("Movie247.Models.Genre", "Genre")
                        .WithMany("MovieGenres")
                        .HasForeignKey("GenreId")
                        .HasConstraintName("FK_movie_genre_genre_ID")
                        .IsRequired();

                    b.HasOne("Movie247.Models.Movie", "Movie")
                        .WithMany("MovieGenres")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_genre_movie_ID")
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Movie247.Models.MovieKeyword", b =>
                {
                    b.HasOne("Movie247.Models.Keyword", "Keyword")
                        .WithMany("MovieKeywords")
                        .HasForeignKey("KeywordId")
                        .HasConstraintName("FK_keyword_id_keyword_ID")
                        .IsRequired();

                    b.HasOne("Movie247.Models.Movie", "Movie")
                        .WithMany("MovieKeywords")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_keywords_movie_ID")
                        .IsRequired();

                    b.Navigation("Keyword");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Movie247.Models.MovieSource", b =>
                {
                    b.HasOne("Movie247.Models.Movie", "Movie")
                        .WithMany("MovieSources")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_source_movie_ID")
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Movie247.Models.Genre", b =>
                {
                    b.Navigation("MovieGenres");
                });

            modelBuilder.Entity("Movie247.Models.Keyword", b =>
                {
                    b.Navigation("MovieKeywords");
                });

            modelBuilder.Entity("Movie247.Models.Movie", b =>
                {
                    b.Navigation("MovieCasts");

                    b.Navigation("MovieCompanies");

                    b.Navigation("MovieCountries");

                    b.Navigation("MovieCrews");

                    b.Navigation("MovieGenres");

                    b.Navigation("MovieKeywords");

                    b.Navigation("MovieSources");
                });

            modelBuilder.Entity("Movie247.Models.Person", b =>
                {
                    b.Navigation("MovieCasts");

                    b.Navigation("MovieCrews");
                });

            modelBuilder.Entity("Movie247.Models.ProductionCompany", b =>
                {
                    b.Navigation("MovieCompanies");
                });

            modelBuilder.Entity("Movie247.Models.ProductionCountry", b =>
                {
                    b.Navigation("MovieCountries");
                });
#pragma warning restore 612, 618
        }
    }
}
