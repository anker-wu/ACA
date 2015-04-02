#region Header

/**
 *  Accela Citizen Access
 *  File: ICustomizeComponent.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011
 *
 *  Description:
 *   The customize component interface.
 *
 *  Notes:
 * $Id: ICustomizeComponent.cs 192687 2011-03-14 05:38:13Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System.Collections.Generic;

namespace Accela.ACA.CustomizeAPI
{
    /// <summary>
    /// The customize component interface.
    /// </summary>
    public interface ICustomizeComponent
    {
        /// <summary>
        /// Gets or sets the subset of the customized component.
        /// </summary>
        /// <value>The subset of the customized component.</value>
        IList<ICustomizeComponent> Children
        {
            get;

            set;
        }

        /// <summary>
        /// It is called when page initialize, it used to display the customize components.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        ResultMessage Show();

        /// <summary>
        /// It is called before save action, it used to validate before save.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        ResultMessage SaveBefore();

        /// <summary>
        /// It is called when doing save action, it used to save the customize component information.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        ResultMessage Save();

        /// <summary>
        /// It is called after save action, it used to do some action after save.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        ResultMessage SaveAfter();

        /// <summary>
        /// It is called before [save and resume] action, it used to validate before [save and resume].
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        ResultMessage SaveAndResumeBefore();

        /// <summary>
        /// It is called when doing [save and resume] action, it used to save the customize component information.
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        ResultMessage SaveAndResume();

        /// <summary>
        /// It is called after [save and resume] action, it used to do some action after [save and resume].
        /// </summary>
        /// <returns>Return the result message indicate the action is success or not.</returns>
        ResultMessage SaveAndResumeAfter();
    }
}
