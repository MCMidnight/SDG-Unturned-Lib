using System;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000755 RID: 1877
	public class HighlighterTool
	{
		// Token: 0x06003D6B RID: 15723 RVA: 0x00126D70 File Offset: 0x00124F70
		public static void color(Transform target, Color color)
		{
			if (target == null)
			{
				return;
			}
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().material.color = color;
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.Find("Model_" + i.ToString());
				if (!(transform == null) && transform.GetComponent<Renderer>() != null)
				{
					transform.GetComponent<Renderer>().material.color = color;
				}
			}
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x00126DF4 File Offset: 0x00124FF4
		public static void destroyMaterials(Transform target)
		{
			if (target == null)
			{
				return;
			}
			if (target.GetComponent<Renderer>() != null)
			{
				Object.DestroyImmediate(target.GetComponent<Renderer>().material);
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.Find("Model_" + i.ToString());
				if (!(transform == null) && transform.GetComponent<Renderer>() != null)
				{
					Object.DestroyImmediate(transform.GetComponent<Renderer>().material);
				}
			}
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x00126E75 File Offset: 0x00125075
		public static void help(Transform target, bool isValid)
		{
			HighlighterTool.help(target, isValid, false);
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x00126E80 File Offset: 0x00125080
		public static void help(Transform target, bool isValid, bool isRecursive)
		{
			Material sharedMaterial = isValid ? ((Material)Resources.Load("Materials/PlacementPreview_Valid")) : ((Material)Resources.Load("Materials/PlacementPreview_Invalid"));
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().sharedMaterial = sharedMaterial;
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Transform transform;
				if (isRecursive)
				{
					transform = target.FindChildRecursive("Model_" + i.ToString());
				}
				else
				{
					transform = target.Find("Model_" + i.ToString());
				}
				if (!(transform == null) && transform.GetComponent<Renderer>() != null)
				{
					transform.GetComponent<Renderer>().sharedMaterial = sharedMaterial;
				}
			}
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x00126F30 File Offset: 0x00125130
		public static void guide(Transform target)
		{
			Material sharedMaterial = (Material)Resources.Load("Materials/Guide");
			HighlighterTool.renderers.Clear();
			target.GetComponentsInChildren<Renderer>(true, HighlighterTool.renderers);
			for (int i = 0; i < HighlighterTool.renderers.Count; i++)
			{
				if (!(HighlighterTool.renderers[i].transform != target) || HighlighterTool.renderers[i].name.IndexOf("Model") != -1)
				{
					HighlighterTool.renderers[i].sharedMaterial = sharedMaterial;
				}
			}
			List<Collider> list = new List<Collider>();
			target.GetComponentsInChildren<Collider>(list);
			for (int j = 0; j < list.Count; j++)
			{
				Object.Destroy(list[j]);
			}
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x00126FE8 File Offset: 0x001251E8
		public static void highlight(Transform target, Color color)
		{
			if (target.CompareTag("Player") || target.CompareTag("Enemy") || target.CompareTag("Zombie") || target.CompareTag("Animal") || target.CompareTag("Agent"))
			{
				return;
			}
			Highlighter highlighter = target.GetComponent<Highlighter>();
			if (highlighter == null)
			{
				highlighter = target.gameObject.AddComponent<Highlighter>();
			}
			highlighter.ConstantOn(color, 0.25f);
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00127060 File Offset: 0x00125260
		public static void unhighlight(Transform target)
		{
			Highlighter component = target.GetComponent<Highlighter>();
			if (component == null)
			{
				return;
			}
			Object.DestroyImmediate(component);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x00127084 File Offset: 0x00125284
		public static void skin(Transform target, Material skin)
		{
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().sharedMaterial = skin;
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.Find("Model_" + i.ToString());
				if (!(transform == null) && transform.GetComponent<Renderer>() != null)
				{
					transform.GetComponent<Renderer>().sharedMaterial = skin;
				}
			}
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x001270F4 File Offset: 0x001252F4
		[Obsolete]
		public static Material getMaterial(Transform target)
		{
			if (target == null)
			{
				return null;
			}
			Renderer component = target.GetComponent<Renderer>();
			if (component != null)
			{
				return component.sharedMaterial;
			}
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.Find("Model_" + i.ToString());
				if (transform == null)
				{
					return null;
				}
				component = transform.GetComponent<Renderer>();
				if (component != null)
				{
					return component.sharedMaterial;
				}
			}
			return null;
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x0012716C File Offset: 0x0012536C
		public static Material getMaterialInstance(Transform target)
		{
			if (target == null)
			{
				return null;
			}
			Renderer component = target.GetComponent<Renderer>();
			if (component != null)
			{
				return component.material;
			}
			Material material = null;
			Material y = null;
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.Find("Model_" + i.ToString());
				if (transform == null)
				{
					break;
				}
				component = transform.GetComponent<Renderer>();
				if (component != null)
				{
					if (material == null)
					{
						y = component.sharedMaterial;
						material = component.material;
					}
					else if (component.sharedMaterial == y)
					{
						component.sharedMaterial = material;
					}
				}
			}
			return material;
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x00127210 File Offset: 0x00125410
		public static void remesh(Transform target, List<Mesh> newMeshes, List<Mesh> outOldMeshes)
		{
			if (newMeshes == null || newMeshes.Count < 1)
			{
				return;
			}
			if (outOldMeshes != null && outOldMeshes != newMeshes)
			{
				outOldMeshes.Clear();
			}
			MeshFilter component = target.GetComponent<MeshFilter>();
			if (component != null)
			{
				Mesh sharedMesh = component.sharedMesh;
				component.sharedMesh = newMeshes[0];
				if (outOldMeshes != null)
				{
					if (outOldMeshes == newMeshes)
					{
						newMeshes[0] = sharedMesh;
						return;
					}
					outOldMeshes.Add(sharedMesh);
					return;
				}
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = target.Find("Model_" + i.ToString());
					if (!(transform == null))
					{
						component = transform.GetComponent<MeshFilter>();
						if (component != null)
						{
							Mesh sharedMesh2 = component.sharedMesh;
							component.sharedMesh = ((i < newMeshes.Count) ? newMeshes[i] : newMeshes[0]);
							if (outOldMeshes != null)
							{
								if (outOldMeshes == newMeshes)
								{
									if (i < newMeshes.Count)
									{
										newMeshes[i] = sharedMesh2;
									}
									else
									{
										newMeshes.Add(sharedMesh2);
									}
								}
								else
								{
									outOldMeshes.Add(sharedMesh2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x00127310 File Offset: 0x00125510
		public static void rematerialize(Transform target, Material newMaterial, out Material oldMaterial)
		{
			oldMaterial = null;
			Renderer component = target.GetComponent<Renderer>();
			if (component != null)
			{
				oldMaterial = component.sharedMaterial;
				component.sharedMaterial = newMaterial;
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.Find("Model_" + i.ToString());
				if (!(transform == null))
				{
					component = transform.GetComponent<Renderer>();
					if (component != null)
					{
						oldMaterial = component.sharedMaterial;
						component.sharedMaterial = newMaterial;
					}
				}
			}
		}

		// Token: 0x040026A7 RID: 9895
		private static List<Renderer> renderers = new List<Renderer>();
	}
}
