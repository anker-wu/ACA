#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes: 
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Inspection View Model
    /// </summary>
    [Serializable]
    public class ExaminationViewModel
    {
        /// <summary>
        /// Gets or sets the examination data model.
        /// </summary>
        /// <value>The examination data model.</value>
        public ExaminationModel ExaminationDataModel
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether [contact visible].
        /// </summary>
        /// <value><c>true</c> if [contact visible]; otherwise, <c>false</c>.</value>
        public bool ContactVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of the contact.
        /// </summary>
        /// <value>The full name of the contact.</value>
        public string ContactFullName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name of the contact.
        /// </summary>
        /// <value>The first name of the contact.</value>
        public string ContactFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the contact middle.
        /// </summary>
        /// <value>The name of the contact middle.</value>
        public string ContactMiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name of the contact.
        /// </summary>
        /// <value>The last name of the contact.</value>
        public string ContactLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact full phone number (with IDD).
        /// </summary>
        /// <value>The contact full phone number (with IDD).</value>
        public string ContactFullPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact phone IDD.
        /// </summary>
        /// <value>The contact phone IDD.</value>
        public string ContactPhoneIDD
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact phone number.
        /// </summary>
        /// <value>The contact phone number.</value>
        public string ContactPhoneNumber
        {
            get;
            set;
        } 

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public int? Score
        {
            get;
            set;
        } 
    }
}
