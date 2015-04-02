#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaAKA.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description: The web control for "Also Know As" field.
 *
 *  Notes:
 * $Id: AccelaAKA.cs 239070 2012-12-07 07:03:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,            Who,        What
 *  Dec 7, 2012      Alan Hu     Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

[assembly: WebResource("Accela.Web.Controls.AccelaAKA.js", "text/javascript")]

namespace Accela.Web.Controls
{
    /// <summary>
    /// The web control for "Also Know As" field.
    /// </summary>
    public class AccelaAKA : AccelaCompositeControl
    {
        #region Fields

        /// <summary>
        /// Attribute name for input fields' "fieldName" attribute.
        /// This attribute is used in client scripts to indicating each input field.
        /// </summary>
        private const string ATTRIBUTE_FIELD_NAME = "fieldName";

        /// <summary>
        /// ID prefix for all first name fields.
        /// This ID also be used in client scripts to indicating the first name field.
        /// </summary>
        private const string ID_PREFIX_FIRSTNAME = "FirstName";

        /// <summary>
        /// ID prefix for all middle name fields.
        /// This ID also be used in client scripts to indicating the middle name field.
        /// </summary>
        private const string ID_PREFIX_MIDDLENAME = "MiddleName";

        /// <summary>
        /// ID prefix for all last name fields.
        /// This ID also be used in client scripts to indicating the last name field.
        /// </summary>
        private const string ID_PREFIX_LASTNAME = "LastName";

        /// <summary>
        /// ID prefix for all last name fields.
        /// This ID also be used in client scripts to indicating the full name field.
        /// </summary>
        private const string ID_PREFIX_FULLNAME = "FullName";

        /// <summary>
        /// The hidden style
        /// </summary>
        private const string HIDDEN_CLASS = " class='ACA_Hide'";

        /// <summary>
        /// Initial row index.
        /// </summary>
        private const string EMPTY_ROW_INDEX = "0";

        /// <summary>
        /// Control ID for State control.
        /// </summary>
        private string _stateControlID = "ClientState";

        /// <summary>
        /// State control used the store the AKA field's status.
        /// Such as: Readonly, Row index and Contact number for each AKA item.
        /// </summary>
        private HtmlInputHidden _stateControl;

        /// <summary>
        /// Local field for <see cref="ClientState"/> property.
        /// </summary>
        private AKAClientState _state;
        
        /// <summary>
        /// Temporary data list for this AKA field.
        /// </summary>
        private IDictionary<string, PeopleAKAModel> _akaList;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the label key for Add button.
        /// </summary>
        public string AddButtonLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key for Delete button.
        /// </summary>
        public string DelButtonLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key for First Name column.
        /// </summary>
        public string FirstNameLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key for Middle Name column.
        /// </summary>
        public string MiddleNameLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key for Last Name column.
        /// </summary>
        public string LastNameLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key for Last Name column.
        /// </summary>
        public string FullNameLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the first name is hidden
        /// </summary>
        public bool IsFirstNameHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the middle name is hidden
        /// </summary>
        public bool IsMiddleNameHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the last name is hidden
        /// </summary>
        public bool IsLastNameHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the full name is hidden
        /// </summary>
        public bool IsFullNameHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the text boxes.
        /// </summary>
        public int MaxLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is readonly.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return ClientState.ReadOnly;
            }

            set
            {
                AKAClientState state = ClientState;
                state.ReadOnly = value;
                ClientState = state;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
        /// </summary>
        /// <returns>true if the control is visible on the page; otherwise false.</returns>
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }

            set
            {
                bool originalVisible = base.Visible;
                base.Visible = value;

                if (!value)
                {
                    if (originalVisible)
                    {
                        //Change AKA from display to hidden, save cotrol value to view state. 
                        ViewState["ControlValue"] = GetValue(null, null, null, null);
                    }
                }
                else
                {
                    if (!originalVisible)
                    {
                        //Change AKA from hidden to display, set view state to control. 
                        this.SetValue(ViewState["ControlValue"] as IEnumerable<PeopleAKAModel>);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> value that corresponds to this Web server control. This property is used primarily by control developers
        /// Override the <see cref="TagKey"/> property to render the AKA control as a div element.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> enumeration values.</returns>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Gets the initial state for the AKA control.
        /// </summary>
        private AKAClientState EmptyState
        {
            get
            {
                AKAClientState state = new AKAClientState();
                state.RowIndexes = new string[] { EMPTY_ROW_INDEX };
                state.AKAIDs = new Dictionary<string, string>();
                state.AKAIDs.Add(EMPTY_ROW_INDEX, null);
                state.FullNames = new Dictionary<string, string>();
                state.FullNames.Add(EMPTY_ROW_INDEX, null);
                state.StartDates = new Dictionary<string, string>();
                state.StartDates.Add(EMPTY_ROW_INDEX, null);
                state.EndDates = new Dictionary<string, string>();
                state.EndDates.Add(EMPTY_ROW_INDEX, null);

                return state;
            }
        }

        /// <summary>
        /// Gets or sets the AKA control state.
        /// </summary>
        private AKAClientState ClientState
        {
            get
            {
                if (_state == null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    /*
                     * Steps for get AKA control's state:
                     * 1st > Get state from state control.
                     * 2nd > If state control is null then get state from HTTP context.
                     * 3rd > If HTTP context is null then get state from temporary AKA data list.
                     * 4th > Get the initial state.
                     */
                    if (_stateControl != null && !string.IsNullOrWhiteSpace(_stateControl.Value))
                    {
                        _state = serializer.Deserialize<AKAClientState>(_stateControl.Value);
                    }
                    else if (HttpContext.Current != null && HttpContext.Current.Request.Params[StateControlUniqueID] != null)
                    {
                        string stateValue = HttpContext.Current.Request.Params[StateControlUniqueID];
                        _state = serializer.Deserialize<AKAClientState>(stateValue);
                    }
                    else if (_akaList != null)
                    {
                        _state = new AKAClientState();
                        _state.RowIndexes = _akaList.Keys;
                        _state.AKAIDs = _akaList.ToDictionary(a => a.Key, a => a.Value.contactNumber != null ? a.Value.contactNumber.ToString() : string.Empty);
                        _state.FullNames = _akaList.ToDictionary(a => a.Key, a => string.IsNullOrEmpty(a.Value.fullName) ? string.Empty : a.Value.fullName);
                        _state.EndDates = _akaList.ToDictionary(
                            a => a.Key,
                            a => a.Value.endDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForUI(a.Value.endDate.Value));
                        _state.StartDates = _akaList.ToDictionary(
                            a => a.Key,
                            a => a.Value.startDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForUI(a.Value.startDate.Value));
                    }
                    else
                    {
                        _state = EmptyState;
                    }
                }

                return _state;
            }

            set
            {
                EnsureChildControls();
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                //Accepting the new state and save to state control.
                _state = value;
                _stateControl.Value = serializer.Serialize(_state);
            }
        }

        /// <summary>
        /// Gets the state control's unique ID.
        /// </summary>
        private string StateControlUniqueID
        {
            get
            {
                return this.UniqueID + this.IdSeparator + _stateControlID;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear the value and set the control to initial status.
        /// </summary>
        public override void ClearValue()
        {
            //Reset the flag and create child controls when clear value.
            this.ChildControlsCreated = false;

            //Clear the control state.
            ClientState = EmptyState;

            //clear the value cache.
            ViewState["ControlValue"] = null;

            //Clear the control value for the initial row.
            TextBox txtFirstName = FindControl(ID_PREFIX_FIRSTNAME + EMPTY_ROW_INDEX) as TextBox;
            TextBox txtMiddleName = FindControl(ID_PREFIX_MIDDLENAME + EMPTY_ROW_INDEX) as TextBox;
            TextBox txtLastName = FindControl(ID_PREFIX_LASTNAME + EMPTY_ROW_INDEX) as TextBox;
            TextBox txtFullName = FindControl(ID_PREFIX_FULLNAME + EMPTY_ROW_INDEX) as TextBox;

            if (txtFirstName != null)
            {
                txtFirstName.Text = null;
            }

            if (txtMiddleName != null)
            {
                txtMiddleName.Text = null;
            }

            if (txtLastName != null)
            {
                txtLastName.Text = null;
            }

            if (txtFullName != null)
            {
                txtFullName.Text = null;
            }
        }

        /// <summary>
        /// Disable the AKA field.
        /// Disabled AKA cannot be add/delete/edit.
        /// </summary>
        public override void DisableEdit()
        {
            ReadOnly = true;
        }

        /// <summary>
        /// Enable the AKA field.
        /// Enabled AKA could be add new item, if the existed data item isn't reference data, it could be edit and delete.
        /// </summary>
        public override void EnableEdit()
        {
            ReadOnly = false;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="contactSeq">The contact sequence.</param>
        /// <param name="agencyCode">The agency code used to build the value list.</param>
        /// <param name="callerId">The caller id.</param>
        /// <param name="auditStatus">The audit status.</param>
        /// <returns>AKA data list.</returns>
        public IEnumerable<PeopleAKAModel> GetValue(string contactSeq, string agencyCode, string callerId, string auditStatus)
        {
            EnsureChildControls();

            List<PeopleAKAModel> valueList = new List<PeopleAKAModel>();
            AKAClientState state = ClientState;
            IEnumerable<string> rowIdxes = state.RowIndexes;

            foreach (string i in rowIdxes)
            {
                PeopleAKAModel aka = new PeopleAKAModel();
                TextBox firstName = FindControl(ID_PREFIX_FIRSTNAME + i) as TextBox;
                TextBox middleName = FindControl(ID_PREFIX_MIDDLENAME + i) as TextBox;
                TextBox lastName = FindControl(ID_PREFIX_LASTNAME + i) as TextBox;
                TextBox fullName = FindControl(ID_PREFIX_FULLNAME + i) as TextBox;
                string resId = state.AKAIDs[i];
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(state.StartDates[i]))
                {
                    startDate = I18nDateTimeUtil.ParseFromUI(state.StartDates[i]);
                }

                if (!string.IsNullOrEmpty(state.EndDates[i]))
                {
                    endDate = I18nDateTimeUtil.ParseFromUI(state.EndDates[i]);
                }
               
                bool isNotNull = false;

                if (firstName != null && !string.IsNullOrWhiteSpace(firstName.Text))
                {
                    aka.firstName = firstName.Text.Trim();
                    isNotNull = true;
                }

                if (middleName != null && !string.IsNullOrWhiteSpace(middleName.Text))
                {
                    aka.middleName = middleName.Text.Trim();
                    isNotNull = true;
                }

                if (lastName != null && !string.IsNullOrWhiteSpace(lastName.Text))
                {
                    aka.lastName = lastName.Text.Trim();
                    isNotNull = true;
                }

                if (fullName != null && !string.IsNullOrWhiteSpace(fullName.Text))
                {
                    aka.fullName = fullName.Text.Trim();
                    isNotNull = true;
                }

                if (!string.IsNullOrWhiteSpace(resId))
                {
                    aka.resId = Convert.ToInt64(resId);
                }

                aka.startDate = startDate;
                aka.endDate = endDate;

                if (!string.IsNullOrEmpty(contactSeq))
                {
                    aka.contactNumber = Convert.ToInt64(contactSeq);
                }

                if (isNotNull)
                {
                    aka.auditModel = new SimpleAuditModel()
                        {
                            auditDate = DateTime.Now,
                            auditStatus = auditStatus,
                            auditID = callerId
                        };
                    aka.serviceProviderCode = agencyCode;
                    valueList.Add(aka);
                }
            }

            return valueList.Count == 0 ? null : valueList;
        }

        /// <summary>
        /// The method inherited from base control but no longer be supported in Accela AKA control.
        /// </summary>
        /// <returns>Will throw NotImplementedException.</returns>
        /// <exception cref="System.NotImplementedException">Not Implement</exception>
        public override string GetValue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(IEnumerable<PeopleAKAModel> value)
        {
            /*
             * If the field is visible, use UI to keep the control value.
             * If the field is invisible, use ViewState the keep the control value.
             */

            if (!this.Visible)
            {
                //Click a auto fill contact, the contact aka control is hidden. should save the AKA value to view state.
                ViewState["ControlValue"] = value;
                return;
            }
            else
            {
                ViewState["ControlValue"] = null;
            }

            if (value != null)
            {
                IEnumerable<PeopleAKAModel> valueList = value;
                _akaList = new Dictionary<string, PeopleAKAModel>();
                int index = 0;
                AKAClientState state = ClientState;

                foreach (PeopleAKAModel aka in valueList)
                {
                    if (aka == null)
                    {
                        continue;
                    }

                    _akaList.Add(index.ToString(), aka);
                    index++;
                }

                state.RowIndexes = _akaList.Keys;
                state.AKAIDs = _akaList.ToDictionary(a => a.Key, a => a.Value.resId != null ? a.Value.resId.ToString() : string.Empty);
                state.FullNames = _akaList.ToDictionary(a => a.Key, a => string.IsNullOrEmpty(a.Value.fullName) ? string.Empty : a.Value.fullName);
                state.EndDates = _akaList.ToDictionary(
                    a => a.Key,
                    a => a.Value.endDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForUI(a.Value.endDate.Value));
                state.StartDates = _akaList.ToDictionary(
                    a => a.Key,
                    a => a.Value.startDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateTimeStringForUI(a.Value.startDate.Value));

                //Reset the flag and to create child controls once the new value had been assigned.
                this.ChildControlsCreated = false;
                ClientState = state;
            }
            else
            {
                ClearValue();
            }
        }

        /// <summary>
        /// inherited from base control but no longer be supported in Accela AKA control.
        /// </summary>
        /// <param name="value">Control value.</param>
        public override void SetValue(string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// initial daily extender control
        /// </summary>
        public override void InitExtenderControl()
        {
            //not need extender
        }

        /// <summary>
        /// Gets a value indicating whether the control is required.
        /// </summary>
        /// <returns>
        /// true or false.
        /// </returns>
        public override bool IsRequired()
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether to hide the require indicator.
        /// </summary>
        /// <returns>
        /// true or false.
        /// </returns>
        public override bool IsHideRequireIndicate()
        {
            return false;
        }

        /// <summary>
        /// Create child controls.
        /// Include state control and each row's input controls(First Name, Middle Name, Last Name).
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            //Get state and row indexes.
            AKAClientState state = ClientState;
            IEnumerable<string> rowIdxes = state.RowIndexes;

            string moduleName = GetModuleName();
            string akaName = LabelConvertUtil.GetTextByKey("aca_contactedit_label_akaname", moduleName).TrimEnd();

            if (akaName.EndsWith(ACAConstant.TIME_SEPARATOR))
            {
                akaName = akaName.Substring(0, akaName.Length - 1);
            }

            //Create input controls for each row.
            foreach (string i in rowIdxes)
            {
                TextBox txtFirstName = new TextBox();
                txtFirstName.EnableViewState = false;
                txtFirstName.ID = ID_PREFIX_FIRSTNAME + i;
                txtFirstName.ToolTip = akaName + ACAConstant.BLANK + LabelConvertUtil.GetTextByKey(FirstNameLabelKey, moduleName);

                TextBox txtMiddleName = new TextBox();
                txtMiddleName.EnableViewState = false;
                txtMiddleName.ID = ID_PREFIX_MIDDLENAME + i;
                txtMiddleName.ToolTip = akaName + ACAConstant.BLANK + LabelConvertUtil.GetTextByKey(MiddleNameLabelKey, moduleName);

                TextBox txtLastName = new TextBox();
                txtLastName.EnableViewState = false;
                txtLastName.ID = ID_PREFIX_LASTNAME + i;
                txtLastName.ToolTip = akaName + ACAConstant.BLANK + LabelConvertUtil.GetTextByKey(LastNameLabelKey, moduleName);

                TextBox txtFullName = new TextBox();
                txtFullName.EnableViewState = false;
                txtFullName.ID = ID_PREFIX_FULLNAME + i;
                txtFullName.ToolTip = akaName + ACAConstant.BLANK + LabelConvertUtil.GetTextByKey(FullNameLabelKey, moduleName);

                /*
                 * Set input controls' initial width as 0.
                 * All input controls' width will be adjusted in client scripts.
                 */
                txtFirstName.Width = new Unit(0);
                txtMiddleName.Width = new Unit(0);
                txtLastName.Width = new Unit(0);
                txtFullName.Width = new Unit(0);

                //Set max length.
                txtFirstName.MaxLength = MaxLength;
                txtMiddleName.MaxLength = MaxLength;
                txtLastName.MaxLength = MaxLength;
                txtFullName.MaxLength = MaxLength;

                //Set the fieldName attribute used to indicating the field in client scripts.
                txtFirstName.Attributes.Add(ATTRIBUTE_FIELD_NAME, ID_PREFIX_FIRSTNAME);
                txtMiddleName.Attributes.Add(ATTRIBUTE_FIELD_NAME, ID_PREFIX_MIDDLENAME);
                txtLastName.Attributes.Add(ATTRIBUTE_FIELD_NAME, ID_PREFIX_LASTNAME);
                txtFullName.Attributes.Add(ATTRIBUTE_FIELD_NAME, ID_PREFIX_FULLNAME);

                if (_akaList != null)
                {
                    PeopleAKAModel aka = _akaList[i];

                    //Assign control value.
                    if (aka != null)
                    {
                        txtFirstName.Text = aka.firstName;
                        txtMiddleName.Text = aka.middleName;
                        txtLastName.Text = aka.lastName;
                        txtFullName.Text = aka.fullName;
                    }
                }

                this.Controls.Add(txtFirstName);
                this.Controls.Add(txtMiddleName);
                this.Controls.Add(txtLastName);
                this.Controls.Add(txtFullName);
            }

            //Create state control.
            _stateControl = new HtmlInputHidden();
            _stateControl.EnableViewState = false;
            _stateControl.ID = _stateControlID;

            //Save state to state control.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            _stateControl.Value = serializer.Serialize(state);

            Controls.Add(_stateControl);
        }

        /// <summary>
        /// Render the client scripts resources.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Page != null)
            {
                if (!IsHidden)
                {
                    this.IsHidden = IsFirstNameHidden && IsMiddleNameHidden && IsLastNameHidden && IsFullNameHidden;
                }

                //Register the client scripts resource.
                ScriptManager.RegisterClientScriptResource(Page, this.GetType(), "Accela.Web.Controls.AccelaAKA.js");

                //Register a action to adjust the control's layout after document ready.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), ClientID, "AdjustAKAField('" + ClientID + "');", true);
            }
        }

        /// <summary>
        /// Render the AKA control.
        /// </summary>
        /// <param name="writer">Html text writer for the control output.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            //Get module name to get the label value.
            string moduleName = GetModuleName();

            //Get state and row indexes.
            AKAClientState state = ClientState;
            IEnumerable<string> rowIdxes = state.RowIndexes;

            //Render the table begin tag and table head.
            writer.Write("<table id='" + ClientID + "_list' stateControlId='" + _stateControl.ClientID
                + "' class='aka_list' style='width:100%;' cellSpacing='0' cellPadding='0' ");
            writer.Write("summary='{0}'>", LabelConvertUtil.GetTextByKey("aca_summary_control_aka", moduleName));
            writer.Write("<caption>{0}</caption>", LabelConvertUtil.GetTextByKey("aca_caption_control_aka", moduleName));
            writer.Write("<thead><tr>");
            writer.Write("<th scope='col'" + (IsFirstNameHidden ? HIDDEN_CLASS : string.Empty) + "><label class='ACA_Label'>" + LabelConvertUtil.GetTextByKey(FirstNameLabelKey, moduleName) + "</label></th>");
            writer.Write("<th scope='col'" + (IsMiddleNameHidden ? HIDDEN_CLASS : string.Empty) + "><label class='ACA_Label'>" + LabelConvertUtil.GetTextByKey(MiddleNameLabelKey, moduleName) + "</label></th>");
            writer.Write("<th scope='col'" + (IsLastNameHidden ? HIDDEN_CLASS : string.Empty) + "><label class='ACA_Label'>" + LabelConvertUtil.GetTextByKey(LastNameLabelKey, moduleName) + "</label></th>");
            writer.Write("<th scope='col'" + (IsFullNameHidden ? HIDDEN_CLASS : string.Empty) + "><label class='ACA_Label'>" + LabelConvertUtil.GetTextByKey(FullNameLabelKey, moduleName) + "</label></th>");
            writer.Write("<th scope='col'></th>");
            writer.Write("</tr></thead>");

            string listId = ClientID + "_list";
            string labelAdd = LabelConvertUtil.GetTextByKey(AddButtonLabelKey, moduleName);
            string labelDelete = LabelConvertUtil.GetTextByKey(DelButtonLabelKey, moduleName);
            
            //Render table body.
            writer.Write("<tbody>");

            foreach (string i in rowIdxes)
            {
                TextBox txtFirstName = (TextBox)FindControl(ID_PREFIX_FIRSTNAME + i);
                TextBox txtMiddleName = (TextBox)FindControl(ID_PREFIX_MIDDLENAME + i);
                TextBox txtLastName = (TextBox)FindControl(ID_PREFIX_LASTNAME + i);
                TextBox txtFullName = (TextBox)FindControl(ID_PREFIX_FULLNAME + i);
                txtFirstName.Attributes.Add("isAKA", "AKA");
                txtMiddleName.Attributes.Add("isAKA", "AKA");
                txtLastName.Attributes.Add("isAKA", "AKA");
                txtFullName.Attributes.Add("isAKA", "AKA");

                bool isReferenceData = !string.IsNullOrWhiteSpace(state.AKAIDs[i]);

                if (state.ReadOnly || isReferenceData)
                {
                    //Set current row's input fields as readonly if the table is readonly or current data is reference data.
                    txtFirstName.ReadOnly = true;
                    txtFirstName.CssClass = WebControlConstant.CSS_CLASS_READONLY;
                    txtMiddleName.ReadOnly = true;
                    txtMiddleName.CssClass = WebControlConstant.CSS_CLASS_READONLY;
                    txtLastName.ReadOnly = true;
                    txtLastName.CssClass = WebControlConstant.CSS_CLASS_READONLY;
                    txtFullName.ReadOnly = true;
                    txtFullName.CssClass = WebControlConstant.CSS_CLASS_READONLY;
                }
                else
                {
                    //Set current row's input fields as editable.
                    txtFirstName.ReadOnly = false;
                    txtFirstName.CssClass = null;
                    txtMiddleName.ReadOnly = false;
                    txtMiddleName.CssClass = null;
                    txtLastName.ReadOnly = false;
                    txtLastName.CssClass = null;
                    txtFullName.ReadOnly = false;
                    txtFullName.CssClass = null;
                }

                //Render the row structure and data columns and input controls.
                writer.Write("<tr rowIdx='" + i + "'><td class='" + (IsFirstNameHidden ? "ACA_Hide " : string.Empty) + "data'>");
                txtFirstName.RenderControl(writer);
                writer.Write("</td><td class='" + (IsMiddleNameHidden ? "ACA_Hide " : string.Empty) + "data'>");
                txtMiddleName.RenderControl(writer);
                writer.Write("</td><td class='" + (IsLastNameHidden ? "ACA_Hide " : string.Empty) + "data'>");
                txtLastName.RenderControl(writer);
                writer.Write("</td><td class='" + (IsFullNameHidden ? "ACA_Hide " : string.Empty) + "data'>");
                txtFullName.RenderControl(writer);
                writer.Write("</td><td class='action'><div class='del_button'>");

                /*
                 * Render the delete link.
                 * If in admin side, will show the Delete link directly.
                 * If in daily side, will hide the Delete link for these scenarios below:
                 *  1. row count <= 1, only 1 row disallow to delete.
                 *  2. current row is reference data.
                 *  3. current AKA is readonly.
                 */
                string delLinkStatus = string.Empty;

                if (!AccelaControlRender.IsAdminRender(this) && (rowIdxes.Count() <= 1 || isReferenceData || state.ReadOnly))
                {
                    delLinkStatus = " style='display:none;' tabindex='-1'";
                }

                writer.Write(
                    "<a class='NotShowLoading' onclick='if(typeof(SetNotAsk)!=\"undefined\")SetNotAsk();' href='javascript:DelAKAItem(\"" + listId + "\"," + i + ");'{0}>" + labelDelete + "</a>",
                    delLinkStatus);

                writer.Write("</div></td></tr>");
            }

            writer.Write("</tbody>");
            writer.Write("<tfoot><tr><td colspan='4' class='action'><div class='add_button'>");

            /*
             * Render the add link.
             * Hide the Add link if the table is readonly, otherwise show the Add link.
             */
            string addLinkStatus = string.Empty;

            if (state.ReadOnly)
            {
                addLinkStatus = " style='display:none' tabindex='-1'";
            }

            writer.Write(
                "<a class='NotShowLoading' onclick='if(typeof(SetNotAsk)!=\"undefined\")SetNotAsk();' href='javascript:AddAKAItem(\"" + this.ClientID + "_list\");'{0}>" + labelAdd + "</a>",
                addLinkStatus);

            writer.Write("</div></td></tr></tfoot>");
            writer.Write("</table>");

            //Render other controls. Such as State control or extender controls.
            foreach (Control ctl in Controls)
            {
                if (!(ctl is TextBox))
                {
                    ctl.RenderControl(writer);
                }
            }
        }

        #endregion

        #region Embedded Classes

        /// <summary>
        /// Define a class for the AKA client state.
        /// This class be used to exchange state data between server side and client scripts.
        /// </summary>
        public class AKAClientState
        {
            /// <summary>
            /// Gets or sets the row indexes.
            /// </summary>
            public IEnumerable<string> RowIndexes
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the AKA unique IDs.
            /// </summary>
            public IDictionary<string, string> AKAIDs
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the AKA full names.
            /// </summary>
            public IDictionary<string, string> FullNames
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the AKA start date.
            /// </summary>
            public IDictionary<string, string> StartDates
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the AKA end date..
            /// </summary>
            public IDictionary<string, string> EndDates
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the AKA field is readonly.
            /// </summary>
            public bool ReadOnly
            {
                get;
                set;
            }
        }

        #endregion
    }
}