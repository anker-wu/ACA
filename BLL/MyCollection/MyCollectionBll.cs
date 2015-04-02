#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MyCollectionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: MyCollectionBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.MyCollection
{
    /// <summary>
    /// This class provide the ability to operation my collection.
    /// </summary>
    public class MyCollectionBll : BaseBll, IMyCollectionBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of MyCollectionService.
        /// </summary>
        private MyCollectionWebServiceService MyCollectionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<MyCollectionWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add selected CAPs to specify collection.
        /// </summary>
        /// <param name="collectionModel4WS">collectionModel4WS object.</param>
        /// <exception cref="DataValidateException">{ <c>collectionModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        void IMyCollectionBll.AddCaps2Collection(MyCollectionModel collectionModel4WS)
        {
            if (collectionModel4WS == null)
            {
                throw new DataValidateException(new string[] { "collectionModel4WS" });
            }

            try
            {
                MyCollectionService.addCaps2Collection(collectionModel4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Copy selected CAPs to  specify collection. after this action is completed
        /// the selected CAPs will be shown in both collections.
        /// </summary>
        /// <param name="sourceCollection">source collection</param>
        /// <param name="targetCollection">target collection</param>
        void IMyCollectionBll.CopyCaps2Collection(MyCollectionModel sourceCollection, MyCollectionModel targetCollection)
        {
            if (sourceCollection == null || targetCollection == null)
            {
                throw new DataValidateException(new string[] { "sourceCollection", "targetCollection" });
            }

            try
            {
                MyCollectionService.copyCaps2Collection(sourceCollection, targetCollection);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Create my collection.
        /// </summary>
        /// <param name="collectionModel4WS">collectionModel4WS object.</param>
        void IMyCollectionBll.CreateMyCollection(MyCollectionModel collectionModel4WS)
        {
            if (collectionModel4WS == null)
            {
                throw new DataValidateException(new string[] { "collectionModel4WS" });
            }

            try
            {
                MyCollectionService.createCollection(collectionModel4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Delete my collection.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="collectionId">collection id</param>
        /// <param name="usrId">current register user</param>
        void IMyCollectionBll.DeleteMyCollection(string serviceProviderCode, string collectionId, string usrId)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(usrId))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "collectionId", "usrId" });
            }

            try
            {
                MyCollectionService.deleteCollection(serviceProviderCode, collectionId, usrId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get collection detail information.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="usrId">current register user</param>
        /// <param name="collectionId">collection id</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>MyCollectionModel4WS object.</returns>
        MyCollectionModel IMyCollectionBll.GetCollectionDetailInfo(string serviceProviderCode, string usrId, string collectionId, string[] viewElementNames)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(usrId))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "collectionId", "usrId" });
            }

            try
            {
                return MyCollectionService.getDetailInfoByCollectionId(serviceProviderCode, collectionId, usrId, viewElementNames);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get my collections for collection management page.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="usrId">current register user</param>
        /// <returns>Array of MyCollectionModel4WS</returns>
        MyCollectionModel[] IMyCollectionBll.GetCollections4Management(string serviceProviderCode, string usrId)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(usrId))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "usrId" });
            }

            try
            {
                return MyCollectionService.getCollections4Management(serviceProviderCode, usrId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get my collection.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="usrId">current register user</param>
        /// <returns>Array of MyCollectionModel4WS</returns>
        MyCollectionModel[] IMyCollectionBll.GetMyCollection(string serviceProviderCode, string usrId)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(usrId))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "usrId" });
            }

            try
            {
                MyCollectionModel[] myCollectionList = MyCollectionService.getCollections(serviceProviderCode, usrId);

                if (myCollectionList == null)
                {
                    myCollectionList = new MyCollectionModel[0];
                }

                return myCollectionList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Move selected CAPs to specify collection. after this action is completed
        /// the selected CAPs will be moved from current collection.
        /// </summary>
        /// <param name="sourceCollection">source collection</param>
        /// <param name="targetCollection">target collection</param>
        void IMyCollectionBll.MoveCaps2Collection(MyCollectionModel sourceCollection, MyCollectionModel targetCollection)
        {
            if (sourceCollection == null || targetCollection == null)
            {
                throw new DataValidateException(new string[] { "sourceCollection", "targetCollection" });
            }

            try
            {
                MyCollectionService.moveCaps2Collection(sourceCollection, targetCollection);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Remove selected CAPs to specify collection. after this action is completed
        /// the selected CAPs will be moved from current collection.
        /// </summary>
        /// <param name="collectionModel4WS">collectionModel4WS object.</param>
        void IMyCollectionBll.RemoveCapsFromCollection(MyCollectionModel collectionModel4WS)
        {
            if (collectionModel4WS == null)
            {
                throw new DataValidateException(new string[] { "collectionModel4WS" });
            }

            try
            {
                MyCollectionService.removeCapsFromCollection(collectionModel4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update my collection.
        /// </summary>
        /// <param name="collectionModel">collectionModel4WS object.</param>
        /// <exception cref="DataValidateException">{ <c>collectionModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        void IMyCollectionBll.UpdateMyCollection(MyCollectionModel collectionModel)
        {
            if (collectionModel == null)
            {
                throw new DataValidateException(new string[] { "collectionModel4WS" });
            }

            try
            {
                MyCollectionService.updateCollection(collectionModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}