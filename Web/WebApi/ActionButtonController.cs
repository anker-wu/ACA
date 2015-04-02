/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ActionButtonController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:ActionButtonController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\dennis.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Config;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.Web.WebApi.Entity;
using Accela.ACA.WSProxy;
using Microsoft.SqlServer.Server;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Web API get Action button data source.
    /// </summary>
    public class ActionButtonController : ApiController
    {
        #region

        /// <summary>
        ///  Whether all added successfully
        /// </summary>
        private bool _isContainPartialCap = false;

        /// <summary>
        /// Gets reference MyCollectionWebServiceService
        /// </summary>
        /// <returns>MyCollectionWebServiceService </returns>
        private MyCollectionWebServiceService MyCollectionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<MyCollectionWebServiceService>();
            }
        }

        #endregion

        /// <summary>
        /// Schedule Button is displayed
        /// </summary>
        /// <param name="capIDs">cap IDS</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>boolean is displayed Inspection</returns>
        [NonAction]
        public static bool DisplayInspcetion(CapIDModel4WS capIDs, string moduleName)
        {
            Dictionary<string, UserRolePrivilegeModel> sectionPermissions = CapUtil.GetSectionPermissions(ConfigManager.AgencyCode, moduleName);
            bool isInspectionSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.INSPECTIONS.ToString(), sectionPermissions, moduleName);
            bool isEnableSchedule = FunctionTable.IsEnableScheduleInspection();
            CapModel4WS recordModel = ApiUtil.GetCapModel4Ws(capIDs);
            bool hasRightSchedule = InspectionViewUtil.CanShowScheduleLink(ConfigManager.AgencyCode, moduleName, AppSession.User, recordModel);
            bool isShowInspcetion = isInspectionSectionVisible && isEnableSchedule && hasRightSchedule;

            return isShowInspcetion;
        }

        /// <summary>
        /// The current record is added to the shopping cart
        /// </summary>
        /// <param name="id">string id</param>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="capClass">cap Class</param>
        /// <param name="hasNopaidFee">has No paid Fee</param>
        /// <param name="renewalStatus">renewal Status</param>
        /// <returns>current record is added to the shopping cart</returns>
        [ActionName("AddRecordToCart")]
        public HttpResponseMessage GetAddRecordToCart(string id, string agencyCode, string capClass, bool hasNopaidFee, string renewalStatus)
        {
            string result = string.Empty;
            string token = AppSession.User.UserToken;

            if (!CapUtil.IsPartialCap(capClass) || string.Equals(ACAConstant.INCOMPLETE_EST, capClass, StringComparison.InvariantCultureIgnoreCase))
            {
                if (ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase)
                    || ACAConstant.RENEWAL_INCOMPLETE.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase)
                    || (hasNopaidFee && !StandardChoiceUtil.IsRemovePayFee(agencyCode))
                    || string.Equals(ACAConstant.INCOMPLETE_EST, capClass, StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] capids = id.Split(new string[] { ACAConstant.SPLIT_CHAR4 }, StringSplitOptions.None);

                    string url = WSConfiguration.GetConfig().WebSites[0].Url;
                    Uri bastUri = new Uri(url);
                    string[] splitUrl = url.Split(new string[] { bastUri.Authority }, StringSplitOptions.RemoveEmptyEntries);

                    CapIDModel4WS capIdModel4Ws = new CapIDModel4WS()
                    {
                        id1 = capids[0],
                        id2 = capids[1],
                        id3 = capids[2],
                        serviceProviderCode = agencyCode
                    };

                    url = splitUrl[0] + bastUri.Authority + "/apis/v4/shoppingCart?token=" + token;
                    string data = "[{\"id\": \"" + id + "\", \"payNow\": \"Y\", \"processType\": \"" + ApiUtil.GetProcessType(capClass, renewalStatus, hasNopaidFee, capIdModel4Ws) +
                                  "\"}]";

                    result = ApiUtil.Post(url, data);
                }
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Get my collection
        /// </summary>
        /// <returns>Collection list</returns>
        [ActionName("GetAllCollcetion")]
        public HttpResponseMessage GetAllCollcetion()
        {
            var responseMessage = string.Empty;
            IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();
            MyCollectionModel[] myCollections = AppSession.GetMyCollectionsFromSession();
            myCollections = myCollectionBll.GetCollections4Management(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);

            if (myCollections != null && myCollections.Length > 0)
            {
                responseMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myCollections);
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(responseMessage)
            };
        }

        /// <summary>
        /// Add selected CAPs to my collection and create new collection.
        /// </summary>
        /// <param name="addToCollection"> AddToCollection Model </param>
        /// <returns>collection list</returns>
        [HttpPost]
        [ActionName("AddToCollection")]
        public HttpResponseMessage Collection([FromBody]PostParameterEntity.AddToCollection[] addToCollection)
        {
            var collectionList = string.Empty;
            var message = string.Empty;
            bool isSuccessful = false;
            IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();
            MyCollectionModel goalCollection = GetGoalMyCollectionModel(addToCollection);

            if (goalCollection == null)
            {
                if (_isContainPartialCap)
                {
                    message = LabelUtil.GetGlobalTextByKey("aca_newui_home_msg_addcollectionfailure");
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { message,  collectionList, isSuccessful }))
                    };
                }            
            }

           MyCollectionModel[] myCollections = AppSession.GetMyCollectionsFromSession();

           myCollectionBll.AddCaps2Collection(goalCollection);

           if (_isContainPartialCap)
           {
               message = LabelUtil.GetGlobalTextByKey("aca_newui_home_msg_add2collectionsuccussfully"); 
            }
           else
           {
               message = LabelUtil.GetGlobalTextByKey("aca_newui_home_msg_addcollectionsuccess");
           }

            isSuccessful = true;
            myCollections = myCollectionBll.GetCollections4Management(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);

            if (myCollections == null || myCollections.Length == 0)
            {
                myCollections = null;
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { message, collectionList = myCollections, isSuccessful }))
            };
        }

        /// <summary>
        /// Get  Collection model
        /// </summary>
        /// <param name="addToCollection"> AddToCollection model </param>
        /// <returns>MyCollection Model</returns>
        private MyCollectionModel GetGoalMyCollectionModel(PostParameterEntity.AddToCollection[] addToCollection)
        {
            int qualifiedCapCount = 0;
            MyCollectionModel goalCollection = new MyCollectionModel();

            IList<SimpleCapModel> simpleCapModels = new List<SimpleCapModel>();

            for (int i = 0; i < addToCollection.Length; i++)
            {
                if (string.IsNullOrEmpty(addToCollection[i].CapClass) ||
                    addToCollection[i].CapClass == ACAConstant.COMPLETED)
                {
                    qualifiedCapCount++;
                    SimpleCapModel models = new SimpleCapModel();
                    string[] splitCapid = addToCollection[i].CapId.Split('-');

                    models.capID = new CapIDModel
                    {
                        ID1 = splitCapid[0],
                        ID2 = splitCapid[1],
                        ID3 = splitCapid[2],
                        serviceProviderCode = ConfigManager.SuperAgencyCode
                    };

                    simpleCapModels.Add(models);
                }
            }

            if (addToCollection.Length > simpleCapModels.ToArray().Length)
            {
                _isContainPartialCap = true;
            }

            if (qualifiedCapCount > 0)
            {
                 goalCollection.auditID = AppSession.User.PublicUserId;
                 goalCollection.serviceProviderCode = ConfigManager.SuperAgencyCode;
                 goalCollection.userId = AppSession.User.PublicUserId;

                 goalCollection.collectionName = addToCollection[0].CollcetionName;
                 goalCollection.collectionDescription = addToCollection[0].CollectionDescription;

                 if (addToCollection[0].CollectionId != null)
                 {
                     goalCollection.collectionId = Convert.ToInt64(addToCollection[0].CollectionId);
                 }

                 goalCollection.simpleCapModels = simpleCapModels.ToArray();
            }
            else
            {
                return null;
            }
           
            return goalCollection;
        }
    }
}