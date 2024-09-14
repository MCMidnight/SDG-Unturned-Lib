using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Payload for the EffectManager.triggerEffect method.
	/// </summary>
	// Token: 0x020005A0 RID: 1440
	public struct TriggerEffectParameters
	{
		/// <summary>
		/// Get world-space rotation for the effect.
		/// </summary>
		// Token: 0x06002E0F RID: 11791 RVA: 0x000C8E34 File Offset: 0x000C7034
		public Quaternion GetRotation()
		{
			if (!this.wasRotationSet)
			{
				return Quaternion.LookRotation(this.direction);
			}
			return this.rotation;
		}

		/// <summary>
		/// Set world-space rotation for the effect.
		/// </summary>
		// Token: 0x06002E10 RID: 11792 RVA: 0x000C8E50 File Offset: 0x000C7050
		public void SetRotation(Quaternion rotation)
		{
			this.rotation = rotation;
			this.wasRotationSet = true;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000C8E60 File Offset: 0x000C7060
		public Vector3 GetDirection()
		{
			if (!this.wasRotationSet)
			{
				return this.direction;
			}
			return this.rotation * Vector3.forward;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000C8E81 File Offset: 0x000C7081
		public void SetDirection(Vector3 forward)
		{
			this.direction = forward;
			this.rotation = Quaternion.LookRotation(forward);
			this.wasRotationSet = true;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000C8E9D File Offset: 0x000C709D
		public void SetDirection(Vector3 forward, Vector3 upwards)
		{
			this.direction = forward;
			this.rotation = Quaternion.LookRotation(forward, upwards);
			this.wasRotationSet = true;
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000C8EBA File Offset: 0x000C70BA
		public void SetUniformScale(float scale)
		{
			this.scale = new Vector3(scale, scale, scale);
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000C8ECA File Offset: 0x000C70CA
		public void SetRelevantPlayer(SteamPlayer player)
		{
			this.relevantTransportConnection = ((player != null) ? player.transportConnection : null);
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000C8EDE File Offset: 0x000C70DE
		public void SetRelevantPlayer(ITransportConnection transportConnection)
		{
			this.relevantTransportConnection = transportConnection;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000C8EE7 File Offset: 0x000C70E7
		public void SetRelevantTransportConnections(PooledTransportConnectionList transportConnections)
		{
			this.relevantTransportConnections = transportConnections;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000C8EF0 File Offset: 0x000C70F0
		[Obsolete("Replaced by the List overload")]
		public void SetRelevantTransportConnections(IEnumerable<ITransportConnection> transportConnections)
		{
			this.relevantTransportConnections = TransportConnectionListPool.Get();
			foreach (ITransportConnection transportConnection in transportConnections)
			{
				this.relevantTransportConnections.Add(transportConnection);
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000C8F48 File Offset: 0x000C7148
		public TriggerEffectParameters(EffectAsset asset)
		{
			this.asset = asset;
			this.position = Vector3.zero;
			this.direction = Vector3.up;
			this.scale = Vector3.one;
			this.shouldReplicate = true;
			this.reliable = false;
			this.wasInstigatedByPlayer = false;
			this.relevantDistance = 128f;
			this.relevantPlayerID = CSteamID.Nil;
			this.rotation = Quaternion.identity;
			this.wasRotationSet = false;
			this.relevantTransportConnection = null;
			this.relevantTransportConnections = null;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000C8FC8 File Offset: 0x000C71C8
		[Obsolete("Please find asset by GUID")]
		public TriggerEffectParameters(ushort id)
		{
			this = new TriggerEffectParameters(Assets.find(EAssetType.EFFECT, id) as EffectAsset);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000C8FDC File Offset: 0x000C71DC
		public TriggerEffectParameters(Guid assetGuid)
		{
			this = new TriggerEffectParameters(Assets.find<EffectAsset>(assetGuid));
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000C8FEA File Offset: 0x000C71EA
		public TriggerEffectParameters(AssetReference<EffectAsset> assetRef)
		{
			this = new TriggerEffectParameters(assetRef.Find());
		}

		/// <summary>
		/// Required effect to spawn.
		/// </summary>
		// Token: 0x040018D4 RID: 6356
		public EffectAsset asset;

		/// <summary>
		/// World-space position to spawn at.
		/// </summary>
		// Token: 0x040018D5 RID: 6357
		public Vector3 position;

		/// <summary>
		/// Local-space scale. Defaults to one.
		/// </summary>
		// Token: 0x040018D6 RID: 6358
		public Vector3 scale;

		/// <summary>
		/// If running as server should this effect be replicated to clients?
		/// Defaults to true. Set to false for code that is called on client AND server.
		/// </summary>
		// Token: 0x040018D7 RID: 6359
		public bool shouldReplicate;

		/// <summary>
		/// Should the RPC be called in reliable mode? Unreliable effects might not be received.
		/// </summary>
		// Token: 0x040018D8 RID: 6360
		public bool reliable;

		/// <summary>
		/// Was a player directly responsible for triggering this effect?
		/// For example grenade explosions are instigated by players, whereas zombie acid explosions are not.
		/// Used to prevent mod damage on the effect prefab from hurting players on PvE servers.
		/// </summary>
		// Token: 0x040018D9 RID: 6361
		public bool wasInstigatedByPlayer;

		/// <summary>
		/// Players within this radius will be sent the effect unless the effect overrides it.
		/// Defaults to 128.
		/// </summary>
		// Token: 0x040018DA RID: 6362
		public float relevantDistance;

		/// <summary>
		/// World-space rotation for the effect.
		/// </summary>
		// Token: 0x040018DB RID: 6363
		private Quaternion rotation;

		/// <summary>
		/// If true, rotation was specified by setter methods.
		/// Required for backwards compatibility because `direction` field is public.
		/// </summary>
		// Token: 0x040018DC RID: 6364
		private bool wasRotationSet;

		/// <summary>
		/// Only send the effect to the given player, if set.
		/// </summary>
		// Token: 0x040018DD RID: 6365
		internal ITransportConnection relevantTransportConnection;

		/// <summary>
		/// Only send the effect to the given players, if set.
		/// Otherwise relevantDistance is used.
		/// </summary>
		// Token: 0x040018DE RID: 6366
		internal PooledTransportConnectionList relevantTransportConnections;

		/// <summary>
		/// Only send the effect to the given player, if set.
		/// </summary>
		// Token: 0x040018DF RID: 6367
		[Obsolete("Please use SetRelevantPlayer instead! This field will be removed.")]
		public CSteamID relevantPlayerID;

		/// <summary>
		/// World-space direction to orient the Z axis along. Defaults to up.
		/// </summary>
		// Token: 0x040018E0 RID: 6368
		[Obsolete("Please use GetDirection and SetDirection instead now that rotation quaternion is supported. This field will be removed.")]
		public Vector3 direction;
	}
}
