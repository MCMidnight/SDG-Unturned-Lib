using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007BC RID: 1980
	public class PlayerBarricadeMannequinUI : SleekFullscreenBox
	{
		// Token: 0x0600429B RID: 17051 RVA: 0x0016F9FC File Offset: 0x0016DBFC
		public void open(InteractableMannequin newMannequin)
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			this.mannequin = newMannequin;
			this.addButton.Text = this.localization.format("Add_Button", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other));
			this.removeButton.Text = this.localization.format("Remove_Button", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other));
			if (this.mannequin != null)
			{
				this.poseButton.state = (int)this.mannequin.pose;
			}
			base.AnimateIntoView();
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x0016FA94 File Offset: 0x0016DC94
		public void close()
		{
			if (!this.active)
			{
				return;
			}
			this.active = false;
			this.mannequin = null;
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x0016FABD File Offset: 0x0016DCBD
		private void onClickedCosmeticsButton(ISleekElement button)
		{
			if (this.mannequin != null)
			{
				this.mannequin.ClientRequestUpdate(EMannequinUpdateMode.COSMETICS);
			}
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x0016FAE4 File Offset: 0x0016DCE4
		private void onClickedAddButton(ISleekElement button)
		{
			if (this.mannequin != null)
			{
				this.mannequin.ClientRequestUpdate(EMannequinUpdateMode.ADD);
			}
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x0016FB0B File Offset: 0x0016DD0B
		private void onClickedRemoveButton(ISleekElement button)
		{
			if (this.mannequin != null)
			{
				this.mannequin.ClientRequestUpdate(EMannequinUpdateMode.REMOVE);
			}
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x0016FB32 File Offset: 0x0016DD32
		private void onClickedSwapButton(ISleekElement button)
		{
			if (this.mannequin != null)
			{
				this.mannequin.ClientRequestUpdate(EMannequinUpdateMode.SWAP);
			}
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x0016FB5C File Offset: 0x0016DD5C
		private void onSwappedPoseState(SleekButtonState button, int index)
		{
			if (this.mannequin != null)
			{
				this.poseButton.state = (int)this.mannequin.pose;
				byte comp = this.mannequin.getComp(this.mannequin.mirror, (byte)index);
				this.mannequin.ClientSetPose(comp);
			}
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x0016FBB4 File Offset: 0x0016DDB4
		private void onClickedMirrorButton(ISleekElement button)
		{
			if (this.mannequin != null)
			{
				bool flag = this.mannequin.mirror;
				flag = !flag;
				byte comp = this.mannequin.getComp(flag, this.mannequin.pose);
				this.mannequin.ClientSetPose(comp);
			}
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x0016FC04 File Offset: 0x0016DE04
		private void onClickedCancelButton(ISleekElement button)
		{
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x0016FC11 File Offset: 0x0016DE11
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.mannequin != null)
			{
				this.poseButton.state = (int)this.mannequin.pose;
			}
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x0016FC40 File Offset: 0x0016DE40
		public PlayerBarricadeMannequinUI()
		{
			this.localization = Localization.read("/Player/PlayerBarricadeMannequin.dat");
			base.PositionScale_Y = 1f;
			base.PositionOffset_X = 10f;
			base.PositionOffset_Y = 10f;
			base.SizeOffset_X = -20f;
			base.SizeOffset_Y = -20f;
			base.SizeScale_X = 1f;
			base.SizeScale_Y = 1f;
			this.active = false;
			this.mannequin = null;
			this.cosmeticsButton = Glazier.Get().CreateButton();
			this.cosmeticsButton.PositionOffset_X = -100f;
			this.cosmeticsButton.PositionOffset_Y = -135f;
			this.cosmeticsButton.PositionScale_X = 0.5f;
			this.cosmeticsButton.PositionScale_Y = 0.5f;
			this.cosmeticsButton.SizeOffset_X = 200f;
			this.cosmeticsButton.SizeOffset_Y = 30f;
			this.cosmeticsButton.Text = this.localization.format("Cosmetics_Button");
			this.cosmeticsButton.TooltipText = this.localization.format("Cosmetics_Button_Tooltip");
			this.cosmeticsButton.OnClicked += new ClickedButton(this.onClickedCosmeticsButton);
			base.AddChild(this.cosmeticsButton);
			this.addButton = Glazier.Get().CreateButton();
			this.addButton.PositionOffset_X = -100f;
			this.addButton.PositionOffset_Y = -95f;
			this.addButton.PositionScale_X = 0.5f;
			this.addButton.PositionScale_Y = 0.5f;
			this.addButton.SizeOffset_X = 200f;
			this.addButton.SizeOffset_Y = 30f;
			this.addButton.Text = this.localization.format("Add_Button");
			this.addButton.TooltipText = this.localization.format("Add_Button_Tooltip");
			this.addButton.OnClicked += new ClickedButton(this.onClickedAddButton);
			base.AddChild(this.addButton);
			this.removeButton = Glazier.Get().CreateButton();
			this.removeButton.PositionOffset_X = -100f;
			this.removeButton.PositionOffset_Y = -55f;
			this.removeButton.PositionScale_X = 0.5f;
			this.removeButton.PositionScale_Y = 0.5f;
			this.removeButton.SizeOffset_X = 200f;
			this.removeButton.SizeOffset_Y = 30f;
			this.removeButton.TooltipText = this.localization.format("Remove_Button_Tooltip");
			this.removeButton.OnClicked += new ClickedButton(this.onClickedRemoveButton);
			base.AddChild(this.removeButton);
			this.swapButton = Glazier.Get().CreateButton();
			this.swapButton.PositionOffset_X = -100f;
			this.swapButton.PositionOffset_Y = -15f;
			this.swapButton.PositionScale_X = 0.5f;
			this.swapButton.PositionScale_Y = 0.5f;
			this.swapButton.SizeOffset_X = 200f;
			this.swapButton.SizeOffset_Y = 30f;
			this.swapButton.Text = this.localization.format("Swap_Button");
			this.swapButton.TooltipText = this.localization.format("Swap_Button_Tooltip");
			this.swapButton.OnClicked += new ClickedButton(this.onClickedSwapButton);
			base.AddChild(this.swapButton);
			this.poseButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("T")),
				new GUIContent(this.localization.format("Classic")),
				new GUIContent(this.localization.format("Lie"))
			});
			this.poseButton.PositionOffset_X = -100f;
			this.poseButton.PositionOffset_Y = 25f;
			this.poseButton.PositionScale_X = 0.5f;
			this.poseButton.PositionScale_Y = 0.5f;
			this.poseButton.SizeOffset_X = 200f;
			this.poseButton.SizeOffset_Y = 30f;
			this.poseButton.tooltip = this.localization.format("Pose_Button_Tooltip");
			this.poseButton.onSwappedState = new SwappedState(this.onSwappedPoseState);
			base.AddChild(this.poseButton);
			this.mirrorButton = Glazier.Get().CreateButton();
			this.mirrorButton.PositionOffset_X = -100f;
			this.mirrorButton.PositionOffset_Y = 65f;
			this.mirrorButton.PositionScale_X = 0.5f;
			this.mirrorButton.PositionScale_Y = 0.5f;
			this.mirrorButton.SizeOffset_X = 200f;
			this.mirrorButton.SizeOffset_Y = 30f;
			this.mirrorButton.Text = this.localization.format("Mirror_Button");
			this.mirrorButton.TooltipText = this.localization.format("Mirror_Button_Tooltip");
			this.mirrorButton.OnClicked += new ClickedButton(this.onClickedMirrorButton);
			base.AddChild(this.mirrorButton);
			this.cancelButton = Glazier.Get().CreateButton();
			this.cancelButton.PositionOffset_X = -100f;
			this.cancelButton.PositionOffset_Y = 105f;
			this.cancelButton.PositionScale_X = 0.5f;
			this.cancelButton.PositionScale_Y = 0.5f;
			this.cancelButton.SizeOffset_X = 200f;
			this.cancelButton.SizeOffset_Y = 30f;
			this.cancelButton.Text = this.localization.format("Cancel_Button");
			this.cancelButton.TooltipText = this.localization.format("Cancel_Button_Tooltip");
			this.cancelButton.OnClicked += new ClickedButton(this.onClickedCancelButton);
			base.AddChild(this.cancelButton);
		}

		// Token: 0x04002BDC RID: 11228
		private Local localization;

		// Token: 0x04002BDD RID: 11229
		public bool active;

		// Token: 0x04002BDE RID: 11230
		private InteractableMannequin mannequin;

		// Token: 0x04002BDF RID: 11231
		private ISleekButton cosmeticsButton;

		// Token: 0x04002BE0 RID: 11232
		private ISleekButton addButton;

		// Token: 0x04002BE1 RID: 11233
		private ISleekButton removeButton;

		// Token: 0x04002BE2 RID: 11234
		private ISleekButton swapButton;

		// Token: 0x04002BE3 RID: 11235
		private SleekButtonState poseButton;

		// Token: 0x04002BE4 RID: 11236
		private ISleekButton mirrorButton;

		// Token: 0x04002BE5 RID: 11237
		private ISleekButton cancelButton;
	}
}
