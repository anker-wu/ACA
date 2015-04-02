#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CAPListItem.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: CAPListItem.cs 130988 2009-8-20  10:13:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;

namespace Accela.ACA.Web.Common.GlobalSearch
{
    /// <summary>
    /// CAP List Item
    /// </summary>
    [Serializable]
    public class CAPView4UI
    {
        #region Private Fields

        /// <summary>
        /// Agency code
        /// </summary>
        private string _agencyCode;

        /// <summary>
        /// The cap class
        /// </summary>
        private string _capClass;

        /// <summary>
        /// The cap id
        /// </summary>
        private string _capID;

        /// <summary>
        /// Module name
        /// </summary>
        private string _moduleName;

        /// <summary>
        /// Create date
        /// </summary>
        private DateTime? _createDate;

        /// <summary>
        /// Permit number
        /// </summary>
        private string _permitNumber;

        /// <summary>
        /// Permit type
        /// </summary>
        private string _permitType;

        /// <summary>
        /// The description
        /// </summary>
        private string _description;

        /// <summary>
        /// Project name
        /// </summary>
        private string _projectName;

        /// <summary>
        /// Rec status, A is active, I is hidden
        /// </summary>
        private string _status;

        /// <summary>
        /// address message
        /// </summary>
        private string _address;

        /// <summary>
        /// related record count
        /// </summary>
        private int _relatedRecords;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the agency code
        /// </summary>
        [FieldMapping(GviewElementId = "lnkAgencyHeader", JavaPropertyName = "servProvCode", Order = 4)]
        public string AgencyCode
        {
            get { return _agencyCode; }
            set { _agencyCode = value; }
        }

        /// <summary>
        /// Gets or sets the cap class
        /// </summary>
        public string CapClass
        {
            get { return _capClass; }
            set { _capClass = value; }
        }

        /// <summary>
        /// Gets or sets the cap id
        /// </summary>
        public string CapID
        {
            get { return _capID; }
            set { _capID = value; }
        }

        /// <summary>
        /// Gets or sets the module name
        /// </summary>
        [FieldMapping(GviewElementId = "lnkModuleHeader", JavaPropertyName = "moduleName", Order = 5)]
        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        /// <summary>
        /// Gets or sets the create date
        /// </summary>
        [FieldMapping(GviewElementId = "lnkDateHeader", JavaPropertyName = "createdDate", Order = 1)]
        public DateTime? CreatedDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        /// <summary>
        /// Gets or sets the permit number
        /// </summary>
        [FieldMapping(GviewElementId = "lnkPermitNumberHeader", JavaPropertyName = "altId", Order = 2)]
        public string PermitNumber
        {
            get { return _permitNumber; }
            set { _permitNumber = value; }
        }

        /// <summary>
        /// Gets or sets the permit type
        /// </summary>
        [FieldMapping(GviewElementId = "lnkPermitTypeHeader", JavaPropertyName = "capType", Order = 3)]
        public string PermitType
        {
            get { return _permitType; }
            set { _permitType = value; }
        }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [FieldMapping(GviewElementId = "lnkDescHeader", JavaPropertyName = "shortNotes", Order = 6)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        [FieldMapping(GviewElementId = "lnkPermitSearchProjectNameHeader", JavaPropertyName = "projectName", Order = 7)]
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        /// <summary>
        /// Gets or sets the address message
        /// </summary>
        [FieldMapping(GviewElementId = "lnkAddressHeader", JavaPropertyName = "location", Order = 8)]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets or sets the REC status, A is active, I is hidden
        /// </summary>
        [FieldMapping(GviewElementId = "lnkStatusHeader", JavaPropertyName = "capStatus", Order = 9)]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Gets or sets the REC status, A is active, I is hidden
        /// </summary>
        [FieldMapping(GviewElementId = "lnkRelatedRecordsHeader", JavaPropertyName = "relatedRecordsCount", Order = 10)]
        public int RelatedRecords
        {
            get { return _relatedRecords; }
            set { _relatedRecords = value; }
        }

        #endregion
    }
}