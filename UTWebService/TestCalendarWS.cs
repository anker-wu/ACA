/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestCalendarWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 *
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{

    [TestFixture()]
    public class TestCalendarWS : TestBase
    {
        private CalendarWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<CalendarWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestgetEventSeriesByCalendarIDAndDateRange()
        {
            string serviceProviderCode = AgencyCode;
            string calendarID = "90";
            string startDateStr = "10/11/2010 00:00:00";
            string endDateStr = "01/31/2011 00:00:00";

            CalendarEventModel[] calendarEventArray = _unitUnderTest.getEventSeriesByCalendarIDAndDateRange(serviceProviderCode, calendarID, startDateStr, endDateStr, CallerID);

            if (calendarEventArray == null)
            {
                return;
            }

            foreach (CalendarEventModel calendarEvent in calendarEventArray)
            { 
                Console.WriteLine("Event ID : " + calendarEvent.eventID);                    
                Console.WriteLine("=================================================");
            }
        }

        [Test()]
        public void TestgetACACalendarByInspType()
        {
            string serviceProviderCode = AgencyCode;
            string inspSeqNbr = "84003061";

            CalendarModel calendar = _unitUnderTest.getACACalendarByInspType(serviceProviderCode, inspSeqNbr);

            if (calendar == null)
            {
                return;
            }

            Console.WriteLine("Calendar ID : " + calendar.calendarID);
            Console.WriteLine("Calendar Name : " + calendar.calendarName);
            Console.WriteLine("Calendar Priority : " + calendar.calendarPriority);
            Console.WriteLine("Calendar Type :  " + calendar.calendarType);                      
        }
    }
}
