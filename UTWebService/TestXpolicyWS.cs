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
 * $Id: TestXpolicyWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestXpolicyWS : TestBase
    {
        private PolicyWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<PolicyWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void test()
        {
            XpolicyUserRolePrivilegeModel4WS[] xpolicy4WSArray = new XpolicyUserRolePrivilegeModel4WS[2];
            XpolicyUserRolePrivilegeModel4WS aa = new XpolicyUserRolePrivilegeModel4WS();
            XpolicyUserRolePrivilegeModel4WS aa1 = new XpolicyUserRolePrivilegeModel4WS();
            UserRolePrivilegeModel4WS urp = new UserRolePrivilegeModel4WS();
            aa.userRolePrivilegeModel = urp;
            aa.userRolePrivilegeModel.allAcaUserAllowed = true;
            aa.userRolePrivilegeModel.capCreatorAllowed = false;
            aa.serviceProviderCode = AgencyCode;
            aa.level = "MODULE";
            aa.policyName = "LoginPolicy";
            aa.levelData = "Building";
            aa.data1 = "USERROLE";
            aa.rightGranted = "Y";
            aa.status = "Y";

            aa1.serviceProviderCode = AgencyCode;
            aa1.level = "MODULE";
            aa1.policyName = "LoginPolicy";
            aa1.levelData = "AMS";
            aa1.data1 = "USERROLE";
            aa1.rightGranted = "Y";
            aa1.status = "Y";

            UserRolePrivilegeModel4WS ur = new UserRolePrivilegeModel4WS();
            aa1.userRolePrivilegeModel = ur;
            aa1.userRolePrivilegeModel.allAcaUserAllowed = true;
            aa1.userRolePrivilegeModel.capCreatorAllowed = true;
                

            xpolicy4WSArray[0] = aa;

            xpolicy4WSArray[1] = aa1;


            //_unitUnderTest.createOrEditPolicy(AgencyCode, xpolicy4WSArray, "admin");
        }

        //[Test()]
        //public void testselect()
        //{
     
        //    String level = "MODULE";


        //    _unitUnderTest.getXpolicyUserRoleList(AgencyCode, "ACA", "admin");
        //}

    }
}
