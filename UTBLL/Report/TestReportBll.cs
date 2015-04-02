/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestReportBll.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestReportBll.cs 177624 2010-07-22 02:16:43Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Test.BLL;
using Accela.ACA.WSProxy;
using System;
using NUnit.Framework;
using Accela.Test.Lib;

namespace Accela.ACA.BLL.Report
{
    [TestFixture()]
    public class TestReportBll : TestBase
    {
        private IReportBll _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            //_unitUnderTest = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestHandleReport()
        {
            return;
            ReportInfoModel4WS reportInfoModel4WS = new ReportInfoModel4WS();

            string servProvCode = "SPOKANE";
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.customID = "08243-00000-00058";
            capID.id1 = "08243";
            capID.id2 = "00000";
            capID.id3 = "00058";

            EDMSEntityIdModel4WS edmsEntityID = new EDMSEntityIdModel4WS();
            edmsEntityID.altId = capID.customID;
            edmsEntityID.capId = capID.id1 + "-" + capID.id2 + "-" + capID.id3;

            reportInfoModel4WS.edMSEntityIdModel = edmsEntityID;
            reportInfoModel4WS.servProvCode = servProvCode;
            reportInfoModel4WS.module = "Building";
            reportInfoModel4WS.callerId = "PUBLICUSER5012";
            reportInfoModel4WS.reportId = 1099;
//            string callID = "Hellen";
//            string passWord = "Hellen";
//            string sessionID = new SSOBll().Signon(servProvCode, callID, passWord);

            reportInfoModel4WS.ssOAuthId = "08012410313125868298";

            string reportType = "PRINT_PERMIT_REPORT";
            ReportResultModel4WS result = _unitUnderTest.HandleReport(reportInfoModel4WS, reportType);

            if (result != null && result.content !=null )
            {
                Console.WriteLine("Report Format:" + result.format);
                Console.WriteLine("Result Content:" + result.content.Length);                
            }
        }

        [Test()]
        public void TestGetReportButtonProperty()
        {
            return;
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.customID = "08243-00000-00058";
            capID.id1 = "08243";
            capID.id2 = "00000";
            capID.id3 = "00058";
            capID.serviceProviderCode = "SPOKANE";
            string publicuserID = "PUBLICUSER5012";
            string moduleName = "Building";
            ReportButtonPropertyModel4WS[] reportButtonArray = _unitUnderTest.GetReportButtonProperty(capID, publicuserID, moduleName);
            if (reportButtonArray != null && reportButtonArray.Length>0)
            {
                foreach (ReportButtonPropertyModel4WS ws in reportButtonArray)
                {
                    string buttonName = ws.buttonName;
                    string errorInfo = ws.errorInfo;
                    string reportID = ws.reportId;
                    string reportName = ws.reportName;
                    bool isDisplayed = ws.isDisplayed;
                    Console.WriteLine("ButtonName:" + buttonName + " /ErrorInfo:" + errorInfo + " /ReportID"
                       + reportID +" /ReportName:" + " /isDisplay:" + isDisplayed);
                }
            }
            else
            {
                Console.WriteLine("None Report Button Currently...");
            }
            
        }
    }
}
