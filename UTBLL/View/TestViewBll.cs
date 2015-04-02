/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestViewBll.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestViewBll.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */
using Accela.ACA.Common;
using Accela.ACA.Test.BLL;
using Accela.ACA.WSProxy;
using System;
using NUnit.Framework;
using Accela.Test.Lib;

namespace Accela.ACA.BLL.View
{
    [TestFixture()]
    public class TestViewBll : TestBase
    {
        private IViewBll _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = (IViewBll)ObjectFactory.GetObject(typeof(IViewBll));
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetXUIGUITextList()
        {
            string agencyCode = "xxx";
            string callerID = "Admin";
            //XUITextModel4WS[] array = _unitUnderTest.GetXUIGUITextList(agencyCode, callerID);
            //foreach (XUITextModel4WS ws in array)
            //{
            //    //                string categoryName = ws.categoryName;
            //    //                string country = ws.country;
            //    //                string countryCode = ws.countryCode;
            //    //                string langCode = ws.langCode;

            //    long textLevelId = ws.textLevelId;
            //    string textLevelType = ws.textLevelType;
            //    string textLevelName = ws.textLevelName;

            //    string servProvCode = ws.servProvCode;
            //    string defaultValue = ws.defaultValue;
            //    string key = ws.stringKey;
            //    string value = ws.stringValue;
            //    Console.WriteLine("servProvCode>>" + servProvCode + "key>> " + key + " value>>" + value
            //        + " defaultValue>>" + defaultValue + " textLevelId>>" + textLevelId
            //        + " textLevelType>>" + textLevelType + " textLevelName>>" + textLevelName);
            //}
        }

        [Test()]
        public void TestUpdateXUIGUITextList()
        {
            return;
            String servProvCode = "xxx";
            string callerID = "Admin";
            XUITextModel[] array = new XUITextModel[2];
            XUITextModel model = new XUITextModel();
            model.servProvCode = "xxx";
            model.textLevelType = "MODULE";
            model.textLevelName = "Building";
            model.stringKey = "per_addressResult_label_Notice";
            model.stringValue = "test";
            model.langCode = "en";
            model.countryCode = "US";
            array[0] = model;
            XUITextModel model1 = new XUITextModel();
            model1.servProvCode = "xxx";
            model1.textLevelType = "APPTYPE";
            model1.textLevelName = "Building\\test\\test\\test";
            model1.stringKey = "per_address_Label_show";
            model1.stringValue = "test1";
            model1.langCode = "en";
            model1.countryCode = "US";
            array[1] = model1;

             //_unitUnderTest.UpdateXUITextList(servProvCode, callerID, array);
        }
    }
}