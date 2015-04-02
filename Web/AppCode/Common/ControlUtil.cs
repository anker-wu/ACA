#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ControlUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ControlUtil.cs 201856 2011-08-18 07:07:49Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Regional;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web
{
    /// <summary>
    /// common function of the control
    /// </summary>
    public static class ControlUtil
    {
        /// <summary>
        /// Clear control value
        /// </summary>
        /// <param name="parent">parent control.</param>
        /// <param name="filterControlIDs">Controls that needn't to be clear. 
        /// if you need to clear all children controls, you can set this parameter to null.</param>
        public static void ClearValue(Control parent, string[] filterControlIDs)
        {
            if (parent == null || parent.Controls.Count == 0)
            {
                return;
            }

            foreach (Control ctl in parent.Controls)
            {
                if (IsExistControls(ctl.ID, filterControlIDs))
                {
                    continue;
                }

                // filter control that needn't to be disabled
                if (ctl is IAccelaControl)
                {
                    (ctl as IAccelaControl).ClearValue();
                    continue;
                }

                // if control is container control, recursive call current method
                if (ctl.Controls.Count > 0)
                {
                    ClearValue(ctl, filterControlIDs);
                }
            }
        }

        /// <summary>
        /// Get the control's value when it is visible.
        /// </summary>
        /// <param name="accelaControl">The Accela Control.</param>
        /// <param name="ignoreVisible">whether ignore the visible property.</param>
        /// <returns>Return the control value.</returns>
        public static string GetControlValue(IAccelaControl accelaControl, bool ignoreVisible = false)
        {
            string result = string.Empty;
            Control ctrl = (Control)accelaControl;

            if (ctrl.Visible || ignoreVisible)
            {
                result = accelaControl.GetValue();
            }

            if ((accelaControl is ITextControl || accelaControl is AccelaStateControl) && !string.IsNullOrEmpty(result))
            {
                result = result.Trim();
            }

            return result;
        }

        /// <summary>
        /// Get the AccelaPhoneText control's country code.
        /// </summary>
        /// <param name="accelaPhoneText">The AccelaPhoneText control.</param>
        /// <param name="ignoreVisible">whether ignore the visible property.</param>
        /// <returns>Return the country code.</returns>
        public static string GetCountryCodeText(AccelaPhoneText accelaPhoneText, bool ignoreVisible = false)
        {
            if (accelaPhoneText.Visible || ignoreVisible)    
            {
                return accelaPhoneText.CountryCodeText.Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the zip.
        /// </summary>
        /// <param name="zipText">The zip text.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>The zip code.</returns>
        public static string GetZip(this AccelaZipText zipText, string countryCode)
        {
            string zipString = string.Empty;

            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();
            RegionalModel regionalModel = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                regionalModel = regionalBll.GetRegionalModelByCountry(countryCode, out states);
            }

            string mask = regionalModel == null || string.IsNullOrEmpty(regionalModel.zipCodeMask)
                              ? string.Empty
                              : regionalModel.zipCodeMask;
            zipString = zipText.GetZip(countryCode, mask);

            return zipString;
        }

        /// <summary>
        /// Gets the phone.
        /// </summary>
        /// <param name="phoneText">The phone text.</param>
        /// <param name="countryCode">The country code.</param>
        /// <param name="ignoreVisible">whether ignore the visible property.</param>
        /// <returns>The phone</returns>
        public static string GetPhone(this AccelaPhoneText phoneText, string countryCode, bool ignoreVisible = false)
        {
            string phoneString = string.Empty;

            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();
            RegionalModel regionalModel = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                regionalModel = regionalBll.GetRegionalModelByCountry(countryCode, out states);
            }

            string mask = regionalModel == null || string.IsNullOrEmpty(regionalModel.phoneNumMask)
                ? string.Empty
                : regionalModel.phoneNumMask;
            phoneString = I18nPhoneUtil.UnFormatPhoneByMask(mask, GetControlValue(phoneText, ignoreVisible));
            return phoneString;
        }
       
        /// <summary>
        /// Sets the mask for phone and zip.
        /// </summary>
        /// <param name="setPhoneCountry">When initial or clear the phone fields, need to set the phone country code.</param>
        /// <param name="getDefaultSetting">if set to <c>true</c> [get default setting].</param>
        /// <param name="zipText">The zip text.</param>
        /// <param name="stateControl">The state control.</param>
        /// <param name="isIgnoreValidate">if set to <c>true</c> [is ignore validate].</param>
        /// <param name="phoneTexts">The phone texts.</param>
        public static void SetMaskForPhoneAndZip(bool setPhoneCountry, bool getDefaultSetting, AccelaZipText zipText, AccelaStateControl stateControl, bool isIgnoreValidate, params AccelaPhoneText[] phoneTexts)
        {
            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();
            RegionalModel regionalModel = null;

            if (!AppSession.IsAdmin && getDefaultSetting)
            {
                regionalModel = regionalBll.GetRegionalModelByCountry(string.Empty, out states);
            }

            string phoneMask = I18nCultureUtil.AAZipMaskToAjaxMask(I18nPhoneUtil.PhoneNumberMask);
            string zipMask = I18nCultureUtil.AAZipMaskToAjaxMask(I18nZipUtil.ZipMask);

            string phoneExpression = I18nPhoneUtil.ValidationExpression;
            string zipExpression = I18nZipUtil.ValidationExpression;

            bool useZip = true; 

            if (regionalModel != null)
            {
                if (!string.IsNullOrEmpty(regionalModel.phoneNumMask))
                {
                    phoneMask = I18nCultureUtil.AAZipMaskToAjaxMask(regionalModel.phoneNumMask);
                    phoneExpression = I18nCultureUtil.ZipMaskToExpression(regionalModel.phoneNumMask);
                }

                if (!string.IsNullOrEmpty(regionalModel.zipCodeMask))
                {
                    zipMask = I18nCultureUtil.AAZipMaskToAjaxMask(regionalModel.zipCodeMask);
                    zipExpression = I18nCultureUtil.ZipMaskToExpression(regionalModel.zipCodeMask);
                }

                //if admin not config it , it is default used zip code.
                useZip = !ValidationUtil.IsNo(regionalModel.useZipCode);
            }

            if (zipText != null)
            {
                zipText.Mask = zipMask;
                zipText.IsIgnoreValidate = isIgnoreValidate;
                zipText.ValidationExpression = zipExpression;
                zipText.ZipMaskFromAA = zipMask;
                zipText.IsUseZip = useZip;
            }

            if (stateControl != null)
            {
                /*
                 * Keep the original value before change the control status,
                 * because change IsPresentAsText property will clear the control ViewState.
                 */
                string oldValue = stateControl.Text;

                if (states != null && states.Count != 0)
                {
                    stateControl.IsPresentAsText = false;
                    stateControl.BindItemsForDDL(states);
                }
                else
                {
                    stateControl.IsPresentAsText = true;
                }

                stateControl.Text = oldValue;
            }

            if (phoneTexts != null)
            {
                foreach (var phoneText in phoneTexts)
                {
                    phoneText.Mask = phoneMask;
                    phoneText.IsIgnoreValidate = isIgnoreValidate;
                    phoneText.ValidationExpression = phoneExpression;

                    if (setPhoneCountry && regionalModel != null)
                    {
                        phoneText.CountryCodeText = regionalModel.phoneNumCode;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the Regional Setting and country value, in edit form, the GView hide the country, need get default country setting to apply. 
        /// </summary>
        /// <param name="country">The country control.</param>
        /// <param name="isIgnoreValidate">if set to <c>true</c> [is ignore validate].</param>
        /// <param name="moduleName">Name of the module. for get GView setting </param>
        /// <param name="permission">The GView permission.</param>
        /// <param name="sectionId">The GView id.</param>
        public static void ClearRegionalSetting(AccelaCountryDropDownList country, bool isIgnoreValidate, string moduleName, GFilterScreenPermissionModel4WS permission, string sectionId)
        {
            RegionalModel regionalModel = null;
            Dictionary<string, string> state = new Dictionary<string, string>();
            string countryCode = string.Empty;

            if (!string.IsNullOrEmpty(sectionId))
            {
                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
                SimpleViewElementModel4WS[] simpleViewElementmodels = gviewBll.GetSimpleViewElementModel(moduleName, permission, sectionId, AppSession.User.UserID);
                SimpleViewElementModel4WS simpleViewElement = simpleViewElementmodels.FirstOrDefault(o => o.viewElementName.Equals(country.ID, StringComparison.OrdinalIgnoreCase));

                if (!ACAConstant.VALID_STATUS.Equals(simpleViewElement.recStatus, StringComparison.OrdinalIgnoreCase))
                {
                    ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                    regionalModel = cacheManager.GetRegionalModelByCountry(string.Empty, out state);

                    if (regionalModel != null)
                    {
                        countryCode = regionalModel.countryCode;
                    }
                }
            }

            DropDownListBindUtil.SetSelectedValue(country, countryCode);

            ApplyRegionalSettingForCountrols(true, country, isIgnoreValidate, regionalModel ?? new RegionalModel(), state, null);
        }

        /// <summary>
        /// Sets the country's child controls,
        /// e.g. zip state phone.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="zipText">The zip text.</param>
        /// <param name="stateControl">The state control.</param>
        /// <param name="phoneTexts">The phone texts.</param>
        public static void SetCountryControls(this AccelaCountryDropDownList country, AccelaZipText zipText, AccelaStateControl stateControl, params AccelaPhoneText[] phoneTexts)
        {
            SetCountryControls(country, zipText, new AccelaStateControl[] { stateControl }, phoneTexts);
        }

        /// <summary>
        /// Sets the country's child controls,
        /// e.g. zip state phone.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="zipText">The zip text.</param>
        /// <param name="stateControls">The state controls.</param>
        /// <param name="phoneTexts">The phone texts.</param>
        public static void SetCountryControls(this AccelaCountryDropDownList country, AccelaZipText zipText, AccelaStateControl[] stateControls, params AccelaPhoneText[] phoneTexts)
        {
            if (!AppSession.IsAdmin)
            {
                country.AutoPostBack = true;
            }
            
            country.ZipText = zipText;
            country.StateControls = stateControls;
            country.PhoneTexts = new List<AccelaPhoneText>();

            if (phoneTexts != null)
            {
                foreach (var phoneText in phoneTexts)
                {
                    country.PhoneTexts.Add(phoneText);
                }
            }
        }

        /// <summary>
        /// Sets the country setting.
        /// </summary>
        /// <param name="isPostBack">if set to <c>true</c> [Is post back].</param>
        /// <param name="isIgnoreValidate">if set to <c>true</c> [is ignore validate].</param>
        /// <param name="isSetCountry">if set to <c>true</c> [is set country].</param>
        /// <param name="getDefaultSetting">if set to <c>true</c> [get default setting].</param>
        /// <param name="country">The country.</param>
        /// <param name="regionalSettings">
        /// Regional settings may be passed by auto-fill post back event.
        /// If <see cref="regionalSettings"/> is null, will try to get regional settings from Post back argument.
        /// </param>
        public static void ApplyRegionalSetting(
            bool isPostBack,
            bool isIgnoreValidate,
            bool isSetCountry,
            bool getDefaultSetting,
            AccelaCountryDropDownList country,
            CountryAutoFillJson[] regionalSettings = null)
        {
            CountryAutoFillJson countryJason = null;

            if (isPostBack)
            {
                if (regionalSettings == null)
                {
                    //If Country field is hidden, the AutoFill function still need change the Country filed and apply the regional settings and other things.
                    string eventArg = HttpContext.Current.Request.Form[Page.postEventArgumentID];
                    int dataPos = !string.IsNullOrEmpty(eventArg)
                                      ? eventArg.IndexOf(ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG, StringComparison.OrdinalIgnoreCase)
                                      : -1;

                    if (dataPos != -1)
                    {
                        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                        string dataString = eventArg.Substring(dataPos + ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG.Length);

                        if (!string.IsNullOrEmpty(dataString))
                        {
                            regionalSettings = jsSerializer.Deserialize<CountryAutoFillJson[]>(dataString);
                        }
                    }
                }

                if (regionalSettings != null)
                {
                    countryJason = regionalSettings.FirstOrDefault(o => o.countryClientID == country.ClientID);

                    if (countryJason != null)
                    {
                        DropDownListBindUtil.SetSelectedValue(country, countryJason.countryCode);
                    }
                }
            }

            ApplyRegionalSettingByCountry(isPostBack, isIgnoreValidate, getDefaultSetting, country, countryJason, isSetCountry);

            country.SelectedIndexChanged += (sender, args) =>
                { 
                    var countryControl = sender as AccelaCountryDropDownList;

                    if (countryControl == null)
                    {
                        return;
                    }

                    string eventArgument = HttpContext.Current.Request.Form[Page.postEventArgumentID];
                    CountryAutoFillJson countryAutoFillJason = new CountryAutoFillJson();
                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    int statePos = string.IsNullOrEmpty(eventArgument) ? -1 : eventArgument.IndexOf(ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG, StringComparison.InvariantCultureIgnoreCase);

                    if (!string.IsNullOrEmpty(eventArgument) && statePos > -1)
                    {
                        string countryString = eventArgument.Substring(statePos + ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG.Length);

                        if (!string.IsNullOrEmpty(countryString))
                        {
                            countryAutoFillJason = jsSerializer.Deserialize<CountryAutoFillJson[]>(countryString).FirstOrDefault(o => o.countryClientID == countryControl.ClientID);
                        }
                    }

                    ApplyRegionalSettingByCountry(true, isIgnoreValidate, false, country, countryAutoFillJason);
                };

            if (isPostBack && IsPostBackOnCurrentControl(country))
            {
                AccessibilityUtil.FocusElement(country.ClientID);
            }
        }

        /// <summary>
        /// Sets the country child controls by country code.
        /// </summary>
        /// <param name="isPostBack">if set to <c>true</c> [is post back].</param>
        /// <param name="isIgnoreValidate">if set to <c>true</c> [is ignore validate].</param>
        /// <param name="isGetDefaultSetting">if set to <c>true</c> [is get default setting].</param>
        /// <param name="country">The country.</param>
        /// <param name="countryAutoFillJason">The country auto fill jason.</param>
        /// <param name="isSetCountry">if set to <c>true</c> [is set country control value].</param>
        public static void ApplyRegionalSettingByCountry(
            bool isPostBack,
            bool isIgnoreValidate,
            bool isGetDefaultSetting,
            AccelaCountryDropDownList country,
            CountryAutoFillJson countryAutoFillJason,
            bool isSetCountry = false)
        {
            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();
            string countrycode = country.SelectedValue;
            RegionalModel regionalModel = null;

            if (!AppSession.IsAdmin && (!string.IsNullOrEmpty(countrycode) || isGetDefaultSetting))
            {
                regionalModel = regionalBll.GetRegionalModelByCountry(countrycode, out states);
            }

            if (regionalModel != null)
            {
                if (isSetCountry)
                {
                    DropDownListBindUtil.SetSelectedValue(country, regionalModel.countryCode);
                }

                //if admin not config it , it is default used zip code.
                bool useZip = !ValidationUtil.IsNo(regionalModel.useZipCode);
                ApplyRegionalSettingForCountrols(isPostBack, country, isIgnoreValidate, regionalModel, states, countryAutoFillJason, useZip);
            }
            else
            {
                ApplyRegionalSettingForCountrols(isPostBack, country, isIgnoreValidate, new RegionalModel(), states, countryAutoFillJason);
            }

            if (isPostBack && IsPostBackOnCurrentControl(country))
            {
                AccessibilityUtil.FocusElement(country.ClientID);
            }
        }

        /// <summary>
        /// If post back on current control
        /// </summary>
        /// <param name="control">current control</param>
        /// <returns>Indicate whether it is post back on current control.</returns>
        public static bool IsPostBackOnCurrentControl(Control control)
        {
            string postSourceID = HttpContext.Current.Request.Form[Page.postEventSourceID];

            if (!string.IsNullOrWhiteSpace(postSourceID) && postSourceID.Contains(control.UniqueID))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the country child controls.
        /// </summary>
        /// <param name="isPostBack">if set to <c>true</c> [is post back].</param>
        /// <param name="country">The country.</param>
        /// <param name="isIgnoreValidate">if set to <c>true</c> [is ignore validate].</param>
        /// <param name="regionalModel">The regional model.</param>
        /// <param name="states">The states.</param>
        /// <param name="countryAutoFillJason">The country auto fill jason.</param>
        /// <param name="isUsedZip">if set to <c>true</c> [is used zip].</param>
        private static void ApplyRegionalSettingForCountrols(
            bool isPostBack,
            AccelaCountryDropDownList country, 
            bool isIgnoreValidate, 
            RegionalModel regionalModel,
            Dictionary<string, string> states, 
            CountryAutoFillJson countryAutoFillJason, 
            bool isUsedZip = true)
        {
            bool postBackByCountry = isPostBack && HttpContext.Current.Request.Form[Page.postEventSourceID] == country.UniqueID;
            
            if (country.ZipText != null)
            {
                string zipMaskFormat = string.IsNullOrEmpty(regionalModel.zipCodeMask) ? I18nZipUtil.ZipMask : regionalModel.zipCodeMask;
                string zipAjaxMask = I18nCultureUtil.AAZipMaskToAjaxMask(zipMaskFormat);
                string zipExpression = isIgnoreValidate
                                           ? string.Empty
                                           : string.IsNullOrEmpty(regionalModel.zipCodeMask)
                                                 ? I18nZipUtil.ValidationExpression
                                                 : I18nCultureUtil.ZipMaskToExpression(zipMaskFormat);

                if (postBackByCountry)
                {
                    country.ZipText.ClearValue();
                }

                country.ZipText.Mask = zipAjaxMask;
                country.ZipText.IsIgnoreValidate = isIgnoreValidate;
                country.ZipText.ValidationExpression = zipExpression;
                country.ZipText.ZipMaskFromAA = regionalModel.zipCodeMask;
                country.ZipText.IsUseZip = isUsedZip;

                if (countryAutoFillJason != null)
                {
                    country.ZipText.Text = countryAutoFillJason.zip;
                }
            }

            if (country.PhoneTexts != null && country.PhoneTexts.Count != 0)
            {
                string phoneMaskFormat = string.IsNullOrEmpty(regionalModel.phoneNumMask) ? I18nPhoneUtil.PhoneNumberMask : regionalModel.phoneNumMask;
                string phoneAjaxMask = I18nCultureUtil.AAPhoneMaskToAjaxMask(phoneMaskFormat);
                string phoneExpression = isIgnoreValidate
                                             ? string.Empty
                                             : string.IsNullOrEmpty(regionalModel.phoneNumMask)
                                                   ? I18nPhoneUtil.ValidationExpression
                                                   : I18nCultureUtil.PhoneMaskToExpression(phoneMaskFormat);

                foreach (var phoneText in country.PhoneTexts)
                {
                    string oldValue = phoneText.CountryCodeText;

                    if (postBackByCountry)
                    {
                        phoneText.ClearValue();

                        if (string.IsNullOrEmpty(regionalModel.countryCode))
                        {
                            phoneText.CountryCodeText = oldValue;
                        }
                        else
                        {
                            phoneText.CountryCodeText = regionalModel.phoneNumCode;
                        }
                    }
                    else if (!isPostBack && string.IsNullOrEmpty(phoneText.CountryCodeText.Trim()))
                    {
                        phoneText.CountryCodeText = regionalModel.phoneNumCode;
                    }

                    phoneText.Mask = phoneAjaxMask;
                    phoneText.IsIgnoreValidate = isIgnoreValidate;
                    phoneText.ValidationExpression = phoneExpression;

                    if (countryAutoFillJason != null && countryAutoFillJason.phone != null && countryAutoFillJason.phone.Length != 0)
                    {
                        var phoneObject =
                            countryAutoFillJason.phone.FirstOrDefault(o => o.name.Equals(phoneText.ID));

                        if (phoneObject != null)
                        {
                            phoneText.CountryCodeText = phoneObject.iddvalue;
                            phoneText.Text = phoneObject.value;
                        }
                    }
                }
            }

            if (country.StateControls != null && country.StateControls.Length != 0)
            {
                foreach (var stateControl in country.StateControls)
                {
                    string oldState = stateControl.GetValue();

                    if (postBackByCountry)
                    {
                        stateControl.ClearValue();
                    }

                    if (states != null && states.Count != 0)
                    {
                        stateControl.IsPresentAsText = false;
                        stateControl.BindItemsForDDL(states);
                    }
                    else
                    {
                        stateControl.IsPresentAsText = true;
                    }

                    if (countryAutoFillJason != null)
                    {
                        stateControl.SetValue(countryAutoFillJason.state);
                    }
                    else if (!string.IsNullOrEmpty(oldState) && !postBackByCountry)
                    {
                        stateControl.SetValue(oldState);
                    }
                }
            }

            if (isPostBack && IsPostBackOnCurrentControl(country))
            {
                AccessibilityUtil.FocusElement(country.ClientID);
            }
        }

        /// <summary>
        /// Judge the specified control id whether is existing control array.
        /// </summary>
        /// <param name="controlID">specified control id.</param>
        /// <param name="controlIDs">control id array.</param>
        /// <returns>
        /// true if the specified control id whether is existing control array.
        /// </returns>
        private static bool IsExistControls(string controlID, string[] controlIDs)
        {
            if (controlIDs == null || controlIDs.Length == 0)
            {
                return false;
            }

            foreach (string id in controlIDs)
            {
                if (id.Equals(controlID))
                {
                    return true;
                }
            }

            return false;
        }
    }
}