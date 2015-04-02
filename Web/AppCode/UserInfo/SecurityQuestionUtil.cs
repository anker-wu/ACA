#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SecurityQuestionUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;
using System.Linq;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.UserInfo
{
    /// <summary>
    /// The Security Question utility
    /// </summary>
    public static class SecurityQuestionUtil
    {
        #region Fields

        /// <summary>
        /// The default quantity of security question.
        /// </summary>
        private const int DEFAULT_QUESTION_QUANTITY = 1;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Gets the authentication by security question setting.
        /// </summary>
        /// <returns>Return the authentication by security question setting.</returns>
        public static AuthBySecurityQuestionModel GetAuthBySecurityQuestionSetting()
        {
            AuthBySecurityQuestionModel authBySecurityQuestion = new AuthBySecurityQuestionModel();
            authBySecurityQuestion.CompulsoryQuantity = DEFAULT_QUESTION_QUANTITY;

            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();
            BizDomainModel4WS[] bizList = bizDomainBll.GetBizDomainValue(ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHENTICATION_BY_SECURITY_QUESTION, new QueryFormat4WS(), false, I18nCultureUtil.UserPreferredCulture);

            if (bizList != null)
            {
                string compulsoryQuantity = string.Empty;

                foreach (BizDomainModel4WS item in bizList)
                {
                    if (item.bizdomainValue == BizDomainConstant.STD_ITEM_ENABLE_AUTHENTICATION_BY_SECURITY_QUESTION)
                    {
                        authBySecurityQuestion.Enable = ValidationUtil.IsYes(item.description);
                    }
                    else if (item.bizdomainValue == BizDomainConstant.STD_ITEM_COMPULSORY_SECURITY_QUESTION_QUANTITY)
                    {
                        compulsoryQuantity = item.description;
                    }
                }

                if (authBySecurityQuestion.Enable && !string.IsNullOrEmpty(compulsoryQuantity))
                {
                    authBySecurityQuestion.CompulsoryQuantity = Convert.ToInt32(compulsoryQuantity);
                }
            }
            
            return authBySecurityQuestion;
        }

        /// <summary>
        /// Generate the security question model randomly.
        /// </summary>
        /// <param name="questionModels">The question model array.</param>
        /// <returns>Public user question model.</returns>
        public static PublicUserQuestionModel GenerateSecurityQuestionRandomly(PublicUserQuestionModel[] questionModels)
        {
            if (questionModels == null || questionModels.Length == 0)
            {
                return null;
            }

            Random random = new Random();
            int randomIndex = random.Next(questionModels.Count());

            return questionModels.ElementAt(randomIndex);
        }

        /// <summary>
        /// Check whether the user question list exist any active question or not.
        /// </summary>
        /// <param name="userQuestionModels">The user question model array.</param>
        /// <returns>true: exist active question, otherwise not exist.</returns>
        public static bool IsExistActiveQuestion(PublicUserQuestionModel[] userQuestionModels)
        {
            if (userQuestionModels == null)
            {
                return false;
            }

            return userQuestionModels.Any();
        }

        /// <summary>
        /// Check whether need to update user's security questions.
        /// </summary>
        /// <param name="userQuestionModels">The user question model array.</param>
        /// <returns>true: need to update, otherwise needn't.</returns>
        public static bool IsNeedUpdateUserQuestions(PublicUserQuestionModel[] userQuestionModels)
        {
            if (userQuestionModels == null)
            {
                return true;
            }

            return userQuestionModels.Count() < GetMultipleQuestionControlCount();
        }

        /// <summary>
        /// Get the multiple question control count.
        /// </summary>
        /// <param name="userQuestionCount">user question count.</param>
        /// <returns>multiple question control count.</returns>
        public static int GetMultipleQuestionControlCount(int userQuestionCount = 0)
        {
            int compulsoryQuestionCount = GetAuthBySecurityQuestionSetting().CompulsoryQuantity;

            return Math.Max(compulsoryQuestionCount, userQuestionCount);
        }

        #endregion Methods
    }
}