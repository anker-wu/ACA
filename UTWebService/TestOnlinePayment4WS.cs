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
 * $Id: TestOnlinePayment4WS.cs 183331 2010-10-29 07:40:02Z ACHIEVO\xinter.peng $.
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
    public class TestOnlinePayment4WS : TestBase
    {
        private OnlinePaymentWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<OnlinePaymentWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        public void TestCheckPayment4ExistingCap4ACA()
        {
            //            _unitUnderTest.checkPayment4ExistingCap4ACA();
        }

        [Test]
        public void TestgetReceiptNoByCapID()
        {
            CapIDModel4WS capID4ws = new CapIDModel4WS();
            capID4ws.serviceProviderCode = AgencyCode;
            capID4ws.id1 = "08999";
            capID4ws.id2 = "00000";
            capID4ws.id3 = "00023";

            QueryFormat4WS qf = null;
            String callID = "admin";
            String receiptNo = _unitUnderTest.getReceiptNoByCapID(capID4ws, qf, callID);
            Assert.IsNotNull(receiptNo);
        }

        [Test]
        public void TestCompleteGenericPaymentForTrustAccountDepositByCreditCard()
        {
            int caseNo = 1;
            CreditCardModel4WS ccModel4ws = (CreditCardModel4WS)base.Deserialize(caseNo, typeof(CreditCardModel4WS), "ccModel4ws");
            CheckModel4WS checkModel4ws = (CheckModel4WS)base.Deserialize(caseNo, typeof(CheckModel4WS), "checkModel4ws");

            string entityID = (string)base.Deserialize(caseNo, typeof(string), "entityID");
            string entityType = (string)base.Deserialize(caseNo, typeof(string), "entityType");
            string paremeters = (string)base.Deserialize(caseNo, typeof(string), "paremeters");
            string paymentProvider = (string)base.Deserialize(caseNo, typeof(string), "paymentProvider"); ;

            //base.Serialize(1, GetCreditCardModel(), "ccModel4ws");
            //base.Serialize(1, GetCheckModel(), "checkModel4ws");
            //base.Serialize(1, entityID , "entityID");
            //base.Serialize(1, entityType, "entityType");
            //base.Serialize(1, paremeters, "paremeters");
            //base.Serialize(1, paymentProvider, "paymentProvider");


            OnlinePaymentResultModel result = _unitUnderTest.completeGenericPayment(ccModel4ws, checkModel4ws, entityID, entityType, paremeters, paymentProvider, CallerID);
        }

        [Test]
        public void TestCompleteGenericPaymentForTrustAccountDepositByCheck()
        {
            CreditCardModel4WS ccModel4ws = GetCreditCardModel();
            CheckModel4WS checkModel4ws = GetCheckModel();

            string entityID = "DemoTrustAcc002";
            string entityType = "TrustAccount";
            string paremeters = "ServProvCode=RENO&PaymentMethod=Check&TOTAL_FEE=100";
            string paymentProvider = "PayPal43_Test";

            OnlinePaymentResultModel result = _unitUnderTest.completeGenericPayment(ccModel4ws, checkModel4ws, entityID, entityType, paremeters, paymentProvider, CallerID);
        }

        [Test]
        public void TestCompletePayment()
        {
            CreditCardModel4WS ccModel4ws = GetCreditCardModel();
            CheckModel4WS checkModel4ws = GetCheckModel();
            
            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = AgencyCode;
            capID.customID = "07527-00000-00044";
            capID.ID1 = "07527";
            capID.ID2 = "00000";
            capID.ID3 = "00044";

            CapIDModel[] capIDList = new CapIDModel[] { capID };

            string paremeters = "ServProvCode=RENO&PaymentMethod=Credit Card&TOTAL_FEE=100";
            string paymentProvider = "PayPal43_Test";

            OnlinePaymentResultModel result = _unitUnderTest.completePayment(ccModel4ws, checkModel4ws, capIDList, paremeters, paymentProvider, CallerID);

        }

        /// <summary>
        /// Construct Demo CreditCardModel for Unit Testing.
        /// </summary>
        /// <returns></returns>
        private CreditCardModel4WS GetCreditCardModel()
        {
            CreditCardModel4WS ccModel4ws = new CreditCardModel4WS();
            ccModel4ws.servProvCode = AgencyCode;
            ccModel4ws.callerID = CallerID;
            ccModel4ws.accountNumber = "4012888888881881";
            ccModel4ws.balance = "30";
            ccModel4ws.accelaFee = "20";
            ccModel4ws.posTransSeq = "10000";
            ccModel4ws.expirationDate = "15";
            ccModel4ws.expirationMonth = "12";
            ccModel4ws.expirationYear = "2020";
            ccModel4ws.cardType = "Visa";
            ccModel4ws.holderName = "John Smith";
            ccModel4ws.businessName = "Achievo";
            ccModel4ws.streetAddress = "1000 Broad Street";
            ccModel4ws.city = "New Orleans";
            ccModel4ws.state = "LA";
            ccModel4ws.postalCode = "70119";
            ccModel4ws.email = "dylan.liang@achievo.com";
            ccModel4ws.securityCode = "5689";

            return ccModel4ws;
        }


        /// <summary>
        /// Construct Demo CheckModel for Unit Testing.
        /// </summary>
        /// <returns></returns>
        private CheckModel4WS GetCheckModel()
        {
            CheckModel4WS checkModel = new CheckModel4WS();
            checkModel.servProvCode = AgencyCode;
            checkModel.callerID = CallerID;
            checkModel.module = "Building";
            checkModel.balance = "50";
            checkModel.accelaFee = "10";
            checkModel.pos = false;
            checkModel.routingNbr = "1234";
            checkModel.accountNbr = "56780439085000";
            checkModel.checkNbr = "1001";
            checkModel.licenseNbr = "CA123456";
            checkModel.ssNbr = "";
            checkModel.name = "Sally";
            checkModel.state = "CA";
            checkModel.city = "Buffalos";
            checkModel.streetAddress = "1234Main";
            checkModel.postalCode = "95050";
            checkModel.email = "dylan.liang@achievo.com";
            checkModel.phoneNbr = "(504) 654-3210";

            return checkModel;
        }
    }
}
