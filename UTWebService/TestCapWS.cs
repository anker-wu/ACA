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
 * $Id: TestCapWS.cs 183331 2010-10-29 07:40:02Z ACHIEVO\xinter.peng $.
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
    public class TestCapWS : TestBase
    {
        private CapWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<CapWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }


        /*
        [Test()]
        public void TestGetAPOListByCap()
        {
          

            CapModel4WS capModel = new CapModel4WS();
            CapIDModel4WS idModel = new CapIDModel4WS();
            idModel.id1 = "07ANY";
            idModel.id2 = "00000";
            idModel.id3 = "00068";
            capModel.capID = idModel;


            string callerID = "ADMIN";
            ParcelInfoModel4WS[] result = _unitUnderTest.getAPOListByCap(AgencyCode, capModel, null, callerID);

            Assert.IsNotNull(result);

        }

        [Test()]
        public void TestCreatePartialCapWithTemplate()
        {
            CapModel4WS capModel4WS = new CapModel4WS();
            string publicUserId = "PUBLICUSER1112";
            string wotModelSeq = "";
            bool isFeeEstimates = false;

            capModel4WS.capType = generateCapTypeModel(AgencyCode);
            capModel4WS.applicantModel = generateApplicantModel(AgencyCode);
            capModel4WS.capDetailModel = new CapDetailModel();
            capModel4WS.capWorkDesModel = new CapWorkDesModel4WS();
            capModel4WS.capWorkDesModel.description = "description";
            capModel4WS.addressModel = generateAddressModel(AgencyCode);
            capModel4WS.ownerModel = generateOwnerModel(AgencyCode);
            capModel4WS.parcelModel = generateParcelModel(AgencyCode);
            capModel4WS.licenseProfessionalModel = generateLicenseProfessionalModel(AgencyCode);
            capModel4WS.contactsGroup = generateContactGroup(AgencyCode);

            capModel4WS.altID = "troy.yang222";

            CapModel4WS newCap = _unitUnderTest.createWrapperForPartialCap(AgencyCode, capModel4WS, publicUserId, wotModelSeq, isFeeEstimates);

            Assert.IsNotNull(newCap, "returned unexpected result.");
            Assert.IsNotNull(newCap.capID, "returned unexpected result.");
        }
         */

        [Test()]
        public void TestSavePartialCapForCreate()
        {
            CapModel4WS capModel4WS = new CapModel4WS();
            string publicUserId = "PUBLICUSER1112";
            string wotModelSeq = "";
            bool isFeeEstimates = false;

            capModel4WS.auditStatus = "A";
            capModel4WS.auditID = "ADMIN";
            capModel4WS.capType = generateCapTypeModel(AgencyCode);
            capModel4WS.capClass = ACAConstant.INCOMPLETE_CAP;
            //capModel4WS.applicantModel = generateApplicantModel(AgencyCode);
            //capModel4WS.capDetailModel = new CapDetailModel4WS();
            //capModel4WS.capWorkDesModel = new CapWorkDesModel4WS();
            //capModel4WS.capWorkDesModel.description = "description";
            //capModel4WS.addressModel = generateAddressModel(serviceProviderCode);
            //capModel4WS.ownerModel = generateOwnerModel(serviceProviderCode);
            //capModel4WS.parcelModel = generateParcelModel(serviceProviderCode);
            //capModel4WS.licenseProfessionalModel = generateLicenseProfessionalModel(serviceProviderCode);
            //capModel4WS.contactsGroup = generateContactGroup(serviceProviderCode);


            //CapModel4WS newCap = _unitUnderTest.saveWrapperForPartialCap(AgencyCode, capModel4WS, publicUserId, wotModelSeq, isFeeEstimates);

            //Assert.IsNotNull(newCap, "returned unexpected result.");
            //Assert.IsNotNull(newCap.capID, "returned unexpected result.");
        }


        [Test()]
        public void TestCreateOrGetRenewalPartialCap()
        {
            CapIDModel4WS parentCapID = new CapIDModel4WS();
            parentCapID.serviceProviderCode = AgencyCode;
            parentCapID.id1 = "08999";
            parentCapID.id2 = "00000";
            parentCapID.id3 = "00049";
            parentCapID.customID = "08888-00000-00049";

            string callerID = "PUBLICUSER8573";

            CapModel4WS cap = _unitUnderTest.createOrGetRenewalPartialCap(parentCapID, callerID);

            Assert.IsNotNull(cap, "returned unexpected result.");
        }

        [Test()]
        public void TestSavePartialCapForUpdate()
        {
            string wotModelSeq = "";
            bool isFeeEstimates = false;

            CapIDModel4WS capID4WS = new CapIDModel4WS();
            capID4WS.serviceProviderCode = AgencyCode;
            capID4WS.id1 = "07EST";
            capID4WS.id2 = "00000";
            capID4WS.id3 = "01923";

            string userSeqNum = "1112";
            string publicUserId = "PUBLICUSER1112";
            //CapWithConditionModel4WS capWithConditionModel = _unitUnderTest.getCapViewBySingle(capID4WS, userSeqNum);

            //CapModel4WS newCap = _unitUnderTest.saveWrapperForPartialCap(AgencyCode, capWithConditionModel.capModel, publicUserId, wotModelSeq, isFeeEstimates);

            //Assert.IsNotNull(newCap, "returned unexpected result.");
            //Assert.IsNotNull(newCap.capID, "returned unexpected result.");
        }

        /*
        public void TestUpdatePartialCapWithTemplate()
        {

            CapIDModel4WS capID4WS = new CapIDModel4WS();
            capID4WS.serviceProviderCode = AgencyCode;
            capID4WS.id1 = "07EST";
            capID4WS.id2 = "00000";
            capID4WS.id3 = "01923";

            string userSeqNum = "1112";
            string publicUserId = "PUBLICUSER1112";
            CapModel4WS capModel4WS = null;
            CapWithConditionModel4WS capWithConditionModel = null;
            //capWithConditionModel = _unitUnderTest.getCapViewBySingle(capID4WS, userSeqNum)
            if (capWithConditionModel != null)
            {
                capModel4WS = capWithConditionModel.capModel;
            }

            if (capModel4WS == null)
            {
                Assert.Fail("Cannot find the CAP.");
            }

            if (capModel4WS.addressModel != null)
            {
                capModel4WS.addressModel.attributes = generateAddressTemplate2(AgencyCode);
            }
            if (capModel4WS.parcelModel != null && capModel4WS.parcelModel.parcelModel != null)
            {
                capModel4WS.parcelModel.parcelModel.parcelAttribute = generateParcelTemplate2(AgencyCode);
            }

            if (capModel4WS.ownerModel != null)
            {
                capModel4WS.ownerModel.attributes = generateOwnerTemplate2(AgencyCode);
            }

            if (capModel4WS.licenseProfessionalModel != null)
            {
                capModel4WS.licenseProfessionalModel.attributes = generateProfessionalTemplate2(AgencyCode);
            }

            if (capModel4WS.applicantModel != null && capModel4WS.applicantModel.people != null)
            {
                capModel4WS.applicantModel.people.attributes = generateApplicantTemplate2(AgencyCode);
            }

            if (capModel4WS.contactsGroup != null && capModel4WS.contactsGroup.Length == 3)
            {
                if (capModel4WS.contactsGroup[0] != null && capModel4WS.contactsGroup[0].people != null)
                {
                    capModel4WS.contactsGroup[0].people.attributes = generateContact1Template2(AgencyCode);
                }
                if (capModel4WS.contactsGroup[1] != null && capModel4WS.contactsGroup[1].people != null)
                {
                    capModel4WS.contactsGroup[1].people.attributes = generateContact2Template2(AgencyCode);
                }
                if (capModel4WS.contactsGroup[2] != null && capModel4WS.contactsGroup[2].people != null)
                {
                    capModel4WS.contactsGroup[2].people.attributes = generateContact3Template2(AgencyCode);
                }
            }

            CapModel4WS updateCap =
                _unitUnderTest.updatePartialCapModelWrapper(AgencyCode, capModel4WS, publicUserId);

            Assert.IsNotNull(updateCap, "returned unexpected result.");
            Assert.IsNotNull(updateCap.capID, "returned unexpected result.");
        }
         */


        private static CapTypeModel generateCapTypeModel(string serviceProviderCode)
        {
            CapTypeModel capType4WS = new CapTypeModel();
            capType4WS.auditStatus = "A";
            capType4WS.serviceProviderCode = serviceProviderCode;
            capType4WS.moduleName = "Building";
            capType4WS.group = "Building";
            capType4WS.type = "ken";
            capType4WS.subType = "ken";
            capType4WS.category = "autoassign_ken";
            return capType4WS;
        }

        /*
        private static CapContactModel4WS generateApplicantModel(string serviceProviderCode)
        {
            CapContactModel4WS applicant = new CapContactModel4WS();
            PeopleModel4WS people = new PeopleModel4WS();
            CompactAddressModel compactAddress = new CompactAddressModel();

            people.businessName = "test";
            compactAddress.addressLine1 = "Tech Park";
            compactAddress.city = "SZ";
            people.contactType = "Applicant";
            TemplateAttributeModel[] applicantAttrs = generateApplicantTemplate(serviceProviderCode);
            people.attributes = applicantAttrs;
            people.compactAddress = compactAddress;
            applicant.people = people;
            return applicant;
        }
         */

        private static AddressModel generateAddressModel(string serviceProviderCode)
        {
            AddressModel address = new AddressModel();
            address.serviceProviderCode = serviceProviderCode;
            address.city = "New York";
            address.state = "LA";
            address.streetName = "King Street";
            address.streetPrefix = "1";
            address.streetSuffix = "23";
            TemplateAttributeModel[] addressAttrs = generateAddressTemplate(serviceProviderCode);
            address.templates = addressAttrs;
            return address;
        }


        private static RefOwnerModel generateOwnerModel(string serviceProviderCode)
        {
            RefOwnerModel refOwnerModel = new RefOwnerModel();
            refOwnerModel.ownerStatus = "A";
            refOwnerModel.ownerFullName = "#1 TROY OUTREACH MINISTRIES";
            refOwnerModel.ownerTitle = "COMPANY";
            refOwnerModel.ownerFirstName = "MINISTRIES";
            refOwnerModel.ownerLastName1 = "#1 TROY OUTREACH";
            refOwnerModel.taxID = "721503953";
            refOwnerModel.address1 = "INC % REV DONALD R BERRYHILL";
            refOwnerModel.address2 = "7201 OLIVE ST";
            refOwnerModel.city = "NEW ORLEANS";
            refOwnerModel.state = "LA";
            refOwnerModel.zip = "701250000";
            refOwnerModel.country = "USA";
            refOwnerModel.mailAddress1 = "INC % REV DONALD R BERRYHILL";
            refOwnerModel.mailAddress2 = "7201 OLIVE ST";
            refOwnerModel.mailCity = "NEW ORLEANS";
            refOwnerModel.mailState = "LA";
            refOwnerModel.mailZip = "701250000";
            refOwnerModel.mailCountry = "OTE";
            refOwnerModel.templates = generateOwnerTemplate(serviceProviderCode);

            refOwnerModel.auditDate = DateTime.Now;
            refOwnerModel.auditID = "ADMIN";
            refOwnerModel.auditStatus = "A";

            return refOwnerModel;
        }

        private static CapParcelModel generateParcelModel(string serviceProviderCode)
        {
            CapParcelModel capParcelModel = new CapParcelModel();
            ParcelModel parcelModel = new ParcelModel();
            parcelModel.sourceSeqNumber = 38;
            parcelModel.parcelNumber = "000000000-0002";
            parcelModel.gisSeqNo = 1;
            parcelModel.parcelStatus = "A";
            parcelModel.book = "Book";
            parcelModel.page = "Page";
            parcelModel.parcel = "Parcel";
            parcelModel.mapRef = "Map Reference";
            parcelModel.mapNo = "Map";
            parcelModel.lot = "Lot";
            parcelModel.block = "Block";
            parcelModel.tract = "Tract";
            parcelModel.legalDesc = "Legal Description";
            parcelModel.parcelArea = 1;
            parcelModel.planArea = "Plan A";
            parcelModel.censusTract = "Census Tra";
            parcelModel.councilDistrict = "Council Di";
            parcelModel.supervisorDistrict = "Supervisor";
            parcelModel.inspectionDistrict = "Inspection Dist.:";
            parcelModel.landValue = 1;
            parcelModel.improvedValue = 1;
            parcelModel.exemptValue = 1;
            parcelModel.templates = generateParcelTemplate(serviceProviderCode);
            parcelModel.auditDate = DateTime.Now;
            parcelModel.auditID = "PUBLICUSER";
            parcelModel.auditStatus = "A";

            capParcelModel.parcelNo = "000000000-0002";
            capParcelModel.parcelModel = parcelModel;

            return capParcelModel;
        }

        /*
        private static LicenseProfessionalModel4WS generateLicenseProfessionalModel(string serviceProviderCode)
        {
            LicenseProfessionalModel4WS model = new LicenseProfessionalModel4WS();
            model.serDes = "Description";
            model.licenseType = "ACTHECT";
            model.licenseNbr = "111111-234";
            model.contactFirstName = "BRADLEY";
            model.contactMiddleName = null;
            model.contactLastName = "CANTRELL";
            model.suffixName = null;
            model.businessName = "BJC CONSTRUCTION INC.";
            model.businessLicense = "0000000000";
            model.licesnseOrigIssueDate = null;
            model.licenseExpirDate = null;
            model.lastUpdateDate = null;
            model.lastRenewalDate = null;
            model.einSs = "999030338";
            model.selfIns = null;
            model.address1 = "6013 E ST BERNARD HWY";
            model.address2 = null;
            model.address3 = null;
            model.city = "VIOLET";
            model.state = "LA";
            model.country = "USA";
            model.zip = "700920000";
            model.phone1 = "5046822172";
            model.phone2 = null;
            model.printFlag = "Y";
            model.attributes = generateProfessionalTemplate(serviceProviderCode);

            model.auditDate = DateTime.Now.ToString();
            model.auditID = "PUBLICUSER";
            model.auditStatus = "A";

            return model;
        }

        private static CapContactModel4WS[] generateContactGroup(string serviceProviderCode)
        {
            CapContactModel4WS[] contacts = new CapContactModel4WS[3];

            contacts[0] = new CapContactModel4WS();
            contacts[0].people = new PeopleModel4WS();
            contacts[0].people.compactAddress = new CompactAddressModel();
            contacts[0].people.businessName = "achievo";
            contacts[0].people.compactAddress.addressLine1 = "Tech Park1";
            contacts[0].people.compactAddress.city = "SZ";
            contacts[0].people.contactType = "Applicant";
            contacts[0].people.attributes = generateContact1Template(serviceProviderCode);

            contacts[1] = new CapContactModel4WS();
            contacts[1].people = new PeopleModel4WS();
            contacts[1].people.compactAddress = new CompactAddressModel();
            contacts[1].people.businessName = "achievo";
            contacts[1].people.compactAddress.addressLine1 = "Tech Park2";
            contacts[1].people.compactAddress.city = "SZ";
            contacts[1].people.contactType = "Applicant";
            contacts[1].people.attributes = generateContact2Template(serviceProviderCode);

            contacts[2] = new CapContactModel4WS();
            contacts[2].people = new PeopleModel4WS();
            contacts[2].people.compactAddress = new CompactAddressModel();
            contacts[2].people.businessName = "achievo";
            contacts[2].people.compactAddress.addressLine1 = "Tech Park3";
            contacts[2].people.compactAddress.city = "SZ";
            contacts[2].people.contactType = "Applicant";
            contacts[2].people.attributes = generateContact3Template(serviceProviderCode);

            return contacts;
        }
         */

        private static TemplateAttributeModel[] generateAddressTemplate(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].templateType = "ADDRESS";
            attributes[0].attributeName = "PHONE1";
            attributes[0].attributeLabel = "Phone1";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "12345678";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";

            return attributes;
        }

        private static TemplateAttributeModel[] generateAddressTemplate2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].templateType = "ADDRESS";
            attributes[0].attributeName = "PHONE1";
            attributes[0].attributeLabel = "Phone1";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "000000";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";

            return attributes;
        }

        private static TemplateAttributeModel[] generateOwnerTemplate(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].templateType = "OWNER";
            attributes[0].attributeName = "WORK";
            attributes[0].attributeLabel = "Work";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "Engineer";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";

            return attributes;
        }

        private static TemplateAttributeModel[] generateOwnerTemplate2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[2];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].templateType = "OWNER";
            attributes[0].attributeName = "WORK";
            attributes[0].attributeLabel = "Work";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "Engineer";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";

            attributes[1] = new TemplateAttributeModel();
            attributes[1].serviceProviderCode = serviceProviderCode;
            attributes[1].templateType = "OWNER";
            attributes[1].attributeName = "OWNERTEMPLAT2";
            attributes[1].attributeLabel = "owner tmplate2";
            attributes[1].attributeUnitType = "";
            attributes[1].attributeValue = "owner tmplate2";
            attributes[1].attributeValueDataType = "Text";
            attributes[1].attributeValueReqFlag = "N";
            attributes[1].vchFlag = "Y";

            return attributes;
        }

        private static TemplateAttributeModel[] generateParcelTemplate(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].templateType = "PARCEL";
            attributes[0].attributeName = "PART";
            attributes[0].attributeLabel = "Part";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "5";
            attributes[0].attributeValueDataType = "Number";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";

            return attributes;
        }

        private static TemplateAttributeModel[] generateParcelTemplate2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[2];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].templateType = "PARCEL";
            attributes[0].attributeName = "PART";
            attributes[0].attributeLabel = "Part";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "112";
            attributes[0].attributeValueDataType = "Number";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";

            attributes[1] = new TemplateAttributeModel();
            attributes[1].serviceProviderCode = serviceProviderCode;
            attributes[1].templateType = "PARCEL";
            attributes[1].attributeName = "DISTRICT";
            attributes[1].attributeLabel = "Disttrict";
            attributes[1].attributeUnitType = "";
            attributes[1].attributeValue = "NanShan";
            attributes[1].attributeValueDataType = "Number";
            attributes[1].attributeValueReqFlag = "N";
            attributes[1].vchFlag = "Y";

            return attributes;
        }

        private static TemplateAttributeModel[] generateApplicantTemplate(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "PHONE1";
            attributes[0].attributeLabel = "Phone1";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "12345678";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "Y";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;


            return attributes;
        }

        private static TemplateAttributeModel[] generateApplicantTemplate2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[2];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "PHONE1";
            attributes[0].attributeLabel = "Phone1";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "6666666";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "Y";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;

            attributes[1] = new TemplateAttributeModel();
            attributes[1].serviceProviderCode = serviceProviderCode;
            attributes[1].attributeName = "MSN";
            attributes[1].attributeLabel = "MSN";
            attributes[1].attributeUnitType = "";
            attributes[1].attributeValue = "applicant2@accela.com";
            attributes[1].attributeValueDataType = "Text";
            attributes[1].attributeValueReqFlag = "Y";
            attributes[1].vchFlag = "Y";
            attributes[1].auditID = "ADMIN";
            attributes[1].auditStatus = "A";
            attributes[1].auditDate = DateTime.Now;


            return attributes;
        }

        private static TemplateAttributeModel[] generateProfessionalTemplate(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "admin@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;


            return attributes;
        }

        private static TemplateAttributeModel[] generateProfessionalTemplate2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "professional@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;


            return attributes;
        }

        private static TemplateAttributeModel[] generateContact1Template(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "contact1@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;


            return attributes;
        }

        private static TemplateAttributeModel[] generateContact1Template2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[2];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "contact1@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;

            attributes[1] = new TemplateAttributeModel();
            attributes[1].serviceProviderCode = serviceProviderCode;
            attributes[1].attributeName = "CONTACT1TEMPLATE2";
            attributes[1].attributeLabel = "Contact1Template2";
            attributes[1].attributeUnitType = "";
            attributes[1].attributeValue = "Contact1Template2";
            attributes[1].attributeValueDataType = "Text";
            attributes[1].attributeValueReqFlag = "Y";
            attributes[1].vchFlag = "Y";
            attributes[1].auditID = "ADMIN";
            attributes[1].auditStatus = "A";
            attributes[1].auditDate = DateTime.Now;

            return attributes;
        }

        private static TemplateAttributeModel[] generateContact2Template(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "contact2@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;


            return attributes;
        }

        private static TemplateAttributeModel[] generateContact2Template2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[2];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "contact2@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;

            attributes[1] = new TemplateAttributeModel();
            attributes[1].serviceProviderCode = serviceProviderCode;
            attributes[1].attributeName = "CONTACT2TEMPLATE2";
            attributes[1].attributeLabel = "Contact2Template2";
            attributes[1].attributeUnitType = "";
            attributes[1].attributeValue = "Contact2Template2";
            attributes[1].attributeValueDataType = "Text";
            attributes[1].attributeValueReqFlag = "Y";
            attributes[1].vchFlag = "Y";
            attributes[1].auditID = "ADMIN";
            attributes[1].auditStatus = "A";
            attributes[1].auditDate = DateTime.Now;

            return attributes;
        }

        private static TemplateAttributeModel[] generateContact3Template(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[1];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "contact3@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;


            return attributes;
        }

        private static TemplateAttributeModel[] generateContact3Template2(string serviceProviderCode)
        {
            TemplateAttributeModel[] attributes = new TemplateAttributeModel[2];
            attributes[0] = new TemplateAttributeModel();
            attributes[0].serviceProviderCode = serviceProviderCode;
            attributes[0].attributeName = "MSN";
            attributes[0].attributeLabel = "MSN";
            attributes[0].attributeUnitType = "";
            attributes[0].attributeValue = "contact3@accela.com";
            attributes[0].attributeValueDataType = "Text";
            attributes[0].attributeValueReqFlag = "N";
            attributes[0].vchFlag = "Y";
            attributes[0].auditID = "ADMIN";
            attributes[0].auditStatus = "A";
            attributes[0].auditDate = DateTime.Now;

            attributes[1] = new TemplateAttributeModel();
            attributes[1].serviceProviderCode = serviceProviderCode;
            attributes[1].attributeName = "CONTACT3TEMPLATE2";
            attributes[1].attributeLabel = "Contact3Template2";
            attributes[1].attributeUnitType = "";
            attributes[1].attributeValue = "Contact3Template2";
            attributes[1].attributeValueDataType = "Text";
            attributes[1].attributeValueReqFlag = "Y";
            attributes[1].vchFlag = "Y";
            attributes[1].auditID = "ADMIN";
            attributes[1].auditStatus = "A";
            attributes[1].auditDate = DateTime.Now;

            return attributes;
        }

        [Test()]
        public void getCapByAltID()
        {
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = AgencyCode;

            capID.customID = "C12815";

            CapModel4WS cap = _unitUnderTest.getCapByAltID(capID);

            Assert.IsNotNull(cap, "returned unexpected result.");
        }

        [Test()]
        public void getMyCapListByCapTypeFilterNmae()
        {
            String callerID = "PUBLICUSER9273";
            String tradeNameFilterName = "TRADE_NAME";
            String tradeLicenseFilterName = "TRADE_LICENSES";
            String moduleName = "Licenses";
            QueryFormat4WS qf = null;


            //SimpleCapModel4WS[] caps = _unitUnderTest.getMyCapListByCapTypeFilterNmae(serviceProviderCode,callerID,tradeNameFilterName,tradeLicenseFilterName,moduleName,qf);

            //Assert.IsNotNull(caps, "returned unexpected result.");
        }

        [Test()]
        public void TestgetParentCapIDByChildID()
        {            
            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = AgencyCode;
            capID.ID1 = "10TMP";
            capID.ID2 = "00000";
            capID.ID3 = "00096";

            string relationship = "R";


            CapIDModel capIDModel = _unitUnderTest.getParentCapIDByChildID(capID, relationship, null, CallerID);

            Console.WriteLine("Return serviceProviderCode : " + capIDModel.serviceProviderCode);
            Console.WriteLine("Return ID1 : " + capIDModel.ID1);
            Console.WriteLine("Return ID2 : " + capIDModel.ID2);
            Console.WriteLine("Return ID3 : " + capIDModel.ID3);
            Assert.IsNotEmpty(capIDModel.serviceProviderCode);
        }

    }
}
