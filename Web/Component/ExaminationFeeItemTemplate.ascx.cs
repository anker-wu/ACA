#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationFeeItemTemplate.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Examination Fee Item Template
    /// </summary>
    public partial class ExaminationFeeItemTemplate : BaseUserControl
    {
        /// <summary>
        /// Binds the data source.
        /// </summary>
        /// <param name="feeItems">The fee items.</param>
        /// <param name="totalFee">The total fee.</param>
        public void BindDataSource(RefFeeDsecVO[] feeItems, string totalFee)
        {
            if (!AppSession.IsAdmin)
            {
                //totalfee is null when examination is readytoschedule status
                //need to calc. total fee.
                string totalFeeString = string.IsNullOrEmpty(totalFee) && feeItems != null && feeItems.Length != 0
                                            ? ExaminationScheduleUtil.TotalFee(feeItems)
                                            : totalFee;
                lblFeeAmount.NumericText = totalFeeString;

                this.feeItemList.DataSource = feeItems;
                this.feeItemList.DataBind();
            }
        }
    }
}