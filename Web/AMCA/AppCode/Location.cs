/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Location.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AppSession.cs 77905 2007-10-15 12:49:28Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  07-18-2008           DWB              2008 Mobile ACA interface redesign
 *                                                  Added support for columns FullAddress, FullOwnerName, and ParcelNumber.
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

/// <summary>
/// Summary description for Location
/// </summary>
public class Location
{
    private string _RefId = string.Empty;
    string _Number = string.Empty;
    string _Fraction = string.Empty;
    string _Dir = string.Empty;
    string _StreetName = string.Empty;
    string _StreetType = string.Empty;

    string _UnitType = string.Empty;
    string _UnitNo = string.Empty;
    string _City = string.Empty;
    string _State = string.Empty;
    string _Zip = string.Empty;
    string _Country = string.Empty;
    string _permitNo = string.Empty;
    string _proximity = string.Empty;
    
    // DWB - 07-19-2008 - Added for redesign project.
    string _OwnerFullName = string.Empty;
    string _ParcelNumber = string.Empty;
    // DWB - 07-23-2008 - Added for redesign project.
    string _FullAddress = string.Empty;

    public string RefId
    {
        get { return _RefId; }
        set { _RefId = value; }
    }
    public string Proximity
    {
        get { return _proximity; }
        set { _proximity = value; }
    }

    public string PermitNo
    {
        get { return _permitNo; }
        set { _permitNo = value; }
    }

    public string Number
    {
        get { return _Number; }
        set { _Number = value; }
    }

    public string Fraction
    {
        get { return _Fraction; }
        set { _Fraction = value; }
    }

    public string Dir
    {
        get { return _Dir; }
        set { _Dir = value; }
    }

    public string StreetName
    {
        get { return _StreetName; }
        set { _StreetName = value; }
    }

    public string StreetType
    {
        get { return _StreetType; }
        set { _StreetType = value; }
    }

    public string UnitType
    {
        get { return _UnitType; }
        set { _UnitType = value; }
    }
    public string UnitNo
    {
        get { return _UnitNo; }
        set { _UnitNo = value; }
    }

    public string City
    {
        get { return _City; }
        set { _City = value; }
    }

    public string State
    {
        get { return _State; }
        set { _State = value; }
    }

    public string Zip
    {
        get { return _Zip; }
        set { _Zip = value; }
    }

    public string Country
    {
        get { return _Country; }
        set { _Country = value; }
    }
    public string ParcelNumber
    {
        get { return _ParcelNumber; }
        set { _ParcelNumber = value; }
    }

    public string OwnerFullName
    {
        get { return _OwnerFullName; }
        set { _OwnerFullName = value; }
    }

    public string FullAddress
    {
        get { return _FullAddress; }
        set { _FullAddress = value; }
    }
}
