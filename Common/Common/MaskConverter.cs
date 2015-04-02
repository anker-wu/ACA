#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MaskConverter.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 *  MaskConverter is used to convert the mask of AA(standard choice) to ACA's
 * 
 *  Notes:
 * $Id: MaskConverter.cs 181867 2013-09-07 08:06:18Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.Common.Common
{
    /// <summary>
    /// This is a class that convert the mask of AA(standard choice) to ACA's
    /// </summary>
    public class MaskConverter
    {
        #region Fields

        /// <summary>
        /// The mask from AA(standard choice). 
        /// L [a-z] 
        /// U [A-Z] 
        /// A [a-zA-Z 0-9] 
        /// ? [A-Z] 
        /// # [0-9] 
        /// * [Any digit. When it is in '[]', it also indicate whether show star characters.]
        /// - [Fixed character]
        /// [ [Start flag of multiple type]
        /// ] [End flag of multiple type]
        /// </summary>
        private string _sourceMask = null;

        /// <summary>
        /// The custom filter
        /// </summary>
        private string _filter = null;

        /// <summary>
        /// The mask from ACA.
        /// </summary>
        private string _destMask = null;

        /// <summary>
        /// The validation expression
        /// </summary>
        private string _validationExpression = null;

        /// <summary>
        /// The password format of the mask, 
        /// such as: -**-***--*--, maybe format as: a**-***12*b4.
        /// </summary>
        private string _starFormat = null;

        /// <summary>
        /// The format of the mask, 
        /// such as: <code>'-xx-xxx--x--' or 'xxxxxxxxxxxx'</code>.
        /// </summary>
        private string _maskFormat = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MaskConverter class: convert the mask of AA(standard choice) to ACA's.
        /// </summary>
        /// <param name="sourceMask">The mask from AA(standard choice)</param>
        public MaskConverter(string sourceMask) : this(sourceMask, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaskConverter class: convert the mask of AA(standard choice) to ACA's.
        /// </summary>
        /// <param name="sourceMask">The mask from AA(standard choice)</param>
        /// <param name="filter">The custom filter</param>
        public MaskConverter(string sourceMask, string filter)
        {
            _sourceMask = sourceMask;
            _filter = filter;

            this.Parse();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the ACA's mask. 
        /// 9 = only numeric 
        /// L,l = only letter 
        /// u = only lower letter 
        /// U = only upper letter 
        /// S = only numeric and letter 
        /// $ = only letter and spaces 
        /// C,c = only Custom - read from this._Filtered 
        /// A,a = only letter and Custom 
        /// N,n = only numeric and Custom 
        /// ? = any digit
        /// </summary>
        public string Mask
        {
            get { return _destMask; }
        }

        /// <summary>
        /// Gets the validation expression.
        /// </summary>
        public string ValidationExpression
        {
            get { return _validationExpression; }
        }

        /// <summary>
        /// Gets a value indicating whether show star character.
        /// </summary>
        public bool ShowStarCharacter
        {
            get { return this.StarFormat != null && this.StarFormat.IndexOf('*') >= 0; }
        }

        /// <summary>
        /// Gets the star format of the mask, 
        /// such as: -**-***--*--, maybe format as: a**-***12*b4.
        /// </summary>
        public string StarFormat
        {
            get { return _starFormat; }
        }

        /// <summary>
        /// Gets the format of the mask, 
        /// such as: <code>'-xx-xxx--x--' or 'xxxxxxxxxxxx'</code>.
        /// </summary>
        public string MaskFormat
        {
            get { return _maskFormat; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Convert the mask of AA(standard choice) to ACA's
        /// </summary>
        private void Parse()
        {
            _destMask = null;
            _validationExpression = null;
            _starFormat = null;

            if (_sourceMask == null)
            {
                return;
            }

            for (int i = 0; i < _sourceMask.Length; i++)
            {
                bool hasStarMask = false;
                bool isFixedCharacter = false;
                char currMask = _sourceMask[i];

                // Deal with mask between '[' and ']'.
                if (currMask == '[')
                {
                    int multTypeEnd = _sourceMask.IndexOf(']', i);

                    if (multTypeEnd < i)
                    {
                        // invalid mask: [
                        continue;
                    }
                    else if (multTypeEnd == i + 1)
                    {
                        // invalid mask: []
                        i++;
                        continue;
                    }
                    else
                    {
                        // valid mask: [xxx]
                        string multType = _sourceMask.Substring(i + 1, multTypeEnd - i - 1);
                        string multTypeWithoutStar = multType.Replace("*", string.Empty);

                        // Because there is '*', show star characters.
                        if (multTypeWithoutStar.Length != multType.Length)
                        {
                            hasStarMask = true;
                        }

                        /* If there are all '*' between '[' and ']', mask is any digit. Otherwise other mask format.
                         * The first character except the star character is valid only.
                         * "[*]" -> currMask = "*"
                         * "[L]" -> currMask = "L"
                         * "[*L]" -> currMask = "L"
                         * "[*LU]" -> currMask = "L"
                         */
                        currMask = multTypeWithoutStar.Length == 0 ? '*' : multTypeWithoutStar[0];

                        i = multTypeEnd;
                    }
                }

                switch (currMask)
                {
                    case 'L': // [a-z]
                        _destMask += "u";
                        break;
                    case 'U': // [A-Z]
                        _destMask += "U";
                        break;
                    case 'A': // [a-zA-Z 0-9]
                        _destMask += "S";
                        break;
                    case '?': // [A-Z]
                        _destMask += "L";
                        break;
                    case '#': // [0-9]
                        _destMask += "9";
                        break;
                    case '*': 
                        // Any digit. When it is in '[]', it also indicate whether show star characters.
                        _destMask += "?";
                        break;
                    case '-': // Fixed character
                        _destMask += currMask;
                        isFixedCharacter = true;
                        break;
                    case ']': // Invalid end flag of multiple type
                        continue;
                    default:
                        _destMask += currMask;
                        break;
                }

                _starFormat += hasStarMask ? "*" : "-";
                _maskFormat += isFixedCharacter ? "-" : "x";
            }

            // Convert to validation expression(Javascript's regex)
            ConvertToValidationExpression();
        }

        /// <summary>
        /// Convert the mask of ACA to validation expression(JavaScript's regex). 
        /// If there is the star character at somewhere, it will allow input the star character in that position.
        /// </summary>
        private void ConvertToValidationExpression()
        {
            _validationExpression = @"(^";

            if (!string.IsNullOrEmpty(Mask))
            {
                char prevMask = Mask[0];
                int prevCharNum = 0;
                int index = 0;

                // Whether the previous character is star.
                bool isStarInPrev = false; 

                foreach (char currMask in Mask)
                {
                    string expressionStart = null;
                    bool isStarInCurr = _starFormat[index] == '*';

                    switch (currMask)
                    {
                        case '9': // only numeric
                            expressionStart = "([0-9";
                            break;
                        case 'L': // only letter
                        case 'l': // only letter
                            expressionStart = "([a-zA-Z";
                            break;
                        case 'u': // only lower letter
                            expressionStart = "([a-z";
                            break;
                        case 'U': // only upper letter
                            expressionStart = "([A-Z";
                            break;
                        case 'S': // only numeric and letter
                            expressionStart = "([a-zA-Z0-9";
                            break;
                        case '$': // only letter and spaces
                            expressionStart = @"([a-zA-Z\s";
                            break;
                        case 'A': // only letter and Custom
                        case 'a': // only letter and Custom
                            expressionStart = "([a-zA-Z" + _filter;
                            break;
                        case 'N': // only numeric and Custom
                        case 'n': // only numeric and Custom
                            expressionStart = "([0-9" + _filter;
                            break;
                        case '?': // any digit
                            expressionStart = @"([\S";
                            break;
                        case '-': // Fixed character
                            if (prevCharNum > 0)
                            {
                                _validationExpression += (isStarInPrev ? "*" : string.Empty) + "]{" + prevCharNum + "})";
                                prevCharNum = 0;
                            }

                            _validationExpression += "-";
                            break;
                        default:
                            expressionStart = "([" + currMask;
                            break;
                    }

                    if (expressionStart != null)
                    {
                        if (index > 0
                            && prevMask != '-'
                            && (prevMask != currMask
                                || (prevMask == currMask && isStarInPrev != isStarInCurr)))
                        {
                            _validationExpression += (isStarInPrev ? "*" : string.Empty) + "]{" + prevCharNum + "})";
                            prevCharNum = 0;
                        }

                        if (prevCharNum == 0)
                        {
                            _validationExpression += expressionStart;
                        }

                        prevCharNum++;
                    }

                    isStarInPrev = isStarInCurr;
                    prevMask = currMask;
                    index++;
                }

                // Add the number of last mask
                if (prevCharNum > 0)
                {
                    _validationExpression += (isStarInPrev ? "*" : string.Empty) + "]{" + prevCharNum + "})";
                }
            }

            _validationExpression += "$)";
        }

        #endregion Methods
    }
}
