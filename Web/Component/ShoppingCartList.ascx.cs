#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ShoppingCartList.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ShoppingCartList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Shopping cart page class.
    /// </summary>
    public partial class ShoppingCartList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Shopping cart items.
        /// </summary>
        private List<ShoppingCartItemModel4WS> _shoppingCartItems = new List<ShoppingCartItemModel4WS>();

        /// <summary>
        /// Address list count.
        /// </summary>
        private int _addressListCount = 0;

        /// <summary>
        /// Weather is pay now section.
        /// </summary>
        private bool _isSelected = false;

        /// <summary>
        /// weather is super agency.
        /// </summary>
        private bool _isSuperAgency = false;

        /// <summary>
        /// whether show payment method
        /// </summary>
        private bool _isShowPaymentMethod = false;

        /// <summary>
        /// SimpleViewElementModel4WS model list.
        /// </summary>
        private Hashtable simpleViewElements = new Hashtable();

        #endregion Fields

        #region Events

        /// <summary>
        /// remove command handler.
        /// </summary>
        public event EventHandler ShoppingCartRemoveCommand;

        /// <summary>
        /// update command handler.
        /// </summary>
        public event EventHandler ShoppingCartUpdateCommand;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the shopping cart item in pay now section or not.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether payment types have inserting or not.
        /// </summary>
        public bool HasIntersectPaymentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets in available exam fee items.
        /// </summary>
        public F4FeeItemModel4WS[] InavailableExamFeeItems
        {
            get; 
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind Data to shopping cart.
        /// </summary>
        /// <param name="cart">shopping cart model.</param>
        public void ShowShoppingCart(ShoppingCartModel4WS cart)
        {
            IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();
            List<ShoppingCartItemModel4WS> addressArray = null;

            if (cart == null || cart.shoppingCartItems == null || cart.shoppingCartItems.Length == 0)
            {
                addressList.DataSource = null;
                addressList.DataBind();
                return;
            }
            
            ArrayList moduleList = new ArrayList();
            List<CapIDModel4WS> capIDModels = new List<CapIDModel4WS>();
            List<string> moduleNames = new List<string>();

            foreach (ShoppingCartItemModel4WS shoppingCartItem in cart.shoppingCartItems)
            {
                _shoppingCartItems.Add(shoppingCartItem);
                moduleList.Add(shoppingCartItem.capType.moduleName);
                if (shoppingCartItem.paymentFlag == ACAConstant.COMMON_Y)
                {
                    capIDModels.Add(shoppingCartItem.capID);

                    if (!moduleNames.Contains(shoppingCartItem.capType.moduleName))
                    {
                        moduleNames.Add(shoppingCartItem.capType.moduleName);
                    }
                }
            }

            _isShowPaymentMethod = !ShoppingCartUtil.HasSamePaymentTypes(moduleNames) && IsSelected;
            if (_isShowPaymentMethod)
            {
                List<ListItem> paymentTypes = ShoppingCartUtil.GetIntersectantPaymentTypes(moduleNames);
                MessageType messageType = paymentTypes.Count > 0 ? MessageType.Notice : MessageType.Error;
                HasIntersectPaymentType = paymentTypes.Count > 0;
                string msg = ShoppingCartUtil.GetMessageWithDifferencePaymentTypes(TempModelConvert.Trim4WSOfCapIDModels(capIDModels.ToArray()));
                MessageUtil.ShowMessage(this.Page, messageType, msg);
            }
            else
            {
                HasIntersectPaymentType = true;
            }
            
            simpleViewElements = ShoppingCartUtil.GetGviewElementByModules(moduleList);
            addressArray = shoppingCartBll.GetAddressByShoppingCartItems(_shoppingCartItems, simpleViewElements);
            _addressListCount = addressArray.Count;
            addressList.DataSource = addressArray;
            addressList.DataBind();
        }

        /// <summary>
        /// Get Fee Description.
        /// </summary>
        /// <param name="objResFeeDescription">Res Fee Description</param>
        /// <param name="objFeeDescription">Fee Description</param>
        /// <returns>Res Fee Description.</returns>
        protected string GetFeeDescription(object objResFeeDescription, object objFeeDescription)
        {
            string resFeeDescription = (string)objResFeeDescription;
            string feeDescription = (string)objFeeDescription;
            return I18nStringUtil.GetString(resFeeDescription, feeDescription);
        }

        /// <summary>
        /// Initializes weather is super agency or not.
        /// </summary>
        /// <param name="sender">System sender.</param>
        /// <param name="e">The event args.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            _isSuperAgency = StandardChoiceUtil.IsSuperAgency();
        }

        /// <summary>
        /// Bind Address to UI.
        /// </summary>
        /// <param name="sender">System sender.</param>
        /// <param name="e">The data list event args.</param>
        protected void AddressList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label lblApplicationNumber = (Label)e.Item.FindControl("lblApplicationNumber");
            Label lblAddressTotalFee = (Label)e.Item.FindControl("lblAddressTotalFee");
            Label lblAddress = (Label)e.Item.FindControl("lblAddress");
            DataList capList = (DataList)e.Item.FindControl("capList");
            HtmlContainerControl hrDivision = (HtmlContainerControl)e.Item.FindControl("hrDivision");
            ShoppingCartItemModel4WS shoppingCartItem = (ShoppingCartItemModel4WS)e.Item.DataItem;
            string addressName = ShoppingCartUtil.FormatAddress(shoppingCartItem.address, (SimpleViewElementModel4WS[])simpleViewElements[shoppingCartItem.capType.moduleName]);

            if (shoppingCartItem.address == null || string.IsNullOrEmpty(addressName))
            {
                lblAddress.Text = GetTextByKey("per_shoppingcart_label_noaddressselected");
            }
            else
            {
                lblAddress.Text = addressName;
            }

            if (e.Item.ItemIndex == _addressListCount - 1 && !IsSelected)
            {
                hrDivision.Visible = false;
            }

            List<ShoppingCartItemModel4WS> caps = ConstrustCapData(addressName);
            lblAddressTotalFee.Text = I18nNumberUtil.FormatMoneyForUI(GetAddressTotalFee(caps));

            if (caps != null && caps.Count > 0)
            {
                lblApplicationNumber.Text = caps.Count.ToString();
                capList.DataSource = caps;
                capList.DataBind();
            }
        }

        /// <summary>
        /// Click cap list button event.
        /// </summary>
        /// <param name="sender">System Sender.</param>
        /// <param name="e">The data list event args.</param>
        protected void CapList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            string cartSeqNumber = ((HiddenField)e.Item.FindControl("hdnCartSeqNumber")).Value;
            string capClass = ((HiddenField)e.Item.FindControl("hdnCapClass")).Value;

            DataListItem itemCap = e.Item;
            CapIDModel4WS capIDModel = GetCapID(itemCap);

            ShoppingCartModel4WS shoppingCart = null;
            string paymentStatus = IsSelected ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            if (e.CommandName == "EditApplication")
            {
                string hdnModuleName = ((HiddenField)e.Item.FindControl("hdnModuleName")).Value;
                string hdnRenewStatus = ((HiddenField)e.Item.FindControl("hdnRenewStatus")).Value;
                string url;

                if (string.Equals(hdnRenewStatus, ACAConstant.CAP_RENEWAL))
                {
                    CapIDModel parentCapIdModel = capBll.GetParentCapIDByChildCapID(TempModelConvert.Trim4WSOfCapIDModel(capIDModel), ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE);
                    CapModel4WS capModel = capBll.CreateOrGetRenewalPartialCap(TempModelConvert.Add4WSForCapIDModel(parentCapIdModel));

                    // Check to see if there is a valid page flow group associated with the capModel.
                    // If not, display an error message and cancel the operation.
                    if (!PageFlowUtil.HasAssociatedPageFlowGroup(capModel))
                    {
                        string msg = LabelUtil.GetTextByKey("per_permitList_error_noRelatedPageflowGroup", capModel.moduleName);
                        MessageUtil.ShowMessageByControl(Page, MessageType.Error, msg);
                        return;
                    }

                    CapUtil.FillCapModelTemplateValue(capModel);
                    CapUtil.SetCapInfoToAppSession(capModel, capModel.capType, hdnModuleName);

                    url = string.Format("../Cap/CapEdit.aspx?permitType=renewal&Module={0}&TabName={1}&stepNumber=2&pageNumber=1&isFeeEstimator={2}&isRenewal=Y&isFromShoppingCart={3}", hdnModuleName, hdnModuleName, ACAConstant.COMMON_N, ACAConstant.COMMON_Y);
                }
                else
                {
                    CapWithConditionModel4WS capWithConditionModel = capBll.GetCapViewBySingle(capIDModel, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);
                    CapModel4WS capModel = capWithConditionModel.capModel;

                    // Check to see if there is a valid page flow group associated with the capModel.
                    // If not, display an error message and cancel the operation.
                    if (!PageFlowUtil.HasAssociatedPageFlowGroup(capModel))
                    {
                        string msg = LabelUtil.GetTextByKey("per_permitList_error_noRelatedPageflowGroup", capModel.moduleName);
                        MessageUtil.ShowMessageByControl(Page, MessageType.Error, msg);
                        return;
                    }

                    CapUtil.AdjustCapModelForShoppingCart(capModel);

                    CapUtil.FillCapModelTemplateValue(capModel);

                    CapUtil.SetCapInfoToAppSession(capModel, capModel.capType, hdnModuleName);

                    PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

                    string stepNumber = (pageflowGroup.stepList.Length + 2).ToString();

                    if (ACAConstant.INCOMPLETE_EST.Equals(capModel.capClass, StringComparison.OrdinalIgnoreCase))
                    {
                        url = "../Cap/CapConfirm.aspx?Module=" + hdnModuleName + "&TabName=" + hdnModuleName +
                              "&isFromShoppingCart=" + ACAConstant.COMMON_Y + "&paymentStatus=" + paymentStatus +
                              "&stepNumber=" + stepNumber;
                    }
                    else
                    {
                        //For HazMat special logic - The incomplete cap can be add to shopping cart because the related caps must be add/delete to/from shopping cart by group.
                        url = "../Cap/CapEdit.aspx?Module=" + hdnModuleName + "&TabName=" + hdnModuleName +
                              "&isFromShoppingCart=" + ACAConstant.COMMON_Y + "&paymentStatus=" + paymentStatus +
                              "&stepNumber=2&pageNumber=1";
                    }

                    //Shopping cart only have sub agency cap.
                    if (StandardChoiceUtil.IsSuperAgency())
                    {
                        url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
                    }

                    //Reset the Page Trace of the current section in Cart List. 
                    PageFlowUtil.ResetPageTrace(capModel);
                }

                url += "&" + UrlConstant.AgencyCode + "=" + capIDModel.serviceProviderCode;

                //Clear the auto-fill owners info.
                Session[SessionConstant.APO_SESSION_PARCELMODEL] = null;
                Response.Redirect(url);
            }
            else
            {
                CapIDModel4WS[] capIDModels = new[] { capIDModel };

                if (CapUtil.IsPartialCap(capClass))
                {
                    //Associated Forms Parent CAP
                    if (e.Item.Attributes["IsAssoFormParent"] != null && bool.Parse(e.Item.Attributes["IsAssoFormParent"]))
                    {
                        CapIDModel4WS[] childCapIDs = capBll.GetChildCaps(capIDModel);

                        if (childCapIDs != null && childCapIDs.Length > 0)
                        {
                            List<CapIDModel4WS> capIDs = new List<CapIDModel4WS>(childCapIDs);
                            capIDs.Insert(0, capIDModel);
                            capIDModels = capIDs.ToArray();
                        }
                    }
                }

                switch (e.CommandName)
                {
                    case "SaveLater":
                        shoppingCart = ConstructShoppingCartModel(capIDModels, cartSeqNumber, 1);
                        shoppingCartBll.UpdateShoppingCart(ConfigManager.AgencyCode, shoppingCart, AppSession.User.PublicUserId);

                        if (ShoppingCartUpdateCommand != null)
                        {
                            ShoppingCartUpdateCommand(null, null);
                        }

                        break;
                    case "AddForPayment":
                        shoppingCart = ConstructShoppingCartModel(capIDModels, cartSeqNumber, 2);
                        shoppingCartBll.UpdateShoppingCart(ConfigManager.AgencyCode, shoppingCart, AppSession.User.PublicUserId);

                        if (ShoppingCartUpdateCommand != null)
                        {
                            ShoppingCartUpdateCommand(null, null);
                        }
                      
                        break;
                    case "RemoveApplication":
                        shoppingCart = ConstructShoppingCartModel(capIDModels, cartSeqNumber, 3);
                        shoppingCartBll.DeleteShoppingCartItem(ConfigManager.AgencyCode, shoppingCart, AppSession.User.PublicUserId);

                        if (ShoppingCartRemoveCommand != null)
                        {
                            ShoppingCartRemoveCommand(null, null);
                        }
                       
                        break;
                }
            }
        }

        /// <summary>
        /// Bind Cap list to UI.
        /// </summary>
        /// <param name="sender">The system sender.</param>
        /// <param name="e">The data list event args.</param>
        protected void CapList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            HtmlContainerControl divSelected = (HtmlContainerControl)e.Item.FindControl("divSelected");
            HtmlContainerControl divSaved = (HtmlContainerControl)e.Item.FindControl("divSaved");
            HiddenField hdnCartSeqNumber = (HiddenField)e.Item.FindControl("hdnCartSeqNumber");
            AccelaLinkButton btnSaveLater = (AccelaLinkButton)e.Item.FindControl("btnSaveLater");
            AccelaLinkButton btnAddForPayment = (AccelaLinkButton)e.Item.FindControl("btnAddForPayment");
            AccelaLinkButton btnEditApplication = (AccelaLinkButton)e.Item.FindControl("btnEditApplication");
            AccelaLinkButton btnRemove = (AccelaLinkButton)e.Item.FindControl("btnRemove");
            AccelaLabel lblCAPTotalFee = (AccelaLabel)e.Item.FindControl("lblCAPTotalFee");
            AccelaLabel lblCapID = (AccelaLabel)e.Item.FindControl("lblCapID");
            AccelaLabel lblCapName = (AccelaLabel)e.Item.FindControl("lblCapName");
            HtmlTable tbFeeList = (HtmlTable)e.Item.FindControl("tbFeeList");
            HtmlTableCell tdAgency = (HtmlTableCell)e.Item.FindControl("tdAgency");
            HtmlTableCell tdCapType = (HtmlTableCell)e.Item.FindControl("tdCapType");
            AccelaGridView feeList = (AccelaGridView)e.Item.FindControl("feeList");
            HtmlImage imgShowFeeItem = (HtmlImage)e.Item.FindControl("imgShowFeeItem");
            HtmlAnchor linkShowFeeItem = (HtmlAnchor)e.Item.FindControl("linkShowFeeItem");
            HtmlImage imgAgencyLogo = (HtmlImage)e.Item.FindControl("imgAgencyLogo");
            HtmlTableCell tbAgencyLogo = (HtmlTableCell)e.Item.FindControl("tbAgencyLogo");
            HtmlImage imgErrRecord = (HtmlImage)e.Item.FindControl("imgErrCap4Payment");
            linkShowFeeItem.Title = GetTitleByKey("img_alt_expand_icon", string.Empty);
            divSelected.Visible = IsSelected;
            divSaved.Visible = !IsSelected;

            tbFeeList.Style["display"] = "none";
            btnRemove.Attributes["onclick"] = "return confirm('" + GetTextByKey("per_shoppingcart_message_comfirmremove").Replace("'", "\\'") + "')";

            ShoppingCartItemModel4WS shoppingCartItem = (ShoppingCartItemModel4WS)e.Item.DataItem;

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            if (!_isSuperAgency)
            {
                tdAgency.Visible = false;
                tdCapType.Style.Add(HtmlTextWriterStyle.Width, "52%");
            }
            else
            {
                ILogoBll logo = ObjectFactory.GetObject<ILogoBll>();
                LogoModel logoModel = logo.GetAgencyLogo(shoppingCartItem.capID.serviceProviderCode);

                if (logoModel != null)
                {
                    imgAgencyLogo.Src = FileUtil.AppendApplicationRoot("Cap/ImageHandler.aspx?") + UrlConstant.AgencyCode + "=" + logoModel.serviceProviderCode;
                    imgAgencyLogo.Alt = GetTextByKey("aca_common_msg_imgalt_agencylogo");

                    if (!string.IsNullOrEmpty(logoModel.docDesc))
                    {
                        imgAgencyLogo.Alt = logoModel.docDesc;
                    }
                }
                else
                {
                    tbAgencyLogo.Visible = false;
                }
            }

            if (string.Equals(shoppingCartItem.processType, ACAConstant.CAP_PAYFEEDUE) || string.Equals(shoppingCartItem.processType, ACAConstant.PAYFEEDUE_RENEWAL))
            {
                btnEditApplication.Visible = false;
            }

            hdnCartSeqNumber.Value = shoppingCartItem.cartSeqNumber.ToString();

            if (string.Equals(shoppingCartItem.processType, ACAConstant.CAP_RENEWAL) || string.Equals(shoppingCartItem.processType, ACAConstant.PAYFEEDUE_RENEWAL))
            {
                CapIDModel parentCapIdModel = capBll.GetParentCapIDByChildCapID(TempModelConvert.Trim4WSOfCapIDModel(shoppingCartItem.capID), ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE);
                
                if (parentCapIdModel != null)
                {
                    CapWithConditionModel4WS parentCapWithConditionModel = capBll.GetCapViewBySingle(TempModelConvert.Add4WSForCapIDModel(parentCapIdModel), AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);
                    ContactUtil.InitializeContactsGroup4CapModel(parentCapWithConditionModel.capModel);
                    CapModel4WS parentCapModel = parentCapWithConditionModel.capModel;

                    //display custom id in order to go though with cap list page.
                    lblCapID.Text = parentCapModel.capID.customID;

                    if (parentCapWithConditionModel.capModel != null && parentCapWithConditionModel.capModel.capType != null)
                    {
                        lblCapName.Text = CAPHelper.GetAliasOrCapTypeLabel(parentCapWithConditionModel.capModel.capType);
                    }
                }
            }
            else
            {
                //display custom id in order to go though with cap list page.
                lblCapID.Text = shoppingCartItem.capID.customID;
                lblCapName.Text = CAPHelper.GetAliasOrCapTypeLabel(shoppingCartItem.capType);
            }

            if (CapUtil.IsPartialCap(shoppingCartItem.capClass))
            {
                //The AssociatedForms's child CAPs couldn't be PayNow/PayLater/Remove by individually,so hide the SaveLater button for AssociatedForms's child CAPs.
                //This page maybe is not come from CapEdit, so couldn't get parent capID from Session.
                CapIDModel4WS parentCapID = CapUtil.GetParentAssoFormCapID(shoppingCartItem.capID);
                CapIDModel4WS childCapID = capBll.GetChildCapIDByMasterID(shoppingCartItem.capID, ACAConstant.CAP_RELATIONSHIP_ASSOFORM, null, false);

                if (!parentCapID.Equals(shoppingCartItem.capID))
                {
                    btnSaveLater.Visible = false;
                    btnAddForPayment.Visible = false;
                    btnRemove.Visible = false;
                }
                else if (childCapID != null)
                {
                    e.Item.Attributes.Add("IsAssoFormParent", true.ToString());
                }
            }

            lblCAPTotalFee.Text = I18nNumberUtil.FormatMoneyForUI(shoppingCartItem.totalFee);

            List<F4FeeItemModel4WS> feeItems = new List<F4FeeItemModel4WS>();

            feeItems = GetFeeItems(shoppingCartItem);
            bool hasFeeItems = !(feeItems == null || feeItems.Count == 0);

            if (hasFeeItems)
            {
                if (!Enable2PaymentExamFee(feeItems))
                {
                    imgShowFeeItem.Visible = true;
                    imgErrRecord.Visible = true;
                    imgErrRecord.Src = ImageUtil.GetImageURL("error_24.gif");
                    imgErrRecord.Alt = GetTextByKey("img_alt_mark_required");
                    imgShowFeeItem.Src = ImageUtil.GetImageURL("caret_expanded.gif");
                    imgShowFeeItem.Alt = GetTextByKey("img_alt_collapse_icon");
                    linkShowFeeItem.Title = GetTitleByKey("img_alt_collapse_icon", string.Empty);
                    tbFeeList.Style["display"] = string.Empty;
                }
                else
                {
                    imgShowFeeItem.Src = ImageUtil.GetImageURL("caret_collapsed.gif");
                    imgShowFeeItem.Alt = GetTextByKey("img_alt_expand_icon");
                }
                
                linkShowFeeItem.Attributes["onclick"] = "ShowFee('" + tbFeeList.ClientID + "','" + imgShowFeeItem.ClientID + "','" + linkShowFeeItem.ClientID + "')";
            }
            else
            {
                linkShowFeeItem.Visible = false;
                imgShowFeeItem.Visible = false;
            }

            feeList.DataSource = feeItems;
            feeList.DataBind();
            if (_isShowPaymentMethod)
            {
                HtmlTable tablePaymentMethod = e.Item.FindControl("tablePaymentMethod") as HtmlTable;
                tablePaymentMethod.Visible = true;
                List<string> paymentMethodItems = ShoppingCartUtil.GetPaymentMethodByModule(shoppingCartItem.capType.moduleName);
                AccelaLabel lblPaymentMethodItem = e.Item.FindControl("lblPaymentMethodItem") as AccelaLabel;
                lblPaymentMethodItem.Text = string.Join(", ", paymentMethodItems);
            }            
        }

        /// <summary>
        /// Bind Fee List to UI.
        /// </summary>
        /// <param name="sender">The System Sender.</param>
        /// <param name="e">The data list event args.</param>
        protected void FeeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HiddenField feeSeqNbr = (HiddenField)e.Row.FindControl("hidFieldFeeSeqNbr");
            string feeSeqNum = feeSeqNbr == null ? string.Empty : feeSeqNbr.Value;

            if (InavailableExamFeeItems != null && InavailableExamFeeItems.Length > 0 
                && InavailableExamFeeItems.Any(feeItemModel => feeSeqNum.Equals(feeItemModel.feeSeqNbr.ToString(CultureInfo.InvariantCulture))))
            {
                HtmlImage errFeeItem = (HtmlImage)e.Row.FindControl("imgErrFeeItem");
                errFeeItem.Visible = true;
                errFeeItem.Src = ImageUtil.GetImageURL("error_16.gif");
                errFeeItem.Alt = GetTextByKey("img_alt_mark_required");
            }

            if (_isSuperAgency)
            {
                HtmlContainerControl spanAccountHeader = (HtmlContainerControl)e.Row.FindControl("spanAccountHeader");
                HtmlContainerControl spanAccountItem = (HtmlContainerControl)e.Row.FindControl("spanAccountItem");

                if (spanAccountHeader != null)
                {
                    spanAccountHeader.Attributes["class"] = "ACA_FRight ACA_Super_ShoppingAccountHeader";
                }

                if (spanAccountItem != null)
                {
                    spanAccountItem.Attributes["class"] = "ACA_FRight ACA_Super_ShoppingAccountItem";
                }
                
                e.Row.Cells[1].Style["Width"] = "27%";
                e.Row.Cells[2].Style["Width"] = "69%";
            }
        }

        /// <summary>
        /// Get cap type label from the sequence
        /// </summary>
        /// <param name="objCapType">CapTypeModel entity</param>
        /// <returns>Alias value Or CapTypeLabel value</returns>
        protected string ShowCapType(object objCapType)
        {
            string result = string.Empty;

            if (objCapType != null)
            {
                CapTypeModel capModel4Ws = objCapType as CapTypeModel;
                result = CAPHelper.GetAliasOrCapTypeLabel(capModel4Ws);
            }

            return result;
        }

        /// <summary>
        /// Construct Shopping Cart Model pass to java to update database.
        /// </summary>
        /// <param name="capIDModels">Cap id models.</param>
        /// <param name="cartSeqNumber">Shopping cart sequence number.</param>
        /// <param name="status">Pay now or pay later.</param>
        /// <returns>Shopping cart model.</returns>
        private ShoppingCartModel4WS ConstructShoppingCartModel(CapIDModel4WS[] capIDModels, string cartSeqNumber, int status)
        {
            ShoppingCartModel4WS shoppingCart = new ShoppingCartModel4WS();
            shoppingCart.cartSeqNumber = long.Parse(cartSeqNumber);

            ShoppingCartItemModel4WS[] shoppingCartItems = new ShoppingCartItemModel4WS[capIDModels.Length];

            for (int i = 0; i < capIDModels.Length; i++)
            {
                ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();

                cartItem.cartSeqNumber = long.Parse(cartSeqNumber);
                cartItem.capID = capIDModels[i];
                cartItem.servProvCode = capIDModels[i].serviceProviderCode;

                switch (status)
                {
                    case 1:
                        //Pay later
                        cartItem.paymentFlag = ACAConstant.COMMON_N;
                        break;
                    case 2:
                        //Pay now
                        cartItem.paymentFlag = ACAConstant.COMMON_Y;
                        break;
                }

                shoppingCartItems[i] = cartItem;
            }

            shoppingCart.shoppingCartItems = shoppingCartItems;

            return shoppingCart;
        }

        /// <summary>
        /// Get Shopping Cart Items group by address.
        /// </summary>
        /// <param name="addressName">The address name.</param>
        /// <returns>The shopping cart item model list.</returns>
        private List<ShoppingCartItemModel4WS> ConstrustCapData(string addressName)
        {
            List<ShoppingCartItemModel4WS> caps = new List<ShoppingCartItemModel4WS>();

            if (_shoppingCartItems == null || _shoppingCartItems.Count <= 0)
            {
                return caps;
            }

            //SortedList list = new SortedList();
            if (string.IsNullOrEmpty(addressName))
            {
                foreach (ShoppingCartItemModel4WS shoppingCartItem in _shoppingCartItems)
                {
                    if (shoppingCartItem.address == null || string.IsNullOrEmpty(ShoppingCartUtil.FormatAddress(shoppingCartItem.address, (SimpleViewElementModel4WS[])simpleViewElements[shoppingCartItem.capType.moduleName])))
                    {
                        caps.Add(shoppingCartItem);
                    }
                }
            }
            else
            {
                foreach (ShoppingCartItemModel4WS shoppingCartItem in _shoppingCartItems)
                {
                    if (shoppingCartItem.address != null && ShoppingCartUtil.FormatAddress(shoppingCartItem.address, (SimpleViewElementModel4WS[])simpleViewElements[shoppingCartItem.capType.moduleName]) == addressName)
                    {
                        caps.Add(shoppingCartItem);
                    }
                }
            }

            return caps;
        }

        /// <summary>
        /// Get Total fee of the same address section.
        /// </summary>
        /// <param name="caps">shopping cart items.</param>
        /// <returns>total fee of the caps.</returns>
        private double GetAddressTotalFee(List<ShoppingCartItemModel4WS> caps)
        {
            double addressTotalFee = 0;
            for (int i = 0; i < caps.Count; i++)
            {
                addressTotalFee += caps[i].totalFee;
            }

            return addressTotalFee;
        }

        /// <summary>
        /// Get Fee Items of the shopping cart item.
        /// </summary>
        /// <param name="shoppingCartItem">Shopping cart item model</param>
        /// <returns>Fee item of the cap.</returns>
        private List<F4FeeItemModel4WS> GetFeeItems(ShoppingCartItemModel4WS shoppingCartItem)
        {
            if (shoppingCartItem.feeItems == null)
            {
                return null;
            }

            var feeItems = new List<F4FeeItemModel4WS>();
            var isOnlyShowAutoInvoiceFeeItems = StandardChoiceUtil.IsOnlyShowAutoInvoiceFeeItems(shoppingCartItem.capID.serviceProviderCode, ModuleName);

            foreach (F4FeeItemModel4WS feeItem in shoppingCartItem.feeItems)
            {
                // When auto invoice disabled and current module is not in the enabled auto-invoice module list, ACA only shows those fee item with auto invoice flag is y.
                if (!ACAConstant.CAP_PAYFEEDUE.Equals(shoppingCartItem.processType, StringComparison.InvariantCultureIgnoreCase)
                    && isOnlyShowAutoInvoiceFeeItems
                    && !ValidationUtil.IsYes(feeItem.autoInvoiceFlag))
                {
                    continue;
                }

                feeItems.Add(feeItem);
            }

            return feeItems;
        }

        /// <summary>
        /// Construct CapIDModel by shopping cart Item.
        /// </summary>
        /// <param name="itemCap">The date list item.</param>
        /// <returns>Cap id model.</returns>
        private CapIDModel4WS GetCapID(DataListItem itemCap)
        {
            HiddenField hdnCapID1 = (HiddenField)itemCap.FindControl("hdnCapID1");
            HiddenField hdnCapID2 = (HiddenField)itemCap.FindControl("hdnCapID2");
            HiddenField hdnCapID3 = (HiddenField)itemCap.FindControl("hdnCapID3");
            HiddenField hdnAgence = (HiddenField)itemCap.FindControl("hdnAgence");
            CapIDModel4WS childCapID = new CapIDModel4WS();
            childCapID.id1 = hdnCapID1.Value;
            childCapID.id2 = hdnCapID2.Value;
            childCapID.id3 = hdnCapID3.Value;
            childCapID.serviceProviderCode = hdnAgence.Value;
            return childCapID;
        }

        /// <summary>
        /// Check if the cap should be payment the exam fees
        /// </summary>
        /// <param name="feeItems">Fee Items</param>
        /// <returns>Return true if the cap should be enable to payment it's exam fees</returns>
        private bool Enable2PaymentExamFee(List<F4FeeItemModel4WS> feeItems)
        {
            if (InavailableExamFeeItems == null || InavailableExamFeeItems.Length == 0 || feeItems == null || feeItems.Count == 0)
            {
                return true;
            }

            //Because there is in-available exam fee existing, so can not go to payment.
            return feeItems.All(feeItem => !InavailableExamFeeItems.Any(item => feeItem.capID.Equals(item.capID) && feeItem.feeSeqNbr == item.feeSeqNbr));
        }

        #endregion Methods
    }
}
