using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200072E RID: 1838
	public class SleekSkill : SleekWrapper
	{
		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06003C8F RID: 15503 RVA: 0x0011FE34 File Offset: 0x0011E034
		// (remove) Token: 0x06003C90 RID: 15504 RVA: 0x0011FE6C File Offset: 0x0011E06C
		public event ClickedButton onClickedButton;

		// Token: 0x06003C91 RID: 15505 RVA: 0x0011FEA4 File Offset: 0x0011E0A4
		public SleekSkill(byte speciality, byte index, Skill skill)
		{
			uint num = Player.player.skills.cost((int)speciality, (int)index);
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.TooltipText = PlayerDashboardSkillsUI.localization.format(string.Concat(new string[]
			{
				"Speciality_",
				speciality.ToString(),
				"_Skill_",
				index.ToString(),
				"_Tooltip"
			}));
			this.button.OnClicked += new ClickedButton(this.onClickedInternalButton);
			this.button.IsClickable = (Player.player.skills.experience >= num && (int)skill.level < skill.GetClampedMaxUnlockableLevel());
			base.AddChild(this.button);
			byte b = 0;
			while ((int)b < skill.GetClampedMaxUnlockableLevel())
			{
				ISleekImage sleekImage = Glazier.Get().CreateImage();
				sleekImage.PositionOffset_X = (float)(-20 - (int)(b * 20));
				sleekImage.PositionOffset_Y = 10f;
				sleekImage.PositionScale_X = 1f;
				sleekImage.SizeOffset_X = 10f;
				sleekImage.SizeOffset_Y = -10f;
				sleekImage.SizeScale_Y = 0.5f;
				if (b < skill.level)
				{
					sleekImage.Texture = PlayerDashboardSkillsUI.icons.load<Texture2D>("Unlocked");
				}
				else
				{
					sleekImage.Texture = PlayerDashboardSkillsUI.icons.load<Texture2D>("Locked");
				}
				base.AddChild(sleekImage);
				b += 1;
			}
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 5f;
			sleekLabel.PositionOffset_Y = 5f;
			sleekLabel.SizeOffset_X = -10f;
			sleekLabel.SizeOffset_Y = 30f;
			sleekLabel.SizeScale_X = 0.5f;
			sleekLabel.TextAlignment = 0;
			sleekLabel.Text = PlayerDashboardSkillsUI.localization.format("Skill", PlayerDashboardSkillsUI.localization.format("Speciality_" + speciality.ToString() + "_Skill_" + index.ToString()), PlayerDashboardSkillsUI.localization.format("Level_" + skill.level.ToString()));
			sleekLabel.FontSize = 3;
			base.AddChild(sleekLabel);
			ISleekImage sleekImage2 = Glazier.Get().CreateImage();
			sleekImage2.PositionOffset_X = 10f;
			sleekImage2.PositionOffset_Y = -10f;
			sleekImage2.PositionScale_Y = 0.5f;
			sleekImage2.SizeOffset_X = 20f;
			sleekImage2.SizeOffset_Y = 20f;
			sleekImage2.TintColor = 2;
			byte b2 = 0;
			while ((int)b2 < PlayerSkills.SKILLSETS.Length)
			{
				byte b3 = 0;
				while ((int)b3 < PlayerSkills.SKILLSETS[(int)b2].Length)
				{
					SpecialitySkillPair specialitySkillPair = PlayerSkills.SKILLSETS[(int)b2][(int)b3];
					if ((int)speciality == specialitySkillPair.speciality && (int)index == specialitySkillPair.skill)
					{
						sleekImage2.Texture = MenuSurvivorsCharacterUI.icons.load<Texture2D>("Skillset_" + b2.ToString());
						break;
					}
					b3 += 1;
				}
				b2 += 1;
			}
			base.AddChild(sleekImage2);
			ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
			sleekLabel2.PositionOffset_X = 5f;
			sleekLabel2.PositionOffset_Y = -35f;
			sleekLabel2.PositionScale_Y = 1f;
			sleekLabel2.SizeOffset_X = -10f;
			sleekLabel2.SizeOffset_Y = 30f;
			sleekLabel2.SizeScale_X = 0.5f;
			sleekLabel2.TextAlignment = 6;
			sleekLabel2.Text = PlayerDashboardSkillsUI.localization.format(string.Concat(new string[]
			{
				"Speciality_",
				speciality.ToString(),
				"_Skill_",
				index.ToString(),
				"_Tooltip"
			}));
			base.AddChild(sleekLabel2);
			if (skill.level > 0)
			{
				ISleekLabel sleekLabel3 = Glazier.Get().CreateLabel();
				sleekLabel3.PositionOffset_X = 5f;
				sleekLabel3.PositionOffset_Y = 5f;
				sleekLabel3.PositionScale_X = 0.25f;
				sleekLabel3.SizeOffset_X = -10f;
				sleekLabel3.SizeOffset_Y = -10f;
				sleekLabel3.SizeScale_X = 0.5f;
				sleekLabel3.SizeScale_Y = 0.5f;
				sleekLabel3.TextAlignment = 4;
				sleekLabel3.Text = PlayerDashboardSkillsUI.localization.format("Bonus_Current", PlayerDashboardSkillsUI.localization.format(string.Concat(new string[]
				{
					"Speciality_",
					speciality.ToString(),
					"_Skill_",
					index.ToString(),
					"_Level_",
					skill.level.ToString()
				})));
				base.AddChild(sleekLabel3);
			}
			if ((int)skill.level < skill.GetClampedMaxUnlockableLevel())
			{
				ISleekLabel sleekLabel4 = Glazier.Get().CreateLabel();
				sleekLabel4.PositionOffset_X = 5f;
				sleekLabel4.PositionOffset_Y = 5f;
				sleekLabel4.PositionScale_X = 0.25f;
				sleekLabel4.PositionScale_Y = 0.5f;
				sleekLabel4.SizeOffset_X = -10f;
				sleekLabel4.SizeOffset_Y = -10f;
				sleekLabel4.SizeScale_X = 0.5f;
				sleekLabel4.SizeScale_Y = 0.5f;
				sleekLabel4.TextAlignment = 4;
				sleekLabel4.Text = PlayerDashboardSkillsUI.localization.format("Bonus_Next", PlayerDashboardSkillsUI.localization.format(string.Concat(new string[]
				{
					"Speciality_",
					speciality.ToString(),
					"_Skill_",
					index.ToString(),
					"_Level_",
					((int)(skill.level + 1)).ToString()
				})));
				base.AddChild(sleekLabel4);
			}
			ISleekLabel sleekLabel5 = Glazier.Get().CreateLabel();
			sleekLabel5.PositionOffset_X = 5f;
			sleekLabel5.PositionOffset_Y = -35f;
			sleekLabel5.PositionScale_X = 0.5f;
			sleekLabel5.PositionScale_Y = 1f;
			sleekLabel5.SizeOffset_X = -10f;
			sleekLabel5.SizeOffset_Y = 30f;
			sleekLabel5.SizeScale_X = 0.5f;
			sleekLabel5.TextAlignment = 8;
			if ((int)skill.level < skill.GetClampedMaxUnlockableLevel())
			{
				sleekLabel5.Text = PlayerDashboardSkillsUI.localization.format("Cost", num);
			}
			else
			{
				sleekLabel5.Text = PlayerDashboardSkillsUI.localization.format("Full");
			}
			base.AddChild(sleekLabel5);
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x001204F3 File Offset: 0x0011E6F3
		private void onClickedInternalButton(ISleekElement internalButton)
		{
			ClickedButton clickedButton = this.onClickedButton;
			if (clickedButton == null)
			{
				return;
			}
			clickedButton.Invoke(this);
		}

		// Token: 0x040025EF RID: 9711
		private ISleekButton button;
	}
}
