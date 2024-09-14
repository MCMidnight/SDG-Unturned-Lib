using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200073C RID: 1852
	public class Boulder : MonoBehaviour
	{
		// Token: 0x06003CD8 RID: 15576 RVA: 0x00121930 File Offset: 0x0011FB30
		private void OnTriggerEnter(Collider other)
		{
			if (this.isExploded)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (other.transform.CompareTag("Agent"))
			{
				return;
			}
			this.isExploded = true;
			Vector3 normalized = (base.transform.position - this.lastPos).normalized;
			if (Provider.isServer)
			{
				float num = Mathf.Clamp(base.transform.parent.GetComponent<Rigidbody>().velocity.magnitude, 0f, 20f);
				if (num < 3f)
				{
					return;
				}
				if (other.transform.CompareTag("Player"))
				{
					Player player = DamageTool.getPlayer(other.transform);
					if (player != null)
					{
						EPlayerKill eplayerKill;
						DamageTool.damage(player, EDeathCause.BOULDER, ELimb.SPINE, CSteamID.Nil, normalized, Boulder.DAMAGE_PLAYER, num, out eplayerKill, true, false, ERagdollEffect.NONE);
						return;
					}
				}
				else if (other.transform.CompareTag("Vehicle"))
				{
					if (Provider.modeConfigData.Zombies.Can_Target_Vehicles)
					{
						InteractableVehicle component = other.transform.GetComponent<InteractableVehicle>();
						if (component != null && component.asset != null && component.asset.isVulnerableToEnvironment)
						{
							VehicleManager.damage(component, Boulder.DAMAGE_VEHICLE, num, true, default(CSteamID), EDamageOrigin.Mega_Zombie_Boulder);
							return;
						}
					}
				}
				else if (other.transform.CompareTag("Barricade"))
				{
					if (Provider.modeConfigData.Zombies.Can_Target_Barricades)
					{
						Transform barricadeRootTransform = DamageTool.getBarricadeRootTransform(other.transform);
						if (barricadeRootTransform != null)
						{
							BarricadeManager.damage(barricadeRootTransform, Boulder.DAMAGE_BARRICADE, num, true, default(CSteamID), EDamageOrigin.Mega_Zombie_Boulder);
							return;
						}
					}
				}
				else if (other.transform.CompareTag("Structure"))
				{
					if (Provider.modeConfigData.Zombies.Can_Target_Structures)
					{
						Transform structureRootTransform = DamageTool.getStructureRootTransform(other.transform);
						if (structureRootTransform != null)
						{
							StructureManager.damage(structureRootTransform, normalized, Boulder.DAMAGE_STRUCTURE, num, true, default(CSteamID), EDamageOrigin.Mega_Zombie_Boulder);
							return;
						}
					}
				}
				else if (other.transform.CompareTag("Resource"))
				{
					Transform resourceRootTransform = DamageTool.getResourceRootTransform(other.transform);
					if (resourceRootTransform != null)
					{
						EPlayerKill eplayerKill2;
						uint num2;
						ResourceManager.damage(resourceRootTransform, normalized, Boulder.DAMAGE_RESOURCE, num, 1f, out eplayerKill2, out num2, default(CSteamID), EDamageOrigin.Mega_Zombie_Boulder, true);
						return;
					}
				}
				else
				{
					InteractableObjectRubble componentInParent = other.transform.GetComponentInParent<InteractableObjectRubble>();
					if (componentInParent != null)
					{
						EPlayerKill eplayerKill3;
						uint num3;
						DamageTool.damage(componentInParent.transform, normalized, componentInParent.getSection(other.transform), Boulder.DAMAGE_OBJECT, num, out eplayerKill3, out num3, default(CSteamID), EDamageOrigin.Mega_Zombie_Boulder);
					}
				}
			}
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x00121BCD File Offset: 0x0011FDCD
		private void FixedUpdate()
		{
			this.lastPos = base.transform.position;
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x00121BE0 File Offset: 0x0011FDE0
		private void Awake()
		{
			this.lastPos = base.transform.position;
		}

		// Token: 0x04002620 RID: 9760
		private static readonly float DAMAGE_PLAYER = 3f;

		// Token: 0x04002621 RID: 9761
		private static readonly float DAMAGE_BARRICADE = 15f;

		// Token: 0x04002622 RID: 9762
		private static readonly float DAMAGE_STRUCTURE = 15f;

		// Token: 0x04002623 RID: 9763
		private static readonly float DAMAGE_OBJECT = 25f;

		// Token: 0x04002624 RID: 9764
		private static readonly float DAMAGE_VEHICLE = 10f;

		// Token: 0x04002625 RID: 9765
		private static readonly float DAMAGE_RESOURCE = 25f;

		// Token: 0x04002626 RID: 9766
		private bool isExploded;

		// Token: 0x04002627 RID: 9767
		private Vector3 lastPos;

		// Token: 0x04002628 RID: 9768
		internal static AssetReference<EffectAsset> Metal_2_Ref = new AssetReference<EffectAsset>("b7d53965bc6545c28e029175af35de30");
	}
}
