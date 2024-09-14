using System;
using System.Collections;
using System.Collections.Generic;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004FA RID: 1274
	public class LightningWeatherComponent : MonoBehaviour
	{
		// Token: 0x06002804 RID: 10244 RVA: 0x000A932B File Offset: 0x000A752B
		public NetId GetNetId()
		{
			return this.netId;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000A9333 File Offset: 0x000A7533
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveLightningStrike(Vector3 hitPosition)
		{
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000A9338 File Offset: 0x000A7538
		private void Update()
		{
			if (!Provider.isServer || this.weatherComponent == null)
			{
				return;
			}
			if (!this.weatherComponent.isFullyTransitionedIn)
			{
				return;
			}
			if (!this.hasInitializedTimer)
			{
				this.timer = Random.Range(this.weatherComponent.asset.minLightningInterval, this.weatherComponent.asset.maxLightningInterval);
				this.hasInitializedTimer = true;
				return;
			}
			this.timer -= Time.deltaTime;
			if (this.timer > 0f)
			{
				return;
			}
			this.hasInitializedTimer = false;
			this.playerPositions.Clear();
			foreach (Player player in this.weatherComponent.EnumerateMaskedPlayers())
			{
				if (player.life.IsAlive)
				{
					this.playerPositions.Add(player.transform.position);
				}
			}
			if (this.playerPositions.IsEmpty<Vector3>())
			{
				return;
			}
			int num = Random.Range(0, this.playerPositions.Count - 1);
			Vector3 vector = MathfEx.RandomPositionInCircleY(this.playerPositions[num], this.weatherComponent.asset.lightningTargetRadius);
			RaycastHit raycastHit;
			Vector3 vector2 = Physics.Raycast(new Vector3(vector.x, Level.HEIGHT, vector.z), Vector3.down, out raycastHit, Level.HEIGHT * 2f, 471449600) ? raycastHit.point : vector;
			LightningWeatherComponent.SendLightningStrike.Invoke(this.GetNetId(), ENetReliability.Reliable, Provider.GatherClientConnectionsWithinSphere(vector2, 600f), vector2);
			base.StartCoroutine(this.DoExplosionDamage(vector2));
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000A94F0 File Offset: 0x000A76F0
		private IEnumerator DoExplosionDamage(Vector3 hitPosition)
		{
			yield return new WaitForSeconds(1f);
			List<EPlayerKill> list;
			DamageTool.explode(new ExplosionParameters(hitPosition, 10f, EDeathCause.BURNING)
			{
				damageOrigin = EDamageOrigin.Lightning,
				playImpactEffect = false,
				playerDamage = 75f,
				zombieDamage = 200f,
				animalDamage = 200f,
				barricadeDamage = 100f,
				structureDamage = 100f,
				vehicleDamage = 200f,
				resourceDamage = 1000f,
				objectDamage = 1000f,
				launchSpeed = 50f
			}, out list);
			yield break;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000A94FF File Offset: 0x000A76FF
		private void Start()
		{
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000A9501 File Offset: 0x000A7701
		private void OnDestroy()
		{
			if (!this.netId.IsNull())
			{
				NetIdRegistry.Release(this.netId);
				this.netId.Clear();
			}
		}

		// Token: 0x04001530 RID: 5424
		public WeatherComponentBase weatherComponent;

		// Token: 0x04001531 RID: 5425
		private static ClientInstanceMethod<Vector3> SendLightningStrike = ClientInstanceMethod<Vector3>.Get(typeof(LightningWeatherComponent), "ReceiveLightningStrike");

		// Token: 0x04001532 RID: 5426
		private List<Vector3> playerPositions = new List<Vector3>();

		// Token: 0x04001533 RID: 5427
		internal NetId netId;

		// Token: 0x04001534 RID: 5428
		private float timer;

		// Token: 0x04001535 RID: 5429
		private bool hasInitializedTimer;

		// Token: 0x04001536 RID: 5430
		private static AssetReference<EffectAsset> LightningHitRef = new AssetReference<EffectAsset>("bed12ffc45694cd69217924d75e96fe9");
	}
}
