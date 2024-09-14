using System;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Interactable;

namespace SDG.Unturned
{
	// Token: 0x0200015B RID: 347
	public class TempNodeBase : DevkitHierarchyWorldItem, IDevkitInteractableBeginSelectionHandler, IDevkitInteractableEndSelectionHandler
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x0001E7B4 File Offset: 0x0001C9B4
		public void beginSelection(InteractionData data)
		{
			this.isSelected = true;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001E7BD File Offset: 0x0001C9BD
		public void endSelection(InteractionData data)
		{
			this.isSelected = false;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001E7C6 File Offset: 0x0001C9C6
		internal virtual ISleekElement CreateMenu()
		{
			return null;
		}

		// Token: 0x04000356 RID: 854
		public bool isSelected;
	}
}
