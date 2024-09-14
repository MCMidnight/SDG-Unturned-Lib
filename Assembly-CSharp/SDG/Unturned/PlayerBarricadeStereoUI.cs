using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007BF RID: 1983
	public class PlayerBarricadeStereoUI : SleekFullscreenBox
	{
		// Token: 0x060042B0 RID: 17072 RVA: 0x00170954 File Offset: 0x0016EB54
		public void open(InteractableStereo newStereo)
		{
			if (this.active)
			{
				this.close();
				return;
			}
			this.active = true;
			this.stereo = newStereo;
			this.hasPendingVolumeUpdate = false;
			this.refreshSongs();
			if (this.stereo != null)
			{
				this.volumeSlider.Value = this.stereo.volume;
			}
			this.updateVolumeSliderLabel();
			base.AnimateIntoView();
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x001709BC File Offset: 0x0016EBBC
		public void close()
		{
			if (!this.active)
			{
				return;
			}
			if (this.stereo != null && this.hasPendingVolumeUpdate)
			{
				this.hasPendingVolumeUpdate = false;
				this.stereo.ClientSetVolume(this.stereo.compressedVolume);
			}
			this.active = false;
			this.stereo = null;
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00170A23 File Offset: 0x0016EC23
		private void refreshSongs()
		{
			if (Assets.HasDefaultAssetMappingChanged(ref this.assetListChangeCounter))
			{
				this.songs.Clear();
				Assets.FindAssetsByType_UseDefaultAssetMapping<StereoSongAsset>(this.songs);
				this.songsBox.NotifyDataChanged();
			}
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x00170A53 File Offset: 0x0016EC53
		private void updateVolumeSliderLabel()
		{
			if (this.stereo != null)
			{
				this.volumeSlider.UpdateLabel(this.localization.format("Volume_Slider_Label", this.stereo.compressedVolume));
			}
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x00170A8E File Offset: 0x0016EC8E
		private void onDraggedVolumeSlider(ISleekSlider slider, float state)
		{
			if (this.stereo != null)
			{
				this.stereo.volume = state;
				this.hasPendingVolumeUpdate = true;
				this.updateVolumeSliderLabel();
			}
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x00170AB7 File Offset: 0x0016ECB7
		private void onClickedStopButton(ISleekElement button)
		{
			if (this.stereo != null)
			{
				this.stereo.ClientSetTrack(Guid.Empty);
			}
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x00170AD7 File Offset: 0x0016ECD7
		private void onClickedCloseButton(ISleekElement button)
		{
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x00170AE4 File Offset: 0x0016ECE4
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.stereo != null && this.hasPendingVolumeUpdate)
			{
				double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
				if (realtimeSinceStartupAsDouble - this.lastUpdateVolumeRealtime > 0.20000000298023224)
				{
					this.lastUpdateVolumeRealtime = realtimeSinceStartupAsDouble;
					this.stereo.ClientSetVolume(this.stereo.compressedVolume);
					this.hasPendingVolumeUpdate = false;
				}
			}
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x00170B4A File Offset: 0x0016ED4A
		private ISleekElement OnCreateSongElement(StereoSongAsset songAsset)
		{
			return new SleekBoomboxSong(songAsset, this);
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x00170B54 File Offset: 0x0016ED54
		public PlayerBarricadeStereoUI()
		{
			this.localization = Localization.read("/Player/PlayerBarricadeStereo.dat");
			base.PositionScale_Y = 1f;
			base.PositionOffset_X = 10f;
			base.PositionOffset_Y = 10f;
			base.SizeOffset_X = -20f;
			base.SizeOffset_Y = -20f;
			base.SizeScale_X = 1f;
			base.SizeScale_Y = 1f;
			this.active = false;
			this.stereo = null;
			this.stopButton = Glazier.Get().CreateButton();
			this.stopButton.PositionOffset_X = -200f;
			this.stopButton.PositionOffset_Y = 5f;
			this.stopButton.PositionScale_X = 0.5f;
			this.stopButton.PositionScale_Y = 0.9f;
			this.stopButton.SizeOffset_X = 195f;
			this.stopButton.SizeOffset_Y = 30f;
			this.stopButton.Text = this.localization.format("Stop_Button");
			this.stopButton.TooltipText = this.localization.format("Stop_Button_Tooltip");
			this.stopButton.OnClicked += new ClickedButton(this.onClickedStopButton);
			base.AddChild(this.stopButton);
			this.closeButton = Glazier.Get().CreateButton();
			this.closeButton.PositionOffset_X = 5f;
			this.closeButton.PositionOffset_Y = 5f;
			this.closeButton.PositionScale_X = 0.5f;
			this.closeButton.PositionScale_Y = 0.9f;
			this.closeButton.SizeOffset_X = 195f;
			this.closeButton.SizeOffset_Y = 30f;
			this.closeButton.Text = this.localization.format("Close_Button");
			this.closeButton.TooltipText = this.localization.format("Close_Button_Tooltip");
			this.closeButton.OnClicked += new ClickedButton(this.onClickedCloseButton);
			base.AddChild(this.closeButton);
			this.volumeSlider = Glazier.Get().CreateSlider();
			this.volumeSlider.PositionOffset_X = -200f;
			this.volumeSlider.PositionOffset_Y = -25f;
			this.volumeSlider.PositionScale_X = 0.5f;
			this.volumeSlider.PositionScale_Y = 0.1f;
			this.volumeSlider.SizeOffset_X = 250f;
			this.volumeSlider.SizeOffset_Y = 20f;
			this.volumeSlider.Orientation = 0;
			this.volumeSlider.OnValueChanged += new Dragged(this.onDraggedVolumeSlider);
			this.volumeSlider.AddLabel("", 1);
			base.AddChild(this.volumeSlider);
			this.songsBox = new SleekList<StereoSongAsset>();
			this.songsBox.PositionOffset_X = -200f;
			this.songsBox.PositionScale_X = 0.5f;
			this.songsBox.PositionScale_Y = 0.1f;
			this.songsBox.SizeOffset_X = 400f;
			this.songsBox.SizeScale_Y = 0.8f;
			this.songsBox.itemHeight = 30;
			this.songsBox.onCreateElement = new SleekList<StereoSongAsset>.CreateElement(this.OnCreateSongElement);
			this.songsBox.SetData(this.songs);
			base.AddChild(this.songsBox);
		}

		// Token: 0x04002BEF RID: 11247
		private List<StereoSongAsset> songs = new List<StereoSongAsset>();

		// Token: 0x04002BF0 RID: 11248
		private Local localization;

		// Token: 0x04002BF1 RID: 11249
		public bool active;

		// Token: 0x04002BF2 RID: 11250
		internal InteractableStereo stereo;

		/// <summary>
		/// Hack to prevent hitting volume rate limit because (at least as of 2022-05-24) we do not have an event for finished dragging.
		/// </summary>
		// Token: 0x04002BF3 RID: 11251
		private double lastUpdateVolumeRealtime;

		// Token: 0x04002BF4 RID: 11252
		private bool hasPendingVolumeUpdate;

		// Token: 0x04002BF5 RID: 11253
		private int assetListChangeCounter;

		// Token: 0x04002BF6 RID: 11254
		private ISleekButton stopButton;

		// Token: 0x04002BF7 RID: 11255
		private ISleekButton closeButton;

		// Token: 0x04002BF8 RID: 11256
		private ISleekSlider volumeSlider;

		// Token: 0x04002BF9 RID: 11257
		private SleekList<StereoSongAsset> songsBox;
	}
}
