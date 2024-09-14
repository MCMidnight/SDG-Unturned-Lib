using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200077A RID: 1914
	public class EditorLevelVisibilityUI
	{
		// Token: 0x06003EB5 RID: 16053 RVA: 0x00134540 File Offset: 0x00132740
		public static void open()
		{
			if (EditorLevelVisibilityUI.active)
			{
				return;
			}
			EditorLevelVisibilityUI.active = true;
			EditorLevelVisibilityUI.update((int)Editor.editor.area.region_x, (int)Editor.editor.area.region_y);
			EditorUI.message(EEditorMessage.VISIBILITY);
			EditorLevelVisibilityUI.container.AnimateIntoView();
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x00134590 File Offset: 0x00132790
		public static void close()
		{
			if (!EditorLevelVisibilityUI.active)
			{
				return;
			}
			EditorLevelVisibilityUI.active = false;
			for (int i = 0; i < EditorLevelVisibilityUI.regionLabels.Length; i++)
			{
				EditorLevelVisibilityUI.regionLabels[i].IsVisible = false;
			}
			EditorLevelVisibilityUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x001345DE File Offset: 0x001327DE
		private static void onToggledRoadsToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.roadsVisible = state;
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x001345E6 File Offset: 0x001327E6
		private static void onToggledNavigationToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.navigationVisible = state;
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x001345EE File Offset: 0x001327EE
		private static void onToggledNodesToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.nodesVisible = state;
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x001345F6 File Offset: 0x001327F6
		private static void onToggledItemsToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.itemsVisible = state;
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x001345FE File Offset: 0x001327FE
		private static void onToggledPlayersToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.playersVisible = state;
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00134606 File Offset: 0x00132806
		private static void onToggledZombiesToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.zombiesVisible = state;
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0013460E File Offset: 0x0013280E
		private static void onToggledVehiclesToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.vehiclesVisible = state;
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x00134616 File Offset: 0x00132816
		private static void onToggledBorderToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.borderVisible = state;
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x0013461E File Offset: 0x0013281E
		private static void onToggledAnimalsToggle(ISleekToggle toggle, bool state)
		{
			LevelVisibility.animalsVisible = state;
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x00134626 File Offset: 0x00132826
		private static void onToggledDecalsToggle(ISleekToggle toggle, bool state)
		{
			DecalSystem.IsVisible = state;
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0013462E File Offset: 0x0013282E
		private static void onRegionUpdated(byte old_x, byte old_y, byte new_x, byte new_y)
		{
			if (!EditorLevelVisibilityUI.active)
			{
				return;
			}
			EditorLevelVisibilityUI.update((int)new_x, (int)new_y);
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x00134640 File Offset: 0x00132840
		private static void update(int x, int y)
		{
			for (int i = 0; i < (int)EditorLevelVisibilityUI.DEBUG_SIZE; i++)
			{
				for (int j = 0; j < (int)EditorLevelVisibilityUI.DEBUG_SIZE; j++)
				{
					int num = i * (int)EditorLevelVisibilityUI.DEBUG_SIZE + j;
					int num2 = x - (int)(EditorLevelVisibilityUI.DEBUG_SIZE / 2) + i;
					int num3 = y - (int)(EditorLevelVisibilityUI.DEBUG_SIZE / 2) + j;
					ISleekLabel sleekLabel = EditorLevelVisibilityUI.regionLabels[num];
					if (Regions.checkSafe(num2, num3))
					{
						int num4 = LevelObjects.objects[num2, num3].Count + LevelGround.trees[num2, num3].Count;
						int num5 = LevelObjects.total + LevelGround.total;
						double num6 = Math.Round((double)num4 / (double)num5 * 1000.0) / 10.0;
						int num7 = 0;
						for (int k = 0; k < LevelObjects.objects[num2, num3].Count; k++)
						{
							LevelObject levelObject = LevelObjects.objects[num2, num3][k];
							if (levelObject.transform)
							{
								levelObject.transform.GetComponents<MeshFilter>(EditorLevelVisibilityUI.meshes);
								if (EditorLevelVisibilityUI.meshes.Count == 0)
								{
									Transform transform = levelObject.transform.Find("Model_0");
									if (transform)
									{
										transform.GetComponentsInChildren<MeshFilter>(true, EditorLevelVisibilityUI.meshes);
									}
								}
								if (EditorLevelVisibilityUI.meshes.Count != 0)
								{
									for (int l = 0; l < EditorLevelVisibilityUI.meshes.Count; l++)
									{
										Mesh sharedMesh = EditorLevelVisibilityUI.meshes[l].sharedMesh;
										if (sharedMesh)
										{
											num7 += sharedMesh.triangles.Length;
										}
									}
								}
							}
						}
						for (int m = 0; m < LevelGround.trees[num2, num3].Count; m++)
						{
							ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[num2, num3][m];
							if (resourceSpawnpoint.model)
							{
								resourceSpawnpoint.model.GetComponents<MeshFilter>(EditorLevelVisibilityUI.meshes);
								if (EditorLevelVisibilityUI.meshes.Count == 0)
								{
									Transform transform2 = resourceSpawnpoint.model.Find("Model_0");
									if (transform2)
									{
										transform2.GetComponentsInChildren<MeshFilter>(true, EditorLevelVisibilityUI.meshes);
									}
								}
								if (EditorLevelVisibilityUI.meshes.Count != 0)
								{
									for (int n = 0; n < EditorLevelVisibilityUI.meshes.Count; n++)
									{
										Mesh sharedMesh2 = EditorLevelVisibilityUI.meshes[n].sharedMesh;
										if (sharedMesh2)
										{
											num7 += sharedMesh2.triangles.Length;
										}
									}
								}
							}
						}
						long num8 = (long)num4 * (long)num7;
						float quality = Mathf.Clamp01((float)(1.0 - (double)num8 / 50000000.0));
						sleekLabel.Text = EditorLevelVisibilityUI.localization.format("Point", num2, num3);
						ISleekLabel sleekLabel2 = sleekLabel;
						sleekLabel2.Text = sleekLabel2.Text + "\n" + EditorLevelVisibilityUI.localization.format("Objects", num4, num6);
						ISleekLabel sleekLabel3 = sleekLabel;
						sleekLabel3.Text = sleekLabel3.Text + "\n" + EditorLevelVisibilityUI.localization.format("Triangles", num7);
						if (num4 == 0 && num7 == 0)
						{
							sleekLabel.TextColor = Color.white;
						}
						else
						{
							sleekLabel.TextColor = ItemTool.getQualityColor(quality);
						}
					}
				}
			}
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x001349AC File Offset: 0x00132BAC
		public static void update()
		{
			for (int i = 0; i < (int)EditorLevelVisibilityUI.DEBUG_SIZE; i++)
			{
				for (int j = 0; j < (int)EditorLevelVisibilityUI.DEBUG_SIZE; j++)
				{
					int num = i * (int)EditorLevelVisibilityUI.DEBUG_SIZE + j;
					int x = (int)(Editor.editor.area.region_x - EditorLevelVisibilityUI.DEBUG_SIZE / 2) + i;
					int y = (int)(Editor.editor.area.region_y - EditorLevelVisibilityUI.DEBUG_SIZE / 2) + j;
					ISleekLabel sleekLabel = EditorLevelVisibilityUI.regionLabels[num];
					Vector3 a;
					if (Regions.tryGetPoint(x, y, out a))
					{
						Vector3 vector = MainCamera.instance.WorldToViewportPoint(a + new Vector3((float)(Regions.REGION_SIZE / 2), 0f, (float)(Regions.REGION_SIZE / 2)));
						if (vector.z > 0f)
						{
							Vector2 vector2 = EditorLevelVisibilityUI.container.ViewportToNormalizedPosition(vector);
							sleekLabel.PositionScale_X = vector2.x;
							sleekLabel.PositionScale_Y = vector2.y;
							sleekLabel.IsVisible = true;
						}
						else
						{
							sleekLabel.IsVisible = false;
						}
					}
					else
					{
						sleekLabel.IsVisible = false;
					}
				}
			}
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x00134ABC File Offset: 0x00132CBC
		public EditorLevelVisibilityUI()
		{
			EditorLevelVisibilityUI.localization = Localization.read("/Editor/EditorLevelVisibility.dat");
			EditorLevelVisibilityUI.container = new SleekFullscreenBox();
			EditorLevelVisibilityUI.container.PositionScale_X = 1f;
			EditorLevelVisibilityUI.container.SizeScale_X = 1f;
			EditorLevelVisibilityUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorLevelVisibilityUI.container);
			EditorLevelVisibilityUI.active = false;
			EditorLevelVisibilityUI.roadsToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.roadsToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.roadsToggle.PositionOffset_Y = 90f;
			EditorLevelVisibilityUI.roadsToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.roadsToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.roadsToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.roadsToggle.Value = LevelVisibility.roadsVisible;
			EditorLevelVisibilityUI.roadsToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Roads_Label"), 1);
			EditorLevelVisibilityUI.roadsToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledRoadsToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.roadsToggle);
			EditorLevelVisibilityUI.navigationToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.navigationToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.navigationToggle.PositionOffset_Y = 140f;
			EditorLevelVisibilityUI.navigationToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.navigationToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.navigationToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.navigationToggle.Value = LevelVisibility.navigationVisible;
			EditorLevelVisibilityUI.navigationToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Navigation_Label"), 1);
			EditorLevelVisibilityUI.navigationToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledNavigationToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.navigationToggle);
			EditorLevelVisibilityUI.nodesToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.nodesToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.nodesToggle.PositionOffset_Y = 190f;
			EditorLevelVisibilityUI.nodesToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.nodesToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.nodesToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.nodesToggle.Value = LevelVisibility.nodesVisible;
			EditorLevelVisibilityUI.nodesToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Nodes_Label"), 1);
			EditorLevelVisibilityUI.nodesToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledNodesToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.nodesToggle);
			EditorLevelVisibilityUI.itemsToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.itemsToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.itemsToggle.PositionOffset_Y = 240f;
			EditorLevelVisibilityUI.itemsToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.itemsToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.itemsToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.itemsToggle.Value = LevelVisibility.itemsVisible;
			EditorLevelVisibilityUI.itemsToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Items_Label"), 1);
			EditorLevelVisibilityUI.itemsToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledItemsToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.itemsToggle);
			EditorLevelVisibilityUI.playersToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.playersToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.playersToggle.PositionOffset_Y = 290f;
			EditorLevelVisibilityUI.playersToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.playersToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.playersToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.playersToggle.Value = LevelVisibility.playersVisible;
			EditorLevelVisibilityUI.playersToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Players_Label"), 1);
			EditorLevelVisibilityUI.playersToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledPlayersToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.playersToggle);
			EditorLevelVisibilityUI.zombiesToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.zombiesToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.zombiesToggle.PositionOffset_Y = 340f;
			EditorLevelVisibilityUI.zombiesToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.zombiesToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.zombiesToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.zombiesToggle.Value = LevelVisibility.zombiesVisible;
			EditorLevelVisibilityUI.zombiesToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Zombies_Label"), 1);
			EditorLevelVisibilityUI.zombiesToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledZombiesToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.zombiesToggle);
			EditorLevelVisibilityUI.vehiclesToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.vehiclesToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.vehiclesToggle.PositionOffset_Y = 390f;
			EditorLevelVisibilityUI.vehiclesToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.vehiclesToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.vehiclesToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.vehiclesToggle.Value = LevelVisibility.vehiclesVisible;
			EditorLevelVisibilityUI.vehiclesToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Vehicles_Label"), 1);
			EditorLevelVisibilityUI.vehiclesToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledVehiclesToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.vehiclesToggle);
			EditorLevelVisibilityUI.borderToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.borderToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.borderToggle.PositionOffset_Y = 440f;
			EditorLevelVisibilityUI.borderToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.borderToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.borderToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.borderToggle.Value = LevelVisibility.borderVisible;
			EditorLevelVisibilityUI.borderToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Border_Label"), 1);
			EditorLevelVisibilityUI.borderToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledBorderToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.borderToggle);
			EditorLevelVisibilityUI.animalsToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.animalsToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.animalsToggle.PositionOffset_Y = 490f;
			EditorLevelVisibilityUI.animalsToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.animalsToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.animalsToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.animalsToggle.Value = LevelVisibility.animalsVisible;
			EditorLevelVisibilityUI.animalsToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Animals_Label"), 1);
			EditorLevelVisibilityUI.animalsToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledAnimalsToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.animalsToggle);
			EditorLevelVisibilityUI.decalsToggle = Glazier.Get().CreateToggle();
			EditorLevelVisibilityUI.decalsToggle.PositionOffset_X = -210f;
			EditorLevelVisibilityUI.decalsToggle.PositionOffset_Y = 540f;
			EditorLevelVisibilityUI.decalsToggle.PositionScale_X = 1f;
			EditorLevelVisibilityUI.decalsToggle.SizeOffset_X = 40f;
			EditorLevelVisibilityUI.decalsToggle.SizeOffset_Y = 40f;
			EditorLevelVisibilityUI.decalsToggle.Value = DecalSystem.IsVisible;
			EditorLevelVisibilityUI.decalsToggle.AddLabel(EditorLevelVisibilityUI.localization.format("Decals_Label"), 1);
			EditorLevelVisibilityUI.decalsToggle.OnValueChanged += new Toggled(EditorLevelVisibilityUI.onToggledDecalsToggle);
			EditorLevelVisibilityUI.container.AddChild(EditorLevelVisibilityUI.decalsToggle);
			EditorLevelVisibilityUI.regionLabels = new ISleekLabel[(int)(EditorLevelVisibilityUI.DEBUG_SIZE * EditorLevelVisibilityUI.DEBUG_SIZE)];
			for (int i = 0; i < EditorLevelVisibilityUI.regionLabels.Length; i++)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.PositionOffset_X = -100f;
				sleekLabel.PositionOffset_Y = -25f;
				sleekLabel.SizeOffset_X = 200f;
				sleekLabel.SizeOffset_Y = 50f;
				sleekLabel.TextContrastContext = 2;
				EditorLevelVisibilityUI.regionLabels[i] = sleekLabel;
				EditorLevelVisibilityUI.container.AddChild(sleekLabel);
			}
			EditorArea area = Editor.editor.area;
			area.onRegionUpdated = (EditorRegionUpdated)Delegate.Combine(area.onRegionUpdated, new EditorRegionUpdated(EditorLevelVisibilityUI.onRegionUpdated));
		}

		// Token: 0x04002789 RID: 10121
		private static readonly byte DEBUG_SIZE = 7;

		// Token: 0x0400278A RID: 10122
		private static Local localization;

		// Token: 0x0400278B RID: 10123
		private static SleekFullscreenBox container;

		// Token: 0x0400278C RID: 10124
		public static bool active;

		// Token: 0x0400278D RID: 10125
		private static List<MeshFilter> meshes = new List<MeshFilter>();

		// Token: 0x0400278E RID: 10126
		public static ISleekToggle roadsToggle;

		// Token: 0x0400278F RID: 10127
		public static ISleekToggle navigationToggle;

		// Token: 0x04002790 RID: 10128
		public static ISleekToggle nodesToggle;

		// Token: 0x04002791 RID: 10129
		public static ISleekToggle itemsToggle;

		// Token: 0x04002792 RID: 10130
		public static ISleekToggle playersToggle;

		// Token: 0x04002793 RID: 10131
		public static ISleekToggle zombiesToggle;

		// Token: 0x04002794 RID: 10132
		public static ISleekToggle vehiclesToggle;

		// Token: 0x04002795 RID: 10133
		public static ISleekToggle borderToggle;

		// Token: 0x04002796 RID: 10134
		public static ISleekToggle animalsToggle;

		// Token: 0x04002797 RID: 10135
		public static ISleekToggle decalsToggle;

		// Token: 0x04002798 RID: 10136
		private static ISleekLabel[] regionLabels;
	}
}
