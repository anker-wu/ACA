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
 *      $Id: CollectionEdit.ascx.cs 277634 2014-08-19 08:16:23Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;

using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for CollectionEdit.
    /// </summary>
    public partial class CollectionEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// collection description.
        /// </summary>
        private string _collectionDesc = string.Empty;

        /// <summary>
        /// collection name.
        /// </summary>
        private string _collectionName = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets my collection description.
        /// </summary>
        public string CollectionDesc
        {
            get
            {
                return _collectionDesc;
            }

            set
            {
                _collectionDesc = value;
            }
        }

        /// <summary>
        /// Gets or sets my collection id.
        /// </summary>
        public string CollectionId
        {
            get
            {
                if (ViewState["CollectionId"] != null)
                {
                    return ViewState["CollectionId"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                ViewState["CollectionId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets my collection name.
        /// </summary>
        public string CollectionName
        {
            get
            {
                return _collectionName;
            }

            set
            {
                _collectionName = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Page Load event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    this.txtName.Text = _collectionName;
                    this.txtDesc.Text = _collectionDesc;

                    string btnChangeId = this.btnChange.ClientID;
                    txtName.Attributes.Add("onpropertychange", "collectionNameChanged()");
                    txtName.Attributes.Add("oninput", "collectionNameChanged()");
                    txtName.Attributes.Add("onblur", "AdjustCollectionEditStyle_" + this.ClientID + "();");
                }
            }
        }

        /// <summary>
        /// Modify collection name.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ChangeButton_Click(object sender, EventArgs e)
        {
            IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();
            MyCollectionModel myCollection = new MyCollectionModel();

            string collectionName = txtName.Text.Trim();
            string collectionDesc = txtDesc.Text.Trim();

            myCollection.auditID = AppSession.User.PublicUserId;
            myCollection.serviceProviderCode = ConfigManager.AgencyCode;
            myCollection.userId = AppSession.User.PublicUserId;

            myCollection.collectionId = Convert.ToInt64(CollectionId);
            myCollection.collectionName = collectionName;
            myCollection.collectionDescription = collectionDesc;

            string renameSuccessMessage = GetTextByKey("mycollection_collectiondetailpage_message_rename");

            try
            {
                myCollectionBll.UpdateMyCollection(myCollection);
            }
            catch (Exception ex)
            {
                renameSuccessMessage = ExceptionUtil.GetErrorMessage(ex);
                MessageUtil.ShowAlertMessage(updatePanelCollectionEdit, renameSuccessMessage);
                return;
            }

            string script = "UpdateMyCollection('" + renameSuccessMessage + "')";
            ScriptManager.RegisterClientScriptBlock(updatePanelCollectionEdit, this.GetType(), "click", script, true);

            MyCollectionModel[] myCollections = myCollectionBll.GetMyCollection(ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            AppSession.SetMyCollectionsToSession(myCollections);
        }

        #endregion Methods
    }
}