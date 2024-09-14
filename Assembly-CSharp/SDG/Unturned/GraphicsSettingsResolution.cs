using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006D2 RID: 1746
	public class GraphicsSettingsResolution
	{
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003AAD RID: 15021 RVA: 0x00111FBC File Offset: 0x001101BC
		// (set) Token: 0x06003AAE RID: 15022 RVA: 0x00111FC4 File Offset: 0x001101C4
		public int Width { get; set; }

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003AAF RID: 15023 RVA: 0x00111FCD File Offset: 0x001101CD
		// (set) Token: 0x06003AB0 RID: 15024 RVA: 0x00111FD5 File Offset: 0x001101D5
		public int Height { get; set; }

		// Token: 0x06003AB1 RID: 15025 RVA: 0x00111FDE File Offset: 0x001101DE
		public GraphicsSettingsResolution(Resolution resolution)
		{
			this.Width = resolution.width;
			this.Height = resolution.height;
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x00112000 File Offset: 0x00110200
		public GraphicsSettingsResolution()
		{
			this.Width = 0;
			this.Height = 0;
		}
	}
}
