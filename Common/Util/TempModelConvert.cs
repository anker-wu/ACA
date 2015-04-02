#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TempModelConvert.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: TempModelConvert.cs 278233 2014-08-29 08:52:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// a temporary class used to convert model between EJB model and WS model.
    /// </summary>
    public static class TempModelConvert
    {
        /// <summary>
        /// converts cap id WS model to cap id EJB model.
        /// </summary>
        /// <param name="capIDModel4WS">cap id WS model</param>
        /// <returns>cap id EJB model</returns>
        public static CapIDModel Trim4WSOfCapIDModel(CapIDModel4WS capIDModel4WS)
        {
            if (capIDModel4WS == null)
            {
                return null;
            }

            CapIDModel capIDModel = new CapIDModel();
            capIDModel.customID = capIDModel4WS.customID;
            capIDModel.ID1 = capIDModel4WS.id1;
            capIDModel.ID2 = capIDModel4WS.id2;
            capIDModel.ID3 = capIDModel4WS.id3;
            capIDModel.serviceProviderCode = capIDModel4WS.serviceProviderCode;
            capIDModel.trackingID = capIDModel4WS.trackingID;
            return capIDModel;
        }

        /// <summary>
        /// convert cap id EJB model to cap id WS model.
        /// </summary>
        /// <param name="capIDModel">cap id EJB model</param>
        /// <returns>cap id WS model</returns>
        public static CapIDModel4WS Add4WSForCapIDModel(CapIDModel capIDModel)
        {
            if (capIDModel == null)
            {
                return null;
            }

            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.customID = capIDModel.customID;
            capIDModel4WS.id1 = capIDModel.ID1;
            capIDModel4WS.id2 = capIDModel.ID2;
            capIDModel4WS.id3 = capIDModel.ID3;
            capIDModel4WS.serviceProviderCode = capIDModel.serviceProviderCode;
            capIDModel4WS.trackingID = capIDModel.trackingID;
            return capIDModel4WS;
        }

        /// <summary>
        /// convert cap id WS models to cap id EJB models.
        /// </summary>
        /// <param name="capIDModel4WSes">cap id WS models</param>
        /// <returns>cap id EJB models</returns>
        public static CapIDModel[] Trim4WSOfCapIDModels(CapIDModel4WS[] capIDModel4WSes)
        {
            if (capIDModel4WSes == null || capIDModel4WSes.Length <= 0)
            {
                return null;
            }

            CapIDModel[] capIDModels = new CapIDModel[capIDModel4WSes.Length];

            for (int i = 0; i < capIDModel4WSes.Length; i++)
            {
                capIDModels[i] = Trim4WSOfCapIDModel(capIDModel4WSes[i]);
            }

            return capIDModels;
        }

        /// <summary>
        /// converts cap id EJB models to WS models
        /// </summary>
        /// <param name="capIDModels">cap id EJB models</param>
        /// <returns>cap id WS models</returns>
        public static CapIDModel4WS[] Add4WSForCapIDModels(CapIDModel[] capIDModels)
        {
            if (capIDModels == null || capIDModels.Length <= 0)
            {
                return null;
            }

            CapIDModel4WS[] capIDModel4WSes = new CapIDModel4WS[capIDModels.Length];
            for (int i = 0; i < capIDModels.Length; i++)
            {
                capIDModel4WSes[i] = Add4WSForCapIDModel(capIDModels[i]);
            }

            return capIDModel4WSes;
        }

        /// <summary>
        /// Convert to LicenseProfessionalModel list.
        /// </summary>
        /// <param name="licenseProfessionalModel4WSList">The LicenseProfessionalModel4WS list.</param>
        /// <returns>Return the LicenseProfessionalModel list.</returns>
        public static LicenseProfessionalModel[] ConvertToLicenseProfessionalModelList(LicenseProfessionalModel4WS[] licenseProfessionalModel4WSList)
        {
            if (licenseProfessionalModel4WSList == null || licenseProfessionalModel4WSList.Length == 0)
            {
                return null;
            }

            LicenseProfessionalModel[] models = new LicenseProfessionalModel[licenseProfessionalModel4WSList.Length];

            for (int i = 0; i < licenseProfessionalModel4WSList.Length; i++)
            {
                models[i] = ConvertToLicenseProfessionalModel(licenseProfessionalModel4WSList[i]);
            }

            return models;
        }

        /// <summary>
        /// Convert AppSpecificInfoModel4WS to AppSpecificInfoModel
        /// </summary>
        /// <param name="appSpecificInfoModel4WS">Application Specific Info Model for web service</param>
        /// <returns>The AppSpecificInfoModel</returns>
        public static AppSpecificInfoModel ConvertToAppSpecificInfoModel(AppSpecificInfoModel4WS appSpecificInfoModel4WS)
        {
            if (appSpecificInfoModel4WS == null)
            {
                return null;
            }

            AppSpecificInfoModel appSpecificInfoModel = new AppSpecificInfoModel();
            appSpecificInfoModel.actStatus = appSpecificInfoModel4WS.actStatus;
            appSpecificInfoModel.alignment = appSpecificInfoModel4WS.alignment;
            appSpecificInfoModel.alternativeLabel = appSpecificInfoModel4WS.alternativeLabel;
            appSpecificInfoModel.attributeUnitType = appSpecificInfoModel4WS.attributeUnitType;
            appSpecificInfoModel.attributeValue = appSpecificInfoModel4WS.attributeValue;
            appSpecificInfoModel.attributeValueReqFlag = appSpecificInfoModel4WS.attributeValueReqFlag;
            appSpecificInfoModel.auditid = appSpecificInfoModel4WS.auditid;
            appSpecificInfoModel.auditStatus = appSpecificInfoModel4WS.auditStatus;
            appSpecificInfoModel.checkboxDesc = appSpecificInfoModel4WS.checkboxDesc;
            appSpecificInfoModel.checkboxInd = appSpecificInfoModel4WS.checkboxInd;
            appSpecificInfoModel.checkboxType = appSpecificInfoModel4WS.checkboxType;
            appSpecificInfoModel.checklistComment = appSpecificInfoModel4WS.checklistComment;
            appSpecificInfoModel.displayOrder = appSpecificInfoModel4WS.displayOrder;
            appSpecificInfoModel.feeIndicator = appSpecificInfoModel4WS.feeIndicator;
            appSpecificInfoModel.fromAdmin = appSpecificInfoModel4WS.fromAdmin;
            appSpecificInfoModel.groupCode = appSpecificInfoModel4WS.groupCode;
            appSpecificInfoModel.permitID1 = appSpecificInfoModel4WS.permitID1;
            appSpecificInfoModel.permitID2 = appSpecificInfoModel4WS.permitID2;
            appSpecificInfoModel.permitID3 = appSpecificInfoModel4WS.permitID3;
            appSpecificInfoModel.perSubType = appSpecificInfoModel4WS.perSubType;
            appSpecificInfoModel.perType = appSpecificInfoModel4WS.perType;
            appSpecificInfoModel.requiredFeeCalc = appSpecificInfoModel4WS.requiredFeeCalc;
            appSpecificInfoModel.serviceProviderCode = appSpecificInfoModel4WS.serviceProviderCode;
            appSpecificInfoModel.vchDispFlag = appSpecificInfoModel4WS.vchDispFlag;

            return appSpecificInfoModel;
        }

        /// <summary>
        /// Convert to LicenseProfessionalModel.
        /// </summary>
        /// <param name="licenseProfessionalModel4WS">The licenseProfessionalModel4WS.</param>
        /// <returns>Return the LicenseProfessionalModel.</returns>
        public static LicenseProfessionalModel ConvertToLicenseProfessionalModel(LicenseProfessionalModel4WS licenseProfessionalModel4WS)
        {
            if (licenseProfessionalModel4WS == null)
            {
                return null;
            }

            LicenseProfessionalModel model = new LicenseProfessionalModel();

            model.address1 = licenseProfessionalModel4WS.address1;
            model.address2 = licenseProfessionalModel4WS.address2;
            model.address3 = licenseProfessionalModel4WS.address3;
            model.agencyCode = licenseProfessionalModel4WS.agencyCode;
            model.attributes = licenseProfessionalModel4WS.attributes;
            model.auditDate = ConvertToDateTimeNullable(licenseProfessionalModel4WS.auditDate);
            model.auditID = licenseProfessionalModel4WS.auditID;
            model.auditStatus = licenseProfessionalModel4WS.auditStatus;
            model.birthDate = ConvertToDateTimeNullable(licenseProfessionalModel4WS.birthDate);
            model.businessLicense = licenseProfessionalModel4WS.businessLicense;
            model.businessName = licenseProfessionalModel4WS.businessName;
            model.busName2 = licenseProfessionalModel4WS.busName2;
            model.capID = Trim4WSOfCapIDModel(licenseProfessionalModel4WS.capID);
            model.city = licenseProfessionalModel4WS.city;
            model.cityCode = licenseProfessionalModel4WS.cityCode;
            model.classCode = licenseProfessionalModel4WS.classCode;
            model.contactFirstName = licenseProfessionalModel4WS.contactFirstName;
            model.contactLastName = licenseProfessionalModel4WS.contactLastName;
            model.contactMiddleName = licenseProfessionalModel4WS.contactMiddleName;
            model.contactType = licenseProfessionalModel4WS.contactType;
            model.contLicBusName = licenseProfessionalModel4WS.contLicBusName;
            model.contrLicNo = licenseProfessionalModel4WS.contrLicNo;
            model.country = licenseProfessionalModel4WS.country;
            model.countryCode = licenseProfessionalModel4WS.countryCode;
            model.createByACA = licenseProfessionalModel4WS.createByACA;
            model.einSs = licenseProfessionalModel4WS.einSs;
            model.email = licenseProfessionalModel4WS.email;
            model.endbirthDate = ConvertToDateTimeNullable(licenseProfessionalModel4WS.endbirthDate);
            model.fax = licenseProfessionalModel4WS.fax;
            model.faxCountryCode = licenseProfessionalModel4WS.faxCountryCode;
            model.fein = licenseProfessionalModel4WS.fein;
            model.gender = licenseProfessionalModel4WS.gender;
            model.holdCode = licenseProfessionalModel4WS.holdCode;
            model.holdDesc = licenseProfessionalModel4WS.holdDesc;
            model.lastRenewalDate = ConvertToDateTimeNullable(licenseProfessionalModel4WS.lastRenewalDate);
            model.lastUpdateDate = ConvertToDateTimeNullable(licenseProfessionalModel4WS.lastUpdateDate);
            model.licenseBoard = string.Empty;
            model.licenseExpirDate = ConvertToDateTimeNullable(licenseProfessionalModel4WS.licenseExpirDate);
            model.licenseNbr = licenseProfessionalModel4WS.licenseNbr;
            model.licenseType = licenseProfessionalModel4WS.licenseType;
            model.licesnseOrigIssueDate = ConvertToDateTimeNullable(licenseProfessionalModel4WS.licesnseOrigIssueDate);
            model.licSeqNbr = licenseProfessionalModel4WS.licSeqNbr;
            model.maskedSsn = licenseProfessionalModel4WS.maskedSsn;
            model.phone1 = licenseProfessionalModel4WS.phone1;
            model.phone1CountryCode = licenseProfessionalModel4WS.phone1CountryCode;
            model.phone2 = licenseProfessionalModel4WS.phone2;
            model.phone2CountryCode = licenseProfessionalModel4WS.phone2CountryCode;
            model.postOfficeBox = licenseProfessionalModel4WS.postOfficeBox;
            model.primStatusCode = licenseProfessionalModel4WS.primStatusCode;
            model.printFlag = licenseProfessionalModel4WS.printFlag;
            model.relatedTradeLic = licenseProfessionalModel4WS.relatedTradeLic;
            model.resLicenseType = licenseProfessionalModel4WS.resLicenseType;
            model.resState = licenseProfessionalModel4WS.resState;
            model.salutation = licenseProfessionalModel4WS.salutation;
            model.selfIns = licenseProfessionalModel4WS.selfIns;
            model.serDes = licenseProfessionalModel4WS.serDes;
            model.socialSecurityNumber = licenseProfessionalModel4WS.socialSecurityNumber;
            model.state = licenseProfessionalModel4WS.state;
            model.suffixName = licenseProfessionalModel4WS.suffixName;
            model.templateAttributes = licenseProfessionalModel4WS.attributes;
            model.typeFlag = licenseProfessionalModel4WS.typeFlag;
            model.workCompExempt = licenseProfessionalModel4WS.workCompExempt;
            model.zip = licenseProfessionalModel4WS.zip;
            model.componentName = licenseProfessionalModel4WS.componentName;
            model.TemporaryID = licenseProfessionalModel4WS.TemporaryID;

            return model;
        }

        /// <summary>
        /// Convert to LicenseProfessionalModel list.
        /// </summary>
        /// <param name="licenseProfessionalModelList">The licenseProfessionalModelList list.</param>
        /// <returns>Return the LicenseProfessionalModel list.</returns>
        public static LicenseProfessionalModel4WS[] ConvertToLicenseProfessionalModel4WSList(LicenseProfessionalModel[] licenseProfessionalModelList)
        {
            if (licenseProfessionalModelList == null || licenseProfessionalModelList.Length == 0)
            {
                return null;
            }

            LicenseProfessionalModel4WS[] model4WSList = new LicenseProfessionalModel4WS[licenseProfessionalModelList.Length];

            for (int i = 0; i < licenseProfessionalModelList.Length; i++)
            {
                model4WSList[i] = ConvertToLicenseProfessionalModel4WS(licenseProfessionalModelList[i]);
            }

            return model4WSList;
        }

        /// <summary>
        /// Convert to LicenseProfessionalModel4WS.
        /// </summary>
        /// <param name="licenseProfessionalModel">The licenseProfessionalModel.</param>
        /// <returns>Return the LicenseProfessionalModel4WS.</returns>
        public static LicenseProfessionalModel4WS ConvertToLicenseProfessionalModel4WS(LicenseProfessionalModel licenseProfessionalModel)
        {
            LicenseProfessionalModel4WS model4WS = new LicenseProfessionalModel4WS();

            if (licenseProfessionalModel != null)
            {
                model4WS.address1 = licenseProfessionalModel.address1;
                model4WS.address2 = licenseProfessionalModel.address2;
                model4WS.address3 = licenseProfessionalModel.address3;
                model4WS.agencyCode = licenseProfessionalModel.agencyCode;
                model4WS.attributes = licenseProfessionalModel.templateAttributes;
                model4WS.auditDate = licenseProfessionalModel.auditDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForWebService(licenseProfessionalModel.auditDate.Value);
                model4WS.auditID = licenseProfessionalModel.auditID;
                model4WS.auditStatus = licenseProfessionalModel.auditStatus;
                model4WS.birthDate = licenseProfessionalModel.birthDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForWebService(licenseProfessionalModel.birthDate.Value);
                model4WS.businessLicense = licenseProfessionalModel.businessLicense;
                model4WS.businessName = licenseProfessionalModel.businessName;
                model4WS.busName2 = licenseProfessionalModel.busName2;
                model4WS.capID = Add4WSForCapIDModel(licenseProfessionalModel.capID);
                model4WS.city = licenseProfessionalModel.city;
                model4WS.cityCode = licenseProfessionalModel.cityCode;
                model4WS.classCode = licenseProfessionalModel.classCode;
                model4WS.componentName = licenseProfessionalModel.componentName;
                model4WS.contactFirstName = licenseProfessionalModel.contactFirstName;
                model4WS.contactLastName = licenseProfessionalModel.contactLastName;
                model4WS.contactMiddleName = licenseProfessionalModel.contactMiddleName;
                model4WS.contactType = licenseProfessionalModel.contactType;
                model4WS.contLicBusName = licenseProfessionalModel.contLicBusName;
                model4WS.contrLicNo = licenseProfessionalModel.contrLicNo;
                model4WS.country = licenseProfessionalModel.country;
                model4WS.countryCode = licenseProfessionalModel.countryCode;
                model4WS.createByACA = licenseProfessionalModel.createByACA;
                model4WS.einSs = licenseProfessionalModel.einSs;
                model4WS.email = licenseProfessionalModel.email;
                model4WS.endbirthDate = licenseProfessionalModel.endbirthDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForWebService(licenseProfessionalModel.endbirthDate.Value);
                model4WS.fax = licenseProfessionalModel.fax;
                model4WS.faxCountryCode = licenseProfessionalModel.faxCountryCode;
                model4WS.fein = licenseProfessionalModel.fein;
                model4WS.gender = licenseProfessionalModel.gender;
                model4WS.holdCode = licenseProfessionalModel.holdCode;
                model4WS.holdDesc = licenseProfessionalModel.holdDesc;
                model4WS.lastRenewalDate = licenseProfessionalModel.lastRenewalDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForWebService(licenseProfessionalModel.lastRenewalDate.Value);
                model4WS.lastUpdateDate = licenseProfessionalModel.lastUpdateDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForWebService(licenseProfessionalModel.lastUpdateDate.Value);
                model4WS.licenseExpirDate = licenseProfessionalModel.licenseExpirDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForWebService(licenseProfessionalModel.licenseExpirDate.Value);
                model4WS.licenseNbr = licenseProfessionalModel.licenseNbr;
                model4WS.licenseType = licenseProfessionalModel.licenseType;
                model4WS.licesnseOrigIssueDate = licenseProfessionalModel.licesnseOrigIssueDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForWebService(licenseProfessionalModel.licesnseOrigIssueDate.Value);
                model4WS.licSeqNbr = licenseProfessionalModel.licSeqNbr;
                model4WS.maskedSsn = licenseProfessionalModel.maskedSsn;
                model4WS.phone1 = licenseProfessionalModel.phone1;
                model4WS.phone1CountryCode = licenseProfessionalModel.phone1CountryCode;
                model4WS.phone2 = licenseProfessionalModel.phone2;
                model4WS.phone2CountryCode = licenseProfessionalModel.phone2CountryCode;
                model4WS.postOfficeBox = licenseProfessionalModel.postOfficeBox;
                model4WS.primStatusCode = licenseProfessionalModel.primStatusCode;
                model4WS.printFlag = licenseProfessionalModel.printFlag;
                model4WS.relatedTradeLic = licenseProfessionalModel.relatedTradeLic;
                model4WS.resLicenseType = licenseProfessionalModel.resLicenseType;
                model4WS.resState = licenseProfessionalModel.resState;
                model4WS.salutation = licenseProfessionalModel.salutation;
                model4WS.selfIns = licenseProfessionalModel.selfIns;
                model4WS.serDes = licenseProfessionalModel.serDes;
                model4WS.socialSecurityNumber = licenseProfessionalModel.socialSecurityNumber;
                model4WS.state = licenseProfessionalModel.state;
                model4WS.suffixName = licenseProfessionalModel.suffixName;
                model4WS.typeFlag = licenseProfessionalModel.typeFlag;
                model4WS.workCompExempt = licenseProfessionalModel.workCompExempt;
                model4WS.zip = licenseProfessionalModel.zip;
                model4WS.componentName = licenseProfessionalModel.componentName;
                model4WS.TemporaryID = licenseProfessionalModel.TemporaryID;
            }

            return model4WS;
        }

        /// <summary>
        /// Convert DateTime string to DateTime.
        /// </summary>
        /// <param name="datetimeString">The string of date time.</param>
        /// <returns>The date time.</returns>
        public static DateTime? ConvertToDateTimeNullable(string datetimeString)
        {
            DateTime? result = null;

            if (!string.IsNullOrEmpty(datetimeString))
            {
                DateTime tempDateTime;
                bool isDateTime = I18nDateTimeUtil.TryParseFromWebService(datetimeString, out tempDateTime);

                if (isDateTime && tempDateTime != DateTime.MinValue)
                {
                    result = tempDateTime;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert to people model
        /// </summary>
        /// <param name="peopleModel4WS">The WS of people model.</param>
        /// <param name="isForView">Is for view </param>
        /// <returns>
        /// The people model.
        /// </returns>
        public static PeopleModel ConvertToPeopleModel(PeopleModel4WS peopleModel4WS, bool isForView = false)
        {
            if (peopleModel4WS == null)
            {
                return null;
            }

            PeopleModel peopleModel = new PeopleModel();
            peopleModel.auditStatus = peopleModel4WS.auditStatus;
            peopleModel.birthDate = string.IsNullOrEmpty(peopleModel4WS.birthDate) ? (DateTime?)null : I18nDateTimeUtil.ParseFromWebService(peopleModel4WS.birthDate);
            peopleModel.businessName = peopleModel4WS.businessName;
            peopleModel.businessName2 = peopleModel4WS.businessName2;
            peopleModel.comment = peopleModel4WS.comment;
            peopleModel.contactSeqNumber = peopleModel4WS.contactSeqNumber;
            peopleModel.contactType = peopleModel4WS.contactType;
            peopleModel.contactTypeFlag = peopleModel4WS.contactTypeFlag;
            peopleModel.country = peopleModel4WS.country;
            peopleModel.countryCode = peopleModel4WS.countryCode;
            peopleModel.email = peopleModel4WS.email;
            peopleModel.endBirthDate = string.IsNullOrEmpty(peopleModel4WS.endBirthDate) ? (DateTime?)null : I18nDateTimeUtil.ParseFromWebService(peopleModel4WS.endBirthDate);

            if (!string.IsNullOrEmpty(peopleModel4WS.endDeceasedDate))
            {
                peopleModel.endDeceasedDate = I18nDateTimeUtil.ParseFromWebService(peopleModel4WS.endDeceasedDate);
            }

            peopleModel.endDate = string.IsNullOrEmpty(peopleModel4WS.endDate) ? (DateTime?)null : I18nDateTimeUtil.ParseFromWebService(peopleModel4WS.endDate);
            peopleModel.fax = peopleModel4WS.fax;
            peopleModel.faxCountryCode = peopleModel4WS.faxCountryCode;
            peopleModel.fein = peopleModel4WS.fein;
            peopleModel.firstName = peopleModel4WS.firstName;
            peopleModel.flag = peopleModel4WS.flag;

            if (!string.IsNullOrWhiteSpace(peopleModel4WS.fullName))
            {
                peopleModel.fullName = peopleModel4WS.fullName;
            }
            else if (isForView)
            {
                //Only for view should format full name with first name, middle name and last name.
                string[] fullName = { peopleModel4WS.firstName, peopleModel4WS.middleName, peopleModel4WS.lastName };
                peopleModel.fullName = DataUtil.ConcatStringWithSplitChar(fullName, ACAConstant.BLANK);
            }

            peopleModel.gender = peopleModel4WS.gender;
            peopleModel.holdCode = peopleModel4WS.holdCode;
            peopleModel.holdDescription = peopleModel4WS.holdDescription;
            peopleModel.id = peopleModel4WS.id;
            peopleModel.ivrPinNumber = peopleModel4WS.ivrPinNumber;
            peopleModel.ivrUserNumber = peopleModel4WS.ivrUserNumber;
            peopleModel.lastName = peopleModel4WS.lastName;
            peopleModel.maskedSsn = peopleModel4WS.maskedSsn;
            peopleModel.middleName = peopleModel4WS.middleName;
            peopleModel.namesuffix = peopleModel4WS.namesuffix;
            peopleModel.phone1 = peopleModel4WS.phone1;
            peopleModel.phone1CountryCode = peopleModel4WS.phone1CountryCode;
            peopleModel.phone2 = peopleModel4WS.phone2;
            peopleModel.phone2CountryCode = peopleModel4WS.phone2CountryCode;
            peopleModel.phone3 = peopleModel4WS.phone3;
            peopleModel.phone3CountryCode = peopleModel4WS.phone3CountryCode;
            peopleModel.postOfficeBox = peopleModel4WS.postOfficeBox;
            peopleModel.preferredChannel = string.IsNullOrEmpty(peopleModel4WS.preferredChannel) ? (int?)null : int.Parse(peopleModel4WS.preferredChannel);
            peopleModel.preferredChannelString = peopleModel4WS.preferredChannelString;
            peopleModel.relation = peopleModel4WS.relation;
            peopleModel.salutation = peopleModel4WS.salutation;
            peopleModel.serviceProviderCode = peopleModel4WS.serviceProviderCode;
            peopleModel.socialSecurityNumber = peopleModel4WS.socialSecurityNumber;
            peopleModel.title = peopleModel4WS.title;
            peopleModel.tradeName = peopleModel4WS.tradeName;
            peopleModel.birthCity = peopleModel4WS.birthCity;
            peopleModel.birthState = peopleModel4WS.birthState;
            peopleModel.birthRegion = peopleModel4WS.birthRegion;
            peopleModel.deceasedDate = string.IsNullOrEmpty(peopleModel4WS.deceasedDate) ? (DateTime?)null : I18nDateTimeUtil.ParseFromWebService(peopleModel4WS.deceasedDate);
            peopleModel.race = peopleModel4WS.race;
            peopleModel.passportNumber = peopleModel4WS.passportNumber;
            peopleModel.driverLicenseNbr = peopleModel4WS.driverLicenseNbr;
            peopleModel.driverLicenseState = peopleModel4WS.driverLicenseState;
            peopleModel.stateIDNbr = peopleModel4WS.stateIDNbr;
            peopleModel.contractorPeopleStatus = peopleModel4WS.contractorPeopleStatus;
            peopleModel.accountOwner = peopleModel4WS.accountOwner;
            CompactAddressModel compactAddress = new CompactAddressModel();

            if (peopleModel4WS.compactAddress != null)
            {
                compactAddress.addressLine1 = peopleModel4WS.compactAddress.addressLine1;
                compactAddress.addressLine2 = peopleModel4WS.compactAddress.addressLine2;
                compactAddress.addressLine3 = peopleModel4WS.compactAddress.addressLine3;
                compactAddress.city = peopleModel4WS.compactAddress.city;
                compactAddress.country = peopleModel4WS.compactAddress.country;
                compactAddress.countryCode = peopleModel4WS.compactAddress.countryCode;
                compactAddress.state = peopleModel4WS.compactAddress.state;
                compactAddress.streetName = peopleModel4WS.compactAddress.streetName;
                compactAddress.zip = peopleModel4WS.compactAddress.zip;

                peopleModel.compactAddress = compactAddress;
            }

            peopleModel.contactAddressLists = peopleModel4WS.contactAddressList;
            peopleModel.template = peopleModel4WS.template;
            peopleModel.attributes = peopleModel4WS.attributes;
            peopleModel.peopleAKAList = peopleModel4WS.peopleAKAList;
            peopleModel.ConditionParameters = peopleModel4WS.ConditionParameters;
            peopleModel.agencyAliasName = GetAgencyDisplayText(peopleModel4WS);

            return peopleModel;
        }

        /// <summary>
        /// Convert people EJB model to people model WS
        /// </summary>
        /// <param name="peopleModel">The people EJB model</param>
        /// <returns>The People WS Model.</returns>
        public static PeopleModel4WS ConvertToPeopleModel4WS(PeopleModel peopleModel)
        {
            if (peopleModel == null)
            {
                return null;
            }

            PeopleModel4WS peopleModel4WS = new PeopleModel4WS();
            peopleModel4WS.auditStatus = peopleModel.auditStatus;
            peopleModel4WS.birthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(peopleModel.birthDate);
            peopleModel4WS.businessName = peopleModel.businessName;
            peopleModel4WS.businessName2 = peopleModel.businessName2;
            peopleModel4WS.comment = peopleModel.comment;
            peopleModel4WS.contactSeqNumber = peopleModel.contactSeqNumber;
            peopleModel4WS.contactType = peopleModel.contactType;
            peopleModel4WS.contactTypeFlag = peopleModel.contactTypeFlag;
            peopleModel4WS.country = peopleModel.country;
            peopleModel4WS.countryCode = peopleModel.countryCode;
            peopleModel4WS.email = peopleModel.email;
            peopleModel4WS.endBirthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(peopleModel.endBirthDate);
            peopleModel4WS.endDeceasedDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(peopleModel.endDeceasedDate);
            peopleModel4WS.endDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(peopleModel.endDate);
            peopleModel4WS.fax = peopleModel.fax;
            peopleModel4WS.faxCountryCode = peopleModel.faxCountryCode;
            peopleModel4WS.fein = peopleModel.fein;
            peopleModel4WS.firstName = peopleModel.firstName;
            peopleModel4WS.flag = peopleModel.flag;
            peopleModel4WS.fullName = peopleModel.fullName;
            peopleModel4WS.gender = peopleModel.gender;
            peopleModel4WS.holdCode = peopleModel.holdCode;
            peopleModel4WS.holdDescription = peopleModel.holdDescription;
            peopleModel4WS.id = peopleModel.id;
            peopleModel4WS.ivrPinNumber = peopleModel.ivrPinNumber;
            peopleModel4WS.ivrUserNumber = peopleModel.ivrUserNumber;
            peopleModel4WS.lastName = peopleModel.lastName;
            peopleModel4WS.maskedSsn = peopleModel.maskedSsn;
            peopleModel4WS.middleName = peopleModel.middleName;
            peopleModel4WS.namesuffix = peopleModel.namesuffix;
            peopleModel4WS.phone1 = peopleModel.phone1;
            peopleModel4WS.phone1CountryCode = peopleModel.phone1CountryCode;
            peopleModel4WS.phone2 = peopleModel.phone2;
            peopleModel4WS.phone2CountryCode = peopleModel.phone2CountryCode;
            peopleModel4WS.phone3 = peopleModel.phone3;
            peopleModel4WS.phone3CountryCode = peopleModel.phone3CountryCode;
            peopleModel4WS.postOfficeBox = peopleModel.postOfficeBox;
            peopleModel4WS.preferredChannel = peopleModel.preferredChannel == null ? string.Empty : peopleModel.preferredChannel.ToString();
            peopleModel4WS.preferredChannelString = peopleModel.preferredChannelString;
            peopleModel4WS.relation = peopleModel.relation;
            peopleModel4WS.salutation = peopleModel.salutation;
            peopleModel4WS.serviceProviderCode = peopleModel.serviceProviderCode;
            peopleModel4WS.socialSecurityNumber = peopleModel.socialSecurityNumber;
            peopleModel4WS.title = peopleModel.title;
            peopleModel4WS.tradeName = peopleModel.tradeName;
            peopleModel4WS.birthCity = peopleModel.birthCity;
            peopleModel4WS.birthState = peopleModel.birthState;
            peopleModel4WS.birthRegion = peopleModel.birthRegion;
            peopleModel4WS.deceasedDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(peopleModel.deceasedDate);
            peopleModel4WS.race = peopleModel.race;
            peopleModel4WS.passportNumber = peopleModel.passportNumber;
            peopleModel4WS.driverLicenseNbr = peopleModel.driverLicenseNbr;
            peopleModel4WS.driverLicenseState = peopleModel.driverLicenseState;
            peopleModel4WS.stateIDNbr = peopleModel.stateIDNbr;
            peopleModel4WS.contractorPeopleStatus = peopleModel.contractorPeopleStatus;
            peopleModel4WS.accountOwner = peopleModel.accountOwner;
            CompactAddressModel4WS compactAddress4WS = new CompactAddressModel4WS();

            if (peopleModel.compactAddress != null)
            {
                compactAddress4WS.addressLine1 = peopleModel.compactAddress.addressLine1;
                compactAddress4WS.addressLine2 = peopleModel.compactAddress.addressLine2;
                compactAddress4WS.addressLine3 = peopleModel.compactAddress.addressLine3;
                compactAddress4WS.city = peopleModel.compactAddress.city;
                compactAddress4WS.country = peopleModel.compactAddress.country;
                compactAddress4WS.countryCode = peopleModel.compactAddress.countryCode;
                compactAddress4WS.state = peopleModel.compactAddress.state;
                compactAddress4WS.streetName = peopleModel.compactAddress.streetName;
                compactAddress4WS.zip = peopleModel.compactAddress.zip;

                peopleModel4WS.compactAddress = compactAddress4WS;
            }

            peopleModel4WS.contactAddressList = peopleModel.contactAddressLists;
            peopleModel4WS.template = peopleModel.template;
            peopleModel4WS.peopleAKAList = peopleModel.peopleAKAList;

            List<TemplateAttributeModel> attributes = null;

            if (peopleModel.attributes != null)
            {
                attributes = new List<TemplateAttributeModel>();

                foreach (object attribute in peopleModel.attributes)
                {
                    attributes.Add((TemplateAttributeModel)attribute);
                }
            }

            peopleModel4WS.attributes = attributes == null ? null : attributes.ToArray();
            peopleModel4WS.ConditionParameters = peopleModel.ConditionParameters;

            return peopleModel4WS;
        }

        /// <summary>
        /// Convert People WS Model List to People EJB model List.
        /// </summary>
        /// <param name="peopleModel4WSArray">The people WS model list.</param>
        /// <param name="isForView">Is for view </param>
        /// <returns>The people EJB model list.</returns>
        public static List<PeopleModel> ConvertToPeopleModel(PeopleModel4WS[] peopleModel4WSArray, bool isForView = false)
        {
            if (peopleModel4WSArray == null || peopleModel4WSArray.Length < 1)
            {
                return null;
            }

            List<PeopleModel> peopleModelList = new List<PeopleModel>();

            for (int i = 0; i < peopleModel4WSArray.Length; i++)
            {
                peopleModelList.Add(ConvertToPeopleModel(peopleModel4WSArray[i], isForView));
            }

            return peopleModelList;
        }

        /// <summary>
        /// Convert people model to peopleModel4WS.
        /// </summary>
        /// <param name="peopleModelArray">the PeopleModel list.</param>
        /// <returns>the PeopleModel4WS list.</returns>
        public static PeopleModel4WS[] ConvertToPeopleModel4WS(PeopleModel[] peopleModelArray)
        {
            if (peopleModelArray == null || peopleModelArray.Length < 1)
            {
                return null;
            }

            List<PeopleModel4WS> peopleModel4WSList = new List<PeopleModel4WS>();

            for (int i = 0; i < peopleModelArray.Length; i++)
            {
                peopleModel4WSList.Add(ConvertToPeopleModel4WS(peopleModelArray[i]));
            }

            if (peopleModel4WSList == null || peopleModel4WSList.Count < 1)
            {
                return null;
            }

            return peopleModel4WSList.ToArray();
        }

        /// <summary>
        /// Convert EducationModel4WS to EducationModel4WS
        /// </summary>
        /// <param name="educationModel">The Education model</param>
        /// <returns>The EducationModel4WS.</returns>
        public static EducationModel4WS ConvertToEducationModel4WS(EducationModel educationModel)
        {
            if (educationModel == null)
            {
                return null;
            }

            EducationModel4WS educationModel4WS = new EducationModel4WS();
            educationModel4WS.approvedFlag = educationModel.approvedFlag;
            educationModel4WS.b1PerId1 = educationModel.b1PerId1;
            educationModel4WS.b1PerId2 = educationModel.b1PerId2;
            educationModel4WS.b1PerId3 = educationModel.b1PerId3;
            educationModel4WS.comments = educationModel.comments;

            if (educationModel.contactSeqNumber.HasValue)
            {
                educationModel4WS.contactSeqNumber = educationModel.contactSeqNumber.Value;
            }

            educationModel4WS.degree = educationModel.degree;
            educationModel4WS.educationName = educationModel.educationName;

            if (educationModel.entityID.HasValue)
            {
                educationModel4WS.entityID = educationModel.entityID;
            }

            educationModel4WS.entityType = educationModel.entityType;
            educationModel4WS.providerName = educationModel.providerName;
            educationModel4WS.providerNo = educationModel.providerNo;
            educationModel4WS.requiredFlag = educationModel.requiredFlag;
            educationModel4WS.syncFlag = educationModel.syncFlag;
            educationModel4WS.yearAttended = educationModel.yearAttended;
            educationModel4WS.yearGraduated = educationModel.yearGraduated;
            educationModel4WS.associatedEduCount = educationModel.associatedEduCount;

            if (educationModel.auditModel != null)
            {
                AuditModel4WS auditModel4WS = new AuditModel4WS();
                auditModel4WS.auditID = educationModel.auditModel.auditID;
                auditModel4WS.auditDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(educationModel.auditModel.auditDate);
                auditModel4WS.auditStatus = educationModel.auditModel.auditStatus;
                educationModel4WS.auditModel = auditModel4WS;
            }

            if (educationModel.educationPKModel != null)
            {
                EducationPKModel4WS educationPkModel4WS = new EducationPKModel4WS();
                educationPkModel4WS.educationNbr = educationModel.educationPKModel.educationNbr;
                educationPkModel4WS.serviceProviderCode = educationModel.educationPKModel.serviceProviderCode;
                educationModel4WS.educationPKModel = educationPkModel4WS;
            }

            if (educationModel.providerDetailModel != null)
            {
                ProviderDetailModel4WS providerDetailModel4WS = new ProviderDetailModel4WS();
                providerDetailModel4WS.address1 = educationModel.providerDetailModel.address1;
                providerDetailModel4WS.address2 = educationModel.providerDetailModel.address2;
                providerDetailModel4WS.address3 = educationModel.providerDetailModel.address3;
                providerDetailModel4WS.city = educationModel.providerDetailModel.city;
                providerDetailModel4WS.countryCode = educationModel.providerDetailModel.countryCode;
                providerDetailModel4WS.email = educationModel.providerDetailModel.email;
                providerDetailModel4WS.fax = educationModel.providerDetailModel.fax;
                providerDetailModel4WS.faxCountryCode = educationModel.providerDetailModel.faxCountryCode;
                providerDetailModel4WS.phone1 = educationModel.providerDetailModel.phone1;
                providerDetailModel4WS.phone1CountryCode = educationModel.providerDetailModel.phone1CountryCode;
                providerDetailModel4WS.phone2 = educationModel.providerDetailModel.phone2;
                providerDetailModel4WS.phone2CountryCode = educationModel.providerDetailModel.phone2CountryCode;
                providerDetailModel4WS.state = educationModel.providerDetailModel.state;
                providerDetailModel4WS.zip = educationModel.providerDetailModel.zip;
                educationModel4WS.providerDetailModel = providerDetailModel4WS;
            }

            educationModel4WS.template = educationModel.template;

            return educationModel4WS;
        }

        /// <summary>
        /// Convert Continuing EducationModel4WS to EducationModel4WS
        /// </summary>
        /// <param name="contEducationModel">The Continuing Education model</param>
        /// <returns>The Continuing ContinuingEducationModel4WS.</returns>
        public static ContinuingEducationModel4WS ConvertToContEducationModel4WS(ContinuingEducationModel contEducationModel)
        {
            if (contEducationModel == null)
            {
                return null;
            }

            ContinuingEducationModel4WS continuingEducation = new ContinuingEducationModel4WS();

            continuingEducation.approvedFlag = contEducationModel.approvedFlag;

            if (contEducationModel.auditModel != null)
            {
                AuditModel4WS audit = new AuditModel4WS();

                audit.auditDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(contEducationModel.auditModel.auditDate);
                audit.auditID = contEducationModel.auditModel.auditID;
                audit.auditStatus = contEducationModel.auditModel.auditStatus;

                continuingEducation.auditModel = audit;
            }

            continuingEducation.b1PerId1 = contEducationModel.b1PerId1;
            continuingEducation.b1PerId2 = contEducationModel.b1PerId2;
            continuingEducation.b1PerId3 = contEducationModel.b1PerId3;
            continuingEducation.className = contEducationModel.className;
            continuingEducation.comments = contEducationModel.comments;
            continuingEducation.contEduName = contEducationModel.contEduName;

            if (contEducationModel.contactSeqNumber.HasValue)
            {
                continuingEducation.contactSeqNumber = contEducationModel.contactSeqNumber.Value;
            }

            if (contEducationModel.continuingEducationPKModel != null)
            {
                ContinuingEducationPKModel4WS continuingEducationPk = new ContinuingEducationPKModel4WS();

                continuingEducationPk.contEduNbr = contEducationModel.continuingEducationPKModel.contEduNbr;
                continuingEducationPk.serviceProviderCode = contEducationModel.continuingEducationPKModel.serviceProviderCode;

                continuingEducation.continuingEducationPKModel = continuingEducationPk;
            }

            continuingEducation.dateOfClass = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(contEducationModel.dateOfClass);

            if (contEducationModel.entityID.HasValue)
            {
                continuingEducation.entityID = contEducationModel.entityID.ToString();
            }

            continuingEducation.entityType = contEducationModel.entityType;

            if (contEducationModel.finalScore.HasValue)
            {
                continuingEducation.finalScore = contEducationModel.finalScore.ToString();
            }

            continuingEducation.gradingStyle = contEducationModel.gradingStyle;

            if (contEducationModel.hoursCompleted.HasValue)
            {
                continuingEducation.hoursCompleted = contEducationModel.hoursCompleted.Value;
            }

            if (contEducationModel.passingScore.HasValue)
            {
                continuingEducation.passingScore = contEducationModel.passingScore.ToString();
            }

            if (contEducationModel.providerDetailModel != null)
            {
                ProviderDetailModel4WS providerDetail = new ProviderDetailModel4WS();

                providerDetail.address1 = contEducationModel.providerDetailModel.address1;
                providerDetail.address2 = contEducationModel.providerDetailModel.address2;
                providerDetail.address3 = contEducationModel.providerDetailModel.address3;
                providerDetail.city = contEducationModel.providerDetailModel.city;
                providerDetail.countryCode = contEducationModel.providerDetailModel.countryCode;
                providerDetail.email = contEducationModel.providerDetailModel.email;
                providerDetail.fax = contEducationModel.providerDetailModel.fax;
                providerDetail.faxCountryCode = contEducationModel.providerDetailModel.faxCountryCode;
                providerDetail.phone1 = contEducationModel.providerDetailModel.phone1;
                providerDetail.phone1CountryCode = contEducationModel.providerDetailModel.phone1CountryCode;
                providerDetail.phone2 = contEducationModel.providerDetailModel.phone2;
                providerDetail.phone2CountryCode = contEducationModel.providerDetailModel.phone2CountryCode;
                providerDetail.state = contEducationModel.providerDetailModel.state;
                providerDetail.zip = contEducationModel.providerDetailModel.zip;

                continuingEducation.providerDetailModel = providerDetail;
            }

            continuingEducation.providerName = contEducationModel.providerName;
            continuingEducation.providerNo = contEducationModel.providerNo;
            continuingEducation.requiredFlag = contEducationModel.requiredFlag;
            continuingEducation.syncFlag = contEducationModel.syncFlag;
            continuingEducation.template = contEducationModel.template;
            continuingEducation.associatedContEduCount = contEducationModel.associatedContEduCount;

            return continuingEducation;
        }

        /// <summary>
        /// Convert educationModel4WS to educationModel
        /// </summary>
        /// <param name="educationModel4WS">The education model 4WS.</param>
        /// <returns>The EducationModel.</returns>
        public static EducationModel ConvertToEducationModel(EducationModel4WS educationModel4WS)
        {
            if (educationModel4WS == null)
            {
                return null;
            }

            EducationModel educationModel = new EducationModel();
            educationModel.approvedFlag = educationModel4WS.approvedFlag;
            educationModel.b1PerId1 = educationModel4WS.b1PerId1;
            educationModel.b1PerId2 = educationModel4WS.b1PerId2;
            educationModel.b1PerId3 = educationModel4WS.b1PerId3;
            educationModel.comments = educationModel4WS.comments;
            educationModel.contactSeqNumber = educationModel4WS.contactSeqNumber;
            educationModel.degree = educationModel4WS.degree;
            educationModel.educationName = educationModel4WS.educationName;
            educationModel.entityID = educationModel4WS.entityID;
            educationModel.entityType = educationModel4WS.entityType;
            educationModel.providerName = educationModel4WS.providerName;
            educationModel.providerNo = educationModel4WS.providerNo;
            educationModel.requiredFlag = educationModel4WS.requiredFlag;
            educationModel.syncFlag = educationModel4WS.syncFlag;
            educationModel.yearAttended = educationModel4WS.yearAttended;
            educationModel.yearGraduated = educationModel4WS.yearGraduated;
            educationModel.associatedEduCount = educationModel4WS.associatedEduCount;

            if (educationModel4WS.auditModel != null)
            {
                SimpleAuditModel auditModel = new SimpleAuditModel();
                auditModel.auditID = educationModel4WS.auditModel.auditID;
                auditModel.auditDate = I18nDateTimeUtil.ParseFromWebService(educationModel4WS.auditModel.auditDate);
                auditModel.auditStatus = educationModel4WS.auditModel.auditStatus;
                educationModel.auditModel = auditModel;
            }

            if (educationModel4WS.educationPKModel != null)
            {
                EducationPKModel educationPkModel = new EducationPKModel();
                educationPkModel.educationNbr = educationModel4WS.educationPKModel.educationNbr;
                educationPkModel.serviceProviderCode = educationModel4WS.educationPKModel.serviceProviderCode;
                educationModel.educationPKModel = educationPkModel;
            }

            if (educationModel4WS.providerDetailModel != null)
            {
                ProviderDetailModel providerDetailModel = new ProviderDetailModel();
                providerDetailModel.address1 = educationModel4WS.providerDetailModel.address1;
                providerDetailModel.address2 = educationModel4WS.providerDetailModel.address2;
                providerDetailModel.address3 = educationModel4WS.providerDetailModel.address3;
                providerDetailModel.city = educationModel4WS.providerDetailModel.city;
                providerDetailModel.countryCode = educationModel4WS.providerDetailModel.countryCode;
                providerDetailModel.email = educationModel4WS.providerDetailModel.email;
                providerDetailModel.fax = educationModel4WS.providerDetailModel.fax;
                providerDetailModel.faxCountryCode = educationModel4WS.providerDetailModel.faxCountryCode;
                providerDetailModel.phone1 = educationModel4WS.providerDetailModel.phone1;
                providerDetailModel.phone1CountryCode = educationModel4WS.providerDetailModel.phone1CountryCode;
                providerDetailModel.phone2 = educationModel4WS.providerDetailModel.phone2;
                providerDetailModel.phone2CountryCode = educationModel4WS.providerDetailModel.phone2CountryCode;
                providerDetailModel.state = educationModel4WS.providerDetailModel.state;
                providerDetailModel.zip = educationModel4WS.providerDetailModel.zip;
                educationModel.providerDetailModel = providerDetailModel;
            }

            educationModel.template = educationModel4WS.template;

            return educationModel;
        }

        /// <summary>
        /// Convert educationModel4WS to educationModel
        /// </summary>
        /// <param name="contEducationModel4Ws">The continuing education model 4WS.</param>
        /// <returns>The EducationModel.</returns>
        public static ContinuingEducationModel ConvertToContEducationModel(ContinuingEducationModel4WS contEducationModel4Ws)
        {
            if (contEducationModel4Ws == null)
            {
                return null;
            }

            ContinuingEducationModel model = new ContinuingEducationModel();

            model.approvedFlag = contEducationModel4Ws.approvedFlag;

            if (contEducationModel4Ws.auditModel != null)
            {
                SimpleAuditModel auditModel = new SimpleAuditModel();

                auditModel.auditID = contEducationModel4Ws.auditModel.auditID;
                auditModel.auditDate = I18nDateTimeUtil.ParseFromWebService(contEducationModel4Ws.auditModel.auditDate);
                auditModel.auditStatus = contEducationModel4Ws.auditModel.auditStatus;

                model.auditModel = auditModel;
            }

            model.b1PerId1 = contEducationModel4Ws.b1PerId1;
            model.b1PerId2 = contEducationModel4Ws.b1PerId2;
            model.b1PerId3 = contEducationModel4Ws.b1PerId3;
            model.className = contEducationModel4Ws.className;
            model.comments = contEducationModel4Ws.comments;
            model.contEduName = contEducationModel4Ws.contEduName;
            model.contactSeqNumber = contEducationModel4Ws.contactSeqNumber;

            if (contEducationModel4Ws.continuingEducationPKModel != null)
            {
                ContinuingEducationPKModel contEducationPkModel = new ContinuingEducationPKModel();

                contEducationPkModel.contEduNbr = contEducationModel4Ws.continuingEducationPKModel.contEduNbr;
                contEducationPkModel.serviceProviderCode = contEducationModel4Ws.continuingEducationPKModel.serviceProviderCode;

                model.continuingEducationPKModel = contEducationPkModel;
            }

            model.dateOfClass = I18nDateTimeUtil.ParseFromUI(contEducationModel4Ws.dateOfClass);

            if (!string.IsNullOrEmpty(contEducationModel4Ws.entityID))
            {
                model.entityID = long.Parse(contEducationModel4Ws.entityID);
            }

            model.entityType = contEducationModel4Ws.entityType;

            if (!string.IsNullOrEmpty(contEducationModel4Ws.finalScore))
            {
                model.finalScore = double.Parse(contEducationModel4Ws.finalScore);
            }

            model.gradingStyle = contEducationModel4Ws.gradingStyle;
            model.hoursCompleted = contEducationModel4Ws.hoursCompleted;

            if (!string.IsNullOrEmpty(contEducationModel4Ws.passingScore))
            {
                model.passingScore = double.Parse(contEducationModel4Ws.passingScore);
            }

            if (contEducationModel4Ws.providerDetailModel != null)
            {
                ProviderDetailModel providerDetailModel = new ProviderDetailModel();

                providerDetailModel.address1 = contEducationModel4Ws.providerDetailModel.address1;
                providerDetailModel.address2 = contEducationModel4Ws.providerDetailModel.address2;
                providerDetailModel.address3 = contEducationModel4Ws.providerDetailModel.address3;
                providerDetailModel.city = contEducationModel4Ws.providerDetailModel.city;
                providerDetailModel.countryCode = contEducationModel4Ws.providerDetailModel.countryCode;
                providerDetailModel.email = contEducationModel4Ws.providerDetailModel.email;
                providerDetailModel.fax = contEducationModel4Ws.providerDetailModel.fax;
                providerDetailModel.faxCountryCode = contEducationModel4Ws.providerDetailModel.faxCountryCode;
                providerDetailModel.phone1 = contEducationModel4Ws.providerDetailModel.phone1;
                providerDetailModel.phone1CountryCode = contEducationModel4Ws.providerDetailModel.phone1CountryCode;
                providerDetailModel.phone2 = contEducationModel4Ws.providerDetailModel.phone2;
                providerDetailModel.phone2CountryCode = contEducationModel4Ws.providerDetailModel.phone2CountryCode;
                providerDetailModel.state = contEducationModel4Ws.providerDetailModel.state;
                providerDetailModel.zip = contEducationModel4Ws.providerDetailModel.zip;

                model.providerDetailModel = providerDetailModel;
            }

            model.providerName = contEducationModel4Ws.providerName;
            model.providerNo = contEducationModel4Ws.providerNo;
            model.requiredFlag = contEducationModel4Ws.requiredFlag;
            model.syncFlag = contEducationModel4Ws.syncFlag;
            model.template = contEducationModel4Ws.template;
            model.associatedContEduCount = contEducationModel4Ws.associatedContEduCount;

            return model;
        }

        /// <summary>
        /// Convert EducationModel array to EducationModel4WS array.
        /// </summary>
        /// <param name="educationModelArray">EducationModel array</param>
        /// <returns>EducationModel4WS array</returns>
        public static EducationModel4WS[] ConvertToEducationModel4WS(EducationModel[] educationModelArray)
        {
            if (educationModelArray == null || educationModelArray.Length < 1)
            {
                return null;
            }

            List<EducationModel4WS> educationModel4WSList = new List<EducationModel4WS>();

            for (int i = 0; i < educationModelArray.Length; i++)
            {
                educationModel4WSList.Add(ConvertToEducationModel4WS(educationModelArray[i]));
            }

            return educationModel4WSList.ToArray();
        }

        /// <summary>
        /// Convert ContinuingEducationModel array to ContinuingEducationModel4WS array.
        /// </summary>
        /// <param name="contEducationModelArray">ContinuingEducationModel array</param>
        /// <returns>ContinuingEducationModel4WS array</returns>
        public static ContinuingEducationModel4WS[] ConvertToContEducationModel4WS(ContinuingEducationModel[] contEducationModelArray)
        {
            if (contEducationModelArray == null || contEducationModelArray.Length < 1)
            {
                return null;
            }

            return contEducationModelArray.Select(ConvertToContEducationModel4WS).ToArray();
        }

        /// <summary>
        /// Convert EducationModel4WS to EducationModel.
        /// </summary>
        /// <param name="educationModel4WSArray">EducationModel4WS array</param>
        /// <returns>EducationModel array.</returns>
        public static EducationModel[] ConvertToEducationModel(EducationModel4WS[] educationModel4WSArray)
        {
            if (educationModel4WSArray == null || educationModel4WSArray.Length < 1)
            {
                return null;
            }

            List<EducationModel> educationModelList = new List<EducationModel>();

            for (int i = 0; i < educationModel4WSArray.Length; i++)
            {
                educationModelList.Add(ConvertToEducationModel(educationModel4WSArray[i]));
            }

            return educationModelList.ToArray();
        }

        /// <summary>
        /// Convert publicUserModel to publicUserModel4WS.
        /// </summary>
        /// <param name="publicUserModel">The public user model.</param>
        /// <returns>The PublicUserModel4WS.</returns>
        public static PublicUserModel4WS ConvertToPublicUserModel4WS(PublicUserModel publicUserModel)
        {
            if (publicUserModel == null)
            {
                return null;
            }

            PublicUserModel4WS publicUserModel4Ws = new PublicUserModel4WS();

            publicUserModel4Ws.UUID = publicUserModel.UUID;
            publicUserModel4Ws.accountType = publicUserModel.accountType;
            publicUserModel4Ws.address = publicUserModel.address;
            publicUserModel4Ws.address2 = publicUserModel.address2;
            publicUserModel4Ws.auditDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(publicUserModel.auditDate);
            publicUserModel4Ws.auditID = publicUserModel.auditID;
            publicUserModel4Ws.auditStatus = publicUserModel.auditStatus;
            publicUserModel4Ws.authAgentID = publicUserModel.authAgentID;
            publicUserModel4Ws.authServiceProviderCode = publicUserModel.authServiceProviderCode;
            publicUserModel4Ws.birthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(publicUserModel.birthDate);
            publicUserModel4Ws.businessName = publicUserModel.businessName;
            publicUserModel4Ws.cellPhone = publicUserModel.cellPhone;
            publicUserModel4Ws.cellPhoneCountryCode = publicUserModel.cellPhoneCountryCode;
            publicUserModel4Ws.city = publicUserModel.city;
            publicUserModel4Ws.cookie = publicUserModel.cookie;
            publicUserModel4Ws.country = publicUserModel.country;
            publicUserModel4Ws.email = publicUserModel.email;
            publicUserModel4Ws.enablePrint = publicUserModel.enablePrint;
            publicUserModel4Ws.fax = publicUserModel.fax;
            publicUserModel4Ws.faxCountryCode = publicUserModel.faxCountryCode;
            publicUserModel4Ws.fein = publicUserModel.fein;
            publicUserModel4Ws.firstName = publicUserModel.firstName;
            publicUserModel4Ws.gender = publicUserModel.gender;
            publicUserModel4Ws.homePhone = publicUserModel.homePhone;
            publicUserModel4Ws.homePhoneCountryCode = publicUserModel.homePhoneCountryCode;
            publicUserModel4Ws.lastName = publicUserModel.lastName;
            publicUserModel4Ws.maskedSsn = publicUserModel.maskedSsn;
            publicUserModel4Ws.middleName = publicUserModel.middleName;
            publicUserModel4Ws.needChangePassword = publicUserModel.needChangePassword;
            publicUserModel4Ws.pager = publicUserModel.pager;
            publicUserModel4Ws.password = publicUserModel.password;
            publicUserModel4Ws.passwordChangeDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(publicUserModel.passwordChangeDate);
            publicUserModel4Ws.passwordHint = publicUserModel.passwordHint;
            publicUserModel4Ws.passwordRequestAnswer = publicUserModel.passwordRequestAnswer;
            publicUserModel4Ws.passwordRequestQuestion = publicUserModel.passwordRequestQuestion;
            publicUserModel4Ws.pobox = publicUserModel.pobox;
            publicUserModel4Ws.prefContactChannel = publicUserModel.prefContactChannel;
            publicUserModel4Ws.prefPhone = publicUserModel.prefPhone;
            publicUserModel4Ws.proxyUserModel = publicUserModel.proxyUserModel;
            publicUserModel4Ws.questions = publicUserModel.questions;
            publicUserModel4Ws.receiveSMS = publicUserModel.receiveSMS;
            publicUserModel4Ws.roleType = publicUserModel.roleType;
            publicUserModel4Ws.salutation = publicUserModel.salutation;
            publicUserModel4Ws.servProvCode = publicUserModel.servProvCode;
            publicUserModel4Ws.ssn = publicUserModel.ssn;
            publicUserModel4Ws.state = publicUserModel.state;
            publicUserModel4Ws.status = publicUserModel.status;
            publicUserModel4Ws.statusOfV360User = publicUserModel.statusOfV360User;
            publicUserModel4Ws.userID = publicUserModel.userID;
            publicUserModel4Ws.userSeqNum = publicUserModel.userSeqNum.ToString();
            publicUserModel4Ws.userTitle = publicUserModel.userTitle;
            publicUserModel4Ws.viadorUrl = publicUserModel.viadorUrl;
            publicUserModel4Ws.workPhone = publicUserModel.workPhone;
            publicUserModel4Ws.workPhoneCountryCode = publicUserModel.workPhoneCountryCode;
            publicUserModel4Ws.xSocialMedia = publicUserModel.xSocialMedia;
            publicUserModel4Ws.zip = publicUserModel.zip;

            return publicUserModel4Ws;
        }

        /// <summary>
        /// Get agency display text from its alias name and agency code
        /// </summary>
        /// <param name="peopleModel">People model for web service</param>
        /// <returns>Agency display text</returns>
        private static string GetAgencyDisplayText(PeopleModel4WS peopleModel)
        {
            if (string.IsNullOrEmpty(peopleModel.serviceProviderCode) && string.IsNullOrEmpty(peopleModel.agencyAliasName))
            {
                return string.Empty;
            }

            return string.IsNullOrEmpty(peopleModel.agencyAliasName)
                    ? peopleModel.serviceProviderCode
                    : peopleModel.agencyAliasName;
        }
    }
}