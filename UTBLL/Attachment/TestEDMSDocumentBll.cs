/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestInspectionTyepBll.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestEDMSDocumentBll.cs 177624 2010-07-22 02:16:43Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  05/07/2007     		troy.yang				Initial.
 * </pre>
 */

using System;

using Accela.ACA.Common;
using Accela.ACA.Test.BLL;
using Accela.ACA.WSProxy;
using NUnit.Framework;
using Accela.Test.Lib;

namespace Accela.ACA.BLL.Attachment
{
    [TestFixture()]
    public class TestEDMSDocumentBll : TestBase
    {
        private IEDMSDocumentBll _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestDoUpload()
        {
            string serviceProviderCode = "xxx";
            DocumentModel4WS documentModel4WS = new DocumentModel4WS();
            string filePath = "C:/acalog.txt";
            _unitUnderTest.DoUpload (serviceProviderCode, documentModel4WS, filePath,false);

        }

        [Test()]
        public void TestGetDocCategoryList()
        {
            string callerID = "PUBLICUSER1112";
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = "xxx";
            capID.id1 = "07222";
            capID.id2 = "00000";
            capID.id3 = "00009";

            //string[] categoryArray = _unitUnderTest.GetDocCategoryList(capID, callerID);
            //if (categoryArray != null && categoryArray.Length > 0)
            //{
            //    Console.WriteLine("The Document Category List size is : " + categoryArray.Length);

            //    foreach (string s in categoryArray)
            //    {
            //        Console.WriteLine("Document Category : " + s);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Document Category List is Empty !");
            //}
        }

        [Test()]
        public void TestgetDocumentList()
        {
            return;
            string serviceProviderCode = "xxx";
            string callerID = "PublicUser";
            string module = "Building";
            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.customID = "07435-00000-00045";
            capIDModel4WS.id1 = "07435";
            capIDModel4WS.id2 = "00000";
            capIDModel4WS.id3 = "00045";

            DocumentModel4WS[] documentArray = _unitUnderTest.GetDocumentList(serviceProviderCode, module, callerID, capIDModel4WS,false);

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
//            resultBoolean = _unitUnderTest.DoUpload(serviceProviderCode, module, callerID, capIDModel4WS, documentModel4WS);
//            Assert.AreEqual(expectedBoolean, resultBoolean, "DoUpload method returned unexpected result.");

        }

        [Test()]
        public void TestdoDelete()
        {
            return;
            string serviceProviderCode = "xxx";
            string callerID = "PublicUser";
            string module = "Building";
            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.customID = "07888-00000-00132";
            capIDModel4WS.serviceProviderCode = serviceProviderCode;
            capIDModel4WS.id1 = "07888";
            capIDModel4WS.id2 = "00000";
            capIDModel4WS.id3 = "00132";
            long documentID = 7052;
            string fileKey = "7052";
            bool expectedBoolean = true;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.DoDelete(serviceProviderCode, module, callerID, capIDModel4WS, documentID, fileKey,false);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoDelete method returned unexpected result.");

        }

        [Test()]
        public void TestdoDownload()
        {
            return;
            string serviceProviderCode = "xxx";
            string callerID = "PublicUser";
            string module = "Building";
            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.serviceProviderCode = serviceProviderCode;
            capIDModel4WS.customID = "07888-00000-00161";
            capIDModel4WS.id1 = "07888";
            capIDModel4WS.id2 = "00000";
            capIDModel4WS.id3 = "00161";
            long documentID = 7065;
            string fileKey = "7065";
            DocumentModel4WS documentModel4WS = null;
            documentModel4WS = _unitUnderTest.DoDownload(serviceProviderCode, module, callerID, capIDModel4WS, documentID, fileKey,false);

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
            //            Assert.IsNotNull(documentModel4WS);

        }

        [Test()]
        public void TestgetSecurityPolicy()
        {
            string serviceProviderCode = "xxx";
            string callerID = "PUBLICUSER1112";
            string module = "Building";
            EdmsPolicyModel4WS edmsPolicyModel = null;
           // edmsPolicyModel = _unitUnderTest.GetSecurityPolicy(serviceProviderCode, module, callerID);
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