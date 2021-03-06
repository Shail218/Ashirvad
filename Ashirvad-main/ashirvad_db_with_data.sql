USE [Ashirvad]
GO
ALTER TABLE [dbo].[USER_DEF] DROP CONSTRAINT [FK_USER_DEF_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[USER_DEF] DROP CONSTRAINT [FK_USER_DEF_BRANCH_STAFF]
GO
ALTER TABLE [dbo].[SUBJECT_MASTER] DROP CONSTRAINT [FK_SUBJECT_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[SUBJECT_MASTER] DROP CONSTRAINT [FK_SUBJECT_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[STUDENT_MAINT] DROP CONSTRAINT [FK_STUDENT_MAINT_STUDENT_MASTER]
GO
ALTER TABLE [dbo].[STD_MASTER] DROP CONSTRAINT [FK_STD_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[STD_MASTER] DROP CONSTRAINT [FK_STD_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[SCHOOL_MASTER] DROP CONSTRAINT [FK_SCHOOL_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[SCHOOL_MASTER] DROP CONSTRAINT [FK_SCHOOL_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_STAFF] DROP CONSTRAINT [FK_BRANCH_STAFF_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_STAFF] DROP CONSTRAINT [FK_BRANCH_STAFF_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_MASTER] DROP CONSTRAINT [FK_BRANCH_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_MAINT] DROP CONSTRAINT [FK_BRANCH_MAINT_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_LICENSE] DROP CONSTRAINT [FK_BRANCH_LICENSE_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_LICENSE] DROP CONSTRAINT [FK_BRANCH_LICENSE_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BATCH_MASTER] DROP CONSTRAINT [FK_BATCH_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[BATCH_MASTER] DROP CONSTRAINT [FK_BATCH_MASTER_STD_MASTER]
GO
ALTER TABLE [dbo].[BATCH_MASTER] DROP CONSTRAINT [FK_BATCH_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[TYPE_DESC] DROP CONSTRAINT [DF_TYPE_DESC_row_sta_cd]
GO
ALTER TABLE [dbo].[TRANSACTION_MASTER] DROP CONSTRAINT [DF_TRANSACTION_MASTER_last_mod_by]
GO
ALTER TABLE [dbo].[TRANSACTION_MASTER] DROP CONSTRAINT [DF_TRANSACTION_MASTER_created_by]
GO
ALTER TABLE [dbo].[TRANSACTION_MASTER] DROP CONSTRAINT [DF_TRANSACTION_MASTER_created_dt]
GO
/****** Object:  Table [dbo].[USER_DEF]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[USER_DEF]
GO
/****** Object:  Table [dbo].[TYPE_DESC]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[TYPE_DESC]
GO
/****** Object:  Table [dbo].[TRANSACTION_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[TRANSACTION_MASTER]
GO
/****** Object:  Table [dbo].[SUBJECT_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[SUBJECT_MASTER]
GO
/****** Object:  Table [dbo].[STUDENT_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[STUDENT_MASTER]
GO
/****** Object:  Table [dbo].[STUDENT_MAINT]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[STUDENT_MAINT]
GO
/****** Object:  Table [dbo].[STD_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[STD_MASTER]
GO
/****** Object:  Table [dbo].[SCHOOL_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[SCHOOL_MASTER]
GO
/****** Object:  Table [dbo].[BRANCH_STAFF]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[BRANCH_STAFF]
GO
/****** Object:  Table [dbo].[BRANCH_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[BRANCH_MASTER]
GO
/****** Object:  Table [dbo].[BRANCH_MAINT]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[BRANCH_MAINT]
GO
/****** Object:  Table [dbo].[BRANCH_LICENSE]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[BRANCH_LICENSE]
GO
/****** Object:  Table [dbo].[BATCH_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
DROP TABLE [dbo].[BATCH_MASTER]
GO
/****** Object:  Table [dbo].[BATCH_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BATCH_MASTER](
	[batch_id] [bigint] IDENTITY(1,1) NOT NULL,
	[branch_id] [bigint] NOT NULL,
	[std_id] [bigint] NOT NULL,
	[batch_time] [int] NOT NULL,
	[mon_fri_batch_time] [nvarchar](50) NOT NULL,
	[sat_batch_time] [nvarchar](50) NOT NULL,
	[sun_batch_time] [nvarchar](50) NOT NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
 CONSTRAINT [PK_BATCH_MASTER] PRIMARY KEY CLUSTERED 
(
	[batch_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BRANCH_LICENSE]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BRANCH_LICENSE](
	[license_id] [bigint] NOT NULL,
	[branch_id] [bigint] NOT NULL,
	[license_key] [nvarchar](255) NOT NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
 CONSTRAINT [PK_BRANCH_LICENSE] PRIMARY KEY CLUSTERED 
(
	[license_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BRANCH_MAINT]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BRANCH_MAINT](
	[branch_id] [bigint] NOT NULL,
	[branch_logo] [varbinary](max) NULL,
	[header_logo] [varbinary](max) NULL,
	[website] [nvarchar](max) NULL,
 CONSTRAINT [PK_BRANCH_MAINT] PRIMARY KEY CLUSTERED 
(
	[branch_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BRANCH_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BRANCH_MASTER](
	[branch_id] [bigint] IDENTITY(1,1) NOT NULL,
	[branch_name] [nvarchar](255) NULL,
	[row_sta_cd] [int] NULL,
	[about_us] [nvarchar](max) NULL,
	[contact_no] [nvarchar](50) NULL,
	[mobile_no] [nvarchar](50) NULL,
	[email_id] [nvarchar](50) NULL,
	[trans_id] [bigint] NOT NULL,
 CONSTRAINT [PK_BRANCH_MASTER] PRIMARY KEY CLUSTERED 
(
	[branch_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BRANCH_STAFF]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BRANCH_STAFF](
	[staff_id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[education] [nvarchar](max) NULL,
	[dob] [datetime] NULL,
	[gender] [int] NULL,
	[address] [nvarchar](1000) NULL,
	[appt_dt] [datetime] NULL,
	[join_dt] [datetime] NULL,
	[leaving_dt] [datetime] NULL,
	[email_id] [nvarchar](80) NULL,
	[branch_id] [bigint] NOT NULL,
	[mobile_no] [nvarchar](50) NOT NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
 CONSTRAINT [PK_BRANCH_STAFF] PRIMARY KEY CLUSTERED 
(
	[staff_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SCHOOL_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCHOOL_MASTER](
	[school_id] [bigint] IDENTITY(1,1) NOT NULL,
	[school_name] [nvarchar](255) NOT NULL,
	[branch_id] [bigint] NOT NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
 CONSTRAINT [PK_SCHOOL_MASTER] PRIMARY KEY CLUSTERED 
(
	[school_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STD_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STD_MASTER](
	[std_id] [bigint] IDENTITY(1,1) NOT NULL,
	[branch_id] [bigint] NOT NULL,
	[standard] [nvarchar](50) NOT NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
 CONSTRAINT [PK_STD_MASTER] PRIMARY KEY CLUSTERED 
(
	[std_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STUDENT_MAINT]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUDENT_MAINT](
	[parent_id] [bigint] IDENTITY(1,1) NOT NULL,
	[student_id] [bigint] NOT NULL,
	[parent_name] [nvarchar](50) NOT NULL,
	[father_occupation] [nvarchar](50) NULL,
	[mother_occupation] [nvarchar](50) NULL,
	[contact_no] [nchar](10) NOT NULL,
 CONSTRAINT [PK_STUDENT_MAINT] PRIMARY KEY CLUSTERED 
(
	[parent_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STUDENT_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[STUDENT_MASTER](
	[student_id] [bigint] IDENTITY(1,1) NOT NULL,
	[gr_no] [nvarchar](50) NOT NULL,
	[first_name] [nvarchar](50) NOT NULL,
	[middle_name] [nvarchar](50) NOT NULL,
	[last_name] [nvarchar](50) NOT NULL,
	[dob] [datetime] NULL,
	[address] [nvarchar](max) NOT NULL,
	[branch_id] [bigint] NOT NULL,
	[std_id] [bigint] NOT NULL,
	[school_id] [bigint] NULL,
	[school_time] [int] NULL,
	[batch_time] [int] NOT NULL,
	[last_yr_result] [int] NULL,
	[grade] [nvarchar](50) NULL,
	[last_yr_class_name] [nvarchar](255) NULL,
	[contact_no] [nvarchar](50) NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
	[stud_img] [varbinary](max) NULL,
 CONSTRAINT [PK_STUDENT_MASTER] PRIMARY KEY CLUSTERED 
(
	[student_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SUBJECT_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUBJECT_MASTER](
	[subject_id] [bigint] IDENTITY(1,1) NOT NULL,
	[subject] [nvarchar](255) NOT NULL,
	[branch_id] [bigint] NOT NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
 CONSTRAINT [PK_SUBJECT_MASTER] PRIMARY KEY CLUSTERED 
(
	[subject_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TRANSACTION_MASTER]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRANSACTION_MASTER](
	[trans_id] [bigint] IDENTITY(1,1) NOT NULL,
	[created_dt] [datetime] NOT NULL,
	[created_by] [nvarchar](50) NOT NULL,
	[created_id] [bigint] NULL,
	[last_mod_dt] [datetime] NULL,
	[last_mod_by] [nvarchar](50) NOT NULL,
	[last_mod_id] [bigint] NULL,
 CONSTRAINT [PK_TRANSACTION_MASTER] PRIMARY KEY CLUSTERED 
(
	[trans_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TYPE_DESC]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TYPE_DESC](
	[class] [int] NOT NULL,
	[type] [int] NOT NULL,
	[short_desc] [nvarchar](255) NULL,
	[long_desc] [nvarchar](max) NULL,
	[row_sta_cd] [char](1) NOT NULL,
 CONSTRAINT [PK_TYPE_DESC] PRIMARY KEY CLUSTERED 
(
	[class] ASC,
	[type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USER_DEF]    Script Date: 30/03/2021 19:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_DEF](
	[user_id] [bigint] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[row_sta_cd] [int] NOT NULL,
	[trans_id] [bigint] NOT NULL,
	[staff_id] [bigint] NULL,
	[branch_id] [bigint] NOT NULL,
	[user_type] [int] NOT NULL,
	[student_id] [bigint] NULL,
	[parent_id] [bigint] NULL,
 CONSTRAINT [PK_USER_DEF] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[BATCH_MASTER] ON 

INSERT [dbo].[BATCH_MASTER] ([batch_id], [branch_id], [std_id], [batch_time], [mon_fri_batch_time], [sat_batch_time], [sun_batch_time], [row_sta_cd], [trans_id]) VALUES (2, 13, 2, 1, N'4PM to 8 PM', N'2 PM to 8 PM', N' 8 AM to 9 PM', 1, 59)
SET IDENTITY_INSERT [dbo].[BATCH_MASTER] OFF
INSERT [dbo].[BRANCH_MAINT] ([branch_id], [branch_logo], [header_logo], [website]) VALUES (13, NULL, NULL, N'www.uniqtechsolutions.com')
SET IDENTITY_INSERT [dbo].[BRANCH_MASTER] ON 

INSERT [dbo].[BRANCH_MASTER] ([branch_id], [branch_name], [row_sta_cd], [about_us], [contact_no], [mobile_no], [email_id], [trans_id]) VALUES (13, N'Test', 1, N'About us', N'9856212024', N'7854123096', N'test@gmail.com', 51)
SET IDENTITY_INSERT [dbo].[BRANCH_MASTER] OFF
SET IDENTITY_INSERT [dbo].[BRANCH_STAFF] ON 

INSERT [dbo].[BRANCH_STAFF] ([staff_id], [name], [education], [dob], [gender], [address], [appt_dt], [join_dt], [leaving_dt], [email_id], [branch_id], [mobile_no], [row_sta_cd], [trans_id]) VALUES (2, N'Test', N'Ph.D', CAST(0x0000ACF600000000 AS DateTime), NULL, N'Testing', CAST(0x0000ACF600000000 AS DateTime), CAST(0x0000ACF600000000 AS DateTime), CAST(0x0000ACF600000000 AS DateTime), N'test@gmail.com', 13, N'9856322105', 1, 52)
SET IDENTITY_INSERT [dbo].[BRANCH_STAFF] OFF
SET IDENTITY_INSERT [dbo].[SCHOOL_MASTER] ON 

INSERT [dbo].[SCHOOL_MASTER] ([school_id], [school_name], [branch_id], [row_sta_cd], [trans_id]) VALUES (2, N'Gurukul', 13, 1, 57)
SET IDENTITY_INSERT [dbo].[SCHOOL_MASTER] OFF
SET IDENTITY_INSERT [dbo].[STD_MASTER] ON 

INSERT [dbo].[STD_MASTER] ([std_id], [branch_id], [standard], [row_sta_cd], [trans_id]) VALUES (2, 13, N'10', 1, 56)
SET IDENTITY_INSERT [dbo].[STD_MASTER] OFF
SET IDENTITY_INSERT [dbo].[STUDENT_MAINT] ON 

INSERT [dbo].[STUDENT_MAINT] ([parent_id], [student_id], [parent_name], [father_occupation], [mother_occupation], [contact_no]) VALUES (1, 17, N'efe', N'Bank', N'Doctor', N'8878646655')
SET IDENTITY_INSERT [dbo].[STUDENT_MAINT] OFF
SET IDENTITY_INSERT [dbo].[STUDENT_MASTER] ON 

INSERT [dbo].[STUDENT_MASTER] ([student_id], [gr_no], [first_name], [middle_name], [last_name], [dob], [address], [branch_id], [std_id], [school_id], [school_time], [batch_time], [last_yr_result], [grade], [last_yr_class_name], [contact_no], [row_sta_cd], [trans_id], [stud_img]) VALUES (17, N'1', N'Uri', N'', N'Patel', CAST(0x0000ACB900000000 AS DateTime), N'Test', 13, 1, 1, 1, 1, 72, N'A', N'Parul Universirty', N'9898200754', 1, 53, 0x)
SET IDENTITY_INSERT [dbo].[STUDENT_MASTER] OFF
SET IDENTITY_INSERT [dbo].[SUBJECT_MASTER] ON 

INSERT [dbo].[SUBJECT_MASTER] ([subject_id], [subject], [branch_id], [row_sta_cd], [trans_id]) VALUES (2, N'Test', 13, 1, 55)
SET IDENTITY_INSERT [dbo].[SUBJECT_MASTER] OFF
SET IDENTITY_INSERT [dbo].[TRANSACTION_MASTER] ON 

INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (51, CAST(0x0000ACFB0102D93E AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (52, CAST(0x0000ACFB010985FD AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (53, CAST(0x0000ACFB0109CFC2 AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (54, CAST(0x0000ACFB010A4D97 AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (55, CAST(0x0000ACFB010A5514 AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (56, CAST(0x0000ACFB010A60A6 AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (57, CAST(0x0000ACFB010A9028 AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (58, CAST(0x0000ACFB010AA8F9 AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (59, CAST(0x0000ACFB010AB3E7 AS DateTime), N'Heet', 0, NULL, N'Heet', NULL)
INSERT [dbo].[TRANSACTION_MASTER] ([trans_id], [created_dt], [created_by], [created_id], [last_mod_dt], [last_mod_by], [last_mod_id]) VALUES (60, CAST(0x0000ACFB010E35AA AS DateTime), N'Heet', 0, CAST(0x0000ACFB010F1B63 AS DateTime), N'Heet', 0)
SET IDENTITY_INSERT [dbo].[TRANSACTION_MASTER] OFF
INSERT [dbo].[TYPE_DESC] ([class], [type], [short_desc], [long_desc], [row_sta_cd]) VALUES (1, 0, N'Batch Time', NULL, N'a')
INSERT [dbo].[TYPE_DESC] ([class], [type], [short_desc], [long_desc], [row_sta_cd]) VALUES (1, 1, N'Morning', NULL, N'a')
INSERT [dbo].[TYPE_DESC] ([class], [type], [short_desc], [long_desc], [row_sta_cd]) VALUES (1, 2, N'Afternoon', NULL, N'a')
INSERT [dbo].[TYPE_DESC] ([class], [type], [short_desc], [long_desc], [row_sta_cd]) VALUES (1, 3, N'Evening', NULL, N'a')
SET IDENTITY_INSERT [dbo].[USER_DEF] ON 

INSERT [dbo].[USER_DEF] ([user_id], [username], [password], [row_sta_cd], [trans_id], [staff_id], [branch_id], [user_type], [student_id], [parent_id]) VALUES (1, N'Test', N'1234', 1, 60, NULL, 13, 3, 17, 1)
SET IDENTITY_INSERT [dbo].[USER_DEF] OFF
ALTER TABLE [dbo].[TRANSACTION_MASTER] ADD  CONSTRAINT [DF_TRANSACTION_MASTER_created_dt]  DEFAULT (getdate()) FOR [created_dt]
GO
ALTER TABLE [dbo].[TRANSACTION_MASTER] ADD  CONSTRAINT [DF_TRANSACTION_MASTER_created_by]  DEFAULT (user_name()) FOR [created_by]
GO
ALTER TABLE [dbo].[TRANSACTION_MASTER] ADD  CONSTRAINT [DF_TRANSACTION_MASTER_last_mod_by]  DEFAULT (user_name()) FOR [last_mod_by]
GO
ALTER TABLE [dbo].[TYPE_DESC] ADD  CONSTRAINT [DF_TYPE_DESC_row_sta_cd]  DEFAULT ('I') FOR [row_sta_cd]
GO
ALTER TABLE [dbo].[BATCH_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_BATCH_MASTER_BRANCH_MASTER] FOREIGN KEY([branch_id])
REFERENCES [dbo].[BRANCH_MASTER] ([branch_id])
GO
ALTER TABLE [dbo].[BATCH_MASTER] CHECK CONSTRAINT [FK_BATCH_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BATCH_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_BATCH_MASTER_STD_MASTER] FOREIGN KEY([std_id])
REFERENCES [dbo].[STD_MASTER] ([std_id])
GO
ALTER TABLE [dbo].[BATCH_MASTER] CHECK CONSTRAINT [FK_BATCH_MASTER_STD_MASTER]
GO
ALTER TABLE [dbo].[BATCH_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_BATCH_MASTER_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
GO
ALTER TABLE [dbo].[BATCH_MASTER] CHECK CONSTRAINT [FK_BATCH_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_LICENSE]  WITH CHECK ADD  CONSTRAINT [FK_BRANCH_LICENSE_BRANCH_MASTER] FOREIGN KEY([branch_id])
REFERENCES [dbo].[BRANCH_MASTER] ([branch_id])
GO
ALTER TABLE [dbo].[BRANCH_LICENSE] CHECK CONSTRAINT [FK_BRANCH_LICENSE_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_LICENSE]  WITH CHECK ADD  CONSTRAINT [FK_BRANCH_LICENSE_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
GO
ALTER TABLE [dbo].[BRANCH_LICENSE] CHECK CONSTRAINT [FK_BRANCH_LICENSE_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_MAINT]  WITH CHECK ADD  CONSTRAINT [FK_BRANCH_MAINT_BRANCH_MASTER] FOREIGN KEY([branch_id])
REFERENCES [dbo].[BRANCH_MASTER] ([branch_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BRANCH_MAINT] CHECK CONSTRAINT [FK_BRANCH_MAINT_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_BRANCH_MASTER_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BRANCH_MASTER] CHECK CONSTRAINT [FK_BRANCH_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_STAFF]  WITH CHECK ADD  CONSTRAINT [FK_BRANCH_STAFF_BRANCH_MASTER] FOREIGN KEY([branch_id])
REFERENCES [dbo].[BRANCH_MASTER] ([branch_id])
GO
ALTER TABLE [dbo].[BRANCH_STAFF] CHECK CONSTRAINT [FK_BRANCH_STAFF_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[BRANCH_STAFF]  WITH CHECK ADD  CONSTRAINT [FK_BRANCH_STAFF_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
GO
ALTER TABLE [dbo].[BRANCH_STAFF] CHECK CONSTRAINT [FK_BRANCH_STAFF_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[SCHOOL_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_SCHOOL_MASTER_BRANCH_MASTER] FOREIGN KEY([branch_id])
REFERENCES [dbo].[BRANCH_MASTER] ([branch_id])
GO
ALTER TABLE [dbo].[SCHOOL_MASTER] CHECK CONSTRAINT [FK_SCHOOL_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[SCHOOL_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_SCHOOL_MASTER_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
GO
ALTER TABLE [dbo].[SCHOOL_MASTER] CHECK CONSTRAINT [FK_SCHOOL_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[STD_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_STD_MASTER_BRANCH_MASTER] FOREIGN KEY([branch_id])
REFERENCES [dbo].[BRANCH_MASTER] ([branch_id])
GO
ALTER TABLE [dbo].[STD_MASTER] CHECK CONSTRAINT [FK_STD_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[STD_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_STD_MASTER_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
GO
ALTER TABLE [dbo].[STD_MASTER] CHECK CONSTRAINT [FK_STD_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[STUDENT_MAINT]  WITH CHECK ADD  CONSTRAINT [FK_STUDENT_MAINT_STUDENT_MASTER] FOREIGN KEY([student_id])
REFERENCES [dbo].[STUDENT_MASTER] ([student_id])
GO
ALTER TABLE [dbo].[STUDENT_MAINT] CHECK CONSTRAINT [FK_STUDENT_MAINT_STUDENT_MASTER]
GO
ALTER TABLE [dbo].[SUBJECT_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_SUBJECT_MASTER_BRANCH_MASTER] FOREIGN KEY([branch_id])
REFERENCES [dbo].[BRANCH_MASTER] ([branch_id])
GO
ALTER TABLE [dbo].[SUBJECT_MASTER] CHECK CONSTRAINT [FK_SUBJECT_MASTER_BRANCH_MASTER]
GO
ALTER TABLE [dbo].[SUBJECT_MASTER]  WITH CHECK ADD  CONSTRAINT [FK_SUBJECT_MASTER_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
GO
ALTER TABLE [dbo].[SUBJECT_MASTER] CHECK CONSTRAINT [FK_SUBJECT_MASTER_TRANSACTION_MASTER]
GO
ALTER TABLE [dbo].[USER_DEF]  WITH CHECK ADD  CONSTRAINT [FK_USER_DEF_BRANCH_STAFF] FOREIGN KEY([staff_id])
REFERENCES [dbo].[BRANCH_STAFF] ([staff_id])
GO
ALTER TABLE [dbo].[USER_DEF] CHECK CONSTRAINT [FK_USER_DEF_BRANCH_STAFF]
GO
ALTER TABLE [dbo].[USER_DEF]  WITH CHECK ADD  CONSTRAINT [FK_USER_DEF_TRANSACTION_MASTER] FOREIGN KEY([trans_id])
REFERENCES [dbo].[TRANSACTION_MASTER] ([trans_id])
GO
ALTER TABLE [dbo].[USER_DEF] CHECK CONSTRAINT [FK_USER_DEF_TRANSACTION_MASTER]
GO
