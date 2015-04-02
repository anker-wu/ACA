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
 * $Id: TestCapTypeFilterManagerWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestCapTypeFilterManagerWS : TestBase
    {
        private CapTypeFilterManagerWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<CapTypeFilterManagerWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestCreateCapTypeFilter()
        {

            CapTypeFilterModel4WS capTypeFilter = new CapTypeFilterModel4WS();
            capTypeFilter.servProvCode = AgencyCode;
            capTypeFilter.moduleName = "Building";

            Random random = new Random();
            int rdmNbr = random.Next(1, 100000);
            capTypeFilter.filterName = "UT" + rdmNbr;

            CapTypeModel capType = new CapTypeModel();
            capType.alias = "Building/Building Permit/New Commercial/na";
            capTypeFilter.filteredCapTypeList = new CapTypeModel[] { capType };

            string callerID = "UTAdmin";

            _unitUnderTest.createCapTypeFilter(capTypeFilter, callerID);

        }

        [Test()]
        public void TestGetCapTypeFilterListByModule()
        {
            string servProvCode = AgencyCode;
            string moduleName = "Building";
            string callerID = "ACA Auto UT";
            string[] results = _unitUnderTest.getCapTypeFilterListByModule(servProvCode, moduleName, callerID);
            if (results != null)
            {
                foreach (string s in results)
                {
                    Console.WriteLine("Filter Name:" + s);
                }
            }
            else
            {
                Console.WriteLine("filter name array is null");
            }
        }

        [Test()]
        public void getCapTypeFilterModel()
        {
            string servProvCode = AgencyCode;
            string moduleName = "Building";
            string filterName = "UT8533";
            string callerID = "ACA Auto UT";

            CapTypeFilterModel4WS capTypeFilter = _unitUnderTest.getCapTypeFilterModel(servProvCode, moduleName, filterName, callerID);

            Assert.IsNotNull(capTypeFilter);
        }

        [Test()]
        public void TestCreateOrEditFilter4Button()
        {
            XButtonFilterModel4WS xBtnFilter4WS = new XButtonFilterModel4WS();
            string callerID = "ACA Auto UT";
            _unitUnderTest.createOrEditFilter4Button(xBtnFilter4WS, callerID);
        }

        [Test()]
        public void TestGetFilter4Button()
        {
            string servProvCode = AgencyCode;
            string moduleName = "Building";
            string labelKey = "per_permitDetail_label_createAmendment";
            string callerID = "ACA Auto UT";
            XButtonFilterModel4WS xbtn = _unitUnderTest.getFilter4Button(servProvCode, moduleName, labelKey, callerID);

            Assert.IsNotNull(xbtn);
        }

        [Test()]
        public void TestGetFilter4ButtonListByFilter()
        {
            string servProvCode = AgencyCode;
            string moduleName = "Building";
            string filterName = "FILTER99999";
            string callerID = "ACA Auto UT";
            XButtonFilterModel4WS[] xbtnArray = _unitUnderTest.getFilter4ButtonListByFilterName(servProvCode, moduleName, filterName, callerID);

            if (xbtnArray != null && xbtnArray.Length > 0)
            {
                foreach (XButtonFilterModel4WS ws in xbtnArray)
                {
                    string labelKey = ws.controlLabelKey;
                    string filter = ws.filterName;

                    Console.WriteLine("filterName = " + filterName + " >> labelKey = " + labelKey);
                }
            }
            else
            {
                Console.WriteLine("This filter hasn't any associated button or link.");
            }
        }


        //
    }
}
