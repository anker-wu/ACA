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
 * $Id: TestBizDomainWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestBizDomainWS : TestBase
    {
        private BizDomainWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<BizDomainWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void getBizDomainListByModel()
        {
            BizDomainModel4WS bizDomainModel4WS = new BizDomainModel4WS();
            bizDomainModel4WS.serviceProviderCode = AgencyCode;
            bizDomainModel4WS.bizdomain = "VIOLATION_CODE";
           
            string callerID = "Admin";
            object[] result = _unitUnderTest.getBizDomainListByModel(bizDomainModel4WS, callerID,true);

            Assert.IsNotNull(result);

        }

    }
}