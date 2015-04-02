#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MyCollectionManagement.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  MyCollectionManagement page
 *
 *  Notes:
 * $Id: MyCollectionManagement.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.MyCollection
{
    /// <summary>
    /// the class for my collection management.
    /// </summary>
    public partial class MyCollectionManagement : BasePage
    {
        #region Methods

        /// <summary>
        /// page load.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();
                MyCollectionModel[] myCollections = myCollectionBll.GetCollections4Management(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);
                if (AppSession.IsAdmin)
                {
                    divAddCaps2Collection.Visible = true;
                    MyCollectionList.BindMyCollectionList(null);
                }
                else
                {
                    MyCollectionList.BindMyCollectionList(myCollections);
                }
            }
        }

        #endregion Methods
    }
}