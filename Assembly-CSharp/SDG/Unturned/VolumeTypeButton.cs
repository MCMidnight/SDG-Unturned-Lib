using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000788 RID: 1928
	internal class VolumeTypeButton : SleekWrapper
	{
		// Token: 0x06003FA0 RID: 16288 RVA: 0x00141770 File Offset: 0x0013F970
		public VolumeTypeButton(EditorVolumesUI owner, VolumeManagerBase volumeType)
		{
			this.owner = owner;
			this.volumeType = volumeType;
			this.visibilityButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent("H", owner.localization.format("Visibility_Hidden")),
				new GUIContent("W", owner.localization.format("Visibility_Wireframe")),
				new GUIContent("S", owner.localization.format("Visibility_Solid"))
			});
			this.visibilityButton.SizeOffset_X = 50f;
			this.visibilityButton.SizeOffset_Y = 30f;
			this.visibilityButton.UseContentTooltip = true;
			SleekButtonState sleekButtonState = this.visibilityButton;
			sleekButtonState.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState.onSwappedState, new SwappedState(this.OnSwappedVisibility));
			this.RefreshVisibility();
			base.AddChild(this.visibilityButton);
			this.nameButton = Glazier.Get().CreateButton();
			this.nameButton.PositionOffset_X = 50f;
			this.nameButton.SizeScale_X = 1f;
			this.nameButton.SizeScale_Y = 1f;
			this.nameButton.SizeOffset_X = -this.nameButton.PositionOffset_X;
			this.nameButton.Text = volumeType.FriendlyName;
			this.nameButton.OnClicked += new ClickedButton(this.OnTypeClicked);
			base.AddChild(this.nameButton);
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x001418E9 File Offset: 0x0013FAE9
		public void RefreshVisibility()
		{
			this.visibilityButton.state = (int)this.volumeType.Visibility;
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x00141901 File Offset: 0x0013FB01
		private void OnSwappedVisibility(SleekButtonState button, int state)
		{
			this.volumeType.Visibility = (ELevelVolumeVisibility)state;
			this.owner.RefreshSelectedVisibility();
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x0014191A File Offset: 0x0013FB1A
		private void OnTypeClicked(ISleekElement element)
		{
			this.owner.SetSelectedType(this.volumeType);
		}

		// Token: 0x04002872 RID: 10354
		public EditorVolumesUI owner;

		// Token: 0x04002873 RID: 10355
		public VolumeManagerBase volumeType;

		// Token: 0x04002874 RID: 10356
		private SleekButtonState visibilityButton;

		// Token: 0x04002875 RID: 10357
		private ISleekButton nameButton;
	}
}
