using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace v6cms.web
{
    /// <summary>
    /// 模版视图路径
    /// </summary>
    public class TemplateViewLocationExpander : IViewLocationExpander
    {
        public string template_dir { get; set; }

        public TemplateViewLocationExpander(string _template_dir)
        {
            template_dir = _template_dir;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //Console.WriteLine($"context.AreaName={context.AreaName}");
            if (string.IsNullOrEmpty(context.AreaName))
            {
                return viewLocations.Select(m => m.Replace("/Views/", $"/Views/{context.Values["template_dir"]}/"));
            }
            else
            {
                return viewLocations;
            }
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //var template = context.ActionContext.RouteData.Values["Template"]?.ToString() ?? "Default";
            var dir = string.IsNullOrEmpty(template_dir) ? "default" : template_dir;
            context.Values["template_dir"] = dir;
        }
    }
}
