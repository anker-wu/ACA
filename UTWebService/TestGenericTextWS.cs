/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestCapWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestGenericTextWS.cs 121852 2009-02-24 07:09:47Z ACHIEVO\jack.su $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  12/19/2007     		troy.yang				Initial.
 * </pre>
 */

using Accela.ACA.Common;
using Accela.ACA.DAO;
using Accela.ACA.WSProxy;
using System;
using NUnit.Framework;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestGenericTextWS : BaseTestWS
    {
        private GenericViewWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetGenericViewService();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void getXUIGUITextList()
        {
            string serviceProviderCode = "FLAGSTAFF";
            GUITextModel4WS text4WS = new GUITextModel4WS();
            text4WS.categoryName = "ACA Admin";
            String callerId = "ACA Admin";
            object[] result = _unitUnderTest.getXUIGUITextList(serviceProviderCode, callerId, text4WS);

            Assert.IsNotNull(result);

        }       
    }
}