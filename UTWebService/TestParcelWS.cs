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
 * $Id: TestParcelWS.cs 183331 2010-10-29 07:40:02Z ACHIEVO\xinter.peng $.
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
    public class TestParcelWS : TestBase
    {
        private ParcelWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<ParcelWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetParcelCondition()
        {
            //string parcelNumber = "19094";
            //string callerID = "ADMIN";
            //ParcelModel result = _unitUnderTest.getParcelCondition(AgencyCode, parcelNumber, callerID);

            //Assert.IsNotNull(result);
            //Assert.IsNotNull(result.hightestCondition);
            //Assert.AreEqual("Lock", result.hightestCondition.impactCode);
            //Assert.IsNotNull(result.noticeConditions);
        }

        [Test()]
        public void TestGetAPOList()
        {
            //search by parcel
            ParcelModel parcelModel = new ParcelModel();
            parcelModel.parcelNumber = "1";
            parcelModel.lot = "";
            parcelModel.block = "";
            parcelModel.subdivision = "";

            ParcelInfoModel parcelInfoModel = new ParcelInfoModel();
            parcelInfoModel.parcelModel = parcelModel;

            ParcelInfoModel[] result = _unitUnderTest.getAPOList(AgencyCode, parcelInfoModel, false);

            Assert.IsNotNull(result);

        }


        [Test()]
        public void TestGetAPOListUseAllSearchCondition()
        {
            //search by parcel
            ParcelModel parcelModel = new ParcelModel();
            parcelModel.parcelNumber = "1";
            parcelModel.lot = "1";
            parcelModel.block = "1";
            parcelModel.subdivision = "1";
            parcelModel.page = "1";
            parcelModel.book = "1";
            parcelModel.tract = "1";
            parcelModel.parcelArea = 1;
            parcelModel.legalDesc = "1";
            parcelModel.exemptValue = 1;
            parcelModel.improvedValue = 1;

            ParcelInfoModel parcelInfoModel = new ParcelInfoModel();
            parcelInfoModel.parcelModel = parcelModel;

            ParcelInfoModel[] result = _unitUnderTest.getAPOList(AgencyCode, parcelInfoModel, false);


        }


        [Test()]
        public void TestGetParcelByPK()
        {
            //long sourceSeqNo = 45;
            //string parcelNo = "15377";
            //string agencyCode = AgencyCode;
            //ParcelModel result = _unitUnderTest.getParcelByPK(agencyCode, parcelNo, sourceSeqNo);

            //Assert.IsNotNull(result);

        }


        //[Test()]
        //public void TestGetParcelListByOwnerNbr()
        //{

        //    long ownerNbr = 217;
        //    string callerID = "ADMIN";

        //    ParcelModel[] result = _unitUnderTest.getParcelListByOwnerNbr(AgencyCode, ownerNbr, null, callerID);

        //    Assert.IsNotNull(result);

        //}


        //[Test()]
        //public void TestGetParcelListByRefAddressId()
        //{

        //    long addressId = 95;
        //    string callerID = "ADMIN";

        //    ParcelModel[] result = _unitUnderTest.getParcelListByRefAddressId(AgencyCode, addressId, null, callerID);

        //    Assert.IsNotNull(result);

        //}
    }
}