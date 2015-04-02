/**
 *  Accela Citizen Access
 *  File: ActionType.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: ActionType.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;

namespace Accela.ACA.PaymentAdapter
{
    /// <summary>
    /// The action type
    /// 'FromACA' means the parameters comes from ACA 
    /// 'FromThirdParty' means the parameters comes from 3rd
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// the parameter comes from ACA
        /// </summary>
        FromACA = 0,

        /// <summary>
        /// the parameter comes from 3rd web site
        /// </summary>
        FromThirdParty
    }
}
