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
 * $Id: IShoppingCartBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.ShoppingCart
{
    /// <summary>
    /// defines methods for shopping cart
    /// </summary>
    public interface IShoppingCartBll
    {
        #region Methods
        /// <summary>
        /// Create Shopping Cart
        /// </summary>
        /// <param name="shoppingCartItems">shopping Cart Item list</param>
        /// <param name="isMultiCap">Whether has child caps</param>
        /// <param name="isIncludeParent">Indicate whether create the parent CAP's shopping cart item</param>
        void CreateShoppingCart(ShoppingCartItemModel4WS[] shoppingCartItems, bool isMultiCap, bool isIncludeParent);

        /// <summary>
        /// Delete Shopping Cart Item 
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="shoppingCart">shopping Cart</param>
        /// <param name="callerID">caller ID number</param>       
        void DeleteShoppingCartItem(string agencyCode, ShoppingCartModel4WS shoppingCart, string callerID);

        /// <summary>
        ///  Get Address ByS hopping Cart Items
        /// </summary>
        /// <param name="allShoppingCartItems"> allShoppingCartItems List </param>
        /// <param name="simpleViewElements"> the simpleViewElement module</param>
        /// <returns>ShoppingCartItemModel4WS list </returns>
        List<ShoppingCartItemModel4WS> GetAddressByShoppingCartItems(List<ShoppingCartItemModel4WS> allShoppingCartItems, Hashtable simpleViewElements);

        /// <summary>
        /// get shoppingCart information .
        /// </summary>
        /// <param name="agencyCode">parent cap ID</param>
        /// <param name="userSeqNumber">user Sequence Number</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>Shopping Cart Item object.</returns>
        Hashtable GetShoppingCart(string agencyCode, long userSeqNumber, string callerID);

        /// <summary>
        /// get shoppingCart id list.
        /// </summary>
        /// <returns>cap id list.</returns>
        CapIDModel4WS[] GetShoppingCartCapIDs();

        /// <summary>
        /// get hashtable which key is cap id, value is shopping cart item.
        /// </summary>
        /// <returns>cap id and cap status hashtable.</returns>
        Hashtable GetCartItemWithCustomID();

        /// <summary>
        /// Get Shopping Cart Items Count 
        /// </summary>
        /// <returns>Shopping Cart Items Count</returns>
        int GetShoppingCartItemsCount();

        /// <summary>
        /// Get total Fee For Shopping Cart 
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="capIDs">Array of CapID.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Total fee value.</returns>
        double GetTotalFeeForShoppingCart(string agencyCode, CapIDModel4WS[] capIDs, string callerID);

        /// <summary>
        /// when fee changed return true, else return false 
        /// </summary>
        /// <returns>is changed fee</returns>
        bool HasChangeFee();

        /// <summary>
        /// Update Shopping Cart
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="shoppingCart">shopping Cart</param>
        /// <param name="callerID">callerID number</param>
        void UpdateShoppingCart(string agencyCode, ShoppingCartModel4WS shoppingCart, string callerID);

        #endregion Methods
    }
}