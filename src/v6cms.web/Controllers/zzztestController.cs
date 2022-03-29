using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.page_views;
using v6cms.services;
using v6cms.utils;

namespace v6cms.web.Controllers
{
    public class zzztestController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;
        private readonly IDataService _service;
        public zzztestController(IMemoryCache cache, db_context context, IDataService service)
        {
            _cache = cache;
            _context = context;
            _service = service;
        }

        public IActionResult index()
        {
            return Content("success");
        }

        public async Task<IActionResult> index2()
        {
            string template = await System.IO.File.ReadAllTextAsync(Path.Combine("wwwroot", "templates", "sscms", "article_details.html"));
            var nav = _service.get_column_nav();
            var article = _service.get_article_details(10429);

            var zzz = new zzztest_model<column_entity>();
            zzz.nav_list = nav;

            var config = new TemplateServiceConfiguration();
            config.Debug = true;
            config.EncodedStringFactory = new RawStringFactory();
            var engine = RazorEngineService.Create(config);


            string contents = engine.RunCompile(template, "test", null, zzz);
            await System.IO.File.WriteAllTextAsync(Path.Combine("wwwroot", "html", "test.html"), contents);

            return Content("success");
        }
    }
}
