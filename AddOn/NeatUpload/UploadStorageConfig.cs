/*

NeatUpload - an HttpModule and User Controls for uploading large files
Copyright (C) 2006  Dean Brettle

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
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;
using System.Web.UI;

namespace Brettle.Web.NeatUpload
{
	public class UploadStorageConfig : NameValueCollection
	{
		// Create a logger for use in this class
		/*
		private static readonly log4net.ILog log
		= log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		*/
		
		protected virtual void Deserialize(Stream s)
		{
			// Deserialize the storageConfig
			System.Web.UI.LosFormatter scFormatter = new System.Web.UI.LosFormatter();
			Hashtable ht = (Hashtable)scFormatter.Deserialize(s);
			
			// Convert to a NameValueCollection.  We only use Hashtable for serialization because
			// LosFormatter can serialize it efficiently.
			if (ht != null)
			{
				foreach (string key in ht.Keys)
				{
					this[key] = (string)ht[key];
				}
			}
		}
		
		protected virtual void Serialize(Stream s)
		{
			// Convert the StorageConfig to a Hashtable because LosFormatter can serialize Hashtables very
			// efficiently.
			Hashtable ht = new Hashtable();
			foreach (string key in Keys)
			{
				ht[key] = this[key];
			}
			LosFormatter scFormatter = new LosFormatter();
			scFormatter.Serialize(s, ht);			
		}
		
		internal void Unprotect(string secureStorageConfigString)
		{			
			byte[] secureStorageConfig = Convert.FromBase64String(secureStorageConfigString);
			MemoryStream secureStorageConfigStream = new MemoryStream(secureStorageConfig);
			BinaryReader binaryReader = new BinaryReader(secureStorageConfigStream);
			byte[] actualHash = binaryReader.ReadBytes(binaryReader.ReadByte());
			byte[] iv = binaryReader.ReadBytes(binaryReader.ReadByte());
			byte[] cipherText = binaryReader.ReadBytes((int)(secureStorageConfigStream.Length - secureStorageConfigStream.Position));
			
			// Verify the hash
			KeyedHashAlgorithm macAlgorithm = KeyedHashAlgorithm.Create();
			macAlgorithm.Key = Config.Current.ValidationKey;
			byte[] expectedHash = macAlgorithm.ComputeHash(cipherText);
			if (actualHash.Length != expectedHash.Length)
			{
				throw new InvalidStorageConfigException("actualHash.Length (" + actualHash.Length + ")" +
				                                        " != expectedHash.Length (" + expectedHash.Length + ")");
			}
			for (int i = 0; i < expectedHash.Length; i++)
			{
				if (actualHash[i] != expectedHash[i])
				{
					throw new InvalidStorageConfigException("actualHash[" + i + "] (" + (int)actualHash[i] + ")" +
					                                         " != expectedHash[" + i + "] (" + (int)expectedHash[i] + ")");
				}
			}
			
			// Decrypt the ciphertext
			MemoryStream cipherTextStream = new MemoryStream(cipherText);
			SymmetricAlgorithm cipher = SymmetricAlgorithm.Create();
			cipher.Mode = CipherMode.CBC;
			cipher.Padding = PaddingMode.PKCS7;
			cipher.Key = Config.Current.EncryptionKey;
			cipher.IV = iv;
			CryptoStream cryptoStream = new CryptoStream(cipherTextStream, cipher.CreateDecryptor(), CryptoStreamMode.Read);
			try
			{
				Deserialize(cryptoStream);
			}
			finally
			{
				cryptoStream.Close();
			}
		}
		
		internal string Protect()
		{
			// Encrypt it
			MemoryStream cipherTextStream = new MemoryStream();
			SymmetricAlgorithm cipher = SymmetricAlgorithm.Create();
			cipher.Mode = CipherMode.CBC;
			cipher.Padding = PaddingMode.PKCS7;
			cipher.Key = Config.Current.EncryptionKey;
			CryptoStream cryptoStream = new CryptoStream(cipherTextStream, cipher.CreateEncryptor(), CryptoStreamMode.Write);
			Serialize(cryptoStream);
			cryptoStream.Close();
			byte[] cipherText = cipherTextStream.ToArray();
			
			// MAC the ciphertext
			KeyedHashAlgorithm macAlgorithm = KeyedHashAlgorithm.Create();
			macAlgorithm.Key = Config.Current.ValidationKey;
			byte[] hash = macAlgorithm.ComputeHash(cipherText);
			
			// Concatenate MAC length, MAC, IV length, IV, and ciphertext into an array.
			MemoryStream secureStorageConfigStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(secureStorageConfigStream);
			binaryWriter.Write((byte)hash.Length);
			binaryWriter.Write(hash);
			binaryWriter.Write((byte)cipher.IV.Length);
			binaryWriter.Write(cipher.IV);
			binaryWriter.Write(cipherText);
			binaryWriter.Close();
			
			// return Base64-encoded value suitable for putting in a hidden form field
			return Convert.ToBase64String(secureStorageConfigStream.ToArray());
		}				
	}
}
