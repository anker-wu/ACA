/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestCapWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestLicenseProfessionalWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestLicenseProfessionalWS : TestBase
    {
        private LicenseProfessionalWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<LicenseProfessionalWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void getTradeNames()
        {
            string callerID = "PUBLICUSER9273";
            string moduleName = "Licenses";
            string capTypePickerName = "TRADE_NAME";

            object result = _unitUnderTest.getTradeNames(AgencyCode, callerID, moduleName, capTypePickerName);

            Assert.IsNotNull(result);

        }

        [Test()]
        public void getTradeNameListByLicenseModel()
        {
            string callerID = "PUBLICUSER9273";

            LicenseModel4WS licenseModel4WS = new LicenseModel4WS();
            licenseModel4WS.serviceProviderCode = AgencyCode;
            licenseModel4WS.businessName = "Yitoa";
            //licenseModel4WS.busName2 = "";

            string capTypePickerName = "TRADE_LICENSES";

            //object[] result = _unitUnderTest.getTradeNameListByLicenseModel(servProvCode,callerID,licenseModel4WS,capTypePickerName);

            //Assert.IsNotNull(result);

        }

        [Test()]
        public void getTradeLicenseByTradeName()
        {
                   
            LicenseModel4WS licenseModel4WS = new LicenseModel4WS();

            licenseModel4WS.serviceProviderCode = AgencyCode;
            licenseModel4WS.licSeqNbr = "28491";
            licenseModel4WS.licenseType = "ARCHITECT";
            licenseModel4WS.stateLicense = "C12815";

            //object result = _unitUnderTest.getCapIDByLP(licenseModel4WS, null);

            //Assert.IsNotNull(result);

        }


    }
}
