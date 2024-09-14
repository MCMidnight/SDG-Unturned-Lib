using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200077F RID: 1919
	public class EditorSpawnsUI
	{
		// Token: 0x06003F09 RID: 16137 RVA: 0x00139316 File Offset: 0x00137516
		public static void open()
		{
			if (EditorSpawnsUI.active)
			{
				return;
			}
			EditorSpawnsUI.active = true;
			EditorSpawnsUI.container.AnimateIntoView();
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x00139330 File Offset: 0x00137530
		public static void close()
		{
			if (!EditorSpawnsUI.active)
			{
				return;
			}
			EditorSpawnsUI.active = false;
			EditorSpawnsItemsUI.close();
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x00139368 File Offset: 0x00137568
		private static void onClickedAnimalsButton(ISleekElement button)
		{
			EditorSpawnsItemsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsAnimalsUI.open();
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0013937E File Offset: 0x0013757E
		private static void onClickItemsButton(ISleekElement button)
		{
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsItemsUI.open();
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x00139394 File Offset: 0x00137594
		private static void onClickedZombiesButton(ISleekElement button)
		{
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsItemsUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsZombiesUI.open();
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x001393AA File Offset: 0x001375AA
		private static void onClickedVehiclesButton(ISleekElement button)
		{
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsItemsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.open();
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x001393C0 File Offset: 0x001375C0
		public EditorSpawnsUI()
		{
			Local local = Localization.read("/Editor/EditorSpawns.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawns/EditorSpawns.unity3d");
			EditorSpawnsUI.container = new SleekFullscreenBox();
			EditorSpawnsUI.container.PositionOffset_X = 10f;
			EditorSpawnsUI.container.PositionOffset_Y = 10f;
			EditorSpawnsUI.container.PositionScale_X = 1f;
			EditorSpawnsUI.container.SizeOffset_X = -20f;
			EditorSpawnsUI.container.SizeOffset_Y = -20f;
			EditorSpawnsUI.container.SizeScale_X = 1f;
			EditorSpawnsUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorSpawnsUI.container);
			EditorSpawnsUI.active = false;
			EditorSpawnsUI.animalsButton = new SleekButtonIcon(bundle.load<Texture2D>("Animals"));
			EditorSpawnsUI.animalsButton.PositionOffset_Y = 40f;
			EditorSpawnsUI.animalsButton.SizeOffset_X = -5f;
			EditorSpawnsUI.animalsButton.SizeOffset_Y = 30f;
			EditorSpawnsUI.animalsButton.SizeScale_X = 0.25f;
			EditorSpawnsUI.animalsButton.text = local.format("AnimalsButtonText");
			EditorSpawnsUI.animalsButton.tooltip = local.format("AnimalsButtonTooltip");
			EditorSpawnsUI.animalsButton.onClickedButton += new ClickedButton(EditorSpawnsUI.onClickedAnimalsButton);
			EditorSpawnsUI.container.AddChild(EditorSpawnsUI.animalsButton);
			EditorSpawnsUI.itemsButton = new SleekButtonIcon(bundle.load<Texture2D>("Items"));
			EditorSpawnsUI.itemsButton.PositionOffset_X = 5f;
			EditorSpawnsUI.itemsButton.PositionOffset_Y = 40f;
			EditorSpawnsUI.itemsButton.PositionScale_X = 0.25f;
			EditorSpawnsUI.itemsButton.SizeOffset_X = -10f;
			EditorSpawnsUI.itemsButton.SizeOffset_Y = 30f;
			EditorSpawnsUI.itemsButton.SizeScale_X = 0.25f;
			EditorSpawnsUI.itemsButton.text = local.format("ItemsButtonText");
			EditorSpawnsUI.itemsButton.tooltip = local.format("ItemsButtonTooltip");
			EditorSpawnsUI.itemsButton.onClickedButton += new ClickedButton(EditorSpawnsUI.onClickItemsButton);
			EditorSpawnsUI.container.AddChild(EditorSpawnsUI.itemsButton);
			EditorSpawnsUI.zombiesButton = new SleekButtonIcon(bundle.load<Texture2D>("Zombies"));
			EditorSpawnsUI.zombiesButton.PositionOffset_X = 5f;
			EditorSpawnsUI.zombiesButton.PositionOffset_Y = 40f;
			EditorSpawnsUI.zombiesButton.PositionScale_X = 0.5f;
			EditorSpawnsUI.zombiesButton.SizeOffset_X = -10f;
			EditorSpawnsUI.zombiesButton.SizeOffset_Y = 30f;
			EditorSpawnsUI.zombiesButton.SizeScale_X = 0.25f;
			EditorSpawnsUI.zombiesButton.text = local.format("ZombiesButtonText");
			EditorSpawnsUI.zombiesButton.tooltip = local.format("ZombiesButtonTooltip");
			EditorSpawnsUI.zombiesButton.onClickedButton += new ClickedButton(EditorSpawnsUI.onClickedZombiesButton);
			EditorSpawnsUI.container.AddChild(EditorSpawnsUI.zombiesButton);
			EditorSpawnsUI.vehiclesButton = new SleekButtonIcon(bundle.load<Texture2D>("Vehicles"));
			EditorSpawnsUI.vehiclesButton.PositionOffset_X = 5f;
			EditorSpawnsUI.vehiclesButton.PositionOffset_Y = 40f;
			EditorSpawnsUI.vehiclesButton.PositionScale_X = 0.75f;
			EditorSpawnsUI.vehiclesButton.SizeOffset_X = -5f;
			EditorSpawnsUI.vehiclesButton.SizeOffset_Y = 30f;
			EditorSpawnsUI.vehiclesButton.SizeScale_X = 0.25f;
			EditorSpawnsUI.vehiclesButton.text = local.format("VehiclesButtonText");
			EditorSpawnsUI.vehiclesButton.tooltip = local.format("VehiclesButtonTooltip");
			EditorSpawnsUI.vehiclesButton.onClickedButton += new ClickedButton(EditorSpawnsUI.onClickedVehiclesButton);
			EditorSpawnsUI.container.AddChild(EditorSpawnsUI.vehiclesButton);
			bundle.unload();
			new EditorSpawnsAnimalsUI();
			new EditorSpawnsItemsUI();
			new EditorSpawnsZombiesUI();
			new EditorSpawnsVehiclesUI();
		}

		// Token: 0x040027E1 RID: 10209
		private static SleekFullscreenBox container;

		// Token: 0x040027E2 RID: 10210
		public static bool active;

		// Token: 0x040027E3 RID: 10211
		private static SleekButtonIcon animalsButton;

		// Token: 0x040027E4 RID: 10212
		private static SleekButtonIcon itemsButton;

		// Token: 0x040027E5 RID: 10213
		private static SleekButtonIcon zombiesButton;

		// Token: 0x040027E6 RID: 10214
		private static SleekButtonIcon vehiclesButton;
	}
}
