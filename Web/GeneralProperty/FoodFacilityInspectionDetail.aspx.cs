#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FoodFacilityInspectionDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FoodFacilityInspectionDetail.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// food facility inspection detail
    /// </summary>
    public partial class FoodFacilityInspectionDetail : BasePage
    {
        #region Fields

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(FoodFacilityInspectionDetail));

        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DisplayDetail();
            }
            catch (Exception ex)
            {
                HandleExecption(ex);
            }
        }
        
        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The exception object.</param>
        private void HandleExecption(Exception ex)
        {
            Logger.Error(ex);
            MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
        }

        /// <summary>
        /// Displays the detail.
        /// </summary>
        private void DisplayDetail()
        {
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    DisplayPageForAdmin();

                    string controlPerfix = foodFacilityGeneralInfo.ClientID + "_";
                    shGeneralInfo.SectionID = string.Format("{1}{0}{2}{0}{3}", ACAConstant.SPLIT_CHAR, ConfigManager.AgencyCode, GviewID.FoodFacilityDetail, controlPerfix);
                }
                else
                {
                    var licenseeModel = GetLicenseModel();
                    string recordAltID = string.IsNullOrEmpty(licenseeModel.stateLicense) ? string.Empty : licenseeModel.stateLicense;
                    var recordModel = GetRecordModelByAltID(recordAltID);
                    DisplayLicenseeInfo(licenseeModel);
                    var viewModels = recordModel == null ? null : FoodFacilityInspectionViewUtil.GetViewModels(recordModel, licenseeModel.licenseType, null);
                    var theMostInspectionViewModel = FoodFacilityInspectionViewUtil.GetTheMostInspectionViewModel(viewModels);
                    DisplayTheMostInspection(theMostInspectionViewModel);
                    inspectionPreviousItemList.DisplayPreviousInspections(viewModels);
                }
            }
        }

        /// <summary>
        /// Display a empty page in ACA admin side.
        /// </summary>
        private void DisplayPageForAdmin()
        {
            ucConditon.HideCondition();
            ucConditon.AdminDataBound();
            divGradeImagePlaceHolder.Visible = false;
        }

        /// <summary>
        /// get licensee with parameters in url.
        /// </summary>
        /// <returns>License Models</returns>
        private LicenseModel4WS GetLicenseModel()
        {
            LicenseModel4WS result = null;

            var licenseeBll = ObjectFactory.GetObject<ILicenseBLL>();

            var licenseeModel = new LicenseModel4WS();
            licenseeModel.serviceProviderCode = ConfigManager.AgencyCode;
            licenseeModel.stateLicense = Request["licenseeNumber"] == null ? string.Empty : Request["LicenseeNumber"].ToString();
            licenseeModel.licenseType = Request["licenseeType"] == null ? string.Empty : Request["LicenseeType"].ToString();

            result = licenseeBll.GetLicense(licenseeModel, false);

            return result;
        }

        /// <summary>
        /// Gets the record model by alt ID.
        /// </summary>
        /// <param name="recordAltID">The record alt ID.</param>
        /// <returns>the record mode</returns>
        private CapModel4WS GetRecordModelByAltID(string recordAltID)
        {
            var capIDModel = new CapIDModel4WS();
            capIDModel.serviceProviderCode = ConfigManager.AgencyCode;
            capIDModel.customID = recordAltID;

            var capBll = ObjectFactory.GetObject<ICapBll>();
            var result = capBll.GetCapViewByAltID(capIDModel);

            return result;
        }

        /// <summary>
        /// Display all information associated to current licensee.
        /// </summary>
        /// <param name="licenseModel">The license model.</param>
        private void DisplayLicenseeInfo(LicenseModel4WS licenseModel)
        {
            if (licenseModel != null)
            {
                StringBuilder sbProperty = new StringBuilder();
                sbProperty.Append(licenseModel.businessName);
                lblPropertyInfo.Text = sbProperty.ToString();

                DisplayConditions(licenseModel.licSeqNbr);
                foodFacilityGeneralInfo.Display(licenseModel);
            }
        }

        /// <summary>
        /// Gets and displays Licensee's condition.
        /// </summary>
        /// <param name="licenseeSeqNum">licensee sequence number</param>
        private void DisplayConditions(string licenseeSeqNum)
        {
            if (!string.IsNullOrEmpty(licenseeSeqNum))
            {
                divCondition.Visible = true;
                ucConditon.HideCondition();
                var licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                var licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(licenseeSeqNum), AppSession.User.PublicUserId);

                if (licenseModel != null && licenseModel.noticeConditions != null && licenseModel.noticeConditions.Length > 0)
                {
                    ucConditon.IsShowCondition(licenseModel.noticeConditions, licenseModel.hightestCondition, ConditionType.License);
                }
            }
        }

        /// <summary>
        /// Displays the most inspection.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        private void DisplayTheMostInspection(InspectionViewModel viewModel)
        {
            if (viewModel != null)
            {
                lblLastInspectionDateValue.Text = viewModel.ResultedDateText;
                lblScoreValue.Text = viewModel.ScoreText;
                lblGradeValue.Text = viewModel.Grade;
                DisplayGradeCardImage(viewModel.GradeImageKey, viewModel.GradeImageDescription);
            }
            else
            {
                DisplayGradeCardImage(string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Displays the grade card image.
        /// </summary>
        /// <param name="gradeImageKey">The grade image key.</param>
        /// <param name="gradeImageDescription">The grade image description.</param>
        private void DisplayGradeCardImage(string gradeImageKey, string gradeImageDescription)
        {
            bool gradeImageVisible = !string.IsNullOrEmpty(gradeImageKey) && InspectionViewUtil.HasValidResultImage(ConfigManager.AgencyCode, gradeImageKey);
            imgGradeImage.Visible = gradeImageVisible;
            divGradeImagePlaceHolder.Visible = gradeImageVisible;
            
            if (gradeImageVisible)
            {
                string imageSrcPattern = "../Cap/ImageHandler.aspx?" + UrlConstant.AgencyCode + "={0}&logoType={1}&forDownload={2}";
                string imageSrc4View = string.Format(imageSrcPattern, Server.UrlEncode(ConfigManager.AgencyCode), Server.UrlEncode(gradeImageKey), string.Empty);
                string imageSrc4Download = string.Format(imageSrcPattern, Server.UrlEncode(ConfigManager.AgencyCode), Server.UrlEncode(gradeImageKey), ACAConstant.COMMON_Y);
                string imageAlt = gradeImageDescription;
                lnkGradeImage.HRef = imageSrc4Download;
                imgGradeImage.Src = imageSrc4View;              

                var logo = ObjectFactory.GetObject<ILogoBll>();
                LogoModel logoModel = logo.GetAgencyLogoByType(UrlConstant.AgencyCode, gradeImageKey);

                if (logoModel != null && !string.IsNullOrEmpty(logoModel.docDesc))
                {
                    imageAlt = logoModel.docDesc;
                }

                lnkGradeImage.Attributes["title"] = imageAlt;
                imgGradeImage.Alt = imageAlt;
            }
        }
    }
}
