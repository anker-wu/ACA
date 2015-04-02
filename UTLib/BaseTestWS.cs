/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: BaseTestWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: BaseTestWS.cs 177463 2010-07-20 06:43:03Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

using NUnit.Framework;
using System.Diagnostics;

namespace Accela.Test.Lib
{
    [TestFixture()]
    public class TestBase
    {
        private static string _agencyCode = null;
        private static string _callerID = null;

        public string AgencyCode
        {
            get
            {    
                if (_agencyCode == null)
                {
                    _agencyCode = ConfigurationSettings.AppSettings["ServProvCode"];
                }
                return _agencyCode;
            }
        }

        public string CallerID
        {
            get
            {
                if (_callerID == null)
                { 
                    _callerID = "Unit Tester";
                }
                return _callerID;
            }
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
            HttpContextMock.Init();

            //Setup I18n Settings
           //StandardChoiceUtil.SetupI18nInitialSettings();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            HttpContextMock.Context.Session.Abandon();
        }

        public void Serialize(int caseNum, object obj, string paramName)
        {
           

            string className = this.GetType().Name;

            StackTrace st = new StackTrace(true);
            string methodName = st.GetFrame(1).GetMethod().Name.ToString();

            string directory = String.Format("{0}\\{1}\\{2}\\Case_{3}", DirectoryUtil.CasesRoot, className, methodName, caseNum);
            string fileName = directory + "\\" + paramName + ".xml";

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            SerializationUtil.XmlSerializeToFile(obj, fileName);
        }

        public object Deserialize( int caseNum, Type type,string paramName)
        {
            string className = this.GetType().Name;

            StackTrace st = new StackTrace(true);
            string methodName = st.GetFrame(1).GetMethod().Name.ToString();
            string directory = String.Format("{0}\\{1}\\{2}\\Case_{3}", DirectoryUtil.CasesRoot, className, methodName, caseNum);
            string fileName = directory + "\\" + paramName + ".xml";

            object obj = SerializationUtil.XmlDeserializeFromFile(type, fileName);
            return obj;
        }
    }
}
