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
 * $Id: TestEmailSettingWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestEmailSettingWS : TestBase
    {
        private EmailSettingWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<EmailSettingWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void getContentTemplateByTypeandSPC()
        {
            string templateType = "ASSOCIATING_LICENSE_TO_MULTIPLE_ACCOUNTS";
            object result = _unitUnderTest.getContentTemplateByTypeandSPC(AgencyCode, templateType);

            Assert.IsNotNull(result);

        }

        [Test()]
        public void createContentTemplate()
        {

            string templateType = "ACA_ACTIVATION_EMAIL_SUBJECT";

            ContentTemplateModel4WS contentTemplate = (ContentTemplateModel4WS)_unitUnderTest.getContentTemplateByTypeandSPC(AgencyCode, templateType);

            contentTemplate.contentText = "(ACA)congratulation! your account have been actived.";//(ACA)congratulation! your account have been actived.
       
            _unitUnderTest.createContentTemplate(contentTemplate);

        }

        [Test()]
        public void editContentTemplate()
        {

            string templateType = "ACA_ACTIVATION_EMAIL_SUBJECT";

            ContentTemplateModel4WS contentTemplate = (ContentTemplateModel4WS)_unitUnderTest.getContentTemplateByTypeandSPC(AgencyCode, templateType);

            contentTemplate.contentText = "(ACA)congratulation! your account have been actived.";//(ACA)congratulation! your account have been actived.
       
            _unitUnderTest.editContentTemplate(contentTemplate);

        }

        [Test()]
        public void getBatchContentTemplate()
        {

            string[] templateType = new string[2];
            templateType[0] = "ASSOCIATING_LICENSE_TO_MULTIPLE_ACCOUNTS";
            //templateType[1] = "DEACTIVATE_ACCOUNT";   
            //DEACTIVATE_ACCOUNT
            //VALIDATE_CITIZEN_USER
            //CITIZEN_USER_REGISTRATION

            object[] result = _unitUnderTest.getBatchContentTemplate(AgencyCode, templateType);

            Assert.IsNotNull(result);

        }
        [Test()]
        public void editBatchContentTemplate()
        {
            string callerID = "Admin";

            string[] templateType = new string[1];
            templateType[0] = "ASSOCIATING_LICENSE_TO_MULTIPLE_ACCOUNTS";
            //templateType[1] = "DEACTIVATE_ACCOUNT";
            //DEACTIVATE_ACCOUNT
            //VALIDATE_CITIZEN_USER
            //CITIZEN_USER_REGISTRATION
            GlobalSettingModel4WS[] globalModelArray = new GlobalSettingModel4WS[1];
            object[] result = _unitUnderTest.getBatchContentTemplate(AgencyCode, templateType);
            for (int i = 0; i < result.Length; i++)
            {
                GlobalSettingModel4WS globalModel = (GlobalSettingModel4WS)result[i];
                ContentTemplateModel4WS contentModel = globalModel.contentInfo;
                contentModel.contentText = contentModel.contentText + "Edit by UT";
                globalModel.contentInfo = contentModel;
                BizDomainModel4WS bizDomainModel = globalModel.mailToInfo;

                if (bizDomainModel != null)
                {
                    bizDomainModel.description = "dylan.liang@achievo.com";
                }
                else
                {
                    bizDomainModel = new BizDomainModel4WS();
                    bizDomainModel.serviceProviderCode = AgencyCode;
                    bizDomainModel.auditDate = DateTime.Now.Date.ToString("MM/dd/yyyy HH:mm:ss");
                    bizDomainModel.auditStatus = "A";
                    bizDomainModel.auditID = callerID;
                    bizDomainModel.bizdomain = "ACA_EMAIL_TO_AND_FROM_SETTING";
                    bizDomainModel.bizdomainValue = contentModel.contentType + "_MAILTO";
                    bizDomainModel.description = "dylan.liang@achievo.com";
                }

                bizDomainModel = globalModel.mailFromInfo;

                if (bizDomainModel != null)
                {
                    bizDomainModel.auditStatus = "I";
                }
                else
                {
                    bizDomainModel = new BizDomainModel4WS();
                    bizDomainModel.serviceProviderCode = AgencyCode;
                    bizDomainModel.auditDate = DateTime.Now.Date.ToString("MM/dd/yyyy HH:mm:ss");
                    bizDomainModel.auditStatus = "A";
                    bizDomainModel.auditID = callerID;
                    bizDomainModel.bizdomain = "ACA_EMAIL_TO_AND_FROM_SETTING";
                    bizDomainModel.bizdomainValue = contentModel.contentType + "_MAILFROM";
                    bizDomainModel.description = "wallance.zhang@achievo.com";
                }

                globalModel.mailToInfo = bizDomainModel;
                globalModelArray[i] = globalModel;
            }
            //editBatchContentTemplate(String servProvCode, String callerID, GlobalSettingModel4WS[] globalModelArray)
            _unitUnderTest.editBatchContentTemplate(AgencyCode, callerID, globalModelArray);
           Assert.IsNotNull(result);

        }
      
    }
}