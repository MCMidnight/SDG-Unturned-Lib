using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000424 RID: 1060
	public class Data
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x0007ACAD File Offset: 0x00078EAD
		public byte[] hash
		{
			get
			{
				return this._hash;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x0007ACB5 File Offset: 0x00078EB5
		public bool isEmpty
		{
			get
			{
				return this.data.Count == 0;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x0007ACC5 File Offset: 0x00078EC5
		// (set) Token: 0x06001FCB RID: 8139 RVA: 0x0007ACCD File Offset: 0x00078ECD
		public List<string> errors { get; protected set; }

		// Token: 0x06001FCC RID: 8140 RVA: 0x0007ACD6 File Offset: 0x00078ED6
		public bool TryReadString(string key, out string value)
		{
			return this.data.TryGetValue(key, ref value);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0007ACE8 File Offset: 0x00078EE8
		public string readString(string key, string defaultValue = null)
		{
			string result;
			if (!this.data.TryGetValue(key, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0007AD08 File Offset: 0x00078F08
		public T readEnum<T>(string key, T defaultValue = default(T)) where T : struct
		{
			string text;
			if (this.data.TryGetValue(key, ref text))
			{
				try
				{
					return (T)((object)Enum.Parse(typeof(T), text, true));
				}
				catch
				{
					return defaultValue;
				}
				return defaultValue;
			}
			return defaultValue;
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0007AD58 File Offset: 0x00078F58
		public bool readBoolean(string key, bool defaultValue = false)
		{
			string text;
			if (this.data.TryGetValue(key, ref text))
			{
				return text.Equals("y", 3) || text == "1" || text.Equals("true", 3);
			}
			return defaultValue;
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0007ADA0 File Offset: 0x00078FA0
		public byte readByte(string key, byte defaultValue = 0)
		{
			byte result;
			if (!byte.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0007ADCC File Offset: 0x00078FCC
		public sbyte readSByte(string key, sbyte defaultValue = 0)
		{
			sbyte result;
			if (!sbyte.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0007ADF8 File Offset: 0x00078FF8
		public byte[] readByteArray(string key)
		{
			string text = this.readString(key, null);
			return Encoding.UTF8.GetBytes(text);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0007AE1C File Offset: 0x0007901C
		public short readInt16(string key, short defaultValue = 0)
		{
			short result;
			if (!short.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0007AE48 File Offset: 0x00079048
		public ushort readUInt16(string key, ushort defaultValue = 0)
		{
			ushort result;
			if (!ushort.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0007AE74 File Offset: 0x00079074
		public int readInt32(string key, int defaultValue = 0)
		{
			int result;
			if (!int.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0007AEA0 File Offset: 0x000790A0
		public uint readUInt32(string key, uint defaultValue = 0U)
		{
			uint result;
			if (!uint.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0007AECC File Offset: 0x000790CC
		public long readInt64(string key, long defaultValue = 0L)
		{
			long result;
			if (!long.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0007AEF8 File Offset: 0x000790F8
		public ulong readUInt64(string key, ulong defaultValue = 0UL)
		{
			ulong result;
			if (!ulong.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0007AF24 File Offset: 0x00079124
		public float readSingle(string key, float defaultValue = 0f)
		{
			float result;
			if (!float.TryParse(this.readString(key, null), 511, CultureInfo.InvariantCulture, ref result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x0007AF50 File Offset: 0x00079150
		public Vector3 readVector3(string key)
		{
			return new Vector3(this.readSingle(key + "_X", 0f), this.readSingle(key + "_Y", 0f), this.readSingle(key + "_Z", 0f));
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0007AFA4 File Offset: 0x000791A4
		public Quaternion readQuaternion(string key)
		{
			return Quaternion.Euler((float)(this.readByte(key + "_X", 0) * 2), (float)this.readByte(key + "_Y", 0), (float)this.readByte(key + "_Z", 0));
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0007AFF1 File Offset: 0x000791F1
		public Color readColor(string key)
		{
			return this.readColor(key, Color.black);
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0007B000 File Offset: 0x00079200
		public Color readColor(string key, Color defaultColor)
		{
			return new Color(this.readSingle(key + "_R", defaultColor.r), this.readSingle(key + "_G", defaultColor.g), this.readSingle(key + "_B", defaultColor.b));
		}

		/// <summary>
		/// Read 8-bit per channel color excluding alpha.
		/// </summary>
		// Token: 0x06001FDE RID: 8158 RVA: 0x0007B058 File Offset: 0x00079258
		public Color32 ReadColor32RGB(string key, Color32 defaultValue)
		{
			return new Color32(this.readByte(key + "_R", defaultValue.r), this.readByte(key + "_G", defaultValue.g), this.readByte(key + "_B", defaultValue.b), byte.MaxValue);
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0007B0B4 File Offset: 0x000792B4
		public CSteamID readSteamID(string key)
		{
			return new CSteamID(this.readUInt64(key, 0UL));
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x0007B0C4 File Offset: 0x000792C4
		public Guid readGUID(string key)
		{
			string text = this.readString(key, null);
			if (string.IsNullOrEmpty(text) || (text.Length == 1 && text.get_Chars(0) == '0'))
			{
				return Guid.Empty;
			}
			return new Guid(text);
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0007B104 File Offset: 0x00079304
		public void ReadGuidOrLegacyId(string key, out Guid guid, out ushort legacyId)
		{
			string text;
			if (this.data.TryGetValue(key, ref text) && !string.IsNullOrEmpty(text) && (text.Length != 1 || text.get_Chars(0) != '0'))
			{
				if (ushort.TryParse(text, 511, CultureInfo.InvariantCulture, ref legacyId))
				{
					guid = Guid.Empty;
					return;
				}
				if (Guid.TryParse(text, ref guid))
				{
					legacyId = 0;
					return;
				}
			}
			guid = Guid.Empty;
			legacyId = 0;
		}

		/// <summary>
		/// Intended as a drop-in replacement for existing assets with property uint16s.
		/// </summary>
		// Token: 0x06001FE2 RID: 8162 RVA: 0x0007B178 File Offset: 0x00079378
		public ushort ReadGuidOrLegacyId(string key, out Guid guid)
		{
			ushort result;
			this.ReadGuidOrLegacyId(key, out guid, out result);
			return result;
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0007B190 File Offset: 0x00079390
		public AssetReference<T> readAssetReference<T>(string key) where T : Asset
		{
			if (this.has(key))
			{
				return new AssetReference<T>(this.readGUID(key));
			}
			return AssetReference<T>.invalid;
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0007B1AD File Offset: 0x000793AD
		public AssetReference<T> readAssetReference<T>(string key, in AssetReference<T> defaultValue) where T : Asset
		{
			if (this.has(key))
			{
				return new AssetReference<T>(this.readGUID(key));
			}
			return defaultValue;
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0007B1CC File Offset: 0x000793CC
		private void ParseMasterBundleReference(string key, string value, out string name, out string path)
		{
			int num = value.IndexOf(':');
			if (num < 0)
			{
				if (Assets.currentMasterBundle != null)
				{
					name = Assets.currentMasterBundle.assetBundleName;
				}
				else
				{
					name = string.Empty;
					this.AddError("MasterBundleRef \"" + key + "\" is not associated with a master bundle nor does it specify one");
				}
				path = value;
				return;
			}
			name = value.Substring(0, num);
			path = value.Substring(num + 1);
			if (string.IsNullOrEmpty(name))
			{
				this.AddError("MasterBundleRef \"" + key + "\" specified asset bundle name is empty");
			}
			if (string.IsNullOrEmpty(path))
			{
				this.AddError("MasterBundleRef \"" + key + "\" specified asset path is empty");
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x0007B274 File Offset: 0x00079474
		public MasterBundleReference<T> readMasterBundleReference<T>(string key) where T : Object
		{
			string value;
			if (this.TryReadString(key, out value))
			{
				string name;
				string path;
				this.ParseMasterBundleReference(key, value, out name, out path);
				return new MasterBundleReference<T>(name, path);
			}
			return MasterBundleReference<T>.invalid;
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x0007B2A8 File Offset: 0x000794A8
		public AudioReference ReadAudioReference(string key)
		{
			string value;
			if (this.TryReadString(key, out value))
			{
				string assetBundleName;
				string path;
				this.ParseMasterBundleReference(key, value, out assetBundleName, out path);
				return new AudioReference(assetBundleName, path);
			}
			return default(AudioReference);
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0007B2DD File Offset: 0x000794DD
		public void writeString(string key, string value)
		{
			this.data.Add(key, value);
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0007B2EC File Offset: 0x000794EC
		public void writeBoolean(string key, bool value)
		{
			this.data.Add(key, value ? "y" : "n");
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x0007B309 File Offset: 0x00079509
		public void writeByte(string key, byte value)
		{
			this.data.Add(key, value.ToString());
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x0007B31E File Offset: 0x0007951E
		public void writeByteArray(string key, byte[] value)
		{
			this.data.Add(key, Encoding.UTF8.GetString(value));
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x0007B337 File Offset: 0x00079537
		public void writeInt16(string key, short value)
		{
			this.data.Add(key, value.ToString());
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x0007B34C File Offset: 0x0007954C
		public void writeUInt16(string key, ushort value)
		{
			this.data.Add(key, value.ToString());
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x0007B361 File Offset: 0x00079561
		public void writeInt32(string key, int value)
		{
			this.data.Add(key, value.ToString());
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x0007B376 File Offset: 0x00079576
		public void writeUInt32(string key, uint value)
		{
			this.data.Add(key, value.ToString());
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0007B38B File Offset: 0x0007958B
		public void writeInt64(string key, long value)
		{
			this.data.Add(key, value.ToString());
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0007B3A0 File Offset: 0x000795A0
		public void writeUInt64(string key, ulong value)
		{
			this.data.Add(key, value.ToString());
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0007B3B8 File Offset: 0x000795B8
		public void writeSingle(string key, float value)
		{
			this.data.Add(key, (Mathf.Floor(value * 100f) / 100f).ToString());
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x0007B3EC File Offset: 0x000795EC
		public void writeVector3(string key, Vector3 value)
		{
			this.writeSingle(key + "_X", value.x);
			this.writeSingle(key + "_Y", value.y);
			this.writeSingle(key + "_Z", value.z);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0007B440 File Offset: 0x00079640
		public void writeQuaternion(string key, Quaternion value)
		{
			Vector3 eulerAngles = value.eulerAngles;
			this.writeByte(key + "_X", MeasurementTool.angleToByte(eulerAngles.x));
			this.writeByte(key + "_Y", MeasurementTool.angleToByte(eulerAngles.y));
			this.writeByte(key + "_Z", MeasurementTool.angleToByte(eulerAngles.z));
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0007B4AC File Offset: 0x000796AC
		public void writeColor(string key, Color value)
		{
			this.writeSingle(key + "_R", value.r);
			this.writeSingle(key + "_G", value.g);
			this.writeSingle(key + "_B", value.b);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0007B4FE File Offset: 0x000796FE
		public void writeSteamID(string key, CSteamID value)
		{
			this.writeUInt64(key, value.m_SteamID);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0007B50D File Offset: 0x0007970D
		public void writeGUID(string key, Guid value)
		{
			this.writeString(key, value.ToString("N"));
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x0007B524 File Offset: 0x00079724
		public string getFile()
		{
			string text = "";
			char c = this.isCSV ? ',' : ' ';
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key,
					c.ToString(),
					keyValuePair.Value,
					"\n"
				});
			}
			return text;
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x0007B5BC File Offset: 0x000797BC
		public string[] getLines()
		{
			string[] array = new string[this.data.Count];
			char c = this.isCSV ? ',' : ' ';
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				array[num] = keyValuePair.Key + c.ToString() + keyValuePair.Value;
				num++;
			}
			return array;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x0007B650 File Offset: 0x00079850
		public KeyValuePair<string, string>[] getContents()
		{
			KeyValuePair<string, string>[] array = new KeyValuePair<string, string>[this.data.Count];
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				array[num] = keyValuePair;
				num++;
			}
			return array;
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x0007B6BC File Offset: 0x000798BC
		public string[] getValuesWithKey(string key)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				if (keyValuePair.Key == key)
				{
					list.Add(keyValuePair.Value);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0007B730 File Offset: 0x00079930
		public string[] getKeysWithValue(string value)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				if (keyValuePair.Value == value)
				{
					list.Add(keyValuePair.Key);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0007B7A4 File Offset: 0x000799A4
		public bool has(string key)
		{
			return this.data.ContainsKey(key);
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x0007B7B2 File Offset: 0x000799B2
		public void reset()
		{
			this.data.Clear();
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0007B7C0 File Offset: 0x000799C0
		public void log()
		{
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				UnturnedLog.info("{0} = {1}", new object[]
				{
					keyValuePair.Key,
					keyValuePair.Value
				});
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0007B830 File Offset: 0x00079A30
		internal Data(StreamReader streamReader, SHA1Stream hashStream)
		{
			this.data = new Dictionary<string, string>();
			string line = string.Empty;
			while ((line = streamReader.ReadLine()) != null)
			{
				this.ParseLine(line);
			}
			this._hash = ((hashStream != null) ? hashStream.Hash : null);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0007B87C File Offset: 0x00079A7C
		public Data(string content)
		{
			this.data = new Dictionary<string, string>();
			StringReader stringReader = null;
			try
			{
				stringReader = new StringReader(content);
				string line = string.Empty;
				while ((line = stringReader.ReadLine()) != null)
				{
					this.ParseLine(line);
				}
				this._hash = Hash.SHA1(content);
			}
			catch (Exception innerException)
			{
				do
				{
					this.AddError("Caught exception: \"" + innerException.Message + "\"\n" + innerException.StackTrace);
					innerException = innerException.InnerException;
				}
				while (innerException != null);
				this.data.Clear();
				this._hash = null;
			}
			finally
			{
				if (stringReader != null)
				{
					stringReader.Close();
				}
			}
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0007B930 File Offset: 0x00079B30
		public Data()
		{
			this.data = new Dictionary<string, string>();
			this._hash = null;
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0007B94A File Offset: 0x00079B4A
		private void AddError(string message)
		{
			if (this.errors == null)
			{
				this.errors = new List<string>();
			}
			this.errors.Add(message);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0007B96C File Offset: 0x00079B6C
		private void ParseLine(string line)
		{
			if (line.Length < 1 || (line.Length > 1 && line.get_Chars(0) == '/' && line.get_Chars(1) == '/'))
			{
				return;
			}
			int num = line.IndexOf(' ');
			string text;
			string text2;
			if (num != -1)
			{
				text = line.Substring(0, num);
				text2 = line.Substring(num + 1, line.Length - num - 1);
			}
			else
			{
				text = line;
				text2 = string.Empty;
			}
			string text3;
			if (this.data.TryGetValue(text, ref text3))
			{
				this.AddError(string.Concat(new string[]
				{
					"Duplicate key: \"",
					text,
					"\" Old value: ",
					text3,
					" New value: ",
					text2
				}));
				this.data[text] = text2;
				return;
			}
			this.data.Add(text, text2);
		}

		// Token: 0x04000FAF RID: 4015
		private Dictionary<string, string> data;

		// Token: 0x04000FB0 RID: 4016
		private byte[] _hash;

		// Token: 0x04000FB1 RID: 4017
		public bool isCSV;
	}
}
