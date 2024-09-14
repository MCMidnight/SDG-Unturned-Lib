using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000407 RID: 1031
	public class EditorRoads : MonoBehaviour
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x0006FF6C File Offset: 0x0006E16C
		// (set) Token: 0x06001E79 RID: 7801 RVA: 0x0006FF73 File Offset: 0x0006E173
		public static bool isPaving
		{
			get
			{
				return EditorRoads._isPaving;
			}
			set
			{
				EditorRoads._isPaving = value;
				EditorRoads.highlighter.gameObject.SetActive(EditorRoads.isPaving);
				if (!EditorRoads.isPaving)
				{
					EditorRoads.select(null);
				}
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x0006FF9C File Offset: 0x0006E19C
		public static Road road
		{
			get
			{
				return EditorRoads._road;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001E7B RID: 7803 RVA: 0x0006FFA3 File Offset: 0x0006E1A3
		public static RoadPath path
		{
			get
			{
				return EditorRoads._path;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x0006FFAA File Offset: 0x0006E1AA
		public static RoadJoint joint
		{
			get
			{
				return EditorRoads._joint;
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0006FFB4 File Offset: 0x0006E1B4
		private static void select(Transform target)
		{
			if (EditorRoads.road != null)
			{
				if (EditorRoads.tangentIndex > -1)
				{
					EditorRoads.path.unhighlightTangent(EditorRoads.tangentIndex);
				}
				else if (EditorRoads.vertexIndex > -1)
				{
					EditorRoads.path.unhighlightVertex();
				}
			}
			if (EditorRoads.selection == target || target == null)
			{
				EditorRoads.deselect();
			}
			else
			{
				EditorRoads.selection = target;
				EditorRoads._road = LevelRoads.getRoad(EditorRoads.selection, out EditorRoads.vertexIndex, out EditorRoads.tangentIndex);
				if (EditorRoads.road != null)
				{
					EditorRoads._path = EditorRoads.road.paths[EditorRoads.vertexIndex];
					EditorRoads._joint = EditorRoads.road.joints[EditorRoads.vertexIndex];
					if (EditorRoads.tangentIndex > -1)
					{
						EditorRoads.path.highlightTangent(EditorRoads.tangentIndex);
					}
					else if (EditorRoads.vertexIndex > -1)
					{
						EditorRoads.path.highlightVertex();
					}
				}
				else
				{
					EditorRoads._path = null;
					EditorRoads._joint = null;
				}
			}
			EditorEnvironmentRoadsUI.updateSelection(EditorRoads.road, EditorRoads.joint);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000700B3 File Offset: 0x0006E2B3
		private static void deselect()
		{
			EditorRoads.selection = null;
			EditorRoads._road = null;
			EditorRoads._path = null;
			EditorRoads._joint = null;
			EditorRoads.vertexIndex = -1;
			EditorRoads.tangentIndex = -1;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000700DC File Offset: 0x0006E2DC
		private void Update()
		{
			if (!EditorRoads.isPaving)
			{
				return;
			}
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (EditorInteract.worldHit.transform != null)
				{
					EditorRoads.highlighter.position = EditorInteract.worldHit.point;
				}
				if ((InputEx.GetKeyDown(KeyCode.Delete) || InputEx.GetKeyDown(KeyCode.Backspace)) && EditorRoads.selection != null && EditorRoads.road != null)
				{
					if (InputEx.GetKey(ControlsSettings.other))
					{
						LevelRoads.removeRoad(EditorRoads.road);
					}
					else
					{
						EditorRoads.road.removeVertex(EditorRoads.vertexIndex);
					}
					EditorRoads.deselect();
				}
				if (InputEx.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.worldHit.transform != null)
				{
					Vector3 point = EditorInteract.worldHit.point;
					if (EditorRoads.road != null)
					{
						if (EditorRoads.tangentIndex > -1)
						{
							EditorRoads.road.moveTangent(EditorRoads.vertexIndex, EditorRoads.tangentIndex, point - EditorRoads.joint.vertex);
						}
						else if (EditorRoads.vertexIndex > -1)
						{
							EditorRoads.road.moveVertex(EditorRoads.vertexIndex, point);
						}
					}
				}
				if (InputEx.GetKeyDown(ControlsSettings.primary))
				{
					if (EditorInteract.logicHit.transform != null)
					{
						if (EditorInteract.logicHit.transform.name.IndexOf("Path") != -1 || EditorInteract.logicHit.transform.name.IndexOf("Tangent") != -1)
						{
							EditorRoads.select(EditorInteract.logicHit.transform);
							return;
						}
					}
					else if (EditorInteract.worldHit.transform != null)
					{
						Vector3 point2 = EditorInteract.worldHit.point;
						if (EditorRoads.road != null)
						{
							if (EditorRoads.tangentIndex > -1)
							{
								EditorRoads.select(EditorRoads.road.addVertex(EditorRoads.vertexIndex + EditorRoads.tangentIndex, point2));
								return;
							}
							float num = Vector3.Dot(point2 - EditorRoads.joint.vertex, EditorRoads.joint.getTangent(0));
							float num2 = Vector3.Dot(point2 - EditorRoads.joint.vertex, EditorRoads.joint.getTangent(1));
							if (num > num2)
							{
								EditorRoads.select(EditorRoads.road.addVertex(EditorRoads.vertexIndex, point2));
								return;
							}
							EditorRoads.select(EditorRoads.road.addVertex(EditorRoads.vertexIndex + 1, point2));
							return;
						}
						else
						{
							EditorRoads.select(LevelRoads.addRoad(point2));
						}
					}
				}
			}
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0007034C File Offset: 0x0006E54C
		private void Start()
		{
			EditorRoads._isPaving = false;
			EditorRoads.highlighter = ((GameObject)Object.Instantiate(Resources.Load("Edit/Highlighter"))).transform;
			EditorRoads.highlighter.name = "Highlighter";
			EditorRoads.highlighter.parent = Level.editing;
			EditorRoads.highlighter.gameObject.SetActive(false);
			EditorRoads.highlighter.GetComponent<Renderer>().material.color = Color.red;
			EditorRoads.deselect();
		}

		// Token: 0x04000EAB RID: 3755
		private static bool _isPaving;

		// Token: 0x04000EAC RID: 3756
		public static byte selected;

		// Token: 0x04000EAD RID: 3757
		private static Road _road;

		// Token: 0x04000EAE RID: 3758
		private static RoadPath _path;

		// Token: 0x04000EAF RID: 3759
		private static RoadJoint _joint;

		// Token: 0x04000EB0 RID: 3760
		private static int vertexIndex;

		// Token: 0x04000EB1 RID: 3761
		private static int tangentIndex;

		// Token: 0x04000EB2 RID: 3762
		private static Transform selection;

		// Token: 0x04000EB3 RID: 3763
		private static Transform highlighter;
	}
}
