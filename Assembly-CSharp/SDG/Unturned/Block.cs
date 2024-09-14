using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000420 RID: 1056
	public class Block
	{
		// Token: 0x06001F5E RID: 8030 RVA: 0x00079568 File Offset: 0x00077768
		private static object[] getObjects(int index)
		{
			object[] array = Block.objects[index];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = null;
			}
			return array;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x00079590 File Offset: 0x00077790
		public string readString()
		{
			if (this.block != null && this.step < this.block.Length)
			{
				byte b = this.block[this.step];
				this.step++;
				string result;
				if (this.step + (int)b <= this.block.Length)
				{
					result = Encoding.UTF8.GetString(this.block, this.step, (int)b);
				}
				else
				{
					result = string.Empty;
				}
				this.step += (int)b;
				return result;
			}
			return string.Empty;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x00079618 File Offset: 0x00077818
		public string[] readStringArray()
		{
			if (this.block != null && this.step < this.block.Length)
			{
				string[] array = new string[(int)this.readByte()];
				byte b = 0;
				while ((int)b < array.Length)
				{
					array[(int)b] = this.readString();
					b += 1;
				}
				return array;
			}
			return new string[0];
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x00079669 File Offset: 0x00077869
		public bool readBoolean()
		{
			if (this.block != null && this.step <= this.block.Length - 1)
			{
				bool result = BitConverter.ToBoolean(this.block, this.step);
				this.step++;
				return result;
			}
			return false;
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000796A8 File Offset: 0x000778A8
		public bool[] readBooleanArray()
		{
			if (this.block != null && this.step < this.block.Length)
			{
				bool[] array = new bool[(int)this.readUInt16()];
				ushort num = (ushort)Mathf.CeilToInt((float)array.Length / 8f);
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					byte b = 0;
					while (b < 8 && (int)(num2 * 8 + (ushort)b) < array.Length)
					{
						array[(int)(num2 * 8 + (ushort)b)] = ((this.block[this.step + (int)num2] & Types.SHIFTS[(int)b]) == Types.SHIFTS[(int)b]);
						b += 1;
					}
				}
				this.step += (int)num;
				return array;
			}
			return new bool[0];
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0007974E File Offset: 0x0007794E
		public byte readByte()
		{
			if (this.block != null && this.step <= this.block.Length - 1)
			{
				byte result = this.block[this.step];
				this.step++;
				return result;
			}
			return 0;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x00079788 File Offset: 0x00077988
		public byte[] readByteArray()
		{
			if (this.block != null && this.step < this.block.Length)
			{
				byte[] array;
				if (this.longBinaryData)
				{
					int num = this.readInt32();
					if (num >= 30000)
					{
						return new byte[0];
					}
					array = new byte[num];
				}
				else
				{
					array = new byte[(int)this.block[this.step]];
					this.step++;
				}
				if (this.step + array.Length <= this.block.Length)
				{
					try
					{
						Buffer.BlockCopy(this.block, this.step, array, 0, array.Length);
					}
					catch
					{
					}
				}
				this.step += array.Length;
				return array;
			}
			return new byte[0];
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x00079854 File Offset: 0x00077A54
		public short readInt16()
		{
			if (this.block != null && this.step <= this.block.Length - 2)
			{
				this.readBitConverterBytes(2);
				short result = BitConverter.ToInt16(this.block, this.step);
				this.step += 2;
				return result;
			}
			return 0;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000798A4 File Offset: 0x00077AA4
		public ushort readUInt16()
		{
			if (this.block != null && this.step <= this.block.Length - 2)
			{
				this.readBitConverterBytes(2);
				ushort result = BitConverter.ToUInt16(this.block, this.step);
				this.step += 2;
				return result;
			}
			return 0;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000798F4 File Offset: 0x00077AF4
		public int readInt32()
		{
			if (this.block != null && this.step <= this.block.Length - 4)
			{
				this.readBitConverterBytes(4);
				int result = BitConverter.ToInt32(this.block, this.step);
				this.step += 4;
				return result;
			}
			return 0;
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x00079944 File Offset: 0x00077B44
		public int[] readInt32Array()
		{
			ushort num = this.readUInt16();
			int[] array = new int[(int)num];
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				int num3 = this.readInt32();
				array[(int)num2] = num3;
			}
			return array;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00079978 File Offset: 0x00077B78
		public uint readUInt32()
		{
			if (this.block != null && this.step <= this.block.Length - 4)
			{
				this.readBitConverterBytes(4);
				uint result = BitConverter.ToUInt32(this.block, this.step);
				this.step += 4;
				return result;
			}
			return 0U;
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000799C8 File Offset: 0x00077BC8
		public float readSingle()
		{
			if (this.block != null && this.step <= this.block.Length - 4)
			{
				this.readBitConverterBytes(4);
				float result = BitConverter.ToSingle(this.block, this.step);
				this.step += 4;
				return result;
			}
			return 0f;
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x00079A1C File Offset: 0x00077C1C
		public long readInt64()
		{
			if (this.block != null && this.step <= this.block.Length - 8)
			{
				this.readBitConverterBytes(8);
				long result = BitConverter.ToInt64(this.block, this.step);
				this.step += 8;
				return result;
			}
			return 0L;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x00079A6C File Offset: 0x00077C6C
		public ulong readUInt64()
		{
			if (this.block != null && this.step <= this.block.Length - 8)
			{
				this.readBitConverterBytes(8);
				ulong result = BitConverter.ToUInt64(this.block, this.step);
				this.step += 8;
				return result;
			}
			return 0UL;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x00079ABC File Offset: 0x00077CBC
		public ulong[] readUInt64Array()
		{
			ushort num = this.readUInt16();
			ulong[] array = new ulong[(int)num];
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				ulong num3 = this.readUInt64();
				array[(int)num2] = num3;
			}
			return array;
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x00079AF0 File Offset: 0x00077CF0
		public CSteamID readSteamID()
		{
			return new CSteamID(this.readUInt64());
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x00079B00 File Offset: 0x00077D00
		public Guid readGUID()
		{
			GuidBuffer guidBuffer = default(GuidBuffer);
			guidBuffer.Read(this.readByteArray(), 0);
			return guidBuffer.GUID;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x00079B2C File Offset: 0x00077D2C
		public Vector3 readUInt16RVector3()
		{
			double num = (double)this.readByte();
			double num2 = (double)this.readUInt16() / 65535.0;
			double num3 = (double)this.readUInt16() / 65535.0;
			byte b = this.readByte();
			double num4 = (double)this.readUInt16() / 65535.0;
			num2 = num * (double)Regions.REGION_SIZE + num2 * (double)Regions.REGION_SIZE - 4096.0;
			num3 = num3 * 2048.0 - 1024.0;
			num4 = (double)(b * Regions.REGION_SIZE) + num4 * (double)Regions.REGION_SIZE - 4096.0;
			return new Vector3((float)num2, (float)num3, (float)num4);
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x00079BD4 File Offset: 0x00077DD4
		public Vector3 readSingleVector3()
		{
			return new Vector3(this.readSingle(), this.readSingle(), this.readSingle());
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x00079BED File Offset: 0x00077DED
		public Quaternion readSingleQuaternion()
		{
			return Quaternion.Euler(this.readSingle(), this.readSingle(), this.readSingle());
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x00079C06 File Offset: 0x00077E06
		public Color readColor()
		{
			return new Color((float)this.readByte() / 255f, (float)this.readByte() / 255f, (float)this.readByte() / 255f);
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x00079C34 File Offset: 0x00077E34
		public object read(Type type)
		{
			if (type == Types.STRING_TYPE)
			{
				return this.readString();
			}
			if (type == Types.STRING_ARRAY_TYPE)
			{
				return this.readStringArray();
			}
			if (type == Types.BOOLEAN_TYPE)
			{
				return this.readBoolean();
			}
			if (type == Types.BOOLEAN_ARRAY_TYPE)
			{
				return this.readBooleanArray();
			}
			if (type == Types.BYTE_TYPE)
			{
				return this.readByte();
			}
			if (type == Types.BYTE_ARRAY_TYPE)
			{
				return this.readByteArray();
			}
			if (type == Types.INT16_TYPE)
			{
				return this.readInt16();
			}
			if (type == Types.UINT16_TYPE)
			{
				return this.readUInt16();
			}
			if (type == Types.INT32_TYPE)
			{
				return this.readInt32();
			}
			if (type == Types.INT32_ARRAY_TYPE)
			{
				return this.readInt32Array();
			}
			if (type == Types.UINT32_TYPE)
			{
				return this.readUInt32();
			}
			if (type == Types.SINGLE_TYPE)
			{
				return this.readSingle();
			}
			if (type == Types.INT64_TYPE)
			{
				return this.readInt64();
			}
			if (type == Types.UINT64_TYPE)
			{
				return this.readUInt64();
			}
			if (type == Types.UINT64_ARRAY_TYPE)
			{
				return this.readUInt64Array();
			}
			if (type == Types.STEAM_ID_TYPE)
			{
				return this.readSteamID();
			}
			if (type == Types.GUID_TYPE)
			{
				return this.readGUID();
			}
			if (type == Types.VECTOR3_TYPE)
			{
				return this.readSingleVector3();
			}
			if (type == Types.COLOR_TYPE)
			{
				return this.readColor();
			}
			throw new NotSupportedException(string.Format("Cannot read type {0}", type));
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00079E10 File Offset: 0x00078010
		public object[] read(int offset, Type type_0)
		{
			object[] array = Block.getObjects(0);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			return array;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x00079E34 File Offset: 0x00078034
		public object[] read(int offset, Type type_0, Type type_1)
		{
			object[] array = Block.getObjects(1);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			return array;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x00079E65 File Offset: 0x00078065
		public object[] read(Type type_0, Type type_1)
		{
			return this.read(0, type_0, type_1);
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x00079E70 File Offset: 0x00078070
		public object[] read(int offset, Type type_0, Type type_1, Type type_2)
		{
			object[] array = Block.getObjects(2);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			return array;
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00079EB0 File Offset: 0x000780B0
		public object[] read(Type type_0, Type type_1, Type type_2)
		{
			return this.read(0, type_0, type_1, type_2);
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00079EBC File Offset: 0x000780BC
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3)
		{
			object[] array = Block.getObjects(3);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			return array;
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00079F0B File Offset: 0x0007810B
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
		{
			return this.read(0, type_0, type_1, type_2, type_3);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x00079F1C File Offset: 0x0007811C
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			object[] array = Block.getObjects(4);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			if (offset < 5)
			{
				array[4] = this.read(type_4);
			}
			return array;
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x00079F7A File Offset: 0x0007817A
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x00079F8C File Offset: 0x0007818C
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			object[] array = Block.getObjects(5);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			if (offset < 5)
			{
				array[4] = this.read(type_4);
			}
			if (offset < 6)
			{
				array[5] = this.read(type_5);
			}
			return array;
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x00079FF9 File Offset: 0x000781F9
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0007A00C File Offset: 0x0007820C
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			object[] array = Block.getObjects(6);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			if (offset < 5)
			{
				array[4] = this.read(type_4);
			}
			if (offset < 6)
			{
				array[5] = this.read(type_5);
			}
			if (offset < 7)
			{
				array[6] = this.read(type_6);
			}
			return array;
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0007A088 File Offset: 0x00078288
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0007A0A8 File Offset: 0x000782A8
		public object[] read(int offset, params Type[] types)
		{
			object[] array = new object[types.Length];
			for (int i = offset; i < types.Length; i++)
			{
				array[i] = this.read(types[i]);
			}
			return array;
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0007A0D9 File Offset: 0x000782D9
		public object[] read(params Type[] types)
		{
			return this.read(0, types);
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0007A0E3 File Offset: 0x000782E3
		protected void readBitConverterBytes(int length)
		{
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0007A0E8 File Offset: 0x000782E8
		protected void writeBitConverterBytes(byte[] bytes)
		{
			int num = this.step;
			int num2 = bytes.Length;
			Buffer.BlockCopy(bytes, 0, Block.buffer, num, num2);
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0007A110 File Offset: 0x00078310
		public void writeString(string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			byte b = (byte)bytes.Length;
			Block.buffer[this.step] = b;
			this.step++;
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, (int)b);
			this.step += (int)b;
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0007A174 File Offset: 0x00078374
		public void writeStringArray(string[] values)
		{
			byte b = (byte)values.Length;
			this.writeByte(b);
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				this.writeString(values[(int)b2]);
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x0007A1A4 File Offset: 0x000783A4
		public void writeBoolean(bool value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Block.buffer[this.step] = bytes[0];
			this.step++;
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x0007A1D8 File Offset: 0x000783D8
		public void writeBooleanArray(bool[] values)
		{
			this.writeUInt16((ushort)values.Length);
			ushort num = (ushort)Mathf.CeilToInt((float)values.Length / 8f);
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				Block.buffer[this.step + (int)num2] = 0;
				byte b = 0;
				while (b < 8 && (int)(num2 * 8 + (ushort)b) < values.Length)
				{
					if (values[(int)(num2 * 8 + (ushort)b)])
					{
						byte[] array = Block.buffer;
						int num3 = this.step + (int)num2;
						array[num3] |= Types.SHIFTS[(int)b];
					}
					b += 1;
				}
			}
			this.step += (int)num;
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0007A268 File Offset: 0x00078468
		public void writeByte(byte value)
		{
			Block.buffer[this.step] = value;
			this.step++;
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0007A288 File Offset: 0x00078488
		public void writeByteArray(byte[] values)
		{
			if (values.Length >= 30000)
			{
				return;
			}
			if (this.longBinaryData)
			{
				this.writeInt32(values.Length);
				Buffer.BlockCopy(values, 0, Block.buffer, this.step, values.Length);
				this.step += values.Length;
				return;
			}
			byte b = (byte)values.Length;
			Block.buffer[this.step] = b;
			this.step++;
			Buffer.BlockCopy(values, 0, Block.buffer, this.step, (int)b);
			this.step += (int)b;
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0007A318 File Offset: 0x00078518
		public void writeInt16(short value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.writeBitConverterBytes(bytes);
			this.step += 2;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0007A344 File Offset: 0x00078544
		public void writeUInt16(ushort value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.writeBitConverterBytes(bytes);
			this.step += 2;
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0007A370 File Offset: 0x00078570
		public void writeInt32(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.writeBitConverterBytes(bytes);
			this.step += 4;
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0007A39C File Offset: 0x0007859C
		public void writeInt32Array(int[] values)
		{
			this.writeUInt16((ushort)values.Length);
			ushort num = 0;
			while ((int)num < values.Length)
			{
				this.writeInt32(values[(int)num]);
				num += 1;
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0007A3CC File Offset: 0x000785CC
		public void writeUInt32(uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.writeBitConverterBytes(bytes);
			this.step += 4;
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x0007A3F8 File Offset: 0x000785F8
		public void writeSingle(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.writeBitConverterBytes(bytes);
			this.step += 4;
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0007A424 File Offset: 0x00078624
		public void writeInt64(long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.writeBitConverterBytes(bytes);
			this.step += 8;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0007A450 File Offset: 0x00078650
		public void writeUInt64(ulong value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.writeBitConverterBytes(bytes);
			this.step += 8;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0007A47C File Offset: 0x0007867C
		public void writeUInt64Array(ulong[] values)
		{
			this.writeUInt16((ushort)values.Length);
			ushort num = 0;
			while ((int)num < values.Length)
			{
				this.writeUInt64(values[(int)num]);
				num += 1;
			}
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0007A4AB File Offset: 0x000786AB
		public void writeSteamID(CSteamID steamID)
		{
			this.writeUInt64(steamID.m_SteamID);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0007A4BC File Offset: 0x000786BC
		public void writeGUID(Guid GUID)
		{
			GuidBuffer guidBuffer = new GuidBuffer(GUID);
			guidBuffer.Write(GuidBuffer.GUID_BUFFER, 0);
			this.writeByteArray(GuidBuffer.GUID_BUFFER);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0007A4EC File Offset: 0x000786EC
		public void writeUInt16RVector3(Vector3 value)
		{
			double num = (double)value.x + 4096.0;
			double num2 = (double)value.y + 1024.0;
			double num3 = (double)value.z + 4096.0;
			byte value2 = (byte)(num / (double)Regions.REGION_SIZE);
			byte value3 = (byte)(num3 / (double)Regions.REGION_SIZE);
			num %= (double)Regions.REGION_SIZE;
			num2 %= 2048.0;
			num3 %= (double)Regions.REGION_SIZE;
			num /= (double)Regions.REGION_SIZE;
			num2 /= 2048.0;
			num3 /= (double)Regions.REGION_SIZE;
			this.writeByte(value2);
			this.writeUInt16((ushort)(num * 65535.0));
			this.writeUInt16((ushort)(num2 * 65535.0));
			this.writeByte(value3);
			this.writeUInt16((ushort)(num3 * 65535.0));
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x0007A5C5 File Offset: 0x000787C5
		public void writeSingleVector3(Vector3 value)
		{
			this.writeSingle(value.x);
			this.writeSingle(value.y);
			this.writeSingle(value.z);
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0007A5EC File Offset: 0x000787EC
		public void writeSingleQuaternion(Quaternion value)
		{
			Vector3 eulerAngles = value.eulerAngles;
			this.writeSingle(eulerAngles.x);
			this.writeSingle(eulerAngles.y);
			this.writeSingle(eulerAngles.z);
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0007A625 File Offset: 0x00078825
		public void writeColor(Color value)
		{
			this.writeByte((byte)(value.r * 255f));
			this.writeByte((byte)(value.g * 255f));
			this.writeByte((byte)(value.b * 255f));
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0007A660 File Offset: 0x00078860
		public void write(object objects)
		{
			Type type = objects.GetType();
			if (type == Types.STRING_TYPE)
			{
				this.writeString((string)objects);
				return;
			}
			if (type == Types.STRING_ARRAY_TYPE)
			{
				this.writeStringArray((string[])objects);
				return;
			}
			if (type == Types.BOOLEAN_TYPE)
			{
				this.writeBoolean((bool)objects);
				return;
			}
			if (type == Types.BOOLEAN_ARRAY_TYPE)
			{
				this.writeBooleanArray((bool[])objects);
				return;
			}
			if (type == Types.BYTE_TYPE)
			{
				this.writeByte((byte)objects);
				return;
			}
			if (type == Types.BYTE_ARRAY_TYPE)
			{
				this.writeByteArray((byte[])objects);
				return;
			}
			if (type == Types.INT16_TYPE)
			{
				this.writeInt16((short)objects);
				return;
			}
			if (type == Types.UINT16_TYPE)
			{
				this.writeUInt16((ushort)objects);
				return;
			}
			if (type == Types.INT32_TYPE)
			{
				this.writeInt32((int)objects);
				return;
			}
			if (type == Types.INT32_ARRAY_TYPE)
			{
				this.writeInt32Array((int[])objects);
				return;
			}
			if (type == Types.UINT32_TYPE)
			{
				this.writeUInt32((uint)objects);
				return;
			}
			if (type == Types.SINGLE_TYPE)
			{
				this.writeSingle((float)objects);
				return;
			}
			if (type == Types.INT64_TYPE)
			{
				this.writeInt64((long)objects);
				return;
			}
			if (type == Types.UINT64_TYPE)
			{
				this.writeUInt64((ulong)objects);
				return;
			}
			if (type == Types.UINT64_ARRAY_TYPE)
			{
				this.writeUInt64Array((ulong[])objects);
				return;
			}
			if (type == Types.STEAM_ID_TYPE)
			{
				this.writeSteamID((CSteamID)objects);
				return;
			}
			if (type == Types.GUID_TYPE)
			{
				this.writeGUID((Guid)objects);
				return;
			}
			if (type == Types.VECTOR3_TYPE)
			{
				this.writeSingleVector3((Vector3)objects);
				return;
			}
			if (type == Types.COLOR_TYPE)
			{
				this.writeColor((Color)objects);
				return;
			}
			throw new NotSupportedException(string.Format("Cannot write {0} of type {1}", objects, type));
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0007A873 File Offset: 0x00078A73
		public void write(object object_0, object object_1)
		{
			this.write(object_0);
			this.write(object_1);
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x0007A883 File Offset: 0x00078A83
		public void write(object object_0, object object_1, object object_2)
		{
			this.write(object_0, object_1);
			this.write(object_2);
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0007A894 File Offset: 0x00078A94
		public void write(object object_0, object object_1, object object_2, object object_3)
		{
			this.write(object_0, object_1, object_2);
			this.write(object_3);
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0007A8A7 File Offset: 0x00078AA7
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4)
		{
			this.write(object_0, object_1, object_2, object_3);
			this.write(object_4);
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0007A8BC File Offset: 0x00078ABC
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
			this.write(object_0, object_1, object_2, object_3, object_4);
			this.write(object_5);
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0007A8D3 File Offset: 0x00078AD3
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
			this.write(object_0, object_1, object_2, object_3, object_4, object_5);
			this.write(object_6);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0007A8EC File Offset: 0x00078AEC
		public void write(params object[] objects)
		{
			for (int i = 0; i < objects.Length; i++)
			{
				this.write(objects[i]);
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0007A910 File Offset: 0x00078B10
		public byte[] getBytes(out int size)
		{
			ThreadUtil.assertIsGameThread();
			if (this.block == null)
			{
				size = this.step;
				return Block.buffer;
			}
			size = this.block.Length;
			return this.block;
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0007A93D File Offset: 0x00078B3D
		public byte[] getHash()
		{
			if (this.block == null)
			{
				return Hash.SHA1(Block.buffer);
			}
			return Hash.SHA1(this.block);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0007A95D File Offset: 0x00078B5D
		public void reset(int prefix, byte[] contents)
		{
			this.step = prefix;
			this.block = contents;
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0007A96D File Offset: 0x00078B6D
		public void reset(byte[] contents)
		{
			this.step = 0;
			this.block = contents;
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0007A97D File Offset: 0x00078B7D
		public void reset(int prefix)
		{
			this.step = prefix;
			this.block = null;
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0007A98D File Offset: 0x00078B8D
		public void reset()
		{
			this.step = 0;
			this.block = null;
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0007A99D File Offset: 0x00078B9D
		public Block(int prefix, byte[] contents)
		{
			this.reset(prefix, contents);
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0007A9AD File Offset: 0x00078BAD
		public Block(byte[] contents)
		{
			this.reset(contents);
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0007A9BC File Offset: 0x00078BBC
		public Block(int prefix)
		{
			this.reset(prefix);
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0007A9CB File Offset: 0x00078BCB
		public Block()
		{
			this.reset();
		}

		// Token: 0x04000FA1 RID: 4001
		public static readonly int BUFFER_SIZE = 65535;

		// Token: 0x04000FA2 RID: 4002
		public static byte[] buffer = new byte[Block.BUFFER_SIZE];

		// Token: 0x04000FA3 RID: 4003
		private static object[][] objects = new object[][]
		{
			new object[1],
			new object[2],
			new object[3],
			new object[4],
			new object[5],
			new object[6],
			new object[7]
		};

		// Token: 0x04000FA4 RID: 4004
		public bool longBinaryData;

		// Token: 0x04000FA5 RID: 4005
		public int step;

		// Token: 0x04000FA6 RID: 4006
		public byte[] block;
	}
}
