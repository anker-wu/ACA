#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CommonUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  utitily for common.
 *
 *  Notes:
 * $Id: CommonUtil.cs 182945 2013-05-22 08:22:49Z ACHIEVO\jone.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Common utility
    /// </summary>
    public static class CommonUtil
    {
        /// <summary>
        /// Gets the random unique id.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The random unique id.</returns>
        public static string GetRandomUniqueID(string format = null)
        {
            string formatString = string.IsNullOrEmpty(format)
                                      ? Guid.NewGuid().ToString()
                                      : Guid.NewGuid().ToString(format);
            return formatString;
        }

        /// <summary>
        /// Gets the double value.
        /// </summary>
        /// <param name="text">A double value with text format</param>
        /// <returns>A double value</returns>
        public static double? GetDoubleValue(string text)
        {
            double? result = null;
            double parsedResult = 0;

            if (I18nNumberUtil.TryParseNumberFromInput(text, out parsedResult))
            {
                result = parsedResult;
            }

            return result;
        }

        /// <summary>
        /// Indicates the user is authorized agent or not.
        /// </summary>
        /// <param name="user">The public user</param>
        /// <returns>true to indicate the user is authorized agent.</returns>
        public static bool IsAuthorizedAgent(PublicUserModel4WS user)
        {
            if (ACAConstant.PUBLICUSER_TYPE_AUTH_AGENT.Equals(user.accountType, StringComparison.OrdinalIgnoreCase)
                && user.authServiceProviderCode == ConfigManager.AgencyCode)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates the user is agent clerk or not.
        /// </summary>
        /// <param name="user">The public user.</param>
        /// <returns>true to indicate the user is authorized agent clerk.</returns>
        public static bool IsAgentClerk(PublicUserModel4WS user)
        {
            if (ACAConstant.PUBLICUSER_TYPE_AUTH_AGENT_CLERK.Equals(user.accountType, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(user.authAgentID) && (user.authServiceProviderCode == ConfigManager.AgencyCode))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The serialized object.</returns>
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            byte[] bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();

            return bytes;
        }

        /// <summary>
        /// Remove Time Value from Birth and Decease Date for People Model
        /// </summary>
        /// <param name="peoples">The People Model list.</param>
        public static void AdjustTime4PeopleBirthAndDeceaseDate(PeopleModel[] peoples)
        {
            if (peoples != null && peoples.Length > 0)
            {
                foreach (PeopleModel people in peoples)
                {
                    AdjustTime4PeopleBirthAndDeceaseDate(people);
                }
            }
        }

        /// <summary>
        /// Remove Time Value from Birth and Decease Date for People Model
        /// </summary>
        /// <param name="people">The People Model.</param>
        public static void AdjustTime4PeopleBirthAndDeceaseDate(PeopleModel people)
        {
            if (people != null)
            {
                people.birthDate = AdjustTimeValue(people.birthDate);
                people.endBirthDate = AdjustTimeValue(people.endBirthDate);
                people.deceasedDate = AdjustTimeValue(people.deceasedDate);
                people.endDeceasedDate = AdjustTimeValue(people.endDeceasedDate);
            }
        }

        /// <summary>
        /// Remove Time Value from Birth for License Professional Model
        /// </summary>
        /// <param name="licenseProfessionals">The License Professional Model list.</param>
        public static void AdjustTime4LicenseProfessionalBirthDate(LicenseProfessionalModel[] licenseProfessionals)
        {
            if (licenseProfessionals != null && licenseProfessionals.Length > 0)
            {
                foreach (LicenseProfessionalModel licenseProfessional in licenseProfessionals)
                {
                    if (licenseProfessional == null)
                    {
                        continue;
                    }

                    licenseProfessional.birthDate = AdjustTimeValue(licenseProfessional.birthDate);
                    licenseProfessional.endbirthDate = AdjustTimeValue(licenseProfessional.endbirthDate);
                }
            }
        }
        
        /// <summary>
        /// Remove Time Value for some date field like BirthDate to resolve the Daylight conversion issue between Java and .NET.
        /// The problem's description: 
        ///     1974/03/09 00:00:00AM in Java side would be changed to 1974/03/08 11:00:00PM in .NET side.
        /// To resolve this problem, Java side would append one 12:00PM and then .NET site remove it(Maybe 11:00AM).
        /// The best solution is change DateTime type to string type in the future.
        /// Related Article: <c>http://msdn.microsoft.com/en-us/library/system.globalization.daylighttime(v=vs.100).aspx</c>
        /// </summary>
        /// <param name="date">The datetime.</param>
        /// <returns>The adjusted time value.</returns>
        private static DateTime? AdjustTimeValue(DateTime? date)
        {
            if (date != null && date.HasValue)
            {
                date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
            }

            return date;
        }
    }
}
