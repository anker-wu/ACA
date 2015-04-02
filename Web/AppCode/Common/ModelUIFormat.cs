#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ModelUIFormat.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  LabelUtil for getting label by label key.
 *  UI should call this class if need to get text in .cs.
 *
 *  Notes:
 *      $Id: ModelUIFormat.cs 278473 2014-09-04 09:19:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Format UI string for model
    /// </summary>
    public static class ModelUIFormat
    {
        #region Fields

        /// <summary>
        /// Dollar char
        /// </summary>
        private const string DOLLAR_CHAR = "$";

        #endregion

        #region Methods

        /// <summary>
        /// Formats the asset template field.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="resValue">The resource value.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>The formatted template field.</returns>
        public static string FormatAssetTemplateField(string value, string resValue, string fieldType)
        {
            string result = I18nStringUtil.GetString(resValue, value);

            switch (fieldType)
            {
                case ACAConstant.AssetTemplateFieldType.Date:
                    result = I18nDateTimeUtil.FormatToDateStringForUI(result);
                    break;
                case ACAConstant.AssetTemplateFieldType.Number:
                    result = I18nNumberUtil.FormatNumberForUI(result);
                    break;
                case ACAConstant.AssetTemplateFieldType.Time:
                    result = I18nDateTimeUtil.ConvertTimeStringFromWebServiceToUI(result, false);
                    break;
                case ACAConstant.AssetTemplateFieldType.Radio:
                    result = ConvertYN2I18NLabel(result);
                    break;
            }

            return result;
        }

        /// <summary>
        /// get the I18N label for Y/N
        /// </summary>
        /// <param name="yn">Y or N string</param>
        /// <returns>I18N string for Y/N</returns>
        public static string ConvertYN2I18NLabel(string yn)
        {
            if (string.IsNullOrEmpty(yn))
            {
                return yn;
            }

            return LabelUtil.GetGlobalTextByKey(ACAConstant.COMMON_Y.Equals(yn, StringComparison.InvariantCulture) ? "aca_checkbox_checked_label" : "aca_checkbox_unchecked_label");
        }

        /// <summary>
        /// if label controls' row is empty, it will hide it.
        /// </summary>
        /// <param name="control">table control.</param>
        public static void HiddenEmptyRow(System.Web.UI.Control control)
        {
            if (control == null)
            {
                return;
            }

            foreach (System.Web.UI.Control childControl in control.Controls)
            {
                if (childControl.Controls.Count > 0)
                {
                    //intial table row is visible.
                    if (childControl is HtmlTableRow)
                    {
                        childControl.Visible = false;
                    }

                    HiddenEmptyRow(childControl);
                }
                else
                {
                    //the control is child control.
                    if (childControl is AccelaLabel && (childControl as AccelaLabel).Text != string.Empty)
                    {
                        if (childControl.Parent.Parent is HtmlTableRow)
                        {
                            HtmlTableRow tableRow = childControl.Parent.Parent as HtmlTableRow;
                            tableRow.Visible = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the formatted value for "Work Location"
        /// </summary>
        /// <param name="addr">AddressModel array</param>
        /// <param name="moduleName">module name</param>
        /// <param name="lines">line number</param>
        /// <returns>Html string for UI display</returns>
        public static string FormatAddress4Display(AddressModel[] addr, string moduleName, out int lines)
        {
            string result = string.Empty;
            lines = 0;

            if (addr != null && addr.Length > 0)
            {
                AddressModel[] addresses = ResortAddress(addr);
                GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                {
                    permissionLevel = GViewConstant.PERMISSION_APO,
                    permissionValue = GViewConstant.SECTION_ADDRESS
                };

                SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.AddressEdit);
                string additionalLocationTitle = LabelUtil.GetTextByKey("aca_permitDetail_label_showAdditionalLocations", moduleName);
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
                result = addressBuilderBll.Build4WorkLocation(addresses, models, additionalLocationTitle);
            }

            return result;
        }

        /// <summary>
        /// Get formatted UI string to display applicant
        /// </summary>
        /// <param name="capContact">CapContactModel4WS object</param>
        /// <param name="moduleName">module name</param>
        /// <param name="lines">line number</param>
        /// <returns>Html string for applicant display</returns>
        public static string FormatApplicant4Display(CapContactModel4WS capContact, string moduleName, out int lines)
        {
            int line = 0;
            StringBuilder breakline = new StringBuilder();
            StringBuilder buf = new StringBuilder();

            if (capContact != null 
                && capContact.people != null
                && !ACAConstant.INVALID_STATUS.Equals(capContact.people.auditStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
                buf.Append("<table role='presentation' class='table_child'>");
                buf.Append("<tr><td class='td_child_left font12px'>");
                StringBuilder contant = new StringBuilder();

                contant.Append(" <div class='ACA_TabRow ACA_FLeft'>");

                GFilterScreenPermissionModel4WS permission =
                    ControlBuildHelper.GetPermissionWithGenericTemplate(GviewID.ContactEdit, GViewConstant.PERMISSION_PEOPLE, capContact.people.contactType, capContact.people.template);
                SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.ContactEdit);

                bool isExistName = false;

                if (!string.IsNullOrEmpty(capContact.people.contactTypeFlag))
                {
                    contant.Append(ScriptFilter.FilterScript(DropDownListBindUtil.GetTypeFlagTextByValue(capContact.people.contactTypeFlag)));
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!I18nCultureUtil.IsChineseCultureEnabled)
                {
                    if (!string.IsNullOrEmpty(capContact.people.salutation) && gviewBll.IsFieldVisible(models, "ddlAppSalutation"))
                    {
                        string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, capContact.people.salutation);
                        contant.AppendFormat("<span class='contactinfo_salutation'>{0} </span>", salutation);
                        breakline.Append(salutation + " ");
                        isExistName = true;
                    }
                }

                if (!string.IsNullOrEmpty(capContact.people.firstName) && gviewBll.IsFieldVisible(models, "txtAppFirstName"))
                {
                    contant.AppendFormat("<span class='contactinfo_firstname'>{0} </span>", ScriptFilter.FilterScript(capContact.people.firstName));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.firstName) + " ");
                    isExistName = true;
                }

                if (!string.IsNullOrEmpty(capContact.people.middleName) &&
                    gviewBll.IsFieldVisible(models, "txtAppMiddleName"))
                {
                    contant.AppendFormat("<span class='contactinfo_middlename'>{0} </span>", ScriptFilter.FilterScript(capContact.people.middleName));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.middleName) + " ");
                    isExistName = true;
                }

                if (!string.IsNullOrEmpty(capContact.people.lastName) && gviewBll.IsFieldVisible(models, "txtAppLastName"))
                {
                    contant.AppendFormat("<span class='contactinfo_lastname'>{0} </span>", ScriptFilter.FilterScript(capContact.people.lastName));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.lastName + " "));
                    isExistName = true;
                }

                if (!string.IsNullOrEmpty(capContact.people.fullName) && gviewBll.IsFieldVisible(models, "txtAppFullName"))
                {
                    contant.AppendFormat("<span class='contactinfo_fullname'>{0} </span>", ScriptFilter.FilterScript(capContact.people.fullName));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.fullName + " "));
                    isExistName = true;
                }

                if (!string.IsNullOrEmpty(capContact.people.namesuffix) && gviewBll.IsFieldVisible(models, "txtAppSuffix"))
                {
                    contant.AppendFormat("<span class='contactinfo_namesuffix'>{0}</span>", ScriptFilter.FilterScript(capContact.people.namesuffix));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.namesuffix));
                    isExistName = true;
                }

                if (I18nCultureUtil.IsChineseCultureEnabled)
                {
                    if (!string.IsNullOrEmpty(capContact.people.salutation) && gviewBll.IsFieldVisible(models, "ddlAppSalutation"))
                    {
                        string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, capContact.people.salutation);
                        contant.AppendFormat("<span class='contactinfo_salutation'> {0}</span>", salutation);
                        breakline.Append(" " + salutation);
                    }
                }

                if (!string.IsNullOrEmpty(capContact.people.title) && gviewBll.IsFieldVisible(models, "txtTitle"))
                {
                    if (isExistName)
                    {
                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()) && !contant.ToString().EndsWith(ACAConstant.HTML_BR))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }

                    contant.AppendFormat("<span class='contactinfo_title'>{0}</span>", ScriptFilter.FilterScript(capContact.people.title));
                    breakline.Append(ScriptFilter.FilterScript(" " + capContact.people.title));
                    isExistName = true;
                }

                //contant.Append(ACAConstant.HTML_BR);--
                if (isExistName)
                {
                    line += BreakWords(breakline.ToString());
                    if (!string.IsNullOrEmpty(breakline.ToString()) && !contant.ToString().EndsWith(ACAConstant.HTML_BR))
                    {
                        contant.Append(ACAConstant.HTML_BR);
                    }

                    breakline = new StringBuilder();
                }

                if (capContact.people.birthDate != null && gviewBll.IsFieldVisible(models, "txtAppBirthDate"))
                {
                    string birthDate = LabelUtil.GetTextByKey("per_appInfoEdit_label_birthDate", moduleName);
                    string bd = birthDate + "<span dir='LTR'>" + ScriptFilter.FilterScript(I18nDateTimeUtil.FormatToDateStringForUI(capContact.people.birthDate)) + "</span> ";
                    contant.AppendFormat("<span class='contactinfo_birthdate'>{0} </span>", bd);
                    breakline.Append(bd);
                    isExistName = true;

                    if (isExistName)
                    {
                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()) &&
                            !contant.ToString().EndsWith(ACAConstant.HTML_BR))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }
                }

                if (!string.IsNullOrEmpty(capContact.people.gender) && gviewBll.IsFieldVisible(models, "radioListAppGender"))
                {
                    string gender = StandardChoiceUtil.GetGenderByKey(capContact.people.gender);
                    contant.AppendFormat("<span class='contactinfo_gender'>{0} </span>", ScriptFilter.FilterScript(gender));
                    breakline.Append(ScriptFilter.FilterScript(gender) + " ");
                    isExistName = true;

                    // --
                    if (isExistName)
                    {
                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()) &&
                            !contant.ToString().EndsWith(ACAConstant.HTML_BR))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }
                }

                if (!string.IsNullOrEmpty(capContact.people.businessName) &&
                    gviewBll.IsFieldVisible(models, "txtAppOrganizationName"))
                {
                    contant.AppendFormat("<span class='contactinfo_businessname'>{0}</span>", ScriptFilter.FilterScript(capContact.people.businessName));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.businessName));
                    isExistName = true;
                    if (isExistName)
                    {
                        line += BreakWords(breakline.ToString());
                        breakline = new StringBuilder();
                        if (!contant.ToString().EndsWith(ACAConstant.HTML_BR))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(capContact.people.businessName2) &&
                    gviewBll.IsFieldVisible(models, "txtBusinessName2"))
                {
                    contant.AppendFormat("<span class='contactinfo_businessname2'>{0}</span>", ScriptFilter.FilterScript(capContact.people.businessName2));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.businessName2));
                    isExistName = true;
                    if (isExistName)
                    {
                        line += BreakWords(breakline.ToString());
                        breakline = new StringBuilder();
                        if (!contant.ToString().EndsWith(ACAConstant.HTML_BR))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(capContact.people.tradeName) && gviewBll.IsFieldVisible(models, "txtAppTradeName"))
                {
                    contant.AppendFormat("<span class='contactinfo_tradename'>{0}</span>", ScriptFilter.FilterScript(capContact.people.tradeName));
                    breakline.Append(ScriptFilter.FilterScript(capContact.people.tradeName));
                    line += BreakWords(breakline.ToString());

                    if (!string.IsNullOrEmpty(breakline.ToString()))
                    {
                        contant.Append(ACAConstant.HTML_BR);
                    }

                    breakline = new StringBuilder();
                }

                if (capContact.people.compactAddress != null)
                {
                    if (!string.IsNullOrEmpty(capContact.people.compactAddress.addressLine1) &&
                        gviewBll.IsFieldVisible(models, "txtAppStreetAdd1"))
                    {
                        contant.AppendFormat("<span class='contactinfo_addressline1'>{0}</span>", ScriptFilter.FilterScript(capContact.people.compactAddress.addressLine1));
                        breakline.Append(ScriptFilter.FilterScript(capContact.people.compactAddress.addressLine1));
                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.compactAddress.addressLine2) && gviewBll.IsFieldVisible(models, "txtAppStreetAdd2"))
                    {
                        contant.AppendFormat("<span class='contactinfo_addressline2'>{0}</span>", ScriptFilter.FilterScript(capContact.people.compactAddress.addressLine2));
                        breakline.Append(ScriptFilter.FilterScript(capContact.people.compactAddress.addressLine2));
                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.compactAddress.addressLine3) && gviewBll.IsFieldVisible(models, "txtAppStreetAdd3"))
                    {
                        contant.AppendFormat("<span class='contactinfo_addressline3'>{0}</span>", ScriptFilter.FilterScript(capContact.people.compactAddress.addressLine3));
                        breakline.Append(ScriptFilter.FilterScript(capContact.people.compactAddress.addressLine3));
                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }

                    //bool isNewLine = false;
                    if (!string.IsNullOrEmpty(capContact.people.compactAddress.city) && gviewBll.IsFieldVisible(models, "txtAppCity"))
                    {
                        contant.AppendFormat("<span class='contactinfo_region'>{0}, </span>", ScriptFilter.FilterScript(capContact.people.compactAddress.city));
                        breakline.Append(ScriptFilter.FilterScript(capContact.people.compactAddress.city) + ", ");
                    }

                    if (!string.IsNullOrEmpty(capContact.people.compactAddress.state) && gviewBll.IsFieldVisible(models, "txtAppState"))
                    {
                        contant.AppendFormat("<span class='contactinfo_region'>{0}, </span>", ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(capContact.people.compactAddress.state, capContact.people.compactAddress.countryCode)));
                        breakline.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(capContact.people.compactAddress.state, capContact.people.compactAddress.countryCode)) + ", ");
                    }

                    if (!string.IsNullOrEmpty(capContact.people.compactAddress.zip) && gviewBll.IsFieldVisible(models, "txtAppZipApplicant"))
                    {
                        contant.AppendFormat("<span class='contactinfo_region'>{0}</span>", FormatZipShow(capContact.people.compactAddress.zip, capContact.people.compactAddress.countryCode));
                        breakline.Append(FormatZipShow(capContact.people.compactAddress.zip, capContact.people.compactAddress.countryCode));

                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.compactAddress.countryCode) && gviewBll.IsFieldVisible(models, "ddlAppCountry"))
                    {
                        string country = StandardChoiceUtil.GetCountryByKey(capContact.people.compactAddress.countryCode);
                        contant.AppendFormat("<span class='contactinfo_country'>{0}</span>", ScriptFilter.FilterScript(country));
                        contant.Append(" ");
                        breakline.Append(ScriptFilter.FilterScript(country) + " ");
                        line += BreakWords(breakline.ToString());

                        if (!string.IsNullOrEmpty(breakline.ToString()))
                        {
                            contant.Append(ACAConstant.HTML_BR);
                        }

                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.postOfficeBox) && gviewBll.IsFieldVisible(models, "txtAppPOBox"))
                    {
                        string postOfficeBox = BasePage.GetStaticTextByKey("per_appInfoEdit_label_poBox");
                        contant.AppendFormat("<span class='contactinfo_postofficebox'>{0}{1}</span>", postOfficeBox, ScriptFilter.FilterScript(capContact.people.postOfficeBox));
                        breakline.Append(ScriptFilter.FilterScript(capContact.people.postOfficeBox));
                        line += BreakWords(breakline.ToString());
                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.phone1) &&
                        gviewBll.IsFieldVisible(models, "txtAppPhone1"))
                    {
                        string phone1 = FormatPhoneShow(capContact.people.phone1CountryCode, capContact.people.phone1, capContact.people.countryCode);
                        contant.AppendFormat("<span class='contactinfo_phone1'>{0}</span>", string.Format(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_phone1"), phone1)));
                        breakline.Append(phone1);
                        line++;
                        breakline = new StringBuilder();
                    }                   

                    if (!string.IsNullOrEmpty(capContact.people.phone3) &&
                        gviewBll.IsFieldVisible(models, "txtAppPhone3"))
                    {
                        string phone3 = ModelUIFormat.FormatPhoneShow(capContact.people.phone3CountryCode, capContact.people.phone3, capContact.people.countryCode);
                        contant.AppendFormat("<span class='contactinfo_phone3'>{0}</span>", string.Format(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_phone3"), phone3)));
                        breakline.Append(phone3);
                        line++;
                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.phone2) &&
                       gviewBll.IsFieldVisible(models, "txtAppPhone2"))
                    {
                        string phone2 = ModelUIFormat.FormatPhoneShow(capContact.people.phone2CountryCode, capContact.people.phone2, capContact.people.countryCode);
                        contant.AppendFormat("<span class='contactinfo_phone2'>{0}</span>", string.Format(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_phone2"), phone2)));
                        breakline.Append(phone2);
                        line++;
                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.fax) &&
                        gviewBll.IsFieldVisible(models, "txtAppFax"))
                    {
                        string fax = ModelUIFormat.FormatPhoneShow(capContact.people.faxCountryCode, capContact.people.fax, capContact.people.countryCode);
                        contant.AppendFormat("<span class='contactinfo_fax'>{0}</span>", string.Format(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_fax"), fax)));
                        breakline.Append(fax);
                        line++;
                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.people.email) && gviewBll.IsFieldVisible(models, "txtAppEmail"))
                    {
                        contant.AppendFormat("<span class='contactinfo_email'>{0}</span>", string.Format(I18nStringUtil.FormatToTableRow(ScriptFilter.FilterScript(capContact.people.email))));
                        breakline.Append(ScriptFilter.FilterScript(capContact.people.email));
                        line++;
                        breakline = new StringBuilder();
                    }

                    if (!string.IsNullOrEmpty(capContact.accessLevel) && gviewBll.IsFieldVisible(models, "radioListContactPermission"))
                    {
                        string accessLevel = ScriptFilter.FilterScript(DropDownListBindUtil.GetContactPermissionTextByValue(capContact.accessLevel, moduleName), false);
                        contant.AppendFormat(I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("contact_permission_viewname", moduleName), accessLevel));
                        breakline.Append(ScriptFilter.FilterScript(capContact.accessLevel));
                        line++;
                        breakline = new StringBuilder();
                    }
                }

                contant.Append("</div>");

                //display generic template fields
                if (capContact.people != null && capContact.people.template != null)
                {
                    var fields = GenericTemplateUtil.GetAllFields(capContact.people.template);
                    StringBuilder genericTemplate = new StringBuilder();

                    if (fields != null && fields.Count() > 0)
                    {
                        foreach (var field in fields)
                        {
                            string asiSecurity = ASISecurityUtil.GetASISecurity(
                              field.serviceProviderCode,
                              field.groupName,
                              field.subgroupName,
                              field.fieldName,
                              moduleName);

                            if ((field.acaTemplateConfig != null && (ValidationUtil.IsHidden(field.acaTemplateConfig.acaDisplayFlag) || ValidationUtil.IsNo(field.acaTemplateConfig.acaDisplayFlag)))
                                || ACAConstant.ASISecurity.None.Equals(asiSecurity))
                            {
                                continue;
                            }

                            string fieldValue = ScriptFilter.FilterScript(GetTemplateValue4Display(field));

                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                string altLabel = string.Empty;

                                if (field.acaTemplateConfig != null)
                                {
                                    altLabel = I18nStringUtil.GetString(field.acaTemplateConfig.resFieldLabel, field.acaTemplateConfig.fieldLabel);
                                }

                                string fieldName = I18nStringUtil.GetString(altLabel, field.displayFieldName, field.fieldName);
                                genericTemplate.Append(fieldName);
                                genericTemplate.Append(ACAConstant.COLON_CHAR);
                                genericTemplate.Append(fieldValue);
                                genericTemplate.Append(ACAConstant.HTML_BR);
                            }
                        }

                        if (genericTemplate.Length > 0)
                        {
                            contant.Append(" <div class='ACA_TabRow ACA_FLeft'>");
                            contant.Append(genericTemplate);
                            contant.Append("</div>");
                        }   
                    }

                    //template table
                    if (capContact.people.template.templateTables != null)
                    {
                        string genericTemplateTableContent = GenerateGenericTemplateTableInDetailView(capContact.people.template.templateTables, moduleName);

                        if (!string.IsNullOrEmpty(genericTemplateTableContent))
                        {
                            string imgTitle = LabelUtil.GetTextByKey("img_alt_expand_icon", moduleName);
                            string keyValue = LabelUtil.GetTextByKey("aca_capdetail_contact_label_templatetable", moduleName);

                            string[] newTitle = { imgTitle, keyValue };
                            string rawTitle = LabelUtil.RemoveHtmlFormat(DataUtil.ConcatStringWithSplitChar(newTitle, ACAConstant.BLANK));
                            string finalTitle = ScriptFilter.AntiXssHtmlEncode(rawTitle);

                            contant.Append("<div class='ACA_TabRow ACA_FLeft capdetail_template_table'>");
                            contant.Append("<div class='Header_h2'><a href=\"javascript:void(0);\" onclick='ControlDisplay($(this).parent().next().get(0),this.firstChild,true,this,$(this).next().get(0))'");
                            contant.AppendFormat("title=\"{0}\" class=\"NotShowLoading\">", finalTitle);
                            contant.AppendFormat("<img style=\"cursor: pointer; border-width:0px;\" alt=\"{0}\" src=\"{1}\"/>", imgTitle, ImageUtil.GetImageURL("plus_expand.gif"));
                            contant.Append("</a>&nbsp;");
                            contant.AppendFormat("<span style=\"display: inline-block\">{0}</span></div>", keyValue);
                            contant.Append("<div style=\"display:none;\">");
                            contant.Append(genericTemplateTableContent);
                            contant.Append("</div>");
                            contant.Append("</div>");
                        }
                    }
                }

                //Init contact address section
                if (StandardChoiceUtil.IsEnableContactAddress())
                {
                    contant.Append(FormatContactAddressSection(moduleName, capContact));
                }

                buf.Append("</td><td>");
                buf.Append(contant);
                buf.Append("</td></tr>");
                buf.Append("</table>");
                lines = line;
                return buf.ToString();
            }
            else
            {
                lines = line;
                return string.Empty;
            }
        }

        /// <summary>
        /// Generate HTML contents for specific generic template table.
        /// </summary>
        /// <param name="templateGroups">Generic template table group.</param>
        /// <param name="moduleName">Module name.</param>
        /// <returns>the html contents for generic template table.</returns>
        public static string GenerateGenericTemplateTableInDetailView(TemplateGroup[] templateGroups, string moduleName)
        {
            ASITUITable[] _asitUITables = null;

            if (templateGroups != null && templateGroups.Length > 0)
            {
                TemplateSubgroup[] allTables = CapUtil.GetGenericTemplateTables(moduleName, templateGroups);

                _asitUITables = ASITUIModelUtil.ConvertTemplateTablesToUITables(allTables, moduleName, string.Empty, string.Empty);
            }

            //loop each field in each group and create corresponding control on UI
            if (_asitUITables == null || _asitUITables.Length == 0)
            {
                return string.Empty;
            }

            List<StringBuilder> resultTableList = new List<StringBuilder>();

            foreach (ASITUITable appTable in _asitUITables)
            {
                bool isEmptyTable = true;
                var asitFieldsWithResValues = ASITUIModelUtil.GetASITFieldsWithResValues(appTable);
                StringBuilder tableContent = new StringBuilder();

                if (appTable.Rows != null && appTable.Rows.Count != 0)
                {
                    tableContent.Append("<tr>");
                    tableContent.Append("<td class='ACA_AlignLeftOrRight'>");
                    tableContent.Append("<div style='width:100%;' class='ACA_TabRow ACA_Title_Text'>" + appTable.TableTitle + "</div>");
                    tableContent.Append("</td>");
                    tableContent.Append("</tr>");

                    foreach (UIRow asitRow in appTable.Rows)
                    {
                        tableContent.Append("<tr>");
                        tableContent.Append("<td style='width:100%' class='ACA_AlignLeftOrRight'>");
                        tableContent.Append("<div style='width:100%;' class=\"ACA_TabRow_ASI\">");

                        foreach (ASITUIField field in asitRow.Fields)
                        {
                            string resFieldValue = ASITUIModelUtil.GetASITFieldResValue(field, asitFieldsWithResValues);
                            string displayValue = I18nStringUtil.GetString(resFieldValue, field.Value);

                            if (field.IsHidden || field.IsHiddenByExp || string.IsNullOrEmpty(displayValue))
                            {
                                continue;
                            }

                            isEmptyTable = false;

                            tableContent.Append("<table role='presentation' class='ACA_AlignLeftOrRight ACA_TableWordBreak' CellPadding='0' CellSpacing='0' style='position:relative;left:2px;width:100%;' border='0'>");
                            tableContent.Append("<tr><td style='vertical-align:top;' class='ACA_AlignLeftOrRight'>");
                            tableContent.Append("<p>");
                            tableContent.Append(ScriptFilter.FilterScript(field.Label));
                            tableContent.Append(ACAConstant.COLON_CHAR);
                            tableContent.Append(ScriptFilter.FilterScript(displayValue));
                            tableContent.Append("</p>");
                            tableContent.Append("</td></tr></table>");
                        }

                        tableContent.Append("</div>");
                        tableContent.Append("</td>");
                        tableContent.Append("</tr>");
                    }
                }

                if (!isEmptyTable)
                {
                    resultTableList.Add(tableContent);
                }
            }

            StringBuilder result = new StringBuilder();

            //Add data table and split line to container.
            for (int i = 0; i < resultTableList.Count; i++)
            {
                result.Append("<table role='presentation' CellPadding='0' CellSpacing='0' style='width:100%'>");
                result.Append(resultTableList[i]);

                if (i < resultTableList.Count - 1)
                {
                    result.Append("<tr>");
                    result.Append("<td>");
                    result.Append("<div class='ACA_TabRow ACA_BkGray'>&nbsp;</div>");
                    result.Append("</td>");
                    result.Append("</tr>");
                }

                result.Append("</table>");
            }

            return result.ToString();
        }

        /// <summary>
        /// Format cap contact for UI display basic information.
        /// </summary>
        /// <param name="capContact">cap contact model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="isInConfirmPage">true if in cap confirm page, false if in cap detail page or others</param>
        /// <returns>Html string for cap contact display</returns>
        public static string FormatCapContactModel4Basic(CapContactModel4WS capContact, string moduleName, bool isInConfirmPage)
        {
            if (capContact == null)
            {
                return string.Empty;
            }

            return FormatCapContactModel4Basic(capContact.people, moduleName, isInConfirmPage);
        }

        /// <summary>
        /// Format cap contact for UI display basic information.
        /// </summary>
        /// <param name="people">cap contact people model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="isInConfirmPage">true if in cap confirm page, false if in cap detail page or others</param>
        /// <returns>Html string for cap contact display</returns>
        public static string FormatCapContactModel4Basic(PeopleModel4WS people, string moduleName, bool isInConfirmPage)
        {
            if (people == null)
            {
                return string.Empty;
            }

            GFilterScreenPermissionModel4WS permission =
                ControlBuildHelper.GetPermissionWithGenericTemplate(GviewID.ContactEdit, GViewConstant.PERMISSION_PEOPLE, people.contactType, people.template);
            SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.ContactEdit);

            StringBuilder buf = new StringBuilder();
            buf.Append("<table role='presentation' style='TEMPLATE_STYLE' class='Confirm_table ACA_SmLabel ACA_SmLabel_FontSize'><tr><td>");
            bool isExistName = false;

            if (!string.IsNullOrEmpty(people.contactTypeFlag))
            {
                buf.Append(ScriptFilter.FilterScript(DropDownListBindUtil.GetTypeFlagTextByValue(people.contactTypeFlag)));
                buf.Append(ACAConstant.HTML_BR);
            }

            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (!I18nCultureUtil.IsChineseCultureEnabled)
            {
                if (!string.IsNullOrEmpty(people.salutation)
                    && gviewBll.IsFieldVisible(models, "ddlAppSalutation"))
                {
                    string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, people.salutation);
                    buf.Append("<span class='contactinfo_salutation'>");
                    buf.Append(ScriptFilter.FilterScript(salutation));
                    buf.Append(" ");
                    buf.Append("</span>");
                    isExistName = true;
                }
            }

            if (!string.IsNullOrEmpty(people.firstName)
                && gviewBll.IsFieldVisible(models, "txtAppFirstName"))
            {
                buf.Append("<span class='contactinfo_firstname'>");
                buf.Append(ScriptFilter.FilterScript(people.firstName));
                buf.Append(" ");
                buf.Append("</span>");
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.middleName) &&
                gviewBll.IsFieldVisible(models, "txtAppMiddleName"))
            {
                buf.Append("<span class='contactinfo_middlename'>");
                buf.Append(ScriptFilter.FilterScript(people.middleName));
                buf.Append(" ");
                buf.Append("</span>");
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.lastName)
                && gviewBll.IsFieldVisible(models, "txtAppLastName"))
            {
                buf.Append("<span class='contactinfo_lastname'>");
                buf.Append(ScriptFilter.FilterScript(people.lastName));
                buf.Append("</span>");
                buf.Append(" ");
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.fullName)
                && gviewBll.IsFieldVisible(models, "txtAppFullName"))
            {
                buf.Append("<span class='contactinfo_fullname'>");
                buf.Append(ScriptFilter.FilterScript(people.fullName));
                buf.Append("</span>");
                buf.Append(" ");
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.namesuffix)
            && gviewBll.IsFieldVisible(models, "txtAppSuffix"))
            {
                buf.Append("<span class='contactinfo_namesuffix'>");
                buf.Append(ScriptFilter.FilterScript(people.namesuffix));
                buf.Append("</span>");
                isExistName = true;
            }

            if (I18nCultureUtil.IsChineseCultureEnabled)
            {
                if (!string.IsNullOrEmpty(people.salutation)
                    && gviewBll.IsFieldVisible(models, "ddlAppSalutation"))
                {
                    string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, people.salutation);
                    buf.Append("<span class='contactinfo_salutation'>");
                    buf.Append(" ");
                    buf.Append(ScriptFilter.FilterScript(salutation));
                    buf.Append("</span>");
                    isExistName = true;
                }
            }

            if (!string.IsNullOrEmpty(people.title) && gviewBll.IsFieldVisible(models, "txtTitle"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                buf.AppendFormat("<span class='contactinfo_title'>{0} </span>", ScriptFilter.FilterScript(people.title));
                isExistName = true;
            }

            if (people.birthDate != null
                && gviewBll.IsFieldVisible(models, "txtAppBirthDate"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                string birthDate = LabelUtil.GetTextByKey("per_appInfoEdit_label_birthDate", moduleName);
                buf.Append(birthDate);
                buf.Append("<span class='contactinfo_birthdate' dir='LTR'>" + ScriptFilter.FilterScript(I18nDateTimeUtil.FormatToDateStringForUI(people.birthDate)) + "</span>");
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.gender)
                && gviewBll.IsFieldVisible(models, "radioListAppGender"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                string gender = StandardChoiceUtil.GetGenderByKey(people.gender);
                buf.AppendFormat("<span class='contactinfo_gender'>{0}</span>", ScriptFilter.FilterScript(gender));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.businessName)
                && gviewBll.IsFieldVisible(models, "txtAppOrganizationName"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                buf.AppendFormat("<span class='contactinfo_businessname'>{0}</span>", ScriptFilter.FilterScript(people.businessName));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.businessName2)
                && gviewBll.IsFieldVisible(models, "txtBusinessName2"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                buf.AppendFormat("<span class='contactinfo_businessname2'>{0}</span>", ScriptFilter.FilterScript(people.businessName2));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.tradeName) && gviewBll.IsFieldVisible(models, "txtAppTradeName"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                buf.AppendFormat("<span class='contactinfo_tradename'>{0}</span>", ScriptFilter.FilterScript(people.tradeName));
                isExistName = true;
            }

            if (isInConfirmPage && !string.IsNullOrEmpty(people.socialSecurityNumber) && gviewBll.IsFieldVisible(models, "txtSSN"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                buf.AppendFormat("<span class='contactinfo_ssn'>{0}</span>", ScriptFilter.FilterScript(MaskUtil.FormatSSNShow(people.socialSecurityNumber)));
                isExistName = true;
            }

            if (isInConfirmPage && !string.IsNullOrEmpty(people.fein) && gviewBll.IsFieldVisible(models, "txtAppFein"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                buf.AppendFormat("<span class='contactinfo_fein'>{0}</span>", ScriptFilter.FilterScript(MaskUtil.FormatFEINShow(people.fein, StandardChoiceUtil.IsEnableFeinMasking())));
                isExistName = true;
            }

            if (people.compactAddress != null)
            {
                if (!string.IsNullOrEmpty(people.compactAddress.addressLine1)
                    && gviewBll.IsFieldVisible(models, "txtAppStreetAdd1"))
                {
                    if (isExistName)
                    {
                        buf.Append(ACAConstant.HTML_BR);
                    }

                    buf.AppendFormat("<span class='contactinfo_addressline1'>{0}</span>", ScriptFilter.FilterScript(people.compactAddress.addressLine1));
                    isExistName = true;
                }

                if (!string.IsNullOrEmpty(people.compactAddress.addressLine2)
                    && gviewBll.IsFieldVisible(models, "txtAppStreetAdd2"))
                {
                    if (isExistName)
                    {
                        buf.Append(ACAConstant.HTML_BR);
                    }

                    buf.AppendFormat("<span class='contactinfo_addressline2'>{0}</span>", ScriptFilter.FilterScript(people.compactAddress.addressLine2));
                    isExistName = true;
                }

                if (!string.IsNullOrEmpty(people.compactAddress.addressLine3) && gviewBll.IsFieldVisible(models, "txtAppStreetAdd3"))
                {
                    if (isExistName)
                    {
                        buf.Append(ACAConstant.HTML_BR);
                    }

                    buf.AppendFormat("<span class='contactinfo_addressline3'>{0}</span>", ScriptFilter.FilterScript(people.compactAddress.addressLine3));
                    isExistName = true;
                }

                string contactRegion = string.Empty;

                if (!string.IsNullOrEmpty(people.compactAddress.city)
                    && gviewBll.IsFieldVisible(models, "txtAppCity"))
                {
                    contactRegion = ConcatComma(string.Empty, ScriptFilter.FilterScript(people.compactAddress.city));
                }

                if (!string.IsNullOrEmpty(people.compactAddress.state)
                    && gviewBll.IsFieldVisible(models, "txtAppState"))
                {
                    contactRegion = ConcatComma(contactRegion, ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(people.compactAddress.state, people.compactAddress.countryCode)));
                }

                if (!string.IsNullOrEmpty(people.compactAddress.zip)
                    && gviewBll.IsFieldVisible(models, "txtAppZipApplicant"))
                {
                    string zip = FormatZipShow(people.compactAddress.zip, people.compactAddress.countryCode);

                    contactRegion = ConcatComma(contactRegion, zip);
                }

                if (!string.IsNullOrEmpty(contactRegion))
                {
                    if (isExistName)
                    {
                        buf.Append(ACAConstant.HTML_BR);
                    }

                    buf.AppendFormat("<span class='contactinfo_region'>{0}</span>", contactRegion);
                    isExistName = true;
                }

                if (!string.IsNullOrEmpty(people.compactAddress.countryCode)
                    && gviewBll.IsFieldVisible(models, "ddlAppCountry"))
                {
                    string country = ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(people.compactAddress.countryCode));

                    if (isExistName)
                    {
                        buf.Append(ACAConstant.HTML_BR);
                    }

                    buf.AppendFormat("<span class='contactinfo_country'>{0}</span>", country);
                    isExistName = true;
                }
            }

            if (!string.IsNullOrEmpty(people.postOfficeBox)
                && gviewBll.IsFieldVisible(models, "txtAppPOBox"))
            {
                if (isExistName)
                {
                    buf.Append(ACAConstant.HTML_BR);
                }

                string postOfficeBox = LabelUtil.GetTextByKey("per_appInfoEdit_label_poBox", moduleName);
                buf.Append(postOfficeBox);
                buf.AppendFormat("<span class='contactinfo_postofficebox'>{0}</span>", ScriptFilter.FilterScript(people.postOfficeBox));
                isExistName = true;
            }

            buf.Append("</td></tr></table>");

            return isExistName ? buf.ToString() : string.Empty;
        }

        /// <summary>
        /// Format cap contact for UI display extend information.
        /// </summary>
        /// <param name="capContact">cap contact model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>Html string for contact UI display</returns>
        public static string FormatCapContactModel4Ext(CapContactModel4WS capContact, string moduleName)
        {
            if (capContact == null || capContact.people == null)
            {
                return string.Empty;
            }

            return FormatCapContactModel4Ext(capContact.people, moduleName, capContact.accessLevel);
        }

        /// <summary>
        /// Format cap contact for UI display extend information.
        /// </summary>
        /// <param name="people">cap contact model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="accessLevel">the access level</param>
        /// <returns>Html string for contact UI display</returns>
        public static string FormatCapContactModel4Ext(PeopleModel4WS people, string moduleName, string accessLevel)
        {
            if (people == null)
            {
                return string.Empty;
            }

            GFilterScreenPermissionModel4WS permission =
                ControlBuildHelper.GetPermissionWithGenericTemplate(GviewID.ContactEdit, GViewConstant.PERMISSION_PEOPLE, people.contactType, people.template);
            SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.ContactEdit);
            StringBuilder buf = new StringBuilder();
            buf.Append("<table role='presentation' class='table_child ACA_SmLabel ACA_SmLabel_FontSize'><tr><td>");
            bool isExistName = false;
            string countryCode = people.compactAddress != null
                                     ? people.compactAddress.countryCode
                                     : people.countryCode;
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (!string.IsNullOrEmpty(people.phone1) && gviewBll.IsFieldVisible(models, "txtAppPhone1"))
            {
                string phone1 = FormatPhoneShow(people.phone1CountryCode, people.phone1, countryCode);
                buf.AppendFormat("<span class='contactinfo_phone1'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_phone1"), phone1));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.phone3) && gviewBll.IsFieldVisible(models, "txtAppPhone3"))
            {
                string phone3 = FormatPhoneShow(people.phone3CountryCode, people.phone3, countryCode);
                buf.AppendFormat("<span class='contactinfo_phone3'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_phone3"), phone3));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.phone2) && gviewBll.IsFieldVisible(models, "txtAppPhone2"))
            {
                string phone2 = FormatPhoneShow(people.phone2CountryCode, people.phone2, countryCode);
                buf.AppendFormat("<span class='contactinfo_phone2'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_phone2"), phone2));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.fax) && gviewBll.IsFieldVisible(models, "txtAppFax"))
            {
                string fax = FormatPhoneShow(people.faxCountryCode, people.fax, countryCode);
                buf.AppendFormat("<span class='contactinfo_fax'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_fax"), fax));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.email) && gviewBll.IsFieldVisible(models, "txtAppEmail"))
            {
                string email = ScriptFilter.FilterScript(people.email);
                buf.AppendFormat("<span class='contactinfo_email'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_appInfoEdit_label_email"), I18nStringUtil.FormatToTableRow(email)));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.birthCity) && gviewBll.IsFieldVisible(models, "txtBirthplaceCity"))
            {
                string birthplaceCity = ScriptFilter.FilterScript(people.birthCity);
                buf.AppendFormat("<span class='contactinfo_region'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_birthplace_city"), birthplaceCity));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.birthState) && gviewBll.IsFieldVisible(models, "ddlBirthplaceState"))
            {
                string birthplaceState = ScriptFilter.FilterScript(people.birthState);
                buf.AppendFormat("<span class='contactinfo_region'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_birthplace_state"), birthplaceState));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.birthRegion) && gviewBll.IsFieldVisible(models, "ddlBirthplaceCountry"))
            {
                string birthplaceCountry = ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(people.birthRegion));
                buf.AppendFormat("<span class='contactinfo_region'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_country"), birthplaceCountry));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.race) && gviewBll.IsFieldVisible(models, "ddlRace"))
            {
                string race = ScriptFilter.FilterScript(people.race);
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_race"), race));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.deceasedDate) && gviewBll.IsFieldVisible(models, "txtDeceasedDate"))
            {
                string deceasedDate = ScriptFilter.FilterScript(I18nDateTimeUtil.FormatToDateStringForUI(people.deceasedDate));
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_deceased_date"), deceasedDate));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.passportNumber) && gviewBll.IsFieldVisible(models, "txtPassportNumber"))
            {
                string passportNumber = ScriptFilter.FilterScript(people.passportNumber);
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_passport_number"), passportNumber));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.driverLicenseNbr) && gviewBll.IsFieldVisible(models, "txtDriverLicenseNumber"))
            {
                string driverLicenseNbr = ScriptFilter.FilterScript(people.driverLicenseNbr);
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_driver_license_number"), driverLicenseNbr));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.driverLicenseState) && gviewBll.IsFieldVisible(models, "ddlDriverLicenseState"))
            {
                string driverLicenseState = ScriptFilter.FilterScript(people.driverLicenseState);
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_driver_license_state"), driverLicenseState));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.stateIDNbr) && gviewBll.IsFieldVisible(models, "txtStateNumber"))
            {
                string stateIdNbr = ScriptFilter.FilterScript(people.stateIDNbr);
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_state_id_number"), stateIdNbr));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(people.preferredChannel) && gviewBll.IsFieldVisible(models, "ddlPreferredChannel"))
            {
                string preferredChannel = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CONTACT_PREFERRED_CHANNEL, people.preferredChannel);
                preferredChannel = ScriptFilter.FilterScript(preferredChannel);

                buf.AppendFormat("<span class='contactinfo_preferredchannel'>{0}</span>", I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("aca_contactedit_label_preferredchannel"), preferredChannel));
                isExistName = true;
            }

            if (!string.IsNullOrEmpty(accessLevel) && gviewBll.IsFieldVisible(models, "radioListContactPermission"))
            {
                string accessText = ScriptFilter.FilterScript(DropDownListBindUtil.GetContactPermissionTextByValue(accessLevel, moduleName), false);
                buf.Append(I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("contact_permission_viewname", moduleName), accessText));
                isExistName = true;
            }

            buf.Append("</td></tr></table>");

            return isExistName ? buf.ToString() : string.Empty;
        }

        /// <summary>
        /// Display full name and address in cap detail page.
        /// </summary>
        /// <param name="owner">Owner model come from spare form.</param>
        /// <param name="moduleName">current module name</param>
        /// <param name="lines">the account of display lines</param>
        /// <returns>owner detail information display in cap detail</returns>
        public static string FormatOwner4Detail(RefOwnerModel owner, string moduleName, out int lines)
        {
            int line = 0;
            StringBuilder buf = new StringBuilder();

            if (owner != null)
            {
                buf.Append("<table role='presentation' style='TEMPLATE_STYLE' class='table_child'>");
                buf.Append("<tr><td class='td_child_left font12px'>");

                StringBuilder contant = new StringBuilder();
                GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                {
                    permissionLevel = GViewConstant.PERMISSION_APO,
                    permissionValue = GViewConstant.SECTION_OWNER
                };
                SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.OwnerEdit);
                contant.Append("<table role='presentation' border='0' cellpadding='0' cellspacing='0'>");

                StringBuilder tmpContant1 = new StringBuilder();
                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

                if (!string.IsNullOrEmpty(owner.ownerTitle) && gviewBll.IsFieldVisible(models, "txtTitle"))
                {
                    tmpContant1.Append(ScriptFilter.FilterScript(owner.ownerTitle) + ACAConstant.BLANK);
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.ownerFullName) && gviewBll.IsFieldVisible(models, "txtName"))
                {
                    tmpContant1.Append(ScriptFilter.FilterScript(owner.ownerFullName));
                    line++;
                }

                if (!string.IsNullOrEmpty(tmpContant1.ToString()))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    contant.Append(tmpContant1);
                    contant.Append("</td></tr>");
                }

                if (!string.IsNullOrEmpty(owner.mailAddress1) && gviewBll.IsFieldVisible(models, "txtAddress1"))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    contant.Append(ScriptFilter.FilterScript(owner.mailAddress1));
                    contant.Append("</td></tr>");
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.mailAddress2) && gviewBll.IsFieldVisible(models, "txtAddress2"))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    contant.Append(ScriptFilter.FilterScript(owner.mailAddress2));
                    contant.Append("</td></tr>");
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.mailAddress3) && gviewBll.IsFieldVisible(models, "txtAddress3"))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    contant.Append(ScriptFilter.FilterScript(owner.mailAddress3));
                    contant.Append("</td></tr>");
                    line++;
                }

                StringBuilder tmpContant2 = new StringBuilder();

                if (!string.IsNullOrEmpty(owner.mailCity) && gviewBll.IsFieldVisible(models, "txtCity"))
                {
                    tmpContant2.Append(ScriptFilter.FilterScript(owner.mailCity) + ACAConstant.BLANK);
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.mailState) && gviewBll.IsFieldVisible(models, "ddlAppState"))
                {
                    tmpContant2.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(owner.mailState, owner.mailCountry)) + ACAConstant.BLANK);
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.mailZip) && gviewBll.IsFieldVisible(models, "txtZip"))
                {
                    tmpContant2.Append(FormatZipShow(owner.mailZip, owner.mailCountry) + ACAConstant.BLANK);
                    line++;
                }

                if (!string.IsNullOrEmpty(tmpContant2.ToString()))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    contant.Append(tmpContant2);
                    contant.Append("</td></tr>");
                }

                if (!string.IsNullOrEmpty(owner.mailCountry) && gviewBll.IsFieldVisible(models, "ddlCountry"))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    contant.Append(ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(owner.mailCountry)));
                    contant.Append("</td></tr>");
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.phone) &&
                       gviewBll.IsFieldVisible(models, "txtPhone"))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    string phone = ModelUIFormat.FormatPhoneShow(owner.phoneCountryCode, owner.phone, owner.mailCountry);
                    contant.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_owneredit_phone"), phone));
                    contant.Append("</td></tr>");
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.fax) &&
                    gviewBll.IsFieldVisible(models, "txtFax"))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    string fax = ModelUIFormat.FormatPhoneShow(owner.faxCountryCode, owner.fax, owner.mailCountry);
                    contant.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_owneredit_fax"), fax));
                    contant.Append("</td></tr>");
                    line++;
                }

                if (!string.IsNullOrEmpty(owner.email) && gviewBll.IsFieldVisible(models, "txtEmail"))
                {
                    contant.Append("<tr><td style='vertical-align:top'>");
                    contant.Append(ScriptFilter.FilterScript(owner.email));
                    contant.Append("</td></tr>");
                    line++;
                }

                contant.Append("</table>");
                buf.Append("</td><td>");
                buf.Append(contant);
                buf.Append("</td></tr>");
                buf.Append("</table>");
                lines = line;
                return buf.ToString();
            }
            else
            {
                lines = line;
                return string.Empty;
            }
        }

        /// <summary>
        /// Format contact address context show.
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="capContact">cap contact model</param>
        /// <returns>Contact address data with html format for display in record detail page.</returns>
        public static string FormatContactAddressSection(string moduleName, CapContactModel4WS capContact)
        {
            //contact address section
            StringBuilder contant = new StringBuilder();

            GFilterScreenPermissionModel4WS contactAddressPermission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_CONTACT_ADDRESS
            };

            SimpleViewElementModel4WS[] fields = GetSimpleViewElementModelBySectionID(moduleName, contactAddressPermission, GviewID.ContactAddress);

            if (capContact.people.contactAddressList != null && capContact.people.contactAddressList.Length > 0)
            {
                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

                foreach (ContactAddressModel contactAddress in capContact.people.contactAddressList)
                {
                    if (ACAConstant.INVALID_STATUS.Equals(contactAddress.auditModel.auditStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    contant.Append("<div class='ACA_TabRow ACA_FLeft'>");
                    List<string> stringContent = new List<string>();

                    if (gviewBll.IsFieldVisible(fields, "ddlAddressType") && !string.IsNullOrEmpty(contactAddress.addressType))
                    {
                        stringContent.Add(ScriptFilter.FilterScript(StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE, contactAddress.addressType)));
                    }

                    if (gviewBll.IsFieldVisible(fields, "txtAddressLine1") && !string.IsNullOrEmpty(contactAddress.addressLine1))
                    {
                        stringContent.Add(ScriptFilter.FilterScript(contactAddress.addressLine1));
                    }

                    // Construct city state zip with blank
                    List<string> stringTemp = new List<string>();

                    if (gviewBll.IsFieldVisible(fields, "txtCity") && !string.IsNullOrEmpty(contactAddress.city))
                    {
                        stringTemp.Add(ScriptFilter.FilterScript(contactAddress.city));
                    }

                    if (gviewBll.IsFieldVisible(fields, "txtState") && !string.IsNullOrEmpty(contactAddress.state))
                    {
                        string i18NState = I18nUtil.DisplayStateForI18N(contactAddress.state, contactAddress.countryCode);
                        stringTemp.Add(ScriptFilter.FilterScript(i18NState));
                    }

                    if (gviewBll.IsFieldVisible(fields, "txtZip") && !string.IsNullOrEmpty(contactAddress.zip))
                    {
                        stringTemp.Add(ScriptFilter.FilterScript(FormatZipShow(contactAddress.zip, contactAddress.countryCode)));
                    }

                    if (stringTemp.Count > 0)
                    {
                        stringContent.Add(DataUtil.ConcatStringWithSplitChar(stringTemp.ToArray(), ACAConstant.COMMA_BLANK));
                    }
                    
                    if (gviewBll.IsFieldVisible(fields, "ddlCountry") && !string.IsNullOrEmpty(contactAddress.countryCode))
                    {
                        stringContent.Add(StandardChoiceUtil.GetCountryByKey(contactAddress.countryCode));
                    }

                    contant.Append(DataUtil.ConcatStringWithSplitChar(stringContent.ToArray(), ACAConstant.HTML_BR));
                    contant.Append("</div>");
                }
            }

            return contant.ToString();
        }

        /// <summary>
        /// Format Phone country code to show (+086)
        /// </summary>
        /// <param name="countrycode">the phone country code need to format</param>
        /// <returns>Formatted phone country code</returns>
        public static string FormatPhoneCountryCodeShow(string countrycode)
        {
            string result = string.Empty;

            countrycode = ScriptFilter.FilterScript(countrycode);

            if (!string.IsNullOrEmpty(countrycode) &&
                StandardChoiceUtil.IsCountryCodeEnabled())
            {
                result = "(+" + countrycode + ")";
            }

            return result;
        }

        /// <summary>
        /// convert input data format to "$x.xx"
        /// </summary>
        /// <param name="jobValueInvariantString">By formatted parameter</param>
        /// <returns>Formatted job value</returns>
        public static string FormatDataForJobValue(string jobValueInvariantString)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(jobValueInvariantString))
            {
                double jobValue = I18nNumberUtil.ParseMoneyFromWebService(jobValueInvariantString);
                double maxOpenValue = Math.Pow(10, 12); //the max number should be 12 digit.

                if (jobValue >= 0 && jobValue < maxOpenValue)
                {
                    result = jobValue.ToString(I18nNumberUtil.DEFAULT_MONEY_FORMAT);
                }
            }

            return result;
        }

        /// <summary>
        /// Get the formatted value for "Licensed Professional"
        /// </summary>
        /// <param name="licenses">LicenseProfessionalModel array</param>
        /// <param name="moduleName">module name</param>
        /// <param name="lines">line number</param>
        /// <param name="isDisplayTempleteField">true to display template field</param>
        /// <returns>formatted value for licensed professional</returns>
        public static string FormatLicenseModel4Basic(LicenseProfessionalModel[] licenses, string moduleName, out int lines, bool isDisplayTempleteField)
        {
            int line = 0;
            StringBuilder breakline = new StringBuilder();
            StringBuilder buf = new StringBuilder();
            if (licenses == null ||
                licenses.Length == 0)
            {
                lines = line;
                return string.Empty;
            }

            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
            buf.Append("<table role='presentation' id='tbl_licensedps' style='TEMPLATE_STYLE' class='table_child'>");
            
            int index = -1;

            for (int i = 0; i < licenses.Length; i++)
            {
                bool showIndex = true;
                string trStyle = string.Empty;
                string strIndex = string.Empty;
                StringBuilder contant = new StringBuilder();
                LicenseProfessionalModel licenseProfessional = licenses[i];

                if (licenseProfessional.licenseNbr == null)
                {
                    continue;
                }
                else
                {
                    index++;
                }

                if (index == 1)
                {
                    string strAdditionalLPs = LabelUtil.GetTextByKey("aca_permitDetail_label_showAdditionalLPs", moduleName);
                    line++;
                    buf.Append("<tr><td class='td_child_left'></td><td>&nbsp;</td></tr>");
                    buf.Append("<tr><td class='td_child_left'></td><td><strong>");
                    buf.Append("<a id='link_licenseProfessional' href='javascript:void(0)' onclick=\"DisplayAdditionInfo('tr_licenseProfessional',this,'tbl_licensedps');\" class=\"NotShowLoading\">");
                    buf.Append(strAdditionalLPs);
                    buf.Append("</a>");
                    buf.Append("</strong></td></tr>");
                    line++;
                }

                if (index > 0)
                {
                    trStyle = " tips='tr_licenseProfessional' style='display:none;'";
                }

                buf.Append("<tr " + trStyle + "><td class='td_child_left'>");

                bool isExistName = false;
                GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                                                             {
                                                                 permissionLevel = GViewConstant.PERMISSION_PEOPLE,
                                                                 permissionValue = licenses[i].licenseType
                                                             };
                  
                SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.LicenseEdit);

                if (!I18nCultureUtil.IsChineseCultureEnabled)
                {
                    if (!string.IsNullOrEmpty(licenseProfessional.salutation) && gviewBll.IsFieldVisible(models, "ddlSalutation"))
                    {
                        string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, licenseProfessional.salutation);

                        if (index > 0 && showIndex == true)
                        {
                            strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                            showIndex = false;
                        }

                        isExistName = true;
                        contant.Append(salutation + " ");
                        breakline.Append(salutation + " ");
                    }
                }

                if (!string.IsNullOrEmpty(licenseProfessional.contactFirstName) && gviewBll.IsFieldVisible(models, "txtFirstName"))
                {
                    if (index > 0 && showIndex == true)
                    {
                        strIndex = index.ToString() + ACAConstant.BRACKET;
                        showIndex = false;
                    }

                    isExistName = true;
                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.contactFirstName) + " ");
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.contactFirstName) + " ");
                }

                if (!string.IsNullOrEmpty(licenseProfessional.contactMiddleName) && gviewBll.IsFieldVisible(models, "txtMiddleName"))
                {
                    if (index > 0 && showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    isExistName = true;
                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.contactMiddleName) + " ");
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.contactMiddleName) + " ");
                }

                if (!string.IsNullOrEmpty(licenseProfessional.contactLastName) && gviewBll.IsFieldVisible(models, "txtLastName"))
                {
                    if (index > 0 && showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    isExistName = true;
                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.contactLastName) + " ");
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.contactLastName) + " ");
                }

                if (!string.IsNullOrEmpty(licenseProfessional.email) && gviewBll.IsFieldVisible(models, "txtEmail"))
                {
                    if (index > 0 && showIndex == true)
                    {
                        strIndex = index.ToString() + ACAConstant.BRACKET;
                        showIndex = false;
                    }

                    isExistName = true;
                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.email) + " ");
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.email) + " ");
                }

                if (!string.IsNullOrEmpty(licenseProfessional.suffixName) && gviewBll.IsFieldVisible(models, "txtSuffix"))
                {
                    if (index > 0 && showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    isExistName = true;
                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.suffixName));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.suffixName));
                }

                if (I18nCultureUtil.IsChineseCultureEnabled)
                {
                    if (!string.IsNullOrEmpty(licenseProfessional.salutation) && gviewBll.IsFieldVisible(models, "ddlSalutation"))
                    {
                        string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, licenseProfessional.salutation);

                        if (index > 0 && showIndex == true)
                        {
                            strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                            showIndex = false;
                        }

                        isExistName = true;
                        contant.Append(" " + salutation);
                        breakline.Append(" " + salutation);
                    }
                }

                if (isExistName)
                {
                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (licenseProfessional.birthDate != null && gviewBll.IsFieldVisible(models, "txtBirthDate"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    string bd = "<span dir='LTR'>" + I18nDateTimeUtil.FormatToDateStringForUI(licenseProfessional.birthDate.Value) + "</span> ";
                    contant.Append(bd);
                    breakline.Append(bd);

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.gender) && gviewBll.IsFieldVisible(models, "radioListGender"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    string gender = StandardChoiceUtil.GetGenderByKey(licenseProfessional.gender);
                    contant.Append(gender);
                    breakline.Append(gender);

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.businessName) &&
                    gviewBll.IsFieldVisible(models, "txtBusName"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.businessName));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.businessName));

                    if (!string.IsNullOrEmpty(licenseProfessional.busName2) &&
                        gviewBll.IsFieldVisible(models, "txtBusName2"))
                    {
                        contant.Append(ACAConstant.SLASH + ScriptFilter.FilterScript(licenseProfessional.busName2));
                        breakline.Append(ACAConstant.SLASH + ScriptFilter.FilterScript(licenseProfessional.busName2));
                    }

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.businessLicense) && gviewBll.IsFieldVisible(models, "txtBusLicense"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.businessLicense));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.businessLicense));

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.address1) && gviewBll.IsFieldVisible(models, "txtAddress1"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.address1));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.address1));

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.address2) &&
                    gviewBll.IsFieldVisible(models, "txtAddress2"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.address2));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.address2));

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.address3) &&
                    gviewBll.IsFieldVisible(models, "txtAddress3"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.address3));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.address3));

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.city) && gviewBll.IsFieldVisible(models, "txtCity"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.city));
                    contant.Append(ACAConstant.COMMA_BLANK);
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.city) + ACAConstant.COMMA_BLANK);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.state) && gviewBll.IsFieldVisible(models, "txtState"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    string i18NState = I18nUtil.DisplayStateForI18N(licenseProfessional.state, licenseProfessional.countryCode);
                    contant.Append(ScriptFilter.FilterScript(i18NState));
                    contant.Append(ACAConstant.COMMA_BLANK);
                    breakline.Append(ScriptFilter.FilterScript(i18NState) + ACAConstant.COMMA_BLANK);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.zip) && gviewBll.IsFieldVisible(models, "txtZipCode"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(FormatZipShow(licenseProfessional.zip, licenseProfessional.countryCode));
                    breakline.Append(FormatZipShow(licenseProfessional.zip, licenseProfessional.countryCode));

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.countryCode) && gviewBll.IsFieldVisible(models, "ddlCountry"))
                {
                    string country = StandardChoiceUtil.GetCountryByKey(licenseProfessional.countryCode);

                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(country);
                    contant.Append(" ");
                    breakline.Append(country + " ");

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.postOfficeBox) && gviewBll.IsFieldVisible(models, "txtPOBox"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.postOfficeBox));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.postOfficeBox));

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                LicenseModel4WS licenseModel = new LicenseModel4WS();

                // Get Reference License Model based on License Type & Number to display license state
                if (!string.IsNullOrEmpty(licenseProfessional.licenseNbr) &&
                   !string.IsNullOrEmpty(licenseProfessional.licenseType))
                {
                    LicenseModel4WS license = new LicenseModel4WS();

                    license.serviceProviderCode = ConfigManager.AgencyCode;
                    license.licenseType = licenseProfessional.licenseType;
                    license.stateLicense = licenseProfessional.licenseNbr;

                    ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                    licenseModel = licenseBll.GetLicenseByStateLicNbr(license);
                }

                if (gviewBll.IsFieldVisible(models, "txtHomePhone"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();

                    breakline.Append(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_homePhone"));

                    if (!string.IsNullOrEmpty(licenseProfessional.phone1))
                    {
                        string phone1 = ModelUIFormat.FormatPhoneShow(licenseProfessional.phone1CountryCode, licenseProfessional.phone1, licenseProfessional.countryCode);
                        contant.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_homePhone"), phone1));
                        breakline.Append(ScriptFilter.FilterScript(licenseProfessional.phone1));
                    }
                }

                if (gviewBll.IsFieldVisible(models, "txtMobilePhone"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    breakline.Append(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_mobile"));

                    if (!string.IsNullOrEmpty(licenseProfessional.phone2))
                    {
                        string phone2 = ModelUIFormat.FormatPhoneShow(licenseProfessional.phone2CountryCode, licenseProfessional.phone2, licenseProfessional.countryCode);
                        contant.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_mobile"), phone2));
                        breakline.Append(ScriptFilter.FilterScript(licenseProfessional.phone2));
                    }
                }

                if (gviewBll.IsFieldVisible(models, "txtFax"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();

                    breakline.Append(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_fax"));

                    if (!string.IsNullOrEmpty(licenseProfessional.fax))
                    {
                        string fax = ModelUIFormat.FormatPhoneShow(licenseProfessional.faxCountryCode, licenseProfessional.fax, licenseProfessional.countryCode);
                        contant.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_fax"), fax));
                        breakline.Append(ScriptFilter.FilterScript(licenseProfessional.fax));
                    }
                }

                if (!string.IsNullOrEmpty(licenseProfessional.resLicenseType) && gviewBll.IsFieldVisible(models, "ddlLicenseType"))
                {
                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();

                    if (licenseModel != null && !string.IsNullOrEmpty(licenseModel.licState) && StandardChoiceUtil.IsDisplayLicenseState())
                    {
                        contant.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)));
                        contant.Append(" ");
                        breakline.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)) + " ");
                    }

                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.resLicenseType));
                    contant.Append(" ");
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.resLicenseType) + " ");
                }

                if (!string.IsNullOrEmpty(licenseProfessional.licenseNbr) && gviewBll.IsFieldVisible(models, "txtLicenseNum"))
                {
                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();

                    if (licenseModel != null && !string.IsNullOrEmpty(licenseModel.licState) && StandardChoiceUtil.IsDisplayLicenseState())
                    {
                        contant.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)));
                        contant.Append("-");
                        breakline.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)) + "-");
                    }

                    contant.Append(" ");
                    contant.Append(ScriptFilter.FilterScript(licenseProfessional.licenseNbr));
                    breakline.Append(ScriptFilter.FilterScript(licenseProfessional.licenseNbr));

                    breakline = new StringBuilder();
                    contant.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.typeFlag) && gviewBll.IsFieldVisible(models, "ddlContactType"))
                {
                    if (index > 0 && showIndex)
                    {
                        strIndex = string.Format("{0}{1}", index, ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    string strTypeFlag = ScriptFilter.FilterScript(DropDownListBindUtil.GetTypeFlagTextByValue(licenseProfessional.typeFlag));

                    contant.Append(strTypeFlag);
                    breakline.Append(strTypeFlag);
                }

                if (gviewBll.IsFieldVisible(models, "txtContractorLicNO"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    breakline.Append(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorlicno"));

                    if (!string.IsNullOrEmpty(licenseProfessional.contrLicNo))
                    {
                        contant.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorlicno"), licenseProfessional.contrLicNo));
                        breakline.Append(ScriptFilter.FilterScript(licenseProfessional.contrLicNo));
                    }
                }

                if (gviewBll.IsFieldVisible(models, "txtContractorBusiName"))
                {
                    if (index > 0 &&
                        showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    if (index == 0)
                    {
                        line += BreakWords(breakline.ToString());
                    }

                    breakline = new StringBuilder();
                    breakline.Append(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorbusiname"));

                    if (!string.IsNullOrEmpty(licenseProfessional.contLicBusName))
                    {
                        contant.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorbusiname"), licenseProfessional.contLicBusName));
                        breakline.Append(ScriptFilter.FilterScript(licenseProfessional.contLicBusName));
                    }
                }

                if (isDisplayTempleteField)
                {
                    TemplateAttributeModel[] attributes = licenseProfessional.templateAttributes;
                    if (attributes != null &&
                        attributes.Length > 0)
                    {
                        foreach (TemplateAttributeModel item in attributes)
                        {
                            if (!string.IsNullOrEmpty(item.attributeValue))
                            {
                                if (index == 0)
                                {
                                    contant.Append("<span style='font-size:11px'>");
                                }
                                else
                                {
                                    contant.Append("<span style='font-size:10px'>");
                                }

                                string valueText = ModelUIFormat.GetTemplateValue4Display(item);
                                contant.Append(" " + ScriptFilter.FilterScript(valueText));
                                breakline.Append(" " + ScriptFilter.FilterScript(valueText));
                                contant.Append("</span>");
                            }
                        }

                        if (index == 0)
                        {
                            line += BreakWords(breakline.ToString());
                        }
                    }
                }

                buf.Append(strIndex);
                buf.Append("</td><td>");
                buf.Append(contant);
                buf.Append("</td></tr>");
            } //end for

            buf.Append("</table>");
            lines = line;
            return buf.ToString();
        }

        /// <summary>
        /// Format license professional for UI display basic information.
        /// </summary>
        /// <param name="licenseProfessional">license professional model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>Html string for license professional UI display</returns>
        public static string FormatLicenseProfessionalModel4Basic(LicenseProfessionalModel licenseProfessional, string moduleName)
        {
            if (licenseProfessional != null)
            {
                GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                                                             {
                                                                 permissionLevel = GViewConstant.PERMISSION_PEOPLE,
                                                                 permissionValue = licenseProfessional.licenseType
                                                             };

                SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.LicenseEdit);

                StringBuilder buf = new StringBuilder();

                if (!string.IsNullOrEmpty(licenseProfessional.salutation))
                {
                    string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, licenseProfessional.salutation);
                    buf.Append(salutation);
                    buf.Append(" ");

                    if (string.IsNullOrEmpty(licenseProfessional.contactFirstName))
                    {
                        buf.Append(ACAConstant.HTML_BR);
                    }
                }

                if (!string.IsNullOrEmpty(licenseProfessional.contactFirstName))
                {
                    buf.Append("<strong>");
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.contactFirstName));
                    buf.Append(" ");
                    buf.Append("</strong>");

                    if (string.IsNullOrEmpty(licenseProfessional.contactLastName))
                    {
                        buf.Append(ACAConstant.HTML_BR);
                    }
                }

                if (!string.IsNullOrEmpty(licenseProfessional.contactLastName))
                {
                    buf.Append("<strong>");
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.contactLastName));
                    buf.Append("</strong>");
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (licenseProfessional.birthDate != null)
                {
                    buf.Append(I18nDateTimeUtil.FormatToDateStringForUI(licenseProfessional.birthDate.Value));
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.gender))
                {
                    string gender = StandardChoiceUtil.GetGenderByKey(licenseProfessional.gender);
                    buf.Append(gender);
                    buf.Append(ACAConstant.HTML_BR);
                }

                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

                if (!string.IsNullOrEmpty(licenseProfessional.businessName) &&
                    gviewBll.IsFieldVisible(models, "txtBusName"))
                {
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.businessName));
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.address1))
                {
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.address1));
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.address2))
                {
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.address2));
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.address3))
                {
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.address3));
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.city))
                {
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.city));
                    buf.Append(", ");
                }

                // State (here display resState, not State)
                if (!string.IsNullOrEmpty(licenseProfessional.resState))
                {
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.resState));
                    buf.Append(", ");
                }

                if (!string.IsNullOrEmpty(licenseProfessional.zip))
                {
                    buf.Append(FormatZipShow(licenseProfessional.zip, licenseProfessional.countryCode));
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.countryCode))
                {
                    string country = StandardChoiceUtil.GetCountryByKey(licenseProfessional.countryCode);
                    buf.Append(country);
                    buf.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licenseProfessional.postOfficeBox))
                {
                    buf.Append(ScriptFilter.FilterScript(licenseProfessional.postOfficeBox));
                }

                return buf.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format license professional for UI display extend information.
        /// </summary>
        /// <param name="licenseProfessionalModel">license professional model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>license information extend</returns>
        public static string FormatLicenseProfessionalModel4Ext(LicenseProfessionalModel licenseProfessionalModel, string moduleName)
        {
            if (licenseProfessionalModel == null)
            {
                return string.Empty;
            }

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                                                         {
                                                             permissionLevel = GViewConstant.PERMISSION_PEOPLE,
                                                             permissionValue = licenseProfessionalModel.licenseType
                                                         };

            SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.LicenseEdit);

            LicenseModel4WS licenseModel = null;

            // Get Reference License Model based on License Type & Number to display license state
            if (!string.IsNullOrEmpty(licenseProfessionalModel.licenseNbr) ||
               !string.IsNullOrEmpty(licenseProfessionalModel.licenseType))
            {
                LicenseModel4WS license = new LicenseModel4WS();
                license.serviceProviderCode = ConfigManager.AgencyCode;
                license.licenseType = licenseProfessionalModel.licenseType;
                license.stateLicense = licenseProfessionalModel.licenseNbr;

                ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                licenseModel = licenseBll.GetLicenseByStateLicNbr(license);
            }

            StringBuilder buf = new StringBuilder();
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));

            if (gviewBll.IsFieldVisible(models, "txtHomePhone"))
            {
                buf.Append(ACAConstant.HTML_BR);

                string phone1Temp = string.Empty;

                if (!string.IsNullOrEmpty(licenseProfessionalModel.phone1))
                {
                    phone1Temp = ModelUIFormat.FormatPhoneShow(licenseProfessionalModel.phone1CountryCode, licenseProfessionalModel.phone1, licenseProfessionalModel.countryCode);
                }

                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_homePhone"), phone1Temp));
            }

            if (gviewBll.IsFieldVisible(models, "txtMobilePhone"))
            {
                buf.Append(ACAConstant.HTML_BR);

                string phone1Temp = string.Empty;

                if (!string.IsNullOrEmpty(licenseProfessionalModel.phone2))
                {
                    phone1Temp = ModelUIFormat.FormatPhoneShow(licenseProfessionalModel.phone2CountryCode, licenseProfessionalModel.phone2, licenseProfessionalModel.countryCode);
                }

                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_mobile"), phone1Temp));
            }

            if (gviewBll.IsFieldVisible(models, "txtFax"))
            {
                buf.Append(ACAConstant.HTML_BR);

                string faxTemp = string.Empty;

                if (!string.IsNullOrEmpty(licenseProfessionalModel.fax))
                {
                    faxTemp = ModelUIFormat.FormatPhoneShow(licenseProfessionalModel.faxCountryCode, licenseProfessionalModel.fax, licenseProfessionalModel.countryCode);
                }

                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_fax"), faxTemp));
            }

            if (!string.IsNullOrEmpty(licenseProfessionalModel.licenseType))
            {
                buf.Append(ACAConstant.HTML_BR);

                if (licenseModel != null)
                {
                    buf.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)));
                    buf.Append("&nbsp;");
                }

                buf.Append(ScriptFilter.FilterScript(licenseProfessionalModel.licenseType));
            }

            if (!string.IsNullOrEmpty(licenseProfessionalModel.licenseNbr))
            {
                buf.Append(ACAConstant.HTML_BR);

                if (licenseModel != null)
                {
                    buf.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)));
                    buf.Append("-");
                }

                buf.Append(ScriptFilter.FilterScript(licenseProfessionalModel.licenseNbr));
            }

            if (gviewBll.IsFieldVisible(models, "txtContractorLicNO"))
            {
                buf.Append(ACAConstant.HTML_BR);

                string contrLicNo = string.Empty;

                if (!string.IsNullOrEmpty(licenseProfessionalModel.contrLicNo))
                {
                    contrLicNo = licenseProfessionalModel.contrLicNo;
                }

                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorlicno"), contrLicNo));
            }

            if (gviewBll.IsFieldVisible(models, "txtContractorBusiName"))
            {
                buf.Append(ACAConstant.HTML_BR);

                string contLicBusName = string.Empty;

                if (!string.IsNullOrEmpty(licenseProfessionalModel.contLicBusName))
                {
                    contLicBusName = licenseProfessionalModel.contLicBusName;
                }

                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorbusiname"), contLicBusName));
            }

            return buf.ToString();
        }

        /// <summary>
        /// Add mask "-" into phone number textbox in edit page.
        /// </summary>
        /// <param name="phone">the phone need to format</param>
        /// <param name="country">The country.</param>
        /// <returns>Formatted phone</returns>
        public static string FormatPhone4EditPage(string phone, string country)
        {
            phone = ScriptFilter.FilterScript(phone);

            string mask = string.Empty;
            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();

            //if entity country is empty, will not the default regional setting.
            RegionalModel regionalModel = string.IsNullOrEmpty(country)
                                              ? null
                                              : regionalBll.GetRegionalModelByCountry(country, out states);

            if (regionalModel != null)
            {
                mask = regionalModel.phoneNumMask;
            }

            return I18nPhoneUtil.FormatPhoneByMask(mask, phone);
        }

        /// <summary>
        /// Format the phone to show
        /// </summary>
        /// <param name="countryCode">country code</param>
        /// <param name="phone">phone number</param>
        /// <param name="country">The country.</param>
        /// <returns>Format phone form left to right.</returns>
        public static string FormatPhoneShow(string countryCode, string phone, string country)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return string.Empty;
            }

            countryCode = ScriptFilter.FilterScript(countryCode);
            phone = ScriptFilter.FilterScript(phone);

            StringBuilder buf = new StringBuilder();
            buf.Append(@"<div class=""ACA_PhoneNumberLTR"">");

            buf.Append(string.IsNullOrEmpty(countryCode) ? string.Empty : FormatPhoneCountryCodeShow(countryCode));
            buf.Append(string.IsNullOrEmpty(phone) ? string.Empty : FormatPhone4EditPage(phone, country));
            buf.Append("</div>");

            return buf.ToString();
        }

        /// <summary>
        /// Get formatted UI string to display project description
        /// </summary>
        /// <param name="capModel">CapModel4WS object</param>
        /// <param name="lines">line number</param>
        /// <param name="isDisplay">true if need to display</param>
        /// <param name="moduleName">The module name</param>
        /// <returns>Html string for project description display</returns>
        public static string FormatProjectDesc4Desplay(CapModel4WS capModel, out int lines, out bool isDisplay, string moduleName)
        {
            int line = 0;
            StringBuilder buf = new StringBuilder();
            StringBuilder contant = new StringBuilder();
            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_DETAIL
            };
            SimpleViewElementModel4WS[] models = GetSimpleViewElementModelBySectionID(moduleName, permission, GviewID.DetailInformation);

            isDisplay = false;

            buf.Append("<table role='presentation' style='TEMPLATE_STYLE' class='table_child'><tr><td class='td_child_left font12px'>");
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (!string.IsNullOrEmpty(capModel.specialText) && gviewBll.IsFieldVisible(models, "txtApplicationName"))
            {
                contant.Append(ScriptFilter.FilterScript(capModel.specialText));
                contant.Append(ACAConstant.HTML_BR);
                line++;
                isDisplay = true;
            }

            if (capModel.capDetailModel != null &&
                !string.IsNullOrEmpty(capModel.capDetailModel.shortNotes) && gviewBll.IsFieldVisible(models, "txtDescriptionGeneral"))
            {
                if (!string.IsNullOrEmpty(capModel.capDetailModel.shortNotes))
                {
                    contant.Append(ScriptFilter.FilterScript(capModel.capDetailModel.shortNotes));
                    contant.Append(ACAConstant.HTML_BR);
                    line += 1;
                    isDisplay = true;
                }
            }

            if (capModel.capWorkDesModel != null)
            {
                if (!string.IsNullOrEmpty(capModel.capWorkDesModel.description) && gviewBll.IsFieldVisible(models, "txtDescriptionDetail"))
                {
                    contant.Append(ScriptFilter.FilterScript(capModel.capWorkDesModel.description));
                    line += 1;
                    isDisplay = true;
                }
            }

            buf.Append("</td><td>");
            buf.Append(contant);
            buf.Append("</td></tr>");
            buf.Append("</table>");
            lines = line;
            return buf.ToString();
        }

        /// <summary>
        /// Format Trade Name for UI display Basic information.
        /// </summary>
        /// <param name="licenseModel">license professional model</param>
        /// <param name="capModel">CapModel4WS object</param>
        /// <returns>license information extend</returns>
        public static string FormatTradeName4Basic(LicenseModel4WS licenseModel, CapModel4WS capModel)
        {
            if (licenseModel == null)
            {
                return string.Empty;
            }

            StringBuilder buf = new StringBuilder();

            buf.Append("<table role='presentation' style='TEMPLATE_STYLE' class='table_child'><tr><td class='td_child_left font12px'></td><td>");
            bool isExist = false;

            if (!string.IsNullOrEmpty(licenseModel.stateLicense))
            {
                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_tradeNameDetail_label_detailTradeNameNumber"), ScriptFilter.FilterScript(licenseModel.stateLicense)));
                isExist = true;
            }

            if (!string.IsNullOrEmpty(licenseModel.busName2))
            {
                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_tradeNameDetail_label_detailArabicTradeName"), ScriptFilter.FilterScript(licenseModel.busName2)));
                isExist = true;
            }

            if (!string.IsNullOrEmpty(licenseModel.businessName))
            {
                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_tradeNameDetail_label_detailEnglishTradeName"), ScriptFilter.FilterScript(licenseModel.businessName)));
                isExist = true;
            }

            if (!string.IsNullOrEmpty(licenseModel.licenseExpirationDate))
            {
                buf.AppendFormat(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_tradeNameDetail_label_detailDateOfExpire"), I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(licenseModel.licenseExpirationDate)));
                isExist = true;
            }

            if (capModel != null && capModel.licenseProfessionalModel != null && capModel.licenseProfessionalModel.attributes != null &&
                capModel.licenseProfessionalModel.attributes.Length > 0)
            {
                TemplateAttributeModel[] attributes = capModel.licenseProfessionalModel.attributes;

                //add all attributes
                foreach (TemplateAttributeModel tmp in attributes)
                {
                    if (!string.IsNullOrEmpty(tmp.attributeName) &&
                        !string.IsNullOrEmpty(tmp.attributeValue))
                    {
                        string valueText = ModelUIFormat.GetTemplateValue4Display(tmp);
                        buf.AppendFormat(I18nStringUtil.FormatToTableRow(ScriptFilter.FilterScript(I18nStringUtil.GetString(tmp.attributeLabel, tmp.attributeName)) + ":", ScriptFilter.FilterScript(valueText)));
                        isExist = true;
                    }
                }
            }

            buf.Append("</td></tr></table>");

            if (isExist)
            {
                return buf.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Format the zip to show
        /// </summary>
        /// <param name="zip">the zip need to format</param>
        /// <param name="country">The country.</param>
        /// <returns>Formatted zip</returns>
        public static string FormatZipShow(string zip, string country)
        {
            return FormatZipShow(zip, country, true);
        }

        /// <summary>
        /// Formats the zip show.
        /// </summary>
        /// <param name="zip">The zip string.</param>
        /// <param name="country">The country.</param>
        /// <param name="needEncode">Indicate whether the text needs to be encoded or not</param>
        /// <returns>Formatted zip</returns>
        public static string FormatZipShow(string zip, string country, bool needEncode)
        {
            zip = needEncode ? ScriptFilter.FilterScript(zip) : zip;
            string mask = string.Empty;
            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();

            //if entity country is empty, will not the default regional setting.
            RegionalModel regionalModel = string.IsNullOrEmpty(country) ? null : regionalBll.GetRegionalModelByCountry(country, out states);

            if (regionalModel != null)
            {
                mask = regionalModel.zipCodeMask;
            }

            return I18nZipUtil.FormatZipByMask(mask, zip);
        }

        /// <summary>
        /// Get value of enable name to description (Enabled/Disabled)
        /// </summary>
        /// <param name="enableValue">the value of enable flag</param>
        /// <returns>Description of Enable value</returns>
        public static string GetEnableFlagDescription(string enableValue)
        {
            string result = string.Empty;

            if (enableValue == ACAConstant.VALID_STATUS)
            {
                result = LabelUtil.GetTextByKey("ACA_Common_Enabled", System.Web.HttpContext.Current.Request[ACAConstant.MODULE_NAME]);
            }
            else if (enableValue == ACAConstant.INVALID_STATUS)
            {
                result = LabelUtil.GetTextByKey("ACA_Common_Disabled", System.Web.HttpContext.Current.Request[ACAConstant.MODULE_NAME]);
            }

            return result;
        }

        /// <summary>
        /// Get APO or people template model value for display, especially for getting formatted date.
        /// </summary>
        /// <param name="model">TemplateAttributeModel object</param>
        /// <returns>APO or people template model value to display</returns>
        public static string GetI18NTemplateValue(TemplateAttributeModel model)
        {
            string result = model == null || string.IsNullOrWhiteSpace(model.attributeValue) ? string.Empty : model.attributeValue;

            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(model.attributeValueDataType))
            {
                if (ControlType.Date.ToString().Equals(model.attributeValueDataType, StringComparison.OrdinalIgnoreCase))
                {
                    result = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(result);
                }
                else if (ControlType.Number.ToString().Equals(model.attributeValueDataType, StringComparison.OrdinalIgnoreCase))
                {
                    result = I18nNumberUtil.ConvertNumberFromWebServiceToInput(result);
                }
            }

            return result;
        }

        /// <summary>
        /// get Simple View Element Model by section id
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="permission">the GFilterScreenPermissionModel</param>
        /// <param name="sectionId">section id</param>
        /// <returns>SimpleViewElementModel4WS array</returns>
        public static SimpleViewElementModel4WS[] GetSimpleViewElementModelBySectionID(string moduleName, GFilterScreenPermissionModel4WS permission, string sectionId)
        {
            if (AppSession.IsAdmin)
            {
                return null;
            }

            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            return gviewBll.GetSimpleViewElementModel(moduleName, permission, sectionId, AppSession.User.UserID);
        }

        /// <summary>
        /// Get APO or people template model value for display, especially for getting DDL selected text or formatted date.
        /// </summary>
        /// <param name="model">TemplateAttributeModel object</param>
        /// <returns>APO or people template model value for display</returns>
        public static string GetTemplateValue4Display(TemplateAttributeModel model)
        {
            string result = model == null ? string.Empty : model.attributeValue;

            if (model != null && !string.IsNullOrEmpty(model.attributeValueDataType))
            {
                if (model.attributeValueDataType.Equals(ControlType.Date.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    result = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(model.attributeValue);
                }
                else if (model.attributeValueDataType.Equals(ControlType.Number.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    result = I18nNumberUtil.ConvertNumberStringFromWebServiceToUI(model.attributeValue);
                }
                else if (model.attributeValueDataType.Equals(ControlType.Radio.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    result = FormatYNLabel(model.attributeValue);
                }
                else if (model.attributeValueDataType.Equals(ControlType.DropdownList.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    TemplateAttrValueModel[] ddlValueModels = model.selectOptions;

                    if (ddlValueModels != null && ddlValueModels.Length > 0)
                    {
                        foreach (TemplateAttrValueModel ddlValueModel in ddlValueModels)
                        {
                            if (ddlValueModel != null && !string.IsNullOrWhiteSpace(ddlValueModel.attributeValue) && ddlValueModel.attributeValue.Equals(model.attributeValue, StringComparison.OrdinalIgnoreCase))
                            {
                                result = I18nStringUtil.GetString(ddlValueModel.resAttributeValue, ddlValueModel.attributeValue);
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get and format value of Generic template field for display.
        /// </summary>
        /// <param name="template">Generic template field.</param>
        /// <returns>String of formatted field value.</returns>
        public static string GetTemplateValue4Display(GenericTemplateAttribute template)
        {
            string result = string.Empty;

            if (template == null)
            {
                return result;
            }

            result = template.defaultValue;

            switch (template.fieldType)
            {
                case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                    result = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(template.defaultValue);
                    break;
                case (int)FieldType.HTML_SELECTBOX:
                    var options = template.options;

                    if (options != null && options.Length > 0)
                    {
                        foreach (var option in options)
                        {
                            if (option != null && !string.IsNullOrWhiteSpace(option.key) && option.key.Equals(template.defaultValue, StringComparison.OrdinalIgnoreCase))
                            {
                                result = I18nStringUtil.GetString(option.value, option.key);
                                break;
                            }
                        }
                    }

                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_TIME:
                    result = template.defaultValue;
                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                    result = I18nNumberUtil.ConvertNumberStringFromWebServiceToUI(template.defaultValue);
                    break;
                case (int)FieldType.HTML_RADIOBOX:
                case (int)FieldType.HTML_CHECKBOX:
                    result = FormatYNLabel(template.defaultValue);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Display proper Y/N,Yes/No,CHECKED/UNCHECKED label for RadioButton and CheckBox in ACA.
        /// </summary>
        /// <param name="value">the value indicate the status</param>
        /// <param name="isDefaultAsNo">is default as No when value is null or empty.</param>
        /// <returns>The value label</returns>
        public static string FormatYNLabel(string value, bool isDefaultAsNo = false)
        {
            string valueLabel = string.Empty;

            // 1. Check if value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                valueLabel = isDefaultAsNo ? LabelUtil.GetGlobalTextByKey("ACA_Common_No") : string.Empty;
            }
            else
            {
                // 2. If value is "CHECKED", "Y", "Yes", then return "Yes"
                //    If value is "UNCHECKED", "N", "No", then return "Yes"
                //    otherwise, return value directly.
                if (ValidationUtil.IsYes(value) || ACAConstant.COMMON_CHECKED.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    valueLabel = LabelUtil.GetGlobalTextByKey("ACA_Common_Yes");
                }
                else if (ValidationUtil.IsNo(value) || ACAConstant.COMMON_UNCHECKED.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    valueLabel = LabelUtil.GetGlobalTextByKey("ACA_Common_No");
                }

                if (string.IsNullOrEmpty(valueLabel))
                {
                    valueLabel = value;
                }
            }

            return valueLabel;
        }

        /// <summary>
        /// Add Key Value.
        /// </summary>
        /// <param name="sb">String Builder</param>
        /// <param name="key">string format key</param>
        /// <param name="value">string value.</param>
        public static void AddKeyValue(StringBuilder sb, string key, string value)
        {
            sb.AppendFormat("\"{0}\":\"{1}\",", key, value == null ? string.Empty : value.Replace("'", "\'").Replace("\"", "\\\""));
        }

        /// <summary>
        /// if the characters larger than 43 then break
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>break line number</returns>
        private static int BreakWords(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            int line = (input.Length / 43) + 1;
            string[] temp = Regex.Split(input, "[ ]+");

            if (temp.Length >= line)
            {
                return line;
            }
            else
            {
                return temp.Length;
            }
        }

        /// <summary>
        /// Concatenate comma between original value and append value.
        /// </summary>
        /// <param name="originalValue">original value.</param>
        /// <param name="appendValue">append value.</param>
        /// <returns>the concatenate string with comma.</returns>
        private static string ConcatComma(string originalValue, string appendValue)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(originalValue) && !string.IsNullOrEmpty(appendValue))
            {
                result = string.Format("{0}, {1}", originalValue, appendValue);
            }
            else
            {
                result = string.Format("{0}{1}", originalValue, appendValue);
            }

            return result;
        }

        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="o">object value</param>
        /// <returns>string for object</returns>
        private static string ConvertToString(object o)
        {
            return o == null ? string.Empty : o.ToString();
        }

        /// <summary>
        /// Put address with primaryFlag='Y' to the first place.
        /// </summary>
        /// <param name="addresses">the address array need to be reorder by Primary.</param>
        /// <returns>The address array has been re-sorted.</returns>
        private static AddressModel[] ResortAddress(AddressModel[] addresses)
        {
            if (addresses == null ||
                addresses.Length <= 1)
            {
                return addresses;
            }

            AddressModel primaryAddress = null;

            foreach (AddressModel addr in addresses)
            {
                if (ValidationUtil.IsYes(addr.primaryFlag))
                {
                    primaryAddress = addr;
                    break;
                }
            }

            // if there is no primary address, just return the original addresses, needn't to re-sort.
            if (primaryAddress == null)
            {
                return addresses;
            }

            AddressModel[] returnAddrs = new AddressModel[addresses.Length];

            // put the primary address to addresses[0]
            returnAddrs[0] = primaryAddress;
            int index = 1;

            for (int i = 0; i < addresses.Length; i++)
            {
                if (addresses[i] != primaryAddress)
                {
                    returnAddrs[index] = addresses[i];
                    index++;
                }
            }

            return returnAddrs;
        }

        #endregion
    }
}
