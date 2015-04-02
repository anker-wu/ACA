#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ShoppingCartUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  LabelUtil for getting label by label key.
 *  UI should call this class if need to get text in .cs.
 *
 *  Notes:
 *      $Id: ShoppingCartUtil.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Shopping cart tool method.
    /// </summary>
    public static class ShoppingCartUtil
    {
        #region Fields

        /// <summary>
        /// zero fee payment adapter name.
        /// </summary>
        private const string PAYMENT_ZEROFEE_ADAPTER = "Internal";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Create real cap , if the caps has no fee.
        /// Pass "TOTAL_FEE=0" to distinguish the cap has fee or not. 
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        /// <returns>return OnlinePaymentResultModel model</returns>
        public static OnlinePaymentResultModel CreateZeroFeeCAPs(CapIDModel4WS[] capIDs)
        {
            string parameters = string.Format(CultureInfo.InvariantCulture, "{0}={1}&{2}={3}", ACAConstant.ServProvCode_Key, ConfigManager.AgencyCode, ACAConstant.PAYMENT_TOTALFEE, 0);
            string adapterName = PAYMENT_ZEROFEE_ADAPTER;

            IPaymentBll paymentBll = (IPaymentBll)ObjectFactory.GetObject(typeof(IPaymentBll));
            OnlinePaymentResultModel onlinePaymentResultModel = paymentBll.CompletePayment(null, null, TempModelConvert.Trim4WSOfCapIDModels(capIDs), parameters, adapterName, AppSession.User.PublicUserId);
            return onlinePaymentResultModel;
        }

        /// <summary>
        /// Get display address from address Model
        /// </summary>
        /// <param name="address">address model</param>
        /// <param name="simpleViewElements">the SimpleViewElementModel4WS model list.</param>
        /// <returns>format address.</returns>
        public static string FormatAddress(AddressModel address, SimpleViewElementModel4WS[] simpleViewElements)
        {
            string displayAddress = string.Empty;
            IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;

            if (address != null)
            {
                displayAddress = addressBuilderBll.BuildAddressByFormatType(address, simpleViewElements, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
            }

            return displayAddress;
        }

        /// <summary>
        /// get Total fee for shopping cart
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        /// <returns>caps total fee</returns>
        public static double GetTotalFeeForShoppingCart(CapIDModel4WS[] capIDs)
        {
            double totalFee = 0;

            if (capIDs != null && capIDs.Length > 0)
            {
                IShoppingCartBll shoppingCartBll = (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));
                totalFee = shoppingCartBll.GetTotalFeeForShoppingCart(ConfigManager.AgencyCode, capIDs, AppSession.User.PublicUserId);
            }

            return totalFee;
        }

        /// <summary>
        /// get shopping cart item count , and setting to session.
        /// </summary>
        public static void SetCartItemNumber()
        {
            IShoppingCartBll shoppingCartBll = (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));
            int shoppingCartItemsNumber = shoppingCartBll.GetShoppingCartItemsCount();
            AppSession.SetCartItemNumberToSession(shoppingCartItemsNumber.ToString());
        }

        /// <summary>
        /// get shopping cart number , and setting to session.
        /// update Navigation cart number, format like "Cart (10)".
        /// </summary>
        /// <param name="moduleName">module Name use for get label key</param>
        /// <returns>return true if the link is enabled or didn't be configured. Otherwise returns false.</returns>
        public static string GetCartNumberHTML(string moduleName)
        {
            IShoppingCartBll shoppingCartBll = (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));
            int shoppingCartItemsNumber = shoppingCartBll.GetShoppingCartItemsCount();
            AppSession.SetCartItemNumberToSession(shoppingCartItemsNumber.ToString());

            string cartNumberHTML = string.Format("{0} ({1})", LabelUtil.GetTextByKey("com_headNav_label_carttitle", moduleName), shoppingCartItemsNumber.ToString());

            return cartNumberHTML;
        }

        /// <summary>
        /// Add Cap to shopping cart.
        /// </summary>
        /// <param name="page">the page control.</param>
        /// <param name="selectedCaps">the selected Caps.</param>
        /// <param name="moduleName">module Name use for get label key</param>
        /// <returns>true or false.</returns>
        public static bool AddToCart(Page page, DataTable selectedCaps, string moduleName)
        {
            bool showAlertMessage = false;
            IShoppingCartBll shoppingCartBll = (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));
            string script = string.Empty;
            ShoppingCartItemModel4WS[] cartItems = null;
            string cartNumberHTML = string.Empty;

            string displayAddingMsg = string.Format("displayAddingToCartMessage('{0}')", LabelUtil.GetTextByKey("per_permitlist_label_addtocart", moduleName).Replace("'", "\\'"));
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "Adding", displayAddingMsg, true);

            if (selectedCaps == null || selectedCaps.Rows.Count == 0)
            {
                script = string.Format("displayCartAlertMessage('{0}')", LabelUtil.GetTextByKey("per_caplist2cart_message_selectonerecord", moduleName).Replace("'", "\\'"));
                showAlertMessage = true;
            }
            else if (selectedCaps.Rows.Count == 1)
            {
                cartItems = FilterAndConstructCartItems(selectedCaps);

                if (cartItems == null || cartItems.Length == 0)
                {
                    script = string.Format("displayCartAlertMessage('{0}')", LabelUtil.GetTextByKey("per_caplist2cart_message_theonefailed", moduleName).Replace("'", "\\'"));
                    showAlertMessage = true;
                }
                else
                {
                    shoppingCartBll.CreateShoppingCart(cartItems, false, false);
                    cartNumberHTML = ShoppingCartUtil.GetCartNumberHTML(moduleName);
                    script = string.Format("displayCartMessage('{0}','{1}','{2}')", LabelUtil.GetTextByKey("per_caplist2cart_message_successfull", moduleName).Replace("'", "\\'"), true, cartNumberHTML);
                }
            }
            else if (selectedCaps.Rows.Count > 1)
            {
                cartItems = FilterAndConstructCartItems(selectedCaps);

                if (cartItems == null || cartItems.Length == 0)
                {
                    script = string.Format("displayCartAlertMessage('{0}')", LabelUtil.GetTextByKey("per_caplist2cart_message_allfailed", moduleName).Replace("'", "\\'"));
                    showAlertMessage = true;
                }
                else if (cartItems != null && cartItems.Length < selectedCaps.Rows.Count)
                {
                    shoppingCartBll.CreateShoppingCart(cartItems, false, false);
                    cartNumberHTML = GetCartNumberHTML(moduleName);
                    script = string.Format("displayCartMessage('{0}','{1}','{2}')", LabelUtil.GetTextByKey("per_caplist2cart_message_somefailed", moduleName).Replace("'", "\\'"), true, cartNumberHTML);
                }
                else if (cartItems != null && cartItems.Length >= selectedCaps.Rows.Count)
                {
                    shoppingCartBll.CreateShoppingCart(cartItems, false, false);
                    cartNumberHTML = GetCartNumberHTML(moduleName);
                    script = string.Format("displayCartMessage('{0}','{1}','{2}')", LabelUtil.GetTextByKey("per_caplist2cart_message_successfull", moduleName).Replace("'", "\\'"), true, cartNumberHTML);
                }
            }

            if (!string.IsNullOrEmpty(script))
            {
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "Added", script, true);
            }

            return showAlertMessage;
        }

        /// <summary>
        /// Get SimpleViewElements model.
        /// </summary>
        /// <param name="moduleList">The module List</param>
        /// <returns>HashTable which contains element model.</returns>
        public static Hashtable GetGviewElementByModules(ArrayList moduleList)
        {
            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS();
            permission.permissionLevel = "APO";
            permission.permissionValue = "ADDRESS";

            Hashtable gviewElements = new Hashtable();

            if (moduleList != null && moduleList.Count > 0)
            {
                IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));

                for (int i = 0; i < moduleList.Count; i++)
                {
                    if (moduleList[i] != null && !string.IsNullOrEmpty(moduleList[i].ToString()) && !gviewElements.ContainsKey(moduleList[i]))
                    {
                        SimpleViewElementModel4WS[] simpleViewElementModel = gviewBll.GetSimpleViewElementModel(moduleList[i].ToString(), permission, GviewID.AddressEdit, AppSession.User.UserID); 
                        gviewElements.Add(moduleList[i], simpleViewElementModel);
                    }
                }
            }

            return gviewElements;
        }

        /// <summary>
        /// Get intersect payment types 
        /// </summary>
        /// <param name="moduleList">module list</param>
        /// <returns>return payment type list</returns>
        public static List<ListItem> GetIntersectantPaymentTypes(List<string> moduleList)
        {
            List<ListItem> list = null;
            if (moduleList != null && moduleList.Count > 1)
            {
                foreach (string module in moduleList)
                {
                    List<ListItem> paymentItems = GetPaymentTypesByModule(module);
                    if (list == null)
                    {
                        list = paymentItems;
                    }
                    else
                    {
                        list = list.Intersect(paymentItems).ToList();
                    }
                }
            }
            else if (moduleList.Count == 1)
            {
                list = GetPaymentTypesByModule(moduleList[0]);
            }
            else
            {
                list = new List<ListItem>();
            }

            return list;
        }

        /// <summary>
        /// Gets the type of the shopping cart transaction.
        /// </summary>
        /// <returns>ShoppingCartTransactionType type</returns>
        public static ShoppingCartTransactionType GetShoppingCartTransactionType()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            string xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_SHOPPING_CART_PAYMENT_TRANSACTION_SETTING);
            xPolicyValue = string.IsNullOrEmpty(xPolicyValue) ? string.Empty : xPolicyValue.Trim();
            xPolicyValue = ValidationUtil.IsInt(xPolicyValue) ? xPolicyValue : ACAConstant.SHOPPINGCART_TRANSACTION_DEFAULT_VALUE;
            ShoppingCartTransactionType result = (ShoppingCartTransactionType)Enum.Parse(typeof(ShoppingCartTransactionType), xPolicyValue, true);

            return result;
        }

        /// <summary>
        /// Get the available payment types and indicates that whether different CAP have the same payment types or not.
        /// </summary>
        /// <param name="capIds">The CAP id model list.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="isSamePaymentTypes">Indicate that whether different CAP have the same payment types.</param>
        /// <returns>Return the available payment type list.</returns>
        public static List<ListItem> GetAvailablePaymentTypes(CapIDModel[] capIds, string moduleName, out bool isSamePaymentTypes)
        {
            List<string> moduleList = new List<string>();

            if (capIds != null)
            {
                moduleList = CapUtil.GetModuleNamesByCapID(capIds);
            }

            List<ListItem> paymentTypes = new List<ListItem>();
            isSamePaymentTypes = HasSamePaymentTypes(moduleList);

            if (isSamePaymentTypes)
            {
                string module = moduleList.Count > 0 ? moduleList[0] : moduleName;
                paymentTypes = GetPaymentTypesByModule(module);
            }
            else
            {
                paymentTypes = GetIntersectantPaymentTypes(moduleList);
            }

            return paymentTypes;
        }

        /// <summary>
        /// whether same payment types for every module.
        /// </summary>
        /// <param name="moduleList">Module list</param>
        /// <returns>return true if every module config same payment types.</returns>
        public static bool HasSamePaymentTypes(List<string> moduleList)
        {
            bool isSame = true;

            if (moduleList.Count > 1)
            {
                List<List<string>> paymentMethodList = new List<List<string>>();
                foreach (string module in moduleList)
                {
                    List<string> paymentMethodItem = ShoppingCartUtil.GetPaymentMethodByModule(module);
                    if (paymentMethodList.Count > 0 && paymentMethodList[paymentMethodList.Count - 1].Count != paymentMethodItem.Count)
                    {
                        return false;
                    }
                    else
                    {
                        paymentMethodList.Add(paymentMethodItem);
                    }
                }

                foreach (string pm in paymentMethodList[0])
                {
                    for (int i = 1; i < paymentMethodList.Count; i++)
                    {
                        if (!paymentMethodList[i].Contains(pm))
                        {
                            return false;
                        }
                    }
                }
            }

            return isSame;
        }

        /// <summary>
        /// Get Payment Types by module
        /// </summary>
        /// <param name="module">the module name</param>
        /// <returns>return payment type list</returns>
        public static List<ListItem> GetPaymentTypesByModule(string module)
        {
            List<ListItem> list = new List<ListItem>();
            IList<ItemValue> stdItems = new List<ItemValue>();

            // if agent user, it use the Admin's BLL.
            IPolicyBLL policyBll = ObjectFactory.GetObject<IPolicyBLL>();

            stdItems = policyBll.GetPolicyListForPayment(BizDomainConstant.STD_CAT_CAP_PAYMENT_TYPE, module);

            if (stdItems == null || stdItems.Count <= 0)
            {
                return list;
            }

            string value = string.Empty;
            foreach (ItemValue item in stdItems)
            {
                value = item.Value.ToString();

                //add payment method to radio list.
                list.Add(new ListItem(item.Key, value));
            }

            return list;
        }

        /// <summary>
        /// Get Message with Difference payment types.
        /// </summary>
        /// <param name="capIDs">capID Model array</param>
        /// <returns>return notice message</returns>
        public static string GetMessageWithDifferencePaymentTypes(CapIDModel[] capIDs)
        {
            List<CapIDModel> capIdlist = new List<CapIDModel>();
            capIdlist.AddRange(capIDs);

            StringBuilder msg = new StringBuilder();
            List<string> modulelist = new List<string>();
            Dictionary<string, List<string>> recordlist = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> methodlist = new Dictionary<string, List<string>>();

            ICapBll capbll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
            StringArray[] modulenames = capbll.GetModuleNamesByCapIDList(capIDs);
            foreach (StringArray modulenameItem in modulenames)
            {
                if (!modulelist.Contains(modulenameItem.item[1]))
                {
                    modulelist.Add(modulenameItem.item[1]);
                    List<string> moduleMethods = ShoppingCartUtil.GetPaymentMethodByModule(modulenameItem.item[1]);
                    methodlist.Add(modulenameItem.item[1], moduleMethods);
                }
            }

            foreach (string module in modulelist)
            {
                List<string> recordItems = new List<string>();
                foreach (StringArray modulenameItem in modulenames)
                {
                    if (string.Equals(module, modulenameItem.item[1], StringComparison.CurrentCultureIgnoreCase))
                    {
                        string customId = capIdlist.Find(f => string.Format("{0}-{1}-{2}", f.ID1, f.ID2, f.ID3) == modulenameItem.item[0]).customID;
                        recordItems.Add(customId);
                    }
                }

                recordlist.Add(module, recordItems);
            }

            foreach (var item in recordlist)
            {
                string resModuleName = I18nUtil.GetResModuleName(item.Key);
                resModuleName = I18nStringUtil.GetString(resModuleName, item.Key);
                msg.AppendFormat(LabelUtil.GetGlobalTextByKey("aca_payment_msg_records"), resModuleName);
                msg.AppendLine();
                msg.AppendFormat(string.Join(", ", item.Value.ToArray()));
                msg.AppendLine();
            }

            msg.AppendLine();
            msg.AppendLine(LabelUtil.GetGlobalTextByKey("aca_payment_msg_methods"));

            foreach (var item in methodlist)
            {
                string resModuleName = I18nUtil.GetResModuleName(item.Key);
                resModuleName = I18nStringUtil.GetString(resModuleName, item.Key);
                msg.AppendFormat(LabelUtil.GetGlobalTextByKey("aca_payment_msg_methods_by_module").Trim(), resModuleName);
                msg.Append(" ");
                msg.AppendFormat(string.Join(", ", item.Value.ToArray()));
                msg.AppendLine();
            }

            return msg.ToString();
        }

        /// <summary>
        /// Get Payment Method by module
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>return payment method array</returns>
        public static List<string> GetPaymentMethodByModule(string moduleName)
        {
            List<string> list = new List<string>();
            IPolicyBLL policyBll = (IPolicyBLL)ObjectFactory.GetObject(typeof(IPolicyBLL));
            IList<ItemValue> stdItems = new List<ItemValue>();
            stdItems = policyBll.GetPolicyListForPayment(BizDomainConstant.STD_CAT_CAP_PAYMENT_TYPE, moduleName);

            if (stdItems == null || stdItems.Count <= 0)
            {
                return list;
            }

            string value = string.Empty;
            foreach (ItemValue item in stdItems)
            {
                value = item.Value.ToString();
                if (value.IndexOf("||") != -1)
                {
                    value = value.Substring(0, value.IndexOf("||")); // remove bizDomainValue
                }

                value = value.Replace("-", string.Empty);

                //add payment method to radio list.
                list.Add(item.Key);
            }

            return list;
        }

        /// <summary>
        /// Filter partial CAP from selected CAPs.
        /// </summary>
        /// <param name="dtSelectedCAPs">select caps table</param>
        /// <returns>Filter Shopping cart items. 1.PayFeeDue 2.Renew Caps which child child is full partial caps. 3.Full partial caps</returns>
        public static ShoppingCartItemModel4WS[] FilterAndConstructCartItems(DataTable dtSelectedCAPs)
        {
            List<ShoppingCartItemModel4WS> cartItemList = new List<ShoppingCartItemModel4WS>();
            ShoppingCartItemModel4WS carItem = new ShoppingCartItemModel4WS();

            //A list to store the checked CAP list for the CheckRelatedCapsForAssoForm.
            //To reduce the number of calls to Web Service to promote the performance.
            IList<CapIDModel4WS> checkedCapIdList = new List<CapIDModel4WS>();

            foreach (DataRow dr in dtSelectedCAPs.Rows)
            {
                // filter the paid records but convert to real cap failed, this record cannot add to shopping cart and do anything. 
                if (ACAConstant.PAYMENT_STATUS_PAID.Equals(dr["PaymentStatus"]))
                {
                    continue;
                }

                carItem = GetCartItem(dr);

                if (carItem != null && carItem.capID != null)
                {
                    //Associated Forms related Caps for current CAP.
                    List<ShoppingCartItemModel4WS> relatedCartItems;

                    if (CheckRelatedCapsForAssoForm(dr, ref checkedCapIdList, out relatedCartItems))
                    {
                        cartItemList.Add(carItem);
                        cartItemList.AddRange(relatedCartItems);
                    }
                }
            }

            return cartItemList.ToArray();
        }

        /// <summary>
        /// Get related Caps for the Associated Forms relationship.
        /// </summary>
        /// <param name="currentCap">Current CAP.</param>
        /// <param name="checkedCapIdList">Have checked the CAP id list.</param>
        /// <param name="relatedCartItems">Output parameter - Related cart items for current CAP.</param>
        /// <returns>Boolean value. True - means all the associated Child Records have been added to the out parameter<see cref="relatedCartItems"/></returns>
        private static bool CheckRelatedCapsForAssoForm(DataRow currentCap, ref IList<CapIDModel4WS> checkedCapIdList, out List<ShoppingCartItemModel4WS> relatedCartItems)
        {
            //Initialize the reated cart item list.
            relatedCartItems = new List<ShoppingCartItemModel4WS>();
            string capClass = currentCap["CapClass"] == null ? string.Empty : currentCap["CapClass"].ToString();

            if (!CapUtil.IsPartialCap(capClass))
            {
                //Not need to check related caps for Real CAP.
                return true;
            }

            //Construct the cap id for current CAP.
            CapIDModel4WS currentCapId = new CapIDModel4WS
                                              {
                                                  serviceProviderCode = currentCap["agencyCode"].ToString(),
                                                  id1 = currentCap["capID1"].ToString(),
                                                  id2 = currentCap["capID2"].ToString(),
                                                  id3 = currentCap["capID3"].ToString()
                                              };

            if (checkedCapIdList.Contains(currentCapId))
            {
                //The current CAP and all the related CAPs already added at previous.
                return false;
            }

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            //Get parent for current CAP.
            CapIDModel4WS parentCapId = CapUtil.GetParentAssoFormCapID(currentCapId);

            if (!parentCapId.Equals(currentCapId))
            {
                if (!checkedCapIdList.Contains(parentCapId))
                {
                    checkedCapIdList.Add(parentCapId);
                }

                //Current CAP is a child CAP, need create cart item for the parent CAP.
                CapWithConditionModel4WS parentCapCondition = capBll.GetCapViewBySingle(parentCapId, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, StandardChoiceUtil.IsSuperAgency());

                if (parentCapCondition != null && parentCapCondition.capModel != null)
                {
                    DataRow drNewItem = currentCap.Table.NewRow();
                    FillDataRow4CartItem(ref drNewItem, parentCapCondition.capModel);
                    ShoppingCartItemModel4WS cartItem = GetCartItem(drNewItem);

                    if (cartItem == null || cartItem.capID == null)
                    {
                        return false;
                    }
                    else
                    {
                        relatedCartItems.Add(cartItem);
                    }
                }
            }

            //Get child CAPs by Parent CAP id and create Cart items for child CAPs except for the current CAP.
            CapModel4WS[] childCaps = capBll.GetChildCapDetailsByMasterID(parentCapId, ACAConstant.CAP_RELATIONSHIP_ASSOFORM, null);

            if (childCaps != null && childCaps.Length > 0)
            {
                foreach (CapModel4WS childCap in childCaps)
                {
                    if (!checkedCapIdList.Contains(childCap.capID))
                    {
                        checkedCapIdList.Add(childCap.capID);
                    }

                    if (!childCap.capID.Equals(currentCapId))
                    {
                        DataRow drNewItem = currentCap.Table.NewRow();
                        FillDataRow4CartItem(ref drNewItem, childCap);
                        ShoppingCartItemModel4WS cartItem = GetCartItem(drNewItem);

                        if (cartItem == null || cartItem.capID == null)
                        {
                            return false;
                        }
                        else
                        {
                            relatedCartItems.Add(cartItem);
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Use the CAP model to fill out the Data Row for the cart item generation.
        /// </summary>
        /// <param name="dataRow">Data row object. It must included the necessary columns.</param>
        /// <param name="capModel">the Cap model</param>
        private static void FillDataRow4CartItem(ref DataRow dataRow, CapModel4WS capModel)
        {
            dataRow["agencyCode"] = capModel.capID.serviceProviderCode;
            dataRow["capID1"] = capModel.capID.id1;
            dataRow["capID2"] = capModel.capID.id2;
            dataRow["capID3"] = capModel.capID.id3;
            dataRow["ModuleName"] = capModel.moduleName;
            dataRow["CapClass"] = capModel.capClass;
            dataRow["hasPrivilegeToHandleCap"] = capModel.hasPrivilegeToHandleCap;
            dataRow["HasNoPaidFees"] = capModel.noPaidFeeFlag;
            dataRow["RenewalStatus"] = capModel.renewalStatus;
        }

        /// <summary>
        /// Get Cart Item from data row.
        /// </summary>
        /// <param name="dr">DataRow object that it including the Record info.</param>
        /// <returns>Shopping cart item</returns>
        private static ShoppingCartItemModel4WS GetCartItem(DataRow dr)
        {
            bool hasPrivilegeToHandleCap = (bool)dr["hasPrivilegeToHandleCap"];
            string moduleName = Convert.ToString(dr["ModuleName"]);
            if (!hasPrivilegeToHandleCap && !StandardChoiceUtil.IsDisplayPayFeeLink(moduleName))
            {
                return null;
            }

            string capClass = dr["CapClass"] == null ? string.Empty : dr["CapClass"].ToString();

            bool hasNopaidFee = (bool)dr["HasNoPaidFees"];
            string renewalStatus = dr["RenewalStatus"] == null ? string.Empty : dr["RenewalStatus"].ToString();
            string agencyCode = dr["agencyCode"].ToString();

            ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();

            //Amendment Cap status is CAConstant.COMPLETED. no need to add to shopping cart.
            if (!CapUtil.IsPartialCap(capClass))
            {
                if (ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCulture))
                {
                    //1.Get PayFeeDue4Renewal Cap.
                    cartItem = GetPayFeeDue4RenewalCap(dr);
                }
                else if (ACAConstant.RENEWAL_INCOMPLETE.Equals(renewalStatus, StringComparison.InvariantCulture))
                {
                    //1.Get Renewal Cap.
                    cartItem = GetRenewCap(dr);
                }
                else if (hasNopaidFee && !StandardChoiceUtil.IsRemovePayFee(agencyCode))
                {
                    //2.Get PayFeeDue Cap
                    cartItem = GetPayFeeDueOrPartialCap(dr, true);
                }
            }
            else if (string.Equals(ACAConstant.INCOMPLETE_EST, capClass, StringComparison.InvariantCulture))
            {
                //Add full Partial cap to shopping cart.
                cartItem = GetPayFeeDueOrPartialCap(dr, false);
            }

            return cartItem;
        }

        /// <summary>
        /// get field visible
        /// </summary>
        /// <param name="models">simple view element models</param>
        /// <param name="fieldName">field name</param>
        /// <returns>true / false</returns>
        private static bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName)
        {
            if (models == null ||
                models.Length == 0)
            {
                return true;
            }

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (model.viewElementName == fieldName)
                {
                    return model.recStatus == ACAConstant.VALID_STATUS;
                }
            }

            return true;
        }

        /// <summary>
        /// add Pay Fee Due Cap to shopping cart
        /// </summary>
        /// <param name="dr">data source</param>
        /// <param name="isPayFeeDue">weather the cap is pay fee due cap.</param>
        /// <returns>Shopping Cart Item Model</returns>
        private static ShoppingCartItemModel4WS GetPayFeeDueOrPartialCap(DataRow dr, bool isPayFeeDue)
        {
            ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();

            cartItem.capID = GetCapID(dr);
            cartItem.servProvCode = cartItem.capID.serviceProviderCode;

            if (isPayFeeDue)
            {
                cartItem.processType = ACAConstant.CAP_PAYFEEDUE;
            }

            return cartItem;
        }

        /// <summary>
        /// add renew cap to shopping cart
        /// </summary>
        /// <param name="dr">data source</param>
        /// <returns>Shopping Cart Item Model</returns>
        private static ShoppingCartItemModel4WS GetRenewCap(DataRow dr)
        {
            ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();
            bool hasNopaidFee = (bool)dr["HasNoPaidFees"];
            string agencyCode = dr["agencyCode"].ToString();
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            //Check the status of the renewal cap to see if it can be added to shopping cart .
            CapModel4WS childCap = capBll.GetCapByRelationshipPartialCap(GetCapID(dr), ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE);

            if (childCap == null || !string.Equals(ACAConstant.INCOMPLETE_EST, childCap.capClass, StringComparison.InvariantCulture))
            {
                //If a renewal cap is not a full partial cap, we still need to check whether this cap is a pay fee due cap. 
                if (hasNopaidFee && !StandardChoiceUtil.IsRemovePayFee(agencyCode))
                {
                    cartItem = GetPayFeeDueOrPartialCap(dr, true);
                }
            }
            else
            {
                cartItem.capID = childCap.capID;
                cartItem.processType = ACAConstant.CAP_RENEWAL;
                cartItem.servProvCode = cartItem.capID.serviceProviderCode;
            }

            return cartItem;
        }
        
        /// <summary>
        /// add PayFeeDue4Renewal cap to shopping cart
        /// </summary>
        /// <param name="dr">data source</param>
        /// <returns>Shopping Cart Item Model</returns>
        private static ShoppingCartItemModel4WS GetPayFeeDue4RenewalCap(DataRow dr)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();

            //1. Get child cap id by params cap id.
            CapIDModel4WS childCapID = capBll.GetChildCapIDByMasterID(GetCapID(dr), ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE, false);

            if (childCapID != null)
            {
                cartItem.capID = childCapID;
                cartItem.servProvCode = childCapID.serviceProviderCode;
                cartItem.processType = ACAConstant.PAYFEEDUE_RENEWAL;
            }

            return cartItem;
        }

        /// <summary>
        /// Construct CapId Model.
        /// </summary>
        /// <param name="dr">data source</param>
        /// <returns>cap id model</returns>
        private static CapIDModel4WS GetCapID(DataRow dr)
        {
            CapIDModel4WS capId = new CapIDModel4WS();

            if (dr != null)
            {
                capId.id1 = dr["capID1"].ToString();
                capId.id2 = dr["capID2"].ToString();
                capId.id3 = dr["capID3"].ToString();
                capId.serviceProviderCode = dr["agencyCode"].ToString();
            }

            return capId;
        }

        #endregion Methods
    }
}