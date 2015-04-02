#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ValuationCalculatorView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ValuationCalculatorView.ascx.cs  
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// The class for AppSpecInfoTableView.
    /// </summary>
    public partial class ValuationCalculatorView : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Raw value used as attribute key
        /// </summary>
        private const string RAW_VALUE = "rawValue";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Fee Condition list count.
        /// </summary>
        private int _valuationCalculatorCount = 0;

        /// <summary>
        /// Hide group name in cap detail page.
        /// </summary>
        private bool _hideGroupName;

        /// <summary>
        /// Gets or sets a value indicating whether hide group name in cap detail page.
        /// </summary>
        public bool HideGroupName
        {
            get
            {
                return _hideGroupName;
            }

            set
            {
                _hideGroupName = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Valuation Calculator List Bind
        /// </summary>
        /// <param name="valuationcalculators">Valuation calculators</param>
        public void DisplayValuationCalculator(BCalcValuatnModel4WS[] valuationcalculators)
        {
            if (valuationcalculators != null && valuationcalculators.Length > 0)
            {
                IValuationCalculatorBll valcalBLL = (IValuationCalculatorBll)ObjectFactory.GetObject(typeof(IValuationCalculatorBll));
                DataTable valcalTable = valcalBLL.GetValationCalculatorTable(valuationcalculators);
                _valuationCalculatorCount = valcalTable.Rows.Count;
                gdvValuationCalculatorGroup.DataSource = valcalTable;
                gdvValuationCalculatorGroup.DataBind();
            }
            else
            {
                AdminDataBound();
            }
        }

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
        /// Row Operation
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="args">AccelaGridView Command Event Args</param>
        protected void ValCalList_RowCommand(object sender, GridViewCommandEventArgs args)
        {
            //if e is null, not process
            if (args == null || args.CommandArgument == null)
            {
                return;
            }

            switch (args.CommandName)
            {
                case "Header":
                    {
                        for (int i = 0; i < gdvValuationCalculatorGroup.Rows.Count; i++)
                        {
                            AccelaGridView gvlist = (AccelaGridView)gdvValuationCalculatorGroup.Rows[i].FindControl("gdvValuationCalculatorList");
                            GridViewBuildHelper.SetSimpleViewElements(gvlist, ModuleName, AppSession.IsAdmin);
                            gvlist.DataBind();
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Binding condition group dataTable
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ValuationCalGroupList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlContainerControl dvSepLineValuationCalculator = (HtmlContainerControl)e.Row.FindControl("dvSepLineValuationCalculator");
                AccelaLabel lblValulationCalculatorTotalJobValue = (AccelaLabel)e.Row.FindControl("lblValulationCalculatorTotalJobValue");
                AccelaLabel lblValulationCalculatorTotalJob = (AccelaLabel)e.Row.FindControl("lblValulationCalculatorTotalJob");
                AccelaLabel lblCalculatorGroup = (AccelaLabel)e.Row.FindControl("lblCalculatorGroup");
                AccelaLabel lblValuationMultiplierValue = (AccelaLabel)e.Row.FindControl("lblValuationMultiplierValue");
                AccelaLabel lblValuationMultiplier = (AccelaLabel)e.Row.FindControl("lblValuationMultiplier");
                AccelaLabel lblValuationExtraAmountValue = (AccelaLabel)e.Row.FindControl("lblValuationExtraAmountValue");
                AccelaLabel lblValuationExtraAmount = (AccelaLabel)e.Row.FindControl("lblValuationExtraAmount");
                AccelaGridView gv = (AccelaGridView)e.Row.FindControl("gdvValuationCalculatorList");
                GridViewBuildHelper.SetSimpleViewElements(gv, ModuleName, AppSession.IsAdmin);
                HtmlGenericControl divValCalTitle = (HtmlGenericControl)e.Row.FindControl("divValCalTitle");
                HtmlGenericControl divAgencyLogo = (HtmlGenericControl)e.Row.FindControl("divAgencyLogo");

                // On cap detail page the list header needs to visible while invisible on review page.an empty datarow will be added on cap detail page.
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

                LogoModel logoModel = logo.GetAgencyLogo(serviceProviderCode);
                Image imgDiv = (Image)e.Row.FindControl("divImage");

                if (!_hideGroupName && StandardChoiceUtil.IsSuperAgency())
                {
                    if (logoModel != null)
                    {
                        divAgencyLogo.Visible = true;
                        imgDiv.ImageUrl = "../Cap/ImageHandler.aspx?" + UrlConstant.AgencyCode + "=" + logoModel.serviceProviderCode;
                        imgDiv.Height = 32;
                    }

                    divValCalTitle.Visible = true;
                }

                List<BCalcValuatnModel4WS> valuationcalculators = (List<BCalcValuatnModel4WS>)rowView["valuationCalculatorList"];

                if (valuationcalculators != null && valuationcalculators.Count > 0)
                {
                    double gvTotal = 0;

                    for (int i = 0; i < valuationcalculators.Count; i++)
                    {
                        gvTotal = gvTotal + valuationcalculators[i].totalValue;
                    }

                    if (valuationcalculators[0].capID != null)
                    {
                        if (lblValuationMultiplierValue.Text != null && lblValuationMultiplierValue.Text.Trim() != string.Empty)
                        {
                            double valuationMultiplierValue = Convert.ToDouble(lblValuationMultiplierValue.Attributes[RAW_VALUE], CultureInfo.InvariantCulture);
                            gvTotal = gvTotal * valuationMultiplierValue;
                        }

                        if (lblValuationExtraAmountValue.Text != null && lblValuationExtraAmountValue.Text.Trim() != string.Empty)
                        {
                            double valuationExtraAmountValue = Convert.ToDouble(lblValuationExtraAmountValue.Attributes[RAW_VALUE], CultureInfo.InvariantCulture);
                            gvTotal = gvTotal + valuationExtraAmountValue;
                        }
                    }

                    lblValulationCalculatorTotalJobValue.Text = I18nNumberUtil.FormatMoneyForUI(gvTotal);

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

        #endregion
    }
}
