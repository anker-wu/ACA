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
 * $Id: TestPUserWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestPUserWS : TestBase
    {
        private SSOWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<SSOWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void signon()
        {
            string servProvCode = AgencyCode;
            string username = "ADMIN";
            string password = "ADMIN";
            string[] attributes = new string[3];
            attributes[0] = "50.10.1.101";
            attributes[1] = "50.10.1.101";
            attributes[2] = "IE7.0";
            string result = _unitUnderTest.signon(servProvCode, username, password);

            Assert.IsNotNull(result);

        }   

    }
}