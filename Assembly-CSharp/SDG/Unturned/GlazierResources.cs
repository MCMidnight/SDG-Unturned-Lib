using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200015F RID: 351
	internal static class GlazierResources
	{
		/// <summary>
		/// White 1x1 texture for solid colored images.
		/// uGUI empty image draws like this, but we need the texture for IMGUI backwards compatibility.
		/// </summary>
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0001EDC8 File Offset: 0x0001CFC8
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x0001EDCF File Offset: 0x0001CFCF
		public static StaticResourceRef<Texture2D> PixelTexture { get; private set; } = new StaticResourceRef<Texture2D>("Materials/Pixel");
	}
}
