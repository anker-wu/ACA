/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: hash.cs
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
/// Hasing / encryption
/// </summary>
public class hash
{ 
	public bool bApplicationLog = false;
    private string sPublicKey = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sSetPublicKey"></param>
	public void setPublicKey(string sSetPublicKey)
	{
		sPublicKey = sSetPublicKey;
	} 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sArray"></param>
    /// <returns></returns>
	public int[] copyTointArray(string[] sArray)
	{
		int iArrayLength = sArray.Length;
		int[] aOutput = new int[iArrayLength];

		for (int x = 0; x < iArrayLength; x++)
		{
			aOutput[x] = int.Parse(sArray[x]);
		}
		return aOutput;
	} 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iValueLength"></param>
    /// <returns></returns>
	private string buildPublickey(int iValueLength)
	{
		string sKey = "";
		int iMLength = sPublicKey.Length;
		int iTemp = iValueLength / iMLength;
		for (int x = 0; x <= iTemp; x++)
		{
			sKey += sPublicKey;
		}
		return sKey;
	} 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sValue"></param>
    /// <returns></returns>
	public string decrypt(string sValue)
	{
		int[] aiValue = copyTointArray(sValue.Split(','));
		string sOutput = "";
		int iValueLength = aiValue.Length;
		string sKey = buildPublickey(iValueLength);
		for (int x = 0; x <= iValueLength - 1; x++)
		{
			sOutput += (char)(((char)aiValue[x]) ^ sKey.ToCharArray(x, 1)[0]);
		}
		return sOutput;
	} 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sValue"></param>
    /// <returns></returns>
	public string encrypt(string sValue)
	{
		int iValueLength = sValue.Length;
		string sKey = buildPublickey(iValueLength);
		string sOutput = "";
		string sDilimiter = "";

		for (int x = 0; x < iValueLength; x++)
		{
			sOutput += sDilimiter + (sValue.ToCharArray(x, 1)[0] ^ sKey.ToCharArray(x, 1)[0]);
			sDilimiter = ",";
		}
		return sOutput;
	} 
}
