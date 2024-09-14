using System;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200042C RID: 1068
	public class River
	{
		// Token: 0x06002070 RID: 8304 RVA: 0x0007D5CC File Offset: 0x0007B7CC
		public string readString()
		{
			if (this.block != null)
			{
				return this.block.readString();
			}
			int num = (int)this.readByte();
			if (num > 0)
			{
				this.stream.Read(River.buffer, 0, num);
				return Encoding.UTF8.GetString(River.buffer, 0, num);
			}
			return string.Empty;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x0007D622 File Offset: 0x0007B822
		public bool readBoolean()
		{
			if (this.block != null)
			{
				return this.block.readBoolean();
			}
			return this.readByte() > 0;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x0007D641 File Offset: 0x0007B841
		public byte readByte()
		{
			if (this.block != null)
			{
				return this.block.readByte();
			}
			return MathfEx.ClampToByte(this.stream.ReadByte());
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x0007D668 File Offset: 0x0007B868
		public byte[] readBytes()
		{
			if (this.block != null)
			{
				return this.block.readByteArray();
			}
			byte[] array = new byte[(int)this.readUInt16()];
			this.stream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x0007D6A7 File Offset: 0x0007B8A7
		public short readInt16()
		{
			if (this.block != null)
			{
				return this.block.readInt16();
			}
			this.stream.Read(River.buffer, 0, 2);
			return BitConverter.ToInt16(River.buffer, 0);
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0007D6DB File Offset: 0x0007B8DB
		public ushort readUInt16()
		{
			if (this.block != null)
			{
				return this.block.readUInt16();
			}
			this.stream.Read(River.buffer, 0, 2);
			return BitConverter.ToUInt16(River.buffer, 0);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x0007D70F File Offset: 0x0007B90F
		public int readInt32()
		{
			if (this.block != null)
			{
				return this.block.readInt32();
			}
			this.stream.Read(River.buffer, 0, 4);
			return BitConverter.ToInt32(River.buffer, 0);
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x0007D743 File Offset: 0x0007B943
		public uint readUInt32()
		{
			if (this.block != null)
			{
				return this.block.readUInt32();
			}
			this.stream.Read(River.buffer, 0, 4);
			return BitConverter.ToUInt32(River.buffer, 0);
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x0007D777 File Offset: 0x0007B977
		public float readSingle()
		{
			if (this.block != null)
			{
				return this.block.readSingle();
			}
			this.stream.Read(River.buffer, 0, 4);
			return BitConverter.ToSingle(River.buffer, 0);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x0007D7AB File Offset: 0x0007B9AB
		public long readInt64()
		{
			if (this.block != null)
			{
				return this.block.readInt64();
			}
			this.stream.Read(River.buffer, 0, 8);
			return BitConverter.ToInt64(River.buffer, 0);
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x0007D7DF File Offset: 0x0007B9DF
		public ulong readUInt64()
		{
			if (this.block != null)
			{
				return this.block.readUInt64();
			}
			this.stream.Read(River.buffer, 0, 8);
			return BitConverter.ToUInt64(River.buffer, 0);
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0007D813 File Offset: 0x0007BA13
		public CSteamID readSteamID()
		{
			return new CSteamID(this.readUInt64());
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x0007D820 File Offset: 0x0007BA20
		public Guid readGUID()
		{
			if (this.block != null)
			{
				return this.block.readGUID();
			}
			GuidBuffer guidBuffer = default(GuidBuffer);
			guidBuffer.Read(this.readBytes(), 0);
			return guidBuffer.GUID;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x0007D85D File Offset: 0x0007BA5D
		public Vector3 readSingleVector3()
		{
			return new Vector3(this.readSingle(), this.readSingle(), this.readSingle());
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x0007D876 File Offset: 0x0007BA76
		public Quaternion readSingleQuaternion()
		{
			return Quaternion.Euler(this.readSingle(), this.readSingle(), this.readSingle());
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x0007D88F File Offset: 0x0007BA8F
		public Color readColor()
		{
			return new Color((float)this.readByte() / 255f, (float)this.readByte() / 255f, (float)this.readByte() / 255f);
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x0007D8C0 File Offset: 0x0007BAC0
		public void writeString(string value)
		{
			if (this.block != null)
			{
				this.block.writeString(value);
				return;
			}
			if (value == null)
			{
				value = string.Empty;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			byte b = MathfEx.ClampToByte(bytes.Length);
			this.stream.WriteByte(b);
			this.stream.Write(bytes, 0, (int)b);
			this.water += (int)(1 + b);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x0007D92B File Offset: 0x0007BB2B
		public void writeBoolean(bool value)
		{
			if (this.block != null)
			{
				this.block.writeBoolean(value);
				return;
			}
			this.stream.WriteByte(value ? 1 : 0);
			this.water++;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0007D962 File Offset: 0x0007BB62
		public void writeByte(byte value)
		{
			if (this.block != null)
			{
				this.block.writeByte(value);
				return;
			}
			this.stream.WriteByte(value);
			this.water++;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x0007D994 File Offset: 0x0007BB94
		public void writeBytes(byte[] values)
		{
			if (this.block != null)
			{
				this.block.writeByteArray(values);
				return;
			}
			ushort num = MathfEx.ClampToUShort(values.Length);
			this.writeUInt16(num);
			this.stream.Write(values, 0, (int)num);
			this.water += (int)num;
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x0007D9E4 File Offset: 0x0007BBE4
		public void writeInt16(short value)
		{
			if (this.block != null)
			{
				this.block.writeInt16(value);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(value);
			this.stream.Write(bytes, 0, 2);
			this.water += 2;
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x0007DA2C File Offset: 0x0007BC2C
		public void writeUInt16(ushort value)
		{
			if (this.block != null)
			{
				this.block.writeUInt16(value);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(value);
			this.stream.Write(bytes, 0, 2);
			this.water += 2;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x0007DA74 File Offset: 0x0007BC74
		public void writeInt32(int value)
		{
			if (this.block != null)
			{
				this.block.writeInt32(value);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(value);
			this.stream.Write(bytes, 0, 4);
			this.water += 4;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0007DABC File Offset: 0x0007BCBC
		public void writeUInt32(uint value)
		{
			if (this.block != null)
			{
				this.block.writeUInt32(value);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(value);
			this.stream.Write(bytes, 0, 4);
			this.water += 4;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x0007DB04 File Offset: 0x0007BD04
		public void writeSingle(float value)
		{
			if (this.block != null)
			{
				this.block.writeSingle(value);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(value);
			this.stream.Write(bytes, 0, 4);
			this.water += 4;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x0007DB4C File Offset: 0x0007BD4C
		public void writeInt64(long value)
		{
			if (this.block != null)
			{
				this.block.writeInt64(value);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(value);
			this.stream.Write(bytes, 0, 8);
			this.water += 8;
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x0007DB94 File Offset: 0x0007BD94
		public void writeUInt64(ulong value)
		{
			if (this.block != null)
			{
				this.block.writeUInt64(value);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(value);
			this.stream.Write(bytes, 0, 8);
			this.water += 8;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x0007DBD9 File Offset: 0x0007BDD9
		public void writeSteamID(CSteamID steamID)
		{
			this.writeUInt64(steamID.m_SteamID);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x0007DBE8 File Offset: 0x0007BDE8
		public void writeGUID(Guid GUID)
		{
			GuidBuffer guidBuffer = new GuidBuffer(GUID);
			guidBuffer.Write(GuidBuffer.GUID_BUFFER, 0);
			this.writeBytes(GuidBuffer.GUID_BUFFER);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x0007DC15 File Offset: 0x0007BE15
		public void writeSingleVector3(Vector3 value)
		{
			this.writeSingle(value.x);
			this.writeSingle(value.y);
			this.writeSingle(value.z);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0007DC3C File Offset: 0x0007BE3C
		public void writeSingleQuaternion(Quaternion value)
		{
			Vector3 eulerAngles = value.eulerAngles;
			this.writeSingle(eulerAngles.x);
			this.writeSingle(eulerAngles.y);
			this.writeSingle(eulerAngles.z);
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x0007DC75 File Offset: 0x0007BE75
		public void writeColor(Color value)
		{
			this.writeByte((byte)(value.r * 255f));
			this.writeByte((byte)(value.g * 255f));
			this.writeByte((byte)(value.b * 255f));
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x0007DCB0 File Offset: 0x0007BEB0
		public byte[] getHash()
		{
			this.stream.Position = 0L;
			return Hash.SHA1(this.stream);
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x0007DCCC File Offset: 0x0007BECC
		public void closeRiver()
		{
			if (this.block != null)
			{
				ReadWrite.writeBlock(this.path, true, this.block);
				return;
			}
			if (this.water > 0)
			{
				this.stream.SetLength((long)this.water);
			}
			this.stream.Flush();
			this.stream.Close();
			this.stream.Dispose();
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x0007DD30 File Offset: 0x0007BF30
		public River(string newPath)
		{
			this.path = ReadWrite.PATH + newPath;
			if (!Directory.Exists(Path.GetDirectoryName(this.path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this.path));
			}
			this.stream = new FileStream(this.path, 4, 3, 3);
			this.water = 0;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x0007DD94 File Offset: 0x0007BF94
		public River(string newPath, bool usePath)
		{
			this.path = newPath;
			if (usePath)
			{
				this.path = ReadWrite.PATH + this.path;
			}
			if (!Directory.Exists(Path.GetDirectoryName(this.path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this.path));
			}
			this.stream = new FileStream(this.path, 4, 3, 3);
			this.water = 0;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x0007DE08 File Offset: 0x0007C008
		public River(string newPath, bool usePath, bool useCloud, bool isReading)
		{
			this.path = newPath;
			if (useCloud)
			{
				if (isReading)
				{
					this.block = ReadWrite.readBlock(this.path, useCloud, 0);
				}
				if (this.block == null)
				{
					this.block = new Block();
					return;
				}
			}
			else
			{
				if (usePath)
				{
					this.path = ReadWrite.PATH + this.path;
				}
				if (!Directory.Exists(Path.GetDirectoryName(this.path)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(this.path));
				}
				this.stream = new FileStream(this.path, 4, 3, 3);
				this.water = 0;
			}
		}

		// Token: 0x04000FC8 RID: 4040
		private static byte[] buffer = new byte[Block.BUFFER_SIZE];

		// Token: 0x04000FC9 RID: 4041
		private int water;

		// Token: 0x04000FCA RID: 4042
		private string path;

		// Token: 0x04000FCB RID: 4043
		private Stream stream;

		// Token: 0x04000FCC RID: 4044
		private Block block;
	}
}
