#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationScheduleView.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

using System;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// The examination schedule view.
    /// </summary>
    public partial class ExaminationScheduleView : PopupDialogBasePage
    {
        #region Properties
         
        /// <summary>
        /// Gets the examination number.
        /// </summary>
        /// <value>The Examination ID.</value>
        private string ExaminationID
        {
            get
            {
                string examinationNum = string.IsNullOrEmpty(Request.QueryString["ExaminationNum"]) ? string.Empty : Request.QueryString["ExaminationNum"].ToString();
                return examinationNum;
            }
        }

        #endregion Properties

        /// <summary>
        /// page load event.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetDialogMaxHeight("600");
            this.SetPageTitleKey("aca_examination_detail_title");

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                try
                {
                    IExaminationBll examBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                    string agencyCode = capModel == null
                                            ? Request.QueryString[UrlConstant.AgencyCode]
                                            : capModel.capID.serviceProviderCode;

                    if (string.IsNullOrEmpty(ExaminationID) && string.IsNullOrEmpty(agencyCode))
                    {
                        return;
                    }

                    var examinationScheduleView =
                        examBll.GetExamScheduleViewModelByExamSeqNbr(agencyCode, long.Parse(ExaminationID));

                    var examinationModel = examBll.GetExamByPK(new ExaminationPKModel()
                    {
                        examNbr = long.Parse(ExaminationID),
                        serviceProviderCode = agencyCode
                    });

                    if (examinationScheduleView != null)
                    {
                        Examination.Display(examinationScheduleView, examinationModel);
                    }
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, err);
                }
            }
        }
    }
}
