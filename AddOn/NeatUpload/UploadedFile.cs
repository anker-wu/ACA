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
using System.IO;
using System.Web;
using System.Configuration;

namespace Brettle.Web.NeatUpload
{
	public abstract class UploadedFile
	{
		private UploadedFile() {}

		protected UploadedFile(string controlUniqueID, string fileName, string contentType)
		{
			// IE sends a full path for the fileName.  We only want the actual filename.
			if (System.Text.RegularExpressions.Regex.IsMatch(fileName, @"^(\\\\[^\\]|([a-zA-Z]:)?\\).*"))
			{
				fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
			}
			FileName = fileName;
			ContentType = contentType;
			ControlUniqueID = controlUniqueID;
		}
		
		public abstract void Dispose();

		public abstract bool IsUploaded	{ get; }

        /// <summary>
        /// Inicates whether the uploated file is deleted immediately by certain reason. eg, deleted by virus software.
        /// </summary>
        public abstract bool IsDeletedByVirusDetect { get; }

		public abstract Stream CreateStream();

		public abstract void MoveTo(string path, MoveToOptions opts);

		public abstract long ContentLength { get; }

		public abstract Stream OpenRead();
		
		public FileInfo TmpFile = null;
		
		public string FileName;
		
		public string ContentType;

		public string ControlUniqueID;
	}
}
