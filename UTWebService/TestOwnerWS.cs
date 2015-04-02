///**
// * <pre>
// * 
// *  Accela Citizen Access
// *  File: TestCapWS.cs
// * 
// *  Accela, Inc.
// *  Copyright (C): 2007-2010
// * 
// *  Description:
// * 
// *  Notes:
// * $Id: TestOwnerWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
// *  Revision History
// *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
// * </pre>
// */

//using Accela.ACA.WSProxy;
//using System;
//using NUnit.Framework;

//namespace Accela.ACA.Test.WebService
//{
//    [TestFixture()]
//    public class TestOwnerWS : TestBase
//    {
//        private OwnerWebServiceService _unitUnderTest;
//        private ParcelWebServiceService parcelTest;

//        [SetUp()]
//        public void SetUp()
//        {
//            _unitUnderTest = WSFactory.Instance.GetOwnerService();
//            parcelTest = WSFactory.Instance.GetParcelService();
//        }

//        [TearDown()]
//        public void TearDown()
//        {
//            _unitUnderTest = null;
//        }

//        [Test()]
//        public void TestGetOwnerCondition()
//        {

//            //long ownerNumber = 18903;
//            //string callerID = "ADMIN";
//            //OwnerModel4WS result = _unitUnderTest.getOwnerCondition(new string[]{AgencyCode}, ownerNumber, callerID);

//            //Assert.IsNotNull(result);
//            //Assert.IsNotNull(result.hightestCondition);
//            //Assert.AreEqual("Lock", result.hightestCondition.impactCode);
//            //Assert.IsNotNull(result.noticeConditions);
//        }

//        [Test()]
//        public void TestGetOwnerListByParcelNbrs()
//        {
//            long sourceSqeNo = 45;
//            string[] parcels = new string[] { "15396", "15397", "15398", "15399" };
//            OwnerModel4WS[] result = _unitUnderTest.getOwnerListByParcelNbrs( AgencyCode, sourceSqeNo, parcels);

//            Assert.IsNotNull(result);
//        }


//        [Test()]
//        public void TestGetOwnerByPK()
//        {
//            long sourceSqeNo = 45;
//            long ownerNbr = 960;
//            OwnerModel4WS result = _unitUnderTest.getOwnerByPK(sourceSqeNo, ownerNbr);

//            Assert.IsNotNull(result);
//        }

//        [Test()]
//        public void TestGetAPOListByOwnerPK()
//        {
//            long sourceSqeNo = 45;
//            long ownerNbr = 960;
//            OwnerModel4WS result = _unitUnderTest.getOwnerByPK(sourceSqeNo, ownerNbr);
//            Assert.IsNotNull(result);

//            ParcelInfoModel4WS parcelInfo = new ParcelInfoModel4WS();
//            parcelInfo.ownerModel = result;
//            ParcelInfoModel4WS[] apoList = _unitUnderTest.getAPOList(AgencyCode, parcelInfo);
//        }
//    }
//}