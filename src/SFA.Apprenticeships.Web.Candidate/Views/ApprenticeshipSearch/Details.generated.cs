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
    
    #line 1 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;
    
    #line default
    #line hidden
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/Details.cshtml")]
    public partial class Details : System.Web.Mvc.WebViewPage<VacancyDetailViewModel>
    {
        public Details()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
  
    ViewBag.Title = "Apprenticeships - " + Model.Title;
    Layout = "~/Views/Shared/_ApprenticeshipLayout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

DefineSection("metatags", () => {

WriteLiteral("\r\n    <meta");

WriteLiteral(" name=\"DCSext.Days2Close\"");

WriteAttribute("content", Tuple.Create(" content=\"", 284), Tuple.Create("\"", 336)
            
            #line 10 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
, Tuple.Create(Tuple.Create("", 294), Tuple.Create<System.Object, System.Int32>((Model.ClosingDate - DateTime.Now).Days
            
            #line default
            #line hidden
, 294), false)
);

WriteLiteral("/>\r\n");

});

WriteLiteral("\r\n<div itemscope");

WriteLiteral(" itemtype=\"http://schema.org/JobPosting\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"hgroup text\"");

WriteLiteral(">\r\n                <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(" id=\"vacancy-title\"");

WriteLiteral(" itemprop=\"title\"");

WriteLiteral(">");

            
            #line 17 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                          Write(Model.Title);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n                <p");

WriteLiteral(" class=\"subtitle\"");

WriteLiteral(" id=\"vacancy-subtitle-employer-name\"");

WriteLiteral(" itemprop=\"hiringOrganization\"");

WriteLiteral(">");

            
            #line 18 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                                 Write(Model.EmployerName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n");

            
            #line 22 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
            
            
            #line default
            #line hidden
            
            #line 22 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
             if (ViewBag.SearchReturnUrl != null)
            {

            
            #line default
            #line hidden
WriteLiteral("                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 893), Tuple.Create("\"", 924)
            
            #line 24 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
, Tuple.Create(Tuple.Create("", 900), Tuple.Create<System.Object, System.Int32>(ViewBag.SearchReturnUrl
            
            #line default
            #line hidden
, 900), false)
);

WriteLiteral(" title=\"Return to search results\"");

WriteLiteral(" class=\"page-link\"");

WriteLiteral(" id=\"lnk-return-search-results\"");

WriteLiteral(">Return to search results</a>\r\n");

            
            #line 25 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
            }
            else
            {
                
            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
           Write(Html.RouteLink("Find an apprenticeship", CandidateRouteNames.ApprenticeshipSearch, null, new { @id="lnk-find-apprenticeship", @class = "page-link" }));

            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                                                                                                      
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n\r\n");

            
            #line 33 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
    
            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
     if (!Model.HasError())
    {

            
            #line default
            #line hidden
WriteLiteral("        <section");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(" itemprop=\"description\"");

WriteLiteral(">\r\n                        <p");

WriteLiteral(" id=\"vacancy-description\"");

WriteLiteral(">");

            
            #line 39 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                               Write(Model.Description);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n   " +
"         <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 44 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
           Write(Html.Partial("_Apply", Model, new ViewDataDictionary() { new KeyValuePair<string, object>("AnalyticsButtonPosition", "Top")}));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </section>\r\n");

WriteLiteral("        <section");

WriteLiteral(" class=\"section-border grid-wrapper\"");

WriteLiteral(" id=\"vacancy-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Apprenticeship summary</h2>\r\n            <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Weekly wage</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-wage\"");

WriteLiteral(">");

            
            #line 52 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                    Write(Model.Wage);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Working week</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-working-week\"");

WriteLiteral(" itemprop=\"workHours\"");

WriteLiteral(">");

            
            #line 55 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                 Write(Model.WorkingWeek);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Apprenticeship duration</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-expected-duration\"");

WriteLiteral(">");

            
            #line 57 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                  Write(string.IsNullOrWhiteSpace(@Model.ExpectedDuration) ? "Not specified" : @Model.ExpectedDuration);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Possible start date</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-start-date\"");

WriteLiteral(">");

            
            #line 59 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                          Write(Html.DisplayFor(m => Model.StartDate));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 60 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 60 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                     if (Model.VacancyLocationType != ApprenticeshipLocationType.National && Model.Distance != null)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Distance</h3>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" id=\"vacancy-distance\"");

WriteLiteral(">");

            
            #line 63 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                            Write(Model.Distance);

            
            #line default
            #line hidden
WriteLiteral(" miles</p>\r\n");

            
            #line 64 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Apprenticeship level</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-type\"");

WriteLiteral(" itemprop=\"employmentType\"");

WriteLiteral(">");

            
            #line 66 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                              Write(Model.ApprenticeshipLevel);

            
            #line default
            #line hidden
WriteLiteral(" Level Apprenticeship</p>\r\n\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Reference number</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-reference-id\"");

WriteLiteral(">");

            
            #line 69 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                            Write(Model.VacancyReference);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(" itemprop=\"responsibilities\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" id=\"vacancy-full-descrpition\"");

WriteLiteral(">");

            
            #line 74 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                Write(Html.Raw(Model.FullDescription));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </div>\r\n\r\n        </section>\r\n");

WriteLiteral("        <section");

WriteLiteral(" class=\"section-border grid-wrapper\"");

WriteLiteral(" id=\"course-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Requirements and prospects</h2>\r\n            <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Desired skills</h3>\r\n                        <p");

WriteLiteral(" id=\"vacancy-skills-required\"");

WriteLiteral(" itemprop=\"skills\"");

WriteLiteral(">");

            
            #line 86 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                     Write(Html.Raw(Model.SkillsRequired));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Qualifications required</h3>\r\n                        <p");

WriteLiteral(" id=\"vacancy-qualifications-required\"");

WriteLiteral(" itemprop=\"qualifications\"");

WriteLiteral(">");

            
            #line 88 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                     Write(Html.Raw(Model.QualificationRequired));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n   " +
"         <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n");

            
            #line 94 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 94 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                     if (!string.IsNullOrWhiteSpace(Model.FutureProspects))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Future prospects</h3>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" id=\"vacancy-future-prospects\"");

WriteLiteral(" itemprop=\"incentives\"");

WriteLiteral(">");

            
            #line 97 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                          Write(Html.Raw(Model.FutureProspects));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 98 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 99 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                     if (!string.IsNullOrWhiteSpace(Model.RealityCheck))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Things to consider</h3>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" id=\"vacancy-reality-check\"");

WriteLiteral(" itemprop=\"incentives\"");

WriteLiteral(">");

            
            #line 102 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                       Write(Html.Raw(Model.RealityCheck));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 103 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n            </div>\r\n        </section>\r\n");

WriteLiteral("        <section");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(" id=\"employer-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">About the employer</h2>\r\n");

            
            #line 109 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
            
            
            #line default
            #line hidden
            
            #line 109 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
             if (!Model.IsEmployerAnonymous)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" id=\"vacancy-employer-description\"");

WriteLiteral(">");

            
            #line 112 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                    Write(Html.Raw(Model.EmployerDescription));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n");

            
            #line 114 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            \r\n            <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                        <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Employer</h3>\r\n                            <p");

WriteLiteral(" id=\"vacancy-employer-name\"");

WriteAttribute("class", Tuple.Create(" class=\"", 5987), Tuple.Create("\"", 6057)
            
            #line 120 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
, Tuple.Create(Tuple.Create("", 5995), Tuple.Create<System.Object, System.Int32>(Model.IsWellFormedEmployerWebsiteUrl ? "no-btm-margin" : ""
            
            #line default
            #line hidden
, 5995), false)
);

WriteLiteral(">");

            
            #line 120 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                                                            Write(Model.EmployerName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 121 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 121 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                             if (Model.IsWellFormedEmployerWebsiteUrl)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <p><a");

WriteLiteral(" itemprop=\"url\"");

WriteAttribute("href", Tuple.Create(" href=\"", 6239), Tuple.Create("\"", 6268)
            
            #line 123 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
, Tuple.Create(Tuple.Create("", 6246), Tuple.Create<System.Object, System.Int32>(Model.EmployerWebsite
            
            #line default
            #line hidden
, 6246), false)
);

WriteLiteral(" id=\"vacancy-employer-website\"");

WriteLiteral(" target=\"_blank\"");

WriteAttribute("title", Tuple.Create(" title=\"", 6315), Tuple.Create("\"", 6350)
            
            #line 123 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                        , Tuple.Create(Tuple.Create("", 6323), Tuple.Create<System.Object, System.Int32>(Model.EmployerName
            
            #line default
            #line hidden
, 6323), false)
, Tuple.Create(Tuple.Create(" ", 6342), Tuple.Create("Website", 6343), true)
);

WriteLiteral(" rel=\"external\"");

WriteLiteral(">");

            
            #line 123 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                                                                                                               Write(Model.EmployerWebsite);

            
            #line default
            #line hidden
WriteLiteral("</a></p>\r\n");

            
            #line 124 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                            }
                            else
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <p>");

            
            #line 127 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                              Write(Model.EmployerWebsite);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 128 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                        ");

            
            #line 129 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                         if (Model.VacancyLocationType != ApprenticeshipLocationType.National && !Model.IsMultiLocation)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <div");

WriteLiteral(" id=\"vacancy-address\"");

WriteLiteral(" itemscope");

WriteLiteral(" itemtype=\"http://schema.org/PostalAddress\"");

WriteLiteral(">\r\n                                <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Address</h3>\r\n                                <div");

WriteLiteral(" itemprop=\"address\"");

WriteLiteral(">\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"streetAddress\"");

WriteLiteral(">");

            
            #line 134 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                    Write(Model.VacancyAddress.AddressLine1);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"streetAddress\"");

WriteLiteral(">");

            
            #line 135 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                    Write(Model.VacancyAddress.AddressLine2);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"addressLocality\"");

WriteLiteral(">");

            
            #line 136 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                      Write(Model.VacancyAddress.AddressLine3);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"addressRegion\"");

WriteLiteral(">");

            
            #line 137 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                    Write(Model.VacancyAddress.AddressLine4);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"postalCode\"");

WriteLiteral(">");

            
            #line 138 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                 Write(Model.VacancyAddress.Postcode);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                </div>\r\n                            </div>\r" +
"\n");

            
            #line 141 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </div>\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n");

            
            #line 145 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 145 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                     if (Model.VacancyLocationType != ApprenticeshipLocationType.National && !Model.IsMultiLocation)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" id=\"vacancy-map\"");

WriteLiteral(" class=\"ad-details__map\"");

WriteLiteral(">\r\n                            <div");

WriteLiteral(" class=\"map-overlay\"");

WriteLiteral(" onclick=\" style.pointerEvents = \'none\' \"");

WriteLiteral("></div>\r\n                            <iframe");

WriteLiteral(" width=\"700\"");

WriteLiteral(" height=\"250\"");

WriteLiteral(" title=\"Map of location\"");

WriteLiteral(" style=\"border: 0\"");

WriteAttribute("src", Tuple.Create(" src=\"", 8231), Tuple.Create("\"", 8386)
, Tuple.Create(Tuple.Create("", 8237), Tuple.Create("https://www.google.com/maps/embed/v1/place?q=", 8237), true)
            
            #line 149 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                         , Tuple.Create(Tuple.Create("", 8282), Tuple.Create<System.Object, System.Int32>(Html.Raw(Model.VacancyAddress.Postcode)
            
            #line default
            #line hidden
, 8282), false)
, Tuple.Create(Tuple.Create("", 8322), Tuple.Create(",+United+Kingdom&amp;key=AIzaSyCusA_0x4bJEjU-_gLOFiXMSBXKZYtvHz8", 8322), true)
);

WriteLiteral("></iframe>\r\n                            <p");

WriteLiteral(" class=\"nojs-notice\"");

WriteLiteral(">You must have JavaScript enabled to view a map of the location</p>\r\n            " +
"            </div>\r\n");

            
            #line 152 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n            </div>\r\n        </section>\r\n");

WriteLiteral("        <section");

WriteLiteral(" class=\"section-border grid-wrapper\"");

WriteLiteral(" id=\"provider-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Training provider</h2>\r\n");

            
            #line 158 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
            
            
            #line default
            #line hidden
            
            #line 158 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
             if (Model.IsNasProvider)
            {

            
            #line default
            #line hidden
WriteLiteral("                <p");

WriteLiteral(" id=\"vacancy-nas-provider\"");

WriteLiteral(">Training provider selection is currently in progress</p>\r\n");

            
            #line 161 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
            }
            else
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n");

            
            #line 167 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 167 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                             if (!string.IsNullOrWhiteSpace(Model.TrainingToBeProvided))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <p");

WriteLiteral(" id=\"vacancy-training-to-be-provided\"");

WriteLiteral(">");

            
            #line 169 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                   Write(Html.Raw(Model.TrainingToBeProvided));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 170 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                            <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Apprenticeship framework</h3>\r\n                            <p");

WriteLiteral(" id=\"vacancy-framework\"");

WriteLiteral(">");

            
            #line 172 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                 Write(Html.Raw(Model.Framework));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 173 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 173 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                             if (Model.ProviderSectorPassRate.HasValue)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <p");

WriteLiteral(" id=\"vacancy-provider-sector-pass-rate\"");

WriteLiteral(">The training provider has achieved a sector success rate of ");

            
            #line 175 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                                                                 Write(Model.ProviderSectorPassRate);

            
            #line default
            #line hidden
WriteLiteral("% for this type of apprenticeship training.</p>\r\n");

            
            #line 176 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                        </div>\r\n                    </div>\r\n                </div" +
">\r\n");

WriteLiteral("                <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Training provider</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-provider-name\"");

WriteLiteral(">");

            
            #line 182 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                             Write(Model.ProviderName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                \r\n");

            
            #line 184 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 184 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                     if (!Model.IsRecruitmentAgencyAnonymous && !string.IsNullOrWhiteSpace(Model.RecruitmentAgency))
                    {

            
            #line default
            #line hidden
WriteLiteral("                       <p");

WriteLiteral(" id=\"recruitment-agency-name\"");

WriteLiteral(">Applications for this apprenticeship are being processed by ");

            
            #line 186 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                                                                              Write(Model.RecruitmentAgency);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 187 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 189 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 189 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                     if (!string.IsNullOrWhiteSpace(Model.Contact))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Contact</h3>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" id=\"vacancy-provider-contact\"");

WriteLiteral(">");

            
            #line 192 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                    Write(Model.Contact);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 193 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n");

            
            #line 195 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </section>\r\n");

            
            #line 197 "..\..\Views\ApprenticeshipSearch\Details.cshtml"

        if (!string.IsNullOrWhiteSpace(Model.OtherInformation))
        {

            
            #line default
            #line hidden
WriteLiteral("            <section");

WriteLiteral(" id=\"other-information\"");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(">\r\n                <h2");

WriteLiteral(" class=\"heading-large collpanel-trigger\"");

WriteLiteral(">Other information</h2>\r\n                <div");

WriteLiteral(" class=\"text collpanel toggle-content\"");

WriteLiteral(">\r\n                    <p>");

            
            #line 203 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                  Write(Html.Raw(Model.OtherInformation));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </section>\r\n");

            
            #line 206 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
        }

        if (Model.ApplyViaEmployerWebsite && !string.IsNullOrWhiteSpace(Model.ApplicationInstructions))
        {      

            
            #line default
            #line hidden
WriteLiteral("            <section");

WriteLiteral(" id=\"application-instructions-container\"");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(">\r\n                <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Employer\'s Application Instructions</h2>\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" id=\"application-instructions\"");

WriteLiteral(">");

            
            #line 213 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
                                                Write(Model.ApplicationInstructions);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </section>\r\n");

            
            #line 216 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
        }
        
        if (!Model.ApplyViaEmployerWebsite && !Request.IsAuthenticated)
        {

            
            #line default
            #line hidden
WriteLiteral("            <section");

WriteLiteral(" id=\"before-apply\"");

WriteLiteral(">\r\n                <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Before you apply</h2>\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                    <p>Before you apply for an apprenticeship you must first c" +
"reate an account. If you already have an account you\'ll need to sign in.</p>\r\n  " +
"              </div>\r\n            </section>\r\n");

            
            #line 226 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
        }


            
            #line default
            #line hidden
WriteLiteral("        <section>\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 230 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
           Write(Html.Partial("_Apply", Model, new ViewDataDictionary() { new KeyValuePair<string, object>("AnalyticsButtonPosition", "Bottom")}));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </section>\r\n");

            
            #line 233 "..\..\Views\ApprenticeshipSearch\Details.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
