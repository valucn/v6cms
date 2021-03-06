USE [v6cms]
GO
SET IDENTITY_INSERT [dbo].[sys_dept] ON 

INSERT [dbo].[sys_dept] ([id], [parent_id], [ancestors], [dept_name], [sort_rank], [column_id_list], [in_rank_list]) VALUES (1, 0, NULL, N'部门', 50, NULL, 0)
SET IDENTITY_INSERT [dbo].[sys_dept] OFF
SET IDENTITY_INSERT [dbo].[sys_site_config] ON 

INSERT [dbo].[sys_site_config] ([id], [site_name], [keywords], [description], [site_url], [site_color], [copyright], [icp], [count_code], [comment_top_days]) VALUES (1, N'v6cms', N'', N'', N'https://v6cms.cn', N'colour', N'', N'京ICP备20016143号-3', NULL, 15)
SET IDENTITY_INSERT [dbo].[sys_site_config] OFF
SET IDENTITY_INSERT [dbo].[sys_user] ON 

INSERT [dbo].[sys_user] ([id], [role_id], [dept_id], [username], [password], [real_name], [avatar], [post], [intro], [mobile], [card_id], [date_of_birth], [is_lock], [is_need_edit_password], [is_leader_mailbox], [sort_rank], [create_time]) VALUES (11, 1, 0, N'admin', N'A0599378870B90517A1B38552270E38AEE74C251', N'管理员', NULL, N'支队', NULL, N'18951616266', NULL, CAST(N'1983-03-11' AS Date), 0, 0, 0, 50, CAST(N'2022-02-26 12:14:37.533' AS DateTime))
SET IDENTITY_INSERT [dbo].[sys_user] OFF
SET IDENTITY_INSERT [dbo].[sys_user_role] ON 

INSERT [dbo].[sys_user_role] ([id], [role_name], [authority_code_list], [level], [data_range], [is_need_review]) VALUES (1, N'超级管理员', N'edit_my_password/index,article/index,article/create,article/edit,article/delete,article/review,column/index,column/create,column/edit,column/edit_content,column/delete,comment/index,comment/edit,comment/delete,create_html/article,link_category/index,link_category/create,link_category/edit,link_category/delete,link_manager/index,link_manager/create,link_manager/edit,link_manager/delete,ip_address/index,ip_address/create,ip_address/edit,ip_address/delete,code_generate/index,code_generate/select_table,code_generate/import_table,code_generate/edit,code_generate/delete,code_generate/generate,advertisement/index,advertisement/create,advertisement/edit,advertisement/delete,leader_mailbox/index,leader_mailbox/create,leader_mailbox/edit,leader_mailbox/delete,leader_mailbox/reply,duty_config/index,duty_config/create,duty_config/edit,duty_config/delete,duty_manager/index,duty_manager/create,duty_manager/edit,duty_manager/delete,duty_manager/import,birthday_list/index,birthday_list/create,birthday_list/edit,birthday_list/delete,member_manager/index,member_manager/create,member_manager/edit,member_manager/delete,ask/index,ask/create,ask/edit,ask/delete,ask_reply/index,ask_reply/create,ask_reply/edit,ask_reply/delete,user_manager/index,user_manager/create,user_manager/edit,user_manager/delete,user_manager/edit_password,user_manager/unlock,user_role/index,user_role/create,user_role/edit,user_role/delete,user_role/set_permissions,dept/index,dept/create,dept/edit,dept/delete,dept/column_permissions,cache_manager/index,cache_manager/delete,site_config/index,site_config/edit', 0, 1, 0)
INSERT [dbo].[sys_user_role] ([id], [role_name], [authority_code_list], [level], [data_range], [is_need_review]) VALUES (2, N'管理员', N'edit_my_password/index,article/index,article/create,article/edit,article/delete,article/review,column/index,column/create,column/edit,column/edit_content,column/delete,link_category/index,link_category/create,link_category/edit,link_category/delete,link/index,link/create,link/edit,link/delete,ip_address/index,ip_address/create,ip_address/edit,ip_address/delete,user/index,user/create,user/edit,user/delete,user/edit_password,user/unlock,user_role/index,user_role/create,user_role/edit,user_role/set_permissions,dept/index,dept/create,dept/edit,dept/delete,dept/column_permissions,advertisement/index,advertisement/create,advertisement/edit,advertisement/delete,cache_manager/index,cache_manager/delete,leader_mailbox/index,leader_mailbox/create,leader_mailbox/edit,leader_mailbox/delete,duty_config/index,duty_config/create,duty_config/edit,duty_config/delete,duty/index,duty/create,duty/edit,duty/delete,duty/import', 0, 1, 0)
INSERT [dbo].[sys_user_role] ([id], [role_name], [authority_code_list], [level], [data_range], [is_need_review]) VALUES (11, N'编辑人员', N'edit_my_password/index,article/index,article/create,article/edit,column/index,duty_manager/index,duty_manager/create,duty_manager/edit,duty_manager/delete,duty_manager/import', 0, 5, 1)
INSERT [dbo].[sys_user_role] ([id], [role_name], [authority_code_list], [level], [data_range], [is_need_review]) VALUES (13, N'测试', N'edit_my_password/index', 0, 1, 0)
SET IDENTITY_INSERT [dbo].[sys_user_role] OFF
SET IDENTITY_INSERT [dbo].[T_duty_config] ON 

INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (1, N'column_B', N'带班领导', 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (2, N'column_C', N'科队领导', 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (3, N'column_D', N'值班民警', 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (4, N'column_E', N'举报中心', 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (5, N'column_F', N'驾驶员', 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (6, N'column_G', N'值班室', 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (7, N'column_H', N'指挥法制应急', 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (8, N'column_I', NULL, 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (9, N'column_J', NULL, 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (10, N'column_K', NULL, 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (11, N'column_L', NULL, 0)
INSERT [dbo].[T_duty_config] ([id], [column_no], [display_name], [is_show]) VALUES (12, N'column_M', NULL, 0)
SET IDENTITY_INSERT [dbo].[T_duty_config] OFF
