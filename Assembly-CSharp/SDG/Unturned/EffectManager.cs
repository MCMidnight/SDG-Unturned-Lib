using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000556 RID: 1366
	public class EffectManager : SteamCaller
	{
		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002B2D RID: 11053 RVA: 0x000B8388 File Offset: 0x000B6588
		public static EffectManager instance
		{
			get
			{
				return EffectManager.manager;
			}
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000B838F File Offset: 0x000B658F
		[Obsolete("Renamed to InstantiateFromPool to fix name conflict with Object.Instantiate")]
		public static GameObject Instantiate(GameObject element)
		{
			return EffectManager.InstantiateFromPool(element);
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000B8397 File Offset: 0x000B6597
		[Obsolete("Replaced with overload that takes an EffectAsset.")]
		public static GameObject InstantiateFromPool(GameObject element)
		{
			return Object.Instantiate<GameObject>(element);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000B83A0 File Offset: 0x000B65A0
		public static GameObject InstantiateFromPool(EffectAsset asset)
		{
			PoolReference poolReference = EffectManager.pool.Instantiate(asset.effect);
			poolReference.excludeFromDestroyAll = true;
			GameObject gameObject = poolReference.gameObject;
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Stop(true);
				component.Clear(true);
			}
			gameObject.tag = "Debris";
			gameObject.layer = 12;
			return gameObject;
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x000B83FA File Offset: 0x000B65FA
		[Obsolete("Renamed to DestroyIntoPool to fix name conflict with Object.Destroy")]
		public static void Destroy(GameObject element)
		{
			EffectManager.DestroyIntoPool(element);
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000B8402 File Offset: 0x000B6602
		public static void DestroyIntoPool(GameObject element)
		{
			if (element == null)
			{
				return;
			}
			EffectManager.pool.Destroy(element);
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000B8419 File Offset: 0x000B6619
		[Obsolete("Renamed to DestroyIntoPool to fix name conflict with Object.Destroy")]
		public static void Destroy(GameObject element, float t)
		{
			EffectManager.DestroyIntoPool(element, t);
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000B8422 File Offset: 0x000B6622
		public static void DestroyIntoPool(GameObject element, float t)
		{
			if (element == null)
			{
				return;
			}
			EffectManager.pool.Destroy(element, t);
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000B843A File Offset: 0x000B663A
		[Obsolete]
		public void tellEffectClearByID(CSteamID steamID, ushort id)
		{
			EffectManager.ReceiveEffectClearById(id);
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000B8444 File Offset: 0x000B6644
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellEffectClearByID")]
		public static void ReceiveEffectClearById(ushort id)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				EffectManager.ClearEffect(effectAsset);
			}
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000B8468 File Offset: 0x000B6668
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveEffectClearByGuid(Guid assetGuid)
		{
			EffectAsset effectAsset = Assets.find<EffectAsset>(assetGuid);
			if (effectAsset != null)
			{
				EffectManager.ClearEffect(effectAsset);
			}
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x000B8488 File Offset: 0x000B6688
		private static void ClearEffect(EffectAsset asset)
		{
			if (asset.effect != null)
			{
				EffectManager.pool.DestroyAllMatchingPrefab(asset.effect);
			}
			if (asset.splatter > 0)
			{
				foreach (GameObject prefab in asset.splatters)
				{
					EffectManager.pool.DestroyAllMatchingPrefab(prefab);
				}
			}
			for (int j = EffectManager.manager.uiEffectInstances.Count - 1; j >= 0; j--)
			{
				EffectManager.UIEffectInstance uieffectInstance = EffectManager.manager.uiEffectInstances[j];
				if (uieffectInstance.asset == asset)
				{
					if (uieffectInstance.gameObject != null)
					{
						Object.Destroy(uieffectInstance.gameObject);
					}
					EffectManager.manager.uiEffectInstances.RemoveAtFast(j);
				}
			}
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x000B8544 File Offset: 0x000B6744
		[Obsolete]
		public static void askEffectClearByID(ushort id, CSteamID steamID)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.askEffectClearByID(id, transportConnection);
			}
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000B8562 File Offset: 0x000B6762
		public static void askEffectClearByID(ushort id, ITransportConnection transportConnection)
		{
			ThreadUtil.assertIsGameThread();
			EffectManager.SendEffectClearById.Invoke(ENetReliability.Reliable, transportConnection, id);
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000B8576 File Offset: 0x000B6776
		public static void ClearEffectByID_AllPlayers(ushort id)
		{
			ThreadUtil.assertIsGameThread();
			EffectManager.SendEffectClearById.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), id);
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000B858E File Offset: 0x000B678E
		public static void ClearEffectByGuid(Guid assetGuid, ITransportConnection transportConnection)
		{
			ThreadUtil.assertIsGameThread();
			EffectManager.SendEffectClearByGuid.Invoke(ENetReliability.Reliable, transportConnection, assetGuid);
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000B85A2 File Offset: 0x000B67A2
		public static void ClearEffectByGuid_AllPlayers(Guid assetGuid)
		{
			ThreadUtil.assertIsGameThread();
			EffectManager.SendEffectClearByGuid.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), assetGuid);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000B85BA File Offset: 0x000B67BA
		[Obsolete]
		public void tellEffectClearAll(CSteamID steamID)
		{
			EffectManager.ReceiveEffectClearAll();
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000B85C1 File Offset: 0x000B67C1
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellEffectClearAll")]
		public static void ReceiveEffectClearAll()
		{
			EffectManager.pool.DestroyAll();
			EffectManager.manager.destroyAllDebris();
			EffectManager.manager.destroyAllUI();
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000B85E1 File Offset: 0x000B67E1
		public static void askEffectClearAll()
		{
			if (Provider.isServer)
			{
				EffectManager.SendEffectClearAll.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections());
			}
		}

		/// <summary>
		/// This effect makes a nice clicky sound and lots of older code used it,
		/// so I moved it into a little helper method here.
		/// </summary>
		// Token: 0x06002B41 RID: 11073 RVA: 0x000B85FC File Offset: 0x000B67FC
		internal static void TriggerFiremodeEffect(Vector3 position)
		{
			EffectAsset effectAsset = EffectManager.FiremodeRef.Find();
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					position = position,
					relevantDistance = EffectManager.SMALL
				});
			}
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000B863C File Offset: 0x000B683C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, byte x, byte y, byte area, Vector3 point, Vector3 normal)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = false;
				parameters.SetRelevantTransportConnections(Regions.GatherClientConnections(x, y, area));
				parameters.position = point;
				parameters.SetDirection(normal);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000B8690 File Offset: 0x000B6890
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, byte x, byte y, byte area, Vector3 point)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = false;
				parameters.SetRelevantTransportConnections(Regions.GatherClientConnections(x, y, area));
				parameters.position = point;
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000B86DC File Offset: 0x000B68DC
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, byte x, byte y, byte area, Vector3 point, Vector3 normal)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantTransportConnections(Regions.GatherClientConnections(x, y, area));
				parameters.position = point;
				parameters.SetDirection(normal);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000B8730 File Offset: 0x000B6930
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, byte x, byte y, byte area, Vector3 point)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantTransportConnections(Regions.GatherClientConnections(x, y, area));
				parameters.position = point;
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000B877C File Offset: 0x000B697C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, float radius, Vector3 point, Vector3 normal)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = false;
				parameters.relevantDistance = radius;
				parameters.position = point;
				parameters.SetDirection(normal);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000B87C8 File Offset: 0x000B69C8
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, float radius, Vector3 point)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					reliable = false,
					relevantDistance = radius,
					position = point
				});
			}
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000B880C File Offset: 0x000B6A0C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, float radius, Vector3 point, Vector3 normal)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.relevantDistance = radius;
				parameters.position = point;
				parameters.SetDirection(normal);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x000B8858 File Offset: 0x000B6A58
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, float radius, Vector3 point)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					reliable = true,
					relevantDistance = radius,
					position = point
				});
			}
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000B889C File Offset: 0x000B6A9C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, CSteamID steamID, Vector3 point, Vector3 normal)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffect(id, transportConnection, point, normal);
			}
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x000B88BC File Offset: 0x000B6ABC
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, ITransportConnection transportConnection, Vector3 point, Vector3 normal)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = false;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				parameters.SetDirection(normal);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000B8908 File Offset: 0x000B6B08
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, CSteamID steamID, Vector3 point)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffect(id, transportConnection, point);
			}
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x000B8928 File Offset: 0x000B6B28
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffect(ushort id, ITransportConnection transportConnection, Vector3 point)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = false;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x000B896C File Offset: 0x000B6B6C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, CSteamID steamID, Vector3 point, Vector3 normal)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffectReliable(id, transportConnection, point, normal);
			}
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x000B898C File Offset: 0x000B6B8C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, ITransportConnection transportConnection, Vector3 point, Vector3 normal)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				parameters.SetDirection(normal);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000B89D8 File Offset: 0x000B6BD8
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, CSteamID steamID, Vector3 point)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffectReliable(id, transportConnection, point);
			}
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000B89F8 File Offset: 0x000B6BF8
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, ITransportConnection transportConnection, Vector3 point)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000B8A3C File Offset: 0x000B6C3C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, CSteamID steamID, Vector3 point, Vector3 normal, float uniformScale)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffectReliable(id, transportConnection, point, normal, uniformScale);
			}
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000B8A60 File Offset: 0x000B6C60
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, ITransportConnection transportConnection, Vector3 point, Vector3 normal, float uniformScale)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				parameters.SetDirection(normal);
				parameters.SetUniformScale(uniformScale);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000B8AB4 File Offset: 0x000B6CB4
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable_NonUniformScale(ushort id, CSteamID steamID, Vector3 point, Vector3 normal, Vector3 scale)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffectReliable_NonUniformScale(id, transportConnection, point, normal, scale);
			}
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000B8AD8 File Offset: 0x000B6CD8
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable_NonUniformScale(ushort id, ITransportConnection transportConnection, Vector3 point, Vector3 normal, Vector3 scale)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				parameters.SetDirection(normal);
				parameters.scale = scale;
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000B8B2C File Offset: 0x000B6D2C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, CSteamID steamID, Vector3 point, float uniformScale)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffectReliable(id, transportConnection, point, uniformScale);
			}
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000B8B4C File Offset: 0x000B6D4C
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable(ushort id, ITransportConnection transportConnection, Vector3 point, float uniformScale)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				parameters.SetUniformScale(uniformScale);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000B8B98 File Offset: 0x000B6D98
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable_NonUniformScale(ushort id, CSteamID steamID, Vector3 point, Vector3 scale)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendEffectReliable_NonUniformScale(id, transportConnection, point, scale);
			}
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000B8BB8 File Offset: 0x000B6DB8
		[Obsolete("Please use TriggerEffectParameters with guid instead")]
		public static void sendEffectReliable_NonUniformScale(ushort id, ITransportConnection transportConnection, Vector3 point, Vector3 scale)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.reliable = true;
				parameters.SetRelevantPlayer(transportConnection);
				parameters.position = point;
				parameters.scale = scale;
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000B8C04 File Offset: 0x000B6E04
		public static void sendUIEffect(ushort id, short key, bool reliable)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect.Invoke(reliability, Provider.GatherClientConnections(), id, key);
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000B8C30 File Offset: 0x000B6E30
		public static void sendUIEffect(ushort id, short key, bool reliable, string arg0)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect1Arg.Invoke(reliability, Provider.GatherClientConnections(), id, key, arg0);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000B8C60 File Offset: 0x000B6E60
		public static void sendUIEffect(ushort id, short key, bool reliable, string arg0, string arg1)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect2Args.Invoke(reliability, Provider.GatherClientConnections(), id, key, arg0, arg1);
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000B8C90 File Offset: 0x000B6E90
		public static void sendUIEffect(ushort id, short key, bool reliable, string arg0, string arg1, string arg2)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect3Args.Invoke(reliability, Provider.GatherClientConnections(), id, key, arg0, arg1, arg2);
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000B8CC4 File Offset: 0x000B6EC4
		public static void sendUIEffect(ushort id, short key, bool reliable, string arg0, string arg1, string arg2, string arg3)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect4Args.Invoke(reliability, Provider.GatherClientConnections(), id, key, arg0, arg1, arg2, arg3);
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000B8CF8 File Offset: 0x000B6EF8
		[Obsolete]
		public static void sendUIEffect(ushort id, short key, CSteamID steamID, bool reliable)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffect(id, key, transportConnection, reliable);
			}
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000B8D18 File Offset: 0x000B6F18
		public static void sendUIEffect(ushort id, short key, ITransportConnection transportConnection, bool reliable)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect.Invoke(reliability, transportConnection, id, key);
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000B8D40 File Offset: 0x000B6F40
		[Obsolete]
		public static void sendUIEffect(ushort id, short key, CSteamID steamID, bool reliable, string arg0)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffect(id, key, transportConnection, reliable, arg0);
			}
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000B8D64 File Offset: 0x000B6F64
		public static void sendUIEffect(ushort id, short key, ITransportConnection transportConnection, bool reliable, string arg0)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect1Arg.Invoke(reliability, transportConnection, id, key, arg0);
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000B8D90 File Offset: 0x000B6F90
		[Obsolete]
		public static void sendUIEffect(ushort id, short key, CSteamID steamID, bool reliable, string arg0, string arg1)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffect(id, key, transportConnection, reliable, arg0, arg1);
			}
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000B8DB4 File Offset: 0x000B6FB4
		public static void sendUIEffect(ushort id, short key, ITransportConnection transportConnection, bool reliable, string arg0, string arg1)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect2Args.Invoke(reliability, transportConnection, id, key, arg0, arg1);
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000B8DE0 File Offset: 0x000B6FE0
		[Obsolete]
		public static void sendUIEffect(ushort id, short key, CSteamID steamID, bool reliable, string arg0, string arg1, string arg2)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffect(id, key, transportConnection, reliable, arg0, arg1, arg2);
			}
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000B8E08 File Offset: 0x000B7008
		public static void sendUIEffect(ushort id, short key, ITransportConnection transportConnection, bool reliable, string arg0, string arg1, string arg2)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect3Args.Invoke(reliability, transportConnection, id, key, arg0, arg1, arg2);
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000B8E38 File Offset: 0x000B7038
		[Obsolete]
		public static void sendUIEffect(ushort id, short key, CSteamID steamID, bool reliable, string arg0, string arg1, string arg2, string arg3)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffect(id, key, transportConnection, reliable, arg0, arg1, arg2, arg3);
			}
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x000B8E60 File Offset: 0x000B7060
		public static void sendUIEffect(ushort id, short key, ITransportConnection transportConnection, bool reliable, string arg0, string arg1, string arg2, string arg3)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffect4Args.Invoke(reliability, transportConnection, id, key, arg0, arg1, arg2, arg3);
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x000B8E90 File Offset: 0x000B7090
		[Obsolete]
		public static void sendUIEffectVisibility(short key, CSteamID steamID, bool reliable, string childNameOrPath, bool visible)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffectVisibility(key, transportConnection, reliable, childNameOrPath, visible);
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000B8EB4 File Offset: 0x000B70B4
		public static void sendUIEffectVisibility(short key, ITransportConnection transportConnection, bool reliable, string childNameOrPath, bool visible)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffectVisibility.Invoke(reliability, transportConnection, key, childNameOrPath, visible);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000B8EE0 File Offset: 0x000B70E0
		[Obsolete]
		public static void sendUIEffectText(short key, CSteamID steamID, bool reliable, string childNameOrPath, string text)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffectText(key, transportConnection, reliable, childNameOrPath, text);
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000B8F04 File Offset: 0x000B7104
		public static void sendUIEffectText(short key, ITransportConnection transportConnection, bool reliable, string childNameOrPath, string text)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffectText.Invoke(reliability, transportConnection, key, childNameOrPath, text);
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000B8F30 File Offset: 0x000B7130
		public static void sendUIEffectImageURL(short key, CSteamID steamID, bool reliable, string childNameOrPath, string url)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffectImageURL(key, transportConnection, reliable, childNameOrPath, url, true, false);
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000B8F54 File Offset: 0x000B7154
		public static void sendUIEffectImageURL(short key, CSteamID steamID, bool reliable, string childNameOrPath, string url, bool shouldCache = true, bool forceRefresh = false)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				EffectManager.sendUIEffectImageURL(key, transportConnection, reliable, childNameOrPath, url, shouldCache, forceRefresh);
			}
		}

		/// <param name="shouldCache">If true, client will download the image once and re-use it for subsequent calls.</param>
		/// <param name="forceRefresh">If true, client will destroy any cached copy of the image and re-acquire it.</param>
		// Token: 0x06002B6F RID: 11119 RVA: 0x000B8F7C File Offset: 0x000B717C
		public static void sendUIEffectImageURL(short key, ITransportConnection transportConnection, bool reliable, string childNameOrPath, string url, bool shouldCache = true, bool forceRefresh = false)
		{
			ThreadUtil.assertIsGameThread();
			ENetReliability reliability = reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			EffectManager.SendUIEffectImageURL.Invoke(reliability, transportConnection, key, childNameOrPath, url, shouldCache, forceRefresh);
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000B8FAA File Offset: 0x000B71AA
		[Obsolete]
		public void tellEffectPointNormal_NonUniformScale(CSteamID steamID, ushort id, Vector3 point, Vector3 normal, Vector3 scale)
		{
			EffectManager.effect(id, point, normal, scale);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000B8FB8 File Offset: 0x000B71B8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveEffectPointNormal_NonUniformScale(Guid assetGuid, Vector3 point, Vector3 normal, Vector3 scale)
		{
			EffectManager.effect(assetGuid, point, normal, scale);
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x000B8FC4 File Offset: 0x000B71C4
		[Obsolete]
		public void tellEffectPointNormal_UniformScale(CSteamID steamID, ushort id, Vector3 point, Vector3 normal, float uniformScale)
		{
			EffectManager.effect(id, point, normal, new Vector3(uniformScale, uniformScale, uniformScale));
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000B8FDB File Offset: 0x000B71DB
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveEffectPointNormal_UniformScale(Guid assetGuid, Vector3 point, Vector3 normal, float uniformScale)
		{
			EffectManager.effect(assetGuid, point, normal, new Vector3(uniformScale, uniformScale, uniformScale));
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000B8FEE File Offset: 0x000B71EE
		[Obsolete]
		public void tellEffectPointNormal(CSteamID steamID, ushort id, Vector3 point, Vector3 normal)
		{
			EffectManager.effect(id, point, normal);
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000B8FFA File Offset: 0x000B71FA
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveEffectPointNormal(Guid assetGuid, Vector3 point, Vector3 normal)
		{
			EffectManager.effect(assetGuid, point, normal);
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000B9005 File Offset: 0x000B7205
		[Obsolete]
		public void tellEffectPoint_NonUniformScale(CSteamID steamID, ushort id, Vector3 point, Vector3 scale)
		{
			EffectManager.effect(id, point, Vector3.up, scale);
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x000B9016 File Offset: 0x000B7216
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellEffectPoint_NonUniformScale")]
		public static void ReceiveEffectPoint_NonUniformScale(Guid assetGuid, Vector3 point, Vector3 scale)
		{
			EffectManager.effect(assetGuid, point, Vector3.up, scale);
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x000B9026 File Offset: 0x000B7226
		[Obsolete]
		public void tellEffectPoint_UniformScale(CSteamID steamID, ushort id, Vector3 point, float uniformScale)
		{
			EffectManager.effect(id, point, Vector3.up, new Vector3(uniformScale, uniformScale, uniformScale));
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000B9040 File Offset: 0x000B7240
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellEffectPoint_UniformScale")]
		public static void ReceiveEffectPoint_UniformScale(Guid assetGuid, Vector3 point, float uniformScale)
		{
			EffectManager.effect(assetGuid, point, Vector3.up, new Vector3(uniformScale, uniformScale, uniformScale));
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x000B9057 File Offset: 0x000B7257
		[Obsolete]
		public void tellEffectPoint(CSteamID steamID, ushort id, Vector3 point)
		{
			EffectManager.effect(id, point, Vector3.up);
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000B9066 File Offset: 0x000B7266
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellEffectPoint")]
		public static void ReceiveEffectPoint(Guid assetGuid, Vector3 point)
		{
			EffectManager.effect(assetGuid, point, Vector3.up);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x000B9078 File Offset: 0x000B7278
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveEffectPositionRotation_NonUniformScale(Guid assetGuid, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			EffectAsset effectAsset = Assets.find(assetGuid) as EffectAsset;
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(assetGuid, effectAsset, "TriggerEffect");
			}
			if (effectAsset != null)
			{
				EffectManager.internalSpawnEffect(effectAsset, position, rotation, scale, false, null);
			}
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x000B90B4 File Offset: 0x000B72B4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveEffectPositionRotation_UniformScale(Guid assetGuid, Vector3 position, Quaternion rotation, float uniformScale)
		{
			EffectAsset effectAsset = Assets.find(assetGuid) as EffectAsset;
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(assetGuid, effectAsset, "TriggerEffect");
			}
			if (effectAsset != null)
			{
				EffectManager.internalSpawnEffect(effectAsset, position, rotation, new Vector3(uniformScale, uniformScale, uniformScale), false, null);
			}
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x000B90F8 File Offset: 0x000B72F8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveEffectPositionRotation(Guid assetGuid, Vector3 position, Quaternion rotation)
		{
			EffectAsset effectAsset = Assets.find(assetGuid) as EffectAsset;
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(assetGuid, effectAsset, "TriggerEffect");
			}
			if (effectAsset != null)
			{
				EffectManager.internalSpawnEffect(effectAsset, position, rotation, Vector3.one, false, null);
			}
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000B9137 File Offset: 0x000B7337
		[Obsolete]
		public void tellUIEffect(CSteamID steamID, ushort id, short key)
		{
			EffectManager.ReceiveUIEffect(id, key);
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x000B9140 File Offset: 0x000B7340
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffect")]
		public static void ReceiveUIEffect(ushort id, short key)
		{
			EffectManager.createUIEffect(id, key);
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x000B914A File Offset: 0x000B734A
		[Obsolete]
		public void tellUIEffect1Arg(CSteamID steamID, ushort id, short key, string arg0)
		{
			EffectManager.ReceiveUIEffect1Arg(id, key, arg0);
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x000B9155 File Offset: 0x000B7355
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffect1Arg")]
		public static void ReceiveUIEffect1Arg(ushort id, short key, string arg0)
		{
			EffectManager.createAndFormatUIEffect(id, key, new object[]
			{
				arg0
			});
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x000B9169 File Offset: 0x000B7369
		[Obsolete]
		public void tellUIEffect2Args(CSteamID steamID, ushort id, short key, string arg0, string arg1)
		{
			EffectManager.ReceiveUIEffect2Args(id, key, arg0, arg1);
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000B9176 File Offset: 0x000B7376
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffect2Args")]
		public static void ReceiveUIEffect2Args(ushort id, short key, string arg0, string arg1)
		{
			EffectManager.createAndFormatUIEffect(id, key, new object[]
			{
				arg0,
				arg1
			});
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000B918E File Offset: 0x000B738E
		[Obsolete]
		public void tellUIEffect3Args(CSteamID steamID, ushort id, short key, string arg0, string arg1, string arg2)
		{
			EffectManager.ReceiveUIEffect3Args(id, key, arg0, arg1, arg2);
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x000B919D File Offset: 0x000B739D
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffect3Args")]
		public static void ReceiveUIEffect3Args(ushort id, short key, string arg0, string arg1, string arg2)
		{
			EffectManager.createAndFormatUIEffect(id, key, new object[]
			{
				arg0,
				arg1,
				arg2
			});
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000B91BA File Offset: 0x000B73BA
		[Obsolete]
		public void tellUIEffect4Args(CSteamID steamID, ushort id, short key, string arg0, string arg1, string arg2, string arg3)
		{
			EffectManager.ReceiveUIEffect4Args(id, key, arg0, arg1, arg2, arg3);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000B91CB File Offset: 0x000B73CB
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffect4Args")]
		public static void ReceiveUIEffect4Args(ushort id, short key, string arg0, string arg1, string arg2, string arg3)
		{
			EffectManager.createAndFormatUIEffect(id, key, new object[]
			{
				arg0,
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000B91ED File Offset: 0x000B73ED
		[Obsolete]
		public void tellUIEffectVisibility(CSteamID steamID, short key, string childNameOrPath, bool visible)
		{
			EffectManager.ReceiveUIEffectVisibility(key, childNameOrPath, visible);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000B91F8 File Offset: 0x000B73F8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffectVisibility")]
		public static void ReceiveUIEffectVisibility(short key, string childNameOrPath, bool visible)
		{
			GameObject gameObject;
			if (!EffectManager.indexedUIEffects.TryGetValue(key, ref gameObject))
			{
				UnturnedLog.info("tellUIEffectVisibility: key {0} not found (childNameOrPath {1})", new object[]
				{
					key,
					childNameOrPath
				});
				return;
			}
			if (gameObject == null)
			{
				UnturnedLog.info("tellUIEffectVisibility: key {0} was destroyed (childNameOrPath {1})", new object[]
				{
					key,
					childNameOrPath
				});
				return;
			}
			Transform transform = gameObject.transform.Find(childNameOrPath);
			if (transform == null)
			{
				transform = gameObject.transform.FindChildRecursive(childNameOrPath);
			}
			if (transform == null)
			{
				UnturnedLog.info("tellUIEffectVisibility: childNameOrPath \"{0}\" not found (key {1})", new object[]
				{
					childNameOrPath,
					key
				});
				return;
			}
			transform.gameObject.SetActive(visible);
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000B92AF File Offset: 0x000B74AF
		[Obsolete]
		public void tellUIEffectText(CSteamID steamID, short key, string childNameOrPath, string text)
		{
			EffectManager.ReceiveUIEffectText(key, childNameOrPath, text);
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000B92BC File Offset: 0x000B74BC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffectText")]
		public static void ReceiveUIEffectText(short key, string childNameOrPath, string text)
		{
			GameObject gameObject;
			if (!EffectManager.indexedUIEffects.TryGetValue(key, ref gameObject))
			{
				UnturnedLog.info("tellUIEffectText: key {0} not found (childNameOrPath {1} text {2})", new object[]
				{
					key,
					childNameOrPath,
					text
				});
				return;
			}
			if (gameObject == null)
			{
				UnturnedLog.info("tellUIEffectText: key {0} was destroyed (childNameOrPath {1} text {2})", new object[]
				{
					key,
					childNameOrPath,
					text
				});
				return;
			}
			Transform transform = gameObject.transform.Find(childNameOrPath);
			if (transform == null)
			{
				transform = gameObject.transform.FindChildRecursive(childNameOrPath);
			}
			if (transform == null)
			{
				UnturnedLog.info("tellUIEffectText: childNameOrPath \"{0}\" not found (key {1} text {2})", new object[]
				{
					childNameOrPath,
					key,
					text
				});
				return;
			}
			Text component = transform.GetComponent<Text>();
			if (component != null)
			{
				ControlsSettings.formatPluginHotkeysIntoText(ref text);
				component.text = text;
				return;
			}
			TextMeshProUGUI component2 = transform.GetComponent<TextMeshProUGUI>();
			if (component2 != null)
			{
				ControlsSettings.formatPluginHotkeysIntoText(ref text);
				component2.text = text;
				return;
			}
			InputField component3 = transform.GetComponent<InputField>();
			if (component3 != null)
			{
				component3.SetTextWithoutNotify(text);
				return;
			}
			TMP_InputField component4 = transform.GetComponent<TMP_InputField>();
			if (component4 != null)
			{
				component4.SetTextWithoutNotify(text);
				return;
			}
			UnturnedLog.info("tellUIEffectText: \"{0}\" does not have a text or input field component (key {1} text {2})", new object[]
			{
				childNameOrPath,
				key,
				text
			});
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000B9408 File Offset: 0x000B7608
		[Obsolete]
		public void tellUIEffectImageURL(CSteamID steamID, short key, string childNameOrPath, string url, bool shouldCache, bool forceRefresh)
		{
			EffectManager.ReceiveUIEffectImageURL(key, childNameOrPath, url, shouldCache, forceRefresh);
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000B9418 File Offset: 0x000B7618
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUIEffectImageURL")]
		public static void ReceiveUIEffectImageURL(short key, string childNameOrPath, string url, bool shouldCache, bool forceRefresh)
		{
			GameObject gameObject;
			if (!EffectManager.indexedUIEffects.TryGetValue(key, ref gameObject))
			{
				UnturnedLog.info("tellUIEffectImageURL: key {0} not found (childNameOrPath {1} url {2})", new object[]
				{
					key,
					childNameOrPath,
					url
				});
				return;
			}
			if (gameObject == null)
			{
				UnturnedLog.info("tellUIEffectImageURL: key {0} was destroyed (childNameOrPath {1} url {2})", new object[]
				{
					key,
					childNameOrPath,
					url
				});
				return;
			}
			Transform transform = gameObject.transform.Find(childNameOrPath);
			if (transform == null)
			{
				transform = gameObject.transform.FindChildRecursive(childNameOrPath);
			}
			if (transform == null)
			{
				UnturnedLog.info("tellUIEffectImageURL: childNameOrPath \"{0}\" not found (key {1} text {2})", new object[]
				{
					childNameOrPath,
					key,
					url
				});
				return;
			}
			Image component = transform.GetComponent<Image>();
			if (component == null)
			{
				UnturnedLog.info("tellUIEffectImageURL: \"{0}\" does not have an image component (key {1} url {2})", new object[]
				{
					childNameOrPath,
					key,
					url
				});
				return;
			}
			WebImage orAddComponent = transform.GetOrAddComponent<WebImage>();
			orAddComponent.targetImage = component;
			orAddComponent.setAddressAndRefresh(url, shouldCache, forceRefresh);
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000B9518 File Offset: 0x000B7718
		[Obsolete]
		public void tellEffectClicked(CSteamID steamID, string buttonName)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			EffectManager.ReceiveEffectClicked(serverInvocationContext, buttonName);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000B9534 File Offset: 0x000B7734
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 20, legacyName = "tellEffectClicked")]
		public static void ReceiveEffectClicked(in ServerInvocationContext context, string buttonName)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			EffectManager.EffectButtonClickedHandler effectButtonClickedHandler = EffectManager.onEffectButtonClicked;
			if (effectButtonClickedHandler == null)
			{
				return;
			}
			effectButtonClickedHandler(player, buttonName);
		}

		/// <summary>
		/// Notify server that a button was clicked in a clientside effect.
		/// </summary>
		// Token: 0x06002B91 RID: 11153 RVA: 0x000B9563 File Offset: 0x000B7763
		public static void sendEffectClicked(string buttonName)
		{
			EffectManager.SendEffectClicked.Invoke(ENetReliability.Reliable, buttonName);
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000B9574 File Offset: 0x000B7774
		[Obsolete]
		public void tellEffectTextCommitted(CSteamID steamID, string inputFieldName, string text)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			EffectManager.ReceiveEffectTextCommitted(serverInvocationContext, inputFieldName, text);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000B9594 File Offset: 0x000B7794
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 20, legacyName = "tellEffectTextCommitted")]
		public static void ReceiveEffectTextCommitted(in ServerInvocationContext context, string inputFieldName, string text)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			EffectManager.EffectTextCommittedHandler effectTextCommittedHandler = EffectManager.onEffectTextCommitted;
			if (effectTextCommittedHandler == null)
			{
				return;
			}
			effectTextCommittedHandler(player, inputFieldName, text);
		}

		/// <summary>
		/// Notify server that an input field text was committed.
		/// </summary>
		// Token: 0x06002B94 RID: 11156 RVA: 0x000B95C4 File Offset: 0x000B77C4
		public static void sendEffectTextCommitted(string inputFieldName, string text)
		{
			EffectManager.SendEffectTextCommitted.Invoke(ENetReliability.Reliable, inputFieldName, text);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000B95D4 File Offset: 0x000B77D4
		public static Transform createAndFormatUIEffect(ushort id, short key, params object[] args)
		{
			Transform transform = EffectManager.createUIEffect(id, key);
			if (transform != null)
			{
				EffectManager.formatTextIntoUIEffect(transform, args);
			}
			return transform;
		}

		/// <summary>
		/// If an effect with a given key exists, destroy it.
		/// </summary>
		// Token: 0x06002B96 RID: 11158 RVA: 0x000B95FC File Offset: 0x000B77FC
		private static void destroyUIEffect(short key)
		{
			GameObject gameObject;
			if (EffectManager.indexedUIEffects.TryGetValue(key, ref gameObject))
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
				EffectManager.indexedUIEffects.Remove(key);
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000B9634 File Offset: 0x000B7834
		public static Transform createUIEffect(ushort id, short key)
		{
			EffectManager.destroyUIEffect(key);
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset == null || effectAsset.effect == null)
			{
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(effectAsset.effect);
			Transform transform = gameObject.transform;
			transform.name = id.ToString();
			if (key == -1)
			{
				if (effectAsset.lifetime > 1E-45f)
				{
					Object.Destroy(transform.gameObject, effectAsset.lifetime + Random.Range(-effectAsset.lifetimeSpread, effectAsset.lifetimeSpread));
				}
			}
			else
			{
				EffectManager.indexedUIEffects.Add(key, transform.gameObject);
			}
			EffectManager.instance.uiEffectInstances.Add(new EffectManager.UIEffectInstance(effectAsset, gameObject));
			EffectManager.hookButtonsInUIEffect(transform);
			EffectManager.hookInputFieldsInUIEffect(transform);
			EffectManager.gatherFormattingForUIEffect(transform);
			EffectManager.formatPluginHotkeysIntoUIEffect(transform);
			return transform;
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000B9700 File Offset: 0x000B7900
		public static void gatherFormattingForUIEffect(Transform effect)
		{
			EffectManager.formattingComponents.Clear();
			EffectManager.tmpTexts.Clear();
			effect.GetComponentsInChildren<Text>(true, EffectManager.formattingComponents);
			if (EffectManager.formattingComponents.Count < 1)
			{
				effect.GetComponentsInChildren<TextMeshProUGUI>(true, EffectManager.tmpTexts);
				foreach (TextMeshProUGUI component in EffectManager.tmpTexts)
				{
					TextMeshProUtils.FixupFont(component);
				}
			}
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000B9788 File Offset: 0x000B7988
		public static void formatTextIntoUIEffect(Transform effect, params object[] args)
		{
			if (EffectManager.formattingComponents.Count > 0)
			{
				using (List<Text>.Enumerator enumerator = EffectManager.formattingComponents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Text text = enumerator.Current;
						text.text = string.Format(text.text, args);
					}
					return;
				}
			}
			foreach (TextMeshProUGUI textMeshProUGUI in EffectManager.tmpTexts)
			{
				textMeshProUGUI.text = string.Format(textMeshProUGUI.text, args);
			}
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000B983C File Offset: 0x000B7A3C
		public static void formatPluginHotkeysIntoUIEffect(Transform effect)
		{
			if (EffectManager.formattingComponents.Count > 0)
			{
				using (List<Text>.Enumerator enumerator = EffectManager.formattingComponents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Text text = enumerator.Current;
						string text2 = text.text;
						ControlsSettings.formatPluginHotkeysIntoText(ref text2);
						text.text = text2;
					}
					return;
				}
			}
			foreach (TextMeshProUGUI textMeshProUGUI in EffectManager.tmpTexts)
			{
				string text3 = textMeshProUGUI.text;
				ControlsSettings.formatPluginHotkeysIntoText(ref text3);
				textMeshProUGUI.text = text3;
			}
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000B98F4 File Offset: 0x000B7AF4
		public static void hookButtonsInUIEffect(Transform effect)
		{
			EffectManager.buttonComponents.Clear();
			effect.GetComponentsInChildren<Button>(true, EffectManager.buttonComponents);
			foreach (Button button in EffectManager.buttonComponents)
			{
				button.gameObject.AddComponent<PluginButtonListener>().targetButton = button;
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000B9968 File Offset: 0x000B7B68
		public static void hookInputFieldsInUIEffect(Transform effect)
		{
			EffectManager.inputFieldComponents.Clear();
			EffectManager.tmpInputFields.Clear();
			effect.GetComponentsInChildren<InputField>(true, EffectManager.inputFieldComponents);
			if (EffectManager.inputFieldComponents.Count > 0)
			{
				using (List<InputField>.Enumerator enumerator = EffectManager.inputFieldComponents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InputField inputField = enumerator.Current;
						inputField.gameObject.AddComponent<PluginInputFieldListener>().targetInputField = inputField;
					}
					return;
				}
			}
			effect.GetComponentsInChildren<TMP_InputField>(true, EffectManager.tmpInputFields);
			foreach (TMP_InputField tmp_InputField in EffectManager.tmpInputFields)
			{
				tmp_InputField.gameObject.AddComponent<TMP_PluginInputFieldListener>().targetInputField = tmp_InputField;
				TextMeshProUtils.FixupFont(tmp_InputField);
			}
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000B9A50 File Offset: 0x000B7C50
		public static Transform effect(ushort id, Vector3 point, Vector3 normal)
		{
			return EffectManager.effect(id, point, normal, Vector3.one);
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x000B9A5F File Offset: 0x000B7C5F
		public static Transform effect(Guid assetGuid, Vector3 point, Vector3 normal)
		{
			return EffectManager.effect(assetGuid, point, normal, Vector3.one);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000B9A6E File Offset: 0x000B7C6E
		public static Transform effect(EffectAsset asset, Vector3 point, Vector3 normal)
		{
			return EffectManager.effect(asset, point, normal, Vector3.one);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000B9A80 File Offset: 0x000B7C80
		public static Transform effect(ushort id, Vector3 point, Vector3 normal, Vector3 scaleMultiplier)
		{
			EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, id) as EffectAsset;
			if (effectAsset != null)
			{
				return EffectManager.internalSpawnEffect(effectAsset, point, normal, scaleMultiplier, false, null);
			}
			return null;
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000B9AAC File Offset: 0x000B7CAC
		public static Transform effect(Guid assetGuid, Vector3 point, Vector3 normal, Vector3 scaleMultiplier)
		{
			EffectAsset effectAsset = Assets.find(assetGuid) as EffectAsset;
			if (effectAsset != null)
			{
				return EffectManager.internalSpawnEffect(effectAsset, point, normal, scaleMultiplier, false, null);
			}
			return null;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000B9AD5 File Offset: 0x000B7CD5
		public static Transform effect(EffectAsset asset, Vector3 point, Vector3 normal, Vector3 scaleMultiplier)
		{
			if (asset != null)
			{
				return EffectManager.internalSpawnEffect(asset, point, normal, scaleMultiplier, false, null);
			}
			return null;
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000B9AE8 File Offset: 0x000B7CE8
		public static Transform effect(AssetReference<EffectAsset> assetRef, Vector3 position)
		{
			EffectAsset effectAsset = assetRef.Find();
			if (effectAsset != null)
			{
				return EffectManager.internalSpawnEffect(effectAsset, position, Vector3.up, Vector3.one, false, null);
			}
			return null;
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000B9B18 File Offset: 0x000B7D18
		internal static Transform internalSpawnEffect(EffectAsset asset, Vector3 point, Vector3 normal, Vector3 scaleMultiplier, bool wasInstigatedByPlayer, Transform parent)
		{
			Quaternion quaternion = Quaternion.LookRotation(normal);
			if (asset.randomizeRotation)
			{
				quaternion *= Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360));
			}
			return EffectManager.internalSpawnEffect(asset, point, quaternion, scaleMultiplier, wasInstigatedByPlayer, parent);
		}

		/// <summary>
		/// parent should only be set if that system also calls ClearAttachments, otherwise attachedEffects will leak memory.
		/// </summary>
		// Token: 0x06002BA5 RID: 11173 RVA: 0x000B9B64 File Offset: 0x000B7D64
		internal static Transform internalSpawnEffect(EffectAsset asset, Vector3 point, Quaternion rotation, Vector3 scaleMultiplier, bool wasInstigatedByPlayer, Transform parent)
		{
			if (parent != null && !parent.gameObject.activeInHierarchy)
			{
				return null;
			}
			if (asset.splatterTemperature != EPlayerTemperature.NONE && (Provider.isPvP || !wasInstigatedByPlayer))
			{
				Transform transform = new GameObject().transform;
				transform.name = "Temperature";
				EffectManager.RegisterDebris(transform.gameObject);
				transform.position = point + Vector3.down * -2f;
				transform.localScale = Vector3.one * 6f;
				transform.gameObject.SetActive(false);
				transform.gameObject.AddComponent<TemperatureTrigger>().temperature = asset.splatterTemperature;
				transform.gameObject.SetActive(true);
				Object.Destroy(transform.gameObject, asset.splatterLifetime - asset.splatterLifetimeSpread);
			}
			if (!asset.spawnOnDedicatedServer)
			{
				return null;
			}
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(asset);
			}
			if (EffectManager.pool == null)
			{
				return null;
			}
			Transform transform2 = EffectManager.pool.Instantiate(asset.effect, point, rotation).transform;
			transform2.localScale = scaleMultiplier;
			transform2.parent = parent;
			if (parent != null)
			{
				EffectManager.RegisterAttachment(transform2.gameObject);
			}
			if (asset.splatter > 0 && (!asset.gore || OptionsSettings.gore))
			{
				for (int i = 0; i < (int)(asset.splatter * ((!asset.splatterLiquid && Player.player != null && Player.player.skills.boost == EPlayerBoost.SPLATTERIFIC) ? 8 : 1)); i++)
				{
					RaycastHit raycastHit;
					if (asset.splatterLiquid)
					{
						float f = Random.Range(0f, 6.2831855f);
						float num = Random.Range(1f, 6f);
						Ray ray = new Ray(point + new Vector3(Mathf.Cos(f) * num, 0f, Mathf.Sin(f) * num), Vector3.down);
						int layerMask = 471433216;
						Physics.Raycast(ray, out raycastHit, 8f, layerMask);
					}
					else
					{
						Ray ray2 = new Ray(point, rotation * new Vector3(0f, 0f, -2f) + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
						int layerMask2 = 471433216;
						Physics.Raycast(ray2, out raycastHit, 8f, layerMask2);
					}
					if (raycastHit.transform != null)
					{
						Transform transform3 = EffectManager.pool.Instantiate(asset.splatters[Random.Range(0, asset.splatters.Length)], raycastHit.point + raycastHit.normal * Random.Range(0.04f, 0.06f), Quaternion.LookRotation(raycastHit.normal) * Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360))).transform;
						transform3.name = "Splatter";
						float num2 = Random.Range(1f, 2f);
						transform3.localScale = new Vector3(num2, num2, num2);
						transform3.parent = raycastHit.transform;
						EffectManager.RegisterAttachment(transform3.gameObject);
						EffectManager.RegisterDebris(transform3.gameObject);
						transform3.gameObject.SetActive(true);
						if (asset.splatterLifetime > 1E-45f)
						{
							EffectManager.pool.Destroy(transform3.gameObject, asset.splatterLifetime + Random.Range(-asset.splatterLifetimeSpread, asset.splatterLifetimeSpread));
						}
						else
						{
							EffectManager.pool.Destroy(transform3.gameObject, GraphicsSettings.effect);
						}
					}
				}
			}
			if (asset.gore)
			{
				transform2.GetComponent<ParticleSystem>().emission.enabled = OptionsSettings.gore;
			}
			if (!asset.isStatic && transform2.GetComponent<AudioSource>() != null)
			{
				transform2.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
			}
			if (asset.lifetime > 1E-45f)
			{
				EffectManager.pool.Destroy(transform2.gameObject, asset.lifetime + Random.Range(-asset.lifetimeSpread, asset.lifetimeSpread));
			}
			else
			{
				float num3 = 0f;
				if (transform2.GetComponent<MeshRenderer>() == null)
				{
					ParticleSystem component = transform2.GetComponent<ParticleSystem>();
					if (component != null)
					{
						if (component.main.loop)
						{
							num3 = component.main.startLifetime.constantMax;
						}
						else
						{
							num3 = component.main.duration + component.main.startLifetime.constantMax;
						}
					}
					AudioSource component2 = transform2.GetComponent<AudioSource>();
					if (component2 != null && component2.clip != null && component2.clip.length > num3)
					{
						num3 = component2.clip.length;
					}
				}
				if (num3 < 1E-45f)
				{
					num3 = GraphicsSettings.effect;
				}
				EffectManager.pool.Destroy(transform2.gameObject, num3);
			}
			if (GraphicsSettings.blast && GraphicsSettings.renderMode == ERenderMode.DEFERRED)
			{
				EffectAsset effectAsset = asset.FindBlastmarkEffectAsset();
				if (effectAsset != null)
				{
					EffectManager.effect(effectAsset, point, new Vector3(Random.Range(-0.1f, 0.1f), 1f, Random.Range(-0.1f, 0.1f)));
				}
			}
			return transform2;
		}

		/// <summary>
		/// Helper for sending and spawning effects.
		/// Newer and refactored code should use this method.
		/// </summary>
		// Token: 0x06002BA6 RID: 11174 RVA: 0x000BA0CC File Offset: 0x000B82CC
		public static void triggerEffect(TriggerEffectParameters parameters)
		{
			if (parameters.asset == null)
			{
				return;
			}
			bool flag = parameters.asset.splatterTemperature != EPlayerTemperature.NONE || parameters.asset.spawnOnDedicatedServer;
			Quaternion quaternion = parameters.GetRotation();
			if (parameters.asset.randomizeRotation)
			{
				quaternion *= Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360));
			}
			if (!parameters.shouldReplicate)
			{
				if (flag)
				{
					EffectManager.internalSpawnEffect(parameters.asset, parameters.position, quaternion, parameters.scale, parameters.wasInstigatedByPlayer, null);
				}
				return;
			}
			ENetReliability reliability = parameters.reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
			ITransportConnection transportConnection = parameters.relevantTransportConnection;
			if (parameters.relevantPlayerID != CSteamID.Nil)
			{
				transportConnection = Provider.findTransportConnection(parameters.relevantPlayerID);
			}
			if (transportConnection == null)
			{
				if (flag)
				{
					EffectManager.internalSpawnEffect(parameters.asset, parameters.position, quaternion, parameters.scale, parameters.wasInstigatedByPlayer, null);
				}
				PooledTransportConnectionList pooledTransportConnectionList = parameters.relevantTransportConnections;
				if (pooledTransportConnectionList == null)
				{
					float relevantDistance = parameters.relevantDistance;
					if (parameters.asset.relevantDistance > 0f)
					{
						relevantDistance = parameters.asset.relevantDistance;
					}
					pooledTransportConnectionList = Provider.GatherClientConnectionsWithinSphere(parameters.position, relevantDistance);
				}
				if (MathfEx.IsNearlyEqual(parameters.scale, Vector3.one, 0.001f))
				{
					EffectManager.SendEffectPositionRotation.Invoke(reliability, pooledTransportConnectionList, parameters.asset.GUID, parameters.position, quaternion);
					return;
				}
				if (parameters.scale.AreComponentsNearlyEqual(0.001f))
				{
					float x = parameters.scale.x;
					EffectManager.SendEffectPositionRotation_UniformScale.Invoke(reliability, pooledTransportConnectionList, parameters.asset.GUID, parameters.position, quaternion, x);
					return;
				}
				EffectManager.SendEffectPositionRotation_NonUniformScale.Invoke(reliability, pooledTransportConnectionList, parameters.asset.GUID, parameters.position, quaternion, parameters.scale);
				return;
			}
			else
			{
				if (MathfEx.IsNearlyEqual(parameters.scale, Vector3.one, 0.001f))
				{
					EffectManager.SendEffectPositionRotation.Invoke(reliability, transportConnection, parameters.asset.GUID, parameters.position, quaternion);
					return;
				}
				if (parameters.scale.AreComponentsNearlyEqual(0.001f))
				{
					float x2 = parameters.scale.x;
					EffectManager.SendEffectPositionRotation_UniformScale.Invoke(reliability, transportConnection, parameters.asset.GUID, parameters.position, quaternion, x2);
					return;
				}
				EffectManager.SendEffectPositionRotation_NonUniformScale.Invoke(reliability, transportConnection, parameters.asset.GUID, parameters.position, quaternion, parameters.scale);
				return;
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000BA337 File Offset: 0x000B8537
		public static void RegisterDebris(GameObject item)
		{
			if (EffectManager.instance != null)
			{
				EffectManager.instance.debrisGameObjects.Add(item);
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000BA358 File Offset: 0x000B8558
		private void destroyAllDebris()
		{
			foreach (GameObject gameObject in this.debrisGameObjects)
			{
				if (gameObject != null)
				{
					EffectManager.pool.Destroy(gameObject);
				}
			}
			this.debrisGameObjects.Clear();
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000BA3C4 File Offset: 0x000B85C4
		private void destroyAllUI()
		{
			foreach (EffectManager.UIEffectInstance uieffectInstance in this.uiEffectInstances)
			{
				if (uieffectInstance.gameObject != null)
				{
					Object.Destroy(uieffectInstance.gameObject);
				}
			}
			this.uiEffectInstances.Clear();
			EffectManager.indexedUIEffects.Clear();
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000BA440 File Offset: 0x000B8640
		private void onLevelLoaded(int level)
		{
			EffectManager.pool = new GameObjectPoolDictionary();
			EffectManager.indexedUIEffects = new Dictionary<short, GameObject>();
			EffectManager.attachedEffects = new Dictionary<Transform, List<GameObject>>();
			EffectManager.attachedEffectsListPool = new Stack<List<GameObject>>();
			this.debrisGameObjects.Clear();
			this.uiEffectInstances.Clear();
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x000BA48C File Offset: 0x000B868C
		private void Start()
		{
			EffectManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000BA4E0 File Offset: 0x000B86E0
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Effect pool assets: {0}", EffectManager.pool.pools.Count));
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<GameObject, GameObjectPool> keyValuePair in EffectManager.pool.pools)
			{
				num += keyValuePair.Value.pool.Count;
				num2 += keyValuePair.Value.active.Count;
			}
			results.Add(string.Format("Inactive pooled effects: {0}", num));
			results.Add(string.Format("Active pooled effects: {0}", num2));
			string text = "Effect debris: {0}";
			List<GameObject> list = this.debrisGameObjects;
			results.Add(string.Format(text, (list != null) ? new int?(list.Count) : default(int?)));
			results.Add(string.Format("Attached effect parents: {0}", EffectManager.attachedEffects.Count));
			int num3 = 0;
			foreach (KeyValuePair<Transform, List<GameObject>> keyValuePair2 in EffectManager.attachedEffects)
			{
				int num4 = num3;
				List<GameObject> value = keyValuePair2.Value;
				num3 = num4 + ((value != null) ? value.Count : 0);
			}
			results.Add(string.Format("Attached effect children: {0}", num3));
			results.Add(string.Format("Attached effect pool size: {0}", EffectManager.attachedEffectsListPool.Count));
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000BA68C File Offset: 0x000B888C
		internal static void ClearAttachments(Transform root)
		{
			List<GameObject> list;
			if (EffectManager.attachedEffects.TryGetValue(root, ref list))
			{
				EffectManager.attachedEffects.Remove(root);
				EffectManager.attachedEffectsListPool.Push(list);
				foreach (GameObject gameObject in list)
				{
					if (gameObject != null && gameObject.transform.root == root)
					{
						EffectManager.pool.Destroy(gameObject);
					}
				}
			}
		}

		/// <summary>
		/// Called prior to destroying effect (if attached) to free up attachments list.
		/// </summary>
		// Token: 0x06002BAE RID: 11182 RVA: 0x000BA720 File Offset: 0x000B8920
		internal static void UnregisterAttachment(GameObject effect)
		{
			Transform root = effect.transform.root;
			List<GameObject> list;
			if (EffectManager.attachedEffects.TryGetValue(root, ref list))
			{
				list.RemoveFast(effect);
				if (list.Count < 1)
				{
					EffectManager.attachedEffects.Remove(root);
					EffectManager.attachedEffectsListPool.Push(list);
				}
			}
		}

		/// <summary>
		/// Called after attaching effect so that it can be returned to pool when/if parent is destroyed.
		/// </summary>
		// Token: 0x06002BAF RID: 11183 RVA: 0x000BA770 File Offset: 0x000B8970
		private static void RegisterAttachment(GameObject effect)
		{
			Transform root = effect.transform.root;
			List<GameObject> list;
			if (!EffectManager.attachedEffects.TryGetValue(root, ref list))
			{
				if (EffectManager.attachedEffectsListPool.Count > 0)
				{
					list = EffectManager.attachedEffectsListPool.Pop();
					list.Clear();
				}
				else
				{
					list = new List<GameObject>(4);
				}
				EffectManager.attachedEffects.Add(root, list);
			}
			list.Add(effect);
		}

		// Token: 0x0400172A RID: 5930
		public static readonly float SMALL = 64f;

		// Token: 0x0400172B RID: 5931
		public static readonly float MEDIUM = 128f;

		// Token: 0x0400172C RID: 5932
		public static readonly float LARGE = 256f;

		// Token: 0x0400172D RID: 5933
		public static readonly float INSANE = 512f;

		// Token: 0x0400172E RID: 5934
		private static List<Text> formattingComponents = new List<Text>();

		// Token: 0x0400172F RID: 5935
		private static List<Button> buttonComponents = new List<Button>();

		// Token: 0x04001730 RID: 5936
		private static List<InputField> inputFieldComponents = new List<InputField>();

		/// <summary>
		/// TextMesh Pro uGUI text components.
		/// </summary>
		// Token: 0x04001731 RID: 5937
		private static List<TextMeshProUGUI> tmpTexts = new List<TextMeshProUGUI>();

		/// <summary>
		/// TextMesh Pro uGUI input field components.
		/// </summary>
		// Token: 0x04001732 RID: 5938
		private static List<TMP_InputField> tmpInputFields = new List<TMP_InputField>();

		// Token: 0x04001733 RID: 5939
		private static EffectManager manager;

		// Token: 0x04001734 RID: 5940
		private static GameObjectPoolDictionary pool;

		// Token: 0x04001735 RID: 5941
		private static Dictionary<short, GameObject> indexedUIEffects;

		// Token: 0x04001736 RID: 5942
		private static readonly ClientStaticMethod<ushort> SendEffectClearById = ClientStaticMethod<ushort>.Get(new ClientStaticMethod<ushort>.ReceiveDelegate(EffectManager.ReceiveEffectClearById));

		// Token: 0x04001737 RID: 5943
		private static readonly ClientStaticMethod<Guid> SendEffectClearByGuid = ClientStaticMethod<Guid>.Get(new ClientStaticMethod<Guid>.ReceiveDelegate(EffectManager.ReceiveEffectClearByGuid));

		// Token: 0x04001738 RID: 5944
		private static readonly ClientStaticMethod SendEffectClearAll = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegate(EffectManager.ReceiveEffectClearAll));

		// Token: 0x04001739 RID: 5945
		private static AssetReference<EffectAsset> FiremodeRef = new AssetReference<EffectAsset>("bc41e0feaebe4e788a3612811b8722d3");

		// Token: 0x0400173A RID: 5946
		private static readonly ClientStaticMethod<Guid, Vector3, Vector3, Vector3> SendEffectPointNormal_NonUniformScale = ClientStaticMethod<Guid, Vector3, Vector3, Vector3>.Get(new ClientStaticMethod<Guid, Vector3, Vector3, Vector3>.ReceiveDelegate(EffectManager.ReceiveEffectPointNormal_NonUniformScale));

		// Token: 0x0400173B RID: 5947
		private static readonly ClientStaticMethod<Guid, Vector3, Vector3, float> SendEffectPointNormal_UniformScale = ClientStaticMethod<Guid, Vector3, Vector3, float>.Get(new ClientStaticMethod<Guid, Vector3, Vector3, float>.ReceiveDelegate(EffectManager.ReceiveEffectPointNormal_UniformScale));

		// Token: 0x0400173C RID: 5948
		private static readonly ClientStaticMethod<Guid, Vector3, Vector3> SendEffectPointNormal = ClientStaticMethod<Guid, Vector3, Vector3>.Get(new ClientStaticMethod<Guid, Vector3, Vector3>.ReceiveDelegate(EffectManager.ReceiveEffectPointNormal));

		// Token: 0x0400173D RID: 5949
		private static readonly ClientStaticMethod<Guid, Vector3, Vector3> SendEffectPoint_NonUniformScale = ClientStaticMethod<Guid, Vector3, Vector3>.Get(new ClientStaticMethod<Guid, Vector3, Vector3>.ReceiveDelegate(EffectManager.ReceiveEffectPoint_NonUniformScale));

		// Token: 0x0400173E RID: 5950
		private static readonly ClientStaticMethod<Guid, Vector3, float> SendEffectPoint_UniformScale = ClientStaticMethod<Guid, Vector3, float>.Get(new ClientStaticMethod<Guid, Vector3, float>.ReceiveDelegate(EffectManager.ReceiveEffectPoint_UniformScale));

		// Token: 0x0400173F RID: 5951
		private static readonly ClientStaticMethod<Guid, Vector3> SendEffectPoint = ClientStaticMethod<Guid, Vector3>.Get(new ClientStaticMethod<Guid, Vector3>.ReceiveDelegate(EffectManager.ReceiveEffectPoint));

		// Token: 0x04001740 RID: 5952
		private static readonly ClientStaticMethod<Guid, Vector3, Quaternion, Vector3> SendEffectPositionRotation_NonUniformScale = ClientStaticMethod<Guid, Vector3, Quaternion, Vector3>.Get(new ClientStaticMethod<Guid, Vector3, Quaternion, Vector3>.ReceiveDelegate(EffectManager.ReceiveEffectPositionRotation_NonUniformScale));

		// Token: 0x04001741 RID: 5953
		private static readonly ClientStaticMethod<Guid, Vector3, Quaternion, float> SendEffectPositionRotation_UniformScale = ClientStaticMethod<Guid, Vector3, Quaternion, float>.Get(new ClientStaticMethod<Guid, Vector3, Quaternion, float>.ReceiveDelegate(EffectManager.ReceiveEffectPositionRotation_UniformScale));

		// Token: 0x04001742 RID: 5954
		private static readonly ClientStaticMethod<Guid, Vector3, Quaternion> SendEffectPositionRotation = ClientStaticMethod<Guid, Vector3, Quaternion>.Get(new ClientStaticMethod<Guid, Vector3, Quaternion>.ReceiveDelegate(EffectManager.ReceiveEffectPositionRotation));

		// Token: 0x04001743 RID: 5955
		private static readonly ClientStaticMethod<ushort, short> SendUIEffect = ClientStaticMethod<ushort, short>.Get(new ClientStaticMethod<ushort, short>.ReceiveDelegate(EffectManager.ReceiveUIEffect));

		// Token: 0x04001744 RID: 5956
		private static readonly ClientStaticMethod<ushort, short, string> SendUIEffect1Arg = ClientStaticMethod<ushort, short, string>.Get(new ClientStaticMethod<ushort, short, string>.ReceiveDelegate(EffectManager.ReceiveUIEffect1Arg));

		// Token: 0x04001745 RID: 5957
		private static readonly ClientStaticMethod<ushort, short, string, string> SendUIEffect2Args = ClientStaticMethod<ushort, short, string, string>.Get(new ClientStaticMethod<ushort, short, string, string>.ReceiveDelegate(EffectManager.ReceiveUIEffect2Args));

		// Token: 0x04001746 RID: 5958
		private static readonly ClientStaticMethod<ushort, short, string, string, string> SendUIEffect3Args = ClientStaticMethod<ushort, short, string, string, string>.Get(new ClientStaticMethod<ushort, short, string, string, string>.ReceiveDelegate(EffectManager.ReceiveUIEffect3Args));

		// Token: 0x04001747 RID: 5959
		private static readonly ClientStaticMethod<ushort, short, string, string, string, string> SendUIEffect4Args = ClientStaticMethod<ushort, short, string, string, string, string>.Get(new ClientStaticMethod<ushort, short, string, string, string, string>.ReceiveDelegate(EffectManager.ReceiveUIEffect4Args));

		// Token: 0x04001748 RID: 5960
		private static readonly ClientStaticMethod<short, string, bool> SendUIEffectVisibility = ClientStaticMethod<short, string, bool>.Get(new ClientStaticMethod<short, string, bool>.ReceiveDelegate(EffectManager.ReceiveUIEffectVisibility));

		// Token: 0x04001749 RID: 5961
		private static readonly ClientStaticMethod<short, string, string> SendUIEffectText = ClientStaticMethod<short, string, string>.Get(new ClientStaticMethod<short, string, string>.ReceiveDelegate(EffectManager.ReceiveUIEffectText));

		// Token: 0x0400174A RID: 5962
		private static readonly ClientStaticMethod<short, string, string, bool, bool> SendUIEffectImageURL = ClientStaticMethod<short, string, string, bool, bool>.Get(new ClientStaticMethod<short, string, string, bool, bool>.ReceiveDelegate(EffectManager.ReceiveUIEffectImageURL));

		// Token: 0x0400174B RID: 5963
		public static EffectManager.EffectButtonClickedHandler onEffectButtonClicked;

		// Token: 0x0400174C RID: 5964
		private static readonly ServerStaticMethod<string> SendEffectClicked = ServerStaticMethod<string>.Get(new ServerStaticMethod<string>.ReceiveDelegateWithContext(EffectManager.ReceiveEffectClicked));

		// Token: 0x0400174D RID: 5965
		public static EffectManager.EffectTextCommittedHandler onEffectTextCommitted;

		// Token: 0x0400174E RID: 5966
		private static readonly ServerStaticMethod<string, string> SendEffectTextCommitted = ServerStaticMethod<string, string>.Get(new ServerStaticMethod<string, string>.ReceiveDelegateWithContext(EffectManager.ReceiveEffectTextCommitted));

		/// <summary>
		/// Objects registered so that they can be destroyed all at once if needed.
		/// May be null if they were destroyed with a timer.
		/// </summary>
		// Token: 0x0400174F RID: 5967
		private List<GameObject> debrisGameObjects = new List<GameObject>();

		/// <summary>
		/// Plugin UIs spawned by the server.
		/// </summary>
		// Token: 0x04001750 RID: 5968
		private List<EffectManager.UIEffectInstance> uiEffectInstances = new List<EffectManager.UIEffectInstance>();

		/// <summary>
		/// Maps root transform to any attached effects.
		/// This allows us to detach effects when returning a barricade/structure to their pool.
		/// </summary>
		// Token: 0x04001751 RID: 5969
		private static Dictionary<Transform, List<GameObject>> attachedEffects;

		/// <summary>
		/// Recycled lists for attachedEffects dictionary.
		/// </summary>
		// Token: 0x04001752 RID: 5970
		private static Stack<List<GameObject>> attachedEffectsListPool;

		// Token: 0x02000972 RID: 2418
		// (Invoke) Token: 0x06004B65 RID: 19301
		public delegate void EffectButtonClickedHandler(Player player, string buttonName);

		// Token: 0x02000973 RID: 2419
		// (Invoke) Token: 0x06004B69 RID: 19305
		public delegate void EffectTextCommittedHandler(Player player, string buttonName, string text);

		// Token: 0x02000974 RID: 2420
		private struct UIEffectInstance
		{
			// Token: 0x06004B6C RID: 19308 RVA: 0x001B4738 File Offset: 0x001B2938
			public UIEffectInstance(EffectAsset asset, GameObject gameObject)
			{
				this.asset = asset;
				this.gameObject = gameObject;
			}

			// Token: 0x0400336F RID: 13167
			public EffectAsset asset;

			// Token: 0x04003370 RID: 13168
			public GameObject gameObject;
		}
	}
}
