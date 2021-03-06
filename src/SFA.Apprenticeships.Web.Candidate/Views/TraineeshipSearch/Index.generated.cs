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

namespace SFA.Apprenticeships.Web.Candidate.Views.TraineeshipSearch
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/TraineeshipSearch/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<TraineeshipSearchViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\TraineeshipSearch\Index.cshtml"
  
    ViewBag.Title = "Find a traineeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n    <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Find a traineeship</h1>\r\n    <p");

WriteLiteral(" class=\"subtitle\"");

WriteLiteral(">Search and apply for a traineeship in England</p>\r\n</div>\r\n<div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n\r\n    <div");

WriteLiteral(" class=\"grid-wrapper controls-3-4\"");

WriteLiteral(">\r\n        <section");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n");

            
            #line 15 "..\..\Views\TraineeshipSearch\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\TraineeshipSearch\Index.cshtml"
             using (Html.BeginRouteForm(CandidateRouteNames.TraineeshipResults, FormMethod.Get, new { @id = "#searchForm" }))
            {
                
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                                       

                
            
            #line default
            #line hidden
            
            #line 19 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.FormTextFor(m => m.Location, hintHtmlAttributes: new { id = "geoLocateContainer" }));

            
            #line default
            #line hidden
            
            #line 19 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                                                                         


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" class=\"button\"");

WriteLiteral(" id=\"search-button\"");

WriteLiteral(">Search</button>\r\n                </div>\r\n");

            
            #line 24 "..\..\Views\TraineeshipSearch\Index.cshtml"

                
            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.HiddenFor(m => m.WithinDistance));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                      ;
                
            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.HiddenFor(m => m.SortType));

            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                ;
                
            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.HiddenFor(m => m.Latitude));

            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                
                
            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.HiddenFor(m => m.Longitude));

            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                 
                
            
            #line default
            #line hidden
            
            #line 29 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.Hidden("Hash", Model.LatLonLocHash()));

            
            #line default
            #line hidden
            
            #line 29 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                           
                
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\TraineeshipSearch\Index.cshtml"
           Write(Html.HiddenFor(m=> m.ResultsPerPage));

            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\TraineeshipSearch\Index.cshtml"
                                                     ;
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

WriteAttribute("href", Tuple.Create(" href=\'", 1551), Tuple.Create("\'", 1601)
            
            #line 39 "..\..\Views\TraineeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create("", 1558), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.Helpdesk)
            
            #line default
            #line hidden
, 1558), false)
);

WriteLiteral(">Send us an email</a></li>\r\n                    </ul>\r\n                    <ul");

WriteLiteral(" class=\"list-text\"");

WriteLiteral(">\r\n                        <li>\r\n                            <a");

WriteLiteral(" href=\"https://www.gov.uk/find-traineeship\"");

WriteLiteral(">About traineeships</a>\r\n                        </li>\r\n                    </ul>" +
"\r\n                </div>\r\n            </div>\r\n        </aside>\r\n    </div>\r\n</di" +
"v>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("   \r\n");

WriteLiteral("    ");

            
            #line 54 "..\..\Views\TraineeshipSearch\Index.cshtml"
Write(Scripts.Render("~/bundles/nas/locationsearch"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <script>\r\n        $(\"#Location\").locationMatch({\r\n            url: \'");

            
            #line 57 "..\..\Views\TraineeshipSearch\Index.cshtml"
             Write(Url.Action("location", "Location"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            longitude: \'#");

            
            #line 58 "..\..\Views\TraineeshipSearch\Index.cshtml"
                     Write(Html.IdFor(m => m.Longitude));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            latitude: \'#");

            
            #line 59 "..\..\Views\TraineeshipSearch\Index.cshtml"
                    Write(Html.IdFor(m => m.Latitude));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            latlonhash: \'#");

            
            #line 60 "..\..\Views\TraineeshipSearch\Index.cshtml"
                      Write(Html.IdFor(m => m.Hash));

            
            #line default
            #line hidden
WriteLiteral("\'\r\n        });\r\n    </script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"https://maps.googleapis.com/maps/api/js?v=3\"");

WriteLiteral("></script>\r\n");

WriteLiteral("    ");

            
            #line 64 "..\..\Views\TraineeshipSearch\Index.cshtml"
Write(Scripts.Render("~/bundles/nas/geoLocater"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

});

        }
    }
}
#pragma warning restore 1591
