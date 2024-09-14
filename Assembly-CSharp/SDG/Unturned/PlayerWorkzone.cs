using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000663 RID: 1635
	public class PlayerWorkzone : PlayerCaller
	{
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06003690 RID: 13968 RVA: 0x000FF5B8 File Offset: 0x000FD7B8
		// (set) Token: 0x06003691 RID: 13969 RVA: 0x000FF5C0 File Offset: 0x000FD7C0
		public bool isBuilding
		{
			get
			{
				return this._isBuilding;
			}
			set
			{
				this._isBuilding = value;
				if (!this._isBuilding)
				{
					if (this.isUsingHandle)
					{
						this.CancelHandleUse();
					}
					this.clearSelection();
				}
				base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.Workzone, this._isBuilding);
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06003692 RID: 13970 RVA: 0x000FF5F7 File Offset: 0x000FD7F7
		// (set) Token: 0x06003693 RID: 13971 RVA: 0x000FF5FF File Offset: 0x000FD7FF
		public EDragMode dragMode
		{
			get
			{
				return this._dragMode;
			}
			set
			{
				this._dragMode = value;
				this.wantsBoundsEditor = false;
				this.UpdateHandlesPreferredPivot();
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003694 RID: 13972 RVA: 0x000FF615 File Offset: 0x000FD815
		// (set) Token: 0x06003695 RID: 13973 RVA: 0x000FF61D File Offset: 0x000FD81D
		public EDragCoordinate dragCoordinate
		{
			get
			{
				return this._dragCoordinate;
			}
			set
			{
				this._dragCoordinate = value;
				this.UpdateHandlesPreferredPivot();
			}
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000FF62C File Offset: 0x000FD82C
		public void SubmitTransformsToServer()
		{
			foreach (WorkzoneSelection workzoneSelection in this.selection)
			{
				if (!(workzoneSelection.transform == null))
				{
					Vector3 position = workzoneSelection.transform.position;
					Quaternion rotation = workzoneSelection.transform.rotation;
					if (workzoneSelection.transform.CompareTag("Barricade"))
					{
						BarricadeManager.transformBarricade(workzoneSelection.transform, position, rotation);
					}
					else if (workzoneSelection.transform.CompareTag("Structure"))
					{
						StructureManager.transformStructure(workzoneSelection.transform, position, rotation);
					}
				}
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000FF6E0 File Offset: 0x000FD8E0
		public void addSelection(Transform select)
		{
			HighlighterTool.highlight(select, Color.yellow);
			this.selection.Add(new WorkzoneSelection(select));
			this.UpdateHandlesPreferredPivot();
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000FF704 File Offset: 0x000FD904
		public void removeSelection(Transform select)
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (this.selection[i].transform == select)
				{
					HighlighterTool.unhighlight(select);
					this.selection.RemoveAt(i);
					break;
				}
			}
			this.UpdateHandlesPreferredPivot();
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x000FF75C File Offset: 0x000FD95C
		private void clearSelection()
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (this.selection[i].transform != null)
				{
					HighlighterTool.unhighlight(this.selection[i].transform);
				}
			}
			this.selection.Clear();
			this.UpdateHandlesPreferredPivot();
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000FF7C0 File Offset: 0x000FD9C0
		public bool containsSelection(Transform select)
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (this.selection[i].transform == select)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000FF7FF File Offset: 0x000FD9FF
		private void CancelHandleUse()
		{
			this.isUsingHandle = false;
			this.handles.MouseUp();
			this.SubmitTransformsToServer();
		}

		/// <summary>
		/// Set handles pivot point according to selection transform.
		/// Doesn't apply if handle is currently being dragged.
		/// </summary>
		// Token: 0x0600369C RID: 13980 RVA: 0x000FF81C File Offset: 0x000FDA1C
		private void UpdateHandlesPreferredPivot()
		{
			if (this.selection.Count == 0)
			{
				return;
			}
			if (this.dragCoordinate == EDragCoordinate.GLOBAL)
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.selection.Count; i++)
				{
					if (!(this.selection[i].transform == null))
					{
						vector += this.selection[i].transform.position;
					}
				}
				vector /= (float)this.selection.Count;
				this.handles.SetPreferredPivot(vector, Quaternion.identity);
				return;
			}
			for (int j = 0; j < this.selection.Count; j++)
			{
				if (!(this.selection[j].transform == null))
				{
					this.handles.SetPreferredPivot(this.selection[j].transform.position, this.selection[j].transform.rotation);
					return;
				}
			}
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x000FF920 File Offset: 0x000FDB20
		private void stopDragging()
		{
			this.dragStartViewportPoint = Vector2.zero;
			this.dragStartScreenPoint = Vector2.zero;
			this.dragEndViewportPoint = Vector2.zero;
			this.dragEndScreenPoint = Vector2.zero;
			this.isDragging = false;
			DragStopped dragStopped = this.onDragStopped;
			if (dragStopped == null)
			{
				return;
			}
			dragStopped();
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000FF970 File Offset: 0x000FDB70
		private IEnumerable<GameObject> EnumerateSelectedGameObjects()
		{
			foreach (WorkzoneSelection workzoneSelection in this.selection)
			{
				if (workzoneSelection.transform != null)
				{
					yield return workzoneSelection.transform.gameObject;
				}
			}
			List<WorkzoneSelection>.Enumerator enumerator = default(List<WorkzoneSelection>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000FF980 File Offset: 0x000FDB80
		private void Update()
		{
			if (!this.isBuilding)
			{
				this.hasDragStart = false;
				if (this.isUsingHandle)
				{
					this.CancelHandleUse();
				}
				if (this.isDragging)
				{
					this.stopDragging();
					this.clearSelection();
				}
				return;
			}
			this.ray = MainCamera.instance.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(this.ray, out this.worldHit, 256f, RayMasks.EDITOR_WORLD);
			Physics.Raycast(this.ray, out this.buildableHit, 256f, RayMasks.EDITOR_BUILDABLE);
			this.handles.snapPositionInterval = this.snapTransform;
			this.handles.snapRotationIntervalDegrees = this.snapRotation;
			if (this.dragMode == EDragMode.TRANSFORM)
			{
				if (this.wantsBoundsEditor)
				{
					this.handles.SetPreferredMode(TransformHandles.EMode.PositionBounds);
					this.handles.UpdateBoundsFromSelection(this.EnumerateSelectedGameObjects());
				}
				else
				{
					this.handles.SetPreferredMode(TransformHandles.EMode.Position);
				}
			}
			else
			{
				this.handles.SetPreferredMode(TransformHandles.EMode.Rotation);
			}
			if (this.selection.Count > 0)
			{
				this.handles.Render(this.ray);
			}
			bool flag = this.selection.Count > 0 && this.handles.Raycast(this.ray);
			if (Glazier.Get().ShouldGameProcessInput)
			{
				if (InputEx.GetKey(ControlsSettings.secondary))
				{
					if (this.isUsingHandle)
					{
						this.CancelHandleUse();
					}
					this.hasDragStart = false;
					if (this.isDragging)
					{
						this.stopDragging();
						this.clearSelection();
					}
					return;
				}
				if (this.isUsingHandle)
				{
					if (!InputEx.GetKey(ControlsSettings.primary))
					{
						this.SubmitTransformsToServer();
						this.isUsingHandle = false;
						this.handles.MouseUp();
					}
					else
					{
						this.handles.wantsToSnap = InputEx.GetKey(ControlsSettings.snap);
						this.handles.MouseMove(this.ray);
					}
				}
				if (InputEx.GetKeyDown(ControlsSettings.tool_0))
				{
					if (this.dragMode != EDragMode.TRANSFORM)
					{
						this.dragMode = EDragMode.TRANSFORM;
					}
					else
					{
						this.wantsBoundsEditor = !this.wantsBoundsEditor;
					}
				}
				if (InputEx.GetKeyDown(ControlsSettings.tool_1))
				{
					this.dragMode = EDragMode.ROTATE;
				}
				if (InputEx.GetKeyDown(KeyCode.B) && this.selection.Count > 0 && InputEx.GetKey(KeyCode.LeftControl))
				{
					this.copyPosition = this.handles.GetPivotPosition();
					this.copyRotation = this.handles.GetPivotRotation();
					this.hasCopiedRotation = (this.dragCoordinate == EDragCoordinate.LOCAL);
				}
				if (InputEx.GetKeyDown(KeyCode.N) && this.selection.Count > 0 && this.copyPosition != Vector3.zero && InputEx.GetKey(KeyCode.LeftControl))
				{
					if (this.selection.Count == 1)
					{
						this.selection[0].transform.position = this.copyPosition;
						if (this.hasCopiedRotation)
						{
							this.selection[0].transform.rotation = this.copyRotation;
						}
						this.UpdateHandlesPreferredPivot();
					}
					else
					{
						this.handles.ExternallyTransformPivot(this.copyPosition, this.copyRotation, this.hasCopiedRotation);
					}
					this.SubmitTransformsToServer();
				}
				if (!this.isUsingHandle)
				{
					if (InputEx.GetKeyDown(ControlsSettings.primary))
					{
						if (flag)
						{
							this.isUsingHandle = true;
							this.handles.MouseDown(this.ray);
						}
						else
						{
							Transform transform = this.buildableHit.transform;
							if (transform != null)
							{
								if (transform.CompareTag("Barricade"))
								{
									transform = DamageTool.getBarricadeRootTransform(transform);
								}
								else if (transform.CompareTag("Structure"))
								{
									transform = DamageTool.getStructureRootTransform(transform);
								}
								else
								{
									transform = null;
								}
							}
							if (transform != null)
							{
								if (InputEx.GetKey(ControlsSettings.modify))
								{
									if (this.containsSelection(transform))
									{
										this.removeSelection(transform);
									}
									else
									{
										this.addSelection(transform);
									}
								}
								else if (this.containsSelection(transform))
								{
									this.clearSelection();
								}
								else
								{
									this.clearSelection();
									this.addSelection(transform);
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
									this.clearSelection();
								}
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
							DragStarted dragStarted = this.onDragStarted;
							if (dragStarted != null)
							{
								dragStarted(vector, vector2);
							}
							if (!this.isDragging)
							{
								this.isDragging = true;
								this.dragable.Clear();
								byte region_x = Player.player.movement.region_x;
								byte region_y = Player.player.movement.region_y;
								if (Regions.checkSafe((int)region_x, (int)region_y))
								{
									foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.vehicleRegions)
									{
										foreach (BarricadeDrop barricadeDrop in vehicleBarricadeRegion.drops)
										{
											if (!(barricadeDrop.model == null))
											{
												Vector3 vector3 = MainCamera.instance.WorldToViewportPoint(barricadeDrop.model.position);
												if (vector3.z >= 0f)
												{
													this.dragable.Add(new EditorDrag(barricadeDrop.model, vector3));
												}
											}
										}
									}
									for (int i = (int)(region_x - 1); i <= (int)(region_x + 1); i++)
									{
										for (int j = (int)(region_y - 1); j <= (int)(region_y + 1); j++)
										{
											if (Regions.checkSafe((int)((byte)i), (int)((byte)j)))
											{
												for (int k = 0; k < BarricadeManager.regions[i, j].drops.Count; k++)
												{
													BarricadeDrop barricadeDrop2 = BarricadeManager.regions[i, j].drops[k];
													if (!(barricadeDrop2.model == null))
													{
														Vector3 vector4 = MainCamera.instance.WorldToViewportPoint(barricadeDrop2.model.position);
														if (vector4.z >= 0f)
														{
															this.dragable.Add(new EditorDrag(barricadeDrop2.model, vector4));
														}
													}
												}
												foreach (StructureDrop structureDrop in StructureManager.regions[i, j].drops)
												{
													Vector3 vector5 = MainCamera.instance.WorldToViewportPoint(structureDrop.model.position);
													if (vector5.z >= 0f)
													{
														this.dragable.Add(new EditorDrag(structureDrop.model, vector5));
													}
												}
											}
										}
									}
								}
							}
							if (!InputEx.GetKey(ControlsSettings.modify))
							{
								for (int l = 0; l < this.selection.Count; l++)
								{
									if (!(this.selection[l].transform == null))
									{
										Vector3 vector6 = MainCamera.instance.WorldToViewportPoint(this.selection[l].transform.position);
										if (vector6.z < 0f)
										{
											this.removeSelection(this.selection[l].transform);
										}
										else if (vector6.x < vector.x || vector6.y < vector.y || vector6.x > vector2.x || vector6.y > vector2.y)
										{
											this.removeSelection(this.selection[l].transform);
										}
									}
								}
							}
							for (int m = 0; m < this.dragable.Count; m++)
							{
								EditorDrag editorDrag = this.dragable[m];
								if (!(editorDrag.transform == null) && !this.containsSelection(editorDrag.transform) && editorDrag.screen.x >= vector.x && editorDrag.screen.y >= vector.y && editorDrag.screen.x <= vector2.x && editorDrag.screen.y <= vector2.y)
								{
									this.addSelection(editorDrag.transform);
								}
							}
						}
					}
					if (this.selection.Count > 0 && InputEx.GetKeyDown(ControlsSettings.tool_2) && this.worldHit.transform != null)
					{
						Vector3 vector7 = this.worldHit.point;
						if (InputEx.GetKey(ControlsSettings.snap))
						{
							vector7 += this.worldHit.normal * this.snapTransform;
						}
						Quaternion pivotRotation = this.handles.GetPivotRotation();
						this.handles.ExternallyTransformPivot(vector7, pivotRotation, false);
						this.SubmitTransformsToServer();
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

		// Token: 0x060036A0 RID: 13984 RVA: 0x00100374 File Offset: 0x000FE574
		internal void InitializePlayer()
		{
			this._isBuilding = false;
			this.selection = new List<WorkzoneSelection>();
			this.handles = new TransformHandles();
			this.handles.OnPreTransform += this.OnHandlePreTransform;
			this.handles.OnTranslatedAndRotated += this.OnHandleTranslatedAndRotated;
			this.dragMode = EDragMode.TRANSFORM;
			this.dragCoordinate = EDragCoordinate.GLOBAL;
			this.dragable = new List<EditorDrag>();
			this.snapTransform = 1f;
			this.snapRotation = 15f;
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x001003FC File Offset: 0x000FE5FC
		private void OnHandlePreTransform(Matrix4x4 worldToPivot)
		{
			foreach (WorkzoneSelection workzoneSelection in this.selection)
			{
				if (!(workzoneSelection.transform == null))
				{
					workzoneSelection.preTransformPosition = workzoneSelection.transform.position;
					workzoneSelection.preTransformRotation = workzoneSelection.transform.rotation;
				}
			}
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x00100478 File Offset: 0x000FE678
		private void OnHandleTranslatedAndRotated(Vector3 worldPositionDelta, Quaternion worldRotationDelta, Vector3 pivotPosition, bool modifyRotation)
		{
			foreach (WorkzoneSelection workzoneSelection in this.selection)
			{
				if (!(workzoneSelection.transform == null))
				{
					Vector3 vector = workzoneSelection.preTransformPosition - pivotPosition;
					if (!vector.IsNearlyZero(0.001f))
					{
						workzoneSelection.transform.position = pivotPosition + worldRotationDelta * vector + worldPositionDelta;
					}
					else
					{
						workzoneSelection.transform.position = workzoneSelection.preTransformPosition + worldPositionDelta;
					}
					if (modifyRotation)
					{
						workzoneSelection.transform.rotation = worldRotationDelta * workzoneSelection.preTransformRotation;
					}
				}
			}
			this.UpdateHandlesPreferredPivot();
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0010054C File Offset: 0x000FE74C
		[Obsolete("No longer necessary")]
		public void pointSelection()
		{
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x0010054E File Offset: 0x000FE74E
		[Obsolete("Renamed to SubmitTransformsToServer")]
		public void applySelection()
		{
			this.SubmitTransformsToServer();
		}

		// Token: 0x04001F88 RID: 8072
		public DragStarted onDragStarted;

		// Token: 0x04001F89 RID: 8073
		public DragStopped onDragStopped;

		// Token: 0x04001F8A RID: 8074
		public float snapTransform;

		// Token: 0x04001F8B RID: 8075
		public float snapRotation;

		// Token: 0x04001F8C RID: 8076
		private bool _isBuilding;

		// Token: 0x04001F8D RID: 8077
		private Ray ray;

		// Token: 0x04001F8E RID: 8078
		private RaycastHit worldHit;

		// Token: 0x04001F8F RID: 8079
		private RaycastHit buildableHit;

		// Token: 0x04001F90 RID: 8080
		private Vector2 dragStartViewportPoint;

		// Token: 0x04001F91 RID: 8081
		private Vector2 dragStartScreenPoint;

		// Token: 0x04001F92 RID: 8082
		private Vector2 dragEndViewportPoint;

		// Token: 0x04001F93 RID: 8083
		private Vector2 dragEndScreenPoint;

		// Token: 0x04001F94 RID: 8084
		private bool hasDragStart;

		// Token: 0x04001F95 RID: 8085
		private bool isDragging;

		// Token: 0x04001F96 RID: 8086
		private bool isUsingHandle;

		// Token: 0x04001F97 RID: 8087
		private List<WorkzoneSelection> selection;

		// Token: 0x04001F98 RID: 8088
		private Vector3 copyPosition;

		// Token: 0x04001F99 RID: 8089
		private Quaternion copyRotation;

		// Token: 0x04001F9A RID: 8090
		private bool hasCopiedRotation;

		// Token: 0x04001F9B RID: 8091
		private TransformHandles handles;

		// Token: 0x04001F9C RID: 8092
		private EDragMode _dragMode;

		// Token: 0x04001F9D RID: 8093
		private bool wantsBoundsEditor;

		// Token: 0x04001F9E RID: 8094
		private EDragCoordinate _dragCoordinate;

		// Token: 0x04001F9F RID: 8095
		private List<EditorDrag> dragable;
	}
}
