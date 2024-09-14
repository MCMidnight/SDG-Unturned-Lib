using System;

namespace SDG.Unturned
{
	// Token: 0x020007BD RID: 1981
	public class PlayerBarricadeSignUI
	{
		// Token: 0x060042A6 RID: 17062 RVA: 0x00170250 File Offset: 0x0016E450
		public static void open(string newText)
		{
			if (PlayerBarricadeSignUI.active)
			{
				return;
			}
			PlayerBarricadeSignUI.active = true;
			PlayerBarricadeSignUI.sign = null;
			PlayerBarricadeSignUI.yesButton.IsVisible = false;
			PlayerBarricadeSignUI.yesButton.IsClickable = true;
			PlayerBarricadeSignUI.noButton.PositionOffset_X = -200f;
			PlayerBarricadeSignUI.noButton.SizeOffset_X = 400f;
			string text = newText;
			ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref text);
			text = text.Replace("<name_char>", Player.player.channel.owner.playerID.characterName);
			PlayerBarricadeSignUI.textBox.Text = text;
			PlayerBarricadeSignUI.textField.IsVisible = false;
			PlayerBarricadeSignUI.textBox.IsVisible = true;
			PlayerBarricadeSignUI.container.AnimateIntoView();
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x00170304 File Offset: 0x0016E504
		public static void open(InteractableSign newSign)
		{
			if (PlayerBarricadeSignUI.active)
			{
				PlayerBarricadeSignUI.close();
				return;
			}
			PlayerBarricadeSignUI.active = true;
			PlayerBarricadeSignUI.sign = newSign;
			PlayerBarricadeSignUI.yesButton.IsVisible = true;
			PlayerBarricadeSignUI.yesButton.IsClickable = true;
			PlayerBarricadeSignUI.noButton.PositionOffset_X = 5f;
			PlayerBarricadeSignUI.noButton.SizeOffset_X = 195f;
			PlayerBarricadeSignUI.textField.Text = PlayerBarricadeSignUI.sign.DisplayText;
			PlayerBarricadeSignUI.textField.IsVisible = true;
			PlayerBarricadeSignUI.textBox.IsVisible = false;
			PlayerBarricadeSignUI.container.AnimateIntoView();
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x00170392 File Offset: 0x0016E592
		public static void close()
		{
			if (!PlayerBarricadeSignUI.active)
			{
				return;
			}
			PlayerBarricadeSignUI.active = false;
			PlayerBarricadeSignUI.sign = null;
			PlayerBarricadeSignUI.textField.ClearFocus();
			PlayerBarricadeSignUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x001703C8 File Offset: 0x0016E5C8
		private static void onTypedSignText(ISleekField field, string text)
		{
			if (PlayerBarricadeSignUI.sign != null)
			{
				string text2 = PlayerBarricadeSignUI.sign.trimText(text);
				PlayerBarricadeSignUI.yesButton.IsClickable = PlayerBarricadeSignUI.sign.isTextValid(text2);
				return;
			}
			PlayerBarricadeSignUI.yesButton.IsClickable = false;
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x00170410 File Offset: 0x0016E610
		private static void onClickedYesButton(ISleekElement button)
		{
			if (PlayerBarricadeSignUI.sign != null)
			{
				string newText = PlayerBarricadeSignUI.sign.trimText(PlayerBarricadeSignUI.textField.Text);
				PlayerBarricadeSignUI.sign.ClientSetText(newText);
			}
			PlayerLifeUI.open();
			PlayerBarricadeSignUI.close();
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00170454 File Offset: 0x0016E654
		private static void onClickedNoButton(ISleekElement button)
		{
			PlayerLifeUI.open();
			PlayerBarricadeSignUI.close();
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00170460 File Offset: 0x0016E660
		public PlayerBarricadeSignUI()
		{
			Local local = Localization.read("/Player/PlayerBarricadeSign.dat");
			PlayerBarricadeSignUI.container = new SleekFullscreenBox();
			PlayerBarricadeSignUI.container.PositionScale_Y = 1f;
			PlayerBarricadeSignUI.container.PositionOffset_X = 10f;
			PlayerBarricadeSignUI.container.PositionOffset_Y = 10f;
			PlayerBarricadeSignUI.container.SizeOffset_X = -20f;
			PlayerBarricadeSignUI.container.SizeOffset_Y = -20f;
			PlayerBarricadeSignUI.container.SizeScale_X = 1f;
			PlayerBarricadeSignUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerBarricadeSignUI.container);
			PlayerBarricadeSignUI.active = false;
			PlayerBarricadeSignUI.sign = null;
			PlayerBarricadeSignUI.textField = Glazier.Get().CreateStringField();
			PlayerBarricadeSignUI.textField.PositionOffset_X = -200f;
			PlayerBarricadeSignUI.textField.PositionScale_X = 0.5f;
			PlayerBarricadeSignUI.textField.PositionScale_Y = 0.1f;
			PlayerBarricadeSignUI.textField.SizeOffset_X = 400f;
			PlayerBarricadeSignUI.textField.SizeScale_Y = 0.8f;
			PlayerBarricadeSignUI.textField.MaxLength = 200;
			PlayerBarricadeSignUI.textField.IsMultiline = true;
			PlayerBarricadeSignUI.textField.OnTextChanged += new Typed(PlayerBarricadeSignUI.onTypedSignText);
			PlayerBarricadeSignUI.container.AddChild(PlayerBarricadeSignUI.textField);
			PlayerBarricadeSignUI.textBox = Glazier.Get().CreateBox();
			PlayerBarricadeSignUI.textBox.PositionOffset_X = -200f;
			PlayerBarricadeSignUI.textBox.PositionScale_X = 0.5f;
			PlayerBarricadeSignUI.textBox.PositionScale_Y = 0.1f;
			PlayerBarricadeSignUI.textBox.SizeOffset_X = 400f;
			PlayerBarricadeSignUI.textBox.SizeScale_Y = 0.8f;
			PlayerBarricadeSignUI.textBox.TextColor = 4;
			PlayerBarricadeSignUI.textBox.AllowRichText = true;
			PlayerBarricadeSignUI.container.AddChild(PlayerBarricadeSignUI.textBox);
			PlayerBarricadeSignUI.yesButton = Glazier.Get().CreateButton();
			PlayerBarricadeSignUI.yesButton.PositionOffset_X = -200f;
			PlayerBarricadeSignUI.yesButton.PositionOffset_Y = 5f;
			PlayerBarricadeSignUI.yesButton.PositionScale_X = 0.5f;
			PlayerBarricadeSignUI.yesButton.PositionScale_Y = 0.9f;
			PlayerBarricadeSignUI.yesButton.SizeOffset_X = 195f;
			PlayerBarricadeSignUI.yesButton.SizeOffset_Y = 30f;
			PlayerBarricadeSignUI.yesButton.Text = local.format("Yes_Button");
			PlayerBarricadeSignUI.yesButton.TooltipText = local.format("Yes_Button_Tooltip");
			PlayerBarricadeSignUI.yesButton.OnClicked += new ClickedButton(PlayerBarricadeSignUI.onClickedYesButton);
			PlayerBarricadeSignUI.container.AddChild(PlayerBarricadeSignUI.yesButton);
			PlayerBarricadeSignUI.noButton = Glazier.Get().CreateButton();
			PlayerBarricadeSignUI.noButton.PositionOffset_X = 5f;
			PlayerBarricadeSignUI.noButton.PositionOffset_Y = 5f;
			PlayerBarricadeSignUI.noButton.PositionScale_X = 0.5f;
			PlayerBarricadeSignUI.noButton.PositionScale_Y = 0.9f;
			PlayerBarricadeSignUI.noButton.SizeOffset_X = 195f;
			PlayerBarricadeSignUI.noButton.SizeOffset_Y = 30f;
			PlayerBarricadeSignUI.noButton.Text = local.format("No_Button");
			PlayerBarricadeSignUI.noButton.TooltipText = local.format("No_Button_Tooltip");
			PlayerBarricadeSignUI.noButton.OnClicked += new ClickedButton(PlayerBarricadeSignUI.onClickedNoButton);
			PlayerBarricadeSignUI.container.AddChild(PlayerBarricadeSignUI.noButton);
		}

		// Token: 0x04002BE6 RID: 11238
		private static SleekFullscreenBox container;

		// Token: 0x04002BE7 RID: 11239
		public static bool active;

		// Token: 0x04002BE8 RID: 11240
		private static InteractableSign sign;

		// Token: 0x04002BE9 RID: 11241
		private static ISleekField textField;

		// Token: 0x04002BEA RID: 11242
		private static ISleekBox textBox;

		// Token: 0x04002BEB RID: 11243
		private static ISleekButton yesButton;

		// Token: 0x04002BEC RID: 11244
		private static ISleekButton noButton;
	}
}
