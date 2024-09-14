using System;
using SDG.Provider;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007AC RID: 1964
	public class MenuSurvivorsClothingInspectUI
	{
		// Token: 0x060041DA RID: 16858 RVA: 0x00163DAC File Offset: 0x00161FAC
		public static void open()
		{
			if (MenuSurvivorsClothingInspectUI.active)
			{
				return;
			}
			MenuSurvivorsClothingInspectUI.active = true;
			MenuSurvivorsClothingInspectUI.camera.gameObject.SetActive(true);
			MenuSurvivorsClothingInspectUI.look._yaw = 0f;
			MenuSurvivorsClothingInspectUI.look.yaw = 0f;
			MenuSurvivorsClothingInspectUI.slider.Value = 0f;
			MenuSurvivorsClothingInspectUI.container.AnimateIntoView();
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x00163E0E File Offset: 0x0016200E
		public static void close()
		{
			if (!MenuSurvivorsClothingInspectUI.active)
			{
				return;
			}
			MenuSurvivorsClothingInspectUI.active = false;
			MenuSurvivorsClothingInspectUI.camera.gameObject.SetActive(false);
			MenuSurvivorsClothingInspectUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x00163E42 File Offset: 0x00162042
		private static bool getInspectedItemStatTrackerValue(out EStatTrackerType type, out int kills)
		{
			return Provider.provider.economyService.getInventoryStatTrackerValue(MenuSurvivorsClothingInspectUI.instance, out type, out kills);
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x00163E5C File Offset: 0x0016205C
		public static void viewItem(int newItem, ulong newInstance)
		{
			MenuSurvivorsClothingInspectUI.item = newItem;
			MenuSurvivorsClothingInspectUI.instance = newInstance;
			if (MenuSurvivorsClothingInspectUI.model != null)
			{
				Object.Destroy(MenuSurvivorsClothingInspectUI.model.gameObject);
			}
			Guid guid;
			Guid guid2;
			Provider.provider.economyService.getInventoryTargetID(MenuSurvivorsClothingInspectUI.item, out guid, out guid2);
			ushort inventorySkinID = Provider.provider.economyService.getInventorySkinID(MenuSurvivorsClothingInspectUI.item);
			ushort num = Provider.provider.economyService.getInventoryMythicID(MenuSurvivorsClothingInspectUI.item);
			if (num == 0)
			{
				num = Provider.provider.economyService.getInventoryParticleEffect(MenuSurvivorsClothingInspectUI.instance);
			}
			ItemAsset itemAsset = Assets.find<ItemAsset>(guid);
			VehicleAsset vehicleAsset = VehicleTool.FindVehicleByGuidAndHandleRedirects(guid2);
			if (itemAsset == null && vehicleAsset == null)
			{
				return;
			}
			if (inventorySkinID != 0)
			{
				SkinAsset skinAsset = Assets.find(EAssetType.SKIN, inventorySkinID) as SkinAsset;
				if (vehicleAsset != null)
				{
					MenuSurvivorsClothingInspectUI.model = VehicleTool.getVehicle(vehicleAsset.id, skinAsset.id, num, vehicleAsset, skinAsset);
				}
				else
				{
					MenuSurvivorsClothingInspectUI.model = ItemTool.getItem(itemAsset.id, inventorySkinID, 100, itemAsset.getState(), false, itemAsset, skinAsset, new GetStatTrackerValueHandler(MenuSurvivorsClothingInspectUI.getInspectedItemStatTrackerValue));
					if (num != 0)
					{
						ItemTool.ApplyMythicalEffect(MenuSurvivorsClothingInspectUI.model, num, EEffectType.THIRD);
					}
				}
			}
			else
			{
				MenuSurvivorsClothingInspectUI.model = ItemTool.getItem(itemAsset.id, 0, 100, itemAsset.getState(), false, itemAsset, new GetStatTrackerValueHandler(MenuSurvivorsClothingInspectUI.getInspectedItemStatTrackerValue));
				if (num != 0)
				{
					ItemTool.ApplyMythicalEffect(MenuSurvivorsClothingInspectUI.model, num, EEffectType.HOOK);
				}
			}
			MenuSurvivorsClothingInspectUI.model.parent = MenuSurvivorsClothingInspectUI.inspect;
			MenuSurvivorsClothingInspectUI.model.localPosition = Vector3.zero;
			if (vehicleAsset != null)
			{
				MenuSurvivorsClothingInspectUI.model.localRotation = Quaternion.identity;
			}
			else if (itemAsset != null && itemAsset.type == EItemType.MELEE)
			{
				MenuSurvivorsClothingInspectUI.model.localRotation = Quaternion.Euler(0f, -90f, 90f);
			}
			else
			{
				MenuSurvivorsClothingInspectUI.model.localRotation = Quaternion.Euler(-90f, 0f, 0f);
			}
			MenuSurvivorsClothingInspectUI.look.target = MenuSurvivorsClothingInspectUI.model.gameObject;
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x00164044 File Offset: 0x00162244
		private static void onDraggedSlider(ISleekSlider slider, float state)
		{
			MenuSurvivorsClothingInspectUI.look.yaw = state * 360f;
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x00164057 File Offset: 0x00162257
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsClothingItemUI.open();
			MenuSurvivorsClothingInspectUI.close();
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x00164064 File Offset: 0x00162264
		public MenuSurvivorsClothingInspectUI()
		{
			MenuSurvivorsClothingInspectUI.container = new SleekFullscreenBox();
			MenuSurvivorsClothingInspectUI.container.PositionOffset_X = 10f;
			MenuSurvivorsClothingInspectUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsClothingInspectUI.container.PositionScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.container.SizeOffset_X = -20f;
			MenuSurvivorsClothingInspectUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsClothingInspectUI.container.SizeScale_X = 1f;
			MenuSurvivorsClothingInspectUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsClothingInspectUI.container);
			MenuSurvivorsClothingInspectUI.active = false;
			MenuSurvivorsClothingInspectUI.inventory = Glazier.Get().CreateConstraintFrame();
			MenuSurvivorsClothingInspectUI.inventory.PositionScale_X = 0.5f;
			MenuSurvivorsClothingInspectUI.inventory.PositionOffset_Y = 10f;
			MenuSurvivorsClothingInspectUI.inventory.PositionScale_Y = 0.125f;
			MenuSurvivorsClothingInspectUI.inventory.SizeScale_X = 0.5f;
			MenuSurvivorsClothingInspectUI.inventory.SizeScale_Y = 0.75f;
			MenuSurvivorsClothingInspectUI.inventory.SizeOffset_Y = -20f;
			MenuSurvivorsClothingInspectUI.inventory.Constraint = 1;
			MenuSurvivorsClothingInspectUI.container.AddChild(MenuSurvivorsClothingInspectUI.inventory);
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.SizeScale_X = 1f;
			sleekBox.SizeScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.inventory.AddChild(sleekBox);
			MenuSurvivorsClothingInspectUI.image = new SleekCameraImage();
			MenuSurvivorsClothingInspectUI.image.SizeScale_X = 1f;
			MenuSurvivorsClothingInspectUI.image.SizeScale_Y = 1f;
			sleekBox.AddChild(MenuSurvivorsClothingInspectUI.image);
			MenuSurvivorsClothingInspectUI.slider = Glazier.Get().CreateSlider();
			MenuSurvivorsClothingInspectUI.slider.PositionOffset_Y = 10f;
			MenuSurvivorsClothingInspectUI.slider.PositionScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.slider.SizeOffset_Y = 20f;
			MenuSurvivorsClothingInspectUI.slider.SizeScale_X = 1f;
			MenuSurvivorsClothingInspectUI.slider.Orientation = 0;
			MenuSurvivorsClothingInspectUI.slider.OnValueChanged += new Dragged(MenuSurvivorsClothingInspectUI.onDraggedSlider);
			sleekBox.AddChild(MenuSurvivorsClothingInspectUI.slider);
			MenuSurvivorsClothingInspectUI.inspect = GameObject.Find("Inspect").transform;
			MenuSurvivorsClothingInspectUI.look = MenuSurvivorsClothingInspectUI.inspect.GetComponent<ItemLook>();
			MenuSurvivorsClothingInspectUI.camera = MenuSurvivorsClothingInspectUI.look.inspectCamera;
			MenuSurvivorsClothingInspectUI.image.SetCamera(MenuSurvivorsClothingInspectUI.camera);
			MenuSurvivorsClothingInspectUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsClothingInspectUI.backButton.PositionOffset_Y = -50f;
			MenuSurvivorsClothingInspectUI.backButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsClothingInspectUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingInspectUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingInspectUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsClothingInspectUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingInspectUI.onClickedBackButton);
			MenuSurvivorsClothingInspectUI.backButton.fontSize = 3;
			MenuSurvivorsClothingInspectUI.backButton.iconColor = 2;
			MenuSurvivorsClothingInspectUI.container.AddChild(MenuSurvivorsClothingInspectUI.backButton);
		}

		// Token: 0x04002AEE RID: 10990
		private static SleekFullscreenBox container;

		// Token: 0x04002AEF RID: 10991
		public static bool active;

		// Token: 0x04002AF0 RID: 10992
		private static SleekButtonIcon backButton;

		// Token: 0x04002AF1 RID: 10993
		private static ISleekConstraintFrame inventory;

		// Token: 0x04002AF2 RID: 10994
		private static SleekCameraImage image;

		// Token: 0x04002AF3 RID: 10995
		private static ISleekSlider slider;

		// Token: 0x04002AF4 RID: 10996
		private static int item;

		// Token: 0x04002AF5 RID: 10997
		private static ulong instance;

		// Token: 0x04002AF6 RID: 10998
		private static Transform inspect;

		// Token: 0x04002AF7 RID: 10999
		private static Transform model;

		// Token: 0x04002AF8 RID: 11000
		private static ItemLook look;

		// Token: 0x04002AF9 RID: 11001
		private static Camera camera;
	}
}
