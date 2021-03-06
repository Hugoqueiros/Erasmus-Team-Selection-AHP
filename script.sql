USE [master]
GO
/****** Object:  Database [SAD]    Script Date: 02/12/2021 15:53:43 ******/
CREATE DATABASE [SAD]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SAD', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SAD.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SAD_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SAD_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SAD] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SAD].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SAD] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SAD] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SAD] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SAD] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SAD] SET ARITHABORT OFF 
GO
ALTER DATABASE [SAD] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SAD] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SAD] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SAD] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SAD] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SAD] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SAD] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SAD] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SAD] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SAD] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SAD] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SAD] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SAD] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SAD] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SAD] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SAD] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SAD] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SAD] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SAD] SET  MULTI_USER 
GO
ALTER DATABASE [SAD] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SAD] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SAD] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SAD] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SAD] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SAD] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SAD] SET QUERY_STORE = OFF
GO
USE [SAD]
GO
/****** Object:  Table [dbo].[Area]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[area_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	[area_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coordinator]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coordinator](
	[coordinator_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[institution_id] [int] NOT NULL,
 CONSTRAINT [PK_Coordinator] PRIMARY KEY CLUSTERED 
(
	[coordinator_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Institution]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Institution](
	[institution_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[phone] [varchar](50) NOT NULL,
	[country] [varchar](50) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[adress] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Institution] PRIMARY KEY CLUSTERED 
(
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[language_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](80) NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[language_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[project_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[desc] [text] NOT NULL,
	[nr_students] [int] NOT NULL,
	[date_start] [date] NOT NULL,
	[date_end] [date] NOT NULL,
	[institution_id] [int] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project_Area]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_Area](
	[project_area] [int] IDENTITY(1,1) NOT NULL,
	[project_id] [int] NOT NULL,
	[area_id] [int] NOT NULL,
 CONSTRAINT [PK_Project_Area] PRIMARY KEY CLUSTERED 
(
	[project_area] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project_Partner]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_Partner](
	[partner_id] [int] IDENTITY(1,1) NOT NULL,
	[project_id] [int] NOT NULL,
	[institution_id] [int] NOT NULL,
 CONSTRAINT [PK_Project_Partner] PRIMARY KEY CLUSTERED 
(
	[partner_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project_SK]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_SK](
	[project_sk_id] [int] IDENTITY(1,1) NOT NULL,
	[project_id] [int] NOT NULL,
	[social_skill_id] [int] NOT NULL,
 CONSTRAINT [PK_Project_SK] PRIMARY KEY CLUSTERED 
(
	[project_sk_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project_Team]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_Team](
	[project_team_id] [int] IDENTITY(1,1) NOT NULL,
	[project_id] [int] NULL,
	[team_id] [int] NULL,
 CONSTRAINT [PK_Project_Team] PRIMARY KEY CLUSTERED 
(
	[project_team_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project_Tech]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_Tech](
	[project_tech_id] [int] IDENTITY(1,1) NOT NULL,
	[project_id] [int] NOT NULL,
	[tech_id] [int] NOT NULL,
 CONSTRAINT [PK_Project_Tech] PRIMARY KEY CLUSTERED 
(
	[project_tech_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Social_Skill]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Social_Skill](
	[social_skill_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Social_Skill] PRIMARY KEY CLUSTERED 
(
	[social_skill_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[student_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[phone] [varchar](50) NOT NULL,
	[date_birth] [date] NOT NULL,
	[degree] [varchar](100) NOT NULL,
	[institution_id] [int] NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[student_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_Area]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Area](
	[student_area_id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NOT NULL,
	[area_id] [int] NOT NULL,
 CONSTRAINT [PK_Student_Area] PRIMARY KEY CLUSTERED 
(
	[student_area_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_Language]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Language](
	[student_language_id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
 CONSTRAINT [PK_Student_Language] PRIMARY KEY CLUSTERED 
(
	[student_language_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_SK]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_SK](
	[student_sk_id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NOT NULL,
	[social_skill_id] [int] NOT NULL,
 CONSTRAINT [PK_Student_SK] PRIMARY KEY CLUSTERED 
(
	[student_sk_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_Tech]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Tech](
	[student_tech_id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NOT NULL,
	[tech_id] [int] NOT NULL,
 CONSTRAINT [PK_Student_Tech] PRIMARY KEY CLUSTERED 
(
	[student_tech_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[team_id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NOT NULL,
	[role_id] [int] NOT NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[team_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tech]    Script Date: 02/12/2021 15:53:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tech](
	[tech_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [int] NOT NULL,
 CONSTRAINT [PK_Tech] PRIMARY KEY CLUSTERED 
(
	[tech_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Coordinator]  WITH CHECK ADD  CONSTRAINT [FK_Coordinator_Institution] FOREIGN KEY([institution_id])
REFERENCES [dbo].[Institution] ([institution_id])
GO
ALTER TABLE [dbo].[Coordinator] CHECK CONSTRAINT [FK_Coordinator_Institution]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Institution] FOREIGN KEY([institution_id])
REFERENCES [dbo].[Institution] ([institution_id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Institution]
GO
ALTER TABLE [dbo].[Project_Area]  WITH CHECK ADD  CONSTRAINT [FK_Project_Area_Area] FOREIGN KEY([area_id])
REFERENCES [dbo].[Area] ([area_id])
GO
ALTER TABLE [dbo].[Project_Area] CHECK CONSTRAINT [FK_Project_Area_Area]
GO
ALTER TABLE [dbo].[Project_Area]  WITH CHECK ADD  CONSTRAINT [FK_Project_Area_Project] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Project_Area] CHECK CONSTRAINT [FK_Project_Area_Project]
GO
ALTER TABLE [dbo].[Project_Partner]  WITH CHECK ADD  CONSTRAINT [FK_Partner_Institution] FOREIGN KEY([institution_id])
REFERENCES [dbo].[Institution] ([institution_id])
GO
ALTER TABLE [dbo].[Project_Partner] CHECK CONSTRAINT [FK_Partner_Institution]
GO
ALTER TABLE [dbo].[Project_Partner]  WITH CHECK ADD  CONSTRAINT [FK_Partner_Project] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Project_Partner] CHECK CONSTRAINT [FK_Partner_Project]
GO
ALTER TABLE [dbo].[Project_SK]  WITH CHECK ADD  CONSTRAINT [FK_Project_SK_Project] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Project_SK] CHECK CONSTRAINT [FK_Project_SK_Project]
GO
ALTER TABLE [dbo].[Project_SK]  WITH CHECK ADD  CONSTRAINT [FK_Project_SK_Social_Skill] FOREIGN KEY([social_skill_id])
REFERENCES [dbo].[Social_Skill] ([social_skill_id])
GO
ALTER TABLE [dbo].[Project_SK] CHECK CONSTRAINT [FK_Project_SK_Social_Skill]
GO
ALTER TABLE [dbo].[Project_Team]  WITH CHECK ADD  CONSTRAINT [FK_Project_Team_Project] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Project_Team] CHECK CONSTRAINT [FK_Project_Team_Project]
GO
ALTER TABLE [dbo].[Project_Team]  WITH CHECK ADD  CONSTRAINT [FK_Project_Team_Team] FOREIGN KEY([team_id])
REFERENCES [dbo].[Team] ([team_id])
GO
ALTER TABLE [dbo].[Project_Team] CHECK CONSTRAINT [FK_Project_Team_Team]
GO
ALTER TABLE [dbo].[Project_Tech]  WITH CHECK ADD  CONSTRAINT [FK_Project_Tech_Project] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Project_Tech] CHECK CONSTRAINT [FK_Project_Tech_Project]
GO
ALTER TABLE [dbo].[Project_Tech]  WITH CHECK ADD  CONSTRAINT [FK_Project_Tech_Tech] FOREIGN KEY([tech_id])
REFERENCES [dbo].[Tech] ([tech_id])
GO
ALTER TABLE [dbo].[Project_Tech] CHECK CONSTRAINT [FK_Project_Tech_Tech]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Institution] FOREIGN KEY([institution_id])
REFERENCES [dbo].[Institution] ([institution_id])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Institution]
GO
ALTER TABLE [dbo].[Student_Area]  WITH CHECK ADD  CONSTRAINT [FK_Student_Area_Area] FOREIGN KEY([area_id])
REFERENCES [dbo].[Area] ([area_id])
GO
ALTER TABLE [dbo].[Student_Area] CHECK CONSTRAINT [FK_Student_Area_Area]
GO
ALTER TABLE [dbo].[Student_Area]  WITH CHECK ADD  CONSTRAINT [FK_Student_Area_Student] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Student_Area] CHECK CONSTRAINT [FK_Student_Area_Student]
GO
ALTER TABLE [dbo].[Student_Language]  WITH CHECK ADD  CONSTRAINT [FK_Student_Language_Language] FOREIGN KEY([language_id])
REFERENCES [dbo].[Language] ([language_id])
GO
ALTER TABLE [dbo].[Student_Language] CHECK CONSTRAINT [FK_Student_Language_Language]
GO
ALTER TABLE [dbo].[Student_Language]  WITH CHECK ADD  CONSTRAINT [FK_Student_Language_Student] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Student_Language] CHECK CONSTRAINT [FK_Student_Language_Student]
GO
ALTER TABLE [dbo].[Student_SK]  WITH CHECK ADD  CONSTRAINT [FK_Student_SK_Social_Skill] FOREIGN KEY([social_skill_id])
REFERENCES [dbo].[Social_Skill] ([social_skill_id])
GO
ALTER TABLE [dbo].[Student_SK] CHECK CONSTRAINT [FK_Student_SK_Social_Skill]
GO
ALTER TABLE [dbo].[Student_SK]  WITH CHECK ADD  CONSTRAINT [FK_Student_SK_Student] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Student_SK] CHECK CONSTRAINT [FK_Student_SK_Student]
GO
ALTER TABLE [dbo].[Student_Tech]  WITH CHECK ADD  CONSTRAINT [FK_Student_Tech_Student] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Student_Tech] CHECK CONSTRAINT [FK_Student_Tech_Student]
GO
ALTER TABLE [dbo].[Student_Tech]  WITH CHECK ADD  CONSTRAINT [FK_Student_Tech_Tech] FOREIGN KEY([tech_id])
REFERENCES [dbo].[Tech] ([tech_id])
GO
ALTER TABLE [dbo].[Student_Tech] CHECK CONSTRAINT [FK_Student_Tech_Tech]
GO
ALTER TABLE [dbo].[Team]  WITH CHECK ADD  CONSTRAINT [FK_Team_Role] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[Team] CHECK CONSTRAINT [FK_Team_Role]
GO
ALTER TABLE [dbo].[Team]  WITH CHECK ADD  CONSTRAINT [FK_Team_Student] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Team] CHECK CONSTRAINT [FK_Team_Student]
GO
USE [master]
GO
ALTER DATABASE [SAD] SET  READ_WRITE 
GO
