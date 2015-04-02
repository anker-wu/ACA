/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestReportWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestReportWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestReportWS : TestBase
    {
        private ReportWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<ReportWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetReportButtonProperty()
        {
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;
            capID.id1 = "06AND";
            capID.id2 = "00000";
            capID.id3 = "00089";
            capID.customID = "06AND-00000-00089";
            ReportButtonInfoModel4WS reportButtonInfo4WS = new ReportButtonInfoModel4WS();
            reportButtonInfo4WS.capID = capID;
            reportButtonInfo4WS.currentModule = "Building";
            reportButtonInfo4WS.callerID = "donny";
            //ReportButtonPropertyModel4WS[] arrayRBPModel4WS = _unitUnderTest.getReportButtonProperty(reportButtonInfo4WS);
            //if (arrayRBPModel4WS != null && arrayRBPModel4WS.Length > 0)
            //{
            //    foreach (ReportButtonPropertyModel4WS ws in arrayRBPModel4WS)
            //    {
            //        string buttonName = ws.buttonName;
            //        string reportName = ws.reportName;
            //        string reportID = ws.reportId;
            //        string errorInfo = ws.errorInfo;
            //        bool isDisplayed = ws.isDisplayed;
            //        Console.WriteLine("button name:>>" + buttonName + "/report name:>>" + reportName + "/report ID:>>"
            //            + reportID + "/error info:>>" + errorInfo + "/isdisplayed:>>" + isDisplayed);
            //    }

            //}
            //else
            //{
            //    Console.WriteLine("Array of report button model is empty.");
            //}
        }
    }
}
