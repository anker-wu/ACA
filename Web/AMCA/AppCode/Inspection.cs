/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Inspection.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AppSession.cs 77905 2007-10-15 12:49:28Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

/// <summary>
/// Summary description for Inspection
/// </summary>
public class Inspection
{ 
    string _Id = string.Empty;
    string _PermitNo = string.Empty;
    string _Date = string.Empty;
    string _Type = string.Empty;
    string _Status = string.Empty; 
    string _Brief = string.Empty;
    string _Address = string.Empty;
    string _Comments = string.Empty;
    string _Inspector = string.Empty;
    InspectionModel _inspectionModel = new InspectionModel();

    // DWB - 7.0 - Added new params
    bool _SameDay = false;
    bool _NextDay = false;
    string _RequiredInspection = string.Empty;
    string _ModuleName = string.Empty;
    string _NextBuisnessDay = string.Empty;
    string _ScheduleManner = string.Empty;
    string _InspectionGroup = string.Empty;
    // DWB - 7.1 added new params
    string _RequestorFirstName = string.Empty;
    string _RequestorMiddleName = string.Empty;
    string _RequestorLastName = string.Empty;
    string _RequestorPhone = string.Empty;
    string _RequestorCountry = string.Empty;


    public string Address
    {
        get { return _Address; }
        set { _Address = value; }
    }

    public string Inspector
    {
        get { return _Inspector; }
        set { _Inspector = value; }
    }

    public string Comments
    {
        get { return _Comments; }
        set { _Comments = value; }
    }

    public string Id
    {
        get { return _Id; }
        set { _Id = value; }
    }

    public string Date
    {
        get { return _Date; }
        set { _Date = value; }
    }

    public string Type
    {
        get { return _Type; }
        set { _Type = value; }
    }

    public string Status
    {
        get { return _Status; }
        set { _Status = value; }
    }

    public string PermitNo
    {
        get { return _PermitNo; }
        set { _PermitNo = value; }
    }

    public InspectionModel InspectionModel
    {
        get
        {
            return _inspectionModel;
        }
        set
        {
            _inspectionModel = value;
        }
    }

    // DWB - 7.0 - Added new params
    public bool SameDay
    {
        get { return _SameDay; }
        set { _SameDay = value; }
    }
    public bool NextDay
    {
        get { return _NextDay; }
        set { _NextDay = value; }
    }

    public string ModuleName
    {
        get { return _ModuleName; }
        set { _ModuleName = value; }
    }

    public string RequiredInspection
    {
        get { return _RequiredInspection; }
        set { _RequiredInspection = value; }
    }
    public string NextBusinessDateString4WS
    {
        get { return _NextBuisnessDay; }
        set { _NextBuisnessDay = value; }
    }

    public string InspectionGroup
    {
        get { return _InspectionGroup; }
        set { _InspectionGroup = value; }
    }

    public InspectionScheduleType ScheduleType
    {
        get;
        set;
    }

    public string RequestorFirstName
    {
        get { return _RequestorFirstName; }
        set { _RequestorFirstName = value; }
    }

     public string RequestorMiddleName
    {
        get { return _RequestorMiddleName; }
        set { _RequestorMiddleName = value; }
    }

    public string RequestorLastName
    {
        get { return _RequestorLastName; }
        set { _RequestorLastName = value; }
    }

    public string RequestorPhone
    {
        get { return _RequestorPhone; }
        set { _RequestorPhone = value; }
    }

    public string RequestorCountry
    {
        get { return _RequestorCountry; }
        set { _RequestorCountry = value; }
    }

}
