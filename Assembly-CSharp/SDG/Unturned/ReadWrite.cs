using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x0200042B RID: 1067
	public class ReadWrite
	{
		// Token: 0x06002032 RID: 8242 RVA: 0x0007CB50 File Offset: 0x0007AD50
		public static byte[] readData()
		{
			FileStream fileStream = new FileStream(UnityPaths.GameDataDirectory.FullName + "/Managed/Assembly-CSharp.dll", 3, 1, 1);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			fileStream.Dispose();
			return Hash.SHA1(array);
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0007CBA3 File Offset: 0x0007ADA3
		public static T deserializeJSON<T>(string path, bool useCloud)
		{
			return ReadWrite.deserializeJSON<T>(path, useCloud, true);
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x0007CBB0 File Offset: 0x0007ADB0
		public static T deserializeJSON<T>(string path, bool useCloud, bool usePath)
		{
			T result = default(T);
			byte[] array = ReadWrite.readBytes(path, useCloud, usePath);
			if (array == null)
			{
				return result;
			}
			string @string = Encoding.UTF8.GetString(array);
			if (@string == null)
			{
				return result;
			}
			return JsonConvert.DeserializeObject<T>(@string);
		}

		/// <summary>
		/// Deserialize JSON onto an existing object instance.
		/// </summary>
		// Token: 0x06002035 RID: 8245 RVA: 0x0007CBEC File Offset: 0x0007ADEC
		public static void populateJSON(string path, object target, bool usePath = true)
		{
			byte[] array = ReadWrite.readBytes(path, false, usePath);
			if (array == null)
			{
				return;
			}
			string @string = Encoding.UTF8.GetString(array);
			if (@string == null)
			{
				return;
			}
			JsonConvert.PopulateObject(@string, target);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0007CC20 File Offset: 0x0007AE20
		public static byte[] cloudFileRead(string path)
		{
			if (!ReadWrite.cloudFileExists(path))
			{
				return null;
			}
			int num;
			Provider.provider.cloudService.getSize(path, out num);
			byte[] array = new byte[num];
			if (!Provider.provider.cloudService.read(path, array))
			{
				UnturnedLog.error("Failed to read the correct file size.");
				return null;
			}
			return array;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x0007CC71 File Offset: 0x0007AE71
		public static void cloudFileWrite(string path, byte[] bytes, int size)
		{
			if (!Provider.provider.cloudService.write(path, bytes, size))
			{
				UnturnedLog.error("Failed to write file.");
			}
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0007CC91 File Offset: 0x0007AE91
		public static void cloudFileDelete(string path)
		{
			Provider.provider.cloudService.delete(path);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0007CCA4 File Offset: 0x0007AEA4
		public static bool cloudFileExists(string path)
		{
			if (ReadWrite.disableSteamCloudRead)
			{
				return false;
			}
			bool result;
			Provider.provider.cloudService.exists(path, out result);
			return result;
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0007CCD3 File Offset: 0x0007AED3
		public static void serializeJSON<T>(string path, bool useCloud, T instance)
		{
			ReadWrite.serializeJSON<T>(path, useCloud, true, instance);
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0007CCE0 File Offset: 0x0007AEE0
		public static void serializeJSON<T>(string path, bool useCloud, bool usePath, T instance)
		{
			string text = JsonConvert.SerializeObject(instance, Formatting.Indented);
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			ReadWrite.writeBytes(path, useCloud, usePath, bytes, bytes.Length);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x0007CD12 File Offset: 0x0007AF12
		public static T deserializeXML<T>(string path, bool useCloud)
		{
			return ReadWrite.deserializeXML<T>(path, useCloud, true);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x0007CD1C File Offset: 0x0007AF1C
		public static T deserializeXML<T>(string path, bool useCloud, bool usePath)
		{
			T result = default(T);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			if (useCloud)
			{
				MemoryStream memoryStream = new MemoryStream(ReadWrite.cloudFileRead(path));
				try
				{
					result = (T)((object)xmlSerializer.Deserialize(memoryStream));
				}
				finally
				{
					memoryStream.Close();
					memoryStream.Dispose();
				}
				return result;
			}
			if (usePath)
			{
				path += ReadWrite.PATH;
			}
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			if (!File.Exists(path))
			{
				UnturnedLog.info("Failed to find file at: " + path);
				return result;
			}
			StreamReader streamReader = new StreamReader(path);
			try
			{
				result = (T)((object)xmlSerializer.Deserialize(streamReader));
			}
			finally
			{
				streamReader.Close();
				streamReader.Dispose();
			}
			return result;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x0007CDF4 File Offset: 0x0007AFF4
		public static void serializeXML<T>(string path, bool useCloud, T instance)
		{
			ReadWrite.serializeXML<T>(path, useCloud, true, instance);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0007CE00 File Offset: 0x0007B000
		public static void serializeXML<T>(string path, bool useCloud, bool usePath, T instance)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			if (useCloud)
			{
				MemoryStream memoryStream = new MemoryStream();
				XmlWriter xmlWriter = XmlWriter.Create(memoryStream, ReadWrite.XML_WRITER_SETTINGS);
				try
				{
					xmlSerializer.Serialize(xmlWriter, instance, ReadWrite.XML_SERIALIZER_NAMESPACES);
					ReadWrite.cloudFileWrite(path, memoryStream.GetBuffer(), (int)memoryStream.Length);
					return;
				}
				finally
				{
					xmlWriter.Close();
					memoryStream.Close();
					memoryStream.Dispose();
				}
			}
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			StreamWriter streamWriter = new StreamWriter(path);
			try
			{
				xmlSerializer.Serialize(streamWriter, instance, ReadWrite.XML_SERIALIZER_NAMESPACES);
			}
			finally
			{
				streamWriter.Close();
				streamWriter.Dispose();
			}
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0007CEDC File Offset: 0x0007B0DC
		public static byte[] readBytes(string path, bool useCloud)
		{
			return ReadWrite.readBytes(path, useCloud, true);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0007CEE8 File Offset: 0x0007B0E8
		public static byte[] readBytes(string path, bool useCloud, bool usePath)
		{
			if (useCloud)
			{
				return ReadWrite.cloudFileRead(path);
			}
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			if (!File.Exists(path))
			{
				UnturnedLog.info("Failed to find file at: " + path);
				return null;
			}
			FileStream fileStream = new FileStream(path, 3, 1, 1);
			byte[] array = new byte[fileStream.Length];
			if (fileStream.Read(array, 0, array.Length) != array.Length)
			{
				UnturnedLog.error("Failed to read the correct file size.");
				return null;
			}
			fileStream.Close();
			fileStream.Dispose();
			return array;
		}

		/// <summary>
		/// Introduced much later (2020) than most of the other methods in this class (2014) in order to properly handle
		/// BOM/preamble of text files. Matches somewhat undesirable legacy behavior like creating directories.
		/// </summary>
		// Token: 0x06002042 RID: 8258 RVA: 0x0007CF84 File Offset: 0x0007B184
		private static string readString(string filePath, bool useCloud, bool prependPath)
		{
			if (useCloud)
			{
				byte[] array = ReadWrite.readBytes(filePath, useCloud, prependPath);
				if (array == null)
				{
					return null;
				}
				return Encoding.UTF8.GetString(array);
			}
			else
			{
				if (prependPath)
				{
					filePath = ReadWrite.PATH + filePath;
				}
				string directoryName = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				if (!File.Exists(filePath))
				{
					UnturnedLog.info("Failed to find file at: " + filePath);
					return null;
				}
				return File.ReadAllText(filePath);
			}
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x0007CFF4 File Offset: 0x0007B1F4
		public static Data readData(string path, bool useCloud)
		{
			return ReadWrite.readData(path, useCloud, true);
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x0007D000 File Offset: 0x0007B200
		public static Data readData(string path, bool useCloud, bool usePath)
		{
			string text = ReadWrite.readString(path, useCloud, usePath);
			if (text == null)
			{
				return null;
			}
			if (text.Length == 0)
			{
				return new Data();
			}
			return new Data(text);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0007D030 File Offset: 0x0007B230
		internal static DatDictionary ReadDataWithoutHash(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			DatDictionary result;
			using (FileStream fileStream = new FileStream(path, 3, 1, 1))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					result = ReadWrite.datParser.Parse(streamReader);
				}
			}
			return result;
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x0007D098 File Offset: 0x0007B298
		public static Block readBlock(string path, bool useCloud, byte prefix)
		{
			return ReadWrite.readBlock(path, useCloud, true, prefix);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x0007D0A4 File Offset: 0x0007B2A4
		public static Block readBlock(string path, bool useCloud, bool usePath, byte prefix)
		{
			byte[] array = ReadWrite.readBytes(path, useCloud, usePath);
			if (array == null)
			{
				return null;
			}
			return new Block((int)prefix, array);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0007D0C6 File Offset: 0x0007B2C6
		public static void writeBytes(string path, bool useCloud, byte[] bytes)
		{
			ReadWrite.writeBytes(path, useCloud, true, bytes, bytes.Length);
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x0007D0D4 File Offset: 0x0007B2D4
		public static void writeBytes(string path, bool useCloud, byte[] bytes, int size)
		{
			ReadWrite.writeBytes(path, useCloud, true, bytes, size);
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x0007D0E0 File Offset: 0x0007B2E0
		public static void writeBytes(string path, bool useCloud, bool usePath, byte[] bytes)
		{
			ReadWrite.writeBytes(path, useCloud, usePath, bytes, bytes.Length);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0007D0F0 File Offset: 0x0007B2F0
		public static void writeBytes(string path, bool useCloud, bool usePath, byte[] bytes, int size)
		{
			if (useCloud)
			{
				ReadWrite.cloudFileWrite(path, bytes, size);
				return;
			}
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			FileStream fileStream = new FileStream(path, 4);
			fileStream.Write(bytes, 0, size);
			fileStream.SetLength((long)size);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x0007D15E File Offset: 0x0007B35E
		public static void writeData(string path, bool useCloud, Data data)
		{
			ReadWrite.writeData(path, useCloud, true, data);
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x0007D16C File Offset: 0x0007B36C
		public static void writeData(string path, bool useCloud, bool usePath, Data data)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(data.getFile());
			ReadWrite.writeBytes(path, useCloud, usePath, bytes);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x0007D193 File Offset: 0x0007B393
		public static void writeBlock(string path, bool useCloud, Block block)
		{
			ReadWrite.writeBlock(path, useCloud, true, block);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x0007D1A0 File Offset: 0x0007B3A0
		public static void writeBlock(string path, bool useCloud, bool usePath, Block block)
		{
			int size;
			byte[] bytes = block.getBytes(out size);
			ReadWrite.writeBytes(path, useCloud, usePath, bytes, size);
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x0007D1C0 File Offset: 0x0007B3C0
		public static void deleteFile(string path, bool useCloud)
		{
			ReadWrite.deleteFile(path, useCloud, true);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x0007D1CA File Offset: 0x0007B3CA
		public static void deleteFile(string path, bool useCloud, bool usePath)
		{
			if (useCloud)
			{
				ReadWrite.cloudFileDelete(path);
				return;
			}
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			File.Delete(path);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x0007D1EC File Offset: 0x0007B3EC
		public static void deleteFolder(string path)
		{
			ReadWrite.deleteFolder(path, true);
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x0007D1F5 File Offset: 0x0007B3F5
		public static void deleteFolder(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			Directory.Delete(path, true);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x0007D20E File Offset: 0x0007B40E
		public static void moveFolder(string origin, string target)
		{
			ReadWrite.moveFolder(origin, target, true);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x0007D218 File Offset: 0x0007B418
		public static void moveFolder(string origin, string target, bool usePath)
		{
			if (usePath)
			{
				origin = ReadWrite.PATH + origin;
				target = ReadWrite.PATH + target;
			}
			Directory.Move(origin, target);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x0007D23E File Offset: 0x0007B43E
		public static void createFolder(string path)
		{
			ReadWrite.createFolder(path, true);
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x0007D247 File Offset: 0x0007B447
		public static void createFolder(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x0007D268 File Offset: 0x0007B468
		public static void createHidden(string path)
		{
			ReadWrite.createHidden(path, true);
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x0007D271 File Offset: 0x0007B471
		public static void createHidden(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path).Attributes = 18;
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x0007D298 File Offset: 0x0007B498
		public static string folderName(string path)
		{
			return new DirectoryInfo(path).Name;
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x0007D2A5 File Offset: 0x0007B4A5
		public static string folderPath(string path)
		{
			return Path.GetDirectoryName(path);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0007D2AD File Offset: 0x0007B4AD
		public static void renameFile(string path_0, string path_1)
		{
			path_0 = ReadWrite.PATH + path_0;
			path_1 = ReadWrite.PATH + path_1;
			File.Move(path_0, path_1);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x0007D2D0 File Offset: 0x0007B4D0
		public static string fileName(string path)
		{
			return Path.GetFileNameWithoutExtension(path);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x0007D2D8 File Offset: 0x0007B4D8
		public static bool fileExists(string path, bool useCloud)
		{
			return ReadWrite.fileExists(path, useCloud, true);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x0007D2E2 File Offset: 0x0007B4E2
		public static bool fileExists(string path, bool useCloud, bool usePath)
		{
			if (useCloud)
			{
				return ReadWrite.cloudFileExists(path);
			}
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			return File.Exists(path);
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x0007D304 File Offset: 0x0007B504
		public static string folderFound(string path)
		{
			return ReadWrite.folderFound(path, true);
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x0007D310 File Offset: 0x0007B510
		public static string folderFound(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			string[] directories = Directory.GetDirectories(path);
			if (directories.Length != 0)
			{
				return directories[0];
			}
			return null;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0007D33D File Offset: 0x0007B53D
		public static bool folderExists(string path)
		{
			return ReadWrite.folderExists(path, true);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0007D346 File Offset: 0x0007B546
		public static bool folderExists(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			return Directory.Exists(path);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0007D360 File Offset: 0x0007B560
		public static bool hasDirectoryWritePermission(string path)
		{
			bool result;
			try
			{
				ReadOnlyCollectionBase accessRules = Directory.GetAccessControl(path).GetAccessRules(true, true, typeof(SecurityIdentifier));
				bool flag = false;
				foreach (object obj in accessRules)
				{
					FileSystemAccessRule fileSystemAccessRule = (FileSystemAccessRule)obj;
					if ((fileSystemAccessRule.FileSystemRights & 278) == 278)
					{
						AccessControlType accessControlType = fileSystemAccessRule.AccessControlType;
						if (accessControlType != null)
						{
							if (accessControlType == 1)
							{
								return false;
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				result = flag;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x0007D410 File Offset: 0x0007B610
		public static string[] getFolders(string path)
		{
			return ReadWrite.getFolders(path, true);
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x0007D419 File Offset: 0x0007B619
		public static string[] getFolders(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			return Directory.GetDirectories(path);
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x0007D431 File Offset: 0x0007B631
		public static string[] getFiles(string path)
		{
			return ReadWrite.getFiles(path, true);
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x0007D43A File Offset: 0x0007B63A
		public static string[] getFiles(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			return Directory.GetFiles(path);
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x0007D452 File Offset: 0x0007B652
		public static void copyFile(string source, string destination)
		{
			source = ReadWrite.PATH + source;
			destination = ReadWrite.PATH + destination;
			if (!Directory.Exists(Path.GetDirectoryName(destination)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(destination));
			}
			File.Copy(source, destination);
		}

		/// <summary>
		/// Read GUI texture from a .jpg or .png file.
		/// </summary>
		// Token: 0x0600206A RID: 8298 RVA: 0x0007D48E File Offset: 0x0007B68E
		public static Texture2D readTextureFromFile(string path, bool useBasePath, EReadTextureFromFileMode mode = EReadTextureFromFileMode.UI)
		{
			if (useBasePath)
			{
				path = ReadWrite.PATH + path;
			}
			return ReadWrite.readTextureFromFile(path, EReadTextureFromFileMode.UI);
		}

		/// <summary>
		/// Read GUI texture from a .jpg or .png file.
		/// </summary>
		// Token: 0x0600206B RID: 8299 RVA: 0x0007D4A8 File Offset: 0x0007B6A8
		public static Texture2D readTextureFromFile(string absolutePath, EReadTextureFromFileMode mode = EReadTextureFromFileMode.UI)
		{
			byte[] array = File.ReadAllBytes(absolutePath);
			bool mipChain = false;
			bool linear = false;
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain, linear);
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			bool flag = true;
			ImageConversion.LoadImage(texture2D, array, flag);
			return texture2D;
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x0007D4DD File Offset: 0x0007B6DD
		public static bool SupportsOpeningFileBrowser
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0007D4E0 File Offset: 0x0007B6E0
		public static void OpenFileBrowser(string folderPath)
		{
			try
			{
				folderPath = Path.GetFullPath(folderPath);
				Process.Start("explorer.exe", "\"" + folderPath + "\"");
				UnturnedLog.info("Opened Windows Explorer at path: \"" + folderPath + "\"");
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception opening Windows Explorer at path: \"" + folderPath + "\"");
			}
		}

		// Token: 0x04000FC3 RID: 4035
		public static readonly string PATH = UnturnedPaths.RootDirectory.FullName;

		/// <summary>
		/// Potentially useful for players with corrupted cloud storage.
		/// https://github.com/SmartlyDressedGames/Unturned-3.x-Community/issues/2756
		/// </summary>
		// Token: 0x04000FC4 RID: 4036
		private static CommandLineFlag disableSteamCloudRead = new CommandLineFlag(false, "-DisableSteamCloudRead");

		// Token: 0x04000FC5 RID: 4037
		private static readonly XmlSerializerNamespaces XML_SERIALIZER_NAMESPACES = new XmlSerializerNamespaces(new XmlQualifiedName[]
		{
			XmlQualifiedName.Empty
		});

		// Token: 0x04000FC6 RID: 4038
		private static readonly XmlWriterSettings XML_WRITER_SETTINGS = new XmlWriterSettings
		{
			Indent = true,
			OmitXmlDeclaration = true,
			Encoding = new UTF8Encoding()
		};

		// Token: 0x04000FC7 RID: 4039
		private static DatParser datParser = new DatParser();
	}
}
