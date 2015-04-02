#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SpellChecker.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: SpellChecker.cs 143930 2009-08-19 10:40:51Z ACHIEVO\weiky chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   
 * </pre>
 */

#endregion Header

using System;
using System.Text;

using Accela.ACA.BLL;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web
{
    /// <summary>
    /// a class to check word spell
    /// </summary>
    public partial class SpellChecker : BasePageWithoutMaster
    {
        /// <summary>
        /// the constant string for NO_DICT
        /// </summary>
        private const string NO_DICT = "NO_DICT";

        /// <summary>
        /// check spell
        /// </summary>
        /// <param name="sentence">the sentence needs to check</param>
        /// <returns>The result of check spell.</returns>
        [System.Web.Services.WebMethod(Description = "DoSpellCheck", EnableSession = true)]
        public static string DoSpellCheck(string sentence)
        {
            ISpellCheckBll spellCheck = (ISpellCheckBll)ObjectFactory.GetObject(typeof(ISpellCheckBll));
            SpellCheckerResultModel result = spellCheck.CheckSpelling(sentence, ConfigManager.AgencyCode);

            if (result == null)
            {
                return string.Empty;
            }
            else if (result.returnCode == NO_DICT)
            {
                // not support current language
                return "-1";
            }

            StringBuilder spells = new StringBuilder();

            if (result.suggestions != null && result.suggestions.Length > 0)
            {
                spells.Append("[");

                foreach (SpellCheckerWordSuggestionsModel suggestionModel in result.suggestions)
                {
                    spells.Append("{Word:'");
                    spells.Append(ScriptFilter.EncodeJson(suggestionModel.word));
                    spells.Append("',WordContextPosition:");
                    spells.Append(suggestionModel.wordContextPosition);
                    spells.Append(",Suggestions:[");

                    if (suggestionModel.suggestions != null && suggestionModel.suggestions.Length > 0)
                    {
                        foreach (string suggestion in suggestionModel.suggestions)
                        {
                            spells.Append("\"");
                            spells.Append(suggestion);
                            spells.Append("\",");
                        }

                        spells.Length -= 1;
                    }

                    spells.Append("]},");
                }

                spells.Length -= 1;
                spells.Append("]");
            }

            return spells.ToString();
        }

        /// <summary>
        /// set page title when page loaded
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = GetTextByKey("acc_spell_check_title");
            original.ToolTip = GetTextByKey("aca_spellcheck_msg_original");
            replaceWith.ToolTip = GetTextByKey("aca_spellcheck_msg_replacewith");
            suggestions.ToolTip = GetTextByKey("aca_spellcheck_msg_suggestions");
        }
    }
}
