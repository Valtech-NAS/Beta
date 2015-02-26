// WebTrends SmartSource Data Collector Tag v10.4.1
// Copyright (c) 2014 Webtrends Inc.  All rights reserved.
// Tag Builder Version: 4.1.3.2
// Created: 2014.03.27
window.webtrendsAsyncInit = function () {
    //alert(window.WebTrendsDscId);
    var dcs=new Webtrends.dcs().init({
        dcsid: window.WebTrendsDscId,
        domain:"stats.matraxis.net",
        timezone:0,
        i18n:true,
        adimpressions:true,
        adsparam:"WT.ac",
        offsite:true,
        download:true,
        downloadtypes: "xls,doc,pdf,txt,csv,zip,docx,xlsx,rar,gzip",
        anchor:true,
        onsitedoms: window.WebTrendsDomainName,
        plugins:{
            //hm:{src:"//s.webtrends.com/js/webtrends.hm.js"}
        }
        }).track();
};
(function () {
   
    var s=document.createElement("script"); s.async=true; s.src="/Content/_assets/js/webtrends/webtrends.min.js";    
    var s2=document.getElementsByTagName("script")[0]; s2.parentNode.insertBefore(s,s2);
}());
