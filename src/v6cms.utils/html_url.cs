using System;
using v6cms.entities;
using v6cms.entities.enums;

namespace v6cms.utils
{
    public static class html_url
    {
        /// <summary>
        /// 栏目路由
        /// </summary>
        /// <param name="id"></param>
        /// <param name="route_value"></param>
        /// <returns></returns>
        public static string to_column_route(this column_entity column)
        {
            string url = $"/column/article_list/{column.id}";
            if (!string.IsNullOrEmpty(column.route_value))
            {
                url = $"/{column.route_value}";
            }
            return url;
        }

        /// <summary>
        /// 栏目路由
        /// </summary>
        /// <param name="id"></param>
        /// <param name="route_value"></param>
        /// <returns></returns>
        public static string to_route_url(this int id, string route_value)
        {
            string url = $"/column/article_list/{id}";
            if (!string.IsNullOrEmpty(route_value))
            {
                url = $"/{route_value}";
            }
            return url;
        }

        /// <summary>
        /// 文章html路径
        /// </summary>
        /// <param name="id">文章主键id</param>
        /// <returns></returns>
        public static string to_html_url(this article_entity article)
        {
            string url = $"/article/details/{article.article_snow_id}";
            if (!string.IsNullOrEmpty(article.column.html_path_rule) && article.column.list_option != list_option_enum.使用动态页)
            {
                string html_path_rule = article.column.html_path_rule;
                html_path_rule = html_path_rule.Replace("{yyyy}", article.publish_time.ToString("yyyy"));
                html_path_rule = html_path_rule.Replace("{MM}", article.publish_time.ToString("MM"));
                html_path_rule = html_path_rule.Replace("{dd}", article.publish_time.ToString("dd"));
                html_path_rule = html_path_rule.Replace("{id}", article.id.ToString());
                html_path_rule = html_path_rule.Replace("{article_snow_id}", article.article_snow_id.ToString());
                url = $"/{html_path_rule}";
            }
            return url;
        }

        /// <summary>
        /// 文章html路径
        /// </summary>
        /// <param name="id"></param>
        /// <param name="article_snow_id"></param>
        /// <param name="publish_time"></param>
        /// <param name="html_path_rule"></param>
        /// <param name="list_option"></param>
        /// <returns></returns>
        public static string to_html_url(this int id, long article_snow_id, DateTime publish_time, string html_path_rule, list_option_enum list_option)
        {
            string url = $"/article/details/{article_snow_id}";
            if (!string.IsNullOrEmpty(html_path_rule) && list_option != list_option_enum.使用动态页)
            {
                html_path_rule = html_path_rule.Replace("{yyyy}", publish_time.ToString("yyyy"));
                html_path_rule = html_path_rule.Replace("{MM}", publish_time.ToString("MM"));
                html_path_rule = html_path_rule.Replace("{dd}", publish_time.ToString("dd"));
                html_path_rule = html_path_rule.Replace("{id}", id.ToString());
                html_path_rule = html_path_rule.Replace("{article_snow_id}", article_snow_id.ToString());
                url = $"/{html_path_rule}";
            }
            return url;
        }
    }
}
