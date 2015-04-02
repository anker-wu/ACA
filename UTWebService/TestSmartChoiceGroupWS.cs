/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestSmartChoiceGroupWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2010
 * 
 *  Description:
 * 
 *  Notes:
 *  $Id: TestSmartChoiceGroupWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  12/19/2007     		troy.yang				Initial.
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
    public class TestSmartChoiceGroupWS : TestBase
    {
        private SmartChoiceGroupWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<SmartChoiceGroupWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetSmartChoiceGroup()
        {
            string groupName = "BUILDING";
            string callerID = "ADMIN";

            SmartChoiceGroupModel4WS[] result =
                _unitUnderTest.getSmartChoiceGroup(AgencyCode, groupName, callerID);

            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestGetAllSmartChoiceGroup()
        {
            string callerID = "ADMIN";

            SmartChoiceGroupModel4WS[] result =
                _unitUnderTest.getSmartChoiceGroup(AgencyCode, null, callerID);

            Assert.IsNotNull(result);
        }
    }
}