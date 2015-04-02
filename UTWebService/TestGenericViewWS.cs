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
 * $Id: TestGenericViewWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.Test.Lib;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestGenericViewWS : TestBase
    {
        private GFilterViewWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<GFilterViewWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void updateXFilterViewElement()
        {
            //String servProvCode,String levelType,String moduleName,String viewID
            string levelType = "MODULE";
            string moduleName = "Parcel";
            string viewID = "60002";
            string callerId = "Admin";
            object[] result = _unitUnderTest.getSimpleViewElementModel(AgencyCode, levelType, moduleName, viewID);
            Boolean flag = false;
            if (result.Length >0)
            {
                SimpleViewElementModel4WS[] elementArray = new SimpleViewElementModel4WS[result.Length];
                for (int i = 0; i < result.Length;i++ )
                {
                    SimpleViewElementModel4WS simple = (SimpleViewElementModel4WS)result[i];
                    //simple.m_recStatus = "I";
                    elementArray[i] = simple;
                }                
                //String servProvCode,String levelType,String moduleName,String viewID,String callerId,SimpleViewElementModel4WS[] elementArray

                //flag = _unitUnderTest.updateXFilterViewElement(AgencyCode, levelType, moduleName, viewID, callerId, elementArray);
            }
            Assert.IsTrue(flag);

        }
        [Test()]
        public void getSimpleViewElementModel()
        {
            //String servProvCode,String levelType,String moduleName,String viewID
            string levelType = "MODULE";
            string moduleName = "Address";
            string viewID = "60001";
            string callerId = "Admin";
            object[] result = _unitUnderTest.getSimpleViewElementModel(AgencyCode, levelType, moduleName, viewID);
            Assert.IsNotNull(result);
        }
        
        [Test()]
        public void updateApplicantDisplayRule()
        {
            AdminConfigurationModel4WS adminConfigurationModel4WS = new AdminConfigurationModel4WS();
            BizDomainModel4WS bizDomainModel4WS = new BizDomainModel4WS();

            bizDomainModel4WS.serviceProviderCode = AgencyCode;
            bizDomainModel4WS.bizdomain = "ACA_APPLICANT_DISPLAY_RULE";
            bizDomainModel4WS.bizdomainValue = "License";
            bizDomainModel4WS.description = "2";

            //adminConfigurationModel4WS.applicantDisplayRule = bizDomainModel4WS;
            adminConfigurationModel4WS.servProvCode = AgencyCode;
            adminConfigurationModel4WS.levelType = "MODULE";
            adminConfigurationModel4WS.moduleName = "Building";
            adminConfigurationModel4WS.callerId = "ACA Admin";

            _unitUnderTest.saveAdminConfigurationData(adminConfigurationModel4WS);
        }
        [Test()]
        public void saveAdminConfigurationData()
        {
            
            //string viewID = "60001";            
            //SectionFieldsModel4WS[] sectionFiledModel4WS = new SectionFieldsModel4WS[0];
            //GUITextModel4WS[] labelModelArray = new GUITextModel4WS[0];
            //BizDomainModel4WS[] standardChoice4HardcodeArray = new BizDomainModel4WS[0];
            //BizDomainModel4WS[] standardChoiceArray = new BizDomainModel4WS[2];
            //BizDomainModel4WS one = new BizDomainModel4WS();
            //one.bizdomain = "TEMP_TEST";
            //one.bizdomainValue = "oneqqqqq";

            //BizDomainModel4WS two = new BizDomainModel4WS();
            //two.bizdomain = "TEMP_TEST";
            //two.bizdomainValue = "twoqqqqq";

            //standardChoiceArray[0] = one;
            //standardChoiceArray[1] = two;
            //string levelType = "MODULE";
            //string moduleName = "Address";
            //string callerId = "Admin";
            //string[] tabsOrder = null;

            
           
        }
       
        [Test()]
        public void updateXuiText()
        {

            //string viewID = "60001";
            //SectionFieldsModel4WS[] sectionFiledModel4WS = new SectionFieldsModel4WS[0];
            //XUITextModel4WS[] labelModelArray = new XUITextModel4WS[3];
            //BizDomainModel4WS[] standardChoice4HardcodeArray = new BizDomainModel4WS[0];
            //BizDomainModel4WS[] standardChoiceArray = new BizDomainModel4WS[0];
            //XUITextModel4WS oneText = new XUITextModel4WS();
            //oneText.stringKey = "acc_contactForm_label_state";
            //oneText.stringValue = "Testss 22222222222222";
            //oneText.textLevelName = "AMS";

            //XUITextModel4WS twoText = new XUITextModel4WS();
            //twoText.stringKey = "acc_contactForm_label_state|sub";
            //twoText.stringValue = "Sub Test Title222222222222";
            //twoText.textLevelName = "AMS";

            //XUITextModel4WS threeText = new XUITextModel4WS();
            //threeText.stringKey = "acc_contactForm_label_contactMethod";
            //threeText.stringValue = "Test Preferred Method of Contact:22222222222222";
            //threeText.textLevelName = "AMS";

            //labelModelArray[0] = oneText;
            //labelModelArray[1] = twoText;
            //labelModelArray[2] = threeText;
            //string serviceProviderCode = AgencyCode;
            //string levelType = "MODULE";
            //string moduleName = "Building";
            //string callerId = "Admin";
            //string[] tabsOrder = null;


            //labelModelArray[0] = threeText;
            //AdminConfigurationModel4WS adminConfigurationModel4WS = new AdminConfigurationModel4WS();
            //adminConfigurationModel4WS.labelModelArray = labelModelArray;

            //_unitUnderTest.saveAdminConfigurationData(adminConfigurationModel4WS);
            
        }

    }
}