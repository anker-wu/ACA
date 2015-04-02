#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SimpleEMSEOnLoginResultModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SimpleCapWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// This class provides one simple cap wrapper model for custom component
    /// </summary>
    [System.SerializableAttribute]
    public class SimpleEMSEOnLoginResultModel
    {
        /// <summary>
        /// the emseOnLoginResultModel model.
        /// </summary>
        private EMSEOnLoginResultModel4WS emseOnLoginResultModel4WS;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleEMSEOnLoginResultModel"/> class.
        /// </summary>
        /// <param name="emseOnLoginResultModel4WS">The emse configuration login result model.</param>
        internal SimpleEMSEOnLoginResultModel(EMSEOnLoginResultModel4WS emseOnLoginResultModel4WS)
        {
            this.emseOnLoginResultModel4WS = emseOnLoginResultModel4WS;
        }

        /// <summary>
        /// Gets EMSE result.
        /// </summary>
        public string Result
        {
            get
            {
                return (string)emseOnLoginResultModel4WS.result;
            }
        }

        /// <summary>
        /// Gets Return code.
        /// </summary>
        public string ReturnCode
        {
            get
            {
                return emseOnLoginResultModel4WS.returnCode;
            }
        }

        /// <summary>
        /// Gets Return message
        /// </summary>
        public string ReturnMessage
        {
            get
            {
                return emseOnLoginResultModel4WS.returnMessage;
            }
        }
    }
}
