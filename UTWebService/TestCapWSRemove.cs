/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestCapWSRemove.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestCapWSRemove.cs 177623 2010-07-22 02:14:46Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Text;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestCapWSRemove : TestBase
    {
        private CapWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<CapWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetCapList4ACA()
        {
            CapModel4WS capModel= new CapModel4WS();
            capModel.moduleName = "Building";
            capModel.altID = "10123-00000-00005";

            //10123-00000-00005 2010-07-28 14:29:50.0   Sun Jan 31 00:00:00 CST 2010

            SimpleCapModel[] result = _unitUnderTest.getCapList4ACA(AgencyCode, capModel, null, "9234", null, false, null);

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].altID == "10123-00000-00005")
                {
                    Assert.IsTrue(result[i].fileDate.Value.Year == 2010);
                    Assert.IsTrue(result[i].fileDate.Value.Month == 7);
                    Assert.IsTrue(result[i].fileDate.Value.Day == 28);
                    Assert.IsTrue(result[i].fileDate.Value.Hour == 14);
                    Assert.IsTrue(result[i].fileDate.Value.Minute == 29);
                    Assert.IsTrue(result[i].fileDate.Value.Second == 50.0);

                    Assert.IsTrue(result[i].expDate.Value.Year == 2010);
                    Assert.IsTrue(result[i].expDate.Value.Month == 1);
                    Assert.IsTrue(result[i].expDate.Value.Day == 31);
                    Assert.IsTrue(result[i].expDate.Value.Hour == 0);
                    Assert.IsTrue(result[i].expDate.Value.Minute == 0);
                    Assert.IsTrue(result[i].expDate.Value.Second == 0);
                }
            }
        }

        [Test()]
        public void TestMyCapList4ACA()
        {
            CapModel4WS capModel = new CapModel4WS();
            capModel.moduleName = "Building";
            capModel.altID = "10123-00000-00005";

            //10123-00000-00005 2010-07-28 14:29:50.0   Sun Jan 31 00:00:00 CST 2010

            SimpleCapModel[] result = _unitUnderTest.getMyCapList4ACA(AgencyCode, capModel, null, "9234", null);

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].altID == "10123-00000-00005")
                {
                    Assert.IsTrue(result[i].fileDate.Value.Year == 2010);
                    Assert.IsTrue(result[i].fileDate.Value.Month == 7);
                    Assert.IsTrue(result[i].fileDate.Value.Day == 28);
                    Assert.IsTrue(result[i].fileDate.Value.Hour == 14);
                    Assert.IsTrue(result[i].fileDate.Value.Minute == 29);
                    Assert.IsTrue(result[i].fileDate.Value.Second == 50.0);

                    Assert.IsTrue(result[i].expDate.Value.Year == 2010);
                    Assert.IsTrue(result[i].expDate.Value.Month == 1);
                    Assert.IsTrue(result[i].expDate.Value.Day == 31);
                    Assert.IsTrue(result[i].expDate.Value.Hour == 0);
                    Assert.IsTrue(result[i].expDate.Value.Minute == 0);
                    Assert.IsTrue(result[i].expDate.Value.Second == 0);
                }
            }
        }

        [Test()]
        public void TestCapsByRefObjectID()
        {
            CapModel4WS capModel = new CapModel4WS();
            capModel.moduleName = "Building";
            //capModel.altID = "10123-00000-00005";
            LicenseProfessionalModel4WS licenseProfessionalModel = new LicenseProfessionalModel4WS();
            licenseProfessionalModel.licenseNbr = "00000";
            licenseProfessionalModel.licenseType = "ARCHITECT";

            capModel.licenseProfessionalModel = licenseProfessionalModel;
            //10123-00000-00005 2010-07-28 14:29:50.0   Sun Jan 31 00:00:00 CST 2010 02/24/2010

            SimpleCapModel[] result = _unitUnderTest.getCapsByRefObjectID(AgencyCode, capModel, "PUBLICUSER9234", false, null);

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].altID == "10AND-00000-00010")
                {
                    Assert.IsTrue(result[i].fileDate.Value.Year == 2010);
                    Assert.IsTrue(result[i].fileDate.Value.Month == 2);
                    Assert.IsTrue(result[i].fileDate.Value.Day == 24);

                    Assert.IsTrue(result[i].expDate == null);
                }
            }
        }

        [Test()]
        public void TestCapsByConditionWithCapStyle()
        {
            CapModel4WS capModel = new CapModel4WS();
            capModel.moduleName = "Building";
            capModel.altID = "10123-00000-00005";

            //10123-00000-00005 2010-07-28 14:29:50.0   Sun Jan 31 00:00:00 CST 2010

            SimpleCapModel[] result = _unitUnderTest.getCapsByConditionWithCapStyle(AgencyCode, capModel, null, "9234", null, false ,null);

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].altID == "10123-00000-00005")
                {
                    Assert.IsTrue(result[i].fileDate.Value.Year == 2010);
                    Assert.IsTrue(result[i].fileDate.Value.Month == 7);
                    Assert.IsTrue(result[i].fileDate.Value.Day == 28);
                    Assert.IsTrue(result[i].fileDate.Value.Hour == 14);
                    Assert.IsTrue(result[i].fileDate.Value.Minute == 29);
                    Assert.IsTrue(result[i].fileDate.Value.Second == 50.0);

                    Assert.IsTrue(result[i].expDate.Value.Year == 2010);
                    Assert.IsTrue(result[i].expDate.Value.Month == 1);
                    Assert.IsTrue(result[i].expDate.Value.Day == 31);
                    Assert.IsTrue(result[i].expDate.Value.Hour == 0);
                    Assert.IsTrue(result[i].expDate.Value.Minute == 0);
                    Assert.IsTrue(result[i].expDate.Value.Second == 0);
                }
            }
        }

    }
}
