/**
 * <pre>
 *
 *  Accela
 *  File: InputCheckBoxField.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InputCheckBoxField.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/10/2007    daly.zeng    Initial.
 * </pre>
 */

using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// RadioButtonField class
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    internal sealed class RadioButtonField : CheckBoxField
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RadioButtonField class.
        /// </summary>
        public RadioButtonField()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///  This method is called by the ExtractRowValues methods of
        /// GridView and DetailsView. Retrieve the current value of the
        /// cell from the Checked state of the Radio button.
        /// </summary>
        /// <param name="dictionary">A System.Collections.IDictionary used to store the values of the specified cell.</param>
        /// <param name="cell">The System.Web.UI.WebControls.DataControlFieldCell that contains the values to retrieve.</param>
        /// <param name="rowState">One of the System.Web.UI.WebControls.DataControlRowState values.</param>
        /// <param name="includeReadOnly">true to include the values of read-only fields; otherwise, false.</param>
        public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
        {
            // Determine whether the cell contains a RadioButton
            // in its Controls collection.
            if (cell.Controls.Count > 0)
            {
                RadioButton radio = cell.Controls[0] as RadioButton;

                object checkedValue = null;
                if (null == radio)
                {
                    // A RadioButton is expected, but a null is encountered.
                    // Add error handling.
                    //throw new InvalidOperationException("RadioButtonField could not extract control.");
                    throw new InvalidOperationException(LabelConvertUtil.GetTextByKey("ACA_AccelaGridView_CanNotExtractRadioButton", string.Empty));
                }
                else
                {
                    checkedValue = radio.Checked;
                }

                // Add the value of the Checked attribute of the
                // RadioButton to the dictionary.
                if (dictionary.Contains(DataField))
                {
                    dictionary[DataField] = checkedValue;
                }
                else
                {
                    dictionary.Add(DataField, checkedValue);
                }
            }
        }

        /// <summary>
        /// make sure that none of radio button selected.
        /// </summary>
        /// <returns>Always returns false.</returns>
        protected override object GetDesignTimeValue()
        {
            return false;
        }

        /// <summary>
        /// This method adds a RadioButton control and any other
        /// content to the cell's Controls collection.
        /// </summary>
        /// <param name="cell">The System.Web.UI.WebControls.DataControlFieldCell to initialize.</param>
        /// <param name="rowState">One of the System.Web.UI.WebControls.DataControlRowState values.</param>
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            base.InitializeDataCell(cell, rowState);

            if (cell.Controls.Count != 0)
            {
                return;
            }

            AccelaGridView grid = (AccelaGridView)Control;

            if (grid.AutoGenerateRadioButtonColumn)
            {
                string checkedString = string.Empty;
                string radioId = grid.GetCheckBoxID();
                string name = string.Format("{0}_{1}", grid.ClientID, AccelaGridView.RADIOBUTTONGROUPNAME);
                string clientId = string.Format("{0}_{1}", grid.ClientID, radioId);
                string hdValue = grid.GetSelectItems();

                if (!string.IsNullOrEmpty(hdValue) && !grid.IsClearSelectedItems && hdValue.IndexOf("," + radioId + ",") > -1)
                {
                    checkedString = " checked=\"checked\" ";
                }

                Literal output = new Literal();

                output.ID = radioId;
                output.Text = string.Format(
                    "<input type=\"radio\" name=\"{0}\" id=\"{1}\" runat=\"server\" value=\"{2}\" title=\"{3}\" onclick=\"Check(this,'{4}', true);{5};\" {6} />",
                    name,
                    clientId,
                    radioId,
                    LabelConvertUtil.GetTextByKey("aca_selectonerecord_checkbox", string.Empty),
                    grid.GetSelectedItemsFieldClientID(),
                    grid.OnClientSelectSingle,
                    checkedString);

                cell.Controls.Add(output);
                cell.CssClass = "ACA_AlignLeftOrRightTop";
            }
        }

        #endregion Methods
    }
}
