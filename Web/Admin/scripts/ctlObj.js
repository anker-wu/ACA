/*
 * <pre>
 *  Accela Citizen Access
 *  File: ctlObj.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: ctlObj.js 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
*/

// JScript File
function TextObj(pageId, controlId, labelKey, outHTML, module, subLabel, pageFlow) {
    this.PageId = pageId;
    this.ControlId = controlId;
    this.LabelKey = labelKey;
    this.HTML = outHTML;
    this.ModuleName = module;
    this.SubLabelKey = labelKey + '|sub';
    this.SubLabel = subLabel;

    if (pageFlow) {
        this.PageFlow = pageFlow;
    }
}

function ApplicantObj(pageId, controlId, labelKey, outHTML, restrict, logoVisible, expanded)
{
    this.PageId = pageId;
    this.ControlId = controlId;
    this.LabelKey = labelKey;
    this.HTML = outHTML;
    this.Restrict = restrict;
    this.LogoVisible = logoVisible;
    this.Expanded = expanded;
}

function InputObj(pageId, controlId, labelKey, label, isTemplateField, templateAttr, subLabelKey, subLabel, watermarkKey, watermarkText, watermarkKey1, watermarkText1, watermarkKey2, watermarkText2, autoFillValue, policyName, positionID) {
    this.PageId = pageId;
    this.ControlId = controlId;
    this.LabelKey = labelKey;
    this.Label = label;
    this.IsTemplateField = isTemplateField;
    this.TemplateAttribute = templateAttr;
    this.SubLabelKey = subLabelKey;
    this.SubLabel = subLabel;
    this.WatermarkKey = watermarkKey;
    this.WatermarkText = EncodeHTMLTag(watermarkText);
    this.WatermarkKey1 = watermarkKey1;
    this.WatermarkText1 = EncodeHTMLTag(watermarkText1);
    this.WatermarkKey2 = watermarkKey2;
    this.WatermarkText2 = EncodeHTMLTag(watermarkText2);
    this.AutoFillValue = autoFillValue;
    this.PolicyName = policyName;
    this.PositionID = positionID;
}

function ButtonObj(pageId,controlId,labelKey,label,module,pageFlow,subLabelKey,subLabel){
    this.PageId = pageId;
    this.ControlId = controlId;
    this.LabelKey = labelKey;
    this.Label = label;
    this.ModuleName = module;
    this.SubLabelKey = subLabelKey;
    this.SubLabel = subLabel;
    if (pageFlow) {
        this.PageFlow = pageFlow;
    }
}

function GridViewHeadWidthObj(pageId, obj) {
    this.PageId = pageId;
    this.ControlId = obj._controlIdValue;
    this.Width = obj._getColumnWidth();
    this.GridViewId = obj._sectionIdValue;
    this.ViewElementName = obj._viewElementIdValue;
    this.Permission = GetSectionArgument(obj);
}

function ConfigureUrlButtonObj(pageId,controlId,labelKey,label,module,configureUrlId){
    this.PageId = pageId;
    this.ControlId = controlId;
    this.LabelKey = labelKey;
    this.Label = label;
    this.ModuleName = module;
    this.ConfigureUrlId = configureUrlId;
}

function FilterObj(pageId,controlId,labelKey,module,filter){
    this.PageId = pageId;
    this.controlId = controlId;
    this.LabelKey = labelKey;
    this.ModuleName = module;
    this.FilterName = filter;
}

function DropDownObj(pageId, controlId, type, labelKey, label, isTemplateField, templateAttr, subLabelKey, subLabel, stdCategoryValue, arrItems, autoFillValue, policyName, positionID, enableSearchRequired) {
    this.PageId = pageId;
    this.ControlId = controlId;
    this.Type = type;
    this.LabelKey = labelKey;
    this.Label = label;
    this.IsTemplateField = isTemplateField;
    this.TemplateAttribute = templateAttr;
    this.SubLabelKey = subLabelKey;
    this.SubLabel = subLabel;
    this.CategoryValue = stdCategoryValue;
    this.Items = arrItems;
    this.AutoFillValue = autoFillValue;
    this.PolicyName = policyName;
    this.PositionID = positionID;
    this.EnableSearchRequired = enableSearchRequired;
}

function ItemObj(labelKey,label,visible){
    this.LabelKey = labelKey;
    this.Label = label;
    this.Visible = visible;
}

function SectionObj(pageId, controlId, labelKey, label, subLabel,itms, permission) {
    this.PageId = pageId;
    this.ControlId = controlId;
    this.LabelKey = labelKey;
    this.Label = label;
    this.SubLabelKey = labelKey + '|sub';
    this.SubLabel = subLabel;
    this.SectionItems = itms;
    this.Permission = permission;
}

function TabObj(pageId,controlId,tabNames){
    this.PageId = pageId;
    this.LabelKey = '';
    this.ControlId = controlId;
    this.TabNames = tabNames;
}

function FieldObj(controlId,visible,required){
    this.ElementName = controlId;
    this.LabelKey = '';
    this.Visible = visible;     
    this.Required=required; 
}

function ReportsObj(pageId,reports) 
{
    this.PageId = pageId;
    this.reports = reports;
}

function GridViewPageSize(pageId, pageSize, obj) {
    this.PageId = pageId;
    this.PageSize = pageSize;
    this.ControlId = obj._id;
    this.LevelData = obj._sectionIdValue.split(Ext.Const.SplitChar)[0];
    this.GridViewId = obj._sectionIdValue.split(Ext.Const.SplitChar)[1];
}

function CustomComponentObj(pageId, controlId, resId, elementId, componentName, visible, path) {
    // PageId and ControlId indicate unique control. 
    this.PageId = pageId;
    this.ControlId = controlId;
    this.ResID = resId;
    this.ElementID = elementId;
    this.ComponentName = componentName;
    this.Visible = visible;
    this.Path = path;
}

function CollapseLineObj(pageId, controlId, CollapseLines) {
    this.PageId = pageId;
    this.ControlId = controlId;
    this.CollapseLines = CollapseLines;
}

function SectionEditableObj(pageId, controlId, editable) {
    this.PageId = pageId;
    this.ControlId = controlId;
    this.Editable = editable;
}

function ExtenseObj(extenseType, extenseItems) {
    this.extenseType = extenseType;
    this.extenseItems = extenseItems;
}

function ExtenseItem(extenseKey, deleteOption, updateOptions) {
    this.extensesKey = extenseKey;
    this.deleteOption = deleteOption;
    this.updateOptions = updateOptions;
}