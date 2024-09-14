using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000799 RID: 1945
	public class MenuPlayConfigUI
	{
		// Token: 0x0600409A RID: 16538 RVA: 0x0014ECFC File Offset: 0x0014CEFC
		public static void open()
		{
			if (MenuPlayConfigUI.active)
			{
				return;
			}
			MenuPlayConfigUI.active = true;
			MenuPlayConfigUI.configData = ConfigData.CreateDefault(true);
			string path = "/Worlds/Singleplayer_" + Characters.selected.ToString() + "/Config.json";
			if (ReadWrite.fileExists(path, false))
			{
				try
				{
					ReadWrite.populateJSON(path, MenuPlayConfigUI.configData, true);
				}
				catch (Exception e)
				{
					UnturnedLog.error("Exception while parsing singleplayer config for menu:");
					UnturnedLog.exception(e);
				}
			}
			switch (PlaySettings.singleplayerMode)
			{
			case EGameMode.EASY:
				MenuPlayConfigUI.modeConfigData = MenuPlayConfigUI.configData.Easy;
				break;
			case EGameMode.NORMAL:
				MenuPlayConfigUI.modeConfigData = MenuPlayConfigUI.configData.Normal;
				break;
			case EGameMode.HARD:
				MenuPlayConfigUI.modeConfigData = MenuPlayConfigUI.configData.Hard;
				break;
			}
			MenuPlayConfigUI.refreshConfig();
			MenuPlayConfigUI.container.AnimateIntoView();
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0014EDD4 File Offset: 0x0014CFD4
		public static void close()
		{
			if (!MenuPlayConfigUI.active)
			{
				return;
			}
			MenuPlayConfigUI.active = false;
			ReadWrite.serializeJSON<ConfigData>("/Worlds/Singleplayer_" + Characters.selected.ToString() + "/Config.json", false, MenuPlayConfigUI.configData);
			MenuPlayConfigUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0014EE2A File Offset: 0x0014D02A
		public static string sanitizeName(string fieldName)
		{
			if (MenuPlayConfigUI.localization.has(fieldName))
			{
				return MenuPlayConfigUI.localization.format(fieldName);
			}
			return fieldName.Replace('_', ' ');
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0014EE50 File Offset: 0x0014D050
		private static void refreshConfig()
		{
			MenuPlayConfigUI.configBox.RemoveAllChildren();
			MenuPlayConfigUI.configOffset = 0;
			MenuPlayConfigUI.configGroups.Clear();
			foreach (FieldInfo fieldInfo in MenuPlayConfigUI.modeConfigData.GetType().GetFields())
			{
				ISleekBox sleekBox = Glazier.Get().CreateBox();
				sleekBox.PositionOffset_Y = (float)MenuPlayConfigUI.configOffset;
				sleekBox.SizeOffset_Y = 30f;
				sleekBox.SizeScale_X = 1f;
				sleekBox.Text = MenuPlayConfigUI.sanitizeName(fieldInfo.Name);
				MenuPlayConfigUI.configBox.AddChild(sleekBox);
				int num = 40;
				MenuPlayConfigUI.configOffset += 40;
				object value = fieldInfo.GetValue(MenuPlayConfigUI.modeConfigData);
				foreach (FieldInfo fieldInfo2 in value.GetType().GetFields())
				{
					object value2 = fieldInfo2.GetValue(value);
					Type type = value2.GetType();
					if (type == typeof(uint))
					{
						ISleekUInt32Field sleekUInt32Field = Glazier.Get().CreateUInt32Field();
						sleekUInt32Field.PositionOffset_Y = (float)num;
						sleekUInt32Field.SizeOffset_X = 200f;
						sleekUInt32Field.SizeOffset_Y = 30f;
						sleekUInt32Field.Value = (uint)value2;
						sleekUInt32Field.AddLabel(MenuPlayConfigUI.sanitizeName(fieldInfo2.Name), 1);
						sleekUInt32Field.OnValueChanged += new TypedUInt32(MenuPlayConfigUI.onTypedUInt32);
						sleekBox.AddChild(sleekUInt32Field);
						num += 40;
						MenuPlayConfigUI.configOffset += 40;
					}
					else if (type == typeof(float))
					{
						ISleekFloat32Field sleekFloat32Field = Glazier.Get().CreateFloat32Field();
						sleekFloat32Field.PositionOffset_Y = (float)num;
						sleekFloat32Field.SizeOffset_X = 200f;
						sleekFloat32Field.SizeOffset_Y = 30f;
						sleekFloat32Field.Value = (float)value2;
						sleekFloat32Field.AddLabel(MenuPlayConfigUI.sanitizeName(fieldInfo2.Name), 1);
						sleekFloat32Field.OnValueChanged += new TypedSingle(MenuPlayConfigUI.onTypedSingle);
						sleekBox.AddChild(sleekFloat32Field);
						num += 40;
						MenuPlayConfigUI.configOffset += 40;
					}
					else if (type == typeof(bool))
					{
						ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
						sleekToggle.PositionOffset_Y = (float)num;
						sleekToggle.SizeOffset_X = 40f;
						sleekToggle.SizeOffset_Y = 40f;
						sleekToggle.Value = (bool)value2;
						sleekToggle.AddLabel(MenuPlayConfigUI.sanitizeName(fieldInfo2.Name), 1);
						sleekToggle.OnValueChanged += new Toggled(MenuPlayConfigUI.onToggled);
						sleekBox.AddChild(sleekToggle);
						num += 50;
						MenuPlayConfigUI.configOffset += 50;
					}
				}
				MenuPlayConfigUI.configOffset += 40;
				MenuPlayConfigUI.configGroups.Add(value);
			}
			MenuPlayConfigUI.configBox.ContentSizeOffset = new Vector2(0f, (float)(MenuPlayConfigUI.configOffset - 50));
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0014F144 File Offset: 0x0014D344
		private static void updateValue(ISleekElement sleek, object state)
		{
			int num = MenuPlayConfigUI.configBox.FindIndexOfChild(sleek.Parent);
			object obj = MenuPlayConfigUI.configGroups[num];
			FieldInfo[] fields = obj.GetType().GetFields();
			int num2 = sleek.Parent.FindIndexOfChild(sleek);
			fields[num2].SetValue(obj, state);
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0014F18F File Offset: 0x0014D38F
		private static void onTypedUInt32(ISleekUInt32Field uint32Field, uint state)
		{
			MenuPlayConfigUI.updateValue(uint32Field, state);
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0014F19D File Offset: 0x0014D39D
		private static void onTypedSingle(ISleekFloat32Field singleField, float state)
		{
			MenuPlayConfigUI.updateValue(singleField, state);
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0014F1AB File Offset: 0x0014D3AB
		private static void onToggled(ISleekToggle toggle, bool state)
		{
			MenuPlayConfigUI.updateValue(toggle, state);
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0014F1B9 File Offset: 0x0014D3B9
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuPlaySingleplayerUI.open();
			MenuPlayConfigUI.close();
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0014F1C8 File Offset: 0x0014D3C8
		private static void onClickedDefaultButton(ISleekElement button)
		{
			MenuPlayConfigUI.modeConfigData = new ModeConfigData(PlaySettings.singleplayerMode);
			MenuPlayConfigUI.modeConfigData.InitSingleplayerDefaults();
			switch (PlaySettings.singleplayerMode)
			{
			case EGameMode.EASY:
				MenuPlayConfigUI.configData.Easy = MenuPlayConfigUI.modeConfigData;
				break;
			case EGameMode.NORMAL:
				MenuPlayConfigUI.configData.Normal = MenuPlayConfigUI.modeConfigData;
				break;
			case EGameMode.HARD:
				MenuPlayConfigUI.configData.Hard = MenuPlayConfigUI.modeConfigData;
				break;
			}
			MenuPlayConfigUI.refreshConfig();
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0014F240 File Offset: 0x0014D440
		public MenuPlayConfigUI()
		{
			MenuPlayConfigUI.localization = Localization.read("/Menu/Play/MenuPlayConfig.dat");
			MenuPlayConfigUI.container = new SleekFullscreenBox();
			MenuPlayConfigUI.container.PositionOffset_X = 10f;
			MenuPlayConfigUI.container.PositionOffset_Y = 10f;
			MenuPlayConfigUI.container.PositionScale_Y = 1f;
			MenuPlayConfigUI.container.SizeOffset_X = -20f;
			MenuPlayConfigUI.container.SizeOffset_Y = -20f;
			MenuPlayConfigUI.container.SizeScale_X = 1f;
			MenuPlayConfigUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayConfigUI.container);
			MenuPlayConfigUI.active = false;
			MenuPlayConfigUI.configBox = Glazier.Get().CreateScrollView();
			MenuPlayConfigUI.configBox.PositionOffset_X = -200f;
			MenuPlayConfigUI.configBox.PositionOffset_Y = 100f;
			MenuPlayConfigUI.configBox.PositionScale_X = 0.5f;
			MenuPlayConfigUI.configBox.SizeOffset_X = 430f;
			MenuPlayConfigUI.configBox.SizeOffset_Y = -200f;
			MenuPlayConfigUI.configBox.SizeScale_Y = 1f;
			MenuPlayConfigUI.configBox.ScaleContentToWidth = true;
			MenuPlayConfigUI.container.AddChild(MenuPlayConfigUI.configBox);
			MenuPlayConfigUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuPlayConfigUI.backButton.PositionOffset_Y = -50f;
			MenuPlayConfigUI.backButton.PositionScale_Y = 1f;
			MenuPlayConfigUI.backButton.SizeOffset_X = 200f;
			MenuPlayConfigUI.backButton.SizeOffset_Y = 50f;
			MenuPlayConfigUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayConfigUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuPlayConfigUI.backButton.onClickedButton += new ClickedButton(MenuPlayConfigUI.onClickedBackButton);
			MenuPlayConfigUI.backButton.fontSize = 3;
			MenuPlayConfigUI.backButton.iconColor = 2;
			MenuPlayConfigUI.container.AddChild(MenuPlayConfigUI.backButton);
			MenuPlayConfigUI.defaultButton = Glazier.Get().CreateButton();
			MenuPlayConfigUI.defaultButton.PositionOffset_X = -200f;
			MenuPlayConfigUI.defaultButton.PositionOffset_Y = -50f;
			MenuPlayConfigUI.defaultButton.PositionScale_X = 1f;
			MenuPlayConfigUI.defaultButton.PositionScale_Y = 1f;
			MenuPlayConfigUI.defaultButton.SizeOffset_X = 200f;
			MenuPlayConfigUI.defaultButton.SizeOffset_Y = 50f;
			MenuPlayConfigUI.defaultButton.Text = MenuPlayConfigUI.localization.format("Default");
			MenuPlayConfigUI.defaultButton.TooltipText = MenuPlayConfigUI.localization.format("Default_Tooltip");
			MenuPlayConfigUI.defaultButton.OnClicked += new ClickedButton(MenuPlayConfigUI.onClickedDefaultButton);
			MenuPlayConfigUI.defaultButton.FontSize = 3;
			MenuPlayConfigUI.container.AddChild(MenuPlayConfigUI.defaultButton);
			MenuPlayConfigUI.configGroups = new List<object>();
		}

		// Token: 0x04002979 RID: 10617
		public static Local localization;

		// Token: 0x0400297A RID: 10618
		private static SleekFullscreenBox container;

		// Token: 0x0400297B RID: 10619
		public static bool active;

		// Token: 0x0400297C RID: 10620
		private static SleekButtonIcon backButton;

		// Token: 0x0400297D RID: 10621
		private static ISleekButton defaultButton;

		// Token: 0x0400297E RID: 10622
		private static ISleekScrollView configBox;

		// Token: 0x0400297F RID: 10623
		private static ConfigData configData;

		// Token: 0x04002980 RID: 10624
		private static ModeConfigData modeConfigData;

		// Token: 0x04002981 RID: 10625
		private static int configOffset;

		// Token: 0x04002982 RID: 10626
		private static List<object> configGroups;
	}
}
