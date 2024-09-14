using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000323 RID: 803
	public class NPCEffectReward : INPCReward
	{
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x00058A07 File Offset: 0x00056C07
		// (set) Token: 0x06001835 RID: 6197 RVA: 0x00058A0F File Offset: 0x00056C0F
		public AssetReference<EffectAsset> AssetRef { get; protected set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x00058A18 File Offset: 0x00056C18
		// (set) Token: 0x06001837 RID: 6199 RVA: 0x00058A20 File Offset: 0x00056C20
		public string Spawnpoint { get; protected set; }

		// Token: 0x06001838 RID: 6200 RVA: 0x00058A2C File Offset: 0x00056C2C
		public override void GrantReward(Player player)
		{
			Spawnpoint spawnpoint = SpawnpointSystemV2.Get().FindSpawnpoint(this.Spawnpoint);
			if (spawnpoint != null)
			{
				Vector3 position = spawnpoint.transform.position;
				Quaternion rotation = spawnpoint.transform.rotation;
				TriggerEffectParameters parameters = new TriggerEffectParameters(this.AssetRef);
				parameters.shouldReplicate = true;
				parameters.reliable = this.IsReliable;
				if (this.OverrideRelevantDistance > 0.01f)
				{
					parameters.relevantDistance = this.OverrideRelevantDistance;
				}
				parameters.position = position;
				parameters.SetRotation(rotation);
				EffectManager.triggerEffect(parameters);
				return;
			}
			UnturnedLog.error("Failed to find NPC effect reward spawnpoint: " + this.Spawnpoint);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00058AD5 File Offset: 0x00056CD5
		public NPCEffectReward(AssetReference<EffectAsset> newAssetRef, string newSpawnpoint, bool newIsReliable, float newRelevantDistance, string newText) : base(newText)
		{
			this.AssetRef = newAssetRef;
			this.Spawnpoint = newSpawnpoint;
			this.IsReliable = newIsReliable;
			this.OverrideRelevantDistance = newRelevantDistance;
		}

		/// <summary>
		/// Should the RPC be called in reliable mode? Unreliable effects might not be received.
		/// </summary>
		// Token: 0x04000AEE RID: 2798
		public bool IsReliable;

		/// <summary>
		/// Applied if greater than zero. Defaults to 128.
		/// </summary>
		// Token: 0x04000AEF RID: 2799
		public float OverrideRelevantDistance;
	}
}
