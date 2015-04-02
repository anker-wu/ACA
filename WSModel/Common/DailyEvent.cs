/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DailyEvent.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 * 
 *  Description:
 *  represent all event of one day in calendar
 * 
 *  Notes:
 *
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;

namespace Accela.ACA.WSProxy.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DailyEvent
    {
        //private int activeEvents;
        private ArrayList calendarEvents;
        private ArrayList activeCalendarEvents;

        //public int ActiveEvents
        //{
        //    get
        //    {
        //        return activeEvents;
        //    }

        //    set
        //    {
        //        activeEvents = value;
        //    }
        //}

        public ArrayList CalendarEvents
        {
            get
            {
                return calendarEvents;
            }

            set
            {
                calendarEvents = value;
            }
        }

        public ArrayList ActiveCalendarEvents
        {
            get
            {
                return activeCalendarEvents;
            }

            set
            {
                activeCalendarEvents = value;
            }
        }
    }
}
