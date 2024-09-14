using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200040A RID: 1034
	public class EditorUI : MonoBehaviour
	{
		// Token: 0x06001E9B RID: 7835 RVA: 0x00070E9D File Offset: 0x0006F09D
		public static void hint(EEditorMessage message, string text)
		{
			if (!EditorUI.isMessaged)
			{
				EditorUI.messageBox.IsVisible = true;
				EditorUI.lastHinted = true;
				EditorUI.isHinted = true;
				if (message == EEditorMessage.FOCUS)
				{
					EditorUI.messageBox.Text = text;
				}
			}
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x00070ECC File Offset: 0x0006F0CC
		public static void message(EEditorMessage message)
		{
			if (!OptionsSettings.hints)
			{
				return;
			}
			EditorUI.messageBox.IsVisible = true;
			EditorUI.lastMessage = Time.realtimeSinceStartup;
			EditorUI.isMessaged = true;
			if (message == EEditorMessage.HEIGHTS)
			{
				EditorUI.messageBox.Text = EditorDashboardUI.localization.format("Heights", ControlsSettings.tool_2);
				return;
			}
			if (message == EEditorMessage.ROADS)
			{
				EditorUI.messageBox.Text = EditorDashboardUI.localization.format("Roads", ControlsSettings.tool_2);
				return;
			}
			if (message == EEditorMessage.NAVIGATION)
			{
				EditorUI.messageBox.Text = EditorDashboardUI.localization.format("Navigation", ControlsSettings.tool_2);
				return;
			}
			if (message == EEditorMessage.OBJECTS)
			{
				EditorUI.messageBox.Text = EditorDashboardUI.localization.format("Objects", ControlsSettings.other, ControlsSettings.tool_2, ControlsSettings.tool_2);
				return;
			}
			if (message == EEditorMessage.NODES)
			{
				EditorUI.messageBox.Text = EditorDashboardUI.localization.format("Nodes", ControlsSettings.tool_2);
				return;
			}
			if (message == EEditorMessage.VISIBILITY)
			{
				EditorUI.messageBox.Text = EditorDashboardUI.localization.format("Visibility");
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x00070FF4 File Offset: 0x0006F1F4
		private void OnEnable()
		{
			EditorUI.instance = this;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x00070FFC File Offset: 0x0006F1FC
		internal void Editor_OnGUI()
		{
			if (EditorUI.window != null)
			{
				Glazier.Get().Root = EditorUI.window;
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x00071014 File Offset: 0x0006F214
		private void Update()
		{
			if (EditorUI.window == null)
			{
				return;
			}
			if (EditorLevelVisibilityUI.active)
			{
				EditorLevelVisibilityUI.update();
			}
			if (InputEx.ConsumeKeyDown(KeyCode.Escape))
			{
				if (MenuConfigurationOptionsUI.active)
				{
					MenuConfigurationOptionsUI.close();
					EditorPauseUI.open();
				}
				else if (MenuConfigurationDisplayUI.active)
				{
					MenuConfigurationDisplayUI.close();
					EditorPauseUI.open();
				}
				else if (MenuConfigurationGraphicsUI.active)
				{
					MenuConfigurationGraphicsUI.close();
					EditorPauseUI.open();
				}
				else if (EditorPauseUI.audioMenu.active)
				{
					EditorPauseUI.audioMenu.close();
					EditorPauseUI.open();
				}
				else if (MenuConfigurationControlsUI.active)
				{
					MenuConfigurationControlsUI.close();
					EditorPauseUI.open();
				}
				else if (EditorPauseUI.active)
				{
					EditorPauseUI.close();
				}
				else
				{
					EditorPauseUI.open();
				}
			}
			if (EditorUI.window != null)
			{
				if (InputEx.GetKeyDown(ControlsSettings.screenshot))
				{
					Provider.RequestScreenshot();
				}
				if (InputEx.GetKeyDown(ControlsSettings.hud))
				{
					EditorUI.window.isEnabled = !EditorUI.window.isEnabled;
					EditorUI.window.drawCursorWhileDisabled = false;
				}
				InputEx.GetKeyDown(ControlsSettings.terminal);
			}
			if (InputEx.GetKeyDown(ControlsSettings.refreshAssets))
			{
				Assets.RequestReloadAllAssets();
			}
			if (EditorTerrainUI.active)
			{
				if (InputEx.ConsumeKeyDown(KeyCode.Alpha1))
				{
					this.dashboardUI.terrainMenu.GoToHeightsTab();
				}
				else if (InputEx.ConsumeKeyDown(KeyCode.Alpha2))
				{
					this.dashboardUI.terrainMenu.GoToMaterialsTab();
				}
				else if (InputEx.ConsumeKeyDown(KeyCode.Alpha3))
				{
					this.dashboardUI.terrainMenu.GoToFoliageTab();
				}
				else if (InputEx.ConsumeKeyDown(KeyCode.Alpha4))
				{
					this.dashboardUI.terrainMenu.GoToTilesTab();
				}
			}
			EditorUI.window.showCursor = !EditorInteract.isFlying;
			if (EditorUI.isMessaged)
			{
				if (Time.realtimeSinceStartup - EditorUI.lastMessage > EditorUI.MESSAGE_TIME)
				{
					EditorUI.isMessaged = false;
					if (!EditorUI.isHinted)
					{
						EditorUI.messageBox.IsVisible = false;
						return;
					}
				}
			}
			else if (EditorUI.isHinted)
			{
				if (!EditorUI.lastHinted)
				{
					EditorUI.isHinted = false;
					EditorUI.messageBox.IsVisible = false;
				}
				EditorUI.lastHinted = false;
			}
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000711F8 File Offset: 0x0006F3F8
		private void Start()
		{
			EditorUI.window = new SleekWindow();
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			OptionsSettings.apply();
			GraphicsSettings.apply("editor loaded");
			this.dashboardUI = new EditorDashboardUI();
			EditorUI.messageBox = Glazier.Get().CreateBox();
			EditorUI.messageBox.PositionOffset_X = -150f;
			EditorUI.messageBox.PositionOffset_Y = -60f;
			EditorUI.messageBox.PositionScale_X = 0.5f;
			EditorUI.messageBox.PositionScale_Y = 1f;
			EditorUI.messageBox.SizeOffset_X = 300f;
			EditorUI.messageBox.SizeOffset_Y = 50f;
			EditorUI.messageBox.FontSize = 3;
			EditorUI.window.AddChild(EditorUI.messageBox);
			EditorUI.messageBox.IsVisible = false;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000712CA File Offset: 0x0006F4CA
		private void OnDestroy()
		{
			if (EditorUI.window == null)
			{
				return;
			}
			this.dashboardUI.OnDestroy();
			if (!Provider.isApplicationQuitting)
			{
				EditorUI.window.InternalDestroy();
			}
			EditorUI.window = null;
		}

		// Token: 0x04000ECC RID: 3788
		public static readonly float MESSAGE_TIME = 2f;

		// Token: 0x04000ECD RID: 3789
		public static readonly float HINT_TIME = 0.15f;

		// Token: 0x04000ECE RID: 3790
		public static SleekWindow window;

		// Token: 0x04000ECF RID: 3791
		private static ISleekBox messageBox;

		// Token: 0x04000ED0 RID: 3792
		private static float lastMessage;

		// Token: 0x04000ED1 RID: 3793
		private static bool isMessaged;

		// Token: 0x04000ED2 RID: 3794
		private static bool lastHinted;

		// Token: 0x04000ED3 RID: 3795
		private static bool isHinted;

		// Token: 0x04000ED4 RID: 3796
		internal static EditorUI instance;

		// Token: 0x04000ED5 RID: 3797
		private EditorDashboardUI dashboardUI;
	}
}
