#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExpressionTypeEnum.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExpressionTypeModel.cs 194095 2011-03-29 12:17:11Z ACHIEVO\hans.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.ExpressionBuild
{
    /// <summary>
    /// Expression types
    /// </summary>
    public enum ExpressionType
    {
        /// <summary>
        /// -1: Expression Type as ASI.
        /// </summary>
        ASI = -1,

        /// <summary>
        /// -2: Expression Type as ASI_Table.
        /// </summary>
        ASI_Table = -2,

        /// <summary>
        /// 20002: Expression Type as Fee_Item.
        /// </summary>
        Fee_Item = 20002,

        /// <summary>
        /// 122: Expression Type as License_Professional.
        /// </summary>
        License_Professional = 122,

        /// <summary>
        /// 121: Expression Type as Applicant.
        /// </summary>
        Applicant = 121,

        /// <summary>
        /// 125: Expression Type as Contact 1.
        /// </summary>
        Contact_1 = 125,

        /// <summary>
        /// 123: Expression Type as Contact 2.
        /// </summary>
        Contact_2 = 123,

        /// <summary>
        /// 124: Expression Type as Contact 3.
        /// </summary>
        Contact_3 = 124,

        /// <summary>
        /// 118: Expression Type as Contacts.
        /// </summary>
        Contacts = 118,

        /// <summary>
        /// 12: Expression Type as reference contact.
        /// </summary>
        ReferenceContact = 12,

        /// <summary>
        /// Contact list generic template view id.
        /// </summary>
        Contact_TPL_Form = -20,

        /// <summary>
        /// Applicant People template view id
        /// </summary>
        APPLICANT_PEOPLE_TEMPLATE = -11,

        /// <summary>
        /// Contact 1 People template view id
        /// </summary>
        CONTACT1_PEOPLE_TEMPLATE = -16,

        /// <summary>
        /// Contact 2 People template view id
        /// </summary>
        CONTACT2_PEOPLE_TEMPLATE = -12,

        /// <summary>
        /// Contact 3 People template view id
        /// </summary>
        CONTACT3_PEOPLE_TEMPLATE = -13,

        /// <summary>
        /// Contact list People template view id
        /// </summary>
        CONTACT_PEOPLE_TEMPLATE = -7,

        /// <summary>
        /// Reference People template
        /// </summary>
        REFPEOPLE_TEMPLATE = -10,

        /// <summary>
        /// Reference contact generic template view id.
        /// </summary>
        RefContact_TPL_Form = -21,

        /// <summary>
        /// Applicant generic template view id.
        /// </summary>
        Applicant_TPL_Form = -22,

        /// <summary>
        /// Contact1 generic template view id.
        /// </summary>
        Contact1_TPL_Form = -23,

        /// <summary>
        /// Contact2 generic template view id.
        /// </summary>
        Contact2_TPL_Form = -24,

        /// <summary>
        /// Contact3 generic template view id.
        /// </summary>
        Contact3_TPL_Form = -25,

        /// <summary>
        /// Contact list generic template table view id.
        /// </summary>
        Contact_TPL_Table = -26,

        /// <summary>
        /// Reference contact list generic template table view id.
        /// </summary>
        RefContact_TPL_Table = -27,

        /// <summary>
        /// Reference contact list generic template table view id.
        /// </summary>
        Applicant_TPL_Table = -28,

        /// <summary>
        /// Contact 1 generic template table view id.
        /// </summary>
        Contact1_TPL_Table = -29,

        /// <summary>
        /// Contact 2 generic template table view id.
        /// </summary>
        Contact2_TPL_Table = -30,

        /// <summary>
        /// Contact 3 generic template table view id.
        /// </summary>
        Contact3_TPL_Table = -31,

        /// <summary>
        /// Authorized Agent people template view id.
        /// </summary>
        AuthAgent_People_Template = -32,

        /// <summary>
        /// Authorized Agent generic template view id.
        /// </summary>
        AuthAgent_TPL_Form = -33,

        /// <summary>
        /// Authorized Agent generic template table view id.
        /// </summary>
        AuthAgent_TPL_Table = -34,

        /// <summary>
        /// Authorized Agent Customer Detail page
        /// </summary>
        AuthAgent_Customer_Detail = 60154,

        /// <summary>
        /// Authorized Agent Address
        /// </summary>
        AuthAgent_Address = 60155,

        /// <summary>
        /// Address template view id.
        /// </summary>
        Address_Template = -35,

        /// <summary>
        /// Expression type as Address
        /// </summary>
        Address = 117,

        /// <summary>
        /// contact address
        /// </summary>
        Contact_Address = 30355,

        /// <summary>
        /// Reference contact address
        /// </summary>
        RefContact_Address = -30355
    }
}
