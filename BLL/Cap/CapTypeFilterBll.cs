#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapTypeFilterBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CapTypeFilterBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using log4net;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// This class provide the ability to operation CAP type filter.
    /// </summary>
    public sealed class CapTypeFilterBll : BaseBll, ICapTypeFilterBll
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapTypeFilterBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of CapTypeFilterWebService.
        /// </summary>
        private CapTypeFilterManagerWebServiceService CapTypeFilterService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapTypeFilterManagerWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Edit filter for button agency level.
        /// </summary>
        /// <param name="xBtnFilter4WS">this model contains the label key of a control and its filter name</param>
        /// <param name="callerId">The caller unique identifier.</param>
        public void EditFilter4ButtonAgencyLevel(XButtonFilterModel4WS xBtnFilter4WS, string callerId)
        {
            CapTypeFilterService.editFilter4ButtonAgencyLevel(xBtnFilter4WS, callerId);
        }

        /// <summary>
        /// Get cap type filter by label key
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="labelKey">label's key</param>
        /// <returns>cap type filter</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetCapTypeFilterByLabelKey(string agencyCode, string moduleName, string labelKey)
        {
            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                Hashtable htCapTypeFilters = cacheManager.GetCachedItem(agencyCode, agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_CAPTYPEFILTER));

                string key = moduleName + labelKey;

                if (htCapTypeFilters.ContainsKey(key) && htCapTypeFilters[key] != null)
                {
                    return htCapTypeFilters[key].ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get filter button list
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="filterName">filter Name</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>Array of Button Filter</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public XButtonFilterModel4WS[] GetFilter4ButtonListByFilterName(string servProvCode, string moduleName, string filterName, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin GetFilter4ButtonListByFilterName");
            }

            try
            {
                XButtonFilterModel4WS[] buttonFilters = CapTypeFilterService.getFilter4ButtonListByFilterName(servProvCode, moduleName, filterName, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End GetFilter4ButtonListByFilterName");
                }

                return buttonFilters;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Create Cap Type Filter
        /// </summary>
        /// <param name="filterModel">filter model</param>
        /// <param name="callerId">caller Id number</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        void ICapTypeFilterBll.CreateCapTypeFilter(CapTypeFilterModel4WS filterModel, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.CreateCapTypeFilter()");
            }

            try
            {
                CapTypeFilterService.createCapTypeFilter(filterModel, callerId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.CreateCapTypeFilter()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Create or edit filter 4 button
        /// </summary>
        /// <param name="xBtnFilter4WS">button filter entity</param>
        /// <param name="callerID">The caller unique identifier.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        void ICapTypeFilterBll.CreateOrEditFilter4Button(XButtonFilterModel4WS xBtnFilter4WS, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CreateOrEditFilter4Button");
            }

            try
            {
                CapTypeFilterService.createOrEditFilter4Button(xBtnFilter4WS, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CreateOrEditFilter4Button");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Delete cap type filter
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter name</param>
        /// <param name="callerId">caller Id number</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        void ICapTypeFilterBll.DeleteCapTypeFilter(string agencyCode, string moduleName, string filterName, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.DeleteCapTypeFilter()");
            }

            try
            {
                CapTypeFilterService.deleteCapTypeFilter(agencyCode, moduleName, filterName, callerId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.DeleteCapTypeFilter()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Edit cap type filter
        /// </summary>
        /// <param name="filterModel">filter Model</param>
        /// <param name="callerId">caller Id number</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        void ICapTypeFilterBll.EditCapTypeFilter(CapTypeFilterModel4WS filterModel, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.EditCapTypeFilter()");
            }

            try
            {
                CapTypeFilterService.editCapTypeFilter(filterModel, callerId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.EditCapTypeFilter()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get all relation on button2 filter
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>all relations</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        Hashtable ICapTypeFilterBll.GetAllRelationOnButton2Filter(string agencyCode, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.GetAllRelationOnButton2Filter");
            }

            try
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.GetAllRelationOnButton2Filter");
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get cap type filter list
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>cap type filter list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        ArrayList ICapTypeFilterBll.GetCapTypeFilterList(string agencyCode, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.GetCapTypeFilterList");
            }

            try
            {
                CapTypeFilterModel4WS[] filterNameList = CapTypeFilterService.getCapTypeFilterList(agencyCode, callerId);

                if (filterNameList == null || filterNameList.Length < 1)
                {
                    return new ArrayList();
                }

                ArrayList filterList = new ArrayList(filterNameList);
                CapTypeFilterComparer filterComparer = new CapTypeFilterComparer();

                filterList.Sort(filterComparer);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.GetCapTypeFilterList");
                }

                return filterList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get cap type filter list by module
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>filter list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        string[] ICapTypeFilterBll.GetCapTypeFilterListByModule(string agencyCode, string moduleName, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.GetCapTypeFilterListByModule");
            }

            try
            {
                string[] filterNames = CapTypeFilterService.getCapTypeFilterListByModule(agencyCode, moduleName, callerId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.GetCapTypeFilterListByModule");
                }

                return filterNames;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// This function gets the detail information of a cap type filter
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter name</param>
        /// <param name="callerId">caller ID number</param>
        /// <returns>Cap Type Filter entity</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        CapTypeFilterModel4WS ICapTypeFilterBll.GetCapTypeFilterModel(string agencyCode, string moduleName, string filterName, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.GetCapTypeFilterModel");
            }

            try
            {
                CapTypeFilterModel4WS capTypeFilter = CapTypeFilterService.getCapTypeFilterModel(agencyCode, moduleName, filterName, callerId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.GetCapTypeFilterModel");
                }

                return capTypeFilter;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get filter 4 button
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="labelKey">label's key</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>Filter 4 button</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        string ICapTypeFilterBll.GetFilter4Button(string agencyCode, string moduleName, string labelKey, string callerId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeFilterBll.GetFilter4Button");
            }

            try
            {
                if (string.IsNullOrEmpty(moduleName) || string.IsNullOrEmpty(labelKey))
                {
                    return string.Empty;
                }

                XButtonFilterModel4WS xButtonFilter = CapTypeFilterService.getFilter4Button(agencyCode, moduleName, labelKey, callerId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeFilterBll.GetFilter4Button");
                }

                return xButtonFilter == null ? string.Empty : xButtonFilter.filterName;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get filter 4 button
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="labelKey">label's key</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>Filter 4 button</returns>
        public XButtonFilterModel4WS GetFilter4ButtonModel(string agencyCode, string moduleName, string labelKey, string callerId)
        {
            XButtonFilterModel4WS xButtonFilter = CapTypeFilterService.getFilter4Button(agencyCode, moduleName, labelKey, callerId);

            return xButtonFilter;
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// the class for CapTypeFilterComparer.
        /// </summary>
        internal class CapTypeFilterComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.</returns>
            int IComparer.Compare(object x, object y)
            {
                CapTypeFilterModel4WS filter1 = x as CapTypeFilterModel4WS;
                CapTypeFilterModel4WS filter2 = y as CapTypeFilterModel4WS;

                //compare the order
                return filter1.moduleName.CompareTo(filter2.moduleName);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}