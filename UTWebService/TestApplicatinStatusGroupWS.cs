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
 * $Id: TestApplicatinStatusGroupWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestApplicationStatusGroupWS : TestBase
    {
        private AppStatusGroupWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<AppStatusGroupWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void getAppStatusGroupBySPC()
        {
            string moduleName = "Building";
            object[] result = _unitUnderTest.getAppStatusGroupBySPC(AgencyCode, moduleName);

            Assert.IsNotNull(result);

        }
        [Test()]
        public void editAppStatusGroupBySPC()
        {
            AppStatusGroupModel4WS[] appStatusGroups = new AppStatusGroupModel4WS[1];

            AppStatusGroupModel4WS appStatusGroup = new AppStatusGroupModel4WS();
            string moduleName = "Building";

            appStatusGroup.servProvCode = AgencyCode;
            appStatusGroup.moduleName = moduleName;
            appStatusGroup.m_appStatusGroupCode = "test";
            appStatusGroup.status = "Albert";
            appStatusGroup.acaStatus = "I";
            appStatusGroup.auditStatus = "A";
            appStatusGroup.auditID = "ACA Admin";

            appStatusGroups[0] = appStatusGroup;

            _unitUnderTest.editAppStatusGroup(appStatusGroups);


        }
    }
}