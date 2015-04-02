/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestRefAddressWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestRefAddressWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestRefAddressWS : TestBase
    {
        private RefAddressWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<RefAddressWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetAddressCondition()
        {
            //long addressID = 5;
            //string callerID = "ADMIN";
            //RefAddressModel4WS resultRefAddressModel4WS =
            //    _unitUnderTest.getAddressCondition(AgencyCode, addressID, callerID);

            //Assert.IsNotNull(resultRefAddressModel4WS);
            //Assert.IsNotNull(resultRefAddressModel4WS.hightestCondition);
            //Assert.AreEqual("Lock", resultRefAddressModel4WS.hightestCondition.impactCode);
            //Assert.IsNotNull(resultRefAddressModel4WS.noticeConditions);
        }

        [Test()]
        public void TestGetRefAddressByPK()
        {
            //String agencyCode = AgencyCode;
            //long addressID = 215;
            //long sourceSeqNo = 45;
            //RefAddressModel4WS resultRefAddressModel4WS =
            //    _unitUnderTest.getRefAddressByPK(agencyCode, sourceSeqNo, addressID);

            //Assert.IsNotNull(resultRefAddressModel4WS);
        }
    }
}