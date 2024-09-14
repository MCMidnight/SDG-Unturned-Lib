using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Interactable;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200010F RID: 271
	public class DevkitSelectionManager
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x0001A3B8 File Offset: 0x000185B8
		public static void select(DevkitSelection select)
		{
			if (select == null)
			{
				return;
			}
			if (!InputEx.GetKey(KeyCode.LeftShift) && !InputEx.GetKey(KeyCode.LeftControl))
			{
				DevkitSelectionManager.clear();
				DevkitSelectionManager.add(select);
				return;
			}
			if (DevkitSelectionManager.selection.Contains(select))
			{
				DevkitSelectionManager.remove(select);
				return;
			}
			DevkitSelectionManager.add(select);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001A408 File Offset: 0x00018608
		public static void add(DevkitSelection select)
		{
			if (select == null || select.gameObject == null)
			{
				return;
			}
			if (DevkitSelectionManager.selection.Contains(select))
			{
				return;
			}
			if (DevkitSelectionManager.beginSelection(select))
			{
				DevkitSelectionManager.selection.Add(select);
				DevkitSelectionManager.mostRecentGameObject = select.gameObject;
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001A454 File Offset: 0x00018654
		public static void remove(DevkitSelection select)
		{
			if (select == null)
			{
				return;
			}
			if (DevkitSelectionManager.selection.Remove(select))
			{
				DevkitSelectionManager.endSelection(select);
				if (select.gameObject == DevkitSelectionManager.mostRecentGameObject)
				{
					DevkitSelectionManager.mostRecentGameObject = null;
				}
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001A488 File Offset: 0x00018688
		public static void clear()
		{
			foreach (DevkitSelection select in DevkitSelectionManager.selection)
			{
				DevkitSelectionManager.endSelection(select);
			}
			DevkitSelectionManager.selection.Clear();
			DevkitSelectionManager.mostRecentGameObject = null;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001A4E8 File Offset: 0x000186E8
		public static bool beginSelection(DevkitSelection select)
		{
			if (select == null || select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.beginSelectionHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableBeginSelectionHandler>(DevkitSelectionManager.beginSelectionHandlers);
			foreach (IDevkitInteractableBeginSelectionHandler devkitInteractableBeginSelectionHandler in DevkitSelectionManager.beginSelectionHandlers)
			{
				devkitInteractableBeginSelectionHandler.beginSelection(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.beginSelectionHandlers.Count > 0;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001A588 File Offset: 0x00018788
		public static bool endSelection(DevkitSelection select)
		{
			if (select == null || select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.endSelectionHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableEndSelectionHandler>(DevkitSelectionManager.endSelectionHandlers);
			foreach (IDevkitInteractableEndSelectionHandler devkitInteractableEndSelectionHandler in DevkitSelectionManager.endSelectionHandlers)
			{
				devkitInteractableEndSelectionHandler.endSelection(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.endSelectionHandlers.Count > 0;
		}

		// Token: 0x040002A1 RID: 673
		protected static List<IDevkitInteractableBeginSelectionHandler> beginSelectionHandlers = new List<IDevkitInteractableBeginSelectionHandler>();

		// Token: 0x040002A2 RID: 674
		protected static List<IDevkitInteractableEndSelectionHandler> endSelectionHandlers = new List<IDevkitInteractableEndSelectionHandler>();

		// Token: 0x040002A3 RID: 675
		public static HashSet<DevkitSelection> selection = new HashSet<DevkitSelection>();

		// Token: 0x040002A4 RID: 676
		public static InteractionData data = new InteractionData();

		// Token: 0x040002A5 RID: 677
		public static GameObject mostRecentGameObject = null;
	}
}
