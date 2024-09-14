using System;
using SDG.Framework.Devkit.Transactions;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A3 RID: 163
	public class LandscapeHoleTransaction : IDevkitTransaction
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00011182 File Offset: 0x0000F382
		public bool delta
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00011188 File Offset: 0x0000F388
		public void undo()
		{
			if (this.tile == null)
			{
				return;
			}
			bool[,] holes = this.tile.holes;
			this.tile.holes = this.holesCopy;
			this.holesCopy = holes;
			this.tile.data.SetHoles(0, 0, this.tile.holes);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000111DF File Offset: 0x0000F3DF
		public void redo()
		{
			this.undo();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000111E8 File Offset: 0x0000F3E8
		public void begin()
		{
			this.holesCopy = LandscapeHoleCopyPool.claim();
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < 256; j++)
				{
					this.holesCopy[i, j] = this.tile.holes[i, j];
				}
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0001123F File Offset: 0x0000F43F
		public void end()
		{
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00011241 File Offset: 0x0000F441
		public void forget()
		{
			LandscapeHoleCopyPool.release(this.holesCopy);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001124E File Offset: 0x0000F44E
		public LandscapeHoleTransaction(LandscapeTile newTile)
		{
			this.tile = newTile;
		}

		// Token: 0x040001C6 RID: 454
		protected LandscapeTile tile;

		// Token: 0x040001C7 RID: 455
		protected bool[,] holesCopy;
	}
}
