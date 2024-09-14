using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007C6 RID: 1990
	public class PlayerDeathUI
	{
		// Token: 0x06004366 RID: 17254 RVA: 0x0017C2AC File Offset: 0x0017A4AC
		public static void open(bool fromDeath)
		{
			if (PlayerDeathUI.active)
			{
				return;
			}
			PlayerDeathUI.active = true;
			PlayerDeathUI.synchronizeDeathCause();
			if (fromDeath && PlayerLife.deathCause != EDeathCause.SUICIDE && OptionsSettings.deathMusicVolume > 0f)
			{
				bool isServer = Provider.isServer;
			}
			if (Player.player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowDeathMenu) && !PlayerDeathUI.containerOnScreen)
			{
				PlayerDeathUI.containerOnScreen = true;
				PlayerDeathUI.container.AnimateIntoView();
			}
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0017C30F File Offset: 0x0017A50F
		public static void close()
		{
			if (!PlayerDeathUI.active)
			{
				return;
			}
			PlayerDeathUI.active = false;
			if (PlayerDeathUI.containerOnScreen)
			{
				PlayerDeathUI.containerOnScreen = false;
				PlayerDeathUI.container.AnimateOutOfView(0f, 1f);
			}
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0017C340 File Offset: 0x0017A540
		private static void synchronizeDeathCause()
		{
			if (PlayerLife.deathCause == EDeathCause.BLEEDING)
			{
				PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Bleeding");
				return;
			}
			if (PlayerLife.deathCause == EDeathCause.BONES)
			{
				PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Bones");
				return;
			}
			if (PlayerLife.deathCause == EDeathCause.FREEZING)
			{
				PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Freezing");
				return;
			}
			if (PlayerLife.deathCause == EDeathCause.BURNING)
			{
				PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Burning");
				return;
			}
			if (PlayerLife.deathCause == EDeathCause.FOOD)
			{
				PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Food");
				return;
			}
			if (PlayerLife.deathCause == EDeathCause.WATER)
			{
				PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Water");
				return;
			}
			if (PlayerLife.deathCause == EDeathCause.GUN || PlayerLife.deathCause == EDeathCause.MELEE || PlayerLife.deathCause == EDeathCause.PUNCH || PlayerLife.deathCause == EDeathCause.ROADKILL || PlayerLife.deathCause == EDeathCause.GRENADE || PlayerLife.deathCause == EDeathCause.MISSILE || PlayerLife.deathCause == EDeathCause.CHARGE || PlayerLife.deathCause == EDeathCause.SPLASH)
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(PlayerLife.deathKiller);
				string text;
				string text2;
				if (steamPlayer != null)
				{
					text = steamPlayer.playerID.characterName;
					text2 = steamPlayer.playerID.playerName;
				}
				else
				{
					text = "?";
					text2 = "?";
				}
				string arg = "";
				if (PlayerLife.deathLimb == ELimb.LEFT_FOOT || PlayerLife.deathLimb == ELimb.LEFT_LEG || PlayerLife.deathLimb == ELimb.RIGHT_FOOT || PlayerLife.deathLimb == ELimb.RIGHT_LEG)
				{
					arg = PlayerDeathUI.localization.format("Leg");
				}
				else if (PlayerLife.deathLimb == ELimb.LEFT_HAND || PlayerLife.deathLimb == ELimb.LEFT_ARM || PlayerLife.deathLimb == ELimb.RIGHT_HAND || PlayerLife.deathLimb == ELimb.RIGHT_ARM)
				{
					arg = PlayerDeathUI.localization.format("Arm");
				}
				else if (PlayerLife.deathLimb == ELimb.SPINE)
				{
					arg = PlayerDeathUI.localization.format("Spine");
				}
				else if (PlayerLife.deathLimb == ELimb.SKULL)
				{
					arg = PlayerDeathUI.localization.format("Skull");
				}
				if (PlayerLife.deathCause == EDeathCause.GUN)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Gun", arg, text, text2);
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.MELEE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Melee", arg, text, text2);
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.PUNCH)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Punch", arg, text, text2);
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.ROADKILL)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Roadkill", text, text2);
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.GRENADE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Grenade", text, text2);
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.MISSILE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Missile", text, text2);
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.CHARGE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Charge", text, text2);
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.SPLASH)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Splash", text, text2);
					return;
				}
			}
			else
			{
				if (PlayerLife.deathCause == EDeathCause.ZOMBIE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Zombie");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.ANIMAL)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Animal");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.SUICIDE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Suicide");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.KILL)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Kill");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.INFECTION)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Infection");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.BREATH)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Breath");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.ZOMBIE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Zombie");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.VEHICLE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Vehicle");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.SHRED)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Shred");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.LANDMINE)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Landmine");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.ARENA)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Arena");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.SENTRY)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Sentry");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.ACID)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Acid");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.BOULDER)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Boulder");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.BURNER)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Burner");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.SPIT)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Spit");
					return;
				}
				if (PlayerLife.deathCause == EDeathCause.SPARK)
				{
					PlayerDeathUI.causeBox.Text = PlayerDeathUI.localization.format("Spark");
				}
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0017C8B0 File Offset: 0x0017AAB0
		private static void onClickedHomeButton(ISleekElement button)
		{
			if (!Provider.isServer && Provider.isPvP)
			{
				if (Time.realtimeSinceStartup - Player.player.life.lastDeath < Provider.modeConfigData.Gameplay.Timer_Home)
				{
					return;
				}
			}
			else if (Time.realtimeSinceStartup - Player.player.life.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
			{
				return;
			}
			Player.player.life.sendRespawn(true);
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0017C92D File Offset: 0x0017AB2D
		private static void onClickedRespawnButton(ISleekElement button)
		{
			if (Time.realtimeSinceStartup - Player.player.life.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
			{
				return;
			}
			Player.player.life.sendRespawn(false);
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0017C968 File Offset: 0x0017AB68
		public PlayerDeathUI()
		{
			PlayerDeathUI.localization = Localization.read("/Player/PlayerDeath.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDeath/PlayerDeath.unity3d");
			PlayerDeathUI.container = new SleekFullscreenBox();
			PlayerDeathUI.container.PositionScale_Y = 1f;
			PlayerDeathUI.container.PositionOffset_X = 10f;
			PlayerDeathUI.container.PositionOffset_Y = 10f;
			PlayerDeathUI.container.SizeOffset_X = -20f;
			PlayerDeathUI.container.SizeOffset_Y = -20f;
			PlayerDeathUI.container.SizeScale_X = 1f;
			PlayerDeathUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerDeathUI.container);
			PlayerDeathUI.active = false;
			PlayerDeathUI.containerOnScreen = false;
			PlayerDeathUI.causeBox = Glazier.Get().CreateBox();
			PlayerDeathUI.causeBox.PositionOffset_Y = -25f;
			PlayerDeathUI.causeBox.PositionScale_Y = 0.8f;
			PlayerDeathUI.causeBox.SizeOffset_Y = 50f;
			PlayerDeathUI.causeBox.SizeScale_X = 1f;
			PlayerDeathUI.container.AddChild(PlayerDeathUI.causeBox);
			PlayerDeathUI.homeButton = new SleekButtonIcon(bundle.load<Texture2D>("Home"));
			PlayerDeathUI.homeButton.PositionOffset_X = -205f;
			PlayerDeathUI.homeButton.PositionOffset_Y = 35f;
			PlayerDeathUI.homeButton.PositionScale_X = 0.5f;
			PlayerDeathUI.homeButton.PositionScale_Y = 0.8f;
			PlayerDeathUI.homeButton.SizeOffset_X = 200f;
			PlayerDeathUI.homeButton.SizeOffset_Y = 30f;
			PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
			PlayerDeathUI.homeButton.tooltip = PlayerDeathUI.localization.format("Home_Button_Tooltip");
			PlayerDeathUI.homeButton.iconColor = 2;
			PlayerDeathUI.homeButton.onClickedButton += new ClickedButton(PlayerDeathUI.onClickedHomeButton);
			PlayerDeathUI.container.AddChild(PlayerDeathUI.homeButton);
			PlayerDeathUI.respawnButton = new SleekButtonIcon(bundle.load<Texture2D>("Respawn"));
			PlayerDeathUI.respawnButton.PositionOffset_X = 5f;
			PlayerDeathUI.respawnButton.PositionOffset_Y = 35f;
			PlayerDeathUI.respawnButton.PositionScale_X = 0.5f;
			PlayerDeathUI.respawnButton.PositionScale_Y = 0.8f;
			PlayerDeathUI.respawnButton.SizeOffset_X = 200f;
			PlayerDeathUI.respawnButton.SizeOffset_Y = 30f;
			PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button");
			PlayerDeathUI.respawnButton.tooltip = PlayerDeathUI.localization.format("Respawn_Button_Tooltip");
			PlayerDeathUI.respawnButton.iconColor = 2;
			PlayerDeathUI.respawnButton.onClickedButton += new ClickedButton(PlayerDeathUI.onClickedRespawnButton);
			PlayerDeathUI.container.AddChild(PlayerDeathUI.respawnButton);
			bundle.unload();
		}

		// Token: 0x04002C9C RID: 11420
		private static SleekFullscreenBox container;

		// Token: 0x04002C9D RID: 11421
		public static Local localization;

		// Token: 0x04002C9E RID: 11422
		public static bool active;

		// Token: 0x04002C9F RID: 11423
		private static ISleekBox causeBox;

		// Token: 0x04002CA0 RID: 11424
		public static SleekButtonIcon homeButton;

		// Token: 0x04002CA1 RID: 11425
		public static SleekButtonIcon respawnButton;

		/// <summary>
		/// Has the contained been animated into visibility on-screen?
		/// Used to disable animating out if disabled.
		/// </summary>
		// Token: 0x04002CA2 RID: 11426
		private static bool containerOnScreen;
	}
}
