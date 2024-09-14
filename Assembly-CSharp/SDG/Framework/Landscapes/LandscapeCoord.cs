using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x0200009F RID: 159
	public struct LandscapeCoord : IFormattedFileReadable, IFormattedFileWritable, IEquatable<LandscapeCoord>
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x00010E32 File Offset: 0x0000F032
		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.x = reader.readValue<int>("X");
			this.y = reader.readValue<int>("Y");
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00010E5E File Offset: 0x0000F05E
		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<int>("X", this.x);
			writer.writeValue<int>("Y", this.y);
			writer.endObject();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00010E8E File Offset: 0x0000F08E
		public static bool operator ==(LandscapeCoord a, LandscapeCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00010EAE File Offset: 0x0000F0AE
		public static bool operator !=(LandscapeCoord a, LandscapeCoord b)
		{
			return !(a == b);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00010EBC File Offset: 0x0000F0BC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			LandscapeCoord landscapeCoord = (LandscapeCoord)obj;
			return this.x.Equals(landscapeCoord.x) && this.y.Equals(landscapeCoord.y);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00010EFB File Offset: 0x0000F0FB
		public override int GetHashCode()
		{
			return this.x ^ this.y;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00010F0C File Offset: 0x0000F10C
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

		// Token: 0x0600041E RID: 1054 RVA: 0x00010F58 File Offset: 0x0000F158
		public bool Equals(LandscapeCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00010F78 File Offset: 0x0000F178
		public LandscapeCoord(int new_x, int new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00010F88 File Offset: 0x0000F188
		public LandscapeCoord(Vector3 position)
		{
			this.x = Mathf.FloorToInt(position.x / Landscape.TILE_SIZE);
			this.y = Mathf.FloorToInt(position.z / Landscape.TILE_SIZE);
		}

		// Token: 0x040001BF RID: 447
		public static LandscapeCoord ZERO = new LandscapeCoord(0, 0);

		// Token: 0x040001C0 RID: 448
		public int x;

		// Token: 0x040001C1 RID: 449
		public int y;
	}
}
