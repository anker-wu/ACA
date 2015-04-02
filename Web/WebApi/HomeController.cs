#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: HomeController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:HomeController.cs 77905 2014-08-19 12:49:28Z ACHIEVO\eric.he $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Report;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Config;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Home controller class
    /// </summary>
    public class HomeController : ApiController
    {
        #region controllers

        /// <summary>
        /// Get the agency logo
        /// </summary>
        /// <returns>Y or N</returns>
        [ActionName("agencyLogo")]
        public HttpResponseMessage GetAgencyLogo()
        {
            var logo = ObjectFactory.GetObject<ILogoBll>();
            LogoModel logoModel = logo.GetAgencyLogoByType(ConfigManager.AgencyCode, ACAConstant.LOGO_TYPE_CATEGORY_FOR_NEWUI);
            string logoSource = "Images/logo.png";

            if (logoModel != null && logoModel.docContent.Length > 0)
            {
                logoSource = "data:image/png;base64," + Convert.ToBase64String(logoModel.docContent);
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"logoSource\": \"" + logoSource + "\"}")
            };
        }

        /// <summary>
        /// Get Official WebSite
        /// </summary>
        /// <returns>json value</returns>
        [ActionName("officialWebSite")]
        public HttpResponseMessage GetOfficialWebSite()
        {
            string officialWebSite = StandardChoiceUtil.GetOfficialWebSite();

            if (!officialWebSite.StartsWith("http://") && !officialWebSite.StartsWith("https://") && !string.IsNullOrEmpty(officialWebSite))
            {
                officialWebSite = officialWebSite.Insert(0, "http://");
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"officialWebSite\": \"" + officialWebSite + "\"}")
            };
        }

        /// <summary>
        /// Get All ShoppingCart
        /// </summary>
        /// <returns>ShoppingCart json value</returns>
        [ActionName("ShoppingCarts")]
        public HttpResponseMessage GetShoppingCart()
        {
            string cartListJson  = string.Empty;
            bool isShowCart = !AppSession.User.IsAnonymous && StandardChoiceUtil.IsEnableShoppingCart();

            if (isShowCart)
            {
                IShoppingCartBll shoppingCartBll =
                    (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));

                Hashtable htShoppingCartItems = shoppingCartBll.GetShoppingCart(
                    ConfigManager.AgencyCode,
                    long.Parse(AppSession.User.UserSeqNum),
                    AppSession.User.PublicUserId);
    
                if (htShoppingCartItems != null && htShoppingCartItems.Count > 0)
                {
                    ArrayList shoppingCartList = new ArrayList();
                    List<ShoppingCartItemModel4WS> selectedShoppingCartItems = GetShoppingItemsFromModel((ShoppingCartModel4WS)htShoppingCartItems[0]);
                    List<ShoppingCartItemModel4WS> savedShoppingCartItems = GetShoppingItemsFromModel((ShoppingCartModel4WS)htShoppingCartItems[1]);

                    if (selectedShoppingCartItems != null && selectedShoppingCartItems.Count > 0)
                    {
                        shoppingCartList.AddRange(selectedShoppingCartItems);
                    }

                    if (savedShoppingCartItems != null && savedShoppingCartItems.Count > 0)
                    {
                        shoppingCartList.AddRange(savedShoppingCartItems);
                    }

                    if (shoppingCartList.Count > 0)
                    {
                        cartListJson = Newtonsoft.Json.JsonConvert.SerializeObject(shoppingCartList);
                    }
                }
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"cartList\":" + cartListJson + ",\"isShowCart\":" + isShowCart.ToString().ToLower() + "}")
            };
        }

        /// <summary>
        /// Get All Announcement
        /// </summary>
        /// <returns>Announcement  to json value</returns>
        [ActionName("Announcements")]
        public HttpResponseMessage GetAnnouncement()
        {
            string annJson = string.Empty;
            string token = AppSession.User.UserToken;

            if (!string.IsNullOrEmpty(token) && StandardChoiceUtil.IsEnableAnnouncement())
            {
                string url = WSConfiguration.GetConfig().WebSites[0].Url;
                Uri bastUri = new Uri(url);
                string[] splitUrl = url.Split(new string[] { bastUri.Authority }, StringSplitOptions.RemoveEmptyEntries);
                url = splitUrl[0] + bastUri.Authority + "/apis/v4/announcements?token=" + token;
                string responseMessage = ApiUtil.Get(url);
                if (!string.IsNullOrEmpty(responseMessage))
                {
                    annJson = responseMessage;
                }
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"annList\":" + annJson + ",\"isShowAnn\":" + StandardChoiceUtil.IsEnableAnnouncement().ToString().ToLower() + "}")
            };
        }

        /// <summary>
        /// Get All Reports
        /// </summary>
        /// <returns>Reports  to json value</returns>
        [ActionName("Reports")]
        public HttpResponseMessage GetReports()
        {
            var responseMessage = string.Empty;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(string.Empty);
            CapIDModel4WS capIDModel = capModel == null ? null : capModel.capID;

            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
            ReportButtonPropertyModel4WS[] reports = reportBll.GetReportLinkProperty(capIDModel, string.Empty, new Welcome().PageID);
            if (reports != null && reports.Length > 0)
            {
                responseMessage = Newtonsoft.Json.JsonConvert.SerializeObject(reports);
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(responseMessage)
            };
        }

        /// <summary>
        /// get global search by aca admin config
        /// </summary>
        /// <returns>global search config data</returns>
        [ActionName("GlobalSearchSwitch")]
        public HttpResponseMessage GetGlobalSearchSwitch()
        {
            Dictionary<string, bool> switchData = new Dictionary<string, bool>();
            switchData.Add("LP", GlobalSearchUtil.IsLPEnabled());
            switchData.Add("APO", GlobalSearchUtil.IsAPOEnabled());
            switchData.Add("CAP", GlobalSearchUtil.IsRecordEnabled());

            return new HttpResponseMessage
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(switchData))
            };
        }

        /// <summary>
        /// force login logic
        /// </summary>
        /// <param name="url">current request URL</param>
        /// <param name="moduleName">current moduleName</param>
        /// <returns>Boolean value</returns>
        [ActionName("ForceLogin")]
        [HttpGet]
        public HttpResponseMessage ForceLoginValidation(string url, string moduleName)
        {
            // if feature need to force login, and user have not login, redirect to login page else do nothing.
            bool isForceLogin = !AuthenticationUtil.IsAuthenticated && IsFeatureForceLogin(url, moduleName);

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"isForceLogin\":" + isForceLogin.ToString().ToLowerInvariant() + "}")
            };
        }
        #endregion

        #region private methods.

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
                shoppingCartItems.Add(new ShoppingCartItemModel4WS
                {
                    capID = shoppingCartItem.capID,
                    totalFee = shoppingCartItem.totalFee
                });
            }

            return shoppingCartItems;
        }

        /// <summary>
        /// check if force login
        /// </summary>
        /// <param name="url">the url from request or special one.</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>true if force login</returns>
        private bool IsFeatureForceLogin(string url, string moduleName)
        {
            IBizDomainBll bizDomain = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            return bizDomain.IsForceLogin(moduleName, url, null);
        }
        #endregion
    }
}