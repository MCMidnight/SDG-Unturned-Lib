using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Button in a list of levels.
	/// </summary>
	// Token: 0x02000723 RID: 1827
	public class SleekLevel : SleekWrapper
	{
		// Token: 0x06003C48 RID: 15432 RVA: 0x0011BC04 File Offset: 0x00119E04
		protected void onClickedButton(ISleekElement button)
		{
			ClickedLevel clickedLevel = this.onClickedLevel;
			if (clickedLevel == null)
			{
				return;
			}
			clickedLevel(this, (byte)(base.PositionOffset_Y / 110f));
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x0011BC24 File Offset: 0x00119E24
		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x0011BC2C File Offset: 0x00119E2C
		public SleekLevel(LevelInfo level)
		{
			this.level = level;
			base.SizeOffset_X = 400f;
			base.SizeOffset_Y = 100f;
			this.button = Glazier.Get().CreateButton();
			this.button.SizeOffset_X = 0f;
			this.button.SizeOffset_Y = 0f;
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.OnClicked += new ClickedButton(this.onClickedButton);
			base.AddChild(this.button);
			this.icon = Glazier.Get().CreateImage();
			this.icon.PositionOffset_X = 10f;
			this.icon.PositionOffset_Y = 10f;
			this.icon.SizeOffset_X = -20f;
			this.icon.SizeOffset_Y = -20f;
			this.icon.SizeScale_X = 1f;
			this.icon.SizeScale_Y = 1f;
			this.icon.Texture = LevelIconCache.GetOrLoadIcon(level);
			this.button.AddChild(this.icon);
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionOffset_Y = 10f;
			this.nameLabel.SizeScale_X = 1f;
			this.nameLabel.SizeOffset_Y = 50f;
			this.nameLabel.TextAlignment = 4;
			this.nameLabel.FontSize = 3;
			this.nameLabel.TextContrastContext = 2;
			this.button.AddChild(this.nameLabel);
			Local localization = level.getLocalization();
			if (localization != null && localization.has("Name"))
			{
				this.nameLabel.Text = localization.format("Name");
			}
			else
			{
				this.nameLabel.Text = level.name;
			}
			this.infoLabel = Glazier.Get().CreateLabel();
			this.infoLabel.PositionOffset_Y = 60f;
			this.infoLabel.SizeScale_X = 1f;
			this.infoLabel.SizeOffset_Y = 30f;
			this.infoLabel.TextAlignment = 4;
			this.infoLabel.TextContrastContext = 2;
			string arg = "#SIZE";
			if (level.size == ELevelSize.TINY)
			{
				arg = MenuPlaySingleplayerUI.localization.format("Tiny");
			}
			else if (level.size == ELevelSize.SMALL)
			{
				arg = MenuPlaySingleplayerUI.localization.format("Small");
			}
			else if (level.size == ELevelSize.MEDIUM)
			{
				arg = MenuPlaySingleplayerUI.localization.format("Medium");
			}
			else if (level.size == ELevelSize.LARGE)
			{
				arg = MenuPlaySingleplayerUI.localization.format("Large");
			}
			else if (level.size == ELevelSize.INSANE)
			{
				arg = MenuPlaySingleplayerUI.localization.format("Insane");
			}
			string arg2 = "#TYPE";
			if (localization != null && localization.has("GameModeLabel"))
			{
				arg2 = localization.format("GameModeLabel");
			}
			else if (level.type == ELevelType.SURVIVAL)
			{
				arg2 = MenuPlaySingleplayerUI.localization.format("Survival");
			}
			else if (level.type == ELevelType.HORDE)
			{
				arg2 = MenuPlaySingleplayerUI.localization.format("Horde");
			}
			else if (level.type == ELevelType.ARENA)
			{
				arg2 = MenuPlaySingleplayerUI.localization.format("Arena");
			}
			this.infoLabel.Text = MenuPlaySingleplayerUI.localization.format("Info_WithVersion", arg, arg2, level.configData.Version);
			this.infoLabel.TextColor = new SleekColor(3, 0.75f);
			this.button.AddChild(this.infoLabel);
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06003C4B RID: 15435 RVA: 0x0011BFB8 File Offset: 0x0011A1B8
		// (set) Token: 0x06003C4C RID: 15436 RVA: 0x0011BFC0 File Offset: 0x0011A1C0
		public LevelInfo level { get; private set; }

		// Token: 0x040025AA RID: 9642
		public ClickedLevel onClickedLevel;

		// Token: 0x040025AC RID: 9644
		protected ISleekButton button;

		// Token: 0x040025AD RID: 9645
		protected ISleekImage icon;

		// Token: 0x040025AE RID: 9646
		protected ISleekLabel nameLabel;

		// Token: 0x040025AF RID: 9647
		protected ISleekLabel infoLabel;
	}
}
