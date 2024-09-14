using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000776 RID: 1910
	public class EditorEnvironmentUI
	{
		// Token: 0x06003E86 RID: 16006 RVA: 0x001325C1 File Offset: 0x001307C1
		public static void open()
		{
			if (EditorEnvironmentUI.active)
			{
				return;
			}
			EditorEnvironmentUI.active = true;
			EditorEnvironmentUI.container.AnimateIntoView();
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x001325DB File Offset: 0x001307DB
		public static void close()
		{
			if (!EditorEnvironmentUI.active)
			{
				return;
			}
			EditorEnvironmentUI.active = false;
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentUI.nodesUI.Close();
			EditorEnvironmentUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x00132618 File Offset: 0x00130818
		private static void onClickedLightingButton(ISleekElement button)
		{
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentUI.nodesUI.Close();
			EditorEnvironmentLightingUI.open();
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00132633 File Offset: 0x00130833
		private static void onClickedRoadsButton(ISleekElement button)
		{
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentUI.nodesUI.Close();
			EditorEnvironmentRoadsUI.open();
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0013264E File Offset: 0x0013084E
		private static void onClickedNavigationButton(ISleekElement button)
		{
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentUI.nodesUI.Close();
			EditorEnvironmentNavigationUI.open();
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x00132669 File Offset: 0x00130869
		private static void onClickedNodesButton(ISleekElement button)
		{
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentUI.nodesUI.Open();
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x00132684 File Offset: 0x00130884
		public void OnDestroy()
		{
			EditorEnvironmentUI.nodesUI.OnDestroy();
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x00132690 File Offset: 0x00130890
		public EditorEnvironmentUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironment.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironment/EditorEnvironment.unity3d");
			EditorEnvironmentUI.container = new SleekFullscreenBox();
			EditorEnvironmentUI.container.PositionOffset_X = 10f;
			EditorEnvironmentUI.container.PositionOffset_Y = 10f;
			EditorEnvironmentUI.container.PositionScale_X = 1f;
			EditorEnvironmentUI.container.SizeOffset_X = -20f;
			EditorEnvironmentUI.container.SizeOffset_Y = -20f;
			EditorEnvironmentUI.container.SizeScale_X = 1f;
			EditorEnvironmentUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorEnvironmentUI.container);
			EditorEnvironmentUI.active = false;
			EditorEnvironmentUI.lightingButton = new SleekButtonIcon(bundle.load<Texture2D>("Lighting"));
			EditorEnvironmentUI.lightingButton.PositionOffset_Y = 40f;
			EditorEnvironmentUI.lightingButton.SizeOffset_X = -5f;
			EditorEnvironmentUI.lightingButton.SizeOffset_Y = 30f;
			EditorEnvironmentUI.lightingButton.SizeScale_X = 0.25f;
			EditorEnvironmentUI.lightingButton.text = local.format("LightingButtonText");
			EditorEnvironmentUI.lightingButton.tooltip = local.format("LightingButtonTooltip");
			EditorEnvironmentUI.lightingButton.onClickedButton += new ClickedButton(EditorEnvironmentUI.onClickedLightingButton);
			EditorEnvironmentUI.container.AddChild(EditorEnvironmentUI.lightingButton);
			EditorEnvironmentUI.roadsButton = new SleekButtonIcon(bundle.load<Texture2D>("Roads"));
			EditorEnvironmentUI.roadsButton.PositionOffset_X = 5f;
			EditorEnvironmentUI.roadsButton.PositionOffset_Y = 40f;
			EditorEnvironmentUI.roadsButton.PositionScale_X = 0.25f;
			EditorEnvironmentUI.roadsButton.SizeOffset_X = -10f;
			EditorEnvironmentUI.roadsButton.SizeOffset_Y = 30f;
			EditorEnvironmentUI.roadsButton.SizeScale_X = 0.25f;
			EditorEnvironmentUI.roadsButton.text = local.format("RoadsButtonText");
			EditorEnvironmentUI.roadsButton.tooltip = local.format("RoadsButtonTooltip");
			EditorEnvironmentUI.roadsButton.onClickedButton += new ClickedButton(EditorEnvironmentUI.onClickedRoadsButton);
			EditorEnvironmentUI.container.AddChild(EditorEnvironmentUI.roadsButton);
			EditorEnvironmentUI.navigationButton = new SleekButtonIcon(bundle.load<Texture2D>("Navigation"));
			EditorEnvironmentUI.navigationButton.PositionOffset_X = 5f;
			EditorEnvironmentUI.navigationButton.PositionOffset_Y = 40f;
			EditorEnvironmentUI.navigationButton.PositionScale_X = 0.5f;
			EditorEnvironmentUI.navigationButton.SizeOffset_X = -10f;
			EditorEnvironmentUI.navigationButton.SizeOffset_Y = 30f;
			EditorEnvironmentUI.navigationButton.SizeScale_X = 0.25f;
			EditorEnvironmentUI.navigationButton.text = local.format("NavigationButtonText");
			EditorEnvironmentUI.navigationButton.tooltip = local.format("NavigationButtonTooltip");
			EditorEnvironmentUI.navigationButton.onClickedButton += new ClickedButton(EditorEnvironmentUI.onClickedNavigationButton);
			EditorEnvironmentUI.container.AddChild(EditorEnvironmentUI.navigationButton);
			EditorEnvironmentUI.nodesButton = new SleekButtonIcon(bundle.load<Texture2D>("Nodes"));
			EditorEnvironmentUI.nodesButton.PositionOffset_X = 5f;
			EditorEnvironmentUI.nodesButton.PositionOffset_Y = 40f;
			EditorEnvironmentUI.nodesButton.PositionScale_X = 0.75f;
			EditorEnvironmentUI.nodesButton.SizeOffset_X = -5f;
			EditorEnvironmentUI.nodesButton.SizeOffset_Y = 30f;
			EditorEnvironmentUI.nodesButton.SizeScale_X = 0.25f;
			EditorEnvironmentUI.nodesButton.text = local.format("NodesButtonText");
			EditorEnvironmentUI.nodesButton.tooltip = local.format("NodesButtonTooltip");
			EditorEnvironmentUI.nodesButton.onClickedButton += new ClickedButton(EditorEnvironmentUI.onClickedNodesButton);
			EditorEnvironmentUI.container.AddChild(EditorEnvironmentUI.nodesButton);
			bundle.unload();
			new EditorEnvironmentLightingUI();
			new EditorEnvironmentRoadsUI();
			new EditorEnvironmentNavigationUI();
			EditorEnvironmentUI.nodesUI = new EditorEnvironmentNodesUI();
			EditorEnvironmentUI.nodesUI.PositionOffset_X = 10f;
			EditorEnvironmentUI.nodesUI.PositionOffset_Y = 90f;
			EditorEnvironmentUI.nodesUI.PositionScale_X = 1f;
			EditorEnvironmentUI.nodesUI.SizeOffset_X = -20f;
			EditorEnvironmentUI.nodesUI.SizeOffset_Y = -100f;
			EditorEnvironmentUI.nodesUI.SizeScale_X = 1f;
			EditorEnvironmentUI.nodesUI.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorEnvironmentUI.nodesUI);
		}

		// Token: 0x0400275F RID: 10079
		private static SleekFullscreenBox container;

		// Token: 0x04002760 RID: 10080
		public static bool active;

		// Token: 0x04002761 RID: 10081
		private static SleekButtonIcon lightingButton;

		// Token: 0x04002762 RID: 10082
		private static SleekButtonIcon roadsButton;

		// Token: 0x04002763 RID: 10083
		private static SleekButtonIcon navigationButton;

		// Token: 0x04002764 RID: 10084
		private static SleekButtonIcon nodesButton;

		// Token: 0x04002765 RID: 10085
		private static EditorEnvironmentNodesUI nodesUI;
	}
}
