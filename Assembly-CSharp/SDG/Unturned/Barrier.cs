using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004AD RID: 1197
	public class Barrier : MonoBehaviour
	{
		// Token: 0x06002512 RID: 9490 RVA: 0x00093AEC File Offset: 0x00091CEC
		private void OnTriggerEnter(Collider other)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (other.transform.CompareTag("Player"))
			{
				Player player = DamageTool.getPlayer(other.transform);
				if (player != null)
				{
					EPlayerKill eplayerKill;
					player.life.askDamage(101, Vector3.up * 10f, EDeathCause.SUICIDE, ELimb.SKULL, CSteamID.Nil, out eplayerKill);
					return;
				}
			}
			else if (other.CompareTag("Agent"))
			{
				Zombie zombie = DamageTool.getZombie(other.transform);
				if (zombie != null)
				{
					DamageZombieParameters parameters = DamageZombieParameters.makeInstakill(zombie);
					parameters.instigator = this;
					EPlayerKill eplayerKill2;
					uint num;
					DamageTool.damageZombie(parameters, out eplayerKill2, out num);
					return;
				}
				Animal animal = DamageTool.getAnimal(other.transform);
				if (animal != null)
				{
					DamageAnimalParameters parameters2 = DamageAnimalParameters.makeInstakill(animal);
					parameters2.instigator = this;
					EPlayerKill eplayerKill3;
					uint num2;
					DamageTool.damageAnimal(parameters2, out eplayerKill3, out num2);
				}
			}
		}
	}
}
