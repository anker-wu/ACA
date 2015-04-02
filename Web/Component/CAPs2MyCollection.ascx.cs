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
 *      $Id: CAPs2MyCollection.ascx.cs 277634 2014-08-19 08:16:23Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Add selected CAPs to my collection.
    /// </summary>
    public partial class CAPs2MyCollection : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// operation type for add CAPs to collection.
        /// </summary>
        private string _actionType = string.Empty;

        /// <summary>
        /// judge selected CAPs whether contains Partial CAP.
        /// </summary>
        private bool _isContainPartialCap = false;

        /// <summary>
        /// my collection model.
        /// </summary>
        private MyCollectionModel _myCollectionModel = null;

        /// <summary>
        /// simple cap model list.
        /// </summary>
        private SimpleCapModel[] _simpleCapModelList = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets action is below
        /// Move to, Copy to.
        /// </summary>
        public string ActionType
        {
            get
            {
                return _actionType;
            }

            set
            {
                _actionType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether selected CAPs contain partial CAP.
        /// </summary>
        public bool IsContainPartialCap
        {
            get
            {
                return _isContainPartialCap;
            }

            set
            {
                _isContainPartialCap = value;
            }
        }

        /// <summary>
        /// Gets or sets my collection model.
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
        /// Gets or sets simple CAP model list.
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
        ///  Gets radio exists collection control id
        /// </summary>
        public string RDOExistCollectionID
        {
           get
           {
               return rdoExistCollection.ClientID;
           }
        }

        /// <summary>
        ///  Gets radio new collection control id
        /// </summary>
        public string RDONewCollectionID
        {
            get
            {
                return rdoNewCollection.ClientID;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// just set any char to the name control to make 
        /// the control required validation past in client
        /// when this control is invisible in client
        /// </summary>
        public void SetCollectionDefaultValue()
        {
            txtName.Text = "name";
        }

        /// <summary>
        /// Page load.
        /// </summary>
        /// <param name="sender"> event object </param>
        /// <param name="e"> event data </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //It used for ACA admin.
                if (AppSession.IsAdmin)
                {
                    ddlMyCollection.Visible = false;
                    rdoExistCollection.Checked = true;
                }
                else
                {
                    MyCollectionModel[] myCollections = AppSession.GetMyCollectionsFromSession();

                    if (myCollections == null)
                    {
                        IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();
                        myCollections = myCollectionBll.GetMyCollection(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);
                    }

                    //Initialize operation.
                    InitMyCollection(myCollections);

                    //Register events.
                    this.RegisterEvents();
                }
            }

            this.rdoExistCollection.InputAttributes.Add("title", LabelUtil.GetTextByKey("mycollection_addpage_lablel_exsisting", ModuleName));
            this.rdoNewCollection.InputAttributes.Add("title", LabelUtil.GetTextByKey("mycollection_addpage_lablel_new", ModuleName));
        }

        /// <summary>
        /// Add selected CAPs to my collection.
        /// </summary>
        /// <param name="sender"> event object </param>
        /// <param name="e"> event data </param>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();
            MyCollectionModel goalCollection = GetGoalMyCollectionModel();
            MyCollectionModel sourceCollection = null;
            MyCollectionModel[] myCollections = AppSession.GetMyCollectionsFromSession();

            //MyCollectionModel is null in CAPHome and Detail page.
            if (MyCollectionModel != null)
            {
                sourceCollection = GetSourceMyCollectionModel();
            }

            if (goalCollection == null)
            {
                //Just select partial cap.
                if (_isContainPartialCap)
                {
                    MessageUtil.ShowMessageByControl(Page, MessageType.Notice, LabelUtil.GetTextByKey("mycollection_caphomepage_message_havepartial", ModuleName));
                }
                else
                {
                    MessageUtil.ShowMessageByControl(Page, MessageType.Notice, LabelUtil.GetTextByKey("mycollection_caphomepage_message_nocapselected", ModuleName));
                }

                InitMyCollection(myCollections);
                return;
            }

            string script = string.Empty;
            string errorMessage = string.Empty;
            string addSuccussMessage = string.Empty;
            bool isCreateCollection = false;

            try
            {
                string messageAdding = GetTextByKey("mycollection_mypermitspage_message_adding").Replace("'", "\\'");
                string scriptMessageAdding = string.Format("displayAddingMessage('{0}')", messageAdding);

                switch (_actionType)
                {
                    case ACAConstant.MOVE_TO:
                        myCollectionBll.MoveCaps2Collection(sourceCollection, goalCollection);

                        addSuccussMessage = GetTextByKey("mycollection_collectiondetailpage_move_message").Replace("'", "\\'");
                        script = string.Format("Reload('{0}')", addSuccussMessage);
                        break;
                    case ACAConstant.COPY_TO:
                        ScriptManager.RegisterClientScriptBlock(updatePanelAdd, this.GetType(), "Adding", scriptMessageAdding, true);
                        myCollectionBll.AddCaps2Collection(goalCollection);

                        if (this.rdoNewCollection.Checked)
                        {
                            isCreateCollection = true;
                        }

                        addSuccussMessage = GetTextByKey("mycollection_mypermitpage_message_added");
                        script = string.Format("displayAddedMessage('{0}','{1}')", addSuccussMessage, isCreateCollection);
                        break;
                    default:
                        ScriptManager.RegisterClientScriptBlock(updatePanelAdd, this.GetType(), "Adding", scriptMessageAdding, true);
                        myCollectionBll.AddCaps2Collection(goalCollection);

                        if (_isContainPartialCap)
                        {
                            addSuccussMessage = GetTextByKey("mycollection_add2collection_succussfully_message");
                        }
                        else
                        {
                            addSuccussMessage = GetTextByKey("mycollection_mypermitpage_message_added");
                        }

                        if (this.rdoNewCollection.Checked)
                        {
                            isCreateCollection = true;
                        }

                        script = string.Format("displayAddedMessage('{0}','{1}')", addSuccussMessage, isCreateCollection);
                        break;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ExceptionUtil.GetErrorMessage(ex);
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageUtil.ShowMessageByControl(this.Page, MessageType.Notice, errorMessage);
                InitMyCollection(myCollections);
                return;
            }

            ScriptManager.RegisterClientScriptBlock(updatePanelAdd, this.GetType(), "Added", script, true);

            myCollections = myCollectionBll.GetMyCollection(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);

            //Add my collection models to session.
            AppSession.SetMyCollectionsToSession(myCollections);

            InitMyCollection(myCollections);
        }

        /// <summary>
        /// Build my collections dropdownlist control.
        /// </summary>
        /// <param name="myCollections"> my collection model. </param>
        private void BuildMyCollections(MyCollectionModel[] myCollections)
        {
            IList<ListItem> items = new List<ListItem>();
            ListItem item = null;

            if (myCollections == null || myCollections.Length == 0)
            {
                return;
            }

            foreach (MyCollectionModel myCollectionModel in myCollections)
            {
                item = new ListItem();
                item.Value = myCollectionModel.collectionId.ToString();
                item.Text = myCollectionModel.collectionName;
                items.Add(item);
            }

            DropDownListBindUtil.BindDDL(items, this.ddlMyCollection, true);
        }

        /// <summary>
        /// Get goal my collection model.
        /// </summary>
        /// <returns> my collection model. </returns>
        private MyCollectionModel GetGoalMyCollectionModel()
        {
            MyCollectionModel goalCollection = new MyCollectionModel();

            goalCollection.auditID = AppSession.User.PublicUserId;
            goalCollection.serviceProviderCode = ConfigManager.SuperAgencyCode;
            goalCollection.userId = AppSession.User.PublicUserId;

            goalCollection.collectionName = txtName.Text.Trim();
            goalCollection.collectionDescription = txtDesc.Text.Trim();

            if (_simpleCapModelList == null || _simpleCapModelList.Length == 0)
            {
                return null;
            }

            goalCollection.simpleCapModels = _simpleCapModelList;

            //Select existing collecion.
            if (this.rdoExistCollection.Checked)
            {
                if (!string.IsNullOrEmpty(this.ddlMyCollection.SelectedValue))
                {
                    goalCollection.collectionId = Convert.ToInt64(this.ddlMyCollection.SelectedValue);
                }
            }

            return goalCollection;
        }

        /// <summary>
        /// Get source my collection model.
        /// </summary>
        /// <returns> my collection model. </returns>
        private MyCollectionModel GetSourceMyCollectionModel()
        {
            MyCollectionModel sourceCollection = new MyCollectionModel();

            sourceCollection.auditID = AppSession.User.PublicUserId;
            sourceCollection.serviceProviderCode = ConfigManager.SuperAgencyCode;
            sourceCollection.userId = AppSession.User.PublicUserId;

            sourceCollection.collectionName = _myCollectionModel.collectionName;
            sourceCollection.collectionId = _myCollectionModel.collectionId;
            sourceCollection.collectionDescription = _myCollectionModel.collectionDescription;

            sourceCollection.simpleCapModels = _simpleCapModelList;

            return sourceCollection;
        }

        /// <summary>
        /// Initialization my collection.
        /// </summary>
        /// <param name="myCollections"> my collection model list. </param>
        private void InitMyCollection(MyCollectionModel[] myCollections)
        {
            BuildMyCollections(myCollections);

            if (myCollections == null || myCollections.Length == 0)
            {
                this.rdoExistCollection.Visible = false;
                this.lblExistingCollection.Visible = false;
                this.ddlMyCollection.Visible = false;
                this.rdoNewCollection.Checked = true;
                this.rdoExistCollection.Checked = false;
                this.txtName.Enabled = true;
                this.txtDesc.Enabled = true;
            }
            else
            {
                this.rdoExistCollection.Checked = true;
                this.rdoNewCollection.Checked = false;
                this.txtName.Enabled = false;
                this.txtDesc.Enabled = false;
                this.txtName.Text = string.Empty;
                this.txtDesc.Text = string.Empty;
                this.rdoExistCollection.Visible = true;
                this.ddlMyCollection.Visible = true;
                this.ddlMyCollection.Enabled = true;

                if (IsPostBack)
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanelAdd, this.GetType(), "SetAddButtonDiabled", string.Format("SetAddButtonDiabled_{0}();", btnAdd.ClientID), true);
                }
            }
        }

        /// <summary>
        /// Register events for controls.
        /// </summary>
        private void RegisterEvents()
        {
            this.rdoExistCollection.Attributes.Add("onclick", string.Format("rdoCollectionClick_{0}(this);", this.rdoExistCollection.ClientID));
            this.rdoNewCollection.Attributes.Add("onclick", string.Format("rdoCollectionClick_{0}(this);", this.rdoNewCollection.ClientID));
            this.ddlMyCollection.Attributes.Add("onchange", string.Format("ddlMyCollectionChanged_{0}();", this.ddlMyCollection.ClientID));
            this.txtName.Attributes.Add("onpropertychange", string.Format("collectionNameChanged_{0}();", this.txtName.ClientID));
            this.txtName.Attributes.Add("oninput", string.Format("collectionNameChanged_{0}();", this.txtName.ClientID));
            this.btnAdd.Attributes.Add("onclick", string.Format("return CloseAddForm_{0}(this)", this.btnAdd.ClientID));
            this.btnCancel.Attributes.Add("onclick", string.Format("ExitAddForm_{0}(this);return false;", this.btnCancel.ClientID));
            btnCancel.Attributes.Add("onkeydown", "focusCurrentObj(event)");
            this.txtName.Attributes.Add("onblur", "AdjustCollectionNameStyle_" + this.ClientID + "();");
        }

        #endregion Methods
    }
}