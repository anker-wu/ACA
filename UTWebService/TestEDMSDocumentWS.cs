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
 * $Id: TestEDMSDocumentWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
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
using Accela.ACA.WSProxy.WSModel;
using Accela.ACA.Test.WebService;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestEDMSDocumentWS : TestBase
    {
        private EDMSDocumentWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<EDMSDocumentWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestDoUpload()
        {
            DocumentModel4WS documentModel4WS = new DocumentModel4WS();
            string filePath = "C:/acalog.txt";
        }

        [Test()]
        public void TestGetDocCategoryList()
        {
            string callerID = "PUBLICUSER8954";
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.id1 = "08SMO";
            capID.id2 = "00000";
            capID.id3 = "00006";

            RefDocumentModel4WS[] refDoc4WS = _unitUnderTest.getDocCategoryList(capID, callerID);

            if (refDoc4WS != null && refDoc4WS.Length > 0)
            {
                Console.WriteLine("The Document Category List size is : " + refDoc4WS.Length);

                foreach (RefDocumentModel4WS d in refDoc4WS)
                {
                    Console.WriteLine("Document Document Code : " + d.documentCode);
                    Console.WriteLine("Document Category : " + d.documentType);
                    Console.WriteLine("Document User Role Code : " + d.restrictRole4ACA);
                    Console.WriteLine("Document User Role Name : All aca user/Cap Creator/Licensed Professional/Contact/Owner");
                    //Console.Write("Document User Role :  " + d.userRolePrivilegeModel.allAcaUserAllowed + "/");
                    //Console.Write(d.userRolePrivilegeModel.capCreatorAllowed + "/");
                    //Console.Write(d.userRolePrivilegeModel.licensendProfessionalAllowed + "/");
                    //Console.Write(d.userRolePrivilegeModel.contactAllowed + "/");
                    //Console.WriteLine(d.userRolePrivilegeModel.ownerAllowed + "/");
                    Console.WriteLine("=================================================");
                }
            }
            else
            {
                Console.WriteLine("Document Category List is Empty !");
            }
        }

        [Test()]
        public void TestgetDocumentList()
        {
            string callerID = "PublicUser";
            string module = "Building";
            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.customID = "07435-00000-00045";
            capIDModel4WS.id1 = "07435";
            capIDModel4WS.id2 = "00000";
            capIDModel4WS.id3 = "00045";

            DocumentModel4WS[] documentArray = _unitUnderTest.getDocumentList(AgencyCode, module, callerID, capIDModel4WS);

            if (documentArray != null && documentArray.Length > 0)
            {
                Console.WriteLine("Document Amount>>" + documentArray.Length);
                foreach (DocumentModel4WS ws in documentArray)
                {
                    string docDes = ws.docDescription;
                    string fileName = ws.fileName;
                    string fileSize = ws.fileSize;
                    string docDate = ws.docDate;
                    string source = ws.source;
                    string fileKey = ws.fileKey;
                    string actionNo = ws.actionNo;
                    string documentNo = ws.documentNo;
                    string docGroup = ws.docGroup;
                    string docType = ws.docType;
                    Console.WriteLine("File Key" + fileKey + "File Name>> " + fileName + " DocType>>" + docType + " Document Description>>" + docDes + " File Size>>" +
                                      fileSize + " docDate>>" + docDate + " source>>" + source + " actionNo" + actionNo + " documentNo" + documentNo + " docGroup>>" + docGroup);
                }
            }
            else
            {
                Console.WriteLine("Document List is null or < 0 !");
            }

            Assert.IsNotNull(documentArray);
        }

        [Test()]
        public void TestdoUpload()
        {
            //            string serviceProviderCode = null;
            //            string callerID = null;
            //            string module = "Building";
            //            CapIDModel4WS capIDModel4WS = null;
            //            DocumentModel4WS documentModel4WS = null;
            //            bool expectedBoolean = true;
            //            bool resultBoolean = false;
            //            resultBoolean = _unitUnderTest.DoUpload(AgencyCode, module, callerID, capIDModel4WS, documentModel4WS);
            //            Assert.AreEqual(expectedBoolean, resultBoolean, "DoUpload method returned unexpected result.");

        }

        [Test()]
        public void TestdoDelete()
        {
            string callerID = "PublicUser";
            string module = "Building";
            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.customID = "07888-00000-00132";
            capIDModel4WS.serviceProviderCode = AgencyCode;
            capIDModel4WS.id1 = "07888";
            capIDModel4WS.id2 = "00000";
            capIDModel4WS.id3 = "00132";
            long documentID = 7052;
            string fileKey = "7052";
            bool expectedBoolean = true;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.doDelete(AgencyCode, module, callerID, capIDModel4WS, documentID, fileKey);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoDelete method returned unexpected result.");

        }

        [Test()]
        public void TestdoDownload()
        {
            string callerID = "PublicUser";
            string module = "Building";
            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.serviceProviderCode = AgencyCode;
            capIDModel4WS.customID = "07888-00000-00161";
            capIDModel4WS.id1 = "07888";
            capIDModel4WS.id2 = "00000";
            capIDModel4WS.id3 = "00161";
            long documentID = 7065;
            string fileKey = "7065";
            DocumentModel4WS documentModel4WS = null;
            //documentModel4WS = _unitUnderTest.doDownload(AgencyCode, module, callerID, capIDModel4WS, documentID, fileKey);

            if (documentModel4WS != null)
            {
                Console.WriteLine("documentModel4WS is not null");
                Console.WriteLine("fileName:" + documentModel4WS.fileName);
                Console.WriteLine("fileKey:" + documentModel4WS.fileKey);
                Console.WriteLine("fileDes:" + documentModel4WS.docDescription);
                Console.WriteLine("fileContent:" + documentModel4WS.fileName);
                Console.WriteLine("fileType:" + documentModel4WS.docType);
                DocumentContentModel4WS documentContentModel4WS = documentModel4WS.documentContent;
                if (documentContentModel4WS != null)
                {
                    Console.WriteLine("documentContentModel4WS is not null");
                    Console.WriteLine("documentContentModel4WS is:" + documentContentModel4WS);
                }
                else
                {
                    Console.WriteLine("documentContentModel4WS is null");
                }
            }
            else
            {
                Console.WriteLine("documentModel4WS is null");
            }
            Assert.IsNotNull(documentModel4WS);
        }

        [Test()]
        public void TestgetSecurityPolicy()
        {
            string callerID = "PUBLICUSER1112";
            string module = "Building";
            EdmsPolicyModel4WS edmsPolicyModel = null;
            //edmsPolicyModel = _unitUnderTest.getSecurityPolicy(AgencyCode, module, callerID);
            if (edmsPolicyModel == null)
            {
                Console.WriteLine("edmsPolicyModel is null");
            }
            else
            {
                Console.WriteLine("documentContentModel4WS is not null");
                Console.WriteLine("agencyName: " + edmsPolicyModel.agencyName);
                Console.WriteLine("sourceName: " + edmsPolicyModel.sourceName);
                Console.WriteLine("configuration: " + edmsPolicyModel.configuration);
                Console.WriteLine("defaultRight: " + edmsPolicyModel.defaultRight);
                Console.WriteLine("deleteRight: " + edmsPolicyModel.deleteRight);
                Console.WriteLine("downloadRight; " + edmsPolicyModel.downloadRight);
                Console.WriteLine("uploadRight: " + edmsPolicyModel.uploadRight);

            }
        }

    }
}
