using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Helper wrapping Unturned's usage of AudioListener.volume, which is the master volume level.
	/// This makes it easier to track what controls the master volume and avoid bugs.
	/// </summary>
	// Token: 0x020006F6 RID: 1782
	public static class UnturnedMasterVolume
	{
		/// <summary>
		/// Is audio muted because this is a dedicated server?
		///
		/// While dedicated server should not even be processing audio code,
		/// older versions of Unity in particular have issues with headless audio.
		/// </summary>
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x001146B6 File Offset: 0x001128B6
		// (set) Token: 0x06003B1B RID: 15131 RVA: 0x001146BD File Offset: 0x001128BD
		public static bool mutedByDedicatedServer
		{
			get
			{
				return UnturnedMasterVolume.internalMutedByDedicatedServer;
			}
			set
			{
				UnturnedMasterVolume.internalMutedByDedicatedServer = value;
				UnturnedMasterVolume.synchronizeAudioListener();
			}
		}

		/// <summary>
		/// Is audio muted because loading screen is visible?
		/// </summary>
		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06003B1C RID: 15132 RVA: 0x001146CA File Offset: 0x001128CA
		// (set) Token: 0x06003B1D RID: 15133 RVA: 0x001146D1 File Offset: 0x001128D1
		public static bool mutedByLoadingScreen
		{
			get
			{
				return UnturnedMasterVolume.internalMutedByLoadingScreen;
			}
			set
			{
				if (value != UnturnedMasterVolume.internalMutedByLoadingScreen)
				{
					UnturnedMasterVolume.internalMutedByLoadingScreen = value;
					UnturnedMasterVolume.synchronizeAudioListener();
				}
			}
		}

		/// <summary>
		/// Player's volume multiplier from the options menu.
		/// </summary>
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06003B1E RID: 15134 RVA: 0x001146E6 File Offset: 0x001128E6
		// (set) Token: 0x06003B1F RID: 15135 RVA: 0x001146ED File Offset: 0x001128ED
		public static float preferredVolume
		{
			get
			{
				return UnturnedMasterVolume.internalPreferredVolume;
			}
			set
			{
				if (UnturnedMasterVolume.internalPreferredVolume != value)
				{
					UnturnedMasterVolume.internalPreferredVolume = value;
					UnturnedMasterVolume.synchronizeAudioListener();
				}
			}
		}

		/// <summary>
		/// Player's unfocused volume multiplier from the options menu.
		/// </summary>
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x00114702 File Offset: 0x00112902
		// (set) Token: 0x06003B21 RID: 15137 RVA: 0x00114709 File Offset: 0x00112909
		public static float UnfocusedVolume
		{
			get
			{
				return UnturnedMasterVolume.internalUnfocusedVolumeMultiplier;
			}
			set
			{
				if (UnturnedMasterVolume.internalUnfocusedVolumeMultiplier != value)
				{
					UnturnedMasterVolume.internalUnfocusedVolumeMultiplier = value;
					UnturnedMasterVolume.synchronizeAudioListener();
				}
			}
		}

		/// <summary>
		/// Mute or un-mute audio depending whether camera is valid.
		/// </summary>
		// Token: 0x06003B22 RID: 15138 RVA: 0x0011471E File Offset: 0x0011291E
		private static void handleMainCameraAvailabilityChanged()
		{
			UnturnedMasterVolume.mutedByCamera = !MainCamera.isAvailable;
			UnturnedMasterVolume.synchronizeAudioListener();
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x00114732 File Offset: 0x00112932
		private static void OnApplicationFocusChanged(bool hasFocus)
		{
			UnturnedMasterVolume.synchronizeAudioListener();
		}

		/// <summary>
		/// Synchronize AudioListener.volume with Unturned's parameters.
		/// </summary>
		// Token: 0x06003B24 RID: 15140 RVA: 0x0011473C File Offset: 0x0011293C
		private static void synchronizeAudioListener()
		{
			float num;
			if (UnturnedMasterVolume.internalMutedByDedicatedServer || UnturnedMasterVolume.internalMutedByLoadingScreen || UnturnedMasterVolume.mutedByCamera)
			{
				num = 0f;
			}
			else
			{
				num = UnturnedMasterVolume.internalPreferredVolume;
				if (!Application.isFocused)
				{
					num *= UnturnedMasterVolume.internalUnfocusedVolumeMultiplier;
				}
			}
			AudioListener.volume = num;
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x00114784 File Offset: 0x00112984
		static UnturnedMasterVolume()
		{
			UnturnedMasterVolume.synchronizeAudioListener();
			MainCamera.availabilityChanged += UnturnedMasterVolume.handleMainCameraAvailabilityChanged;
			Application.focusChanged += new Action<bool>(UnturnedMasterVolume.OnApplicationFocusChanged);
		}

		// Token: 0x04002504 RID: 9476
		private static bool internalMutedByDedicatedServer = true;

		// Token: 0x04002505 RID: 9477
		private static bool internalMutedByLoadingScreen = true;

		// Token: 0x04002506 RID: 9478
		private static bool mutedByCamera = true;

		// Token: 0x04002507 RID: 9479
		private static float internalPreferredVolume = 1f;

		// Token: 0x04002508 RID: 9480
		private static float internalUnfocusedVolumeMultiplier = 0.5f;
	}
}
