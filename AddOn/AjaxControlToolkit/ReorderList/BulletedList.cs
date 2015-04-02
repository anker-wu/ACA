// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.


using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace AjaxControlToolkit
{
    /// <summary>
    /// We create our own BulletedList because the ASP.NET one
    /// Takes ListItems rather than regular controls as it's children.
    /// </summary>
    /// 
    [ToolboxItem(false)]
    public class BulletedList : WebControl
    {
        public BulletedList()
        {            
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {                
                return HtmlTextWriterTag.Ul;
            }
        }
    }
}
