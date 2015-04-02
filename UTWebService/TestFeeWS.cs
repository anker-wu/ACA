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
 * $Id: TestFeeWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestFeeWS : TestBase
    {
        private FeeWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<FeeWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetFeeItemsByCapID()
        {
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;
            capID.id1 = "08999";
            capID.id2 = "00000";
            capID.id3 = "00023";
            capID.customID = "08999-00000-00023";
//            capID.trackingID = 0;

            string callerID = "PUBLICUSER8573";

            F4FeeModel4WS[] resultArray = _unitUnderTest.getFeeItemsByCapID(capID,callerID);

            foreach (F4FeeModel4WS ws in resultArray)
            {
                F4FeeItemModel4WS f4FeeItemModel = ws.f4FeeItemModel;
                X4FeeItemInvoiceModel4WS x4FeeItemInvoiceModel = ws.x4FeeItemInvoiceModel;

            }

            Assert.IsNotNull(resultArray);
        }

        [Test()]
        public void TestGetNoPaidFeeItemByCapID()
        {
         

            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;
            capID.id1 = "08999";
            capID.id2 = "00000";
            capID.id3 = "00023";
            capID.customID = "08999-00000-00023";
            //            capID.trackingID = 0;

            string callerID = "PUBLICUSER8573";

            F4FeeModel4WS[] resultArray = _unitUnderTest.getNoPaidFeeItemByCapID(capID, callerID);

            foreach (F4FeeModel4WS ws in resultArray)
            {
                F4FeeItemModel4WS f4FeeItemModel = ws.f4FeeItemModel;
                X4FeeItemInvoiceModel4WS x4FeeItemInvoiceModel = ws.x4FeeItemInvoiceModel;
            }

            Assert.IsNotNull(resultArray);
        }

        [Test()]
        public void TestGetPaidFeeItemByCapID()
        {
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;
            capID.id1 = "08999";
            capID.id2 = "00000";
            capID.id3 = "00049";

            capID.customID = "08999-00000-00049";
            //            capID.trackingID = 0;

            string callerID = "PUBLICUSER8573";

            F4FeeModel4WS[] resultArray = _unitUnderTest.getPaidFeeItemByCapID(capID, callerID);

            foreach (F4FeeModel4WS ws in resultArray)
            {
                F4FeeItemModel4WS f4FeeItemModel = ws.f4FeeItemModel;
                X4FeeItemInvoiceModel4WS x4FeeItemInvoiceModel = ws.x4FeeItemInvoiceModel;
                long receiptNbr = ws.receiptNbr;
                Console.WriteLine("Receipt Number " + receiptNbr);
            }           

            Assert.IsNotNull(resultArray);
        }
    }
}
