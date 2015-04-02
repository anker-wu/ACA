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

using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// This class is only used in GridView to generate checkbox column
    /// </summary>
    internal sealed class InputCheckBoxField : CheckBoxField
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the InputCheckBoxField class.
        /// </summary>
        public InputCheckBoxField()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initializes the specified System.Web.UI.WebControls.DataControlFieldCell
        /// object to the specified row state.
        /// </summary>
        /// <param name="cell">The System.Web.UI.WebControls.DataControlFieldCell to initialize.</param>
        /// <param name="rowState">One of the System.Web.UI.WebControls.DataControlRowState values.</param>
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            base.InitializeDataCell(cell, rowState);

            // Add a checkbox anyway, if not done already
            if (cell.Controls.Count == 0)
            {
                CheckBox chk = new CheckBox();
                chk.InputAttributes.Add("class", "aca_gridview_checkbox");

                AccelaGridView grid = (AccelaGridView)this.Control;

                if (grid.AutoGenerateCheckBoxColumn)
                {
                    chk.ID = grid.GetCheckBoxID(); //AccelaGridView.CHECKBOXID; //
                    chk.Attributes.Add("onclick", "Check(this,'" + grid.GetSelectedItemsFieldClientID() + "');" + grid.OnClientSelectSingle);
                    chk.InputAttributes.Add("title", LabelConvertUtil.GetTextByKey("aca_selectonerecord_checkbox", string.Empty));
                    string hdValue = grid.GetSelectItems();

                    if (!string.IsNullOrEmpty(hdValue))
                    {
                        if (!grid.IsClearSelectedItems)
                        {
                            chk.Checked = hdValue.IndexOf("," + chk.ID + ",") > -1;
                        }
                        else
                        {
                            chk.Checked = false;
                        }
                    }

                    cell.Controls.Add(chk);
                    cell.CssClass = "ACA_AlignLeftOrRightTop";
                }
                else
                {
                    chk.ID = AccelaGridView.CHECKBOXID;
                    cell.Controls.Add(chk);
                    cell.CssClass = "ACA_Column_Short";
                }
            }
        }

        #endregion Methods
    }
}
