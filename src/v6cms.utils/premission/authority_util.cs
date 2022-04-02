using v6cms.models.admin;
using System.Collections.Generic;

namespace v6cms.utils.premission
{
    /// <summary>
    /// 管理员组权限
    /// </summary>
    public static class authority_util
    {
        /// <summary>
        /// 初始化权限列表
        /// </summary>
        /// <returns></returns>
        public static List<authority_role_model> init()
        {
            var self = new List<authority_role_model.authority_model>();
            self.Add(new authority_role_model.authority_model { key = "edit_my_password/index", name = "修改自己密码" });

            //网站栏目管理
            var column = new List<authority_role_model.authority_model>();
            column.Add(new authority_role_model.authority_model { key = "column/index", name = "网站栏目列表" });
            column.Add(new authority_role_model.authority_model { key = "column/create", name = "添加网站栏目" });
            column.Add(new authority_role_model.authority_model { key = "column/edit", name = "修改网站栏目" });
            column.Add(new authority_role_model.authority_model { key = "column/edit_content", name = "修改栏目内容" });
            column.Add(new authority_role_model.authority_model { key = "column/delete", name = "删除网站栏目" });

            //文章管理
            var article = new List<authority_role_model.authority_model>();
            article.Add(new authority_role_model.authority_model { key = "article/index", name = "文章列表" });
            article.Add(new authority_role_model.authority_model { key = "article/create", name = "添加文章" });
            article.Add(new authority_role_model.authority_model { key = "article/edit", name = "修改文章" });
            article.Add(new authority_role_model.authority_model { key = "article/delete", name = "删除文章" });
            article.Add(new authority_role_model.authority_model { key = "article/review", name = "审核文章" });

            //评论管理
            var comment = new List<authority_role_model.authority_model>();
            comment.Add(new authority_role_model.authority_model { key = "comment/index", name = "评论列表" });
            comment.Add(new authority_role_model.authority_model { key = "comment/edit", name = "修改评论" });
            comment.Add(new authority_role_model.authority_model { key = "comment/delete", name = "删除评论" });

            //生成html
            var create_html = new List<authority_role_model.authority_model>();
            create_html.Add(new authority_role_model.authority_model { key = "create_html/article", name = "生成文章html" });

            //链接分类管理
            var link_category = new List<authority_role_model.authority_model>();
            link_category.Add(new authority_role_model.authority_model { key = "link_category/index", name = "链接分类列表" });
            link_category.Add(new authority_role_model.authority_model { key = "link_category/create", name = "添加链接分类" });
            link_category.Add(new authority_role_model.authority_model { key = "link_category/edit", name = "修改链接分类" });
            link_category.Add(new authority_role_model.authority_model { key = "link_category/delete", name = "删除链接分类" });

            //链接管理
            var link_manager = new List<authority_role_model.authority_model>();
            link_manager.Add(new authority_role_model.authority_model { key = "link_manager/index", name = "链接列表" });
            link_manager.Add(new authority_role_model.authority_model { key = "link_manager/create", name = "添加链接" });
            link_manager.Add(new authority_role_model.authority_model { key = "link_manager/edit", name = "修改链接" });
            link_manager.Add(new authority_role_model.authority_model { key = "link_manager/delete", name = "删除链接" });

            //IP管理
            var ip_address = new List<authority_role_model.authority_model>();
            ip_address.Add(new authority_role_model.authority_model { key = "ip_address/index", name = "IP列表" });
            ip_address.Add(new authority_role_model.authority_model { key = "ip_address/create", name = "添加IP" });
            ip_address.Add(new authority_role_model.authority_model { key = "ip_address/edit", name = "修改IP" });
            ip_address.Add(new authority_role_model.authority_model { key = "ip_address/delete", name = "删除IP" });

            //代码生成管理
            var code_generate = new List<authority_role_model.authority_model>();
            code_generate.Add(new authority_role_model.authority_model { key = "code_generate/index", name = "生成表列表" });
            code_generate.Add(new authority_role_model.authority_model { key = "code_generate/select_table", name = "选择表" });
            code_generate.Add(new authority_role_model.authority_model { key = "code_generate/import_table", name = "导入表" });
            code_generate.Add(new authority_role_model.authority_model { key = "code_generate/edit", name = "修改表" });
            code_generate.Add(new authority_role_model.authority_model { key = "code_generate/delete", name = "删除表" });
            code_generate.Add(new authority_role_model.authority_model { key = "code_generate/generate", name = "生成代码" });

            //会员管理
            var member_manager = new List<authority_role_model.authority_model>();
            member_manager.Add(new authority_role_model.authority_model { key = "member_manager/index", name = "会员列表" });
            member_manager.Add(new authority_role_model.authority_model { key = "member_manager/create", name = "添加会员" });
            member_manager.Add(new authority_role_model.authority_model { key = "member_manager/edit", name = "修改会员" });
            member_manager.Add(new authority_role_model.authority_model { key = "member_manager/delete", name = "删除会员" });

            //问答管理
            var ask = new List<authority_role_model.authority_model>();
            ask.Add(new authority_role_model.authority_model { key = "ask/index", name = "问答列表" });
            ask.Add(new authority_role_model.authority_model { key = "ask/create", name = "添加问答" });
            ask.Add(new authority_role_model.authority_model { key = "ask/edit", name = "修改问答" });
            ask.Add(new authority_role_model.authority_model { key = "ask/delete", name = "删除问答" });

            //问答回复管理
            var ask_reply = new List<authority_role_model.authority_model>();
            ask_reply.Add(new authority_role_model.authority_model { key = "ask_reply/index", name = "问答回复列表" });
            ask_reply.Add(new authority_role_model.authority_model { key = "ask_reply/create", name = "添加问答回复" });
            ask_reply.Add(new authority_role_model.authority_model { key = "ask_reply/edit", name = "修改问答回复" });
            ask_reply.Add(new authority_role_model.authority_model { key = "ask_reply/delete", name = "删除问答回复" });

            //用户管理
            var user_manager = new List<authority_role_model.authority_model>();
            user_manager.Add(new authority_role_model.authority_model { key = "user_manager/index", name = "用户列表" });
            user_manager.Add(new authority_role_model.authority_model { key = "user_manager/create", name = "添加用户" });
            user_manager.Add(new authority_role_model.authority_model { key = "user_manager/edit", name = "修改用户" });
            user_manager.Add(new authority_role_model.authority_model { key = "user_manager/delete", name = "删除用户" });
            user_manager.Add(new authority_role_model.authority_model { key = "user_manager/edit_password", name = "修改用户密码" });
            user_manager.Add(new authority_role_model.authority_model { key = "user_manager/unlock", name = "解锁用户" });

            //角色管理
            var user_role = new List<authority_role_model.authority_model>();
            user_role.Add(new authority_role_model.authority_model { key = "user_role/index", name = "角色列表" });
            user_role.Add(new authority_role_model.authority_model { key = "user_role/create", name = "添加角色" });
            user_role.Add(new authority_role_model.authority_model { key = "user_role/edit", name = "修改角色" });
            user_role.Add(new authority_role_model.authority_model { key = "user_role/delete", name = "删除角色" });
            user_role.Add(new authority_role_model.authority_model { key = "user_role/set_permissions", name = "设置角色权限" });

            //部门管理
            var dept_role = new List<authority_role_model.authority_model>();
            dept_role.Add(new authority_role_model.authority_model { key = "dept/index", name = "部门列表" });
            dept_role.Add(new authority_role_model.authority_model { key = "dept/create", name = "添加部门" });
            dept_role.Add(new authority_role_model.authority_model { key = "dept/edit", name = "修改部门" });
            dept_role.Add(new authority_role_model.authority_model { key = "dept/delete", name = "删除部门" });
            dept_role.Add(new authority_role_model.authority_model { key = "dept/column_permissions", name = "设置栏目权限" });

            //广告管理
            var advertisement = new List<authority_role_model.authority_model>();
            advertisement.Add(new authority_role_model.authority_model { key = "advertisement/index", name = "广告列表" });
            advertisement.Add(new authority_role_model.authority_model { key = "advertisement/create", name = "添加广告" });
            advertisement.Add(new authority_role_model.authority_model { key = "advertisement/edit", name = "修改广告" });
            advertisement.Add(new authority_role_model.authority_model { key = "advertisement/delete", name = "删除广告" });

            //领导信箱管理
            var leader_mailbox = new List<authority_role_model.authority_model>();
            leader_mailbox.Add(new authority_role_model.authority_model { key = "leader_mailbox/index", name = "领导信箱列表" });
            leader_mailbox.Add(new authority_role_model.authority_model { key = "leader_mailbox/create", name = "添加领导信箱" });
            leader_mailbox.Add(new authority_role_model.authority_model { key = "leader_mailbox/edit", name = "修改领导信箱" });
            leader_mailbox.Add(new authority_role_model.authority_model { key = "leader_mailbox/delete", name = "删除领导信箱" });
            leader_mailbox.Add(new authority_role_model.authority_model { key = "leader_mailbox/reply", name = "回复领导信箱" });

            //值班表配置管理
            var duty_config = new List<authority_role_model.authority_model>();
            duty_config.Add(new authority_role_model.authority_model { key = "duty_config/index", name = "值班表配置列表" });
            duty_config.Add(new authority_role_model.authority_model { key = "duty_config/create", name = "添加值班表配置" });
            duty_config.Add(new authority_role_model.authority_model { key = "duty_config/edit", name = "修改值班表配置" });
            duty_config.Add(new authority_role_model.authority_model { key = "duty_config/delete", name = "删除值班表配置" });

            //值班表管理
            var duty_manager = new List<authority_role_model.authority_model>();
            duty_manager.Add(new authority_role_model.authority_model { key = "duty_manager/index", name = "值班列表" });
            duty_manager.Add(new authority_role_model.authority_model { key = "duty_manager/create", name = "添加值班表" });
            duty_manager.Add(new authority_role_model.authority_model { key = "duty_manager/edit", name = "修改值班表" });
            duty_manager.Add(new authority_role_model.authority_model { key = "duty_manager/delete", name = "删除值班表" });
            duty_manager.Add(new authority_role_model.authority_model { key = "duty_manager/import", name = "导入值班表" });

            //生日名单管理
            var birthday_list = new List<authority_role_model.authority_model>();
            birthday_list.Add(new authority_role_model.authority_model { key = "birthday_list/index", name = "生日名单列表" });
            birthday_list.Add(new authority_role_model.authority_model { key = "birthday_list/create", name = "添加生日名单" });
            birthday_list.Add(new authority_role_model.authority_model { key = "birthday_list/edit", name = "修改生日名单" });
            birthday_list.Add(new authority_role_model.authority_model { key = "birthday_list/delete", name = "删除生日名单" });

            //缓存管理
            var cache_manager = new List<authority_role_model.authority_model>();
            cache_manager.Add(new authority_role_model.authority_model { key = "cache_manager/index", name = "缓存列表" });
            cache_manager.Add(new authority_role_model.authority_model { key = "cache_manager/delete", name = "删除缓存" });

            //网站配置
            var site_config = new List<authority_role_model.authority_model>();
            site_config.Add(new authority_role_model.authority_model { key = "site_config/index", name = "查看配置" });
            site_config.Add(new authority_role_model.authority_model { key = "site_config/edit", name = "修改配置" });

            //添加权限组
            var group = new List<authority_role_model>();
            group.Add(new authority_role_model { role_name = "自身权限", authority_list = self });
            group.Add(new authority_role_model { role_name = "文章管理", authority_list = article });
            group.Add(new authority_role_model { role_name = "网站栏目", authority_list = column });
            group.Add(new authority_role_model { role_name = "评论管理", authority_list = comment });

            group.Add(new authority_role_model { role_name = "生成html", authority_list = create_html });

            group.Add(new authority_role_model { role_name = "链接分类", authority_list = link_category });
            group.Add(new authority_role_model { role_name = "链接管理", authority_list = link_manager });
            group.Add(new authority_role_model { role_name = "IP管理", authority_list = ip_address });

            group.Add(new authority_role_model { role_name = "代码生成", authority_list = code_generate });

            group.Add(new authority_role_model { role_name = "广告管理", authority_list = advertisement });
            group.Add(new authority_role_model { role_name = "领导信箱", authority_list = leader_mailbox });
            group.Add(new authority_role_model { role_name = "值班表配置", authority_list = duty_config });
            group.Add(new authority_role_model { role_name = "值班表管理", authority_list = duty_manager });
            group.Add(new authority_role_model { role_name = "生日名单管理", authority_list = birthday_list });

            group.Add(new authority_role_model { role_name = "会员管理", authority_list = member_manager });
            group.Add(new authority_role_model { role_name = "问答管理", authority_list = ask });
            group.Add(new authority_role_model { role_name = "问答回复", authority_list = ask_reply });

            group.Add(new authority_role_model { role_name = "用户管理", authority_list = user_manager });
            group.Add(new authority_role_model { role_name = "角色管理", authority_list = user_role });
            group.Add(new authority_role_model { role_name = "部门管理", authority_list = dept_role });

            group.Add(new authority_role_model { role_name = "缓存管理", authority_list = cache_manager });
            group.Add(new authority_role_model { role_name = "网站配置", authority_list = site_config });
            return group;
        }
    }
}
