#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SocialMediaBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.SocialMedia
{
    /// <summary>
    /// Class Social Media business.
    /// </summary>
    public class SocialMediaBll : ISocialMediaBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of SocialMediaWebServiceService.
        /// </summary>
        /// <value>The social media web service.</value>
        private SocialMediaWebServiceService SocialMediaWebService
        {
            get
            {
                return WSFactory.Instance.GetWebService<SocialMediaWebServiceService>();
            }
        }

        /// <summary>
        /// Gets the public user web service service.
        /// </summary>
        /// <value>The public user web service service.</value>
        private PublicUserWebServiceService PublicUserWebServiceService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PublicUserWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Saves the social media4 public user.
        /// </summary>
        /// <param name="socialMediaUserModel">The social media user model.</param>
        public void SaveSocialMedia4PublicUser(XSocialMediaUserModel socialMediaUserModel)
        {
            try
            {
                SocialMediaWebService.saveSocialMedia4PublicUser(socialMediaUserModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets my share cap list.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="hiddenElementNameList">The hidden element name list.</param>
        /// <param name="callId">The call id.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>simple cap information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SimpleCapModel[] GetMyShareCapList(string servProvCode, string[] hiddenElementNameList, string callId, QueryFormat queryFormat)
        {
            try
            {
                return SocialMediaWebService.getMyShareCapList(servProvCode, hiddenElementNameList, callId, queryFormat);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Saves the social media entity.
        /// </summary>
        /// <param name="xSocialMediaEntityModel">The x social media entity model.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void SaveSocialMediaEntity(XSocialMediaEntityModel xSocialMediaEntityModel)
        {
            try
            {
                SocialMediaWebService.saveSocialMediaEntity(xSocialMediaEntityModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the public user by social media.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        /// <returns>public user information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS GetPublicUserBySocialMedia(string agencyCode, string id, string type)
        {
            try
            {
                return PublicUserWebServiceService.getPublicUserBySocialMedia(agencyCode, id, type);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        #endregion
    }
}
