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
 * $Id: TestLicenseWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestLicenseWS : TestBase
    {
        private LicenseWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<LicenseWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetLicenseCondition()
        {
            long licenseSeqNo = 5630;
            string callerID = "ADMIN";
            //LicenseModel4WS result = _unitUnderTest.getLicenseCondition(AgencyCode, licenseSeqNo, callerID);

            //Assert.IsNotNull(result);
            //Assert.IsNotNull(result.hightestCondition);
            //Assert.AreEqual("Lock", result.hightestCondition.impactCode);
            //Assert.IsNotNull(result.noticeConditions);
        }

        [Test()]
        public void getLicenseByStateLicNbr()
        {
            LicenseModel4WS licenseModel4WS = new LicenseModel4WS();
            licenseModel4WS.serviceProviderCode = AgencyCode;
            licenseModel4WS.licSeqNbr = "28491";
            licenseModel4WS.licenseType = "ARCHITECT";
            licenseModel4WS.stateLicense = "C12815";

            LicenseModel4WS result = _unitUnderTest.getLicenseByStateLicNbr(licenseModel4WS);

            Assert.IsNotNull(result);
           
        }
    }
}