#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaWebControlExtenderExtender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaWebControlExtenderExtender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.ComponentModel;
using System.Web.UI;
using Accela.ACA.WSProxy.Common;
using Accela.Web.Controls;
using AjaxControlToolkit;

[assembly: WebResource("Accela.Web.Controls.AccelaWebControlExtender.AccelaWebControlExtenderBehavior.js", "text/javascript")]

namespace AccelaWebControlExtender
{
    /// <summary>
    /// Extender Accela web control for ACA admin
    /// </summary>
    [Designer(typeof(AccelaWebControlExtenderDesigner))]
    [ClientScriptResource("AccelaWebControlExtender.AccelaWebControlExtenderBehavior", "Accela.Web.Controls.AccelaWebControlExtender.AccelaWebControlExtenderBehavior.js")]
    [TargetControlType(typeof(IAccelaBaseControl))]
    public sealed class AccelaWebControlExtenderExtender : ExtenderControlBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the control is visible in client
        /// </summary>
        [ExtenderControlProperty]
        public bool ClientVisible
        {
            get
            {
                return GetPropertyValue("ClientVisible", true);
            }

            set
            {
                SetPropertyValue("ClientVisible", value);
            }
        }

        /// <summary>
        /// Gets or sets Control ID
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string ControlID
        {
            get
            {
                return GetPropertyValue("ControlID", string.Empty);
            }

            set
            {
                SetPropertyValue("ControlID", value);
            }
        }

        /// <summary>
        /// Gets or sets Default Label
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string DefaultLabel
        {
            get
            {
                return GetPropertyValue("DefaultLabel", string.Empty);
            }

            set
            {
                SetPropertyValue("DefaultLabel", value);
            }
        }

        /// <summary>
        /// Gets or sets Default Language Sub Label
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string DefaultLanguageSubLabel
        {
            get
            {
                return GetPropertyValue("DefaultLanguageSubLabel", string.Empty);
            }

            set
            {
                SetPropertyValue("DefaultLanguageSubLabel", value);
            }
        }

        /// <summary>
        /// Gets or sets Default Language Label
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string DefaultLanguageText
        {
            get
            {
                return GetPropertyValue("DefaultLanguageText", string.Empty);
            }

            set
            {
                SetPropertyValue("DefaultLanguageText", value);
            }
        }

        /// <summary>
        /// Gets or sets Default Label
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string DefaultSubLabel
        {
            get
            {
                return GetPropertyValue("DefaultSubLabel", string.Empty);
            }

            set
            {
                SetPropertyValue("DefaultSubLabel", value);
            }
        }

        /// <summary>
        /// Gets or sets element type
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string ElementType
        {
            get
            {
                return GetPropertyValue("ElementType", string.Empty);
            }

            set
            {
                SetPropertyValue("ElementType", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether configure URL is enabled
        /// </summary>
        [ExtenderControlProperty]
        public bool EnableConfigureURL
        {
            get
            {
                return GetPropertyValue("EnableConfigureURL", false);
            }

            set
            {
                SetPropertyValue("EnableConfigureURL", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether RecordType Filter is enabled
        /// </summary>
        [ExtenderControlProperty]
        public bool EnableRecordTypeFilter
        {
            get
            {
                return GetPropertyValue("EnableRecordTypeFilter", false);
            }

            set
            {
                SetPropertyValue("EnableRecordTypeFilter", value);
            }
        }

        /// <summary>
        /// Gets or sets Grid Columns Visible
        /// </summary>
        [ExtenderControlProperty]
        public string GridColumnsVisible
        {
            get
            {
                return GetPropertyValue("GridColumnsVisible", string.Empty);
            }

            set
            {
                SetPropertyValue("GridColumnsVisible", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is not render label
        /// </summary>
        [ExtenderControlProperty]
        [ClientPropertyName("IsHiddenLabel")]
        public bool IsHiddenLabel
        {
            get
            {
                return GetPropertyValue("IsHiddenLabel", false);
            }

            set
            {
                SetPropertyValue("IsHiddenLabel", value);
            }
        }

        /// <summary>
        /// Gets or sets Label Key
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string LabelKey
        {
            get
            {
                return GetPropertyValue("LabelKey", string.Empty);
            }

            set
            {
                SetPropertyValue("LabelKey", value);
            }
        }

        /// <summary>
        /// Gets or sets the real width of the grid view
        /// </summary>
        [ExtenderControlProperty]
        public int GridViewRealWidth
        {
            get
            {
                return GetPropertyValue("GridViewRealWidth", 0);
            }

            set
            {
                SetPropertyValue("GridViewRealWidth", value);
            }
        }

        /// <summary>
        /// Gets or sets Label type
        /// </summary>
        [ExtenderControlProperty]
        public int LabelType
        {
            get
            {
                return GetPropertyValue("LabelType", -1);
            }

            set
            {
                SetPropertyValue("LabelType", value);
            }
        }

        /// <summary>
        /// Gets or sets module name
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string ModuleName
        {
            get
            {
                return GetPropertyValue("ModuleName", string.Empty);
            }

            set
            {
                SetPropertyValue("ModuleName", value);
            }
        }

        /// <summary>
        /// Gets or sets Report ID
        /// </summary>
        [ExtenderControlProperty]
        public string ReportID
        {
            get
            {
                return GetPropertyValue("ReportID", string.Empty);
            }

            set
            {
                SetPropertyValue("ReportID", value);
            }
        }

        /// <summary>
        /// Gets or sets restrict display. when the LabelType is ApplicantText, this property indicate the restrict display option
        /// </summary>
        [ExtenderControlProperty]
        public int RestrictDisplay
        {
            get
            {
                return GetPropertyValue("RestrictDisplay", 0);
            }

            set
            {
                SetPropertyValue("RestrictDisplay", value);
            }
        }

        /// <summary>
        /// Gets or sets Auto Fill Type
        /// </summary>
        [ExtenderControlProperty]
        public int AutoFillType
        {
            get
            {
                return GetPropertyValue("AutoFillType", -1);
            }

            set
            {
                SetPropertyValue("AutoFillType", value);
            }
        }

        /// <summary>
        /// Gets or sets Section ID
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string SectionID
        {
            get
            {
                return GetPropertyValue("SectionID", string.Empty);
            }

            set
            {
                SetPropertyValue("SectionID", value);
            }
        }

        /// <summary>
        /// Gets or sets View Element ID
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string ViewElementID
        {
            get
            {
                return GetPropertyValue("ViewElementID", string.Empty);
            }

            set
            {
                SetPropertyValue("ViewElementID", value);
            }
        }

        /// <summary>
        /// Gets or sets PositionID
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string PositionID
        {
            get
            {
                return GetPropertyValue("PositionID", string.Empty);
            }

            set
            {
                SetPropertyValue("PositionID", value);
            }
        }

        /// <summary>
        /// Gets or sets show type
        /// </summary>
        [ExtenderControlProperty]
        public int ShowType
        {
            get
            {
                return GetPropertyValue("ShowType", -1);
            }

            set
            {
                SetPropertyValue("ShowType", value);
            }
        }

        /// <summary>
        /// Gets or sets Source type
        /// </summary>
        [ExtenderControlProperty]
        public int SourceType
        {
            get
            {
                return GetPropertyValue("SourceType", -1);
            }

            set
            {
                SetPropertyValue("SourceType", value);
            }
        }

        /// <summary>
        /// Gets or sets the bind standard choice category name
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public string StdCategory
        {
            get
            {
                return GetPropertyValue("StdCategory", string.Empty);
            }

            set
            {
                SetPropertyValue("StdCategory", value);
            }
        }

        /// <summary>
        /// Gets or sets the bind standard choice category name
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public int ListType
        {
            get
            {
                return GetPropertyValue("ListType", -1);
            }

            set
            {
                SetPropertyValue("ListType", value);
            }
        }

        /// <summary>
        /// Gets or sets the max value length for standard choice item
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        public int MaxLength
        {
            get
            {
                return GetPropertyValue("MaxLength", 0);
            }

            set
            {
                SetPropertyValue("MaxLength", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is a template field
        /// </summary>
        [ExtenderControlProperty]
        [ClientPropertyName("IsTemplateField")]
        public bool IsTemplateField
        {
            get
            {
                return GetPropertyValue("IsTemplateField", false);
            }

            set
            {
                SetPropertyValue("IsTemplateField", value);
            }
        }

        /// <summary>
        /// Gets or sets the template attribute.
        /// </summary>
        /// <value>The template attribute.</value>
        [ExtenderControlProperty(true, true)]
        [ClientPropertyName("TemplateAttribute")]
        public TemplateAttribute TemplateAttribute
        {
            get
            {
                return GetPropertyValue("TemplateAttribute", new TemplateAttribute());
            }

            set
            {
                SetPropertyValue("TemplateAttribute", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [grid view allow paging].
        /// </summary>
        /// <value><c>true</c> if [grid view allow paging]; otherwise, <c>false</c>.</value>
        [ExtenderControlProperty]
        public bool GridViewAllowPaging
        {
            get
            {
                return GetPropertyValue("GridViewAllowPaging", false);
            }

            set
            {
                SetPropertyValue("GridViewAllowPaging", value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the grid view page.
        /// </summary>
        /// <value>The size of the grid view page.</value>
        [ExtenderControlProperty]
        public int GridViewPageSize
        {
            get
            {
                return GetPropertyValue("GridViewPageSize", 0);
            }

            set
            {
                SetPropertyValue("GridViewPageSize", value);
            }
        }

        /// <summary>
        /// Gets or sets div type
        /// </summary>
        [ExtenderControlProperty]
        public int DivType
        {
            get
            {
                return GetPropertyValue("DivType", -1);
            }

            set
            {
                SetPropertyValue("DivType", value);
            }
        }

        /// <summary>
        /// Gets or sets template genus
        /// </summary>
        [ExtenderControlProperty]
        public string TemplateGenus
        {
            get
            {
                return GetPropertyValue("TemplateGenus", string.Empty);
            }

            set
            {
                SetPropertyValue("TemplateGenus", value);
            }
        }

        /// <summary>
        ///  Gets or sets a control's ClientID.
        /// The value of this control contains a section's permission value.
        /// In Admin, section property grid will get the section permission value from this control.
        /// </summary>
        [ExtenderControlProperty]
        public string PermissionValueId
        {
            get
            {
                return GetPropertyValue("PermissionValueId", string.Empty);
            }

            set
            {
                SetPropertyValue("PermissionValueId", value);
            }
        }

        /// <summary>
        /// Gets or sets the sub-container's client id.
        /// </summary>
        [ExtenderControlProperty]
        public string SubContainerClientID
        {
            get
            {
                return GetPropertyValue("SubContainerClientID", string.Empty);
            }

            set
            {
                SetPropertyValue("SubContainerClientID", value);
            }
        }

        #endregion Properties
    }
}