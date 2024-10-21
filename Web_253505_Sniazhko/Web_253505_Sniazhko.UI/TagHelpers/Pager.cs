using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web_253505_Sniazhko.Domain.Entities;

namespace Web_253505_Sniazhko.UI.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; } = 1;
        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; } = 1;
        [HtmlAttributeName("category")]
        public string category { get; set; } = "";
        [HtmlAttributeName("is-admin")]
        public bool IsAdmin { get; set; } = false;
        private LinkGenerator _linkGenerator;
        private IHttpContextAccessor _contextAccessor;
        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _contextAccessor = httpContextAccessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var controllerName = IsAdmin ? "Admin" : "Product";
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("class", "pagination justify-content-center");
            var prev = new TagBuilder("li");
            if (CurrentPage == 1)
            {
                prev.Attributes.Add("class", "page-item disabled");
            }
            else
            {
                prev.Attributes.Add("class", "page-item");
            }
            var prevLink = new TagBuilder("a");
            int? prevPage = CurrentPage == 1 ? 1 : CurrentPage - 1;
            var prevUrl = _linkGenerator.GetPathByAction("Index", controllerName, new { pageNo = prevPage, category });
            prevLink.Attributes.Add("href", prevUrl);
            prevLink.Attributes["class"] = "page-link ajax-link";
            prevLink.InnerHtml.AppendHtml("&laquo");
            prev.InnerHtml.AppendHtml(prevLink);
            ul.InnerHtml.AppendHtml(prev);
            foreach (var number in Enumerable.Range(1, TotalPages))
            {
                var currentLi = new TagBuilder("li");
                if (CurrentPage == number)
                {
                    currentLi.Attributes.Add("class", "page-item active");
                }
                else
                {
                    currentLi.Attributes.Add("class", "page-item");
                }
                var currentA = new TagBuilder("a");
                var currentUrl = _linkGenerator.GetPathByAction("Index", controllerName, new { pageNo = number, category });
                currentA.Attributes.Add("href", currentUrl);
                currentA.Attributes["class"] = "page-link ajax-link";
                currentA.Attributes["id"] = $"number{number}";
                currentA.InnerHtml.AppendHtml(number.ToString());
                currentLi.InnerHtml.AppendHtml(currentA);
                ul.InnerHtml.AppendHtml(currentLi);
            }
            var next = new TagBuilder("li");
            if (CurrentPage == TotalPages)
            {
                next.Attributes.Add("class", "page-item disabled");
            }
            else
            {
                next.Attributes.Add("class", "page-item");
            }
            var nextLink = new TagBuilder("a");
            int? nextPage = CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;
            var nextUrl = _linkGenerator.GetPathByAction("Index", controllerName, new { pageNo = nextPage, category = category });
            nextLink.Attributes.Add("href", nextUrl);
            nextLink.Attributes["class"] = "page-link ajax-link";
            nextLink.InnerHtml.AppendHtml("&raquo");
            next.InnerHtml.AppendHtml(nextLink);
            ul.InnerHtml.AppendHtml(nextLink);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(ul);
        }
    }
}
