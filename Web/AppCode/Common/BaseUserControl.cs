#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BaseUserControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: BaseUserControl.cs 278148 2014-08-28 07:30:09Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.UI;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.SSOInterface;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Base user control class
    /// </summary>
    public class BaseUserControl : UserControl
    {
        #region Properties
        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(BaseUserControl));

        /// <summary>
        /// time ticks.
        /// </summary>
        private long _timeFlag;

        /// <summary>
        /// Stopwatch instance.
        /// </summary>
        private System.Diagnostics.Stopwatch _watch = null;

        /// <summary>
        /// Gets or sets the module name from request.
        /// </summary>
        protected virtual string ModuleName
        {
            get
            {
                return ScriptFilter.AntiXssHtmlEncode(Request.QueryString[ACAConstant.MODULE_NAME]);
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the reference entity data to prevent lose the data which fields hidden by form designer..
        /// </summary>
        /// <value>The reference entity.</value>
        protected object RefEntityCache
        {
            get
            {
                return Session[SessionConstant.SESSION_REFERENCE_ENTITY_PREFIX + this.ID];
            }

            set
            {
                Session[SessionConstant.SESSION_REFERENCE_ENTITY_PREFIX + this.ID] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether needs to apply the Regional settings.
        /// Use this flag to prevent apply the regional setting twice - If already applied in <c>OnInit</c> event, do not need apply in Page_Load event.
        /// </summary>
        protected bool IsAppliedRegional { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clears the reference entity cache.
        /// </summary>
        public void ClearRefEntityCache()
        {
            if (Session[SessionConstant.SESSION_REFERENCE_ENTITY_PREFIX + this.ID] != null)
            {
                Session.Remove(SessionConstant.SESSION_REFERENCE_ENTITY_PREFIX + this.ID);
            }
        }

        /// <summary>
        /// Construct title value with image alt value with blank. 
        /// </summary>
        /// <param name="alt">the label key</param>
        /// <param name="title">the title after encode.</param>
        /// <returns>the title value</returns>
        public string GetTitleByKey(string alt, string title)
        {
            return GetTitleByKey(alt, title, true);
        }

        /// <summary>
        /// Gets the title by key.
        /// </summary>
        /// <param name="alt">The alt string.</param>
        /// <param name="title">The title string.</param>
        /// <param name="needEncode">Indicating whether the text needs to be encoded or not</param>
        /// <returns>the last title string</returns>
        public string GetTitleByKey(string alt, string title, bool needEncode)
        {
            string[] newTitle = { GetTextByKey(alt), GetTextByKey(title) };
            string rawTitle = LabelUtil.RemoveHtmlFormat(DataUtil.ConcatStringWithSplitChar(newTitle, ACAConstant.BLANK));
            string finalTitle = needEncode ? ScriptFilter.AntiXssHtmlEncode(rawTitle) : rawTitle;
            return finalTitle;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.DebugFormat("OnInit of {0}.ascx", this.GetType().BaseType.Name);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Encodes a URL string.
        /// </summary>
        /// <param name="context">Context for encode</param>
        /// <returns>a encode url</returns>
        protected string UrlEncode(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return string.Empty;
            }

            return HttpUtility.UrlEncode(context);
        }

        /// <summary>
        /// Disabled all children control.
        /// This method only handle IAccelaControl control.
        /// </summary>
        /// <param name="parent">parent control.</param>
        protected void DisableAllEdit(Control parent)
        {
            if (parent == null || parent.Controls.Count == 0)
            {
                return;
            }

            foreach (Control ctl in parent.Controls)
            {
                if (ctl is IAccelaControl)
                {
                    if (ctl is AccelaRadioButtonList)
                    {
                        AccelaRadioButtonList control = ctl as AccelaRadioButtonList;
                        control.Attributes.Remove("data-editable");
                        control.IsAlwaysEditable = false;
                        control.DisableEdit();
                    }
                    else
                    {
                        (ctl as IAccelaControl).DisableEdit();
                    }
                }

                if (ctl is AccelaAKA)
                {
                    (ctl as AccelaAKA).DisableEdit();
                }

                // if control is container control, recursive call current method
                if (ctl.Controls.Count > 0)
                {
                    DisableAllEdit(ctl);
                }
            }
        }

        /// <summary>
        /// Disabled all children control except specified in filterControlIDs array.
        /// This method only handle IAccelaControl control.
        /// </summary>
        /// <param name="parent">parent control.</param>
        /// <param name="filterControlIDs">Controls that needn't to be disabled. 
        /// if you need to disabled all children controls, you can set this parameter to null.</param>
        protected void DisableEdit(Control parent, string[] filterControlIDs)
        {
            if (parent == null ||
                parent.Controls.Count == 0)
            {
                return;
            }

            foreach (Control ctl in parent.Controls)
            {
                if (IsExistControls(ctl.ID, filterControlIDs))
                {
                    continue;
                }
               
                // filter control that needn't to be disabled
                if (ctl is IAccelaControl &&
                    !IsExistControls(ctl.ID, filterControlIDs))
                {
                    IAccelaControl control = ctl as IAccelaControl;

                    //if template field is alwaysseditalbe or standard field is not (required and empty).
                    if (!control.IsAlwaysEditable || !(control.IsRequired() && string.IsNullOrEmpty(control.GetValue())))
                    {
                        (ctl as IAccelaControl).DisableEdit();
                    }

                    continue;
                }
                else if (ctl is AccelaAKA)
                {
                    (ctl as AccelaAKA).DisableEdit();
                    continue;
                }

                // if control is container control, recursive call current method
                if (ctl.Controls.Count > 0)
                {
                    DisableEdit(ctl, filterControlIDs);
                }
            }
        }

        /// <summary>
        /// Enabled all children control except specified in filterControlIDs array.
        /// This method only handle IAccelaControl control.
        /// </summary>
        /// <param name="parent">parent control.</param>
        /// <param name="filterControlIDs">Controls that needn't to be enabled. 
        /// if you need to enabled all children controls, you can set this parameter to null.</param>
        protected void EnableEdit(Control parent, string[] filterControlIDs)
        {
            if (parent == null ||
                parent.Controls.Count == 0)
            {
                return;
            }

            foreach (Control ctl in parent.Controls)
            {
                // filter control that needn't to be disabled
                if (ctl is IAccelaControl &&
                    !IsExistControls(ctl.ID, filterControlIDs))
                {
                    (ctl as IAccelaControl).EnableEdit();
                    continue;
                }

                // if control is container control, recursive call current method
                if (ctl.Controls.Count > 0)
                {
                    EnableEdit(ctl, filterControlIDs);
                }
            }
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        protected string GetTextByKey(string key)
        {
            return LabelUtil.GetTextByKey(key, ModuleName);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        protected string GetSuperAgencyTextByKey(string key)
        {
            return LabelUtil.GetSuperAgencyTextByKey(key, ModuleName);
        }

        /// <summary>
        /// Get Dropdownlist control's selected value.Selected value is empty when the control hasn't selected value.
        /// </summary>
        /// <param name="dropDownList">AccelaDropDownList control</param>
        /// <returns>selected item text</returns>
        protected string GetDLLSelectedText(AccelaDropDownList dropDownList)
        {
            string selectedValue = string.Empty;

            if (!string.IsNullOrEmpty(dropDownList.SelectedValue))
            {
                selectedValue = dropDownList.SelectedItem.Text;
            }

            return selectedValue;
        }

        /// <summary>
        /// If refresh the current page, the Request.UrlReferrer.LocalPath will change to <c>"/" or "/default.aspx"</c>.
        /// The <c>"/" or "/default.aspx"</c> is the default patch of the start page on current Accela City Access site.
        /// </summary>
        /// <returns>Indicating whether it is refresh on current page or not.</returns>
        protected bool IsRefreshOnCurrentpage()
        {
            bool isRefresh =
                Request.UrlReferrer != null
                && (Request.UrlReferrer.LocalPath.EndsWith("/", StringComparison.InvariantCultureIgnoreCase)
                    || Request.UrlReferrer.LocalPath.EndsWith("/default.aspx", StringComparison.InvariantCultureIgnoreCase));

            return isRefresh;
        }

        /// <summary>
        /// If PostBack on current control
        /// </summary>
        /// <returns>Indicating whether it is PostBack on current control or not.</returns>
        protected bool IsPostBackOnCurrentControl()
        {
            return ControlUtil.IsPostBackOnCurrentControl(this);
        }

        /// <summary>
        /// Judge the specified control id whether is existing control array.
        /// </summary>
        /// <param name="controlID">specified control id.</param>
        /// <param name="controlIDs">control id array.</param>
        /// <returns>true if the specified control id whether is existing control array.</returns>
        private bool IsExistControls(string controlID, string[] controlIDs)
        {
            if (controlIDs == null ||
                controlIDs.Length == 0)
            {
                return false;
            }

            bool needFilter = false;

            foreach (string id in controlIDs)
            {
                if (id == controlID)
                {
                    needFilter = true;
                    break;
                }
            }

            return needFilter;
        }

        #endregion Methods
    }
}