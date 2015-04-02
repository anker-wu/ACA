// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System;
using System.Web.UI;


[assembly: WebResource("AjaxControlToolkit.Animation.Animations.js", "text/javascript")]
[assembly: WebResource("AjaxControlToolkit.Animation.Animations.debug.js", "text/javascript")]

namespace AjaxControlToolkit
{
    /// <summary>
    /// The AnimationScripts class is used to load all of the animation support for the AJAX
    /// Control Toolkit.  To use any of the animations you find in Animations.js, simply include
    /// the attribute [RequiredScript(typeof(AnimationScripts))] on your extender.
    /// </summary>
    [ClientScriptResource(null, "AjaxControlToolkit.Animation.Animations.js")]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [RequiredScript(typeof(TimerScript))]
    public static class AnimationScripts
    {
    }
}
