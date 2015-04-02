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
 * $Id: TestAcaTreeWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestAcaTreeWS : TestBase
    {
        private AcaAdminTreeWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<AcaAdminTreeWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void getNodeList()
        {


            //object[] result = _unitUnderTest.getNodeList(AgencyCode);

            //Assert.IsNotNull(result);

        }
        [Test()]
        public void getSubTreeNode()
        {
            string callerid = AgencyCode;

            object[] result = _unitUnderTest.getSubTreeNode(AgencyCode, callerid);

            Assert.IsNotNull(result);

        }
        [Test()]
        public void editSubTreeNode()
        {
            string callerid = AgencyCode;
            String REGISTRATION_ENABLED = "REGISTRATION_ENABLED";
            String LOGIN_ENABLED = "LOGIN_ENABLED";


            object[] result = _unitUnderTest.getSubTreeNode(AgencyCode, callerid);
            AcaAdminTreeNodeModel4WS [] nodeArray = new AcaAdminTreeNodeModel4WS[result.Length];
            for (int i = 0; i < nodeArray.Length;i++ )
            {
                nodeArray[i] = (AcaAdminTreeNodeModel4WS)result[i];
                if (nodeArray[i].pageType.Equals("REGISTRATION_ENABLED") || nodeArray[i].pageType.Equals("LOGIN_ENABLED"))
                {
                    nodeArray[i].recStatus = "I";
                    nodeArray[i].nodeDescribe = "OK";
                    nodeArray[i].elementID = null;
                }
                
            }
            _unitUnderTest.editSubTreeNode(AgencyCode, nodeArray);
            Assert.IsNotNull(result);

        }

    }
}