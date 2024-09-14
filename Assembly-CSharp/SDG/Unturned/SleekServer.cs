using System;
using SDG.SteamworksProvider.Services.Store;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200072D RID: 1837
	public class SleekServer : SleekWrapper
	{
		/// <summary>
		/// Is the server this widget represents currently favorited?
		/// Can be false on the favorites list.
		/// </summary>
		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06003C89 RID: 15497 RVA: 0x0011E42F File Offset: 0x0011C62F
		public bool isCurrentlyFavorited
		{
			get
			{
				return Provider.GetServerIsFavorited(this.info.ip, this.info.queryPort);
			}
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x0011E44C File Offset: 0x0011C64C
		public void SynchronizeVisibleColumns()
		{
			float num = 0f;
			if (FilterSettings.columns.anticheat)
			{
				num -= this.anticheatBox.SizeOffset_X;
				this.anticheatBox.PositionOffset_X = num;
				this.anticheatBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.anticheatBox.IsVisible = false;
			}
			if (FilterSettings.columns.cheats)
			{
				num -= this.cheatsBox.SizeOffset_X;
				this.cheatsBox.PositionOffset_X = num;
				this.cheatsBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.cheatsBox.IsVisible = false;
			}
			if (FilterSettings.columns.plugins)
			{
				num -= this.pluginsBox.SizeOffset_X;
				this.pluginsBox.PositionOffset_X = num;
				this.pluginsBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.pluginsBox.IsVisible = false;
			}
			if (FilterSettings.columns.workshop)
			{
				num -= this.workshopBox.SizeOffset_X;
				this.workshopBox.PositionOffset_X = num;
				this.workshopBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.workshopBox.IsVisible = false;
			}
			if (FilterSettings.columns.monetization)
			{
				num -= this.monetizationBox.SizeOffset_X;
				this.monetizationBox.PositionOffset_X = num;
				this.monetizationBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.monetizationBox.IsVisible = false;
			}
			if (FilterSettings.columns.gold)
			{
				num -= this.goldBox.SizeOffset_X;
				this.goldBox.PositionOffset_X = num;
				this.goldBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.goldBox.IsVisible = false;
			}
			if (FilterSettings.columns.perspective)
			{
				num -= this.perspectiveBox.SizeOffset_X;
				this.perspectiveBox.PositionOffset_X = num;
				this.perspectiveBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.perspectiveBox.IsVisible = false;
			}
			if (FilterSettings.columns.combat)
			{
				num -= this.combatBox.SizeOffset_X;
				this.combatBox.PositionOffset_X = num;
				this.combatBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.combatBox.IsVisible = false;
			}
			if (FilterSettings.columns.password)
			{
				num -= this.passwordBox.SizeOffset_X;
				this.passwordBox.PositionOffset_X = num;
				this.passwordBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.passwordBox.IsVisible = false;
			}
			if (FilterSettings.columns.fullnessPercentage)
			{
				num -= this.fullnessBox.SizeOffset_X;
				this.fullnessBox.PositionOffset_X = num;
				this.fullnessBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.fullnessBox.IsVisible = false;
			}
			if (FilterSettings.columns.maxPlayers)
			{
				num -= this.maxPlayersBox.SizeOffset_X;
				this.maxPlayersBox.PositionOffset_X = num;
				this.maxPlayersBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.maxPlayersBox.IsVisible = false;
			}
			if (FilterSettings.columns.players)
			{
				if (FilterSettings.columns.maxPlayers)
				{
					this.playersBox.SizeOffset_X = 80f;
					this.playersBox.Text = this.info.players.ToString();
				}
				else
				{
					this.playersBox.SizeOffset_X = 120f;
					this.playersBox.Text = MenuPlayUI.serverListUI.localization.format("Server_Players", this.info.players, this.info.maxPlayers);
				}
				num -= this.playersBox.SizeOffset_X;
				this.playersBox.PositionOffset_X = num;
				this.playersBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.playersBox.IsVisible = false;
			}
			if (FilterSettings.columns.ping)
			{
				num -= this.pingBox.SizeOffset_X;
				this.pingBox.PositionOffset_X = num;
				this.pingBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.pingBox.IsVisible = false;
			}
			if (FilterSettings.columns.map)
			{
				num -= this.mapBox.SizeOffset_X;
				this.mapBox.PositionOffset_X = num;
				this.mapBox.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.mapBox.IsVisible = false;
			}
			num -= this.button.PositionOffset_X;
			this.button.SizeOffset_X = num;
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x0011E8F7 File Offset: 0x0011CAF7
		private void onClickedFavoriteOffButton(ISleekElement button)
		{
			Provider.SetServerIsFavorited(this.info.ip, this.info.connectionPort, this.info.queryPort, !this.isCurrentlyFavorited);
			this.refreshFavoriteButton();
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x0011E930 File Offset: 0x0011CB30
		private void refreshFavoriteButton()
		{
			if (this.isCurrentlyFavorited)
			{
				this.button.IsClickable = true;
				this.favoriteButton.tooltip = MenuPlayUI.serverListUI.localization.format("Favorite_Off_Button_Tooltip");
				this.favoriteButton.icon = MenuPlayUI.serverListUI.icons.load<Texture2D>("Favorite_Off");
				return;
			}
			this.button.IsClickable = false;
			this.favoriteButton.tooltip = MenuPlayUI.serverListUI.localization.format("Favorite_On_Button_Tooltip");
			this.favoriteButton.icon = MenuPlayUI.serverListUI.icons.load<Texture2D>("Favorite_On");
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x0011E9DC File Offset: 0x0011CBDC
		private void onClickedButton(ISleekElement button)
		{
			if (!Provider.isPro && this.info.isPro)
			{
				Provider.provider.storeService.open(new SteamworksStorePackageID(Provider.PRO_ID.m_AppId));
				return;
			}
			ClickedServer clickedServer = this.onClickedServer;
			if (clickedServer == null)
			{
				return;
			}
			clickedServer(this, this.info);
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x0011EA34 File Offset: 0x0011CC34
		public SleekServer(ESteamServerList list, SteamServerAdvertisement newInfo)
		{
			this.info = newInfo;
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.OnClicked += new ClickedButton(this.onClickedButton);
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionOffset_X = 45f;
			this.nameLabel.SizeScale_X = 1f;
			this.nameLabel.SizeOffset_X = -45f;
			this.nameLabel.TextAlignment = 3;
			this.nameLabel.Text = this.info.name;
			this.button.AddChild(this.nameLabel);
			if (string.IsNullOrEmpty(this.info.descText))
			{
				this.nameLabel.SizeOffset_Y = 40f;
			}
			else
			{
				this.nameLabel.SizeOffset_Y = 30f;
				this.descLabel = Glazier.Get().CreateLabel();
				this.descLabel.PositionOffset_X = 45f;
				this.descLabel.PositionOffset_Y = 15f;
				this.descLabel.SizeScale_X = 1f;
				this.descLabel.SizeOffset_X = -45f;
				this.descLabel.SizeOffset_Y = 30f;
				this.descLabel.FontSize = 1;
				this.descLabel.AllowRichText = true;
				this.descLabel.TextColor = 4;
				this.descLabel.TextContrastContext = 1;
				this.descLabel.TextAlignment = 3;
				this.descLabel.Text = this.info.descText;
				this.button.AddChild(this.descLabel);
			}
			this.mapBox = Glazier.Get().CreateBox();
			this.mapBox.PositionScale_X = 1f;
			this.mapBox.SizeOffset_X = 153f;
			this.mapBox.SizeScale_Y = 1f;
			Texture2D orLoadIcon = LevelIconCache.GetOrLoadIcon(this.info.map);
			if (orLoadIcon != null)
			{
				ISleekImage sleekImage = Glazier.Get().CreateImage(orLoadIcon);
				sleekImage.PositionOffset_X = 5f;
				sleekImage.PositionOffset_Y = 5f;
				sleekImage.SizeOffset_X = 143f;
				sleekImage.SizeOffset_Y = 30f;
				this.mapBox.AddChild(sleekImage);
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.SizeScale_X = 1f;
				sleekLabel.SizeScale_Y = 1f;
				sleekLabel.TextAlignment = 4;
				sleekLabel.TextContrastContext = 2;
				sleekLabel.Text = this.info.map;
				this.mapBox.AddChild(sleekLabel);
			}
			else
			{
				this.mapBox.Text = this.info.map;
			}
			this.playersBox = Glazier.Get().CreateBox();
			this.playersBox.PositionScale_X = 1f;
			this.playersBox.SizeOffset_X = 80f;
			this.playersBox.SizeScale_Y = 1f;
			this.maxPlayersBox = Glazier.Get().CreateBox();
			this.maxPlayersBox.PositionScale_X = 1f;
			this.maxPlayersBox.SizeOffset_X = 80f;
			this.maxPlayersBox.SizeScale_Y = 1f;
			this.maxPlayersBox.Text = this.info.maxPlayers.ToString();
			this.fullnessBox = Glazier.Get().CreateBox();
			this.fullnessBox.PositionScale_X = 1f;
			this.fullnessBox.SizeOffset_X = 80f;
			this.fullnessBox.SizeScale_Y = 1f;
			this.fullnessBox.Text = this.info.NormalizedPlayerCount.ToString("P0");
			this.fullnessBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Server_Players", this.info.players, this.info.maxPlayers);
			this.pingBox = Glazier.Get().CreateBox();
			this.pingBox.PositionScale_X = 1f;
			this.pingBox.SizeOffset_X = 80f;
			this.pingBox.SizeScale_Y = 1f;
			this.pingBox.Text = string.Format("{0} ms", this.info.ping);
			this.anticheatBox = Glazier.Get().CreateBox();
			this.anticheatBox.PositionScale_X = 1f;
			this.anticheatBox.SizeOffset_X = 80f;
			this.anticheatBox.SizeScale_Y = 1f;
			ISleekImage sleekImage2 = Glazier.Get().CreateImage();
			sleekImage2.PositionOffset_X = 15f;
			sleekImage2.PositionOffset_Y = 10f;
			sleekImage2.SizeOffset_X = 20f;
			sleekImage2.SizeOffset_Y = 20f;
			sleekImage2.TintColor = 2;
			this.anticheatBox.AddChild(sleekImage2);
			if (this.info.IsBattlEyeSecure)
			{
				sleekImage2.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("BattlEye");
			}
			else
			{
				sleekImage2.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("BattlEye_Off");
			}
			ISleekImage sleekImage3 = Glazier.Get().CreateImage();
			sleekImage3.PositionOffset_X = 45f;
			sleekImage3.PositionOffset_Y = 10f;
			sleekImage3.SizeOffset_X = 20f;
			sleekImage3.SizeOffset_Y = 20f;
			sleekImage3.TintColor = 2;
			this.anticheatBox.AddChild(sleekImage3);
			if (this.info.IsVACSecure)
			{
				sleekImage3.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("VAC");
			}
			else
			{
				sleekImage3.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("VAC_Off");
			}
			if (this.info.IsBattlEyeSecure && this.info.IsVACSecure)
			{
				this.anticheatBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Anticheat_Column_Both_Tooltip");
			}
			else if (this.info.IsBattlEyeSecure)
			{
				this.anticheatBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Anticheat_Column_BattlEye_Tooltip");
			}
			else if (this.info.IsVACSecure)
			{
				this.anticheatBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Anticheat_Column_VAC_Tooltip");
			}
			else
			{
				this.anticheatBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Anticheat_Column_None_Tooltip");
			}
			this.perspectiveBox = Glazier.Get().CreateBox();
			this.perspectiveBox.PositionScale_X = 1f;
			this.perspectiveBox.SizeOffset_X = 40f;
			this.perspectiveBox.SizeScale_Y = 1f;
			ISleekImage sleekImage4 = Glazier.Get().CreateImage();
			sleekImage4.PositionOffset_X = 10f;
			sleekImage4.PositionOffset_Y = 10f;
			sleekImage4.SizeOffset_X = 20f;
			sleekImage4.SizeOffset_Y = 20f;
			sleekImage4.TintColor = 2;
			switch (this.info.cameraMode)
			{
			case ECameraMode.FIRST:
				sleekImage4.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Perspective_FirstPerson");
				this.perspectiveBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Perspective_Column_First_Tooltip");
				break;
			case ECameraMode.THIRD:
				sleekImage4.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Perspective_ThirdPerson");
				this.perspectiveBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Perspective_Column_Third_Tooltip");
				break;
			case ECameraMode.BOTH:
				sleekImage4.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Perspective_Both");
				this.perspectiveBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Perspective_Column_Both_Tooltip");
				break;
			case ECameraMode.VEHICLE:
				sleekImage4.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Perspective_Vehicle");
				this.perspectiveBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Perspective_Column_Vehicle_Tooltip");
				break;
			}
			this.perspectiveBox.AddChild(sleekImage4);
			this.combatBox = Glazier.Get().CreateBox();
			this.combatBox.PositionScale_X = 1f;
			this.combatBox.SizeOffset_X = 40f;
			this.combatBox.SizeScale_Y = 1f;
			if (this.info.isPvP)
			{
				this.combatBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Combat_Column_PvP_Tooltip");
			}
			else
			{
				this.combatBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Combat_Column_PvE_Tooltip");
			}
			ISleekImage sleekImage5 = Glazier.Get().CreateImage();
			sleekImage5.PositionOffset_X = 10f;
			sleekImage5.PositionOffset_Y = 10f;
			sleekImage5.SizeOffset_X = 20f;
			sleekImage5.SizeOffset_Y = 20f;
			sleekImage5.TintColor = 2;
			sleekImage5.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>(this.info.isPvP ? "PvP" : "PvE");
			this.combatBox.AddChild(sleekImage5);
			this.passwordBox = Glazier.Get().CreateBox();
			this.passwordBox.PositionScale_X = 1f;
			this.passwordBox.SizeOffset_X = 40f;
			this.passwordBox.SizeScale_Y = 1f;
			ISleekImage sleekImage6 = Glazier.Get().CreateImage();
			sleekImage6.PositionOffset_X = 10f;
			sleekImage6.PositionOffset_Y = 10f;
			sleekImage6.SizeOffset_X = 20f;
			sleekImage6.SizeOffset_Y = 20f;
			sleekImage6.TintColor = 2;
			this.passwordBox.AddChild(sleekImage6);
			if (this.info.isPassworded)
			{
				sleekImage6.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("PasswordProtected");
				this.passwordBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Password_Column_Yes_Tooltip");
			}
			else
			{
				sleekImage6.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("NotPasswordProtected");
				this.passwordBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Password_Column_No_Tooltip");
			}
			this.workshopBox = Glazier.Get().CreateBox();
			this.workshopBox.PositionScale_X = 1f;
			this.workshopBox.SizeOffset_X = 40f;
			this.workshopBox.SizeScale_Y = 1f;
			ISleekImage sleekImage7 = Glazier.Get().CreateImage();
			sleekImage7.PositionOffset_X = 10f;
			sleekImage7.PositionOffset_Y = 10f;
			sleekImage7.SizeOffset_X = 20f;
			sleekImage7.SizeOffset_Y = 20f;
			sleekImage7.TintColor = 2;
			this.workshopBox.AddChild(sleekImage7);
			if (this.info.isWorkshop)
			{
				sleekImage7.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("HasMods");
				this.workshopBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Workshop_Column_Yes_Tooltip");
			}
			else
			{
				sleekImage7.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("NoMods");
				this.workshopBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Workshop_Column_No_Tooltip");
			}
			this.goldBox = Glazier.Get().CreateBox();
			this.goldBox.PositionScale_X = 1f;
			this.goldBox.SizeOffset_X = 40f;
			this.goldBox.SizeScale_Y = 1f;
			this.goldBox.BackgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
			this.goldBox.TextColor = Palette.PRO;
			ISleekImage sleekImage8 = Glazier.Get().CreateImage();
			sleekImage8.PositionOffset_X = 10f;
			sleekImage8.PositionOffset_Y = 10f;
			sleekImage8.SizeOffset_X = 20f;
			sleekImage8.SizeOffset_Y = 20f;
			sleekImage8.TintColor = Palette.PRO;
			this.goldBox.AddChild(sleekImage8);
			if (this.info.isPro)
			{
				sleekImage8.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("GoldRequired");
				this.goldBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Gold_Column_Yes_Tooltip");
			}
			else
			{
				sleekImage8.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("GoldNotRequired");
				this.goldBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Gold_Column_No_Tooltip");
			}
			this.cheatsBox = Glazier.Get().CreateBox();
			this.cheatsBox.PositionScale_X = 1f;
			this.cheatsBox.SizeOffset_X = 40f;
			this.cheatsBox.SizeScale_Y = 1f;
			ISleekImage sleekImage9 = Glazier.Get().CreateImage();
			sleekImage9.PositionOffset_X = 10f;
			sleekImage9.PositionOffset_Y = 10f;
			sleekImage9.SizeOffset_X = 20f;
			sleekImage9.SizeOffset_Y = 20f;
			sleekImage9.TintColor = 2;
			this.cheatsBox.AddChild(sleekImage9);
			if (this.info.hasCheats)
			{
				sleekImage9.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("CheatCodes");
				this.cheatsBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Cheats_Column_Yes_Tooltip");
			}
			else
			{
				sleekImage9.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("CheatCodes_None");
				this.cheatsBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Cheats_Column_No_Tooltip");
			}
			this.monetizationBox = Glazier.Get().CreateBox();
			this.monetizationBox.PositionScale_X = 1f;
			this.monetizationBox.SizeOffset_X = 40f;
			this.monetizationBox.SizeScale_Y = 1f;
			ISleekImage sleekImage10 = Glazier.Get().CreateImage();
			sleekImage10.PositionOffset_X = 10f;
			sleekImage10.PositionOffset_Y = 10f;
			sleekImage10.SizeOffset_X = 20f;
			sleekImage10.SizeOffset_Y = 20f;
			sleekImage10.TintColor = 2;
			this.monetizationBox.AddChild(sleekImage10);
			switch (this.info.monetization)
			{
			case EServerMonetizationTag.Unspecified:
				sleekImage10.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Unknown");
				this.monetizationBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Monetization_Column_Unspecified_Tooltip");
				break;
			case EServerMonetizationTag.None:
				sleekImage10.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Monetization_None");
				this.monetizationBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Monetization_Column_None_Tooltip");
				break;
			case EServerMonetizationTag.NonGameplay:
				sleekImage10.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("NonGameplayMonetization");
				this.monetizationBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Monetization_Column_NonGameplay_Tooltip");
				break;
			case EServerMonetizationTag.Monetized:
				sleekImage10.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Monetized");
				this.monetizationBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Monetization_Column_Monetized_Tooltip");
				break;
			}
			this.pluginsBox = Glazier.Get().CreateBox();
			this.pluginsBox.PositionScale_X = 1f;
			this.pluginsBox.SizeOffset_X = 40f;
			this.pluginsBox.SizeScale_Y = 1f;
			ISleekImage sleekImage11 = Glazier.Get().CreateImage();
			sleekImage11.PositionOffset_X = 10f;
			sleekImage11.PositionOffset_Y = 10f;
			sleekImage11.SizeOffset_X = 20f;
			sleekImage11.SizeOffset_Y = 20f;
			sleekImage11.TintColor = 2;
			this.pluginsBox.AddChild(sleekImage11);
			switch (this.info.pluginFramework)
			{
			case SteamServerAdvertisement.EPluginFramework.None:
				sleekImage11.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Plugins_None");
				this.pluginsBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Plugins_Column_None_Tooltip");
				break;
			case SteamServerAdvertisement.EPluginFramework.Rocket:
				sleekImage11.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("RocketMod");
				this.pluginsBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Plugins_Column_Rocket_Tooltip");
				break;
			case SteamServerAdvertisement.EPluginFramework.OpenMod:
				sleekImage11.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("OpenMod");
				this.pluginsBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Plugins_Column_OpenMod_Tooltip");
				break;
			case SteamServerAdvertisement.EPluginFramework.Unknown:
				sleekImage11.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("Unknown");
				this.pluginsBox.TooltipText = MenuPlayUI.serverListUI.localization.format("Plugins_Column_Unknown_Tooltip");
				break;
			}
			this.SynchronizeVisibleColumns();
			base.AddChild(this.button);
			base.AddChild(this.mapBox);
			base.AddChild(this.playersBox);
			base.AddChild(this.maxPlayersBox);
			base.AddChild(this.fullnessBox);
			base.AddChild(this.pingBox);
			base.AddChild(this.anticheatBox);
			base.AddChild(this.perspectiveBox);
			base.AddChild(this.combatBox);
			base.AddChild(this.passwordBox);
			base.AddChild(this.workshopBox);
			base.AddChild(this.goldBox);
			base.AddChild(this.cheatsBox);
			base.AddChild(this.monetizationBox);
			base.AddChild(this.pluginsBox);
			if (!string.IsNullOrEmpty(this.info.thumbnailURL))
			{
				this.thumbnail = new SleekWebImage();
				this.thumbnail.PositionOffset_X = 4f;
				this.thumbnail.PositionOffset_Y = 4f;
				this.thumbnail.SizeOffset_X = 32f;
				this.thumbnail.SizeOffset_Y = 32f;
				this.thumbnail.Refresh(this.info.thumbnailURL, true);
				this.button.AddChild(this.thumbnail);
			}
			if (this.info.isPro && !Provider.isPro)
			{
				this.button.TextColor = Palette.PRO;
				this.button.TooltipText = MenuPlayUI.serverListUI.localization.format("Gold_Column_Yes_Tooltip");
				ISleekImage sleekImage12 = Glazier.Get().CreateImage();
				sleekImage12.PositionOffset_X = 10f;
				sleekImage12.PositionOffset_Y = 10f;
				sleekImage12.SizeOffset_X = 20f;
				sleekImage12.SizeOffset_Y = 20f;
				sleekImage12.TintColor = Palette.PRO;
				sleekImage12.Texture = MenuPlayUI.serverListUI.icons.load<Texture2D>("GoldRequired");
				this.button.AddChild(sleekImage12);
			}
			if (list == ESteamServerList.FAVORITES)
			{
				this.button.PositionOffset_X += 40f;
				this.button.SizeOffset_X -= 40f;
				this.favoriteButton = new SleekButtonIcon(null);
				this.favoriteButton.SizeOffset_X = 40f;
				this.favoriteButton.SizeScale_Y = 1f;
				this.favoriteButton.iconPositionOffset = 10;
				this.favoriteButton.iconColor = 2;
				this.favoriteButton.onClickedButton += new ClickedButton(this.onClickedFavoriteOffButton);
				base.AddChild(this.favoriteButton);
				this.refreshFavoriteButton();
			}
		}

		// Token: 0x040025D9 RID: 9689
		private SteamServerAdvertisement info;

		// Token: 0x040025DA RID: 9690
		private SleekButtonIcon favoriteButton;

		// Token: 0x040025DB RID: 9691
		private ISleekButton button;

		// Token: 0x040025DC RID: 9692
		private ISleekBox mapBox;

		// Token: 0x040025DD RID: 9693
		private ISleekBox playersBox;

		// Token: 0x040025DE RID: 9694
		private ISleekBox maxPlayersBox;

		// Token: 0x040025DF RID: 9695
		private ISleekBox fullnessBox;

		// Token: 0x040025E0 RID: 9696
		private ISleekBox pingBox;

		// Token: 0x040025E1 RID: 9697
		private ISleekBox anticheatBox;

		// Token: 0x040025E2 RID: 9698
		private ISleekBox perspectiveBox;

		// Token: 0x040025E3 RID: 9699
		private ISleekBox combatBox;

		// Token: 0x040025E4 RID: 9700
		private ISleekBox passwordBox;

		// Token: 0x040025E5 RID: 9701
		private ISleekBox workshopBox;

		// Token: 0x040025E6 RID: 9702
		private ISleekBox goldBox;

		// Token: 0x040025E7 RID: 9703
		private ISleekBox cheatsBox;

		// Token: 0x040025E8 RID: 9704
		private ISleekBox monetizationBox;

		// Token: 0x040025E9 RID: 9705
		private ISleekBox pluginsBox;

		// Token: 0x040025EA RID: 9706
		private SleekWebImage thumbnail;

		// Token: 0x040025EB RID: 9707
		private ISleekLabel nameLabel;

		// Token: 0x040025EC RID: 9708
		private ISleekLabel descLabel;

		// Token: 0x040025ED RID: 9709
		public ClickedServer onClickedServer;
	}
}
