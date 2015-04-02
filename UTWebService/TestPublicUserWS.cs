/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestPublicUserWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestPublicUserWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  12/19/2007     		troy.yang				Initial.
 * </pre>
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.Test.Lib;
using Accela.ACA.Test.WebService;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestPublicUserWS : TestBase
    {
        private PublicUserWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<PublicUserWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void signon()
        {
            string username = "dylan.liang@achievo.com";
            string password = "11111111";

            long result = _unitUnderTest.signon(AgencyCode, username, password);

            if (result > 0 || result == 0)
            {
                Console.WriteLine("This user " + result + " login successfully");
            }
            else
            {
                Console.WriteLine("login failed " + result);
            }
        }

        [Test()]
        public void isExistingEmailID()
        {
            string userEmail = "Dylan.liang@Achievo.com";
            string result = _unitUnderTest.isExistingEmailID(AgencyCode, userEmail);

            Console.WriteLine("This email is exist in database " + result);
        }

        [Test()]
        public void isExistingUserID()
        {
            string userID = "Dylan";
            string result = _unitUnderTest.isExistingUserID(AgencyCode, userID);

            Console.WriteLine("This user id is exist in database " + result);
        }

        [Test()]
        public void getPublicUser()
        {
            long userSeqNum = 143;
            _unitUnderTest.getPublicUser(AgencyCode, userSeqNum);

        }
    }
}
