using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Ideally component Awake/Start order should not matter, but Unturned's menu is a mess.
	/// For most players the default order was fine, but it seems it was not deterministic so it would break for some players.
	/// </summary>
	// Token: 0x020005BC RID: 1468
	internal class MenuStartup : MonoBehaviour
	{
		// Token: 0x06002FAE RID: 12206 RVA: 0x000D2715 File Offset: 0x000D0915
		private void Start()
		{
			this.charactersComponent.customStart();
			this.uiComponent.customStart();
		}

		// Token: 0x040019B4 RID: 6580
		[SerializeField]
		public Characters charactersComponent;

		// Token: 0x040019B5 RID: 6581
		[SerializeField]
		public MenuUI uiComponent;
	}
}
