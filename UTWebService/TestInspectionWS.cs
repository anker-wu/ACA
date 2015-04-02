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
 * $Id: TestInspectionWS.cs 182266 2010-10-13 03:12:22Z ACHIEVO\xinter.peng $.
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
    public class TestInspectionWS : TestBase
    {
        private InspectionWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<InspectionWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }
        
        [Test()]
        public void TestgetInspectionListByCapID()
        {            
            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = AgencyCode;
            capID.ID1 = "10CAP";
            capID.ID2 = "00000";
            capID.ID3 = "000A1";

            InspectionModel[] inspectionArray = _unitUnderTest.getInspectionListByCapID(capID, null, CallerID);

            if (inspectionArray == null)
            {
                return;
            }

            foreach (InspectionModel inspection in inspectionArray)
            {
                if (inspection != null && inspection.activity != null)
                {
                    Console.WriteLine("Activity End Time2 : " + inspection.activity.actEndTime2);
                    Console.WriteLine("User Role Name : All aca user/Cap Creator/Licensed Professional/Contact/Owner");
                    Console.Write("User Role :  " + inspection.activity.userRolePrivilegeModel.allAcaUserAllowed + "/");
                    Console.Write(inspection.activity.userRolePrivilegeModel.capCreatorAllowed + "/");
                    Console.Write(inspection.activity.userRolePrivilegeModel.licensendProfessionalAllowed + "/");
                    Console.Write(inspection.activity.userRolePrivilegeModel.contactAllowed + "/");
                    Console.WriteLine(inspection.activity.userRolePrivilegeModel.ownerAllowed + "/");
                    Console.WriteLine("=================================================");
                }               
            }
        }

        [Test()]
        public void TestbatchScheduleInspections()
        {
            string servProvCode = AgencyCode;

            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = servProvCode;
            capID.ID1 = "10CAP";
            capID.ID2 = "00000";
            capID.ID3 = "000A1";

            ActivityModel activity = new ActivityModel();
            activity.activityDate = new DateTime(2010, 10, 12, 8, 0, 0);
            
            activity.activityDescription = "MEW";
            activity.auditDate = DateTime.Now;
            activity.auditID = CallerID;
            activity.auditStatus = "A";
            activity.capID = capID;
            activity.serviceProviderCode = servProvCode;
            activity.status = "Scheduled";
            activity.time1 = "AM";
            activity.time2 = "08:00";
            activity.inspSequenceNumber = 2770;

            InspectionModel inspection = new InspectionModel();
            inspection.activity = activity;

            InspectionModel[] inspectionArray = new InspectionModel[] 
            { 
                inspection 
            };

            string actModel = "Schedule";

            SysUserModel sysUser = new SysUserModel();
            sysUser.agencyCode = servProvCode;
            sysUser.userID = CallerID;

            string[] returnValue = _unitUnderTest.batchScheduleInspections(servProvCode, inspectionArray, actModel, sysUser);

            if (returnValue == null)
            {
                return;
            }

            foreach (string value in returnValue)
            {
                Console.WriteLine("Return Value : " + value);
                Console.WriteLine("=================================================");
            }
        }

        [Test()]
        public void TestcancelInspection()
        {
            string servProvCode = AgencyCode;
            
            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = servProvCode;
            capID.ID1 = "10CAP";
            capID.ID2 = "00000";
            capID.ID3 = "000A1";

            ActivityModel activity = new ActivityModel();           

            activity.activityDescription = "HCSEZ Inspection";
            activity.activityGroup = "AD";
            activity.activityType = "HCSEZ Inspection";
            activity.idNumber = 63746;
            activity.inAdvanceFlag = "N";            
            activity.capID = capID;
            activity.serviceProviderCode = servProvCode;

            InspectionModel inspection = new InspectionModel();
            inspection.activity = activity;

            InspectionModel[] inspectionArray = new InspectionModel[] 
            {
                inspection
            };
            
            SysUserModel sysUser = new SysUserModel();
            sysUser.agencyCode = servProvCode;
            sysUser.userID = CallerID;

            int count = _unitUnderTest.cancelInspection(servProvCode, CallerID, inspectionArray, sysUser);

            Console.WriteLine("Count Value : " + count);
        }

        [Test()]
        public void TestgetConfirmMessageWhenCancel()
        {
            string servProvCode = AgencyCode;

            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = servProvCode;
            capID.ID1 = "10CAP";
            capID.ID2 = "00000";
            capID.ID3 = "000A1";

            ActivityModel activity = new ActivityModel();

            activity.activityDescription = "HCSEZ Inspection";
            activity.activityGroup = "AD";
            activity.activityType = "HCSEZ Inspection";
            activity.idNumber = 63746;
            activity.inAdvanceFlag = "N";
            activity.capID = capID;
            activity.serviceProviderCode = servProvCode;

            InspectionModel inspection = new InspectionModel();
            inspection.activity = activity;

            InspectionModel[] inspectionArray = new InspectionModel[] 
            {
                inspection
            };

            string returnValue = _unitUnderTest.getConfirmMessageWhenCancel(inspectionArray, servProvCode, CallerID);

            Console.WriteLine("Return Value : " + returnValue);            
        }

        [Test()]
        public void TestvalidateScheduleDateByFlow()
        {
            string servProvCode = AgencyCode;

            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = servProvCode;
            capID.ID1 = "10CAP";
            capID.ID2 = "00000";
            capID.ID3 = "000A1";

            ActivityModel activity = new ActivityModel();
            activity.activityDate = new DateTime(2010, 10, 12, 8, 0, 0);

            activity.activityDescription = "MEW";
            activity.auditDate = DateTime.Now;
            activity.auditID = CallerID;
            activity.auditStatus = "A";
            activity.capID = capID;
            activity.serviceProviderCode = servProvCode;
            activity.status = "Scheduled";
            activity.time1 = "AM";
            activity.time2 = "08:00";
            activity.inspSequenceNumber = 2770;

            InspectionModel inspection = new InspectionModel();
            inspection.activity = activity;

            string returnValue = _unitUnderTest.validateScheduleDateByFlow(capID, inspection);

            Console.WriteLine("Return Value : " + returnValue);            
        }

        [Test()]
        public void TestgetInspections()
        {
            string servProvCode = AgencyCode;

            InspectionModel[] inspectionArray = _unitUnderTest.getInspections(servProvCode, null, CallerID);

            if (inspectionArray == null)
            {
                return;
            }

            foreach (InspectionModel inspection in inspectionArray)
            {
                if (inspection != null && inspection.activity != null)
                {
                    Console.WriteLine("User Role Code : " + inspection.activity.restrictRole);
                    Console.WriteLine("User Role Name : All aca user/Cap Creator/Licensed Professional/Contact/Owner");
                    Console.Write("User Role :  " + inspection.activity.userRolePrivilegeModel.allAcaUserAllowed + "/");
                    Console.Write(inspection.activity.userRolePrivilegeModel.capCreatorAllowed + "/");
                    Console.Write(inspection.activity.userRolePrivilegeModel.licensendProfessionalAllowed + "/");
                    Console.Write(inspection.activity.userRolePrivilegeModel.contactAllowed + "/");
                    Console.WriteLine(inspection.activity.userRolePrivilegeModel.ownerAllowed + "/");
                    Console.WriteLine("=================================================");
                }
            }
        }
    }
}
