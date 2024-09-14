using System;
using System.IO;

namespace SDG.Framework.IO.Streams
{
	// Token: 0x020000B1 RID: 177
	public class NetworkStream
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x000134BB File Offset: 0x000116BB
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x000134C3 File Offset: 0x000116C3
		private Stream stream { get; set; }

		// Token: 0x060004A3 RID: 1187 RVA: 0x000134CC File Offset: 0x000116CC
		public sbyte readSByte()
		{
			return (sbyte)this.stream.ReadByte();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000134DA File Offset: 0x000116DA
		public byte readByte()
		{
			return (byte)this.stream.ReadByte();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000134E8 File Offset: 0x000116E8
		public short readInt16()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			return (short)(b << 8 | b2);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00013508 File Offset: 0x00011708
		public ushort readUInt16()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			return (ushort)(b << 8 | b2);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00013528 File Offset: 0x00011728
		public int readInt32()
		{
			int num = (int)this.readByte();
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			return num << 24 | (int)b << 16 | (int)b2 << 8 | (int)b3;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00013560 File Offset: 0x00011760
		public uint readUInt32()
		{
			uint num = (uint)this.readByte();
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			return num << 24 | (uint)((uint)b << 16) | (uint)((uint)b2 << 8) | (uint)b3;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00013598 File Offset: 0x00011798
		public long readInt64()
		{
			int num = (int)this.readByte();
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			byte b4 = this.readByte();
			byte b5 = this.readByte();
			byte b6 = this.readByte();
			byte b7 = this.readByte();
			return (long)(num << 24 | (int)b << 16 | (int)b2 << 8 | (int)b3 | (int)b4 << 24 | (int)b5 << 16 | (int)b6 << 8 | (int)b7);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00013604 File Offset: 0x00011804
		public ulong readUInt64()
		{
			int num = (int)this.readByte();
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			byte b4 = this.readByte();
			byte b5 = this.readByte();
			byte b6 = this.readByte();
			byte b7 = this.readByte();
			return (ulong)((long)(num << 24 | (int)b << 16 | (int)b2 << 8 | (int)b3 | (int)b4 << 24 | (int)b5 << 16 | (int)b6 << 8 | (int)b7));
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001366D File Offset: 0x0001186D
		public char readChar()
		{
			return (char)this.readUInt16();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00013678 File Offset: 0x00011878
		public string readString()
		{
			ushort num = this.readUInt16();
			char[] array = new char[(int)num];
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				char c = this.readChar();
				array[(int)num2] = c;
			}
			return new string(array);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000136B1 File Offset: 0x000118B1
		public void readBytes(byte[] data, ulong offset, ulong length)
		{
			this.stream.Read(data, (int)offset, (int)length);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000136C4 File Offset: 0x000118C4
		public void writeSByte(sbyte data)
		{
			this.stream.WriteByte((byte)data);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000136D3 File Offset: 0x000118D3
		public void writeByte(byte data)
		{
			this.stream.WriteByte(data);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000136E1 File Offset: 0x000118E1
		public void writeInt16(short data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000136F5 File Offset: 0x000118F5
		public void writeUInt16(ushort data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00013709 File Offset: 0x00011909
		public void writeInt32(int data)
		{
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00013733 File Offset: 0x00011933
		public void writeUInt32(uint data)
		{
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00013760 File Offset: 0x00011960
		public void writeInt64(long data)
		{
			this.writeByte((byte)(data >> 56));
			this.writeByte((byte)(data >> 48));
			this.writeByte((byte)(data >> 40));
			this.writeByte((byte)(data >> 32));
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000137C4 File Offset: 0x000119C4
		public void writeUInt64(ulong data)
		{
			this.writeByte((byte)(data >> 56));
			this.writeByte((byte)(data >> 48));
			this.writeByte((byte)(data >> 40));
			this.writeByte((byte)(data >> 32));
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00013825 File Offset: 0x00011A25
		public void writeChar(char data)
		{
			this.writeUInt16((ushort)data);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00013830 File Offset: 0x00011A30
		public void writeString(string data)
		{
			ushort num = (ushort)data.Length;
			char[] array = data.ToCharArray();
			this.writeUInt16(num);
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				char data2 = array[(int)num2];
				this.writeChar(data2);
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001386B File Offset: 0x00011A6B
		public void writeBytes(byte[] data, ulong offset, ulong length)
		{
			this.stream.Write(data, (int)offset, (int)length);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001387D File Offset: 0x00011A7D
		public NetworkStream(Stream newStream)
		{
			this.stream = newStream;
		}
	}
}
