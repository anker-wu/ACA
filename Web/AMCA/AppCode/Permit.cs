/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AppSession.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2010
 * 
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AppSession.cs 77905 2007-10-15 12:49:28Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * 04/01/2009           Dave Brewster           Added property TypeAlias.
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
using Accela.ACA.WSProxy;

/// <summary>
/// Summary description for Permit
/// </summary>
public class Permit
{
    string _Number = string.Empty;
    string _Alias = string.Empty;
    string _Date = string.Empty;
    private string _EndDate = string.Empty;
    string _Type = string.Empty;
    string _TypeAlias = string.Empty;
    string _Status = string.Empty;
    string _Desc = string.Empty;
    string _Owner = string.Empty;
    string _Address = string.Empty;
    string _LicensedPro = string.Empty;
    string _Application = string.Empty;
    string _info = string.Empty;
    string _specialText = String.Empty;
    //DWB - 7.0 - 05/26/2009 - Added agency and module id to permit object.
    string _Agency = string.Empty;
    string _Module = string.Empty;
    string _capId1 = string.Empty;
    string _capId2 = string.Empty;
    string _capId3 = string.Empty;
    string _capClass = string.Empty;
    string _sortDate = string.Empty;

    AddressModel _addressModel = null;
    // DWB - 07-08-2008 - Added LicensedType to support new link on Permits.View
    string _LicensedProType = string.Empty;

    public string Application
    {
        get { return _Application; }
        set { _Application = value; }
    }

    public string LicensedPro
    {
        get { return _LicensedPro; }
        set { _LicensedPro = value; }
    }

    public string LicensedProType
    {
        get { return _LicensedProType; }
        set { _LicensedProType = value; }
    }

    public string Number
    {
        get { return _Number; }
        set { _Number = value; }
    }
    public string Alias
    {
        get { return _Alias; }
        set { _Alias = value; }
    }
    public string Date
    {
        get { return _Date; }
        set { _Date = value; }
    }

    public string EndDate
    {
        get { return _EndDate; }
        set { _EndDate = value; }
    }

    public string Type
    {
        get { return _Type; }
        set { _Type = value; }
    }

    public string TypeAlias
    {
        get { return _TypeAlias; }
        set { _TypeAlias = value; }
    }

    public string Status
    {
        get { return _Status; }
        set { _Status = value; }
    }

    public string Desc
    {
        get { return _Desc; }
        set { _Desc = value; }
    }

    public string Owner
    {
        get { return _Owner; }
        set { _Owner = value; }
    }

    public string Address
    {
        get { return _Address; }
        set { _Address = value; }
    }

    public string Info
    {
        get { return _info; }
        set { _info = value; }
    }

    public string SpecialText
    {
        get
        {
            return _specialText;
        }
        set
        {
            _specialText = value;
        }
    }

    public AddressModel AddressModel
    {
        get
        {
            return _addressModel;
        }
        set
        {
            _addressModel = value;
        }
    }

    //DWB - 7.0 - 05/26/2009 - Added agency and module id to permit object.
    public string Agency
    {
        get { return _Agency; }
        set { _Agency = value; }
    }

    public string Module
    {
        get { return _Module; }
        set { _Module = value; }
    }
    public string capId1
    {
        get { return _capId1; }
        set { _capId1 = value; }
    }
    public string capId2
    {
        get { return _capId2; }
        set { _capId2 = value; }
    }
    public string capId3
    {
        get { return _capId3; }
        set { _capId3 = value; }
    }
    public string capClass
    {
        get { return _capClass; }
        set { _capClass = value; }
    }
    public string sortDate
    {
        get { return _sortDate; }
        set { _sortDate = value; }
    }

}
