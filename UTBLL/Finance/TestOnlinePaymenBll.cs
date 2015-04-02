/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestOnlinePaymenBll.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestOnlinePaymenBll.cs 183331 2010-10-29 07:40:02Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  05/15/2007     		ken.huang				Initial.
 * </pre>
 */

using System;

using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Test.BLL;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;

namespace ACA.BLL.fee
{
    using NUnit.Framework;


    [TestFixture()]
    public class TestOnlinePaymenBll : TestBase
    {
        private IOnlinePaymenBll _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = (IOnlinePaymenBll)ObjectFactory.GetObject(typeof(IOnlinePaymenBll)); 
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test]
        public void TestgetReceiptNoByCapID()
        {
            return;
            CapIDModel4WS capID4ws = new CapIDModel4WS();
            capID4ws.serviceProviderCode = "xxx";
            capID4ws.id1 = "07BLD";
            capID4ws.id2 = "00000";
            capID4ws.id3 = "00180";

            QueryFormat4WS qf = null;
            String callID = "admin";
            String receiptNo = _unitUnderTest.GetReceiptNoByCapID(capID4ws, qf, callID);
            Assert.IsNotNull(receiptNo);
        }

        //[Test]
        //public void TestCreateCapByCheckPayment4ACA()
        //{
        //    return;
        //    string agencyCode = "SPOKANE";
        //    string module = "Building";
        //    string callerID = "Admin";

        //    CapModel4WS cap = new CapModel4WS();
        //    CapIDModel4WS capID = new CapIDModel4WS();
        //    capID.serviceProviderCode = agencyCode;
        //    capID.customID = "07527-00000-00044";
        //    capID.ID1 = "07527";
        //    capID.ID2 = "00000";
        //    capID.ID3 = "00044";
        //    cap.capID = capID;
            

        //    CheckModel4WS checkModel = new CheckModel4WS();
        //    checkModel.module = module;
        //    checkModel.callerID = callerID;
        //    checkModel.servProvCode = agencyCode;
        //    checkModel.pos = false;

        //    ACAModel4WS acaModel = new ACAModel4WS();
        //    acaModel.callerID = callerID;
        //    acaModel.servProvCode = agencyCode;
        //    acaModel.strAction = "";
        //    acaModel.isACA = "Yes";
        //    acaModel.module = module;

        //    PaymentModel4WS payment = new PaymentModel4WS();
        //    double dAmount = 10.0;
        //    string cashierID = "System";
        //    string terminal = "";
        //    string comment = "";
        //    double dAccelaFee = 11.00;
        //    payment.capID = capID;
        //    payment.paymentMethod = "Check";
        //    payment.paymentRefNbr = "";
        //    payment.ccExpDate = DateTime.Now.Date.ToString("MM/dd/yyyy HH:mm:ss");
        //    payment.payee="";
        //    payment.paymentDate = DateTime.Now.Date.ToString("MM/dd/yyyy HH:mm:ss");
        //    payment.paymentAmount = dAmount;
        //    payment.paymentChange = 0;
        //    payment.amountNotAllocated = dAmount;
        //    payment.paymentStatus = "Paid";

        //    payment.cashierID = cashierID;
        //    payment.registerNbr = terminal;
        //    payment.paymentComment = comment;
        //    payment.processingFee = dAccelaFee;

        //    string wotModelSeq = "";
        //    CapModel4WS newcap = _unitUnderTest..CreateCapByCheckPayment4ACA(checkModel, acaModel, payment, cap, callerID, wotModelSeq);

        //}
    }
}
