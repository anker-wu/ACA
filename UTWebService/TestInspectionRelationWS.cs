/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestCapWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestParcelWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestInspectionRelationWS : TestBase
    {
        private InspectionRelationWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<InspectionRelationWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }


        [Test()]
        public void TestGetRelatedInspection()
        {
            CapIDModel capID = new CapIDModel();
            string serviceProviderCode = "ADDEV";

            capID.serviceProviderCode = serviceProviderCode;
		    capID.ID1 = "10CAP";
		    capID.ID2 = "00000";
		    capID.ID3 = "000BA";

            long inspectionID = 64903;

            InspectionTreeNodeModel ispectionTreeNode = _unitUnderTest.getRelatedInspections(capID, inspectionID);
            Assert.IsNotNull(ispectionTreeNode);
            Assert.IsNotNull(ispectionTreeNode.inspectionModel);
            Assert.AreEqual(1, ispectionTreeNode.children.Length);
            Assert.IsNotNull(ispectionTreeNode.children[0].children);
            Assert.AreEqual(1, ispectionTreeNode.children[0].children.Length);

        }
    }
}