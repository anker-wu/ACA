#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GradingStyle.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  Examination,Education Pass Score
 *
 *  Notes:
 *      $Id: GradingStyle.ascx.cs 139167 2009-07-15 06:20:30Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Display Grade Style Information in Education,Examination Form
    /// </summary>
    public partial class GradingStyle : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Default Value
        /// </summary>
        private const int DEFAULTVALUE = 0;

        /// <summary>
        /// Final score id for GView.
        /// </summary>
        private const string FINAL_SCORE_ID = "txtFinalScore_txtFinalScore";

        /// <summary>
        /// Section ID
        /// </summary>
        private string _sectionId = string.Empty;

        /// <summary>
        /// Grading Style Define
        /// </summary>
        public enum GradingStyleType
        {
            /// <summary>
            /// Pass Fail, Display DropDownList
            /// </summary>
            passfail,

            /// <summary>
            /// Score, Display TextBox
            /// </summary>
            score,

            /// <summary>
            /// Percentage Score, Display TextBox with Percentage
            /// </summary>
            percentage,

            /// <summary>
            /// None, Display Nothing
            /// </summary>
            none
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Display Type.
        /// </summary>
        public GradingStyleType DisplayType
        {
            get
            {
                string displayType = txtGradingStyle.Text;

                return GetGradingStyleByName(displayType);
            }

            set
            {
                txtGradingStyle.Text = value.ToString();

                DisplayControl(value);
            }
        }

        /// <summary>
        /// Gets or sets Display Type String
        /// </summary>
        public string DisplayTypeString
        {
            get
            {
                return DisplayType.ToString();
            }

            set
            {
                DisplayType = GetGradingStyleByName(value);
            }
        }
         
        /// <summary>
        /// Gets or sets GradingStyle Value. Set value after Set DisplayType
        /// </summary>
        public string Value
        {
            get
            {
                return GetValue(GetGradingStyleByName(txtGradingStyle.Text));
            }

            set
            {
                SetValue(GetGradingStyleByName(txtGradingStyle.Text), value);
            }
        }

        /// <summary>
        /// Gets or sets CSSClass Value.
        /// </summary>
        public string CssClass
        {
            get
            {
                return Convert.ToString(ViewState["CssClass"]);
            }

            set 
            {
                ViewState["CssClass"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Label Key
        /// </summary>
        public string LabelKey
        {
            get
            {
                return Convert.ToString(ViewState["LabelKey"]);
            }

            set
            {
                ViewState["LabelKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Required
        /// </summary>
        public bool Required
        {
            get
            {
                return Convert.ToBoolean(ViewState["Required"]);
            }

            set
            {
                ViewState["Required"] = value;
            }
        }

        /// <summary>
        /// Gets or sets section id. 
        /// </summary>
        public string SectionId
        {
            get
            {
                return _sectionId;
            }

            set
            {
                _sectionId = value;
            }
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        public GFilterScreenPermissionModel4WS Permission
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the final score disable or not
        /// </summary>
        public bool Disable
        {
            get
            {
                return !txtPassScore.Enabled;
            }

            set
            {
                ddlPassFail.Enabled = !value;
                txtPassScore.Enabled = !value;
                txtPercentageScore.Enabled = !value;
                txtFinalScore.Enabled = !value;

                if (value)
                {
                    ddlPassFail.ClearSelection();
                    txtPassScore.Text = string.Empty;
                    txtPercentageScore.Text = string.Empty;
                    txtFinalScore.Text = string.Empty;
                }                
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">sender Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            txtPercentageScore.FieldUnit = ACAConstant.COMMA_PERCENT.ToString();

            //Set LabelKey and Value
            ddlPassFail.LabelKey = LabelKey;
            txtPassScore.LabelKey = LabelKey;
            txtPercentageScore.LabelKey = LabelKey;
            txtFinalScore.LabelKey = LabelKey;

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(CssClass))
                {
                    ddlPassFail.CssClass += ACAConstant.BLANK + CssClass;
                    txtPassScore.CssClass += ACAConstant.BLANK + CssClass;
                    txtPercentageScore.CssClass += ACAConstant.BLANK + CssClass;
                    txtFinalScore.CssClass += ACAConstant.BLANK + CssClass;
                }
            }
            else
            {
                string currentValue = Value;
                DisplayTypeString = txtGradingStyle.Text;
                Value = currentValue;
            }

            // add validation final score field.
            AddValidationForFinalScore();

            //Set Required
            RequireControl();
        }

        /// <summary>
        /// Control Initial Event
        /// </summary>
        /// <param name="e">Event Args</param>
        protected override void OnInit(EventArgs e)
        {
            txtFinalScore.ID = ID;

            // Textbox txtGradingStyle is hidden field and is not presented to users.
            txtGradingStyle.Attributes.Add("title", GetTextByKey(LabelKey));

            if (!IsPostBack)
            {  
                DropDownListBindUtil.BindPassFailDropDown(ddlPassFail, true);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Get Grading Style by Name
        /// </summary>
        /// <param name="s">Grading Style Key Name</param>
        /// <returns>Grading Style</returns>
        private GradingStyleType GetGradingStyleByName(string s)
        {
            GradingStyleType displayType;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    displayType = (GradingStyleType)Enum.Parse(typeof(GradingStyleType), s, true);
                }
                catch
                {
                    displayType = GradingStyleType.none;
                }
            }
            else
            {
                displayType = GradingStyleType.none;
            }

            return displayType;
        }

        /// <summary>
        /// Get Value by Grading Style
        /// </summary>
        /// <param name="displayType">Display Type</param>
        /// <returns>return value of this Control</returns>
        private string GetValue(GradingStyleType displayType)
        {
            string value = string.Empty;

            switch (displayType)
            {
                case GradingStyleType.passfail:
                    {
                        value = ddlPassFail.SelectedValue;

                        break;
                    }

                case GradingStyleType.score:
                    {
                        double? nullablePassScore = txtPassScore.DoubleValue;
                        value = nullablePassScore == null ? string.Empty : I18nNumberUtil.FormatNumberForWebService(nullablePassScore.Value);

                        break;
                    }

                case GradingStyleType.percentage:
                    {
                        double? nullablePercentageScore = txtPercentageScore.DoubleValue;
                        value = nullablePercentageScore == null ? string.Empty : I18nNumberUtil.FormatNumberForWebService(nullablePercentageScore.Value);

                        break;
                    }

                case GradingStyleType.none:
                    {
                        double? nullableFinalScore = txtFinalScore.DoubleValue;
                        value = nullableFinalScore == null ? string.Empty : I18nNumberUtil.FormatNumberForWebService(nullableFinalScore.Value);

                        break;
                    }
            }

            return value;
        }

        /// <summary>
        /// Set value by Grading Style
        /// </summary>
        /// <param name="displayType">Display Type</param>
        /// <param name="value">Text Value of this control</param>
        private void SetValue(GradingStyleType displayType, string value)
        {
            if (value != null)
            {
                switch (displayType)
                {
                    case GradingStyleType.passfail:
                        {
                            DropDownListBindUtil.SetSelectedValue(ddlPassFail, value);

                            break;
                        }

                    case GradingStyleType.score:
                        {
                            txtPassScore.Text = I18nNumberUtil.ConvertNumberFromWebServiceToInput(value);

                            break;
                        }

                    case GradingStyleType.percentage:
                        {
                            txtPercentageScore.Text = I18nNumberUtil.ConvertNumberFromWebServiceToInput(value);

                            break;
                        }

                    case GradingStyleType.none:
                        {
                            txtFinalScore.Text = I18nNumberUtil.ConvertNumberFromWebServiceToInput(value);

                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Display Control Style by Display Type
        /// </summary>
        /// <param name="displayType">Display Type</param>
        private void DisplayControl(GradingStyleType displayType)
        {
            divPassFail.Style.Add("display", "none");
            divPassScore.Style.Add("display", "none");
            divPercentageScore.Style.Add("display", "none");
            divScore.Style.Add("display", "none");
           
            //Display Control by Type,and Set Value
            switch (displayType)
            {
                case GradingStyleType.passfail:
                    {
                        divPassFail.Style.Add("display", "block");

                        break;
                    }

                case GradingStyleType.score:
                    {
                        divPassScore.Style.Add("display", "block");

                        break;
                    }

                case GradingStyleType.percentage:
                    {
                        divPercentageScore.Style.Add("display", "block");

                        break;
                    }

                case GradingStyleType.none:
                    {
                        if (!txtFinalScore.IsHidden)
                        {
                            divScore.Style.Add("display", "block");
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// add required validation with final score field according to Admin config
        /// </summary>        
        private void AddValidationForFinalScore()
        {
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(ModuleName, Permission, _sectionId, AppSession.User.UserID);

            if (models == null)
            {
                return;
            }

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (model.recStatus == ACAConstant.INVALID_STATUS)
                {
                    continue;
                }

                if (model.required != null &&
                    model.required.ToUpper() == ACAConstant.COMMON_Y && model.viewElementName == FINAL_SCORE_ID && !Disable)
                {
                    txtFinalScore.Validate += string.IsNullOrEmpty(txtFinalScore.Validate) ? "required" : ";required";

                    break;
                }
            }
        }

        /// <summary>
        /// Require Control
        /// </summary>
        private void RequireControl()
        {
            if (txtFinalScore.Validate.Contains("required") && !Disable)
            {
                ddlPassFail.Required = true;
                txtPassScore.Validate += string.IsNullOrEmpty(txtPassScore.Validate) ? "required" : ";required";
                txtPercentageScore.Validate += string.IsNullOrEmpty(txtPercentageScore.Validate) ? "required" : ";required";
            }
            else
            {
                ddlPassFail.Required = false;
                txtPassScore.Validate = txtPassScore.Validate.Replace("required", string.Empty);
                txtPercentageScore.Validate = txtPercentageScore.Validate.Replace("required", string.Empty);
            }
        }
 
        #endregion
    }
}
