/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: EducationSearchModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * </pre>
 */
using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// Education Search Model for Daily
    /// </summary>
    [Serializable]
    public class EducationSearchModel
    {
        /// <summary>
        ///  Gets or sets provider
        /// </summary>
        public ProviderModel4WS providerModel4WS
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets cap type
        /// </summary>
        public CapTypeModel capTypeModel
        {
            get;
            set;
        }
    }
}