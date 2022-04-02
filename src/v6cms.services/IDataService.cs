using v6cms.entities;
using v6cms.entities.enums;
using v6cms.entities.sys;
using v6cms.models;
using Webdiyer.AspNetCore;

namespace v6cms.services
{
    /// <summary>
    /// 连接服务
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="advertisement_id">广告id</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间秒</param>
        /// <returns></returns>
        T get_value<T>(int advertisement_id, string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取部门统计
        /// </summary>
        /// <param name="column_id_list">统计栏目id集合</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间秒</param>
        /// <returns></returns>
        List<dept_count_model> get_dept_count(int[] column_id_list = null, string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取广告内容
        /// </summary>
        /// <param name="ad_id">广告id</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间秒</param>
        /// <returns></returns>
        advertisement_entity get_ad_details(int ad_id, string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="ad_type">广告类型</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<advertisement_entity> get_ad_list(ad_type_enum ad_type, string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取值班表
        /// </summary>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        string get_today_duty_list(string cache_name = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取栏目导航
        /// </summary>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<column_entity> get_column_nav(string cache_name = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <param name="ids">栏目id集合</param>
        /// <param name="is_recommend">是否推荐</param>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<column_entity> get_column_list(int[] ids, bool? is_recommend, string cache_name = "", int cache_seconds = 60000);

        /// <summary>
        /// 获取子栏目列表
        /// </summary>
        /// <param name="parent_id">父栏目id</param>
        /// <param name="is_recommend">是否推荐</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<column_entity> get_column_child_list(int parent_id, bool? is_recommend = null, string cache_key = "", int cache_seconds = 60000);

        /// <summary>
        /// 获取栏目详情
        /// </summary>
        /// <param name="column_id">栏目id</param>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        column_entity get_column_details(int column_id, string cache_name = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="id">文章主键id</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        article_entity get_article_details(int id, string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="article_snow_id">文章id雪花算法</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        article_entity get_article_details(long article_snow_id, string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="column_id">栏目id</param>
        /// <param name="is_slide">是否幻灯</param>
        /// <param name="is_top">是否置顶</param>
        /// <param name="is_best">是否精华</param>
        /// <param name="is_recommend">是否推荐</param>
        /// <param name="is_hot">是否热门</param>
        /// <param name="is_pic">是否图片</param>
        /// <param name="skip">跳过几条</param>
        /// <param name="take">调用几条</param>
        /// <param name="orderby_field">排序字段</param>
        /// <param name="orderby">排序</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<article_entity> get_article_list(int column_id, bool? is_slide = null, bool? is_top = null, bool? is_best = null,
            bool? is_recommend = null, bool? is_hot = null, bool? is_pic = null, int? skip = null, int take = 10, string orderby_field = "publish_time",
            string orderby = "desc", string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="column_ids">栏目id集合</param>
        /// <param name="is_slide">是否幻灯</param>
        /// <param name="is_top">是否置顶</param>
        /// <param name="is_best">是否精华</param>
        /// <param name="is_recommend">是否推荐</param>
        /// <param name="is_hot">是否热门</param>
        /// <param name="is_pic">是否图片</param>
        /// <param name="skip">跳过几条</param>
        /// <param name="take">调用几条</param>
        /// <param name="orderby_field">排序字段</param>
        /// <param name="orderby">排序</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<article_entity> get_article_list(int[] column_ids, bool? is_slide = null, bool? is_top = null, bool? is_best = null,
            bool? is_recommend = null, bool? is_hot = null, bool? is_pic = null, int? skip = null, int take = 10, string orderby_field = "publish_time",
            string orderby = "desc", string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取文章列表并分页
        /// </summary>
        /// <param name="column_id"></param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        PagedList<article_entity> get_article_page(int column_id, int page = 1, int page_size = 15, string orderby_field = "publish_time", string orderby = "desc");

        /// <summary>
        /// 搜索文章分页
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        PagedList<article_entity> search_article_page(string keyword, int page = 1, int page_size = 15, string orderby_field = "publish_time", string orderby = "desc");

        /// <summary>
        /// 获取图片文章列表
        /// </summary>
        /// <param name="take">调用几条</param>
        /// <param name="cache_name"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        List<article_entity> get_pic_article_list(int take, string cache_name = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取图片文章列表
        /// </summary>
        /// <param name="column_id">栏目id</param>
        /// <param name="take">调用几条</param>
        /// <param name="cache_key"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        List<article_entity> get_pic_article_list(int column_id, int take, string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="article_id">文章id</param>
        /// <param name="module">模块：文章=article, 问答=ask</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<comment_entity> get_comment_list(int article_id, string module = "article", string cache_key = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取ip地址列表
        /// </summary>
        /// <param name="ip_type"></param>
        /// <param name="cache_name"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        List<ip_address_entity> get_ip_address(ip_type_enum ip_type, string cache_name = "", int cache_seconds = 6000);

        /// <summary>
        /// 获取当天过生日的用户列表
        /// </summary>
        /// <param name="cache_key"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        List<birthday_list_entity> get_today_birthday(string cache_key = "today_birthday_cache", int cache_seconds = 60000);

        /// <summary>
        /// 获取链接分类列表
        /// </summary>
        /// <param name="ids">分类id集合</param>
        /// <param name="take">调用多少条</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<link_category_entity> get_link_category_list(int[] ids, int take = 10, string cache_key = "", int cache_seconds = 6000000);

        /// <summary>
        /// 获取友情链接列表
        /// </summary>
        /// <param name="category_id">分类id</param>
        /// <param name="take">调用多少条</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        List<link_entity> get_link_list(int category_id, int take = 200, string cache_key = "", int cache_seconds = 6000000);

        /// <summary>
        /// 获取网站配置
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="cache_name"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        site_config_entity get_site_config(string cache_name = "", int cache_seconds = 60000);
    }
}