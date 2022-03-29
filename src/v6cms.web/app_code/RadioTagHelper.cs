using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;

namespace v6cms.web
{
    /// <summary>
    /// 单选框
    /// </summary>
    [HtmlTargetElement(RadioTagName)]
    public class RadioTagHelper : TagHelper
    {
        private const string RadioTagName = "v6cms-radio";
        private const string ForAttributeName = "asp-for";
        private const string ItemsAttributeName = "asp-items";
        private const string SelectedValueAttributeName = "asp-selected-value";

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(ItemsAttributeName)]
        public IEnumerable<SelectListItem> Items { get; set; }

        [HtmlAttributeName(SelectedValueAttributeName)]
        public object SelectedValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (For == null)
            {
                throw new ArgumentException("必须绑定模型");
            }
            foreach (var item in Items)
            {
                var radio = new TagBuilder("input");
                radio.TagRenderMode = TagRenderMode.SelfClosing;
                radio.Attributes.Add("id", ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name));
                radio.Attributes.Add("name", ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name));
                radio.Attributes.Add("value", item.Value);
                radio.Attributes.Add("title", item.Text);
                radio.Attributes.Add("type", "radio");
                if (item.Disabled)
                {
                    radio.Attributes.Add("disabled", "disabled");
                }
                if (item.Selected || item.Value == For.Model?.ToString() || item.Value == SelectedValue?.ToString())
                {
                    radio.Attributes.Add("checked", "checked");
                }
                output.Content.AppendHtml(radio);
            }
            output.TagName = "";
        }
    }
}
