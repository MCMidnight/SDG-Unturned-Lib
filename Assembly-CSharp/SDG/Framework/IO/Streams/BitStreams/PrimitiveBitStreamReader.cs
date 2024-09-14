using System;
using System.IO;

namespace SDG.Framework.IO.Streams.BitStreams
{
	// Token: 0x020000B4 RID: 180
	public class PrimitiveBitStreamReader : BitStreamReader
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x00013B15 File Offset: 0x00011D15
		public void readByte(ref byte data)
		{
			base.readBits(ref data, 8);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00013B20 File Offset: 0x00011D20
		public void readInt16(ref short data)
		{
			byte b = 0;
			byte b2 = 0;
			this.readByte(ref b);
			this.readByte(ref b2);
			data = (short)((int)b << 8 | (int)b2);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00013B4C File Offset: 0x00011D4C
		public void readInt16(ref short data, byte length)
		{
			if (length == 16)
			{
				this.readInt16(ref data);
				return;
			}
			if (length > 8)
			{
				byte b = 0;
				byte b2 = 0;
				base.readBits(ref b, length - 8);
				this.readByte(ref b2);
				data = (short)((int)b << 8 | (int)b2);
				return;
			}
			if (length == 8)
			{
				byte b3 = 0;
				this.readByte(ref b3);
				data = (short)b3;
				return;
			}
			byte b4 = 0;
			base.readBits(ref b4, length);
			data = (short)b4;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00013BAC File Offset: 0x00011DAC
		public void readUInt16(ref ushort data)
		{
			byte b = 0;
			byte b2 = 0;
			this.readByte(ref b);
			this.readByte(ref b2);
			data = (ushort)((int)b << 8 | (int)b2);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00013BD8 File Offset: 0x00011DD8
		public void readUInt16(ref ushort data, byte length)
		{
			if (length == 16)
			{
				this.readUInt16(ref data);
				return;
			}
			if (length > 8)
			{
				byte b = 0;
				byte b2 = 0;
				base.readBits(ref b, length - 8);
				this.readByte(ref b2);
				data = (ushort)((int)b << 8 | (int)b2);
				return;
			}
			if (length == 8)
			{
				byte b3 = 0;
				this.readByte(ref b3);
				data = (ushort)b3;
				return;
			}
			byte b4 = 0;
			base.readBits(ref b4, length);
			data = (ushort)b4;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00013C38 File Offset: 0x00011E38
		public void readInt32(ref int data)
		{
			byte b = 0;
			byte b2 = 0;
			byte b3 = 0;
			byte b4 = 0;
			this.readByte(ref b);
			this.readByte(ref b2);
			this.readByte(ref b3);
			this.readByte(ref b4);
			data = ((int)b << 24 | (int)b2 << 16 | (int)b3 << 8 | (int)b4);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00013C80 File Offset: 0x00011E80
		public void readInt32(ref int data, byte length)
		{
			if (length == 32)
			{
				this.readInt32(ref data);
				return;
			}
			if (length > 24)
			{
				byte b = 0;
				byte b2 = 0;
				byte b3 = 0;
				byte b4 = 0;
				base.readBits(ref b, length - 8);
				this.readByte(ref b2);
				this.readByte(ref b3);
				this.readByte(ref b4);
				data = ((int)b << 24 | (int)b2 << 16 | (int)b3 << 8 | (int)b4);
				return;
			}
			if (length == 24)
			{
				byte b5 = 0;
				byte b6 = 0;
				byte b7 = 0;
				this.readByte(ref b5);
				this.readByte(ref b6);
				this.readByte(ref b7);
				data = ((int)b5 << 16 | (int)b6 << 8 | (int)b7);
				return;
			}
			if (length > 16)
			{
				byte b8 = 0;
				byte b9 = 0;
				byte b10 = 0;
				base.readBits(ref b8, length - 8);
				this.readByte(ref b9);
				this.readByte(ref b10);
				data = ((int)b8 << 16 | (int)b9 << 8 | (int)b10);
				return;
			}
			if (length == 16)
			{
				byte b11 = 0;
				byte b12 = 0;
				this.readByte(ref b11);
				this.readByte(ref b12);
				data = ((int)b11 << 8 | (int)b12);
				return;
			}
			if (length > 8)
			{
				byte b13 = 0;
				byte b14 = 0;
				base.readBits(ref b13, length - 8);
				this.readByte(ref b14);
				data = ((int)b13 << 8 | (int)b14);
				return;
			}
			if (length == 8)
			{
				byte b15 = 0;
				this.readByte(ref b15);
				data = (int)b15;
				return;
			}
			byte b16 = 0;
			base.readBits(ref b16, length);
			data = (int)b16;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00013DBE File Offset: 0x00011FBE
		public PrimitiveBitStreamReader(Stream newStream) : base(newStream)
		{
		}
	}
}
