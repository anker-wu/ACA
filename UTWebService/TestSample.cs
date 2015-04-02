#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TestSample.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *  UT cases sample
 *
 *  Notes:
 * $Id: SerializationUtil.cs 179604 2010-08-24 01:00:45Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using NUnit.Framework;
using Accela.Test.Lib;
using System.Collections;

namespace Accela.ACA.Test
{
    /// <summary>
    /// TestSample
    /// </summary>
    class TestSample : TestBase
    {
        [Test()]
        public void TestSampleMethod()
        {
            //int caseNo = 1;
            //string[] agencys = new String[2];
            //agencys[0] = "DG";
            //agencys[1] = "SZ";
            //int age = 100;
            //string name = "John";
            //CreditCardModel4WS ccm = GetCreditCardModel();

            //base.Serialize(caseNo,agencys,  "agencys");
            //base.Serialize(caseNo,age,  "age");
            //base.Serialize(caseNo,name,  "name");
            //base.Serialize(caseNo,ccm,  "CreditCardModel4WS");

            //caseNo = 2;
            //agencys[0] = "OREGON";
            //agencys[1] = "RENO";
            //age = 101;
            //name = "Tom";
            //ccm = GetCreditCardModel();

            //base.Serialize(caseNo,agencys,  "agencys");
            //base.Serialize(caseNo,age,  "age");
            //base.Serialize(caseNo,name,  "name");
            //base.Serialize(caseNo,ccm,  "CreditCardModel4WS");

            //----------- CASE #1 Start -----------
            int caseNo = 1;
            string[] agencys = (string[])base.Deserialize(caseNo, typeof(string[]), "agencys");
            int age = (int)base.Deserialize(caseNo, typeof(int), "age");
            string name = (string)base.Deserialize(caseNo, typeof(string), "name");
            CreditCardModel4WS ccm = (CreditCardModel4WS)base.Deserialize(caseNo, typeof(CreditCardModel4WS), "CreditCardModel4WS");

            // invoke method for case 1 test
            Assert.IsTrue( IsCardNameMatch(agencys,age,name,ccm));

            //----------- CASE #1 End -----------

            //----------- CASE #2 Start -----------
            caseNo = 2;
            agencys = (string[])base.Deserialize(caseNo, typeof(string[]), "agencys");
            age = (int)base.Deserialize(caseNo, typeof(int), "age");
            name = (string)base.Deserialize(caseNo, typeof(string), "name");
            ccm = (CreditCardModel4WS)base.Deserialize(caseNo, typeof(CreditCardModel4WS), "CreditCardModel4WS");

            // invoke method for case 2 test
            Assert.IsFalse(IsCardNameMatch(agencys, age, name, ccm));

            //----------- CASE #2 End -----------
        }

        //private CreditCardModel4WS GetCreditCardModel()
        //{
        //    CreditCardModel4WS ccModel4ws = new CreditCardModel4WS();
        //    ccModel4ws.servProvCode = AgencyCode;
        //    ccModel4ws.callerID = CallerID;
        //    ccModel4ws.accountNumber = "4012888888881881";
        //    ccModel4ws.balance = "30";
        //    ccModel4ws.accelaFee = "20";
        //    ccModel4ws.posTransSeq = "10000";
        //    ccModel4ws.expirationDate = "15";
        //    ccModel4ws.expirationMonth = "12";
        //    ccModel4ws.expirationYear = "2020";
        //    ccModel4ws.cardType = "Visa";
        //    ccModel4ws.holderName = "John Smith";
        //    ccModel4ws.businessName = "Achievo";
        //    ccModel4ws.streetAddress = "1000 Broad Street";
        //    ccModel4ws.city = "New Orleans";
        //    ccModel4ws.state = "LA";
        //    ccModel4ws.postalCode = "70119";
        //    ccModel4ws.email = "dylan.liang@achievo.com";
        //    ccModel4ws.securityCode = "5689";

        //    return ccModel4ws;
        //}

        private bool IsCardNameMatch(string[] agencys, int age, string name, CreditCardModel4WS ccm)
        {
            if (ccm.accountNumber == "4012888888881881" && name == "John")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
