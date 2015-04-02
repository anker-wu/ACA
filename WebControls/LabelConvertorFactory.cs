#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: LabelConvertorFactory.cs
*
*  Accela, Inc.
*  Copyright (C): 2008-2014
*
*  Description:
*  LabelConvertor Factory to create the ILableConvertor object.
*
*  Notes:
*      $Id: LabelConvertorFactory.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion Header

using Accela.ACA.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// LabelConvertor Factory to create the ILabelConvertor object.
    /// </summary>
    internal class LabelConvertorFactory
    {
        #region Fields

        /// <summary>
        /// Instance LabelConvertorFactory
        /// </summary>
        internal static readonly LabelConvertorFactory Instant = new LabelConvertorFactory();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the LabelConvertorFactory class from being created
        /// </summary>
        private LabelConvertorFactory()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Create the default implement for ILabelConvert.
        /// singleton pattern implement.
        /// </summary>
        /// <returns>The instance of ILabelConvertor.</returns>
        internal ILabelConvertor GetLabelConvertor()
        {
            //if (_labelConvertor == null)
            //{
            //    lock (typeof (ILabelConvertor))
            //    {
            //        if (_labelConvertor == null)
            //        {
            //            Type type = Type.GetType(TYPE_NAME_CONTROL_LABEL_CONVERT);

            //            _labelConvertor = (ILabelConvertor)Activator.CreateInstance(type);
            //        }
            //    }
            //}
            return ObjectFactory.GetObject(typeof(ILabelConvertor)) as ILabelConvertor;
        }

        #endregion Methods
    }
}