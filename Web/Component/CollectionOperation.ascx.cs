#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description: Display CAP list
 *
 *  Notes:
 *      $Id: CollectionOperation.ascx.cs 278296 2014-09-01 08:35:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;

using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for CollectionOperation.
    /// </summary>
    public partial class CollectionOperation : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// collection Id.
        /// </summary>
        private long? _collectionId;

        /// <summary>
        ///  my Collection Model
        /// </summary>
        private MyCollectionModel _myCollectionModel = null;

        /// <summary>
        ///  simple Cap Model List.
        /// </summary>
        private SimpleCapModel[] _simpleCapModelList = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets collection id.
        /// </summary>
        public long? CollectionId
        {
            get
            {
                return _collectionId;
            }

            set
            {
                _collectionId = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected Item field client ID
        /// </summary>
        public string SelectedItemsFieldClientID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets MyCollectionModel4WS
        /// </summary>
        public MyCollectionModel MyCollectionModel
        {
            get
            {
                return _myCollectionModel;
            }

            set
            {
                _myCollectionModel = value;
            }
        }

        /// <summary>
        /// Gets or sets SimpleCapModel4WS list.
        /// </summary>
        public SimpleCapModel[] SimpleCapModelList
        {
            get
            {
                return _simpleCapModelList;
            }

            set
            {
                _simpleCapModelList = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is right to left or not.
        /// </summary>
        protected bool IsRightToLeft
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IsRightToLeft = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
                if (!AppSession.IsAdmin)
                {
                    //Register client event for copy to and move to action.
                    lnkCopy.Attributes.Add("onclick", string.Format("copy2Collection_{0}(event, '{1}','{2}');return false;", lnkCopy.ClientID, divCopy.ClientID, OperateCAPs2Collection.RDOExistCollectionID));
                    lnkMove.Attributes.Add("onclick", string.Format("move2Collection_{0}(event, '{1}','{2}');return false;", lnkMove.ClientID, divCopy.ClientID, OperateCAPs2Collection.RDOExistCollectionID));
                    lnkRemove.Attributes.Add("onclick", string.Format("return ConfirmRemoveCap_{0}();", lnkRemove.ClientID));
                }
            }
            else
            {
                //Get selected items when click add button.
                if (!string.IsNullOrEmpty(Request.Form["__EVENTTARGET"]))
                {
                    if (Request.Form["__EVENTTARGET"].IndexOf("btnAdd") > -1 || Request.Form["__EVENTTARGET"].IndexOf("lnkRemove") > -1)
                    {
                        OperateCAPs2Collection.SimpleCapModelList = _simpleCapModelList;
                        OperateCAPs2Collection.MyCollectionModel = _myCollectionModel;
                        OperateCAPs2Collection.ActionType = hfActionFlag.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Remove selected CAPs from my collection.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RemoveLink_Click(object sender, EventArgs e)
        {
            IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();

            SimpleCapModel[] simpleCapModelList = _simpleCapModelList;

            MyCollectionModel myCollection = new MyCollectionModel();
            myCollection.auditID = AppSession.User.PublicUserId;
            myCollection.serviceProviderCode = ConfigManager.AgencyCode;
            myCollection.userId = AppSession.User.PublicUserId;

            myCollection.collectionId = _collectionId;
            myCollection.simpleCapModels = simpleCapModelList;

            myCollectionBll.RemoveCapsFromCollection(myCollection);

            //string script = string.Format("Reload('{0}')", GetTextByKey("mycollection_collectiondetailpage_remove_message"));
            ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "click", "Reload('')", true);
        }

        #endregion Methods
    }
}