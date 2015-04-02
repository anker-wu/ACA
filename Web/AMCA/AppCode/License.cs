/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: License.cs
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
/// Summary description for License
/// </summary>
public class License
{
    string _LicenseNumber = string.Empty;
    string _LicenseType = string.Empty;
    string _FirstName = string.Empty;
    string _MiddleName = string.Empty;
    string _LastName = string.Empty;
    string _BusinessName = string.Empty;
    string _StreetAddress1 = string.Empty;
    string _StreetAddress2 = string.Empty;
    string _City = string.Empty;
    string _State = string.Empty;
    string _Country = string.Empty;
    string _Zip = string.Empty;
    string _permitNo = string.Empty;
    string _Phone1 = string.Empty;
    string _Module = string.Empty;

    public string PermitNo
    {
        get { return _permitNo; }
        set { _permitNo = value; }
    }

    public string LicenseNumber
    {
        get { return _LicenseNumber; }
        set { _LicenseNumber = value; }
    }

    public string LicenseType
    {
        get { return _LicenseType; }
        set { _LicenseType = value; }
    }

    public string FirstName
    {
        get { return _FirstName; }
        set { _FirstName = value; }
    }

    public string MiddleName
    {
        get { return _MiddleName; }
        set { _MiddleName = value; }
    }

    public string LastName
    {
        get { return _LastName; }
        set { _LastName = value; }
    }

    public string BusinessName
    {
        get { return _BusinessName; }
        set { _BusinessName = value; }
    }
    public string StreetAddress1
    {
        get { return _StreetAddress1; }
        set { _StreetAddress1 = value; }
    }

    public string StreetAddress2
    {
        get { return _StreetAddress2; }
        set { _StreetAddress2 = value; }
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

    public string Phone1
    {
        get { return _Phone1; }
        set { _Phone1 = value; }
    }

    public string ModuleName
    {
        get { return _Module; }
        set { _Module = value; }
    }
}
