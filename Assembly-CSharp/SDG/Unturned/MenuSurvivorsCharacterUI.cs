using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007A8 RID: 1960
	public class MenuSurvivorsCharacterUI
	{
		// Token: 0x060041B5 RID: 16821 RVA: 0x00161165 File Offset: 0x0015F365
		public static void open()
		{
			if (MenuSurvivorsCharacterUI.active)
			{
				return;
			}
			MenuSurvivorsCharacterUI.active = true;
			MenuSurvivorsCharacterUI.container.AnimateIntoView();
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x0016117F File Offset: 0x0015F37F
		public static void close()
		{
			if (!MenuSurvivorsCharacterUI.active)
			{
				return;
			}
			MenuSurvivorsCharacterUI.active = false;
			MenuSurvivorsCharacterUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x001611A4 File Offset: 0x0015F3A4
		private static void onCharacterUpdated(byte index, Character character)
		{
			if (index == Characters.selected)
			{
				MenuSurvivorsCharacterUI.nameField.Text = character.name;
				MenuSurvivorsCharacterUI.nickField.Text = character.nick;
				SleekBoxIcon sleekBoxIcon = MenuSurvivorsCharacterUI.skillsetBox;
				Bundle bundle = MenuSurvivorsCharacterUI.icons;
				string text = "Skillset_";
				int skillset = (int)character.skillset;
				sleekBoxIcon.icon = bundle.load<Texture2D>(text + skillset.ToString());
				MenuSurvivorsCharacterUI.skillsetBox.text = MenuSurvivorsCharacterUI.localization.format("Skillset_" + ((byte)character.skillset).ToString());
			}
			MenuSurvivorsCharacterUI.characterButtons[(int)index].updateCharacter(character);
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x0016123F File Offset: 0x0015F43F
		private static void onTypedNameField(ISleekField field, string text)
		{
			Characters.rename(text);
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x00161247 File Offset: 0x0015F447
		private static void onTypedNickField(ISleekField field, string text)
		{
			Characters.renick(text);
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x0016124F File Offset: 0x0015F44F
		private static void onClickedCharacter(SleekCharacter character, byte index)
		{
			Characters.selected = index;
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x00161257 File Offset: 0x0015F457
		private static void onClickedSkillset(ISleekElement button)
		{
			Characters.skillify((EPlayerSkillset)(button.PositionOffset_Y / 40f));
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x0016126B File Offset: 0x0015F46B
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsUI.open();
			MenuSurvivorsCharacterUI.close();
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00161277 File Offset: 0x0015F477
		public void OnDestroy()
		{
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Remove(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsCharacterUI.onCharacterUpdated));
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x0016129C File Offset: 0x0015F49C
		public MenuSurvivorsCharacterUI()
		{
			if (MenuSurvivorsCharacterUI.icons != null)
			{
				MenuSurvivorsCharacterUI.icons.unload();
			}
			MenuSurvivorsCharacterUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsCharacter.dat");
			MenuSurvivorsCharacterUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsCharacter/MenuSurvivorsCharacter.unity3d");
			MenuSurvivorsCharacterUI.container = new SleekFullscreenBox();
			MenuSurvivorsCharacterUI.container.PositionOffset_X = 10f;
			MenuSurvivorsCharacterUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsCharacterUI.container.PositionScale_Y = 1f;
			MenuSurvivorsCharacterUI.container.SizeOffset_X = -20f;
			MenuSurvivorsCharacterUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsCharacterUI.container.SizeScale_X = 1f;
			MenuSurvivorsCharacterUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsCharacterUI.container);
			MenuSurvivorsCharacterUI.active = false;
			MenuSurvivorsCharacterUI.characterBox = Glazier.Get().CreateScrollView();
			MenuSurvivorsCharacterUI.characterBox.PositionOffset_X = -100f;
			MenuSurvivorsCharacterUI.characterBox.PositionOffset_Y = 45f;
			MenuSurvivorsCharacterUI.characterBox.PositionScale_X = 0.75f;
			MenuSurvivorsCharacterUI.characterBox.PositionScale_Y = 0.5f;
			MenuSurvivorsCharacterUI.characterBox.SizeOffset_X = 230f;
			MenuSurvivorsCharacterUI.characterBox.SizeOffset_Y = -145f;
			MenuSurvivorsCharacterUI.characterBox.SizeScale_Y = 0.5f;
			MenuSurvivorsCharacterUI.characterBox.ScaleContentToWidth = true;
			MenuSurvivorsCharacterUI.characterBox.ContentSizeOffset = new Vector2(0f, (float)((Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS) * 80 - 10));
			MenuSurvivorsCharacterUI.container.AddChild(MenuSurvivorsCharacterUI.characterBox);
			MenuSurvivorsCharacterUI.characterButtons = new SleekCharacter[(int)(Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS)];
			byte b = 0;
			while ((int)b < MenuSurvivorsCharacterUI.characterButtons.Length)
			{
				SleekCharacter sleekCharacter = new SleekCharacter(b);
				sleekCharacter.PositionOffset_Y = (float)(b * 80);
				sleekCharacter.SizeOffset_X = 200f;
				sleekCharacter.SizeOffset_Y = 70f;
				sleekCharacter.onClickedCharacter = new ClickedCharacter(MenuSurvivorsCharacterUI.onClickedCharacter);
				MenuSurvivorsCharacterUI.characterBox.AddChild(sleekCharacter);
				MenuSurvivorsCharacterUI.characterButtons[(int)b] = sleekCharacter;
				b += 1;
			}
			MenuSurvivorsCharacterUI.nameField = Glazier.Get().CreateStringField();
			MenuSurvivorsCharacterUI.nameField.PositionOffset_X = -100f;
			MenuSurvivorsCharacterUI.nameField.PositionOffset_Y = 100f;
			MenuSurvivorsCharacterUI.nameField.PositionScale_X = 0.75f;
			MenuSurvivorsCharacterUI.nameField.SizeOffset_X = 200f;
			MenuSurvivorsCharacterUI.nameField.SizeOffset_Y = 30f;
			MenuSurvivorsCharacterUI.nameField.MaxLength = 32;
			MenuSurvivorsCharacterUI.nameField.AddLabel(MenuSurvivorsCharacterUI.localization.format("Name_Field_Label"), 0);
			MenuSurvivorsCharacterUI.nameField.OnTextChanged += new Typed(MenuSurvivorsCharacterUI.onTypedNameField);
			MenuSurvivorsCharacterUI.container.AddChild(MenuSurvivorsCharacterUI.nameField);
			MenuSurvivorsCharacterUI.nickField = Glazier.Get().CreateStringField();
			MenuSurvivorsCharacterUI.nickField.PositionOffset_X = -100f;
			MenuSurvivorsCharacterUI.nickField.PositionOffset_Y = 140f;
			MenuSurvivorsCharacterUI.nickField.PositionScale_X = 0.75f;
			MenuSurvivorsCharacterUI.nickField.SizeOffset_X = 200f;
			MenuSurvivorsCharacterUI.nickField.SizeOffset_Y = 30f;
			MenuSurvivorsCharacterUI.nickField.MaxLength = 32;
			MenuSurvivorsCharacterUI.nickField.AddLabel(MenuSurvivorsCharacterUI.localization.format("Nick_Field_Label"), 0);
			MenuSurvivorsCharacterUI.nickField.OnTextChanged += new Typed(MenuSurvivorsCharacterUI.onTypedNickField);
			MenuSurvivorsCharacterUI.container.AddChild(MenuSurvivorsCharacterUI.nickField);
			MenuSurvivorsCharacterUI.skillsetBox = new SleekBoxIcon(null);
			MenuSurvivorsCharacterUI.skillsetBox.PositionOffset_X = -100f;
			MenuSurvivorsCharacterUI.skillsetBox.PositionOffset_Y = 180f;
			MenuSurvivorsCharacterUI.skillsetBox.PositionScale_X = 0.75f;
			MenuSurvivorsCharacterUI.skillsetBox.SizeOffset_X = 200f;
			MenuSurvivorsCharacterUI.skillsetBox.SizeOffset_Y = 30f;
			MenuSurvivorsCharacterUI.skillsetBox.iconColor = 2;
			MenuSurvivorsCharacterUI.skillsetBox.AddLabel(MenuSurvivorsCharacterUI.localization.format("Skillset_Box_Label"), 0);
			MenuSurvivorsCharacterUI.container.AddChild(MenuSurvivorsCharacterUI.skillsetBox);
			MenuSurvivorsCharacterUI.skillsetsBox = Glazier.Get().CreateScrollView();
			MenuSurvivorsCharacterUI.skillsetsBox.PositionOffset_X = -100f;
			MenuSurvivorsCharacterUI.skillsetsBox.PositionOffset_Y = 220f;
			MenuSurvivorsCharacterUI.skillsetsBox.PositionScale_X = 0.75f;
			MenuSurvivorsCharacterUI.skillsetsBox.SizeOffset_X = 230f;
			MenuSurvivorsCharacterUI.skillsetsBox.SizeOffset_Y = -185f;
			MenuSurvivorsCharacterUI.skillsetsBox.SizeScale_Y = 0.5f;
			MenuSurvivorsCharacterUI.skillsetsBox.ScaleContentToWidth = true;
			MenuSurvivorsCharacterUI.skillsetsBox.ContentSizeOffset = new Vector2(0f, (float)(Customization.SKILLSETS * 40 - 10));
			MenuSurvivorsCharacterUI.container.AddChild(MenuSurvivorsCharacterUI.skillsetsBox);
			for (int i = 0; i < (int)Customization.SKILLSETS; i++)
			{
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuSurvivorsCharacterUI.icons.load<Texture2D>("Skillset_" + i.ToString()));
				sleekButtonIcon.PositionOffset_Y = (float)(i * 40);
				sleekButtonIcon.SizeOffset_X = 200f;
				sleekButtonIcon.SizeOffset_Y = 30f;
				sleekButtonIcon.text = MenuSurvivorsCharacterUI.localization.format("Skillset_" + i.ToString());
				sleekButtonIcon.iconColor = 2;
				sleekButtonIcon.onClickedButton += new ClickedButton(MenuSurvivorsCharacterUI.onClickedSkillset);
				MenuSurvivorsCharacterUI.skillsetsBox.AddChild(sleekButtonIcon);
			}
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Combine(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsCharacterUI.onCharacterUpdated));
			MenuSurvivorsCharacterUI.onCharacterUpdated(Characters.selected, Characters.list[(int)Characters.selected]);
			MenuSurvivorsCharacterUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsCharacterUI.backButton.PositionOffset_Y = -50f;
			MenuSurvivorsCharacterUI.backButton.PositionScale_Y = 1f;
			MenuSurvivorsCharacterUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsCharacterUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsCharacterUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsCharacterUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsCharacterUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsCharacterUI.onClickedBackButton);
			MenuSurvivorsCharacterUI.backButton.fontSize = 3;
			MenuSurvivorsCharacterUI.backButton.iconColor = 2;
			MenuSurvivorsCharacterUI.container.AddChild(MenuSurvivorsCharacterUI.backButton);
		}

		// Token: 0x04002AA8 RID: 10920
		public static Local localization;

		// Token: 0x04002AA9 RID: 10921
		public static Bundle icons;

		// Token: 0x04002AAA RID: 10922
		private static SleekFullscreenBox container;

		// Token: 0x04002AAB RID: 10923
		public static bool active;

		// Token: 0x04002AAC RID: 10924
		private static SleekButtonIcon backButton;

		// Token: 0x04002AAD RID: 10925
		private static ISleekScrollView characterBox;

		// Token: 0x04002AAE RID: 10926
		private static SleekCharacter[] characterButtons;

		// Token: 0x04002AAF RID: 10927
		private static ISleekField nameField;

		// Token: 0x04002AB0 RID: 10928
		private static ISleekField nickField;

		// Token: 0x04002AB1 RID: 10929
		private static SleekBoxIcon skillsetBox;

		// Token: 0x04002AB2 RID: 10930
		private static ISleekScrollView skillsetsBox;
	}
}
