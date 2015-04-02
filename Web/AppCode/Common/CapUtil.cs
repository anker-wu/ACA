#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapUtil.cs 278418 2014-09-03 08:54:25Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.BLL;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.Payment;
using Accela.ACA.Web.Util;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.Web.Controls;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// an utility class for Cap relevant logic.
    /// </summary>
    public static class CapUtil
    {
        #region Fields

        /// <summary>
        /// The last step in shopping cart breadcrumb.
        /// </summary>
        private const int LASTSTEP_IN_SHOPPINGCART = 4;

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(CapList));

        #endregion

        #region Methods
        /// <summary>
        /// set cap model's license professional's TemporaryID
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        public static void SetLPTemporaryID(CapModel4WS capModel)
        {
            if (capModel == null || capModel.licenseProfessionalList == null)
            {
                return;
            }

            foreach (var tempLp in capModel.licenseProfessionalList)
            {
                if (string.IsNullOrEmpty(tempLp.TemporaryID))
                {
                    tempLp.TemporaryID = CommonUtil.GetRandomUniqueID();
                }
            }
        }

        /// <summary>
        /// Shows the result message.
        /// </summary>
        /// <param name="anchorControl">The anchor control.</param>
        /// <param name="moduleName">Name of the module.</param>
        public static void ShowResultMessage(HtmlControl anchorControl, string moduleName)
        {
            OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();

            if (onlinePaymentResultModel == null || onlinePaymentResultModel.exceptionMsg == null || onlinePaymentResultModel.exceptionMsg.Length == 0)
            {
                return;
            }

            foreach (string exMsg in onlinePaymentResultModel.exceptionMsg)
            {
                if (exMsg.StartsWith(ACAConstant.AUTHORIZED_AGENT_PAYMENT_RESULT_WIN_LOTTERY))
                {
                    string message = string.Format(LabelUtil.GetTextByKey("aca_authagent_receipt_message_win_lottery", moduleName), exMsg.Replace(ACAConstant.AUTHORIZED_AGENT_PAYMENT_RESULT_WIN_LOTTERY, string.Empty));

                    MessageUtil.ShowMessage(anchorControl, MessageType.Notice, message);
                }
                else if (exMsg.StartsWith(ACAConstant.AUTHORIZED_AGENT_PAYMENT_RESULT_LOSE_LOTTERY))
                {
                    MessageUtil.ShowMessage(anchorControl, MessageType.Notice, LabelUtil.GetTextByKey("aca_authagent_receipt_message_lose_lottery", moduleName));
                }
            }
        }

        /// <summary>
        /// add json key-value
        /// </summary>
        /// <param name="sb">string builder</param>
        /// <param name="key">string key.</param>
        /// <param name="value">string value.</param>
        /// <param name="isEnd">indicates whether it is last parameter, if true - don't append the delimiter.</param>
        public static void AddKeyValue(StringBuilder sb, string key, string value, bool isEnd)
        {
            string format = string.Empty;

            if (isEnd)
            {
                format = "\"{0}\":\"{1}\"";
            }
            else
            {
                format = "\"{0}\":\"{1}\",";
            }

            sb.AppendFormat(format, key, value == null ? string.Empty : value.Replace("'", "\'").Replace("\"", "\\\"").Replace(@"\", @"\\"));
        }

        /// <summary>
        /// add json key-value
        /// </summary>
        /// <param name="sb">string builder</param>
        /// <param name="key">string key.</param>
        /// <param name="value">string value.</param>
        public static void AddKeyValue(StringBuilder sb, string key, string value)
        {
            AddKeyValue(sb, key, value, false);
        }

        /// <summary>
        /// Convert CapModel to AdditionInfo object for presentation.
        /// </summary>
        /// <param name="cap">CapModel4WS object</param>
        /// <returns>The AdditionalInfo object.</returns>
        public static AddtionalInfo BuildAddtionalInfo(CapModel4WS cap)
        {
            AddtionalInfo addtionalInfo = new AddtionalInfo();
            addtionalInfo.JobValueModel = cap.bvaluatnModel;
            addtionalInfo.JobValueModel.capID = cap.capID;
            addtionalInfo.ApplicationName = cap.specialText;

            if (cap.capWorkDesModel != null)
            {
                addtionalInfo.DetailedDesc = cap.capWorkDesModel.description;
            }

            if (cap.capDetailModel != null)
            {
                addtionalInfo.GeneralDesc = cap.capDetailModel.shortNotes;
                addtionalInfo.BuildingNumber = Convert.ToString(cap.capDetailModel.buildingCount);
                addtionalInfo.HousingUnit = Convert.ToString(cap.capDetailModel.houseCount);
                addtionalInfo.ConstructionType = cap.capDetailModel.constTypeCode;
                addtionalInfo.PublicOwner = cap.capDetailModel.publicOwned;
            }

            return addtionalInfo;
        }

        /// <summary>
        /// Builds the template filed string.
        /// </summary>
        /// <param name="templateAttributes">the template Attributes</param>
        /// <param name="prefix">the prefix</param>
        /// <returns>the template field string.</returns>
        public static string BuildTemplateFiledString(TemplateAttributeModel[] templateAttributes, string prefix)
        {
            StringBuilder sbattr = new StringBuilder();

            if (templateAttributes != null)
            {
                sbattr.Append("{");
                foreach (var templateAttribute in templateAttributes)
                {
                    sbattr.Append("\"" + TemplateUtil.GetTemplateControlID(templateAttribute.attributeName, prefix) +
                                  "\":{");

                    if (templateAttribute.attributeValueDataType == "Date")
                    {
                        AddKeyValue(sbattr, templateAttribute.attributeValueDataType, I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(templateAttribute.attributeValue), true);
                    }
                    else if (ControlType.Number.ToString().Equals(templateAttribute.attributeValueDataType, StringComparison.OrdinalIgnoreCase))
                    {
                        double number = 0;
                        I18nNumberUtil.TryParseNumberFromWebService(templateAttribute.attributeValue, out number);
                        AddKeyValue(sbattr, templateAttribute.attributeValueDataType, I18nNumberUtil.FormatNumberForUI(number), true);
                    }
                    else
                    {
                        AddKeyValue(sbattr, templateAttribute.attributeValueDataType, ScriptFilter.EncodeJson(templateAttribute.attributeValue), true);
                    }

                    sbattr.Append("},");
                }

                sbattr.Length -= 1;
                sbattr.Append("}");
            }
            else
            {
                return "null";
            }

            return sbattr.ToString();
        }

        /// <summary>
        /// Build generic template field.
        /// </summary>
        /// <param name="model">The template model.</param>
        /// <returns>Generic template fields string.</returns>
        public static string BuildGenericTemplateFieldString(TemplateModel model)
        {
            StringBuilder sb = new StringBuilder();
            var templateFields = GenericTemplateUtil.GetAllFields(model);

            if (templateFields != null && templateFields.Count() > 0)
            {
                sb.Append("{");

                foreach (var field in templateFields)
                {
                    sb.Append("\"");
                    sb.Append(ScriptFilter.EncodeJson(field.fieldName));
                    sb.Append("\":");
                    sb.Append("\"");
                    int fieldType = field.fieldType;
                    string value = ControlBuildHelper.FormatFieldValue((FieldType)fieldType, ScriptFilter.EncodeJson(field.defaultValue));
                    sb.Append(value);
                    sb.Append("\",");
                }

                sb.Length -= 1;

                sb.Append("}");
            }
            else
            {
                return "null";
            }

            return sb.ToString();
        }

        /// <summary>
        /// Clear the parent CapID | CapType | PageFlowGroup from AppSession for Associated Forms.
        /// </summary>
        public static void ClearAssoFormParentFromAppSession()
        {
            AppSession.SetParentCapIDModelToSession(ACAConstant.CAP_RELATIONSHIP_ASSOFORM, null);
            AppSession.SetParentCapTypeToSession(null);
            AppSession.SetParentPageflowGroupToSession(null);
        }

        /// <summary>
        /// This is common method for clone simple object,
        /// Notice:The clone object is included value-type properties only.
        /// </summary>
        /// <param name="cloneObject">clone object </param>
        /// <param name="newObject">generate new object which is from cloned object</param>
        /// <returns>new object its type is like with clone object</returns>
        public static object CloneSimpleObject(object cloneObject, object newObject)
        {
            Type type = cloneObject.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.CanWrite &&
                    pi.CanRead)
                {
                    pi.SetValue(newObject, pi.GetValue(cloneObject, null), null);
                }
            }

            return newObject;
        }

        /// <summary>
        /// Construct a new cap model by pass in cap model.
        /// </summary>
        /// <param name="oldCapModel4WS">old Cap Model.</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="isRenewal">weather is renew cap.</param>
        /// <returns>cap model.</returns>
        public static CapModel4WS ConstructCapModel(CapModel4WS oldCapModel4WS, string moduleName, string isRenewal)
        {
            if (oldCapModel4WS == null)
            {
                return null;
            }

            string publicUserId = AppSession.User.PublicUserId;

            CapModel4WS capModel4WS = oldCapModel4WS.Clone() as CapModel4WS;
            capModel4WS.moduleName = moduleName;
            capModel4WS.auditID = publicUserId;

            if (capModel4WS.licenseProfessionalModel != null)
            {
                capModel4WS.licenseProfessionalModel.classCode = null;
            }

            if (capModel4WS.capDetailModel != null)
            {
                capModel4WS.capDetailModel.auditStatus = ACAConstant.VALID_STATUS;
            }

            if (capModel4WS.capWorkDesModel != null)
            {
                capModel4WS.capWorkDesModel.auditID = publicUserId;
            }

            //This partial cap will can be display in ACA, this flag should be Y, when public user click save and resume button to save.
            //But if this is a renewal child cap, this flag should be N, doesn't be displayed in ACA.
            capModel4WS.accessByACA = ACAConstant.COMMON_Y.Equals(isRenewal, StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_N : ACAConstant.COMMON_Y;
            capModel4WS.createdByACA = ACAConstant.COMMON_Y;

            EducationModel4WS[] eduModels = capModel4WS.educationList;

            if (eduModels != null)
            {
                foreach (var item in eduModels)
                {
                    item.entityType = ACAConstant.CAP_EDUCATION_ENTITY_TYPE;
                }
            }

            ExaminationModel[] examModels = capModel4WS.examinationList;

            if (examModels != null)
            {
                foreach (var item in examModels)
                {
                    item.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;
                }
            }

            ContinuingEducationModel4WS[] contEduModels = capModel4WS.contEducationList;

            if (contEduModels != null)
            {
                foreach (var item in contEduModels)
                {
                    item.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;
                }
            }

            return capModel4WS;
        }

        /// <summary>
        /// Convert reference owner model to daily owner model.
        /// RefOwnerModel daily model,history denominate issue 
        /// OwnerModel reference model,history denominate issue
        /// </summary>
        /// <param name="ownerModel">reference owner model</param>
        /// <returns>ref owner model.</returns>
        public static RefOwnerModel ConvertOwnerModel2RefOwnerModel(OwnerModel ownerModel)
        {
            return ownerModel == null ? null : ownerModel.ToRefOwnerModel();
        }

        /// <summary>
        /// Convert the reference owner model to daily owner model.
        /// </summary>
        /// <param name="owner">the owner model.</param>
        /// <returns>the owner model after converted.</returns>
        public static OwnerModel ConvertRefOwnerModel2OwnerModel(RefOwnerModel owner)
        {
            if (owner == null)
            {
                return null;
            }

            OwnerModel ownerModel = new OwnerModel();
            ownerModel.ownerFullName = owner.ownerFullName;
            ownerModel.ownerTitle = owner.ownerTitle;
            ownerModel.mailAddress1 = owner.mailAddress1;
            ownerModel.mailAddress2 = owner.mailAddress2;
            ownerModel.mailAddress3 = owner.mailAddress3;
            ownerModel.mailCity = owner.mailCity;
            ownerModel.mailZip = owner.mailZip;
            ownerModel.mailState = owner.mailState;
            ownerModel.mailCountry = owner.mailCountry;
            ownerModel.fax = owner.fax;
            ownerModel.faxCountryCode = owner.faxCountryCode;
            ownerModel.phone = owner.phone;
            ownerModel.phoneCountryCode = owner.phoneCountryCode;
            ownerModel.email = owner.email;

            if (owner.l1OwnerNumber.HasValue)
            {
                ownerModel.ownerNumber = long.Parse(owner.l1OwnerNumber.Value.ToString());
            }

            ownerModel.UID = owner.UID;
            ownerModel.templates = owner.templates;

            return ownerModel;
        }

        /// <summary>
        /// Convert Daily Address Model to Reference Address Model
        /// </summary>
        /// <param name="addressModel">Daily Address Model</param>
        /// <returns>Reference Address Model</returns>
        public static RefAddressModel ConvertAddressModel2RefAddressModel(AddressModel addressModel)
        {
            if (addressModel == null)
            {
                return null;
            }

            RefAddressModel refAddressModel = new RefAddressModel();

            refAddressModel.city = addressModel.city;
            refAddressModel.houseNumberStart = addressModel.houseNumberStart;
            refAddressModel.unitStart = addressModel.unitStart;
            refAddressModel.state = addressModel.state;
            refAddressModel.zip = addressModel.zip;
            refAddressModel.streetName = addressModel.streetName;
            refAddressModel.county = addressModel.county;
            refAddressModel.refAddressId = addressModel.refAddressId;
            refAddressModel.streetSuffix = addressModel.streetSuffix;
            refAddressModel.streetDirection = addressModel.streetDirection;
            refAddressModel.unitType = addressModel.unitType;

            refAddressModel.streetPrefix = addressModel.streetPrefix;
            refAddressModel.houseNumberEnd = addressModel.houseNumberEnd;
            refAddressModel.unitEnd = addressModel.unitEnd;
            refAddressModel.country = addressModel.country;
            refAddressModel.countryCode = addressModel.countryCode;

            refAddressModel.houseFractionStart = addressModel.houseFractionStart;
            refAddressModel.houseFractionEnd = addressModel.houseFractionEnd;
            refAddressModel.addressType = addressModel.addressType;
            refAddressModel.addressTypeFlag = addressModel.addressTypeFlag;
            refAddressModel.streetSuffixdirection = addressModel.streetSuffixdirection;
            refAddressModel.addressDescription = addressModel.addressDescription;
            refAddressModel.distance = addressModel.distance;
            refAddressModel.secondaryRoad = addressModel.secondaryRoad;
            refAddressModel.secondaryRoadNumber = addressModel.secondaryRoadNumber;
            refAddressModel.inspectionDistrict = addressModel.inspectionDistrict;
            refAddressModel.inspectionDistrictPrefix = addressModel.inspectionDistrictPrefix;
            refAddressModel.neighborhoodPrefix = addressModel.neighborhoodPrefix;
            refAddressModel.neighborhood = addressModel.neighborhood;
            refAddressModel.XCoordinator = addressModel.XCoordinator;
            refAddressModel.YCoordinator = addressModel.YCoordinator;
            refAddressModel.fullAddress = addressModel.fullAddress;
            refAddressModel.primaryFlag = addressModel.primaryFlag;
            refAddressModel.addressType = addressModel.addressType;
            refAddressModel.addressLine1 = addressModel.addressLine1;
            refAddressModel.addressLine2 = addressModel.addressLine2;

            refAddressModel.levelPrefix = addressModel.levelPrefix;
            refAddressModel.levelNumberStart = addressModel.levelNumberStart;
            refAddressModel.levelNumberEnd = addressModel.levelNumberEnd;
            refAddressModel.houseNumberAlphaStart = addressModel.houseNumberAlphaStart;
            refAddressModel.houseNumberAlphaEnd = addressModel.houseNumberAlphaEnd;

            refAddressModel.UID = addressModel.UID;
            refAddressModel.templates = addressModel.templates;

            return refAddressModel;
        }

        /// <summary>
        /// convert parcel model to cap parcel model.
        /// </summary>
        /// <param name="parcelModel">parcel model.</param>
        /// <returns>cap parcel model</returns>
        public static CapParcelModel ConvertParcelModel2CapParcelModel(ParcelModel parcelModel)
        {
            if (parcelModel == null)
            {
                return null;
            }

            CapParcelModel capParcelModel = new CapParcelModel();

            capParcelModel.parcelModel = parcelModel;
            capParcelModel.l1ParcelNo = parcelModel.parcelNumber;
            capParcelModel.parcelNo = parcelModel.parcelNumber;
            capParcelModel.UID = parcelModel.UID;

            return capParcelModel;
        }

        /// <summary>
        /// convert ref address model to address model.
        /// </summary>
        /// <param name="refAddressModel">ref address model</param>
        /// <returns>address model.</returns>
        public static AddressModel ConvertRefAddressModel2AddressModel(RefAddressModel refAddressModel)
        {
            if (refAddressModel == null)
            {
                return null;
            }

            AddressModel addressModel = new AddressModel();
            addressModel.city = refAddressModel.city;
            addressModel.houseNumberStart = refAddressModel.houseNumberStart;
            addressModel.unitStart = refAddressModel.unitStart;
            addressModel.state = refAddressModel.state;
            addressModel.zip = refAddressModel.zip;
            addressModel.streetName = refAddressModel.streetName;
            addressModel.county = refAddressModel.county;
            addressModel.refAddressId = refAddressModel.refAddressId;
            addressModel.streetSuffix = refAddressModel.streetSuffix;
            addressModel.streetDirection = refAddressModel.streetDirection;
            addressModel.unitType = refAddressModel.unitType;

            addressModel.streetPrefix = refAddressModel.streetPrefix;
            addressModel.houseNumberEnd = refAddressModel.houseNumberEnd;
            addressModel.unitEnd = refAddressModel.unitEnd;
            addressModel.country = refAddressModel.country;
            addressModel.countryCode = refAddressModel.countryCode;

            addressModel.houseFractionStart = refAddressModel.houseFractionStart;
            addressModel.houseFractionEnd = refAddressModel.houseFractionEnd;
            addressModel.addressType = refAddressModel.addressType;
            addressModel.addressTypeFlag = refAddressModel.addressTypeFlag;
            addressModel.streetSuffixdirection = refAddressModel.streetSuffixdirection;
            addressModel.addressDescription = refAddressModel.addressDescription;
            addressModel.distance = refAddressModel.distance;
            addressModel.secondaryRoad = refAddressModel.secondaryRoad;
            addressModel.secondaryRoadNumber = refAddressModel.secondaryRoadNumber;
            addressModel.inspectionDistrict = refAddressModel.inspectionDistrict;
            addressModel.inspectionDistrictPrefix = refAddressModel.inspectionDistrictPrefix;
            addressModel.neighborhoodPrefix = refAddressModel.neighborhoodPrefix;
            addressModel.neighborhood = refAddressModel.neighborhood;
            addressModel.XCoordinator = refAddressModel.XCoordinator;
            addressModel.YCoordinator = refAddressModel.YCoordinator;
            addressModel.fullAddress = refAddressModel.fullAddress;
            addressModel.primaryFlag = refAddressModel.primaryFlag;
            addressModel.addressType = refAddressModel.addressType;
            addressModel.addressLine1 = refAddressModel.addressLine1;
            addressModel.addressLine2 = refAddressModel.addressLine2;

            addressModel.levelPrefix = refAddressModel.levelPrefix;
            addressModel.levelNumberStart = refAddressModel.levelNumberStart;
            addressModel.levelNumberEnd = refAddressModel.levelNumberEnd;
            addressModel.houseNumberAlphaStart = refAddressModel.houseNumberAlphaStart;
            addressModel.houseNumberAlphaEnd = refAddressModel.houseNumberAlphaEnd;

            addressModel.UID = refAddressModel.UID;
            addressModel.templates = refAddressModel.templates;
            addressModel.duplicatedAPOKeys = refAddressModel.duplicatedAPOKeys;

            return addressModel;
        }

        /// <summary>
        /// copy template value from a cap to another cap
        /// </summary>
        /// <param name="oldCap">old cap model</param>
        /// <param name="newCap">new cap model</param>
        public static void CopyTemplateValueToCapModel(CapModel4WS oldCap, CapModel4WS newCap)
        {
            if (oldCap == null ||
                newCap == null)
            {
                return;
            }

            /*
             * In normal agency, when passed the confirm page, user can still use the breadcrumb links go back to spear form,
             *  but CapWebService.saveWrapperForPartialCap interface never to get the template and generic template data,
             *  so in order to keep the template data in this scenario, need to re-assign the template and generic template data.
             * In 7.2.0 FP4 Contact management feature, contact template and contact generic template will be retrieved in the CapWebService.saveWrapperForPartialCap interface,
             *  and the order of the contact data will not be the same, so not need to set contact template.
             */

            if (newCap.ownerModel != null &&
                oldCap.ownerModel != null)
            {
                newCap.ownerModel.templates = oldCap.ownerModel.templates;
            }

            if (newCap.parcelModel != null && newCap.parcelModel.parcelModel != null && oldCap.parcelModel != null &&
                oldCap.parcelModel.parcelModel != null)
            {
                newCap.parcelModel.parcelModel.templates = oldCap.parcelModel.parcelModel.templates;
            }

            if (newCap.addressModel != null &&
                oldCap.addressModel != null)
            {
                newCap.addressModel.templates = oldCap.addressModel.templates;
            }

            if (newCap.licenseProfessionalModel != null &&
                oldCap.licenseProfessionalModel != null)
            {
                newCap.licenseProfessionalModel.attributes = oldCap.licenseProfessionalModel.attributes;
            }

            if (newCap.bCalcValuationListField != null &&
                oldCap.bCalcValuationListField != null)
            {
                newCap.bCalcValuationListField = oldCap.bCalcValuationListField;
            }
        }

        /// <summary>
        /// copy asi layout and instruction/watermark from a cap to another cap
        /// </summary>
        /// <param name="oldCap">old cap model</param>
        /// <param name="newCap">new cap model</param>
        public static void CopyASILayoutToCapModel(CapModel4WS oldCap, CapModel4WS newCap)
        {
            if (newCap.appSpecificInfoGroups == null || oldCap.appSpecificInfoGroups == null)
            {
                return;
            }

            for (int i = 0; i < newCap.appSpecificInfoGroups.Length; i++)
            {
                AppSpecificInfoGroupModel4WS newASIGroup = newCap.appSpecificInfoGroups[i];

                if (newASIGroup == null || i >= oldCap.appSpecificInfoGroups.Length || oldCap.appSpecificInfoGroups[i] == null)
                {
                    continue;
                }

                //copy asi group properties
                AppSpecificInfoGroupModel4WS oldASIGroup = oldCap.appSpecificInfoGroups[i];
                newASIGroup.columnArrangement = oldASIGroup.columnArrangement;
                newASIGroup.columnLayout = oldASIGroup.columnLayout;
                newASIGroup.labelDisplay = oldASIGroup.labelDisplay;
                newASIGroup.resInstruction = oldASIGroup.resInstruction;
                newASIGroup.instruction = oldASIGroup.instruction;

                //copy asi fields properties
                for (int j = 0; j < newASIGroup.fields.Length; j++)
                {
                    if (newASIGroup.fields[j] != null && oldASIGroup.fields.Length > j && oldASIGroup.fields[j] != null)
                    {
                        AppSpecificInfoModel4WS asiField = oldASIGroup.fields[j];
                        newASIGroup.fields[j].alignment = asiField.alignment;
                        newASIGroup.fields[j].instruction = asiField.instruction;
                        newASIGroup.fields[j].resInstruction = asiField.resInstruction;
                        newASIGroup.fields[j].waterMark = asiField.waterMark;
                        newASIGroup.fields[j].resWaterMark = asiField.resWaterMark;
                    }
                }
            }
        }

        /// <summary>
        /// Create url for amendment
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <returns>amendment url</returns>
        public static string CreateUrlForAmendment(CapModel4WS capModel)
        {
            string parentCapModelID = capModel.capID.id1 + "-" + capModel.capID.id2 + "-" + capModel.capID.id3;
            parentCapModelID = "&parentCapModelID=" + parentCapModelID;

            string trackingID = capModel.capID.trackingID.ToString();
            trackingID = string.IsNullOrEmpty(trackingID) ? string.Empty : "&trackingID=" + trackingID;
            ICapTypePermissionBll capTypPermissionBll = ObjectFactory.GetObject<ICapTypePermissionBll>();

            CapTypePermissionModel capTypePermission = new CapTypePermissionModel();
            capTypePermission.controllerType = ControllerType.COMBINEBUTTIONSETTING.ToString();
            capTypePermission.moduleName = capModel.moduleName;
            capTypePermission.group = capModel.capType.group;
            capTypePermission.type = capModel.capType.type;
            capTypePermission.subType = capModel.capType.subType;
            capTypePermission.category = capModel.capType.category;
            capTypePermission.entityType = EntityType.CreateAmendment.ToString();
            capTypePermission.entityKey2 = capModel.capStatus;

            string capTypeFilterName =
                capTypPermissionBll.GetCapTypeFilterByAppStatus(capModel.capID.serviceProviderCode, capTypePermission);
            capTypeFilterName = string.IsNullOrEmpty(capTypeFilterName)
                                    ? string.Empty
                                    : string.Format("&{0}={1}", UrlConstant.FILTER_NAME, HttpUtility.UrlEncode(capTypeFilterName));

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(capModel);
            string createdBy = "&createdBy=" + ACAConstant.PUBLIC_USER_NAME + user.userSeqNum;

            string isSubAgencyCap = StandardChoiceUtil.IsSuperAgency()
                                        ? "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y
                                        : string.Empty;

            return "~/Cap/CapType.aspx?Module=" + capModel.moduleName + "&stepNumber=0" + parentCapModelID + trackingID
                    + capTypeFilterName + createdBy + isSubAgencyCap + "&isAmendment=Y";
        }

        /// <summary>
        /// Construct license professional model
        /// </summary>
        /// <param name="licenseModel">license model.</param>
        /// <returns>license professional model.</returns>
        public static LicenseProfessionalModel CreateLicenseProfessionalModel(LicenseModel4WS licenseModel)
        {
            LicenseProfessionalModel licenseProModel = null;
            if (licenseModel != null)
            {
                licenseProModel = new LicenseProfessionalModel();
                licenseProModel.licenseNbr = licenseModel.stateLicense;
                licenseProModel.address1 = licenseModel.address1;
                licenseProModel.address2 = licenseModel.address2;
                licenseProModel.address3 = licenseModel.address3;
                licenseProModel.agencyCode = licenseModel.serviceProviderCode;
                licenseProModel.auditID = licenseModel.auditID;
                licenseProModel.auditStatus = licenseModel.auditStatus;
                licenseProModel.businessLicense = licenseModel.businessLicense;
                licenseProModel.businessName = licenseModel.businessName;
                licenseProModel.city = licenseModel.city;
                licenseProModel.cityCode = licenseModel.cityCode;
                licenseProModel.contactFirstName = licenseModel.contactFirstName;
                licenseProModel.contactLastName = licenseModel.contactLastName;
                licenseProModel.contactMiddleName = licenseModel.contactMiddleName;
                licenseProModel.country = licenseModel.country;
                licenseProModel.countryCode = licenseModel.countryCode;
                licenseProModel.einSs = licenseModel.einSs;
                licenseProModel.email = licenseModel.emailAddress;
                licenseProModel.fax = licenseModel.fax;
                licenseProModel.faxCountryCode = licenseModel.faxCountryCode;
                licenseProModel.holdCode = licenseModel.holdCode;
                licenseProModel.holdDesc = licenseModel.holdDesc;
                licenseProModel.licSeqNbr = licenseModel.licSeqNbr;
                licenseProModel.licenseType = licenseModel.licenseType;
                licenseProModel.phone1 = licenseModel.phone1;
                licenseProModel.phone1CountryCode = licenseModel.phone1CountryCode;
                licenseProModel.phone2 = licenseModel.phone2;
                licenseProModel.phone2CountryCode = licenseModel.phone2CountryCode;
                licenseProModel.printFlag = "N";
                licenseProModel.selfIns = licenseModel.selfIns;
                licenseProModel.serDes = "Description";
                licenseProModel.state = licenseModel.state;
                licenseProModel.suffixName = licenseModel.suffixName;
                licenseProModel.zip = licenseModel.zip;
                licenseProModel.classCode = licenseModel.licState;

                licenseProModel.socialSecurityNumber = licenseModel.socialSecurityNumber;
                licenseProModel.fein = licenseModel.fein;
                licenseProModel.typeFlag = licenseModel.typeFlag;
                licenseProModel.salutation = licenseModel.salutation;
                if (!string.IsNullOrEmpty(licenseModel.birthDate))
                {
                    licenseProModel.birthDate = I18nDateTimeUtil.ParseFromWebService(licenseModel.birthDate);
                }

                licenseProModel.gender = licenseModel.gender;
                licenseProModel.busName2 = licenseModel.busName2;
                licenseProModel.postOfficeBox = licenseModel.postOfficeBox;
                licenseProModel.contrLicNo = licenseModel.contrLicNo;
                licenseProModel.contLicBusName = licenseModel.contLicBusName;
            }

            return licenseProModel;
        }

        /// <summary>
        /// Create new Cap type by given string
        /// </summary>
        /// <param name="type">cap type, format must be X/X/X/X, X can be any string</param>
        /// <param name="alias">alias of cap type</param>
        /// <param name="moduleName">current module name</param>
        /// <returns>new cap type</returns>
        public static CapTypeModel CreateNewCapType(string type, string alias, string moduleName)
        {
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }

            string[] capLevels = type.Split('/');

            if (capLevels.Length != 4)
            {
                return null;
            }

            CapTypeModel capTypeModel = new CapTypeModel();
            capTypeModel.serviceProviderCode = ConfigManager.AgencyCode;
            capTypeModel.alias = alias;
            capTypeModel.resAlias = alias;
            capTypeModel.moduleName = moduleName;
            capTypeModel.group = capLevels[0];
            capTypeModel.type = capLevels[1];
            capTypeModel.subType = capLevels[2];
            capTypeModel.category = capLevels[3];

            return capTypeModel;
        }

        /// <summary>
        /// Indicating whether cross module search.
        /// </summary>
        /// <returns>true of false.</returns>
        public static bool EnableCrossModuleSearch()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;
            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_ENABLE_CROSS_MODULE_SEARCH);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                xPolicyValue = ACAConstant.COMMON_N;
            }

            bool isEnabled = xPolicyValue.Equals(ACAConstant.COMMON_Y);

            return isEnabled;
        }

        /// <summary>
        /// Fill template data in all contact object.
        /// </summary>
        /// <param name="templateBll"><c>ITemplateBll</c> object</param>
        /// <param name="capModel">CapModel4WS object</param>
        public static void FillAllContactTemplateValue(ITemplateBll templateBll, CapModel4WS capModel)
        {
            //Fill people tempate for Applicant model.
            FillContactTemplateValue(templateBll, capModel.capID, capModel.applicantModel);

            //Fill generic tempate for Applicant model.
            if (capModel.applicantModel != null
                && capModel.applicantModel.people != null
                && !string.IsNullOrEmpty(capModel.applicantModel.people.contactSeqNumber))
            {
                FillContactGenericTemplateValue(templateBll, capModel.applicantModel.people);
            }

            if (capModel.contactsGroup != null)
            {
                for (int i = 0; i < capModel.contactsGroup.Length; i++)
                {
                    //Fill people tempate for contactsGroup model.
                    FillContactTemplateValue(templateBll, capModel.capID, capModel.contactsGroup[i]);

                    if (capModel.contactsGroup[i].people != null)
                    {
                        //Fill generic tempate for contactsGroup model.
                        FillContactGenericTemplateValue(templateBll, capModel.contactsGroup[i].people);
                    }
                }
            }
        }

        /// <summary>
        /// Fill Contact generic template model.
        /// </summary>
        /// <param name="templateBll">The <c>ITemplateBll</c></param>
        /// <param name="people">The PeopleModel4WS</param>
        public static void FillContactGenericTemplateValue(ITemplateBll templateBll, PeopleModel4WS people)
        {
            if (people == null
                || string.IsNullOrEmpty(people.serviceProviderCode)
                || string.IsNullOrEmpty(people.contactSeqNumber))
            {
                return;
            }

            EntityPKModel entityPKModel = new EntityPKModel()
            {
                serviceProviderCode = people.serviceProviderCode,
                seq1 = long.Parse(people.contactSeqNumber),
                entityType = (int)GenericTemplateEntityType.DailyContact
            };

            people.template = templateBll.GetDailyGenericTemplate(entityPKModel);
        }

        /// <summary>
        /// get template value from web service then fill the template to specified cap model.
        /// </summary>
        /// <param name="capModel">need to be filled cap model with template.</param>
        public static void FillCapModelTemplateValue(CapModel4WS capModel)
        {
            if (capModel == null ||
                capModel.capID == null)
            {
                return;
            }

            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));

            if (capModel.addressModel != null &&
                capModel.addressModel.addressId != 0)
            {
                capModel.addressModel.templates = templateBll.GetDailyAPOTemplateAttributes(TemplateType.CAP_ADDRESS, capModel.capID, Convert.ToString(capModel.addressModel.addressId), ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }

            if (capModel.parcelModel != null && capModel.parcelModel.parcelModel != null &&
                !string.IsNullOrEmpty(capModel.parcelModel.parcelModel.parcelNumber))
            {
                capModel.parcelModel.parcelModel.templates = templateBll.GetDailyAPOTemplateAttributes(TemplateType.CAP_PARCEL, capModel.capID, capModel.parcelModel.parcelModel.parcelNumber, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }

            if (capModel.ownerModel != null &&
                capModel.ownerModel.ownerNumber != null &&
                AppSession.User != null)
            {
                capModel.ownerModel.templates = templateBll.GetDailyAPOTemplateAttributes(TemplateType.CAP_OWNER, capModel.capID, Convert.ToString(capModel.ownerModel.ownerNumber), ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }

            FillAllContactTemplateValue(templateBll, capModel);
        }

        /// <summary>
        /// Filter Same License Professional.
        /// </summary>
        /// <param name="capModel">the Cap Model</param>
        public static void FilterSameLicenseType(CapModel4WS capModel)
        {
            if (capModel.licenseProfessionalList == null)
            {
                return;
            }

            Dictionary<string, LicenseProfessionalModel4WS> licenseList = new Dictionary<string, LicenseProfessionalModel4WS>();
            List<LicenseProfessionalModel4WS> licenses = new List<LicenseProfessionalModel4WS>();
            foreach (LicenseProfessionalModel4WS item in capModel.licenseProfessionalList)
            {
                string key = item.licenseType + item.licenseNbr;
                if (!licenseList.ContainsKey(key))
                {
                    licenseList.Add(key, item);
                    licenses.Add(item);
                }
            }

            capModel.licenseProfessionalList = licenses.ToArray();
        }

        /// <summary>
        /// get agency code by cap model session.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>agency code</returns>
        public static string GetAgencyCode(string moduleName)
        {
            string agencyCode = ConfigManager.AgencyCode;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (capModel != null && capModel.capID != null &&
                !string.IsNullOrEmpty(capModel.capID.serviceProviderCode))
            {
                agencyCode = capModel.capID.serviceProviderCode;
            }

            return agencyCode;
        }

        /// <summary>
        /// Get all visible (ASIT Group's security is not 'N') ASI Tables from the CAP model and combined into an array.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="appSpecTableGroups">The ASIT group list.</param>
        /// <param name="allVisibleGroups">All visible ASIT Groups list.</param>
        /// <returns>Array of AppSpecificTableModel4WS model</returns>
        public static AppSpecificTableModel4WS[] GetAllVisibleASITables(string moduleName, AppSpecificTableGroupModel4WS[] appSpecTableGroups, IList<AppSpecificTableGroupModel4WS> allVisibleGroups = null)
        {
            List<AppSpecificTableModel4WS> allVisibleTableList = new List<AppSpecificTableModel4WS>();

            if (appSpecTableGroups != null && appSpecTableGroups.Length > 0)
            {
                foreach (AppSpecificTableGroupModel4WS asitGroup in appSpecTableGroups)
                {
                    if (asitGroup != null)
                    {
                        string groupSecurity = ASISecurityUtil.GetASITSecurity(asitGroup, moduleName);
                        if (!ACAConstant.ASISecurity.None.Equals(groupSecurity) && asitGroup.tablesMapValues != null && asitGroup.tablesMapValues.Length > 0)
                        {
                            allVisibleTableList.AddRange(asitGroup.tablesMapValues);

                            if (allVisibleGroups != null)
                            {
                                allVisibleGroups.Add(asitGroup);
                            }
                        }
                    }
                }
            }

            return allVisibleTableList.ToArray();
        }

        /// <summary>
        /// Get all generic template tables and combined into an array.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="templateTableGroups">The ASIT group list.</param>
        /// <param name="onlyGetVisibleTables">
        ///     true -- only get the visible template tables, do not get the hidden tables(Template Group's security is None).
        ///     false -- get all template tables, include hidden tables which the Template Group's ASI Security set as None.
        /// </param>
        /// <returns>Array of TemplateSubgroup model</returns>
        public static TemplateSubgroup[] GetGenericTemplateTables(string moduleName, TemplateGroup[] templateTableGroups, bool onlyGetVisibleTables = true)
        {
            List<TemplateSubgroup> allVisibleTableList = new List<TemplateSubgroup>();

            if (templateTableGroups != null && templateTableGroups.Length > 0)
            {
                foreach (TemplateGroup templateGroup in templateTableGroups)
                {
                    if (templateGroup == null || templateGroup.subgroups == null || templateGroup.subgroups.Length == 0)
                    {
                        continue;
                    }

                    if (!onlyGetVisibleTables)
                    {
                        allVisibleTableList.AddRange(templateGroup.subgroups);
                        continue;
                    }

                    string groupSecurity = ASISecurityUtil.GetASITSecurity(templateGroup, moduleName);

                    if (!ACAConstant.ASISecurity.None.Equals(groupSecurity))
                    {
                        allVisibleTableList.AddRange(templateGroup.subgroups);
                    }
                }
            }

            return allVisibleTableList.ToArray();
        }

        /// <summary>
        /// Get all generic template tables and combined into an array.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="templateTableGroups">The ASIT sub group list.</param>
        /// <param name="onlyGetVisibleTables">
        ///     true -- only get the visible template tables, do not get the hidden tables(Template Group's security is None).
        ///     false -- get all template tables, include hidden tables which the Template Group's ASI Security set as None.
        /// </param>
        /// <returns>Array of TemplateSubgroup model</returns>
        public static TemplateSubgroup[] GetGenericTemplateTableSubGroups(string moduleName, TemplateGroup[] templateTableGroups, bool onlyGetVisibleTables = true)
        {
            if (templateTableGroups == null || templateTableGroups.Length == 0)
            {
                return null;
            }

            List<TemplateGroup> availableGroupList = new List<TemplateGroup>();

            foreach (TemplateGroup templateGroup in templateTableGroups)
            {
                if (templateGroup == null)
                {
                    continue;
                }

                if (!onlyGetVisibleTables)
                {
                    availableGroupList.Add(templateGroup);
                    continue;
                }

                string groupSecurity = ASISecurityUtil.GetASITSecurity(templateGroup, moduleName);

                if (!ACAConstant.ASISecurity.None.Equals(groupSecurity))
                {
                    availableGroupList.Add(templateGroup);
                }
            }

            List<TemplateSubgroup> allSubGroupList = new List<TemplateSubgroup>();

            foreach (TemplateGroup group in availableGroupList)
            {
                if (!onlyGetVisibleTables)
                {
                    allSubGroupList.AddRange(group.subgroups);
                    continue;
                }

                foreach (TemplateSubgroup templateSubgroup in group.subgroups)
                {
                    string tableSecurity = ASISecurityUtil.GetASITSecurity(templateSubgroup, moduleName);

                    if ((templateSubgroup.acaTemplateConfigModel != null && ValidationUtil.IsNo(templateSubgroup.acaTemplateConfigModel.acaDisplayFlag))
                        || ACAConstant.ASISecurity.None.Equals(tableSecurity)
                        || ASISecurityUtil.IsAllFieldsNoAccess(templateSubgroup, moduleName))
                    {
                        continue;
                    }

                    allSubGroupList.Add(templateSubgroup);
                }
            }

            return allSubGroupList.ToArray();
        }

        /// <summary>
        /// Gets permit find start date range of agency current date 
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code</param>
        /// <param name="endDate">The end Date</param>
        /// <returns>agency current date</returns>
        public static DateTime GetCapDefaultFindDateRange(string serviceProviderCode, DateTime endDate)
        {
            int range = 0;
            DateTime startDate = endDate.AddDays(0 - ACAConstant.DEFAULT_FIND_APP_DATE_RANGE);

            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            IList<ItemValue> stdItems = bizBll.GetBizDomainList(serviceProviderCode, BizDomainConstant.STD_CAT_FIND_APP_DATE_RANGE, false);

            if (stdItems != null && stdItems.Count > 0)
            {
                string value = string.Empty;
                DateTime tmpStartDate = new DateTime();

                foreach (ItemValue itemValue in stdItems)
                {
                    value = itemValue.Key.ToString();

                    if (int.TryParse(value, out range))
                    {
                        startDate = endDate.AddDays(0 - range);
                        break;
                    }

                    if (DateTime.TryParse(value, out tmpStartDate))
                    {
                        startDate = tmpStartDate;
                        break;
                    }
                }
            }

            return startDate;
        }

        /// <summary>
        /// get cap model.
        /// </summary>
        /// <param name="page">the UI page.</param>
        /// <returns>cap model of the page.</returns>
        public static CapModel4WS GetCapModel(Page page)
        {
            return GetCapModel(page, "~/Expriation.aspx");
        }

        /// <summary>
        /// get cap model.
        /// </summary>
        /// <param name="page">the UI page.</param>
        /// <param name="url">redirect url.</param>
        /// <returns>cap model of the page.</returns>
        public static CapModel4WS GetCapModel(Page page, string url)
        {
            CapModel4WS capModel = page.Session[SessionConstant.SESSION_CAP_MODEL] as CapModel4WS;

            if (capModel == null)
            {
                page.Response.Redirect(url, true);
            }

            return capModel;
        }

        /// <summary>
        /// Get CapWithConditionModel4WS object from session, if this session is null, it will get by the method GetCapViewBySingle of <c>CapBll</c> class.
        /// </summary>
        /// <param name="capIdModel">cap id module</param>
        /// <param name="userSeqNum">user sequence number</param>
        /// <returns>CapWithConditionModel4WS object</returns>
        public static CapWithConditionModel4WS GetCapWithConditionModel4WS(CapIDModel4WS capIdModel, string userSeqNum)
        {
            return GetCapWithConditionModel4WS(capIdModel, userSeqNum, false);
        }

        /// <summary>
        /// Get CapWithConditionModel4WS object from session,if this session is null,it will get by the method GetCapViewBySingle of <c>CapBll</c> class.
        /// </summary>
        /// <param name="capIdModel">CapIDModel4WS object</param>
        /// <param name="userSeqNum">user sequence number</param>
        /// <param name="isForView">if for view or not</param>
        /// <param name="isApproval">"Y" or "N".</param>
        /// <returns>CapWithConditionModel4WS object</returns>
        public static CapWithConditionModel4WS GetCapWithConditionModel4WS(CapIDModel4WS capIdModel, string userSeqNum, bool isForView, string isApproval = "N")
        {
            return GetCapWithConditionModel4WS(capIdModel, userSeqNum, isForView, false, isApproval);
        }

        /// <summary>
        /// Get CapWithConditionModel4WS object from session,if this session is null,it will get by the method GetCapViewBySingle of <c>CapBll</c> class.
        /// </summary>
        /// <param name="capIdModel">CapIDModel4WS object</param>
        /// <param name="userSeqNum">user sequence number</param>
        /// <param name="isForView">if for view or not</param>
        /// <param name="isForRenew">if it is true, get the child record for renewal.</param>
        /// <param name="isApproval">"Y" or "N".</param>
        /// <returns>CapWithConditionModel4WS object</returns>
        public static CapWithConditionModel4WS GetCapWithConditionModel4WS(CapIDModel4WS capIdModel, string userSeqNum, bool isForView, bool isForRenew, string isApproval = "N")
        {
            bool isNeedUpdated = true;

            if (System.Web.HttpContext.Current.Session["capWithConditionModel"] != null)
            {
                CapWithConditionModel4WS capWithCondition = System.Web.HttpContext.Current.Session["capWithConditionModel"] as CapWithConditionModel4WS;

                if (capIdModel != null
                    && capWithCondition != null
                    && capWithCondition.capModel != null
                    && capWithCondition.capModel.capID != null
                    && !string.IsNullOrEmpty(capWithCondition.capModel.capID.id1)
                    && capWithCondition.capModel.capID.id1.Equals(capIdModel.id1)
                    && !string.IsNullOrEmpty(capWithCondition.capModel.capID.id2)
                    && capWithCondition.capModel.capID.id2.Equals(capIdModel.id2)
                    && !string.IsNullOrEmpty(capWithCondition.capModel.capID.id3)
                    && capWithCondition.capModel.capID.id3.Equals(capIdModel.id3)
                    && !string.IsNullOrEmpty(capWithCondition.capModel.capID.serviceProviderCode)
                    && capWithCondition.capModel.capID.serviceProviderCode.Equals(capIdModel.serviceProviderCode))
                {
                    isNeedUpdated = false;
                }
            }

            if (isNeedUpdated)
            {
                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                System.Web.HttpContext.Current.Session["capWithConditionModel"] = capBll.GetCapViewBySingle(capIdModel, userSeqNum, isForView, isApproval, StandardChoiceUtil.IsSuperAgency(), isForRenew);
            }

            return (CapWithConditionModel4WS)System.Web.HttpContext.Current.Session["capWithConditionModel"];
        }

        /// <summary>
        /// Get child cap ids.
        /// </summary>
        /// <param name="capModel">parent cap model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="isAssoFormEnabled">if it is true, include the parent cap, the first cap is the parent cap.</param>
        /// <returns>Child CapIDs</returns>
        public static CapIDModel4WS[] GetChildCapIDs(CapModel4WS capModel, string moduleName, bool isAssoFormEnabled)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            CapIDModel4WS[] capIds = null;

            if (IsSuperCAP(moduleName) || isAssoFormEnabled)
            {
                capIds = capBll.GetChildCaps(capModel.capID);

                // if it is normal assoicated form, it need save the parent cap.
                if (GetAssoFormType(isAssoFormEnabled, capModel.capID) == ACAConstant.AssoFormType.Normal)
                {
                    List<CapIDModel4WS> allCaps;
                    if (capIds != null)
                    {
                        allCaps = new List<CapIDModel4WS>(capIds);
                    }
                    else
                    {
                        allCaps = new List<CapIDModel4WS>();
                    }

                    allCaps.Insert(0, capModel.capID);
                    capIds = allCaps.ToArray();
                }
            }
            else
            {
                capIds = new CapIDModel4WS[1];
                capIds[0] = capModel.capID;
            }

            return capIds;
        }

        /// <summary>
        /// Check if CAP Model stored in session is updated or not(10ACC-09223)
        /// </summary>
        /// <param name="currentCapID">Current CAP ID From Page View State</param>
        /// <param name="isPartialCAP">Is Partial CAP</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>True or False</returns>
        public static bool IsCAPUpdatedInSession(CapIDModel4WS currentCapID, bool isPartialCAP, string moduleName)
        {
            bool isUpdated = false;

            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (capModel != null)
            {
                // CASE 1: On CAP Edit and Review Page, the CAP should not be completed CAP
                if (isPartialCAP
                    && (capModel.capClass == null || ACAConstant.COMPLETED.Equals(capModel.capClass.ToUpper())))
                {
                    isUpdated = true;
                }

                // CASE 2: On all pages, the CAP Model should match with CAP ID in page's view state
                if (!isUpdated
                    && (capModel.capID != null && currentCapID != null && !currentCapID.Equals(capModel.capID)))
                {
                    isUpdated = true;
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// get continue button label key.
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <param name="isFeeEstimator">Whether it is fee estimator.</param>
        /// <returns>Continue button label key.</returns>
        public static string GetContinueButtonLabelKey(string moduleName, string isFeeEstimator)
        {
            if (moduleName == "ServiceRequest")
            {
                return "srq_applyServiceReq_label_continueServiceReq";
            }

            if (isFeeEstimator == ACAConstant.COMMON_Y)
            {
                return "per_fee_label_continueFeeContinue";
            }

            return "per_applyPermit_label_continueAppPro";
        }

        /// <summary>
        /// Get EDMS policy model object from cache,if this cache is null,it will get by the method GetSecurityPolicy of <c>EDMSDocumentBll</c> class.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">Module name</param>
        /// <param name="publicUserId">public user id</param>
        /// <param name="capID">CapIDModel4WS object</param>
        /// <returns>The EDMS policy model object</returns>
        public static EdmsPolicyModel4WS GetEdmsPolicyModel4WS(string agencyCode, string moduleName, string publicUserId, CapIDModel4WS capID)
        {
            string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_EDMS_POLICY;
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            Hashtable cacheItem = cacheManager.GetSingleCachedItem(cacheKey) as Hashtable;
            string levelData = !string.IsNullOrEmpty(moduleName) ? moduleName : agencyCode;
            EdmsPolicyModel4WS edmsPolicyModel = null;

            if (cacheItem != null)
            {
                edmsPolicyModel = cacheItem[levelData] as EdmsPolicyModel4WS;
            }

            // if cache is null, will get EDMS policy from biz server and add it to cache.  
            if (edmsPolicyModel == null)
            {
                IEDMSDocumentBll edmsBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
                edmsPolicyModel = edmsBll.GetSecurityPolicy(agencyCode, moduleName, publicUserId, capID);

                if (edmsPolicyModel != null)
                {
                    if (cacheItem == null)
                    {
                        cacheItem = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                    }

                    cacheItem.Add(levelData, edmsPolicyModel);
                    cacheManager.AddSingleItemToCache(cacheKey, cacheItem, cacheManager.GetDefaultExpirationTime());
                }
            }

            return edmsPolicyModel;
        }

        /// <summary>
        /// get Once Construction Type Description
        /// </summary>
        /// <param name="constuctionTypeCode">construction type code</param>
        /// <param name="agencyCode">the agency code</param>
        /// <returns>Construction Type Description</returns>
        public static string GetOneConstuctionTypeDescription(string constuctionTypeCode, string agencyCode)
        {
            string returnValue = string.Empty;

            if (constuctionTypeCode == null ||
                constuctionTypeCode.Trim().Length <= 0)
            {
                return returnValue;
            }

            returnValue = constuctionTypeCode;

            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_CONSTUCTION_TYPE, false);

            if (stdItems == null ||
                stdItems.Count == 0)
            {
                return returnValue;
            }

            constuctionTypeCode = constuctionTypeCode.Trim();

            foreach (ItemValue item in stdItems)
            {
                if (item.Key != null &&
                    item.Key.CompareTo(constuctionTypeCode.Trim()) == 0)
                {
                    returnValue = item.Value == null ? string.Empty : item.Value.ToString();
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// determine whether a cap type has fee items
        /// </summary>
        /// <param name="capIDModel">the cap model</param> 
        /// <param name="isMultiCap">Whether has child caps</param>
        /// <param name="isIncludeParent">Whether judge the parent cap's Fees</param>
        /// <returns>if this cap is associated with fee items return true else false</returns>
        public static bool HasFee(CapIDModel4WS capIDModel, bool isMultiCap, bool isIncludeParent)
        {
            if (capIDModel == null)
            {
                throw new ArgumentException("The parameters could not be empty in the function HasFee");
            }

            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));

            return feeBll.HasFee(capIDModel, isMultiCap, isIncludeParent);
        }

        /// <summary>
        /// determine whether the cap list's fee changed.
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        /// <returns>if these has cap fee changed.</returns>
        public static CapIDModel4WS[] GetFeeChangedCapIdList(CapIDModel4WS[] capIDs)
        {
            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));
            CapIDModel4WS[] feeChangeCapIDs = null;

            if (capIDs != null && capIDs.Length > 0)
            {
                feeChangeCapIDs = feeBll.GetFeeChangedCapIdList(capIDs);
            }

            return feeChangeCapIDs;
        }

        /// <summary>
        /// determine whether the cap list's fee changed.
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        public static void UpdateFeeScheduleOrFeeItems(CapIDModel4WS[] capIDs)
        {
            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));

            if (capIDs != null && capIDs.Length > 0)
            {
                feeBll.UpdateFeeScheduleOrFeeItems(capIDs);
            }
        }

        /// <summary>
        /// According to the data filter configuration to judge whether the current user has the privilege.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="dataFilterConfig">Data filter role which is configured in Admin</param>
        /// <returns>true-the current user is able to view the CAP relevant information.</returns>
        public static bool HasPrivilege(string moduleName, UserRoleType dataFilterConfig)
        {
            UserRoleType urt = GetPrivilegeForCAP(moduleName);

            if (dataFilterConfig == UserRoleType.CAPCreator)
            {
                if (urt == UserRoleType.CAPCreator)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (dataFilterConfig == UserRoleType.CAPCreatorAndAssociatedLicensedProfessionals)
            {
                if (urt == UserRoleType.CAPCreator ||
                    urt == UserRoleType.CAPCreatorAndAssociatedLicensedProfessionals)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// indicates cap type whether fee estimate or not.
        /// </summary>
        /// <param name="capType">the cap type model</param>
        /// <returns>true if fee estimate</returns>
        public static bool IsFeeEstimateCapType(CapTypeModel capType)
        {
            if (capType != null)
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                CapTypeDetailModel capTypeDetail = capTypeBll.GetCapTypeByPK(capType);

                if (capTypeDetail != null &&
                    (capTypeDetail.udCode3 == ACAConstant.VCH_TYPE_EST || capTypeDetail.udcode3 == ACAConstant.VCH_TYPE_EST))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Whether current CAP is the Super cap.
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>Whether is Super CAP.</returns>
        public static bool IsSuperCAP(string moduleName)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            return IsSuperCAP(capModel);
        }

        /// <summary>
        /// Whether is super CAP
        /// </summary>
        /// <param name="capModel">Cap model</param>
        /// <returns>true or false</returns>
        public static bool IsSuperCAP(CapModel4WS capModel)
        {
            /*
             * 1. Pay4ExistingCap | RenewalCap | Amendment: all are single CAP;
             * 2. If is SuperAgency and CapModel.refID is null, is SuperCAP (except the CAP cloning);
             * 3. If is SuperAgency and CapModel.refID is not null, is single CAP.
             * 
             * Remarks: 
             * 1. refID is SuperCAP ID
             * 2. parentCapID for the Amendment
             */

            bool isPay4ExistingCap = ACAConstant.COMMON_Y.Equals(HttpContext.Current.Request.QueryString["isPay4ExistingCap"], StringComparison.InvariantCultureIgnoreCase);
            bool isRenewalCap = ACAConstant.COMMON_Y.Equals(HttpContext.Current.Request["isRenewal"], StringComparison.InvariantCultureIgnoreCase);
            bool isSubAgencyCap = ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]);
            bool isAmendment = false;

            if (capModel.parentCapID != null)
            {
                isAmendment = true;
            }

            string refID = capModel.refID;

            if (!string.IsNullOrEmpty(refID) || isPay4ExistingCap || isRenewalCap || isAmendment ||
                !StandardChoiceUtil.IsSuperAgency() || isSubAgencyCap)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value to indicates whether the specified CAP ID is a super CAP in super agency environment.
        /// This method will search the CAP relationship table(XAPP2REF) to determine the specified CAP ID.
        /// </summary>
        /// <param name="capID">the cap id model</param>
        /// <returns>true: is a super cap, false:is a single cap.</returns>
        public static bool IsSuperCAP(CapIDModel4WS capID)
        {
            bool isSuperCap = false;

            //If the cap don't have child cap, it is subagency cap, and use subagency record create logic.
            if (StandardChoiceUtil.IsSuperAgency())
            {
                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                CapIDModel4WS childCapID = capBll.GetChildCapIDByMasterID(capID, ACAConstant.CAP_RELATIONSHIP_RELATED, ACAConstant.INCOMPLETE, true);

                if (childCapID != null)
                {
                    isSuperCap = true;
                }
            }

            return isSuperCap;
        }

        /// <summary>
        /// Indicate the current CAP(from Session) whether is the Child of the parent CAP(from Session) for the Associated Forms.
        /// </summary>
        /// <param name="moduleName">Module name</param>
        /// <returns>true or false.</returns>
        public static bool IsAssoFormChild(string moduleName)
        {
            CapIDModel4WS parentCapIDModel = AppSession.GetParentCapIDModelFromSession(ACAConstant.CAP_RELATIONSHIP_ASSOFORM);
            CapModel4WS currentCapModel = AppSession.GetCapModelFromSession(moduleName);

            if (parentCapIDModel == null || currentCapModel == null || currentCapModel.capID == null || currentCapModel.capID.Equals(parentCapIDModel))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indicate whether the Associated Forms has been enabled for the specified cap type.
        /// </summary>
        /// <param name="capType">CapTypeModel entity</param>
        /// <returns>true or false.</returns>
        public static bool IsAssoFormEnabled(CapTypeModel capType)
        {
            bool isAssoFormEnabled = false;

            if (capType != null)
            {
                isAssoFormEnabled = ValidationUtil.IsYes(capType.associatedFormFlag);
            }

            return isAssoFormEnabled;
        }

        /// <summary>
        /// Indicate whether the partial submission is enabled for the special cap type.
        /// </summary>
        /// <param name="capID">CapIDModel4WS instance</param>
        /// <returns>true or false.</returns>
        public static bool EnablePartialSubmission(CapIDModel4WS capID)
        {
            bool isPartialSubmissionEnable = false;

            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            CapTypeModel capType = capTypeBll.GetCapTypeByCapID(capID);

            if (capType != null)
            {
                isPartialSubmissionEnable = ValidationUtil.IsYes(capType.partialSubmissionFlag);
            }

            return isPartialSubmissionEnable;
        }

        /// <summary>
        /// Redirect the page with non-feeEstimator flag after the cap saving
        /// </summary>
        /// <param name="response">HttpResponse object</param>
        /// <param name="moduleName">module name</param>
        /// <param name="capID">CapIDModel4WS object</param>
        /// <param name="isRenewal">is renewal string</param>
        public static void SaveResumeRedirect(HttpResponse response, string moduleName, CapIDModel4WS capID, string isRenewal)
        {
            SaveResumeRedirect(response, moduleName, capID, isRenewal, ACAConstant.COMMON_N);
        }

        /// <summary>
        /// Redirect the page after the cap saving that [isRenewal] indicate the isRenewal flag, <c>[isSuperAgencyAssoForm]</c> indicate the SuperAgency's Associated Form flag.
        /// </summary>
        /// <param name="response">The HttpResponse object.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="capID">The cap id.</param>
        /// <param name="isRenewal">is renewal string.</param>
        /// <param name="isSuperAgencyAssoForm">is super agency associated form string.</param>
        public static void SaveResumeRedirect(HttpResponse response, string moduleName, CapIDModel4WS capID, string isRenewal, string isSuperAgencyAssoForm)
        {
            SaveResumeRedirect(response, moduleName, capID, ACAConstant.COMMON_N, isRenewal, isSuperAgencyAssoForm);
        }

        /// <summary>
        /// According to parent page flow data to generate the url for the <c>AssociatedForms.aspx</c> page
        /// </summary>
        /// <returns>url string with the Application root</returns>
        public static string GetAssoFormUrl()
        {
            PageFlowGroupModel parentPageflow = AppSession.GetParentPageflowGroupFromSession();
            int stepNumber = parentPageflow.stepList.Length + 3;

            // use parentCapType.group to specified the module name, NOT use parentCapType.moduleName, its value is null.
            CapTypeModel parentCapType = AppSession.GetParentCapTypeFromSession();
            string url = string.Format(FileUtil.AppendApplicationRoot("Cap/AssociatedForms.aspx?Module={0}&stepNumber={1}&isFromShoppingCart={2}"), parentCapType.group, stepNumber, HttpContext.Current.Request.QueryString[ACAConstant.FROMSHOPPINGCART]);

            // Should not pass "isSubAgencyCap" and "agencyCode" to Super Agency Associated Forms.
            if (!ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[UrlConstant.IS_SUPERAGENCY_ASSOFORM]))
            {
                if (ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
                {
                    url += "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y;
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode]))
                {
                    url += "&" + UrlConstant.AgencyCode + "=" + HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode];
                }
            }

            return url;
        }

        /// <summary>
        /// Convert 'Y','y','n','N' to 'false' or 'true'.
        /// </summary>
        /// <param name="flag">the indicate flag.</param>
        /// <returns>true or false.</returns>
        public static bool Convert2BooleanValue(string flag)
        {
            return ACAConstant.COMMON_Y == flag;
        }

        /// <summary>
        /// Is Section Editable.
        /// </summary>
        /// <param name="flag">The flag to indicate the section editable.</param>
        /// <returns>true or false.</returns>
        public static bool IsSectionEditable(string flag)
        {
            return ACAConstant.COMMON_Y == flag || string.IsNullOrEmpty(flag);
        }

        /// <summary>
        /// Set the parent CapID | CapType | PageFlowGroup model to Session for the Associated Forms.
        /// </summary>
        /// <param name="capModel">Current CapModel</param>
        /// <param name="capType">CapType model of the current CapModel</param>
        /// <param name="pageflow">Page flow group of the current CapModel</param>
        public static void SetAssoFormParentToAppSession(CapModel4WS capModel, CapTypeModel capType, PageFlowGroupModel pageflow)
        {
            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            CapIDModel4WS parentCapID = GetParentAssoFormCapID(capModel.capID);
            bool hasAssoFormParent = !parentCapID.Equals(capModel.capID);
            CapTypeModel parentCapType = null;
            PageFlowGroupModel parentPageflow = null;

            if (!hasAssoFormParent)
            {
                parentCapType = capType;
                parentPageflow = pageflow;
            }
            else
            {
                parentCapType = capTypeBll.GetCapTypeByCapID(parentCapID);
            }

            if (IsAssoFormEnabled(parentCapType) || hasAssoFormParent)
            {
                AppSession.SetParentCapIDModelToSession(ACAConstant.CAP_RELATIONSHIP_ASSOFORM, parentCapID);
                AppSession.SetParentCapTypeToSession(parentCapType);

                if (parentPageflow == null)
                {
                    IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));
                    parentPageflow = pageflowBll.GetPageflowGroupByCapType(parentCapType);
                }

                AppSession.SetParentPageflowGroupToSession(parentPageflow);
            }
            else
            {
                ClearAssoFormParentFromAppSession();
            }
        }

        /// <summary>
        /// Set hasFee /capName of <c>BreadCrumbParmsInfo</c> after create really cap.
        /// </summary>
        /// <param name="callerId">module name</param>
        /// <param name="moduleName">the public user id</param>
        /// <param name="hasFee">has fee or not</param>
        public static void SetBreadCrumbSession(string callerId, string moduleName, bool hasFee)
        {
            BreadCrumbParmsInfo breadcrumbParmsInfo = AppSession.BreadcrumbParams;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            breadcrumbParmsInfo.CapName = CAPHelper.GetAliasOrCapTypeLabel(capModel);

            if (hasFee)
            {
                breadcrumbParmsInfo.HasFee = ACAConstant.COMMON_YES;
            }
            else
            {
                breadcrumbParmsInfo.HasFee = ACAConstant.COMMON_NO;
            }

            AppSession.BreadcrumbParams = breadcrumbParmsInfo;
        }

        /// <summary>
        /// set cap contact model the cap model
        /// </summary>
        /// <param name="capModel">the cap model.</param>
        /// <param name="capContact">the cap contact model.</param>
        public static void SetCapContactToCap(CapModel4WS capModel, CapContactModel4WS capContact)
        {
            if (capModel == null || capContact == null)
            {
                return;
            }

            capModel.contactsGroup = ContactUtil.AppendContactsListToGroup(capModel.contactsGroup, new List<CapContactModel4WS> { capContact });
        }

        /// <summary>
        /// Set the Cap information to App session(including the CapModel, PageFlowModel).
        /// If Associated Forms enabled, and set the Parent CapModel and parent PageFlowModel to Session.
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <param name="capType">the cap type model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="pageFlow">Specify page flow and set to session.
        /// If <see cref="pageFlow"/> parameter is null, will use <see cref="capType"/> and <see cref="capModel"/> to get page flow.
        /// </param>
        public static void SetCapInfoToAppSession(CapModel4WS capModel, CapTypeModel capType, string moduleName, PageFlowGroupModel pageFlow = null)
        {
            AppSession.SetCapModelToSession(moduleName, capModel);

            if (pageFlow == null)
            {
                pageFlow = PageFlowUtil.GetAssociatedPageFlowGroup(capModel);
            }

            pageFlow = GetPageFlowWithoutBlankPage(capModel, pageFlow);
            AppSession.SetPageflowGroupToSession(pageFlow);
            AppSession.BreadcrumbParams = null;

            //Set the parent info to session for the Associated Forms.
            SetAssoFormParentToAppSession(capModel, capType, pageFlow);
        }

        /// <summary>
        /// Get Agency Code List
        /// </summary>
        /// <param name="moduleName">the module Name</param>
        /// <returns>Agency code list.</returns>
        public static string[] GetAgencyCodeList(string moduleName)
        {
            IList<string> agencies = new List<string>();

            if (IsSuperCAP(moduleName))
            {
                ServiceModel[] services = AppSession.GetSelectedServicesFromSession();

                if (services != null)
                {
                    foreach (ServiceModel service in services)
                    {
                        if (!agencies.Contains(service.servPorvCode))
                        {
                            agencies.Add(service.servPorvCode);
                        }
                    }
                }
                else
                {
                    // from resume,get agencies from child caps.
                    CapModel4WS parentCap = AppSession.GetCapModelFromSession(moduleName);

                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                    CapIDModel4WS[] childCapIDs = capBll.GetChildCaps(parentCap.capID);

                    foreach (CapIDModel4WS childCap in childCapIDs)
                    {
                        if (!agencies.Contains(childCap.serviceProviderCode))
                        {
                            agencies.Add(childCap.serviceProviderCode);
                        }
                    }
                }
            }
            else
            {
                agencies.Add(ConfigManager.AgencyCode);
            }

            string[] agencyList = new string[agencies.Count];
            agencies.CopyTo(agencyList, 0);

            return agencyList;
        }

        /// <summary>
        /// set the save and resume later button's visible
        /// </summary>
        /// <param name="div">HtmlGenericControl object</param>
        /// <param name="moduleName">module name</param>
        public static void ShowSaveAndResumeLaterButton(HtmlGenericControl div, string moduleName)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string value = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_NO_SAVEANDRESUMELATER_BUTTON_MODULE);

            if (!string.IsNullOrEmpty(value))
            {
                string[] vs = value.Split(',');

                foreach (string v in vs)
                {
                    //if (v.Trim().ToUpper() == moduleName.ToUpper())
                    if (v.Trim().Equals(moduleName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        div.Visible = false;
                        return;
                    }
                }
            }

            div.Visible = true;
        }

        /// <summary>
        /// Handle the payment process
        /// </summary>
        /// <param name="capModel">the Cap Model</param>
        /// <param name="moduleName">the Module Name</param>
        /// <param name="isFromShoppingCart">whether come from the Shopping cart</param>
        public static void ToPaymentApplication(CapModel4WS capModel, string moduleName, bool isFromShoppingCart)
        {
            HttpContext context = HttpContext.Current;
            bool isSuperCap = IsSuperCAP(moduleName);
            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            bool isMultiCap = isSuperCap || isFromShoppingCart || isAssoFormEnabled;

            string preStepNumber = context.Request.QueryString["stepNumber"];
            bool isNumber = ValidationUtil.IsNumber(preStepNumber);
            int stepNumber = 0;
            if (isNumber)
            {
                stepNumber = int.Parse(preStepNumber) + 1;
            }

            string publicUserId = AppSession.User.PublicUserId;
            string isRenewalFlag = ACAConstant.COMMON_Y == context.Request["isRenewal"] ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            if ((capModel != null) && (capModel.capID != null))
            {
                // if there is no fee item list, directly go to successfully page.
                string url = string.Empty;
                bool hasFee = false;

                if (HasFee(capModel.capID, isSuperCap || isAssoFormEnabled, isAssoFormEnabled))
                {
                    //When all fee items is read only(user can't change the qty) and the total is zero, then skip pay fee page
                    if (IsSkipCapFeePage(capModel.capID, moduleName))
                    {
                        url = ToPaymentZeroFeeApplication(capModel, moduleName, stepNumber + 1, isMultiCap, isAssoFormEnabled, isRenewalFlag);
                    }
                    else
                    {
                        // Fee Items mey be added by EMSE Script
                        hasFee = true;

                        AppSession.SetCapModelToSession(moduleName, capModel);

                        url = string.Format("CapFees.aspx?stepNumber={0}&pageNumber=1&Module={1}&isRenewal={2}&isFromShoppingCart={3}", stepNumber, moduleName, isRenewalFlag, isFromShoppingCart ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);

                        //Typically - In Super agency environment, when user handler one sub agency's cap, needs a flag to indicates system to get data from corresponding agency.(such as: Fee items)
                        if (CloneRecordUtil.IsCloneRecord(context.Request) || ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
                        {
                            url += "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y;
                        }

                        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode]))
                        {
                            url += "&" + UrlConstant.AgencyCode + "=" + HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode];
                        }
                    }
                }
                else
                {
                    url = ToPaymentZeroFeeApplication(capModel, moduleName, stepNumber, isMultiCap, isAssoFormEnabled, isRenewalFlag);
                }

                CapUtil.SetBreadCrumbSession(publicUserId, moduleName, hasFee);

                context.Response.Redirect(url);
            }
        }

        /// <summary>
        /// Handle the zero fee  payment process
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="stepNumber">Step number for the next steps url</param>
        /// <param name="isMultiCap">Whether has child cap</param>
        /// <param name="isAssoFormEnabled">Whether Associated Forms is enabled</param>
        /// <param name="isRenewalFlag">Renewal flag</param>
        /// <returns>the url after payment.</returns>
        public static string ToPaymentZeroFeeApplication(CapModel4WS capModel, string moduleName, int stepNumber, bool isMultiCap, bool isAssoFormEnabled, string isRenewalFlag)
        {
            SysUserModel4WS sysUser = new SysUserModel4WS();
            sysUser.userID = AppSession.User.PublicUserId;
            sysUser.firstName = AppSession.User.FirstName;
            sysUser.lastName = AppSession.User.LastName;
            capModel.sysUser = sysUser;

            CapIDModel4WS[] capIds = GetChildCapIDs(capModel, moduleName, isAssoFormEnabled);
            AppSession.SetOnlinePaymentResultModelToSession(ShoppingCartUtil.CreateZeroFeeCAPs(capIds));

            string url;
            if (isMultiCap || StandardChoiceUtil.IsEnableShoppingCart())
            {
                if (StandardChoiceUtil.IsEnableShoppingCart())
                {
                    /*
                     * Anonymous user will not see the Shopping Cart step, the 'stepNumber' need not be changed£¬
                     * otherwise the url will be redirected incorrectly.
                     */
                    if (!AppSession.User.IsAnonymous)
                    {
                        stepNumber = LASTSTEP_IN_SHOPPINGCART;
                    }
                }

                url = string.Format("CapCompletions.aspx?&stepNumber={0}&Module={1}", stepNumber, moduleName);
            }
            else
            {
                url = string.Format("CapCompletion.aspx?&stepNumber={0}&Module={1}&isRenewal={2}", stepNumber, moduleName, isRenewalFlag);
            }

            return url;
        }

        /// <summary>
        /// Validated credit card expiration date
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="theYeaOfExpDate">the year of expiration date</param>
        /// <param name="theMonthOfExpDate">the month of expiration date</param>
        /// <returns>return true if expiration date less than current date</returns>
        public static bool ValidatedCreditCardExpDate(string agencyCode, int theYeaOfExpDate, int theMonthOfExpDate)
        {
            ITimeZoneBll timeZoneBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
            DateTime agencyDate = timeZoneBll.GetAgencyCurrentDate(agencyCode);

            DateTime agecyCurrentMonth = new DateTime(agencyDate.Year, agencyDate.Month, 1);
            DateTime expDate = new DateTime(theYeaOfExpDate, theMonthOfExpDate, 1);

            if (agecyCurrentMonth.CompareTo(expDate) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Validate License Professional Type
        /// </summary>
        /// <param name="licensePros">the license professional model.</param>
        /// <returns>return true if cap model match license type.</returns>
        public static bool ValidateLicenseProfessionType(LicenseProfessionalModel4WS[] licensePros)
        {
            ServiceModel[] serviceList = AppSession.GetSelectedServicesFromSession();

            bool isMatch = true;
            if (serviceList == null || serviceList.Length == 0)
            {
                return true;
            }

            foreach (ServiceModel service in serviceList)
            {
                string[] lpTypes = service.licProType;
                if (lpTypes == null || lpTypes.Length == 0)
                {
                    continue;
                }

                bool isContain = false;
                if (licensePros != null)
                {
                    foreach (LicenseProfessionalModel4WS lp in licensePros)
                    {
                        if (lpTypes.Contains(lp.licenseType))
                        {
                            isContain = true;
                            break;
                        }
                    }
                }

                if (!isContain)
                {
                    isMatch = false;
                    break;
                }
            }

            return isMatch;
        }

        /// <summary>
        /// Combine the applicant model and contacts group to one group
        /// </summary>
        /// <param name="applicant">applicant model</param>
        /// <param name="contactsGroup">old contacts group</param>
        /// <returns>new contacts group</returns>
        public static CapContactModel4WS[] CombineApplicantToContactsGroup(CapContactModel4WS applicant, CapContactModel4WS[] contactsGroup)
        {
            CapContactModel4WS[] contacts = contactsGroup;

            if (applicant != null && applicant.people != null && !string.IsNullOrEmpty(applicant.people.contactType))
            {
                if (contactsGroup == null || contactsGroup.Length == 0)
                {
                    contacts = new CapContactModel4WS[1];
                }
                else
                {
                    contacts = new CapContactModel4WS[contactsGroup.Length + 1];

                    for (int i = 0; i < contactsGroup.Length; i++)
                    {
                        contacts[i + 1] = contactsGroup[i];
                    }
                }

                contacts[0] = applicant;
            }

            return contacts;
        }

        /// <summary>
        /// TransForm PublicUserModel4WS to OwnerModel.
        /// </summary>
        /// <param name="sourceSeqNumber">source sequence Number</param>
        /// <param name="ownerNumber">owner Number</param>
        /// <param name="ownerUID">owner unique ID</param>
        /// <param name="moduleName">the module name.</param>
        /// <param name="agencyCode">the agency Code.</param>
        /// <returns>owner model</returns>
        public static StringBuilder ConvertToOwnerModel(string sourceSeqNumber, string ownerNumber, string ownerUID, string moduleName, string agencyCode)
        {
            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(moduleName));
            TemplateAttributeModel[] attributes = null;

            if (user == null || user.ownerModel == null)
            {
                return null;
            }

            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));

            OwnerModel refOwnerModel =
                ownerBll.GetOwnerCondition(agencyCode, sourceSeqNumber, ownerNumber, ownerUID);

            StringBuilder sb = new StringBuilder("{\"normalField\":{");

            if (ConditionsUtil.IsOwnerLocked(refOwnerModel))
            {
                CapUtil.AddKeyValue(sb, "IsLocked", "true");
                sb.Length -= 1;
                sb.Append("}");
                sb.Append(",");
                return sb;
            }
            else
            {
                foreach (OwnerModel ownerModel in user.ownerModel)
                {
                    bool isExternalAPO = false;
                    bool isInternalAPO = false;

                    if (string.IsNullOrEmpty(ownerNumber) || ownerModel.ownerNumber == null)
                    {
                        if (ownerModel.sourceSeqNumber == StringUtil.ToLong(sourceSeqNumber)
                            && ownerModel.UID.Equals(ownerUID, StringComparison.InvariantCultureIgnoreCase))
                        {
                            isExternalAPO = true;
                        }
                    }
                    else
                    {
                        if (ownerModel.sourceSeqNumber == StringUtil.ToLong(sourceSeqNumber)
                           && ownerModel.ownerNumber == StringUtil.ToLong(ownerNumber))
                        {
                            isInternalAPO = true;
                        }
                    }

                    if (isExternalAPO || isInternalAPO)
                    {
                        CapUtil.AddKeyValue(sb, "Title", ScriptFilter.EncodeJson(ownerModel.ownerTitle));
                        CapUtil.AddKeyValue(sb, "OwnerName", ScriptFilter.EncodeJson(ownerModel.ownerFullName));
                        CapUtil.AddKeyValue(sb, "Address1", ScriptFilter.EncodeJson(ownerModel.mailAddress1));
                        CapUtil.AddKeyValue(sb, "Address2", ScriptFilter.EncodeJson(ownerModel.mailAddress2));
                        CapUtil.AddKeyValue(sb, "Address3", ScriptFilter.EncodeJson(ownerModel.mailAddress3));
                        CapUtil.AddKeyValue(sb, "Zip", ModelUIFormat.FormatZipShow(ownerModel.mailZip, ownerModel.mailCountry));
                        CapUtil.AddKeyValue(sb, "City", ownerModel.mailCity);
                        CapUtil.AddKeyValue(sb, "State", ownerModel.mailState);
                        CapUtil.AddKeyValue(sb, "Country", ownerModel.mailCountry);
                        CapUtil.AddKeyValue(sb, "OwnerNumber", ownerNumber);
                        CapUtil.AddKeyValue(sb, "OwnerUID", ownerModel.UID);
                        CapUtil.AddKeyValue(sb, "IsLocked", "false");
                        CapUtil.AddKeyValue(sb, "Fein", string.Empty);
                        CapUtil.AddKeyValue(sb, "TradeName", string.Empty);
                        CapUtil.AddKeyValue(sb, "Fax", ModelUIFormat.FormatPhone4EditPage(ownerModel.fax, ownerModel.mailCountry));
                        CapUtil.AddKeyValue(sb, "FaxIDD", ownerModel.faxCountryCode);
                        CapUtil.AddKeyValue(sb, "Phone", ModelUIFormat.FormatPhone4EditPage(ownerModel.phone, ownerModel.mailCountry));
                        CapUtil.AddKeyValue(sb, "PhoneIDD", ownerModel.phoneCountryCode);
                        CapUtil.AddKeyValue(sb, "Email", ownerModel.email);
                        CapUtil.AddKeyValue(sb, "SourceSeq", ownerModel.sourceSeqNumber == null ? string.Empty : Convert.ToString(ownerModel.sourceSeqNumber));
                        string apoKeysJson = string.Empty;

                        if (ownerModel.duplicatedAPOKeys != null)
                        {
                            apoKeysJson = JsonConvert.SerializeObject(ownerModel.duplicatedAPOKeys);
                            sb.AppendFormat("\"{0}\":{1},", "APOKeys", apoKeysJson);
                        }
                        else
                        {
                            sb.AppendFormat("\"{0}\":\"{1}\",", "APOKeys", apoKeysJson);
                        }

                        if (!ConfigManager.SuperAgencyCode.Equals(agencyCode, StringComparison.OrdinalIgnoreCase))
                        {
                            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                            attributes = templateBll.GetRefAPOTemplateAttributes(TemplateType.CAP_OWNER, ownerNumber, agencyCode, AppSession.User.PublicUserId);
                        }
                        else
                        {
                            attributes = ownerModel.templates;
                        }

                        break;
                    }
                }
            }

            if (attributes != null && attributes.Length > 0)
            {
                sb.Length -= 1;
                sb.Append("}" + ",\"templateField\":{");
                foreach (TemplateAttributeModel attribute in attributes)
                {
                    string controlType = attribute.attributeValueDataType.ToUpperInvariant();
                    string value4Setting = ModelUIFormat.GetI18NTemplateValue(attribute);

                    if (controlType.Equals(ControlType.Radio.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        value4Setting = ValidationUtil.IsYes(value4Setting) ? ACA.Common.ACAConstant.COMMON_Yes : value4Setting;
                        value4Setting = ValidationUtil.IsNo(value4Setting) ? ACA.Common.ACAConstant.COMMON_No : value4Setting;
                    }

                    IControlEntity ctlEntity = ControlBuildHelper.BuildControlEntity(attribute, ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX);
                    CapUtil.AddKeyValue(sb, ctlEntity.ControlID, value4Setting);
                }
            }

            sb.Length -= 1;
            sb.Append("}");
            sb.Append(",");

            return sb;
        }

        /// <summary>
        /// Gets reference data from cache and merge reference data to UI data.
        /// If cannot find reference data from cache, then get reference data from current public user.
        /// To prevent lose the data which fields hidden by form designer.
        /// </summary>
        /// <typeparam name="T">Target model type</typeparam>
        /// <typeparam name="R">Public user associated model type</typeparam>
        /// <param name="targetObject">The target object.</param>
        /// <param name="publicUserPropName">Public user's property name used to get the data model from public user model.</param>
        /// <param name="propNamePrefix">Property name prefix stored in View element for the nesting data model.</param>
        /// <param name="pkPropNames">
        /// PK fields' property names of the Data model associated to public user used to get the unique data from Public User model.
        /// Split by ACAConstant.SPLIT_CHAR, only supported numeric, string, boolean field</param>
        /// <param name="pkPropValues">
        /// PK fields' values of the Data model associated to public user used to get the unique data from public user Model.
        /// Split by ACAConstant.SPLIT_CHAR, only supported numeric, string, boolean field</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="referenceEntity">The reference entity.</param>
        /// <param name="permission">The GView permission used to get the view elements.</param>
        /// <param name="viewId">The view id used to get the view elements.</param>
        public static void MergeRefDataToUIData<T, R>(
            ref T targetObject,
            string publicUserPropName,
            string propNamePrefix,
            string pkPropNames,
            string pkPropValues,
            string moduleName,
            object referenceEntity,
            GFilterScreenPermissionModel4WS permission,
            string viewId)
            where T : class
            where R : class
        {
            if (targetObject == null)
            {
                return;
            }

            bool findCache = false;
            T tEntityCache = null;
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(
                moduleName, permission, viewId, AppSession.User.UserID);

            if (referenceEntity is T)
            {
                tEntityCache = referenceEntity as T;
                findCache = true;
                CopyInvisibleFieldsByGview<T>(tEntityCache, targetObject, models, propNamePrefix);
            }

            if (!findCache)
            {
                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                PublicUserModel4WS model = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(moduleName));

                PropertyInfo property = model.GetType().GetProperty(publicUserPropName);

                if (property == null)
                {
                    return;
                }

                R[] refEntityList = property.GetValue(model, null) as R[];

                string[] refFieldNameArray = pkPropNames.Split(ACAConstant.SPLIT_CHAR);
                string[] refselectedValueArray = pkPropValues.Split(ACAConstant.SPLIT_CHAR);

                if (refEntityList != null && refEntityList.Length > 0 && refFieldNameArray.Length == refselectedValueArray.Length)
                {
                    foreach (var refEntity in refEntityList)
                    {
                        findCache = true;

                        for (int i = 0; i < refFieldNameArray.Length; i++)
                        {
                            object perpObject = refEntity.GetType().GetProperty(refFieldNameArray[i]).GetValue(refEntity, null);
                            string perpValue = perpObject == null ? string.Empty : perpObject.ToString();

                            if (!refselectedValueArray[i].Equals(perpValue) &&
                                (!string.IsNullOrEmpty(refselectedValueArray[i]) || !string.IsNullOrEmpty(perpValue)))
                            {
                                findCache = false;
                                break;
                            }
                        }

                        if (findCache)
                        {
                            tEntityCache = ObjectR2ObjectT<T, R>(refEntity);
                            CopyInvisibleFieldsByGview<T>(tEntityCache, targetObject, models, propNamePrefix);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get ASI Field's value that will be display in UI
        /// </summary>
        /// <param name="field">ASI Model Value</param>
        /// <param name="expField">Expression field</param>
        /// <returns>ASI Field's value</returns>
        public static string GetASIFieldValue(AppSpecificInfoModel4WS field, ExpressionFieldModel expField)
        {
            string fieldValue = string.Empty;

            if (field == null)
            {
                return string.Empty;
            }

            int fieldType = field.fieldType != null ? int.Parse(field.fieldType) : 0;

            if (expField != null)
            {
                fieldValue = expField.value != null ? expField.value.ToString() : string.Empty;

                // if drop down field is set by expression, it should judge it is in the drop field options or not.
                if (fieldType == (int)FieldType.HTML_SELECTBOX && field.valueList != null)
                {
                    string tempValue = fieldValue;
                    fieldValue = string.Empty;

                    foreach (var option in field.valueList)
                    {
                        if (option.resAttrValue == tempValue || option.attrValue == tempValue)
                        {
                            fieldValue = I18nStringUtil.GetString(option.resAttrValue, option.attrValue);
                            break;
                        }
                    }
                }
            }
            else
            {
                switch (fieldType)
                {
                    case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                    case (int)FieldType.HTML_CHECKBOX:
                    case (int)FieldType.HTML_RADIOBOX:
                        fieldValue = field.checklistComment;
                        break;

                    default:
                        fieldValue = I18nStringUtil.GetString(field.resChecklistComment, field.checklistComment);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(fieldValue))
            {
                switch (fieldType)
                {
                    case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                        fieldValue = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(fieldValue);
                        break;

                    case (int)FieldType.HTML_CHECKBOX:
                    case (int)FieldType.HTML_RADIOBOX:
                        fieldValue = ModelUIFormat.FormatYNLabel(fieldValue);
                        break;

                    case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                    case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                        fieldValue = I18nNumberUtil.ConvertDecimalFromWebServiceToInput(fieldValue);
                        break;
                }
            }

            return fieldValue;
        }

        /// <summary>
        /// Get ASI field's label
        /// </summary>
        /// <param name="field">ASI Field Model</param>
        /// <returns>Field Label</returns>
        public static string GetASIFieldLabel(AppSpecificInfoModel4WS field)
        {
            string fieldLabel = string.Empty;

            if (field != null)
            {
                fieldLabel = I18nStringUtil.GetString(field.resAlternativeLabel, field.alternativeLabel);

                if (string.IsNullOrEmpty(fieldLabel))
                {
                    fieldLabel = I18nStringUtil.GetString(field.resFiledLabel, field.fieldLabel);
                }

                fieldLabel += ACAConstant.COLON_CHAR;
            }

            return fieldLabel;
        }

        /// <summary>
        /// Get matched reference address model from public user model and convert to JSON string.
        /// </summary>
        /// <param name="sourceNumber">the APO source sequence Number</param>
        /// <param name="refAddressId">the ref AddressId</param>
        /// <param name="refAddressUID">the ref Address UID</param>
        /// <param name="moduleName">the module Name</param>
        /// <param name="agencyCode">the agency Code</param>
        /// <returns>Address information of JSON string</returns>
        public static StringBuilder ConvertToAddressModel(string sourceNumber, string refAddressId, string refAddressUID, string moduleName, string agencyCode)
        {
            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS model = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(moduleName));
            IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();
            StringBuilder sb = new StringBuilder();
            TemplateAttributeModel[] attributes = null;

            if (model != null && model.addressList != null)
            {
                RefAddressModel[] refAddresses = model.addressList;

                if (refAddresses.Length > 0)
                {
                    foreach (RefAddressModel refAddress in refAddresses)
                    {
                        //Find matched reference address from public user associated address list.
                        if (refAddress.sourceNumber == StringUtil.ToInt(sourceNumber) && refAddress.refAddressId == StringUtil.ToLong(refAddressId)
                            && ((string.IsNullOrEmpty(refAddress.UID) && string.IsNullOrEmpty(refAddressUID)) || refAddress.UID == refAddressUID))
                        {
                            sb.Append("{");

                            if (conditionBll.IsAddressLocked(ConfigManager.AgencyCode, refAddress, AppSession.User.PublicUserId))
                            {
                                CapUtil.AddKeyValue(sb, "IsLocked", "true");
                                return sb;
                            }

                            //CapUtil.AddKeyValue(sb, "ContactRefSeqNumber", stateContact);
                            CapUtil.AddKeyValue(sb, "houseNumberStart", Convert.ToString(refAddress.houseNumberStart));
                            CapUtil.AddKeyValue(sb, "streetDirection", refAddress.streetDirection);
                            CapUtil.AddKeyValue(sb, "streetSuffix", refAddress.streetSuffix);
                            CapUtil.AddKeyValue(sb, "unitType", refAddress.unitType);
                            CapUtil.AddKeyValue(sb, "streetName", ScriptFilter.EncodeJson(refAddress.streetName));
                            CapUtil.AddKeyValue(sb, "city", refAddress.city);
                            CapUtil.AddKeyValue(sb, "zip", ModelUIFormat.FormatZipShow(refAddress.zip, refAddress.countryCode));
                            CapUtil.AddKeyValue(sb, "unitStart", refAddress.unitStart);
                            CapUtil.AddKeyValue(sb, "county", refAddress.county);
                            CapUtil.AddKeyValue(sb, "refAddressId", Convert.ToString(refAddress.refAddressId));
                            CapUtil.AddKeyValue(sb, "UID", refAddress.UID);
                            CapUtil.AddKeyValue(sb, "streetPrefix", refAddress.streetPrefix);
                            CapUtil.AddKeyValue(sb, "houseNumberEnd", Convert.ToString(refAddress.houseNumberEnd));
                            CapUtil.AddKeyValue(sb, "unitEnd", refAddress.unitEnd);
                            CapUtil.AddKeyValue(sb, "countryCode", refAddress.countryCode);
                            CapUtil.AddKeyValue(sb, "state", refAddress.state);
                            CapUtil.AddKeyValue(sb, "houseFractionStart", refAddress.houseFractionStart);
                            CapUtil.AddKeyValue(sb, "houseFractionEnd", refAddress.houseFractionEnd);
                            CapUtil.AddKeyValue(sb, "streetSuffixdirection", refAddress.streetSuffixdirection);
                            CapUtil.AddKeyValue(sb, "addressDescription", ScriptFilter.EncodeJson(refAddress.addressDescription));
                            CapUtil.AddKeyValue(sb, "distance", Convert.ToString(refAddress.distance));
                            CapUtil.AddKeyValue(sb, "secondaryRoad", refAddress.secondaryRoad);
                            CapUtil.AddKeyValue(sb, "secondaryRoadNumber", Convert.ToString(refAddress.secondaryRoadNumber));
                            CapUtil.AddKeyValue(sb, "inspectionDistrict", refAddress.inspectionDistrict);
                            CapUtil.AddKeyValue(sb, "inspectionDistrictPrefix", refAddress.inspectionDistrictPrefix);
                            CapUtil.AddKeyValue(sb, "neighberhoodPrefix", refAddress.neighborhoodPrefix);
                            CapUtil.AddKeyValue(sb, "neighborhood", refAddress.neighborhood);
                            CapUtil.AddKeyValue(sb, "XCoordinator", Convert.ToString(refAddress.XCoordinator));
                            CapUtil.AddKeyValue(sb, "YCoordinator", Convert.ToString(refAddress.YCoordinator));
                            CapUtil.AddKeyValue(sb, "fullAddress", ScriptFilter.EncodeJson(refAddress.fullAddress));
                            CapUtil.AddKeyValue(sb, "addressLine1", ScriptFilter.EncodeJson(refAddress.addressLine1));
                            CapUtil.AddKeyValue(sb, "addressLine2", ScriptFilter.EncodeJson(refAddress.addressLine2));

                            CapUtil.AddKeyValue(sb, "levelPrefix", ScriptFilter.EncodeJson(refAddress.levelPrefix));
                            CapUtil.AddKeyValue(sb, "levelNumberStart", ScriptFilter.EncodeJson(refAddress.levelNumberStart));
                            CapUtil.AddKeyValue(sb, "levelNumberEnd", ScriptFilter.EncodeJson(refAddress.levelNumberEnd));
                            CapUtil.AddKeyValue(sb, "houseAlphaStart", ScriptFilter.EncodeJson(refAddress.houseNumberAlphaStart));
                            CapUtil.AddKeyValue(sb, "houseAlphaEnd", ScriptFilter.EncodeJson(refAddress.houseNumberAlphaEnd));
                            CapUtil.AddKeyValue(sb, "sourceNumber", Convert.ToString(refAddress.sourceNumber));

                            if (!ConfigManager.SuperAgencyCode.Equals(agencyCode, StringComparison.OrdinalIgnoreCase))
                            {
                                ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                                attributes = templateBll.GetRefAPOTemplateAttributes(TemplateType.CAP_ADDRESS, refAddressId, agencyCode, AppSession.User.PublicUserId);
                            }
                            else
                            {
                                attributes = refAddress.templates;
                            }

                            break;
                        }
                    }
                }
            }

            if (attributes != null && attributes.Length > 0)
            {
                sb.Length -= 1;
                sb.Append("}" + ACAConstant.SPLIT_CHAR + "{");
                int i = 0;

                foreach (TemplateAttributeModel attribute in attributes)
                {
                    string controlType = attribute.attributeValueDataType.ToUpperInvariant();
                    string value4Setting = ModelUIFormat.GetI18NTemplateValue(attribute);

                    if (controlType.Equals(ControlType.Radio.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        value4Setting = ValidationUtil.IsYes(value4Setting) ? ACA.Common.ACAConstant.COMMON_Yes : value4Setting;
                        value4Setting = ValidationUtil.IsNo(value4Setting) ? ACA.Common.ACAConstant.COMMON_No : value4Setting;
                    }

                    IControlEntity ctlEntity = ControlBuildHelper.BuildControlEntity(attribute, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
                    CapUtil.AddKeyValue(sb, ctlEntity.ControlID, value4Setting);
                    i++;
                }
            }

            return sb;
        }

        /// <summary>
        /// Get matched reference parcel model from public user model and convert to JSON string.
        /// </summary>
        /// <param name="sourceNumber">the APO source sequence Number</param>
        /// <param name="parcelNumber">the parcel Number</param>
        /// <param name="parcelUID">the parcel UID</param>
        /// <param name="moduleName">the module Name</param>
        /// <param name="agencyCode">the agency Code</param>
        /// <returns>Parcel information of JSON string</returns>
        public static StringBuilder ConvertToParcelModel(string sourceNumber, string parcelNumber, string parcelUID, string moduleName, string agencyCode)
        {
            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS model = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(moduleName));
            IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();
            StringBuilder sb = new StringBuilder();
            TemplateAttributeModel[] attributes = null;

            if (model != null && model.parcelList != null)
            {
                ParcelModel[] parcels = model.parcelList;
                int parcelsLen = parcels.Length;

                if (parcelsLen > 0)
                {
                    foreach (ParcelModel parcel in parcels)
                    {
                        //Find matched parcel from public user associated parcel list.
                        if (parcel.sourceSeqNumber == StringUtil.ToLong(sourceNumber)
                            && ((string.IsNullOrEmpty(parcel.parcelNumber) && string.IsNullOrEmpty(parcelNumber)) || parcel.parcelNumber == parcelNumber)
                            && ((string.IsNullOrEmpty(parcel.UID) && string.IsNullOrEmpty(parcelUID)) || parcel.UID == parcelUID))
                        {
                            sb.Append("{");

                            if (conditionBll.IsParcelLocked(ConfigManager.AgencyCode, parcel, AppSession.User.PublicUserId))
                            {
                                CapUtil.AddKeyValue(sb, "IsLocked", "true");
                                return sb;
                            }

                            CapUtil.AddKeyValue(sb, "parcelNumber", parcel.parcelNumber);
                            CapUtil.AddKeyValue(sb, "lot", parcel.lot);
                            CapUtil.AddKeyValue(sb, "block", parcel.block);
                            CapUtil.AddKeyValue(sb, "subdivision", parcel.subdivision);
                            CapUtil.AddKeyValue(sb, "book", parcel.book);
                            CapUtil.AddKeyValue(sb, "page", parcel.page);
                            CapUtil.AddKeyValue(sb, "tract", ScriptFilter.EncodeJson(parcel.tract));
                            CapUtil.AddKeyValue(sb, "legalDesc", ScriptFilter.EncodeJson(parcel.legalDesc));
                            CapUtil.AddKeyValue(sb, "parcelArea", Convert.ToString(parcel.parcelArea));
                            CapUtil.AddKeyValue(sb, "landValue", Convert.ToString(parcel.landValue));
                            CapUtil.AddKeyValue(sb, "improvedValue", Convert.ToString(parcel.improvedValue));
                            CapUtil.AddKeyValue(sb, "exemptValue", Convert.ToString(parcel.exemptValue));
                            CapUtil.AddKeyValue(sb, "l1ParcelNo", parcel.parcelNumber);
                            CapUtil.AddKeyValue(sb, "sourceSeq", Convert.ToString(parcel.sourceSeqNumber));
                            CapUtil.AddKeyValue(sb, "UID", parcel.UID);

                            if (!ConfigManager.SuperAgencyCode.Equals(agencyCode, StringComparison.OrdinalIgnoreCase))
                            {
                                ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));

                                attributes = templateBll.GetRefAPOTemplateAttributes(
                                    TemplateType.CAP_PARCEL,
                                    parcelNumber,
                                    agencyCode,
                                    AppSession.User.PublicUserId);
                            }
                            else
                            {
                                attributes = parcel.templates;
                            }

                            break;
                        }
                    }
                }
            }

            if (attributes != null && attributes.Length > 0)
            {
                sb.Length -= 1;
                sb.Append("}" + ACAConstant.SPLIT_CHAR + "{");
                int i = 0;

                foreach (TemplateAttributeModel attribute in attributes)
                {
                    string controlType = attribute.attributeValueDataType.ToUpperInvariant();
                    string value4Setting = ModelUIFormat.GetI18NTemplateValue(attribute);

                    if (controlType.Equals(ControlType.Radio.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        value4Setting = ValidationUtil.IsYes(value4Setting) ? ACA.Common.ACAConstant.COMMON_Yes : value4Setting;
                        value4Setting = ValidationUtil.IsNo(value4Setting) ? ACA.Common.ACAConstant.COMMON_No : value4Setting;
                    }

                    IControlEntity ctlEntity = ControlBuildHelper.BuildControlEntity(attribute, ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX);
                    CapUtil.AddKeyValue(sb, ctlEntity.ControlID, value4Setting);
                    i++;
                }
            }

            return sb;
        }

        /// <summary>
        /// TransForm LicenseModel to PeopleModel
        /// </summary>
        /// <param name="stateLicense">a License model</param>
        /// <param name="licenseType">a License type model</param>
        /// <returns>a PeopleModel4WS model</returns>
        public static StringBuilder ConvertToPeopleModel(string stateLicense, string licenseType)
        {
            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));

            LicenseModel4WS model = new LicenseModel4WS();
            model.licenseType = licenseType;
            model.stateLicense = stateLicense;
            model.serviceProviderCode = ConfigManager.AgencyCode;

            model = licenseBll.GetLicenseByStateLicNbr(model);

            if (model == null)
            {
                return null;
            }

            LicenseModel4WS licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(model.licSeqNbr), AppSession.User.PublicUserId);

            StringBuilder sb = new StringBuilder("{");

            if (ConditionsUtil.IsLicenseLocked(licenseModel))
            {
                AddKeyValue(sb, "IsLocked", "true");
                return sb;
            }
            else
            {
                AddKeyValue(sb, "Salutation", model.salutation);
                AddKeyValue(sb, "Title", model.title);
                AddKeyValue(sb, "FirstName", ScriptFilter.EncodeJson(model.contactFirstName));
                AddKeyValue(sb, "LastName", ScriptFilter.EncodeJson(model.contactLastName));
                AddKeyValue(sb, "FullName", string.Empty);
                AddKeyValue(sb, "BirthDate", I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(model.birthDate));
                AddKeyValue(sb, "Gender", model.gender);
                AddKeyValue(sb, "MiddleName", ScriptFilter.EncodeJson(model.contactMiddleName));
                AddKeyValue(sb, "BusinessName", ScriptFilter.EncodeJson(model.businessName));
                AddKeyValue(sb, "Country", model.countryCode);
                AddKeyValue(sb, "Address1", ScriptFilter.EncodeJson(model.address1));
                AddKeyValue(sb, "Address2", ScriptFilter.EncodeJson(model.address2));
                AddKeyValue(sb, "Address3", ScriptFilter.EncodeJson(model.address3));
                AddKeyValue(sb, "City", ScriptFilter.EncodeJson(model.city));
                AddKeyValue(sb, "State", model.state);
                AddKeyValue(sb, "Zip", ModelUIFormat.FormatZipShow(model.zip, model.countryCode));
                AddKeyValue(sb, "HomePhoneIDD", model.phone1CountryCode);
                AddKeyValue(sb, "HomePhone", ModelUIFormat.FormatPhone4EditPage(model.phone1, model.countryCode));
                AddKeyValue(sb, "PostOfficeBox", model.postOfficeBox);
                AddKeyValue(sb, "MobilePhoneIDD", model.phone2CountryCode);
                AddKeyValue(sb, "MobilePhone", ModelUIFormat.FormatPhone4EditPage(model.phone2, model.countryCode));
                AddKeyValue(sb, "WorkPhoneIDD", model.phone3CountryCode);
                AddKeyValue(sb, "WorkPhone", ModelUIFormat.FormatPhone4EditPage(model.phone3, model.countryCode));
                AddKeyValue(sb, "FaxIDD", model.faxCountryCode);
                AddKeyValue(sb, "Fax", ModelUIFormat.FormatPhone4EditPage(model.fax, model.countryCode));
                AddKeyValue(sb, "Email", model.emailAddress);
                AddKeyValue(sb, "Suffix", ScriptFilter.EncodeJson(model.suffixName));
                AddKeyValue(sb, "SSN", model.socialSecurityNumber);
                AddKeyValue(sb, "Fein", model.fein);
                AddKeyValue(sb, "TradeName", string.Empty);
                AddKeyValue(sb, "ContactTypeFlag", model.typeFlag);

                AddKeyValue(sb, "BusinessName2", string.Empty);
                AddKeyValue(sb, "BirthplaceCity", string.Empty);
                AddKeyValue(sb, "BirthplaceState", string.Empty);
                AddKeyValue(sb, "BirthplaceCountry", string.Empty);
                AddKeyValue(sb, "Race", string.Empty);
                AddKeyValue(sb, "DeceasedDate", string.Empty);
                AddKeyValue(sb, "PassportNumber", string.Empty);
                AddKeyValue(sb, "DriverLicenseNumber", string.Empty);
                AddKeyValue(sb, "DriverLicenseState", string.Empty);
                AddKeyValue(sb, "StateIdNumber", string.Empty);
                AddKeyValue(sb, "PreferredChannel", string.Empty);
                AddKeyValue(sb, "Notes", string.Empty);
                AddKeyValue(sb, "PeopleAKA", string.Empty);

                sb.Append("\"Templates\":" + BuildTemplateFiledString(model.templateAttributes, string.Empty) + ",");
                sb.Append("\"GenericTemplate\":" + BuildGenericTemplateFieldString(model.template));
                sb.Append(",");
            }

            return sb;
        }

        /// <summary>
        /// TransForm OwnerModel to PeopleModel
        /// </summary>
        /// <param name="ownerNumber">a Owner model</param>
        /// <param name="sourceSeqNumber">Source sequence number of APO</param>
        /// <param name="ownerUID">owner unique id for external APO.</param>
        /// <param name="agencyCode">the agency Code.</param>
        /// <returns> a PeopleModel4WS model</returns>
        public static StringBuilder ConvertToPeopleModel(string ownerNumber, string sourceSeqNumber, string ownerUID, string agencyCode)
        {
            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
            TemplateAttributeModel[] attributes = null;
            OwnerModel ownerPK = new OwnerModel();
            ownerPK.sourceSeqNumber = StringUtil.ToLong(sourceSeqNumber);
            ownerPK.ownerNumber = StringUtil.ToLong(ownerNumber);
            ownerPK.UID = ownerUID;

            OwnerModel model = ownerBll.GetOwnerByPK(ConfigManager.AgencyCode, ownerPK);

            if (model == null)
            {
                return null;
            }

            OwnerModel refOwnerModel =
                ownerBll.GetOwnerCondition(ConfigManager.AgencyCode, sourceSeqNumber, ownerNumber, ownerUID);

            StringBuilder sb = new StringBuilder("{");

            if (ConditionsUtil.IsOwnerLocked(refOwnerModel))
            {
                AddKeyValue(sb, "IsLocked", "true");
                return sb;
            }
            else
            {
                AddKeyValue(sb, "Salutation", string.Empty);
                AddKeyValue(sb, "Title", ScriptFilter.EncodeJson(model.ownerTitle));
                AddKeyValue(sb, "FirstName", ScriptFilter.EncodeJson(model.ownerFirstName));
                AddKeyValue(sb, "LastName", ScriptFilter.EncodeJson(model.ownerLastName));
                AddKeyValue(sb, "FullName", ScriptFilter.EncodeJson(model.ownerFullName));
                AddKeyValue(sb, "BirthDate", string.Empty);
                AddKeyValue(sb, "Gender", string.Empty);
                AddKeyValue(sb, "MiddleName", ScriptFilter.EncodeJson(model.ownerMiddleName));
                AddKeyValue(sb, "BusinessName", ScriptFilter.EncodeJson(model.ownerFullName));
                AddKeyValue(sb, "Address1", ScriptFilter.EncodeJson(model.mailAddress1));
                AddKeyValue(sb, "Address2", ScriptFilter.EncodeJson(model.mailAddress2));
                AddKeyValue(sb, "Address3", ScriptFilter.EncodeJson(model.mailAddress3));
                AddKeyValue(sb, "City", model.mailCity);
                AddKeyValue(sb, "State", model.mailState);
                AddKeyValue(sb, "Zip", ModelUIFormat.FormatZipShow(model.mailZip, model.country));
                AddKeyValue(sb, "HomePhoneIDD", model.phoneCountryCode);
                AddKeyValue(sb, "HomePhone", ModelUIFormat.FormatPhone4EditPage(model.phone, model.country));
                AddKeyValue(sb, "PostOfficeBox", string.Empty);
                AddKeyValue(sb, "MobilePhoneIDD", string.Empty);
                AddKeyValue(sb, "MobilePhone", string.Empty); //Owner has only a phone
                AddKeyValue(sb, "WorkPhoneIDD", string.Empty);
                AddKeyValue(sb, "WorkPhone", string.Empty); //Owner has only a phone
                AddKeyValue(sb, "FaxIDD", model.faxCountryCode);
                AddKeyValue(sb, "Fax", ModelUIFormat.FormatPhone4EditPage(model.fax, model.country));
                AddKeyValue(sb, "Email", model.email);
                AddKeyValue(sb, "Country", model.mailCountry);
                AddKeyValue(sb, "SSN", string.Empty);
                AddKeyValue(sb, "Fein", string.Empty);
                AddKeyValue(sb, "TradeName", string.Empty);

                AddKeyValue(sb, "BusinessName2", string.Empty);
                AddKeyValue(sb, "BirthplaceCity", string.Empty);
                AddKeyValue(sb, "BirthplaceState", string.Empty);
                AddKeyValue(sb, "BirthplaceCountry", string.Empty);
                AddKeyValue(sb, "Race", string.Empty);
                AddKeyValue(sb, "DeceasedDate", string.Empty);
                AddKeyValue(sb, "PassportNumber", string.Empty);
                AddKeyValue(sb, "DriverLicenseNumber", string.Empty);
                AddKeyValue(sb, "DriverLicenseState", string.Empty);
                AddKeyValue(sb, "StateIdNumber", string.Empty);
                AddKeyValue(sb, "PreferredChannel", string.Empty);
                AddKeyValue(sb, "Notes", string.Empty);
                AddKeyValue(sb, "PeopleAKA", string.Empty);

                if (!ConfigManager.SuperAgencyCode.Equals(agencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                    attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_OWNER, agencyCode, AppSession.User.PublicUserId);
                }
                else
                {
                    attributes = model.templates;
                }

                sb.Append("\"Templates\":" + BuildTemplateFiledString(attributes, string.Empty) + ",");
            }

            return sb;
        }

        /// <summary>
        /// Get the parent CapID for the Associated Forms relationship.
        /// if the parent Cap is null, return the inputted CapID by parameter.
        /// </summary>
        /// <param name="capID">Cap ID model</param>
        /// <returns>parent Cap ID model</returns>
        public static CapIDModel4WS GetParentAssoFormCapID(CapIDModel4WS capID)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            CapIDModel capIdModel = TempModelConvert.Trim4WSOfCapIDModel(capID);
            CapIDModel parentCapID = capBll.GetParentCapIDByChildCapID(capIdModel, ACAConstant.CAP_RELATIONSHIP_ASSOFORM, null);
            CapIDModel4WS parentCapID4WS = TempModelConvert.Add4WSForCapIDModel(parentCapID);

            if (parentCapID4WS == null)
            {
                //The current Cap is the parent Cap of the Associated Forms
                return capID;
            }
            else
            {
                return parentCapID4WS;
            }
        }

        /// <summary>
        /// Get Module Name list by CapID model
        /// </summary>
        /// <param name="capIdModels">CapID model array</param>
        /// <returns>return module name list</returns>
        public static List<string> GetModuleNamesByCapID(CapIDModel[] capIdModels)
        {
            List<string> moduleNames = new List<string>();
            ICapBll capbll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
            StringArray[] modules = capbll.GetModuleNamesByCapIDList(capIdModels);
            foreach (StringArray item in modules)
            {
                if (!moduleNames.Contains(item.item[1]))
                {
                    moduleNames.Add(item.item[1]);
                }
            }

            return moduleNames;
        }

        /// <summary>
        /// Get payment group id list by cap ID list.
        /// </summary>
        /// <param name="capIDs">Cap ID List</param>
        /// <returns>Payment Group List</returns>
        public static IList<string> GetGroupIDListByCapIDs(CapIDModel[] capIDs)
        {
            IList<string> groupIDs = new List<string>();

            if (capIDs == null || capIDs.Length <= 0)
            {
                return groupIDs;
            }

            //1. get module list by capIDs
            IList<string> moduleNameList = GetModuleNamesByCapID(capIDs);

            //2. get group ID list by module list
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> paymentGroups = bizDomainBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PAYMENT_GROUP, false);

            if (moduleNameList != null && moduleNameList.Count > 0 && paymentGroups != null && paymentGroups.Count > 0)
            {
                foreach (string moduleName in moduleNameList)
                {
                    foreach (ItemValue group in paymentGroups)
                    {
                        if (group == null || string.IsNullOrEmpty(group.Key) || group.Value == null || string.IsNullOrEmpty(group.Value.ToString()))
                        {
                            continue;
                        }

                        //Append new group id, if group id or product id doesn't exist in the group id list.
                        if (string.Equals(moduleName, group.Key, StringComparison.OrdinalIgnoreCase) && !groupIDs.Contains<string>(group.Value.ToString()))
                        {
                            groupIDs.Add(group.Value.ToString());
                        }
                    }
                }
            }

            return groupIDs;
        }

        /// <summary>
        /// Check if some Records belong to different payment group in shopping cart.
        /// </summary>
        /// <param name="capIDs">Cap ID List</param>
        /// <returns>True: Has different payment group; False: without different payment group</returns>
        public static bool HasDifferentPaymentGroup(CapIDModel[] capIDs)
        {
            bool hasDifferentPaymentGroup = false;

            //1. Check payment group configuration in standard choice. If without the configuration, it means system use agency level as payment group.
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> paymentGroups = bizDomainBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PAYMENT_GROUP, false);

            if (capIDs != null && capIDs.Length > 0 && paymentGroups != null && paymentGroups.Count > 0)
            {
                //Check if current CapIDs belong to different payment group
                IList<string> groupIDs = GetGroupIDListByCapIDs(capIDs);

                //1. If GroupIDs.Count > 1, current Records must belong to different payment groups;
                //2. If GroupIDs.Count == 0, current Records is in same payment group;
                //3. If GroupIDs.Count == 1, needs to check more detail.
                if (groupIDs != null && groupIDs.Count > 1)
                {
                    hasDifferentPaymentGroup = true;
                }
                else if (groupIDs != null && groupIDs.Count == 1)
                {
                    Hashtable paymentGroup = new Hashtable();
                    foreach (ItemValue group in paymentGroups)
                    {
                        if (group == null || string.IsNullOrEmpty(group.Key) || group.Value == null || string.IsNullOrEmpty(group.Value.ToString()))
                        {
                            continue;
                        }

                        if (!paymentGroup.ContainsKey(group.Key))
                        {
                            paymentGroup.Add(group.Key, group.Value);
                        }
                    }

                    //Check current Records' module name in payment group
                    IList<string> moduleNameList = GetModuleNamesByCapID(capIDs);
                    if (moduleNameList != null && moduleNameList.Count > 1)
                    {
                        foreach (string moduleName in moduleNameList)
                        {
                            if (moduleName != null && !paymentGroup.ContainsKey(moduleName))
                            {
                                hasDifferentPaymentGroup = true;
                                break;
                            }
                        }
                    }
                }
            }

            return hasDifferentPaymentGroup;
        }

        /// <summary>
        /// build contact information with JSON format to client
        /// </summary>
        /// <param name="value">Comes from user action</param>
        /// <param name="moduleName">the module name.</param>
        /// <param name="agencyCode">the agency Code.</param>
        /// <param name="isFromAuthAgent">is from authorized agent or not.</param>
        /// <returns>JSON string value</returns>
        public static string GetPublicUserModel(string value, string moduleName, string agencyCode, bool isFromAuthAgent = false)
        {
            StringBuilder sb = null;

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

            //if AutoFillType have set.
            if (parameter.AutoFillType != ACAConstant.AutoFillType4SpearForm.None && !string.IsNullOrEmpty(parameter.SectionId))
            {
                if (HttpContext.Current.Session[SessionConstant.SESSION_REFERENCE_ENTITY_PREFIX + parameter.SectionId]
                    != null)
                {
                    HttpContext.Current.Session.Remove(
                        SessionConstant.SESSION_REFERENCE_ENTITY_PREFIX + parameter.SectionId);
                }

                switch (parameter.AutoFillType)
                {
                    case ACAConstant.AutoFillType4SpearForm.ContactOwner:
                        //Parameters:"Owner|ownerNumber|sourceSeqNumber|ownerUID".
                        sb = ConvertToPeopleModel(
                        parameter.EntityId, parameter.EntityType, parameter.EntityRefId, agencyCode);
                        break;
                    case ACAConstant.AutoFillType4SpearForm.Owner:
                        //Parameters:"OwnerItem|sourceSeqNumber|ownerNumber|ownerUID". 
                        //Get matched owner that associated with current public user.
                        sb = ConvertToOwnerModel(
                            parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName, agencyCode);
                        break;
                    case ACAConstant.AutoFillType4SpearForm.Address:
                        //Parameters:"AddressItem|sourceSeqNumber|refAddressID|addressUID". 
                        //Get matched address that associated with current public user.
                        sb = ConvertToAddressModel(
                            parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName, agencyCode);
                        break;
                    case ACAConstant.AutoFillType4SpearForm.Parcel:
                        //Parameters:"ParcelItem|sourceSeqNumber|ParcelNumber|ParcelUID". 
                        //Get matched parcel that associated with current public user.
                        sb = ConvertToParcelModel(
                            parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName, agencyCode);
                        break;
                    case ACAConstant.AutoFillType4SpearForm.Contact:
                        /*
                        * if has 3 parameters. 
                        * Parameters: "Contact|contactSeqNumber|contactType".
                        * 
                        * For Authorized Agent, the People is created in session before entered the Spear form,
                        *  so the people data will be retrieved from the session.
                        * For the normal process, the people data will be retrieved from the public user associated contacts.
                        */
                        if (isFromAuthAgent)
                        {
                            sb = ConvertToContactModel4AuthAgent(parameter.EntityRefId);
                        }
                        else
                        {
                            sb = ConvertToContactModel(parameter.EntityRefId, parameter.EntityType, moduleName);
                        }

                        break;
                    case ACAConstant.AutoFillType4SpearForm.License:
                        //Parameters:"License|stateLicense|licenseType|licenseSeqNumber". 
                        //Auto fill contact with Licensed Professional information.
                        sb = ConvertToPeopleModel(parameter.EntityId, parameter.EntityType);
                        break;
                }
            }

            if (sb == null)
            {
                return string.Empty;
            }

            sb.Length -= 1;
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Judge template attributes whether contains EMSE dropdown-list.
        /// </summary>
        /// <param name="attributes">template attribute model list</param>
        /// <returns>Yes: contains EMSE dropdown-list;No: don't contains EMSE dropdown-list</returns>
        public static bool IsContainsEMSEAttribute(TemplateAttributeModel[] attributes)
        {
            bool isContainsEMSEAttribute = false;

            if (attributes == null)
            {
                return isContainsEMSEAttribute;
            }

            foreach (TemplateAttributeModel attribute in attributes)
            {
                if (!string.IsNullOrEmpty(attribute.attributeScriptCode))
                {
                    isContainsEMSEAttribute = true;
                    break;
                }
            }

            return isContainsEMSEAttribute;
        }

        /// <summary>
        /// Judge license professional model whether contains EMSE dropdown list attributes.
        /// </summary>
        /// <param name="capIdModel">cap ID MODEL</param>
        /// <returns>TRUE:CONTAIN EMSE DROPDOWN LIST;FALSE:NO EMSE DROPDOWN LIST</returns>
        public static bool IsContainsEMSEDropDown(CapIDModel4WS capIdModel)
        {
            ILicenseProfessionalBll licenseProBll = (ILicenseProfessionalBll)ObjectFactory.GetObject(typeof(ILicenseProfessionalBll));
            ITemplateBll _templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));

            LicenseProfessionalModel[] licProModels = licenseProBll.GetLPListByCapID(ConfigManager.AgencyCode, capIdModel, AppSession.User.PublicUserId);
            bool isContainsEMSEDropDown = false;

            if (licProModels == null)
            {
                return isContainsEMSEDropDown;
            }

            foreach (LicenseProfessionalModel licenseModel in licProModels)
            {
                TemplateAttributeModel[] attributes = _templateBll.GetLPAttributes4SupportEMSE(
                    licenseModel.capID.serviceProviderCode,
                    licenseModel.licenseType,
                    licenseModel.licSeqNbr,
                    licenseModel.licenseNbr,
                    AppSession.User.PublicUserId);
                if (attributes != null && IsContainsEMSEAttribute(attributes))
                {
                    isContainsEMSEDropDown = true;
                    break;
                }
            }

            return isContainsEMSEDropDown;
        }

        /// <summary>
        /// template unit value internationalization.
        /// </summary>
        /// <param name="templateAttributes">people template attributes</param>
        /// <returns>people template attributes with unit res value</returns>
        public static TemplateAttributeModel[] SetPeopleTemplateUnitResValue(TemplateAttributeModel[] templateAttributes)
        {
            if (templateAttributes == null || templateAttributes.Length <= 0)
            {
                return templateAttributes;
            }

            for (int i = 0; i < templateAttributes.Length; i++)
            {
                TemplateAttributeModel template = templateAttributes[i];

                if (template == null || template.attributeUnitType == null)
                {
                    continue;
                }

                templateAttributes[i].resAttributeUnitType = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_PEOPLE_ATTRIBUTE_UNIT, template.attributeUnitType);
            }

            return templateAttributes;
        }

        /// <summary>
        /// Format phone show
        /// this method used by hasn't country's page, so send the empty country to get default setting.
        /// </summary>
        /// <param name="hfPhoneCode">control for phone code.</param>
        /// <param name="lblPhone">control for phone.</param>
        /// <param name="countryCode">country code</param>
        public static void FormatPhoneShow(HiddenField hfPhoneCode, AccelaLabel lblPhone, string countryCode)
        {
            string phoneCode = hfPhoneCode != null ? hfPhoneCode.Value : string.Empty;
            string phone = lblPhone != null ? lblPhone.Text : string.Empty;

            if (lblPhone != null)
            {
                //so send the empty country to get default setting.
                lblPhone.Text = ModelUIFormat.FormatPhoneShow(phoneCode, phone, countryCode);
            }
        }

        /// <summary>
        /// When all fee items is read only(user can't change the quantity) and the total is zero, then skip pay fee page
        /// </summary>
        /// <param name="capId">CapID model value</param>
        /// <param name="moduleName">module name value</param>
        /// <returns>true or false.</returns>
        public static bool IsSkipCapFeePage(CapIDModel4WS capId, string moduleName)
        {
            var fees = GetAllFeeItems(capId, moduleName);
            var skipCapFeePage = IsReadOnlyForAllFeeItems(fees) && CalculateFeeAmount(capId, moduleName) == 0;

            if (!skipCapFeePage && StandardChoiceUtil.IsOnlyShowAutoInvoiceFeeItems(capId.serviceProviderCode, moduleName))
            {
                // If OnlyShowAutoInvoiceFeeItems is enabled and does not exists any auto-invoice fee items, also need skip the pay fee step.
                skipCapFeePage = !fees.Any(fee => ValidationUtil.IsYes(fee.f4FeeItemModel.autoInvoiceFlag));
            }

            return skipCapFeePage;
        }

        /// <summary>
        /// construct Section Labels
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>license verification section labels.</returns>
        public static Dictionary<string, string> GetLicenseVerificationSectionLabels(string moduleName)
        {
            Dictionary<string, string> sectionDic = new Dictionary<string, string>();
            sectionDic.Add(LicenseVerificationSectionType.RELATED_RECORDS.ToString(), LabelUtil.GetTextByKey("per_licensee_label_relatedpermit", moduleName));
            sectionDic.Add(LicenseVerificationSectionType.EDUCATION.ToString(), LabelUtil.GetTextByKey("per_detail_education_section_name", moduleName));
            sectionDic.Add(LicenseVerificationSectionType.CONTINUE_EDUCATION.ToString(), LabelUtil.GetTextByKey("continuing_education_capdetail_section_name", moduleName));
            sectionDic.Add(LicenseVerificationSectionType.EXAMINATION.ToString(), LabelUtil.GetTextByKey("examination_title", moduleName));
            sectionDic.Add(LicenseVerificationSectionType.PUBLIC_DOCUMENTS.ToString(), LabelUtil.GetTextByKey("attachment_title", moduleName));

            return sectionDic;
        }

        /// <summary>
        /// construct Section Labels
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>cap detail section labels.</returns>
        public static Dictionary<string, string> GetCapDetailSectionLabels(string moduleName)
        {
            string recordDetailPrefix = LabelUtil.GetTextByKey("per_permitDetail_label_detail", moduleName) + ACAConstant.SPLITLINE;

            Dictionary<string, string> sectionDic = new Dictionary<string, string>();
            sectionDic.Add(CapDetailSectionType.APPLICANT.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitDetail_label_applicant", moduleName));
            sectionDic.Add(CapDetailSectionType.WORKLOCATION.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitDetail_label_workLocation", moduleName));
            sectionDic.Add(CapDetailSectionType.LICENSED_PROFESSIONAL.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitDetail_label_license", moduleName));
            sectionDic.Add(CapDetailSectionType.PROJECT_DESCRIPTION.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitDetail_label_projectl", moduleName));
            sectionDic.Add(CapDetailSectionType.OWNER.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitdetail_label_owner", moduleName));
            sectionDic.Add(CapDetailSectionType.RELATED_CONTACTS.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitConfirm_label_relatedContacts", moduleName));
            sectionDic.Add(CapDetailSectionType.ADDITIONAL_INFORMATION.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitConfirm_label_description", moduleName));
            sectionDic.Add(CapDetailSectionType.APPLICATION_INFORMATION.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitConfirm_label_appSpecInfo", moduleName));
            sectionDic.Add(CapDetailSectionType.APPLICATION_INFORMATION_TABLE.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitConfirm_label_appSpecInfoTable", moduleName));
            sectionDic.Add(CapDetailSectionType.PARCEL_INFORMATION.ToString(), recordDetailPrefix + LabelUtil.GetTextByKey("per_permitConfirm_label_parcel", moduleName));
            sectionDic.Add(CapDetailSectionType.FEE.ToString(), LabelUtil.GetTextByKey("per_feeDetails_label_feeTitel", moduleName));
            sectionDic.Add(CapDetailSectionType.INSPECTIONS.ToString(), LabelUtil.GetTextByKey("ins_inspectionList_label_inspection", moduleName));
            sectionDic.Add(CapDetailSectionType.PROCESSING_STATUS.ToString(), LabelUtil.GetTextByKey("per_workStatus_label_proceeStatus", moduleName));
            sectionDic.Add(CapDetailSectionType.ATTACHMENTS.ToString(), LabelUtil.GetTextByKey("per_attachment_Label_attachTitle", moduleName));
            sectionDic.Add(CapDetailSectionType.RELATED_RECORDS.ToString(), LabelUtil.GetTextByKey("per_permitDetail_label_relatedPermit", moduleName));
            sectionDic.Add(CapDetailSectionType.EDUCATION.ToString(), LabelUtil.GetTextByKey("per_detail_education_section_name", moduleName));
            sectionDic.Add(CapDetailSectionType.CONTINUING_EDUCATION.ToString(), LabelUtil.GetTextByKey("continuing_education_capdetail_section_name", moduleName));
            sectionDic.Add(CapDetailSectionType.EXAMINATION.ToString(), LabelUtil.GetTextByKey("examination_title", moduleName));
            sectionDic.Add(CapDetailSectionType.VALUATION_CALCULATOR.ToString(), LabelUtil.GetTextByKey("valuationcalculator_title", moduleName));
            sectionDic.Add(CapDetailSectionType.TRUST_ACCOUNT.ToString(), LabelUtil.GetTextByKey("per_permitdetail_trustaccount_title", moduleName));
            sectionDic.Add(CapDetailSectionType.ASSETS.ToString(), LabelUtil.GetTextByKey("acaadmin_modulesetting_label_assettitle", moduleName));
            return sectionDic;
        }

        /// <summary>
        /// Validate list whether existing value is empty for required fields.
        /// </summary>
        /// <param name="htUserControls">the user controls</param>
        /// <param name="capModel">the cap model</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>True indicate existing value is not empty(validate success), false indicate some existing value is empty(validate failed).</returns>
        public static bool ValidateRequiredFields4List(Hashtable htUserControls, CapModel4WS capModel, string moduleName)
        {
            bool isValidateSuccess = true;

            string educationEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EDUCATION);
            EducationEdit educationEdit = (EducationEdit)htUserControls[educationEditKey];
            bool isEducationSectionEditable = educationEdit != null && educationEdit.IsEditable;

            string contEducationEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_CONT_EDUCATION);
            ContinuingEducationEdit contEducationEdit = (ContinuingEducationEdit)htUserControls[contEducationEditKey];
            bool isContEducationSectionEditable = contEducationEdit != null && contEducationEdit.IsEditable;

            string examinationEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EXAMINATION);
            ExaminationEdit examinationEdit = (ExaminationEdit)htUserControls[examinationEditKey];
            bool isExaminationSectionEditable = examinationEdit != null && examinationEdit.IsEditable;

            bool isContactValidateFailed = false;
            bool isLPListValidateFailed = false;

            foreach (string key in htUserControls.Keys)
            {
                if (key.StartsWith(PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX))
                {
                    MultiContactsEdit contactListEdit = (MultiContactsEdit)htUserControls[key];
                    bool isContactListSectionEditable = contactListEdit != null && contactListEdit.IsEditable;
                    bool isContactListSectionValidate = contactListEdit != null && contactListEdit.IsValidate;
                    bool isContactDataSourceNoLimitation = contactListEdit != null && ComponentDataSource.NoLimitation.Equals(contactListEdit.ValidateFlag, StringComparison.OrdinalIgnoreCase);
                    bool isEnableContactAddress = StandardChoiceUtil.IsEnableContactAddress();

                    CapContactModel4WS[] capContactsGroup = ContactUtil.FindContactListWithComponentName(capModel.contactsGroup, contactListEdit.ComponentName);

                    isContactValidateFailed =
                        isContactListSectionEditable
                            && ((IsNeedValidateContacts(isContactListSectionValidate, isContactListSectionEditable, capContactsGroup)
                            && (!RequiredValidationUtil.ValidateFields4ContactList(moduleName, GviewID.ContactEdit, capContactsGroup, contactListEdit.ContactEdit.ContactSectionPosition)
                                || !FormatValidationUtil.ValidateFormat4ContactList(moduleName, GviewID.ContactEdit, capContactsGroup, isContactDataSourceNoLimitation, contactListEdit.ContactEdit.ContactSectionPosition)
                                || (isEnableContactAddress && !RequiredValidationUtil.ValidateContactAddressType(capContactsGroup))))
                            || (isEnableContactAddress && !RequiredValidationUtil.ValidateCAPrimary(isContactListSectionEditable, contactListEdit.ContactEdit.ContactSectionPosition, capContactsGroup)));

                    if (isContactValidateFailed)
                    {
                        return false;
                    }
                }

                if (key.StartsWith(PageFlowConstant.SECTION_NAME_MULTI_LICENSE_PREFIX))
                {
                    MultiLicensesEdit lPListEdit = (MultiLicensesEdit)htUserControls[key];

                    //validate for license list.
                    isLPListValidateFailed = lPListEdit != null && !ValidateRequiredField4LPList(capModel, moduleName, lPListEdit);

                    if (isLPListValidateFailed)
                    {
                        return false;
                    }
                }
            }

            // validate required field's value whether is empty in list.
            GFilterScreenPermissionModel4WS permissionEdu = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_EDUCATOIN
            };

            GFilterScreenPermissionModel4WS permissionContEdu = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_CONTINUING_EDUCATION
            };

            GFilterScreenPermissionModel4WS permissionExamination = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_EXAMINATION
            };

            if ((isEducationSectionEditable && !RequiredValidationUtil.ValidateLicenseCertificationList(moduleName, permissionEdu, GviewID.EducationEdit, capModel.educationList))
                || (isContEducationSectionEditable && !RequiredValidationUtil.ValidateLicenseCertificationList(moduleName, permissionContEdu, GviewID.ContinuingEducationEdit, capModel.contEducationList))
                || (isExaminationSectionEditable && !RequiredValidationUtil.ValidateLicenseCertificationList(moduleName, permissionExamination, GviewID.ExaminationEdit, capModel.examinationList))
                || isLPListValidateFailed
                || isContactValidateFailed)
            {
                isValidateSuccess = false;
            }

            return isValidateSuccess;
        }

        /// <summary>
        /// Get the miss required document types
        /// </summary>
        /// <param name="requireDocumentTypes">require document types.</param>
        /// <param name="documentModels">The document model list.</param>
        /// <returns>Return the miss required document types.</returns>
        public static List<string> GetMissRequiredDocumentTypes(Dictionary<string, string> requireDocumentTypes, DocumentModel[] documentModels)
        {
            List<string> missDocumentTypes = new List<string>();
            if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ATTACHMENT))
            {
                return missDocumentTypes;
            }

            if (requireDocumentTypes != null && requireDocumentTypes.Count > 0)
            {
                List<string> documentTypes = new List<string>();

                if (documentModels != null)
                {
                    documentTypes.AddRange(documentModels.Select(documentModel => documentModel.docCategory).Distinct());
                }

                foreach (KeyValuePair<string, string> item in requireDocumentTypes)
                {
                    if (!documentTypes.Contains(item.Key))
                    {
                        missDocumentTypes.Add(item.Value);
                    }
                }
            }

            return missDocumentTypes;
        }

        /// <summary>
        /// Get required document type list.If not found return null.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>return required document types dictionary</returns>
        public static Dictionary<string, string> GetRequiredDocumentTypeList(CapTypeModel capType)
        {
            RefRequiredDocumentModel searchModel = new RefRequiredDocumentModel();

            // required type need hard code to 0
            searchModel.requiredType = 0;
            searchModel.group = capType.group;
            searchModel.type = capType.type;
            searchModel.subType = capType.subType;
            searchModel.category = capType.category;
            searchModel.serviceProviderCode = capType.serviceProviderCode;

            IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            RefRequiredDocumentModel[] requiredDocumentModels = edmsBll.GetRequiredDocumentList(searchModel);
            string[] requiredDocTypeLists = null;

            if (requiredDocumentModels != null && requiredDocumentModels.Length > 0)
            {
                //requiredType is 0; so the array size only one
                if (requiredDocumentModels[0] != null)
                {
                    string requiredDocTypeList = requiredDocumentModels[0].requiredDocTypeList;

                    if (!string.IsNullOrEmpty(requiredDocTypeList))
                    {
                        requiredDocTypeLists = requiredDocTypeList.Split(new[] { ACAConstant.SPLIT_CHAR7 }, 0);
                    }
                }
            }

            if (requiredDocTypeLists != null && requiredDocTypeLists.Length > 0)
            {
                requiredDocTypeLists = requiredDocTypeLists.Select(item => item.Split(new[] { ACAConstant.SPLIT_DOUBLE_COLON }, 0)[1]).ToArray();
                RefDocumentModel[] docTypes = edmsBll.GetAllDocumentTypes(ConfigManager.AgencyCode, capType);

                if (docTypes != null && docTypes.Length > 0)
                {
                    docTypes = docTypes.Where(document => ValidationUtil.IsYes(document.isRestrictDocType4ACA)).ToArray();
                }

                if (docTypes == null || docTypes.Length == 0)
                {
                    return null;
                }

                Dictionary<string, string> dictResult = new Dictionary<string, string>();

                foreach (RefDocumentModel docType in docTypes)
                {
                    if (requiredDocTypeLists.Contains(docType.documentType))
                    {
                        string resDocType = I18nStringUtil.GetCurrentLanguageString(docType.resDocumentType, docType.documentType);
                        resDocType = !string.IsNullOrEmpty(resDocType) ? resDocType : docType.documentType;
                        dictResult.Add(docType.documentType, resDocType);
                    }
                }

                return dictResult;
            }

            return null;
        }

        /// <summary>
        /// Get agency code from the caps users want to pay for
        /// </summary>
        /// <returns>array list.</returns>
        public static ArrayList GetAgenciesFromCaps()
        {
            ArrayList agencies = new ArrayList();
            CapIDModel4WS[] capIDs = AppSession.GetCapIDModelsFromSession();

            if (capIDs == null)
            {
                return agencies;
            }

            foreach (CapIDModel4WS capID in capIDs)
            {
                if (!agencies.Contains(capID.serviceProviderCode))
                {
                    agencies.Add(capID.serviceProviderCode);
                }
            }

            return agencies;
        }

        /// <summary>
        /// Gets a boolean value indicate whether the given section needs to display for the current user according
        /// to the given section permissions.
        /// Note:
        /// For compatibility with previous version, in this feature, the following sections:
        ///   1.Education, 
        ///   2.Continuing Education, 
        ///   3.Examination
        /// by default will be NOT visible for any users (except Admin), and the other sections will be visible for any users
        /// </summary>
        /// <param name="section">The section to determine whether to display </param>
        /// <param name="sectionPermissions">The section permissions used to determine the specified whether to display</param>
        /// <param name="moduleName">Module name.</param>
        /// <returns>true if the specified section need to display; otherwise, false</returns>
        /// <remarks>by default, all sections is visible for any user unless administrator explicitly configures permissions for sections</remarks>
        public static bool GetSectionVisibility(string section, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, string moduleName)
        {
            return GetSectionVisibility(section, sectionPermissions, AppSession.GetCapModelFromSession(moduleName));
        }

        /// <summary>
        /// Gets a boolean value indicate whether the given section needs to display for the current user according
        /// to the given section permissions.
        /// Note:
        /// For compatibility with previous version, in this feature, the following sections:
        ///   1.Education, 
        ///   2.Continuing Education, 
        ///   3.Examination
        /// by default will be NOT visible for any users (except Admin), and the other sections will be visible for any users
        /// </summary>
        /// <param name="section">The section to determine whether to display </param>
        /// <param name="sectionPermissions">The section permissions used to determine the specified whether to display</param>
        /// <param name="cap">cap model</param>
        /// <returns>true if the specified section need to display; otherwise, false</returns>
        /// <remarks>by default, all sections is visible for any user unless administrator explicitly configures permissions for sections</remarks>
        public static bool GetSectionVisibility(string section, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, CapModel4WS cap)
        {
            bool isSectionVisible = true;

            // The default situation that no section permission is set
            if (sectionPermissions == null || sectionPermissions.Count == 0)
            {
                isSectionVisible = GetDefaultVisibility(section);
            }
            else
            {
                UserRolePrivilegeModel findedPermission = null;
                if (sectionPermissions.ContainsKey(section))
                {
                    findedPermission = sectionPermissions[section];
                }

                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

                if (findedPermission != null)
                {
                    isSectionVisible = proxyUserRoleBll.HasReadOnlyPermission(cap, findedPermission);
                }
                else
                {
                    // The default situation that no section permission is set for the specified section
                    isSectionVisible = GetDefaultVisibility(section);
                }
            }

            return isSectionVisible;
        }

        /// <summary>
        /// Gets the section permissions for the specified module.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">The name of the module to get section permissions </param>
        /// <returns>returns all section permissions of this module. returns empty Dictionary instance if no section permissions</returns>
        public static Dictionary<string, UserRolePrivilegeModel> GetSectionPermissions(string agencyCode, string moduleName)
        {
            Dictionary<string, UserRolePrivilegeModel> sectionPermissions = new Dictionary<string, UserRolePrivilegeModel>();

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] xpolicys = xPolicyBll.GetPolicyListByCategory(
                agencyCode,
                BizDomainConstant.STD_ITEM_CAPDETAIL_SECTIONROLES,
                ACAConstant.LEVEL_TYPE_MODULE,
                moduleName);

            if (xpolicys == null)
            {
                return sectionPermissions;
            }

            // find general section permissions
            foreach (XPolicyModel xpolicy in xpolicys)
            {
                if (EntityType.GENERAL.ToString().Equals(xpolicy.data3, StringComparison.InvariantCultureIgnoreCase))
                {
                    if ((!string.IsNullOrEmpty(xpolicy.data2) && xpolicy.data2.Length < 5) ||
                        string.IsNullOrEmpty(xpolicy.data4))
                    {
                        throw new ACAException("Invalid section permissions");
                    }

                    if (sectionPermissions.ContainsKey(xpolicy.data4))
                    {
                        continue;
                    }

                    UserRolePrivilegeModel userRole = null;
                    if (!string.IsNullOrEmpty(xpolicy.data2))
                    {
                        var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                        userRole = userRoleBll.ConvertToUserRolePrivilegeModel(xpolicy.data2);

                        // setting the value to true to let lower level methods know that agency and agent clerk are not in RegisterUsers Group in current scenario.
                        userRole.AgentOrClerkNotInRegisteredUsers = true;
                    }

                    FillInLicenseTypesInfo(xpolicys, xpolicy.data4, userRole);
                    sectionPermissions.Add(xpolicy.data4, userRole);
                }
            }

            return sectionPermissions;
        }

        /// <summary>
        /// Get all sub cap according to the <see cref="asiGroups"/>.
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="asiGroups">ASI Groups.</param>
        /// <returns>sub cap model.</returns>
        public static IDictionary<string, CapModel4WS> GetSubCapsByASIGroups(string moduleName, IEnumerable<AppSpecificInfoGroupModel4WS> asiGroups)
        {
            IDictionary<string, CapModel4WS> result = new Dictionary<string, CapModel4WS>();

            if (!IsSuperCAP(moduleName) || asiGroups == null)
            {
                return result;
            }

            CapModel4WS partialCap = AppSession.GetCapModelFromSession(moduleName);
            ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();

            //appSpecInfoGroup need be sorted by agency. 
            foreach (AppSpecificInfoGroupModel4WS model in asiGroups)
            {
                //If a group has no fields
                if (model == null || model.fields == null)
                {
                    continue;
                }

                string key = model.capID.toKey();

                // some sub ASI group may belong to the same cap model.
                if (!result.ContainsKey(key))
                {
                    CapTypeModel capTypeModel = capTypeBll.GetCapTypeByCapID(model.capID);
                    CapModel4WS capModel = new CapModel4WS();
                    capModel.capID = model.capID;
                    capModel.capType = capTypeModel;
                    capModel.moduleName = capTypeModel.group;
                    capModel.appSpecificInfoGroups = new[] { model };

                    if (partialCap != null)
                    {
                        capModel.auditID = partialCap.auditID;
                    }

                    result.Add(key, capModel);
                }
            }

            return result;
        }

        /// <summary>
        /// Get all sub cap according to the <see cref="asitGroups"/>.
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="asitGroups">ASIT Groups.</param>
        /// <returns>sub cap model.</returns>
        public static IDictionary<string, CapModel4WS> GetSubCapsByASITGroups(string moduleName, IEnumerable<AppSpecificTableGroupModel4WS> asitGroups)
        {
            IDictionary<string, CapModel4WS> result = new Dictionary<string, CapModel4WS>();

            if (!IsSuperCAP(moduleName) || asitGroups == null)
            {
                return result;
            }

            var capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));

            foreach (AppSpecificTableGroupModel4WS group in asitGroups)
            {
                if (group == null)
                {
                    continue;
                }

                string key = group.capIDModel.toKey();

                // some sub ASI group may belong to the same cap model.
                if (!result.ContainsKey(key))
                {
                    CapTypeModel capTypeModel = capTypeBll.GetCapTypeByCapID(group.capIDModel);
                    CapModel4WS capModel = new CapModel4WS();
                    capModel.capID = group.capIDModel;
                    capModel.capType = capTypeModel;
                    capModel.moduleName = capTypeModel.group;
                    capModel.appSpecTableGroups = new[] { group };
                    CapModel4WS partialCap = AppSession.GetCapModelFromSession(moduleName);

                    if (partialCap != null)
                    {
                        capModel.auditID = partialCap.auditID;
                    }

                    result.Add(key, capModel);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="capClass"/> is partial CAP class.
        /// </summary>
        /// <param name="capClass">CAP class</param>
        /// <returns>true or false.</returns>
        public static bool IsPartialCap(string capClass)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            return capBll.IsPartialCap(capClass);
        }

        /// <summary>
        /// Gets the ASI group model list.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="currentComponent">The current component.</param>
        /// <param name="componentList">The component list.</param>
        /// <param name="pfGroupModel">The page flow group model.</param>
        /// <returns>Return the AppSpecificInfoGroupModel list.</returns>
        public static AppSpecificInfoGroupModel4WS[] GetASIGroupModelList(string moduleName, ComponentModel currentComponent, List<ComponentModel> componentList, PageFlowGroupModel pfGroupModel)
        {
            List<AppSpecificInfoGroupModel4WS> result = new List<AppSpecificInfoGroupModel4WS>();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            AppSpecificInfoGroupModel4WS[] totalModelList = capModel.appSpecificInfoGroups;

            if (totalModelList == null || totalModelList.Length == 0)
            {
                return null;
            }

            // Get ASI groups associated with current record type for ASI component which isn't assigned asi group
            if (string.IsNullOrEmpty(currentComponent.portletRange1))
            {
                List<string> assignedGroups = GetAssignedGroups(pfGroupModel);

                foreach (AppSpecificInfoGroupModel4WS asiGroupModel in totalModelList)
                {
                    //asiGroupModel.groupName is sub group name
                    string key = string.Format("{0}_{1}", currentComponent.componentID, asiGroupModel.groupName);

                    if (!assignedGroups.Contains(key))
                    {
                        result.Add(asiGroupModel);
                    }
                }
            }
            else
            {
                // Get ASI group and sub group from assigned.
                foreach (ComponentModel cptModel in componentList)
                {
                    bool groupEqual = cptModel.portletRange1 == currentComponent.portletRange1;
                    bool customHeadingEqual = cptModel.customHeading == currentComponent.customHeading;

                    if (groupEqual && customHeadingEqual)
                    {
                        AppSpecificInfoGroupModel4WS asiGroupModel = GetSingleASIGroupModel(cptModel.portletRange1, cptModel.portletRange2, totalModelList);

                        if (asiGroupModel != null)
                        {
                            result.Add(asiGroupModel);
                        }
                    }
                }
            }

            // Sometimes, tha value of capID is missing during the process of record renewal for some unknown reasons, if coming back from the shopping cart. As a result of that, from the end user point of view, the ASI data is missing.
            result.ForEach(asi =>
                {
                    if (asi != null && asi.capID == null)
                    {
                        asi.capID = capModel.capID;
                    }
                });

            return result.ToArray();
        }

        /// <summary>
        /// Gets the ASIT group model list.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="currentComponent">The current component.</param>
        /// <param name="componentList">The component list.</param>
        /// <param name="pfGroupModel">The page flow group model.</param>
        /// <returns>Return the AppSpecificTableGroupModel list.</returns>
        public static AppSpecificTableGroupModel4WS[] GetASITGroupModelList(string moduleName, ComponentModel currentComponent, List<ComponentModel> componentList, PageFlowGroupModel pfGroupModel)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            AppSpecificTableGroupModel4WS[] totalModelList = capModel.appSpecTableGroups;

            if (totalModelList == null || totalModelList.Length == 0)
            {
                return null;
            }

            List<AppSpecificTableGroupModel4WS> result = new List<AppSpecificTableGroupModel4WS>();
            string asitGroup = currentComponent.portletRange1;

            // Get ASI group and sub group from CAP type associate.
            if (string.IsNullOrEmpty(asitGroup))
            {
                List<string> assignedGroups = GetAssignedGroups(pfGroupModel);

                foreach (AppSpecificTableGroupModel4WS groupModel in totalModelList)
                {
                    List<AppSpecificTableModel4WS> tableList = new List<AppSpecificTableModel4WS>();

                    // loop add the non-assigned AppSpecificTableModel
                    if (groupModel != null && groupModel.tablesMapValues != null)
                    {
                        foreach (AppSpecificTableModel4WS model in groupModel.tablesMapValues)
                        {
                            // it only judge the subgroup, the same subgroup MUST NOT display.
                            string key = string.Format("{0}_{1}", currentComponent.componentID, model.tableName);

                            if (!assignedGroups.Contains(key))
                            {
                                tableList.Add(model);
                            }
                        }
                    }

                    if (tableList.Count > 0)
                    {
                        AppSpecificTableGroupModel4WS tempGroupModel = new AppSpecificTableGroupModel4WS();
                        tempGroupModel.capIDModel = groupModel.capIDModel;
                        tempGroupModel.groupName = groupModel.groupName;
                        tempGroupModel.instruction = groupModel.instruction;
                        tempGroupModel.resInstruction = groupModel.resInstruction;
                        tempGroupModel.tablesMap = groupModel.tablesMap;
                        tempGroupModel.tablesMapValues = tableList.ToArray();

                        result.Add(tempGroupModel);
                    }
                }
            }
            else
            {
                // Get ASIT group and sub group from assigned.
                foreach (ComponentModel cptModel in componentList)
                {
                    bool groupEqual = cptModel.portletRange1 == currentComponent.portletRange1;
                    bool customHeadingEqual = cptModel.customHeading == currentComponent.customHeading;

                    if (groupEqual && customHeadingEqual)
                    {
                        AppSpecificTableGroupModel4WS asitGroupModel = GetSingleASITGroupModel(cptModel.portletRange1, cptModel.portletRange2, totalModelList);

                        if (asitGroupModel != null)
                        {
                            result.Add(asitGroupModel);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///  Get CAP Type detail model according to module name and jump CAP type
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="capTypeString">cap type</param>
        /// <param name="agencyCode">Agency code - specified by selected server or default.</param>
        /// <returns>CAP type detail model</returns>
        public static CapTypeModel ConstructCAPTypeModel(string moduleName, string capTypeString, string agencyCode)
        {
            CapTypeModel capType = new CapTypeModel();
            CapTypeDetailModel capTypeDetail = new CapTypeDetailModel();
            string[] capTypeModleParameters = capTypeString.Split('/');
            try
            {
                if (capTypeModleParameters != null && capTypeModleParameters.Length == 4)
                {
                    capType.serviceProviderCode = string.IsNullOrEmpty(agencyCode)
                        ? ConfigManager.AgencyCode
                        : agencyCode;
                    capType.moduleName = moduleName;
                    capType.group = capTypeModleParameters[0];
                    capType.type = capTypeModleParameters[1];
                    capType.subType = capTypeModleParameters[2];
                    capType.category = capTypeModleParameters[3];
                }
                else
                {
                    capType.alias = capTypeString;
                }

                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                capTypeDetail = capTypeBll.GetCapTypeByPK(capType);
            }
            catch
            {
            }

            return capTypeDetail;
        }

        /// <summary>
        /// Build the Redirect URL about back to Spear Form page.
        /// </summary>
        /// <param name="searchComponentModel">the search component model</param>
        /// <param name="updatePanelName">update panel name for the section position</param>
        /// <param name="pageflowGroup">the page flow Group</param>
        /// <param name="rowIndex">the row item index which belong to a record.</param>
        /// <returns>The redirect url.</returns>
        public static string BuildRedirectUrl(ComponentModel searchComponentModel, string updatePanelName, PageFlowGroupModel pageflowGroup, string rowIndex)
        {
            string moduleName = HttpContext.Current.Request.QueryString["Module"];
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length == 0)
            {
                IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();

                pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);
                pageflowGroup = GetPageFlowWithoutBlankPage(capModel, pageflowGroup);
                AppSession.SetPageflowGroupToSession(pageflowGroup);
                return null;
            }
            
            int currentStep = 0;
            int currentPage = 0;
            ComponentModel cptModel = null;

            if (searchComponentModel != null)
            {
                // Get the step order and page order which indicates where appointed page and appointed section should be.
                cptModel = PageFlowUtil.FindComponent(pageflowGroup, searchComponentModel, out currentStep, out currentPage);
            }

            if (cptModel == null && !PageFlowUtil.IsPageFlowTraceUpdated)
            {
                return null;
            }

            string isRenewal = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[ACAConstant.REQUEST_PARMETER_ISRENEWAL]) ? string.Empty : HttpContext.Current.Request.QueryString[ACAConstant.REQUEST_PARMETER_ISRENEWAL];
            string isFromShoppingCart = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[ACAConstant.FROMSHOPPINGCART]) ? string.Empty : HttpContext.Current.Request.QueryString[ACAConstant.FROMSHOPPINGCART];
            int confirmStepNumber = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["stepNumber"]) ? 0 : int.Parse(HttpContext.Current.Request.QueryString["stepNumber"]);

            int stepNumber = 0;
            int pageNumber = 0;

            if (PageFlowUtil.IsPageFlowTraceUpdated)
            {
                stepNumber = 2;
                pageNumber = 1;
                currentStep = 0;
                currentPage = 0;
            }
            else
            {
                stepNumber = currentStep + (confirmStepNumber - pageflowGroup.stepList.Length);
                pageNumber = currentPage + 1;
            }

            string url = string.Format(
                                        "CapEdit.aspx?stepNumber={0}&pageNumber={1}&currentStep={2}&currentPage={3}&{8}={4}&Module={5}&isRenewal={6}&isFromShoppingCart={7}",
                                        stepNumber,
                                        pageNumber,
                                        currentStep,
                                        currentPage,
                                        rowIndex,
                                        moduleName,
                                        isRenewal,
                                        isFromShoppingCart,
                                        UrlConstant.ROW_INDEX);

            if (ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
            {
                url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
            }

            // Use IsSuperAgencyAssoForm to indicates current request is come from Super Agency Associated Forms.
            if (ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[UrlConstant.IS_SUPERAGENCY_ASSOFORM]))
            {
                url += ACAConstant.AMPERSAND + UrlConstant.IS_SUPERAGENCY_ASSOFORM + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
            }

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode]))
            {
                url += ACAConstant.AMPERSAND + UrlConstant.AgencyCode + ACAConstant.EQUAL_MARK + HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode];
            }

            string ssn = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ssn"]) ? string.Empty : HttpContext.Current.Request.QueryString["ssn"];

            if (!string.IsNullOrEmpty(ssn))
            {
                url += "&ssn=" + ssn;
            }

            /* If the Page Flow Trace is updated, the page will turn back to first Spear Form after click Edit button/Link in each section on Review Page and the Public User need 
             * load all visible Pages on Spear Form one by one, so below parameters in URL is useless under this environment.
             */
            if (!PageFlowUtil.IsPageFlowTraceUpdated)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]))
                {
                    url += "&" + UrlConstant.CONTACT_SEQ_NUMBER + "=" + HttpContext.Current.Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
                }

                // pass the flag that from the confirm page and the confirm step number.
                url += "&" + UrlConstant.IS_FROM_CONFIRMPAGE + "=Y&confirmStepNumber=" + confirmStepNumber + "&" + UrlConstant.SECTION_NAME + "=" + updatePanelName + "&goto=" + updatePanelName + "#" + updatePanelName;
            }

            FileUtil.AppendApplicationRoot(url);

            return url;
        }

        /// <summary>
        /// Redirect back to the spear form page which contain the section
        /// </summary>
        /// <param name="searchComponentModel">the search component model</param>
        /// <param name="updatePanelName">update panel name for the section position</param>
        /// <param name="pageflowGroup">the page flow Group</param>
        /// <param name="rowIndex">the row item index which belong to a record clicked in Cap confirm page</param>
        public static void BackToPageContainSection(ComponentModel searchComponentModel, string updatePanelName, PageFlowGroupModel pageflowGroup, string rowIndex)
        {
            string url = BuildRedirectUrl(searchComponentModel, updatePanelName, pageflowGroup, rowIndex);

            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            AppSession.IsEditFromConfirmFlag = true;

            HttpContext.Current.Response.Redirect(url);
        }

        /// <summary>
        /// Filter the blank page/step in page flow group.
        /// </summary>
        /// <param name="capModel">The CAP model.</param>
        /// <param name="pfGroupModel">The page flow group model.</param>
        /// <returns>Return the page flow group without blank page/step.</returns>
        public static PageFlowGroupModel GetPageFlowWithoutBlankPage(CapModel4WS capModel, PageFlowGroupModel pfGroupModel)
        {
            if (capModel == null || pfGroupModel == null)
            {
                return pfGroupModel;
            }

            List<StepModel> resultStepList = new List<StepModel>();
            int blankStepNumber = 0;

            foreach (StepModel stepModel in pfGroupModel.stepList)
            {
                bool isBlankPage = true;
                int blankPageNumber = 0;
                List<PageModel> resultPageList = new List<PageModel>();

                foreach (PageModel pageModel in stepModel.pageList)
                {
                    foreach (ComponentModel cptModel in pageModel.componentList)
                    {
                        // has non ASI/ASIT component, it is not a blank page
                        if (!GViewConstant.SECTION_ASI.Equals(cptModel.componentName, StringComparison.InvariantCultureIgnoreCase) &&
                            !GViewConstant.SECTION_ASIT.Equals(cptModel.componentName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            isBlankPage = false;
                            break;
                        }

                        // ASI component
                        if (GViewConstant.SECTION_ASI.Equals(cptModel.componentName, StringComparison.InvariantCultureIgnoreCase) &&
                            !IsASIEmpty(capModel, pfGroupModel, cptModel))
                        {
                            isBlankPage = false;
                            break;
                        }

                        // ASIT component
                        if (GViewConstant.SECTION_ASIT.Equals(cptModel.componentName, StringComparison.InvariantCultureIgnoreCase) &&
                            !IsASITEmpty(capModel, pfGroupModel, cptModel))
                        {
                            isBlankPage = false;
                            break;
                        }
                    }

                    // add the no blank page
                    if (!isBlankPage)
                    {
                        PageModel resultPage = ObjectCloneUtil.DeepCopy(pageModel);
                        resultPage.pageOrder = pageModel.pageOrder - blankPageNumber;
                        resultPageList.Add(resultPage);
                    }
                    else
                    {
                        blankPageNumber++;
                    }
                }

                // add the no blank step
                if (resultPageList.Count > 0)
                {
                    StepModel resultStep = ObjectCloneUtil.DeepCopy(stepModel);
                    resultStep.stepOrder = stepModel.stepOrder - blankStepNumber;
                    resultStep.pageList = resultPageList.ToArray();

                    resultStepList.Add(resultStep);
                }
                else
                {
                    blankStepNumber++;
                }
            }

            PageFlowGroupModel result = ObjectCloneUtil.DeepCopy(pfGroupModel);
            result.stepList = resultStepList.ToArray();

            return result;
        }

        /// <summary>
        /// Get Permit Types by Module Name
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <returns>Permit Types</returns>
        public static IList<ListItem> GetPermitTypesByModuleName(string moduleName)
        {
            IList<ListItem> lstPermitType = new List<ListItem>();

            if (!string.IsNullOrEmpty(moduleName))
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                CapTypeModel[] permitTypelist = capTypeBll.GetCapTypeList(ConfigManager.AgencyCode, moduleName, null);

                if (permitTypelist != null)
                {
                    foreach (CapTypeModel typemodel in permitTypelist)
                    {
                        ListItem item = new ListItem();

                        item.Text = CAPHelper.GetAliasOrCapTypeLabel(typemodel);
                        item.Value = CAPHelper.GetCapTypeValue(typemodel);

                        lstPermitType.Add(item);
                    }
                }
            }

            return lstPermitType;
        }

        /// <summary>
        ///  Get selected CAP Type
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="capType">Cap type value</param>
        /// <param name="capTypeAlias">Cap type alias</param>
        /// <returns>cap type model</returns>
        public static CapTypeModel GetCAPTypeModelByString(string moduleName, string capType, string capTypeAlias)
        {
            CapTypeModel capTypeModel = null;

            if (!string.IsNullOrEmpty(moduleName) && !string.IsNullOrEmpty(capType) && capType.Split('/').Length >= 4)
            {
                capTypeModel = new CapTypeModel();
                string[] capLevels = capType.Trim().Split('/');
                capTypeModel.resAlias = capTypeAlias;
                capTypeModel.moduleName = moduleName;
                capTypeModel.group = capLevels[0];
                capTypeModel.type = capLevels[1];
                capTypeModel.subType = capLevels[2];
                capTypeModel.category = capLevels[3];
            }

            return capTypeModel;
        }

        /// <summary>
        /// Indicate to validate cap contact.
        /// when validate is true and not reference contact need to validate.
        /// when validate is false need to validate.
        /// </summary>
        /// <param name="isReferenceContact">True means the contact data must be reference contact.</param>
        /// <param name="isEditable">is editable.</param>
        /// <param name="capContacts">cap contact model</param>
        /// <returns>is need to validate</returns>
        public static bool IsNeedValidateContacts(bool isReferenceContact, bool isEditable, CapContactModel4WS[] capContacts)
        {
            bool hasInvalidData = false;

            if (isReferenceContact && capContacts != null)
            {
                /*
                 * 1. Contact data must come from reference side when "isValidate" is true.
                 * 2. Contact type must validate when the contact come from Amendment.
                 */
                foreach (CapContactModel4WS contact in capContacts)
                {
                    if (string.IsNullOrEmpty(contact.refContactNumber)
                        || (contact.people != null && string.IsNullOrEmpty(contact.people.contactType)))
                    {
                        hasInvalidData = true;
                        break;
                    }
                }
            }
            
            return !isReferenceContact || hasInvalidData || !ValidateTemplateFields(isReferenceContact, isEditable, capContacts);
        }

        /// <summary>
        /// Indicate to validate template fields cap contact.
        /// </summary>
        /// <param name="isReferenceContact">if reference contact</param>
        /// <param name="isEditable">is editable</param>
        /// <param name="capContacts">cap contact model</param>
        /// <returns>succeeded return true</returns>
        public static bool ValidateTemplateFields(bool isReferenceContact, bool isEditable, CapContactModel4WS[] capContacts)
        {
            if (capContacts == null)
            {
                return true;
            }

            bool contactNeedValidate = isEditable && !isReferenceContact;

            foreach (CapContactModel4WS contact in capContacts)
            {
                TemplateAttributeModel[] requiredFailedFields = null;

                if (contact != null && contact.people != null && contact.people.attributes != null)
                { 
                    requiredFailedFields = contact.people.attributes.Where(
                        attribute => !ValidationUtil.IsNo(attribute.vchFlag)
                        && ValidationUtil.IsYes(attribute.attributeValueReqFlag) 
                        && string.IsNullOrEmpty(attribute.attributeValue)).ToArray();
                }

                if (requiredFailedFields == null || requiredFailedFields.Length == 0)
                {
                    continue;
                }

                foreach (TemplateAttributeModel field in requiredFailedFields)
                {
                    if (contactNeedValidate || (!contactNeedValidate && field.vchFlag == ACAConstant.TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Get associated form type.
        /// </summary>
        /// <param name="isAssociatedForm">If the associated form enabled.</param>
        /// <param name="parentId">The parent id model.</param>
        /// <returns>Return the associated form type.</returns>
        public static ACAConstant.AssoFormType GetAssoFormType(bool isAssociatedForm, CapIDModel4WS parentId)
        {
            if (!isAssociatedForm)
            {
                return ACAConstant.AssoFormType.NotAssoForm;
            }

            return IsSuperCAP(parentId)
                       ? ACAConstant.AssoFormType.SuperAgency
                       : ACAConstant.AssoFormType.Normal;
        }

        /// <summary>
        /// Initial renewal button.
        /// </summary>
        /// <param name="control">button control</param>
        /// <param name="row">DataRow object</param>
        /// <returns>Initial success or not</returns>
        public static bool InitRenewalButton(WebControl control, DataRow row)
        {
            // set renewal status button and label status
            var success = false;
            var filterName = row["filterName"].ToString();
            var moduleName = row["ModuleName"].ToString();
            var renewalStatus = row["RenewalStatus"].ToString();
            var capIDModel = new CapIDModel4WS
            {
                id1 = row["capID1"].ToString(),
                id2 = row["capID2"].ToString(),
                id3 = row["capID3"].ToString(),
                serviceProviderCode = row["AgencyCode"].ToString()
            };

            if (!string.IsNullOrEmpty(renewalStatus) && FunctionTable.IsEnableRenewRecord())
            {
                SetRenewalStatusButton(control, capIDModel, renewalStatus, filterName, moduleName);
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Initial renewal detail button.
        /// </summary>
        /// <param name="lblControl">label control</param>
        /// <param name="lnkControl">link control</param>
        /// <param name="row">DataRow object</param>
        /// <returns>Initial success or not</returns>
        public static bool InitRenewalDetailButton(WebControl lblControl, HtmlControl lnkControl, DataRow row)
        {
            // set renewal status button and label status
            var success = false;
            var moduleName = row["ModuleName"].ToString();
            var renewalStatus = row["RenewalStatus"].ToString();
            var lblRenewalDetail = lblControl as AccelaLabel;
            var lnkRenewalDetail = lnkControl as HtmlAnchor;

            var capIDModel = new CapIDModel4WS
            {
                id1 = row["capID1"].ToString(),
                id2 = row["capID2"].ToString(),
                id3 = row["capID3"].ToString(),
                serviceProviderCode = row["AgencyCode"].ToString()
            };

            if (ACAConstant.RENEWAL_REVIEW.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                success = true;
                string url = string.Format(
                   "Cap/CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}&{6}={7}&{8}={9}&{10}={11}",
                   ScriptFilter.AntiXssUrlEncode(moduleName),
                   capIDModel.id1,
                   capIDModel.id2,
                   capIDModel.id3,
                   UrlConstant.AgencyCode,
                   HttpContext.Current.Server.UrlEncode(capIDModel.serviceProviderCode),
                   ACAConstant.IS_TO_SHOW_INSPECTION,
                   HttpContext.Current.Request.QueryString[ACAConstant.IS_TO_SHOW_INSPECTION],
                   UrlConstant.IS_FOR_RENEW,
                   ACAConstant.COMMON_Y,
                   UrlConstant.IS_POPUP_PAGE,
                   ACAConstant.COMMON_Y);

                lblRenewalDetail.Visible = true;
                lblRenewalDetail.LabelKey = "per_permitList_label_renewalReviewing";
                
                lnkRenewalDetail.Visible = true;
                lnkRenewalDetail.HRef = "javascript:window.ACADialog.popup({ url: '" + FileUtil.AppendApplicationRoot(url) + "', width: 798, height: 750 });";
            }

            return success;
        }

        /// <summary>
        /// Initial trade license button.
        /// </summary>
        /// <param name="control">button control</param>
        /// <param name="row">DataRow object</param>
        /// <returns>Initial success or not</returns>
        public static bool InitTradeLicenseButton(WebControl control, DataRow row)
        {
            // set request trade license button
            var success = false;
            var isTnExpired = (bool)row["isTNExpired"];
            var appStatusGroup = row["AppStatusGroup"] as string;
            var appStatus = row["AppStatus"] as string;
            var agencyCode = row["AgencyCode"].ToString();
            var hasPrivilegeToHandleCap = (bool)row["hasPrivilegeToHandleCap"];
            var moduleName = row["ModuleName"].ToString();
            var filterName = row["filterName"].ToString();
            var btnRequestTradeLic = (AccelaLinkButton)control;
            var capIDModel = new CapIDModel4WS
            {
                id1 = row["capID1"].ToString(),
                id2 = row["capID2"].ToString(),
                id3 = row["capID3"].ToString(),
                serviceProviderCode = row["AgencyCode"].ToString()
            };

            if (!AppSession.User.IsAnonymous && hasPrivilegeToHandleCap
                && ACAConstant.REQUEST_PARMETER_TRADE_NAME.Equals(filterName, StringComparison.InvariantCultureIgnoreCase)
                && !isTnExpired
                && LicenseUtil.IsDisplayRequestTradeLicenseLink(agencyCode, moduleName, appStatusGroup, appStatus))
            {
                var licenseType = row["licenseType"].ToString();

                btnRequestTradeLic.Visible = true;
                btnRequestTradeLic.CommandArgument = GetRowButtonCommandArgument(capIDModel, ACAConstant.REQUEST_TRADE_LICENSE, licenseType);
                btnRequestTradeLic.ModuleName = moduleName;
                btnRequestTradeLic.LabelKey = "per_tradeName_msg_requestTradeLicense";
                success = true;
            }
            else
            {
                btnRequestTradeLic.Visible = false;
            }

            return success;
        }

        /// <summary>
        /// Get action command url.
        /// </summary>
        /// <param name="control">current page object</param>
        /// <param name="commandArgument">command Argument</param>
        /// <returns>url string</returns>
        public static string GetActionCommandUrl(System.Web.UI.Control control, object commandArgument)
        {
            string[] parameters = commandArgument.ToString().Split(ACAConstant.COMMA_CHAR);
            string id1 = parameters[0];
            string id2 = parameters[1];
            string id3 = parameters[2];
            string actionFlag = parameters.Length > 3 ? parameters[3] : string.Empty;
            string agencyCode = parameters.Length > 4 ? parameters[4] : ConfigManager.AgencyCode;
            string customID = parameters.Length > 5 ? parameters[5] : string.Empty;
            var capIdModel = new CapIDModel4WS { id1 = id1, id2 = id2, id3 = id3, serviceProviderCode = agencyCode };

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            CapModel4WS capModel = null;
            string url = string.Empty;
            bool isPayFeeUrl = false;

            if (ACAConstant.CAP_PAYFEEDUE.Equals(actionFlag, StringComparison.InvariantCulture))
            {
                capModel = capBll.GetCapByPK(capIdModel);
                
                if (capModel == null)
                {
                    return string.Empty;
                }

                url = string.Format("~/Cap/CapFees.aspx?permitType=PayFees&Module={0}&stepNumber=0&isPay4ExistingCap={1}", capModel.moduleName, ACAConstant.COMMON_Y);
                isPayFeeUrl = true;
            }
            else 
            {
                bool isSuperCAP = IsSuperCAP(capIdModel);

                CapWithConditionModel4WS capWithConditionModel = capBll.GetCapViewBySingle(capIdModel, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, isSuperCAP);
                capModel = capWithConditionModel.capModel;

                if (capModel == null)
                {
                    return string.Empty;
                }

                /* If the current section exists the page trace for the partial record, needthe Reset the Page Trace for the partial record of the current section in Record List.
                 * Otherwise, it will get the using page trace in the session and can't get the latest page trace if the page flow update(Add/Remove pages).
                 */
                PageFlowUtil.ResetPageTrace(capModel);

                //check current user's permission
                if (!capModel.hasPrivilegeToHandleCap)
                {
                    string msg = LabelUtil.GetTextByKey("per_permitList_error_handle", capModel.moduleName);
                    MessageUtil.ShowMessageByControl(control, MessageType.Error, msg);
                    return string.Empty;
                }

                // validate page flow
                if ((!ACAConstant.AMENDMENT.Equals(actionFlag, StringComparison.InvariantCultureIgnoreCase))
                    && !ACAConstant.CAP_RENEWAL.Equals(actionFlag, StringComparison.InvariantCulture)
                    && !PageFlowUtil.HasAssociatedPageFlowGroup(capModel))
                {
                    string msg = LabelUtil.GetTextByKey("per_permitList_error_noRelatedPageflowGroup", capModel.moduleName);
                    MessageUtil.ShowMessageByControl(control, MessageType.Error, msg);
                    return string.Empty;
                }
                else if (ACAConstant.PAYFEEDUE_RENEWAL.Equals(actionFlag, StringComparison.InvariantCultureIgnoreCase))
                {
                    //1. Get child cap id by params cap id.
                    CapIDModel4WS childCapID = capBll.GetChildCapIDByMasterID(capIdModel, ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE, false);

                    if (childCapID == null)
                    {
                        return string.Empty;
                    }

                    //2. Get CapModel by child cap ID. Currently, renewnal logic does not supports super agency.
                    capWithConditionModel = capBll.GetCapViewBySingle(childCapID, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);

                    if (capWithConditionModel == null || capWithConditionModel.capModel == null)
                    {
                        return string.Empty;
                    }

                    capModel = capWithConditionModel.capModel;

                    //3. Build URL for renewal to pay fee due.
                    url = string.Format("~/Cap/CapFees.aspx?permitType=PayFees&Module={0}&stepNumber=0&isPay4ExistingCap=Y&isRenewal=Y", capModel.moduleName);
                    isPayFeeUrl = true;
                }
                else if (ACAConstant.CAP_RENEWAL.Equals(actionFlag, StringComparison.InvariantCulture))
                {
                    //for renewal licenses and permit process
                    if (!EmseUtil.IsConfigEventScript(agencyCode, ACAConstant.EMSE_APPLICATION_SUBMIT_AFTER))
                    {
                        string msg = LabelUtil.GetTextByKey("per_permitList_renewal_noemseconfiguration", capModel.moduleName);
                        MessageUtil.ShowMessageByControl(control, MessageType.Error, msg);
                        return string.Empty;
                    }

                    if (!capBll.IsValidConfig4Renewal(capIdModel))
                    {
                        string msg = LabelUtil.GetTextByKey("per_permitList_renewal_configuration_error", capModel.moduleName);
                        MessageUtil.ShowMessageByControl(control, MessageType.Error, msg);
                        return string.Empty;
                    }

                    capModel = capBll.CreateOrGetRenewalPartialCap(capIdModel);

                    //Validate the Page Flow of Child Record Type.
                    if (!PageFlowUtil.HasAssociatedPageFlowGroup(capModel))
                    {
                        string msg = LabelUtil.GetTextByKey("per_permitList_error_noRelatedPageflowGroup", capModel.moduleName);
                        MessageUtil.ShowMessageByControl(control, MessageType.Error, msg);
                        return string.Empty;
                    }

                    if (ACAConstant.PAYMENT_STATUS_PAID.Equals(capModel.paymentStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CapIDModel tempCapIDModel = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);

                        // complete the renew record that have paid
                        url = CompletePaidRecordAndReturnUrl(agencyCode, capModel.moduleName, tempCapIDModel, ACAConstant.COMMON_Y, control);

                        if (string.IsNullOrEmpty(url))
                        {
                            return string.Empty;
                        }
                    }
                    else if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.Etisalat && ACAConstant.CAP_STATUS_PENDING.Equals(capModel.capClass, StringComparison.InvariantCultureIgnoreCase))
                    {
                        actionFlag = EtisalatHelper.ACTIONSOURCE_RENEWLICENSE;
                        agencyCode = capModel.capID.serviceProviderCode;
                        string isPay4ExistingCapFlag = ACAConstant.COMMON_N;
                        string isRenewalFlag = ACAConstant.COMMON_Y;
                        url = EtisalatAdapter.CheckPayment(capModel.moduleName, capModel.capID.id1, capModel.capID.id2, capModel.capID.id3, capModel.capID.customID, actionFlag, agencyCode, isPay4ExistingCapFlag, isRenewalFlag);

                        if (string.IsNullOrEmpty(url))
                        {
                            string msg = LabelUtil.GetTextByKey("ACA_Pageflow_SaveError_Message", capModel.moduleName);
                            MessageUtil.ShowMessageByControl(control, MessageType.Error, msg);
                            return string.Empty;
                        }
                    }
                    else
                    {
                        string filterName = parameters.Length > 5 ? parameters[5] : string.Empty;
                        string isSubAgencyCap = StandardChoiceUtil.IsSuperAgency() ? "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y : string.Empty;

                    url = string.Format(
                        "~/Cap/CapEdit.aspx?permitType=renewal&Module={0}&stepNumber=2&pageNumber=1&isFeeEstimator={1}&isRenewal=Y&FilterName={2}{3}",
                        capModel.moduleName,
                        ACAConstant.COMMON_N,
                        filterName,
                        isSubAgencyCap);
                }
            }
            else if (ACAConstant.AMENDMENT.Equals(actionFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                url = CreateUrlForAmendment(capModel);
            }
            else if (ACAConstant.REQUEST_TRADE_LICENSE.Equals(actionFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                url = string.Format("~/Cap/CapType.aspx?Module={0}&stepNumber=1&licenseNumber={1}&licenseType={2}&FilterName={3}", capModel.moduleName, capModel.altID, parameters[5], ACAConstant.REQUEST_PARMETER_TRADE_LICENSE);
            }
            else if (ACAConstant.CAP_STATUS_PENDING.Equals(actionFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                // PAYING status only used in Etisalat payment.
                string isPay4ExistingCapFlag = ACAConstant.COMMON_N;
                string isRenewalFlag = ACAConstant.COMMON_N;
                url = EtisalatAdapter.CheckPayment(capModel.moduleName, id1, id2, id3, customID, actionFlag, agencyCode, isPay4ExistingCapFlag, isRenewalFlag);

                    //if url is empty, show error message.
                    if (string.IsNullOrEmpty(url))
                    {
                        string msg = LabelUtil.GetTextByKey("ACA_Pageflow_SaveError_Message", capModel.moduleName);
                        MessageUtil.ShowMessageByControl(control, MessageType.Error, msg);
                        return string.Empty;
                    }
                }
                else if (ACAConstant.ACTION_COMPLETE_PAID.Equals(actionFlag, StringComparison.InvariantCultureIgnoreCase))
                {
                    // resume the record that have paid
                    CapIDModel tempCapIDModel = TempModelConvert.Trim4WSOfCapIDModel(capIdModel);
                    url = CompletePaidRecordAndReturnUrl(agencyCode, capModel.moduleName, tempCapIDModel, ACAConstant.COMMON_N, control);

                    if (string.IsNullOrEmpty(url))
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    //for resume cap process
                    url = CreateUrlForResume(capModel);

                    //If the cap don't have clild cap, it is subagency cap, pass "isSubAgencyCap" to url, and use subagency record create logic.
                    if (StandardChoiceUtil.IsSuperAgency() && !isSuperCAP)
                    {
                        url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
                    }
                }
            }

            //Check if the exam fee can not be paied, then return error message and stop redirect to payment page.
            if (isPayFeeUrl)
            {
                string inavailableExams = GetInavailableExamsWithNoPaidExamFee(capIdModel);

                if (!string.IsNullOrEmpty(inavailableExams))
                {
                    string msg = LabelUtil.GetTextByKey("aca_exam_msg_inavailable2payfee", capModel.moduleName);
                    MessageUtil.ShowMessageByControl(control, MessageType.Error, string.Format(msg, inavailableExams));
                    return string.Empty;
                }
            }

            FillCapModelTemplateValue(capModel);
            CapTypeModel capType = capModel.capType;
            SetCapInfoToAppSession(capModel, capType, capModel.moduleName);

            //In SuperAgency environment, always use SuperAgency's receipt page.
            if (!ACAConstant.ACTION_COMPLETE_PAID.Equals(actionFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                url += "&" + UrlConstant.AgencyCode + "=" + capIdModel.serviceProviderCode;
            }

            return url;
        }

        /// <summary>
        /// Update the data for the CapModel in Cart List page under Multiple Agencies environment.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        public static void AdjustCapModelForShoppingCart(CapModel4WS capModel)
        {
            if (capModel == null)
            {
                return;
            }

            ContactUtil.InitializeContactsGroup4CapModel(capModel);

            PageFlowGroupModel pageFlow = PageFlowUtil.GetAssociatedPageFlowGroup(capModel);
            PageFlowGroupModel parentPageFlow = PageFlowUtil.GetAssociatedParentPageFlowGroup(capModel);

            if (parentPageFlow != null
                && !(parentPageFlow.pageFlowGrpCode.Equals(pageFlow.pageFlowGrpCode, StringComparison.InvariantCultureIgnoreCase)
                     && parentPageFlow.serviceProviderCode.Equals(pageFlow.serviceProviderCode, StringComparison.InvariantCultureIgnoreCase)))
            {
                Dictionary<string, string> componentNameMapping = PageFlowUtil.CheckTheSamePageFlow(parentPageFlow, pageFlow);

                if (componentNameMapping != null && componentNameMapping.Count > 0)
                {
                    //If the update the component name for contact/LP record if ParentPageFlow is the same as the ChildPageFlow.
                    PageFlowUtil.UpdateExistingComponentNameForCapModel(capModel, componentNameMapping);
                }
            }
        }

        /// <summary>
        /// Get the Agency Logo Html.
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>The agency logo's html.</returns>
        public static string GetAgencyLogoHtml(string agencyCode, string moduleName)
        {
            ILogoBll logo = ObjectFactory.GetObject<ILogoBll>();
            LogoModel logoModel = logo.GetAgencyLogo(agencyCode);
            string agencyLogoAlt = LabelUtil.GetTextByKey("aca_common_msg_imgalt_agencylogo", moduleName);

            if (logoModel != null)
            {
                if (!string.IsNullOrEmpty(logoModel.docDesc))
                {
                    agencyLogoAlt = logoModel.docDesc;
                }

                return string.Format(
                    "<div class=\"ACA_Logo\"><img id=\"imgAgencyLogo\" alt='{0}' src=\"{1}?{2}={3}\" /></div>",
                    agencyLogoAlt,
                    FileUtil.AppendApplicationRoot("Cap/ImageHandler.aspx"),
                    UrlConstant.AgencyCode,
                    logoModel.serviceProviderCode);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the in-available exam with no paid fee item on it
        /// </summary>
        /// <param name="capID">The cap id model</param>
        /// <returns>The in-available exam sessions</returns>
        public static string GetInavailableExamsWithNoPaidExamFee(CapIDModel4WS capID)
        {
            var examBll = ObjectFactory.GetObject<IExaminationBll>();
            ExaminationModel[] exams = examBll.GetInavailableExamListByCapIds(new[] { capID });

            if (exams == null || exams.Length == 0)
            {
                return null;
            }

            var feeBll = ObjectFactory.GetObject<IFeeBll>();
            F4FeeItemModel4WS[] feeItems = feeBll.GetNoPaidExamFeeItemsByExams(exams);

            if (feeItems != null && feeItems.Length > 0)
            {
                StringBuilder examInfo = new StringBuilder();

                foreach (ExaminationModel exam in exams)
                {
                    examInfo.Append(exam.examName);

                    if (!string.IsNullOrEmpty(exam.providerName))
                    {
                        examInfo.AppendFormat("({0})", exam.providerName);
                    }

                    examInfo.Append(", ");
                }

                return examInfo.ToString().Substring(0, examInfo.Length - 2);
            }

            return null;
        }

        /// <summary>
        /// Get section name for ASI
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="asiGroupModelList">The AppSpecificInfo Group Model list.</param>
        /// <returns>section name.</returns>
        public static string GetSectionName4ASI(ComponentModel component, AppSpecificInfoGroupModel4WS[] asiGroupModelList)
        {
            // To generate one fixed id for custom script, the hash code value of the combination(agency code + ASIT group + ASI Table Name) would be used.
            string sectionNameSuffix = string.Empty;

            if (asiGroupModelList != null
                && asiGroupModelList.Length >= 1
                && asiGroupModelList[0].fields != null
                && asiGroupModelList[0].fields.Length > 0)
            {
                // If ASIT Component does contain only one ASI Table by configuration in ACA Admin, the ID comes from the combination of agency code + ASIT Group name + ASIT table name
                if (string.IsNullOrEmpty(component.portletRange1))
                {
                    sectionNameSuffix = asiGroupModelList[0].fields[0].serviceProviderCode + ACAConstant.SPLIT_CHAR + asiGroupModelList[0].groupCode;
                }
                else
                {
                    sectionNameSuffix = string.Format(
                        "{1}{0}{2}{0}{3}",
                        ACAConstant.SPLIT_CHAR,
                        asiGroupModelList[0].fields[0].serviceProviderCode,
                        asiGroupModelList[0].fields[0].groupCode,
                        asiGroupModelList[0].fields[0].checkboxType);
                }
            }

            string sectionName = PageFlowConstant.SECTION_NAME_ASI + sectionNameSuffix.GetHashCode().ToString("X2");
            return sectionName;
        }

        /// <summary>
        /// Get section name for ASIT
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="asitGroupModelList">The asit group model list.</param>
        /// <returns>section name.</returns>
        public static string GetSectionName4ASIT(ComponentModel component, AppSpecificTableGroupModel4WS[] asitGroupModelList)
        {
            // To generate one fixed id for custom script, the hash code value of the combination(agency code + ASIT group + ASI Table Name) would be used.
            string sectionNameSuffix = string.Empty;

            if (asitGroupModelList != null
                && asitGroupModelList[0].tablesMap != null
                && asitGroupModelList[0].tablesMap.Length >= 1
                && asitGroupModelList[0].tablesMapValues[0].columns != null)
            {
                // If ASIT Component does contain only one ASI Table by configuration in ACA Admin, the ID comes from the combination of agency code + ASIT Group name + ASIT table name
                if (string.IsNullOrEmpty(component.portletRange1))
                {
                    sectionNameSuffix = asitGroupModelList[0].tablesMapValues[0].columns[0].servProvCode
                        + ACAConstant.SPLIT_CHAR + asitGroupModelList[0].tablesMapValues[0].columns[0].groupName;
                }
                else
                {
                    sectionNameSuffix = asitGroupModelList[0].tablesMapValues[0].columns[0].servProvCode
                        + ACAConstant.SPLIT_CHAR + asitGroupModelList[0].tablesMapValues[0].columns[0].groupName
                        + ACAConstant.SPLIT_CHAR + asitGroupModelList[0].tablesMapValues[0].columns[0].tableName;
                }
            }

            string sectionName = PageFlowConstant.SECTION_NAME_ASIT + sectionNameSuffix.GetHashCode().ToString("X2");
            return sectionName;
        }

        /// <summary>
        /// create url for resume application
        /// </summary>
        /// <param name="capModel">Cap model for ACA.</param>
        /// <returns>string for URL.</returns>
        private static string CreateUrlForResume(CapModel4WS capModel)
        {
            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            bool isTradeLicenseCap = capTypeBll.IsMatchTheFilter(capModel.capType, capModel.moduleName, ACAConstant.REQUEST_PARMETER_TRADE_LICENSE);

            string requestStr = "~/Cap/CapEdit.aspx?permitType=resume&Module={0}&stepNumber=2&pageNumber=1&isFeeEstimator={1}";

            if (isTradeLicenseCap)
            {
                requestStr = requestStr + "&FilterName=" + ACAConstant.REQUEST_PARMETER_TRADE_LICENSE;
            }
            else
            {
                bool isTradeNameCap = capTypeBll.IsMatchTheFilter(capModel.capType, capModel.moduleName, ACAConstant.REQUEST_PARMETER_TRADE_NAME);

                if (isTradeNameCap)
                {
                    requestStr = requestStr + "&FilterName=" + ACAConstant.REQUEST_PARMETER_TRADE_NAME;
                }
            }

            return string.Format(requestStr, capModel.moduleName, capModel.capClass == ACAConstant.INCOMPLETE ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
        }

        /// <summary>
        /// Complete the paid record and return the redirect finish url.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="capId">The CAP id model.</param>
        /// <param name="renewParameter">The renew parameter, the value is Y/N.</param>
        /// <param name="control">The control</param>
        /// <returns>The redirect finish url.</returns>
        private static string CompletePaidRecordAndReturnUrl(string agencyCode, string moduleName, CapIDModel capId, string renewParameter, System.Web.UI.Control control)
        {
            // complete the renew record that have paid
            bool completeSuccess = CompletePaidRecord(agencyCode, moduleName, capId, control);

            if (!completeSuccess)
            {
                return string.Empty;
            }

            string url = StandardChoiceUtil.IsEnableShoppingCart() || StandardChoiceUtil.IsSuperAgency()
                ? string.Format("~/Cap/CapCompletions.aspx?Module={0}&{1}={2}", moduleName, UrlConstant.BREADCRUMB_HIDDEN_NAVIGATE, ACAConstant.COMMON_Y)
                : string.Format("~/Cap/CapCompletion.aspx?Module={0}&isRenewal={1}&{2}={3}", moduleName, renewParameter, UrlConstant.BREADCRUMB_HIDDEN_NAVIGATE, ACAConstant.COMMON_Y);

            return url;
        }

        /// <summary>
        /// Complete the paid record.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="control">web control</param>
        /// <returns>Return true or false to indicates that the operation is success or not.</returns>
        private static bool CompletePaidRecord(string agencyCode, string moduleName, CapIDModel capId, System.Web.UI.Control control)
        {
            OnlinePaymentResultModel paymentResult;
            string msg4CompleteFail = LabelUtil.GetTextByKey("aca_caplist_msg_completepaidrecordfail", moduleName);

            try
            {
                ICommonPaymentBll commonPaymentBll = ObjectFactory.GetObject<ICommonPaymentBll>();
                paymentResult = commonPaymentBll.ResumeOnlinePayment(agencyCode, capId, AppSession.User.PublicUserId);

                // store the payment result to session because it will reuse in receipt page.
                AppSession.SetOnlinePaymentResultModelToSession(paymentResult);
            }
            catch (Exception ex)
            {
                Logger.Error("Error occurred during completing paid record.", ex);

                MessageUtil.ShowMessageByControl(control, MessageType.Error, msg4CompleteFail);
                return false;
            }

            // show error message when resume failed.
            if (paymentResult != null && paymentResult.capPaymentResultModels != null && paymentResult.capPaymentResultModels.Length > 0)
            {
                foreach (CapPaymentResultModel result in paymentResult.capPaymentResultModels)
                {
                    if (result == null || !result.paymentStatus)
                    {
                        MessageUtil.ShowMessageByControl(control, MessageType.Error, msg4CompleteFail);
                        return false;
                    }
                }
            }
            else
            {
                MessageUtil.ShowMessageByControl(control, MessageType.Error, msg4CompleteFail);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the renewal status button.
        /// </summary>
        /// <param name="btnRenewalStatus">The linkButton <c>btnRenewalStatus</c>.</param>
        /// <param name="capIDModel">The CapID model.</param>
        /// <param name="renewalStatus">The renewal status.</param>
        /// <param name="filterName">The filter name.</param>
        /// <param name="moduleName">The module name.</param>
        private static void SetRenewalStatusButton(WebControl btnRenewalStatus, CapIDModel4WS capIDModel, string renewalStatus, string filterName, string moduleName)
        {
            var btnRenewal = btnRenewalStatus as AccelaLinkButton;

            if (ACAConstant.RENEWAL_INCOMPLETE.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                var labelKey = GetLabelKeyByFilterName(filterName);

                btnRenewal.Visible = true;
                btnRenewal.CommandArgument = GetRowButtonCommandArgument(capIDModel, ACAConstant.CAP_RENEWAL, filterName);
                btnRenewal.ModuleName = moduleName;
                btnRenewal.LabelKey = labelKey;
            }
            else if (ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase) &&
                        !StandardChoiceUtil.IsRemovePayFee(capIDModel.serviceProviderCode))
            {
                btnRenewal.Visible = true;
                btnRenewal.CommandArgument = GetRowButtonCommandArgument(capIDModel, ACAConstant.PAYFEEDUE_RENEWAL);
                btnRenewal.ModuleName = moduleName;
                btnRenewal.LabelKey = "per_permitList_label_renewal_payfeedue";
            }
            else
            {
                btnRenewal.Visible = false;
            }
        }

        /// <summary>
        /// get text by filter name
        /// </summary>
        /// <param name="filterName">filter name</param>
        /// <returns>label key by filter name.</returns>
        private static string GetLabelKeyByFilterName(string filterName)
        {
            string labelKey;

            if (ACAConstant.REQUEST_PARMETER_TRADE_NAME.Equals(filterName, StringComparison.InvariantCultureIgnoreCase))
            {
                labelKey = "per_permitList_label_renewaltradename"; // renew trade name
            }
            else if (ACAConstant.REQUEST_PARMETER_TRADE_LICENSE.Equals(filterName, StringComparison.InvariantCultureIgnoreCase))
            {
                labelKey = "per_permitList_label_renewaltradelicense"; // renew trade license
            }
            else
            {
                labelKey = "per_permitList_label_renewalApplication";
            }

            return labelKey;
        }

        /// <summary>
        /// Gets the row button CommandArgument.
        /// </summary>
        /// <param name="capIDModel">The CapIDModel.</param>
        /// <param name="actionFlag">The action flag.</param>
        /// <param name="externalFlag">The external flag, default value is null.</param>
        /// <returns>Return the row button CommandArgument</returns>
        private static string GetRowButtonCommandArgument(CapIDModel4WS capIDModel, string actionFlag, string externalFlag = null)
        {
            string result = string.Format(
                                    "{1}{0}{2}{0}{3}{0}{4}{0}{5}",
                                   ACAConstant.COMMA,
                                   capIDModel.id1,
                                   capIDModel.id2,
                                   capIDModel.id3,
                                   actionFlag,
                                   capIDModel.serviceProviderCode);

            if (!string.IsNullOrEmpty(externalFlag))
            {
                result += ACAConstant.COMMA + externalFlag;
            }

            return result;
        }

        /// <summary>
        /// Objects the T to object R.
        /// 1. convert OwnerModel to RefOwnerModel
        /// 2. convert RefAddressModel to AddressModel
        /// 3. otherwise, the T and R is same type.
        /// </summary>
        /// <typeparam name="T">Daily entity</typeparam>
        /// <typeparam name="R">Ref entity</typeparam>
        /// <param name="objR">The object R.</param>
        /// <returns>The object T.</returns>
        private static T ObjectR2ObjectT<T, R>(R objR)
            where T : class
            where R : class
        {
            T objT = default(T);

            if (objR is OwnerModel)
            {
                //convert OwnerModel to RefOwnerModel
                var objSourceTemp = objR as OwnerModel;
                objT = ConvertOwnerModel2RefOwnerModel(objSourceTemp) as T;
            }
            else if (objR is RefAddressModel)
            {
                //convert RefAddressModel to AddressModel
                var objSourceTemp = objR as RefAddressModel;
                objT = ConvertRefAddressModel2AddressModel(objSourceTemp) as T;
            }
            else
            {
                // ohterwise, the T and R is same type
                objT = objR as T;
            }

            return objT;
        }

        /// <summary>
        /// According to GView settings to copy invisible fields from <see cref="source"/> to <see cref="target"/>.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="target">The target object</param>
        /// <param name="gViewElements">The GView elements.</param>
        /// <param name="propNamePrefix">Property name prefix stored in View element for the nesting data model.</param>
        private static void CopyInvisibleFieldsByGview<T>(T source, T target, SimpleViewElementModel4WS[] gViewElements, string propNamePrefix) where T : class
        {
            if (source == null || target == null)
            {
                return;
            }

            foreach (SimpleViewElementModel4WS simpleViewElementModel4Ws in gViewElements)
            {
                //hidden field 
                if (ACAConstant.INVALID_STATUS.Equals(simpleViewElementModel4Ws.recStatus)
                    && !string.IsNullOrEmpty(simpleViewElementModel4Ws.viewElementDesc))
                {
                    string viewElementDesc = simpleViewElementModel4Ws.viewElementDesc;
                    int objectNamePosition = viewElementDesc.IndexOf(propNamePrefix, StringComparison.InvariantCultureIgnoreCase);

                    if (objectNamePosition == -1)
                    {
                        objectNamePosition = 0;
                    }

                    string[] fieldReflection = viewElementDesc.Substring(propNamePrefix.Length + objectNamePosition).Split('*');

                    SetHiddenFieldsPropertyValue(source, target, fieldReflection);

                    /*
                     * Keep the reference value for all Country Code fields of the phone and fax fields.
                     * Because the GVIEW_ELEMENT never maintain the Country Code part of the phone field.
                     *  so we need add the special logic keep the Country Code part.
                     */
                    if (simpleViewElementModel4Ws.viewElementDesc.IndexOf("phone", StringComparison.InvariantCultureIgnoreCase) != -1
                        || simpleViewElementModel4Ws.viewElementDesc.IndexOf("fax", StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        string[] fieldForCountryCodeReflection = (viewElementDesc.Substring(propNamePrefix.Length + objectNamePosition) + "CountryCode").Split('*');
                        SetHiddenFieldsPropertyValue(source, target, fieldForCountryCodeReflection);
                    }
                }
            }
        }

        /// <summary>
        /// set the property value
        /// </summary>
        /// <typeparam name="T">the source model type</typeparam>
        /// <param name="source">the source model</param>
        /// <param name="target">the target model</param>
        /// <param name="fieldReflection">the field name</param>
        private static void SetHiddenFieldsPropertyValue<T>(T source, T target, string[] fieldReflection) where T : class
        {
            PropertyInfo propSource = null;
            PropertyInfo propTarget = null;
            object sourceObj = null;
            object targetObj = null;

            foreach (string fieldNameString in fieldReflection)
            {
                sourceObj = propSource == null ? source : propSource.GetValue(sourceObj, null);

                if (sourceObj != null)
                {
                    propSource = sourceObj.GetType().GetProperty(fieldNameString);
                }

                targetObj = propTarget == null ? target : propTarget.GetValue(targetObj, null);

                if (targetObj != null)
                {
                    propTarget = targetObj.GetType().GetProperty(fieldNameString);
                }
            }

            if (propSource != null && propTarget != null
                && sourceObj != null && targetObj != null)
            {
                object tempObj = propTarget.GetValue(targetObj, null);

                if (tempObj == null || string.IsNullOrEmpty(tempObj.ToString()))
                {
                    propTarget.SetValue(targetObj, propSource.GetValue(sourceObj, null), null);
                }
            }
        }

        /// <summary>
        /// Validate LP list whether existing value is empty for required fields.
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="lpListEdit">LPList Section</param>
        /// <returns>true:success to validate,false:failed to validate</returns>
        private static bool ValidateRequiredField4LPList(CapModel4WS capModel, string moduleName, MultiLicensesEdit lpListEdit)
        {
            bool isSucceeded = true;
            string serviceProviderCode = null;
            LicenseProfessionalModel4WS[] licenseProfessionalList = LicenseUtil.FindLicenseProfessionalsWithComponentName(capModel, lpListEdit.ComponentName);

            if (licenseProfessionalList == null || licenseProfessionalList.Length == 0)
            {
                return true;
            }

            if (capModel.capID != null)
            {
                serviceProviderCode = capModel.capID.serviceProviderCode;
            }

            LicenseProfessionalModel[] licenses = LicenseUtil.ResetLicenseeAgency(licenseProfessionalList, serviceProviderCode);

            if (!lpListEdit.IsEditable || lpListEdit.IsValidate)
            {
                List<TemplateAttributeModel> fields = new List<TemplateAttributeModel>();

                foreach (LicenseProfessionalModel lp in licenses)
                {
                    //judge whethere exist template is always,required.
                    fields.AddRange(TemplateUtil.GetAlwaysEditableRequiredTemplateFields(lp.templateAttributes));
                }

                if (fields.Any())
                {
                    isSucceeded = RequiredValidationUtil.ValidateFields4Template(fields.ToArray());
                }
            }
            else
            {
                if (!RequiredValidationUtil.ValidateFields4LPList(moduleName, GviewID.LicenseEdit, licenses)
                    || !FormatValidationUtil.ValidateFormat4LPList(moduleName, GviewID.LicenseEdit, licenses))
                {
                    isSucceeded = false;
                }
            }

            return isSucceeded;
        }

        /// <summary>
        /// fill contact template value to contact model.
        /// </summary>
        /// <param name="templateBll">template contact model.</param>
        /// <param name="capID">cap id model.</param>
        /// <param name="contactModel">cap contact model.</param>
        private static void FillContactTemplateValue(ITemplateBll templateBll, CapIDModel4WS capID, CapContactModel4WS contactModel)
        {
            if (contactModel != null && contactModel.people != null && !string.IsNullOrEmpty(contactModel.people.contactType) &&
                !string.IsNullOrEmpty(contactModel.people.contactSeqNumber))
            {
                contactModel.people.attributes = templateBll.GetDailyPeopleTemplateAttributes(contactModel.people.contactType, capID, contactModel.people.contactSeqNumber, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }
        }

        /// <summary>
        /// Get the data filter privilege for current user.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>Data filter role.</returns>
        private static UserRoleType GetPrivilegeForCAP(string moduleName)
        {
            object objCAP = AppSession.GetCapModelFromSession(moduleName);

            if (objCAP == null)
            {
                return UserRoleType.AllACAUsers;
            }

            CapModel4WS capModel = objCAP as CapModel4WS;

            // if the cap creator is the current user
            if (capModel.createdBy ==
                AppSession.User.PublicUserId)
            {
                return UserRoleType.CAPCreator;
            }

            // if the current user is CAP creator or the user's license is related with this CAP.
            if (capModel.hasPrivilegeToHandleCap)
            {
                return UserRoleType.CAPCreatorAndAssociatedLicensedProfessionals;
            }

            return UserRoleType.AllACAUsers;
        }

        /// <summary>
        /// Gets all fee items by  cap id.
        /// </summary>
        /// <param name="capId">Cap id.</param>
        /// <param name="moduleName">Module name, used to get current cap from session and to determine if it's super cap.</param>
        /// <returns>Array of fee item.</returns>
        private static F4FeeModel4WS[] GetAllFeeItems(CapIDModel4WS capId, string moduleName)
        {
            // Get all of fee items related to the current cap id.
            F4FeeModel4WS[] fees;
            var feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));
            var isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());

            if (IsSuperCAP(moduleName) || isAssoFormEnabled)
            {
                fees = feeBll.GetFeeItemsByParentCapID(capId, isAssoFormEnabled, AppSession.User.PublicUserId);
            }
            else
            {
                fees = feeBll.GetFeeItemsByCapID(capId, AppSession.User.PublicUserId);
            }

            return fees;
        }

        /// <summary>
        /// Check if any one fee item is read only.
        /// </summary>
        /// <param name="fees">fee items</param>
        /// <returns>
        /// True  ---- No fee items or All Fee item are read only.
        /// False ---- Any fee item is not read only.
        /// </returns>
        private static bool IsReadOnlyForAllFeeItems(F4FeeModel4WS[] fees)
        {
            var isReadOnly = true;

            // If no fee items, regarded that all fee items is read only.
            if (fees != null && fees.Length > 0)
            {
                if (fees.Any(fee => fee.f4FeeItemModel != null && !ValidationUtil.IsReadOnly(fee.f4FeeItemModel.defaultFlag)))
                {
                    isReadOnly = false;
                }
            }

            return isReadOnly;
        }

        /// <summary>
        /// Calculate fee amount
        /// </summary>
        /// <param name="capId">Cap ID Model.</param>
        /// <param name="moduleName">the module Name.</param>
        /// <returns>Fee amount</returns>
        private static double CalculateFeeAmount(CapIDModel4WS capId, string moduleName)
        {
            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));

            double feeAmount = 0;

            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            if (IsSuperCAP(moduleName) || isAssoFormEnabled)
            {
                feeAmount = feeBll.GetTotalFeeByParentCapID(capId, isAssoFormEnabled, AppSession.User.PublicUserId);
            }
            else
            {
                feeAmount = feeBll.GetTotalFee(capId, AppSession.User.PublicUserId);
            }

            return feeAmount;
        }

        /// <summary>
        /// Finds license type setting information for the specified section in the given XPolicyModel array and fills 
        /// found license types into the specified 
        /// </summary>
        /// <param name="xpolicys">XPolicy list in which to find matching license types for the section</param>
        /// <param name="sectionName">the name of the section for which to find license types</param>
        /// <param name="sectionPermission">UserRolePrivilegeModel instance to be filled in</param>
        private static void FillInLicenseTypesInfo(XPolicyModel[] xpolicys, string sectionName, UserRolePrivilegeModel sectionPermission)
        {
            // find related license types
            foreach (XPolicyModel xpolicy in xpolicys)
            {
                if (EntityType.LICENSETYPE.ToString().Equals(xpolicy.data3, StringComparison.InvariantCultureIgnoreCase) &&
                    sectionName.Equals(xpolicy.data4, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(xpolicy.data2))
                    {
                        sectionPermission.licenseTypeRuleArray
                            = xpolicy.data2.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Get the default section permission for the specified section.
        /// </summary>
        /// <param name="section">The section to get the default section permission</param>
        /// <returns>true if the default section permission is visible; otherwise, false</returns>
        /// <remarks>For compatibility with previous version, in this feature, the following sections:
        ///   1.Education, 
        ///   2.Continuing Education, 
        ///   3.Examination
        /// by default will be NOT visible for any users (except Admin), and the other sections will be visible for any users
        /// </remarks>
        private static bool GetDefaultVisibility(string section)
        {
            bool visible = !(CapDetailSectionType.EDUCATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase) ||
                             CapDetailSectionType.CONTINUING_EDUCATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase) ||
                             CapDetailSectionType.EXAMINATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase) ||
                             CapDetailSectionType.ASSETS.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase));

            return visible;
        }

        /// <summary>
        /// Gets the single ASI group model.
        /// </summary>
        /// <param name="asiGroup">The ASI group.</param>
        /// <param name="asiSubgroup">The ASI subgroup.</param>
        /// <param name="totalModelList">The total AppSpecificInfoGroupModel list.</param>
        /// <returns>Return the AppSpecificInfoGroupModel that filter by ASI group and subgroup.</returns>
        private static AppSpecificInfoGroupModel4WS GetSingleASIGroupModel(string asiGroup, string asiSubgroup, AppSpecificInfoGroupModel4WS[] totalModelList)
        {
            if (string.IsNullOrEmpty(asiGroup) || string.IsNullOrEmpty(asiSubgroup))
            {
                return null;
            }

            AppSpecificInfoGroupModel4WS result = null;

            foreach (AppSpecificInfoGroupModel4WS model in totalModelList)
            {
                if (model.groupCode == asiGroup && model.groupName == asiSubgroup)
                {
                    result = model;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the single ASIT group model.
        /// </summary>
        /// <param name="asitGroup">The ASIT group.</param>
        /// <param name="asitSubgroup">The ASIT subgroup.</param>
        /// <param name="totalModelList">The total AppSpecificTableGroupModel list.</param>
        /// <returns>Return the AppSpecificTableGroupModel that filter by ASI group and subgroup.</returns>
        private static AppSpecificTableGroupModel4WS GetSingleASITGroupModel(string asitGroup, string asitSubgroup, AppSpecificTableGroupModel4WS[] totalModelList)
        {
            if (string.IsNullOrEmpty(asitGroup) || string.IsNullOrEmpty(asitSubgroup))
            {
                return null;
            }

            AppSpecificTableGroupModel4WS result = null;

            foreach (AppSpecificTableGroupModel4WS groupModel in totalModelList)
            {
                List<AppSpecificTableModel4WS> tableList = new List<AppSpecificTableModel4WS>();

                if (groupModel != null && groupModel.tablesMapValues != null)
                {
                    foreach (AppSpecificTableModel4WS model in groupModel.tablesMapValues)
                    {
                        if (model.groupName == asitGroup && model.tableName == asitSubgroup)
                        {
                            tableList.Add(model);
                        }
                    }
                }

                if (tableList.Count > 0)
                {
                    AppSpecificTableGroupModel4WS tempGroupModel = new AppSpecificTableGroupModel4WS();
                    tempGroupModel.capIDModel = groupModel.capIDModel;
                    tempGroupModel.groupName = groupModel.groupName;
                    tempGroupModel.instruction = groupModel.instruction;
                    tempGroupModel.resInstruction = groupModel.resInstruction;
                    tempGroupModel.tablesMap = groupModel.tablesMap;
                    tempGroupModel.tablesMapValues = tableList.ToArray();

                    result = tempGroupModel;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the assigned groups in the page flow configuration.
        /// </summary>
        /// <param name="pfGroupModel">The page flow group model.</param>
        /// <returns>Return the assigned groups, the format: 'componentId_Subgroup' list.</returns>
        private static List<string> GetAssignedGroups(PageFlowGroupModel pfGroupModel)
        {
            List<string> result = new List<string>();

            foreach (StepModel stepModel in pfGroupModel.stepList)
            {
                foreach (PageModel pageModel in stepModel.pageList)
                {
                    foreach (ComponentModel componentModel in pageModel.componentList)
                    {
                        if (!string.IsNullOrEmpty(componentModel.portletRange1) && !string.IsNullOrEmpty(componentModel.portletRange2))
                        {
                            // it only judge the subgroup, the same subgroup MUST NOT display.
                            string key = string.Format("{0}_{1}", componentModel.componentID, componentModel.portletRange2);

                            result.Add(key);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether ASI is empty.
        /// </summary>
        /// <param name="capModel">The CAP model.</param>
        /// <param name="pfGroupModel">The page flow group model.</param>
        /// <param name="cptModel">The component model.</param>
        /// <returns>
        /// return true if ASI is empty, otherwise false.
        /// </returns>
        private static bool IsASIEmpty(CapModel4WS capModel, PageFlowGroupModel pfGroupModel, ComponentModel cptModel)
        {
            bool isEmpty = true;

            if (capModel.appSpecificInfoGroups == null || capModel.appSpecificInfoGroups.Length == 0)
            {
                return true;
            }

            // the assigned ASI group/subgroup
            if (!string.IsNullOrEmpty(cptModel.portletRange1))
            {
                AppSpecificInfoGroupModel4WS asiGroupModel = GetSingleASIGroupModel(cptModel.portletRange1, cptModel.portletRange2, capModel.appSpecificInfoGroups);

                if (ASIBaseUC.ExistsASIFields(new[] { asiGroupModel }))
                {
                    isEmpty = false;
                }
            }
            else
            {
                // the non-assigned ASI group/subgroup
                List<string> assignedGroups = GetAssignedGroups(pfGroupModel);

                foreach (AppSpecificInfoGroupModel4WS asiGroupModel in capModel.appSpecificInfoGroups)
                {
                    //asiGroupModel.groupName is sub group name
                    string key = string.Format("{0}_{1}", cptModel.componentID, asiGroupModel.groupName);

                    if (!assignedGroups.Contains(key) && ASIBaseUC.ExistsASIFields(new[] { asiGroupModel }))
                    {
                        isEmpty = false;
                        break;
                    }
                }
            }

            return isEmpty;
        }

        /// <summary>
        /// Determines whether ASIT is empty.
        /// </summary>
        /// <param name="capModel">The CAP model.</param>
        /// <param name="pfGroupModel">The page flow group model.</param>
        /// <param name="cptModel">The component model.</param>
        /// <returns>
        /// return true if ASIT is empty, otherwise false.
        /// </returns>
        private static bool IsASITEmpty(CapModel4WS capModel, PageFlowGroupModel pfGroupModel, ComponentModel cptModel)
        {
            bool isASITEmpty = true;

            if (capModel.appSpecTableGroups == null || capModel.appSpecTableGroups.Length == 0)
            {
                return true;
            }

            // assigned ASIT group/subgroup
            if (!string.IsNullOrEmpty(cptModel.portletRange1))
            {
                AppSpecificTableGroupModel4WS asitGroupModel = GetSingleASITGroupModel(cptModel.portletRange1, cptModel.portletRange2, capModel.appSpecTableGroups);

                if (ASIBaseUC.ExistsASITFields(new[] { asitGroupModel }))
                {
                    isASITEmpty = false;
                }
            }
            else
            {
                // the non-assigned ASIT group/subgroup
                List<string> assignedGroups = GetAssignedGroups(pfGroupModel);

                foreach (AppSpecificTableGroupModel4WS groupModel in capModel.appSpecTableGroups)
                {
                    // loop add the non-assigned AppSpecificTableModel
                    if (groupModel != null && groupModel.tablesMapValues != null)
                    {
                        foreach (AppSpecificTableModel4WS model in groupModel.tablesMapValues)
                        {
                            // it only judge the subgroup, the same subgroup MUST NOT display.
                            string key = string.Format("{0}_{1}", cptModel.componentID, model.tableName);
                            bool existFields = model.columns != null && model.columns.Length > 0;

                            if (!assignedGroups.Contains(key) && existFields)
                            {
                                isASITEmpty = false;
                                break;
                            }
                        }
                    }
                }
            }

            return isASITEmpty;
        }

        /// <summary>
        /// Builds the AKA json.
        /// </summary>
        /// <param name="peopleAkaModels">The people aka models.</param>
        /// <returns>The AKA json object.</returns>
        private static string BuildAKAJson(PeopleAKAModel[] peopleAkaModels)
        {
            if (peopleAkaModels == null || peopleAkaModels.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder akaJson = new StringBuilder();
            akaJson.Append("[");

            foreach (var peopleAkaModel in peopleAkaModels)
            {
                akaJson.Append("{");
                akaJson.AppendFormat(
                    "\"FirstName\":\"{0}\",\"MiddleName\":\"{1}\",\"LastName\":\"{2}\",\"FullName\":\"{3}\",\"StartDate\":\"{4}\",\"EndDate\":\"{5}\",\"AKAID\":\"{6}\"",
                    peopleAkaModel.firstName,
                    peopleAkaModel.middleName,
                    peopleAkaModel.lastName,
                    string.IsNullOrEmpty(peopleAkaModel.fullName) ? string.Empty : peopleAkaModel.fullName,
                    peopleAkaModel.startDate,
                    peopleAkaModel.endDate,
                    peopleAkaModel.resId == null ? string.Empty : peopleAkaModel.resId.ToString());
                akaJson.Append("},");
            }

            akaJson.Length -= 1;
            akaJson.Append("]");

            return akaJson.ToString();
        }

        /// <summary>
        /// Redirect the page after the cap saving that <para>isFeeEstimator</para> indicate the feeEstimator flag
        /// </summary>
        /// <param name="response">HttpResponse object</param>
        /// <param name="moduleName">module name</param>
        /// <param name="capId">CapIDModel4WS object</param>
        /// <param name="isFeeEstimator">in fee estimator string</param>
        /// <param name="isRenewal">is renewal string</param>
        /// <param name="isSuperAgencyAssoForm">Indicate whether it is associate form in super agency environment.</param>
        private static void SaveResumeRedirect(HttpResponse response, string moduleName, CapIDModel4WS capId, string isFeeEstimator, string isRenewal, string isSuperAgencyAssoForm)
        {
            string url;

            if (IsAssoFormChild(moduleName))
            {
                url = GetAssoFormUrl();
            }
            else
            {
                url = GetCapHomeUrl(moduleName, isFeeEstimator, isRenewal, isSuperAgencyAssoForm, capId);
            }

            response.Redirect(url);
        }

        /// <summary>
        /// get cap home url.
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <param name="isFeeEstimator">is fee estimator</param>
        /// <param name="isRenewal">is renewal</param>
        /// <param name="isSuperAgencyAssoForm">is associate form in super agency</param>
        /// <param name="capId">capId model</param>
        /// <returns>success message</returns>
        private static string GetCapHomeUrl(string moduleName, string isFeeEstimator, string isRenewal, string isSuperAgencyAssoForm, CapIDModel4WS capId)
        {
            var altId = string.Empty;
            var successMsgKey = string.Empty;
            var url = "CapHome.aspx?Module={0}&successMsgKey={1}&altId={2}";
            isRenewal = string.IsNullOrEmpty(isRenewal) ? ACAConstant.COMMON_N : isRenewal;
            isFeeEstimator = string.IsNullOrEmpty(isFeeEstimator) ? ACAConstant.COMMON_N : isFeeEstimator;

            if (capId != null)
            {
                var capBll = ObjectFactory.GetObject<ICapBll>();
                var capModel = capBll.GetCapByPK(capId);

                if (capModel != null)
                {
                    altId = capModel.altID;

                    if (ACAConstant.COMMON_Y.Equals(isFeeEstimator, StringComparison.InvariantCultureIgnoreCase))
                    {
                        successMsgKey = "per_permitList_msg_estimate2_success";
                    }
                    else if (ACAConstant.COMMON_Y.Equals(isRenewal, StringComparison.InvariantCultureIgnoreCase))
                    {
                        successMsgKey = "per_permitList_msg_saveRenewalProcess_success";
                    }
                    else
                    {
                        // if come from super agency associated form, it need show the child caps create success.
                        if (ValidationUtil.IsYes(isSuperAgencyAssoForm))
                        {
                            var childCaps = capBll.GetChildCapDetailsByMasterID(capId, ACAConstant.CAP_RELATIONSHIP_RELATED, null);

                            if (childCaps.Length > 0)
                            {
                                var childCapsAltId = new StringBuilder();

                                foreach (var cap in childCaps)
                                {
                                    childCapsAltId.Append(ACAConstant.COMMA_BLANK);
                                    childCapsAltId.Append(cap.altID);
                                }

                                childCapsAltId.Remove(0, ACAConstant.COMMA_BLANK.Length);
                                altId = childCapsAltId.ToString();
                            }
                        }

                        successMsgKey = "per_permitList_msg_application_success";
                    }
                }
            }

            url = string.Format(url, moduleName, successMsgKey, altId);

            return url;
        }

        /// <summary>
        /// TransForm PublicUserModel4WS to ContactModel
        /// </summary>
        /// <param name="stateContact">contact sequence number</param>
        /// <param name="contactType">contact Type</param>
        /// <param name="moduleName">the module Name</param>
        /// <returns>string used for auto fill control</returns>
        private static StringBuilder ConvertToContactModel(string stateContact, string contactType, string moduleName)
        {
            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS model = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(moduleName));

            StringBuilder sb = new StringBuilder();

            if (model != null && model.peopleModel != null)
            {
                PeopleModel4WS[] peopleModelList = model.peopleModel;
                int contactListLength = peopleModelList.Length;

                if (contactListLength > 0)
                {
                    foreach (PeopleModel4WS peopleModel in peopleModelList)
                    {
                        if (peopleModel.contactSeqNumber == stateContact && peopleModel.contactType == contactType)
                        {
                            sb.Append("{");

                            if (ConditionsUtil.IsContactLocked(stateContact))
                            {
                                AddKeyValue(sb, "IsLocked", "true");
                                return sb;
                            }

                            GeneralContactJSON(sb, peopleModel);

                            break;
                        }
                    }
                }
            }

            return sb;
        }

        /// <summary>
        /// Convert to contact model for authorized agent
        /// </summary>
        /// <param name="contactSeqNumber">The contact sequence number.</param>
        /// <returns>Return the contact model.</returns>
        private static StringBuilder ConvertToContactModel4AuthAgent(string contactSeqNumber)
        {
            StringBuilder sb = new StringBuilder();

            PeopleModel peopleModel = AppSession.GetPeopleModelFromSession(contactSeqNumber);

            if (peopleModel != null)
            {
                PeopleModel4WS peopleModel4WS = TempModelConvert.ConvertToPeopleModel4WS(peopleModel);

                sb.Append("{");
                GeneralContactJSON(sb, peopleModel4WS);
            }

            return sb;
        }

        /// <summary>
        /// General the contact JSON
        /// </summary>
        /// <param name="sb">The string builder</param>
        /// <param name="peopleModel">The people model</param>
        private static void GeneralContactJSON(StringBuilder sb, PeopleModel4WS peopleModel)
        {
            AddKeyValue(sb, "ContactRefSeqNumber", peopleModel.contactSeqNumber);
            AddKeyValue(sb, "Salutation", peopleModel.salutation);
            AddKeyValue(sb, "FirstName", ScriptFilter.EncodeJson(peopleModel.firstName));
            AddKeyValue(sb, "LastName", ScriptFilter.EncodeJson(peopleModel.lastName));
            AddKeyValue(sb, "FullName", ScriptFilter.EncodeJson(peopleModel.fullName));
            AddKeyValue(sb, "BirthDate", I18nDateTimeUtil.FormatToDateStringForUI(peopleModel.birthDate));
            AddKeyValue(sb, "Gender", peopleModel.gender);
            AddKeyValue(sb, "MiddleName", ScriptFilter.EncodeJson(peopleModel.middleName));
            AddKeyValue(sb, "BusinessName", ScriptFilter.EncodeJson(peopleModel.businessName));
            AddKeyValue(sb, "BusinessName2", ScriptFilter.EncodeJson(peopleModel.businessName2));
            AddKeyValue(sb, "Country", peopleModel.countryCode);
            string countryCode = peopleModel.countryCode;

            if (peopleModel.compactAddress != null)
            {
                countryCode = peopleModel.compactAddress.countryCode;

                AddKeyValue(sb, "Address1", ScriptFilter.EncodeJson(peopleModel.compactAddress.addressLine1));
                AddKeyValue(sb, "Address2", ScriptFilter.EncodeJson(peopleModel.compactAddress.addressLine2));
                AddKeyValue(sb, "Address3", ScriptFilter.EncodeJson(peopleModel.compactAddress.addressLine3));
                AddKeyValue(sb, "City", peopleModel.compactAddress.city);
                AddKeyValue(sb, "State", peopleModel.compactAddress.state);
                AddKeyValue(sb, "Zip", ModelUIFormat.FormatZipShow(peopleModel.compactAddress.zip, peopleModel.compactAddress.countryCode));
            }
            else
            {
                AddKeyValue(sb, "Address1", string.Empty);
                AddKeyValue(sb, "Address2", string.Empty);
                AddKeyValue(sb, "Address3", string.Empty);
                AddKeyValue(sb, "City", string.Empty);
                AddKeyValue(sb, "State", string.Empty);
                AddKeyValue(sb, "Zip", string.Empty);
            }

            AddKeyValue(sb, "PostOfficeBox", peopleModel.postOfficeBox);
            AddKeyValue(sb, "HomePhoneIDD", peopleModel.phone1CountryCode);
            AddKeyValue(sb, "HomePhone", ModelUIFormat.FormatPhone4EditPage(peopleModel.phone1, countryCode));
            AddKeyValue(sb, "MobilePhoneIDD", peopleModel.phone2CountryCode);
            AddKeyValue(sb, "MobilePhone", ModelUIFormat.FormatPhone4EditPage(peopleModel.phone2, countryCode));
            AddKeyValue(sb, "WorkPhoneIDD", peopleModel.phone3CountryCode);
            AddKeyValue(sb, "WorkPhone", ModelUIFormat.FormatPhone4EditPage(peopleModel.phone3, countryCode));
            AddKeyValue(sb, "FaxIDD", peopleModel.faxCountryCode);
            AddKeyValue(sb, "Fax", ModelUIFormat.FormatPhone4EditPage(peopleModel.fax, countryCode));
            AddKeyValue(sb, "Email", peopleModel.email);
            AddKeyValue(sb, "Suffix", ScriptFilter.EncodeJson(peopleModel.namesuffix));
            AddKeyValue(sb, "Title", ScriptFilter.EncodeJson(peopleModel.title));
            AddKeyValue(sb, "ContactType", peopleModel.contactType);
            AddKeyValue(sb, "SSN", peopleModel.socialSecurityNumber);
            AddKeyValue(sb, "Fein", peopleModel.fein);
            AddKeyValue(sb, "TradeName", ScriptFilter.EncodeJson(peopleModel.tradeName));
            AddKeyValue(sb, "ContactTypeFlag", peopleModel.contactTypeFlag);
            AddKeyValue(sb, "BirthplaceCity", ScriptFilter.EncodeJson(peopleModel.birthCity));
            AddKeyValue(sb, "BirthplaceState", ScriptFilter.EncodeJson(peopleModel.birthState));
            AddKeyValue(sb, "BirthplaceCountry", ScriptFilter.EncodeJson(peopleModel.birthRegion));
            AddKeyValue(sb, "Race", ScriptFilter.EncodeJson(peopleModel.race));
            AddKeyValue(sb, "DeceasedDate", I18nDateTimeUtil.FormatToDateStringForUI(peopleModel.deceasedDate));
            AddKeyValue(sb, "PassportNumber", ScriptFilter.EncodeJson(peopleModel.passportNumber));
            AddKeyValue(sb, "DriverLicenseNumber", ScriptFilter.EncodeJson(peopleModel.driverLicenseNbr));
            AddKeyValue(sb, "DriverLicenseState", ScriptFilter.EncodeJson(peopleModel.driverLicenseState));
            AddKeyValue(sb, "StateIdNumber", ScriptFilter.EncodeJson(peopleModel.stateIDNbr));
            AddKeyValue(sb, "PreferredChannel", peopleModel.preferredChannel == "0" ? string.Empty : peopleModel.preferredChannel);
            AddKeyValue(sb, "Notes", ScriptFilter.EncodeJson(peopleModel.comment));
            string akaString = peopleModel.peopleAKAList == null || peopleModel.peopleAKAList.Length == 0 ? "\"\"" : BuildAKAJson(peopleModel.peopleAKAList);
            sb.Append("\"PeopleAKA\":" + akaString + ",");

            if (StandardChoiceUtil.IsEnableContactAddress())
            {
                if (peopleModel.contactAddressList != null && peopleModel.contactAddressList.Length > 0)
                {
                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    string contactAddressList = jsSerializer.Serialize(peopleModel.contactAddressList);
                    AddKeyValue(sb, "ContactAddress", ScriptFilter.EncodeJson(contactAddressList));
                }
                else
                {
                    AddKeyValue(sb, "ContactAddress", "[]");
                }
            }

            sb.Append("\"Templates\":" + BuildTemplateFiledString(peopleModel.attributes, string.Empty) + ",");
            sb.Append("\"GenericTemplate\":" + BuildGenericTemplateFieldString(peopleModel.template));
            sb.Append(",");

            if (peopleModel.template != null && peopleModel.template.templateTables != null &&
                peopleModel.template.templateTables.Length > 0)
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                string genericTemplateTables = jsSerializer.Serialize(peopleModel.template.templateTables);
                AddKeyValue(sb, "GenericTemplateTables", ScriptFilter.EncodeJson(genericTemplateTables));
            }
        }

        #endregion Methods
    }
}
