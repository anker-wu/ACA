#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ValuationCalculatorEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ValuationCalculatorEdit.ascx.cs  
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// ValuationCalculatorEdit Control.
    /// </summary>
    public partial class ValuationCalculatorEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// raw value used as attribute key
        /// </summary>
        private const string RAW_VALUE = "rawValue";

        #endregion Fields

        #region Properties

        /// <summary>
        /// indicate the license form is editable or not.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        ///  valuation calculator list count.
        /// </summary>
        private int _valuationCalculatorCount = 0;

        /// <summary>
        /// Gets or sets ref valuation calculator data source
        /// </summary>
        public IList<BCalcValuatnModel4WS> RefDataSource
        {
            get
            {
                if (ViewState["refValuationCalculatorDataSource"] == null)
                {
                    ViewState["refValuationCalculatorDataSource"] = new List<BCalcValuatnModel4WS>();
                }

                return (List<BCalcValuatnModel4WS>)ViewState["refValuationCalculatorDataSource"];
            }

            set
            {
                ViewState["refValuationCalculatorDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets focus click id.
        /// </summary>
        public string SkippingToParentClickID
        {
            get
            {
                return ViewState["SkippingToParentClickID"] as string;
            }

            set
            {
                ViewState["SkippingToParentClickID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Admin Data Bound
        /// </summary>
        public void AdminDataBound()
        {
            DataTable dtGroup = new DataTable();
            dtGroup.Columns.Add(new DataColumn("valuationCalculatorGroup", typeof(string)));
            dtGroup.Columns.Add(new DataColumn("valuationCalculatorList", typeof(object)));
            dtGroup.Columns.Add(new DataColumn("capTypeDisplay", typeof(string)));

            string groupName = string.Empty;
            string capTypeDisplay = string.Empty;
            IValuationCalculatorBll valcalBLL = (IValuationCalculatorBll)ObjectFactory.GetObject(typeof(IValuationCalculatorBll));

            DataRow rowGrp = dtGroup.NewRow();
            rowGrp["valuationCalculatorGroup"] = groupName;
            rowGrp["capTypeDisplay"] = capTypeDisplay;
            rowGrp["valuationCalculatorList"] = ObjectConvertUtil.ConvertArrayToList(valcalBLL.GetNullValuationCalculator());
            dtGroup.Rows.Add(rowGrp);
            gdvValuationCalculatorGroup.DataSource = dtGroup;
            gdvValuationCalculatorGroup.DataBind();
        }

        /// <summary>
        /// Display Valuation Calculator
        /// </summary>
        /// <param name="valuationcalculators">valuation calculators</param>
        public void DisplayValuationCalculator(BCalcValuatnModel4WS[] valuationcalculators)
        {
            if (valuationcalculators != null && valuationcalculators.Length > 0)
            {
                IValuationCalculatorBll valcalBLL = (IValuationCalculatorBll)ObjectFactory.GetObject(typeof(IValuationCalculatorBll));
                DataTable valcalTable = valcalBLL.GetValationCalculatorTable(valuationcalculators);
                _valuationCalculatorCount = valcalTable.Rows.Count;
                gdvValuationCalculatorGroup.DataSource = valcalTable;
                gdvValuationCalculatorGroup.DataBind();
                SetSumItem();
            }
            else
            {
                AdminDataBound();
            }

            if (!IsEditable && !AppSession.IsAdmin)
            {
                DisableEdit(this, null);
            }
        }

        /// <summary>
        /// Get all valuation calculators
        /// </summary>
        /// <returns>ValuationModel array</returns>
        public BCalcValuatnModel4WS[] GetCalValuationModel()
        {
            ArrayList alcalval = new ArrayList();
            for (int i = 0; i < gdvValuationCalculatorGroup.Rows.Count; i++)
            {
                double sumJobTotal = 0;
                AccelaGridView gvlist = (AccelaGridView)gdvValuationCalculatorGroup.Rows[i].FindControl("gdvValuationCalculatorList");
                AccelaLabel lblCalculatorGroup = (AccelaLabel)gdvValuationCalculatorGroup.Rows[i].FindControl("lblCalculatorGroup");
                HiddenField hdnRegModifier = (HiddenField)gdvValuationCalculatorGroup.Rows[i].FindControl("hdnRegModifier");

                if (gvlist != null && gvlist.Rows.Count > 0)
                {
                    for (int j = 0; j < gvlist.Rows.Count; j++)
                    {
                        GridViewRow dvr = gvlist.Rows[j];
                        BCalcValuatnModel4WS calval = new BCalcValuatnModel4WS();
                        AccelaDropDownList ddlOccupancy = (AccelaDropDownList)dvr.FindControl("ddlOccupancy");
                        AccelaDropDownList ddlUnitType = (AccelaDropDownList)dvr.FindControl("ddlUnitType");
                        AccelaNumberText txtUnitAmount = (AccelaNumberText)dvr.FindControl("txtUnitAmount");
                        HiddenField hidUnitCost = (HiddenField)dvr.FindControl("hidUnitCost");
                        AccelaLabel lblJobValue = (AccelaLabel)dvr.FindControl("lblJobValue");
                        AccelaLabel lblUnit = (AccelaLabel)dvr.FindControl("lblUnit");
                        HiddenField lblUnitValue = (HiddenField)dvr.FindControl("lblUnitValue");
                        AccelaLabel lblVersion = (AccelaLabel)dvr.FindControl("lblVersion");
                        HiddenField lblVersionValue = (HiddenField)dvr.FindControl("lblVersionValue");
                        HiddenField hdnExcludeRegionalModifier = (HiddenField)dvr.FindControl("hdnExcludeRegionalModifier");

                        double unitAmout = txtUnitAmount.DoubleValue == null ? 0 : txtUnitAmount.DoubleValue.Value;
                        double itemTotal = unitAmout * Convert.ToDouble(hidUnitCost.Value, CultureInfo.InvariantCulture);

                        if (!string.IsNullOrEmpty(hdnRegModifier.Value))
                        {
                            if (!ACAConstant.COMMON_Y.Equals(hdnExcludeRegionalModifier.Value, StringComparison.InvariantCultureIgnoreCase))
                            {
                                itemTotal = itemTotal * Convert.ToDouble(hdnRegModifier.Value.Trim(), CultureInfo.InvariantCulture);
                            }
                        }

                        sumJobTotal = sumJobTotal + itemTotal;
                        calval.totalValue = itemTotal;
                        calval.unitCost = Convert.ToDouble(hidUnitCost.Value, CultureInfo.InvariantCulture);
                        calval.unitTyp = lblUnitValue.Value.Trim();
                        calval.disUnitType = lblUnit.Text.Trim();
                        calval.unitValue = unitAmout;
                        calval.version = lblVersionValue.Value.Trim();
                        calval.disVersion = lblVersion.Text.Trim();
                        calval.conTyp = ddlUnitType.SelectedItem.Value.Trim();
                        calval.useTyp = ddlOccupancy.SelectedItem.Value.Trim();
                        calval.disConType = ddlUnitType.SelectedItem.Text.Trim();
                        calval.disUseType = ddlOccupancy.SelectedItem.Text.Trim();

                        calval.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
                        calval.auditID = AppSession.User.PublicUserId;
                        calval.auditStatus = ACAConstant.VALID_STATUS;
                        calval.capID = new CapIDModel4WS();
                        AccelaNumberText txtcalcValueSeqNbr = (AccelaNumberText)dvr.FindControl("txtcalcValueSeqNbr");
                        AccelaNumberText txtfeeIndicator = (AccelaNumberText)dvr.FindControl("txtfeeIndicator");
                        AccelaTextBox txtCAPID1 = (AccelaTextBox)dvr.FindControl("txtCAPID1");
                        AccelaTextBox txtCAPID2 = (AccelaTextBox)dvr.FindControl("txtCAPID2");
                        AccelaTextBox txtCAPID3 = (AccelaTextBox)dvr.FindControl("txtCAPID3");
                        if (lblCalculatorGroup != null)
                        {
                            calval.capID.serviceProviderCode = lblCalculatorGroup.Text;
                        }

                        if (txtCAPID1 != null)
                        {
                            calval.capID.id1 = txtCAPID1.Text;
                        }

                        if (txtCAPID2 != null)
                        {
                            calval.capID.id2 = txtCAPID2.Text;
                        }

                        if (txtCAPID3 != null)
                        {
                            calval.capID.id3 = txtCAPID3.Text;
                        }

                        if (txtcalcValueSeqNbr != null && txtcalcValueSeqNbr.Text != null && txtcalcValueSeqNbr.Text.Trim() != string.Empty)
                        {
                            calval.calcValueSeqNbr = Convert.ToInt32(txtcalcValueSeqNbr.Text.Trim());
                        }

                        if (hdnExcludeRegionalModifier != null)
                        {
                            string flag = hdnExcludeRegionalModifier.Value.Trim();

                            if (ValidationUtil.IsYes(flag) || ValidationUtil.IsTrue(flag))
                            {
                                calval.excludeRegionalModifier = ACAConstant.COMMON_Y;
                            }
                            else if (ValidationUtil.IsNo(flag) || ValidationUtil.IsFalse(flag))
                            {
                                calval.excludeRegionalModifier = ACAConstant.COMMON_N;
                            }
                        }

                        if (txtfeeIndicator != null)
                        {
                            calval.feeIndicator = txtfeeIndicator.Text;
                        }

                        alcalval.Add(calval);
                    }
                }
            }

            return (BCalcValuatnModel4WS[])alcalval.ToArray(typeof(BCalcValuatnModel4WS));
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// binding valuation group list
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void ValuationCalGroupList_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlContainerControl dvSepLineValuationCalculator = (HtmlContainerControl)e.Row.FindControl("dvSepLineValuationCalculator");
                AccelaLabel lblValuationMultiplierValue = (AccelaLabel)e.Row.FindControl("lblValuationMultiplierValue");
                AccelaLabel lblValuationMultiplier = (AccelaLabel)e.Row.FindControl("lblValuationMultiplier");
                AccelaLabel lblValuationExtraAmountValue = (AccelaLabel)e.Row.FindControl("lblValuationExtraAmountValue");
                AccelaLabel lblValuationExtraAmount = (AccelaLabel)e.Row.FindControl("lblValuationExtraAmount");
                AccelaGridView gv = (AccelaGridView)e.Row.FindControl("gdvValuationCalculatorList");
                GridViewBuildHelper.SetSimpleViewElements(gv, ModuleName, AppSession.IsAdmin);
                HtmlGenericControl divValCalTitle = (HtmlGenericControl)e.Row.FindControl("divValCalTitle");
                HtmlGenericControl divAgencyLogo = (HtmlGenericControl)e.Row.FindControl("divAgencyLogo");
                HiddenField hdnRegModifier = (HiddenField)e.Row.FindControl("hdnRegModifier");
                AccelaLabel lblValulationCalculatorTotalJob = (AccelaLabel)e.Row.FindControl("lblValulationCalculatorTotalJob");
                
                if (_valuationCalculatorCount == 0)
                {
                    lblValuationMultiplierValue.Visible = false;
                    lblValuationExtraAmountValue.Visible = false;

                    if (!AppSession.IsAdmin)
                    {
                        lblValuationMultiplier.Visible = false;
                        lblValuationExtraAmount.Visible = false;
                        lblValulationCalculatorTotalJob.Visible = false;
                    }

                    dvSepLineValuationCalculator.Visible = false;
                    gv.DataSource = null;
                    gv.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
                    gv.DataBind();
                    return;
                }               

                if (e.Row.RowIndex == _valuationCalculatorCount - 1)
                {
                    dvSepLineValuationCalculator.Visible = false;
                }

                DataRowView rowView = (DataRowView)e.Row.DataItem;

                ILogoBll logo = (ILogoBll)ObjectFactory.GetObject(typeof(ILogoBll));

                string serviceProviderCode = rowView["valuationCalculatorGroup"].ToString();

                // Set the regional modifier value for the current agency
                hdnRegModifier.Value = StandardChoiceUtil.GetRegionalModifierValue(serviceProviderCode);   

                if (lblValuationMultiplierValue != null && lblValuationExtraAmountValue != null)
                {
                    double valuationMultiplier = rowView["valuationMultiplier"] == null ? 1 : Convert.ToDouble(rowView["valuationMultiplier"], CultureInfo.InvariantCulture);
                    lblValuationMultiplierValue.Attributes[RAW_VALUE] = I18nNumberUtil.ConvertNumberToInvariantString(valuationMultiplier);
                    lblValuationMultiplierValue.Text = valuationMultiplier.ToString("0.0000");

                    double valuationExtraAmount = Convert.ToDouble(rowView["valuationExtraAmount"], CultureInfo.InvariantCulture);
                    lblValuationExtraAmountValue.Attributes[RAW_VALUE] = I18nNumberUtil.ConvertMoneyToInvariantString(valuationExtraAmount);
                    lblValuationExtraAmountValue.Text = valuationExtraAmount.ToString("0.00");

                    if (valuationMultiplier == 1)
                    {                     
                        lblValuationMultiplierValue.Style.Add("display", "none");
                        lblValuationMultiplier.Style.Add("display", "none");
                    }

                    if (valuationExtraAmount == 0)
                    {
                        lblValuationExtraAmountValue.Style.Add("display", "none");
                        lblValuationExtraAmount.Style.Add("display", "none");
                    }
                    else
                    {
                        lblValuationExtraAmountValue.Text = I18nNumberUtil.FormatMoneyForUI(valuationExtraAmount);
                    }
                }

                LogoModel logoModel = logo.GetAgencyLogo(serviceProviderCode);
                Image imgDiv = (Image)e.Row.FindControl("divImage");
                
                if (StandardChoiceUtil.IsSuperAgency())
                {
                    if (logoModel != null)
                    {
                        divAgencyLogo.Visible = true;
                        imgDiv.ImageUrl = "../Cap/ImageHandler.aspx?" + UrlConstant.AgencyCode + "=" + logoModel.serviceProviderCode;
                        imgDiv.Height = 32;
                        imgDiv.AlternateText = logoModel.serviceProviderCode;
                    }

                    divValCalTitle.Visible = true;
                }               
                
                List<BCalcValuatnModel4WS> valuationcalculators = (List<BCalcValuatnModel4WS>)rowView["valuationCalculatorList"];
                if (valuationcalculators != null && valuationcalculators.Count > 0)
                {
                    // Sort by display use type and display con type
                    valuationcalculators.Sort(
                        delegate(BCalcValuatnModel4WS x, BCalcValuatnModel4WS y)
                        {
                            int useTypeOrder = string.Compare(x.disUseType, y.disUseType);

                            return useTypeOrder != 0 ? useTypeOrder : string.Compare(x.disConType, y.disConType);
                        });

                    gv.DataSource = valuationcalculators;                 
                    gv.DataBind();
                }
            }
        }  

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">sender Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !AppSession.IsAdmin)
            {                
                SetSumItem();
                hlEnd.NextControlClientID = SkippingToParentClickID;
            }
        }

        /// <summary>
        /// UnitType Selected Index Changed
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void UnitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccelaDropDownList ddlUnitType = (AccelaDropDownList)sender;
            GridViewRow dvr = (GridViewRow)ddlUnitType.NamingContainer;
            AccelaDropDownList ddlOccupancy = (AccelaDropDownList)dvr.FindControl("ddlOccupancy");    
                    
            if (ddlOccupancy.SelectedItem != null && ddlUnitType.SelectedItem != null)
            {                
                SetControlValue(dvr, ddlOccupancy.SelectedItem.Value.Trim(), ddlUnitType.SelectedItem.Value.Trim(), GetCapIDByGridViewRow(dvr));                
            }           
        }

        /// <summary>
        /// Occupancy Selected Index Changed
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e)</param>
        protected void Occupancy_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccelaDropDownList ddlOccupancy = (AccelaDropDownList)sender;
            GridViewRow dvr = (GridViewRow)ddlOccupancy.NamingContainer;
            AccelaDropDownList ddlUnitType = (AccelaDropDownList)dvr.FindControl("ddlUnitType");
            string oldSelectText = string.Empty;

            if (ddlUnitType.SelectedItem != null)
            {
                oldSelectText = ddlUnitType.SelectedItem.Value.Trim();
            }

            if (ddlOccupancy.SelectedItem != null)
            {
                ddlUnitType.Items.Clear();
                ddlUnitType.DataSource = GetAllUnitTypeByOccupancy(ddlOccupancy.SelectedItem.Value.Trim(), GetCapIDByGridViewRow(dvr));
                ddlUnitType.DataBind();
                for (int i = 0; i < ddlUnitType.Items.Count; i++)
                {
                    ddlUnitType.Items[i].Attributes.Add("title", ddlUnitType.Items[i].Text);
                    if (oldSelectText.Trim() != string.Empty && ddlUnitType.Items[i].Text.Trim() == oldSelectText)
                    {
                        ddlUnitType.SelectedIndex = i;
                    }
                }

                if (ddlUnitType.SelectedItem != null)
                {
                    SetControlValue(dvr, ddlOccupancy.SelectedItem.Value.Trim(), ddlUnitType.SelectedItem.Value.Trim(), GetCapIDByGridViewRow(dvr));
                }
            }
        }

        /// <summary>
        /// Valuation Calculator Row bind event.
        /// </summary>
        /// <param name="sender">Event object.</param>
        /// <param name="e">Event arguments.</param>
        protected void ValuationCalculatorList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                BCalcValuatnModel4WS valcal = (BCalcValuatnModel4WS)e.Row.DataItem;

                if (e.Row.FindControl("ddlOccupancy") != null)
                {
                    AccelaDropDownList ddlOccupancy = (AccelaDropDownList)e.Row.FindControl("ddlOccupancy") as AccelaDropDownList;
                    AccelaDropDownList ddlUnitType = (AccelaDropDownList)e.Row.FindControl("ddlUnitType") as AccelaDropDownList;
                    ddlOccupancy.Items.Clear();
                    ddlOccupancy.DataSource = GetAllOccupancy(valcal.capID);
                    ddlOccupancy.DataTextField = "disUseType";
                    ddlOccupancy.DataValueField = "useTyp";
                    ddlOccupancy.DataBind();
                    bool setOccupancy = false;
                    for (int i = 0; i < ddlOccupancy.Items.Count; i++)
                    {
                        ddlOccupancy.Items[i].Attributes.Add("title", ddlOccupancy.Items[i].Value);

                        if (ddlOccupancy.Items[i].Value.Trim() == valcal.useTyp.Trim())
                        {
                            ddlOccupancy.SelectedIndex = i;
                            setOccupancy = true;
                        }
                    }

                    if (!setOccupancy)
                    {
                        ddlOccupancy.Items.Clear();
                        ddlOccupancy.Items.Insert(0, new ListItem(valcal.disUseType, valcal.useTyp));
                        ddlOccupancy.SelectedIndex = 0;

                        ddlUnitType.Items.Clear();
                        ddlUnitType.Items.Insert(0, new ListItem(valcal.disConType, valcal.conTyp));
                        ddlUnitType.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlUnitType.Items.Clear();
                        if (ddlOccupancy.SelectedItem != null)
                        {
                            ddlUnitType.DataSource = GetAllUnitTypeByOccupancy(ddlOccupancy.SelectedItem.Value, valcal.capID);
                            ddlUnitType.DataTextField = "disConType";
                            ddlUnitType.DataValueField = "conTyp";
                            ddlUnitType.DataBind();

                            for (int i = 0; i < ddlUnitType.Items.Count; i++)
                            {
                                ddlUnitType.Items[i].Attributes.Add("title", ddlUnitType.Items[i].Text);
                                if (ddlUnitType.Items[i].Value.Trim() == valcal.conTyp.Trim())
                                {
                                    ddlUnitType.SelectedIndex = i;
                                }
                            }
                        }
                    }
                }

                AccelaNumberText txtUnitAmount = (AccelaNumberText)e.Row.FindControl("txtUnitAmount");
                AccelaLabel lblUnitCost = (AccelaLabel)e.Row.FindControl("lblUnitCost");
                AccelaLabel lblJobValue = (AccelaLabel)e.Row.FindControl("lblJobValue");
                AccelaLabel lblUnit = (AccelaLabel)e.Row.FindControl("lblUnit");
                HiddenField lblUnitValue = (HiddenField)e.Row.FindControl("lblUnitValue");

                AccelaLabel lblVersion = (AccelaLabel)e.Row.FindControl("lblVersion");
                HiddenField lblVersionValue = (HiddenField)e.Row.FindControl("lblVersionValue");
                HiddenField hdnExcludeRegionalModifier = (HiddenField)e.Row.FindControl("hdnExcludeRegionalModifier");

                AccelaLabel lblValulationCalculatorTotalJobValue = (AccelaLabel)e.Row.Parent.Parent.Parent.FindControl("lblValulationCalculatorTotalJobValue");
                AccelaLabel lblValuationMultiplierValue = (AccelaLabel)e.Row.Parent.Parent.Parent.FindControl("lblValuationMultiplierValue");
                double valuationMultiplierValue = Convert.ToDouble(lblValuationMultiplierValue.Attributes[RAW_VALUE], CultureInfo.InvariantCulture);
                AccelaLabel lblValuationExtraAmountValue = (AccelaLabel)e.Row.Parent.Parent.Parent.FindControl("lblValuationExtraAmountValue");
                double valuationExtraAmountValue = Convert.ToDouble(lblValuationExtraAmountValue.Attributes[RAW_VALUE], CultureInfo.InvariantCulture);
                AccelaGridView gvVCList = (AccelaGridView)e.Row.Parent.Parent.Parent.FindControl("gdvValuationCalculatorList");
                HiddenField hdnRegModifier = (HiddenField)e.Row.Parent.Parent.Parent.FindControl("hdnRegModifier");

                if (txtUnitAmount != null)
                {
                    txtUnitAmount.Attributes.Add("onchange", "calculator(this,'" + lblValulationCalculatorTotalJobValue.ClientID + "'," + valuationMultiplierValue + "," + valuationExtraAmountValue + ");");
                    txtUnitAmount.DoubleValue = valcal.unitValue;
                    txtUnitAmount.Attributes["unitCostObjId"] = lblUnitCost.ClientID;
                    txtUnitAmount.Attributes["jobValueObjId"] = lblJobValue.ClientID;
                    txtUnitAmount.Attributes["groupId"] = gvVCList.ClientID;
                    txtUnitAmount.Attributes["excludeRegionalModifierObjId"] = hdnExcludeRegionalModifier.ClientID;
                    txtUnitAmount.Attributes["regModifierObjId"] = hdnRegModifier.ClientID;
                }

                if (lblUnitCost != null)
                {
                    lblUnitCost.Text = I18nNumberUtil.FormatMoneyForUI(valcal.unitCost);
                    lblUnitCost.Attributes[RAW_VALUE] = I18nNumberUtil.ConvertMoneyToInvariantString(valcal.unitCost);
                }

                if (lblJobValue != null)
                {
                    lblJobValue.Text = I18nNumberUtil.FormatMoneyForUI(valcal.totalValue);
                }

                if (lblUnit != null)
                {
                    lblUnit.Text = valcal.disUnitType;
                }

                if (lblUnitValue != null)
                {
                    lblUnitValue.Value = valcal.unitTyp;
                }

                if (lblVersion != null)
                {
                    lblVersion.Text = valcal.disVersion;
                }

                if (lblVersionValue != null)
                {
                    lblVersionValue.Value = valcal.version;
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Get cap id by grid view row
        /// </summary>
        /// <param name="gvr">A grid view row</param>
        /// <returns>Cap id model</returns>
        private CapIDModel4WS GetCapIDByGridViewRow(GridViewRow gvr)
        {
            CapIDModel4WS capID = new CapIDModel4WS();
            AccelaTextBox txtCAPID1 = (AccelaTextBox)gvr.FindControl("txtCAPID1");
            AccelaTextBox txtCAPID2 = (AccelaTextBox)gvr.FindControl("txtCAPID2");
            AccelaTextBox txtCAPID3 = (AccelaTextBox)gvr.FindControl("txtCAPID3");
            AccelaLabel lblCalculatorGroup = (AccelaLabel)gvr.Parent.Parent.Parent.FindControl("lblCalculatorGroup");
            capID.id1 = txtCAPID1.Text;
            capID.id2 = txtCAPID2.Text;
            capID.id3 = txtCAPID3.Text;
            capID.serviceProviderCode = lblCalculatorGroup.Text;
            return capID;            
        }
       
        /// <summary>
        /// Set the control value according to occupancy unit type and version
        /// </summary>
        /// <param name="dvr">Grid view row</param>
        /// <param name="occupancy">The Occupancy</param>
        /// <param name="unitType">Unit Type</param>
        /// <param name="capId">Cap id</param>
        private void SetControlValue(GridViewRow dvr, string occupancy, string unitType, CapIDModel4WS capId)
        {            
            List<BCalcValuatnModel4WS> selectObj = RefDataSource.Where(o => o.useTyp == occupancy && o.conTyp == unitType && o.capID.Equals(capId)).ToList();

            if (selectObj != null && selectObj.Count > 0)
            {
                HiddenField hidUnitCost = (HiddenField)dvr.FindControl("hidUnitCost");
                AccelaLabel lblUnitCost = (AccelaLabel)dvr.FindControl("lblUnitCost");
                AccelaLabel lblJobValue = (AccelaLabel)dvr.FindControl("lblJobValue");
                AccelaLabel lblUnit = (AccelaLabel)dvr.FindControl("lblUnit");
                HiddenField lblUnitValue = (HiddenField)dvr.FindControl("lblUnitValue");
              
                AccelaLabel lblVersion = (AccelaLabel)dvr.FindControl("lblVersion");
                HiddenField lblVersionValue = (HiddenField)dvr.FindControl("lblVersionValue");
                HiddenField hdnExcludeRegionalModifier = (HiddenField)dvr.FindControl("hdnExcludeRegionalModifier");
                AccelaNumberText txtfeeIndicator = (AccelaNumberText)dvr.FindControl("txtfeeIndicator");

                hidUnitCost.Value = I18nNumberUtil.ConvertNumberToInvariantString(selectObj[0].unitCost);
                lblUnitCost.Text = I18nNumberUtil.FormatMoneyForUI(selectObj[0].unitCost);
                lblUnitCost.Attributes[RAW_VALUE] = I18nNumberUtil.ConvertMoneyToInvariantString(selectObj[0].unitCost);
                lblJobValue.Text = I18nNumberUtil.FormatMoneyForUI(selectObj[0].totalValue);
                lblUnit.Text = selectObj[0].disUnitType == null ? string.Empty : selectObj[0].disUnitType.Trim();
                lblUnitValue.Value = selectObj[0].unitTyp == null ? string.Empty : selectObj[0].unitTyp.Trim();

                lblVersion.Text = selectObj[0].disVersion == null ? string.Empty : selectObj[0].disVersion.Trim();
                lblVersionValue.Value = selectObj[0].version == null ? string.Empty : selectObj[0].version.Trim();
                hdnExcludeRegionalModifier.Value = selectObj[0].excludeRegionalModifier == null ? string.Empty : selectObj[0].excludeRegionalModifier.Trim();
                txtfeeIndicator.Text = selectObj[0].feeIndicator == null ? string.Empty : selectObj[0].feeIndicator.Trim();
            }

            DisplayValuationCalculator(GetCalValuationModel());
        }     

        /// <summary>
        /// Get All Unit Type By Occupancy
        /// </summary>
        /// <param name="occupancy">The occupancy</param>
        /// <param name="capID">Cap id</param>
        /// <returns>Unit type</returns>
        private ArrayList GetAllUnitTypeByOccupancy(string occupancy, CapIDModel4WS capID)
        {
            List<BCalcValuatnModel4WS> unitTypeList = RefDataSource.Where(o => o.useTyp == occupancy && o.capID.Equals(capID)).ToList();
            ArrayList arrayUnitType = new ArrayList();
            arrayUnitType.AddRange(unitTypeList);
            return arrayUnitType;
        }

        /// <summary>
        /// Get All Occupancy
        /// </summary>
        /// <param name="capID">The CapIDModel</param>
        /// <returns>The occupancy</returns>
        private ArrayList GetAllOccupancy(CapIDModel4WS capID)
        {
            ArrayList alOccupancy = new ArrayList();
            var occupancyList = RefDataSource.Where(o => o.capID.Equals(capID)).ToList().Distinct(new OccupancyComparint()).ToList<BCalcValuatnModel4WS>();
            ArrayList occupancyType = new ArrayList();
            occupancyType.AddRange(occupancyList);
            return occupancyType;
        }

        /// <summary>
        /// Set sum item value for the whole grid view group control
        /// </summary>
        private void SetSumItem()
        {            
            for (int i = 0; i < gdvValuationCalculatorGroup.Rows.Count; i++)
            {
                double dSumJobTotal = 0;
                AccelaGridView gvlist = (AccelaGridView)gdvValuationCalculatorGroup.Rows[i].FindControl("gdvValuationCalculatorList");
                AccelaLabel lblValuationMultiplierValue = (AccelaLabel)gdvValuationCalculatorGroup.Rows[i].FindControl("lblValuationMultiplierValue");
                AccelaLabel lblValuationExtraAmountValue = (AccelaLabel)gdvValuationCalculatorGroup.Rows[i].FindControl("lblValuationExtraAmountValue");
                HiddenField hdnRegModifier = (HiddenField)gdvValuationCalculatorGroup.Rows[i].FindControl("hdnRegModifier");
                GridViewBuildHelper.SetSimpleViewElements(gvlist, ModuleName, AppSession.IsAdmin);
                if (gvlist != null && gvlist.Rows.Count > 0)
                {
                    for (int j = 0; j < gvlist.Rows.Count; j++)
                    {
                        GridViewRow dvr = gvlist.Rows[j];
                        AccelaNumberText txtUnitAmount = (AccelaNumberText)dvr.FindControl("txtUnitAmount");
                        AccelaLabel lblUnitCost = (AccelaLabel)dvr.FindControl("lblUnitCost");
                        AccelaLabel lblJobValue = (AccelaLabel)dvr.FindControl("lblJobValue");
                        AccelaLabel lblUnit = (AccelaLabel)dvr.FindControl("lblUnit");
                        AccelaLabel lblVersion = (AccelaLabel)dvr.FindControl("lblVersion");
                        HiddenField hdnExcludeRegionalModifier = (HiddenField)dvr.FindControl("hdnExcludeRegionalModifier");
                        double unitAmout = txtUnitAmount.DoubleValue == null ? 0 : txtUnitAmount.DoubleValue.Value;
                        double dItemTotal = unitAmout * Convert.ToDouble(lblUnitCost.Attributes[RAW_VALUE], CultureInfo.InvariantCulture);

                        if (!string.IsNullOrEmpty(hdnRegModifier.Value))
                        {
                            if (!ACAConstant.COMMON_Y.Equals(hdnExcludeRegionalModifier.Value, StringComparison.InvariantCultureIgnoreCase))
                            {
                                dItemTotal = dItemTotal * Convert.ToDouble(hdnRegModifier.Value.Trim(), CultureInfo.InvariantCulture);
                            }
                        }

                        dSumJobTotal = dSumJobTotal + dItemTotal;
                        lblJobValue.Text = I18nNumberUtil.FormatMoneyForUI(dItemTotal);
                    }

                    double extraAmount = Convert.ToDouble(lblValuationExtraAmountValue.Attributes[RAW_VALUE], CultureInfo.InvariantCulture);
                    dSumJobTotal = (dSumJobTotal * Convert.ToDouble(lblValuationMultiplierValue.Attributes[RAW_VALUE], CultureInfo.InvariantCulture)) + extraAmount;

                    AccelaLabel lblValulationCalculatorTotalJobValue = (AccelaLabel)gdvValuationCalculatorGroup.Rows[i].FindControl("lblValulationCalculatorTotalJobValue");
                    lblValulationCalculatorTotalJobValue.Text = I18nNumberUtil.FormatMoneyForUI(dSumJobTotal);
                }
            }            
        }

        /// <summary>
        /// <c>Occupancy Comparint</c> According to UseType
        /// </summary>
        private class OccupancyComparint : IEqualityComparer<BCalcValuatnModel4WS>
        {
            /// <summary>
            /// Judge the use type in two <c>BCalcValuatnModel4WS</c> whether they are same or not.
            /// </summary>
            /// <param name="x"><c>BCalcValuatnModel4WS</c> x</param>
            /// <param name="y"><c>BCalcValuatnModel4WS</c> y</param>
            /// <returns>The compared result flag.</returns>
            bool IEqualityComparer<BCalcValuatnModel4WS>.Equals(BCalcValuatnModel4WS x, BCalcValuatnModel4WS y)
            {
                if (x == null && y == null)
                {
                    return false;
                }

                return x.useTyp == y.useTyp;
            }

            /// <summary>
            /// Get the <c>useTyp.GetHashCode</c>.
            /// </summary>
            /// <param name="obj">The <c>BCalcValuatnModel</c></param>
            /// <returns>The <c>useTyp.GetHashCode</c></returns>
            int IEqualityComparer<BCalcValuatnModel4WS>.GetHashCode(BCalcValuatnModel4WS obj)
            {
                return obj.useTyp.GetHashCode();
            }
        }
     
        #endregion Private Methods
    }
}
