using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Rendering;
using SDG.Framework.Utilities;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200041A RID: 1050
	public class SelectionTool : IDevkitTool
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x000727DE File Offset: 0x000709DE
		// (set) Token: 0x06001ED3 RID: 7891 RVA: 0x000727E6 File Offset: 0x000709E6
		public SelectionTool.ESelectionMode mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				this._mode = value;
				this.wantsBoundsEditor = false;
			}
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000727F8 File Offset: 0x000709F8
		protected void transformSelection()
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					IDevkitSelectionTransformableHandler component = devkitSelection.gameObject.GetComponent<IDevkitSelectionTransformableHandler>();
					if (component != null)
					{
						component.transformSelection();
					}
				}
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x00072868 File Offset: 0x00070A68
		private void OnHandlePreTransform(Matrix4x4 worldToPivot)
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					devkitSelection.preTransformPosition = devkitSelection.transform.position;
					devkitSelection.preTransformRotation = devkitSelection.transform.rotation;
					devkitSelection.preTransformLocalScale = devkitSelection.transform.localScale;
					devkitSelection.localToWorld = devkitSelection.transform.localToWorldMatrix;
					devkitSelection.relativeToPivot = worldToPivot * devkitSelection.localToWorld;
				}
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x00072918 File Offset: 0x00070B18
		private void OnHandleTranslatedAndRotated(Vector3 worldPositionDelta, Quaternion worldRotationDelta, Vector3 pivotPosition, bool modifyRotation)
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					Vector3 vector2;
					if (modifyRotation)
					{
						Vector3 vector = devkitSelection.preTransformPosition - pivotPosition;
						if (!vector.IsNearlyZero(0.001f))
						{
							vector2 = pivotPosition + worldRotationDelta * vector + worldPositionDelta;
						}
						else
						{
							vector2 = devkitSelection.preTransformPosition + worldPositionDelta;
						}
					}
					else
					{
						vector2 = devkitSelection.preTransformPosition + worldPositionDelta;
					}
					Quaternion quaternion = worldRotationDelta * devkitSelection.preTransformRotation;
					ITransformedHandler component = devkitSelection.gameObject.GetComponent<ITransformedHandler>();
					if (component != null)
					{
						component.OnTransformed(devkitSelection.preTransformPosition, devkitSelection.preTransformRotation, Vector3.zero, vector2, quaternion, Vector3.zero, modifyRotation, false);
					}
					else
					{
						if (!vector2.IsNearlyEqual(devkitSelection.transform.position, 0.001f))
						{
							devkitSelection.transform.position = vector2;
						}
						if (modifyRotation)
						{
							devkitSelection.transform.rotation = quaternion;
						}
					}
				}
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x00072A44 File Offset: 0x00070C44
		private void OnHandleTransformed(Matrix4x4 pivotToWorld)
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					Matrix4x4 matrix = pivotToWorld * devkitSelection.relativeToPivot;
					ITransformedHandler component = devkitSelection.gameObject.GetComponent<ITransformedHandler>();
					if (component != null)
					{
						component.OnTransformed(devkitSelection.preTransformPosition, devkitSelection.preTransformRotation, devkitSelection.preTransformLocalScale, matrix.GetPosition(), matrix.GetRotation(), matrix.lossyScale, true, true);
					}
					else
					{
						devkitSelection.transform.position = matrix.GetPosition();
						devkitSelection.transform.SetRotation_RoundIfNearlyAxisAligned(matrix.GetRotation(), 0.05f);
						devkitSelection.transform.SetLocalScale_RoundIfNearlyEqualToOne(matrix.lossyScale, 0.001f);
					}
				}
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x00072B34 File Offset: 0x00070D34
		protected void moveHandle(Vector3 position, Quaternion rotation, Vector3 scale, bool doRotation, bool hasScale)
		{
			DevkitTransactionManager.beginTransaction("Transform");
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					DevkitTransactionUtility.recordObjectDelta(devkitSelection.transform);
				}
			}
			if (DevkitSelectionManager.selection.Count == 1)
			{
				DevkitSelection devkitSelection2 = EnumerableEx.EnumerateFirst<DevkitSelection>(DevkitSelectionManager.selection);
				if (devkitSelection2 != null && devkitSelection2.transform != null)
				{
					ITransformedHandler component = devkitSelection2.gameObject.GetComponent<ITransformedHandler>();
					if (component != null)
					{
						component.OnTransformed(devkitSelection2.preTransformPosition, devkitSelection2.preTransformRotation, devkitSelection2.preTransformLocalScale, position, rotation, scale, doRotation, hasScale);
					}
					else
					{
						devkitSelection2.transform.position = position;
						if (doRotation)
						{
							devkitSelection2.transform.rotation = rotation;
						}
						if (hasScale)
						{
							devkitSelection2.transform.localScale = scale;
						}
					}
				}
			}
			else
			{
				this.handles.ExternallyTransformPivot(position, rotation, doRotation);
			}
			this.transformSelection();
			DevkitTransactionManager.endTransaction();
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x00072C44 File Offset: 0x00070E44
		protected virtual bool RaycastSelectableObjects(Ray ray, out RaycastHit hitInfo)
		{
			hitInfo = default(RaycastHit);
			return false;
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00072C4E File Offset: 0x00070E4E
		protected virtual void RequestInstantiation(Vector3 position)
		{
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x00072C50 File Offset: 0x00070E50
		protected virtual bool HasBoxSelectableObjects()
		{
			return false;
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00072C53 File Offset: 0x00070E53
		protected virtual IEnumerable<GameObject> EnumerateBoxSelectableObjects()
		{
			return null;
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x00072C56 File Offset: 0x00070E56
		private IEnumerable<GameObject> EnumerateSelectedGameObjects()
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (devkitSelection.gameObject != null)
				{
					yield return devkitSelection.gameObject;
				}
			}
			HashSet<DevkitSelection>.Enumerator enumerator = default(HashSet<DevkitSelection>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x00072C60 File Offset: 0x00070E60
		public virtual void update()
		{
			if (this.copySelectionDelay.Count > 0)
			{
				DevkitSelectionManager.clear();
				foreach (GameObject newGameObject in this.copySelectionDelay)
				{
					DevkitSelectionManager.add(new DevkitSelection(newGameObject, null));
				}
				this.copySelectionDelay.Clear();
			}
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (InputEx.GetKeyDown(KeyCode.Q))
				{
					if (this.mode != SelectionTool.ESelectionMode.POSITION)
					{
						this.mode = SelectionTool.ESelectionMode.POSITION;
					}
					else
					{
						this.wantsBoundsEditor = !this.wantsBoundsEditor;
					}
				}
				if (InputEx.GetKeyDown(KeyCode.W))
				{
					this.mode = SelectionTool.ESelectionMode.ROTATION;
				}
				if (InputEx.GetKeyDown(KeyCode.R))
				{
					if (this.mode != SelectionTool.ESelectionMode.SCALE)
					{
						this.mode = SelectionTool.ESelectionMode.SCALE;
					}
					else
					{
						this.wantsBoundsEditor = !this.wantsBoundsEditor;
					}
				}
				Ray ray = EditorInteract.ray;
				bool flag = DevkitSelectionManager.selection.Count > 0 && this.handles.Raycast(ray);
				if (DevkitSelectionManager.selection.Count > 0)
				{
					this.handles.Render(ray);
				}
				if (InputEx.GetKeyDown(KeyCode.Mouse0))
				{
					RaycastHit raycastHit = default(RaycastHit);
					if (!flag)
					{
						this.RaycastSelectableObjects(ray, out raycastHit);
						if (raycastHit.transform != null)
						{
							IDevkitHierarchyItem componentInParent = raycastHit.transform.GetComponentInParent<IDevkitHierarchyItem>();
							if (componentInParent != null && !componentInParent.CanBeSelected)
							{
								raycastHit = default(RaycastHit);
							}
						}
					}
					this.pendingClickSelection = new DevkitSelection((raycastHit.transform != null) ? raycastHit.transform.gameObject : null, raycastHit.collider);
					if (this.pendingClickSelection.isValid)
					{
						DevkitSelectionManager.data.point = raycastHit.point;
					}
					this.isDragging = flag;
					if (this.isDragging)
					{
						this.handles.MouseDown(ray);
						DevkitTransactionManager.beginTransaction("Transform");
						using (HashSet<DevkitSelection>.Enumerator enumerator2 = DevkitSelectionManager.selection.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								DevkitSelection devkitSelection = enumerator2.Current;
								DevkitTransactionUtility.recordObjectDelta(devkitSelection.transform);
							}
							goto IL_230;
						}
					}
					this.beginAreaSelect = MainCamera.instance.ScreenToViewportPoint(Input.mousePosition);
					this.beginAreaSelectTime = Time.time;
				}
				IL_230:
				if (InputEx.GetKey(KeyCode.Mouse0) && !this.isDragging && this.HasBoxSelectableObjects() && !this.isAreaSelecting && Time.time - this.beginAreaSelectTime > 0.1f)
				{
					this.isAreaSelecting = true;
					this.areaSelection.Clear();
					if (!InputEx.GetKey(KeyCode.LeftShift) && !InputEx.GetKey(KeyCode.LeftControl))
					{
						DevkitSelectionManager.clear();
					}
				}
				if (this.isDragging)
				{
					TransformHandles transformHandles = this.handles;
					DevkitSelectionToolOptions instance = DevkitSelectionToolOptions.instance;
					transformHandles.snapPositionInterval = ((instance != null) ? instance.snapPosition : 1f);
					TransformHandles transformHandles2 = this.handles;
					DevkitSelectionToolOptions instance2 = DevkitSelectionToolOptions.instance;
					transformHandles2.snapRotationIntervalDegrees = ((instance2 != null) ? instance2.snapRotation : 1f);
					this.handles.wantsToSnap = InputEx.GetKey(ControlsSettings.snap);
					this.handles.MouseMove(ray);
				}
				else if (InputEx.GetKeyDown(KeyCode.E))
				{
					RaycastHit raycastHit2;
					Physics.Raycast(ray, out raycastHit2, 8192f, (int)DevkitSelectionToolOptions.instance.selectionMask);
					if (raycastHit2.transform != null)
					{
						if (DevkitSelectionManager.selection.Count > 0)
						{
							this.moveHandle(raycastHit2.point, Quaternion.identity, Vector3.one, false, false);
						}
						else
						{
							this.RequestInstantiation(raycastHit2.point);
						}
					}
				}
				if (this.isAreaSelecting && this.HasBoxSelectableObjects())
				{
					Vector3 vector = MainCamera.instance.ScreenToViewportPoint(Input.mousePosition);
					Vector2 vector2;
					Vector2 vector3;
					if (vector.x < this.beginAreaSelect.x)
					{
						vector2.x = vector.x;
						vector3.x = this.beginAreaSelect.x;
					}
					else
					{
						vector2.x = this.beginAreaSelect.x;
						vector3.x = vector.x;
					}
					if (vector.y < this.beginAreaSelect.y)
					{
						vector2.y = vector.y;
						vector3.y = this.beginAreaSelect.y;
					}
					else
					{
						vector2.y = this.beginAreaSelect.y;
						vector3.y = vector.y;
					}
					foreach (GameObject gameObject in this.EnumerateBoxSelectableObjects())
					{
						if (!(gameObject == null))
						{
							Vector3 vector4 = MainCamera.instance.WorldToViewportPoint(gameObject.transform.position);
							DevkitSelection devkitSelection2 = new DevkitSelection(gameObject, null);
							if (vector4.z > 0f && vector4.x > vector2.x && vector4.x < vector3.x && vector4.y > vector2.y && vector4.y < vector3.y)
							{
								if (!this.areaSelection.Contains(devkitSelection2))
								{
									this.areaSelection.Add(devkitSelection2);
									DevkitSelectionManager.add(devkitSelection2);
								}
							}
							else if (this.areaSelection.Contains(devkitSelection2))
							{
								this.areaSelection.Remove(devkitSelection2);
								DevkitSelectionManager.remove(devkitSelection2);
							}
						}
					}
				}
				if (InputEx.GetKeyUp(KeyCode.Mouse0))
				{
					if (this.isDragging)
					{
						this.handles.MouseUp();
						this.pendingClickSelection = DevkitSelection.invalid;
						this.isDragging = false;
						this.transformSelection();
						DevkitTransactionManager.endTransaction();
					}
					else if (this.isAreaSelecting)
					{
						this.isAreaSelecting = false;
					}
					else
					{
						DevkitSelectionManager.select(this.pendingClickSelection);
					}
				}
			}
			if (DevkitSelectionManager.selection.Count > 0)
			{
				if (this.mode == SelectionTool.ESelectionMode.POSITION)
				{
					this.handles.SetPreferredMode(this.wantsBoundsEditor ? TransformHandles.EMode.PositionBounds : TransformHandles.EMode.Position);
				}
				else if (this.mode == SelectionTool.ESelectionMode.SCALE)
				{
					this.handles.SetPreferredMode(this.wantsBoundsEditor ? TransformHandles.EMode.ScaleBounds : TransformHandles.EMode.Scale);
				}
				else
				{
					this.handles.SetPreferredMode(TransformHandles.EMode.Rotation);
				}
				bool flag2 = this.mode != SelectionTool.ESelectionMode.SCALE && !this.wantsBoundsEditor && !DevkitSelectionToolOptions.instance.localSpace;
				this.handlePosition = Vector3.zero;
				this.handleRotation = Quaternion.identity;
				bool flag3 = flag2;
				foreach (DevkitSelection devkitSelection3 in DevkitSelectionManager.selection)
				{
					if (!(devkitSelection3.gameObject == null))
					{
						this.handlePosition += devkitSelection3.transform.position;
						if (!flag3)
						{
							this.handleRotation = devkitSelection3.transform.rotation;
							flag3 = true;
						}
					}
				}
				this.handlePosition /= (float)DevkitSelectionManager.selection.Count;
				this.handles.SetPreferredPivot(this.handlePosition, this.handleRotation);
				if (this.wantsBoundsEditor)
				{
					this.handles.UpdateBoundsFromSelection(this.EnumerateSelectedGameObjects());
				}
				if (InputEx.GetKeyDown(KeyCode.C))
				{
					this.copyBuffer.Clear();
					foreach (DevkitSelection devkitSelection4 in DevkitSelectionManager.selection)
					{
						this.copyBuffer.Add(devkitSelection4.gameObject);
					}
				}
				if (InputEx.GetKeyDown(KeyCode.V))
				{
					DevkitTransactionManager.beginTransaction("Paste");
					foreach (GameObject gameObject2 in this.copyBuffer)
					{
						IDevkitSelectionCopyableHandler component = gameObject2.GetComponent<IDevkitSelectionCopyableHandler>();
						GameObject gameObject3;
						if (component != null)
						{
							gameObject3 = component.copySelection();
						}
						else
						{
							gameObject3 = Object.Instantiate<GameObject>(gameObject2);
						}
						IDevkitHierarchyItem component2 = gameObject3.GetComponent<IDevkitHierarchyItem>();
						if (component2 != null)
						{
							component2.instanceID = LevelHierarchy.generateUniqueInstanceID();
						}
						DevkitTransactionUtility.recordInstantiation(gameObject3);
						this.copySelectionDelay.Add(gameObject3);
					}
					DevkitTransactionManager.endTransaction();
				}
				if (InputEx.GetKeyDown(KeyCode.Delete))
				{
					DevkitTransactionManager.beginTransaction("Delete");
					foreach (DevkitSelection devkitSelection5 in DevkitSelectionManager.selection)
					{
						DevkitTransactionUtility.recordDestruction(devkitSelection5.gameObject);
					}
					DevkitSelectionManager.clear();
					DevkitTransactionManager.endTransaction();
				}
				if (InputEx.GetKeyDown(KeyCode.B))
				{
					this.referencePosition = this.handlePosition;
					this.referenceRotation = this.handleRotation;
					this.hasReferenceRotation = !flag2;
					this.referenceScale = Vector3.one;
					this.hasReferenceScale = false;
					if (DevkitSelectionManager.selection.Count == 1)
					{
						foreach (DevkitSelection devkitSelection6 in DevkitSelectionManager.selection)
						{
							if (!(devkitSelection6.gameObject == null))
							{
								this.referenceScale = devkitSelection6.transform.localScale;
								this.hasReferenceScale = true;
							}
						}
					}
				}
				if (InputEx.GetKeyDown(KeyCode.N))
				{
					this.moveHandle(this.referencePosition, this.referenceRotation, this.referenceScale, this.hasReferenceRotation, this.hasReferenceScale);
				}
			}
			if (InputEx.GetKeyDown(ControlsSettings.focus))
			{
				if (DevkitSelectionManager.selection.Count > 0)
				{
					MainCamera.instance.transform.parent.position = this.handlePosition - 15f * MainCamera.instance.transform.forward;
					return;
				}
				MainCamera.instance.transform.parent.position = MainCamera.instance.transform.forward * -512f;
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x00073658 File Offset: 0x00071858
		public virtual void equip()
		{
			GLRenderer.render += this.handleGLRender;
			this.mode = SelectionTool.ESelectionMode.POSITION;
			this.handles = new TransformHandles();
			this.handles.OnPreTransform += this.OnHandlePreTransform;
			this.handles.OnTranslatedAndRotated += this.OnHandleTranslatedAndRotated;
			this.handles.OnTransformed += this.OnHandleTransformed;
			DevkitSelectionManager.clear();
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000736D2 File Offset: 0x000718D2
		public virtual void dequip()
		{
			GLRenderer.render -= this.handleGLRender;
			DevkitSelectionManager.clear();
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000736EC File Offset: 0x000718EC
		protected void handleGLRender()
		{
			if (!this.isAreaSelecting)
			{
				return;
			}
			GLUtility.LINE_FLAT_COLOR.SetPass(0);
			GL.Begin(1);
			GL.Color(Color.yellow);
			GLUtility.matrix = MathUtility.IDENTITY_MATRIX;
			Vector3 vector = this.beginAreaSelect;
			vector.z = 16f;
			Vector3 vector2 = MainCamera.instance.ScreenToViewportPoint(Input.mousePosition);
			vector2.z = 16f;
			Vector3 position = vector;
			position.x = vector2.x;
			Vector3 position2 = vector2;
			position2.x = vector.x;
			Vector3 v = MainCamera.instance.ViewportToWorldPoint(vector);
			Vector3 v2 = MainCamera.instance.ViewportToWorldPoint(position);
			Vector3 v3 = MainCamera.instance.ViewportToWorldPoint(position2);
			Vector3 v4 = MainCamera.instance.ViewportToWorldPoint(vector2);
			GL.Vertex(v);
			GL.Vertex(v2);
			GL.Vertex(v2);
			GL.Vertex(v4);
			GL.Vertex(v4);
			GL.Vertex(v3);
			GL.Vertex(v3);
			GL.Vertex(v);
			GL.End();
		}

		// Token: 0x04000F37 RID: 3895
		protected List<GameObject> copyBuffer = new List<GameObject>();

		// Token: 0x04000F38 RID: 3896
		protected List<GameObject> copySelectionDelay = new List<GameObject>();

		// Token: 0x04000F39 RID: 3897
		private SelectionTool.ESelectionMode _mode;

		// Token: 0x04000F3A RID: 3898
		private bool wantsBoundsEditor;

		// Token: 0x04000F3B RID: 3899
		protected DevkitSelection pendingClickSelection;

		// Token: 0x04000F3C RID: 3900
		protected Vector3 handlePosition;

		// Token: 0x04000F3D RID: 3901
		protected Quaternion handleRotation;

		// Token: 0x04000F3E RID: 3902
		protected Vector3 referencePosition;

		// Token: 0x04000F3F RID: 3903
		protected Quaternion referenceRotation;

		// Token: 0x04000F40 RID: 3904
		protected Vector3 referenceScale;

		// Token: 0x04000F41 RID: 3905
		protected bool hasReferenceRotation;

		// Token: 0x04000F42 RID: 3906
		protected bool hasReferenceScale;

		// Token: 0x04000F43 RID: 3907
		private TransformHandles handles;

		// Token: 0x04000F44 RID: 3908
		protected Vector3 beginAreaSelect;

		// Token: 0x04000F45 RID: 3909
		protected float beginAreaSelectTime;

		// Token: 0x04000F46 RID: 3910
		protected bool isAreaSelecting;

		// Token: 0x04000F47 RID: 3911
		protected bool isDragging;

		// Token: 0x04000F48 RID: 3912
		protected HashSet<DevkitSelection> areaSelection = new HashSet<DevkitSelection>();

		// Token: 0x02000936 RID: 2358
		public enum ESelectionMode
		{
			// Token: 0x040032B5 RID: 12981
			POSITION,
			// Token: 0x040032B6 RID: 12982
			ROTATION,
			// Token: 0x040032B7 RID: 12983
			SCALE
		}
	}
}
