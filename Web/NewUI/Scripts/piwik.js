/**
* <pre>
*
*  Accela
*  File: Piwik.js
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description:
*  Piwik is the leading open source web analytics platform that  
*  gives you valuable insights into your website's 
*  visitors, your marketing campaigns and much more, so you can optimize your strategy and online experience of your visitors.
*  Notes:
*  $Id: Piwik.js 185465 2010-11-29 08:27:07Z karthikeyan.rajmohan $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var _paq = _paq || [];
  _paq.push(["trackPageView"]);
  _paq.push(["enableLinkTracking"]);

  (function() {
    var u=(("https:" == document.location.protocol) ? "https" : "http") + "://rbuipiwik.cloudapp.net:81/piwik/";
    _paq.push(["setTrackerUrl", u+"piwik.php"]);
    _paq.push(["setSiteId", "5"]);
    var d=document, g=d.createElement("script"), s=d.getElementsByTagName("script")[0]; g.type="text/javascript";
    g.defer=true; g.async=true; g.src=u+"piwik.js"; s.parentNode.insertBefore(g,s);
  })();
