#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASITWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ASITWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.UI.Model;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// Wrapper Model for the ASIT definition
    /// </summary>
    public class ASITWrapperModel
    {
        /// <summary>
        /// private field for UITable of ASIT
        /// </summary>
        private ASITUITable asitUITable;

        /// <summary>
        /// private list for columns definition of ASIT
        /// </summary>
        private List<ASITColumnWrapperModel> asitCols = new List<ASITColumnWrapperModel>();

        /// <summary>
        /// Initializes a new instance of the ASITWrapperModel class.
        /// </summary>
        /// <param name="uiTable">A derived class from UITable for ASIT.</param>
        /// <param name="originalAsitColList">WSDL model AppSpecificTableColumnModel4WS of AA service</param>
        internal ASITWrapperModel(ASITUITable uiTable, AppSpecificTableColumnModel4WS[] originalAsitColList)
        {
            asitUITable = uiTable;

            for (int i = 0; i < originalAsitColList.Length; i++)
            {
                ASITColumnWrapperModel col = new ASITColumnWrapperModel(originalAsitColList[i]);
                asitCols.Add(col);
            }
        }
        
        /// <summary>
        /// Gets ASIT Agency Code
        /// </summary>
        public string AgencyCode
        {
            get
            {
                return asitUITable.AgencyCode;
            }
        }

        /// <summary>
        /// Gets ASIT Group Name
        /// </summary>
        public string GroupName
        {
            get
            {
                return asitUITable.GroupName;
            }
        }

        /// <summary>
        /// Gets ASIT Sub Group Name
        /// </summary>
        public string SubGroupName 
        {
            get
            {
                return asitUITable.TableName;
            }
        }

        /// <summary>
        /// Gets ASIT Columns definition
        /// </summary>
        public List<ASITColumnWrapperModel> ASITCols
        {
            get
            {
                return asitCols;
            }
        }
    }
}
