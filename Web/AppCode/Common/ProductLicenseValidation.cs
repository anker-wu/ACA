#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProductLicenseValidation.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ProductLicenseValidation.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web;

using Accela.ACA.BLL;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Common
{
    /// <summary>
    /// This class provide the product license validation.
    /// </summary>
    public class ProductLicenseValidation
    {
        #region Fields

        /// <summary>
        /// notify the admin user if the license will expired in 30 days.
        /// </summary>
        private const int EXPIRED_DAY = 30; 

        /// <summary>
        /// the product license is expired.
        /// </summary>
        private const int PRODUCT_LICENSE_EXPIRED_CODE = 0;

        /// <summary>
        /// the product license is missing.
        /// </summary>
        private const int PRODUCT_LICENSE_MISSING_CODE = -1;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Validate the License,if false, ACA user can not use this portal
        /// </summary>
        /// <param name="isAdmin">Is admin site</param>
        public static void ValidateProductLicense(bool isAdmin)
        {
            ProductLicenseMessageInfo msgInfo = GetProductLicenseMessageInfo();
            if (msgInfo.IsRedirect)
            {
                string type = isAdmin ? "1" : "0";
                string url = "~/ProductLicenseError.aspx?type=" + type;
                HttpContext.Current.Response.Redirect(url);
            }
        }

        /// <summary>
        /// Get the product license message information.
        /// </summary>
        /// <returns>The product license message information.</returns>
        private static ProductLicenseMessageInfo GetProductLicenseMessageInfo()
        {
            ProductLicenseMessageInfo info = new ProductLicenseMessageInfo();

            if (HttpRuntime.Cache.Get(ACAConstant.PRODUCT_LICENSE_KEY) == null)
            {
                ILicensingValidationBll licensingValidationBll = ObjectFactory.GetObject<ILicensingValidationBll>();
                int remainDays = licensingValidationBll.CheckProductLicense();

                //if remainDays=0, means the product license is expired
                if (remainDays == PRODUCT_LICENSE_EXPIRED_CODE)
                {
                    SetExpiredMessage(info);
                }
                else if (remainDays == PRODUCT_LICENSE_MISSING_CODE)
                {
                    //if remainDays=-1,means the product license is not available
                    SetAvailableMessage(info);
                }
                else if (remainDays - EXPIRED_DAY <= 0)
                {
                    //the available days remains, if less than EXPIRED_DAY,show warning message
                    SetWarningMessage(info, remainDays);
                }
                else
                {
                    //unlimited product license
                    SetUnlimitMessage(info);
                }

                //set data to cache
                HttpRuntime.Cache.Insert(ACAConstant.PRODUCT_LICENSE_KEY, info);
                return info;
            }
            else
            {
                info = HttpRuntime.Cache.Get(ACAConstant.PRODUCT_LICENSE_KEY) as ProductLicenseMessageInfo;
                return info;
            }
        }

        /// <summary>
        /// Set the available message.
        /// </summary>
        /// <param name="info">The product license message information.</param>
        private static void SetAvailableMessage(ProductLicenseMessageInfo info)
        {
            //"An error has occurred.<br>The license for the Accela Citizen Access Add-on is not available."
            info.DailyMessage = LabelUtil.GetTextByKey("aca_product_license_daily_inavailable_info", string.Empty);

            //"The license for the Accela Citizen Access Add-on is not available.<br>Please contact Accela CRC to activeate the license."
            info.AdminMessage = LabelUtil.GetTextByKey("aca_product_license_admin_inavailable_info", string.Empty);
            info.IsRedirect = true;
        }

        /// <summary>
        /// Set the expired message.
        /// </summary>
        /// <param name="info">The product license message information.</param>
        private static void SetExpiredMessage(ProductLicenseMessageInfo info)
        {
            //"An error has occurred.<br>The license for the Accela Citizen Access Add-on has expired."
            info.DailyMessage = LabelUtil.GetTextByKey("aca_product_license_daily_expired_info", string.Empty);

            //"The license for the Accela Citizen Access Add-on has expired.<br>Please contact Accela CRC to activate a new license."
            info.AdminMessage = LabelUtil.GetTextByKey("aca_product_license_admin_expired_info", string.Empty);
            info.IsRedirect = true;
        }

        /// <summary>
        /// Set the unlimited message.
        /// </summary>
        /// <param name="info">The product license message information.</param>
        private static void SetUnlimitMessage(ProductLicenseMessageInfo info)
        {
            info.DailyMessage = string.Empty;
            info.AdminMessage = string.Empty;
            info.IsRedirect = false;
        }

        /// <summary>
        /// Set the warning message.
        /// </summary>
        /// <param name="info">The product license message information.</param>
        /// <param name="remainDays">The remain days.</param>
        private static void SetWarningMessage(ProductLicenseMessageInfo info, int remainDays)
        {
            //"The license for the Accela Citizen Access Add-on expires in {0} days."
            string warningMsg = LabelUtil.GetTextByKey("aca_product_license_admin_warning_info", string.Empty);
            info.AdminMessage = string.Format(warningMsg, remainDays);
            info.IsRedirect = false;
            info.RemainDays = remainDays;
        }

        #endregion Methods
    }
}
