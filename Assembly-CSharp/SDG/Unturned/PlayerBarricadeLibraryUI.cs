using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007BB RID: 1979
	public class PlayerBarricadeLibraryUI
	{
		// Token: 0x06004293 RID: 17043 RVA: 0x0016F100 File Offset: 0x0016D300
		public static void open(InteractableLibrary newLibrary)
		{
			if (PlayerBarricadeLibraryUI.active)
			{
				return;
			}
			PlayerBarricadeLibraryUI.active = true;
			PlayerBarricadeLibraryUI.library = newLibrary;
			if (PlayerBarricadeLibraryUI.library != null)
			{
				PlayerBarricadeLibraryUI.capacityBox.Text = PlayerBarricadeLibraryUI.localization.format("Capacity_Text", PlayerBarricadeLibraryUI.library.amount, PlayerBarricadeLibraryUI.library.capacity);
				PlayerBarricadeLibraryUI.walletBox.Text = Player.player.skills.experience.ToString();
				PlayerBarricadeLibraryUI.amountField.Value = 0U;
				PlayerBarricadeLibraryUI.updateTax();
			}
			PlayerBarricadeLibraryUI.container.AnimateIntoView();
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x0016F1A0 File Offset: 0x0016D3A0
		public static void close()
		{
			if (!PlayerBarricadeLibraryUI.active)
			{
				return;
			}
			PlayerBarricadeLibraryUI.active = false;
			PlayerBarricadeLibraryUI.library = null;
			PlayerBarricadeLibraryUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x0016F1CC File Offset: 0x0016D3CC
		private static void updateTax()
		{
			if (PlayerBarricadeLibraryUI.library != null)
			{
				if (PlayerBarricadeLibraryUI.transactionButton.state == 0)
				{
					PlayerBarricadeLibraryUI.tax = (uint)Math.Ceiling(PlayerBarricadeLibraryUI.amountField.Value * ((double)PlayerBarricadeLibraryUI.library.tax / 100.0));
					PlayerBarricadeLibraryUI.net = PlayerBarricadeLibraryUI.amountField.Value - PlayerBarricadeLibraryUI.tax;
					PlayerBarricadeLibraryUI.yesButton.IsClickable = (PlayerBarricadeLibraryUI.amountField.Value <= Player.player.skills.experience && PlayerBarricadeLibraryUI.net + PlayerBarricadeLibraryUI.library.amount <= PlayerBarricadeLibraryUI.library.capacity);
				}
				else
				{
					PlayerBarricadeLibraryUI.tax = 0U;
					PlayerBarricadeLibraryUI.net = PlayerBarricadeLibraryUI.amountField.Value - PlayerBarricadeLibraryUI.tax;
					PlayerBarricadeLibraryUI.yesButton.IsClickable = (PlayerBarricadeLibraryUI.net <= PlayerBarricadeLibraryUI.library.amount);
				}
				ESleekTint esleekTint = PlayerBarricadeLibraryUI.yesButton.IsClickable ? 3 : 6;
				PlayerBarricadeLibraryUI.amountField.TextColor = esleekTint;
				PlayerBarricadeLibraryUI.taxBox.TextColor = esleekTint;
				PlayerBarricadeLibraryUI.netBox.TextColor = esleekTint;
			}
			PlayerBarricadeLibraryUI.taxBox.Text = PlayerBarricadeLibraryUI.tax.ToString();
			PlayerBarricadeLibraryUI.netBox.Text = PlayerBarricadeLibraryUI.net.ToString();
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x0016F325 File Offset: 0x0016D525
		private static void onTypedAmountField(ISleekUInt32Field field, uint state)
		{
			PlayerBarricadeLibraryUI.updateTax();
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x0016F32C File Offset: 0x0016D52C
		private static void onSwappedTransactionState(SleekButtonState button, int index)
		{
			PlayerBarricadeLibraryUI.updateTax();
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x0016F334 File Offset: 0x0016D534
		private static void onClickedYesButton(ISleekElement button)
		{
			if (PlayerBarricadeLibraryUI.library != null)
			{
				if (PlayerBarricadeLibraryUI.transactionButton.state == 0)
				{
					if (PlayerBarricadeLibraryUI.amountField.Value > Player.player.skills.experience || PlayerBarricadeLibraryUI.net + PlayerBarricadeLibraryUI.library.amount > PlayerBarricadeLibraryUI.library.capacity)
					{
						return;
					}
				}
				else if (PlayerBarricadeLibraryUI.net > PlayerBarricadeLibraryUI.library.amount)
				{
					return;
				}
				if (PlayerBarricadeLibraryUI.net > 0U)
				{
					PlayerBarricadeLibraryUI.library.ClientTransfer((byte)PlayerBarricadeLibraryUI.transactionButton.state, PlayerBarricadeLibraryUI.amountField.Value);
				}
			}
			PlayerLifeUI.open();
			PlayerBarricadeLibraryUI.close();
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x0016F3D5 File Offset: 0x0016D5D5
		private static void onClickedNoButton(ISleekElement button)
		{
			PlayerLifeUI.open();
			PlayerBarricadeLibraryUI.close();
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0016F3E4 File Offset: 0x0016D5E4
		public PlayerBarricadeLibraryUI()
		{
			PlayerBarricadeLibraryUI.localization = Localization.read("/Player/PlayerBarricadeLibrary.dat");
			PlayerBarricadeLibraryUI.container = new SleekFullscreenBox();
			PlayerBarricadeLibraryUI.container.PositionScale_Y = 1f;
			PlayerBarricadeLibraryUI.container.PositionOffset_X = 10f;
			PlayerBarricadeLibraryUI.container.PositionOffset_Y = 10f;
			PlayerBarricadeLibraryUI.container.SizeOffset_X = -20f;
			PlayerBarricadeLibraryUI.container.SizeOffset_Y = -20f;
			PlayerBarricadeLibraryUI.container.SizeScale_X = 1f;
			PlayerBarricadeLibraryUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerBarricadeLibraryUI.container);
			PlayerBarricadeLibraryUI.active = false;
			PlayerBarricadeLibraryUI.library = null;
			PlayerBarricadeLibraryUI.capacityBox = Glazier.Get().CreateBox();
			PlayerBarricadeLibraryUI.capacityBox.PositionOffset_X = -100f;
			PlayerBarricadeLibraryUI.capacityBox.PositionOffset_Y = -135f;
			PlayerBarricadeLibraryUI.capacityBox.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.capacityBox.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.capacityBox.SizeOffset_X = 200f;
			PlayerBarricadeLibraryUI.capacityBox.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.capacityBox.AddLabel(PlayerBarricadeLibraryUI.localization.format("Capacity_Label"), 0);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.capacityBox);
			PlayerBarricadeLibraryUI.walletBox = Glazier.Get().CreateBox();
			PlayerBarricadeLibraryUI.walletBox.PositionOffset_X = -100f;
			PlayerBarricadeLibraryUI.walletBox.PositionOffset_Y = -95f;
			PlayerBarricadeLibraryUI.walletBox.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.walletBox.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.walletBox.SizeOffset_X = 200f;
			PlayerBarricadeLibraryUI.walletBox.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.walletBox.AddLabel(PlayerBarricadeLibraryUI.localization.format("Wallet_Label"), 0);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.walletBox);
			PlayerBarricadeLibraryUI.amountField = Glazier.Get().CreateUInt32Field();
			PlayerBarricadeLibraryUI.amountField.PositionOffset_X = -100f;
			PlayerBarricadeLibraryUI.amountField.PositionOffset_Y = -15f;
			PlayerBarricadeLibraryUI.amountField.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.amountField.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.amountField.SizeOffset_X = 200f;
			PlayerBarricadeLibraryUI.amountField.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.amountField.AddLabel(PlayerBarricadeLibraryUI.localization.format("Amount_Label"), 0);
			PlayerBarricadeLibraryUI.amountField.OnValueChanged += new TypedUInt32(PlayerBarricadeLibraryUI.onTypedAmountField);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.amountField);
			PlayerBarricadeLibraryUI.transactionButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(PlayerBarricadeLibraryUI.localization.format("Deposit"), PlayerBarricadeLibraryUI.localization.format("Deposit_Tooltip")),
				new GUIContent(PlayerBarricadeLibraryUI.localization.format("Withdraw"), PlayerBarricadeLibraryUI.localization.format("Withdraw_Tooltip"))
			});
			PlayerBarricadeLibraryUI.transactionButton.PositionOffset_X = -100f;
			PlayerBarricadeLibraryUI.transactionButton.PositionOffset_Y = -55f;
			PlayerBarricadeLibraryUI.transactionButton.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.transactionButton.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.transactionButton.SizeOffset_X = 200f;
			PlayerBarricadeLibraryUI.transactionButton.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.transactionButton.AddLabel(PlayerBarricadeLibraryUI.localization.format("Transaction_Label"), 0);
			PlayerBarricadeLibraryUI.transactionButton.onSwappedState = new SwappedState(PlayerBarricadeLibraryUI.onSwappedTransactionState);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.transactionButton);
			PlayerBarricadeLibraryUI.taxBox = Glazier.Get().CreateBox();
			PlayerBarricadeLibraryUI.taxBox.PositionOffset_X = -100f;
			PlayerBarricadeLibraryUI.taxBox.PositionOffset_Y = 25f;
			PlayerBarricadeLibraryUI.taxBox.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.taxBox.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.taxBox.SizeOffset_X = 200f;
			PlayerBarricadeLibraryUI.taxBox.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.taxBox.AddLabel(PlayerBarricadeLibraryUI.localization.format("Tax_Label"), 0);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.taxBox);
			PlayerBarricadeLibraryUI.netBox = Glazier.Get().CreateBox();
			PlayerBarricadeLibraryUI.netBox.PositionOffset_X = -100f;
			PlayerBarricadeLibraryUI.netBox.PositionOffset_Y = 65f;
			PlayerBarricadeLibraryUI.netBox.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.netBox.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.netBox.SizeOffset_X = 200f;
			PlayerBarricadeLibraryUI.netBox.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.netBox.AddLabel(PlayerBarricadeLibraryUI.localization.format("Net_Label"), 0);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.netBox);
			PlayerBarricadeLibraryUI.yesButton = Glazier.Get().CreateButton();
			PlayerBarricadeLibraryUI.yesButton.PositionOffset_X = -100f;
			PlayerBarricadeLibraryUI.yesButton.PositionOffset_Y = 105f;
			PlayerBarricadeLibraryUI.yesButton.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.yesButton.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.yesButton.SizeOffset_X = 95f;
			PlayerBarricadeLibraryUI.yesButton.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.yesButton.Text = PlayerBarricadeLibraryUI.localization.format("Yes_Button");
			PlayerBarricadeLibraryUI.yesButton.TooltipText = PlayerBarricadeLibraryUI.localization.format("Yes_Button_Tooltip");
			PlayerBarricadeLibraryUI.yesButton.OnClicked += new ClickedButton(PlayerBarricadeLibraryUI.onClickedYesButton);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.yesButton);
			PlayerBarricadeLibraryUI.noButton = Glazier.Get().CreateButton();
			PlayerBarricadeLibraryUI.noButton.PositionOffset_X = 5f;
			PlayerBarricadeLibraryUI.noButton.PositionOffset_Y = 105f;
			PlayerBarricadeLibraryUI.noButton.PositionScale_X = 0.5f;
			PlayerBarricadeLibraryUI.noButton.PositionScale_Y = 0.5f;
			PlayerBarricadeLibraryUI.noButton.SizeOffset_X = 95f;
			PlayerBarricadeLibraryUI.noButton.SizeOffset_Y = 30f;
			PlayerBarricadeLibraryUI.noButton.Text = PlayerBarricadeLibraryUI.localization.format("No_Button");
			PlayerBarricadeLibraryUI.noButton.TooltipText = PlayerBarricadeLibraryUI.localization.format("No_Button_Tooltip");
			PlayerBarricadeLibraryUI.noButton.OnClicked += new ClickedButton(PlayerBarricadeLibraryUI.onClickedNoButton);
			PlayerBarricadeLibraryUI.container.AddChild(PlayerBarricadeLibraryUI.noButton);
		}

		// Token: 0x04002BCE RID: 11214
		private static SleekFullscreenBox container;

		// Token: 0x04002BCF RID: 11215
		private static Local localization;

		// Token: 0x04002BD0 RID: 11216
		public static bool active;

		// Token: 0x04002BD1 RID: 11217
		private static InteractableLibrary library;

		// Token: 0x04002BD2 RID: 11218
		private static ISleekBox capacityBox;

		// Token: 0x04002BD3 RID: 11219
		private static ISleekBox walletBox;

		// Token: 0x04002BD4 RID: 11220
		private static ISleekUInt32Field amountField;

		// Token: 0x04002BD5 RID: 11221
		private static SleekButtonState transactionButton;

		// Token: 0x04002BD6 RID: 11222
		private static ISleekBox taxBox;

		// Token: 0x04002BD7 RID: 11223
		private static ISleekBox netBox;

		// Token: 0x04002BD8 RID: 11224
		private static uint tax;

		// Token: 0x04002BD9 RID: 11225
		private static uint net;

		// Token: 0x04002BDA RID: 11226
		private static ISleekButton yesButton;

		// Token: 0x04002BDB RID: 11227
		private static ISleekButton noButton;
	}
}
