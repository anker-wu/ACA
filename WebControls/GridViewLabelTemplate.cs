#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: GridViewLabelTemplate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GridViewLabelTemplate.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide label template for grid view 
    /// </summary>
    public class GridViewLabelTemplate : ITemplate
    {
        #region Fields

        /// <summary>
        /// grid view column name
        /// </summary>
        private string _columnName;

        /// <summary>
        /// CSS class for template
        /// </summary>
        private string _cssClass;

        /// <summary>
        /// data type for template
        /// </summary>
        private string _dataType;

        /// <summary>
        /// mask for label
        /// </summary>
        private string _mask;

        /// <summary>
        /// template type
        /// </summary>
        private DataControlRowType _templateType;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GridViewLabelTemplate class.
        /// </summary>
        /// <param name="type">grid view row type</param>
        /// <param name="colName">column name</param>
        /// <param name="dataType">data type for grid view</param>
        /// <param name="cssClass">CSS for label row</param>
        /// <param name="mask">mask for label text</param>
        public GridViewLabelTemplate(DataControlRowType type, string colName, string dataType, string cssClass, string mask)
        {
            _templateType = type;
            _columnName = colName;
            _dataType = dataType;
            _cssClass = cssClass;
            _mask = mask;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// InstantiateIn method
        /// </summary>
        /// <param name="container">Control object</param>
        public void InstantiateIn(System.Web.UI.Control container)
        {
            DataControlFieldCell fieldCell = null;

            switch (_templateType)
            {
                case DataControlRowType.Header:
                    // build the header for this column
                    Literal literal = new Literal();
                    literal.Text = "<b>" + _columnName + "</b>";
                    container.Controls.Add(literal);
                    break;

                case DataControlRowType.DataRow:
                    // build one row in this column
                    Label label = new Label();
                    fieldCell = (DataControlFieldCell)container;
                    fieldCell.CssClass = label.CssClass = _cssClass;

                    // register an event handler to perform the data binding
                    label.DataBinding += new EventHandler(Label_DataBinding);
                    container.Controls.Add(label);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Raise data binding event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        private void Label_DataBinding(object sender, EventArgs e)
        {
            // get the control that raised this event
            Label label = (Label)sender;

            // get the containing row
            GridViewRow row = (GridViewRow)label.NamingContainer;

            // get the raw data value and make it pretty
            string rawValue = DataBinder.Eval(row.DataItem, _columnName).ToString();
            if (rawValue.Trim() == string.Empty)
            {
                return;
            }

            switch (_dataType)
            {
                case "System.DateTime":
                    label.Text = DateTime.Parse(rawValue).ToString(_mask, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    break;
                case "System.Double":
                    label.Text = string.Format(_mask, double.Parse(rawValue));
                    break;
                case "System.Int16":
                case "System.Int32":
                    label.Text = rawValue;
                    break;
                case "System.String":
                    label.Text = rawValue;
                    break;
            }
        }

        #endregion Methods
    }
}