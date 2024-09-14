using System;
using SDG.Framework.Devkit.Transactions;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A1 RID: 161
	public class LandscapeHeightmapTransaction : IDevkitTransaction
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0001103B File Offset: 0x0000F23B
		public bool delta
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00011040 File Offset: 0x0000F240
		public void undo()
		{
			if (this.tile == null)
			{
				return;
			}
			float[,] heightmap = this.tile.heightmap;
			this.tile.heightmap = this.heightmapCopy;
			this.heightmapCopy = heightmap;
			this.tile.SetHeightsDelayLOD();
			this.tile.SyncHeightmap();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00011090 File Offset: 0x0000F290
		public void redo()
		{
			this.undo();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00011098 File Offset: 0x0000F298
		public void begin()
		{
			this.heightmapCopy = LandscapeHeightmapCopyPool.claim();
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					this.heightmapCopy[i, j] = this.tile.heightmap[i, j];
				}
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x000110EF File Offset: 0x0000F2EF
		public void end()
		{
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x000110F1 File Offset: 0x0000F2F1
		public void forget()
		{
			LandscapeHeightmapCopyPool.release(this.heightmapCopy);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000110FE File Offset: 0x0000F2FE
		public LandscapeHeightmapTransaction(LandscapeTile newTile)
		{
			this.tile = newTile;
		}

		// Token: 0x040001C3 RID: 451
		protected LandscapeTile tile;

		// Token: 0x040001C4 RID: 452
		protected float[,] heightmapCopy;
	}
}
