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
 * $Id: TestInsepctionTypeWS.cs 182420 2010-10-14 09:35:33Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using NUnit.Framework;

using Accela.ACA.BLL;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestInsepctionTypeWS : TestBase
    {
        private InspectionTypeWebServiceService _insepctionTypeWS;

        [SetUp()]
        public void SetUp()
        {
            _insepctionTypeWS = WSFactory.Instance.GetWebService<InspectionTypeWebServiceService>();
        }

        [Test()]
        public void TestgetInspectionTypesByCapType()
        {
            CapTypeModel capTypeModel = new CapTypeModel();
            capTypeModel.serviceProviderCode = AgencyCode;
            capTypeModel.group = "Building";
            capTypeModel.type = "wallance";
            capTypeModel.subType = "wallance";
            capTypeModel.category = "APO";

            QueryFormat queryFormat = null;
            CapIDModel capId = new CapIDModel();
            capId.ID1 = "10CAP";
            capId.ID2 = "00000";
            capId.ID3 = "000A1";
            capId.serviceProviderCode = AgencyCode;

            InspectionTypeModel[] inspectionTypes = _insepctionTypeWS.getInspectionTypesByCapType(capTypeModel, capId, queryFormat, CallerID);

            Assert.IsNotNull(inspectionTypes);
            Assert.AreEqual(4, inspectionTypes.Length);
        }

        [Test()]
        public void TestgetInspectionResultByGroupName()
        {
            string serviceProviderCode = AgencyCode;
            string groupName = "Building";

            InspectionResultModel[] inspectionTypes = _insepctionTypeWS.getInspectionResultByGroupName(serviceProviderCode, groupName);

            Assert.IsNotNull(inspectionTypes);
            Assert.AreEqual(4, inspectionTypes.Length);
        }
    }
}