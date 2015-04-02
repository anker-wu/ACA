/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DataModelConverter.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Accela.ACA.FormDesigner.GFilterViewService;
using Accela.Controls.Designer.Data;

namespace Accela.ACA.FormDesigner
{
    public class DataModelConverter
    {
        /// <summary>
        /// method for convert SimpleViewMode4WS to FormInfo model
        /// </summary>
        /// <param name="simpleViewModel">SimpleViewModel4WS model</param>
        /// <returns>FormInfo model</returns>
        public FormInfo ConvertToFormInfo(SimpleViewModel4WS simpleViewModel)
        {
            FormInfo formInfo = null;
            if (simpleViewModel != null)
            {
                formInfo = new FormInfo();
                formInfo.GridColumnWidth = simpleViewModel.sizeUnit;
                formInfo.Height = simpleViewModel.screenHeight;
                formInfo.Width = simpleViewModel.screenWidth;
                formInfo.Width = 640;
                formInfo.MinColumnSpacing = 16;
                formInfo.GridColumnWidth = 16;
                if (string.IsNullOrEmpty(simpleViewModel.labelLayoutType) || string.Equals(simpleViewModel.labelLayoutType,  FDConstant.LABEL_DISPLAY_TOP, StringComparison.InvariantCultureIgnoreCase))
                {
                    formInfo.LabelLayoutType = LabelLayoutType.Vertical;
                }
                else
                {
                    formInfo.LabelLayoutType = LabelLayoutType.Horizontal;
                }

                formInfo.Elements = ConvertToElementInfoList(simpleViewModel.simpleViewElements.ToList<SimpleViewElementModel4WS>()).ToArray();
            }

            return formInfo;
        }

        #region Update SimpleViewModel4WS

        /// <summary>
        /// method for update simpleViewModel's elements size and position by formInfo's elements.
        /// </summary>
        /// <param name="simpleViewModel">SimpleViewModel4WS model</param>
        /// <param name="formInfo">FormInfo model</param>
        public void UpdateSimpleViewMode4WS(SimpleViewModel4WS simpleViewModel, FormInfo formInfo)
        {
            simpleViewModel.labelLayoutType = formInfo.LabelLayoutType == LabelLayoutType.Vertical ? FDConstant.LABEL_DISPLAY_TOP : FDConstant.LABEL_DISPLAY_LEFT;
            if (formInfo != null && simpleViewModel != null && simpleViewModel.simpleViewElements != null && simpleViewModel.simpleViewElements.Length>0)
            {
                foreach (var item in simpleViewModel.simpleViewElements)
                {
                    ConvertToSimpleViewElementModel4WS(item, formInfo.Elements.ToList<ElementInfo>(), simpleViewModel.labelLayoutType);
                }

                UpdateForSeparator(simpleViewModel, formInfo);     
            }
        }

        /// <summary>
        /// Update(add,remove) separator
        /// </summary>
        /// <param name="simpleViewModel">SimpleView Model </param>
        /// <param name="formInfo">Form Info</param>
        private static void UpdateForSeparator(SimpleViewModel4WS simpleViewModel, FormInfo formInfo)
        {
            List<SimpleViewElementModel4WS> list = new List<SimpleViewElementModel4WS>();
            list.AddRange(simpleViewModel.simpleViewElements);

            /// remove separator if simpleviewmodel exist but form desiger not exist.
            foreach (var item in simpleViewModel.simpleViewElements)
            {
                if (string.Equals(item.elementType, UIElementType.Line.ToString(), StringComparison.InvariantCultureIgnoreCase) && formInfo.Elements.ToList<ElementInfo>().Count(c => c.ElementId == item.viewElementId) == 0)
                {
                    list.Remove(item);
                }
            }

            ///add separator
            formInfo.Elements.ToList<ElementInfo>().ForEach(element =>
            {
                if (element.ElementType == UIElementType.Line && simpleViewModel.simpleViewElements.ToList<SimpleViewElementModel4WS>().Count(c => c.viewElementId == element.ElementId) == 0)
                {
                    SimpleViewElementModel4WS simpleviewElement = new SimpleViewElementModel4WS()
                    {
                        viewElementId = element.ElementId,
                        viewElementName = element.ElementName,
                        standard = FDConstant.COMMON_Y,
                        recStatus = FDConstant.COMMON_A,
                        elementHeight = 0,
                        elementLeft = 0,
                        elementWidth = int.Parse(element.Width.ToString()),
                        elementTop = int.Parse(element.Top.ToString()),
                        elementType = UIElementType.Line.ToString(),
                        required = FDConstant.COMMON_N,
                        displayOrder = element.ElementId
                    };

                    list.Add(simpleviewElement);
                }
            });

            simpleViewModel.simpleViewElements = list.ToArray();
        }

        /// <summary>
        /// method for update simpleViewElement's size bases elementInfos.
        /// </summary>
        /// <param name="simpleViewElementModel">SimpleViewElementModel4WS object</param>
        /// <param name="elementInfos">ElementInfo list</param>
        private void ConvertToSimpleViewElementModel4WS(SimpleViewElementModel4WS simpleViewElementModel, List<ElementInfo> elementInfos,string labelLayoutType)
        {
            if (elementInfos != null && elementInfos.Count > 0)
            {
                foreach (var item in elementInfos)
                {
                    if (item.ElementId.Equals(simpleViewElementModel.viewElementId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        simpleViewElementModel.elementTop = int.Parse(item.Top.ToString());
                        simpleViewElementModel.elementLeft = int.Parse(item.Left.ToString());
                        simpleViewElementModel.elementWidth = int.Parse(item.Width.ToString());

                        // if label on the top, reset labelwidth,inputwidth,unitwidth.
                        if (string.Equals(labelLayoutType, FDConstant.LABEL_DISPLAY_TOP, StringComparison.InvariantCultureIgnoreCase))
                        {
                            simpleViewElementModel.inputWidth = 0;
                            simpleViewElementModel.labelWidth = 0;
                            simpleViewElementModel.unitWidth = 0;
                        }
                        else
                        {
                            simpleViewElementModel.inputWidth = int.Parse(item.InputWidth.ToString());
                            simpleViewElementModel.labelWidth = int.Parse(item.TextWidth.ToString());
                            simpleViewElementModel.unitWidth = int.Parse(item.UnitWidth.ToString());
                        }

                        if (string.Equals(AcaElementType.TextArea.ToString(), simpleViewElementModel.elementType, StringComparison.InvariantCultureIgnoreCase))
                        {
                            simpleViewElementModel.elementHeight = int.Parse(item.Height.ToString());
                        }
                    }
                }
            }
        }

        #endregion

        #region Convert to elementinfo

        /// <summary>
        /// the method for convert simpleViewElements to ElementInfo list.
        /// </summary>
        /// <param name="simpleViewElements">SimpleViewElementModel4WS list</param>
        /// <returns>ElementInfo list</returns>
        private List<ElementInfo> ConvertToElementInfoList(List<SimpleViewElementModel4WS> simpleViewElements)
        {
            List<ElementInfo> elementInfos = null;
            if (simpleViewElements != null && simpleViewElements.Count > 0)
            {
                elementInfos = new List<ElementInfo>();
                foreach (var item in simpleViewElements)
                {
                    ElementInfo tmpElementInfo=ConvertToElementInfo(item);
                    elementInfos.Add(tmpElementInfo);
                }
            }

            return elementInfos;
        }

        /// <summary>
        /// the method for convert simpleViewElement to ElementInfo
        /// </summary>
        /// <param name="simpleViewElement">SimpleViewElementModel4WS object</param>
        /// <returns>ElementInfo object</returns>
        private ElementInfo ConvertToElementInfo(SimpleViewElementModel4WS simpleViewElement)
        {
            ElementInfo elementInfo = null;
            if (simpleViewElement != null)
            {
                elementInfo = new ElementInfo();
                elementInfo.IsRequired = string.Equals(FDConstant.COMMON_Y, simpleViewElement.required, StringComparison.InvariantCultureIgnoreCase) ? true : false;
                elementInfo.TextWidth = simpleViewElement.labelWidth;
                elementInfo.UnitWidth = simpleViewElement.unitWidth;
                elementInfo.InputWidth = simpleViewElement.inputWidth;
                elementInfo.ElementId = simpleViewElement.viewElementId;
                elementInfo.ElementName = FilterHtml(simpleViewElement.labelValue);
                elementInfo.ElementDescription = simpleViewElement.elementInstruction;
                elementInfo.ElementType = ConvertToUIElementType(simpleViewElement.elementType);

                if (elementInfo.ElementType == UIElementType.TextArea)
                {
                    elementInfo.Width = simpleViewElement.elementWidth;
                    elementInfo.Height = simpleViewElement.elementHeight;
                }
                else if (elementInfo.ElementType == UIElementType.Line)
                {
                    elementInfo.Width = 0;
                    elementInfo.Height = 0;
                }
                else
                {
                    elementInfo.Width = simpleViewElement.elementWidth;
                    elementInfo.Height = 0;
                }

                elementInfo.Left = simpleViewElement.elementLeft;
                elementInfo.Top = simpleViewElement.elementTop;

                //if generic template field is hidden or no, not to show.
                if (FDConstant.COMMON_H.Equals(simpleViewElement.acaDisplayFlag, StringComparison.OrdinalIgnoreCase)
                    || FDConstant.COMMON_N.Equals(simpleViewElement.acaDisplayFlag, StringComparison.OrdinalIgnoreCase))
                {
                    elementInfo.Visible = false;
                }
                else
                {
                    //if recStatus is A then this element need to show ,else if is "I" no need to show.
                    elementInfo.Visible = FDConstant.COMMON_A.Equals(simpleViewElement.recStatus, StringComparison.OrdinalIgnoreCase) ? true : false;
                }

                elementInfo.Attributes = BuildElementAttribute(elementInfo.ElementType, simpleViewElement);
            }

            return elementInfo;
        }

        /// <summary>
        /// Fitler html tag
        /// </summary>
        /// <param name="html">a string including html tags.</param>
        /// <returns>return a string without html tags.</returns>
        private string FilterHtml(string html)
        {
            if (String.IsNullOrEmpty(html))
            {
                return html;
            }

            string stringPattern = @"</?(?(?=\s)notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
            return Regex.Replace(html, stringPattern, string.Empty);
        }

        /// <summary>
        /// the method for convert elementType to UIElementType
        /// </summary>
        /// <param name="elementType">elementType string</param>
        /// <returns>UIElementType value</returns>
        private UIElementType ConvertToUIElementType(string elementType)
        {
            UIElementType uielementType=UIElementType.TextBox;

            if (String.IsNullOrEmpty(elementType))
            {
                return uielementType;
            }

            string etype = elementType.ToLower();

            switch (etype)
            {
                case "text":
                case "number":
                case "money":
                case "time":
                    uielementType = UIElementType.TextBox;
                    break;
                case "date":
                    uielementType = UIElementType.DateTimePicker;
                    break;
                case "radio":
                    uielementType = UIElementType.RadioButtons;
                    break;
                case "textarea":
                    uielementType = UIElementType.TextArea;
                    break;
                case "dropdownlist":
                case "dropdown":
                    uielementType = UIElementType.DropDownList;
                    break;
                case "checkbox":
                case "checkboxlist":
                    uielementType = UIElementType.CheckBoxes;
                    break;
                case "upload":
                    uielementType = UIElementType.Upload;
                    break;
                case "line":
                    uielementType = UIElementType.Line;
                    break;
                case "label":
                    uielementType = UIElementType.Label;
                    break;
                case "table":
                    uielementType = UIElementType.Table;
                    break;
                default:
                    uielementType = UIElementType.TextBox;
                    break;
            }

            return uielementType;
        }

        #region build attibutes

        /// <summary>
        /// the method for get ElementAttribute list for diffrent UIElement.
        /// </summary>
        /// <param name="elementType">UIElementType value</param>
        /// <returns>ElementAttribute list</returns>
        private List<ElementAttribute> BuildElementAttribute(UIElementType elementType,SimpleViewElementModel4WS simpleViewElement)
        {
            List<ElementAttribute> attributes = new List<ElementAttribute>();
            BuildAttributes(attributes, simpleViewElement);
            return attributes;
        }

        /// <summary>
        /// build all attribute
        /// </summary>
        /// <param name="elementAttribute">Attribute list</param>
        /// <param name="simpleViewElement">SimpleViewElement object</param>
        private void BuildAttributes(List<ElementAttribute> elementAttribute, SimpleViewElementModel4WS simpleViewElement)
        {
            //hidden checkbox label and set option value as label.
            if (string.Equals(AcaElementType.CheckBox.ToString(), simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_HASLABEL, FDConstant.ATTRIBUTE_HASLABEL, FDConstant.BOOL_FALSE, ElementValueType.Bool));
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_OPTIONS, FDConstant.ATTRIBUTE_OPTIONS,
                                                          FilterHtml(simpleViewElement.labelValue),
                                                          ElementValueType.String));
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_OPTIONDIRECTION, FDConstant.ATTRIBUTE_OPTIONDIRECTION, FDConstant.DIRECTION_HORIZONTAL, ElementValueType.String));
            }
            else if (string.Equals(AcaElementType.CheckBoxList.ToString(), simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_HASLABEL, FDConstant.ATTRIBUTE_HASLABEL, FDConstant.BOOL_TRUE, ElementValueType.Bool));
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_OPTIONDIRECTION, FDConstant.ATTRIBUTE_OPTIONDIRECTION, FDConstant.DIRECTION_HORIZONTAL, ElementValueType.String));
            }
            else if (string.Equals(AcaElementType.Line.ToString(), simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_HASLABEL, FDConstant.ATTRIBUTE_HASLABEL, FDConstant.BOOL_FALSE, ElementValueType.Bool));
            }
            else
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_HASLABEL, FDConstant.ATTRIBUTE_HASLABEL, FDConstant.BOOL_TRUE, ElementValueType.Bool));
            }

            if (string.IsNullOrEmpty(simpleViewElement.elementInstruction))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_SHOWHELPICON, FDConstant.ATTRIBUTE_SHOWHELPICON, FDConstant.BOOL_FALSE, ElementValueType.Bool));
            }
            else
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_SHOWHELPICON, FDConstant.ATTRIBUTE_SHOWHELPICON, FDConstant.BOOL_TRUE, ElementValueType.Bool));
            }

            if (!string.Equals(AcaElementType.CheckBox.ToString(), simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_LABEL, FDConstant.ATTRIBUTE_LABEL,
                                                          FilterHtml(simpleViewElement.labelValue),
                                                          ElementValueType.String));
            }
            else
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_LABEL, FDConstant.ATTRIBUTE_LABEL, string.Empty, ElementValueType.String));
            }

            elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_INSTRACTION, FDConstant.ATTRIBUTE_INSTRACTION, simpleViewElement.elementInstruction, ElementValueType.String));

            if (string.Equals(AcaElementType.TextArea.ToString(),simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_HEIGHTRESIZEABLE, FDConstant.ATTRIBUTE_HEIGHTRESIZEABLE, FDConstant.BOOL_TRUE, ElementValueType.Bool));
            }
            else if (string.Equals(AcaElementType.Line.ToString(), simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_HEIGHTRESIZEABLE, FDConstant.ATTRIBUTE_HEIGHTRESIZEABLE, FDConstant.BOOL_FALSE, ElementValueType.Bool));
            }
            else
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_HEIGHTRESIZEABLE, FDConstant.ATTRIBUTE_HEIGHTRESIZEABLE, FDConstant.BOOL_FALSE, ElementValueType.Bool));
            }

            if (string.Equals(AcaElementType.Line.ToString(), simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_WIDTHRESIZEABLE, FDConstant.ATTRIBUTE_WIDTHRESIZEABLE, FDConstant.BOOL_FALSE, ElementValueType.Bool));
            }
            else
            {
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_WIDTHRESIZEABLE, FDConstant.ATTRIBUTE_WIDTHRESIZEABLE, FDConstant.BOOL_TRUE, ElementValueType.Bool));
            }

            string optionStr = string.Empty;
            //checkbox option special
            if (simpleViewElement.selectOptions != null && !string.Equals(AcaElementType.CheckBox.ToString(), simpleViewElement.elementType, StringComparison.InvariantCultureIgnoreCase))
            {
                optionStr = string.Join(FDConstant.SPLIT_STRING, (from e in simpleViewElement.selectOptions
                                                                  select e.attributeValue).ToArray());
                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_OPTIONS, FDConstant.ATTRIBUTE_OPTIONS, optionStr, ElementValueType.String));

                elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_OPTIONDIRECTION, FDConstant.ATTRIBUTE_OPTIONDIRECTION, FDConstant.DIRECTION_HORIZONTAL, ElementValueType.String));
            }

            //this.BuildLabelDirection(elementAttribute, simpleViewElement.unitType);
            elementAttribute.Add(new ElementAttribute(FDConstant.ATTRIBUTE_UNIT, FDConstant.ATTRIBUTE_UNIT, simpleViewElement.unitType, ElementValueType.String));
        }
        #endregion

        #endregion
    }
}
