#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Parcel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 * The parcel class shall be manage common function for parcel object on web layer
   so we create the class be beneficial to share function in diference page about parcel object.
 *
 *  Notes:
 *
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Parcel Field
    /// </summary>
    public struct ParcelField
    {
        /// <summary>
        /// Gets and sets Label Key, Label Name or Field Name
        /// </summary>
        public string Key;

        /// <summary>
        /// Gets and sets Field Value
        /// </summary>
        public string Value;

        /// <summary>
        /// Initializes a new instance of the ParcelField struct.
        /// </summary>
        /// <param name="key">The label key.</param>
        /// <param name="value">The field value.</param>
        public ParcelField(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    /// <summary>
    /// The parcel class shall be manage common function for parcel object on web layer 
    /// so we create the class be beneficial to share function in difference page about parcel object.
    /// </summary>
    public class Parcel
    {
        #region Fields

        /// <summary>
        /// cap model.
        /// </summary>
        private CapModel4WS _capModel;

        /// <summary>
        /// cap parcel model
        /// </summary>
        private CapParcelModel _capParcel;

        /// <summary>
        /// Creates an instance of ParcelField list to store parcel list
        /// </summary>
        private List<ParcelField> _parcelList = new List<ParcelField>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Parcel class.
        /// </summary>
        /// <param name="capModel">CapModel4WS object</param>
        public Parcel(CapModel4WS capModel)
        {
            _capParcel = capModel.parcelModel;
            _capModel = capModel;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets parcel list
        /// </summary>
        public List<ParcelField> ParcelList
        {
            get
            {
                if (_parcelList.Count == 0 &&
                    _capParcel != null)
                {
                    GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                    {
                        permissionLevel = GViewConstant.PERMISSION_APO,
                        permissionValue = GViewConstant.SECTION_PARCEL
                    };

                    SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(_capModel.moduleName, permission, GviewID.ParcelEdit);
                    IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
                    ParcelModel parcel = _capParcel.parcelModel;

                    if (!string.IsNullOrEmpty(_capParcel.parcelNo))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_parcelNo", ScriptFilter.FilterScript(_capParcel.parcelNo)));
                    }

                    if (!string.IsNullOrEmpty(parcel.lot) &&
                        gviewBll.IsFieldVisible(models, "txtLot"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_lot", ScriptFilter.FilterScript(parcel.lot)));
                    }

                    if (!string.IsNullOrEmpty(parcel.block) &&
                        gviewBll.IsFieldVisible(models, "txtBlock"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_block", ScriptFilter.FilterScript(parcel.block)));
                    }

                    if (!string.IsNullOrEmpty(parcel.subdivision) &&
                        gviewBll.IsFieldVisible(models, "ddlSubdivision"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_subdivision", ScriptFilter.FilterScript(I18nStringUtil.GetString(parcel.resSubdivision, parcel.subdivision))));
                    }

                    if (!string.IsNullOrEmpty(parcel.book) &&
                        gviewBll.IsFieldVisible(models, "txtBook"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_book", ScriptFilter.FilterScript(parcel.book)));
                    }

                    if (!string.IsNullOrEmpty(parcel.page) &&
                        gviewBll.IsFieldVisible(models, "txtPage"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_page", ScriptFilter.FilterScript(parcel.page)));
                    }

                    if (!string.IsNullOrEmpty(parcel.tract) &&
                        gviewBll.IsFieldVisible(models, "txtTract"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_tract", ScriptFilter.FilterScript(parcel.tract)));
                    }

                    if (!string.IsNullOrEmpty(parcel.legalDesc) &&
                        gviewBll.IsFieldVisible(models, "txtLegalDescription"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_legalDescription", ScriptFilter.FilterScript(parcel.legalDesc)));
                    }

                    if (parcel.parcelArea != null &&
                        gviewBll.IsFieldVisible(models, "txtParcelArea"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_parcelArea", ScriptFilter.FilterScript(Convert.ToString(parcel.parcelArea))));
                    }

                    if (parcel.landValue != null &&
                        gviewBll.IsFieldVisible(models, "txtLandValue"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_landValue", ScriptFilter.FilterScript(Convert.ToString(parcel.landValue))));
                    }

                    if (parcel.improvedValue != null &&
                        gviewBll.IsFieldVisible(models, "txtImprovedValue"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_improvedValue", ScriptFilter.FilterScript(Convert.ToString(parcel.improvedValue))));
                    }

                    if (parcel.exemptValue != null &&
                        gviewBll.IsFieldVisible(models, "txtExceptionValue"))
                    {
                        _parcelList.Add(new ParcelField("per_parcel_label_exceptionValue", ScriptFilter.FilterScript(Convert.ToString(parcel.exemptValue))));
                    }

                    TemplateAttributeModel[] attributes = _capParcel.parcelModel.templates;

                    if (attributes == null)
                    {
                        ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                        attributes = templateBll.GetDailyAPOTemplateAttributes(TemplateType.CAP_PARCEL, _capModel.capID, _capParcel.parcelNumber, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
                    }

                    if (attributes != null &&
                        attributes.Length > 0)
                    {
                        foreach (TemplateAttributeModel item in attributes)
                        {
                            string label = string.IsNullOrEmpty(item.attributeLabel) ? item.attributeName : item.attributeLabel;

                            if (!string.IsNullOrEmpty(label) &&
                                !string.IsNullOrEmpty(item.attributeValue) &&
                                !string.IsNullOrEmpty(item.vchFlag) &&
                                !ACAConstant.COMMON_N.Equals(item.vchFlag))
                            {
                                string valueText = ModelUIFormat.GetTemplateValue4Display(item);
                                _parcelList.Add(new ParcelField(label, ScriptFilter.FilterScript(valueText)));
                            }
                        }
                    }
                }

                return _parcelList;
            }
        }

        #endregion Properties
    }
}