#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConditionNoticeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2010
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: ConditionNoticeModel.cs 130988 2009-9-18  16:26:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// condition notice model class
    /// </summary>
    public class ConditionNoticeModel
    {
        /// <summary>
        /// Gets or sets the highest condition
        /// </summary>
        public NoticeConditionModel HighestCondition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the notice condition collection
        /// </summary>
        public NoticeConditionModel[] NoticeConditions
        {
            get;
            set;
        }
    }
}
