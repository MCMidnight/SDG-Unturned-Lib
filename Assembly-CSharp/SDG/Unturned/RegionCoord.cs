using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006BC RID: 1724
	public struct RegionCoord : IFormattedFileReadable, IFormattedFileWritable, IEquatable<RegionCoord>
	{
		// Token: 0x06003989 RID: 14729 RVA: 0x0010DF2C File Offset: 0x0010C12C
		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.x = reader.readValue<byte>("X");
			this.y = reader.readValue<byte>("Y");
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x0010DF58 File Offset: 0x0010C158
		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<byte>("X", this.x);
			writer.writeValue<byte>("Y", this.y);
			writer.endObject();
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x0010DF88 File Offset: 0x0010C188
		public static bool operator ==(RegionCoord a, RegionCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x0010DFA8 File Offset: 0x0010C1A8
		public static bool operator !=(RegionCoord a, RegionCoord b)
		{
			return !(a == b);
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x0010DFB4 File Offset: 0x0010C1B4
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			RegionCoord regionCoord = (RegionCoord)obj;
			return this.x == regionCoord.x && this.y == regionCoord.y;
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x0010DFEB File Offset: 0x0010C1EB
		public override int GetHashCode()
		{
			return (int)(this.x ^ this.y);
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0010DFFC File Offset: 0x0010C1FC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				")"
			});
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x0010E048 File Offset: 0x0010C248
		public bool Equals(RegionCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x0010E068 File Offset: 0x0010C268
		public void ClampIntoBounds()
		{
			this.x = (byte)Mathf.Max((int)this.x, 0);
			this.x = (byte)Mathf.Min((int)this.x, 63);
			this.y = (byte)Mathf.Max((int)this.y, 0);
			this.y = (byte)Mathf.Min((int)this.y, 63);
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x0010E0C3 File Offset: 0x0010C2C3
		public RegionCoord(byte new_x, byte new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x0010E0D3 File Offset: 0x0010C2D3
		public RegionCoord(Vector3 position)
		{
			Regions.tryGetCoordinate(position, out this.x, out this.y);
		}

		// Token: 0x0400223C RID: 8764
		public static RegionCoord ZERO = new RegionCoord(0, 0);

		// Token: 0x0400223D RID: 8765
		public byte x;

		// Token: 0x0400223E RID: 8766
		public byte y;
	}
}
