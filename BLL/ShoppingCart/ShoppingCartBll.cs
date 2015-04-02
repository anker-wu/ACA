#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ShoppingCart.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: ShoppingCartBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.ShoppingCart
{
    /// <summary>
    /// This class provide the ability to operation shopping cart.
    /// </summary>
    internal class ShoppingCartBll : BaseBll, IShoppingCartBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of ShoppingCartService.
        /// </summary>
        private ShoppingCartWebServiceService ShoppingCartService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ShoppingCartWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create Shopping Cart
        /// </summary>
        /// <param name="shoppingCartItems">shopping Cart Item list</param>
        /// <param name="isMultiCap">Whether has child caps</param>
        /// <param name="isIncludeParent">Indicate whether create the parent CAP's shopping cart item</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void CreateShoppingCart(ShoppingCartItemModel4WS[] shoppingCartItems, bool isMultiCap, bool isIncludeParent)
        {
            try
            {
                ShoppingCartModel4WS shoppingCartModel = new ShoppingCartModel4WS();

                shoppingCartModel.userSeqNumber = long.Parse(User.UserSeqNum);
                shoppingCartModel.shoppingCartItems = shoppingCartItems;

                ShoppingCartService.createShoppingCart(AgencyCode, shoppingCartModel, isMultiCap, isIncludeParent, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Delete Shopping Cart Item 
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="shoppingCart">shopping Cart</param>
        /// <param name="callerID">caller ID number</param>    
        public void DeleteShoppingCartItem(string agencyCode, ShoppingCartModel4WS shoppingCart, string callerID)
        {
            try
            {
                ShoppingCartService.deleteShoppingCartItem(agencyCode, shoppingCart, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        ///  Get Address ByS hopping Cart Items
        /// </summary>
        /// <param name="allShoppingCartItems"> allShoppingCartItems List </param>
        /// <param name="simpleViewElements"> The SimpleViewElementModel4WS model list. </param>
        /// <returns>ShoppingCartItemModel4WS list </returns>
        public List<ShoppingCartItemModel4WS> GetAddressByShoppingCartItems(List<ShoppingCartItemModel4WS> allShoppingCartItems, Hashtable simpleViewElements)
        {
            List<ShoppingCartItemModel4WS> addressArray = new List<ShoppingCartItemModel4WS>();
            SortedList list = new SortedList();

            if (allShoppingCartItems == null)
            {
                return null;
            }

            string tempAddressName = string.Empty;
            bool hasEmptyAddressName = false;
            ShoppingCartItemModel4WS emptyAddressShoppingCartItem = new ShoppingCartItemModel4WS();
            string moduleName = string.Empty;

            for (int i = 0; i < allShoppingCartItems.Count; i++)
            {
                AddressModel address = allShoppingCartItems[i].address;
                moduleName = allShoppingCartItems[i].capType.moduleName;

                string addressName = FormatAddress(address, (SimpleViewElementModel4WS[])simpleViewElements[moduleName]);

                if (address == null || string.IsNullOrEmpty(addressName))
                {
                    hasEmptyAddressName = true;
                    emptyAddressShoppingCartItem = allShoppingCartItems[i];
                    continue;
                }
                else
                {
                    tempAddressName = addressName;
                }

                if (!list.ContainsKey(tempAddressName))
                {
                    list.Add(tempAddressName, allShoppingCartItems[i]);
                }
            }

            foreach (DictionaryEntry de in list)
            {
                addressArray.Add((ShoppingCartItemModel4WS)de.Value);
            }

            if (hasEmptyAddressName)
            {
                addressArray.Add(emptyAddressShoppingCartItem);
            }

            return addressArray;
        }

        /// <summary>
        /// get shoppingCart information .
        /// </summary>
        /// <param name="agencyCode">parent cap ID</param>
        /// <param name="userSeqNumber">user Sequence Number</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>Shopping Cart Item object.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public Hashtable GetShoppingCart(string agencyCode, long userSeqNumber, string callerID)
        {
            Hashtable htShoppingFilterModel = new Hashtable();
            ShoppingCartModel4WS selectedShoppingCart = new ShoppingCartModel4WS();
            ShoppingCartModel4WS savedShoppingCart = new ShoppingCartModel4WS();
            List<ShoppingCartItemModel4WS> selectedShoppingCartItems = new List<ShoppingCartItemModel4WS>();
            List<ShoppingCartItemModel4WS> savedShoppingCartItems = new List<ShoppingCartItemModel4WS>();

            try
            {
                ShoppingCartModel4WS shoppingCartModel = ShoppingCartService.getShoppingCart(agencyCode, userSeqNumber, callerID);

                if (shoppingCartModel == null || shoppingCartModel.shoppingCartItems == null)
                {
                    return null;
                }

                ShoppingCartItemModel4WS[] shoppingCartItems = shoppingCartModel.shoppingCartItems;

                foreach (ShoppingCartItemModel4WS shoppingCartItem in shoppingCartItems)
                {
                    if (shoppingCartItem.paymentFlag == "Y")
                    {
                        selectedShoppingCartItems.Add(shoppingCartItem);
                    }
                    else
                    {
                        savedShoppingCartItems.Add(shoppingCartItem);
                    }
                }

                selectedShoppingCart.recDate = shoppingCartModel.recDate;
                selectedShoppingCart.cartSeqNumber = shoppingCartModel.cartSeqNumber;
                selectedShoppingCart.recFullName = shoppingCartModel.recFullName;
                selectedShoppingCart.recStatus = shoppingCartModel.recStatus;
                selectedShoppingCart.shoppingCartItems = selectedShoppingCartItems.ToArray();
                htShoppingFilterModel.Add(0, selectedShoppingCart);

                savedShoppingCart.recDate = shoppingCartModel.recDate;
                savedShoppingCart.cartSeqNumber = shoppingCartModel.cartSeqNumber;
                savedShoppingCart.recFullName = shoppingCartModel.recFullName;
                savedShoppingCart.recStatus = shoppingCartModel.recStatus;
                savedShoppingCart.shoppingCartItems = savedShoppingCartItems.ToArray();
                htShoppingFilterModel.Add(1, savedShoppingCart);

                return htShoppingFilterModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get shopping cart id list
        /// </summary>
        /// <returns>cap id list</returns>
        public CapIDModel4WS[] GetShoppingCartCapIDs()
        {
            ArrayList capIds = new ArrayList();

            try
            {
                ShoppingCartModel4WS shoppingCartModel = ShoppingCartService.getShoppingCart(AgencyCode, long.Parse(User.UserSeqNum), User.PublicUserId);

                if (shoppingCartModel != null && shoppingCartModel.shoppingCartItems != null && shoppingCartModel.shoppingCartItems.Length > 0)
                {
                    foreach (ShoppingCartItemModel4WS item in shoppingCartModel.shoppingCartItems)
                    {
                        capIds.Add(item.capID);
                    }
                }

                return (CapIDModel4WS[])capIds.ToArray(typeof(CapIDModel4WS));
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get hashtable which key is cap id, value is cap status.
        /// </summary>
        /// <returns>cap id and cap status hashtable.</returns>
        public Hashtable GetCartItemWithCustomID()
        {
            Hashtable htCapIDAndStatus = new Hashtable();

            try
            {
                ShoppingCartModel4WS shoppingCartModel = ShoppingCartService.getShoppingCart(AgencyCode, long.Parse(User.UserSeqNum), User.PublicUserId);

                if (shoppingCartModel != null && shoppingCartModel.shoppingCartItems != null && shoppingCartModel.shoppingCartItems.Length > 0)
                {
                    foreach (ShoppingCartItemModel4WS item in shoppingCartModel.shoppingCartItems)
                    {
                        string customerID = ConstructCustomerID(item.capID);

                        if (!htCapIDAndStatus.Contains(customerID))
                        {
                            htCapIDAndStatus.Add(customerID, item);
                        }
                    }
                }

                return htCapIDAndStatus;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Shopping Cart Items Count 
        /// </summary>
        /// <returns>Shopping Cart Items Count</returns>
        public int GetShoppingCartItemsCount()
        {
            try
            {
                return ShoppingCartService.getShoppingCartItemsCount(SuperAgencyCode, int.Parse(User.UserSeqNum), User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get total Fee For Shopping Cart 
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="capIDs">Array of CapID.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Total fee value.</returns>
        public double GetTotalFeeForShoppingCart(string agencyCode, CapIDModel4WS[] capIDs, string callerID)
        {
            try
            {
                if (capIDs == null || capIDs.Length < 1)
                {
                    return 0;
                }

                return ShoppingCartService.getTotalFeeForShoppingCart(agencyCode, capIDs, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// when fee changed return true, else return false 
        /// </summary>
        /// <returns>is changed fee</returns>
        public bool HasChangeFee()
        {
            try
            {
                return ShoppingCartService.hasChangeFee(AgencyCode, int.Parse(User.UserSeqNum), User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update Shopping Cart
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="shoppingCart">shopping Cart</param>
        /// <param name="callerID">callerID number</param>
        public void UpdateShoppingCart(string agencyCode, ShoppingCartModel4WS shoppingCart, string callerID)
        {
            try
            {
                ShoppingCartService.updateShoppingCart(agencyCode, shoppingCart, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get display address from address Model
        /// </summary>
        /// <param name="address">address model</param>
        /// <param name="simpleViewElements">The simple view elements.</param>
        /// <returns>string format for address.</returns>
        private string FormatAddress(AddressModel address, SimpleViewElementModel4WS[] simpleViewElements)
        {
            string displayAddress = string.Empty;

            if (address != null)
            {
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                displayAddress = addressBuilderBll.BuildAddressByFormatType(address, simpleViewElements, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
            }

            return displayAddress;
        }

        /// <summary>
        /// Construct cap id as "09EST-00000-09123"
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <returns>format cap id string.</returns>
        private string ConstructCustomerID(CapIDModel4WS capID)
        {
            string customerID = string.Empty;

            if (capID != null)
            {
                customerID = string.Format("{0}-{1}-{2}", capID.id1, capID.id2, capID.id3);
            }

            return customerID;
        }

        #endregion Methods
    }
}