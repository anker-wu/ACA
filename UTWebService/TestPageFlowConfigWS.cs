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
 * $Id: TestPageFlowConfigWS.cs 180838 2010-09-10 01:51:20Z ACHIEVO\jackie.yu $.
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
    public class TestPageFlowConfigWS : TestBase
    {
        private PageFlowConfigWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<PageFlowConfigWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetAllPageFlowGroup()
        {
            string callerID = "ACA ADMIN";

            if (_unitUnderTest != null)
            {
                PageFlowGroupModel[] resultArray = _unitUnderTest.getAllPageFlowGroup(AgencyCode, callerID);

                foreach (PageFlowGroupModel ws in resultArray)
                {
                    string groupCode = ws.pageFlowGrpCode;
                    StepModel[] steps = ws.stepList;
                    Console.WriteLine("Group Code:" + groupCode);
                }
            }
        }

        [Test()]
        public void TestGetPageFlowGroup()
        {
            string groupCode = "DYLAN2";
            string callerID = "ACA ADMIN";
            string moduleName = "Building";

            PageFlowGroupModel result =
                _unitUnderTest.getPageFlowGroup(AgencyCode, moduleName, groupCode, callerID);

            Assert.IsNotNull(result);
        }

        [Test()]
        public void TestCreatePageFlowGroup()
        {
            //construct component model.
            ComponentModel compnt1 = new ComponentModel();
            compnt1.componentID = 1;
            compnt1.componentName = "Address";
            compnt1.customHeading = "my address";
            compnt1.displayFlag = "Y";
            compnt1.displayOrder = 1;
            compnt1.requiredFlag = "Y";
            compnt1.validateFlag = "Y";

            ComponentModel compnt2 = new ComponentModel();
            compnt2.componentID = 2;
            compnt2.componentName = "Parcel";
            compnt2.customHeading = "my parcel";
            compnt2.displayFlag = "Y";
            compnt2.displayOrder = 2;
            compnt2.requiredFlag = "Y";
            compnt2.validateFlag = "Y";

            ComponentModel compnt3 = new ComponentModel();
            compnt3.componentID = 3;
            compnt3.componentName = "Owner";
            compnt3.customHeading = "my owner";
            compnt3.displayFlag = "Y";
            compnt3.displayOrder = 1;
            compnt3.requiredFlag = "Y";
            compnt3.validateFlag = "Y";

            ComponentModel compnt4 = new ComponentModel();
            compnt4.componentID = 4;
            compnt4.componentName = "Licensed Professional";
            compnt4.customHeading = "my Licensed Professional";
            compnt4.displayFlag = "Y";
            compnt4.displayOrder = 1;
            compnt4.requiredFlag = "Y";
            compnt4.validateFlag = "Y";



            //construct page model
            PageModel page1 = new PageModel();
            page1.componentList = new ComponentModel[] { compnt1, compnt2 };
            page1.pageName = "my page 1";
            page1.pageOrder = 1;

            PageModel page2 = new PageModel();
            page2.componentList = new ComponentModel[] { compnt3 };
            page2.pageName = "my page 2";
            page2.pageOrder = 2;

            PageModel page3 = new PageModel();
            page3.componentList = new ComponentModel[] { compnt4 };
            page3.pageName = "my page 3";
            page3.pageOrder = 1;

            //construct step model
            StepModel step1 = new StepModel();
            step1.pageList = new PageModel[] { page1, page2 };
            step1.stepName = "my step 1";
            step1.stepOrder = 1;

            StepModel step2 = new StepModel();
            step2.pageList = new PageModel[] { page3 };
            step2.stepName = "my step 2";
            step2.stepOrder = 2;

            //construct pageflow group model.
            PageFlowGroupModel pageFlowGroup = new PageFlowGroupModel();
            Random random = new Random();
            int rdmNbr = random.Next(1, 100000);
            pageFlowGroup.pageFlowGrpCode = "UT" + rdmNbr;
            Console.WriteLine("group code: " + pageFlowGroup.pageFlowGrpCode);
            pageFlowGroup.pageFlowType = "PERMIT";
            pageFlowGroup.serviceProviderCode = AgencyCode;
            pageFlowGroup.stepList = new StepModel[] { step1, step2 };
            string[] capTypeNameArray = new string[] 
                { "Building/0105foraccela/0105foraccela/123",
                "Building/0105foraccela/0105foraccela/0105foraccela",
                "Building/0105foraccela/0105foraccela/EEE",
                "Cap Type For ACA Test Page Flow Group", "test" };
            pageFlowGroup.capTypeNameList = capTypeNameArray;

            string callerID = "ACA Admin";
            string moduleName = "Building";

            _unitUnderTest.createPageFlowGroup(pageFlowGroup, moduleName, callerID);
        }

        [Test()]
        public void TestEditPageFlowGroup()
        {
            //construct component model.
            ComponentModel compnt1 = new ComponentModel();
            compnt1.componentID = 1;
            compnt1.componentName = "updated Address";
            compnt1.customHeading = "updated my address";
            compnt1.displayFlag = "Y";
            compnt1.displayOrder = 5;
            compnt1.requiredFlag = "Y";
            compnt1.validateFlag = "Y";

            ComponentModel compnt2 = new ComponentModel();
            compnt2.componentID = 2;
            compnt2.componentName = "updated Parcel";
            compnt2.customHeading = "my parcel";
            compnt2.displayFlag = "Y";
            compnt2.displayOrder = 2;
            compnt2.requiredFlag = "Y";
            compnt2.validateFlag = "Y";

            ComponentModel compnt4 = new ComponentModel();
            compnt4.componentID = 4;
            compnt4.componentName = "Licensed Professional";
            compnt4.customHeading = "my Licensed Professional";
            compnt4.displayFlag = "Y";
            compnt4.displayOrder = 1;
            compnt4.requiredFlag = "Y";
            compnt4.validateFlag = "Y";

            //construct page model
            PageModel page1 = new PageModel();
            page1.componentList = new ComponentModel[] { compnt1 };
            page1.pageName = "my page 1";
            page1.pageOrder = 1;

            PageModel page2 = new PageModel();
            page2.componentList = new ComponentModel[] { compnt2 };
            page2.pageName = "my page 2";
            page2.pageOrder = 2;

            PageModel page3 = new PageModel();
            page3.componentList = new ComponentModel[] { compnt4 };
            page3.pageName = "my page 3";
            page3.pageOrder = 1;

            //construct step model
            StepModel step1 = new StepModel();
            step1.pageList = new PageModel[] { page1};
            step1.stepName = "my step 1";
            step1.stepOrder = 1;

            StepModel step2 = new StepModel();
            step2.pageList = new PageModel[] { page3 };
            step2.stepName = "my step 2";
            step2.stepOrder = 2;

            StepModel step3 = new StepModel();
            step3.pageList = new PageModel[] { page2 };
            step3.stepName = "updated my step 3";
            step3.stepOrder = 5;

            //construct pageflow group model.
            PageFlowGroupModel pageFlowGroup = new PageFlowGroupModel();
            pageFlowGroup.pageFlowGrpCode = "UT13883";
            Console.WriteLine("group code: " + pageFlowGroup.pageFlowGrpCode);
            pageFlowGroup.pageFlowType = "PERMIT";
            pageFlowGroup.serviceProviderCode = AgencyCode;
            pageFlowGroup.stepList = new StepModel[] { step1, step2, step3 };
            string[] capTypeNameArray = new string[] 
                { "Building/0105foraccela/0105foraccela/123",
                "Building/APO/ExternalAPO/APO",
                "Building/APO/ExternalAPO/Address",
                "Cap Type For ACA Test Page Flow Group", "test" };
            pageFlowGroup.capTypeNameList = capTypeNameArray;

            string callerID = "ACA Admin";
            string moduleName = "Building";

            _unitUnderTest.editePageFlowGroup(pageFlowGroup, moduleName, callerID);
        }

        [Test()]
        public void TestGetPageFlowGroupNameList()
        {
            string groupType = "PERMIT";

            string[] groupNameArray = _unitUnderTest.getPageFlowGroupNameList(AgencyCode, groupType);

            if (groupNameArray.Length > 0)
            {
                foreach (string s in groupNameArray)
                {
                    Console.WriteLine("PageFlow Group Name: " + s);
                }
            }
            else
            {
                Console.WriteLine("Can't find PageFlow Group Name.");
            }
        }

        [Test()]
        public void TestGetComponentDefList()
        {
            string groupType = "PERMIT";

            ComponentModel[] componentArray = _unitUnderTest.getComponentDefList(AgencyCode, groupType);

            if (componentArray.Length > 0)
            {
                foreach (ComponentModel s in componentArray)
                {
                    long componentID = s.componentID;
                    string componentName = s.componentName;

                    Console.WriteLine("Component ID: >>" + componentID + "  Def Component Name: >>" + componentName);
                }
            }
            else
            {
                Console.WriteLine("Can't find Def Component Model.");
            }
        }

        [Test()]
        public void TestDeletePageFlowGroup()
        {
            string pageFlowGroupCode = "Enter a name for this group";
            string callerID = "ACA Admin";
            string moduleName = "Building";

            _unitUnderTest.deletePageFlowGroup(AgencyCode, moduleName, pageFlowGroupCode, callerID);
        }

    }
}