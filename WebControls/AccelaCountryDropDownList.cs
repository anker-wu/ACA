#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaCountryDropDownList.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaCountryDropDownList.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Country dropdown-list web control, the country items will be loaded automatically by self-control.
    /// If country code is turned on, this control will not present anything.
    /// </summary>
    public class AccelaCountryDropDownList : AccelaDropDownList
    {
        #region Fields

        /// <summary>
        /// control id for dropdown state
        /// </summary>
        private string _controlIDForDDLState;

        /// <summary>
        /// control id for text state
        /// </summary>
        private string _controlIDForTxtState;

        /// <summary>
        /// relevant control ID string
        /// </summary>
        private string _relevantControlIDs = string.Empty;

        /// <summary>
        /// zip text control
        /// </summary>
        private AccelaZipText _zipText;

        /// <summary>
        /// phone number controls
        /// </summary>
        private List<AccelaPhoneText> _phoneTexts;

        /// <summary>
        /// state control
        /// </summary>
        private AccelaStateControl[] _stateControls;

        /// <summary>
        /// zip mask string
        /// </summary>
        private string _zipMask = string.Empty;

        /// <summary>
        /// phone mask string
        /// </summary>
        private string _phoneMask = string.Empty;

        /// <summary>
        /// toolTip label key string.
        /// </summary>
        private string _toolTipLabelKey;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets state dropdownlist control client id for USA
        /// </summary>
        public string ControlIDForDDLState
        {
            get
            {
                return _controlIDForDDLState;
            }

            set
            {
                _controlIDForDDLState = value;
            }
        }

        /// <summary>
        /// Gets or sets state text control client id for Non-USA
        /// </summary>
        public string ControlIDForTextState
        {
            get
            {
                return _controlIDForTxtState;
            }

            set
            {
                _controlIDForTxtState = value;
            }
        }

        /// <summary>
        /// Gets or sets the relevant country code text fields.Split the different control id with comma.
        /// when the onChange event of country dropdown-list is raised, 
        /// The selected IDD will be populated into the specified controls which is defined by this property. 
        /// </summary>
        public string RelevantControlIDs
        {
            get
            {
                return _relevantControlIDs;
            }

            set
            {
                _relevantControlIDs = value;
            }
        }

        /// <summary>
        /// Gets or sets the zip text.
        /// </summary>
        /// <value>The zip text.</value>
        public AccelaZipText ZipText
        {
            get
            {
                return _zipText;
            }

            set
            {
                _zipText = value;
            }
        }

        /// <summary>
        /// Gets or sets the phone texts.
        /// </summary>
        /// <value>The phone texts.</value>
        public List<AccelaPhoneText> PhoneTexts
        {
            get
            {
                return _phoneTexts;
            }

            set
            {
                _phoneTexts = value;
            }
        }

        /// <summary>
        /// Gets or sets the state control.
        /// </summary>
        /// <value>The state control.</value>
        public AccelaStateControl[] StateControls
        {
            get
            {
                return _stateControls;
            }

            set
            {
                _stateControls = value;
            }
        }

        /// <summary>
        /// Gets or sets the country zip mask.
        /// </summary>
        /// <value>The country zip mask.</value>
        public string CountryZipMask
        {
            get
            {
                return _zipMask;
            }

            set
            {
                _zipMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the country phone mask.
        /// </summary>
        /// <value>The country phone mask.</value>
        public string CountryPhoneMask
        {
            get
            {
                return _phoneMask;
            }

            set
            {
                _phoneMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the ToolTipLabelKey
        /// </summary>
        public override string ToolTipLabelKey
        {
            get
            {
                return !string.IsNullOrEmpty(_toolTipLabelKey) ? _toolTipLabelKey : "aca_common_msg_dropdown_changecountry_tip";
            }

            set
            {
                _toolTipLabelKey = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind the country list items.
        /// </summary>
        public void BindItems()
        {
            if (this.DesignMode)
            {
                return;
            }

            this.Items.Clear();

            IBizdomainProvider bizProvider = ObjectFactory.GetObject(typeof(IBizdomainProvider)) as IBizdomainProvider;

            IList<ItemValue> countryNameList = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY);
            IList<ItemValue> countryDefalut = bizProvider.GetBizDomainList(BizDomainConstant.STD_COUNTRY_DEFAULT_VALUE);

            if (countryNameList != null && countryNameList.Count > 0)
            {
                foreach (ItemValue item in countryNameList)
                {
                    // if country is empty, needn't to add to items
                    if (item.Value == null || item.Value.ToString().Trim() == string.Empty)
                    {
                        continue;
                    }

                    // country name - langCode
                    if (countryDefalut != null && countryDefalut.Count > 0 && countryDefalut[0].Key.Equals(item.Key))
                    {
                        this.Items.Insert(0, new ListItem(item.Value.ToString(), item.Key));
                    }
                    else
                    {
                        this.Items.Add(new ListItem(item.Value.ToString(), item.Key));
                    }
                }
            }

            // this.SourceType = DropDownListDataSourceType.StandardChoice;
            // added --select--
            this.Items.Insert(0, new ListItem(LabelConvertUtil.GetGlobalTextByKey("aca_common_select"), string.Empty));
        }

        /// <summary>
        /// Initialize the country list items.
        /// </summary>
        public void RegisterScripts()
        {
            bool isAdmin = false;

            // if the control is in admin mode, needn't to register any scripts.
            if (this.Page is IPage)
            {
                if ((this.Page as IPage).IsControlRenderAsAdmin)
                {
                    isAdmin = true;
                }
            }

            if (!isAdmin)
            {
                // control level script
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.ClientID))
                {
                    StringBuilder sbScripts = new StringBuilder();
                    sbScripts.Append("<script type=\"text/javascript\" language=\"javascript\">\n");
                    sbScripts.Append(GetRelevantControlIdArray());
                    sbScripts.Append("</script>\n");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), this.ClientID, sbScripts.ToString(), false);
                }

                string relPhoneCtlsArrayName = "relevantControls_" + this.ClientID;

                StringBuilder sbOnChangeScript = new StringBuilder();
                sbOnChangeScript.Append("javascript:onCountryChange(this.value,");
                sbOnChangeScript.Append("'").Append(relPhoneCtlsArrayName).Append("');");

                this.Attributes.Add("onchange", sbOnChangeScript.ToString());

                string scriptName = "country_idd"; // page level

                if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptName))
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), scriptName, GetCountryCodeScript(), false);
                }
            }
        }

        /// <summary>
        /// Generate the set phone control's country code.
        /// </summary>
        /// <returns>script string for country code</returns>
        private string GetCountryCodeScript()
        {
            StringBuilder sbScripts = new StringBuilder();
            sbScripts.Append("<script type=\"text/javascript\" language=\"javascript\">\n");
            sbScripts.Append("var iddList = new Array();\n");

            IBizdomainProvider bizProvider = ObjectFactory.GetObject(typeof(IBizdomainProvider)) as IBizdomainProvider;
            IList<ItemValue> countryIDDList = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY_IDD);

            int index = 0;

            foreach (ItemValue item in countryIDDList)
            {
                sbScripts.Append("iddList[").Append(index.ToString()).Append("] = new Array(");
                sbScripts.Append("'").Append(item.Key).Append("'").Append(",");
                sbScripts.Append("'").Append(Convert.ToString(item.Value)).Append("'");
                sbScripts.Append(");\n");
                index++;
            }

            //sbScripts.Append("window.onload = setIDDToRelevantControls(document.getElementById('").Append(this.ClientID).Append("').value);");
            sbScripts.Append(
                @"
            function onCountryChange(selectedLangId,relPhoneCltsArrName) {
                var relevantControls = eval(relPhoneCltsArrName);

                if(relevantControls != null) {
                    var idd = getIDDbyLanguageID(selectedLangId);
                    for(var i=0;i<relevantControls.length;i++) {
                        var objCountryCode = document.getElementById(relevantControls[i]);
                        if(objCountryCode != null) {
                            objCountryCode.value = idd;
                        }
                    }
                }

                if (typeof (myValidationErrorPanel) != 'undefined'){
                    myValidationErrorPanel.clearErrors();
                }
            }

            function getIDDbyLanguageID(langId) {
                for(var i=0;i<iddList.length;i++) {
                    if(iddList[i][0] == langId) {
                        return iddList[i][1];
                    }
                }
                return '';
            }");

            sbScripts.Append("</script>\n");
            return sbScripts.ToString();
        }

        /// <summary>
        /// Get the javascript array to store the relevant control id (client id). 
        /// </summary>
        /// <returns>relevant control ids string</returns>
        private string GetRelevantControlIdArray()
        {
            StringBuilder sbRelevantCtls = new StringBuilder();
            string arrayName = "relevantControls_" + this.ClientID;
            sbRelevantCtls.Append("var ").Append(arrayName).Append(" = new Array();\n");

            if (string.IsNullOrEmpty(_relevantControlIDs))
            {
                return sbRelevantCtls.ToString();
            }

            string[] controlIds = _relevantControlIDs.Split(new char[] { ',' });

            if (controlIds != null && controlIds.Length > 0)
            {
                for (int i = 0; i < controlIds.Length; i++)
                {
                    sbRelevantCtls.Append(arrayName).Append("[").Append(i.ToString()).Append("]");
                    sbRelevantCtls.Append("=").Append("'").Append(controlIds[i] + "_IDD").Append("';\n");
                }
            }

            return sbRelevantCtls.ToString();
        }

        #endregion Methods
    }
}