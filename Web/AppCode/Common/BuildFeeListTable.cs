#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BuildFeeListTable.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: BuildFeeListTable.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Text;
using System.Web.UI;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Build html for FeeList
    /// </summary>
    public class BuildFeeListTable : Page
    {
        #region Fields

        /// <summary>
        /// Current Page
        /// </summary>
        private const int CURRENTPAGE = 1;

        /// <summary>
        /// Default Grid View Width
        /// </summary>
        private const string DEFAULTGRIDVIEWWIDTH = "100%";

        /// <summary>
        /// Default Page Size
        /// </summary>
        private const int PAGESIZE = 5;

        /// <summary>
        /// Current Page Index
        /// </summary>
        private int _currentPageIndex = CURRENTPAGE;

        /// <summary>
        /// Display Receipt Report or not
        /// </summary>
        private bool _displayReceiptReport = false;

        /// <summary>
        /// Header Columns
        /// </summary>
        private string[] _headColumns = null;

        /// <summary>
        /// Initial Data Source
        /// </summary>
        private DataTable _initDataSource = null;

        /// <summary>
        /// Module Name
        /// </summary>
        private string _moduleName = "Building";

        /// <summary>
        /// Current Page Size
        /// </summary>
        private int _pageSize = PAGESIZE;

        /// <summary>
        /// Indicates if the fee item is paid or not
        /// </summary>
        private bool _paid = true;

        /// <summary>
        /// Receipt Number
        /// </summary>
        private string _receiptNbr = "0";

        /// <summary>
        /// Receipt Report ID
        /// </summary>
        private string _receiptReportID = "0";

        /// <summary>
        /// Total Fee Amount
        /// </summary>
        private string _totalFees = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BuildFeeListTable class
        /// </summary>
        /// <param name="dtSource">Fee List Source</param>
        /// <param name="headColumns">Header Columns</param>
        public BuildFeeListTable(DataTable dtSource, string[] headColumns)
        {
            this.InitDataSource = dtSource;
            this.HeadColumns = headColumns;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets current page index
        /// </summary>        
        public int CurrentPageIndex
        {
            get
            {
                return _currentPageIndex;
            }

            set
            {
                _currentPageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether display or no
        /// </summary>        
        public bool DisplayReceiptReport
        {
            get
            {
                return _displayReceiptReport;
            }

            set
            {
                _displayReceiptReport = value;
            }
        }

        /// <summary>
        /// Gets or sets ModelName
        /// </summary>        
        public string ModuleName
        {
            get
            {
                return _moduleName;
            }

            set
            {
                _moduleName = value;
            }
        }

        /// <summary>
        /// Gets or sets  page size
        /// </summary>        
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Outstanding or Paid
        /// </summary>        
        public bool Paid
        {
            get
            {
                return _paid;
            }

            set
            {
                _paid = value;
            }
        }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        public string ReceiptNbr
        {
            get
            {
                return _receiptNbr;
            }

            set
            {
                _receiptNbr = value;
            }
        }

        /// <summary>
        /// Gets or sets ReceiptReportID
        /// </summary>        
        public string ReceiptReportID
        {
            get
            {
                return _receiptReportID;
            }

            set
            {
                _receiptReportID = value;
            }
        }

        /// <summary>
        /// Gets or sets total fees
        /// </summary>        
        public string TotalFees
        {
            get
            {
                return _totalFees;
            }

            set
            {
                _totalFees = value;
            }
        }

        /// <summary>
        /// Gets or sets head columns
        /// </summary>        
        private string[] HeadColumns
        {
            get
            {
                return _headColumns;
            }

            set
            {
                _headColumns = value;
            }
        }

        /// <summary>
        /// Gets or sets initial data source
        /// </summary>        
        private DataTable InitDataSource
        {
            get
            {
                return _initDataSource != null ? _initDataSource : null;
            }

            set
            {
                _initDataSource = value;
            }
        }

        /// <summary>
        /// Gets pager data source
        /// </summary>        
        private DataTable PagerDataSource
        {
            get
            {
                if (this.InitDataSource != null)
                {
                    DataTable dtPage = InitDataSource.Clone();
                    int missRow = this.PageSize * (this.CurrentPageIndex - 1); //rows before current page index
                    int currentRowsCount = InitDataSource.Rows.Count - missRow;

                    if (currentRowsCount > this.PageSize)
                    {
                        currentRowsCount = this.PageSize;
                    }

                    for (int i = 0; i < currentRowsCount; i++)
                    {
                        dtPage.ImportRow(InitDataSource.Rows[missRow++]);
                    }

                    return dtPage;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// build html for fee list
        /// </summary>
        /// <param name="readOnly">if is readonly, hide pay fee link</param>
        /// <returns>html string</returns>
        public string GetBuildTable(bool readOnly)
        {
            DataTable dtSource = InitDataSource;
            StringBuilder sbHtmlTable = new StringBuilder();

            if (dtSource == null ||
                dtSource.Rows.Count == 0)
            {
                return sbHtmlTable.ToString();
            }

            string summary = LabelUtil.GetTextByKey("aca_summary_fee_paid", ModuleName);

            if (!Paid)
            {
                summary = LabelUtil.GetTextByKey("aca_summary_fee_unpaid", ModuleName);
            }

            DataTable dtFee = PagerDataSource;
            sbHtmlTable.Append("<table class=\"FeeList\" cellspacing=\"0\" border=\"0\" id=\"ctl00_PlaceHolderMain_FeeList_gdvFeeUnpaidList\" style=\"width:" + DEFAULTGRIDVIEWWIDTH + ";\" summary=\"" + summary + "\">\r");
            sbHtmlTable.Append(BuildCaption());
            sbHtmlTable.Append(BuildHeader());
            sbHtmlTable.Append(BuildBody(dtFee, readOnly));
            sbHtmlTable.Append(BuildPager(dtSource));
            sbHtmlTable.Append(BuildAcountTotal(dtSource));
            sbHtmlTable.Append("</table>");

            return sbHtmlTable.ToString();
        }

        /// <summary>
        /// Build Total Fee Amount
        /// </summary>
        /// <param name="dtSource">Fee List Source</param>
        /// <returns>Html Text for Total Fee Amount</returns>
        private StringBuilder BuildAcountTotal(DataTable dtSource)
        {
            if (string.IsNullOrEmpty(TotalFees))
            {
                return null;
            }

            string totalAccount = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("per_feeDetails_label_paidTotal", ModuleName), ACAConstant.BLANK, TotalFees);

            if (!Paid)
            {
                totalAccount = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("per_feeDetails_label_unpaidTotal", ModuleName), ACAConstant.BLANK, TotalFees);
            }

            StringBuilder strAcount = new StringBuilder();
            strAcount.Append("<tr><td colspan=\"" + dtSource.Columns.Count + "\">\r");
            strAcount.Append("<div class=\"ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize\">\r");
            strAcount.Append("<strong><i>\r");
            strAcount.Append(totalAccount);
            strAcount.Append("</i></strong>\r");
            strAcount.Append("</div>\r");
            strAcount.Append("</td></tr>");
            return strAcount;
        }

        /// <summary>
        /// Build Fee List Table
        /// </summary>
        /// <param name="dtFee">Fee List Source</param>
        /// <param name="readOnly">if is readonly, hide pay fee link</param>
        /// <returns>Html Text for Fee List Table</returns>
        private StringBuilder BuildBody(DataTable dtFee, bool readOnly)
        {
            string url = string.Empty;
            StringBuilder strBody = new StringBuilder();            
            bool isLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(AppSession.GetCapModelFromSession(ModuleName).capID, AppSession.User.UserSeqNum);

            for (int i = 0; i < dtFee.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    strBody.Append("<tr class=\"ACA_TabRow_Odd ACA_TabRow_Odd_FontSize\">\r");
                }
                else
                {
                    strBody.Append("<tr class=\"ACA_TabRow_Even ACA_TabRow_Even_FontSize\">\r");
                }

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<div class=\"ACA_NShot\">\r");
                strBody.Append(I18nDateTimeUtil.FormatToDateStringForUI(dtFee.Rows[i][0]) + "\r"); //Date
                strBody.Append("</div></td>\r");

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<div class=\"ACA_Medium\">\r");
                strBody.Append(dtFee.Rows[i][1].ToString() + "\r"); //Invoice Number
                strBody.Append("</div></td>\r");

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<div class=\"ACA_Medium\">\r");
                strBody.Append(dtFee.Rows[i][2].ToString() + "\r"); //Acount
                strBody.Append("</div></td>\r");

                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (this.Paid)
                {
                    if (this.DisplayReceiptReport)
                    {
                        CapIDModel4WS capID = capModel.capID;

                        if (StandardChoiceUtil.IsSuperAgency() && capID != null &&
                            !ConfigManager.AgencyCode.Equals(capID.serviceProviderCode, StringComparison.InvariantCulture))
                        {
                            url = string.Format("../Report/ReportParameter.aspx?Module={0}&reportType={1}&RecepitNbr={2}&reportID={3}&subID1={4}&subID2={5}&subID3={6}&{7}={8}&subCustomerID={9}&SubModule={10}", ScriptFilter.AntiXssUrlEncode(ModuleName), ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT, dtFee.Rows[i]["ReceiptNbr"], ReceiptReportID, Server.UrlEncode(capID.id1), Server.UrlEncode(capID.id2), Server.UrlEncode(capID.id3), UrlConstant.AgencyCode, capID.serviceProviderCode, Server.UrlEncode(capID.customID), ModuleName);
                        }
                        else
                        {
                            url = string.Format("../Report/ReportParameter.aspx?Module={0}&{1}={2}&{3}={4}&{5}={6}", ScriptFilter.AntiXssUrlEncode(ModuleName), "reportType", ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT, "RecepitNbr", dtFee.Rows[i]["ReceiptNbr"], "reportID", this.ReceiptReportID);
                        }

                        url = string.Format("<a id=\"detail\" class=\"NotShowLoading\" title=\"{0}\"+ href=\"javascript:;\" onClick=\"feesSection_report_onclick('{1}')\">{2}</a>", dtFee.Rows[i][4], url, dtFee.Rows[i][3]);
                    }
                }
                else if (!readOnly)
                {
                    // Hidden Pay Fee Link when current user is anonymous user or he does not have privilege to handle with this cap
                    //If only set contact permission,we must be consider the permission set in capcontact model in spear form.
                    string agencyCode = capModel.capID == null ? string.Empty : capModel.capID.serviceProviderCode;
                    var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                    UserRolePrivilegeModel userRole = userRoleBll.GetRecordSearchRole(ConfigManager.AgencyCode, ModuleName, capModel.capType);

                    if (userRole.allAcaUserAllowed || userRole.registeredUserAllowed)
                    {
                        userRole = userRoleBll.GetDefaultRole();
                    }

                    var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

                    if (!isLockedOrHold
                        && !StandardChoiceUtil.IsRemovePayFee(agencyCode)
                        && (StandardChoiceUtil.IsDisplayPayFeeLink(ModuleName) ||
                            (!AppSession.User.IsAnonymous && proxyUserRoleBll.HasPermission(AppSession.GetCapModelFromSession(ModuleName), userRole, ProxyPermissionType.MAKE_PAYMENTS)))
                        && FunctionTable.IsEnableMakePayment())
                    {
                        url = string.Format("CapFees.aspx?Module={0}&permitType=PayFees&stepNumber=0&isPay4ExistingCap={1}&{2}={3}", this.ModuleName, ACAConstant.COMMON_Y, UrlConstant.AgencyCode, agencyCode);
                        string inavailableExam = CapUtil.GetInavailableExamsWithNoPaidExamFee(capModel.capID);
                        
                        if (!string.IsNullOrEmpty(inavailableExam))
                        {
                            string msg = LabelUtil.GetTextByKey("aca_exam_msg_inavailable2payfee", ModuleName);
                            msg = string.Format(msg, inavailableExam);
                            url = string.Format(
                                            "<a id=\"detail\" class='NotShowLoading' href='{0}' onclick=\"showNormalMessage('{1}', 'Error'); return false;\">{2}</a>",
                                            url,
                                            msg,
                                            dtFee.Rows[i][3]);
                        }
                        else
                        {
                            url = string.Format("<a id=\"detail\" href='{0}'>{1}</a>", url, dtFee.Rows[i][3]);
                        }
                    }
                }

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<div class=\"ACA_NLonger\">\r");
                strBody.Append(url); //Link
                strBody.Append("</div></td>\r");
                strBody.Append("</tr>\r");
            }

            return strBody;
        }

        /// <summary>
        /// Build Fee List Caption
        /// </summary>
        /// <returns>Html Text for Caption</returns>
        private string BuildCaption()
        {
            string caption = LabelUtil.GetTextByKey("per_feeDetails_label_paidList", ModuleName);

            if (!Paid)
            {
                caption = LabelUtil.GetTextByKey("per_feeDetails_label_outstandingList", ModuleName);
            }

            return string.Format(
                        @"<caption>
                            <div class=""ACA_TabRow"">
                            <h1><i>
                            {0}
                            </i></h1>
                            </div>
                        </caption>",
                        caption);
        }

        /// <summary>
        /// Build Fee List Header
        /// </summary>
        /// <returns>Html Text for Header Section</returns>
        private StringBuilder BuildHeader()
        {
            string h4Class = string.Empty;
            StringBuilder strHeader = new StringBuilder();
            strHeader.Append("<tr class=\"ACA_BkTit\">\r");

            for (int i = 0; i < this.HeadColumns.Length; i++)
            {
                h4Class = "ACA_Medium";

                if (i == 0)
                {
                    h4Class = "ACA_NShot"; //Date
                }

                if (i == this.HeadColumns.Length)
                {
                    h4Class = "ACA_NLonger"; //Link
                }

                strHeader.Append("<th scope='col' class=\"ACA_AlignLeftOrRight\">\r");
                strHeader.Append("<div class=\"Header_h4 " + h4Class + " fontbold\">\r");
                strHeader.Append(this.HeadColumns[i] + "\r");
                strHeader.Append("</div></th>\r");
            }

            strHeader.Append("</tr>\r");
            return strHeader;
        }

        /// <summary>
        /// Build Fee List Pager Section
        /// </summary>
        /// <param name="dtSource">Fee List Source</param>
        /// <returns>Html Text for Pager Section</returns>
        private StringBuilder BuildPager(DataTable dtSource)
        {
            int totalPager = TotalPager(dtSource);

            if (totalPager == 1)
            {
                return null;
            }

            string prevPage = LabelUtil.GetTextByKey("ACA_NextPrevNumbericPagerTemplate_PrevText", this.ModuleName); //prev
            string nextPage = LabelUtil.GetTextByKey("ACA_NextPrevNumbericPagerTemplate_NextText", this.ModuleName); //next
            string additionalResult = LabelUtil.GetTextByKey("ACA_NextPrevNumbericPagerTemplate_AdditionalResult", this.ModuleName); //Additional Results:

            //page
            StringBuilder strPager = new StringBuilder();
            strPager.Append("<tr class=\"ACA_Table_Pages ACA_Table_Pages_FontSize\" align=\"center\" valign=\"bottom\">\r");
            strPager.Append("<td colspan=\"" + dtSource.Columns.Count + "\">\r");
            strPager.Append("<table role='presentation' border=\"0\">\r");
            strPager.Append("<tr style=\"text-align:center;\">\r");

            //prev
            strPager.Append("<td nowrap='true'>\r");
            if (CanPrev(dtSource))
            {
                strPager.AppendFormat("<a href=\"javascript:;\" onClick=\"changePage('{0}','{1}')\">{2}</a>\r", (CurrentPageIndex - 1).ToString(), Paid.ToString().ToLower(), prevPage);
            }
            else
            {
                strPager.Append("<span class=\"SelectedPageButton font11px\">" + prevPage + "</span>\r");
            }

            strPager.Append("</td>\r");

            //pager
            strPager.Append("<td nowrap=\"true\" style=\"white-space:nowrap;width:70%;\">\r");
            strPager.Append("<table role='presentation' nowrap=\"true\" border=\"0\">\r"); //table begin
            strPager.Append("<tr>\r");
            strPager.Append("<td>" + additionalResult.ToString() + "</td>\r");
            int startPager = 1, endPager = totalPager;
            bool hasPrev = false, hasNext = false;
            GetCurrentPager(totalPager, ref startPager, ref endPager, ref hasPrev, ref hasNext);

            if (hasPrev)
            {
                strPager.AppendFormat("<td><a href=\"javascript:;\" title=\"{0}\" onClick=\"changePage('1','{1}')\">\r", LabelUtil.GetTextByKey("aca_common_label_showmorepages", string.Empty), this.Paid.ToString().ToLower());
                strPager.Append("...");
                strPager.Append("</a></td>\r");
            }

            for (int i = startPager; i <= endPager; i++)
            {
                if (i == this.CurrentPageIndex)
                {
                    strPager.Append("<td><span class=\"SelectedPageButton font11px\">\r");
                    strPager.Append(i.ToString());
                    strPager.Append("</span></td>\r");
                }
                else
                {
                    strPager.AppendFormat("<td><a href=\"javascript:;\" onClick=\"changePage('{0}','{1}')\">\r", i.ToString(), this.Paid.ToString().ToLower()); //location to next page
                    strPager.Append(i.ToString());
                    strPager.Append("</a></td>\r");
                }
            }

            if (hasNext)
            {
                strPager.AppendFormat("<td><a href=\"javascript:;\" title=\"{0}\" onClick=\"changePage('{1}','{2}')\">\r", LabelUtil.GetTextByKey("aca_common_label_showmorepages", string.Empty), totalPager.ToString(), this.Paid.ToString().ToLower());
                strPager.Append("...");
                strPager.Append("</a></td>\r");
            }

            strPager.Append("</tr>\r");
            strPager.Append("</table>\r"); //table end
            strPager.Append("</td>\r");

            //next
            strPager.Append("<td nowrap='true'>\r");

            if (CanNext(dtSource))
            {
                strPager.AppendFormat("<a href=\"javascript:;\" onClick=\"changePage('{0}','{1}')\">{2}</a>\r", (CurrentPageIndex + 1).ToString(), Paid.ToString().ToLower(), nextPage);
            }
            else
            {
                strPager.Append("<span class=\"SelectedPageButton font11px\">" + nextPage + "</span>\r");
            }

            strPager.Append("</td>\r");

            //page
            strPager.Append("</tr>\r");
            strPager.Append("</table>\r");
            strPager.Append("</td>\r");
            strPager.Append("</tr>\r");

            return strPager;
        }

        /// <summary>
        /// Check if "Next" is enabled
        /// </summary>
        /// <param name="dtSource">Fee List Source</param>
        /// <returns>True or false</returns>
        private bool CanNext(DataTable dtSource)
        {
            bool canNext = false;
            int totalPager = TotalPager(dtSource);

            if (totalPager > 1 &&
                this.CurrentPageIndex != totalPager)
            {
                canNext = true;
            }

            return canNext;
        }

        /// <summary>
        /// Check if <c>"Prev"</c> is enabled
        /// </summary>
        /// <param name="dtSource">Fee List Source</param>
        /// <returns>True or false</returns>
        private bool CanPrev(DataTable dtSource)
        {
            bool canPrev = false;
            int totalPager = TotalPager(dtSource);

            if (totalPager > 1 &&
                this.CurrentPageIndex != 1)
            {
                canPrev = true;
            }

            return canPrev;
        }

        /// <summary>
        /// Get Current Page Index
        /// </summary>
        /// <param name="totalPager">Total Page Size</param>
        /// <param name="startPager">Start Page Index</param>
        /// <param name="endPager">End Page Index</param>
        /// <param name="hasPrev"><c>"Prev"</c> is enabled or not</param>
        /// <param name="hasNext">"Next" is enabled or not</param>
        private void GetCurrentPager(int totalPager, ref int startPager, ref int endPager, ref bool hasPrev, ref bool hasNext)
        {
            hasPrev = false;
            hasNext = false;
            startPager = 1;
            endPager = totalPager;

            if (totalPager > 10)
            {
                startPager = 1 + (this.CurrentPageIndex - 5);
                endPager = this.CurrentPageIndex + 5;
                if (startPager > 1 &&
                    endPager > totalPager)
                {
                    startPager = startPager - (endPager - totalPager);
                    endPager = totalPager;
                }

                if (startPager < 1 &&
                    endPager < totalPager)
                {
                    endPager = endPager + (1 - startPager);
                    startPager = 1;
                }

                if (startPager > 1)
                {
                    hasPrev = true;
                }

                if (endPager < totalPager)
                {
                    hasNext = true;
                }
            }
        }

        /// <summary>
        /// Get Total Page Size
        /// </summary>
        /// <param name="dtSource">Fee List Source</param>
        /// <returns>Total Page Size</returns>
        private int TotalPager(DataTable dtSource)
        {
            int totalRows = dtSource.Rows.Count;
            int totalPager = totalRows / this.PageSize;

            if (totalPager * this.PageSize < totalRows)
            {
                totalPager++;
            }

            return totalPager;
        }

        #endregion Methods
    }
}