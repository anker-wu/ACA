#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FileUploadPage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: PlanUploadPage.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Plan;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Brettle.Web.NeatUpload;

namespace Accela.ACA.Web.FileUpload
{
    /// <summary>
    /// the class for PlanUploadPage.
    /// </summary>
    public partial class PlanUploadPage : BasePageWithoutMaster
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether show document type.
        /// </summary>
        protected bool IsShowDocType
        {
            get
            {
                if (ViewState["IsShowDocType"] == null)
                {
                    return false;
                }

                if ((bool)ViewState["IsShowDocType"])
                {
                    hfDocType.Value = "1";
                }

                return (bool)ViewState["IsShowDocType"];
            }

            set
            {
                ViewState["IsShowDocType"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblCheckProgress.Text = GetTextByKey("ACA_FileUploadPage_Label_CheckProgress");
                string[] titles = { GetTextByKey("ACA_FileUploadPage_Label_Browse"), GetTextByKey("aca_required_field") };
                string uploadTitle = DataUtil.ConcatStringWithSplitChar(titles, ACAConstant.BLANK);
                inputFile.Attributes.Add("title", uploadTitle);
                BindRuleSet();
                if (AppSession.IsAdmin)
                {
                    inputFile.Enabled = false;
                    for (int i = 0; i < cblRuleSet.Items.Count; i++)
                    {
                        cblRuleSet.Items[i].Enabled = false;
                    }

                    progressBar.Visible = false;
                }
            }

            hfCallbackFunc.Value = string.Empty;
        }

        /// <summary>
        /// submitButton Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (GetRuleSetNum() < 1)
            {
                hfCallbackFunc.Value = string.Format("Callback('{0}','{1}','{2}');", GetTextByKey("planreview_start_noruleset_msg"), false, Request["CallbackFunc"]);
                return;
            }

            if (inputFile.HasFile)
            {
                string fileName = Path.Combine(GetTempPath(), CommonUtil.GetRandomUniqueID().Replace("-", string.Empty));
                inputFile.MoveTo(fileName, MoveToOptions.Overwrite);
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                try
                {
                    string returnMessage = string.Empty;

                    if (!VerifyFileFormat(ref returnMessage, ref fileName))
                    {
                        hfCallbackFunc.Value = string.Format("Callback('{0}','{1}','{2}');", returnMessage.Replace("'", "&#39;"), true, Request["CallbackFunc"]);
                        return;
                    }

                    PublicUserPlanModel4WS model = new PublicUserPlanModel4WS();
                    model.servProvCode = ConfigManager.AgencyCode;
                    model.planName = txbPlanName.Text;
                    model.planStatus = PlanReviewStatus.Uploaded.ToString();
                    model.publicUserID = AppSession.User.PublicUserId;
                    model.recFulName = AppSession.User.PublicUserId;
                    model.fileDisplayName = inputFile.FileName;
                    model.filePath = fileName;
                    model.ruleSet = GetAllRuleSet();
                    IPlanBll planBll = ObjectFactory.GetObject<IPlanBll>();
                    planBll.CreatePublicUserPlan(model);
                    ResetUploadForm();
                    hfCallbackFunc.Value = string.Format("Callback('{0}','{1}','{2}');", string.Empty, false, Request["CallbackFunc"]);
                }
                catch (ACAException ex)
                {
                    hfCallbackFunc.Value = string.Format("Callback('{0}','{1}','{2}');", ex.Message.Replace("'", "&#39;"), true, Request["CallbackFunc"]);
                }
            }
        }

        /// <summary>
        /// Handles the Render event of the ProgressBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ProgressBar_Render(object sender, EventArgs e)
        {
            Type progressBarType = sender.GetType();
            FieldInfo uploadProgressFi = progressBarType.GetField("uploadProgressUrl", BindingFlags.NonPublic | BindingFlags.Instance);
            if (uploadProgressFi != null)
            {
                object url = uploadProgressFi.GetValue(sender);
                progressBar.InnerHTML = string.Format(LabelUtil.GetGlobalTextByKey("iframe_nonsupport_message"), url ?? string.Empty);
            }
        }

        /// <summary>
        /// Get temporary path.
        /// </summary>
        /// <returns>string of temporary path</returns>
        private static string GetTempPath()
        {
            //string tempDir = ConfigManager.TempDirectory.ToLower();
            string tempDir = ConfigManager.TempDirectory.ToLowerInvariant();

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            return tempDir;
        }

        /// <summary>
        /// Bind rule sets.
        /// </summary>
        private void BindRuleSet()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> items = bizBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_SELF_PLAN_RULESET, false);
            cblRuleSet.Items.Clear();
            foreach (ItemValue item in items)
            {
                ListItem li = new ListItem();
                li.Text = item.Value as string;
                li.Value = item.Key;
                cblRuleSet.Items.Add(li);
            }
        }

        /// <summary>
        /// Get selected rule sets.
        /// </summary>
        /// <returns>string array of selected rule sets</returns>
        private string[] GetAllRuleSet()
        {
            int rules = GetRuleSetNum();
            string[] retu = new string[rules];
            rules = 0;

            for (int i = 0; i < cblRuleSet.Items.Count; i++)
            {
                if (cblRuleSet.Items[i].Selected)
                {
                    retu[rules] = cblRuleSet.Items[i].Value;
                    rules = rules + 1;
                }
            }

            return retu;
        }

        /// <summary>
        /// Get selected rule set count
        /// </summary>
        /// <returns>The selected count of the rule sets.</returns>
        private int GetRuleSetNum()
        {
            int rules = 0;

            for (int i = 0; i < cblRuleSet.Items.Count; i++)
            {
                if (cblRuleSet.Items[i].Selected)
                {
                    rules = rules + 1;
                }
            }

            return rules;
        }

        /// <summary>
        /// Reset upload form.
        /// </summary>
        private void ResetUploadForm()
        {
            txbPlanName.Text = string.Empty;

            for (int i = 0; i < cblRuleSet.Items.Count; i++)
            {
                cblRuleSet.Items[i].Selected = false;
            }
        }

        /// <summary>
        /// Validate IFC file format.
        /// </summary>
        /// <param name="ifcFile">IFC file name.</param>
        /// <param name="isValidateFileName">is Validate FileName</param>
        /// <returns>string for validate result</returns>
        private string ValidateIFCFile(string ifcFile, bool isValidateFileName)
        {
            string result = null;
            
            if (!isValidateFileName && ifcFile != null && ifcFile.LastIndexOf(".IFC", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                return GetTextByKey("planreview_start_invalidfile_msg");
            }

            bool isIFC = true;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(ifcFile);
                int maxLines = 100;
                int lineNum = 0;

                for (string line = reader.ReadLine(); line != null && lineNum < maxLines; line = reader.ReadLine())
                {
                    lineNum++;

                    if (line.IndexOf("FILE_SCHEMA", StringComparison.InvariantCulture) != -1 && line.IndexOf("IFC2", StringComparison.InvariantCulture) != -1)
                    {
                        isIFC = true;
                        break;
                    }

                    isIFC = false;
                }
            }
            catch (IOException)
            {
                isIFC = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            if (!isIFC)
            {
                result = GetTextByKey("planreview_start_invalidfile_msg");
            }

            return result;
        }

        /// <summary>
        /// Verify File Format.
        /// </summary>
        /// <param name="returnMessage">return Message</param>
        /// <param name="fileName">the file name</param>
        /// <returns>true or false.</returns>
        private bool VerifyFileFormat(ref string returnMessage, ref string fileName)
        {
            //if (inputFile.FileName.ToUpper().LastIndexOf(".ZIP",StringComparison.InvariantCulture) == -1 && inputFile.FileName.ToUpper().LastIndexOf(".IFC",StringComparison.InvariantCulture) == -1)
            if (inputFile.FileName.LastIndexOf(".ZIP", StringComparison.InvariantCultureIgnoreCase) == -1 && inputFile.FileName.LastIndexOf(".IFC", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                returnMessage = GetTextByKey("planreview_start_invalidfile_msg");
                return false;
            }

            fileName = Path.Combine(GetTempPath(), DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond);
            inputFile.MoveTo(fileName, MoveToOptions.Overwrite);

            //if (inputFile.FileName.ToUpper().LastIndexOf(".IFC",StringComparison.InvariantCulture) >= 0)
            if (inputFile.FileName.LastIndexOf(".IFC", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                //Validate IFC file.
                string error = ValidateIFCFile(fileName, true);
                if (error != null)
                {
                    returnMessage = error;
                    return false;
                }
            }
            else
            {
                //Validate ZIP file.
                string decompressFile = string.Empty;

                try
                {
                    decompressFile = ZipTool.UnZip(fileName, GetTempPath() + "\\");
                }
                catch (Exception ex)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(decompressFile))
                {
                    returnMessage = GetTextByKey("planreview_start_invalidfile_msg");
                    return false;
                }

                string error = ValidateIFCFile(decompressFile, false);

                try
                {
                    File.Delete(decompressFile);
                }
                catch (Exception ex)
                {
                    return false;
                }

                if (error != null)
                {
                    returnMessage = error;
                    return false;
                }
            }

            try
            {
                IUploadAttachmentBll uploadBll = ObjectFactory.GetObject<IUploadAttachmentBll>();
                fileName = uploadBll.UploadAttachment("application/octet-stream", inputFile.FileName, fileName);
            }
            catch (ACAException ex)
            {
                returnMessage = ex.ToString();

                return false;
            }

            return true;
        }

        #endregion Methods
    }
}