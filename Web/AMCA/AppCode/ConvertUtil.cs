/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ConvertUtil.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2012
 * 
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: ConvertUtil.cs 125774 2009-04-01 17:35:13Z dave.brewster $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * 04/01/2009           Dave Brewster           Added code to populate permit.TypeAlias field, and 
 *                                              added code to move AltID to permit.Alias.
 * </pre>
 */

using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;

/// <summary>
/// Summary description for ConvertUtil
/// </summary>
public class ConvertUtil
{
    public static Permit convertSimpleCapModelToPermit(SimpleCapModel capModel)
    {
        if(capModel == null)
        {
            return null;
        }
        Permit permit = new Permit();
        permit.Address = "";
        //DWB - 07-21-2008 - captured the address model
        permit.AddressModel = capModel.addressModel;
        permit.Agency = capModel.capID.serviceProviderCode;
        permit.Status = capModel.capStatus;
        permit.Desc = "";
        //Kale Modi permit.Number = capModel.altID; altID is only a alias.Not the valid CAP Model ID.
        permit.Number = capModel.capID.ID1 + "-" + capModel.capID.ID2 + "-" + capModel.capID.ID3;
        permit.Alias = capModel.altID != null ? capModel.altID.ToString() : String.Empty;
        permit.Status = capModel.capStatus;
        permit.Type = capModel.capType.type;
        permit.Date = capModel.fileDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateStringForWebService(capModel.fileDate);
        permit.Info = capModel.capClass;
        permit.Module = capModel.moduleName;
        permit.capId1 = capModel.capID.ID1;
        permit.capId2 = capModel.capID.ID2;
        permit.capId3 = capModel.capID.ID3;
        permit.capClass = capModel.capClass;
        return permit;
    }
    //Kale.OK
    public static Permit convertCapModelToPermit(CapModel4WS capModel)
    {
        if (capModel == null)
        {
            return null;
        }
        Permit permit = new Permit();
        if(capModel.addressModel!=null)
            permit.Address = capModel.addressModel.displayAddress;


        string projectDesc = string.Empty;
        string projectNote = string.Empty;
        if (capModel.capWorkDesModel != null && !string.IsNullOrEmpty(capModel.capWorkDesModel.description))
        {
            projectDesc = capModel.capWorkDesModel.description;
        }

        if (capModel.capDetailModel != null && !string.IsNullOrEmpty(capModel.capDetailModel.shortNotes))
        {
            projectNote = capModel.capDetailModel.shortNotes;
        }
        permit.Desc = projectDesc;
        // DWB - 07-28-2008 - Changed to return CAP id instead of alt id
        //                    see Kale's comments "convertSimpleCapModelToPermit"
        // permit.Number = capModel.altID;
        permit.Number = capModel.capID.id1 + "-" + capModel.capID.id2 + "-" + capModel.capID.id3;
        permit.Alias = capModel.altID != null ? capModel.altID.ToString() : String.Empty;
        //DWB - 7.0 - 05/26/2009 - Added agency and module id to permit object.
        if (capModel.capID != null)
        {
            permit.Agency = capModel.capID.serviceProviderCode;
        }
        else
        {
            permit.Agency = capModel.capDetailModel.capID.serviceProviderCode;
        }
        if (capModel.moduleName != null)
        {
            permit.Module = capModel.moduleName;
        }
        else
        {
            permit.Module = capModel.capType.moduleName;
        }
        permit.Status = capModel.capStatus;
        if(capModel.capType!=null)
            permit.Type = String.Format("{0}/{1}/{2}/{3}", capModel.capType.group, capModel.capType.type, capModel.capType.subType, capModel.capType.category);
        permit.Date = capModel.reportedDate;
        permit.Info = capModel.capClass;
        if(capModel.ownerModel!=null)
            permit.Owner = capModel.ownerModel.ownerFullName;
        // DWB - 07-25-2008 - Added null checking and modified the permit object
        //                    to include the inspection type id.  
        if (capModel.licenseProfessionalModel != null)
        {
            permit.LicensedPro = capModel.licenseProfessionalModel.licenseNbr;
            permit.LicensedProType = capModel.licenseProfessionalModel.licenseType;
        }
        if (capModel.applicantModel != null && capModel.applicantModel.people != null)
        {
            // permit.Application = ModelUIFormat.FormatCapContactModel4Basic(capModel.applicantModel);
            permit.Application = Accela.ACA.BLL.Cap.CAPHelper.GetAliasOrCapTypeLabel(capModel);
            permit.Application = permit.Application.Replace("<br/>", "&nbsp;&nbsp;&nbsp;");
            permit.Application = permit.Application.Replace("<strong>", "");
            permit.Application = permit.Application.Replace("</strong>", "");
        }
        permit.TypeAlias = capModel.appTypeAlias != null ? capModel.appTypeAlias : string.Empty;
        //permit.Desc = capModel.bvaluatnModel == null ? String.Empty : capModel.bvaluatnModel.estimatedValue;
        //if (!String.IsNullOrEmpty(permit.Desc) && ValidationUtil.IsNumber(permit.Desc) && double.Parse(permit.Desc) > 0)
        //    permit.Desc = string.Format("{0:C}", double.Parse(permit.Desc));

        if (permit.Desc == null)
            permit.Desc = String.Empty;

        permit.Module = capModel.moduleName;
        permit.capId1 = capModel.capID.id1;
        permit.capId2 = capModel.capID.id2;
        permit.capId3 = capModel.capID.id3;
        permit.capClass = capModel.capClass;

        return permit;
    }
    //DWB- 11-16-2008 - Added for related permits list modification.
    public static Permit convertDataRowToPermit(DataRow aRow)
    {
        if (aRow == null)
        {
            return null;
        }
        Permit permit = new Permit();
        permit.Address = aRow["Address"].ToString();
        permit.Number = aRow["capID1"].ToString() + "-" + aRow["capID2"].ToString() + "-" + aRow["capID3"].ToString();
        permit.Status = aRow["Status"].ToString();
        permit.Alias = aRow["AltID"].ToString();
        permit.Module = aRow["Module"].ToString();
        permit.Agency = aRow["AgencyCode"].ToString();
        if (permit.Desc == null)
            permit.Desc = String.Empty;
        return permit;
    }
    public static Permit convertCapListDataRowToPermit(DataRowView  aRow)
    {
        if (aRow == null)
        {
            return null;
        }
        Permit permit = new Permit();
        permit.Agency = aRow["Agency"].ToString();
        permit.Module = aRow["Module"].ToString();
        permit.Number = aRow["capID1"].ToString() + "-" + aRow["capID2"].ToString() + "-" + aRow["capID3"].ToString();
        permit.Alias = aRow["Alias"].ToString();
        DateTime dateValue;
        if (DateTime.TryParse(aRow["Date"].ToString(), out dateValue))
        {
            permit.Date = dateValue.ToString();
        }
        else
        {

            permit.Date = aRow["Date"].ToString();
        }
        permit.Status = aRow["Status"].ToString();
        permit.capClass = aRow["capClass"].ToString();
        permit.capId1 = aRow["capID1"].ToString();
        permit.capId2 = aRow["capID2"].ToString();
        permit.capId3 = aRow["capID3"].ToString();
        permit.Address = aRow["Address"].ToString();

        return permit;
    }
    //Kale.OK
    public static CapModel4WS convertPermitToCapModel(Permit permit)
    {
        if(permit == null)
        {
            return null;
        }

        CapModel4WS capModel = new CapModel4WS();
        capModel.altID = permit.Number;
        //capModel.capStatus = permit.Status;
        capModel.fileDate = permit.Date;
        capModel.endFileDate = permit.EndDate;
        capModel.capClass = permit.Info;

        if (capModel.addressModel != null)
        {
            capModel.addressModel = capModel.addressModel;
        }
        
        if(!string.IsNullOrEmpty(permit.Number))
        {
            CapIDModel4WS capId = new CapIDModel4WS();
            capId.serviceProviderCode = permit.Agency;
            string[] ids = permit.Number.Split('-');

            capId.id1 = ids[0];
            capId.id2=ids.Length > 1?ids[1]:string.Empty;
            capId.id3=ids.Length>2?ids[2]:string.Empty;                    
            capModel.capID = capId;
        }
        
        if(!string.IsNullOrEmpty(permit.Type))
        {
            CapTypeModel capType = new CapTypeModel();
            capType.serviceProviderCode = permit.Agency;
            string[] types = permit.Type.Split('/');

            capType.group = types[0];
            capType.type = types.Length>1?types[1]:String.Empty;
            capType.subType = types.Length > 2 ? types[2] : String.Empty;
            capType.category = types.Length > 3 ? types[3] : String.Empty;

            capModel.capType = capType;
        }

        return capModel;
    }   
    //Kale.OK
    public static Permit[] convertSimpleCapsToPermits(SimpleCapModel[] caps)
    {
        if(caps == null || caps.Length == 0)
        {
            return null;
        }

        Permit[] permits = new Permit[caps.Length];
        int index = 0;

        // Convert capModel to permit model
        foreach(SimpleCapModel cap in caps)
        {
            Permit permit = convertSimpleCapModelToPermit(cap);

            permits[index] = permit;
            index++;
        }

        return permits;
    }
    public static Permit[] convertSimpleCapsToPermits(SimpleCapModel[] caps, string moduleName)
    {
        if (caps == null || caps.Length == 0)
        {
            return null;
        }

        int index = 0;
        foreach (SimpleCapModel cap in caps)
        {
            Permit permit = convertSimpleCapModelToPermit(cap);
            if (permit.Module == moduleName)
            {
                index++;
            }
        }
        if (index != 0)
        {

            Permit[] permits = new Permit[index];
            index = 0;

            // Convert capModel to permit model
            foreach (SimpleCapModel cap in caps)
            {
                Permit permit = convertSimpleCapModelToPermit(cap);
                if (permit.Module == moduleName)
                {
                    permits[index] = permit;
                    index++;
                }
            }
            return permits;
        }
        else
        {
            Permit[] permits = null;
            return permits;
        }

    }
    //Kale.OK
    public static Permit[] convertCapsToPermits(CapModel4WS[] caps)
    {
        if (caps == null || caps.Length == 0)
        {
            return null;
        }

        Permit[] permits = new Permit[caps.Length];
        int index = 0;

        // Convert capModel to permit model
        foreach (CapModel4WS cap in caps)
        {
            Permit permit = convertCapModelToPermit(cap);

            permits[index] = permit;
            index++;
        }

        return permits;
    }
    public static Permit[] convertDatatableToPermit(DataTable caps)
    {
        if (caps == null || caps.Rows.Count == 0)
        {
            return null;
        }

        Permit[] permits = new Permit[caps.Rows.Count];
        int index = 0;
        // Convert capModel to permit model
        for (int rows = 0; rows < caps.Rows.Count; rows++)
        {
            Permit permit = convertDataRowToPermit(caps.Rows[rows]);
            permits[index] = permit;
            index++;
        }

        return permits;
    }
    public static Permit[] convertCapListDatatableToPermit(DataTable caps)
    {
        if (caps == null || caps.Rows.Count == 0)
        {
            return null;
        }

        Permit[] permits = new Permit[caps.DefaultView.Count];
        int index = 0;
        // Convert capModel to permit model
        foreach (DataRowView drRow in caps.DefaultView)
        {
        //for (int rows = 0; rows < caps.DefaultView.Count; rows++)
        //{
            Permit permit = convertCapListDataRowToPermit(drRow);
            permits[index] = permit;
            index++;
        }

        return permits;
    }

    //Kale.OK
    public static LicenseProfessionalModel4WS convertLicenseToProfessional(License license)
    {
        LicenseProfessionalModel4WS professional = new LicenseProfessionalModel4WS();
        professional.businessName = license.BusinessName;
        professional.city = license.City;
        professional.country = license.Country;
        professional.contactFirstName = license.FirstName;
        professional.contactMiddleName = license.MiddleName;
        professional.contactLastName = license.LastName;
        professional.licenseNbr = license.LicenseNumber;
        professional.licenseType = license.LicenseType;

        return professional;
    }
    //Kale.OK
    public static AddressModel convertLocationToAddress(Location location)
    {
        AddressModel address = new AddressModel();
        address.city = location.City;
        //address.country = location.Country;
        address.streetDirection = location.Dir;
        address.houseFractionStart = location.Fraction;
        address.houseNumberStart = StringUtil.ToInt(location.Number);
        //address.capID = location.PermitNo;
        //address. = location.Proximity;
        address.state = location.State;
        address.streetName = location.StreetName;
        //address. = location.StreetType;
        address.unitStart = location.UnitNo;
        address.unitType = location.UnitType;
        address.zip = location.Zip;

        return address;
    }
    ////Kale.OK
    //public static Location convertRefAddressToLocation(RefAddressModel4WS refAddress)
    //{
    //    if(refAddress == null)
    //    {
    //        return null;
    //    }

    //    Location location = new Location();
    //    location.City = refAddress.city;
    //    location.Country = refAddress.country;
    //    location.Dir = refAddress.streetDirection;
    //    location.Fraction = refAddress.houseFractionStart;
    //    location.Number = refAddress.houseNumberStart;
    //    //address.capID = location.PermitNo;
    //    //address. = location.Proximity;
    //    location.State = refAddress.state;
    //    location.StreetName = refAddress.streetName;
    //    //address. = location.StreetType;
    //    location.UnitNo = refAddress.unitStart;
    //    location.UnitType = refAddress.unitType;
    //    location.Zip = refAddress.zip;
    //    // DWB - 07-25-2008 - Added Full Address to location object 
    //    location.FullAddress = refAddress.fullAddress;

    //    return location;
    //}
    ////Kale.OK
    //public static License convertLicenseModelToLicense(LicenseModel4WS licenseModel)
    //{
    //    if(licenseModel == null)
    //    {
    //        return null;
    //    }

    //    License license = new License();
    //    license.BusinessName = licenseModel.businessName;
    //    license.City = licenseModel.city;
    //    license.Country = licenseModel.country;
    //    license.FirstName = licenseModel.contactFirstName;
    //    license.MiddleName = licenseModel.contactMiddleName;
    //    license.LastName = licenseModel.contactLastName;
    //    license.LicenseNumber = licenseModel.stateLicense;
    //    license.LicenseType = licenseModel.licenseType;
    //    // DWB - 07-25-2008 - Added phone number for 2008 redesign.
    //    license.Phone1 = licenseModel.phone1;

    //    return license;
    //}
    //Kale.OK
   
    public static Inspection convertInspectionWSModelToInspection(InspectionModel inpsectionModel)
    {
        if (inpsectionModel == null)
        {
            return null;
        }

        Inspection inspection = new Inspection();

        if (inpsectionModel.primaryAddress != null)
        {
            inspection.Address = inpsectionModel.primaryAddress.displayAddress;
        }
        if (inpsectionModel.resultComment != null)
        {
            inspection.Comments = inpsectionModel.resultComment.ToString();
        }
        if (inpsectionModel.cap!=null && inpsectionModel.cap.capID != null)
        {
            inspection.PermitNo = inpsectionModel.cap.capID.ID1 + "-" + inpsectionModel.cap.capID.ID2 + "-" + inpsectionModel.cap.capID.ID3;
        }
        else if (inpsectionModel.activity.capID != null)
        {
            inspection.PermitNo=inpsectionModel.activity.capID.ID1 + "-" + inpsectionModel.activity.capID.ID2 + "-" + inpsectionModel.activity.capID.ID3;
        }

        if (inpsectionModel.activity != null && inpsectionModel.activity.activityDate != null)
        {
            inspection.Date = I18nDateTimeUtil.FormatToDateTimeStringForWebService(inpsectionModel.activity.activityDate.Value);
            inspection.Id = inpsectionModel.activity.idNumber.ToString();
            //inspection.Inspector = inpsectionModel.activity.

            inspection.Status = inpsectionModel.activity.status;
            inspection.Type = inpsectionModel.activity.activityType;
        }
        if (inpsectionModel!= null)
            inspection.InspectionModel = inpsectionModel;

        return inspection;
    }

    public static Inspection convertInspectionDataModelToInspection(InspectionDataModel inspectionModel)
    {
        if (inspectionModel == null)
        {
            return null;
        }

        Inspection inspection = new Inspection();

        //if (inpsectionModel.primaryAddress != null)
        //{
         //   inspection.Address = inpsectionModel.primaryAddress.displayAddress;
        // }

        if (inspectionModel.ResultComments != null)
        {
            inspection.Comments = inspectionModel.ResultComments.ToString();
        }

        if (inspectionModel.InspectionModel.cap != null && inspectionModel.InspectionModel.cap.capID != null)
        {
            inspection.PermitNo = inspectionModel.InspectionModel.cap.capID.ID1 + "-"
                + inspectionModel.InspectionModel.cap.capID.ID2 + "-"
                + inspectionModel.InspectionModel.cap.capID.ID3;
        }

        inspection.Date = inspectionModel.ScheduledDateTime.ToString();
        inspection.Id = inspectionModel.ID.ToString();
        inspection.Status = inspectionModel.Status.ToString();
        inspection.Type = inspectionModel.Type;
        inspection.InspectionModel = inspectionModel.InspectionModel;
    /*
        if (inpsectionModel.activity != null && inpsectionModel.activity.activityDate != null)
        {
            inspection.Date = I18nDateTimeUtil.FormatToDateTimeStringForWebService(inpsectionModel.activity.activityDate.Value);
            inspection.Id = inpsectionModel.activity.idNumber.ToString();
            //inspection.Inspector = inpsectionModel.activity.

            inspection.Status = inpsectionModel.activity.status;
            inspection.Type = inpsectionModel.activity.activityType;
        }
        if (inpsectionModel != null)
            inspection.InspectionModel = inpsectionModel;
   */
        return inspection;
    }

    //Kale.OK
    public static Inspection[] convertInspectionWSModelsToInspections(InspectionModel[] inpsectionModels)
    {
        if (inpsectionModels == null || inpsectionModels.Length == 0)
        {
            return null;
        }

        Inspection[] inspections = new Inspection[inpsectionModels.Length];

        int index = 0;
        foreach (InspectionModel inspectionModel in inpsectionModels)
        {
            inspections[index] = convertInspectionWSModelToInspection(inspectionModel);
            index++;
        }

        return inspections;
    }

    //Kale.OK
    public static DataTable ConvertOjectListToDataTable<T>(List<T> obj, string objectType) where T : Inspection
    {
        DataTable dt = new DataTable();
        if (objectType == "Inspection")
        {
            dt.Columns.Add("Id",Type.GetType("System.String"));
            dt.Columns.Add("Inspector", Type.GetType("System.String"));
            dt.Columns.Add("Address", Type.GetType("System.String"));
            dt.Columns.Add("Comments", Type.GetType("System.String"));
            dt.Columns.Add("Date", Type.GetType("System.DateTime"));
            dt.Columns.Add("PermitNo", Type.GetType("System.String"));
            dt.Columns.Add("Status", Type.GetType("System.String"));
            dt.Columns.Add("Type", Type.GetType("System.String"));
            foreach (Inspection ist in obj)
            {
                DataRow row = dt.NewRow();
                row["Id"]=ist.Id;
                row["Inspector"]=ist.Inspector;
                row["Address"]=ist.Address;
                row["Comments"]= ist.Comments;
                row["Date"]=ist.Date;
                row["PermitNo"]=ist.PermitNo;
                row["Status"]=ist.Status;
                row["Type"] = ist.Type;
                dt.Rows.Add(row);                
            }
            dt.AcceptChanges();
        }
        return dt;
        
    }

    public static DataTable ConvertGlobalSearchResultCapListToDataTable<T>(List<T> obj, string ModuleName, out bool MultipleModules, out bool MultipleAgencies, int PreviousRecsRead) where T : CAPView4UI
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("AltId", Type.GetType("System.String"));
        dt.Columns.Add("Address", Type.GetType("System.String"));
        dt.Columns.Add("Status", Type.GetType("System.String"));
        dt.Columns.Add("Agency", Type.GetType("System.String"));
        dt.Columns.Add("Module", Type.GetType("System.String"));
        dt.Columns.Add("Date", Type.GetType("System.DateTime"));
        dt.Columns.Add("sortDate", Type.GetType("System.String"));
        dt.Columns.Add("Number", Type.GetType("System.String"));
        dt.Columns.Add("Type", Type.GetType("System.String"));
        dt.Columns.Add("capClass", Type.GetType("System.String"));
        string firstModule = "X";
        string firstAgency = "X";
        bool hasMultipleModules = false;
        bool hasMultipleAgencies = false;
        for (int i = PreviousRecsRead; i < obj.Count; i++)
        {
            if (obj[i] != null && (ModuleName == null || ModuleName == string.Empty || obj[i].ModuleName == ModuleName))
            {
                if (firstModule == "X")
                    firstModule = obj[i].ModuleName;
                else if (firstModule != obj[i].ModuleName)
                    hasMultipleModules = true;

                if (firstAgency == "X")
                    firstAgency = obj[i].AgencyCode;
                else if (firstAgency != obj[i].AgencyCode)
                    hasMultipleAgencies = true;

                DataRow row = dt.NewRow();
                row["AltId"] = obj[i].PermitNumber;
                row["Number"] = obj[i].CapID;
                row["Type"] = obj[i].PermitType;
                row["Address"] = obj[i].Address;
                row["Status"] = obj[i].Status;
                row["Module"] = obj[i].ModuleName;
                row["Agency"] = obj[i].AgencyCode;
                row["capClass"] = obj[i].CapClass;
                if (obj[i].CreatedDate != null)
                {
                    DateTime workDate;
                    row["Date"] = obj[i].CreatedDate;
                    if (DateTime.TryParse(obj[i].CreatedDate.ToString(), out workDate))
                    {
                        row["sortDate"] = workDate.ToString("yyyyMMddhhmmss"); // , (DateTime)obj[i].CreatedDate);
                    }
                }
                dt.Rows.Add(row);
            }
        }
        dt.AcceptChanges();
        MultipleModules = hasMultipleModules;
        MultipleAgencies = hasMultipleAgencies;
        return dt;
    }
    public static DataTable ConvertGlobalSearchResultApoListToDataTable<T>(List<T> obj, int PreviousRecsRead) where T : APOView4UI
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ParcelNumber", Type.GetType("System.String"));
        dt.Columns.Add("ParcelSeqNbr", Type.GetType("System.String"));
        dt.Columns.Add("OwnerName", Type.GetType("System.String"));
        dt.Columns.Add("OwnerSeqNumber", Type.GetType("System.String"));
        dt.Columns.Add("OwnerSourceNumber", Type.GetType("System.String"));
        dt.Columns.Add("AddressDescription", Type.GetType("System.String"));
        dt.Columns.Add("AddressSeqNumber", Type.GetType("System.String"));
        dt.Columns.Add("AddressSourceNumber", Type.GetType("System.String"));
        for (int i = PreviousRecsRead; i < obj.Count; i++)
        {
            if (obj[i] != null)
            {
                DataRow row = dt.NewRow();
                row["ParcelNumber"] = obj[i].ParcelNumber;
                row["ParcelSeqNbr"] = obj[i].ParcelSeqNbr;
                row["OwnerName"] = obj[i].OwnerName;
                row["OwnerSeqNumber"] = obj[i].OwnerSeqNumber;
                row["OwnerSourceNumber"] = obj[i].OwnerSourceNumber;
                row["AddressDescription"] = obj[i].AddressDescription;
                row["AddressSeqNumber"] = obj[i].AddressSeqNumber;
                row["AddressSourceNumber"] = obj[i].AddressSourceNumber;
                dt.Rows.Add(row);
            }
        }
        dt.AcceptChanges();
        return dt;
    }
    public static DataTable ConvertGlobalSearchResultLpListToDataTable<T>(List<T> obj, out bool MultipleAgencies, int PreviousRecsRead, string moduleName) where T : LPView4UI
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("AgencyCode", Type.GetType("System.String"));
        dt.Columns.Add("LicenseNumber", Type.GetType("System.String"));
        dt.Columns.Add("LicenseType", Type.GetType("System.String"));
        dt.Columns.Add("LicenseProfessionalName", Type.GetType("System.String"));
        dt.Columns.Add("BusinessName", Type.GetType("System.String"));
        //dt.Columns.Add("BusinessLicense", Type.GetType("System.String"));
        string firstAgency = "X";
        bool hasMultipleAgencies = false;
        for (int i = PreviousRecsRead; i < obj.Count; i++)
        {
            if (obj[i] != null)
            {
                if (firstAgency == "X")
                {
                    firstAgency = obj[i].AgencyCode;
                }
                else if (firstAgency != obj[i].AgencyCode)
                {
                    hasMultipleAgencies = true;
                }

                //LicenseProxy tempProxy = new LicenseProxy();
                //LicenseModel4WS licenseModel = tempProxy.GetLicenseModel4WS(obj[i].LicenseNumber.ToString(), obj[i].LicenseType.ToString(), obj[i].AgencyCode, moduleName);
                DataRow row = dt.NewRow();
                row["AgencyCode"] = obj[i].AgencyCode;
                row["LicenseNumber"] = obj[i].LicenseNumber;
                row["LicenseType"] = obj[i].LicenseType;
                row["LicenseProfessionalName"] = obj[i].LicensedProfessionalName;
                row["BusinessName"] = obj[i].BusinessName;
                /*
                if (licenseModel != null && licenseModel.businessLicense != null && licenseModel.businessLicense.Length > 0)
                {
                    row["BusinessLicense"] = licenseModel.businessLicense.ToString();
                }
                 */
                dt.Rows.Add(row);
                //if (i > 99)
                //{
                //    break;
                //}
            }
        }
        dt.AcceptChanges();
        MultipleAgencies = hasMultipleAgencies;
        return dt;
    }
}
