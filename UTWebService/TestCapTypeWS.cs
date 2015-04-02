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
 * $Id: TestCapTypeWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestCapTypeWS : TestBase
    {
        private CapTypeWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<CapTypeWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetCapTypeByPageFlowGroup()
        {
            string groupCode = "Contact";
            string callerID = "ACA ADMIN";
            string moduleName = "Licenses";

            CapTypeModel[] result = _unitUnderTest.getCapTypeList4ACAPageFlowGroup(AgencyCode, moduleName, groupCode, callerID);

            if (result != null && result.Length > 0)
            {
                foreach (CapTypeModel ws in result)
                {
                    StringBuilder outString = new StringBuilder();
                    outString.Append("Service Provider Code: ");
                    outString.Append(ws.serviceProviderCode);
                    outString.Append("| Cap Type:");
                    outString.Append(ws.group);
                    outString.Append("/");
                    outString.Append(ws.type);
                    outString.Append("/");
                    outString.Append(ws.subType);
                    outString.Append("/");
                    outString.Append(ws.category);
                    outString.Append("| Cap Alias:");
                    outString.Append(ws.alias);
                    outString.Append("| udCode3:");
                    outString.Append(ws.udCode3);

                    Console.WriteLine(outString);
                }
            }
            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestGetCapTypeListByModule()
        {
            string servProvCode = AgencyCode;
            string callerID = "ACA Admin";
            string module = "Building";


            CapTypeModel[] capTypeArray = _unitUnderTest.getCapTypeListByModule(servProvCode, module, null, null);

            if (capTypeArray != null && capTypeArray.Length > 0)
            {
                foreach (CapTypeModel ws in capTypeArray)
                {
                    string alias = ws.alias;
                    string group = ws.group;
                    string type = ws.type;
                    string subType = ws.subType;
                    string category = ws.category;
                    string smartChoiceGroup = ws.smartChoiceCode;

                    Console.WriteLine("Cap Name: " + group + "/" + type + "/" + subType + "/" + category + "||" + "Cap Alias: " + alias + "||" + "SmartChoiceGroup:" + smartChoiceGroup);
                }
            }
            else
            {
                Console.WriteLine("cap type array is empty");
            }
        }

        [Test()]
        public void TestGetCapTypeDetailListByModule()
        {
            string servProvCode = AgencyCode;
            string callerID = "ACA Admin";
            string module = "Building";


            CapTypeDetailModel[] capTypeArray = _unitUnderTest.getCapTypeDetailListByModule(servProvCode, module, null);

            if (capTypeArray != null && capTypeArray.Length > 0)
            {
                foreach (CapTypeDetailModel ws in capTypeArray)
                {
                    string alias = ws.alias;
                    string group = ws.group;
                    string type = ws.type;
                    string subType = ws.subType;
                    string category = ws.category;
                    string smartChoiceGroup = ws.smartChoiceCode;
                    string smartChoiceGroup4ACA = ws.smartchoiceCode4ACA;

                    Console.WriteLine("Cap Name: " + group + "/" + type + "/" + subType + "/" + category + "||"
                        + "Cap Alias: " + alias + "||"
                        + "SmartChoiceGroup: " + smartChoiceGroup + "||"
                        + "PageFlowGroup: " + smartChoiceGroup4ACA);
                }
            }
            else
            {
                Console.WriteLine("cap type array is empty");
            }
        }

        //[Test()]
        //public void getCapsTypeNameList()
        //{
        //    string levelType = "Building";

        //    CapTypeModel4WS[] result = _unitUnderTest.getCapTypeList(AgencyCode, levelType, null, true);

        //    Assert.IsNotNull(result);

        //}

        [Test()]
        public void isMatchTheFilter()
        {
            CapTypeModel capTypeModel = new CapTypeModel();

            capTypeModel.group = "Licenses";
            capTypeModel.type = "MYLicense";
            capTypeModel.subType = "MYLicense";
            capTypeModel.category = "MYLicense";

            string moduleNmae = "Licenses";
            string filtername = "TradeName";
            //string filtername = "TRADE_NAME";

            bool result = _unitUnderTest.isMatchTheFilter(AgencyCode, capTypeModel, moduleNmae, filtername);

            Assert.IsTrue(result);

        }

        [Test()]
        public void TestGetCapTypeList4ACAByModule()
        {
            string servProvCode = AgencyCode;
            string callerID = "ACA Admin";
            string module = "Building";


            CapTypeModel[] capTypeArray = _unitUnderTest.getCapTypeList4ACAByModule(servProvCode, module, null);

            if (capTypeArray != null && capTypeArray.Length > 0)
            {
                Console.WriteLine("Cap Type number is: " + capTypeArray.Length);
                foreach (CapTypeModel ws in capTypeArray)
                {
                    string alias = ws.alias;
                    string group = ws.group;
                    string type = ws.type;
                    string subType = ws.subType;
                    string category = ws.category;
                    string smartChoiceGroup4ACA = ws.smartChoiceCode4ACA;
                    string udCode3 = ws.udCode3;

                    if ("UT21292".Equals(smartChoiceGroup4ACA))
                    {

                        Console.WriteLine("Cap Name: " + group + "/" + type + "/" + subType + "/" + category + "||Cap Alias: " + alias + "||SmartChoiceGroup for ACA :" + smartChoiceGroup4ACA + "|| udCode3" + udCode3);

                    }
                }
            }
            else
            {
                Console.WriteLine("cap type array is empty");
            }
        }

        [Test()]
        public void TestGetCapTypeListByFilter()
        {
            string servProvCode =AgencyCode;
            string callerID = "ACA Admin";
            string module = "Building";
            string filterName = "FILTER2";
            string vchType = "EST";

            //CapTypeModel4WS[] capTypeArray = _unitUnderTest.getCapTypeListByFilter(servProvCode, module, filterName, vchType, "",callerID, "3");

            //if (capTypeArray != null && capTypeArray.Length > 0)
            //{
            //    foreach (CapTypeModel4WS ws in capTypeArray)
            //    {
            //        string alias = ws.alias;
            //        string group = ws.group;
            //        string type = ws.type;
            //        string subType = ws.subType;
            //        string category = ws.category;
            //        string filter = ws.filterName;

            //        Console.WriteLine("Cap Name: " + group + "/" + type + "/" + subType + "/" + category + "||"
            //            + "Cap Alias: " + alias + "||"
            //            + "filterName: " + filter);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("cap type array is empty");
            //}

        }

        [Test()]
        public void TestGetButtonSetting4CapType()
        {
            string callerID = "ACA Admin";
            string module = "Building";

            ButtonSettingModel4WS btnSetting4WS = new ButtonSettingModel4WS();
            btnSetting4WS.serviceProviderCode = AgencyCode;
            btnSetting4WS.moduleName = module;
            btnSetting4WS.buttonName = ACAConstant.BUTTON_CREATE_AMENDMENT;

            //ButtonSettingModel4WS resultBtnSetting = _unitUnderTest.getButtonSetting4CapType(btnSetting4WS, callerID);

            //CapTypeModel4WS[] availableCapTypes = resultBtnSetting.availableCapTypeList;
            //CapTypeModel4WS[] selectedCapTypes = resultBtnSetting.selectedCapTypeList;
        }

        [Test()]
        public void TestUpdateButtonSetting4CapType()
        {
            string callerID = "ACA Admin";
            string module = "Building";

            ButtonSettingModel4WS btnSetting4WS = new ButtonSettingModel4WS();
            btnSetting4WS.serviceProviderCode = AgencyCode;
            btnSetting4WS.moduleName = module;
            btnSetting4WS.buttonName = ACAConstant.BUTTON_CREATE_AMENDMENT;

            CapTypeModel capType = new CapTypeModel();
            capType.alias = "Building/Building Permit/New Commercial/na";
            CapTypeModel capType2 = new CapTypeModel();
            capType2.alias = "Building/may/1031/fee calc";

            btnSetting4WS.selectedCapTypeList = new CapTypeModel[] { capType, capType2 };

            //_unitUnderTest.updateButtonSetting4CapType(btnSetting4WS, callerID);
        }
    }
}