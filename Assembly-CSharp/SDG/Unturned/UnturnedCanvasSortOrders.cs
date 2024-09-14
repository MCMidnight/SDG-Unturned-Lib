using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Keep all the uGUI Canvas sort orders in the same place.
	/// </summary>
	// Token: 0x020006F9 RID: 1785
	public static class UnturnedCanvasSortOrders
	{
		/// <summary>
		/// Manually created canvas in the Menu scene.
		/// </summary>
		// Token: 0x0400250C RID: 9484
		public const int MenuNewsFeed = 0;

		/// <summary>
		/// Devkit canvas in the Setup scene.
		/// </summary>
		// Token: 0x0400250D RID: 9485
		public const int Devkit = 1;

		/// <summary>
		/// Dropdowns, drag-drop content, tab destinations, etc.
		/// </summary>
		// Token: 0x0400250E RID: 9486
		public const int DevkitOverlay = 5;

		/// <summary>
		/// Devkit tooltips should be visible over all other devkit content.
		/// </summary>
		// Token: 0x0400250F RID: 9487
		public const int DevkitTooltip = 10;

		/// <summary>
		/// uGUI glazier contains tooltips and cursor regardless of mode (e.g. devkit), so takes absolute priority.
		/// </summary>
		// Token: 0x04002510 RID: 9488
		public const int Glazier = 15;

		/// <summary>
		/// Plugins were spawning canvases with high sort orders that showed over the loading screen, so as a hacky
		/// workaround we put the uGUI loading screen on a higher sort order than normal glazier.
		/// </summary>
		// Token: 0x04002511 RID: 9489
		public const int LoadingScreen = 29000;

		/// <summary>
		/// uGUI cursor needs to show above plugin canvas.
		/// Unity exposes sort order as an int32, but it is actually an int16, so this value is slightly below the 32767 max.
		/// </summary>
		// Token: 0x04002512 RID: 9490
		public const int Cursor = 30000;
	}
}
