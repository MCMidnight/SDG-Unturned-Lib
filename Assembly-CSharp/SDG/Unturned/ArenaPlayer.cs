using System;

namespace SDG.Unturned
{
	// Token: 0x0200056E RID: 1390
	public class ArenaPlayer
	{
		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002C58 RID: 11352 RVA: 0x000C014C File Offset: 0x000BE34C
		public SteamPlayer steamPlayer
		{
			get
			{
				return this._steamPlayer;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002C59 RID: 11353 RVA: 0x000C0154 File Offset: 0x000BE354
		public bool hasDied
		{
			get
			{
				return this._hasDied;
			}
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x000C015C File Offset: 0x000BE35C
		private void onLifeUpdated(bool isDead)
		{
			if (isDead)
			{
				this._hasDied = true;
			}
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000C0168 File Offset: 0x000BE368
		public ArenaPlayer(SteamPlayer newSteamPlayer)
		{
			this._steamPlayer = newSteamPlayer;
			this._hasDied = false;
			PlayerLife life = this.steamPlayer.player.life;
			life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
		}

		// Token: 0x040017EA RID: 6122
		private SteamPlayer _steamPlayer;

		// Token: 0x040017EB RID: 6123
		private bool _hasDied;

		/// <summary>
		/// Time.time damage was last dealt so that damage is applied once per second.
		/// </summary>
		// Token: 0x040017EC RID: 6124
		public float lastAreaDamage;

		/// <summary>
		/// Timer increased while taking damage, and reset to zero while inside zone.
		/// </summary>
		// Token: 0x040017ED RID: 6125
		public float timeOutsideArea;
	}
}
