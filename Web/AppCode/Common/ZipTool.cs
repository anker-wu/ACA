#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Disclaimer.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ZipTool.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.IO.Compression;

using ICSharpCode.SharpZipLib.Zip;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// This class provide the compress/decompress utility.
    /// </summary>
    public class ZipTool
    {
        #region Methods

        /// <summary>
        /// Compress file.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destinationFile">The destination file.</param>
        public static void CompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException();
            }

            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buffer = new byte[sourceStream.Length];
                int checkCounter = sourceStream.Read(buffer, 0, buffer.Length);
                if (checkCounter != buffer.Length)
                {
                    throw new ApplicationException();
                }

                using (FileStream destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (GZipStream compressedStream = new GZipStream(destinationStream, CompressionMode.Compress, true))
                    {
                        compressedStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Decompress file.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destinationFile">The destination file.</param>
        public static void DecompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException();
            }

            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open))
            {
                byte[] quartetBuffer = new byte[4];
                int position = (int)sourceStream.Length - 4;
                sourceStream.Position = position;
                sourceStream.Read(quartetBuffer, 0, 4);
                sourceStream.Position = 0;
                int checkLength = BitConverter.ToInt32(quartetBuffer, 0);
                byte[] buffer = new byte[checkLength + 100];
                using (GZipStream decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true))
                {
                    int total = 0;
                    for (int offset = 0;;)
                    {
                        int bytesRead = decompressedStream.Read(buffer, offset, 100);
                        if (bytesRead == 0)
                        {
                            break;
                        }

                        offset += bytesRead;
                        total += bytesRead;
                    }

                    using (FileStream destinationStream = new FileStream(destinationFile, FileMode.Create))
                    {
                        destinationStream.Write(buffer, 0, total);
                        destinationStream.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Unzip for plan review. Zip allow to contain only one file.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destinationFile">The destination file.</param>
        /// <returns>Path of the unzipped file</returns>
        public static string UnZip(string sourceFile, string destinationFile)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(sourceFile));

            ZipEntry theEntry;
            string path = string.Empty;
            int index = 0;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                //If zip contains more than one file, return an empty string as path.
                if (index > 0)
                {
                    path = string.Empty;
                    break;
                }

                string fileName = Path.GetFileName(theEntry.Name);

                if (fileName != string.Empty)
                {
                    path = destinationFile + "\\" + theEntry.Name;
                    FileStream streamWriter = File.Create(path);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }

                index++;
            }

            s.Close();

            return path;
        }

        #endregion Methods
    }
}