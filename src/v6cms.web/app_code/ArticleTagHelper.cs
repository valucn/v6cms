using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using v6cms.entities.db_set;

namespace v6cms.web
{
    /// <summary>
    /// 文章标签
    /// </summary>
    [HtmlTargetElement("jd-article")]
    public class ArticleTagHelper : TagHelper
    {
        private readonly db_context _context;
        public ArticleTagHelper(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 栏目id
        /// </summary>
        public int column_id { get; set; }
        public string class_name { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", class_name);

            var out_html = new StringBuilder(string.Empty);
            output.PreContent.SetHtmlContent(out_html.ToString());
        }
    }
}
