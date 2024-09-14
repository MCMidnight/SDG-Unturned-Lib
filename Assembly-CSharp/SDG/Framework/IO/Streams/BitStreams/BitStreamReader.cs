using System;
using System.IO;

namespace SDG.Framework.IO.Streams.BitStreams
{
	// Token: 0x020000B2 RID: 178
	public class BitStreamReader
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0001388C File Offset: 0x00011A8C
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x00013894 File Offset: 0x00011A94
		public Stream stream { get; protected set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001389D File Offset: 0x00011A9D
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x000138A5 File Offset: 0x00011AA5
		private byte buffer { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x000138AE File Offset: 0x00011AAE
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x000138B6 File Offset: 0x00011AB6
		private byte bitIndex { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x000138BF File Offset: 0x00011ABF
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x000138C7 File Offset: 0x00011AC7
		private byte bitsAvailable { get; set; }

		// Token: 0x060004C2 RID: 1218 RVA: 0x000138D0 File Offset: 0x00011AD0
		public void readBit(ref byte data)
		{
			this.readBits(ref data, 1);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000138DC File Offset: 0x00011ADC
		public void readBits(ref byte data, byte length)
		{
			if (this.bitIndex == 8 && this.bitsAvailable == 0)
			{
				this.fillBuffer();
			}
			if (length > this.bitsAvailable)
			{
				byte b = length - this.bitsAvailable;
				this.readBits(ref data, this.bitsAvailable);
				data = (byte)(data << (int)b);
				this.readBits(ref data, b);
				return;
			}
			byte b2 = 8 - length - this.bitIndex;
			byte b3 = (byte)(255 >> (int)(8 - length));
			data |= (byte)(this.buffer >> (int)b2 & (int)b3);
			this.bitIndex += length;
			this.bitsAvailable -= length;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001397F File Offset: 0x00011B7F
		private void fillBuffer()
		{
			this.buffer = (byte)this.stream.ReadByte();
			this.bitIndex = 0;
			this.bitsAvailable = 8;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000139A1 File Offset: 0x00011BA1
		public void reset()
		{
			this.buffer = 0;
			this.bitIndex = 8;
			this.bitsAvailable = 0;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000139B8 File Offset: 0x00011BB8
		public BitStreamReader(Stream newStream)
		{
			this.stream = newStream;
			this.reset();
		}
	}
}
