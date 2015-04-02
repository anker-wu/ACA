#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaMultipleControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: The composite control for multiple controls.
 *
 *  Notes:
 * $Id: AccelaMultipleControl.cs 238998 2012-12-04 09:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,            Who,        What
 *  Dec 4, 2012      Alan Hu     Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;

[assembly: WebResource("Accela.Web.Controls.AccelaMultipleControl.js", "text/javascript")]

namespace Accela.Web.Controls
{
    /// <summary>
    /// This class implement the composite control that render the multiple controls which have the same control type.
    /// </summary>
    public class AccelaMultipleControl : AccelaCompositeControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the child control count
        /// </summary>
        public int ChildControlCount
        {
            get
            {
                return Convert.ToInt32(ViewState["ChildControlCount"]);
            }

            set
            {
                ViewState["ChildControlCount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the control type.
        /// </summary>
        public ControlType ChildControlType { get; set; }

        /// <summary>
        /// Gets or sets the child control sub label.
        /// </summary>
        public string ChildControlSubLabel { get; set; }

        /// <summary>
        /// Gets or sets the child control sub label full name.
        /// </summary>
        public string ChildControlSubLabelFullName { get; set; }

        /// <summary>
        /// Gets or sets the duplicate validation setting.
        /// </summary>
        public DuplicateValidateSetting DuplicateValidate { get; set; }

        /// <summary>
        /// Gets or sets the AccelaDropDownList's set.
        /// </summary>
        public AccelaDropDownList DropDownListSet { get; set; }

        /// <summary>
        /// Gets or sets the AccelaTextBox's set.
        /// </summary>
        public AccelaTextBox TextBoxSet { get; set; }

        /// <summary>
        /// Gets the child control collection.
        /// </summary>
        public IList<IAccelaControl> Children
        {
            get
            {
                IList<IAccelaControl> children = new List<IAccelaControl>();

                for (int i = 0; i < ChildControlCount; i++)
                {
                    string controlID = string.Format("{0}{1}", CHILD_CONTROL_ID_PREFIX, i);
                    IAccelaControl accelaControl = FindControl(controlID) as IAccelaControl;

                    if (accelaControl != null)
                    {
                        children.Add(accelaControl);
                    }
                }

                return children;
            }
        }

        /// <summary>
        /// Gets or sets next focus control id.
        /// </summary>
        public string NextFocusControlID { get; set; }

        /// <summary>
        /// Gets or sets the control width.
        /// </summary>
        public Unit ControlWidth { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Initialize extender controls.
        /// </summary>
        public override void InitExtenderControl()
        {
        }

        /// <summary>
        /// Is hide the required indicate.
        /// </summary>
        /// <returns>If need hide the required indicate.</returns>
        public override bool IsHideRequireIndicate()
        {
            return false;
        }

        /// <summary>
        /// Check the fields is required or not.
        /// </summary>
        /// <returns>If the fields is required.</returns>
        public override bool IsRequired()
        {
            return IsFieldRequired;
        }

        /// <summary>
        /// Clear value, it not implement now.
        /// </summary>
        public override void ClearValue()
        {
        }

        /// <summary>
        /// Disable the edit form, it not implement now.
        /// </summary>
        public override void DisableEdit()
        {
        }

        /// <summary>
        /// Enable the edit form, it not implement now.
        /// </summary>
        public override void EnableEdit()
        {
        }

        /// <summary>
        /// Get the multiple control's value, split with split character.
        /// </summary>
        /// <returns>The multiple control's value.</returns>
        public override string GetValue()
        {
            string result = string.Empty;

            foreach (IAccelaControl accelaControl in Children.Where(accelaControl => accelaControl != null))
            {
                string value = accelaControl.GetValue();

                if (!string.IsNullOrEmpty(value))
                {
                    result += value + ACAConstant.SPLIT_CHAR;
                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = result.TrimEnd(ACAConstant.SPLIT_CHAR);
            }

            return result;
        }

        /// <summary>
        /// Set the multiple control's value.
        /// </summary>
        /// <param name="value">The value that want to set.</param>
        public override void SetValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            string[] values = value.Split(ACAConstant.SPLIT_CHAR);

            for (int i = 0; i < values.Length; i++)
            {
                IAccelaControl accelaControl = Children[i];
                accelaControl.SetValue(values[i]);
            }
        }

        #endregion Public Methods

        #region Protected Control Event

        /// <summary>
        /// Create children controls
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            base.CreateChildControls();

            bool isNeedAddSubLabel = !string.IsNullOrEmpty(ChildControlSubLabel) && ChildControlCount > 1;

            if (isNeedAddSubLabel)
            {
                Controls.Add(new Literal { Text = "<table role='presentation' cellspacing='0' cellpadding='0'>" });
            }

            for (int i = 0; i < ChildControlCount; i++)
            {
                // create the sub label for each child control.
                if (isNeedAddSubLabel)
                {
                    Literal literal = new Literal();
                    string subLabel = LabelConvertUtil.GetTextByKey(ChildControlSubLabel, GetModuleName());
                    literal.Text = string.Format("<tr><td class='SubLabel ACA_Label font12px'>{0}</td><td class='SubControl'>", subLabel + (i + 1));
                    
                    Controls.Add(literal);
                }

                CreateChildControl(i, isNeedAddSubLabel);

                if (isNeedAddSubLabel)
                {
                    Controls.Add(new Literal { Text = "</td></tr>" });
                }
            }

            if (isNeedAddSubLabel)
            {
                Controls.Add(new Literal { Text = "</table>" });
            }

            ChildControlsCreated = true;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            bool isAdmin = AccelaControlRender.IsAdminRender(this);

            if (!isAdmin && Page != null)
            {
                InitValidatorExtender();

                //Register the client scripts resource.
                ScriptManager.RegisterClientScriptResource(Page, GetType(), "Accela.Web.Controls.AccelaMultipleControl.js");

                string javascript = string.Format("AccelaMultipleControl.initializeControl('{0}', '{1}', '{2}');", ClientID, NextFocusControlID, ControlWidth);
                ScriptManager.RegisterStartupScript(Page, GetType(), "initMultipleCtl" + ClientID, javascript, true);
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the control on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode || string.IsNullOrEmpty(ChildControlSubLabel))
            {
                base.Render(writer);
                return;
            }

            if (LayoutType == ControlLayoutType.Vertical)
            {
                writer.Write("<div class=\"MultipleControl_Vertical\">");
            }
            else
            {
                writer.Write("<div class=\"MultipleControl_Horizontal\">");
            }

            //Register a action to adjust the control's layout after document ready.
            string javascript = string.Format(
                                "AccelaMultipleControl.adjustSubFieldLayout('{0}', '{1}', '{2}');",
                                ClientID, 
                                Convert2HtmlType(ChildControlType),
                                LayoutType);

            ScriptManager.RegisterStartupScript(Page, GetType(), "adjustLayout" + ClientID, javascript, true);

            base.Render(writer);

            writer.Write("</div>");
        }

        #endregion Protected Control Event

        #region Private Methods

        /// <summary>
        /// Create the child control
        /// </summary>
        /// <param name="idSeqNbr">The id sequence number.</param>
        /// <param name="needAddSubLabel">Indicate whether need add sub label or not.</param>
        private void CreateChildControl(int idSeqNbr, bool needAddSubLabel)
        {
            string moduleName = GetModuleName();
            StringBuilder tooltip = new StringBuilder();

            // Add the parent label to tooltip.
            if (idSeqNbr == 0)
            {
                tooltip.Append(LabelConvertUtil.GetTextByKey(LabelKey, moduleName));
            }

            if (needAddSubLabel)
            {
                tooltip.Append(LabelConvertUtil.GetTextByKey(ChildControlSubLabelFullName, moduleName));
                tooltip.Append(idSeqNbr + 1);
            }

            if (IsRequired())
            {
                tooltip.Append(ACAConstant.BLANK);
                tooltip.Append(LabelConvertUtil.GetGlobalTextByKey("aca_required_field"));
            }

            // create the child control. Do NOT set the LabelKey, or it will set all label/help to all child controls.
            switch (ChildControlType)
            {
                case ControlType.Text:
                    AccelaTextBox textBox = new AccelaTextBox();
                    textBox.ID = string.Format("{0}{1}", CHILD_CONTROL_ID_PREFIX, idSeqNbr);
                    textBox.Width = TextBoxSet.Width.Value > 0 ? TextBoxSet.Width : Unit.Percentage(100);
                    textBox.MaxLength = TextBoxSet.MaxLength;
                    textBox.TrimValue = TextBoxSet.TrimValue;
                    TextBoxSet.LabelKey = LabelKey;

                    string waterMarkText = TextBoxSet.GetWatermarkText();

                    if (!string.IsNullOrEmpty(waterMarkText))
                    {
                        tooltip.Append(ACAConstant.BLANK);
                        tooltip.Append(waterMarkText);
                    }

                    textBox.WatermarkText = waterMarkText;
                    textBox.ToolTip = tooltip.ToString();

                    Controls.Add(textBox);

                    break;
                case ControlType.DropdownList:
                    AccelaDropDownList dropDownList = new AccelaDropDownList();
                    dropDownList.ID = string.Format("{0}{1}", CHILD_CONTROL_ID_PREFIX, idSeqNbr);
                    dropDownList.Width = DropDownListSet.Width.Value > 0 ? DropDownListSet.Width : Unit.Percentage(100);
                    dropDownList.ShowType = DropDownListSet.ShowType;
                    dropDownList.ToolTip = tooltip.ToString();

                    Controls.Add(dropDownList);

                    break;
            }
        }

        /// <summary>
        /// Initialize validator extender control
        /// </summary>
        private void InitValidatorExtender()
        {
            bool isRequired = IsRequired();
            bool isNeedDuplicateValidate = DuplicateValidate != null && DuplicateValidate.NeedValidate;

            if (!isRequired && !isNeedDuplicateValidate)
            {
                return;
            }

            // Create the custom validator controls for requried field.
            for (int i = 0; i < Children.Count; i++)
            {
                WebControl control = Children[i] as WebControl;

                if (control != null)
                {
                    if (isRequired)
                    {
                        string validateId = string.Format("RequiredValidate{0}", i);
                        CreateRequiredValidate(validateId, "AccelaMultipleControl.validatorForRequired", control);
                    }

                    if (isNeedDuplicateValidate)
                    {
                        string validateId = string.Format("DuplicateValidate{0}", i);
                        CreateDuplicateValidate(validateId, control.ID, control.ClientID);
                    }
                }
            }
        }

        /// <summary>
        /// Create duplicate validate control
        /// </summary>
        /// <param name="id">The new required validate control's id</param>
        /// <param name="controlToValidate">The input control id to validate</param>
        /// <param name="currentValidateClientID">The current child control when validate</param>
        private void CreateDuplicateValidate(string id, string controlToValidate, string currentValidateClientID)
        {
            var customValidator = new CustomValidator();
            string errorMsgLabelKey = DuplicateValidate != null ? DuplicateValidate.MessageLabelKey : string.Empty;
            string errorMessage = !string.IsNullOrEmpty(errorMsgLabelKey) ? LabelConvertUtil.GetTextByKey(errorMsgLabelKey, this) : string.Empty;

            customValidator.ID = id;
            customValidator.ControlToValidate = controlToValidate;
            customValidator.Display = ValidatorDisplay.None;
            customValidator.ClientValidationFunction = "AccelaMultipleControl.validatorForDuplicate";
            customValidator.ErrorMessage = errorMessage;
            customValidator.ValidateEmptyText = true;
            Controls.Add(customValidator);
            CreateValidatorCallbackExtender(customValidator.ID, currentValidateClientID);
        }

        /// <summary>
        /// Convert to html type.
        /// </summary>
        /// <param name="controlType">control type</param>
        /// <returns>html control type</returns>
        private string Convert2HtmlType(ControlType controlType)
        {
            string result = string.Empty;

            switch (controlType)
            {
                case ControlType.DropdownList:
                    result = "select";
                    break;

                case ControlType.Text:
                    result = "input[type=text]";
                    break;
            }

            return result;
        }

        #endregion Private Methods

        #region Inner Class

        /// <summary>
        /// This class provide the duplicate validate setting
        /// </summary>
        public class DuplicateValidateSetting
        {
            /// <summary>
            /// Gets or sets a value indicating whether need duplicate validate or not.
            /// </summary>
            public bool NeedValidate { get; set; }

            /// <summary>
            /// Gets or sets the duplicate validate message.
            /// </summary>
            public string MessageLabelKey { get; set; }
        }

        #endregion Inner Class
    }
}
