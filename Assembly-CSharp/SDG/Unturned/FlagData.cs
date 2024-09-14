using System;

namespace SDG.Unturned
{
	// Token: 0x020004C5 RID: 1221
	public class FlagData
	{
		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600256D RID: 9581 RVA: 0x00094EFD File Offset: 0x000930FD
		// (set) Token: 0x0600256E RID: 9582 RVA: 0x00094F08 File Offset: 0x00093108
		public string difficultyGUID
		{
			get
			{
				return this._difficultyGUID;
			}
			set
			{
				this._difficultyGUID = value;
				try
				{
					this.difficulty = new AssetReference<ZombieDifficultyAsset>(new Guid(this.difficultyGUID));
				}
				catch
				{
					this.difficulty = AssetReference<ZombieDifficultyAsset>.invalid;
				}
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x00094F54 File Offset: 0x00093154
		// (set) Token: 0x06002570 RID: 9584 RVA: 0x00094F5C File Offset: 0x0009315C
		public AssetReference<ZombieDifficultyAsset> difficulty { get; private set; }

		// Token: 0x06002571 RID: 9585 RVA: 0x00094F68 File Offset: 0x00093168
		public ZombieDifficultyAsset resolveDifficulty()
		{
			if (this.cachedDifficulty == null && this.difficulty.isValid)
			{
				this.cachedDifficulty = Assets.find<ZombieDifficultyAsset>(this.difficulty);
			}
			return this.cachedDifficulty;
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x00094FA4 File Offset: 0x000931A4
		public FlagData(string newDifficultyGUID = "", byte newMaxZombies = 64, bool newSpawnZombies = true, bool newHyperAgro = false, int maxBossZombies = -1)
		{
			this.difficultyGUID = newDifficultyGUID;
			this.maxZombies = newMaxZombies;
			this.spawnZombies = newSpawnZombies;
			this.hyperAgro = newHyperAgro;
			this.maxBossZombies = maxBossZombies;
		}

		// Token: 0x04001331 RID: 4913
		private string _difficultyGUID;

		// Token: 0x04001333 RID: 4915
		private ZombieDifficultyAsset cachedDifficulty;

		// Token: 0x04001334 RID: 4916
		public byte maxZombies;

		// Token: 0x04001335 RID: 4917
		public bool spawnZombies;

		// Token: 0x04001336 RID: 4918
		public bool hyperAgro;

		/// <summary>
		/// Maximum count of naturally spawned boss zombies. Unlimited if negative.
		/// Game will not spawn/respawn boss zombie types passing this limit, but quest spawns can bypass it.
		/// </summary>
		// Token: 0x04001337 RID: 4919
		public int maxBossZombies;
	}
}
