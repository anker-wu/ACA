#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TopShoppingCartItems.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: TopShoppingCartItems.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Top Shopping cart Class.
    /// </summary>
    public partial class TopShoppingCartItems : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The items count display at welcome page.
        /// </summary>
        private int itemsCount = 5;

        /// <summary>
        /// SimpleViewElementModel4WS model list.
        /// </summary>
        private Hashtable simpleViewElements = new Hashtable();

        #endregion Fields

        #region Methods
        
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!StandardChoiceUtil.IsEnableShoppingCart())
            {
                return;
            }

            Hashtable htShoppingCartItems = null;

            //configure label key in admin.
            bool isAdminModeRegistered = !string.IsNullOrEmpty(Request["registered"]);
            if (isAdminModeRegistered)
            {
                lblCartName.Visible = true;
                lblEmpty.Visible = true;
            }

            if (AppSession.IsAdmin || AppSession.User == null || AppSession.User.IsAnonymous)
            {
                return;
            }

            if (!IsPostBack)
            {
                htShoppingCartItems = GetShoppingCart();
                GetGviewElementByModules(htShoppingCartItems);
                List<ShoppingCartItemModel4WS> capsArray = ConstrustCapsData(htShoppingCartItems);
                BindCartNumber();

                if (capsArray == null || capsArray.Count == 0)
                {
                    lblEmpty.Visible = true;
                    divCaps.Visible = true;
                    btnMore.Visible = false;
                }
                else
                {
                    capList.DataSource = capsArray;
                    capList.DataBind();
                }
            }
        }

        /// <summary>
        /// Raise More button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void MoreButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShoppingCart/ShoppingCart.aspx?TabName=Home&stepNumber=2");
        }

        /// <summary>
        /// Bind Cap to UI
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void CapList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            AccelaLabel lblCapID = (AccelaLabel)e.Item.FindControl("lblCapID");
            ShoppingCartItemModel4WS shoppingCartItem = (ShoppingCartItemModel4WS)e.Item.DataItem;
            AccelaLabel lblCAPTotalFee = (AccelaLabel)e.Item.FindControl("lblCAPTotalFee");
            if (string.Equals(shoppingCartItem.processType, ACAConstant.CAP_RENEWAL))
            {
                ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                CapIDModel parentCapIdModel = capBll.GetParentCapIDByChildCapID(TempModelConvert.Trim4WSOfCapIDModel(shoppingCartItem.capID), ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE);
                if (parentCapIdModel != null)
                {
                    CapWithConditionModel4WS parentCapWithConditionModel = capBll.GetCapViewBySingle(TempModelConvert.Add4WSForCapIDModel(parentCapIdModel), AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);
                    CapModel4WS parentCapModel = parentCapWithConditionModel.capModel;

                    // Display custom id in order to go though with cap list page.
                    lblCapID.Text = parentCapModel.capID.customID;
                }
            }
            else
            {
                lblCapID.Text = shoppingCartItem.capID.customID == null ? string.Empty : shoppingCartItem.capID.customID;
            }

            lblCAPTotalFee.Text = I18nNumberUtil.FormatMoneyForUI(shoppingCartItem.totalFee);
        }

        /// <summary>
        /// Show Shopping Cart Number
        /// </summary>
        private void BindCartNumber()
        {
            string shoppingCartItemNumber = AppSession.GetCartItemNumberFromSession();
            if (string.IsNullOrEmpty(shoppingCartItemNumber))
            {
                lblCartName.Text = string.Format("{0} ({1})", GetTextByKey("per_topshoppingcartitem_label_carttitle"), "0");
                btnMore.Visible = false;
            }
            else
            {
                lblCartName.Text = string.Format("{0} ({1})", GetTextByKey("per_topshoppingcartitem_label_carttitle"), shoppingCartItemNumber);
                btnMore.Visible = ValidationUtil.IsInt(shoppingCartItemNumber) && (Convert.ToInt32(shoppingCartItemNumber) > itemsCount);
            }
        }

        /// <summary>
        /// Set SimpleViewElements model.
        /// </summary>
        /// <param name="htShoppingCartItems">HT ShoppingCartItems</param>
        private void GetGviewElementByModules(Hashtable htShoppingCartItems)
        {
            if (htShoppingCartItems == null || htShoppingCartItems.Count < 1)
            {
                return;
            }

            ArrayList moduleList = new ArrayList(); 
            List<ShoppingCartItemModel4WS> selectedShoppingCartItems = GetShoppingItemsFromModel((ShoppingCartModel4WS)htShoppingCartItems[0]);
            List<ShoppingCartItemModel4WS> savedShoppingCartItems = GetShoppingItemsFromModel((ShoppingCartModel4WS)htShoppingCartItems[1]);

            if (selectedShoppingCartItems != null && selectedShoppingCartItems.Count > 0)
            {
                foreach (ShoppingCartItemModel4WS model in selectedShoppingCartItems)
                {
                    if (!moduleList.Contains(model.capType.moduleName))
                    {
                        moduleList.Add(model.capType.moduleName);
                    }
                }
            }

            if (savedShoppingCartItems != null && savedShoppingCartItems.Count > 0)
            {
                foreach (ShoppingCartItemModel4WS model in savedShoppingCartItems)
                {
                    if (!moduleList.Contains(model.capType.moduleName))
                    {
                        moduleList.Add(model.capType.moduleName);
                    }
                }
            }

            simpleViewElements = ShoppingCartUtil.GetGviewElementByModules(moduleList);
        }

        /// <summary>
        /// Get Shopping Cart Item Models, The Order must same as Shopping cart page.
        /// </summary>
        /// <param name="htShoppingCartModels">The Hashtable contains Shopping cart model.</param>
        /// <returns>Shopping cart Item model list.</returns>
        private List<ShoppingCartItemModel4WS> ConstrustCapsData(Hashtable htShoppingCartModels)
        {
            if (htShoppingCartModels == null)
            {
                return null;
            }

            List<ShoppingCartItemModel4WS> selectedShoppingCartItems = GetShoppingItemsFromModel((ShoppingCartModel4WS)htShoppingCartModels[0]);
            List<ShoppingCartItemModel4WS> savedShoppingCartItems = GetShoppingItemsFromModel((ShoppingCartModel4WS)htShoppingCartModels[1]);

            List<ShoppingCartItemModel4WS> selectedCapsArray = new List<ShoppingCartItemModel4WS>();
            List<ShoppingCartItemModel4WS> savedCapsArray = new List<ShoppingCartItemModel4WS>();
            List<ShoppingCartItemModel4WS> capsArray = new List<ShoppingCartItemModel4WS>();

            selectedCapsArray = GetCapsArrayGroupByAddress(selectedShoppingCartItems);
            int capsCount = 0;
            int cartNumer = 0;

            if (selectedCapsArray != null)
            {
                cartNumer += selectedCapsArray.Count;
                foreach (ShoppingCartItemModel4WS selectedCaps in selectedCapsArray)
                {
                    if (capsCount >= itemsCount)
                    {
                        break;
                    }

                    capsArray.Add(selectedCaps);
                    capsCount++;
                }
            }

            savedCapsArray = GetCapsArrayGroupByAddress(savedShoppingCartItems);

            if (savedCapsArray != null)
            {
                cartNumer += savedCapsArray.Count;
                foreach (ShoppingCartItemModel4WS savedCaps in savedCapsArray)
                {
                    if (capsCount >= itemsCount)
                    {
                        break;
                    }

                    capsArray.Add(savedCaps);
                    capsCount++;
                }
            }

            AppSession.SetCartItemNumberToSession(cartNumer.ToString());
            return capsArray;
        }

        /// <summary>
        /// Get cap list group by address, InOrder to keep same order as shopping cart page.
        /// </summary>
        /// <param name="shoppingCartItems">Shopping cart Item list.</param>
        /// <returns>Shopping cart Item list group by address.</returns>
        private List<ShoppingCartItemModel4WS> GetCapsArrayGroupByAddress(List<ShoppingCartItemModel4WS> shoppingCartItems)
        {
            if (shoppingCartItems == null)
            {
                return null;
            }

            IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();
            List<ShoppingCartItemModel4WS> shoppingCartitemsGroupByAddress = new List<ShoppingCartItemModel4WS>();
            List<ShoppingCartItemModel4WS> capsArray = new List<ShoppingCartItemModel4WS>();

            //get shopping cart items which have different address.
            shoppingCartitemsGroupByAddress = shoppingCartBll.GetAddressByShoppingCartItems(shoppingCartItems, simpleViewElements);

            foreach (ShoppingCartItemModel4WS item in shoppingCartitemsGroupByAddress)
            {
                string parentAddress = ShoppingCartUtil.FormatAddress(item.address, (SimpleViewElementModel4WS[])simpleViewElements[item.capType.moduleName]);

                if (string.IsNullOrEmpty(parentAddress))
                {
                    //Caps have no address.
                    foreach (ShoppingCartItemModel4WS shoppingCartItem in shoppingCartItems)
                    {
                        if (shoppingCartItem.address == null || string.IsNullOrEmpty(ShoppingCartUtil.FormatAddress(shoppingCartItem.address, (SimpleViewElementModel4WS[])simpleViewElements[shoppingCartItem.capType.moduleName])))
                        {
                            capsArray.Add(shoppingCartItem);
                        }
                    }
                }
                else
                {
                    //Caps have address.
                    foreach (ShoppingCartItemModel4WS shoppingCartItem in shoppingCartItems)
                    {
                        if (ShoppingCartUtil.FormatAddress(shoppingCartItem.address, (SimpleViewElementModel4WS[])simpleViewElements[shoppingCartItem.capType.moduleName]) == parentAddress)
                        {
                            capsArray.Add(shoppingCartItem);
                        }
                    }
                }
            }

            return capsArray;
        }

        /// <summary>
        /// Get DataSource.
        /// </summary>
        /// <returns>The HashTable contains Shopping cart Model.</returns>
        private Hashtable GetShoppingCart()
        {
            Hashtable htShoppingCartItems = null;

            IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();

            if (AppSession.User.IsAnonymous)
            {
                return null;
            }

            htShoppingCartItems = shoppingCartBll.GetShoppingCart(ConfigManager.AgencyCode, long.Parse(AppSession.User.UserSeqNum), AppSession.User.PublicUserId);

            return htShoppingCartItems;
        }

        /// <summary>
        /// Get Shopping Cart Item Model from Shopping Cart Model.
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Shopping cart item model list.</returns>
        private List<ShoppingCartItemModel4WS> GetShoppingItemsFromModel(ShoppingCartModel4WS cart)
        {
            List<ShoppingCartItemModel4WS> shoppingCartItems = new List<ShoppingCartItemModel4WS>();

            if (cart == null || cart.shoppingCartItems == null || cart.shoppingCartItems.Length == 0)
            {
                return null;
            }

            foreach (ShoppingCartItemModel4WS shoppingCartItem in cart.shoppingCartItems)
            {
                shoppingCartItems.Add(shoppingCartItem);
            }

            return shoppingCartItems;
        }

        #endregion Methods
    }
}