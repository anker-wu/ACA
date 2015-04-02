#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaWatermarkBox.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaWatermarkText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a watermark box to input string 
    /// </summary>
    public class AccelaWatermarkBox : AccelaTextBox
    {
        #region Fields

        /// <summary>
        /// the description for the water-mask text-box.
        /// </summary>
        private string _watermarkText;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the description for the WaterMask textbox.
        /// </summary>
        public string WatermarkText
        {
            get
            {
                return _watermarkText;
            }

            set
            {
                _watermarkText = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            TextBoxWatermarkExtender watermarkExd = new TextBoxWatermarkExtender();
            watermarkExd.TargetControlID = ID;
            watermarkExd.WatermarkText = WatermarkText;
            watermarkExd.WatermarkCssClass = "watermarked";
            Controls.Add(watermarkExd);

            base.OnPreRender(e);
        }

        #endregion Methods
    }
}