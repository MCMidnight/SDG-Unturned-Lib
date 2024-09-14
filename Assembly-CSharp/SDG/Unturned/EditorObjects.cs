using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000406 RID: 1030
	public class EditorObjects : MonoBehaviour
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x0006E92F File Offset: 0x0006CB2F
		// (set) Token: 0x06001E5F RID: 7775 RVA: 0x0006E936 File Offset: 0x0006CB36
		public static bool isBuilding
		{
			get
			{
				return EditorObjects._isBuilding;
			}
			set
			{
				EditorObjects._isBuilding = value;
				if (!EditorObjects.isBuilding)
				{
					EditorObjects.clearSelection();
				}
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0006E94A File Offset: 0x0006CB4A
		public static GameObject GetMostRecentSelectedGameObject()
		{
			if (EditorObjects.selection.Count <= 0)
			{
				return null;
			}
			EditorSelection editorSelection = EditorObjects.selection[EditorObjects.selection.Count - 1];
			if (editorSelection == null)
			{
				return null;
			}
			Transform transform = editorSelection.transform;
			if (transform == null)
			{
				return null;
			}
			return transform.gameObject;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0006E987 File Offset: 0x0006CB87
		public static IEnumerable<GameObject> EnumerateSelectedGameObjects()
		{
			foreach (EditorSelection editorSelection in EditorObjects.selection)
			{
				if (editorSelection.transform != null)
				{
					yield return editorSelection.transform.gameObject;
				}
			}
			List<EditorSelection>.Enumerator enumerator = default(List<EditorSelection>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x0006E990 File Offset: 0x0006CB90
		// (set) Token: 0x06001E63 RID: 7779 RVA: 0x0006E997 File Offset: 0x0006CB97
		public static EDragMode dragMode
		{
			get
			{
				return EditorObjects._dragMode;
			}
			set
			{
				if (value == EDragMode.SCALE)
				{
					EditorObjects._dragCoordinate = EDragCoordinate.LOCAL;
				}
				else if (EditorObjects.dragMode == EDragMode.SCALE)
				{
					EditorObjects._dragCoordinate = (EDragCoordinate)EditorLevelObjectsUI.coordinateButton.state;
				}
				EditorObjects.wantsBoundsEditor = false;
				EditorObjects._dragMode = value;
				EditorObjects.calculateHandleOffsets();
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x0006E9CD File Offset: 0x0006CBCD
		// (set) Token: 0x06001E65 RID: 7781 RVA: 0x0006E9D4 File Offset: 0x0006CBD4
		public static EDragCoordinate dragCoordinate
		{
			get
			{
				return EditorObjects._dragCoordinate;
			}
			set
			{
				if (EditorObjects.dragMode == EDragMode.SCALE)
				{
					return;
				}
				EditorObjects._dragCoordinate = value;
				EditorObjects.calculateHandleOffsets();
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0006E9EC File Offset: 0x0006CBEC
		public static void applySelection()
		{
			LevelObjects.step++;
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				LevelObjects.registerTransformObject(EditorObjects.selection[i].transform, EditorObjects.selection[i].transform.position, EditorObjects.selection[i].transform.rotation, EditorObjects.selection[i].transform.localScale, EditorObjects.selection[i].fromPosition, EditorObjects.selection[i].fromRotation, EditorObjects.selection[i].fromScale);
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x0006EAA4 File Offset: 0x0006CCA4
		public static void pointSelection()
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				EditorObjects.selection[i].fromPosition = EditorObjects.selection[i].transform.position;
				EditorObjects.selection[i].fromRotation = EditorObjects.selection[i].transform.rotation;
				EditorObjects.selection[i].fromScale = EditorObjects.selection[i].transform.localScale;
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0006EB38 File Offset: 0x0006CD38
		private static void selectDecals(Transform select, bool isSelected)
		{
			EditorObjects.decals.Clear();
			select.GetComponentsInChildren<Decal>(true, EditorObjects.decals);
			for (int i = 0; i < EditorObjects.decals.Count; i++)
			{
				EditorObjects.decals[i].isSelected = isSelected;
			}
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0006EB81 File Offset: 0x0006CD81
		public static void addSelection(Transform select)
		{
			HighlighterTool.highlight(select, Color.yellow);
			EditorObjects.selectDecals(select, true);
			EditorObjects.selection.Add(new EditorSelection(select, select.position, select.rotation, select.localScale));
			EditorObjects.calculateHandleOffsets();
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0006EBBC File Offset: 0x0006CDBC
		public static void removeSelection(Transform select)
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				if (EditorObjects.selection[i].transform == select)
				{
					HighlighterTool.unhighlight(select);
					EditorObjects.selectDecals(select, false);
					if (EditorObjects.selection[i].transform.CompareTag("Barricade") || EditorObjects.selection[i].transform.CompareTag("Structure"))
					{
						EditorObjects.selection[i].transform.localScale = Vector3.one;
					}
					EditorObjects.selection.RemoveAt(i);
					break;
				}
			}
			EditorObjects.calculateHandleOffsets();
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0006EC70 File Offset: 0x0006CE70
		private static void clearSelection()
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				if (EditorObjects.selection[i].transform != null)
				{
					HighlighterTool.unhighlight(EditorObjects.selection[i].transform);
					EditorObjects.selectDecals(EditorObjects.selection[i].transform, false);
					if (EditorObjects.selection[i].transform.CompareTag("Barricade") || EditorObjects.selection[i].transform.CompareTag("Structure"))
					{
						EditorObjects.selection[i].transform.localScale = Vector3.one;
					}
				}
			}
			EditorObjects.selection.Clear();
			EditorObjects.calculateHandleOffsets();
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0006ED3C File Offset: 0x0006CF3C
		public static bool containsSelection(Transform select)
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				if (EditorObjects.selection[i].transform == select)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0006ED7C File Offset: 0x0006CF7C
		private static void calculateHandleOffsets()
		{
			if (EditorObjects.selection.Count == 0)
			{
				return;
			}
			if (EditorObjects.dragCoordinate == EDragCoordinate.GLOBAL)
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < EditorObjects.selection.Count; i++)
				{
					vector += EditorObjects.selection[i].transform.position;
				}
				vector /= (float)EditorObjects.selection.Count;
				EditorObjects.handles.SetPreferredPivot(vector, Quaternion.identity);
				return;
			}
			EditorObjects.handles.SetPreferredPivot(EditorObjects.selection[0].transform.position, EditorObjects.selection[0].transform.rotation);
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0006EE2C File Offset: 0x0006D02C
		private void OnHandlePreTransform(Matrix4x4 worldToPivot)
		{
			foreach (EditorSelection editorSelection in EditorObjects.selection)
			{
				editorSelection.fromPosition = editorSelection.transform.position;
				editorSelection.fromRotation = editorSelection.transform.rotation;
				editorSelection.fromScale = editorSelection.transform.localScale;
				editorSelection.relativeToPivot = worldToPivot * editorSelection.transform.localToWorldMatrix;
			}
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0006EEC4 File Offset: 0x0006D0C4
		private void OnHandleTranslatedAndRotated(Vector3 worldPositionDelta, Quaternion worldRotationDelta, Vector3 pivotPosition, bool modifyRotation)
		{
			foreach (EditorSelection editorSelection in EditorObjects.selection)
			{
				Vector3 vector = editorSelection.fromPosition - pivotPosition;
				if (!vector.IsNearlyZero(0.001f))
				{
					editorSelection.transform.position = pivotPosition + worldRotationDelta * vector + worldPositionDelta;
				}
				else
				{
					editorSelection.transform.position = editorSelection.fromPosition + worldPositionDelta;
				}
				if (modifyRotation)
				{
					editorSelection.transform.rotation = worldRotationDelta * editorSelection.fromRotation;
				}
			}
			EditorObjects.calculateHandleOffsets();
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0006EF80 File Offset: 0x0006D180
		private void OnHandleTransformed(Matrix4x4 pivotToWorld)
		{
			foreach (EditorSelection editorSelection in EditorObjects.selection)
			{
				Matrix4x4 matrix = pivotToWorld * editorSelection.relativeToPivot;
				editorSelection.transform.position = matrix.GetPosition();
				editorSelection.transform.SetRotation_RoundIfNearlyAxisAligned(matrix.GetRotation(), 0.05f);
				editorSelection.transform.SetLocalScale_RoundIfNearlyEqualToOne(matrix.lossyScale, 0.001f);
			}
			EditorObjects.calculateHandleOffsets();
		}

		/// <summary>
		/// Reset dragging handle and register transformation.
		/// </summary>
		// Token: 0x06001E71 RID: 7793 RVA: 0x0006F01C File Offset: 0x0006D21C
		private void releaseHandle()
		{
			EditorObjects.applySelection();
			this.isUsingHandle = false;
			EditorObjects.handles.MouseUp();
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0006F034 File Offset: 0x0006D234
		private void stopDragging()
		{
			this.dragStartViewportPoint = Vector2.zero;
			this.dragStartScreenPoint = Vector2.zero;
			this.dragEndViewportPoint = Vector2.zero;
			this.dragEndScreenPoint = Vector2.zero;
			this.isDragging = false;
			DragStopped dragStopped = EditorObjects.onDragStopped;
			if (dragStopped == null)
			{
				return;
			}
			dragStopped();
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0006F084 File Offset: 0x0006D284
		private void Update()
		{
			if (!EditorObjects.isBuilding)
			{
				return;
			}
			if (Glazier.Get().ShouldGameProcessInput)
			{
				if (EditorInteract.isFlying)
				{
					if (this.isUsingHandle)
					{
						this.releaseHandle();
					}
					this.hasDragStart = false;
					if (this.isDragging)
					{
						this.stopDragging();
						EditorObjects.clearSelection();
					}
					return;
				}
				EditorObjects.handles.snapPositionInterval = EditorObjects.snapTransform;
				EditorObjects.handles.snapRotationIntervalDegrees = EditorObjects.snapRotation;
				if (EditorObjects.dragMode == EDragMode.TRANSFORM)
				{
					if (EditorObjects.wantsBoundsEditor)
					{
						EditorObjects.handles.SetPreferredMode(TransformHandles.EMode.PositionBounds);
						EditorObjects.handles.UpdateBoundsFromSelection(EditorObjects.EnumerateSelectedGameObjects());
					}
					else
					{
						EditorObjects.handles.SetPreferredMode(TransformHandles.EMode.Position);
					}
				}
				else if (EditorObjects.dragMode == EDragMode.SCALE)
				{
					if (EditorObjects.wantsBoundsEditor)
					{
						EditorObjects.handles.SetPreferredMode(TransformHandles.EMode.ScaleBounds);
						EditorObjects.handles.UpdateBoundsFromSelection(EditorObjects.EnumerateSelectedGameObjects());
					}
					else
					{
						EditorObjects.handles.SetPreferredMode(TransformHandles.EMode.Scale);
					}
				}
				else
				{
					EditorObjects.handles.SetPreferredMode(TransformHandles.EMode.Rotation);
				}
				bool flag = EditorObjects.selection.Count > 0 && EditorObjects.handles.Raycast(EditorInteract.ray);
				if (EditorObjects.selection.Count > 0)
				{
					EditorObjects.handles.Render(EditorInteract.ray);
				}
				if (this.isUsingHandle)
				{
					if (!InputEx.GetKey(ControlsSettings.primary))
					{
						this.releaseHandle();
						return;
					}
					EditorObjects.handles.wantsToSnap = InputEx.GetKey(ControlsSettings.snap);
					EditorObjects.handles.MouseMove(EditorInteract.ray);
					return;
				}
				else
				{
					if (InputEx.GetKeyDown(ControlsSettings.tool_0))
					{
						if (EditorObjects.dragMode != EDragMode.TRANSFORM)
						{
							EditorObjects.dragMode = EDragMode.TRANSFORM;
						}
						else
						{
							EditorObjects.wantsBoundsEditor = !EditorObjects.wantsBoundsEditor;
						}
					}
					if (InputEx.GetKeyDown(ControlsSettings.tool_1))
					{
						EditorObjects.dragMode = EDragMode.ROTATE;
					}
					if (InputEx.GetKeyDown(ControlsSettings.tool_3))
					{
						if (EditorObjects.dragMode != EDragMode.SCALE)
						{
							EditorObjects.dragMode = EDragMode.SCALE;
						}
						else
						{
							EditorObjects.wantsBoundsEditor = !EditorObjects.wantsBoundsEditor;
						}
					}
					if ((InputEx.GetKeyDown(KeyCode.Delete) || InputEx.GetKeyDown(KeyCode.Backspace)) && EditorObjects.selection.Count > 0)
					{
						LevelObjects.step++;
						for (int i = 0; i < EditorObjects.selection.Count; i++)
						{
							LevelObjects.registerRemoveObject(EditorObjects.selection[i].transform);
						}
						EditorObjects.selection.Clear();
						EditorObjects.calculateHandleOffsets();
					}
					if (InputEx.GetKeyDown(KeyCode.Z) && InputEx.GetKey(KeyCode.LeftControl))
					{
						EditorObjects.clearSelection();
						LevelObjects.undo();
					}
					if (InputEx.GetKeyDown(KeyCode.X) && InputEx.GetKey(KeyCode.LeftControl))
					{
						EditorObjects.clearSelection();
						LevelObjects.redo();
					}
					if (InputEx.GetKeyDown(KeyCode.B) && EditorObjects.selection.Count > 0 && InputEx.GetKey(KeyCode.LeftControl))
					{
						EditorObjects.copyPosition = EditorObjects.handles.GetPivotPosition();
						EditorObjects.copyRotation = EditorObjects.handles.GetPivotRotation();
						EditorObjects.hasCopiedRotation = (EditorObjects.dragCoordinate == EDragCoordinate.LOCAL);
						if (EditorObjects.selection.Count == 1)
						{
							EditorObjects.copyScale = EditorObjects.selection[0].transform.localScale;
							EditorObjects.hasCopyScale = true;
						}
						else
						{
							EditorObjects.copyScale = Vector3.one;
							EditorObjects.hasCopyScale = false;
						}
					}
					if (InputEx.GetKeyDown(KeyCode.N) && EditorObjects.selection.Count > 0 && EditorObjects.copyPosition != Vector3.zero && InputEx.GetKey(KeyCode.LeftControl))
					{
						EditorObjects.pointSelection();
						if (EditorObjects.selection.Count == 1)
						{
							EditorObjects.selection[0].transform.position = EditorObjects.copyPosition;
							if (EditorObjects.hasCopiedRotation)
							{
								EditorObjects.selection[0].transform.rotation = EditorObjects.copyRotation;
							}
							if (EditorObjects.hasCopyScale)
							{
								EditorObjects.selection[0].transform.localScale = EditorObjects.copyScale;
							}
							EditorObjects.calculateHandleOffsets();
						}
						else
						{
							EditorObjects.handles.ExternallyTransformPivot(EditorObjects.copyPosition, EditorObjects.copyRotation, EditorObjects.hasCopiedRotation);
						}
						EditorObjects.applySelection();
					}
					if (InputEx.GetKeyDown(KeyCode.C) && EditorObjects.selection.Count > 0 && InputEx.GetKey(KeyCode.LeftControl))
					{
						EditorObjects.copies.Clear();
						for (int j = 0; j < EditorObjects.selection.Count; j++)
						{
							ObjectAsset objectAsset;
							ItemAsset itemAsset;
							LevelObjects.getAssetEditor(EditorObjects.selection[j].transform, out objectAsset, out itemAsset);
							if (objectAsset != null || itemAsset != null)
							{
								EditorObjects.copies.Add(new EditorCopy(EditorObjects.selection[j].transform.position, EditorObjects.selection[j].transform.rotation, EditorObjects.selection[j].transform.localScale, objectAsset, itemAsset));
							}
						}
					}
					if (InputEx.GetKeyDown(KeyCode.V) && EditorObjects.copies.Count > 0 && InputEx.GetKey(KeyCode.LeftControl))
					{
						EditorObjects.clearSelection();
						LevelObjects.step++;
						for (int k = 0; k < EditorObjects.copies.Count; k++)
						{
							Transform transform = LevelObjects.registerAddObject(EditorObjects.copies[k].position, EditorObjects.copies[k].rotation, EditorObjects.copies[k].scale, EditorObjects.copies[k].objectAsset, EditorObjects.copies[k].itemAsset);
							if (transform != null)
							{
								EditorObjects.addSelection(transform);
							}
						}
					}
					if (!this.isUsingHandle)
					{
						if (InputEx.GetKeyDown(ControlsSettings.primary))
						{
							if (flag)
							{
								EditorObjects.pointSelection();
								EditorObjects.handles.MouseDown(EditorInteract.ray);
								this.isUsingHandle = true;
							}
							else if (EditorInteract.objectHit.transform != null)
							{
								if (InputEx.GetKey(ControlsSettings.modify))
								{
									if (EditorObjects.containsSelection(EditorInteract.objectHit.transform))
									{
										EditorObjects.removeSelection(EditorInteract.objectHit.transform);
									}
									else
									{
										EditorObjects.addSelection(EditorInteract.objectHit.transform);
									}
								}
								else if (EditorObjects.containsSelection(EditorInteract.objectHit.transform))
								{
									EditorObjects.clearSelection();
								}
								else
								{
									EditorObjects.clearSelection();
									EditorObjects.addSelection(EditorInteract.objectHit.transform);
								}
							}
							else
							{
								if (!this.isDragging)
								{
									this.hasDragStart = true;
									this.dragStartViewportPoint = InputEx.NormalizedMousePosition;
									this.dragStartScreenPoint = Input.mousePosition;
								}
								if (!InputEx.GetKey(ControlsSettings.modify))
								{
									EditorObjects.clearSelection();
								}
							}
						}
						else if (InputEx.GetKey(ControlsSettings.primary) && this.hasDragStart)
						{
							this.dragEndViewportPoint = InputEx.NormalizedMousePosition;
							this.dragEndScreenPoint = Input.mousePosition;
							if (this.isDragging || Mathf.Abs(this.dragEndScreenPoint.x - this.dragStartScreenPoint.x) > 50f || Mathf.Abs(this.dragEndScreenPoint.x - this.dragStartScreenPoint.x) > 50f)
							{
								Vector2 vector = this.dragStartViewportPoint;
								Vector2 vector2 = this.dragEndViewportPoint;
								if (vector2.x < vector.x)
								{
									float x = vector2.x;
									vector2.x = vector.x;
									vector.x = x;
								}
								if (vector2.y < vector.y)
								{
									float y = vector2.y;
									vector2.y = vector.y;
									vector.y = y;
								}
								DragStarted dragStarted = EditorObjects.onDragStarted;
								if (dragStarted != null)
								{
									dragStarted(vector, vector2);
								}
								if (!this.isDragging)
								{
									this.isDragging = true;
									EditorObjects.dragable.Clear();
									byte region_x = Editor.editor.area.region_x;
									byte region_y = Editor.editor.area.region_y;
									if (Regions.checkSafe((int)region_x, (int)region_y))
									{
										for (int l = (int)(region_x - 1); l <= (int)(region_x + 1); l++)
										{
											for (int m = (int)(region_y - 1); m <= (int)(region_y + 1); m++)
											{
												if (Regions.checkSafe((int)((byte)l), (int)((byte)m)) && LevelObjects.regions[l, m])
												{
													for (int n = 0; n < LevelObjects.objects[l, m].Count; n++)
													{
														LevelObject levelObject = LevelObjects.objects[l, m][n];
														if (!(levelObject.transform == null))
														{
															Vector3 vector3 = MainCamera.instance.WorldToViewportPoint(levelObject.transform.position);
															if (vector3.z >= 0f)
															{
																EditorObjects.dragable.Add(new EditorDrag(levelObject.transform, vector3));
															}
														}
													}
													for (int num = 0; num < LevelObjects.buildables[l, m].Count; num++)
													{
														LevelBuildableObject levelBuildableObject = LevelObjects.buildables[l, m][num];
														if (!(levelBuildableObject.transform == null))
														{
															Vector3 vector4 = MainCamera.instance.WorldToViewportPoint(levelBuildableObject.transform.position);
															if (vector4.z >= 0f)
															{
																EditorObjects.dragable.Add(new EditorDrag(levelBuildableObject.transform, vector4));
															}
														}
													}
												}
											}
										}
									}
								}
								if (!InputEx.GetKey(ControlsSettings.modify))
								{
									for (int num2 = 0; num2 < EditorObjects.selection.Count; num2++)
									{
										Vector3 vector5 = MainCamera.instance.WorldToViewportPoint(EditorObjects.selection[num2].transform.position);
										if (vector5.z < 0f)
										{
											EditorObjects.removeSelection(EditorObjects.selection[num2].transform);
										}
										else if (vector5.x < vector.x || vector5.y < vector.y || vector5.x > vector2.x || vector5.y > vector2.y)
										{
											EditorObjects.removeSelection(EditorObjects.selection[num2].transform);
										}
									}
								}
								for (int num3 = 0; num3 < EditorObjects.dragable.Count; num3++)
								{
									EditorDrag editorDrag = EditorObjects.dragable[num3];
									if (!(editorDrag.transform == null) && !EditorObjects.containsSelection(editorDrag.transform) && editorDrag.screen.x >= vector.x && editorDrag.screen.y >= vector.y && editorDrag.screen.x <= vector2.x && editorDrag.screen.y <= vector2.y)
									{
										EditorObjects.addSelection(editorDrag.transform);
									}
								}
							}
						}
						if (EditorObjects.selection.Count > 0)
						{
							if (InputEx.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.worldHit.transform != null)
							{
								EditorObjects.pointSelection();
								Vector3 vector6 = EditorInteract.worldHit.point;
								if (InputEx.GetKey(ControlsSettings.snap))
								{
									vector6 += EditorInteract.worldHit.normal * EditorObjects.snapTransform;
								}
								Quaternion pivotRotation = EditorObjects.handles.GetPivotRotation();
								EditorObjects.handles.ExternallyTransformPivot(vector6, pivotRotation, false);
								EditorObjects.applySelection();
							}
							if (InputEx.GetKeyDown(ControlsSettings.focus))
							{
								MainCamera.instance.transform.parent.position = EditorObjects.handles.GetPivotPosition() - 15f * MainCamera.instance.transform.forward;
							}
						}
						else if (EditorInteract.worldHit.transform != null)
						{
							if (EditorInteract.worldHit.transform.CompareTag("Large") || EditorInteract.worldHit.transform.CompareTag("Medium") || EditorInteract.worldHit.transform.CompareTag("Small") || EditorInteract.worldHit.transform.CompareTag("Barricade") || EditorInteract.worldHit.transform.CompareTag("Structure"))
							{
								ObjectAsset objectAsset2;
								ItemAsset itemAsset2;
								LevelObjects.getAssetEditor(EditorInteract.worldHit.transform, out objectAsset2, out itemAsset2);
								if (objectAsset2 != null)
								{
									EEditorMessage message = EEditorMessage.FOCUS;
									string objectName = objectAsset2.objectName;
									string text = "\n";
									AssetOrigin origin = objectAsset2.origin;
									EditorUI.hint(message, objectName + text + (((origin != null) ? origin.name : null) ?? "Unknown"));
								}
								else if (itemAsset2 != null)
								{
									EEditorMessage message2 = EEditorMessage.FOCUS;
									string itemName = itemAsset2.itemName;
									string text2 = "\n";
									AssetOrigin origin2 = itemAsset2.origin;
									EditorUI.hint(message2, itemName + text2 + (((origin2 != null) ? origin2.name : null) ?? "Unknown"));
								}
							}
							if (InputEx.GetKeyDown(ControlsSettings.tool_2))
							{
								Vector3 vector7 = EditorInteract.worldHit.point;
								if (InputEx.GetKey(ControlsSettings.snap))
								{
									vector7 += EditorInteract.worldHit.normal * EditorObjects.snapTransform;
								}
								Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
								EditorObjects.handles.SetPreferredPivot(vector7, rotation);
								if (EditorObjects.selectedObjectAsset != null || EditorObjects.selectedItemAsset != null)
								{
									LevelObjects.step++;
									Transform transform2 = LevelObjects.registerAddObject(vector7, rotation, Vector3.one, EditorObjects.selectedObjectAsset, EditorObjects.selectedItemAsset);
									if (transform2 != null)
									{
										EditorObjects.addSelection(transform2);
									}
								}
							}
						}
					}
				}
			}
			if (InputEx.GetKeyUp(ControlsSettings.primary))
			{
				this.hasDragStart = false;
				if (this.isDragging)
				{
					this.stopDragging();
				}
			}
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0006FE14 File Offset: 0x0006E014
		private void Start()
		{
			EditorObjects._isBuilding = false;
			EditorObjects.selection = new List<EditorSelection>();
			EditorObjects.handles = new TransformHandles();
			EditorObjects.handles.OnPreTransform += this.OnHandlePreTransform;
			EditorObjects.handles.OnTranslatedAndRotated += this.OnHandleTranslatedAndRotated;
			EditorObjects.handles.OnTransformed += this.OnHandleTransformed;
			EditorObjects.dragMode = EDragMode.TRANSFORM;
			EditorObjects.dragCoordinate = EDragCoordinate.GLOBAL;
			EditorObjects.dragable = new List<EditorDrag>();
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Objects.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Objects.dat", false, false, 1);
				EditorObjects.snapTransform = block.readSingle();
				EditorObjects.snapRotation = block.readSingle();
				return;
			}
			EditorObjects.snapTransform = 1f;
			EditorObjects.snapRotation = 15f;
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0006FEF8 File Offset: 0x0006E0F8
		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorObjects.SAVEDATA_VERSION);
			block.writeSingle(EditorObjects.snapTransform);
			block.writeSingle(EditorObjects.snapRotation);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Objects.dat", false, false, block);
		}

		// Token: 0x04000E8F RID: 3727
		public static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x04000E90 RID: 3728
		private static List<Decal> decals = new List<Decal>();

		// Token: 0x04000E91 RID: 3729
		public static DragStarted onDragStarted;

		// Token: 0x04000E92 RID: 3730
		public static DragStopped onDragStopped;

		// Token: 0x04000E93 RID: 3731
		public static float snapTransform;

		// Token: 0x04000E94 RID: 3732
		public static float snapRotation;

		// Token: 0x04000E95 RID: 3733
		private static bool _isBuilding;

		// Token: 0x04000E96 RID: 3734
		private Vector2 dragStartViewportPoint;

		// Token: 0x04000E97 RID: 3735
		private Vector2 dragStartScreenPoint;

		// Token: 0x04000E98 RID: 3736
		private Vector2 dragEndViewportPoint;

		// Token: 0x04000E99 RID: 3737
		private Vector2 dragEndScreenPoint;

		// Token: 0x04000E9A RID: 3738
		private bool hasDragStart;

		// Token: 0x04000E9B RID: 3739
		private bool isDragging;

		// Token: 0x04000E9C RID: 3740
		private bool isUsingHandle;

		// Token: 0x04000E9D RID: 3741
		public static ObjectAsset selectedObjectAsset;

		// Token: 0x04000E9E RID: 3742
		public static ItemAsset selectedItemAsset;

		// Token: 0x04000E9F RID: 3743
		private static List<EditorSelection> selection;

		// Token: 0x04000EA0 RID: 3744
		private static List<EditorCopy> copies = new List<EditorCopy>();

		// Token: 0x04000EA1 RID: 3745
		private static Vector3 copyPosition;

		// Token: 0x04000EA2 RID: 3746
		private static Quaternion copyRotation;

		// Token: 0x04000EA3 RID: 3747
		private static Vector3 copyScale;

		// Token: 0x04000EA4 RID: 3748
		private static bool hasCopyScale;

		// Token: 0x04000EA5 RID: 3749
		private static bool hasCopiedRotation;

		// Token: 0x04000EA6 RID: 3750
		private static TransformHandles handles;

		// Token: 0x04000EA7 RID: 3751
		private static EDragMode _dragMode;

		// Token: 0x04000EA8 RID: 3752
		private static bool wantsBoundsEditor;

		// Token: 0x04000EA9 RID: 3753
		private static EDragCoordinate _dragCoordinate;

		// Token: 0x04000EAA RID: 3754
		private static List<EditorDrag> dragable;
	}
}
