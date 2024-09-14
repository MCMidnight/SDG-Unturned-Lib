using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000E9 RID: 233
	public struct FoliageCoord : IFormattedFileReadable, IFormattedFileWritable, IEquatable<FoliageCoord>
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x0001581E File Offset: 0x00013A1E
		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.x = reader.readValue<int>("X");
			this.y = reader.readValue<int>("Y");
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001584A File Offset: 0x00013A4A
		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<int>("X", this.x);
			writer.writeValue<int>("Y", this.y);
			writer.endObject();
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001587A File Offset: 0x00013A7A
		public static bool operator ==(FoliageCoord a, FoliageCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001589A File Offset: 0x00013A9A
		public static bool operator !=(FoliageCoord a, FoliageCoord b)
		{
			return !(a == b);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000158A8 File Offset: 0x00013AA8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			FoliageCoord foliageCoord = (FoliageCoord)obj;
			return this.x == foliageCoord.x && this.y == foliageCoord.y;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000158DF File Offset: 0x00013ADF
		public override int GetHashCode()
		{
			return this.x ^ this.y;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000158F0 File Offset: 0x00013AF0
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

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001593C File Offset: 0x00013B3C
		public bool Equals(FoliageCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001595C File Offset: 0x00013B5C
		public FoliageCoord(int new_x, int new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001596C File Offset: 0x00013B6C
		public FoliageCoord(Vector3 position)
		{
			this.x = Mathf.FloorToInt(position.x / FoliageSystem.TILE_SIZE);
			this.y = Mathf.FloorToInt(position.z / FoliageSystem.TILE_SIZE);
		}

		// Token: 0x0400020A RID: 522
		public static FoliageCoord ZERO = new FoliageCoord(0, 0);

		// Token: 0x0400020B RID: 523
		public int x;

		// Token: 0x0400020C RID: 524
		public int y;
	}
}
