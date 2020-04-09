using PhysicManagement.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhysicManagement.Helpers
{

    public static class HtmlHelperExtention
    {
        /// <summary>
        /// Show text instead of boolean property 
        /// </summary>
        /// <param name="helper">html helpter class</param>
        /// <param name="item">boolean item</param>
        /// <returns>text for show</returns>
        public static string DisplayForBoolean(this HtmlHelper helper, bool? item)
        {
            if (helper is null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            if (item is null)
            {
                return "نامشخص";
            }

            if (item.Value)
            {
                return "بلی";
            }
            else
            {
                return "خیر";
            }
        }
        //public static string DisplayUserName(this HtmlHelper helper, int? item)
        //{
        //    if (item.HasValue)
        //    {
        //        if (item == 1)
        //            return "مدیر سایت";

        //        var userData = Logic.Account.Services.UserService.GetFullnameUserById(item.Value);
        //        if (userData != null)
        //            return userData;
        //        else
        //            return "نامشخص";
        //    }
        //    return "نامشخص";
        //}

        /// <summary>
        /// convert and show date as persian date instead of gregorian date
        /// </summary>
        /// <param name="helper">html helpter class</param>
        /// <param name="item">date object</param>
        /// <returns>text for show</returns>
        public static string DisplayForDate(this HtmlHelper helper, DateTime? item)
        {
            if (helper is null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            if (item is null)
            {
                return "";
            }

            return DateUtility.GetPersianDate(item);
        }

        /// <summary>
        /// convert and show date as persian date and time instead of gregorian date
        /// </summary>
        /// <param name="helper">html helpter class</param>
        /// <param name="item">date object</param>
        /// <returns>text for show</returns>
        public static string DisplayForDateTime(this HtmlHelper helper, DateTime? item)
        {
            if (helper is null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DateUtility.GetPersianDateTime(item);
        }
        public static MvcHtmlString ColorPicker<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (html is null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                return null;
            var name = memberExpression.Member.Name;

            var inputTag = new TagBuilder("input");
            inputTag.Attributes["id"] = $"txt{name}";
            inputTag.Attributes["type"] = "text";
            inputTag.Attributes["name"] = $"{name}";
            inputTag.Attributes["class"] = "form-control jscolor";
            inputTag.Attributes["autocomplete"] = "off";
            inputTag.Attributes["readonly"] = "readonly";

            if (html.ViewData.Model != null)
            {
                Func<TModel, TValue> method = expression.Compile();
                string value = method(html.ViewData.Model).ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    inputTag.Attributes["value"] = value;
                }
            }
            inputTag.InnerHtml += "<script>jscolor.installByClassName(\"jscolor\");</script>";
            return MvcHtmlString.Create(inputTag.ToString(TagRenderMode.Normal));

        }

        public static MvcHtmlString EditorPersianDatePicker<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelName)
        {
            if (html is null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                return null;
            var name = memberExpression.Member.Name;


            var inputTag = new TagBuilder("input");
            inputTag.Attributes["id"] = $"txt{name}";
            inputTag.Attributes["type"] = "text";
            inputTag.Attributes["name"] = $"persian_{name}";
            inputTag.Attributes["class"] = "form-control";
            inputTag.Attributes["autocomplete"] = "off";
            inputTag.Attributes["style"] = "text-align:left";

            var hiddenTag = new TagBuilder("input");
            hiddenTag.Attributes["type"] = "hidden";
            hiddenTag.Attributes["name"] = name;

            if (html.ViewData.Model != null)
            {
                Func<TModel, TValue> method = expression.Compile();
                string value = method(html.ViewData.Model).ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    var date = DateTime.Parse(value);
                    inputTag.Attributes["value"] = date.ToString("yyyy/MM/dd");
                    hiddenTag.Attributes["value"] = date.ToString("yyyy/MM/dd");
                }
            }


            var script = @"
<script type='text/javascript'>
    $(document).ready(function () {
        var dateConfig = { 
            format: 'YYYY/MM/DD', 
            autoClose: true, 
" +
(!inputTag.Attributes.ContainsKey("value") ? @" initialValue: false, " : "") +
(inputTag.Attributes.ContainsKey("value") ? @" initialValueType: 'gregorian', " : "") +
@"
            onSelect: function (unix) {
                var date = new Date(unix);
                var gregorianDate = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
                $('[name=" + name + @"]').val(gregorianDate);
            }
        };
        $('#txt" + name + @"').pDatepicker(dateConfig);
    });
</script>
";

            var container = new TagBuilder("div");
            container.InnerHtml += $"<label for=\"txt{name}\" class=\"bmd-label-floating\">{labelName}</label>";
            container.InnerHtml += inputTag;
            container.InnerHtml += hiddenTag;
            container.InnerHtml += script;
            return MvcHtmlString.Create(container.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString EditorPersianDatePicker(this HtmlHelper helper, string name, string labelName, DateTime? value = null)
        {

            var inputTag = new TagBuilder("input");
            inputTag.Attributes["id"] = $"txt{name}";
            inputTag.Attributes["type"] = "text";
            inputTag.Attributes["name"] = $"persian_{name}";
            inputTag.Attributes["class"] = "form-control";
            inputTag.Attributes["autocomplete"] = "off";
            inputTag.Attributes["readonly"] = "readonly";

            var hiddenTag = new TagBuilder("input");
            hiddenTag.Attributes["type"] = "hidden";
            hiddenTag.Attributes["name"] = name;

            if (value != null)
            {
                inputTag.Attributes["value"] = value.Value.ToString("yyyy/MM/dd");
                hiddenTag.Attributes["value"] = value.Value.ToString("yyyy/MM/dd");
            }

            var script = @"
<script type='text/javascript'>
    $(document).ready(function () {
        var dateConfig = { 
            format: 'YYYY/MM/DD', 
            autoClose: true, 
" +
(!inputTag.Attributes.ContainsKey("value") ? @" initialValue: false, " : "") +
(inputTag.Attributes.ContainsKey("value") ? @" initialValueType: 'gregorian', " : "") +
@"
            onSelect: function (unix) {
                var date = new Date(unix);
                var gregorianDate = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
                $('[name=" + name + @"]').val(gregorianDate);
            }
        };
        $('#txt" + name + @"').pDatepicker(dateConfig);
    });
</script>
";

            var container = new TagBuilder("div");
            container.AddCssClass("date-picker-container");
            container.InnerHtml += $"<label for=\"txt{name}\" class=\"bmd-label-floating\">{labelName}</label>";
            container.InnerHtml += inputTag;
            container.InnerHtml += hiddenTag;
            container.InnerHtml += script;
            return MvcHtmlString.Create(container.ToString(TagRenderMode.Normal));
        }

        //public static MvcHtmlString ImagePreview<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, char Size)
        //{
        //    if (html is null)
        //    {
        //        throw new ArgumentNullException(nameof(html));
        //    }

        //    if (expression is null)
        //    {
        //        throw new ArgumentNullException(nameof(expression));
        //    }

        //    if (!(expression.Body is MemberExpression memberExpression))
        //        return null;
        //    var name = memberExpression.Member.Name;

        //    var imageTag = new TagBuilder("img");
        //    imageTag.Attributes["id"] = $"img{name}";
        //    imageTag.Attributes["class"] = "rounded";

        //    if (html.ViewData.Model != null)
        //    {
        //        string ImageAddress;
        //        try
        //        {
        //            Func<TModel, TValue> method = expression.Compile();
        //            string value = method(html.ViewData.Model)?.ToString();
        //            if (!string.IsNullOrEmpty(value))
        //            {
        //                var ImageData = JSON.Desrialize<Logic.FileUpload.ViewModel.FileData>(value);
        //                ImageAddress = "/FileStorage/" + Size + "/" + ImageData.FileId + "/" + ImageData.FileName + "?rnd=" + Common.FileID.NewID();
        //            }
        //            else
        //            {

        //                ImageAddress = (Size == 't' ? "/Images/no-image-t.jpg" : "/Images/no-image.jpg");
        //            }
        //        }
        //        catch
        //        {
        //            ImageAddress = (Size == 't' ? "/Images/no-image-t.jpg" : "/Images/no-image.jpg");
        //        }
        //        imageTag.Attributes["src"] = ImageAddress;
        //    }

        //    return MvcHtmlString.Create(imageTag.ToString(TagRenderMode.Normal));
        //}
        public static MvcHtmlString FileUpload<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (html is null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!(expression.Body is MemberExpression memberExpression))
                return null;
            var name = memberExpression.Member.Name;

            var inputTag = new TagBuilder("input");
            inputTag.Attributes["type"] = "file";
            inputTag.Attributes["name"] = $"file{name}";
            inputTag.Attributes["class"] = "form-control";

            if (html.ViewData.Model != null)
            {
                Func<TModel, TValue> method = expression.Compile();
                string value = method(html.ViewData.Model)?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    inputTag.Attributes["value"] = value;
                }
            }
            return MvcHtmlString.Create(inputTag.ToString(TagRenderMode.Normal));

        }
        public static MvcHtmlString Pagger(this HtmlHelper htmlHelper, int TotalRecords, int PageSize = 25)
        {
            int TotalPages = (TotalRecords / PageSize) + (TotalRecords % PageSize != 0 ? 1 : 0);
            if (TotalPages == 1 || TotalPages == 0)
                return MvcHtmlString.Create("");

            int CurrentPage = int.Parse(htmlHelper.ViewContext.HttpContext.Request["p"] ?? "1");
            string ControllerName = htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
            string ActionName = htmlHelper.ViewContext.RouteData.Values["action"].ToString();
            string IDName = htmlHelper.ViewContext.RouteData.Values["id"]?.ToString();
            NameValueCollection QueryStrings = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            Dictionary<string, object> qs = new Dictionary<string, object>();
            foreach (var item in QueryStrings)
            {
                string qsName = item.ToString();
                if (qsName != "p")
                {
                    qs.Add(qsName, QueryStrings[qsName]);
                }
            }


            string BaseAddress =
                ((Route)htmlHelper.ViewContext.RouteData.Route).Url.
                Replace("{controller}", ControllerName).
                Replace("{action}", ActionName).
                Replace("{id}", IDName);
            string Url = "/" + BaseAddress;
            if (qs.Count == 0)
            {
                Url += "?p=";
            }
            else
            {
                Url += "?" + string.Join("&", qs.Select(x => $"{x.Key}={x.Value}").ToList()) + "&p=";
            }
            TagBuilder tagBuilder = new TagBuilder("ul");
            tagBuilder.AddCssClass("pagination");
            tagBuilder.AddCssClass("justify-content-center");
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{Url}{1}\">صفحه نخست</a></li>");

            if (CurrentPage > 1 && TotalPages > 5)
            {
                stringBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{Url}{(CurrentPage - 1)}\">صفحه‌ی قبلی</a></li>");
            }
            int StartPage = CurrentPage;
            if (TotalPages < 10)
            {
                StartPage = 1;
            }
            int EndPage = TotalPages;
            if ((TotalPages - CurrentPage) > 5)
            {
                EndPage = CurrentPage + 5;
            }
            for (var page = StartPage; page <= EndPage; page++)
            {
                stringBuilder.Append(
                    $"<li class=\"page-item {(page == CurrentPage ? "active" : "")}\"><a class=\"page-link\" href=\"{Url}{page}\">{page}</a></li>");
            }

            if (CurrentPage < TotalPages && TotalPages > 5)
            {
                stringBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{Url}{(CurrentPage + 1)}\">صفحه‌ی بعدی</a></li>");
            }
            stringBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{Url}{TotalPages}\">صفحه‌ی آخر</a></li>");

            tagBuilder.InnerHtml = stringBuilder.ToString();
            return MvcHtmlString.Create(tagBuilder.ToString());

        }

    }
}