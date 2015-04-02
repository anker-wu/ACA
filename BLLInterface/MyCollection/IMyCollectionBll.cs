#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IMyCollectionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IMyCollectionBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.MyCollection
{
    /// <summary>
    /// Defines method signs for My Collection.
    /// </summary>
    public interface IMyCollectionBll
    {
        #region Methods

        /// <summary>
        /// Add selected CAPs to specify collection.
        /// </summary>
        /// <param name="collectionModel4WS">collectionModel4WS object.</param>
        void AddCaps2Collection(MyCollectionModel collectionModel4WS);

        /// <summary>
        /// Copy selected CAPs to  specify collection. after this action is completed
        /// the selected CAPs will be shown in both collections.
        /// </summary>
        /// <param name="sourceCollection">source collection</param>
        /// <param name="targetCollection">target collection</param>
        void CopyCaps2Collection(MyCollectionModel sourceCollection, MyCollectionModel targetCollection);

        /// <summary>
        /// Create my collection.
        /// </summary>
        /// <param name="collectionModel4WS">collectionModel4WS object.</param>
        void CreateMyCollection(MyCollectionModel collectionModel4WS);

        /// <summary>
        /// Delete my collection.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="collectionId">collection id</param>
        /// <param name="usrId">current register user</param>
        void DeleteMyCollection(string serviceProviderCode, string collectionId, string usrId);

        /// <summary>
        /// Get collection detail information.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="usrId">current register user</param>
        /// <param name="collectionId">collection id</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>MyCollectionModel4WS object.</returns>
        MyCollectionModel GetCollectionDetailInfo(string serviceProviderCode, string usrId, string collectionId, string[] viewElementNames);

        /// <summary>
        /// Get my collections for collection management page.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="usrId">current register user</param>
        /// <returns>Array of MyCollectionModel4WS</returns>
        MyCollectionModel[] GetCollections4Management(string serviceProviderCode, string usrId);

        /// <summary>
        /// Get my collection.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="usrId">current register user</param>
        /// <returns>Array of MyCollectionModel4WS</returns>
        MyCollectionModel[] GetMyCollection(string serviceProviderCode, string usrId);

        /// <summary>
        /// Move selected CAPs to specify collection. after this action is completed
        /// the selected CAPs will be moved from current collection.
        /// </summary>
        /// <param name="sourceCollection">source collection</param>
        /// <param name="targetCollection">target collection</param>
        void MoveCaps2Collection(MyCollectionModel sourceCollection, MyCollectionModel targetCollection);

        /// <summary>
        /// Remove selected CAPs to specify collection. after this action is completed
        /// the selected CAPs will be moved from current collection.
        /// </summary>
        /// <param name="collectionModel4WS">collectionModel4WS object.</param>
        void RemoveCapsFromCollection(MyCollectionModel collectionModel4WS);

        /// <summary>
        /// Update my collection.
        /// </summary>
        /// <param name="collectionModel">collectionModel4WS object.</param>
        void UpdateMyCollection(MyCollectionModel collectionModel);

        #endregion Methods
    }
}