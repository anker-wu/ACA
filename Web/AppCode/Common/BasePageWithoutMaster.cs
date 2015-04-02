/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BasePageWithoutMaster.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: BasePageWithoutMaster.cs 201856 2011-08-18 07:07:49Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.Web
{
    /// <summary>
    /// base page without master
    /// </summary>
    public class BasePageWithoutMaster : BasePage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BasePageWithoutMaster class.
        /// </summary>
        public BasePageWithoutMaster()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Chang mater page
        /// </summary>
        protected override void ChangeMasterPage()
        {
        }

        /// <summary>
        /// override GotoTop method
        /// </summary>
        protected override void GotoTop()
        {
            // Since there is no GoTop object is defined in withoutMaster, we needn't to implement it.
        }

        /// <summary>
        /// override RecordUrl method
        /// </summary>
        protected override void RecordUrl()
        {
            //don't record this url in Iframe.
        }

        #endregion Methods
    }
}