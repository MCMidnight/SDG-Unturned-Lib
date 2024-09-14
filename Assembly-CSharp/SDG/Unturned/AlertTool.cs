using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000747 RID: 1863
	public class AlertTool
	{
		// Token: 0x06003CFB RID: 15611 RVA: 0x001225C8 File Offset: 0x001207C8
		private static bool check(Vector3 forward, Vector3 offset, float sqrRadius, bool sneak, Vector3 spotlightDir, bool isSpotlightOn, bool isLightSensitive)
		{
			return (isSpotlightOn && offset.sqrMagnitude < 576f && Vector3.Dot(spotlightDir, offset.normalized) > (isLightSensitive ? 0.4f : 0.75f)) || (offset.sqrMagnitude <= sqrRadius && ((double)Vector3.Dot(forward, offset.normalized) <= 0.5 || !sneak));
		}

		/// <summary>
		/// Alerts any agents in the area to the player if needed.
		/// </summary>
		/// <param name="player">The player causing this alert.</param>
		/// <param name="position">The position of the alert.</param>
		/// <param name="radius">The detection radius.</param>
		/// <param name="sneak">Whether or not to hide.</param>
		// Token: 0x06003CFC RID: 15612 RVA: 0x00122638 File Offset: 0x00120838
		public static void alert(Player player, Vector3 position, float radius, bool sneak, Vector3 spotDir, bool isSpotOn)
		{
			LevelAsset asset = Level.getAsset();
			float num = (asset != null) ? asset.minStealthRadius : 0f;
			float min = Mathf.Max(1f, radius);
			radius *= Provider.modeConfigData.Players.Detect_Radius_Multiplier;
			radius = Mathf.Clamp(radius, min, 64f);
			if (player == null)
			{
				return;
			}
			float sqrRadius = radius * radius;
			if (player.movement.nav != 255)
			{
				ZombieRegion zombieRegion = ZombieManager.regions[(int)player.movement.nav];
				if (zombieRegion.HasInfiniteAgroRange)
				{
					for (int i = 0; i < zombieRegion.zombies.Count; i++)
					{
						Zombie zombie = zombieRegion.zombies[i];
						if (!zombie.isDead && zombie.checkAlert(player))
						{
							zombie.alert(player);
						}
					}
				}
				AlertTool.zombiesInRadius.Clear();
				ZombieManager.getZombiesInRadius(position, sqrRadius, AlertTool.zombiesInRadius);
				for (int j = 0; j < AlertTool.zombiesInRadius.Count; j++)
				{
					Zombie zombie2 = AlertTool.zombiesInRadius[j];
					if (!zombie2.isDead && zombie2.checkAlert(player))
					{
						Vector3 vector = zombie2.transform.position - position;
						if (AlertTool.check(zombie2.transform.forward, vector, sqrRadius, sneak, spotDir, isSpotOn, zombie2.speciality.IsDLVolatile()))
						{
							RaycastHit raycastHit;
							Physics.Raycast(zombie2.transform.position + Vector3.up, -vector, out raycastHit, vector.magnitude * 0.95f, RayMasks.BLOCK_VISION);
							if (!(raycastHit.transform != null))
							{
								zombie2.alert(player);
							}
						}
					}
				}
			}
			AlertTool.animalsInRadius.Clear();
			AnimalManager.getAnimalsInRadius(position, sqrRadius, AlertTool.animalsInRadius);
			for (int k = 0; k < AlertTool.animalsInRadius.Count; k++)
			{
				Animal animal = AlertTool.animalsInRadius[k];
				if (!animal.isDead && animal.asset != null)
				{
					if (animal.asset.behaviour == EAnimalBehaviour.DEFENSE)
					{
						if (!animal.isFleeing)
						{
							Vector3 vector2 = animal.transform.position - position;
							if (!AlertTool.check(animal.transform.forward, vector2, sqrRadius, sneak, spotDir, isSpotOn, false))
							{
								goto IL_33C;
							}
							RaycastHit raycastHit;
							Physics.Raycast(animal.transform.position + Vector3.up, -vector2, out raycastHit, vector2.magnitude * 0.95f, RayMasks.BLOCK_VISION);
							if (raycastHit.transform != null)
							{
								goto IL_33C;
							}
						}
						animal.alertRunAwayFromPoint(player.transform.position, true);
					}
					else if (animal.asset.behaviour == EAnimalBehaviour.OFFENSE && animal.checkAlert(player))
					{
						Vector3 vector3 = animal.transform.position - position;
						if (AlertTool.check(animal.transform.forward, vector3, sqrRadius, sneak, spotDir, isSpotOn, false))
						{
							RaycastHit raycastHit;
							Physics.Raycast(animal.transform.position + Vector3.up, -vector3, out raycastHit, vector3.magnitude * 0.95f, RayMasks.BLOCK_VISION);
							if (!(raycastHit.transform != null))
							{
								animal.alertPlayer(player, true);
							}
						}
					}
				}
				IL_33C:;
			}
		}

		/// <summary>
		/// Alerts any agents in the area.
		/// </summary>
		/// <param name="position">The position of the alert.</param>
		/// <param name="radius">The detection radius.</param>
		// Token: 0x06003CFD RID: 15613 RVA: 0x00122998 File Offset: 0x00120B98
		public static void alert(Vector3 position, float radius)
		{
			float sqrRadius = radius * radius;
			if (LevelNavigation.checkNavigation(position))
			{
				AlertTool.zombiesInRadius.Clear();
				ZombieManager.getZombiesInRadius(position, sqrRadius, AlertTool.zombiesInRadius);
				for (int i = 0; i < AlertTool.zombiesInRadius.Count; i++)
				{
					Zombie zombie = AlertTool.zombiesInRadius[i];
					if (!zombie.isDead)
					{
						zombie.alert(position, true);
					}
				}
			}
			AlertTool.animalsInRadius.Clear();
			AnimalManager.getAnimalsInRadius(position, sqrRadius, AlertTool.animalsInRadius);
			for (int j = 0; j < AlertTool.animalsInRadius.Count; j++)
			{
				Animal animal = AlertTool.animalsInRadius[j];
				if (!animal.isDead && animal.asset != null)
				{
					if (animal.asset.behaviour == EAnimalBehaviour.DEFENSE)
					{
						animal.alertRunAwayFromPoint(position, true);
					}
					else if (animal.asset.behaviour == EAnimalBehaviour.OFFENSE)
					{
						animal.alertGoToPoint(position, true);
					}
				}
			}
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00122A74 File Offset: 0x00120C74
		[Conditional("LOG_ALERTS")]
		private static void LogAlert(string format, params object[] args)
		{
			UnturnedLog.info(format, args);
		}

		// Token: 0x04002655 RID: 9813
		private static List<Zombie> zombiesInRadius = new List<Zombie>();

		// Token: 0x04002656 RID: 9814
		private static List<Animal> animalsInRadius = new List<Animal>();
	}
}
