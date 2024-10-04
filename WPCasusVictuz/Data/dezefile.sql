USE [master]
GO
/****** Object:  Database [Victuz]    Script Date: 4-10-2024 16:50:49 ******/
CREATE DATABASE [Victuz]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Victuz', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Victuz.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Victuz_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Victuz_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Victuz] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Victuz].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Victuz] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Victuz] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Victuz] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Victuz] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Victuz] SET ARITHABORT OFF 
GO
ALTER DATABASE [Victuz] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Victuz] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Victuz] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Victuz] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Victuz] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Victuz] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Victuz] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Victuz] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Victuz] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Victuz] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Victuz] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Victuz] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Victuz] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Victuz] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Victuz] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Victuz] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Victuz] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Victuz] SET RECOVERY FULL 
GO
ALTER DATABASE [Victuz] SET  MULTI_USER 
GO
ALTER DATABASE [Victuz] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Victuz] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Victuz] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Victuz] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Victuz] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Victuz] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Victuz', N'ON'
GO
ALTER DATABASE [Victuz] SET QUERY_STORE = OFF
GO
USE [Victuz]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4-10-2024 16:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Activities]    Script Date: 4-10-2024 16:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Date] [datetime2](7) NULL,
	[MaxParticipants] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedbyBM] [int] NULL,
 CONSTRAINT [PK_Activities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BoardMembers]    Script Date: 4-10-2024 16:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BoardMembers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[MemberId] [int] NULL,
 CONSTRAINT [PK_BoardMembers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 4-10-2024 16:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NOT NULL,
	[BoardMemberId] [int] NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polls]    Script Date: 4-10-2024 16:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polls](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Question] [nvarchar](max) NULL,
	[Options] [nvarchar](max) NULL,
	[CreatedByBoardMemberId] [int] NULL,
	[OptionsString] [nvarchar](max) NULL,
 CONSTRAINT [PK_Polls] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registrations]    Script Date: 4-10-2024 16:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registrations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NULL,
	[AktivityId] [int] NULL,
	[RegistrationDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Registrations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Votes]    Script Date: 4-10-2024 16:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Votes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PollId] [int] NULL,
	[MemberId] [int] NULL,
	[SelectedOption] [nvarchar](max) NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Activities_CreatedbyBM]    Script Date: 4-10-2024 16:50:49 ******/
CREATE NONCLUSTERED INDEX [IX_Activities_CreatedbyBM] ON [dbo].[Activities]
(
	[CreatedbyBM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BoardMembers_MemberId]    Script Date: 4-10-2024 16:50:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_BoardMembers_MemberId] ON [dbo].[BoardMembers]
(
	[MemberId] ASC
)
WHERE ([MemberId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Polls_CreatedByBoardMemberId]    Script Date: 4-10-2024 16:50:49 ******/
CREATE NONCLUSTERED INDEX [IX_Polls_CreatedByBoardMemberId] ON [dbo].[Polls]
(
	[CreatedByBoardMemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Registrations_AktivityId]    Script Date: 4-10-2024 16:50:49 ******/
CREATE NONCLUSTERED INDEX [IX_Registrations_AktivityId] ON [dbo].[Registrations]
(
	[AktivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Registrations_MemberId]    Script Date: 4-10-2024 16:50:49 ******/
CREATE NONCLUSTERED INDEX [IX_Registrations_MemberId] ON [dbo].[Registrations]
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Votes_MemberId]    Script Date: 4-10-2024 16:50:49 ******/
CREATE NONCLUSTERED INDEX [IX_Votes_MemberId] ON [dbo].[Votes]
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Votes_PollId]    Script Date: 4-10-2024 16:50:49 ******/
CREATE NONCLUSTERED INDEX [IX_Votes_PollId] ON [dbo].[Votes]
(
	[PollId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Members] ADD  DEFAULT (N'') FOR [Status]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_BoardMembers_CreatedbyBM] FOREIGN KEY([CreatedbyBM])
REFERENCES [dbo].[BoardMembers] ([Id])
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_BoardMembers_CreatedbyBM]
GO
ALTER TABLE [dbo].[BoardMembers]  WITH CHECK ADD  CONSTRAINT [FK_BoardMembers_Members_MemberId] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[BoardMembers] CHECK CONSTRAINT [FK_BoardMembers_Members_MemberId]
GO
ALTER TABLE [dbo].[Polls]  WITH CHECK ADD  CONSTRAINT [FK_Polls_BoardMembers_CreatedByBoardMemberId] FOREIGN KEY([CreatedByBoardMemberId])
REFERENCES [dbo].[BoardMembers] ([Id])
GO
ALTER TABLE [dbo].[Polls] CHECK CONSTRAINT [FK_Polls_BoardMembers_CreatedByBoardMemberId]
GO
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Activities_AktivityId] FOREIGN KEY([AktivityId])
REFERENCES [dbo].[Activities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Activities_AktivityId]
GO
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Members_MemberId] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Members] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Members_MemberId]
GO
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Members_MemberId] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Members] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Members_MemberId]
GO
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Polls_PollId] FOREIGN KEY([PollId])
REFERENCES [dbo].[Polls] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Polls_PollId]
GO
USE [master]
GO
ALTER DATABASE [Victuz] SET  READ_WRITE 
GO
