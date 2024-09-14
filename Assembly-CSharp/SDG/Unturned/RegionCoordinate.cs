using System;

namespace SDG.Unturned
{
	// Token: 0x020006C0 RID: 1728
	public struct RegionCoordinate
	{
		// Token: 0x060039A7 RID: 14759 RVA: 0x0010E378 File Offset: 0x0010C578
		public RegionCoordinate(byte x, byte y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x0010E388 File Offset: 0x0010C588
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

		// Token: 0x04002244 RID: 8772
		public byte x;

		// Token: 0x04002245 RID: 8773
		public byte y;
	}
}
