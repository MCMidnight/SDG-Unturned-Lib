using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200078A RID: 1930
	public class MenuConfigurationAudioUI : SleekFullscreenBox
	{
		// Token: 0x06003FB7 RID: 16311 RVA: 0x0014276F File Offset: 0x0014096F
		public void open()
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			base.AnimateIntoView();
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x00142787 File Offset: 0x00140987
		public void close()
		{
			if (!this.active)
			{
				return;
			}
			this.active = false;
			OptionsSettings.save();
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x001427AE File Offset: 0x001409AE
		private void OnVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.volume = state;
			OptionsSettings.apply();
			this.masterVolumeSlider.UpdateLabel(this.localization.format("Volume_Slider_Label", OptionsSettings.volume.ToString("P0")));
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x001427E8 File Offset: 0x001409E8
		private void OnVoiceVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.voiceVolume = state;
			this.voiceVolumeSlider.UpdateLabel(this.localization.format("Voice_Slider_Label", OptionsSettings.voiceVolume.ToString("P0")));
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x00142828 File Offset: 0x00140A28
		private void OnAtmosphereVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.AtmosphereVolume = state;
			this.atmosphereVolumeSlider.UpdateLabel(this.localization.format("Atmosphere_Volume_Slider_Label", OptionsSettings.AtmosphereVolume.ToString("P0")));
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x00142868 File Offset: 0x00140A68
		private void OnGameVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.gameVolume = state;
			this.gameVolumeSlider.UpdateLabel(this.localization.format("Game_Volume_Slider_Label", OptionsSettings.gameVolume.ToString("P0")));
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x001428A8 File Offset: 0x00140AA8
		private void OnUnfocusedVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.UnfocusedVolume = state;
			this.unfocusedVolumeSlider.UpdateLabel(this.localization.format("Unfocused_Volume_Slider_Label", OptionsSettings.UnfocusedVolume.ToString("P0")));
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x001428E8 File Offset: 0x00140AE8
		private void OnMusicMasterVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.MusicMasterVolume = state;
			this.musicMasterVolumeSlider.UpdateLabel(this.localization.format("Music_Master_Volume_Slider_Label", OptionsSettings.MusicMasterVolume.ToString("P0")));
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00142928 File Offset: 0x00140B28
		private void OnLoadingScreenMusicVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.loadingScreenMusicVolume = state;
			this.loadingScreenMusicVolumeSlider.UpdateLabel(this.localization.format("Loading_Screen_Music_Volume_Slider_Label", OptionsSettings.loadingScreenMusicVolume.ToString("P0")));
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0014295A File Offset: 0x00140B5A
		private void OnDeathMusicVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.deathMusicVolume = state;
			this.deathMusicVolumeSlider.UpdateLabel(this.localization.format("Death_Music_Volume_Slider_Label", OptionsSettings.deathMusicVolume.ToString("P0")));
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0014298C File Offset: 0x00140B8C
		private void OnMainMenuMusicVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.MainMenuMusicVolume = state;
			this.mainMenuMusicVolumeSlider.UpdateLabel(this.localization.format("Main_Menu_Music_Volume_Slider_Label", OptionsSettings.MainMenuMusicVolume.ToString("P0")));
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x001429CC File Offset: 0x00140BCC
		private void OnAmbientMusicVolumeSliderDragged(ISleekSlider slider, float state)
		{
			OptionsSettings.ambientMusicVolume = state;
			this.ambientMusicVolumeSlider.UpdateLabel(this.localization.format("Ambient_Music_Volume_Slider_Label", OptionsSettings.ambientMusicVolume.ToString("P0")));
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x001429FE File Offset: 0x00140BFE
		private void onClickedBackButton(ISleekElement button)
		{
			if (Player.player != null)
			{
				PlayerPauseUI.open();
			}
			else if (Level.isEditor)
			{
				EditorPauseUI.open();
			}
			else
			{
				MenuConfigurationUI.open();
			}
			this.close();
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00142A2D File Offset: 0x00140C2D
		private void onClickedDefaultButton(ISleekElement button)
		{
			OptionsSettings.RestoreAudioDefaults();
			this.updateAll();
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00142A3C File Offset: 0x00140C3C
		private void updateAll()
		{
			this.masterVolumeSlider.Value = OptionsSettings.volume;
			this.masterVolumeSlider.UpdateLabel(this.localization.format("Volume_Slider_Label", OptionsSettings.volume.ToString("P0")));
			this.unfocusedVolumeSlider.Value = OptionsSettings.UnfocusedVolume;
			this.unfocusedVolumeSlider.UpdateLabel(this.localization.format("Unfocused_Volume_Slider_Label", OptionsSettings.UnfocusedVolume.ToString("P0")));
			this.musicMasterVolumeSlider.Value = OptionsSettings.MusicMasterVolume;
			this.musicMasterVolumeSlider.UpdateLabel(this.localization.format("Music_Master_Volume_Slider_Label", OptionsSettings.MusicMasterVolume.ToString("P0")));
			this.loadingScreenMusicVolumeSlider.Value = OptionsSettings.loadingScreenMusicVolume;
			this.loadingScreenMusicVolumeSlider.UpdateLabel(this.localization.format("Loading_Screen_Music_Volume_Slider_Label", OptionsSettings.loadingScreenMusicVolume.ToString("P0")));
			this.deathMusicVolumeSlider.Value = OptionsSettings.deathMusicVolume;
			this.deathMusicVolumeSlider.UpdateLabel(this.localization.format("Death_Music_Volume_Slider_Label", OptionsSettings.deathMusicVolume.ToString("P0")));
			this.mainMenuMusicVolumeSlider.Value = OptionsSettings.MainMenuMusicVolume;
			this.mainMenuMusicVolumeSlider.UpdateLabel(this.localization.format("Main_Menu_Music_Volume_Slider_Label", OptionsSettings.MainMenuMusicVolume.ToString("P0")));
			this.ambientMusicVolumeSlider.Value = OptionsSettings.ambientMusicVolume;
			this.ambientMusicVolumeSlider.UpdateLabel(this.localization.format("Ambient_Music_Volume_Slider_Label", OptionsSettings.ambientMusicVolume.ToString("P0")));
			this.voiceVolumeSlider.Value = OptionsSettings.voiceVolume;
			this.voiceVolumeSlider.UpdateLabel(this.localization.format("Voice_Slider_Label", OptionsSettings.voiceVolume.ToString("P0")));
			this.gameVolumeSlider.Value = OptionsSettings.gameVolume;
			this.gameVolumeSlider.UpdateLabel(this.localization.format("Game_Volume_Slider_Label", OptionsSettings.gameVolume.ToString("P0")));
			this.atmosphereVolumeSlider.Value = OptionsSettings.AtmosphereVolume;
			this.atmosphereVolumeSlider.UpdateLabel(this.localization.format("Atmosphere_Volume_Slider_Label", OptionsSettings.AtmosphereVolume.ToString("P0")));
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x00142CA0 File Offset: 0x00140EA0
		public MenuConfigurationAudioUI()
		{
			this.localization = Localization.read("/Menu/Configuration/MenuConfigurationAudio.dat");
			new Color32(240, 240, 240, byte.MaxValue);
			new Color32(180, 180, 180, byte.MaxValue);
			this.active = false;
			this.audioBox = Glazier.Get().CreateScrollView();
			this.audioBox.PositionOffset_X = -200f;
			this.audioBox.PositionOffset_Y = 100f;
			this.audioBox.PositionScale_X = 0.5f;
			this.audioBox.SizeOffset_X = 430f;
			this.audioBox.SizeOffset_Y = -200f;
			this.audioBox.SizeScale_Y = 1f;
			this.audioBox.ScaleContentToWidth = true;
			base.AddChild(this.audioBox);
			int num = 0;
			this.masterVolumeSlider = Glazier.Get().CreateSlider();
			this.masterVolumeSlider.PositionOffset_Y = (float)num;
			this.masterVolumeSlider.SizeOffset_X = 200f;
			this.masterVolumeSlider.SizeOffset_Y = 20f;
			this.masterVolumeSlider.Orientation = 0;
			this.masterVolumeSlider.AddLabel(this.localization.format("Volume_Slider_Label", OptionsSettings.volume.ToString("P0")), 1);
			this.masterVolumeSlider.OnValueChanged += new Dragged(this.OnVolumeSliderDragged);
			this.audioBox.AddChild(this.masterVolumeSlider);
			num += 30;
			this.gameVolumeSlider = Glazier.Get().CreateSlider();
			this.gameVolumeSlider.PositionOffset_Y = (float)num;
			this.gameVolumeSlider.SizeOffset_X = 200f;
			this.gameVolumeSlider.SizeOffset_Y = 20f;
			this.gameVolumeSlider.Orientation = 0;
			this.gameVolumeSlider.AddLabel(this.localization.format("Game_Volume_Slider_Label", OptionsSettings.gameVolume.ToString("P0")), 1);
			this.gameVolumeSlider.OnValueChanged += new Dragged(this.OnGameVolumeSliderDragged);
			this.audioBox.AddChild(this.gameVolumeSlider);
			num += 30;
			this.unfocusedVolumeSlider = Glazier.Get().CreateSlider();
			this.unfocusedVolumeSlider.PositionOffset_Y = (float)num;
			this.unfocusedVolumeSlider.SizeOffset_X = 200f;
			this.unfocusedVolumeSlider.SizeOffset_Y = 20f;
			this.unfocusedVolumeSlider.Orientation = 0;
			this.unfocusedVolumeSlider.AddLabel(this.localization.format("Unfocused_Volume_Slider_Label", OptionsSettings.UnfocusedVolume.ToString("P0")), 1);
			this.unfocusedVolumeSlider.OnValueChanged += new Dragged(this.OnUnfocusedVolumeSliderDragged);
			this.audioBox.AddChild(this.unfocusedVolumeSlider);
			num += 30;
			this.voiceVolumeSlider = Glazier.Get().CreateSlider();
			this.voiceVolumeSlider.PositionOffset_Y = (float)num;
			this.voiceVolumeSlider.SizeOffset_X = 200f;
			this.voiceVolumeSlider.SizeOffset_Y = 20f;
			this.voiceVolumeSlider.Orientation = 0;
			this.voiceVolumeSlider.AddLabel(this.localization.format("Voice_Slider_Label", OptionsSettings.voiceVolume.ToString("P0")), 1);
			this.voiceVolumeSlider.OnValueChanged += new Dragged(this.OnVoiceVolumeSliderDragged);
			this.audioBox.AddChild(this.voiceVolumeSlider);
			num += 30;
			this.atmosphereVolumeSlider = Glazier.Get().CreateSlider();
			this.atmosphereVolumeSlider.PositionOffset_Y = (float)num;
			this.atmosphereVolumeSlider.SizeOffset_X = 200f;
			this.atmosphereVolumeSlider.SizeOffset_Y = 20f;
			this.atmosphereVolumeSlider.Orientation = 0;
			this.atmosphereVolumeSlider.AddLabel(this.localization.format("Atmosphere_Volume_Slider_Label", OptionsSettings.AtmosphereVolume.ToString("P0")), 1);
			this.atmosphereVolumeSlider.OnValueChanged += new Dragged(this.OnAtmosphereVolumeSliderDragged);
			this.audioBox.AddChild(this.atmosphereVolumeSlider);
			num += 30;
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.PositionOffset_Y = (float)num;
			sleekBox.SizeOffset_X = 400f;
			sleekBox.SizeOffset_Y = 30f;
			sleekBox.Text = this.localization.format("Music_Header");
			this.audioBox.AddChild(sleekBox);
			num += 40;
			this.musicMasterVolumeSlider = Glazier.Get().CreateSlider();
			this.musicMasterVolumeSlider.PositionOffset_Y = (float)num;
			this.musicMasterVolumeSlider.SizeOffset_X = 200f;
			this.musicMasterVolumeSlider.SizeOffset_Y = 20f;
			this.musicMasterVolumeSlider.Orientation = 0;
			this.musicMasterVolumeSlider.AddLabel(this.localization.format("Music_Master_Volume_Slider_Label", OptionsSettings.MusicMasterVolume.ToString("P0")), 1);
			this.musicMasterVolumeSlider.OnValueChanged += new Dragged(this.OnMusicMasterVolumeSliderDragged);
			this.audioBox.AddChild(this.musicMasterVolumeSlider);
			num += 30;
			this.loadingScreenMusicVolumeSlider = Glazier.Get().CreateSlider();
			this.loadingScreenMusicVolumeSlider.PositionOffset_Y = (float)num;
			this.loadingScreenMusicVolumeSlider.SizeOffset_X = 200f;
			this.loadingScreenMusicVolumeSlider.SizeOffset_Y = 20f;
			this.loadingScreenMusicVolumeSlider.Orientation = 0;
			this.loadingScreenMusicVolumeSlider.AddLabel(this.localization.format("Loading_Screen_Music_Volume_Slider_Label", OptionsSettings.loadingScreenMusicVolume.ToString("P0")), 1);
			this.loadingScreenMusicVolumeSlider.OnValueChanged += new Dragged(this.OnLoadingScreenMusicVolumeSliderDragged);
			this.audioBox.AddChild(this.loadingScreenMusicVolumeSlider);
			num += 30;
			this.deathMusicVolumeSlider = Glazier.Get().CreateSlider();
			this.deathMusicVolumeSlider.PositionOffset_Y = (float)num;
			this.deathMusicVolumeSlider.SizeOffset_X = 200f;
			this.deathMusicVolumeSlider.SizeOffset_Y = 20f;
			this.deathMusicVolumeSlider.Orientation = 0;
			this.deathMusicVolumeSlider.AddLabel(this.localization.format("Death_Music_Volume_Slider_Label", OptionsSettings.deathMusicVolume.ToString("P0")), 1);
			this.deathMusicVolumeSlider.OnValueChanged += new Dragged(this.OnDeathMusicVolumeSliderDragged);
			this.audioBox.AddChild(this.deathMusicVolumeSlider);
			num += 30;
			this.mainMenuMusicVolumeSlider = Glazier.Get().CreateSlider();
			this.mainMenuMusicVolumeSlider.PositionOffset_Y = (float)num;
			this.mainMenuMusicVolumeSlider.SizeOffset_X = 200f;
			this.mainMenuMusicVolumeSlider.SizeOffset_Y = 20f;
			this.mainMenuMusicVolumeSlider.Orientation = 0;
			this.mainMenuMusicVolumeSlider.AddLabel(this.localization.format("Main_Menu_Music_Volume_Slider_Label", OptionsSettings.MainMenuMusicVolume.ToString("P0")), 1);
			this.mainMenuMusicVolumeSlider.OnValueChanged += new Dragged(this.OnMainMenuMusicVolumeSliderDragged);
			this.audioBox.AddChild(this.mainMenuMusicVolumeSlider);
			num += 30;
			this.ambientMusicVolumeSlider = Glazier.Get().CreateSlider();
			this.ambientMusicVolumeSlider.PositionOffset_Y = (float)num;
			this.ambientMusicVolumeSlider.SizeOffset_X = 200f;
			this.ambientMusicVolumeSlider.SizeOffset_Y = 20f;
			this.ambientMusicVolumeSlider.Orientation = 0;
			this.ambientMusicVolumeSlider.AddLabel(this.localization.format("Ambient_Music_Volume_Slider_Label", OptionsSettings.ambientMusicVolume.ToString("P0")), 1);
			this.ambientMusicVolumeSlider.OnValueChanged += new Dragged(this.OnAmbientMusicVolumeSliderDragged);
			this.audioBox.AddChild(this.ambientMusicVolumeSlider);
			num += 30;
			this.audioBox.ContentSizeOffset = new Vector2(0f, (float)(num - 10));
			this.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			this.backButton.PositionOffset_Y = -50f;
			this.backButton.PositionScale_Y = 1f;
			this.backButton.SizeOffset_X = 200f;
			this.backButton.SizeOffset_Y = 50f;
			this.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			this.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			this.backButton.onClickedButton += new ClickedButton(this.onClickedBackButton);
			this.backButton.fontSize = 3;
			this.backButton.iconColor = 2;
			base.AddChild(this.backButton);
			this.defaultButton = Glazier.Get().CreateButton();
			this.defaultButton.PositionOffset_X = -200f;
			this.defaultButton.PositionOffset_Y = -50f;
			this.defaultButton.PositionScale_X = 1f;
			this.defaultButton.PositionScale_Y = 1f;
			this.defaultButton.SizeOffset_X = 200f;
			this.defaultButton.SizeOffset_Y = 50f;
			this.defaultButton.Text = MenuPlayConfigUI.localization.format("Default");
			this.defaultButton.TooltipText = MenuPlayConfigUI.localization.format("Default_Tooltip");
			this.defaultButton.OnClicked += new ClickedButton(this.onClickedDefaultButton);
			this.defaultButton.FontSize = 3;
			base.AddChild(this.defaultButton);
			this.updateAll();
		}

		// Token: 0x0400288A RID: 10378
		public Local localization;

		// Token: 0x0400288B RID: 10379
		public bool active;

		// Token: 0x0400288C RID: 10380
		private SleekButtonIcon backButton;

		// Token: 0x0400288D RID: 10381
		private ISleekButton defaultButton;

		// Token: 0x0400288E RID: 10382
		private ISleekScrollView audioBox;

		// Token: 0x0400288F RID: 10383
		private ISleekSlider masterVolumeSlider;

		// Token: 0x04002890 RID: 10384
		private ISleekSlider unfocusedVolumeSlider;

		// Token: 0x04002891 RID: 10385
		private ISleekSlider musicMasterVolumeSlider;

		// Token: 0x04002892 RID: 10386
		private ISleekSlider loadingScreenMusicVolumeSlider;

		// Token: 0x04002893 RID: 10387
		private ISleekSlider deathMusicVolumeSlider;

		// Token: 0x04002894 RID: 10388
		private ISleekSlider mainMenuMusicVolumeSlider;

		// Token: 0x04002895 RID: 10389
		private ISleekSlider ambientMusicVolumeSlider;

		// Token: 0x04002896 RID: 10390
		private ISleekSlider gameVolumeSlider;

		// Token: 0x04002897 RID: 10391
		private ISleekSlider voiceVolumeSlider;

		// Token: 0x04002898 RID: 10392
		private ISleekSlider atmosphereVolumeSlider;
	}
}
