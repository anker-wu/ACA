/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestTemplateWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestTemplateWS.cs 183331 2010-10-29 07:40:02Z ACHIEVO\xinter.peng $.
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
    public class TestTemplateWS : TestBase
    {
        private TemplateWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<TemplateWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetAPOAttributesFromTemplate()
        {
            string templateGenus = "APO";
            string templateType = "CAP_ADDRESS";
            string callerID = "ADMIN";
            TemplateAttributeModel[] result =
                _unitUnderTest.getAttributes(AgencyCode, templateGenus, templateType, null, callerID);

            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestGetAPOAttributesFromRefAPO()
        {
            string templateGenus = "APO";
            string templateType = "CAP_ADDRESS";
            string templateRefNum = "1501044";
            string callerID = "ADMIN";
            TemplateAttributeModel[] result =
                _unitUnderTest.getAttributes(AgencyCode, templateGenus, templateType, templateRefNum, callerID);

            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestGetAPOEditAttributesFromDailyAPO()
        {
            string templateGenus = "APO";
            string templateType = "CAP_ADDRESS";
            CapIDModel4WS capIDModel = new CapIDModel4WS();
            capIDModel.serviceProviderCode = AgencyCode;
            capIDModel.id1 = "07555";
            capIDModel.id2 = "00000";
            capIDModel.id3 = "00001";

            string templateObjectID = "38098";
            string callerID = "ADMIN";
            TemplateAttributeModel[] result =
                _unitUnderTest.getEditAttributes(AgencyCode, templateGenus, templateType, capIDModel, templateObjectID, callerID);

            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestGetPeopleAttributesFromTemplate()
        {
            string templateGenus = "People";
            string templateType = "MECHANICAL";
            string callerID = "ADMIN";
            TemplateAttributeModel[] result =
                _unitUnderTest.getAttributes(AgencyCode, templateGenus, templateType, null, callerID);

            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestGetPeopleAttributesFromRefAPO()
        {
            string templateGenus = "People";
            string templateType = "MECHANICAL";
            string templateRefNum = "6557";
            string callerID = "ADMIN";
            TemplateAttributeModel[] result =
                _unitUnderTest.getAttributes(AgencyCode, templateGenus, templateType, templateRefNum, callerID);

            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestGetPeopleEditAttributesFromDailyAPO()
        {
            string templateGenus = "People";
            string templateType = "MECHANICAL";
            CapIDModel4WS capIDModel = new CapIDModel4WS();
            capIDModel.serviceProviderCode = AgencyCode;
            capIDModel.id1 = "07555";
            capIDModel.id2 = "00000";
            capIDModel.id3 = "00003";

            string templateObjectID = "ACHIEVO";
            string callerID = "ADMIN";
            TemplateAttributeModel[] result =
                _unitUnderTest.getEditAttributes(AgencyCode, templateGenus, templateType, capIDModel, templateObjectID, callerID);

            Assert.IsNotNull(result);
        }
    }
}