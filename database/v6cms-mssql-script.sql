USE [v6cms]
GO
/****** Object:  FullTextCatalog [article_content_nohtml]    Script Date: 2022/4/2 17:27:30 ******/
CREATE FULLTEXT CATALOG [article_content_nohtml]WITH ACCENT_SENSITIVITY = ON

GO
/****** Object:  Table [dbo].[sys_code_generate]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_code_generate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[table_name] [nvarchar](50) NULL,
	[table_desc] [nvarchar](50) NULL,
	[model_name] [nvarchar](50) NULL,
	[business_name] [nvarchar](50) NULL,
	[function_name] [nvarchar](50) NULL,
	[remark] [nvarchar](140) NULL,
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_code_generate_create_time]  DEFAULT (getdate()),
	[update_time] [datetime] NULL,
 CONSTRAINT [PK_T_code_generate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sys_dept]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_dept](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent_id] [int] NOT NULL CONSTRAINT [DF_T_dept_parent_id]  DEFAULT ((0)),
	[ancestors] [nvarchar](max) NULL,
	[dept_name] [nvarchar](20) NULL,
	[sort_rank] [int] NOT NULL CONSTRAINT [DF_T_dept_sort_rank]  DEFAULT ((50)),
	[column_id_list] [nvarchar](max) NULL,
	[in_rank_list] [bit] NOT NULL CONSTRAINT [DF_sys_dept_in_rank_list]  DEFAULT ((0)),
 CONSTRAINT [PK_T_dept] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sys_site_config]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_site_config](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[site_name] [nvarchar](50) NULL,
	[keywords] [nvarchar](150) NULL,
	[description] [nvarchar](150) NULL,
	[site_url] [nvarchar](50) NULL,
	[site_color] [nvarchar](20) NULL,
	[copyright] [nvarchar](max) NULL,
	[icp] [nvarchar](20) NULL,
	[count_code] [nvarchar](max) NULL,
	[comment_top_days] [int] NOT NULL CONSTRAINT [DF_sys_site_config_comment_top_days]  DEFAULT ((15)),
 CONSTRAINT [PK_T_sys_config] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sys_table_column]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_table_column](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code_generate_id] [int] NOT NULL CONSTRAINT [DF_T_table_column_code_generate_id]  DEFAULT ((0)),
	[column_name] [nvarchar](50) NULL,
	[column_desc] [nvarchar](50) NULL,
	[column_display] [nvarchar](50) NULL,
	[data_type] [nvarchar](50) NULL,
	[dotnet_type] [nvarchar](50) NULL,
	[data_length] [int] NOT NULL CONSTRAINT [DF_T_table_column_data_length]  DEFAULT ((0)),
	[scale] [int] NOT NULL CONSTRAINT [DF_T_table_column_data_length1]  DEFAULT ((0)),
	[is_identity] [bit] NOT NULL CONSTRAINT [DF_T_table_column_scale1]  DEFAULT ((0)),
	[is_pk] [bit] NOT NULL CONSTRAINT [DF_T_table_column_is_identity1]  DEFAULT ((0)),
	[is_nullable] [bit] NOT NULL CONSTRAINT [DF_T_table_column_is_pk1]  DEFAULT ((0)),
	[default_value] [nvarchar](50) NOT NULL,
	[update_time] [datetime] NULL,
 CONSTRAINT [PK_T_table_column] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sys_user]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role_id] [int] NOT NULL CONSTRAINT [DF_T_user_role_id]  DEFAULT ((0)),
	[dept_id] [int] NULL CONSTRAINT [DF_sys_user_dept_id]  DEFAULT ((0)),
	[username] [nvarchar](20) NULL,
	[password] [nvarchar](40) NULL,
	[real_name] [nvarchar](20) NULL,
	[avatar] [nvarchar](200) NULL,
	[post] [nvarchar](20) NULL,
	[intro] [nvarchar](max) NULL,
	[mobile] [nvarchar](11) NULL,
	[card_id] [nvarchar](18) NULL,
	[date_of_birth] [date] NULL,
	[is_lock] [bit] NOT NULL CONSTRAINT [DF_T_user_is_lock]  DEFAULT ((0)),
	[is_need_edit_password] [bit] NOT NULL CONSTRAINT [DF_sys_user_need_edit_password]  DEFAULT ((0)),
	[is_leader_mailbox] [bit] NOT NULL CONSTRAINT [DF_sys_user_is_leader_mailbox]  DEFAULT ((0)),
	[sort_rank] [int] NOT NULL CONSTRAINT [DF_sys_user_sort_rank]  DEFAULT ((50)),
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_user_create_time]  DEFAULT (getdate()),
 CONSTRAINT [PK_T_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sys_user_log]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user_log](
	[id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[function_name] [nvarchar](20) NULL,
	[request_path] [nvarchar](200) NULL,
	[log_content] [nvarchar](140) NULL,
	[result] [bit] NOT NULL,
	[ip] [nvarchar](50) NULL,
	[create_time] [datetime] NOT NULL,
 CONSTRAINT [PK_sys_user_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sys_user_role]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user_role](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [nvarchar](20) NULL,
	[authority_code_list] [nvarchar](max) NULL,
	[level] [int] NOT NULL CONSTRAINT [DF_sys_user_role_level]  DEFAULT ((0)),
	[data_range] [int] NOT NULL CONSTRAINT [DF_sys_user_role_data_range]  DEFAULT ((0)),
	[is_need_review] [bit] NOT NULL CONSTRAINT [DF_sys_user_role_is_need_review]  DEFAULT ((0)),
 CONSTRAINT [PK_T_user_role] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_advertisement]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_advertisement](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ad_name] [nvarchar](20) NULL,
	[ad_type] [int] NOT NULL CONSTRAINT [DF_T_advertisement_ad_type]  DEFAULT ((0)),
	[view_time_limit] [int] NOT NULL CONSTRAINT [DF_T_advertisement_ad_view]  DEFAULT ((0)),
	[text] [nvarchar](50) NULL,
	[url] [nvarchar](200) NULL,
	[pic] [nvarchar](200) NULL,
	[end_time] [datetime] NULL,
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_advertisement_create_time]  DEFAULT (getdate()),
 CONSTRAINT [PK_T_advertisement] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_advertisement_pic_list]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_advertisement_pic_list](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ad_id] [int] NOT NULL,
	[pic] [nvarchar](200) NULL,
 CONSTRAINT [PK_T_advertisement_pic_list] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_article]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_article](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[article_snow_id] [bigint] NOT NULL CONSTRAINT [DF_T_article_article_snow_id]  DEFAULT ((0)),
	[user_id] [int] NOT NULL CONSTRAINT [DF_T_article_user_id]  DEFAULT ((0)),
	[dept_id] [int] NOT NULL CONSTRAINT [DF_T_article_dept_id]  DEFAULT ((0)),
	[route_value] [nvarchar](20) NULL,
	[column_id] [int] NOT NULL CONSTRAINT [DF_T_article_column_id]  DEFAULT ((0)),
	[sub_column] [nvarchar](max) NULL,
	[title] [nvarchar](140) NULL,
	[author] [nvarchar](30) NULL,
	[source] [nvarchar](30) NULL,
	[summary] [nvarchar](140) NULL,
	[content_nohtml] [nvarchar](max) NULL,
	[details_view_path] [nvarchar](50) NULL,
	[views] [int] NOT NULL CONSTRAINT [DF_T_article_views]  DEFAULT ((0)),
	[is_review] [bit] NOT NULL CONSTRAINT [DF_T_article_is_need_examine]  DEFAULT ((1)),
	[is_slide] [bit] NOT NULL CONSTRAINT [DF_T_article_is_pic1]  DEFAULT ((0)),
	[is_top] [bit] NOT NULL CONSTRAINT [DF_T_article_is_top]  DEFAULT ((0)),
	[is_best] [bit] NOT NULL CONSTRAINT [DF_T_article_is_best]  DEFAULT ((0)),
	[is_recommend] [bit] NOT NULL CONSTRAINT [DF_T_article_is_recommend]  DEFAULT ((0)),
	[is_sr] [bit] NOT NULL CONSTRAINT [DF_T_article_is_recommend1]  DEFAULT ((0)),
	[is_hot] [bit] NOT NULL CONSTRAINT [DF_T_article_is_hot]  DEFAULT ((0)),
	[is_pic] [bit] NULL CONSTRAINT [DF_T_article_is_pic_news]  DEFAULT ((0)),
	[pic] [nvarchar](128) NULL,
	[video] [nvarchar](128) NULL,
	[is_limit_ip] [bit] NOT NULL CONSTRAINT [DF_T_article_is_limit_ip]  DEFAULT ((0)),
	[use_gab] [bit] NOT NULL CONSTRAINT [DF_T_article_use_gab]  DEFAULT ((0)),
	[use_province] [bit] NOT NULL CONSTRAINT [DF_T_article_use_province]  DEFAULT ((0)),
	[use_city] [bit] NOT NULL CONSTRAINT [DF_T_article_use_city]  DEFAULT ((0)),
	[use_branch] [bit] NOT NULL CONSTRAINT [DF_T_article_use_branch]  DEFAULT ((0)),
	[html_template] [nvarchar](50) NULL,
	[html_path] [nvarchar](50) NULL,
	[publish_time] [datetime] NOT NULL CONSTRAINT [DF_T_article_publish_time]  DEFAULT (getdate()),
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_article_create_time]  DEFAULT (getdate()),
	[update_time] [datetime] NULL,
	[comment_time] [datetime] NULL,
 CONSTRAINT [PK_T_article] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_T_article_article_snow_id] UNIQUE NONCLUSTERED 
(
	[article_snow_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_article_content]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_article_content](
	[article_id] [int] NOT NULL,
	[content] [nvarchar](max) NULL,
 CONSTRAINT [PK_T_article_content] PRIMARY KEY CLUSTERED 
(
	[article_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ask]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ask](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[member_id] [int] NOT NULL CONSTRAINT [DF_T_ask_member_id]  DEFAULT ((0)),
	[title] [nvarchar](100) NULL,
	[content] [nvarchar](max) NULL,
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_ask_create_time]  DEFAULT (getdate()),
	[ip] [nvarchar](128) NULL,
	[is_review] [bit] NOT NULL CONSTRAINT [DF_T_ask_is_review]  DEFAULT ((0)),
	[reply_status] [int] NOT NULL CONSTRAINT [DF_T_ask_is_new_reply]  DEFAULT ((0)),
 CONSTRAINT [PK_T_ask] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ask_reply]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ask_reply](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ask_id] [int] NOT NULL CONSTRAINT [DF_T_ask_reply_ask_id]  DEFAULT ((0)),
	[user_id] [int] NOT NULL CONSTRAINT [DF_T_ask_reply_member_id]  DEFAULT ((0)),
	[content] [nvarchar](max) NULL,
	[reply_time] [datetime] NOT NULL,
	[ip] [nvarchar](128) NULL,
 CONSTRAINT [PK_T_ask_reply] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ask_up]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ask_up](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[module] [nvarchar](20) NULL,
	[source_id] [int] NOT NULL CONSTRAINT [DF_T_ask_up_ask_id]  DEFAULT ((0)),
	[member_id] [int] NOT NULL CONSTRAINT [DF_T_ask_up_member_id]  DEFAULT ((0)),
	[connection_id] [nvarchar](50) NULL,
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_ask_up_up_time]  DEFAULT (getdate()),
	[ip] [nvarchar](128) NULL,
 CONSTRAINT [PK_T_ask_up] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_attachment]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_attachment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL CONSTRAINT [DF_T_attachment_user_id]  DEFAULT ((0)),
	[module] [nvarchar](20) NULL,
	[source_id] [int] NOT NULL CONSTRAINT [DF_T_pic_source_id]  DEFAULT ((0)),
	[org_name] [nvarchar](128) NULL,
	[file_type] [nvarchar](10) NULL,
	[file_path] [nvarchar](128) NULL,
	[upload_time] [datetime] NOT NULL CONSTRAINT [DF_T_pic_upload_time]  DEFAULT (getdate()),
 CONSTRAINT [PK_T_pic] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_birthday_list]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_birthday_list](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[real_name] [nvarchar](20) NULL,
	[date_of_birth] [date] NULL,
 CONSTRAINT [PK_T_tirthday_list] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_collector]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_collector](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[intro] [nvarchar](140) NULL,
	[list_rule] [nvarchar](max) NULL,
	[list_node] [nvarchar](100) NULL,
	[item_node] [nvarchar](100) NULL,
	[item_href_rule] [nvarchar](100) NULL,
	[content_title_rule] [nvarchar](100) NULL,
	[content_create_time_rule] [nvarchar](100) NULL,
	[content_rule] [nvarchar](100) NULL,
 CONSTRAINT [PK_T_collector] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_collector_content]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_collector_content](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[collector_id] [int] NOT NULL,
	[url] [nvarchar](200) NULL,
	[title] [nvarchar](200) NULL,
	[create_time] [datetime] NOT NULL,
	[content] [nvarchar](max) NULL,
	[is_publish] [bit] NOT NULL,
 CONSTRAINT [PK_T_collector_content] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_column]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_column](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[route_value] [nvarchar](50) NULL,
	[article_route] [nvarchar](50) NULL,
	[parent_id] [int] NOT NULL CONSTRAINT [DF_T_column_parent_id]  DEFAULT ((0)),
	[level] [int] NOT NULL CONSTRAINT [DF_T_column_level]  DEFAULT ((0)),
	[column_name] [nvarchar](100) NULL,
	[column_name_abbr] [nvarchar](20) NULL,
	[intro] [nvarchar](140) NULL,
	[content] [nvarchar](max) NULL,
	[pic] [nvarchar](100) NULL,
	[list_view_path] [nvarchar](50) NULL,
	[details_view_path] [nvarchar](50) NULL,
	[html_template] [nvarchar](50) NULL,
	[html_path_rule] [nvarchar](100) NULL,
	[list_option] [int] NOT NULL CONSTRAINT [DF_T_column_list_option]  DEFAULT ((0)),
	[column_attribute] [int] NOT NULL CONSTRAINT [DF_T_column_column_attribute]  DEFAULT ((0)),
	[external_link] [nvarchar](200) NULL,
	[target] [nvarchar](20) NULL,
	[is_recommend] [bit] NOT NULL CONSTRAINT [DF_T_column_is_show_nav1]  DEFAULT ((0)),
	[is_show_nav] [bit] NOT NULL CONSTRAINT [DF_T_column_is_show_nav]  DEFAULT ((0)),
	[is_need_review] [bit] NOT NULL CONSTRAINT [DF_T_column_is_need_examine]  DEFAULT ((0)),
	[is_limit_ip] [bit] NOT NULL CONSTRAINT [DF_T_column_is_limit_ip]  DEFAULT ((0)),
	[sort_rank] [int] NOT NULL CONSTRAINT [DF_T_column_sort_rank]  DEFAULT ((50)),
	[score] [decimal](16, 2) NOT NULL CONSTRAINT [DF_T_column_score]  DEFAULT ((0)),
	[score_gab] [decimal](16, 2) NOT NULL CONSTRAINT [DF_T_column_score1]  DEFAULT ((0)),
	[score_province] [decimal](16, 2) NOT NULL CONSTRAINT [DF_T_column_score2]  DEFAULT ((0)),
	[score_city] [decimal](16, 2) NOT NULL CONSTRAINT [DF_T_column_score3]  DEFAULT ((0)),
	[score_branch] [decimal](16, 2) NOT NULL CONSTRAINT [DF_T_column_score31]  DEFAULT ((0)),
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_column_create_time]  DEFAULT (getdate()),
	[update_time] [datetime] NULL,
 CONSTRAINT [PK_T_column] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_comment]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_comment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[module] [nvarchar](20) NULL,
	[user_id] [int] NOT NULL CONSTRAINT [DF_T_comment_column_id1_1]  DEFAULT ((0)),
	[member_id] [int] NOT NULL CONSTRAINT [DF_T_comment_member_id]  DEFAULT ((0)),
	[column_id] [int] NOT NULL CONSTRAINT [DF_T_comment_column_id]  DEFAULT ((0)),
	[source_id] [int] NOT NULL CONSTRAINT [DF_T_comment_column_id1]  DEFAULT ((0)),
	[comment_name] [nvarchar](50) NULL,
	[comment_content] [nvarchar](max) NULL,
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_comment_comment_time]  DEFAULT (getdate()),
	[ip] [nvarchar](128) NULL,
 CONSTRAINT [PK_T_comment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_duty]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_duty](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL,
	[column_B] [nvarchar](20) NULL,
	[column_C] [nvarchar](20) NULL,
	[column_D] [nvarchar](20) NULL,
	[column_E] [nvarchar](20) NULL,
	[column_F] [nvarchar](20) NULL,
	[column_G] [nvarchar](20) NULL,
	[column_H] [nvarchar](20) NULL,
	[column_I] [nvarchar](20) NULL,
	[column_J] [nvarchar](20) NULL,
	[column_K] [nvarchar](20) NULL,
	[column_L] [nvarchar](20) NULL,
	[column_M] [nvarchar](20) NULL,
 CONSTRAINT [PK_T_duty] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_duty_config]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_duty_config](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[column_no] [nvarchar](20) NULL,
	[display_name] [nvarchar](20) NULL,
	[is_show] [bit] NOT NULL CONSTRAINT [DF_T_duty_config_is_show]  DEFAULT ((0)),
 CONSTRAINT [PK_T_duty_config] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ip_address]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ip_address](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ip_start] [nvarchar](32) NULL,
	[ip_end] [nvarchar](32) NULL,
	[ip_type] [int] NOT NULL CONSTRAINT [DF_T_ip_address_ip_type]  DEFAULT ((0)),
	[remark] [nvarchar](50) NULL,
 CONSTRAINT [PK_ip_address] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_leader_mailbox]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_leader_mailbox](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL CONSTRAINT [DF_T_leader_mailbox_user_id]  DEFAULT ((0)),
	[guest_name] [nvarchar](20) NULL,
	[guest_unit] [nvarchar](50) NULL,
	[guest_email] [nvarchar](50) NULL,
	[guest_mobile] [nvarchar](11) NULL,
	[title] [nvarchar](50) NULL,
	[mail_type] [nvarchar](20) NULL,
	[reply_type] [int] NOT NULL CONSTRAINT [DF_T_leader_mailbox_reply_type]  DEFAULT ((0)),
	[content] [nvarchar](max) NULL,
	[ip] [nvarchar](32) NULL,
	[create_time] [datetime] NOT NULL CONSTRAINT [DF_T_leader_mailbox_create_time]  DEFAULT (getdate()),
	[reply_content] [nvarchar](max) NULL,
	[reply_time] [datetime] NULL,
 CONSTRAINT [PK_T_leader_mailbox] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_link]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_link](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_id] [int] NOT NULL CONSTRAINT [DF_link_category_id]  DEFAULT ((0)),
	[title] [nvarchar](128) NULL,
	[logo] [nvarchar](128) NULL,
	[url] [nvarchar](512) NULL,
	[sort_rank] [int] NOT NULL CONSTRAINT [DF_link_sort]  DEFAULT ((50)),
	[font_weight] [nvarchar](10) NULL,
	[font_color] [nvarchar](10) NULL,
 CONSTRAINT [PK_link] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_link_category]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_link_category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [nvarchar](50) NULL,
	[sort_rank] [int] NOT NULL CONSTRAINT [DF_link_category_sort]  DEFAULT ((50)),
 CONSTRAINT [PK_link_category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_member]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_member](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](32) NULL,
	[password] [nvarchar](40) NULL,
	[nick_name] [nvarchar](20) NULL,
	[real_name] [nvarchar](20) NULL,
	[avatar] [nvarchar](200) NULL,
	[company] [nvarchar](100) NULL,
	[mobile] [nvarchar](11) NULL,
	[card_id] [nvarchar](18) NULL,
	[is_lock] [bit] NOT NULL CONSTRAINT [DF_T_member_is_lock]  DEFAULT ((0)),
	[reg_time] [datetime] NOT NULL CONSTRAINT [DF_T_member_reg_time]  DEFAULT (getdate()),
	[reg_ip] [nvarchar](128) NULL,
	[member_level] [int] NOT NULL CONSTRAINT [DF_T_member_member_level]  DEFAULT ((0)),
	[member_question1] [nvarchar](50) NULL,
	[member_answer1] [nvarchar](50) NULL,
	[member_question2] [nvarchar](50) NULL,
	[member_answer2] [nvarchar](50) NULL,
	[member_question3] [nvarchar](50) NULL,
	[member_answer3] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_member] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_site_stat]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_site_stat](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL,
	[ip] [int] NOT NULL CONSTRAINT [DF_T_stat_stat]  DEFAULT ((0)),
	[pv] [int] NOT NULL CONSTRAINT [DF_T_site_stat_pv]  DEFAULT ((0)),
 CONSTRAINT [PK_T_site_stat] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[view_dept_score]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_dept_score]
AS
SELECT  id, parent_id, ancestors, dept_name, sort_rank, column_id_list,
                       (SELECT  ISNULL(SUM(score), 0) AS total_score
                        FROM       (SELECT  (SELECT  score
                                                                FROM      dbo.T_column
                                                                WHERE   (id = dbo.T_article.column_id)) AS score
                                            FROM       dbo.T_article
                                            WHERE    (dept_id = dbo.sys_dept.id)) AS T) AS total_score
FROM      dbo.sys_dept

GO
/****** Object:  Index [IX_T_article_publish_time]    Script Date: 2022/4/2 17:27:30 ******/
CREATE NONCLUSTERED INDEX [IX_T_article_publish_time] ON [dbo].[T_article]
(
	[publish_time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_T_article_title]    Script Date: 2022/4/2 17:27:30 ******/
CREATE NONCLUSTERED INDEX [IX_T_article_title] ON [dbo].[T_article]
(
	[title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  FullTextIndex     Script Date: 2022/4/2 17:27:30 ******/
CREATE FULLTEXT INDEX ON [dbo].[T_article](
[content_nohtml] LANGUAGE 'Simplified Chinese')
KEY INDEX [PK_T_article]ON ([article_content_nohtml], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO
ALTER TABLE [dbo].[sys_user_log] ADD  CONSTRAINT [DF_sys_user_log_user_id]  DEFAULT ((0)) FOR [user_id]
GO
ALTER TABLE [dbo].[sys_user_log] ADD  CONSTRAINT [DF_sys_user_log_result]  DEFAULT ((0)) FOR [result]
GO
ALTER TABLE [dbo].[sys_user_log] ADD  CONSTRAINT [DF_sys_user_log_create_time]  DEFAULT (getdate()) FOR [create_time]
GO
ALTER TABLE [dbo].[T_collector_content] ADD  CONSTRAINT [DF_T_collector_content_collector_id]  DEFAULT ((0)) FOR [collector_id]
GO
ALTER TABLE [dbo].[T_collector_content] ADD  CONSTRAINT [DF_T_collector_content_is_publish]  DEFAULT ((0)) FOR [is_publish]
GO
/****** Object:  StoredProcedure [dbo].[sp_dept_score]    Script Date: 2022/4/2 17:27:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_dept_score]
	-- Add the parameters for the stored procedure here
	@dept_id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SUM(score) AS total_score FROM (
		SELECT (SELECT score FROM T_column WHERE id=T_article.column_id) AS score FROM T_article WHERE dept_id=@dept_id
	) AS T
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'table_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'table_desc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实体类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'model_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务名称(英文)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'business_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能名称(中文)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'function_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate', @level2type=N'COLUMN',@level2name=N'update_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代码生成表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_code_generate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级部门id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_dept', @level2type=N'COLUMN',@level2name=N'parent_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'祖级列表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_dept', @level2type=N'COLUMN',@level2name=N'ancestors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_dept', @level2type=N'COLUMN',@level2name=N'dept_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_dept', @level2type=N'COLUMN',@level2name=N'sort_rank'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有权限栏目id列表集合' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_dept', @level2type=N'COLUMN',@level2name=N'column_id_list'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否参与排名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_dept', @level2type=N'COLUMN',@level2name=N'in_rank_list'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_dept'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'site_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键词' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'keywords'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网页描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站地址，不以/结尾' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'site_url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站颜色：彩色=colour,黑白=gray' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'site_color'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版权内容，支持html' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'copyright'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站备案号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'icp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'统计代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'count_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'领导评论置顶天数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config', @level2type=N'COLUMN',@level2name=N'comment_top_days'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_site_config'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代码生成表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'code_generate_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字段列名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'column_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'column_desc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字段显示名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'column_display'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'.net类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'dotnet_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据长度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'data_length'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小数位数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'scale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'is_identity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'is_pk'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否允许空' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'is_nullable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'default_value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column', @level2type=N'COLUMN',@level2name=N'update_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表字段表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_table_column'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'role_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'dept_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'username'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'real_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'avatar'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'post'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'intro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'card_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出生日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'date_of_birth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'is_lock'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要修改密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'is_need_edit_password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开通领导信箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'is_leader_mailbox'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'sort_rank'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log', @level2type=N'COLUMN',@level2name=N'function_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log', @level2type=N'COLUMN',@level2name=N'request_path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作结果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log', @level2type=N'COLUMN',@level2name=N'result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户IP地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log', @level2type=N'COLUMN',@level2name=N'ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户操作日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_role', @level2type=N'COLUMN',@level2name=N'role_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色权限编码集' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_role', @level2type=N'COLUMN',@level2name=N'authority_code_list'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'级别：越小权限越大' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_role', @level2type=N'COLUMN',@level2name=N'level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据范围：1全部数据权限，2自定数据权限，3本部门数据权限，4本部门及以下数据权限，5仅本人数据权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_role', @level2type=N'COLUMN',@level2name=N'data_range'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布文章是否需要审核' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_role', @level2type=N'COLUMN',@level2name=N'is_need_review'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户角色表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'ad_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告类型：0文字广告，1图片广告，2飘窗广告，3浮动广告，4幻灯广告' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'ad_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示时间限制：0不限制，1到期不显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'view_time_limit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告文字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'text'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'pic'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'end_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement_pic_list', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement_pic_list', @level2type=N'COLUMN',@level2name=N'ad_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement_pic_list', @level2type=N'COLUMN',@level2name=N'pic'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告图片集合表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_advertisement_pic_list'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章雪花算法id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'article_snow_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'dept_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'路由值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'route_value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站栏目id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'column_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'副栏目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'sub_column'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'概要' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'summary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章内容无html代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'content_nohtml'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容页视图路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'details_view_path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'views'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否审核' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_review'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否幻灯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_slide'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否置顶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_top'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否精华文章' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_best'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否推荐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_recommend'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否特别推荐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_sr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否热门' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_hot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否图片文章' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_pic'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'封面图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'pic'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视频地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'video'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否限制ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'is_limit_ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公安部采用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'use_gab'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省厅采用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'use_province'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'市局采用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'use_city'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分局采用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'use_branch'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'html模板路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'html_template'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'html静态页面路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'html_path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'publish_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'update_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article', @level2type=N'COLUMN',@level2name=N'comment_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article_content', @level2type=N'COLUMN',@level2name=N'article_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article_content', @level2type=N'COLUMN',@level2name=N'content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章内容表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_article_content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'答疑id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'member_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提问时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否通过审核' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'is_review'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复状态：未回复=0, 新回复=1, 回复已读=2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask', @level2type=N'COLUMN',@level2name=N'reply_status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'答疑表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'答疑回复id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_reply', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'答疑id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_reply', @level2type=N'COLUMN',@level2name=N'ask_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_reply', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_reply', @level2type=N'COLUMN',@level2name=N'content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_reply', @level2type=N'COLUMN',@level2name=N'reply_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_reply', @level2type=N'COLUMN',@level2name=N'ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'答疑回复表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_reply'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'问答点赞主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块：文章=article, 问答=ask' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up', @level2type=N'COLUMN',@level2name=N'module'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up', @level2type=N'COLUMN',@level2name=N'source_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up', @level2type=N'COLUMN',@level2name=N'member_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游客连接id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up', @level2type=N'COLUMN',@level2name=N'connection_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'点赞时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'点赞IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up', @level2type=N'COLUMN',@level2name=N'ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'问答点赞表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ask_up'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源类型：文章=article, 广告=advertisement' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'module'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'source_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源文件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'org_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'file_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'file_path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment', @level2type=N'COLUMN',@level2name=N'upload_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件资源表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_attachment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_birthday_list', @level2type=N'COLUMN',@level2name=N'real_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_birthday_list', @level2type=N'COLUMN',@level2name=N'date_of_birth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生日名单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_birthday_list'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'采集器名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'intro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列表规则' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'list_rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列表节点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'list_node'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目节点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'item_node'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目链接规则' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'item_href_rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容标题规则' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'content_title_rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间规则' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'content_create_time_rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容规则' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector', @level2type=N'COLUMN',@level2name=N'content_rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'采集器表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'采集器id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector_content', @level2type=N'COLUMN',@level2name=N'collector_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'采集网址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector_content', @level2type=N'COLUMN',@level2name=N'url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector_content', @level2type=N'COLUMN',@level2name=N'title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector_content', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector_content', @level2type=N'COLUMN',@level2name=N'content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否发布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector_content', @level2type=N'COLUMN',@level2name=N'is_publish'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'采集内容表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_collector_content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'路由值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'route_value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章页路由' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'article_route'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父栏目id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'parent_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'栏目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'column_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'栏目名称简称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'column_name_abbr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'intro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'栏目内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'栏目图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'pic'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列表页视图路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'list_view_path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容页视图路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'details_view_path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'html模板' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'html_template'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'html路径规则' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'html_path_rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列表选项：链接到默认页=0, 链接到列表第一页=1, 使用动态页=2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'list_option'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'栏目属性：0最终列表栏目，1频道封面，2外部链接' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'column_attribute'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外部链接' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'external_link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'打开窗口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'target'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否推荐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'is_recommend'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示在导航栏' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'is_show_nav'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布文章是否需要审核' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'is_need_review'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否限制ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'is_limit_ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'sort_rank'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'score'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公安部分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'score_gab'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省厅分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'score_province'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'市局分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'score_city'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分局分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'score_branch'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column', @level2type=N'COLUMN',@level2name=N'update_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站栏目表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_column'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块：文章=article, 问答=ask' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'module'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'member_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'栏目id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'column_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'source_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'comment_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'comment_content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment', @level2type=N'COLUMN',@level2name=N'ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'B列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_B'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'C列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_C'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'D列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_D'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'E列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_E'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'G列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_F'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'G列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_G'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'H列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_H'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'I列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_I'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'J列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_J'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'K列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_K'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'L列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_L'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty', @level2type=N'COLUMN',@level2name=N'column_M'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值班表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty_config', @level2type=N'COLUMN',@level2name=N'column_no'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty_config', @level2type=N'COLUMN',@level2name=N'display_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty_config', @level2type=N'COLUMN',@level2name=N'is_show'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值班表配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_duty_config'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP地址开始' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ip_address', @level2type=N'COLUMN',@level2name=N'ip_start'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP结束' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ip_address', @level2type=N'COLUMN',@level2name=N'ip_end'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP类型：1IP白名单，2IP黑名单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ip_address', @level2type=N'COLUMN',@level2name=N'ip_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ip_address', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ip_address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留言人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'guest_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留言人单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'guest_unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留言人电子邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'guest_email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留言人手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'guest_mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信件类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'mail_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复方式：公开回复=0, 私下回复=1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'reply_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留言人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留言时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'reply_content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox', @level2type=N'COLUMN',@level2name=N'reply_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'领导信箱表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_leader_mailbox'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'category_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'logo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'logo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'sort_rank'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字体粗细' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'font_weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字体颜色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link', @level2type=N'COLUMN',@level2name=N'font_color'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link_category', @level2type=N'COLUMN',@level2name=N'category_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link_category', @level2type=N'COLUMN',@level2name=N'sort_rank'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_link_category'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'username'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码sha1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昵称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'nick_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'real_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'avatar'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'company'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'card_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'is_lock'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'reg_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'reg_ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员级别：注册会员=0, 审核会员=1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'member_level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员问题1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'member_question1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员回答1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'member_answer1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员问题2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'member_question2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员回答2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'member_answer2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员问题2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'member_question3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员回答3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member', @level2type=N'COLUMN',@level2name=N'member_answer3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_member'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_site_stat', @level2type=N'COLUMN',@level2name=N'date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip统计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_site_stat', @level2type=N'COLUMN',@level2name=N'ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'pv' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_site_stat', @level2type=N'COLUMN',@level2name=N'pv'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站访问统计表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_site_stat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "sys_dept"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 246
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_dept_score'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_dept_score'
GO
