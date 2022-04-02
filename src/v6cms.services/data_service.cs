using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.entities.enums;
using v6cms.entities.sys;
using v6cms.models;
using v6cms.utils;
using Webdiyer.AspNetCore;

namespace v6cms.services
{
    public class data_service : IDataService
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public data_service(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="advertisement_id">广告id</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public T get_value<T>(int advertisement_id, string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"advertisement_details_{advertisement_id}_cache";
            }

            var cache_value = _cache.Get<T>(cache_key);
            if (cache_value != null)
            {
                return cache_value;
            }

            var model = _context.advertisement.FirstOrDefault(m => m.id == advertisement_id);
            _cache.Set(cache_key, model, TimeSpan.FromSeconds(cache_seconds));
            return (T)(object)model;
        }

        /// <summary>
        /// 获取部门统计
        /// </summary>
        /// <param name="column_id_list">统计栏目id集合</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<dept_count_model> get_dept_count(int[] column_id_list = null, string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"dept_count_cache";
            }

            var cache_value = _cache.Get<List<dept_count_model>>(cache_key);
            if (cache_value != null)
            {
                return cache_value;
            }

            var model = new List<dept_count_model>();
            var list = _context.dept.Where(m => m.in_rank_list).ToList();
            foreach (var item in list)
            {
                var query = _context.article.Where(m => m.is_review && m.dept_id == item.id).AsQueryable();
                if (column_id_list != null)
                {
                    query = query.Where(m => column_id_list.Contains(m.column_id));
                }
                query = query.Include(m => m.column);
                decimal sum_gab = query.Where(m => m.use_gab).Sum(m => m.column.score_gab);
                decimal sum_province = query.Where(m => m.use_province).Sum(m => m.column.score_province);
                decimal sum_city = query.Where(m => m.use_city).Sum(m => m.column.score_city);
                decimal sum_branch = query.Where(m => m.use_branch).Sum(m => m.column.score_branch);
                model.Add(new dept_count_model
                {
                    dept_name = item.dept_name,
                    score_gab = sum_gab,
                    score_province = sum_province,
                    score_city = sum_city,
                    score_branch = sum_branch,
                    score_total = (sum_gab + sum_province + sum_city + sum_branch)
                });
            }
            model = model.OrderByDescending(m => m.score_total).ToList();
            _cache.Set(cache_key, model, TimeSpan.FromSeconds(cache_seconds));
            return model;
        }

        /// <summary>
        /// 获取广告内容
        /// </summary>
        /// <param name="ad_id">广告id</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public advertisement_entity get_ad_details(int ad_id, string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"advertisement_details_{ad_id}_cache";
            }

            var cache_value = _cache.Get<advertisement_entity>(cache_key);
            if (cache_value != null)
            {
                return cache_value;
            }

            var model = new advertisement_entity();
            var data = _context.advertisement.Where(m => m.id == ad_id).Include(m => m.pic_list).FirstOrDefault();
            if (data != null)
            {
                if (data.view_time_limit == view_time_limit_enum.到期不显示)
                {
                    if (data.end_time.HasValue)
                    {
                        if (data.end_time >= DateTime.Now)
                        {
                            model = data;
                        }
                    }
                }
                else
                {
                    model = data;
                }
            }
            _cache.Set(cache_key, model, TimeSpan.FromSeconds(cache_seconds));
            return model;
        }

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="ad_type">广告类型</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<advertisement_entity> get_ad_list(ad_type_enum ad_type, string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"advertisement_list_{ad_type}_cache";
            }

            var cache_value = _cache.Get<List<advertisement_entity>>(cache_key);
            if (cache_value != null)
            {
                return cache_value;
            }

            var list = new List<advertisement_entity>();
            var data = _context.advertisement.Where(m => m.ad_type == ad_type).ToList();
            foreach (var item in data)
            {
                if (item.view_time_limit == view_time_limit_enum.到期不显示)
                {
                    if (item.end_time.HasValue)
                    {
                        if (item.end_time >= DateTime.Now)
                        {
                            list.Add(item);
                        }
                    }
                }
                else
                {
                    list.Add(item);
                }
            }
            _cache.Set(cache_key, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取值班表
        /// </summary>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public string get_today_duty_list(string cache_name = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                cache_name = "get_today_duty_list";
            }

            var cache_value = _cache.Get<string>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var today = DateTime.Today;
            string line_str = "暂无值班信息";
            var model = _context.duty.FirstOrDefault(m => m.date == today);
            if (model != null)
            {
                line_str = "";
                var configs = _context.duty_config.ToList();
                foreach (var item in configs)
                {
                    if (!string.IsNullOrEmpty(item.display_name))
                    {
                        //如果表头显示名称不为空
                        line_str += $"{item.display_name}：";

                        string column_value = "";
                        var column_obj = model.GetType().GetProperty(item.column_no).GetValue(model, null);
                        if (column_obj != null)
                        {
                            //如果表格值不为空
                            column_value = column_obj.ToString();
                        }
                        else
                        {
                            column_value = "无";
                        }
                        line_str += column_value + "，";
                    }
                }
            }
            _cache.Set(cache_name, line_str, TimeSpan.FromSeconds(cache_seconds));
            return line_str;
        }

        /// <summary>
        /// 获取栏目导航
        /// </summary>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<column_entity> get_column_nav(string cache_name = "", int cache_seconds = 60000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                cache_name = $"column_nav_cache";
            }

            var cache_value = _cache.Get<List<column_entity>>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var list = _context.column.Where(m => m.is_show_nav).OrderBy(m => m.sort_rank).ThenByDescending(m => m.id).ToList();
            _cache.Set(cache_name, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <param name="ids">栏目id集合</param>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<column_entity> get_column_list(int[] ids, bool? is_recommend, string cache_name = "", int cache_seconds = 60000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                string ids_str = string.Join(',', ids);
                cache_name = $"column_list_{ids_str}_cache";
            }

            var cache_value = _cache.Get<List<column_entity>>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var query = _context.column.AsQueryable();
            if (ids.Count() > 0)
            {
                query = query.Where(m => ids.Contains(m.id));
            }
            if (is_recommend.HasValue)
            {
                query = query.Where(m => m.is_recommend == is_recommend.Value);
            }
            var list = query.OrderBy(m => m.sort_rank).ThenByDescending(m => m.id).ToList();
            _cache.Set(cache_name, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取子栏目列表
        /// </summary>
        /// <param name="parent_id">父栏目id</param>
        /// <param name="is_recommend">是否推荐</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<column_entity> get_column_child_list(int parent_id, bool? is_recommend = null, string cache_key = "", int cache_seconds = 60000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"column_child_list_{parent_id}_cache";
            }

            var cache_value = _cache.Get<List<column_entity>>(cache_key);
            if (cache_value != null)
            {
                return cache_value;
            }

            var query = _context.column.Where(m => m.parent_id == parent_id).AsQueryable();
            if (is_recommend.HasValue)
            {
                query = query.Where(m => m.is_recommend == is_recommend.Value);
            }
            var list = query.OrderBy(m => m.sort_rank).ToList();
            _cache.Set(cache_key, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取栏目详情
        /// </summary>
        /// <param name="column_id">栏目id</param>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public column_entity get_column_details(int column_id, string cache_name = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                cache_name = $"column_details_{column_id}_cache";
            }
            var cache_value = _cache.Get<column_entity>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var column = _context.column.FirstOrDefault(m => m.id == column_id);
            if (column != null)
            {
                if (column.parent_id > 0)
                {
                    var data = _context.column.ToList();
                    GetParents(column, data);
                    column.parent_list = parent_list;
                }
            }

            _cache.Set(cache_name, column, TimeSpan.FromSeconds(cache_seconds));
            return column;
        }

        List<column_entity> parent_list = new List<column_entity>();
        private List<column_entity> GetParents(column_entity item, List<column_entity> data)
        {
            var result = data.Where(x => x.id == item.parent_id).FirstOrDefault();
            if (result != null)
            {
                parent_list.Add(result);
                if (result.parent_id > 0)
                {
                    return GetParents(result, data);
                }
            }
            return parent_list;
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="id">文章主键id</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public article_entity get_article_details(int id, string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"article_details_{id}_cache";
            }
            var cache_details = _cache.Get<article_entity>(cache_key);
            if (cache_details != null)
            {
                return cache_details;
            }

            var article = _context.article.Where(m => m.id == id).Include(m => m.column).FirstOrDefault();
            if (article != null)
            {
                var article_content = _context.article_content.Where(m => m.article_id == article.id).FirstOrDefault();
                if (article_content != null)
                {
                    article.content = article_content.content;
                }
            }
            _cache.Set(cache_key, article, TimeSpan.FromSeconds(cache_seconds));
            return article;
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="article_snow_id">文章id雪花算法</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public article_entity get_article_details(long article_snow_id, string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"article_details_{article_snow_id}_cache";
            }
            var cache_details = _cache.Get<article_entity>(cache_key);
            if (cache_details != null)
            {
                return cache_details;
            }

            var article = _context.article.Where(m => m.article_snow_id == article_snow_id).Include(m => m.column).FirstOrDefault();
            if (article != null)
            {
                var article_content = _context.article_content.Where(m => m.article_id == article.id).FirstOrDefault();
                if (article_content != null)
                {
                    article.content = article_content.content;
                }
            }
            _cache.Set(cache_key, article, TimeSpan.FromSeconds(cache_seconds));
            return article;
        }

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
        public List<article_entity> get_article_list(int column_id = 0, bool? is_slide = null, bool? is_top = null, bool? is_best = null,
            bool? is_recommend = null, bool? is_hot = null, bool? is_pic = null, int? skip = null, int take = 10, string orderby_field = "publish_time",
            string orderby = "desc", string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"article_list_{column_id}_{is_slide}_{is_top}_{is_best}_{is_recommend}_{is_hot}_{is_pic}_{skip}_{take}_{orderby_field}_{orderby}_cache";
                cache_key = cache_key.ToLower();
            }
            var cache_list = _cache.Get<List<article_entity>>(cache_key);
            if (cache_list != null)
            {
                return cache_list;
            }

            var query = _context.article.Where(m => m.is_review).AsQueryable();
            if (column_id > 0)
            {
                var column = _context.column.Where(m => m.id == column_id).FirstOrDefault();
                if (column != null)
                {
                    //查询子栏目
                    var child = _context.column.Where(m => m.parent_id == column.id);
                    if (child != null)
                    {
                        query = query.Where(m => (m.column_id == column_id || child.Select(m => m.id).Contains(m.column_id)
                            || m.sub_column.Contains(column_id.ToString())));
                    }
                    else
                    {
                        query = query.Where(m => (m.column_id == column_id || m.sub_column.Contains(column_id.ToString())));
                    }
                }
            }
            if (is_slide.HasValue)
            {
                query = query.Where(m => m.is_slide == is_slide.Value);
            }
            if (is_top.HasValue)
            {
                query = query.Where(m => m.is_top == is_top.Value);
            }
            if (is_best.HasValue)
            {
                query = query.Where(m => m.is_best == is_best.Value);
            }
            if (is_recommend.HasValue)
            {
                query = query.Where(m => m.is_recommend == is_recommend.Value);
            }
            if (is_hot.HasValue)
            {
                query = query.Where(m => m.is_hot == is_hot.Value);
            }
            if (is_pic.HasValue)
            {
                query = query.Where(m => m.is_pic == is_pic.Value);
            }
            query = query.Include(m => m.column).OrderByD(orderby_field, orderby);
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            query = query.Take(take);
            var list = query.ToList();
            _cache.Set(cache_key, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

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
        public List<article_entity> get_article_list(int[] column_ids, bool? is_slide = null, bool? is_top = null, bool? is_best = null,
            bool? is_recommend = null, bool? is_hot = null, bool? is_pic = null, int? skip = null, int take = 10, string orderby_field = "publish_time",
            string orderby = "desc", string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                string column_ids_str = string.Join(',', column_ids);
                cache_key = $"article_list_{column_ids_str}_{is_slide}_{is_top}_{is_best}_{is_recommend}_{is_hot}_{is_pic}_{skip}_{take}_{orderby_field}_{orderby}_cache";
            }
            var cache_list = _cache.Get<List<article_entity>>(cache_key);
            if (cache_list != null)
            {
                return cache_list;
            }

            var query = _context.article.Where(m => column_ids.Contains(m.column_id) && m.is_review).AsQueryable();
            if (is_slide.HasValue)
            {
                query = query.Where(m => m.is_slide == is_slide.Value);
            }
            if (is_top.HasValue)
            {
                query = query.Where(m => m.is_top == is_top.Value);
            }
            if (is_best.HasValue)
            {
                query = query.Where(m => m.is_best == is_best.Value);
            }
            if (is_recommend.HasValue)
            {
                query = query.Where(m => m.is_recommend == is_recommend.Value);
            }
            if (is_hot.HasValue)
            {
                query = query.Where(m => m.is_hot == is_hot.Value);
            }
            if (is_pic.HasValue)
            {
                query = query.Where(m => m.is_pic == is_pic.Value);
            }
            query = query.OrderByD(orderby_field, orderby).Include(m => m.column);
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            query = query.Take(take);
            var list = query.ToList();
            _cache.Set(cache_key, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取文章列表并分页
        /// </summary>
        /// <param name="column_id"></param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        public PagedList<article_entity> get_article_page(int column_id, int page = 1, int page_size = 15,
            string orderby_field = "publish_time", string orderby = "desc")
        {
            var query = _context.article.Where(m => m.is_review).AsQueryable();

            //查询文章所属栏目
            var column = _context.column.Where(m => m.id == column_id).FirstOrDefault();
            if (column != null)
            {
                //查询子栏目
                var child = _context.column.Where(m => m.parent_id == column.id);
                if (child != null)
                {
                    query = query.Where(m => (m.column_id == column_id || child.Select(m => m.id).Contains(m.column_id) || m.sub_column.Contains(column_id.ToString())));
                }
                else
                {
                    query = query.Where(m => (m.column_id == column_id || m.sub_column.Contains(column_id.ToString())));
                }
            }
            var list = query.OrderByD(orderby_field, orderby).Include(m => m.column).ToPagedList(page, page_size);
            return list;
        }

        /// <summary>
        /// 搜索文章分页
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        public PagedList<article_entity> search_article_page(string keyword, int page = 1, int page_size = 15, string orderby_field = "publish_time", string orderby = "desc")
        {
            //var query = _context.article.Where(m => m.title.Contains(keyword) || m.content_nohtml.Contains(keyword)).AsQueryable();
            if (string.IsNullOrEmpty(keyword))
            {
                var no_data = new List<article_entity>();
                return no_data.ToPagedList(page, page_size);
            }
            keyword = keyword.Trim().Replace(" ", "%");
            keyword = V6.ChkSql(keyword);
            var query = _context.article.FromSqlRaw($"SELECT * FROM T_article WHERE title LIKE '%{keyword}%' OR CONTAINS(content_nohtml, '{keyword}')");
            query = query.Where(m => m.is_review);
            var list = query.OrderByD(orderby_field, orderby).Include(m => m.column).ToPagedList(page, page_size);
            return list;
        }

        /// <summary>
        /// 获取图片文章列表
        /// </summary>
        /// <param name="take">调用几条</param>
        /// <param name="cache_name"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        public List<article_entity> get_pic_article_list(int take, string cache_name = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                cache_name = $"pic_article_list_cache";
            }
            var cache_list = _cache.Get<List<article_entity>>(cache_name);
            if (cache_list != null)
            {
                return cache_list;
            }

            var query = _context.article.Where(m => m.is_pic && m.is_review).AsQueryable();
            var list = query.Take(take).OrderByDescending(m => m.publish_time).ToList();
            _cache.Set(cache_name, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取图片文章列表
        /// </summary>
        /// <param name="column_id">栏目id</param>
        /// <param name="take">调用几条</param>
        /// <param name="cache_key"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        public List<article_entity> get_pic_article_list(int column_id, int take, string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"article_list_{column_id}_pic_cache";
            }
            var cache_list = _cache.Get<List<article_entity>>(cache_key);
            if (cache_list != null)
            {
                return cache_list;
            }

            var query = _context.article.Where(m => m.column_id == column_id && m.is_pic && m.is_review).AsQueryable();
            var list = query.Take(take).OrderByDescending(m => m.publish_time).ToList();
            _cache.Set(cache_key, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="source_id">文章id</param>
        /// <param name="module">模块：文章=article, 问答=ask</param>
        /// <param name="cache_key">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<comment_entity> get_comment_list(int source_id, string module = "article", string cache_key = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_key))
            {
                cache_key = $"comment_list_{source_id}_{module}_cache";
            }
            var cache_list = _cache.Get<List<comment_entity>>(cache_key);
            if (cache_list != null)
            {
                return cache_list;
            }

            var query = _context.comment.Where(m => m.source_id == source_id).AsQueryable();
            var list = query.OrderByDescending(m => m.id).ToList();
            _cache.Set(cache_key, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取ip地址列表
        /// </summary>
        /// <param name="ip_type"></param>
        /// <param name="cache_name"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        public List<ip_address_entity> get_ip_address(ip_type_enum ip_type, string cache_name = "", int cache_seconds = 6000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                cache_name = $"ip_address_{ip_type}_cache";
            }
            var cache_value = _cache.Get<List<ip_address_entity>>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var query = _context.ip_address.Where(m => m.ip_type == ip_type).AsQueryable();
            var list = query.ToList();
            _cache.Set(cache_name, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取当天过生日的用户列表
        /// </summary>
        /// <param name="cache_key"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        public List<birthday_list_entity> get_today_birthday(string cache_key = "today_birthday_cache", int cache_seconds = 60000)
        {
            var cache_value = _cache.Get<List<birthday_list_entity>>(cache_key);
            if (cache_value != null)
            {
                return cache_value;
            }

            var today = DateTime.Today;
            var model = new List<birthday_list_entity>();
            var user_list = _context.birthday_list.ToList();
            foreach (var item in user_list)
            {
                if (item.date_of_birth.HasValue)
                {
                    var date_of_birth = item.date_of_birth.Value;
                    if (date_of_birth.Month == today.Month && date_of_birth.Day == today.Day)
                    {
                        model.Add(new birthday_list_entity
                        {
                            real_name = item.real_name
                        });
                    }
                }
            }
            _cache.Set(cache_key, model, TimeSpan.FromSeconds(cache_seconds));
            return model;
        }

        /// <summary>
        /// 获取链接分类列表
        /// </summary>
        /// <param name="ids">分类id集合</param>
        /// <param name="take">调用多少条</param>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<link_category_entity> get_link_category_list(int[] ids, int take = 100, string cache_name = "", int cache_seconds = 600000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                string ids_str = string.Join(',', ids);
                cache_name = $"link_category_list_{ids_str}_cache";
            }
            var cache_value = _cache.Get<List<link_category_entity>>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var query = _context.link_category.Where(m => ids.Contains(m.id)).Take(take).AsQueryable();
            var list = query.OrderBy(m => m.sort_rank).ThenBy(m => m.id).ToList();
            _cache.Set(cache_name, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取友情链接列表
        /// </summary>
        /// <param name="category_id">分类id</param>
        /// <param name="take">调用多少条</param>
        /// <param name="cache_name">缓存名</param>
        /// <param name="cache_seconds">缓存时间</param>
        /// <returns></returns>
        public List<link_entity> get_link_list(int category_id, int take = 200, string cache_name = "", int cache_seconds = 600000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                cache_name = $"link_list_{category_id}_cache";
            }
            var cache_value = _cache.Get<List<link_entity>>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var query = _context.link.Where(m => m.category_id == category_id).Take(take).AsQueryable();
            var list = query.OrderBy(m => m.sort_rank).ThenBy(m => m.id).ToList();
            _cache.Set(cache_name, list, TimeSpan.FromSeconds(cache_seconds));
            return list;
        }

        /// <summary>
        /// 获取网站配置
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="cache_name"></param>
        /// <param name="cache_seconds"></param>
        /// <returns></returns>
        public site_config_entity get_site_config(string cache_name = "", int cache_seconds = 60000)
        {
            if (string.IsNullOrEmpty(cache_name))
            {
                cache_name = "site_config";
            }
            var cache_value = _cache.Get<site_config_entity>(cache_name);
            if (cache_value != null)
            {
                return cache_value;
            }

            var model = _context.site_config.FirstOrDefault();
            _cache.Set(cache_name, model, TimeSpan.FromSeconds(cache_seconds));
            return model;
        }
    }
}