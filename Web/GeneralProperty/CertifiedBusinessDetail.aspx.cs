#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CertifiedBusinessDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CertifiedBusinessDetail.ascx.cs 190488 2011-02-17 10:00:36Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// Class for certified business detail page.
    /// </summary>
    public partial class CertifiedBusinessDetail : BasePage
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    LicenseModel4WS selectedLicenseeModel = GetLicense();
                    DisplayCertifiedBusinessInfo(selectedLicenseeModel);
                }
            }
        }

        /// <summary>
        /// ItemDataBound event method for repeat NIGP to create tree.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void NigpClassList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("rptNigpSubClass") as Repeater;
                KeyValuePair<string, IList<string>> dict = (KeyValuePair<string, IList<string>>)e.Item.DataItem;

                rep.DataSource = dict.Value;
                rep.DataBind();
            }
        }

        /// <summary>
        /// Get license selected in certified business list.
        /// </summary>
        /// <returns>LicenseModel selected in certified business list</returns>
        private LicenseModel4WS GetLicense()
        {
            LicenseModel4WS result = null;
            LicenseModel4WS licenseeModel = new LicenseModel4WS();
            var licenseeBll = ObjectFactory.GetObject<ILicenseBLL>();

            licenseeModel.stateLicense = Request["stateLicense"] == null ? string.Empty : Request["stateLicense"].ToString();
            licenseeModel.licenseType = Request["licenseType"] == null ? string.Empty : Request["licenseType"].ToString();
            licenseeModel.serviceProviderCode = string.IsNullOrEmpty(Request[UrlConstant.AgencyCode]) ? string.Empty : Request[UrlConstant.AgencyCode];

            result = licenseeBll.GetLicense(licenseeModel, true);

            return result;
        }

        /// <summary>
        /// Display certified business information.
        /// </summary>
        /// <param name="license">LicenseModel selected in certified business list</param>
        private void DisplayCertifiedBusinessInfo(LicenseModel4WS license)
        {
            if (license != null)
            {
                lblCertifiedBusinessName.Text = license.businessName;
                certifiedBusinessGeneralInfo.DisplayGerneralInfo(license);
                DisplayExperienceList(license);
                DisplayNIGPCodeTree(license);
            }
        }

        /// <summary>
        /// Display certified experience record.
        /// </summary>
        /// <param name="licensee">license associated to experience.</param>
        private void DisplayExperienceList(LicenseModel4WS licensee)
        {
            DataTable dataSource = LicenseUtil.GetExperiences(licensee.template);

            if (!AppSession.IsAdmin && dataSource != null && dataSource.Rows.Count > 0)
            {
                experienceList.BindExperienceList(dataSource);
            }
            else
            {
                experienceList.BindExperienceList(new DataTable());
            }
        }

        /// <summary>
        /// Display NIGP code tree.
        /// </summary>
        /// <param name="licensee">License containing appointed NIGP code.</param>
        private void DisplayNIGPCodeTree(LicenseModel4WS licensee)
        {
            List<object> nigpClassList = LicenseUtil.GetValueFromTemplateTables(licensee.template, ACAConstant.NIGP_FIELD_NIGPCODE_CLASS);
            List<object> nigpSubClassList = LicenseUtil.GetValueFromTemplateTables(licensee.template, ACAConstant.NIGP_FIELD_NIGPCODE_SUBCLASS);
 
            if (nigpClassList != null && nigpClassList.Count > 0)
            {
                Dictionary<string, IList<string>> dict = new Dictionary<string, IList<string>>();

                Comparison<object> comparison = new Comparison<object>(
                    delegate(object a, object b) 
                    {
                        if (a == null && b == null)
                        {
                            return 0;
                        }
                        else if (a == null)
                        {
                            return -1;
                        }
                        else if (b == null)
                        {
                            return 1;
                        }

                        return a.ToString().CompareTo(b);                        
                    });

                nigpClassList.Sort(comparison);
                nigpSubClassList.Sort(comparison);

                foreach (var item in nigpClassList)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    string nigpClass = item.ToString();

                    if (dict.ContainsKey(nigpClass))
                    {
                        continue;
                    }
                    else
                    {
                        dict.Add(nigpClass, null);
                    }
                }

                foreach (var item in nigpSubClassList)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    string nigpSubClass = item.ToString();

                    if (nigpSubClass.Length >= 3)
                    {
                        string prefix = nigpSubClass.Substring(0, 3);

                        foreach (string nigpClass in dict.Keys)
                        {
                            if (nigpClass.Length >= 3 && nigpClass.StartsWith(prefix))
                            {
                                IList<string> list = dict[nigpClass];

                                if (list == null) 
                                {
                                    list = new List<string>();
                                }

                                if (!list.Contains(nigpSubClass))
                                {
                                    list.Add(nigpSubClass);
                                }
                                
                                dict[nigpClass] = list;

                                break;
                            }
                        }
                    }
                }

                rptNigpClass.DataSource = dict;
                rptNigpClass.DataBind();
            }
        }
    }
}
