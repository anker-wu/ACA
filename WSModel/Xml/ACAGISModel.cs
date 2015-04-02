#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ACAGISModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 *
 *  Description:
 *  represent all event of one day in calendar
 *
 *  Notes:
 *
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    #region Enumerations

    /// <summary>
    /// GIS User Group type
    /// </summary>
    public enum GISUserGroup
    {
        /// <summary>
        /// Anonymouse User Group
        /// </summary>
        Anonymous,

        /// <summary>
        /// Registered user group
        /// </summary>
        Register,

        /// <summary>
        /// Record Creator
        /// </summary>
        RecordCreator,

        /// <summary>
        /// Contact User group
        /// </summary>
        Contact,

        /// <summary>
        /// Owner user group
        /// </summary>
        Owner,

        /// <summary>
        /// Licensed Professional user group.
        /// </summary>
        LicensedProfessional
    }

    #endregion Enumerations

    /// <summary>
    /// ACAGISModel Class
    /// </summary>
    [Serializable]
    public class ACAGISModel
    {
        #region Fields

        /// <summary>
        /// Has permission for show or not.
        /// </summary>
        private bool _hasPermission4Show = true;
        
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ACAGISModel class
        /// </summary>
        public ACAGISModel()
        {
            UserGroups = new List<string>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets AddressInfoModels, it is used by Looking up APO by Address
        /// </summary>
        public ParcelInfoModel[] AddressInfoModels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Agency
        /// </summary>
        public string Agency
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ArrayOfCapIDModels.
        /// </summary>
        public CapIDModel[] CapIDModels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets GIS Command Name
        /// </summary>
        public string CommandName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ResumeCommand label
        /// </summary>
        public string ResumeCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Context;
        /// </summary>
        public string Context
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets CreateRecordActions
        /// </summary>
        public List<KeyValue> CreateRecordActions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets GisObjects.
        /// </summary>
        public GISObjectModel[] GisObjects
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide send address command
        /// </summary>
        public bool IsHideSendAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide send featurs command.
        /// </summary>
        public bool IsHideSendFeatures
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it's MiniMap.
        /// </summary>
        public bool IsMiniMap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Language.
        /// </summary>
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ModuleKey
        /// </summary>
        public string ModuleKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ModuleName
        /// </summary>
        public string ModuleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Modules
        /// </summary>
        public List<KeyValue> Modules
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ParcelInfoModels
        /// </summary>
        public ParcelInfoModel[] ParcelInfoModels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ArrayOfParcelModel4WS
        /// </summary>
        public ParcelModel[] ParcelModels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets PostURL
        /// </summary>
        public string PostUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets RefAddressModels
        /// </summary>
        public RefAddressModel[] RefAddressModels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets SimpleCapModels
        /// </summary>
        public SimpleCapModel[] SimpleCapModels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets UserGroup
        /// </summary>
        public List<string> UserGroups
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets UserID.
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Map is plug-in or not.
        /// </summary>
        public bool Windowless
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether has permission for show or not.
        /// </summary>
        public bool HasPermission4Show
        {
            get
            {
                return _hasPermission4Show;
            }

            set
            {
                _hasPermission4Show = value;
            }
        }

        #endregion Properties
    }

    /// <summary>
    /// KeyValue Class
    /// </summary>
    [Serializable]
    public class KeyValue
    {
        #region Properties

        /// <summary>
        /// Gets or sets Key
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        #endregion Properties
    }
}