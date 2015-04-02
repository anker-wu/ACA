#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaFormDesigerPlaceHolder.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaFormDesigerPlaceHolder.cs 151830 2010-04-26 13:39:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a PlaceHolder to mix the stander/template fields
    /// </summary>
    public class AccelaFormDesignerPlaceHolder : PlaceHolder
    {
        /// <summary>
        /// The div PassPail
        /// </summary>
        private const string DIV_FASSFAIL = "divPassFail";

        /// <summary>
        /// The div PassScore
        /// </summary>
        private const string DIV_PASSSCORE = "divPassScore";

        /// <summary>
        /// The div PercentageScore
        /// </summary>
        private const string DIV_PERCENTAGESCORE = "divPercentageScore";

        /// <summary>
        /// The div score
        /// </summary>
        private const string DIV_SCORE = "divScore";

        /// <summary>
        /// The control Pass or Fail
        /// </summary>
        private const string CTL_DDLPASSFAIL = "ddlPassFail";

        /// <summary>
        /// The control txtPassScore
        /// </summary>
        private const string CTL_TXTPASSSCORE = "txtPassScore";

        /// <summary>
        /// The control txtPercentageScore
        /// </summary>
        private const string CTL_TXTPERCENTAGESCORE = "txtPercentageScore";

        /// <summary>
        /// The control txtFinalScore
        /// </summary>
        private const string CTL_TXTFINALSCORE = "txtFinalScore";

        /// <summary>
        /// The control txtGradingStyle
        /// </summary>
        private const string CTL_TXTGRADINGSTYLE = "txtGradingStyle";

        /// <summary>
        /// The grading style name
        /// </summary>
        private const string GRADINGSTYLE_NAME = "txtFinalScore_txtFinalScore";

        /// <summary>
        /// The ID suffix
        /// </summary>
        private const string ID_SUFFIX = "_parentGrid";

        /// <summary>
        /// The div contact
        /// </summary>
        private const string DIV_CONTACT = "divContact";

        /// <summary>
        /// The RadioList contact permission
        /// </summary>
        private const string RADIOLIST_CONTACT = "radioListContactPermission";

        #region Public Properties
        /// <summary>
        /// Gets or sets the SimpleViewModel that provide for layout information with controls in the container.
        /// </summary>
        public SimpleViewModel4WS SimpleViewModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the TemplateType
        /// for example: is the license type value in LP
        /// </summary>
        public string TemplateType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is admin.
        /// </summary>
        /// <value><c>true</c> if this instance is admin; otherwise, <c>false</c>.</value>
        public bool IsAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id's prefix
        /// </summary>
        public string IdPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets current component's template control id prefix.
        /// </summary>
        public string TemplateControlIDPrefix
        {
            get;
            set;
        }

        #endregion

        #region overrid methods

        /// <summary>
        /// Raise the PreRender event.
        /// </summary>
        /// <param name="e">The Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (IsAdmin)
            {
                return;
            }

            // set the control's visible when it not render to the page.
            foreach (SimpleViewElementModel4WS elementModel in SimpleViewModel.simpleViewElements)
            {
                Control control = FindControl(elementModel.viewElementName);

                if (control != null)
                {
                    control.Visible = ACAConstant.VALID_STATUS.Equals(elementModel.recStatus, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        /// <summary>
        /// override the Render
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            AdjustElementsLayout(SimpleViewModel, TemplateType, IsAdmin, TemplateControlIDPrefix, this, writer);
        }

        #endregion

        #region private mothods

        /// <summary>
        /// Adjust the Layout for elements
        /// </summary>
        /// <param name="simpleViewModel">the model provide the layout information for element</param>
        /// <param name="templateType">the template type</param>
        /// <param name="isAdmin">if in admin side</param>
        /// <param name="templateControlIDPrefix">template control id prefix.</param>
        /// <param name="placeHolder">the container that controls belong to</param>
        /// <param name="writer">the HtmlTextWriter for render the HTML to client</param>
        private static void AdjustElementsLayout(SimpleViewModel4WS simpleViewModel, string templateType, bool isAdmin, string templateControlIDPrefix, PlaceHolder placeHolder, HtmlTextWriter writer)
        {
            if (placeHolder != null)
            {
                // Get the control collection of the placeHolder
                ControlCollection elements = placeHolder.Controls;

                // Create a copy of the control collection
                Dictionary<string, Control> copyControls = new Dictionary<string, Control>();

                List<Control> outPutControls = new List<Control>();

                SeparateControlCollection(elements, copyControls, outPutControls);

                // Because the ID value coming from web service doesn't have the prefix so it need to map the old ID value and the new ID value 
                // The format is: Dictionary<OldId, NewId>
                var mappingList = simpleViewModel.simpleViewElements.Where(p => p.standard != ACAConstant.COMMON_Y);

                Dictionary<string, string> mappingID = new Dictionary<string, string>(mappingList.Count());

                MapControlIds(mappingList, mappingID, templateControlIDPrefix);
                SetLayoutType(simpleViewModel, copyControls, mappingID);
                /*
                 * Sort the configuration settings collection order by top and left location first and then groud by top location
                 * Separete to two groups, one for visible elements and the other for invisible.
                 * */
                var visibleGroup = from style in simpleViewModel.simpleViewElements
                                   where style.recStatus == "A"
                                   orderby style.elementTop ascending, style.elementLeft ascending
                                   group style by style.elementTop into newGroup
                                   orderby newGroup.Key
                                   select newGroup;

                var invisibleElements = from style in simpleViewModel.simpleViewElements
                                        where style.recStatus != "A"
                                        select style;

                // Populate the visible elements' HTML markup
                foreach (var group in visibleGroup)
                {
                    if (string.Equals(group.First().elementType, ControlType.Line.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        // when element is separator line, the separator line is not included in 960 container.
                        PopulateSeparatorLine(writer, copyControls, mappingID, group);
                    }
                    else
                    {
                        writer.WriteLine(Grid960SystemHelper.ContainerBeginTag());

                        PopulatePaddingElement(writer, copyControls, mappingID, group);

                        PopulateElements(writer, copyControls, mappingID, group);

                        PopulateEndingElement(writer, copyControls, mappingID, group);

                        // When finish adding a group, create a new line to add the other group       
                        writer.WriteLine(Grid960SystemHelper.NewLineTag());

                        writer.WriteLine(Grid960SystemHelper.EndTag());
                    }
                }

                if (isAdmin)
                {
                    // Populate the invisible elements' HTML markup
                    foreach (var element in invisibleElements)
                    {
                        element.elementLeft = 0;

                        PopulateEndingElement(writer, copyControls, mappingID, element);

                        // When finish adding a group, create a new line to add the other group       
                        writer.WriteLine(Grid960SystemHelper.NewLineTag());
                    }
                }

                // Insert the controls which do not have any customized settings such as Javacripts.
                foreach (Control control in outPutControls)
                {
                    if (control.Visible)
                    {
                        control.RenderControl(writer);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the type of the layout.
        /// </summary>
        /// <param name="simpleViewModel">The simple view model.</param>
        /// <param name="copyControls">The copy controls.</param>
        /// <param name="mappingID">The mapping ID.</param>
        private static void SetLayoutType(SimpleViewModel4WS simpleViewModel, Dictionary<string, Control> copyControls, Dictionary<string, string> mappingID)
        {
            //// set standard fields.
            ControlLayoutType layouttype = string.IsNullOrEmpty(simpleViewModel.labelLayoutType)
                                           || string.Equals(
                                               "Top",
                                               simpleViewModel.labelLayoutType,
                                               StringComparison.InvariantCultureIgnoreCase)
                ? ControlLayoutType.Vertical : ControlLayoutType.Horizontal;

            if (simpleViewModel.sectionID == GviewID.LicenseeDetail)
            {
                layouttype = string.IsNullOrEmpty(simpleViewModel.labelLayoutType)
                             || string.Equals(
                                 "Left",
                                 simpleViewModel.labelLayoutType,
                                 StringComparison.InvariantCultureIgnoreCase)
                    ? ControlLayoutType.Horizontal : ControlLayoutType.Vertical;
            }

            foreach (var item in simpleViewModel.simpleViewElements)
            {
                if (item.standard == "Y" && copyControls.ContainsKey(item.viewElementName))
                {
                    Type type = copyControls[item.viewElementName].GetType();
                    PropertyInfo pInfo = type.GetProperty("LayoutType");
                    if (pInfo != null)
                    {
                        pInfo.SetValue(copyControls[item.viewElementName], layouttype, null);
                    }
                }
                else if (string.Equals(item.viewElementName, GRADINGSTYLE_NAME))
                {
                    SetGradingStyleLayout(copyControls, layouttype);
                }
            }

            // set template fields.
            foreach (var item in mappingID)
            {
                if (copyControls.ContainsKey(item.Value))
                {
                    Type type = copyControls[item.Value].GetType();
                    PropertyInfo pInfo = type.GetProperty("LayoutType");
                    if (pInfo != null)
                    {
                        pInfo.SetValue(copyControls[item.Value], layouttype, null);
                    }
                }
            }
        }

        /// <summary>
        /// Set GradingStyle Layout.
        /// </summary>
        /// <param name="copyControls">control collection for the form. </param>
        /// <param name="layouttype">control layout type</param>
        private static void SetGradingStyleLayout(Dictionary<string, Control> copyControls, ControlLayoutType layouttype)
        {
            Control divPassFail = copyControls[DIV_FASSFAIL] as Control;
            AccelaDropDownList ddlPassFail = divPassFail.FindControl(CTL_DDLPASSFAIL) as AccelaDropDownList;
            ddlPassFail.LayoutType = layouttype;

            Control divPassScore = copyControls[DIV_PASSSCORE] as Control;
            AccelaNumberText txtPassScore = divPassScore.FindControl(CTL_TXTPASSSCORE) as AccelaNumberText;
            txtPassScore.LayoutType = layouttype;

            Control divPercentageScore = copyControls[DIV_PERCENTAGESCORE] as Control;
            AccelaNumberText txtPercentageScore = divPercentageScore.FindControl(CTL_TXTPERCENTAGESCORE) as AccelaNumberText;
            txtPercentageScore.LayoutType = layouttype;

            Control divScore = copyControls[DIV_SCORE] as Control;
            AccelaTextBox txtFinalScore = divScore.FindControl(CTL_TXTFINALSCORE) as AccelaTextBox;
            txtFinalScore.LayoutType = layouttype;

            AccelaTextBox txtGradingStyle = copyControls[CTL_TXTGRADINGSTYLE] as AccelaTextBox;
            txtGradingStyle.LayoutType = layouttype;
        }

        /// <summary>
        /// populate separator line.
        /// </summary>
        /// <param name="writer">HtmlText Writer object</param>
        /// <param name="copyControls">copyControls array</param>
        /// <param name="mappingID">element name and controlId mapping array</param>
        /// <param name="group">current row data.</param>
        private static void PopulateSeparatorLine(HtmlTextWriter writer, Dictionary<string, Control> copyControls, Dictionary<string, string> mappingID, IGrouping<int, SimpleViewElementModel4WS> group)
        {
            SimpleViewElementModel4WS el = group.First();
            string key = GetControlKey(el, mappingID);
            if (copyControls.ContainsKey(key))
            {
                string controlId = copyControls[key].ClientID;
                Control control = copyControls[key];
                control.RenderControl(writer);
            }
        }

        /// <summary>
        /// Populates the padding element.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="copyControl">The copy control.</param>
        /// <param name="mappingID">The mapping ID.</param>
        /// <param name="group">The group.</param>
        private static void PopulatePaddingElement(HtmlTextWriter writer, Dictionary<string, Control> copyControl, Dictionary<string, string> mappingID, IGrouping<int, SimpleViewElementModel4WS> group)
        {
            var firstElement = group.ElementAt(0);

            if (firstElement.elementLeft != 0)
            {
                OutputPaddingHTML(writer, firstElement);
            }
        }

        /// <summary>
        /// Outputs the padding HTML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="element">The element.</param>
        private static void OutputPaddingHTML(HtmlTextWriter writer, SimpleViewElementModel4WS element)
        {
            writer.WriteLine(Grid960SystemHelper.PaddingBeginTag(element));
            writer.WriteLine(Grid960SystemHelper.PaddingContent());
            writer.WriteLine(Grid960SystemHelper.EndTag());
        }

        /// <summary>
        /// Populates the elements.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="copyControl">The copy control.</param>
        /// <param name="mappingID">The mapping ID.</param>
        /// <param name="group">The group.</param>
        private static void PopulateElements(HtmlTextWriter writer, Dictionary<string, Control> copyControl, Dictionary<string, string> mappingID, IGrouping<int, SimpleViewElementModel4WS> group)
        {
            for (int i = 0; i < group.Count() - 1; i++)
            {
                var currentElement = group.ElementAt(i);
                var nextElement = group.ElementAt(i + 1);

                string key = GetControlKey(currentElement, mappingID);

                if (copyControl.ContainsKey(key))
                {
                    string controlId = copyControl[key].ClientID;

                    if (key == DIV_FASSFAIL)
                    {
                        OutputGradingStyleHTML(writer, currentElement, nextElement, copyControl, controlId);
                    }
                    else if (key == DIV_CONTACT)
                    {
                        OutputContactPermissionControlHTML(writer, currentElement, nextElement, copyControl, controlId);
                    }
                    else
                    {
                        OutputElementsHTML(writer, currentElement, nextElement, copyControl[key], true, controlId);
                    }
                }
            }
        }

        /// <summary>
        /// Outputs the grading style HTML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="currentElement">The current element.</param>
        /// <param name="nextElement">The next element.</param>
        /// <param name="controls">The controls.</param>
        /// <param name="controlId">The control id.</param>
        private static void OutputGradingStyleHTML(HtmlTextWriter writer, SimpleViewElementModel4WS currentElement, SimpleViewElementModel4WS nextElement, Dictionary<string, Control> controls, string controlId)
        {
            // Apply the user customized settings
            // Control Begin
            if (nextElement != null)
            {
                if (nextElement.elementLeft == currentElement.elementLeft)
                {
                    writer.WriteLine(Grid960SystemHelper.NewLineTag());

                    writer.WriteLine(Grid960SystemHelper.ElementBeginTag(currentElement, controlId));
                }
                else
                {
                    writer.WriteLine(Grid960SystemHelper.ElementBeginTag(currentElement, nextElement, controlId));
                }
            }
            else
            {
                if (!currentElement.standard.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (currentElement.elementWidth == 0)
                    {
                        writer.WriteLine(Grid960SystemHelper.NewLineTag());
                    }
                }

                writer.Write(Grid960SystemHelper.ElementBeginTag(currentElement, controlId));
            }

            Control divPassFail = controls[DIV_FASSFAIL] as Control;
            AccelaDropDownList ddlPassFail = divPassFail.FindControl(CTL_DDLPASSFAIL) as AccelaDropDownList;
            if (ddlPassFail.LayoutType == ControlLayoutType.Vertical)
            {
                ddlPassFail.Width = Grid960SystemHelper.GetWidth(currentElement, ddlPassFail);
            }
            else
            {
                Grid960SystemHelper.SetAttributes(currentElement, ddlPassFail);
            }

            ddlPassFail.Attributes.CssStyle.Add(HtmlTextWriterStyle.ZIndex, "10");
            divPassFail.RenderControl(writer);

            Control divPassScore = controls[DIV_PASSSCORE] as Control;
            AccelaNumberText txtPassScore = divPassScore.FindControl(CTL_TXTPASSSCORE) as AccelaNumberText;
            if (txtPassScore.LayoutType == ControlLayoutType.Vertical)
            {
                txtPassScore.Width = Grid960SystemHelper.GetWidth(currentElement, txtPassScore);
            }
            else
            {
                Grid960SystemHelper.SetAttributes(currentElement, txtPassScore);
            }

            txtPassScore.Attributes.CssStyle.Add(HtmlTextWriterStyle.ZIndex, "10");
            divPassScore.RenderControl(writer);

            Control divPercentageScore = controls[DIV_PERCENTAGESCORE] as Control;
            AccelaNumberText txtPercentageScore = divPercentageScore.FindControl(CTL_TXTPERCENTAGESCORE) as AccelaNumberText;
            
            if (txtPercentageScore.LayoutType == ControlLayoutType.Vertical)
            {
                txtPercentageScore.Width = Grid960SystemHelper.GetWidth(currentElement, txtPercentageScore);
            }
            else
            {
                Grid960SystemHelper.SetAttributes(currentElement, txtPercentageScore);
            }

            Grid960SystemHelper.ShowPercentage = true;           
            txtPercentageScore.Attributes.CssStyle.Add(HtmlTextWriterStyle.ZIndex, "10");
            divPercentageScore.RenderControl(writer);

            Control divScore = controls[DIV_SCORE] as Control;
            AccelaTextBox txtFinalScore = divScore.FindControl(CTL_TXTFINALSCORE) as AccelaTextBox;
            if (txtFinalScore.LayoutType == ControlLayoutType.Vertical)
            {
                txtFinalScore.Width = Grid960SystemHelper.GetWidth(currentElement, txtFinalScore);
            }
            else
            {
                Grid960SystemHelper.SetAttributes(currentElement, txtFinalScore);
            }

            txtFinalScore.Attributes.CssStyle.Add(HtmlTextWriterStyle.ZIndex, "10");
            divScore.RenderControl(writer);

            writer.Write("<div  style=\"display:none;\">");
            AccelaTextBox txtGradingStyle = controls[CTL_TXTGRADINGSTYLE] as AccelaTextBox;
            if (txtGradingStyle.LayoutType == ControlLayoutType.Vertical)
            {
                txtGradingStyle.Width = Grid960SystemHelper.GetWidth(currentElement, txtGradingStyle);
            }
            else
            {
                Grid960SystemHelper.SetAttributes(currentElement, txtGradingStyle);
            }

            txtGradingStyle.RenderControl(writer);
            writer.Write("</div>");
            
            // Control End
            writer.WriteLine(Grid960SystemHelper.EndTag());
        }

        /// <summary>
        /// Out put contact permission control
        /// </summary>
        /// <param name="writer">the html writer</param>
        /// <param name="currentElement">the current element.</param>
        /// <param name="nextElement">the next element.</param>
        /// <param name="controls">the controls.</param>
        /// <param name="controlId">the control id.</param>
        private static void OutputContactPermissionControlHTML(HtmlTextWriter writer, SimpleViewElementModel4WS currentElement, SimpleViewElementModel4WS nextElement, Dictionary<string, Control> controls, string controlId)
        {
            // Apply the user customized settings
            // Control Begin
            if (nextElement != null)
            {
                if (nextElement.elementLeft == currentElement.elementLeft)
                {
                    writer.WriteLine(Grid960SystemHelper.NewLineTag());

                    writer.WriteLine(Grid960SystemHelper.ElementBeginTag(currentElement, controlId));
                }
                else
                {
                    writer.WriteLine(Grid960SystemHelper.ElementBeginTag(currentElement, nextElement, controlId));
                }
            }
            else
            {
                if (!currentElement.standard.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (currentElement.elementWidth == 0)
                    {
                        writer.WriteLine(Grid960SystemHelper.NewLineTag());
                    }
                }

                writer.Write(Grid960SystemHelper.ElementBeginTag(currentElement, controlId));
            }

            Control divPercentageScore = controls[DIV_CONTACT] as Control;
            WebControl contactperm = divPercentageScore.FindControl(RADIOLIST_CONTACT) as WebControl;
            contactperm.Width = Grid960SystemHelper.GetWidth(currentElement, contactperm);
            contactperm.Attributes.CssStyle.Add(HtmlTextWriterStyle.ZIndex, "10");
            divPercentageScore.RenderControl(writer);

            // Control End
            writer.WriteLine(Grid960SystemHelper.EndTag());
        }

        /// <summary>
        /// Outputs the elements HTML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="currentElement">The current element.</param>
        /// <param name="nextElement">The next element.</param>
        /// <param name="control">The control.</param>
        /// <param name="needAdjustCheckBoxHeight">if set to <c>true</c> [need adjust check box height].</param>
        /// <param name="controlId">The control id.</param>
        private static void OutputElementsHTML(HtmlTextWriter writer, SimpleViewElementModel4WS currentElement, SimpleViewElementModel4WS nextElement, Control control, bool needAdjustCheckBoxHeight, string controlId)
        {
            // Check if it is textarea control
            bool isTextArea = false;

            if (!string.IsNullOrEmpty(currentElement.elementType))
            {
                if (currentElement.elementType.Equals("textarea", StringComparison.InvariantCultureIgnoreCase))
                {
                    isTextArea = true;
                }
            }

            // Apply the user customized settings
            // Control Begin
            if (nextElement != null)
            {
                if (nextElement.elementLeft == currentElement.elementLeft)
                {
                    writer.WriteLine(Grid960SystemHelper.NewLineTag());

                    writer.WriteLine(Grid960SystemHelper.ElementBeginTag(currentElement, controlId));
                }
                else
                {
                    writer.WriteLine(Grid960SystemHelper.ElementBeginTag(currentElement, nextElement, controlId));
                }
            }
            else
            {
                if (!currentElement.standard.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (currentElement.elementWidth == 0)
                    {
                        writer.WriteLine(Grid960SystemHelper.NewLineTag());
                    }
                }

                writer.Write(Grid960SystemHelper.ElementBeginTag(currentElement, controlId));
            }

            if (control is WebControl)
            {
                WebControl webControl = (WebControl)control;

                //control's layout type is horizontal, set labelwith, inputwidth and unitwidth.
                if (webControl is IAccelaControl && ((IAccelaControl)webControl).LayoutType == ControlLayoutType.Horizontal)
                {
                    Grid960SystemHelper.SetAttributes(currentElement, webControl);
                }
                else if (webControl is AccelaNameValueLabel &&
                         ((AccelaNameValueLabel)webControl).LayoutType == ControlLayoutType.Horizontal)
                {
                    Grid960SystemHelper.SetNameValueAttributes(currentElement, webControl);
                }
                else
                {
                    webControl.Width = Grid960SystemHelper.GetWidth(currentElement, webControl);
                }

                if (isTextArea || string.Equals(currentElement.elementType, ControlType.Table.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    webControl.Height = Grid960SystemHelper.GetHeight(currentElement, webControl);
                }

                webControl.Attributes.CssStyle.Add(HtmlTextWriterStyle.ZIndex, "10");
            }

            // Render the control
            control.RenderControl(writer);

            // Control End
            writer.WriteLine(Grid960SystemHelper.EndTag());
        }

        /// <summary>
        /// Populates the ending element.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="copyControls">The copy controls.</param>
        /// <param name="mappingID">The mapping ID.</param>
        /// <param name="group">The group.</param>
        private static void PopulateEndingElement(HtmlTextWriter writer, Dictionary<string, Control> copyControls, Dictionary<string, string> mappingID, IGrouping<int, SimpleViewElementModel4WS> group)
        {
            var endingElement = group.ElementAt(group.Count() - 1);

            string key = GetControlKey(endingElement, mappingID);

            if (copyControls.ContainsKey(key))
            {
                string controlId = copyControls[key].ClientID;

                if (key == DIV_FASSFAIL)
                {
                    OutputGradingStyleHTML(writer, endingElement, null, copyControls, controlId);
                }
                else if (key == DIV_CONTACT)
                {
                    OutputContactPermissionControlHTML(writer, endingElement, null, copyControls, controlId);
                }
                else
                {
                    OutputElementsHTML(writer, endingElement, null, copyControls[key], group.Count() > 1 ? true : false, controlId);
                }
            }
        }

        /// <summary>
        /// Populates the ending element.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="copyControls">The copy controls.</param>
        /// <param name="mappingID">The mapping ID.</param>
        /// <param name="element">The element.</param>
        private static void PopulateEndingElement(HtmlTextWriter writer, Dictionary<string, Control> copyControls, Dictionary<string, string> mappingID, SimpleViewElementModel4WS element)
        {
            string key = GetControlKey(element, mappingID);

            if (copyControls.ContainsKey(key))
            {
                string controlId = copyControls[key].ClientID;

                if (key == DIV_FASSFAIL)
                {
                    OutputGradingStyleHTML(writer, element, null, copyControls, controlId);
                }
                else if (key == DIV_CONTACT)
                {
                    OutputContactPermissionControlHTML(writer, element, null, copyControls, controlId);
                }
                else
                {
                    OutputElementsHTML(writer, element, null, copyControls[key], false, controlId);
                }
            }
        }

        /// <summary>
        /// Get the control id of a specific control
        /// </summary>
        /// <param name="element">The specific control</param>
        /// <param name="mappingID">Template filed ID mapping table</param>
        /// <returns>control key field</returns>
        private static string GetControlKey(SimpleViewElementModel4WS element, Dictionary<string, string> mappingID)
        {
            string key = string.Empty;
            
            if (element.standard.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase))
            {
                if (element.viewElementName == GRADINGSTYLE_NAME)
                {
                    key = DIV_FASSFAIL;
                }
                else if (element.viewElementName == "radioListContactPermission")
                {
                    key = DIV_CONTACT;
                }
                else
                {
                    key = element.viewElementName;
                }                
            }
            else
            {
                key = mappingID[element.viewElementName];
            }

            return key;
        }

        /// <summary>
        /// Separate the control collection to two different types of control collections. One is web control and the other is literal control.
        /// </summary>
        /// <param name="cotrolCollection">The collection which will be Separate.</param>
        /// <param name="webControls">The web control collection.</param>
        /// <param name="outPutControls">The Controls collection need output.</param>
        private static void SeparateControlCollection(ControlCollection cotrolCollection, Dictionary<string, Control> webControls, List<Control> outPutControls)
        {
            foreach (Control control in cotrolCollection)
            {
                if (control is Literal || control is RadioButtonListRequiredFieldValidator || control is AjaxControlToolkit.ValidatorCallbackExtender)
                {
                    outPutControls.Add(control);
                }
                else if (control is UserControl)
                {
                    foreach (Control userControl in control.Controls)
                    {
                        if (string.IsNullOrEmpty(userControl.ID))
                        {
                            continue;
                        }

                        if (userControl is RadioButtonListRequiredFieldValidator || userControl is AjaxControlToolkit.ValidatorCallbackExtender)
                        {
                            outPutControls.Add(userControl);
                        }
                        else
                        {
                            webControls.Add(userControl.ID, userControl);
                        }
                    }
                }
                else
                {
                    webControls.Add(control.ID, control);
                }
            }
        }

        /// <summary>
        /// Map the database's template filed ID to the page control ID.
        /// </summary>
        /// <param name="idList">The database's filed ID list.</param>
        /// <param name="mapList">The mapped ID list.</param>
        /// <param name="controlIDPrefix">The control unique identifier prefix.</param>
        private static void MapControlIds(IEnumerable<SimpleViewElementModel4WS> idList, Dictionary<string, string> mapList, string controlIDPrefix)
        {
            for (int i = 0; i < idList.Count(); i++)
            {
                var element = idList.ElementAt(i);
                string tempId = string.Empty;

                if (element.viewElementName.Contains("::"))
                {
                    tempId = TemplateUtil.GetTemplateControlID(element.viewElementName, string.Empty);
                }
                else
                {
                    tempId = TemplateUtil.GetTemplateControlID(element.viewElementName, controlIDPrefix);
                }

                mapList.Add(element.viewElementName, tempId);
            }
        }
        #endregion

        #region Nested Types
        /// <summary>
        /// Grid960 System Helper class
        /// </summary>
        private static class Grid960SystemHelper
        {
            #region Fields

            /// <summary>
            /// The grid960 default field width
            /// </summary>
            private const int GRID960_DEFAULT_FIELD_WIDTH = 208;

            /// <summary>
            /// The grid960 width default unit
            /// </summary>
            private const int GRID960_WIDTH_DEFAULT_UNIT = 10;

            /// <summary>
            /// The grid960 full width
            /// </summary>
            private const int GRID960_FULL_WIDTH = 640;

            /// <summary>
            /// The grid960 icon percentage
            /// </summary>
            private const double GRID960_ICON_PERCENTAGE = 4.5;

            /// <summary>
            /// The grid960 icon percentage smaller
            /// </summary>
            private const double GRID960_ICON_PERCENTAGE_SMALLER = 5.5;

            /// <summary>
            /// The grid960 icon percentage small
            /// </summary>
            private const double GRID960_ICON_PERCENTAGE_SMALL = 6;

            /// <summary>
            /// The grid960 icon percentage normal
            /// </summary>
            private const double GRID960_ICON_PERCENTAGE_NORMAL = 6.5;

            /// <summary>
            /// The grid960 icon percentage large
            /// </summary>
            private const double GRID960_ICON_PERCENTAGE_LARGE = 7;

            /// <summary>
            /// The grid960 icon percentage larger
            /// </summary>
            private const double GRID960_ICON_PERCENTAGE_LARGER = 7.5;

            /// <summary>
            /// The grid960 search icon percentage
            /// </summary>
            private const double GRID960_SEARCH_ICON_PERCENTAGE = 4.5;

            /// <summary>
            /// The grid960 search icon large correction
            /// </summary>
            private const double GRID960_SEARCH_ICON_LARGE_CORRECTION = 0.3;

            /// <summary>
            /// The grid960 search icon small correction
            /// </summary>
            private const double GRID960_SEARCH_ICON_SMALL_CORRECTION = 0.5;

            /// <summary>
            /// The grid960 configuration hundred percentage
            /// </summary>
            private const int GRID960_ONE_HUNDRED_PERCENTAGE = 100;

            /// <summary>
            /// The grid960 EM unit
            /// </summary>
            private const double GRID960_EM_UNIT = 10;

            /// <summary>
            /// The grid960 EM correction
            /// </summary>
            private const double GRID960_EM_CORRECTION = 0.5;

            /// <summary>
            /// The grid960 phone country number symbol width
            /// </summary>
            private const double GRID960_PHONE_COUNTRY_NUMBER_SYMBOL_WIDTH = 1.5;

            /// <summary>
            /// The grid960 phone country number text width
            /// </summary>
            private const double GRID960_PHONE_COUNTRY_NUMBER_TEXT_WIDTH = 4;

            /// <summary>
            /// The grid960 phone gutter
            /// </summary>
            private const double GRID960_PHONE_GUTTER = 1.8;

            /// <summary>
            /// The grid960 time selection DDL width
            /// </summary>
            private const double GRID960_TIME_SELECTION_DDL_WIDTH = 5.3;

            /// <summary>
            /// The grid960 default prefix width
            /// </summary>
            private const int GRID960_DEFAULT_PREFIX_WIDTH = 1;

            /// <summary>
            /// The grid960 input default percentage
            /// </summary>
            private const int GRID960_INPUT_DEFAULT_PERCENTAGE = 500;

            /// <summary>
            /// The grid960 text area correction
            /// </summary>
            private const int GRID960_TEXTAREA_CORRECTION = 40;

            /// <summary>
            /// The grid960 textbox larger correction
            /// </summary>
            private const int GRID960_TEXTBOX_LARGER_CORRECTION = 600;

            /// <summary>
            /// The grid960 textbox small correction
            /// </summary>
            private const int GRID960_TEXTBOX_SMALL_CORRECTION = 700;

            /// <summary>
            /// The grid960 textbox default height
            /// </summary>
            private const double GRID960_TEXTBOX_DEFAULT_HEIGHT = 1.2;

            /// <summary>
            /// The grid960 text area default height
            /// </summary>
            private const double GRID960_TEXTAREA_DEFAULT_HEIGHT = 7.6;

            /// <summary>
            /// The grid960 dropdownlist default height
            /// </summary>
            private const double GRID960_DROPDOWNLIST_DEFAULT_HEIGHT = 1.9;

            /// <summary>
            /// The grid960 table default height
            /// </summary>
            private const double GRID960_TABLE_DEFAULT_HEIGHT = 6.2;

            /// <summary>
            /// The label width rate without unit
            /// </summary>
            private const double LABEL_WIDTH_RATE_WITHOUT_UNIT = 0.5;

            /// <summary>
            /// The label width rate with unit
            /// </summary>
            private const double LABEL_WIDTH_RATE_WITH_UNIT = 0.4;

            /// <summary>
            /// The input width rate without unit
            /// </summary>
            private const double INPUT_WIDTH_RATE_WITHOUT_UNIT = 0.5;

            /// <summary>
            /// The input width rate with unit
            /// </summary>
            private const double INPUT_WIDTH_RATE_WITH_UNIT = 0.4;

            /// <summary>
            /// The unit width rate
            /// </summary>
            private const double UNIT_WIDTH_RATE = 0.2;

            /// <summary>
            /// The country width
            /// </summary>
            private const int COUNTRY_WIDTH = 55;

            /// <summary>
            /// The calendar image width
            /// </summary>
            private const int CANLENDEAR_IMAGE_WIDTH = 16;

            /// <summary>
            /// The label EM to PX rate
            /// </summary>
            private const int LABEL_EM_PX_RATE = 10;

            /// <summary>
            /// The unit EM to PX rate
            /// </summary>
            private const int UNIT_EM_PX_RATE = 10;

            /// <summary>
            /// The phone correction
            /// </summary>
            private const int PHONE_CORRECTION = 14;

            /// <summary>
            /// The text image width
            /// </summary>
            private const int TEXT_IMAGE_WIDTH = 16;

            /// <summary>
            /// The name value control EM to PX rate
            /// </summary>
            private const int NAMEVALUE_EM_PX_RATE = 10;

            /// <summary>
            /// The checkbox help icon width
            /// </summary>
            private const int CHECKBOX_HELP_ICON_WIDTH = 16;

            /// <summary>
            /// The checkbox EM to PX rate
            /// </summary>
            private const int CHECKBOX_EM_PX_RATE = 10;

            /// <summary>
            /// The time AM/PM width
            /// </summary>
            private const int TIME_AMPM_WIDTH = 49;

            /// <summary>
            /// The CSS class container
            /// </summary>
            private const string CSS_CLASS_CONTAINER = "container_";

            /// <summary>
            /// The CSS class grid
            /// </summary>
            private const string CSS_CLASS_GRID = "grid_";

            /// <summary>
            /// The CSS class suffix
            /// </summary>
            private const string CSS_CLASS_SUFFIX = "suffix_";

            /// <summary>
            /// The CSS format only class
            /// </summary>
            private const string CSS_FORMAT_ONLY_CLASS = "<div class=\"{0}\">";

            /// <summary>
            /// The CSS format ID and class
            /// </summary>
            private const string CSS_FORMAT_ID_AND_CLASS = "<div id=\"{0}" + ID_SUFFIX + "\" class=\"{1}\">";

            /// <summary>
            /// The CSS format class and style
            /// </summary>
            private const string CSS_FORMAT_CLASS_AND_STYLE = "<div id=\"{0}" + ID_SUFFIX + "\" class=\"{1}\" style = \"{2}\">";

            /// <summary>
            /// The CSS format new line
            /// </summary>
            private const string CSS_FORMAT_NEW_LINE = "<div class=\"{0}\"></div>";

            /// <summary>
            /// The CSS attribute clear:
            /// </summary>
            private const string CSS_DIV_CLEAR = "clear";

            /// <summary>
            /// The CSS div end
            /// </summary>
            private const string CSS_DIV_END = "</div>";

            /// <summary>
            /// The CSS div hidden
            /// </summary>
            private const string CSS_DIV_HIDDEN = "display: none; ";

            #endregion Fields

            #region Constructors

            /// <summary>
            /// Initializes static members of the <see cref="Grid960SystemHelper"/> class.
            /// </summary>
            static Grid960SystemHelper()
            {
                Column = 40;
                Grid = 16;
                Prefix = 1;
                Gutter = 0;
            }

            #endregion Constructors

            #region Properties

            /// <summary>
            /// Gets or sets a value indicating whether [show percentage].
            /// </summary>
            /// <value><c>true</c> if [show percentage]; otherwise, <c>false</c>.</value>
            public static bool ShowPercentage { get; set; }

            /// <summary>
            /// Gets or sets the amount of column on 960 grid system.
            /// </summary>
            public static int Column
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the width of grid on 960 grid system.
            /// </summary>
            public static int Grid
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the width of prefix on 960 grid system.
            /// </summary>
            public static int Prefix
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the width of gutter on 960 grid system.
            /// </summary>
            public static int Gutter
            {
                get;
                set;
            }

            /// <summary>
            /// Gets Grid width
            /// </summary>
            /// <value>The width of the grid.</value>
            public static int GridWidth
            {
                get
                {
                    return Grid + Gutter;
                }
            }

            #endregion Properties

            #region Methods
            /// <summary>
            /// Gets the opening tag of the container.
            /// </summary>
            /// <returns>A specific HTML markup string</returns>
            public static string ContainerBeginTag()
            {
                return string.Format(CSS_FORMAT_ONLY_CLASS, GetContainerCss());
            }

            /// <summary>
            /// Convertor pixel to EM.
            /// </summary>
            /// <param name="pixel">a length of pixel</param>
            /// <returns>return EM unit.</returns>
            public static Unit ConvertToEm(double pixel)
            {
                return new Unit(string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0}em", pixel / GRID960_EM_UNIT), CultureInfo.InvariantCulture);
            }

            /// <summary>
            /// Gets the opening tag of the specified markup element.
            /// </summary>
            /// <param name="element">The specified element</param>
            /// <param name="controlId">The control unique identifier.</param>
            /// <returns>A specific HTML markup string</returns>
            public static string ElementBeginTag(SimpleViewElementModel4WS element, string controlId)
            {
                string resultCss = GetElementCss(element.elementLeft, element.elementWidth);

                if (IsElementHidden(element))
                {
                    return string.Format(CSS_FORMAT_CLASS_AND_STYLE, controlId, resultCss, CSS_DIV_HIDDEN);
                }
                else
                {
                    return string.Format(CSS_FORMAT_ID_AND_CLASS, controlId, resultCss);
                }
            }

            /// <summary>
            /// Gets the opening tag of the specified markup element.
            /// </summary>
            /// <param name="currentElement">The current element</param>
            /// <param name="nextElement">The next element.</param>
            /// <param name="controlId">The control unique identifier.</param>
            /// <returns>A specific HTML markup string</returns>
            public static string ElementBeginTag(SimpleViewElementModel4WS currentElement, SimpleViewElementModel4WS nextElement, string controlId)
            {
                string resultCss = GetElementCss(currentElement.elementLeft, currentElement.elementWidth, nextElement.elementLeft, nextElement.elementWidth);

                if (IsElementHidden(currentElement))
                {
                    return string.Format(CSS_FORMAT_CLASS_AND_STYLE, controlId, resultCss, CSS_DIV_HIDDEN);
                }
                else
                {
                    return string.Format(CSS_FORMAT_ID_AND_CLASS, controlId, resultCss);
                }
            }

            /// <summary>
            /// Gets the ending tag.
            /// </summary>
            /// <returns>A specific HTML markup string</returns>
            public static string EndTag()
            {
                return CSS_DIV_END;
            }

            /// <summary>
            /// Gets the new line tag of the specified markup element.
            /// </summary>
            /// <returns>A specific HTML markup string</returns>
            public static string NewLineTag()
            {
                return string.Format(CSS_FORMAT_NEW_LINE, CSS_DIV_CLEAR);
            }

            /// <summary>
            /// Gets the width of element on the HTML page.
            /// </summary>
            /// <param name="element">The element which needs to translate the width of form designer to the width of HTML page</param>
            /// <param name="control">The element</param>
            /// <returns>the with Unit.</returns>
            public static Unit GetWidth(SimpleViewElementModel4WS element, WebControl control)
            {
                Unit resultWidth;

                int tempWidth = element.elementWidth;

                if (tempWidth == 0)
                {
                    tempWidth = GRID960_DEFAULT_FIELD_WIDTH;
                }

                resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - ((double)GRID960_INPUT_DEFAULT_PERCENTAGE / (double)tempWidth), UnitType.Percentage);

                if (!string.IsNullOrEmpty(element.unitType))
                {
                    if (tempWidth > GRID960_FULL_WIDTH / 2)
                    {
                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - ((double)GRID960_TEXTBOX_LARGER_CORRECTION / (double)tempWidth), UnitType.Percentage);
                    }
                    else
                    {
                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - ((double)GRID960_TEXTBOX_SMALL_CORRECTION / (double)tempWidth), UnitType.Percentage);
                    }
                }
                else
                {
                    if (!ShowPercentage)
                    {
                        resultWidth = new Unit(((double)tempWidth / GRID960_EM_UNIT) - GRID960_EM_CORRECTION, UnitType.Em);
                    }
                    else
                    {
                        resultWidth = new Unit(((double)tempWidth / GRID960_EM_UNIT) - GRID960_EM_CORRECTION - 1.7, UnitType.Em);
                        ShowPercentage = false;
                    }
                }

                if (control is AccelaCalendarText)
                {
                    if (!string.IsNullOrEmpty(element.unitType))
                    {
                        int unitLength = element.unitType.Length;

                        if (tempWidth > GRID960_FULL_WIDTH / 2)
                        {
                            resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE) / (float)tempWidth), UnitType.Percentage);
                        }
                        else
                        {
                            if (tempWidth > GRID960_FULL_WIDTH / 4)
                            {
                                if (unitLength < 6)
                                {
                                    resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE_SMALLER) / (float)tempWidth), UnitType.Percentage);
                                }
                                else
                                {
                                    if (unitLength < 8)
                                    {
                                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE_SMALL) / (float)tempWidth), UnitType.Percentage);
                                    }
                                    else
                                    {
                                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE_NORMAL) / (float)tempWidth), UnitType.Percentage);
                                    }
                                }
                            }
                            else
                            {
                                if (unitLength < 6)
                                {
                                    resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE_SMALLER) / (float)tempWidth), UnitType.Percentage);
                                }
                                else
                                {
                                    if (unitLength < 8)
                                    {
                                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE_LARGE) / (float)tempWidth), UnitType.Percentage);
                                    }
                                    else
                                    {
                                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE_LARGER) / (float)tempWidth), UnitType.Percentage);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE - (((float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE) / (float)tempWidth), UnitType.Percentage);
                    }
                }

                if (control is AccelaTextBoxWithImageButton)
                {
                    //resultWidth = new Unit((GRID960_ONE_HUNDRED_PERCENTAGE - (float)GRID960_FULL_WIDTH * GRID960_ICON_PERCENTAGE / (float)tempWidth), UnitType.Percentage);
                    if (tempWidth > GRID960_FULL_WIDTH / 2)
                    {
                        resultWidth = new Unit((double)GRID960_ONE_HUNDRED_PERCENTAGE - ((((double)GRID960_FULL_WIDTH * GRID960_SEARCH_ICON_PERCENTAGE) / (double)tempWidth) + GRID960_SEARCH_ICON_LARGE_CORRECTION), UnitType.Percentage);
                    }
                    else
                    {
                        resultWidth = new Unit((double)GRID960_ONE_HUNDRED_PERCENTAGE - ((((double)GRID960_FULL_WIDTH * GRID960_SEARCH_ICON_PERCENTAGE) / (double)tempWidth) + GRID960_SEARCH_ICON_SMALL_CORRECTION), UnitType.Percentage);
                    }
                }

                if (control is AccelaPhoneText)
                {
                    if (ControlConfigureProvider.IsCountryCodeEnabled())
                    {
                        double tmpWidth = ((double)element.elementWidth / GRID960_EM_UNIT) - GRID960_PHONE_COUNTRY_NUMBER_SYMBOL_WIDTH - GRID960_PHONE_COUNTRY_NUMBER_TEXT_WIDTH - GRID960_PHONE_GUTTER;

                        if (tmpWidth > 0)
                        {
                            resultWidth = new Unit(tmpWidth, UnitType.Em);
                        }
                    }
                }

                if (control is AccelaDropDownList)
                {
                    resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE, UnitType.Percentage);
                }

                if (control is AccelaCountryDropDownList)
                {
                    resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE, UnitType.Percentage);
                }

                if (control is AccelaStateControl)
                {
                    if (!ControlConfigureProvider.IsCountryCodeEnabled())
                    {
                        resultWidth = new Unit(GRID960_ONE_HUNDRED_PERCENTAGE, UnitType.Percentage);
                    }
                }

                if (control is AccelaTimeSelection)
                {
                    if (!string.IsNullOrEmpty(I18nDateTimeUtil.ShortTimePattern) && I18nDateTimeUtil.ShortTimePattern.IndexOf("h") != -1)
                    {
                        double tmpWidth = ((double)element.elementWidth / GRID960_EM_UNIT) - GRID960_TIME_SELECTION_DDL_WIDTH;

                        if (tmpWidth > 0)
                        {
                            resultWidth = new Unit(tmpWidth, UnitType.Em);
                        }
                    }
                }

                if (control is AccelaCheckBox)
                {
                    double tmpWidth = element.elementWidth;
                    if (!string.IsNullOrEmpty(element.elementInstruction))
                    {
                        tmpWidth = tmpWidth - CHECKBOX_HELP_ICON_WIDTH;
                    }

                    tmpWidth = tmpWidth / CHECKBOX_EM_PX_RATE;

                    if (tmpWidth > 0)   
                    {
                        resultWidth = new Unit(tmpWidth, UnitType.Em);
                    }
                }

                if (control is AccelaNameValueLabel)
                {
                    double tmpWidth = element.elementWidth == 0 ? GRID960_DEFAULT_FIELD_WIDTH : element.elementWidth;
                    resultWidth = new Unit(tmpWidth / NAMEVALUE_EM_PX_RATE, UnitType.Em);
                }

                return resultWidth;
            }

            /// <summary>
            /// Gets the height of element on the HTML page.
            /// </summary>
            /// <param name="element">The element which needs to translate the width of form designer to the width of HTML page</param>
            /// <param name="control">The element</param>
            /// <returns>the height unit</returns>
            public static Unit GetHeight(SimpleViewElementModel4WS element, WebControl control)
            {
                Unit resultHeight;

                int height = element.elementHeight;

                if (height == 0)
                {
                    resultHeight = new Unit(GRID960_TEXTBOX_DEFAULT_HEIGHT, UnitType.Em);

                    if (control is AccelaDropDownList)
                    {
                        resultHeight = new Unit(GRID960_DROPDOWNLIST_DEFAULT_HEIGHT, UnitType.Em);
                    }

                    if (control is AccelaStateControl)
                    {
                        if (!ControlConfigureProvider.IsCountryCodeEnabled())
                        {
                            resultHeight = new Unit(GRID960_DROPDOWNLIST_DEFAULT_HEIGHT, UnitType.Em);
                        }
                    }

                    if (string.Equals(element.elementType, ControlType.Table.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (control is Image)
                        {
                            resultHeight = new Unit(GRID960_TABLE_DEFAULT_HEIGHT, UnitType.Em);
                        }
                    }

                    if (control is AccelaTextBox && ((AccelaTextBox)control).TextMode == TextBoxMode.MultiLine)
                    {
                        // Set the default height of Textarea
                        resultHeight = new Unit(GRID960_TEXTAREA_DEFAULT_HEIGHT, UnitType.Em);
                    }
                }
                else
                {
                    height = height < 52 ? 52 : height;
                    resultHeight = new Unit((double)(height - GRID960_TEXTAREA_CORRECTION) / (double)GRID960_WIDTH_DEFAULT_UNIT, UnitType.Em);
                }

                return resultHeight;
            }

            /// <summary>
            /// Gets the padding div tag.
            /// </summary>
            /// <param name="element">The element.</param>
            /// <returns>the begin tag</returns>
            public static string PaddingBeginTag(SimpleViewElementModel4WS element)
            {
                StringBuilder cssClass = new StringBuilder();

                cssClass.Append(CSS_CLASS_GRID);

                int gridNumber = 0;

                int width = element.elementLeft;

                if (width < GridWidth)
                {
                    width = GridWidth;
                }

                gridNumber = (width / GridWidth) + ((width % GridWidth == 0) ? 0 : 1);

                cssClass.Append(gridNumber.ToString());

                return string.Format(CSS_FORMAT_ONLY_CLASS, cssClass.ToString());
            }

            /// <summary>
            /// Gets the padding content.
            /// </summary>
            /// <returns>the html space</returns>
            public static string PaddingContent()
            {
                return "&nbsp;";
            }

            /// <summary>
            /// Set Attributes of Control.
            /// </summary>
            /// <param name="currentElement">SimpleViewElement Model</param>
            /// <param name="control">current control to need format label, input and unit width.</param>
            public static void SetAttributes(SimpleViewElementModel4WS currentElement, WebControl control)
            {
                IAccelaControl actrl = (IAccelaControl)control;
                bool hasSubLabel = !string.IsNullOrEmpty(actrl.GetSubLabel());

                double labelWidth = currentElement.labelWidth;
                double inputWidth = currentElement.inputWidth;
                double unitWidth = currentElement.unitWidth;
                double elementWidth = currentElement.elementWidth;

                if (elementWidth == 0)
                {
                    elementWidth = Grid960SystemHelper.GRID960_DEFAULT_FIELD_WIDTH;
                }

                if (labelWidth == 0 && inputWidth == 0)
                {
                    if (string.IsNullOrEmpty(currentElement.unitType))
                    {
                        if (control is AccelaCheckBox)
                        {
                            inputWidth = elementWidth;
                        }
                        else
                        {
                            labelWidth = elementWidth * LABEL_WIDTH_RATE_WITHOUT_UNIT;
                            inputWidth = elementWidth * INPUT_WIDTH_RATE_WITHOUT_UNIT;
                        }
                    }
                    else
                    {
                        if (control is AccelaCheckBox)
                        {
                            inputWidth = elementWidth * LABEL_WIDTH_RATE_WITH_UNIT * 2;
                            unitWidth = elementWidth * UNIT_WIDTH_RATE;
                        }
                        else
                        {
                            labelWidth = elementWidth * LABEL_WIDTH_RATE_WITH_UNIT;
                            inputWidth = elementWidth * INPUT_WIDTH_RATE_WITH_UNIT;
                            unitWidth = elementWidth * UNIT_WIDTH_RATE;
                        }
                    }
                }

                if (control is AccelaPhoneText)
                {
                    if (ControlConfigureProvider.IsCountryCodeEnabled())
                    {
                        inputWidth = inputWidth > COUNTRY_WIDTH + PHONE_CORRECTION ? inputWidth - COUNTRY_WIDTH - PHONE_CORRECTION : 0;
                    }
                }
                else if (control is AccelaCalendarText)
                {
                    inputWidth = inputWidth - CANLENDEAR_IMAGE_WIDTH;
                }
                else if (control is AccelaTextBoxWithImageButton)
                {
                    inputWidth = inputWidth - TEXT_IMAGE_WIDTH;
                }
                else if (control is AccelaTimeSelection)
                {
                    AccelaTimeSelection timeControl = control as AccelaTimeSelection;

                    if (!timeControl.IsUse24Hours)
                    {
                        inputWidth = inputWidth - TIME_AMPM_WIDTH;
                    }
                }

                //handle the exception, input width must equals or greater than 0
                if (inputWidth < 0)
                {
                    inputWidth = 0;
                }

                Unit inputUnit = Grid960SystemHelper.ConvertToEm(inputWidth);
                
                control.Width = inputUnit;
                actrl.LabelWidth = string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0}em", labelWidth / LABEL_EM_PX_RATE);
                actrl.UnitWidth = string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0}em", unitWidth / UNIT_EM_PX_RATE);
            }

            /// <summary>
            /// Set attributes for name value control.
            /// </summary>
            /// <param name="currentElement">SimpleViewElement Model</param>
            /// <param name="control">current control to need format label with, input width.</param>
            public static void SetNameValueAttributes(SimpleViewElementModel4WS currentElement, WebControl control)
            {
                AccelaNameValueLabel label = control as AccelaNameValueLabel;
                if (currentElement.inputWidth == 0 && currentElement.labelWidth == 0)
                {
                    int tempWidth = (currentElement.elementWidth == 0
                                         ? GRID960_DEFAULT_FIELD_WIDTH
                                         : currentElement.elementWidth) / 2;

                    currentElement.inputWidth = tempWidth;
                    currentElement.labelWidth = tempWidth;
                }

                label.LabelWidth = string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0}em", (double)currentElement.labelWidth / NAMEVALUE_EM_PX_RATE);
                label.FieldWidth = string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0}em", (double)currentElement.inputWidth / NAMEVALUE_EM_PX_RATE);
            }

            #region private methods
            /// <summary>
            /// get container CSS
            /// </summary>
            /// <returns>container html</returns>
            private static string GetContainerCss()
            {
                StringBuilder cssClass = new StringBuilder();

                cssClass.Append(CSS_CLASS_CONTAINER);
                cssClass.Append(Column.ToString());

                return cssClass.ToString();
            }

            /// <summary>
            /// get the element CSS
            /// </summary>
            /// <param name="left">the left</param>
            /// <param name="width">the width</param>
            /// <returns>element CSS string</returns>
            private static string GetElementCss(int left, int width)
            {
                int widthValue = width;

                StringBuilder cssClass = new StringBuilder();

                cssClass.Append(CSS_CLASS_GRID);

                int gridNumber = 0;

                if (widthValue == 0)
                {
                    widthValue = GRID960_DEFAULT_FIELD_WIDTH;
                }

                gridNumber = (widthValue / GridWidth) + ((widthValue % GridWidth == 0) ? 0 : 1);

                cssClass.Append(gridNumber.ToString());

                return cssClass.ToString();
            }

            /// <summary>
            /// get the element CSS
            /// </summary>
            /// <param name="currentLeft">The left location of the current element</param>
            /// <param name="currentWidth">The width of the current element</param>
            /// <param name="nextLeft">The next left.</param>
            /// <param name="nextWidth">Width of the next.</param>
            /// <returns>the element CSS string.</returns>
            private static string GetElementCss(int currentLeft, int currentWidth, int nextLeft, int nextWidth)
            {
                int currentLeftValue = currentLeft;
                int currentWidthValue = currentWidth;

                int nextLeftValue = nextLeft;

                StringBuilder cssClass = new StringBuilder();

                cssClass.Append(CSS_CLASS_GRID);

                int gridNumber = 0;

                if (currentWidthValue == 0)
                {
                    currentWidthValue = GRID960_DEFAULT_FIELD_WIDTH;
                }

                gridNumber = (currentWidthValue / GridWidth) + ((currentWidthValue % GridWidth == 0) ? 0 : 1);

                cssClass.Append(gridNumber.ToString());

                if (nextLeftValue != currentLeftValue)
                {
                    int suffixNumber = 0;

                    if (nextLeftValue - currentLeftValue - currentWidthValue > 0)
                    {
                        suffixNumber = (nextLeftValue - currentLeftValue - currentWidthValue) / GridWidth;
                    }
                    else
                    {
                        suffixNumber = GRID960_DEFAULT_PREFIX_WIDTH;
                    }

                    if (suffixNumber != 0)
                    {
                        cssClass.Append(" ");
                        cssClass.Append(CSS_CLASS_SUFFIX);
                        cssClass.Append(suffixNumber.ToString());
                    }
                }

                return cssClass.ToString();
            }

            /// <summary>
            /// Gets a value indicating whether the element is visible
            /// </summary>
            /// <param name="currentElement">Simple view element.</param>
            /// <returns>whether element hidden</returns>
            private static bool IsElementHidden(SimpleViewElementModel4WS currentElement)
            {
                return !ACAConstant.VALID_STATUS.Equals(currentElement.recStatus, StringComparison.InvariantCultureIgnoreCase)
                       || ValidationUtil.IsNo(currentElement.acaDisplayFlag)
                       || ValidationUtil.IsHidden(currentElement.acaDisplayFlag);
            }

            #endregion

            #endregion Methods
        }

        #endregion Nested Types
    }
}