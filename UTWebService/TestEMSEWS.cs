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
 * $Id: TestEMSEWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.Test.WebService;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestEMSEWS : TestBase
    {
        private EMSEWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<EMSEWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestCreateOrUpdateEventScriptCode()
        {
            string eventName = "EventUpdateAfter";
            string scriptCode = "88888888";
            string callerID = "ACA Auto UT";
            _unitUnderTest.createOrUpdateEventScriptCode(AgencyCode, eventName, scriptCode, callerID);
        }

        [Test()]
        public void TestGetEventScriptByPK()
        {
            string servProvCode = AgencyCode;
            string eventName = "EventUpdateAfter";
            string callerID = "ACA Auto UT";
            string scriptCode = _unitUnderTest.getEventScriptByPK(servProvCode, eventName, callerID);

            Console.WriteLine("Script Code: " + scriptCode);
        }

        [Test()]
        public void TestTriggerFeeEstimateAfter4ACAEvent()
        {
            CapModel4WS cap = new CapModel4WS();
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.customID = "EST-08EST-00000-00095";
            capID.serviceProviderCode = AgencyCode;            
            capID.id1 = "08120";
            capID.id2 = "00000";
            capID.id3 = "00004";
            cap.capID = capID;
            string callerID = "ACA Auto UT";

            _unitUnderTest.triggerFeeEstimateAfter4ACAEvent(cap,callerID);
        }
    }
}
