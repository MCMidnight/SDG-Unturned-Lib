using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000403 RID: 1027
	public class EditorNavigation : MonoBehaviour
	{
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x0006E688 File Offset: 0x0006C888
		// (set) Token: 0x06001E50 RID: 7760 RVA: 0x0006E68F File Offset: 0x0006C88F
		public static bool isPathfinding
		{
			get
			{
				return EditorNavigation._isPathfinding;
			}
			set
			{
				EditorNavigation._isPathfinding = value;
				EditorNavigation.marker.gameObject.SetActive(EditorNavigation.isPathfinding);
				if (!EditorNavigation.isPathfinding)
				{
					EditorNavigation.select(null);
				}
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0006E6B8 File Offset: 0x0006C8B8
		public static Flag flag
		{
			get
			{
				return EditorNavigation._flag;
			}
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x0006E6C0 File Offset: 0x0006C8C0
		private static void select(Transform select)
		{
			if (EditorNavigation.selection != null)
			{
				EditorNavigation.selection.GetComponent<Renderer>().material.color = Color.white;
			}
			if (EditorNavigation.selection == select || select == null)
			{
				EditorNavigation.selection = null;
				EditorNavigation._flag = null;
			}
			else
			{
				EditorNavigation.selection = select;
				EditorNavigation._flag = LevelNavigation.getFlag(EditorNavigation.selection);
				EditorNavigation.selection.GetComponent<Renderer>().material.color = Color.red;
			}
			EditorEnvironmentNavigationUI.updateSelection(EditorNavigation.flag);
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0006E750 File Offset: 0x0006C950
		private void Update()
		{
			if (!EditorNavigation.isPathfinding)
			{
				return;
			}
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (EditorInteract.worldHit.transform != null)
				{
					EditorNavigation.marker.position = EditorInteract.worldHit.point;
				}
				if ((InputEx.GetKeyDown(KeyCode.Delete) || InputEx.GetKeyDown(KeyCode.Backspace)) && EditorNavigation.selection != null)
				{
					Transform select = EditorNavigation.selection;
					EditorNavigation.select(null);
					LevelNavigation.removeFlag(select);
				}
				if (InputEx.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.worldHit.transform != null && EditorNavigation.selection != null)
				{
					Vector3 point = EditorInteract.worldHit.point;
					EditorNavigation.flag.move(point);
				}
				if (InputEx.GetKeyDown(ControlsSettings.primary))
				{
					if (EditorInteract.logicHit.transform != null)
					{
						if (EditorInteract.logicHit.transform.name == "Flag")
						{
							EditorNavigation.select(EditorInteract.logicHit.transform);
							return;
						}
					}
					else if (EditorInteract.worldHit.transform != null)
					{
						EditorNavigation.select(LevelNavigation.addFlag(EditorInteract.worldHit.point));
					}
				}
			}
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x0006E8A0 File Offset: 0x0006CAA0
		private void Start()
		{
			EditorNavigation._isPathfinding = false;
			EditorNavigation.marker = ((GameObject)Object.Instantiate(Resources.Load("Edit/Marker"))).transform;
			EditorNavigation.marker.name = "Marker";
			EditorNavigation.marker.parent = Level.editing;
			EditorNavigation.marker.gameObject.SetActive(false);
			EditorNavigation.marker.GetComponent<Renderer>().material.color = Color.red;
			Object.Destroy(EditorNavigation.marker.GetComponent<Collider>());
		}

		// Token: 0x04000E8B RID: 3723
		private static bool _isPathfinding;

		// Token: 0x04000E8C RID: 3724
		private static Flag _flag;

		// Token: 0x04000E8D RID: 3725
		private static Transform selection;

		// Token: 0x04000E8E RID: 3726
		private static Transform marker;
	}
}
