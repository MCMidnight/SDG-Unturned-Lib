using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E4 RID: 2020
	public class BulletInfo
	{
		/// <summary>
		/// Only available on the server. For use by plugins developers who want to analyze deviation between approximate
		/// start direction and final hit position using <see cref="E:SDG.Unturned.UseableGun.onBulletSpawned" /> and <see cref="E:SDG.Unturned.UseableGun.onBulletHit" />
		/// per public issue #4450. Note that origin and direction on server are not necessarily exactly the same as on
		/// the client for a variety of reasons, including that bullets on the client can be spawned between simulation
		/// frames when the aim direction was different. (Aim direction is updated every drawn frame on the client as
		/// opposed to every simulation frame on the server.) Rather than kicking for one particularly large deviation
		/// we would recommend tracking stats for each shot's actual deviation vs max theoretical deviation. Remember
		/// to account for bullet drop and that aim spread is relative to this direction. (For example, a shotgun may
		/// fire ~8 pellets in a cone around this direction.) 
		/// </summary>
		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x0018FF05 File Offset: 0x0018E105
		// (set) Token: 0x060044D9 RID: 17625 RVA: 0x0018FF0D File Offset: 0x0018E10D
		public Vector3 ApproximatePlayerAimDirection { get; internal set; }

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060044DA RID: 17626 RVA: 0x0018FF16 File Offset: 0x0018E116
		// (set) Token: 0x060044DB RID: 17627 RVA: 0x0018FF1E File Offset: 0x0018E11E
		public Vector3 position { get; internal set; }

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060044DC RID: 17628 RVA: 0x0018FF27 File Offset: 0x0018E127
		// (set) Token: 0x060044DD RID: 17629 RVA: 0x0018FF2F File Offset: 0x0018E12F
		public Vector3 velocity { get; internal set; }

		// Token: 0x060044DE RID: 17630 RVA: 0x0018FF38 File Offset: 0x0018E138
		public Vector3 GetDirection()
		{
			return this.velocity.normalized;
		}

		/// <summary>
		/// Starting position when the bullet was fired.
		/// </summary>
		// Token: 0x04002E23 RID: 11811
		public Vector3 origin;

		// Token: 0x04002E27 RID: 11815
		public byte steps;

		// Token: 0x04002E28 RID: 11816
		public float quality;

		// Token: 0x04002E29 RID: 11817
		public byte pellet;

		// Token: 0x04002E2A RID: 11818
		public ushort dropID;

		// Token: 0x04002E2B RID: 11819
		public byte dropAmount;

		// Token: 0x04002E2C RID: 11820
		public byte dropQuality;

		// Token: 0x04002E2D RID: 11821
		public ItemBarrelAsset barrelAsset;

		// Token: 0x04002E2E RID: 11822
		public ItemMagazineAsset magazineAsset;
	}
}
