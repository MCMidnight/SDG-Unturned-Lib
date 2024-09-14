using System;
using SDG.Framework.Devkit.Transactions;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A9 RID: 169
	public class LandscapeSplatmapTransaction : IDevkitTransaction
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0001197A File Offset: 0x0000FB7A
		public bool delta
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00011980 File Offset: 0x0000FB80
		public void undo()
		{
			if (this.tile == null)
			{
				return;
			}
			float[,,] splatmap = this.tile.splatmap;
			this.tile.splatmap = this.splatmapCopy;
			this.splatmapCopy = splatmap;
			this.tile.data.SetAlphamaps(0, 0, this.tile.splatmap);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000119D7 File Offset: 0x0000FBD7
		public void redo()
		{
			this.undo();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000119E0 File Offset: 0x0000FBE0
		public void begin()
		{
			this.splatmapCopy = LandscapeSplatmapCopyPool.claim();
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						this.splatmapCopy[i, j, k] = this.tile.splatmap[i, j, k];
					}
				}
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011A49 File Offset: 0x0000FC49
		public void end()
		{
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011A4B File Offset: 0x0000FC4B
		public void forget()
		{
			LandscapeSplatmapCopyPool.release(this.splatmapCopy);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011A58 File Offset: 0x0000FC58
		public LandscapeSplatmapTransaction(LandscapeTile newTile)
		{
			this.tile = newTile;
		}

		// Token: 0x040001CF RID: 463
		protected LandscapeTile tile;

		// Token: 0x040001D0 RID: 464
		protected float[,,] splatmapCopy;
	}
}
