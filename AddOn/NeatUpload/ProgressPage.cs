#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProgressPage.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description: 
 *
 *  Notes:
 *      $Id: ProgressPage.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

/*
NeatUpload - an HttpModule and User Controls for uploading large files
Copyright (C) 2005  Dean Brettle

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Brettle.Web.NeatUpload;

namespace Brettle.Web.NeatUpload
{
	public class ProgressPage : Page
	{
/*		
		// Create a logger for use in this class
		private static readonly log4net.ILog log 
			= log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		*/

        private const int BYTE_UNIT = 1024;

		protected override void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
		}
		
		protected long BytesRead;
		protected long BytesTotal;
		protected double FractionComplete;
		protected int BytesPerSec;
		protected UploadException Rejection;
		protected Exception Failure;
		protected TimeSpan TimeRemaining;
		protected TimeSpan TimeElapsed;
		protected string CurrentFileName;
		
		protected UploadStatus Status = UploadStatus.Unknown;
		
		protected bool CancelVisible;
		protected bool StartRefreshVisible;
		protected bool StopRefreshVisible;
		protected string CancelUrl;
		protected string StartRefreshUrl;
		protected string StopRefreshUrl;

        protected bool IsNormalStatus; // add back-james.shi
		
		protected virtual string GetResourceString(string resourceName)
		{
			return Config.Current.ResourceManager.GetString(resourceName);
		}
		
		protected string FormatCount(long count)
		{
			lock(this)
			{
                if (UnitSelector < BYTE_UNIT)
                {
                    return count.ToString();
                }
                if (UnitSelector < BYTE_UNIT * BYTE_UNIT)
                {
                    return string.Format("{0:0.00}", (double)count / BYTE_UNIT);
                }
                return string.Format("{0:0.00}", (double)count / BYTE_UNIT / BYTE_UNIT);
			}
		}
		
		private long UnitSelector
		{
			get { return (BytesTotal < 0) ? BytesRead : BytesTotal; }
		}
				
		protected string CountUnits
		{
			get
			{
				if (UnitSelector < BYTE_UNIT)
					return GetResourceString("ByteUnits");
				else if (UnitSelector < BYTE_UNIT*BYTE_UNIT)
					return GetResourceString("KBUnits");
				else
					return GetResourceString("MBUnits");
			}
		}
		
		protected string FormatRate(int rate)
		{
			string format;
			if (rate < BYTE_UNIT)
				format = GetResourceString("ByteRateFormat");
			else if (rate < BYTE_UNIT*BYTE_UNIT)
				format = GetResourceString("KBRateFormat");
			else
				format = GetResourceString("MBRateFormat");
            return FormatCount(rate) + format;
		}					
				
		protected string FormatTimeSpan(TimeSpan ts)
		{
            if (ts.TotalSeconds < 60)
                return "00:" + GetString(ts.Seconds);
            else if (ts.TotalSeconds < 60 * 60)
            {
                return GetString(ts.Minutes) + ":" + GetString(ts.Seconds);
            }
            else
                return GetString(ts.Hours) + ":" + GetString(ts.Minutes) + ":" + GetString(ts.Seconds);
		}

        private string GetString(int value)
        {
            return value < 10 ? ("0" + value) : value.ToString();
        }
		
		private UploadContext UploadContext;					
		private UploadStatus CurrentStatus = UploadStatus.Unknown;
		private bool CanScript;
		private bool CanCancel;
		private bool IsRefreshing;
		private string RefreshUrl;
		
		protected override void OnLoad(EventArgs e)
		{
            //RegisterClientScriptBlock("NeatUpload-ProgressPage", "<script type='text/javascript' language='javascript' src='../Scripts/FileUploadProgress.js'></script>");
            // remove back --james.shi
			SetupContext();
			SetupBindableProps();
			
			// Set the status to Cancelled if requested.
			if (this.UploadContext != null && Request.Params["cancelled"] == "true")
			{
				this.UploadContext.Status = UploadStatus.Cancelled;
			}
			
			if (Request.Params["useXml"] == "true")
			{
				Response.ContentType = "text/xml";
				Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
				Response.Write(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
			}
			
			string prevStatus = Request.Params["prevStatus"];
			if (prevStatus == null)
			{
				prevStatus = UploadStatus.Unknown.ToString();
			}
			string curStatus = Status.ToString();

			// If the status is unchanged from the last refresh and it is not Unknown nor *InProgress,
			// then the page is not refreshed.  Instead, if an UploadException occurred we try to cancel
			// the upload.  Otherwise, no exception occurred we close the progress display window (if it's a pop-up)  
			if (curStatus == prevStatus 
			       && (curStatus != UploadStatus.Unknown.ToString()
			           && curStatus != UploadStatus.NormalInProgress.ToString()
			           && curStatus != UploadStatus.ChunkedInProgress.ToString()))
			{
				if (curStatus == UploadStatus.Rejected.ToString())
				{
					if (CanCancel)
					{
                        RegisterScript4UploadCancel();
					}
				}
				else if (curStatus != UploadStatus.Failed.ToString())
				{
					RegisterStartupScript("scrNeatUploadClose", @"<script type='text/javascript' language='javascript'>
<!--
window.close();
// -->
</script>");
				}
			}
			// Otherwise, if we are refreshing, we refresh the page in one second
			else if (IsRefreshing)
			{
				// And add a prevStatus param to the url to track the previous status. 			
				string refreshUrl = RefreshUrl + "&prevStatus=" + curStatus;
				Refresh(refreshUrl);
			}
            this.IsNormalStatus = this.Status == UploadStatus.NormalInProgress; // add back -james.shi
			base.OnLoad(e);			
		}
		
		private void SetupContext()
		{
			// Find the current upload context
			string postBackID = Request.Params["postBackID"];
			this.UploadContext = UploadContext.FindByID(postBackID);

			if (this.UploadContext == null || this.UploadContext.Status == UploadStatus.Unknown)
			{
				CurrentStatus = UploadStatus.Unknown;
				// Status is unknown, so try to find the last post back based on the lastPostBackID param.
				// If successful, use the status of the last post back.
				string lastPostBackID = Page.Request.Params["lastPostBackID"];
				if (lastPostBackID != null && lastPostBackID.Length > 0 && Page.Request.Params["refresher"] == null)
				{
					this.UploadContext = UploadContext.FindByID(lastPostBackID);
					if (this.UploadContext != null && this.UploadContext.FileBytesRead == 0)
					{
						this.UploadContext = null;
					}
				}
			}
			else
			{
				CurrentStatus = this.UploadContext.Status;
			}

            // Cancel the upload immediately if the status is rejected. 
            // Fix the chrome browser issue, if not cancel immediately, chrome sometime can upload and throw exception. 
            if (CurrentStatus == UploadStatus.Rejected)
            {
                RegisterScript4UploadCancel();
            }
		}
		
		private void SetupBindableProps()
		{
			if (this.UploadContext != null)
			{
				lock (this.UploadContext)
				{
					FractionComplete = this.UploadContext.FractionComplete;
                    if (FractionComplete == 1)
                    {
                        FractionComplete = 0;
                    }
					BytesRead = this.UploadContext.BytesRead;
					BytesTotal = this.UploadContext.ContentLength;
					BytesPerSec = this.UploadContext.BytesPerSec;
					if (this.UploadContext.Exception is UploadException)
					{
						Rejection = (UploadException)this.UploadContext.Exception;
					}
					else
					{
						Failure = this.UploadContext.Exception;
					}
					TimeRemaining = this.UploadContext.TimeRemaining;
					TimeElapsed = this.UploadContext.TimeElapsed;
					CurrentFileName = this.UploadContext.CurrentFileName;
					Status = this.UploadContext.Status;
				}
			}
			CanScript = (Request.Params["canScript"] != null && Boolean.Parse(Request.Params["canScript"]));
			CanCancel = (Request.Params["canCancel"] != null && Boolean.Parse(Request.Params["canCancel"]));
			IsRefreshing = (Request.Params["refresh"] != "false" && Request.Params["refresher"] != null);
			StartRefreshVisible = (!CanScript && !IsRefreshing
		                          && (CurrentStatus == UploadStatus.Unknown
                                      || CurrentStatus == UploadStatus.NormalInProgress
                                      || CurrentStatus == UploadStatus.ChunkedInProgress));
			StopRefreshVisible = (!CanScript && IsRefreshing
		                          && (CurrentStatus == UploadStatus.Unknown
                                      || CurrentStatus == UploadStatus.NormalInProgress
                                      || CurrentStatus == UploadStatus.ChunkedInProgress));
			CancelVisible = (CanCancel
		                    && (CurrentStatus == UploadStatus.NormalInProgress || CurrentStatus == UploadStatus.ChunkedInProgress));
			
			// The base refresh url contains just the postBackID (which is the first parameter)
			RefreshUrl = Request.Url.PathAndQuery;
			int ampIndex = RefreshUrl.IndexOf("&");
			if (ampIndex != -1)
			{
				RefreshUrl = RefreshUrl.Substring(0, ampIndex);
			}
			RefreshUrl += "&canScript=" + CanScript + "&canCancel=" + CanCancel;

            // Anti XSS Security Issue here.
            RefreshUrl = AntiXSSUtil.AntiXssUrlEncode(RefreshUrl);
			
            StartRefreshUrl = RefreshUrl + "&refresher=server";	
			StopRefreshUrl = RefreshUrl + "&refresh=false";
			CancelUrl = "javascript:NeatUpload_CancelClicked()";
			
			DataBind();			
		}
		
		private void Refresh(string refreshUrl)
		{
			if (Request.Params["refresher"] == "client")
			{
				RefreshWithClientScript(refreshUrl);
			}
			else
			{
				RefreshWithServerHeader(refreshUrl);
			}  
		}
		
		private void RefreshWithClientScript(string refreshUrl)
		{
			refreshUrl += "&refresher=client";
			RegisterStartupScript("scrNeatUploadRefresh", @"<script type='text/javascript' language='javascript'>
<!--
NeatUploadRefreshUrl = '" + refreshUrl + @"';
window.onload = NeatUpload_CombineHandlers(window.onload, function () 
{
	NeatUploadReloadTimeoutId = setTimeout('NeatUploadRefresh()', 1000);
});
// -->
</script>");
		}
		
		private void RefreshWithServerHeader(string refreshUrl)
		{
			refreshUrl += "&refresher=server";
			Response.AddHeader("Refresh", "1; URL=" + refreshUrl);
		}

        /// <summary>
        /// Register the javascript for cancel upload.
        /// </summary>
        private void RegisterScript4UploadCancel()
        {
            string key = "scrNeatUploadError";
            string script = @"
                                <script type=""text/javascript"" language=""javascript"">
                                <!--
                                window.onload = function() {
                                    NeatUploadCancel();
                                }
                                // -->
                                </script>";

            if (!ClientScript.IsStartupScriptRegistered(key))
            {
                ClientScript.RegisterStartupScript(Page.GetType(), key, script);
            }
        }
	}
}
