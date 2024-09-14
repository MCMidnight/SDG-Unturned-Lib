using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007C4 RID: 1988
	public class PlayerDashboardSkillsUI
	{
		// Token: 0x06004353 RID: 17235 RVA: 0x0017B526 File Offset: 0x00179726
		public static void open()
		{
			if (PlayerDashboardSkillsUI.active)
			{
				return;
			}
			PlayerDashboardSkillsUI.active = true;
			PlayerDashboardSkillsUI.updateSelection(PlayerDashboardSkillsUI.selectedSpeciality);
			PlayerDashboardSkillsUI.container.AnimateIntoView();
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x0017B54A File Offset: 0x0017974A
		public static void close()
		{
			if (!PlayerDashboardSkillsUI.active)
			{
				return;
			}
			PlayerDashboardSkillsUI.active = false;
			PlayerDashboardSkillsUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x0017B570 File Offset: 0x00179770
		private static void updateSelection(byte specialityIndex)
		{
			PlayerDashboardSkillsUI.skills = Player.player.skills.skills[(int)specialityIndex];
			PlayerDashboardSkillsUI.skillsScrollBox.RemoveAllChildren();
			PlayerDashboardSkillsUI.skillsScrollBox.ContentSizeOffset = new Vector2(0f, (float)(PlayerDashboardSkillsUI.skills.Length * 90 - 10));
			byte b = 0;
			while ((int)b < PlayerDashboardSkillsUI.skills.Length)
			{
				Skill skill = PlayerDashboardSkillsUI.skills[(int)b];
				SleekSkill sleekSkill = new SleekSkill(specialityIndex, b, skill);
				sleekSkill.PositionOffset_Y = (float)(b * 90);
				sleekSkill.SizeOffset_Y = 80f;
				sleekSkill.SizeScale_X = 1f;
				sleekSkill.onClickedButton += new ClickedButton(PlayerDashboardSkillsUI.onClickedSkillButton);
				PlayerDashboardSkillsUI.skillsScrollBox.AddChild(sleekSkill);
				b += 1;
			}
			if (PlayerDashboardSkillsUI.boostButton != null)
			{
				PlayerDashboardSkillsUI.backdropBox.RemoveChild(PlayerDashboardSkillsUI.boostButton);
			}
			PlayerDashboardSkillsUI.boostButton = new SleekBoost((byte)Player.player.skills.boost);
			PlayerDashboardSkillsUI.boostButton.PositionOffset_X = 5f;
			PlayerDashboardSkillsUI.boostButton.PositionOffset_Y = -90f;
			PlayerDashboardSkillsUI.boostButton.PositionScale_X = 0.5f;
			PlayerDashboardSkillsUI.boostButton.PositionScale_Y = 1f;
			PlayerDashboardSkillsUI.boostButton.SizeOffset_X = -15f;
			PlayerDashboardSkillsUI.boostButton.SizeOffset_Y = 80f;
			PlayerDashboardSkillsUI.boostButton.SizeScale_X = 0.5f;
			PlayerDashboardSkillsUI.boostButton.onClickedButton += new ClickedButton(PlayerDashboardSkillsUI.onClickedBoostButton);
			PlayerDashboardSkillsUI.backdropBox.AddChild(PlayerDashboardSkillsUI.boostButton);
			PlayerDashboardSkillsUI.selectedSpeciality = specialityIndex;
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x0017B6E5 File Offset: 0x001798E5
		private static void onClickedSpecialityButton(ISleekElement button)
		{
			PlayerDashboardSkillsUI.updateSelection((byte)((button.PositionOffset_X + 85f) / 60f));
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x0017B6FF File Offset: 0x001798FF
		private static void onClickedBoostButton(ISleekElement button)
		{
			if (Player.player.skills.experience >= PlayerSkills.BOOST_COST)
			{
				Player.player.skills.sendBoost();
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x0017B728 File Offset: 0x00179928
		private static void onClickedSkillButton(ISleekElement button)
		{
			byte b = (byte)(button.PositionOffset_Y / 90f);
			if ((int)PlayerDashboardSkillsUI.skills[(int)b].level < PlayerDashboardSkillsUI.skills[(int)b].GetClampedMaxUnlockableLevel() && Player.player.skills.experience >= Player.player.skills.cost((int)PlayerDashboardSkillsUI.selectedSpeciality, (int)b))
			{
				Player.player.skills.sendUpgrade(PlayerDashboardSkillsUI.selectedSpeciality, b, InputEx.GetKey(ControlsSettings.other));
			}
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x0017B7A2 File Offset: 0x001799A2
		private static void onExperienceUpdated(uint newExperience)
		{
			PlayerDashboardSkillsUI.experienceBox.Text = PlayerDashboardSkillsUI.localization.format("Experience", newExperience.ToString());
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x0017B7C4 File Offset: 0x001799C4
		private static void onBoostUpdated(EPlayerBoost newBoost)
		{
			if (!PlayerDashboardSkillsUI.active || !PlayerDashboardUI.active)
			{
				return;
			}
			PlayerDashboardSkillsUI.updateSelection(PlayerDashboardSkillsUI.selectedSpeciality);
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0017B7DF File Offset: 0x001799DF
		private static void onSkillsUpdated()
		{
			if (!PlayerDashboardSkillsUI.active || !PlayerDashboardUI.active)
			{
				return;
			}
			PlayerDashboardSkillsUI.updateSelection(PlayerDashboardSkillsUI.selectedSpeciality);
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0017B7FC File Offset: 0x001799FC
		public PlayerDashboardSkillsUI()
		{
			if (PlayerDashboardSkillsUI.icons != null)
			{
				PlayerDashboardSkillsUI.icons.unload();
			}
			PlayerDashboardSkillsUI.localization = Localization.read("/Player/PlayerDashboardSkills.dat");
			PlayerDashboardSkillsUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardSkills/PlayerDashboardSkills.unity3d");
			PlayerDashboardSkillsUI.container = new SleekFullscreenBox();
			PlayerDashboardSkillsUI.container.PositionScale_Y = 1f;
			PlayerDashboardSkillsUI.container.PositionOffset_X = 10f;
			PlayerDashboardSkillsUI.container.PositionOffset_Y = 10f;
			PlayerDashboardSkillsUI.container.SizeOffset_X = -20f;
			PlayerDashboardSkillsUI.container.SizeOffset_Y = -20f;
			PlayerDashboardSkillsUI.container.SizeScale_X = 1f;
			PlayerDashboardSkillsUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerDashboardSkillsUI.container);
			PlayerDashboardSkillsUI.active = false;
			PlayerDashboardSkillsUI.selectedSpeciality = byte.MaxValue;
			PlayerDashboardSkillsUI.backdropBox = Glazier.Get().CreateBox();
			PlayerDashboardSkillsUI.backdropBox.PositionOffset_Y = 60f;
			PlayerDashboardSkillsUI.backdropBox.SizeOffset_Y = -60f;
			PlayerDashboardSkillsUI.backdropBox.SizeScale_X = 1f;
			PlayerDashboardSkillsUI.backdropBox.SizeScale_Y = 1f;
			PlayerDashboardSkillsUI.backdropBox.BackgroundColor = new SleekColor(1, 0.5f);
			PlayerDashboardSkillsUI.container.AddChild(PlayerDashboardSkillsUI.backdropBox);
			PlayerDashboardSkillsUI.experienceBox = Glazier.Get().CreateBox();
			PlayerDashboardSkillsUI.experienceBox.PositionOffset_X = 10f;
			PlayerDashboardSkillsUI.experienceBox.PositionOffset_Y = -90f;
			PlayerDashboardSkillsUI.experienceBox.PositionScale_Y = 1f;
			PlayerDashboardSkillsUI.experienceBox.SizeOffset_X = -15f;
			PlayerDashboardSkillsUI.experienceBox.SizeOffset_Y = 80f;
			PlayerDashboardSkillsUI.experienceBox.SizeScale_X = 0.5f;
			PlayerDashboardSkillsUI.experienceBox.FontSize = 3;
			PlayerDashboardSkillsUI.backdropBox.AddChild(PlayerDashboardSkillsUI.experienceBox);
			for (int i = 0; i < (int)PlayerSkills.SPECIALITIES; i++)
			{
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(PlayerDashboardSkillsUI.icons.load<Texture2D>("Speciality_" + i.ToString()));
				sleekButtonIcon.PositionOffset_X = (float)(-85 + i * 60);
				sleekButtonIcon.PositionOffset_Y = 10f;
				sleekButtonIcon.PositionScale_X = 0.5f;
				sleekButtonIcon.SizeOffset_X = 50f;
				sleekButtonIcon.SizeOffset_Y = 50f;
				sleekButtonIcon.tooltip = PlayerDashboardSkillsUI.localization.format("Speciality_" + i.ToString() + "_Tooltip");
				sleekButtonIcon.iconColor = 2;
				sleekButtonIcon.onClickedButton += new ClickedButton(PlayerDashboardSkillsUI.onClickedSpecialityButton);
				PlayerDashboardSkillsUI.backdropBox.AddChild(sleekButtonIcon);
			}
			PlayerDashboardSkillsUI.skillsScrollBox = Glazier.Get().CreateScrollView();
			PlayerDashboardSkillsUI.skillsScrollBox.PositionOffset_X = 10f;
			PlayerDashboardSkillsUI.skillsScrollBox.PositionOffset_Y = 70f;
			PlayerDashboardSkillsUI.skillsScrollBox.SizeOffset_X = -20f;
			PlayerDashboardSkillsUI.skillsScrollBox.SizeOffset_Y = -170f;
			PlayerDashboardSkillsUI.skillsScrollBox.SizeScale_X = 1f;
			PlayerDashboardSkillsUI.skillsScrollBox.SizeScale_Y = 1f;
			PlayerDashboardSkillsUI.skillsScrollBox.ScaleContentToWidth = true;
			PlayerDashboardSkillsUI.backdropBox.AddChild(PlayerDashboardSkillsUI.skillsScrollBox);
			PlayerDashboardSkillsUI.boostButton = null;
			PlayerDashboardSkillsUI.updateSelection(0);
			PlayerSkills playerSkills = Player.player.skills;
			playerSkills.onExperienceUpdated = (ExperienceUpdated)Delegate.Combine(playerSkills.onExperienceUpdated, new ExperienceUpdated(PlayerDashboardSkillsUI.onExperienceUpdated));
			PlayerDashboardSkillsUI.onExperienceUpdated(Player.player.skills.experience);
			PlayerSkills playerSkills2 = Player.player.skills;
			playerSkills2.onBoostUpdated = (BoostUpdated)Delegate.Combine(playerSkills2.onBoostUpdated, new BoostUpdated(PlayerDashboardSkillsUI.onBoostUpdated));
			PlayerSkills playerSkills3 = Player.player.skills;
			playerSkills3.onSkillsUpdated = (SkillsUpdated)Delegate.Combine(playerSkills3.onSkillsUpdated, new SkillsUpdated(PlayerDashboardSkillsUI.onSkillsUpdated));
		}

		// Token: 0x04002C8B RID: 11403
		public static Local localization;

		// Token: 0x04002C8C RID: 11404
		public static Bundle icons;

		// Token: 0x04002C8D RID: 11405
		private static SleekFullscreenBox container;

		// Token: 0x04002C8E RID: 11406
		public static bool active;

		// Token: 0x04002C8F RID: 11407
		private static ISleekBox backdropBox;

		// Token: 0x04002C90 RID: 11408
		private static Skill[] skills;

		// Token: 0x04002C91 RID: 11409
		private static ISleekScrollView skillsScrollBox;

		// Token: 0x04002C92 RID: 11410
		private static SleekBoost boostButton;

		// Token: 0x04002C93 RID: 11411
		private static ISleekBox experienceBox;

		// Token: 0x04002C94 RID: 11412
		private static byte selectedSpeciality;
	}
}
