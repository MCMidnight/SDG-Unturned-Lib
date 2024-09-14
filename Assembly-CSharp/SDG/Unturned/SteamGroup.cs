using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200068F RID: 1679
	public class SteamGroup
	{
		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x0600388A RID: 14474 RVA: 0x0010B674 File Offset: 0x00109874
		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x0600388B RID: 14475 RVA: 0x0010B67C File Offset: 0x0010987C
		public string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x0600388C RID: 14476 RVA: 0x0010B684 File Offset: 0x00109884
		public Texture2D icon
		{
			get
			{
				return this._icon;
			}
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0010B68C File Offset: 0x0010988C
		public SteamGroup(CSteamID newSteamID, string newName, Texture2D newIcon)
		{
			this._steamID = newSteamID;
			this._name = newName;
			this._icon = newIcon;
		}

		// Token: 0x04002182 RID: 8578
		private CSteamID _steamID;

		// Token: 0x04002183 RID: 8579
		private string _name;

		// Token: 0x04002184 RID: 8580
		private Texture2D _icon;
	}
}
