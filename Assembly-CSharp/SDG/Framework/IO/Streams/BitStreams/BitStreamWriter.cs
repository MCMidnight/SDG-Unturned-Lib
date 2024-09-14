using System;
using System.IO;

namespace SDG.Framework.IO.Streams.BitStreams
{
	// Token: 0x020000B3 RID: 179
	public class BitStreamWriter
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x000139CD File Offset: 0x00011BCD
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x000139D5 File Offset: 0x00011BD5
		public Stream stream { get; protected set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x000139DE File Offset: 0x00011BDE
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x000139E6 File Offset: 0x00011BE6
		private byte buffer { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x000139EF File Offset: 0x00011BEF
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x000139F7 File Offset: 0x00011BF7
		private byte bitIndex { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00013A00 File Offset: 0x00011C00
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00013A08 File Offset: 0x00011C08
		private byte bitsAvailable { get; set; }

		// Token: 0x060004CF RID: 1231 RVA: 0x00013A11 File Offset: 0x00011C11
		public void writeBit(byte data)
		{
			this.writeBits(data, 1);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00013A1C File Offset: 0x00011C1C
		public void writeBits(byte data, byte length)
		{
			if (length > this.bitsAvailable)
			{
				byte b = length - this.bitsAvailable;
				this.writeBits((byte)(data >> (int)b), this.bitsAvailable);
				this.writeBits(data, b);
				return;
			}
			byte b2 = 8 - length - this.bitIndex;
			byte b3 = (byte)(255 >> (int)(8 - length));
			this.buffer |= (byte)((data & b3) << (int)b2);
			this.bitIndex += length;
			this.bitsAvailable -= length;
			if (this.bitIndex == 8 && this.bitsAvailable == 0)
			{
				this.emptyBuffer();
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00013ABE File Offset: 0x00011CBE
		private void emptyBuffer()
		{
			this.stream.WriteByte(this.buffer);
			this.reset();
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00013AD7 File Offset: 0x00011CD7
		public void flush()
		{
			if (this.bitsAvailable == 8)
			{
				return;
			}
			this.emptyBuffer();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00013AE9 File Offset: 0x00011CE9
		public void reset()
		{
			this.buffer = 0;
			this.bitIndex = 0;
			this.bitsAvailable = 8;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00013B00 File Offset: 0x00011D00
		public BitStreamWriter(Stream newStream)
		{
			this.stream = newStream;
			this.reset();
		}
	}
}
