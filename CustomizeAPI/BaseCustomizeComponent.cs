#region Header

/**
 *  Accela Citizen Access
 *  File: BaseCustomizeComponent.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *   It provides the base class for customized component that customer need inherit.
 *
 *  Notes:
 * $Id: BaseCustomizeComponent.cs 192687 2011-03-14 05:38:13Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Accela.ACA.CustomizeAPI
{
    /// <summary>
    /// It provides the base class for customized component. 
    /// </summary>
    public class BaseCustomizeComponent : UserControl, ICustomizeComponent
    {
        #region Fields

        /// <summary>
        /// The user context key
        /// </summary>
        private const string USER_CONTEXT_KEY = "USER_CONTEXT_KEY";

        /// <summary>
        /// The subset of customized component.
        /// </summary>
        private IList<ICustomizeComponent> _children;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the user context.
        /// </summary>
        public static UserContext UserContext
        {
            get
            {
                UserContext userContext = HttpContext.Current.Session[USER_CONTEXT_KEY] as UserContext;

                if (userContext != null)
                {
                    return userContext;
                }

                userContext = new UserContext();
                HttpContext.Current.Session[USER_CONTEXT_KEY] = userContext;

                return userContext;
            }
        }

        /// <summary>
        /// Gets or sets the subset of the customized component.
        /// </summary>
        public IList<ICustomizeComponent> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<ICustomizeComponent>();
                }

                return _children;
            }

            set
            {
                _children = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// It is called when page initialize, it used to display the customize components.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        public virtual ResultMessage Show()
        {
            if (_children != null && _children.Count > 0)
            {
                foreach (ICustomizeComponent component in _children)
                {
                    ResultMessage result = component.Show();

                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                }
            }

            // do self show
            return new ResultMessage
            {
                IsSuccess = true,
                Message = string.Empty
            };
        }

        /// <summary>
        /// It is called before save action, it used to validate before save.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        public virtual ResultMessage SaveBefore()
        {
            return new ResultMessage();
        }

        /// <summary>
        /// It is called when doing save action, it used to save the customize component information.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        public virtual ResultMessage Save()
        {
            return new ResultMessage();
        }

        /// <summary>
        /// It is called after save action, it used to do some action after save.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        public virtual ResultMessage SaveAfter()
        {
            return new ResultMessage();
        }

        /// <summary>
        /// It is called before [save and resume] action, it used to validate before [save and resume].
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        public virtual ResultMessage SaveAndResumeBefore()
        {
            return new ResultMessage();
        }

        /// <summary>
        /// It is called when doing [save and resume] action, it used to save the customize component information.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        public virtual ResultMessage SaveAndResume()
        {
            return new ResultMessage();
        }

        /// <summary>
        /// It is called after [save and resume] action, it used to do some action after [save and resume].
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        public virtual ResultMessage SaveAndResumeAfter()
        {
            return new ResultMessage();
        }
        
        #endregion Methods
    }
}
