#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PopupDialogBasePage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PopupDialogBasePage.cs 176401 2010-06-25 12:11:30Z ACHIEVO\Grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web
{
    /// <summary>
    /// This class provider the base page for popup dialog.
    /// </summary>
    public class PopupDialogBasePage : BasePage
    {
        #region Methods

        /// <summary>
        /// Set PageTile key
        /// </summary>
        /// <param name="labelKey">label Key</param>
        public void SetPageTitleKey(string labelKey)
        {
            Dialog master = this.Master as Dialog;
            master.SetTitleLabelKey(labelKey);
        }

        /// <summary>
        /// Set Page Title visible
        /// </summary>
        /// <param name="visible">a value indicating whether paget title can display or not.</param>
        public void SetPageTitleVisible(bool visible)
        {
            Dialog master = this.Master as Dialog;
            master.SetPageTileVisible(visible);
        }

        /// <summary>
        /// Set auto height
        /// </summary>
        /// <param name="isAutoHeight">whether auto height.</param>
        public void SetAutoHeight(bool isAutoHeight)
        {
            Dialog master = this.Master as Dialog;
            master.IsAutoHeight = isAutoHeight;
        }

        /// <summary>
        /// Set Dialog Width
        /// </summary>
        /// <param name="width">The fixed width.</param>
        public void SetDialogFixedWidth(string width)
        {
            Dialog master = this.Master as Dialog;
            master.FixedWidth = width;
        }

        /// <summary>
        /// Set dialog min height;
        /// </summary>
        /// <param name="minHeight">The minimum height.</param>
        public void SetDialogMinHeight(string minHeight)
        {
            Dialog master = this.Master as Dialog;
            master.MinHeight = minHeight;
        }

        /// <summary>
        /// set dialog max height
        /// </summary>
        /// <param name="maxHeight">The maximum height.</param>
        public void SetDialogMaxHeight(string maxHeight)
        {
            Dialog master = this.Master as Dialog;
            master.MaxHeight = maxHeight;
        }

        /// <summary>
        /// overwrite gotoTop with nothing to do in popup dialog.
        /// </summary>
        protected override void GotoTop()
        {
        }

        /// <summary>
        /// Change the master page.
        /// </summary>
        protected override void ChangeMasterPage()
        {
            MasterPageFile = ApplicationRoot + "Dialog.Master";
        }

        /// <summary>
        /// Override the RecordUrl, so that Session[ACAConstant.CURRENT_URL] not set.
        /// </summary>
        protected override void RecordUrl()
        {
        }

        #endregion Methods
    }
}