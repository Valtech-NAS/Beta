﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SFA.Apprenticeships.Web.Candidate.Views.ApprenticeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Locations;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Login;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<ApprenticeshipSearchViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
  
    ViewBag.Title = "Apprenticeships - Find an apprenticeship";
    Layout = "~/Views/Shared/_ApprenticeshipLayout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n    <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Find an apprenticeship</h1>\r\n    <p");

WriteLiteral(" class=\"subtitle\"");

WriteLiteral(">Search and apply for an apprenticeship in England</p>\r\n</div>\r\n<div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n\r\n    <div");

WriteLiteral(" class=\"grid-wrapper controls-3-4\"");

WriteLiteral(">\r\n        <section");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n");

            
            #line 15 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
             using (Html.BeginRouteForm(CandidateRouteNames.ApprenticeshipSearchValidation, FormMethod.Get, new { @id = "#searchForm" }))
            {
                
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
           Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                                                                       


            
            #line default
            #line hidden
WriteLiteral("                <nav");

WriteLiteral(" class=\"tabbed-nav\"");

WriteLiteral(">\r\n");

            
            #line 20 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 20 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                      
                        var keywordTabControlClass = Model.SearchMode == ApprenticeshipSearchMode.Keyword ? " active" : "";
                        var categoriesTabControlClass = Model.SearchMode == ApprenticeshipSearchMode.Category ? " active" : "";

                        var categoriesElementControlClass = Model.SearchMode == ApprenticeshipSearchMode.Category && Model.Categories != null && Model.Categories.Any() ? " active" : "";
                        var categoriesTabClass = Model.Categories != null && Model.Categories.Any() ? " tab2" : "";
                        var elementControlClass = Model.SearchMode == ApprenticeshipSearchMode.Keyword ? keywordTabControlClass : categoriesElementControlClass;
                    
            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 28 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.RouteLink("Keywords", CandidateRouteNames.ApprenticeshipSearch, new { searchMode = ApprenticeshipSearchMode.Keyword }, new { @id = "keywords-tab-control", @class = "tabbed-tab" + keywordTabControlClass, tab = "#tab1" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 29 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.RouteLink("Categories", CandidateRouteNames.ApprenticeshipSearch, new { searchMode = ApprenticeshipSearchMode.Category }, new { @id = "categories-tab-control", @class = "tabbed-tab" + categoriesTabControlClass, tab = "#tab2" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </nav>\r\n");

            
            #line 31 "..\..\Views\ApprenticeshipSearch\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"tabbed-content active\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("                    ");

            
            #line 34 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.Partial("_categories", Model));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 36 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 36 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                      
                        
            
            #line default
            #line hidden
            
            #line 37 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                   Write(Html.FormTextFor(m => m.Keywords, controlHtmlAttributes: new { autofocus = "autofocus", aria_describedby = "keywordsHint" }, containerHtmlAttributes: new { @class = "tabbed-element tab1" + keywordTabControlClass }));

            
            #line default
            #line hidden
            
            #line 37 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                                                                                                                                                                                                                                               
                    
            
            #line default
            #line hidden
WriteLiteral("\r\n                    <p");

WriteLiteral(" class=\"visuallyhidden\"");

WriteLiteral(" id=\"keywordsHint\"");

WriteLiteral(">Try words that describe the type of apprenticeship you want, for example “carpen" +
"try” or “mechanics”.</p>\r\n                    \r\n");

WriteLiteral("                    ");

            
            #line 41 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.FormTextFor(m => m.Location, containerHtmlAttributes: new { @class = "tabbed-element tab1" + categoriesTabClass + elementControlClass }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2810), Tuple.Create("\"", 2885)
, Tuple.Create(Tuple.Create("", 2818), Tuple.Create("inline", 2818), true)
, Tuple.Create(Tuple.Create(" ", 2824), Tuple.Create("tabbed-element", 2825), true)
, Tuple.Create(Tuple.Create(" ", 2839), Tuple.Create("tab1", 2840), true)
            
            #line 43 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 2844), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 2845), false)
            
            #line 43 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 2864), Tuple.Create<System.Object, System.Int32>(elementControlClass
            
            #line default
            #line hidden
, 2865), false)
);

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" for=\"loc-within\"");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Within</label>\r\n");

WriteLiteral("                            ");

            
            #line 46 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                       Write(Html.DropDownListFor(m => m.WithinDistance, Model.Distances, new { @id = "loc-within", @name = "WithinDistance" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" for=\"apprenticeship-level\"");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship level</label>\r\n");

WriteLiteral("                            ");

            
            #line 50 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                       Write(Html.DropDownListFor(m => m.ApprenticeshipLevel, Model.ApprenticeshipLevels, new { @id = "apprenticeship-level", @name = "ApprenticeshipLevel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n\r\n                 " +
"   <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <button");

WriteAttribute("class", Tuple.Create(" class=\"", 3679), Tuple.Create("\"", 3737)
, Tuple.Create(Tuple.Create("", 3687), Tuple.Create("button", 3687), true)
, Tuple.Create(Tuple.Create(" ", 3693), Tuple.Create("tabbed-element", 3694), true)
, Tuple.Create(Tuple.Create(" ", 3708), Tuple.Create("tab1", 3709), true)
            
            #line 55 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 3713), Tuple.Create<System.Object, System.Int32>(keywordTabControlClass
            
            #line default
            #line hidden
, 3714), false)
);

WriteLiteral(" id=\"search-button\"");

WriteLiteral(">Search</button>\r\n                        <button");

WriteAttribute("class", Tuple.Create(" class=\"", 3806), Tuple.Create("\"", 3886)
, Tuple.Create(Tuple.Create("", 3814), Tuple.Create("button", 3814), true)
, Tuple.Create(Tuple.Create(" ", 3820), Tuple.Create("tabbed-element", 3821), true)
            
            #line 56 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 3835), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 3836), false)
            
            #line 56 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 3855), Tuple.Create<System.Object, System.Int32>(categoriesElementControlClass
            
            #line default
            #line hidden
, 3856), false)
);

WriteLiteral(" id=\"browse-button\"");

WriteLiteral(">Browse</button>\r\n                    </div>\r\n\r\n");

WriteLiteral("                    ");

            
            #line 59 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.Latitude));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 60 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.Longitude));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 61 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.Hidden("Hash", Model.LatLonLocHash()));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 62 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.LocationType));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 63 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.ResultsPerPage));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 64 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.SearchMode));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                </div>\r\n");

            
            #line 67 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </section>\r\n        <aside");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"inner-block\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"gov-border\"");

WriteLiteral(">\r\n                    <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">Contact us</h2>\r\n                    <ul");

WriteLiteral(" class=\"list-text\"");

WriteLiteral(">\r\n                        <li>0800 015 0400</li>\r\n                        <li><a" +
"");

WriteLiteral(" href=\"mailto:nationalhelpdesk@findapprenticeship.service.gov.uk\"");

WriteLiteral(">Send us an email</a></li>\r\n                    </ul>\r\n                    <ul");

WriteLiteral(" class=\"list-text\"");

WriteLiteral(">\r\n                        <li>\r\n                            <a");

WriteLiteral(" href=\"https://www.gov.uk/apprenticeships-guide\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">About apprenticeships</a>\r\n                        </li>\r\n                    </" +
"ul>\r\n                </div>\r\n            </div>\r\n        </aside>\r\n    </div>\r\n<" +
"/div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("   \r\n");

WriteLiteral("    ");

            
            #line 90 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
Write(Scripts.Render("~/bundles/nas/locationsearch"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <script>\r\n        $(\"#Location\").locationMatch({\r\n            url: \'");

            
            #line 93 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
             Write(Url.Action("location", "Location"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            longitude: \'#");

            
            #line 94 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                     Write(Html.IdFor(m => m.Longitude));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            latitude: \'#");

            
            #line 95 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                    Write(Html.IdFor(m => m.Latitude));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            latlonhash: \'#");

            
            #line 96 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                      Write(Html.IdFor(m => m.Hash));

            
            #line default
            #line hidden
WriteLiteral(@"'
        });

        $(""#keywords-tab-control"").click(function () {
            $(""#SearchMode"").val(""Keyword"");
        });

        $(""#categories-tab-control"").click(function () {
            $(""#SearchMode"").val(""Category"");
        });
    </script>
");

});

        }
    }
}
#pragma warning restore 1591
