using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007CD RID: 1997
	public class PlayerNPCVendorUI
	{
		// Token: 0x060043DB RID: 17371 RVA: 0x00183C2B File Offset: 0x00181E2B
		public static void open(VendorAsset newVendor, DialogueAsset newDialogue, DialogueMessage newNextMessage, bool newHasNextDialogue)
		{
			if (PlayerNPCVendorUI.active)
			{
				return;
			}
			PlayerNPCVendorUI.active = true;
			PlayerNPCVendorUI.updateVendor(newVendor, newDialogue, newNextMessage, newHasNextDialogue);
			PlayerNPCVendorUI.container.AnimateIntoView();
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x00183C4E File Offset: 0x00181E4E
		public static void close()
		{
			if (!PlayerNPCVendorUI.active)
			{
				return;
			}
			PlayerNPCVendorUI.active = false;
			PlayerNPCVendorUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x00183C72 File Offset: 0x00181E72
		public static void closeNicely()
		{
			PlayerNPCVendorUI.close();
			PlayerNPCDialogueUI.open(PlayerNPCVendorUI.dialogue, PlayerNPCVendorUI.nextMessage, PlayerNPCVendorUI.hasNextDialogue);
		}

		/// <summary>
		/// Update currency and owned items if inventory has changed and menu is open.
		/// </summary>
		// Token: 0x060043DE RID: 17374 RVA: 0x00183C90 File Offset: 0x00181E90
		public static void MaybeRefresh()
		{
			if (!PlayerNPCVendorUI.active || !PlayerNPCVendorUI.needsRefresh || PlayerNPCVendorUI.vendor == null)
			{
				return;
			}
			Player player = Player.player;
			if (player == null || player.inventory == null)
			{
				return;
			}
			PlayerNPCVendorUI.needsRefresh = false;
			PlayerNPCVendorUI.RefreshExperienceOrCurrencyBoxAmount();
			PlayerNPCVendorUI.RefreshButtonVisibility();
			foreach (SleekVendor sleekVendor in PlayerNPCVendorUI.buyingButtons)
			{
				sleekVendor.updateAmount();
			}
			foreach (SleekVendor sleekVendor2 in PlayerNPCVendorUI.sellingButtons)
			{
				sleekVendor2.updateAmount();
			}
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x00183D64 File Offset: 0x00181F64
		private static void RefreshButtonVisibility()
		{
			Player player = Player.player;
			float num = 0f;
			for (int i = 0; i < PlayerNPCVendorUI.buying.Count; i++)
			{
				bool flag = PlayerNPCVendorUI.buying[i].areConditionsMet(player);
				PlayerNPCVendorUI.buyingButtons[i].IsVisible = flag;
				if (flag)
				{
					PlayerNPCVendorUI.buyingButtons[i].PositionOffset_Y = num;
					num += PlayerNPCVendorUI.buyingButtons[i].SizeOffset_Y;
				}
			}
			PlayerNPCVendorUI.buyingBox.IsVisible = (num > 0f);
			PlayerNPCVendorUI.buyingBox.ContentSizeOffset = new Vector2(0f, num);
			float num2 = 0f;
			for (int j = 0; j < PlayerNPCVendorUI.selling.Count; j++)
			{
				bool flag2 = PlayerNPCVendorUI.selling[j].areConditionsMet(player);
				PlayerNPCVendorUI.sellingButtons[j].IsVisible = flag2;
				if (flag2)
				{
					PlayerNPCVendorUI.sellingButtons[j].PositionOffset_Y = num2;
					num2 += PlayerNPCVendorUI.sellingButtons[j].SizeOffset_Y;
				}
			}
			PlayerNPCVendorUI.sellingBox.IsVisible = (num2 > 0f);
			PlayerNPCVendorUI.sellingBox.ContentSizeOffset = new Vector2(0f, num2);
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x00183EA0 File Offset: 0x001820A0
		private static void RefreshExperienceOrCurrencyBoxAmount()
		{
			if (PlayerNPCVendorUI.experienceBox.IsVisible)
			{
				PlayerNPCVendorUI.experienceBox.Text = PlayerNPCVendorUI.localization.format("Experience", Player.player.skills.experience.ToString());
				return;
			}
			if (PlayerNPCVendorUI.currencyBox.IsVisible)
			{
				ItemCurrencyAsset itemCurrencyAsset = PlayerNPCVendorUI.vendor.currency.Find();
				if (itemCurrencyAsset != null)
				{
					uint inventoryValue = itemCurrencyAsset.getInventoryValue(Player.player);
					if (string.IsNullOrEmpty(itemCurrencyAsset.valueFormat))
					{
						PlayerNPCVendorUI.currencyLabel.Text = inventoryValue.ToString("N");
						return;
					}
					PlayerNPCVendorUI.currencyLabel.Text = string.Format(itemCurrencyAsset.valueFormat, inventoryValue);
				}
			}
		}

		/// <summary>
		/// Update currency or experience depending what the vendor accepts.
		/// </summary>
		// Token: 0x060043E1 RID: 17377 RVA: 0x00183F58 File Offset: 0x00182158
		private static void updateCurrencyOrExperienceBox()
		{
			PlayerNPCVendorUI.currencyBox.IsVisible = PlayerNPCVendorUI.vendor.currency.isValid;
			PlayerNPCVendorUI.experienceBox.IsVisible = !PlayerNPCVendorUI.currencyBox.IsVisible;
			if (!PlayerNPCVendorUI.currencyBox.IsVisible)
			{
				return;
			}
			PlayerNPCVendorUI.currencyPanel.RemoveAllChildren();
			ItemCurrencyAsset itemCurrencyAsset = PlayerNPCVendorUI.vendor.currency.Find();
			if (itemCurrencyAsset == null)
			{
				Assets.reportError(PlayerNPCVendorUI.vendor, "unable to find currency");
				PlayerNPCVendorUI.currencyLabel.Text = "Invalid";
				return;
			}
			float num = 5f;
			foreach (ItemCurrencyAsset.Entry entry in itemCurrencyAsset.entries)
			{
				AssetReference<ItemAsset> item = entry.item;
				ItemAsset itemAsset = item.Find();
				if (itemAsset == null)
				{
					Assets.reportError(PlayerNPCVendorUI.vendor, "unable to find entry item {0}", entry.item);
				}
				else if (entry.isVisibleInVendorMenu)
				{
					SleekItemIcon sleekItemIcon = new SleekItemIcon();
					sleekItemIcon.PositionOffset_X = num;
					sleekItemIcon.PositionOffset_Y = 5f;
					float num2 = (float)itemAsset.size_x / (float)itemAsset.size_y;
					sleekItemIcon.SizeOffset_X = (float)Mathf.RoundToInt(num2 * 40f);
					sleekItemIcon.SizeOffset_Y = 40f;
					PlayerNPCVendorUI.currencyPanel.AddChild(sleekItemIcon);
					sleekItemIcon.Refresh(itemAsset.id, 100, itemAsset.getState(false), itemAsset, Mathf.RoundToInt(sleekItemIcon.SizeOffset_X), Mathf.RoundToInt(sleekItemIcon.SizeOffset_Y));
					ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
					sleekLabel.PositionOffset_X = sleekItemIcon.PositionOffset_X;
					sleekLabel.PositionOffset_Y = 0f;
					sleekLabel.SizeOffset_X = sleekItemIcon.SizeOffset_X;
					sleekLabel.SizeScale_Y = 1f;
					ISleekLabel sleekLabel2 = sleekLabel;
					uint value = entry.value;
					sleekLabel2.Text = value.ToString();
					sleekLabel.TextAlignment = 7;
					sleekLabel.TextContrastContext = 1;
					PlayerNPCVendorUI.currencyPanel.AddChild(sleekLabel);
					num += sleekItemIcon.SizeOffset_X + 2f;
				}
			}
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x00184168 File Offset: 0x00182368
		private static void updateVendor(VendorAsset newVendor, DialogueAsset newDialogue, DialogueMessage newNextMessage, bool newHasNextDialogue)
		{
			PlayerNPCVendorUI.vendor = newVendor;
			PlayerNPCVendorUI.dialogue = newDialogue;
			PlayerNPCVendorUI.nextMessage = newNextMessage;
			PlayerNPCVendorUI.hasNextDialogue = newHasNextDialogue;
			if (PlayerNPCVendorUI.vendor == null)
			{
				return;
			}
			if (PlayerLifeUI.npc != null)
			{
				PlayerLifeUI.npc.SetFaceOverride(PlayerNPCVendorUI.vendor.faceOverride);
			}
			PlayerNPCVendorUI.nameLabel.Text = PlayerNPCVendorUI.vendor.vendorName;
			PlayerNPCVendorUI.descriptionLabel.Text = PlayerNPCVendorUI.vendor.vendorDescription;
			PlayerNPCVendorUI.buyingButtons.Clear();
			PlayerNPCVendorUI.sellingButtons.Clear();
			PlayerNPCVendorUI.buying.Clear();
			PlayerNPCVendorUI.buying.AddRange(PlayerNPCVendorUI.vendor.buying);
			if (PlayerNPCVendorUI.vendor.enableSorting)
			{
				PlayerNPCVendorUI.buying.Sort(PlayerNPCVendorUI.buyingComparator);
			}
			PlayerNPCVendorUI.buyingBox.RemoveAllChildren();
			foreach (VendorBuying newElement in PlayerNPCVendorUI.buying)
			{
				SleekVendor sleekVendor = new SleekVendor(newElement);
				sleekVendor.SizeScale_X = 1f;
				sleekVendor.onClickedButton += new ClickedButton(PlayerNPCVendorUI.onClickedBuyingButton);
				PlayerNPCVendorUI.buyingBox.AddChild(sleekVendor);
				PlayerNPCVendorUI.buyingButtons.Add(sleekVendor);
			}
			PlayerNPCVendorUI.selling.Clear();
			PlayerNPCVendorUI.selling.AddRange(PlayerNPCVendorUI.vendor.selling);
			if (PlayerNPCVendorUI.vendor.enableSorting)
			{
				PlayerNPCVendorUI.selling.Sort(PlayerNPCVendorUI.sellingComparator);
			}
			PlayerNPCVendorUI.sellingBox.RemoveAllChildren();
			foreach (VendorSellingBase newElement2 in PlayerNPCVendorUI.selling)
			{
				SleekVendor sleekVendor2 = new SleekVendor(newElement2);
				sleekVendor2.SizeScale_X = 1f;
				sleekVendor2.onClickedButton += new ClickedButton(PlayerNPCVendorUI.onClickedSellingButton);
				PlayerNPCVendorUI.sellingBox.AddChild(sleekVendor2);
				PlayerNPCVendorUI.sellingButtons.Add(sleekVendor2);
			}
			PlayerNPCVendorUI.needsRefresh = false;
			PlayerNPCVendorUI.updateCurrencyOrExperienceBox();
			PlayerNPCVendorUI.RefreshExperienceOrCurrencyBoxAmount();
			PlayerNPCVendorUI.RefreshButtonVisibility();
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x00184378 File Offset: 0x00182578
		private static void onInventoryStateUpdated()
		{
			PlayerNPCVendorUI.needsRefresh = true;
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x00184380 File Offset: 0x00182580
		private static void onExperienceUpdated(uint newExperience)
		{
			PlayerNPCVendorUI.needsRefresh = true;
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x00184388 File Offset: 0x00182588
		private static void onReputationUpdated(int newReputation)
		{
			PlayerNPCVendorUI.needsRefresh = true;
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x00184390 File Offset: 0x00182590
		private static void onFlagsUpdated()
		{
			PlayerNPCVendorUI.needsRefresh = true;
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x00184398 File Offset: 0x00182598
		private static void onFlagUpdated(ushort id)
		{
			PlayerNPCVendorUI.needsRefresh = true;
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x001843A0 File Offset: 0x001825A0
		private static void onClickedBuyingButton(ISleekElement button)
		{
			byte b = (byte)PlayerNPCVendorUI.buyingBox.FindIndexOfChild(button);
			VendorBuying vendorBuying = PlayerNPCVendorUI.buying[(int)b];
			if (!vendorBuying.canSell(Player.player))
			{
				return;
			}
			Player.player.quests.sendSellToVendor(PlayerNPCVendorUI.vendor.GUID, vendorBuying.index, InputEx.GetKey(ControlsSettings.other));
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x00184400 File Offset: 0x00182600
		private static void onClickedSellingButton(ISleekElement button)
		{
			byte b = (byte)PlayerNPCVendorUI.sellingBox.FindIndexOfChild(button);
			VendorSellingBase vendorSellingBase = PlayerNPCVendorUI.selling[(int)b];
			if (!vendorSellingBase.canBuy(Player.player))
			{
				return;
			}
			Player.player.quests.sendBuyFromVendor(PlayerNPCVendorUI.vendor.GUID, vendorSellingBase.index, InputEx.GetKey(ControlsSettings.other));
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x0018445D File Offset: 0x0018265D
		private static void onClickedReturnButton(ISleekElement button)
		{
			PlayerNPCVendorUI.closeNicely();
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x00184464 File Offset: 0x00182664
		public PlayerNPCVendorUI()
		{
			PlayerNPCVendorUI.localization = Localization.read("/Player/PlayerNPCVendor.dat");
			PlayerNPCVendorUI.container = new SleekFullscreenBox();
			PlayerNPCVendorUI.container.PositionScale_Y = 1f;
			PlayerNPCVendorUI.container.PositionOffset_X = 10f;
			PlayerNPCVendorUI.container.PositionOffset_Y = 10f;
			PlayerNPCVendorUI.container.SizeOffset_X = -20f;
			PlayerNPCVendorUI.container.SizeOffset_Y = -20f;
			PlayerNPCVendorUI.container.SizeScale_X = 1f;
			PlayerNPCVendorUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerNPCVendorUI.container);
			PlayerNPCVendorUI.active = false;
			PlayerNPCVendorUI.buying = new List<VendorBuying>();
			PlayerNPCVendorUI.selling = new List<VendorSellingBase>();
			PlayerNPCVendorUI.buyingButtons = new List<SleekVendor>();
			PlayerNPCVendorUI.sellingButtons = new List<SleekVendor>();
			PlayerNPCVendorUI.vendorBox = Glazier.Get().CreateBox();
			PlayerNPCVendorUI.vendorBox.SizeOffset_Y = -60f;
			PlayerNPCVendorUI.vendorBox.SizeScale_X = 1f;
			PlayerNPCVendorUI.vendorBox.SizeScale_Y = 1f;
			PlayerNPCVendorUI.container.AddChild(PlayerNPCVendorUI.vendorBox);
			PlayerNPCVendorUI.nameLabel = Glazier.Get().CreateLabel();
			PlayerNPCVendorUI.nameLabel.PositionOffset_X = 5f;
			PlayerNPCVendorUI.nameLabel.PositionOffset_Y = 5f;
			PlayerNPCVendorUI.nameLabel.SizeOffset_X = -10f;
			PlayerNPCVendorUI.nameLabel.SizeOffset_Y = 40f;
			PlayerNPCVendorUI.nameLabel.SizeScale_X = 1f;
			PlayerNPCVendorUI.nameLabel.TextColor = 4;
			PlayerNPCVendorUI.nameLabel.TextContrastContext = 1;
			PlayerNPCVendorUI.nameLabel.AllowRichText = true;
			PlayerNPCVendorUI.nameLabel.FontSize = 4;
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.nameLabel);
			PlayerNPCVendorUI.descriptionLabel = Glazier.Get().CreateLabel();
			PlayerNPCVendorUI.descriptionLabel.PositionOffset_X = 5f;
			PlayerNPCVendorUI.descriptionLabel.PositionOffset_Y = 40f;
			PlayerNPCVendorUI.descriptionLabel.SizeOffset_X = -10f;
			PlayerNPCVendorUI.descriptionLabel.SizeOffset_Y = 40f;
			PlayerNPCVendorUI.descriptionLabel.SizeScale_X = 1f;
			PlayerNPCVendorUI.descriptionLabel.TextColor = 4;
			PlayerNPCVendorUI.descriptionLabel.TextContrastContext = 1;
			PlayerNPCVendorUI.descriptionLabel.AllowRichText = true;
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.descriptionLabel);
			PlayerNPCVendorUI.buyingLabel = Glazier.Get().CreateLabel();
			PlayerNPCVendorUI.buyingLabel.PositionOffset_X = 5f;
			PlayerNPCVendorUI.buyingLabel.PositionOffset_Y = 80f;
			PlayerNPCVendorUI.buyingLabel.SizeOffset_X = -40f;
			PlayerNPCVendorUI.buyingLabel.SizeOffset_Y = 30f;
			PlayerNPCVendorUI.buyingLabel.SizeScale_X = 0.5f;
			PlayerNPCVendorUI.buyingLabel.FontSize = 3;
			PlayerNPCVendorUI.buyingLabel.Text = PlayerNPCVendorUI.localization.format("Buying");
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.buyingLabel);
			PlayerNPCVendorUI.buyingBox = Glazier.Get().CreateScrollView();
			PlayerNPCVendorUI.buyingBox.PositionOffset_X = 5f;
			PlayerNPCVendorUI.buyingBox.PositionOffset_Y = 115f;
			PlayerNPCVendorUI.buyingBox.SizeOffset_X = -10f;
			PlayerNPCVendorUI.buyingBox.SizeOffset_Y = -120f;
			PlayerNPCVendorUI.buyingBox.SizeScale_X = 0.5f;
			PlayerNPCVendorUI.buyingBox.SizeScale_Y = 1f;
			PlayerNPCVendorUI.buyingBox.ScaleContentToWidth = true;
			PlayerNPCVendorUI.buyingBox.ContentSizeOffset = new Vector2(0f, 1024f);
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.buyingBox);
			PlayerNPCVendorUI.sellingLabel = Glazier.Get().CreateLabel();
			PlayerNPCVendorUI.sellingLabel.PositionOffset_X = 5f;
			PlayerNPCVendorUI.sellingLabel.PositionOffset_Y = 80f;
			PlayerNPCVendorUI.sellingLabel.PositionScale_X = 0.5f;
			PlayerNPCVendorUI.sellingLabel.SizeOffset_X = -40f;
			PlayerNPCVendorUI.sellingLabel.SizeOffset_Y = 30f;
			PlayerNPCVendorUI.sellingLabel.SizeScale_X = 0.5f;
			PlayerNPCVendorUI.sellingLabel.FontSize = 3;
			PlayerNPCVendorUI.sellingLabel.Text = PlayerNPCVendorUI.localization.format("Selling");
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.sellingLabel);
			PlayerNPCVendorUI.sellingBox = Glazier.Get().CreateScrollView();
			PlayerNPCVendorUI.sellingBox.PositionOffset_X = 5f;
			PlayerNPCVendorUI.sellingBox.PositionOffset_Y = 115f;
			PlayerNPCVendorUI.sellingBox.PositionScale_X = 0.5f;
			PlayerNPCVendorUI.sellingBox.SizeOffset_X = -10f;
			PlayerNPCVendorUI.sellingBox.SizeOffset_Y = -120f;
			PlayerNPCVendorUI.sellingBox.SizeScale_X = 0.5f;
			PlayerNPCVendorUI.sellingBox.SizeScale_Y = 1f;
			PlayerNPCVendorUI.sellingBox.ScaleContentToWidth = true;
			PlayerNPCVendorUI.sellingBox.ContentSizeOffset = new Vector2(0f, 1024f);
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.sellingBox);
			PlayerNPCVendorUI.experienceBox = Glazier.Get().CreateBox();
			PlayerNPCVendorUI.experienceBox.PositionOffset_Y = 10f;
			PlayerNPCVendorUI.experienceBox.PositionScale_Y = 1f;
			PlayerNPCVendorUI.experienceBox.SizeOffset_X = -5f;
			PlayerNPCVendorUI.experienceBox.SizeOffset_Y = 50f;
			PlayerNPCVendorUI.experienceBox.SizeScale_X = 0.5f;
			PlayerNPCVendorUI.experienceBox.FontSize = 3;
			PlayerNPCVendorUI.experienceBox.IsVisible = false;
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.experienceBox);
			PlayerNPCVendorUI.currencyBox = Glazier.Get().CreateBox();
			PlayerNPCVendorUI.currencyBox.PositionOffset_Y = 10f;
			PlayerNPCVendorUI.currencyBox.PositionScale_Y = 1f;
			PlayerNPCVendorUI.currencyBox.SizeOffset_X = -5f;
			PlayerNPCVendorUI.currencyBox.SizeOffset_Y = 50f;
			PlayerNPCVendorUI.currencyBox.SizeScale_X = 0.5f;
			PlayerNPCVendorUI.currencyBox.IsVisible = false;
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.currencyBox);
			PlayerNPCVendorUI.currencyPanel = Glazier.Get().CreateFrame();
			PlayerNPCVendorUI.currencyPanel.SizeScale_X = 1f;
			PlayerNPCVendorUI.currencyPanel.SizeScale_Y = 1f;
			PlayerNPCVendorUI.currencyBox.AddChild(PlayerNPCVendorUI.currencyPanel);
			PlayerNPCVendorUI.currencyLabel = Glazier.Get().CreateLabel();
			PlayerNPCVendorUI.currencyLabel.PositionOffset_X = -160f;
			PlayerNPCVendorUI.currencyLabel.PositionScale_X = 1f;
			PlayerNPCVendorUI.currencyLabel.SizeOffset_X = 150f;
			PlayerNPCVendorUI.currencyLabel.SizeScale_Y = 1f;
			PlayerNPCVendorUI.currencyLabel.TextAlignment = 5;
			PlayerNPCVendorUI.currencyLabel.FontSize = 3;
			PlayerNPCVendorUI.currencyBox.AddChild(PlayerNPCVendorUI.currencyLabel);
			PlayerNPCVendorUI.returnButton = Glazier.Get().CreateButton();
			PlayerNPCVendorUI.returnButton.PositionOffset_X = 5f;
			PlayerNPCVendorUI.returnButton.PositionOffset_Y = 10f;
			PlayerNPCVendorUI.returnButton.PositionScale_X = 0.5f;
			PlayerNPCVendorUI.returnButton.PositionScale_Y = 1f;
			PlayerNPCVendorUI.returnButton.SizeOffset_X = -5f;
			PlayerNPCVendorUI.returnButton.SizeOffset_Y = 50f;
			PlayerNPCVendorUI.returnButton.SizeScale_X = 0.5f;
			PlayerNPCVendorUI.returnButton.FontSize = 3;
			PlayerNPCVendorUI.returnButton.Text = PlayerNPCVendorUI.localization.format("Return");
			PlayerNPCVendorUI.returnButton.TooltipText = PlayerNPCVendorUI.localization.format("Return_Tooltip");
			PlayerNPCVendorUI.returnButton.OnClicked += new ClickedButton(PlayerNPCVendorUI.onClickedReturnButton);
			PlayerNPCVendorUI.vendorBox.AddChild(PlayerNPCVendorUI.returnButton);
			PlayerInventory inventory = Player.player.inventory;
			inventory.onInventoryStateUpdated = (InventoryStateUpdated)Delegate.Combine(inventory.onInventoryStateUpdated, new InventoryStateUpdated(PlayerNPCVendorUI.onInventoryStateUpdated));
			PlayerSkills skills = Player.player.skills;
			skills.onExperienceUpdated = (ExperienceUpdated)Delegate.Combine(skills.onExperienceUpdated, new ExperienceUpdated(PlayerNPCVendorUI.onExperienceUpdated));
			PlayerSkills skills2 = Player.player.skills;
			skills2.onReputationUpdated = (ReputationUpdated)Delegate.Combine(skills2.onReputationUpdated, new ReputationUpdated(PlayerNPCVendorUI.onReputationUpdated));
			PlayerQuests quests = Player.player.quests;
			quests.onFlagsUpdated = (FlagsUpdated)Delegate.Combine(quests.onFlagsUpdated, new FlagsUpdated(PlayerNPCVendorUI.onFlagsUpdated));
			PlayerQuests quests2 = Player.player.quests;
			quests2.onFlagUpdated = (FlagUpdated)Delegate.Combine(quests2.onFlagUpdated, new FlagUpdated(PlayerNPCVendorUI.onFlagUpdated));
			PlayerNPCVendorUI.needsRefresh = true;
		}

		// Token: 0x04002D46 RID: 11590
		private static SleekFullscreenBox container;

		// Token: 0x04002D47 RID: 11591
		public static Local localization;

		// Token: 0x04002D48 RID: 11592
		public static bool active;

		// Token: 0x04002D49 RID: 11593
		private static VendorAsset vendor;

		// Token: 0x04002D4A RID: 11594
		private static DialogueAsset dialogue;

		// Token: 0x04002D4B RID: 11595
		private static DialogueMessage nextMessage;

		// Token: 0x04002D4C RID: 11596
		private static bool hasNextDialogue;

		// Token: 0x04002D4D RID: 11597
		private static List<VendorBuying> buying;

		// Token: 0x04002D4E RID: 11598
		private static List<VendorSellingBase> selling;

		// Token: 0x04002D4F RID: 11599
		private static List<SleekVendor> buyingButtons;

		// Token: 0x04002D50 RID: 11600
		private static List<SleekVendor> sellingButtons;

		// Token: 0x04002D51 RID: 11601
		private static VendorBuyingNameAscendingComparator buyingComparator = new VendorBuyingNameAscendingComparator();

		// Token: 0x04002D52 RID: 11602
		private static VendorSellingNameAscendingComparator sellingComparator = new VendorSellingNameAscendingComparator();

		// Token: 0x04002D53 RID: 11603
		private static ISleekBox vendorBox;

		// Token: 0x04002D54 RID: 11604
		private static ISleekLabel nameLabel;

		// Token: 0x04002D55 RID: 11605
		private static ISleekLabel descriptionLabel;

		// Token: 0x04002D56 RID: 11606
		private static ISleekLabel sellingLabel;

		// Token: 0x04002D57 RID: 11607
		private static ISleekScrollView sellingBox;

		// Token: 0x04002D58 RID: 11608
		private static ISleekLabel buyingLabel;

		// Token: 0x04002D59 RID: 11609
		private static ISleekScrollView buyingBox;

		// Token: 0x04002D5A RID: 11610
		private static ISleekBox experienceBox;

		// Token: 0x04002D5B RID: 11611
		private static ISleekBox currencyBox;

		// Token: 0x04002D5C RID: 11612
		private static ISleekElement currencyPanel;

		// Token: 0x04002D5D RID: 11613
		private static ISleekLabel currencyLabel;

		// Token: 0x04002D5E RID: 11614
		private static ISleekButton returnButton;

		// Token: 0x04002D5F RID: 11615
		private static bool needsRefresh;
	}
}
