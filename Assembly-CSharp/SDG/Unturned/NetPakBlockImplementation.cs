using System;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Exposes the same API as the older Block class used by existing netcode, but implemented using new bit reader/writer. 
	/// </summary>
	// Token: 0x02000281 RID: 641
	internal class NetPakBlockImplementation
	{
		// Token: 0x06001292 RID: 4754 RVA: 0x00042114 File Offset: 0x00040314
		public object read(Type type)
		{
			if (type == Types.STRING_TYPE)
			{
				string result;
				SystemNetPakReaderEx.ReadString(this.reader, ref result, 11);
				return result;
			}
			if (type == Types.STRING_ARRAY_TYPE)
			{
				byte b;
				SystemNetPakReaderEx.ReadUInt8(this.reader, ref b);
				string[] array = new string[(int)b];
				for (int i = 0; i < array.Length; i++)
				{
					SystemNetPakReaderEx.ReadString(this.reader, ref array[i], 11);
				}
				return array;
			}
			if (type == Types.BOOLEAN_TYPE)
			{
				bool flag;
				this.reader.ReadBit(ref flag);
				return flag;
			}
			if (type == Types.BOOLEAN_ARRAY_TYPE)
			{
				ushort num;
				SystemNetPakReaderEx.ReadUInt16(this.reader, ref num);
				bool[] array2 = new bool[(int)num];
				for (int j = 0; j < array2.Length; j++)
				{
					this.reader.ReadBit(ref array2[j]);
				}
				return array2;
			}
			if (type == Types.BYTE_TYPE)
			{
				byte b2;
				SystemNetPakReaderEx.ReadUInt8(this.reader, ref b2);
				return b2;
			}
			if (type == Types.BYTE_ARRAY_TYPE)
			{
				byte b3;
				SystemNetPakReaderEx.ReadUInt8(this.reader, ref b3);
				byte[] array3 = new byte[(int)b3];
				this.reader.ReadBytes(array3);
				return array3;
			}
			if (type == Types.INT16_TYPE)
			{
				short num2;
				SystemNetPakReaderEx.ReadInt16(this.reader, ref num2);
				return num2;
			}
			if (type == Types.UINT16_TYPE)
			{
				ushort num3;
				SystemNetPakReaderEx.ReadUInt16(this.reader, ref num3);
				return num3;
			}
			if (type == Types.INT32_TYPE)
			{
				int num4;
				SystemNetPakReaderEx.ReadInt32(this.reader, ref num4);
				return num4;
			}
			if (type == Types.INT32_ARRAY_TYPE)
			{
				ushort num5;
				SystemNetPakReaderEx.ReadUInt16(this.reader, ref num5);
				int[] array4 = new int[(int)num5];
				for (int k = 0; k < array4.Length; k++)
				{
					SystemNetPakReaderEx.ReadInt32(this.reader, ref array4[k]);
				}
				return array4;
			}
			if (type == Types.UINT32_TYPE)
			{
				uint num6;
				SystemNetPakReaderEx.ReadUInt32(this.reader, ref num6);
				return num6;
			}
			if (type == Types.SINGLE_TYPE)
			{
				float num7;
				SystemNetPakReaderEx.ReadFloat(this.reader, ref num7);
				return num7;
			}
			if (type == Types.INT64_TYPE)
			{
				long num8;
				SystemNetPakReaderEx.ReadInt64(this.reader, ref num8);
				return num8;
			}
			if (type == Types.UINT64_TYPE)
			{
				ulong num9;
				SystemNetPakReaderEx.ReadUInt64(this.reader, ref num9);
				return num9;
			}
			if (type == Types.UINT64_ARRAY_TYPE)
			{
				ushort num10;
				SystemNetPakReaderEx.ReadUInt16(this.reader, ref num10);
				ulong[] array5 = new ulong[(int)num10];
				for (int l = 0; l < array5.Length; l++)
				{
					SystemNetPakReaderEx.ReadUInt64(this.reader, ref array5[l]);
				}
				return array5;
			}
			if (type == Types.STEAM_ID_TYPE)
			{
				CSteamID csteamID;
				SteamworksNetPakReaderEx.ReadSteamID(this.reader, ref csteamID);
				return csteamID;
			}
			if (type == Types.GUID_TYPE)
			{
				Guid guid;
				SystemNetPakReaderEx.ReadGuid(this.reader, ref guid);
				return guid;
			}
			if (type == Types.VECTOR3_TYPE)
			{
				Vector3 vector;
				UnityNetPakReaderEx.ReadClampedVector3(this.reader, ref vector, 13, 9);
				return vector;
			}
			if (type == Types.QUATERNION_TYPE)
			{
				Quaternion quaternion;
				UnityNetPakReaderEx.ReadQuaternion(this.reader, ref quaternion, 9);
				return quaternion;
			}
			if (type == Types.COLOR_TYPE)
			{
				Color32 c;
				UnityNetPakReaderEx.ReadColor32RGB(this.reader, ref c);
				return c;
			}
			if (type == typeof(NetId))
			{
				NetId netId;
				this.reader.ReadNetId(out netId);
				return netId;
			}
			if (type.IsEnum)
			{
				byte b4;
				SystemNetPakReaderEx.ReadUInt8(this.reader, ref b4);
				return Enum.ToObject(type, b4);
			}
			throw new NotSupportedException(string.Format("Cannot read type {0}", type));
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000424F8 File Offset: 0x000406F8
		public object[] read(int offset, Type type_0)
		{
			object[] array = NetPakBlockImplementation.getObjects(0);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			return array;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0004251C File Offset: 0x0004071C
		public object[] read(int offset, Type type_0, Type type_1)
		{
			object[] array = NetPakBlockImplementation.getObjects(1);
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

		// Token: 0x06001295 RID: 4757 RVA: 0x0004254D File Offset: 0x0004074D
		public object[] read(Type type_0, Type type_1)
		{
			return this.read(0, type_0, type_1);
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00042558 File Offset: 0x00040758
		public object[] read(int offset, Type type_0, Type type_1, Type type_2)
		{
			object[] array = NetPakBlockImplementation.getObjects(2);
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

		// Token: 0x06001297 RID: 4759 RVA: 0x00042598 File Offset: 0x00040798
		public object[] read(Type type_0, Type type_1, Type type_2)
		{
			return this.read(0, type_0, type_1, type_2);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000425A4 File Offset: 0x000407A4
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3)
		{
			object[] array = NetPakBlockImplementation.getObjects(3);
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

		// Token: 0x06001299 RID: 4761 RVA: 0x000425F3 File Offset: 0x000407F3
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
		{
			return this.read(0, type_0, type_1, type_2, type_3);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00042604 File Offset: 0x00040804
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			object[] array = NetPakBlockImplementation.getObjects(4);
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

		// Token: 0x0600129B RID: 4763 RVA: 0x00042662 File Offset: 0x00040862
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00042674 File Offset: 0x00040874
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			object[] array = NetPakBlockImplementation.getObjects(5);
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

		// Token: 0x0600129D RID: 4765 RVA: 0x000426E1 File Offset: 0x000408E1
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x000426F4 File Offset: 0x000408F4
		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			object[] array = NetPakBlockImplementation.getObjects(6);
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

		// Token: 0x0600129F RID: 4767 RVA: 0x00042770 File Offset: 0x00040970
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00042790 File Offset: 0x00040990
		public object[] readForLegacyRPC(int offset, Type[] types)
		{
			object[] array = new object[types.Length];
			for (int i = offset; i < types.Length; i++)
			{
				array[i] = this.read(types[i]);
			}
			return array;
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x000427C4 File Offset: 0x000409C4
		public object[] read(int offset, params Type[] types)
		{
			object[] array = new object[types.Length];
			for (int i = offset; i < types.Length; i++)
			{
				array[i] = this.read(types[i]);
			}
			return array;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000427F5 File Offset: 0x000409F5
		public object[] read(params Type[] types)
		{
			return this.read(0, types);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00042800 File Offset: 0x00040A00
		public void write(object objects)
		{
			Type type = objects.GetType();
			if (type == Types.STRING_TYPE)
			{
				SystemNetPakWriterEx.WriteString(this.writer, (string)objects, 11);
				return;
			}
			if (type == Types.STRING_ARRAY_TYPE)
			{
				string[] array = (string[])objects;
				byte b = (byte)array.Length;
				SystemNetPakWriterEx.WriteUInt8(this.writer, b);
				for (int i = 0; i < (int)b; i++)
				{
					SystemNetPakWriterEx.WriteString(this.writer, array[i], 11);
				}
				return;
			}
			if (type == Types.BOOLEAN_TYPE)
			{
				this.writer.WriteBit((bool)objects);
				return;
			}
			if (type == Types.BOOLEAN_ARRAY_TYPE)
			{
				bool[] array2 = (bool[])objects;
				ushort num = (ushort)array2.Length;
				SystemNetPakWriterEx.WriteUInt16(this.writer, num);
				for (int j = 0; j < (int)num; j++)
				{
					this.writer.WriteBit(array2[j]);
				}
				return;
			}
			if (type == Types.BYTE_TYPE)
			{
				SystemNetPakWriterEx.WriteUInt8(this.writer, (byte)objects);
				return;
			}
			if (type == Types.BYTE_ARRAY_TYPE)
			{
				byte[] array3 = (byte[])objects;
				byte b2 = (byte)array3.Length;
				SystemNetPakWriterEx.WriteUInt8(this.writer, b2);
				this.writer.WriteBytes(array3, (int)b2);
				return;
			}
			if (type == Types.INT16_TYPE)
			{
				SystemNetPakWriterEx.WriteInt16(this.writer, (short)objects);
				return;
			}
			if (type == Types.UINT16_TYPE)
			{
				SystemNetPakWriterEx.WriteUInt16(this.writer, (ushort)objects);
				return;
			}
			if (type == Types.INT32_TYPE)
			{
				SystemNetPakWriterEx.WriteInt32(this.writer, (int)objects);
				return;
			}
			if (type == Types.INT32_ARRAY_TYPE)
			{
				int[] array4 = (int[])objects;
				ushort num2 = (ushort)array4.Length;
				SystemNetPakWriterEx.WriteUInt16(this.writer, num2);
				for (int k = 0; k < (int)num2; k++)
				{
					SystemNetPakWriterEx.WriteInt32(this.writer, array4[k]);
				}
				return;
			}
			if (type == Types.UINT32_TYPE)
			{
				SystemNetPakWriterEx.WriteUInt32(this.writer, (uint)objects);
				return;
			}
			if (type == Types.SINGLE_TYPE)
			{
				SystemNetPakWriterEx.WriteFloat(this.writer, (float)objects);
				return;
			}
			if (type == Types.INT64_TYPE)
			{
				SystemNetPakWriterEx.WriteInt64(this.writer, (long)objects);
				return;
			}
			if (type == Types.UINT64_TYPE)
			{
				SystemNetPakWriterEx.WriteUInt64(this.writer, (ulong)objects);
				return;
			}
			if (type == Types.UINT64_ARRAY_TYPE)
			{
				ulong[] array5 = (ulong[])objects;
				ushort num3 = (ushort)array5.Length;
				SystemNetPakWriterEx.WriteUInt16(this.writer, num3);
				for (int l = 0; l < (int)num3; l++)
				{
					SystemNetPakWriterEx.WriteUInt64(this.writer, array5[l]);
				}
				return;
			}
			if (type == Types.STEAM_ID_TYPE)
			{
				SteamworksNetPakWriterEx.WriteSteamID(this.writer, (CSteamID)objects);
				return;
			}
			if (type == Types.GUID_TYPE)
			{
				SystemNetPakWriterEx.WriteGuid(this.writer, (Guid)objects);
				return;
			}
			if (type == Types.VECTOR3_TYPE)
			{
				UnityNetPakWriterEx.WriteClampedVector3(this.writer, (Vector3)objects, 13, 9);
				return;
			}
			if (type == Types.QUATERNION_TYPE)
			{
				Quaternion quaternion = (Quaternion)objects;
				UnityNetPakWriterEx.WriteQuaternion(this.writer, quaternion, 9);
				return;
			}
			if (type == Types.COLOR_TYPE)
			{
				Color c = (Color)objects;
				UnityNetPakWriterEx.WriteColor32RGB(this.writer, c);
				return;
			}
			if (type == typeof(NetId))
			{
				NetId value = (NetId)objects;
				this.writer.WriteNetId(value);
				return;
			}
			throw new NotSupportedException(string.Format("Cannot write {0} of type {1}", objects, type));
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00042BA9 File Offset: 0x00040DA9
		public void write(object object_0, object object_1)
		{
			this.write(object_0);
			this.write(object_1);
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00042BB9 File Offset: 0x00040DB9
		public void write(object object_0, object object_1, object object_2)
		{
			this.write(object_0, object_1);
			this.write(object_2);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00042BCA File Offset: 0x00040DCA
		public void write(object object_0, object object_1, object object_2, object object_3)
		{
			this.write(object_0, object_1, object_2);
			this.write(object_3);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00042BDD File Offset: 0x00040DDD
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4)
		{
			this.write(object_0, object_1, object_2, object_3);
			this.write(object_4);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00042BF2 File Offset: 0x00040DF2
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
			this.write(object_0, object_1, object_2, object_3, object_4);
			this.write(object_5);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00042C09 File Offset: 0x00040E09
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
			this.write(object_0, object_1, object_2, object_3, object_4, object_5);
			this.write(object_6);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00042C24 File Offset: 0x00040E24
		public void write(params object[] objects)
		{
			for (int i = 0; i < objects.Length; i++)
			{
				this.write(objects[i]);
			}
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00042C48 File Offset: 0x00040E48
		public void resetForRead(int prefix, byte[] buffer, int size)
		{
			this.reader.SetBuffer(buffer);
			this.reader.Reset();
			this.reader.readByteIndex = prefix;
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00042C6D File Offset: 0x00040E6D
		public void resetForWrite(int prefix)
		{
			this.writer.Reset();
			this.writer.writeByteIndex = prefix;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00042C86 File Offset: 0x00040E86
		public byte[] getBytes(out int size)
		{
			ThreadUtil.assertIsGameThread();
			this.writer.Flush();
			size = this.writer.writeByteIndex;
			return this.writer.buffer;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00042CB1 File Offset: 0x00040EB1
		public NetPakBlockImplementation()
		{
			this.reader = new NetPakReader();
			this.reader.SetBuffer(Provider.buffer);
			this.writer = new NetPakWriter();
			this.writer.buffer = Block.buffer;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00042CF0 File Offset: 0x00040EF0
		private static object[] getObjects(int index)
		{
			object[] array = NetPakBlockImplementation.objects[index];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = null;
			}
			return array;
		}

		// Token: 0x040005EA RID: 1514
		[Obsolete]
		public bool longBinaryData;

		// Token: 0x040005EB RID: 1515
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

		// Token: 0x040005EC RID: 1516
		private NetPakReader reader;

		// Token: 0x040005ED RID: 1517
		private NetPakWriter writer;
	}
}
