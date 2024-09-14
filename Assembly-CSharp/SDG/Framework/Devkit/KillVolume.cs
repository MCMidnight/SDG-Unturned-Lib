using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200011B RID: 283
	public class KillVolume : LevelVolume<KillVolume, KillVolumeManager>
	{
		// Token: 0x06000744 RID: 1860 RVA: 0x0001AE38 File Offset: 0x00019038
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new KillVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001AE54 File Offset: 0x00019054
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.killPlayers = reader.readValue<bool>("Kill_Players");
			this.killZombies = reader.readValue<bool>("Kill_Zombies");
			this.killAnimals = reader.readValue<bool>("Kill_Animals");
			this.killVehicles = reader.readValue<bool>("Kill_Vehicles");
			this.deathCause = reader.readValue<EDeathCause>("Death_Cause");
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001AEC0 File Offset: 0x000190C0
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("Kill_Players", this.killPlayers);
			writer.writeValue<bool>("Kill_Zombies", this.killZombies);
			writer.writeValue<bool>("Kill_Animals", this.killAnimals);
			writer.writeValue<bool>("Kill_Vehicles", this.killVehicles);
			writer.writeValue<EDeathCause>("Death_Cause", this.deathCause);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001AF29 File Offset: 0x00019129
		protected override void Awake()
		{
			this.forceShouldAddCollider = true;
			base.Awake();
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001AF38 File Offset: 0x00019138
		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger)
			{
				return;
			}
			if (!Provider.isServer)
			{
				return;
			}
			if (other.CompareTag("Player"))
			{
				if (this.killPlayers)
				{
					Player player = DamageTool.getPlayer(other.transform);
					if (player != null)
					{
						EPlayerKill eplayerKill;
						DamageTool.damage(player, this.deathCause, ELimb.SPINE, CSteamID.Nil, Vector3.up, 101f, 1f, out eplayerKill, false, false, ERagdollEffect.NONE);
						return;
					}
				}
			}
			else if (other.CompareTag("Agent"))
			{
				if (this.killZombies || this.killAnimals)
				{
					Zombie zombie = DamageTool.getZombie(other.transform);
					if (zombie != null)
					{
						if (this.killZombies)
						{
							DamageZombieParameters parameters = DamageZombieParameters.makeInstakill(zombie);
							parameters.instigator = this;
							EPlayerKill eplayerKill2;
							uint num;
							DamageTool.damageZombie(parameters, out eplayerKill2, out num);
							return;
						}
					}
					else if (this.killAnimals)
					{
						Animal animal = DamageTool.getAnimal(other.transform);
						if (animal != null)
						{
							DamageAnimalParameters parameters2 = DamageAnimalParameters.makeInstakill(animal);
							parameters2.instigator = this;
							EPlayerKill eplayerKill3;
							uint num2;
							DamageTool.damageAnimal(parameters2, out eplayerKill3, out num2);
							return;
						}
					}
				}
			}
			else if (other.CompareTag("Vehicle"))
			{
				InteractableVehicle vehicle = DamageTool.getVehicle(other.transform);
				if (vehicle != null && !vehicle.isDead)
				{
					if (this.killPlayers)
					{
						for (int i = vehicle.passengers.Length - 1; i >= 0; i--)
						{
							Passenger passenger = vehicle.passengers[i];
							if (passenger != null && passenger.player != null)
							{
								Player player2 = passenger.player.player;
								if (!(player2 == null))
								{
									EPlayerKill eplayerKill4;
									DamageTool.damage(player2, this.deathCause, ELimb.SPINE, CSteamID.Nil, Vector3.up, 101f, 1f, out eplayerKill4, false, false, ERagdollEffect.NONE);
								}
							}
						}
					}
					if (this.killVehicles && !vehicle.isDead)
					{
						EPlayerKill eplayerKill5;
						DamageTool.damage(vehicle, false, Vector3.zero, false, 65000f, 1f, false, out eplayerKill5, default(CSteamID), EDamageOrigin.Kill_Volume);
					}
				}
			}
		}

		// Token: 0x040002B4 RID: 692
		public bool killPlayers = true;

		// Token: 0x040002B5 RID: 693
		public bool killZombies = true;

		// Token: 0x040002B6 RID: 694
		public bool killAnimals = true;

		// Token: 0x040002B7 RID: 695
		public bool killVehicles;

		// Token: 0x040002B8 RID: 696
		public EDeathCause deathCause = EDeathCause.BURNING;

		// Token: 0x0200086C RID: 2156
		private class Menu : SleekWrapper
		{
			// Token: 0x06004820 RID: 18464 RVA: 0x001AEF18 File Offset: 0x001AD118
			public Menu(KillVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 190f;
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = volume.killPlayers;
				sleekToggle.AddLabel("Kill Players", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnKillPlayersToggled);
				base.AddChild(sleekToggle);
				ISleekToggle sleekToggle2 = Glazier.Get().CreateToggle();
				sleekToggle2.PositionOffset_Y = 40f;
				sleekToggle2.SizeOffset_X = 40f;
				sleekToggle2.SizeOffset_Y = 40f;
				sleekToggle2.Value = volume.killZombies;
				sleekToggle2.AddLabel("Kill Zombies", 1);
				sleekToggle2.OnValueChanged += new Toggled(this.OnKillZombiesToggled);
				base.AddChild(sleekToggle2);
				ISleekToggle sleekToggle3 = Glazier.Get().CreateToggle();
				sleekToggle3.PositionOffset_Y = 80f;
				sleekToggle3.SizeOffset_X = 40f;
				sleekToggle3.SizeOffset_Y = 40f;
				sleekToggle3.Value = volume.killAnimals;
				sleekToggle3.AddLabel("Kill Animals", 1);
				sleekToggle3.OnValueChanged += new Toggled(this.OnKillAnimalsToggled);
				base.AddChild(sleekToggle3);
				ISleekToggle sleekToggle4 = Glazier.Get().CreateToggle();
				sleekToggle4.PositionOffset_Y = 120f;
				sleekToggle4.SizeOffset_X = 40f;
				sleekToggle4.SizeOffset_Y = 40f;
				sleekToggle4.Value = volume.killVehicles;
				sleekToggle4.AddLabel("Kill Vehicles", 1);
				sleekToggle4.OnValueChanged += new Toggled(this.OnKillVehiclesToggled);
				base.AddChild(sleekToggle4);
				SleekButtonStateEnum<EDeathCause> sleekButtonStateEnum = new SleekButtonStateEnum<EDeathCause>();
				sleekButtonStateEnum.PositionOffset_Y = 160f;
				sleekButtonStateEnum.SizeOffset_X = 200f;
				sleekButtonStateEnum.SizeOffset_Y = 30f;
				sleekButtonStateEnum.SetEnum(volume.deathCause);
				sleekButtonStateEnum.AddLabel("Death Cause", 1);
				SleekButtonStateEnum<EDeathCause> sleekButtonStateEnum2 = sleekButtonStateEnum;
				sleekButtonStateEnum2.OnSwappedEnum = (Action<SleekButtonStateEnum<EDeathCause>, EDeathCause>)Delegate.Combine(sleekButtonStateEnum2.OnSwappedEnum, new Action<SleekButtonStateEnum<EDeathCause>, EDeathCause>(this.OnSwappedDeathCause));
				base.AddChild(sleekButtonStateEnum);
			}

			// Token: 0x06004821 RID: 18465 RVA: 0x001AF121 File Offset: 0x001AD321
			private void OnKillPlayersToggled(ISleekToggle toggle, bool state)
			{
				this.volume.killPlayers = state;
			}

			// Token: 0x06004822 RID: 18466 RVA: 0x001AF12F File Offset: 0x001AD32F
			private void OnKillZombiesToggled(ISleekToggle toggle, bool state)
			{
				this.volume.killZombies = state;
			}

			// Token: 0x06004823 RID: 18467 RVA: 0x001AF13D File Offset: 0x001AD33D
			private void OnKillAnimalsToggled(ISleekToggle toggle, bool state)
			{
				this.volume.killAnimals = state;
			}

			// Token: 0x06004824 RID: 18468 RVA: 0x001AF14B File Offset: 0x001AD34B
			private void OnKillVehiclesToggled(ISleekToggle toggle, bool state)
			{
				this.volume.killVehicles = state;
			}

			// Token: 0x06004825 RID: 18469 RVA: 0x001AF159 File Offset: 0x001AD359
			private void OnSwappedDeathCause(SleekButtonStateEnum<EDeathCause> button, EDeathCause deathCause)
			{
				this.volume.deathCause = deathCause;
			}

			// Token: 0x04003175 RID: 12661
			private KillVolume volume;
		}
	}
}
