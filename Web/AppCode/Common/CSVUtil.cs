#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CSVUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  CSV File Parse
 *
 *  Notes:
 *      $Id: CSVUtil.cs 130988 2009-05-15 10:48:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// CSV file structure parse
    /// </summary>
    public static class CSVUtil
    {
        #region Private Property

        /// <summary>
        /// CSV String Split Char
        /// </summary>
        private const char SPLIT_CHAR = ',';

        /// <summary>
        /// CSV String Split Char
        /// </summary>
        private const char SPLIT_CHAR2 = ';';

        /// <summary>
        /// CSV String Split Char
        /// </summary>
        private const char SPLIT_CHAR3 = '=';

        /// <summary>
        /// CSV String Split Char
        /// </summary>
        private const char SPLIT_CHAR4 = '|';

        /// <summary>
        /// CSV Standard Column Name
        /// </summary>
        private const string COLUMN_NAME = "COLUMN";

        /// <summary>
        /// CSV Column in BizDomain value for grading style.
        /// </summary>
        private const string COLUMN_NAME_GRADING_STYLE = "GRADING_STYLE";

        /// <summary>
        /// CSV Column in BizDomain value for score.
        /// </summary>
        private const string COLUMN_NAME_SCORE = "SCORE";

        /// <summary>
        /// CSV Column in BizDomain value for passing score.
        /// </summary>
        private const string COLUMN_NAME_PASSING_SCORE = "PASSING_SCORE";

        /// <summary>
        /// CSV Column in BizDomain value for provider number.
        /// </summary>
        private const string COLUMN_NAME_PROVIDER_NBR = "PROVIDER_NBR";

        /// <summary>
        /// CSV Column in BizDomain value for provider name.
        /// </summary>
        private const string COLUMN_NAME_PROVIDER_NAME = "PROVIDER_NAME"; 

        /// <summary>
        /// Store BizDomain value of the CSV Column
        /// </summary>
        private const string BIZ_KEY = "BIZ_KEY";

        /// <summary>
        /// CSV Standard Format Name
        /// </summary>
        private const string COLUMN_FORMAT = "FORMAT";

        /// <summary>
        /// CSV Standard Type Name
        /// </summary>
        private const string COLUMN_TYPE = "TYPE";

        /// <summary>
        /// CSV Standard Type enumeration
        /// </summary>
        private const string COLUMN_TYPE_ENUM = "ENUM";

        /// <summary>
        /// The score range start.
        /// </summary>
        private const int SCORE_RANGE_START = 0;

        /// <summary>
        /// The score range end.
        /// </summary>
        private const int SCORE_RANGE_END = 99999;

        /// <summary>
        /// The score range end for the percentage.
        /// </summary>
        private const int SCORE_RANGE_PERCENTAGE_END = 100;

        /// <summary>
        /// Record Begin Row Number
        /// </summary>
        private const int RECORD_BEGINROWNUMBER = 2;

        /// <summary>
        /// Header Structure Error Message
        /// </summary>
        private const string HEADER_ERROR_MESSAGE = "csv_parse_header_error_message";

        /// <summary>
        /// Record Structure Error Message.
        /// </summary>
        private const string RECORD_LENGTH_ERROR_MESSAGE = "csv_parse_record_length_error_message";

        /// <summary>
        /// Record Empty Error Message.
        /// </summary>
        private const string RECORD_EMPTY_ERROR_MESSAGE = "csv_parse_record_empty_error_message";

        /// <summary>
        /// Record Item Type Error Message
        /// </summary>
        private const string RECORD_TYPE_ERROR_MESSAGE = "csv_parse_type_error_message";

        /// <summary>
        /// Record Item Date Invalid Error Message
        /// </summary>
        private const string RECORD_DATE_INVALID_MESSAGE = "csv_parse_date_invalid_message";

        /// <summary>
        /// Record Item Provider Invalid Error Message
        /// </summary>
        private const string RECORD_PROVIDER_INVALID_MESSAGE = "csv_parse_provider_invalid_message";

        /// <summary>
        /// Record Item Row Message
        /// </summary>
        private const string RECORD_ROW_MESSAGE = "csv_parse_row_number_message";

        /// <summary>
        /// Record None Message
        /// </summary>
        private const string RECORD_NONE_MESSAGE = "csv_parse_error_message_norecord";

        /// <summary>
        /// Record Format Message
        /// </summary>
        private const string RECORD_FORMAT_MESSAGE = "&nbsp;({0})";

        /// <summary>
        /// current Error Message Index
        /// </summary>
        private static int errorMesssageIndex = 0;

        #endregion

        #region public Methods

        /// <summary>
        /// Check CSV Format. return error message, if return empty means file format is right
        /// </summary>
        /// <param name="filePath">CSV File Path</param>
        /// <param name="csvFormat">CSV File Format</param>
        /// <param name="returnMessageRow">Max Check Message error rows</param>
        /// <param name="selectProviderName">selected provider name.</param>
        /// <param name="selectProviderNumber">selected provider number.</param>
        /// <returns>check error Message, return Empty means file format is right.</returns>
        public static string CheckCSVFormat(string filePath, string csvFormat, int returnMessageRow, string selectProviderName, string selectProviderNumber)
        {
            StringBuilder checkMessage = new StringBuilder();

            using (StreamReader reader = new StreamReader(filePath))
            {
                //Get standard CSV cells.
                Hashtable standardHeader = GetStandardCSVHeader(csvFormat);

                //Get current CSV cells.
                string[] currentHeader = GetCurrentCSVRecord(reader.ReadLine());

                //Check Header Structure validity
                string checkHeaderMessage = CheckCSVHeader(standardHeader, ConvertArrayToHashTable(currentHeader));

                //checkHeaderMessage is empty means heander format is right, continue to check Record.
                //Get CSV Records
                if (string.IsNullOrEmpty(checkHeaderMessage))
                {
                    int recordRow = RECORD_BEGINROWNUMBER;
                    errorMesssageIndex = 0;
                    
                    while (!reader.EndOfStream)
                    {
                        if (errorMesssageIndex >= returnMessageRow)
                        {
                            checkMessage.Append(ACAConstant.HTML_BR + "......");
                            break;
                        }

                        //Get current record
                        string[] currentRecord = GetCurrentCSVRecord(reader.ReadLine());

                        //Check Record structure validity
                        string checkRecordMessage = CheckCSVRecord(standardHeader, currentHeader, currentRecord, recordRow, selectProviderName, selectProviderNumber);

                        if (!string.IsNullOrEmpty(checkRecordMessage))
                        {
                            checkMessage.Append(checkRecordMessage);
                        }
                        
                        recordRow++;
                    }

                    //Blank Record
                    if (recordRow == RECORD_BEGINROWNUMBER)
                    {
                        checkMessage.Append(LabelUtil.GetTextByKey(RECORD_NONE_MESSAGE, null));
                    }
                }
                else
                {
                    checkMessage.Append(checkHeaderMessage);
                }
            }

            return checkMessage.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts a string into CSV - encoded string
        /// </summary>
        /// <param name="content">go to Encode string</param>
        /// <returns>CSV - encoded string</returns>
        private static string CSVEncode(string content)
        {
            string strEn = content;

            if (!string.IsNullOrEmpty(strEn))
            {
            }

            return strEn;
        }

        /// <summary>
        /// Converts a string that has been CSV - encoded string into Decode string. 
        /// </summary>
        /// <param name="content">go to Decode string</param>
        /// <returns>decode string</returns>
        private static string CSVDecode(string content)
        {
            string strDe = content;

            if (!string.IsNullOrEmpty(strDe))
            {
            }

            return strDe;
        }

        /// <summary>
        /// Get Standard CSV Header
        /// </summary>
        /// <param name="csvFormat">CSV File Format</param>
        /// <returns>Hashtable with Standard CSV Header Property and Type</returns>
        private static Hashtable GetStandardCSVHeader(string csvFormat)
        {
            Hashtable standardHeader = new Hashtable();

            if (csvFormat != null)
            {
                //Get BizModels
                IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
                IList<ItemValue> bizModels = bizBll.GetBizDomainList(ConfigManager.AgencyCode, csvFormat, false);

                if (bizModels != null && bizModels.Count > 0)
                {
                    foreach (ItemValue bizModel in bizModels)
                    {
                        //Build Standard Header 
                        Hashtable htCell = ConvertBizDomainToHashTable(bizModel);

                        if (htCell[COLUMN_NAME] != null)
                        {
                            standardHeader.Add(htCell[COLUMN_NAME], htCell);
                        }
                    }
                }
            }

            return standardHeader;
        }

        /// <summary>
        /// Convert a string into Hashtable
        /// </summary>
        /// <param name="content">Header String</param>
        /// <returns>Hashtable with Header Property</returns>
        private static string[] GetCurrentCSVRecord(string content)
        {
            string[] currentRecord = null;

            if (!string.IsNullOrEmpty(content))
            {
                currentRecord = content.Split(SPLIT_CHAR);
            }

            return currentRecord;
        }

        /// <summary>
        /// Check Cell Value
        /// </summary>
        /// <param name="content">Record Cell Value</param>
        /// <param name="columnType">The column type</param>
        /// <param name="columnFormat">Column Format</param>
        /// <returns>Cell Value type is Standard type, return true</returns>
        private static bool CheckCellType(string content, string columnType, string columnFormat)
        {
            bool checkType = true;

            if (string.IsNullOrEmpty(content))
            {
                checkType = false;
            }
            else if (!string.IsNullOrEmpty(columnType))
            {
                try
                {
                    if (COLUMN_TYPE_ENUM.Equals(columnType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        checkType = false;
                        string[] enumValues = columnFormat.Split(SPLIT_CHAR4);

                        foreach (string enumValue in enumValues)
                        {
                            if (content.Equals(enumValue, StringComparison.InvariantCultureIgnoreCase))
                            {
                                checkType = true;
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    checkType = false;
                }
            }

            return checkType;
        }

        /// <summary>
        /// Check CSV Header, return empty means header format is right
        /// </summary>
        /// <param name="standardHeader">Standard Header</param>
        /// <param name="currentHeader">Current Header</param>
        /// <returns>Message of Check , return empty means header format is right</returns>
        private static string CheckCSVHeader(Hashtable standardHeader, Hashtable currentHeader)
        {
            StringBuilder checkCSVHeaderMessage = new StringBuilder();

            if (standardHeader != null && currentHeader != null)
            {
                foreach (string standardKey in standardHeader.Keys)
                {
                    if (!currentHeader.ContainsKey(standardKey.ToUpper()))
                    {
                        checkCSVHeaderMessage.Append(string.Format(LabelUtil.GetTextByKey(HEADER_ERROR_MESSAGE, null), standardKey));
                        checkCSVHeaderMessage.Append(ACAConstant.HTML_BR);
                    }
                }
            }

            return checkCSVHeaderMessage.ToString();
        }

        /// <summary>
        /// Check Record, return empty means record format is right
        /// </summary>
        /// <param name="standardHeader">Standard Header HashTable</param>
        /// <param name="currentHeader">Current Header Structure</param>
        /// <param name="currentRecord">Current Record Structure</param>
        /// <param name="recordRow">Record Row Index</param>
        /// <param name="selectProviderName">selected provider name.</param>
        /// <param name="selectProviderNumber">selected provider number.</param>
        /// <returns>Check Message, return empty means record format is right.</returns>
        private static string CheckCSVRecord(Hashtable standardHeader, string[] currentHeader, string[] currentRecord, int recordRow, string selectProviderName, string selectProviderNumber)
        {
            StringBuilder checkCSVRecordMessage = new StringBuilder();            

            if (standardHeader != null && standardHeader.Count > 0 && currentHeader != null && currentHeader.Length > 0 && currentRecord != null && currentRecord.Length > 0)
            {
                if (currentHeader.Length != currentRecord.Length)
                {
                    checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_ROW_MESSAGE, null), recordRow.ToString()));
                    checkCSVRecordMessage.Append(LabelUtil.GetTextByKey(RECORD_LENGTH_ERROR_MESSAGE, null));
                    checkCSVRecordMessage.Append(ACAConstant.HTML_BR);
                    errorMesssageIndex++;
                }
                else
                {
                    string gradingStyle = string.Empty;
                    string score = string.Empty;
                    string scoreCellName = string.Empty;
                    string passingScore = string.Empty;
                    string passingScoreCellName = string.Empty;
                    string providerName = string.Empty;
                    string providerNumber = string.Empty;
                    string examDate = string.Empty;
                    string examDateHeader = string.Empty;
                    string dateFormat = string.Empty;

                    for (int i = 0; i < currentHeader.Length; i++)
                    {
                        bool isCheckType = true;
                        string cellName = currentHeader[i].ToString();
                        string cellValue = currentRecord[i].ToString();
                        Hashtable cellData = standardHeader[cellName] as Hashtable;

                        // Remove special character ". For example: "123" should be 123.
                        if (!string.IsNullOrEmpty(cellValue) && cellValue.StartsWith("\"") && cellValue.EndsWith("\"") && cellValue.Length >= 2)
                        {
                            cellValue = cellValue.Remove(0, 1);
                            cellValue = cellValue.Remove(cellValue.Length - 1, 1);
                        }

                        if (cellData == null)
                        {
                            continue;
                        }

                        string cellType = (cellData != null && cellData[COLUMN_TYPE] != null) ? cellData[COLUMN_TYPE].ToString() : string.Empty;
                        string errorMsg = (cellData != null && cellData[COLUMN_FORMAT] != null) ? cellData[COLUMN_FORMAT].ToString() : string.Empty;

                        if (COLUMN_NAME_GRADING_STYLE.Equals(cellData[BIZ_KEY].ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            gradingStyle = cellValue;
                        }

                        if (COLUMN_NAME_SCORE.Equals(cellData[BIZ_KEY].ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            score = cellValue;
                            scoreCellName = cellName;

                            //Needs not check "Score" column.
                            isCheckType = false;
                        }

                        if (COLUMN_NAME_PROVIDER_NBR.Equals(cellData[BIZ_KEY].ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            providerNumber = cellValue;
                        }

                        if (COLUMN_NAME_PROVIDER_NAME.Equals(cellData[BIZ_KEY].ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            providerName = cellValue;
                        }

                        if (COLUMN_NAME_PASSING_SCORE.Equals(cellData[BIZ_KEY].ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            passingScore = cellValue;
                            passingScoreCellName = cellName;

                            //Needs not check "Passing Score" column.
                            isCheckType = false;
                        }

                        if (ACAConstant.INSPECTION_ACTIVITY_DATE.Equals(cellType, StringComparison.InvariantCultureIgnoreCase))
                        {
                            examDate = cellValue;
                            dateFormat = (cellData != null && cellData[COLUMN_FORMAT] != null) ? cellData[COLUMN_FORMAT].ToString() : string.Empty;
                            examDateHeader = cellName;
                        }

                        if (isCheckType && !CheckCellType(cellValue, cellType, errorMsg))
                        {
                            errorMsg = string.IsNullOrEmpty(errorMsg) ? errorMsg : string.Format(RECORD_FORMAT_MESSAGE, errorMsg);

                            checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_ROW_MESSAGE, null), recordRow.ToString()));
                            checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_TYPE_ERROR_MESSAGE, null), cellValue, cellName + errorMsg));
                            checkCSVRecordMessage.Append(ACAConstant.HTML_BR);
                            errorMesssageIndex++;
                        }
                    }

                    // Validate final score by grading style.
                    CheckScoreByGradingStyle(checkCSVRecordMessage, scoreCellName, score, gradingStyle, recordRow, false);

                    // Validate passing score by grading style.
                    CheckScoreByGradingStyle(checkCSVRecordMessage, passingScoreCellName, passingScore, gradingStyle, recordRow, true);

                    // Validate provider and corresponding examination.
                    CheckProviderInfo(checkCSVRecordMessage, providerName, selectProviderName, providerNumber, selectProviderNumber, recordRow);
                    
                    if (!string.IsNullOrEmpty(examDate))
                    {
                        CheckExamDate(checkCSVRecordMessage, examDate, dateFormat, examDateHeader, recordRow);
                    }
                }
            }
            else
            {
                checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_ROW_MESSAGE, null), recordRow.ToString()));
                checkCSVRecordMessage.Append(LabelUtil.GetTextByKey(RECORD_EMPTY_ERROR_MESSAGE, null));
                checkCSVRecordMessage.Append(ACAConstant.HTML_BR);
                errorMesssageIndex++;
            }

            return checkCSVRecordMessage.ToString();
        }

        /// <summary>
        /// Convert Array into HashTable
        /// </summary>
        /// <param name="array">String Array</param>
        /// <returns>HashTable,Key is string,Value is null</returns>
        private static Hashtable ConvertArrayToHashTable(string[] array)
        {
            Hashtable ht = new Hashtable();

            if (array != null && array.Length > 0)
            {
                foreach (string item in array)
                {
                    ht[item.ToUpper()] = null;
                }
            }

            return ht;
        }

        /// <summary>
        /// Convert Biz Domain To HashTable
        /// </summary>
        /// <param name="bizDomain">BizDomain Module</param>
        /// <returns>Hash Table</returns>
        private static Hashtable ConvertBizDomainToHashTable(ItemValue bizDomain)
        {
            Hashtable htCell = new Hashtable();

            if (bizDomain != null && bizDomain.Value != null && !string.IsNullOrEmpty(bizDomain.Value.ToString()))
            {
                string[] cells = bizDomain.Value.ToString().Split(SPLIT_CHAR2);

                foreach (string cell in cells)
                {
                    if (string.IsNullOrEmpty(cell))
                    {
                        continue;
                    }

                    string[] values = cell.Trim().Split(SPLIT_CHAR3);
                    string key = values[0].Trim().ToUpper();
                    string value = values[1].Trim();

                    if (values.Length == 2 && !htCell.ContainsKey(key))
                    {
                        htCell.Add(key, value);
                    }
                }

                htCell.Add(BIZ_KEY, bizDomain.Key);
            }

            return htCell;
        }

        /// <summary>
        /// Validate score by grading style.
        /// </summary>
        /// <param name="checkCSVRecordMessage">StringBuilder for error message</param>
        /// <param name="cellName">score column name</param>
        /// <param name="score">score value</param>
        /// <param name="gradingStyle">grading style value</param>
        /// <param name="recordRow">record row index</param>
        /// <param name="isPassingScore">checking passing score or final score</param>
        private static void CheckScoreByGradingStyle(StringBuilder checkCSVRecordMessage, string cellName, string score, string gradingStyle, int recordRow, bool isPassingScore)
        {
            if (string.IsNullOrEmpty(gradingStyle))
            {
                return;
            }

            string scoreLabel = string.Empty;

            if (isPassingScore)
            {
                scoreLabel = LabelUtil.GetTextByKey("csv_parse_error_passingscore_label", null);
            }
            else
            {
                scoreLabel = LabelUtil.GetTextByKey("csv_parse_error_finalscore_label", null);
            }

            string errorMsg = string.Empty;

            if (GradingStyle.Passfail.ToString().Equals(gradingStyle, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!ACAConstant.EXAM_SCORE_PASS.Equals(score, StringComparison.InvariantCultureIgnoreCase) 
                    && !ACAConstant.EXAM_SCORE_FAIL.Equals(score, StringComparison.InvariantCultureIgnoreCase))
                {
                    errorMsg = string.Format(RECORD_FORMAT_MESSAGE, scoreLabel + ACAConstant.BLANK + LabelUtil.GetTextByKey("csv_parse_error_passfail_range", null));
                }
            }
            else if (GradingStyle.Score.ToString().Equals(gradingStyle, StringComparison.InvariantCultureIgnoreCase))
            {
                errorMsg = CheckScoreNumber(score, scoreLabel);
            }
            else if (GradingStyle.None.ToString().Equals(gradingStyle, StringComparison.InvariantCultureIgnoreCase))
            {
                if (isPassingScore)
                {
                    //If GradingStyle is None, the "Passing Score" must be empty.
                    if (!string.IsNullOrEmpty(score))
                    {
                        errorMsg = string.Format(RECORD_FORMAT_MESSAGE, scoreLabel + ACAConstant.BLANK + LabelUtil.GetTextByKey("aca_csv_parse_error_passing_score_empty", null));
                    }
                }
                else
                {
                    errorMsg = CheckScoreNumber(score, scoreLabel);
                }
            }
            else if (GradingStyle.Percentage.ToString().Equals(gradingStyle, StringComparison.InvariantCultureIgnoreCase))
            {
                if (ValidationUtil.IsNumber(score))
                {
                    double? nullableScore = StringUtil.ToDouble(score);
                    double dScore = nullableScore.Value;

                    if (dScore < SCORE_RANGE_START || dScore > SCORE_RANGE_PERCENTAGE_END)
                    {
                        errorMsg = string.Format(scoreLabel + ACAConstant.BLANK + LabelUtil.GetTextByKey("csv_parse_error_score_range", null), SCORE_RANGE_START, SCORE_RANGE_PERCENTAGE_END);
                        errorMsg = string.Format(RECORD_FORMAT_MESSAGE, errorMsg);
                    }
                }
                else
                {
                    errorMsg = string.Format(RECORD_FORMAT_MESSAGE, scoreLabel + ACAConstant.BLANK + LabelUtil.GetTextByKey("csv_parse_error_score_number", null));
                }
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_ROW_MESSAGE, null), recordRow.ToString()));
                checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_TYPE_ERROR_MESSAGE, null), score, cellName + errorMsg));
                checkCSVRecordMessage.Append(ACAConstant.HTML_BR);
                errorMesssageIndex++;            
            }
        }

        /// <summary>
        /// Check the score number is validate
        /// </summary>
        /// <param name="score">the score number</param>
        /// <param name="scoreLabel">the score label</param>
        /// <returns>the error message for check score number.</returns>
        private static string CheckScoreNumber(string score, string scoreLabel)
        {
            string errorMsg = string.Empty;

            if (ValidationUtil.IsNumber(score))
            {
                double? nullableScore = StringUtil.ToDouble(score);
                double dScore = nullableScore.Value;

                if (dScore < SCORE_RANGE_START || dScore > SCORE_RANGE_END)
                {
                    errorMsg = string.Format(scoreLabel + ACAConstant.BLANK + LabelUtil.GetTextByKey("csv_parse_error_score_range", null), SCORE_RANGE_START, SCORE_RANGE_END);
                    errorMsg = string.Format(RECORD_FORMAT_MESSAGE, errorMsg);
                }
            }
            else
            {
                errorMsg = string.Format(RECORD_FORMAT_MESSAGE, scoreLabel + ACAConstant.BLANK + LabelUtil.GetTextByKey("csv_parse_error_score_number", null));
            }

            return errorMsg;
        }

        /// <summary>
        /// Check examination date whether it is validate
        /// </summary>
        /// <param name="checkCSVRecordMessage">CSV Record Message</param>
        /// <param name="examDate">Examination Date</param>
        /// <param name="cellFormat">Date Format</param>
        /// <param name="examDateHeader">Examination date header</param>
        /// <param name="recordRow">Record Row</param>
        private static void CheckExamDate(StringBuilder checkCSVRecordMessage, string examDate, string cellFormat, string examDateHeader, int recordRow)
        {
            try
            {   
                DateTime inputedExamDate = string.IsNullOrEmpty(cellFormat) ? DateTime.Parse(examDate, I18nCultureUtil.WebServiceCultureInfo) : DateTime.ParseExact(examDate, cellFormat, I18nCultureUtil.WebServiceCultureInfo);
                var timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
                DateTime currentAgencyDateTime = timeBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);
                if (currentAgencyDateTime < inputedExamDate)
                {                    
                    checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_DATE_INVALID_MESSAGE, null), recordRow, examDate)); 
                    checkCSVRecordMessage.Append(ACAConstant.HTML_BR);
                    errorMesssageIndex++;
                }
            }
            catch
            {
                cellFormat = string.IsNullOrEmpty(cellFormat) ? cellFormat : string.Format(RECORD_FORMAT_MESSAGE, cellFormat);
                checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_ROW_MESSAGE, null), recordRow));
                checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_TYPE_ERROR_MESSAGE, null), examDate, examDateHeader + cellFormat));
                checkCSVRecordMessage.Append(ACAConstant.HTML_BR);
                errorMesssageIndex++;
            }
        }

        /// <summary>
        /// Check the provider information 
        /// </summary>
        /// <param name="checkCSVRecordMessage">check CSV Record Message</param>
        /// <param name="providerName">provider Name</param>
        /// <param name="selectedProviderName">selected Provider Name</param>
        /// <param name="providerNumber">provider Number</param>
        /// <param name="selectProviderNumber">select Provider Number</param>
        /// <param name="recordRow">record Row</param>
        private static void CheckProviderInfo(StringBuilder checkCSVRecordMessage, string providerName, string selectedProviderName, string providerNumber, string selectProviderNumber, int recordRow)
        {
            if (!string.Equals(selectedProviderName.Trim(), providerName.Trim(), StringComparison.InvariantCultureIgnoreCase) || !string.Equals(selectProviderNumber.Trim(), providerNumber.Trim(), StringComparison.InvariantCultureIgnoreCase))
            {                
                checkCSVRecordMessage.Append(string.Format(LabelUtil.GetTextByKey(RECORD_PROVIDER_INVALID_MESSAGE, null), recordRow, selectedProviderName, selectProviderNumber));                
                checkCSVRecordMessage.Append(ACAConstant.HTML_BR);
                errorMesssageIndex++;
            }
        }

        #endregion
    }
}
