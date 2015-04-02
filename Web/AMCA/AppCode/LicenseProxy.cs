/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LicenseProxy.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 *  Methods from ACA 7.0 code modules that have adapted for use by AMCA.
 *  Notes:
 *      $Id: AppSession.cs 77905 2007-10-15 12:49:28Z dave.brewster.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  Methods from ACA 7.0 code modules that have adapted for use by AMCA.
 * </pre>
 */

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using System.Collections.Generic;
using System.Text;
// using System.Web.UI.MobileControls;
using Accela.ACA.WSProxy.Common;

/// <summary>
/// Summary description for LicenseProxy
/// </summary>
public class LicenseProxy
{
    public bool OnErrorReturn = false;
    public bool TestReturnError = false;
    public string ExceptionMessage = string.Empty;

    #region LICENSE retrieve

    /// <summary>
    /// License retrieval    
    /// </summary>
    /// <param name="ThisLicense"></param>
    /// <returns></returns>
    /// 
    public LicenseModel4WS GetLicenseModel4WS(string licenseNo, string licenseType, string agency, string moduleName)
    {
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
 
            ILicenseBLL licenseBLL = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));

            LicenseModel4WS licenseCondtion = new LicenseModel4WS();

            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            licenseCondtion.serviceProviderCode = agency;
            licenseCondtion.licenseType = licenseType;
            licenseCondtion.stateLicense = licenseNo;

            LicenseModel4WS licenseModel = licenseBLL. GetLicenseByStateLicNbr(licenseCondtion);

            return licenseModel;

        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        //  Returns Demo Data for UI
        return null;

        //call LicenseBLL.GetLicenseByStateLicNbr()
    }

    /// <summary>
    /// License retrieval    
    /// </summary>
    /// <param name="ThisLicense"></param>
    /// <returns></returns>
    /// 
    public License LicenseRetrieve(License ThisLicense)
    {
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            if (ThisLicense == null)
            {
                return null;
            }

            ILicenseBLL licenseBLL = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));

            LicenseModel4WS licenseCondtion = new LicenseModel4WS();

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ThisLicense.ModuleName);

            licenseCondtion.serviceProviderCode = capModel.capID.serviceProviderCode;
            licenseCondtion.licenseType = ThisLicense.LicenseType;
            licenseCondtion.stateLicense = ThisLicense.LicenseNumber;

            LicenseModel4WS licenseModel = licenseBLL.GetLicenseByStateLicNbr(licenseCondtion);

            return convertLicenseModelToLicense(licenseModel);

        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        //  Returns Demo Data for UI
        return null;

        //call LicenseBLL.GetLicenseByStateLicNbr()
    }
    private  License convertLicenseModelToLicense(LicenseModel4WS licenseModel)
    {
        if (licenseModel == null)
        {
            return null;
        }

        License license = new License();
        license.BusinessName = licenseModel.businessName;
        license.City = licenseModel.city;
        license.Country = licenseModel.country;
        license.FirstName = licenseModel.contactFirstName;
        license.MiddleName = licenseModel.contactMiddleName;
        license.LastName = licenseModel.contactLastName;
        license.LicenseNumber = licenseModel.stateLicense;
        license.LicenseType = licenseModel.licenseType;
        license.Phone1 = licenseModel.phone1;

        return license;
    }
    #endregion
}
