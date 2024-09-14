using System;
using System.IO;

namespace SDG.Framework.IO.Streams.BitStreams
{
	// Token: 0x020000B5 RID: 181
	public class PrimitiveBitStreamWriter : BitStreamWriter
	{
		// Token: 0x060004DD RID: 1245 RVA: 0x00013DC7 File Offset: 0x00011FC7
		public void writeByte(byte data)
		{
			base.writeBits(data, 8);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00013DD1 File Offset: 0x00011FD1
		public void writeInt16(short data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00013DE5 File Offset: 0x00011FE5
		public void writeInt16(short data, byte length)
		{
			if (length == 16)
			{
				this.writeInt16(data);
				return;
			}
			if (length > 8)
			{
				base.writeBits((byte)(data >> 8), length - 8);
				this.writeByte((byte)data);
				return;
			}
			if (length == 8)
			{
				this.writeByte((byte)data);
				return;
			}
			base.writeBits((byte)data, length);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00013E25 File Offset: 0x00012025
		public void writeUInt16(ushort data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00013E39 File Offset: 0x00012039
		public void writeUInt16(ushort data, byte length)
		{
			if (length == 16)
			{
				this.writeUInt16(data);
				return;
			}
			if (length > 8)
			{
				base.writeBits((byte)(data >> 8), length - 8);
				this.writeByte((byte)data);
				return;
			}
			if (length == 8)
			{
				this.writeByte((byte)data);
				return;
			}
			base.writeBits((byte)data, length);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00013E79 File Offset: 0x00012079
		public void writeInt32(int data)
		{
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00013EA4 File Offset: 0x000120A4
		public void writeInt32(int data, byte length)
		{
			if (length == 32)
			{
				this.writeInt32(data);
				return;
			}
			if (length > 24)
			{
				base.writeBits((byte)(data >> 24), length - 8);
				this.writeByte((byte)(data >> 16));
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
				return;
			}
			if (length == 24)
			{
				this.writeByte((byte)(data >> 16));
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
				return;
			}
			if (length > 16)
			{
				base.writeBits((byte)(data >> 16), length - 8);
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
				return;
			}
			if (length == 16)
			{
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
				return;
			}
			if (length > 8)
			{
				base.writeBits((byte)(data >> 8), length - 8);
				this.writeByte((byte)data);
				return;
			}
			if (length == 8)
			{
				this.writeByte((byte)data);
				return;
			}
			base.writeBits((byte)data, length);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00013F83 File Offset: 0x00012183
		public PrimitiveBitStreamWriter(Stream newStream) : base(newStream)
		{
		}
	}
}
