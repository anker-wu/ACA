/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestWorkFlowWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestWorkflowWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
    public class TestWorkflowWS : TestBase
    {
        private WorkflowWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<WorkflowWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetTaskItemByCapID()
        {
            string callerID = "PUBLICUSER8954";
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;
            capID.id1 = "08SMO";
            capID.id2 = "00000";
            capID.id3 = "00006";

            TaskItemModel4WS[] result =
                _unitUnderTest.getTaskItemByCapID(capID, null, callerID);

            foreach (TaskItemModel4WS ws in result)
            {
                if (ws == null || ws.userRolePrivilegeModel == null)
                {
                    continue;
                }

                Console.WriteLine("User Role Name : All aca user/Cap Creator/Licensed Professional/Contact/Owner");
                Console.Write("Document User Role :  " + ws.userRolePrivilegeModel.allAcaUserAllowed + "/");
                Console.Write(ws.userRolePrivilegeModel.capCreatorAllowed + "/");
                Console.Write(ws.userRolePrivilegeModel.licensendProfessionalAllowed + "/");
                Console.Write(ws.userRolePrivilegeModel.contactAllowed + "/");
                Console.WriteLine(ws.userRolePrivilegeModel.ownerAllowed + "/");
                Console.WriteLine("=================================================");
            }
        }

        [Test()]
        public void TestGetTaskHistoryList()
        {
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;
            capID.id1 = "08572";
            capID.id2 = "00000";
            capID.id3 = "00007";

            TaskItemModel4WS[] result =
                _unitUnderTest.getTaskHistoryList(capID, null);

            Assert.IsNotNull(result);
        }
        [Test()]
        public void getSubProcessesByPK()
        {
            string callerID = "PUBLICUSER8954";
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;
            capID.id1 = "08SMO";
            capID.id2 = "00000";
            capID.id3 = "00006";
            string parentProcessId = null;
            string stepNumber =null; ;
            //SimpleInspectionModel4WS[] result =
            //    _unitUnderTest.getSubProcessesByPK(capID, stepNumber, parentProcessId);



            //foreach (SimpleInspectionModel4WS ws in result)
            //{
            //    if (ws == null || ws.userRolePrivilegeModel == null)
            //    {
            //        continue;
            //    }

            //    Console.WriteLine("User Role Name : All aca user/Cap Creator/Licensed Professional/Contact/Owner");
            //    Console.Write("Document User Role :  " + ws.userRolePrivilegeModel.allAcaUserAllowed + "/");
            //    Console.Write(ws.userRolePrivilegeModel.capCreatorAllowed + "/");
            //    Console.Write(ws.userRolePrivilegeModel.licensendProfessionalAllowed + "/");
            //    Console.Write(ws.userRolePrivilegeModel.contactAllowed + "/");
            //    Console.WriteLine(ws.userRolePrivilegeModel.ownerAllowed + "/");
            //    Console.WriteLine("=================================================");
            //}
        }
    }
}