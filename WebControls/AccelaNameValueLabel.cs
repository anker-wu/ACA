#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaNameValueLabel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaLabel.cs 195191 2011-09-02 02:02:50Z ACHIEVO\Daniel.Shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide a label to show key-value pair.
    /// </summary>
    public class AccelaNameValueLabel : AccelaLabel
    {
        #region Properties

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Layout type.
        /// </summary>
        public ControlLayoutType LayoutType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value CSS class.
        /// </summary>
        /// <value>The value CSS class.</value>
        public string ValueCssClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets DateType
        /// </summary>
        public DateType DateType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date time object
        /// </summary>
        public object DateValue
        {
            get { return this.ViewState["DateValue"] as object; }
            set { this.ViewState["DateValue"] = value; }
        }

        /// <summary>
        /// Gets the value label client ID.
        /// </summary>
        public override string ValueLabelClientID
        {
            get
            {
                return ClientID + "_value";
            }
        }

        /// <summary>
        /// Gets or sets labelWidth.
        /// </summary>
        public string LabelWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Field width.
        /// </summary>
        public string FieldWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is need progress.
        /// </summary>
        /// <value><c>true</c> if this instance is need progress; otherwise, <c>false</c>.</value>
        public bool IsNeedProgress
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating file upload behavior.
        /// </summary>
        public FileUploadBehavior FileUploadBehavior
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that initialize silverlight progress parameter.
        /// </summary>
        public string ProgressParams 
        {
            get; 
            set; 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            //If Label control has child controls, it will not render the 'Text' attribute.
            //So, need to remove all the children before the control rendering.
            this.RenderChildren(writer);
            this.Controls.Clear();

            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(LabelKey))
            {
                this.Text = LabelConvertUtil.GetTextByKey(LabelKey, GetModuleName(), this);

                if (this.Text == LabelConvertUtil.GetGlobalTextByKey("aca_sys_default_module_name") && !string.IsNullOrEmpty(ModuleName))
                {
                    this.Text = DataUtil.AddBlankToString(ModuleName);
                }
            }

            if (LayoutType == ControlLayoutType.Horizontal)
            {
                writer.Write("<table style=\"width:" + Width + ";\" role='presentation' parentcontrol='true' cellpadding=0 cellspacing=0 id='" + ClientID + "_table' class='ACA_TDAlignLeftOrRightTop' >");
                writer.Write("<tr><td>");
                writer.Write("<div id=\"" + ClientID + "_element_group\"><table role='presentation' cellpadding=0 cellspacing=0><tr>");
                writer.Write("<td style='vertical-align: top;width:" + LabelWidth + "'>");
                writer.Write("<p>");
                base.Render(writer);
                writer.Write("&nbsp;</p>");
                writer.Write("</td>");
                writer.Write("<td style='vertical-align: top;width:" + FieldWidth + "'>");
                writer.Write("<p class='break-word'>");

                if (string.IsNullOrEmpty(ValueCssClass))
                {
                    writer.Write("<span id='" + this.ClientID + "_value'>");
                }
                else
                {
                    writer.Write("<span id='" + this.ClientID + "_value' class='" + ValueCssClass + "'>");
                }

                writer.Write(GetText());
                writer.Write("</span>");
                writer.Write("</p>");
                writer.Write("</td></tr>");

                if (IsNeedProgress)
                {
                    writer.Write("<tr><td colspan='2'>");
                    writer.Write(GetProgressObject());
                    writer.Write("</td></tr>");
                }

                writer.Write("</table></div></td></tr></table>");
            }
            else
            {
                writer.Write("<table role='presentation' parentcontrol='true' cellpadding=0 cellspacing=0 id='" + ClientID + "_table' class='ACA_TDAlignLeftOrRightTop' >");
                writer.Write("<tr>");
                writer.Write("<td>");
                writer.Write("<p>");
                base.Render(writer);
                writer.Write("</p>");
                writer.Write("</td>");
                writer.Write("</tr>");
                writer.Write("<tr>");
                writer.Write("<td style=\"vertical-align: top;\">");
                writer.Write("<p class='break-word'>");

                if (string.IsNullOrEmpty(ValueCssClass))
                {
                    writer.Write("<span id='" + this.ClientID + "_value'>");
                }
                else
                {
                    writer.Write("<span id='" + this.ClientID + "_value' class='" + ValueCssClass + "'>");
                }

                writer.Write(GetText());
                writer.Write("</span>");
                writer.Write("</p>");
                writer.Write("</td>");
                writer.Write("</tr>");

                if (IsNeedProgress)
                {
                    writer.Write("<tr><td>");
                    writer.Write(GetProgressObject());
                    writer.Write("</td></tr>");
                }

                writer.Write("</table>");
            }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>the formatted text</returns>
        private string GetText()
        {
            if (this.DateValue == null)
            {
                return Value;
            }

            string result = string.Empty;
            DateTime date = DateTime.MinValue;
            bool isDateTime = false;

            if (this.DateValue is DateTime)
            {
                isDateTime = true;
                date = (DateTime)this.DateValue;
            }
            else
            {
                isDateTime = I18nDateTimeUtil.TryParseFromWebService(this.DateValue.ToString(), out date);
            }

            if (isDateTime)
            {
                switch (this.DateType)
                {
                    case DateType.LongDate:
                        result = I18nDateTimeUtil.FormatToLongDateStringForUI(date);
                        break;
                    case DateType.DateAndTime:
                        result = I18nDateTimeUtil.FormatToDateTimeStringForUI(date);
                        break;
                    case DateType.ShortDate:
                    default:
                        result = I18nDateTimeUtil.FormatToDateStringForUI(date);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Construct process object html.
        /// </summary>
        /// <returns>silver light object</returns>
        private string GetProgressObject()
        {
            var objectTarget = new StringBuilder();

            if (FileUploadBehavior == FileUploadBehavior.Html5)
            {
                string percent = "0%";

                if (ProgressParams == "100%")
                {
                    percent = "100%";
                }

                objectTarget.Append("<div class='divProgressBar'>");
                objectTarget.Append("<div class='html5ProgressBar'>");
                objectTarget.Append("<div class='bgColor' style='width:" + percent + "'></div>");
                objectTarget.Append("<font>" + percent + "</font>");
                objectTarget.Append("</div>");
                objectTarget.Append("</div>");
            }
            else
            {
                objectTarget.Append("<div id='divProgressBar'>");
                objectTarget.Append("<object id='fileProgress' data='data:application/x-silverlight-2,' type='application/x-silverlight-2' width='150' height='20'>");
                objectTarget.Append("<param name='source' value='../ClientBin/mpost.SilverlightMultiFileUpload.Progress.xap?<%=CommonUtil.GetRandomUniqueID() %>' />");
                objectTarget.Append("<param name='initParams' value='" + ProgressParams + "' />");
                objectTarget.Append("<param name='background' value='white' />");
                objectTarget.Append("<param name='onload' value='FileProgress_Loaded' />");
                objectTarget.Append("<param name='minRuntimeVersion' value='4.0.50401.0' />");
                objectTarget.Append("<param name='autoUpgrade' value='true' />");
                objectTarget.Append("<param name='windowless' value='true' />");
                objectTarget.Append("<a class='NotShowLoading' href='http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0' title='Get Microsoft Silverlight' style='text-decoration: none'>");
                objectTarget.Append("<img src='http://go.microsoft.com/fwlink/?LinkId=161376' alt='Get Microsoft Silverlight' style='border-style: none' />");
                objectTarget.Append("</a>");
                objectTarget.Append("</object>");
                objectTarget.Append("</div>");
            }

            return objectTarget.ToString();
        }

        #endregion
    }
}
