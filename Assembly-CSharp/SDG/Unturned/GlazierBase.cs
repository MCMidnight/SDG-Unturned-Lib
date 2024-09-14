using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x0200015D RID: 349
	internal abstract class GlazierBase : MonoBehaviour
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0001E7F1 File Offset: 0x0001C9F1
		public bool ShouldGameProcessInput
		{
			get
			{
				return GUIUtility.hotControl == 0 && !EventSystem.current.IsPointerOverGameObject();
			}
		}

		/// <summary>
		/// Originally this was only in the uGUI implementation, but plugins can create uGUI text fields
		/// regardless of which glazier is used.
		/// </summary>
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0001E80C File Offset: 0x0001CA0C
		public virtual bool ShouldGameProcessKeyDown
		{
			get
			{
				EventSystem current = EventSystem.current;
				GameObject gameObject = (current != null) ? current.currentSelectedGameObject : null;
				if (gameObject == null)
				{
					return true;
				}
				InputField component = gameObject.GetComponent<InputField>();
				if (component != null)
				{
					return !component.isFocused;
				}
				TMP_InputField component2 = gameObject.GetComponent<TMP_InputField>();
				return !(component2 != null) || !component2.isFocused;
			}
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001E86C File Offset: 0x0001CA6C
		protected void UpdateDebugStats()
		{
			this.frames++;
			if (Time.realtimeSinceStartup - this.lastFrame > 1f)
			{
				this.fps = (int)((float)this.frames / (Time.realtimeSinceStartup - this.lastFrame));
				this.lastFrame = Time.realtimeSinceStartup;
				this.frames = 0;
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0001E8C8 File Offset: 0x0001CAC8
		protected void UpdateDebugString()
		{
			this.debugStringColor = Color.green;
			this.debugBuilder.Length = 0;
			Local localization = Provider.localization;
			if (Provider.isConnected)
			{
				if (!Provider.isServer && Time.realtimeSinceStartup - Provider.timeLastPacketWasReceivedFromServer > 3f)
				{
					this.debugStringColor = Color.red;
					int num = (int)(Time.realtimeSinceStartup - Provider.timeLastPacketWasReceivedFromServer);
					int num2 = Provider.CLIENT_TIMEOUT - num;
					this.debugBuilder.AppendFormat(localization.format("HUD_DC"), num, num2);
				}
				else
				{
					this.debugBuilder.AppendFormat(localization.format("HUD_FPS"), this.fps);
					this.debugBuilder.Append(' ');
					this.debugBuilder.AppendFormat(localization.format("HUD_Ping"), (int)(Provider.ping * 1000f));
					this.debugBuilder.Append(' ');
					this.debugBuilder.Append(Provider.APP_VERSION);
					if (Player.player != null && Player.player.look.canUseFreecam)
					{
						this.debugBuilder.Append(' ');
						this.debugBuilder.Append(Player.player.look.IsControllingFreecam ? localization.format("HUD_Freecam_Orbiting") : "F1");
						this.debugBuilder.Append(' ');
						this.debugBuilder.Append(Player.player.look.isTracking ? localization.format("HUD_Freecam_Tracking") : "F2");
						this.debugBuilder.Append(' ');
						this.debugBuilder.Append(Player.player.look.isLocking ? localization.format("HUD_Freecam_Locking") : "F3");
						this.debugBuilder.Append(' ');
						this.debugBuilder.Append(Player.player.look.isFocusing ? localization.format("HUD_Freecam_Focusing") : "F4");
						this.debugBuilder.Append(' ');
						this.debugBuilder.Append(Player.player.look.isSmoothing ? localization.format("HUD_Freecam_Smoothing") : "F5");
						this.debugBuilder.Append(' ');
						this.debugBuilder.Append(Player.player.workzone.isBuilding ? localization.format("HUD_Freecam_Building") : "F6");
						this.debugBuilder.Append(' ');
						this.debugBuilder.Append(Player.player.look.areSpecStatsVisible ? localization.format("HUD_Freecam_Spectating") : "F7");
					}
					if (Assets.isLoading)
					{
						this.debugBuilder.Append(" Assets");
					}
					if (Provider.isLoadingInventory)
					{
						this.debugBuilder.Append(" Economy");
					}
					if (Provider.isLoadingUGC)
					{
						this.debugBuilder.Append(" Workshop");
					}
					if (Level.isLoadingContent)
					{
						this.debugBuilder.Append(" Content");
					}
					if (Level.isLoadingLighting)
					{
						this.debugBuilder.Append(" Lighting");
					}
					if (Level.isLoadingVehicles)
					{
						this.debugBuilder.Append(" Vehicles");
					}
					if (Level.isLoadingBarricades)
					{
						this.debugBuilder.Append(" Barricades");
					}
					if (Level.isLoadingStructures)
					{
						this.debugBuilder.Append(" Structures");
					}
					if (Level.isLoadingArea)
					{
						this.debugBuilder.Append(" Area");
					}
					if (Player.isLoadingInventory)
					{
						this.debugBuilder.Append(" Inventory");
					}
					if (Player.isLoadingLife)
					{
						this.debugBuilder.Append(" Life");
					}
					if (Player.isLoadingClothing)
					{
						this.debugBuilder.Append(" Clothing");
					}
				}
			}
			else
			{
				this.debugBuilder.AppendFormat(localization.format("HUD_FPS"), this.fps);
			}
			if (this.shouldShowTimeOverlay)
			{
				this.debugBuilder.AppendFormat("\n{0:N3} s", Time.realtimeSinceStartupAsDouble);
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001ED0F File Offset: 0x0001CF0F
		protected virtual void OnEnable()
		{
			this.debugBuilder = new StringBuilder(512);
			this.fps = 0;
			this.frames = 0;
			this.lastFrame = Time.realtimeSinceStartup;
			this.shouldShowTimeOverlay = new CommandLineFlag(false, "-TimeOverlay");
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0001ED4B File Offset: 0x0001CF4B
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x0001ED53 File Offset: 0x0001CF53
		private protected Color debugStringColor { protected get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0001ED5C File Offset: 0x0001CF5C
		protected string debugString
		{
			get
			{
				return this.debugBuilder.ToString();
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0001ED69 File Offset: 0x0001CF69
		public static float ScrollViewSensitivityMultiplier
		{
			get
			{
				if (!GlazierBase.clScrollViewSensitivityMultiplier.hasValue)
				{
					return 1f;
				}
				return GlazierBase.clScrollViewSensitivityMultiplier.value;
			}
		}

		// Token: 0x04000358 RID: 856
		private StringBuilder debugBuilder;

		// Token: 0x04000359 RID: 857
		private int fps;

		// Token: 0x0400035A RID: 858
		private float lastFrame;

		// Token: 0x0400035B RID: 859
		private int frames;

		// Token: 0x0400035C RID: 860
		private CommandLineFlag shouldShowTimeOverlay;

		// Token: 0x0400035D RID: 861
		private static CommandLineFloat clScrollViewSensitivityMultiplier = new CommandLineFloat("-ScrollViewSensitivity");
	}
}
