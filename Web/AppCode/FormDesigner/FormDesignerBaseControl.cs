#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FormDesignerBaseControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FormDesignerBaseControl.cs 2010-04-14 09:28:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.FormDesigner
{
    /// <summary>
    /// the base class must inherit which the element need layout
    /// </summary>
    public abstract class FormDesignerBaseControl : BaseUserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormDesignerBaseControl"/> class.
        /// </summary>
        /// <param name="viewId">The view id.</param>
        public FormDesignerBaseControl(string viewId)
        {
            this.ViewId = viewId;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the view ID.
        /// </summary>
        /// <value>The view ID.</value>
        protected string ViewId { get; set; }

        /// <summary>
        /// Gets or sets Permission Model
        /// </summary>
        protected virtual GFilterScreenPermissionModel4WS Permission { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initial the form designer's placeholder properties for layout
        /// </summary>
        /// <param name="fdPlaceHolder">the placeholder</param>
        protected void InitFormDesignerPlaceHolder(AccelaFormDesignerPlaceHolder fdPlaceHolder)
        {
            InitFormDesignerPlaceHolder(fdPlaceHolder, string.Empty);
        }

        /// <summary>
        /// Initial the form designer's placeholder properties for layout
        /// </summary>
        /// <param name="fdPlaceHolder">The form designer place holder.</param>
        /// <param name="templateType">Type of the template.</param>
        protected void InitFormDesignerPlaceHolder(AccelaFormDesignerPlaceHolder fdPlaceHolder, string templateType)
        {
            fdPlaceHolder.SimpleViewModel = GetSimpleViewModel();
            fdPlaceHolder.TemplateType = templateType;
            fdPlaceHolder.IsAdmin = AppSession.IsAdmin;
            CreateControl(fdPlaceHolder);
            string templateControlID = string.Empty;

            foreach (Control ctrl in fdPlaceHolder.Controls)
            {
                // find the template userControl's ID
                // gradingstyle
                if (ctrl is TemplateEdit)
                {
                    templateControlID = ctrl.ID;
                    break;
                }
            }

            fdPlaceHolder.IdPrefix = string.Format("{0}_{1}_", this.ClientID, templateControlID);
        }

        /// <summary>
        /// Get SimpleViewModel by View id.
        /// </summary>
        /// <returns>The simple view model.</returns>
        protected SimpleViewModel4WS GetSimpleViewModel()
        {
            IGviewBll admin = ObjectFactory.GetObject<IGviewBll>();
            string callerId = AppSession.IsAdmin ? ACAConstant.ADMIN_CALLER_ID : AppSession.User.UserID;

            SimpleViewModel4WS simpleViewModel = admin.GetSimpleViewModel(ConfigManager.AgencyCode, ModuleName, ViewId, Permission.permissionLevel, Permission.permissionValue, callerId);

            simpleViewModel.simpleViewElements = admin.GetSimpleViewElementModel(ModuleName, Permission, ViewId, callerId);
            simpleViewModel.permissionModel = Permission;

            return simpleViewModel;
        }

        /// <summary>
        /// Create control by simpleViewElementModel
        /// </summary>
        /// <param name="fdPlaceHolder">AccelaFormDesigner PlaceHolder</param>
        private void CreateControl(AccelaFormDesignerPlaceHolder fdPlaceHolder)
        {
            SimpleViewModel4WS simpleViewModel = fdPlaceHolder.SimpleViewModel;
            List<SimpleViewElementModel4WS> elements = new List<SimpleViewElementModel4WS>();
            elements.AddRange(simpleViewModel.simpleViewElements);

            foreach (SimpleViewElementModel4WS item in elements)
            {
                //create separator line if control type is separator and visibled.
                if (string.Equals(ControlType.Line.ToString(), item.elementType, StringComparison.InvariantCultureIgnoreCase) && item.recStatus == ACAConstant.VALID_STATUS)
                {
                    AccelaSeparatorLine line = new AccelaSeparatorLine();
                    line.ID = item.viewElementName;
                    line.Visible = true;
                    fdPlaceHolder.Controls.Add(line);
                }
            }
        }

        #endregion Methods
    }
}