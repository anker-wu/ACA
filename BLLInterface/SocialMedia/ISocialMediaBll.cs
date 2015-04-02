#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ISocialMediaBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.SocialMedia
{
    /// <summary>
    /// Interface Social Media business
    /// </summary>
    public interface ISocialMediaBll
    {
        /// <summary>
        /// Saves the social media4 public user.
        /// </summary>
        /// <param name="socialMediaUserModel">The social media user model.</param>
        void SaveSocialMedia4PublicUser(XSocialMediaUserModel socialMediaUserModel);

        /// <summary>
        /// Gets my share cap list.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="hiddenElementNameList">The hidden element name list.</param>
        /// <param name="callId">The call id.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>simple cap information</returns>
        SimpleCapModel[] GetMyShareCapList(string servProvCode, string[] hiddenElementNameList, string callId, QueryFormat queryFormat);

        /// <summary>
        /// Saves the social media entity.
        /// </summary>
        /// <param name="xSocialMediaEntityModel">The x social media entity model.</param>
        void SaveSocialMediaEntity(XSocialMediaEntityModel xSocialMediaEntityModel);

        /// <summary>
        /// Gets the public user by social media.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        /// <returns>public user information</returns>
        PublicUserModel4WS GetPublicUserBySocialMedia(string agencyCode, string id, string type);
    }
}
