USE [master]
GO

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'CCS')
BEGIN
	ALTER DATABASE CCS SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE CCS SET ONLINE;
	DROP DATABASE CCS;
END
GO

/****** Object:  Database CCS    Script Date: 2022-02-07 11:09:28 AM ******/
CREATE DATABASE CCS
-- CONTAINMENT = NONE
-- ON  PRIMARY 
--( NAME = N'CCS', FILENAME = N'E:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\CCS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
-- LOG ON 
--( NAME = N'CCS_log', FILENAME = N'F:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\Data\CCS.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE CCS SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC CCS.[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE CCS SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE CCS SET ANSI_NULLS OFF 
GO
ALTER DATABASE CCS SET ANSI_PADDING OFF 
GO
ALTER DATABASE CCS SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE CCS SET ARITHABORT OFF 
GO
ALTER DATABASE CCS SET AUTO_CLOSE OFF 
GO
ALTER DATABASE CCS SET AUTO_SHRINK OFF 
GO
ALTER DATABASE CCS SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE CCS SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE CCS SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE CCS SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE CCS SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE CCS SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE CCS SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE CCS SET  ENABLE_BROKER 
GO
ALTER DATABASE CCS SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE CCS SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE CCS SET TRUSTWORTHY OFF 
GO
ALTER DATABASE CCS SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE CCS SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE CCS SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE CCS SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE CCS SET RECOVERY FULL 
GO
ALTER DATABASE CCS SET  MULTI_USER 
GO
ALTER DATABASE CCS SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE CCS SET DB_CHAINING OFF 
GO
ALTER DATABASE CCS SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE CCS SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE CCS SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE CCS SET QUERY_STORE = OFF
GO
USE CCS
GO
/****** Object:  User [TEAMCCS]    Script Date: 2022-02-07 11:09:28 AM ******/
CREATE USER [TEAMCCS] FOR LOGIN [TEAMCCS] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [IIS APPPOOL\CCSAppPool]    Script Date: 2022-02-07 11:09:28 AM ******/
CREATE USER [IIS APPPOOL\CCSAppPool] FOR LOGIN [IIS APPPOOL\CCSAppPool] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_datareader] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [TEAMCCS]
GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\CCSAppPool]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\CCSAppPool]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\CCSAppPool]
GO
/****** Object:  Table [dbo].[Challenge]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Challenge](
	[challenge_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[title] [varchar](50) NOT NULL,
	[description] [varchar](600) NOT NULL,
	[example] [varchar](300) NULL,
	[function_name] [varchar](20) NOT NULL,
	[teacher_id] [numeric](7, 0) NOT NULL,
	[active] [bit] NOT NULL,
	[difficulty_level] [varchar](15) NOT NULL,
	[creation_date] [date] NOT NULL,
	[return_type_id][numeric](7, 0) NOT NULL
 	CONSTRAINT [PK_Challenge] PRIMARY KEY CLUSTERED ([challenge_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChallengeLanguage]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChallengeLanguage](
	[challenge_language_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[language_id] [numeric](7, 0) NOT NULL,
	[challenge_id] [numeric](7, 0) NOT NULL,
 	CONSTRAINT [PK_ChallengeLanguage] PRIMARY KEY CLUSTERED ([challenge_language_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CodeSubmission]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodeSubmission](
	[code_submission_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[code] [varchar](1000) NOT NULL,
	[student_id] [numeric](7, 0) NOT NULL,
	[challenge_language_id] [numeric](7, 0) NOT NULL,
	[status] [bit] NOT NULL,
	[last_attempted] [date] NOT NULL,
	CONSTRAINT [PK_CodeSubmisssion] PRIMARY KEY CLUSTERED ([code_submission_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataType]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataType](
	[data_type_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[data_type_name] [varchar](15) NOT NULL,
	[language_id] [numeric](7, 0) NOT NULL,
 	CONSTRAINT [PK_DataType] PRIMARY KEY CLUSTERED ([data_type_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[language_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[language_name] [varchar](15) NOT NULL,
 	CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([language_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestCaseParameter]    Script Date: 2022-03-21 9:35:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TestCaseParameter](
	[test_case_parameter_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[test_case_id] [numeric](7, 0) NOT NULL,
	[parameter_id] [numeric](7, 0) NOT NULL,
	[value] [varchar](7999) NOT NULL,
 	CONSTRAINT [PK_TestCaseParameter] PRIMARY KEY CLUSTERED ([test_case_parameter_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parameter]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameter](
	[parameter_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[data_type_id] [numeric](7, 0) NOT NULL,
	[challenge_language_id] [numeric](7, 0) NOT NULL,
	[position] [numeric](2, 0) NOT NULL,
	[default_value] [varchar](7999) NULL,
	[parameter_name] [varchar](35) NOT NULl,
	CONSTRAINT [PK_Parameter] PRIMARY KEY CLUSTERED ([parameter_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Result]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Result](
	[result_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[passed] [bit] NOT NULL,
	[test_case_id] [numeric](7, 0) NOT NULL,
	[code_submission_id] [numeric](7, 0) NOT NULL,
	[code_output] [varchar](15) NOT NULL,
 	CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED ([result_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[student_id] [numeric](7, 0) NOT NULL,
	[first_name] [varchar](60) NOT NULL,
	[last_name] [varchar](60) NOT NULL,
	CONSTRAINT [Student_PK] PRIMARY KEY CLUSTERED ([student_id])
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[teacher_id] [numeric](7, 0) NOT NULL,
 	CONSTRAINT [Teacher_PK] PRIMARY KEY CLUSTERED ([teacher_id]) 
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestCase]    Script Date: 2022-02-07 11:09:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestCase](
	[test_case_id] [numeric](7, 0) IDENTITY(1,1) NOT NULL,
	[expected_result] [varchar](15) NOT NULL,
	[challenge_language_id] [numeric](7, 0) NOT NULL,
 	CONSTRAINT [PK_TestCase] PRIMARY KEY CLUSTERED ([test_case_id])
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Challenge]  WITH NOCHECK ADD  CONSTRAINT [Challenge_Teacher_FK] FOREIGN KEY([teacher_id])
REFERENCES [dbo].[Teacher] ([teacher_id])
GO
ALTER TABLE [dbo].[Challenge] CHECK CONSTRAINT [Challenge_Teacher_FK]
GO
ALTER TABLE [dbo].[Challenge]  WITH NOCHECK ADD  CONSTRAINT [Challenge_DataType_FK] FOREIGN KEY([return_type_id])
REFERENCES [dbo].[DataType] ([data_type_id])
GO
ALTER TABLE [dbo].[Challenge] CHECK CONSTRAINT [Challenge_DataType_FK]
GO
ALTER TABLE [dbo].[ChallengeLanguage]  WITH NOCHECK ADD  CONSTRAINT [ChallengeLanguage_Challenge_FK] FOREIGN KEY([challenge_id])
REFERENCES [dbo].[Challenge] ([challenge_id])
GO
ALTER TABLE [dbo].[ChallengeLanguage] CHECK CONSTRAINT [ChallengeLanguage_Challenge_FK]
GO
ALTER TABLE [dbo].[ChallengeLanguage]  WITH NOCHECK ADD  CONSTRAINT [ChallengeLanguage_Language_FK] FOREIGN KEY([language_id])
REFERENCES [dbo].[Language] ([language_id])
GO
ALTER TABLE [dbo].[ChallengeLanguage] CHECK CONSTRAINT [ChallengeLanguage_Language_FK]
GO
ALTER TABLE [dbo].[CodeSubmission]  WITH CHECK ADD  CONSTRAINT [CodeSubmission_Student_FK] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[CodeSubmission] CHECK CONSTRAINT [CodeSubmission_Student_FK]
GO
ALTER TABLE [dbo].[CodeSubmission]  WITH NOCHECK ADD  CONSTRAINT [CodeSubmisssion_Challenge_FK] FOREIGN KEY([challenge_language_id])
REFERENCES [dbo].[Challenge] ([challenge_id])
GO
ALTER TABLE [dbo].[CodeSubmission] CHECK CONSTRAINT [CodeSubmisssion_Challenge_FK]
GO
ALTER TABLE [dbo].[DataType]  WITH CHECK ADD  CONSTRAINT [DataType_Language_FK] FOREIGN KEY([language_id])
REFERENCES [dbo].[Language] ([language_id])
GO
ALTER TABLE [dbo].[DataType] CHECK CONSTRAINT [DataType_Language_FK]
GO
ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD  CONSTRAINT [FK_Parameter_ChallengeLanguage] FOREIGN KEY([challenge_language_id])
REFERENCES [dbo].[ChallengeLanguage] ([challenge_language_id])
GO
ALTER TABLE [dbo].[Parameter] CHECK CONSTRAINT [FK_Parameter_ChallengeLanguage]
GO
ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD  CONSTRAINT [Parameter_DataType_FK] FOREIGN KEY([data_type_id])
REFERENCES [dbo].[DataType] ([data_type_id])
GO
ALTER TABLE [dbo].[Parameter] CHECK CONSTRAINT [Parameter_DataType_FK]
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD  CONSTRAINT [Result_CodeSubmission_FK] FOREIGN KEY([code_submission_id])
REFERENCES [dbo].[CodeSubmission] ([code_submission_id])
GO
ALTER TABLE [dbo].[Result] CHECK CONSTRAINT [Result_CodeSubmission_FK]
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD  CONSTRAINT [Result_TestCase_FK] FOREIGN KEY([test_case_id])
REFERENCES [dbo].[TestCase] ([test_case_id])
GO
ALTER TABLE [dbo].[Result] CHECK CONSTRAINT [Result_TestCase_FK]
GO
ALTER TABLE [dbo].[TestCase]  WITH NOCHECK ADD  CONSTRAINT [TestCase_ChallengeLanguage_FK] FOREIGN KEY([challenge_language_id])
REFERENCES [dbo].[ChallengeLanguage] ([challenge_language_id])
GO
ALTER TABLE [dbo].[TestCase] CHECK CONSTRAINT [TestCase_ChallengeLanguage_FK]
GO
ALTER TABLE [dbo].[TestCaseParameter]  WITH CHECK ADD  CONSTRAINT [FK_TestCaseParameter_Parameter] FOREIGN KEY([parameter_id])
REFERENCES [dbo].[Parameter] ([parameter_id])
GO
ALTER TABLE [dbo].[TestCaseParameter] CHECK CONSTRAINT [FK_TestCaseParameter_Parameter]
GO
ALTER TABLE [dbo].[TestCaseParameter]  WITH CHECK ADD  CONSTRAINT [FK_TestCaseParameter_TestCase] FOREIGN KEY([test_case_id])
REFERENCES [dbo].[TestCase] ([test_case_id])
GO
ALTER TABLE [dbo].[TestCaseParameter] CHECK CONSTRAINT [FK_TestCaseParameter_TestCase]
GO

USE [master]
GO
ALTER DATABASE CCS SET  READ_WRITE 
GO
