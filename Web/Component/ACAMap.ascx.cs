#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ACAMap.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ACAMap.ascx.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// ACAMap class
    /// </summary>
    public partial class ACAMap : BaseUserControl
    {
        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ACAMap));

        #region Events

        /// <summary>
        /// Show On Map event handler.
        /// </summary>
        public event EventHandler ShowOnMap;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets XmlModel.
        /// </summary>
        public ACAGISModel ACAGISModel
        {
            get
            {
                if (string.IsNullOrEmpty(hfldXml.Value))
                {
                    return null;
                }

                return (ACAGISModel)SerializationUtil.XmlDeserialize(hfldXml.Value, typeof(ACAGISModel));
            }

            set
            {
                if (value != null)
                {
                    value.ResumeCommand = GetTextByKey("per_permitList_label_resumeApplication");
                    value.Windowless = true;
                    hfldXml.Value = SerializationUtil.XmlSerialize(value);
                }
                else
                {
                    hfldXml.Value = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets AGISContext.
        /// </summary>
        public string AGISContext
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets GISButtonLabelKey.
        /// </summary>
        public string GISButtonLabelKey
        {
            get
            {
                return btnGIS.LabelKey;
            }

            set
            {
                btnGIS.LabelKey = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether only display map frame without toolbar.
        /// </summary>
        public bool IsMiniMode
        {
            get
            {
                if (ViewState["IsMiniMode"] != null)
                {
                    return (bool)ViewState["IsMiniMode"];
                }

                return false;
            }

            set
            {
                if (value)
                {
                    dvMap.Attributes.Remove("class");
                    dvMap.Attributes.Add("class", "MiniMap_Container");
                    dvCancel.Visible = false;
                    dvGISButton.Visible = false;
                }
                else
                {
                    dvMap.Attributes.Remove("class");
                    dvMap.Attributes.Add("class", "Map_Container");
                    dvCancel.Visible = false;
                    dvGISButton.Visible = true;
                }

                ViewState["IsMiniMode"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto show map when load page or not.
        /// </summary>
        public bool IsAutoShow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether map is opened.
        /// </summary>
        public bool Opened
        {
            get
            {
                return dvMap.Visible;
            }
        }

        /// <summary>
        /// Gets or sets LastAgency
        /// </summary>
        public string LastAgency
        {
            get
            {
                return (string)ViewState["LastAgency"];
            }

            set
            {
                ViewState["LastAgency"] = value;
            }
        }

        /// <summary>
        /// Gets the script function name.
        /// </summary>
        protected string ScriptFunctionName
        {
            get
            {
                return string.Format("ShowMap{0}", ClientID);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Close Map.
        /// </summary>
        public void CloseMap()
        {
            updatePanel.Visible = true;
            dvMap.Visible = false;
            frmMap.Visible = false;
            frmMap.Attributes.Remove("src");
            frmMap.Attributes.Remove("onload");
            btnCancel.Visible = false;
            dvCancel.Visible = false;
            LastAgency = string.Empty;
        }

        /// <summary>
        /// Hide ACAMap.
        /// </summary>
        public void HideGISButton()
        {
            CloseMap();
            dvGISButton.Visible = false;
        }

        /// <summary>
        /// Show GIS Button.
        /// </summary>
        public void ShowGISButton()
        {
            dvGISButton.Visible = true;
        }

        /// <summary>
        /// Show ACAMap.
        /// </summary>
        public void ShowMap()
        {
            GISButton_Click(btnGIS, null);

            if (IsAutoShow)
            {
                dvGISButton.Visible = false;
                dvCancel.Visible = false;
            }
        }

        /// <summary>
        /// Initialize the ACAMap
        /// </summary>
        /// <param name="e">EventArgs object</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DialogUtil.RegisterScriptForDialog(this.Page);
            frmMap.Attributes.Add("name", frmMap.ClientID);
            btnGIS.ImageUrl = Accela.ACA.Web.Common.ImageUtil.GetImageURL("gis-normal.gif");
            btnGIS.Attributes.Add("onmouseout", "this.src='" + ImageUtil.GetImageURL("gis-normal.gif") + "'");
            btnGIS.Attributes.Add("onmouseover", "this.src='" + ImageUtil.GetImageURL("gis-over.gif") + "'");
            btnGIS.Attributes.Add("onmousedown", "this.src='" + ImageUtil.GetImageURL("gis-down.gif") + "'");
            btnGIS.AlternateText = GetTextByKey(GISButtonLabelKey);

            if (AppSession.IsAdmin)
            {
                btnGIS.Enabled = false;
                rblAgency.Visible = false;
            }

            if (!IsPostBack)
            {
                frmMap.Attributes.Add("title", LabelUtil.GetGlobalTextByKey("iframe_gisview_request_title"));
            }
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            CloseMap();
        }

        /// <summary>
        /// GIS button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void GISButton_Click(object sender, ImageClickEventArgs e)
        {
            if (ShowOnMap != null)
            {
                ShowOnMap(this, e);
            }

            if (!ACAGISModel.HasPermission4Show)
            {
                CloseMap();
                UpdatePanel2.Update();
                return;
            }

            if (ACAGISModel.IsMiniMap)
            {
                OpenMap();
                return;
            }

            if (!StandardChoiceUtil.IsSuperAgency())
            {
                ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(this.ACAGISModel);
                model.Agency = ConfigManager.AgencyCode;
                ACAGISModel = model;
                OpenMap();
                return;
            }

            if (Opened)
            {
                ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(this.ACAGISModel);
                model.Agency = LastAgency;
                ACAGISModel = model;
                if (e == null)
                {
                    return;
                }
            }

            List<string> agencyList = GISUtil.GetAgencies(ACAGISModel);

            if (agencyList != null && agencyList.Count == 1)
            {
                ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(this.ACAGISModel);
                model.Agency = agencyList[0];
                ACAGISModel = model;
                OpenMap();
            }
            else
            {
                if (agencyList != null && agencyList.Count == 0)
                {
                    IServiceProviderBll providerBll = ObjectFactory.GetObject(typeof(IServiceProviderBll)) as IServiceProviderBll;
                    string[] agencies = providerBll.GetSubAgencies(AppSession.User.PublicUserId);
                    if (agencies != null)
                    {
                        agencyList.AddRange(agencies);
                    }
                }

                if (agencyList != null && agencyList.Count == 1)
                {
                    ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(this.ACAGISModel);
                    model.Agency = agencyList[0];
                    ACAGISModel = model;
                    OpenMap();
                    return;
                }

                agencyList.Sort();
                rblAgency.Items.Clear();
                foreach (string item in agencyList)
                {
                    rblAgency.Items.Add(item);
                }

                rblAgency.Items[0].Selected = true;

                string pageTitle = GetTextByKey("aca_mappopup_label_pagetitle");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "SelectAgency", "ACADialog.popup({parentId:'" + mappopupdiv.ClientID + "',id:'" + popopContent.ClientID + "',buttonId:'" + divbutton.ClientID + "',title:'" + pageTitle + "',objectTarget:'" + btnGIS.ClientID + "',width:400,height:277});", true);
            }
        }

        /// <summary>
        /// Submit button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rblAgency.SelectedValue))
            {
                ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(ACAGISModel);
                model.Agency = rblAgency.SelectedValue;
                this.ACAGISModel = model;
                OpenMap();
            }
        }

        /// <summary>
        /// Filter ParcelInfoModel
        /// </summary>
        /// <param name="parcelInfos">ParcelInfo Model array</param>
        private static void FilterParcelInfoModel(ParcelInfoModel[] parcelInfos)
        {
            foreach (ParcelInfoModel apo in parcelInfos)
            {
                if (apo.RAddressModel != null && apo.RAddressModel.duplicatedAPOKeys != null)
                {
                    apo.RAddressModel.duplicatedAPOKeys = null;
                }

                if (apo.parcelModel != null && apo.parcelModel.duplicatedAPOKeys != null)
                {
                    apo.parcelModel.duplicatedAPOKeys = null;
                }

                if (apo.ownerModel != null && apo.ownerModel.duplicatedAPOKeys != null)
                {
                    apo.ownerModel.duplicatedAPOKeys = null;
                }
            }
        }

        /// <summary>
        /// Filter DuplicateAPOKey model.
        /// </summary>
        private void FilterDuplicateAPOKey()
        {
            ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(ACAGISModel);

            if (model != null && model.AddressInfoModels != null && model.AddressInfoModels.Length > 0)
            {
                FilterParcelInfoModel(model.AddressInfoModels);
            }

            if (model != null && model.ParcelInfoModels != null && model.ParcelInfoModels.Length > 0)
            {
                FilterParcelInfoModel(model.ParcelInfoModels);
            }

            if (model.ParcelModels != null && model.ParcelModels.Length > 0)
            {
                foreach (ParcelModel item in model.ParcelModels)
                {
                    if (item.duplicatedAPOKeys != null)
                    {
                        item.duplicatedAPOKeys = null;
                    }
                }
            }

            if (model.RefAddressModels != null && model.RefAddressModels.Length > 0)
            {
                foreach (RefAddressModel item in model.RefAddressModels)
                {
                    if (item.duplicatedAPOKeys != null)
                    {
                        item.duplicatedAPOKeys = null;
                    }
                }
            }

            ACAGISModel = model;
        }

        /// <summary>
        /// Open Map
        /// </summary>
        private void OpenMap()
        {
            dvMap.Visible = true;
            frmMap.Visible = true;
            Random rnd = new Random();
            SetCreateRecordMenus();
            SetModuleList();
            FilterDuplicateAPOKey();
            frmMap.Attributes.Add("src", FileUtil.AppendApplicationRoot("gis/agisrequest.aspx?" + UrlConstant.AgencyCode + "=" + Server.UrlEncode(ACAGISModel.Agency) + "&rnd=" + rnd.Next().ToString()));
            frmMap.Attributes.Add("onload", string.Format("{0}()", ScriptFunctionName));

            if (!IsMiniMode && !IsAutoShow)
            {
                btnCancel.Visible = true;
                dvCancel.Visible = true;
            }

            UpdatePanel2.Update();

            LastAgency = ACAGISModel.Agency;
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(this.hfldXml.Value);
            }
        }

        /// <summary>
        /// Set CreateRecordMenu for ACAGISModel
        /// </summary>
        private void SetCreateRecordMenus()
        {
            if (!FunctionTable.IsEnableCreateApplication())
            {
                return;
            }

            ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(ACAGISModel);
            Dictionary<string, string> modules = TabUtil.GetAllEnableModules(false);
            IList<TabItem> tabItems = TabUtil.GetTabList(AppSession.IsAdmin);

            if (!string.IsNullOrEmpty(model.ModuleName) && modules.ContainsValue(model.ModuleName))
            {
                foreach (KeyValuePair<string, string> item in modules)
                {
                    if (string.Equals(item.Value, model.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        model.ModuleKey = item.Key;
                        break;
                    }
                }
            }

            model.CreateRecordActions = new List<KeyValue>();
            foreach (TabItem item in tabItems)
            {
                if (string.IsNullOrEmpty(item.Module) || !item.TabVisible)
                {
                    continue;
                }

                if ((string.IsNullOrEmpty(model.ModuleName)
                    || (string.Equals(model.ModuleName, item.Module) && !string.IsNullOrEmpty(model.ModuleName)))
                    && modules.ContainsKey(item.Module)
                    && item.Children.Exists(f => f.Label == "aca_sys_feature_apply_a_permit"))
                {
                    KeyValue kv = new KeyValue();
                    kv.Key = item.Module;
                    kv.Value = LabelUtil.GetTextByKey("aca_sys_feature_apply_a_permit", kv.Key);
                    model.CreateRecordActions.Add(kv);
                }
                else if ((string.IsNullOrEmpty(model.ModuleName)
                    || (string.Equals(model.ModuleName, item.Module) && !string.IsNullOrEmpty(model.ModuleName)))
                    && modules.ContainsKey(item.Module)
                    && item.Children.Exists(f => f.Label == "aca_sys_feature_apply_a_permit_by_service"))
                {
                    KeyValue kv = new KeyValue();
                    kv.Key = item.Module;
                    kv.Value = LabelUtil.GetTextByKey("aca_sys_feature_apply_a_permit_by_service", kv.Key);
                    model.CreateRecordActions.Add(kv);
                }
            }

            ACAGISModel = model;
        }

        /// <summary>
        /// set Module list for ACAGISModel
        /// </summary>
        private void SetModuleList()
        {
            ACAGISModel model = ObjectCloneUtil.DeepCopy<ACAGISModel>(this.ACAGISModel);
            List<KeyValue> list = new List<KeyValue>();

            if (!string.IsNullOrEmpty(ACAGISModel.ModuleName))
            {
                IList<string> modulenamelist = TabUtil.GetCrossModules(this.ACAGISModel.ModuleName);

                // current module name
                list.Add(new KeyValue()
                {
                    Key = this.ACAGISModel.ModuleName,
                    Value = HttpUtility.HtmlDecode(LabelUtil.GetI18NModuleName(this.ACAGISModel.ModuleName))
                });

                // if there are cross modules, insert to module list.
                if (CapUtil.EnableCrossModuleSearch())
                {
                    if (modulenamelist != null && modulenamelist.Count > 0)
                    {
                        foreach (string modulename in modulenamelist)
                        {
                            list.Add(new KeyValue()
                            {
                                Key = modulename,
                                Value = HttpUtility.HtmlDecode(LabelUtil.GetI18NModuleName(modulename))
                            });
                        }
                    }
                }

                model.Modules = list;
            }
            else
            {
                Dictionary<string, string> modules = TabUtil.GetAllEnableModules(false);

                foreach (KeyValuePair<string, string> item in modules)
                {
                    list.Add(new KeyValue()
                    {
                        Key = item.Key,
                        Value = HttpUtility.HtmlDecode(LabelUtil.GetI18NModuleName(item.Key))
                    });
                }
            }

            model.Modules = list;
            ACAGISModel = model;
        }

        #endregion Methods
    }
}