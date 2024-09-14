using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200073D RID: 1853
	public class Carepackage : MonoBehaviour
	{
		/// <summary>
		/// Kill any players inside the spawned interactable box.
		/// Uses hardcoded size of 4 x 4 x 4.
		/// </summary>
		// Token: 0x06003CDD RID: 15581 RVA: 0x00121C54 File Offset: 0x0011FE54
		private void squishPlayersUnderBox(Transform barricade)
		{
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer != null && !(steamPlayer.player == null) && !(steamPlayer.player.life == null))
				{
					Vector3 vector = barricade.InverseTransformPoint(steamPlayer.model.position);
					if (Mathf.Abs(vector.x) < 2f && Mathf.Abs(vector.y) < 2f && Mathf.Abs(vector.z) < 2f)
					{
						EPlayerKill eplayerKill;
						DamageTool.damagePlayer(new DamagePlayerParameters(steamPlayer.player)
						{
							damage = 101f,
							applyGlobalArmorMultiplier = false
						}, out eplayerKill);
					}
				}
			}
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00121D40 File Offset: 0x0011FF40
		private void OnCollisionEnter(Collision collision)
		{
			if (this.isExploded)
			{
				return;
			}
			if (collision.collider.isTrigger)
			{
				return;
			}
			this.isExploded = true;
			if (Provider.isServer)
			{
				Vector3 position = base.transform.position;
				ItemBarricadeAsset itemBarricadeAsset = this.barricadeAsset;
				if (itemBarricadeAsset == null)
				{
					itemBarricadeAsset = (Assets.find(EAssetType.ITEM, this.barricadeID) as ItemBarricadeAsset);
				}
				Transform transform = BarricadeManager.dropBarricade(new Barricade(itemBarricadeAsset), null, base.transform.position, 0f, 0f, 0f, 0UL, 0UL);
				if (transform != null)
				{
					this.squishPlayersUnderBox(transform);
					InteractableStorage component = transform.GetComponent<InteractableStorage>();
					component.despawnWhenDestroyed = true;
					if (component != null && component.items != null)
					{
						int i = 0;
						while (i < 8)
						{
							ushort num = SpawnTableTool.ResolveLegacyId(this.id, EAssetType.ITEM, new Func<string>(this.OnGetSpawnTableErrorContext));
							if (num == 0)
							{
								break;
							}
							if (!component.items.tryAddItem(new Item(num, EItemOrigin.ADMIN), false))
							{
								i++;
							}
						}
						component.items.onStateUpdated();
					}
					transform.gameObject.AddComponent<CarepackageDestroy>();
					Transform transform2 = transform.Find("Flare");
					if (transform2 != null)
					{
						position = transform2.position;
					}
				}
				EffectAsset effectAsset = Assets.find<EffectAsset>(new AssetReference<EffectAsset>(this.landedEffectGuid));
				if (effectAsset != null)
				{
					EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
					{
						position = position,
						reliable = true,
						relevantDistance = EffectManager.INSANE
					});
				}
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x00121EC9 File Offset: 0x001200C9
		private string OnGetSpawnTableErrorContext()
		{
			return "airdrop care package";
		}

		/// <summary>
		/// Item ID of barricade to spawn after landing.
		/// </summary>
		// Token: 0x04002629 RID: 9769
		[Obsolete]
		public ushort barricadeID = 1374;

		/// <summary>
		/// Barricade to spawn after landing.
		/// </summary>
		// Token: 0x0400262A RID: 9770
		public ItemBarricadeAsset barricadeAsset;

		// Token: 0x0400262B RID: 9771
		public ushort id;

		// Token: 0x0400262C RID: 9772
		public string landedEffectGuid = "2c17fbd0f0ce49aeb3bc4637b68809a2";

		// Token: 0x0400262D RID: 9773
		private bool isExploded;
	}
}
