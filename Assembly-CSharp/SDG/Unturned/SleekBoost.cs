using System;

namespace SDG.Unturned
{
	// Token: 0x02000700 RID: 1792
	public class SleekBoost : SleekWrapper
	{
		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06003B4D RID: 15181 RVA: 0x00116434 File Offset: 0x00114634
		// (remove) Token: 0x06003B4E RID: 15182 RVA: 0x0011646C File Offset: 0x0011466C
		public event ClickedButton onClickedButton;

		// Token: 0x06003B4F RID: 15183 RVA: 0x001164A4 File Offset: 0x001146A4
		public SleekBoost(byte boost)
		{
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.TooltipText = PlayerDashboardSkillsUI.localization.format("Boost_" + boost.ToString() + "_Tooltip");
			this.button.OnClicked += new ClickedButton(this.onClickedInternalButton);
			this.button.IsClickable = (Player.player.skills.experience >= PlayerSkills.BOOST_COST);
			base.AddChild(this.button);
			this.infoLabel = Glazier.Get().CreateLabel();
			this.infoLabel.PositionOffset_X = 5f;
			this.infoLabel.PositionOffset_Y = 5f;
			this.infoLabel.SizeOffset_X = -10f;
			this.infoLabel.SizeOffset_Y = -5f;
			this.infoLabel.SizeScale_X = 0.5f;
			this.infoLabel.SizeScale_Y = 0.5f;
			this.infoLabel.TextAlignment = 3;
			this.infoLabel.Text = PlayerDashboardSkillsUI.localization.format("Boost_" + boost.ToString());
			this.infoLabel.FontSize = 3;
			base.AddChild(this.infoLabel);
			this.descriptionLabel = Glazier.Get().CreateLabel();
			this.descriptionLabel.PositionOffset_X = 5f;
			this.descriptionLabel.PositionOffset_Y = 5f;
			this.descriptionLabel.PositionScale_Y = 0.5f;
			this.descriptionLabel.SizeOffset_X = -10f;
			this.descriptionLabel.SizeOffset_Y = -5f;
			this.descriptionLabel.SizeScale_X = 0.5f;
			this.descriptionLabel.SizeScale_Y = 0.5f;
			this.descriptionLabel.TextAlignment = 3;
			this.descriptionLabel.Text = PlayerDashboardSkillsUI.localization.format("Boost_" + boost.ToString() + "_Tooltip");
			base.AddChild(this.descriptionLabel);
			if (boost > 0)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.PositionOffset_X = 5f;
				sleekLabel.PositionOffset_Y = 5f;
				sleekLabel.PositionScale_X = 0.25f;
				sleekLabel.SizeOffset_X = -10f;
				sleekLabel.SizeOffset_Y = -10f;
				sleekLabel.SizeScale_X = 0.5f;
				sleekLabel.SizeScale_Y = 1f;
				sleekLabel.TextAlignment = 4;
				sleekLabel.Text = PlayerDashboardSkillsUI.localization.format("Boost_" + boost.ToString() + "_Bonus");
				base.AddChild(sleekLabel);
			}
			this.costLabel = Glazier.Get().CreateLabel();
			this.costLabel.PositionOffset_X = 5f;
			this.costLabel.PositionOffset_Y = 5f;
			this.costLabel.PositionScale_X = 0.5f;
			this.costLabel.SizeOffset_X = -10f;
			this.costLabel.SizeOffset_Y = -10f;
			this.costLabel.SizeScale_X = 0.5f;
			this.costLabel.SizeScale_Y = 1f;
			this.costLabel.TextAlignment = 5;
			this.costLabel.Text = PlayerDashboardSkillsUI.localization.format("Cost", PlayerSkills.BOOST_COST);
			base.AddChild(this.costLabel);
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x00116825 File Offset: 0x00114A25
		private void onClickedInternalButton(ISleekElement internalButton)
		{
			ClickedButton clickedButton = this.onClickedButton;
			if (clickedButton == null)
			{
				return;
			}
			clickedButton.Invoke(this);
		}

		// Token: 0x04002535 RID: 9525
		private ISleekButton button;

		// Token: 0x04002536 RID: 9526
		private ISleekLabel infoLabel;

		// Token: 0x04002537 RID: 9527
		private ISleekLabel descriptionLabel;

		// Token: 0x04002538 RID: 9528
		private ISleekLabel costLabel;
	}
}
