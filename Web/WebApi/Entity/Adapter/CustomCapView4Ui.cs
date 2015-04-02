#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CustomCapView4Ui.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: CustomCapView4Ui.cs 183096 2014-8-11 03:00:43Z ACHIEVO\Awen-deng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Web.Common.GlobalSearch;

namespace Accela.ACA.Web.WebApi.Entity.Adapter
{
    /// <summary>
    /// Customize record view entity for html page data bind.
    /// </summary>
    [Serializable]
    public class CustomCapView4Ui : CAPView4UI
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCapView4Ui" /> class.
        /// </summary>
        public CustomCapView4Ui()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCapView4Ui" /> class.
        /// </summary>
        /// <param name="capView4Ui">CAPView4UI Instantiation to CustomCapView4 UI</param>
        public CustomCapView4Ui(CAPView4UI capView4Ui)
        {
            if (capView4Ui != null)
            {
                this.Address = capView4Ui.Address;
                this.AgencyCode = capView4Ui.AgencyCode;
                this.CapClass = capView4Ui.CapClass;
                this.CapID = capView4Ui.CapID;
                this.CreatedDate = capView4Ui.CreatedDate;
                this.Description = capView4Ui.Description;
                this.ModuleName = capView4Ui.ModuleName;
                this.PermitNumber = capView4Ui.PermitNumber;
                this.PermitType = capView4Ui.PermitType;
                this.ProjectName = capView4Ui.ProjectName;
                this.RelatedRecords = capView4Ui.RelatedRecords;
                this.Status = capView4Ui.Status;
            }
        }

        /// <summary>
        /// Gets or sets detail view url.
        /// </summary>
        public string DetailViewUrl { get; set; }

        /// <summary>
        /// Gets or sets resume url.
        /// </summary>
        public string ResumeUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is partial or not.
        /// </summary>
        public bool IsPartialCap { get; set; }

        /// <summary>
        /// Gets or sets renewal status.
        /// </summary>
        public string RenewalStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether have no paid fee or not.
        /// </summary>
        public bool HasNoPaidFees { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display inspection or not.
        /// </summary>
        public bool DisplayInspcetion { get; set; }

        /// <summary>
        /// Gets or sets pay fee url.
        /// </summary>
        public string PayfeesUrl { get; set; }

        /// <summary>
        ///  Gets or sets Inspections quantity.
        /// </summary>
        public int Inspections { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show PrintPermitBut or not.
        /// </summary>
        public bool IsShowPrintPermit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show PrintSummaryBut or not.
        /// </summary>
        public bool IsShowPrintSummary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show PrintReceiptBut or not.
        /// </summary>
        public bool IsShowPrintReceipt { get; set; }

        /// <summary>
        /// Gets or sets PrintPermitReportId.
        /// </summary>
        public string PrintPermitReportId { get; set; }

        /// <summary>
        /// Gets or sets PrintPermitReportType.
        /// </summary>
        public string PrintPermitReportType { get; set; }

        /// <summary>
        /// Gets or sets PrintSummaryReportId.
        /// </summary>
        public string PrintSummaryReportId { get; set; }

        /// <summary>
        /// Gets or sets PrintSummaryReportType.
        /// </summary>
        public string PrintSummaryReportType { get; set; }

        /// <summary>
        /// Gets or sets PrintReceiptReportId.
        /// </summary>
        public string PrintReceiptReportId { get; set; }

        /// <summary>
        /// Gets or sets PrintReceiptReportType.
        /// </summary>
        public string PrintReceiptReportType { get; set; }

        /// <summary>
        /// Gets or sets PrintPermitReportName.
        /// </summary>
        public string PrintPermitReportName { get; set; }

        /// <summary>
        /// Gets or sets PrintSummaryReportName.
        /// </summary>
        public string PrintSummaryReportName { get; set; }

        /// <summary>
        /// Gets or sets PrintReceiptReportName.
        /// </summary>
        public string PrintReceiptReportName { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether to show AddToCart button or not.
        /// </summary>
        public bool IsShowAddToCart { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether to show upload button or not.
        /// </summary>
        public bool IsShowUpload { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether to show CopyRecord button or not.
        /// </summary>
        public bool IsShowCopyRecord { get; set; }
    }
}