using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Merging the devkit, legacy, and housing transform handles into one place.
	/// </summary>
	// Token: 0x0200041D RID: 1053
	public class TransformHandles
	{
		/// <summary>
		/// Invoked when handle is clicked so that tool can save selection transform relative to pivot.
		/// This avoids floating point precision loss of applying delta for each Transformed event.
		/// </summary>
		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06001F30 RID: 7984 RVA: 0x000765DC File Offset: 0x000747DC
		// (remove) Token: 0x06001F31 RID: 7985 RVA: 0x00076614 File Offset: 0x00074814
		public event TransformHandles.PreTransformEventHandler OnPreTransform;

		/// <summary>
		/// Invoked when handle is dragged and value actually changes.
		/// </summary>
		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06001F32 RID: 7986 RVA: 0x0007664C File Offset: 0x0007484C
		// (remove) Token: 0x06001F33 RID: 7987 RVA: 0x00076684 File Offset: 0x00074884
		public event TransformHandles.TranslatedAndRotatedEventHandler OnTranslatedAndRotated;

		/// <summary>
		/// Invoked when handle is dragged and value actually changes.
		/// </summary>
		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06001F34 RID: 7988 RVA: 0x000766BC File Offset: 0x000748BC
		// (remove) Token: 0x06001F35 RID: 7989 RVA: 0x000766F4 File Offset: 0x000748F4
		public event TransformHandles.TransformedEventHandler OnTransformed;

		// Token: 0x06001F36 RID: 7990 RVA: 0x00076729 File Offset: 0x00074929
		public Vector3 GetPivotPosition()
		{
			return this.pivotPosition;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x00076731 File Offset: 0x00074931
		public Quaternion GetPivotRotation()
		{
			return this.pivotRotation;
		}

		/// <summary>
		/// Preferred mode only takes effect while not dragging.
		/// Bounds modes fall back to non-bounds modes if bounds are not set.
		/// </summary>
		// Token: 0x06001F38 RID: 7992 RVA: 0x00076739 File Offset: 0x00074939
		public void SetPreferredMode(TransformHandles.EMode preferredMode)
		{
			this.preferredMode = preferredMode;
			this.SyncMode();
		}

		/// <summary>
		/// Pivot only takes effect while not dragging. This is to help ensure
		/// the caller does not depend on the internal pivot values.
		/// </summary>
		// Token: 0x06001F39 RID: 7993 RVA: 0x00076748 File Offset: 0x00074948
		public void SetPreferredPivot(Vector3 position, Quaternion rotation)
		{
			this.preferredPivotPosition = position;
			this.preferredPivotRotation = rotation;
			this.SyncPivot();
		}

		/// <summary>
		/// Somewhat hacky, useful to make the "copy-paste transform" feature easier to implement.
		/// Invoke tranformed callback as if pivot were manually dragged to the new position and rotation.
		/// </summary>
		// Token: 0x06001F3A RID: 7994 RVA: 0x00076760 File Offset: 0x00074960
		public void ExternallyTransformPivot(Vector3 position, Quaternion rotation, bool modifyRotation)
		{
			if (this.dragComponent != TransformHandles.EComponent.NONE)
			{
				return;
			}
			Matrix4x4 inverse = Matrix4x4.TRS(this.pivotPosition, this.pivotRotation, Vector3.one).inverse;
			TransformHandles.PreTransformEventHandler onPreTransform = this.OnPreTransform;
			if (onPreTransform != null)
			{
				onPreTransform(inverse);
			}
			Vector3 worldPositionDelta = position - this.pivotPosition;
			Quaternion worldRotationDelta = rotation * Quaternion.Inverse(this.pivotRotation);
			TransformHandles.TranslatedAndRotatedEventHandler onTranslatedAndRotated = this.OnTranslatedAndRotated;
			if (onTranslatedAndRotated != null)
			{
				onTranslatedAndRotated(worldPositionDelta, worldRotationDelta, this.pivotPosition, modifyRotation);
			}
			this.SetPreferredPivot(position, rotation);
		}

		/// <summary>
		/// Called before raycasting into the regular physics scene to give transform tool priority.
		/// </summary>
		// Token: 0x06001F3B RID: 7995 RVA: 0x000767E8 File Offset: 0x000749E8
		public bool Raycast(Ray mouseRay)
		{
			this.hoverComponent = TransformHandles.EComponent.NONE;
			this.UpdateViewProperties();
			if (this.mode == TransformHandles.EMode.Position)
			{
				if (this.RaycastPositionPlane(mouseRay, this.pivotRotation * Vector3.up * this.viewAxisFlip.y, this.pivotRotation * Vector3.forward * this.viewAxisFlip.z, this.pivotRotation * Vector3.right * this.viewAxisFlip.x))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_PLANE_X;
				}
				else if (this.RaycastPositionPlane(mouseRay, this.pivotRotation * Vector3.right * this.viewAxisFlip.x, this.pivotRotation * Vector3.forward * this.viewAxisFlip.z, this.pivotRotation * Vector3.up * this.viewAxisFlip.y))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_PLANE_Y;
				}
				else if (this.RaycastPositionPlane(mouseRay, this.pivotRotation * Vector3.right * this.viewAxisFlip.x, this.pivotRotation * Vector3.up * this.viewAxisFlip.y, this.pivotRotation * Vector3.forward * this.viewAxisFlip.z))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_PLANE_Z;
				}
				else if (this.RaycastPositionAxis(mouseRay, this.pivotRotation * Vector3.right * this.viewAxisFlip.x))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_AXIS_X;
				}
				else if (this.RaycastPositionAxis(mouseRay, this.pivotRotation * Vector3.up * this.viewAxisFlip.y))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_AXIS_Y;
				}
				else if (this.RaycastPositionAxis(mouseRay, this.pivotRotation * Vector3.forward * this.viewAxisFlip.z))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_AXIS_Z;
				}
			}
			else if (this.mode == TransformHandles.EMode.Rotation)
			{
				float num = -1f;
				float num2;
				bool flag = this.RaycastRotationPlane(mouseRay, this.pivotRotation * Vector3.right, out num2);
				if (flag)
				{
					num = num2;
					this.hoverComponent = TransformHandles.EComponent.ROTATION_X;
				}
				if (this.RaycastRotationPlane(mouseRay, this.pivotRotation * Vector3.up, out num2) && (!flag || num2 < num))
				{
					flag = true;
					num = num2;
					this.hoverComponent = TransformHandles.EComponent.ROTATION_Y;
				}
				if (this.RaycastRotationPlane(mouseRay, this.pivotRotation * Vector3.forward, out num2) && (!flag || num2 < num))
				{
					this.hoverComponent = TransformHandles.EComponent.ROTATION_Z;
				}
			}
			else if (this.mode == TransformHandles.EMode.Scale)
			{
				if (this.RaycastSphere(mouseRay))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_UNIFORM;
				}
				else if (this.RaycastPositionAxis(mouseRay, this.pivotRotation * Vector3.right * this.viewAxisFlip.x))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_AXIS_X;
				}
				else if (this.RaycastPositionAxis(mouseRay, this.pivotRotation * Vector3.up * this.viewAxisFlip.y))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_AXIS_Y;
				}
				else if (this.RaycastPositionAxis(mouseRay, this.pivotRotation * Vector3.forward * this.viewAxisFlip.z))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_AXIS_Z;
				}
			}
			else if (this.mode == TransformHandles.EMode.PositionBounds)
			{
				Vector3 min = this.pivotBounds.min;
				Vector3 max = this.pivotBounds.max;
				if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * -Vector3.right, -min.x))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_BOUNDS_NEGATIVE_X;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * -Vector3.up, -min.y))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_BOUNDS_NEGATIVE_Y;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * -Vector3.forward, -min.z))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_BOUNDS_NEGATIVE_Z;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * Vector3.right, max.x))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_BOUNDS_POSITIVE_X;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * Vector3.up, max.y))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_BOUNDS_POSITIVE_Y;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * Vector3.forward, max.z))
				{
					this.hoverComponent = TransformHandles.EComponent.POSITION_BOUNDS_POSITIVE_Z;
				}
			}
			else if (this.mode == TransformHandles.EMode.ScaleBounds)
			{
				Vector3 min2 = this.pivotBounds.min;
				Vector3 max2 = this.pivotBounds.max;
				if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * -Vector3.right, -min2.x))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_BOUNDS_NEGATIVE_X;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * -Vector3.up, -min2.y))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_BOUNDS_NEGATIVE_Y;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * -Vector3.forward, -min2.z))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_BOUNDS_NEGATIVE_Z;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * Vector3.right, max2.x))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_BOUNDS_POSITIVE_X;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * Vector3.up, max2.y))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_BOUNDS_POSITIVE_Y;
				}
				else if (this.RaycastPositionBoundsAxis(mouseRay, this.pivotRotation * Vector3.forward, max2.z))
				{
					this.hoverComponent = TransformHandles.EComponent.SCALE_BOUNDS_POSITIVE_Z;
				}
			}
			return this.hoverComponent > TransformHandles.EComponent.NONE;
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x00076E28 File Offset: 0x00075028
		public void MouseDown(Ray mouseRay)
		{
			this.dragComponent = this.hoverComponent;
			this.dragPreviousPosition = this.pivotPosition;
			this.dragPreviousRotation = this.pivotRotation;
			this.dragPreviousAngle = 0f;
			this.dragPreviousScale = Vector3.one;
			if (this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_AXIS))
			{
				this.dragAxisOrigin = this.pivotPosition;
				if (this.dragComponent.HasFlag(TransformHandles.EComponent.X))
				{
					this.dragAxisDirection = this.pivotRotation * Vector3.right * this.viewAxisFlip.x;
				}
				else if (this.dragComponent.HasFlag(TransformHandles.EComponent.Y))
				{
					this.dragAxisDirection = this.pivotRotation * Vector3.up * this.viewAxisFlip.y;
				}
				else
				{
					this.dragAxisDirection = this.pivotRotation * Vector3.forward * this.viewAxisFlip.z;
				}
				this.dragAxisInitialDistance = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragAxisOrigin, this.dragAxisDirection);
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_PLANE))
			{
				this.dragPlaneOrigin = this.pivotPosition;
				if (this.dragComponent.HasFlag(TransformHandles.EComponent.X))
				{
					this.dragPlaneAxis0 = this.pivotRotation * Vector3.up * this.viewAxisFlip.y;
					this.dragPlaneAxis1 = this.pivotRotation * Vector3.forward * this.viewAxisFlip.z;
					this.dragPlaneNormal = this.pivotRotation * Vector3.right * this.viewAxisFlip.x;
				}
				else if (this.dragComponent.HasFlag(TransformHandles.EComponent.Y))
				{
					this.dragPlaneAxis0 = this.pivotRotation * Vector3.right * this.viewAxisFlip.x;
					this.dragPlaneAxis1 = this.pivotRotation * Vector3.forward * this.viewAxisFlip.z;
					this.dragPlaneNormal = this.pivotRotation * Vector3.up * this.viewAxisFlip.y;
				}
				else
				{
					this.dragPlaneAxis0 = this.pivotRotation * Vector3.right * this.viewAxisFlip.x;
					this.dragPlaneAxis1 = this.pivotRotation * Vector3.up * this.viewAxisFlip.y;
					this.dragPlaneNormal = this.pivotRotation * Vector3.forward * this.viewAxisFlip.z;
				}
				Plane plane = new Plane(this.dragPlaneNormal, this.dragPlaneOrigin);
				float d;
				if (plane.Raycast(mouseRay, out d))
				{
					Vector3 rhs = mouseRay.origin + mouseRay.direction * d - this.dragPlaneOrigin;
					this.dragPlaneInitialDistance0 = Vector3.Dot(this.dragPlaneAxis0, rhs);
					this.dragPlaneInitialDistance1 = Vector3.Dot(this.dragPlaneAxis1, rhs);
				}
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.ROTATION))
			{
				this.dragRotationOrigin = this.pivotRotation;
				if (this.dragComponent.HasFlag(TransformHandles.EComponent.X))
				{
					this.dragRotationAxis = this.pivotRotation * Vector3.right * this.viewAxisFlip.x;
				}
				else if (this.dragComponent.HasFlag(TransformHandles.EComponent.Y))
				{
					this.dragRotationAxis = this.pivotRotation * Vector3.up * this.viewAxisFlip.y;
				}
				else
				{
					this.dragRotationAxis = this.pivotRotation * Vector3.forward * this.viewAxisFlip.z;
				}
				Plane plane2 = new Plane(this.dragRotationAxis, this.pivotPosition);
				float d2;
				if (plane2.Raycast(mouseRay, out d2))
				{
					Vector3 a = mouseRay.origin + mouseRay.direction * d2;
					this.dragRotationOutwardDirection = (a - this.pivotPosition).normalized;
					this.dragRotationEdgePoint = a;
					this.dragRotationTangent = Vector3.Cross(this.dragRotationAxis, this.dragRotationOutwardDirection).normalized;
				}
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.SCALE))
			{
				this.dragScaleOrigin = this.pivotPosition;
				if (this.dragComponent == TransformHandles.EComponent.SCALE_UNIFORM)
				{
					Plane plane3 = new Plane(-this.cameraForward, this.dragScaleOrigin);
					float d3;
					if (plane3.Raycast(mouseRay, out d3))
					{
						Vector3 a2 = mouseRay.origin + mouseRay.direction * d3;
						this.dragScaleLocalDirection = Vector3.one;
						this.dragScaleWorldDirection = (a2 - this.dragScaleOrigin).normalized;
					}
				}
				else if (this.dragComponent.HasFlag(TransformHandles.EComponent.X))
				{
					this.dragScaleLocalDirection = Vector3.right;
					this.dragScaleWorldDirection = this.pivotRotation * this.dragScaleLocalDirection * this.viewAxisFlip.x;
				}
				else if (this.dragComponent.HasFlag(TransformHandles.EComponent.Y))
				{
					this.dragScaleLocalDirection = Vector3.up;
					this.dragScaleWorldDirection = this.pivotRotation * this.dragScaleLocalDirection * this.viewAxisFlip.y;
				}
				else
				{
					this.dragScaleLocalDirection = Vector3.forward;
					this.dragScaleWorldDirection = this.pivotRotation * this.dragScaleLocalDirection * this.viewAxisFlip.z;
				}
				this.dragScaleInitialDistance = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragScaleOrigin, this.dragScaleWorldDirection);
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_BOUNDS))
			{
				this.dragAxisOrigin = this.pivotPosition;
				if (this.dragComponent.HasFlag(TransformHandles.EComponent.X))
				{
					this.dragAxisDirection = this.pivotRotation * Vector3.right;
				}
				else if (this.dragComponent.HasFlag(TransformHandles.EComponent.Y))
				{
					this.dragAxisDirection = this.pivotRotation * Vector3.up;
				}
				else
				{
					this.dragAxisDirection = this.pivotRotation * Vector3.forward;
				}
				if (this.dragComponent.HasFlag(TransformHandles.EComponent.NEGATIVE))
				{
					this.dragAxisDirection *= -1f;
				}
				this.dragAxisInitialDistance = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragAxisOrigin, this.dragAxisDirection);
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.SCALE_BOUNDS))
			{
				this.dragScaleOrigin = this.pivotPosition;
				if (this.dragComponent.HasFlag(TransformHandles.EComponent.X))
				{
					this.dragScaleLocalDirection = Vector3.right;
					this.dragScaleWorldDirection = this.pivotRotation * this.dragScaleLocalDirection;
					this.dragScaleBounds = this.pivotBounds.size.x;
				}
				else if (this.dragComponent.HasFlag(TransformHandles.EComponent.Y))
				{
					this.dragScaleLocalDirection = Vector3.up;
					this.dragScaleWorldDirection = this.pivotRotation * this.dragScaleLocalDirection;
					this.dragScaleBounds = this.pivotBounds.size.y;
				}
				else
				{
					this.dragScaleLocalDirection = Vector3.forward;
					this.dragScaleWorldDirection = this.pivotRotation * this.dragScaleLocalDirection;
					this.dragScaleBounds = this.pivotBounds.size.z;
				}
				this.dragScaleBoundsCenter = this.pivotPosition + this.pivotRotation * this.pivotBounds.center;
				this.dragPreviousPosition = this.dragScaleBoundsCenter;
				this.dragScaleBoundsSize = this.pivotBounds.size;
				if (this.dragComponent.HasFlag(TransformHandles.EComponent.NEGATIVE))
				{
					this.dragScaleWorldDirection *= -1f;
				}
				this.dragScaleInitialDistance = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragScaleOrigin, this.dragScaleWorldDirection);
				this.dragAxisOrigin = this.dragScaleOrigin;
				this.dragAxisDirection = this.dragScaleWorldDirection;
				this.dragAxisInitialDistance = this.dragScaleInitialDistance;
			}
			Matrix4x4 inverse = Matrix4x4.TRS(this.dragPreviousPosition, this.dragPreviousRotation, this.dragPreviousScale).inverse;
			TransformHandles.PreTransformEventHandler onPreTransform = this.OnPreTransform;
			if (onPreTransform == null)
			{
				return;
			}
			onPreTransform(inverse);
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x00077770 File Offset: 0x00075970
		public void MouseMove(Ray mouseRay)
		{
			if (this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_AXIS) || this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_BOUNDS))
			{
				float num = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragAxisOrigin, this.dragAxisDirection) - this.dragAxisInitialDistance;
				if (this.wantsToSnap && this.snapPositionInterval > Mathf.Epsilon)
				{
					num = (float)Mathf.RoundToInt(num / this.snapPositionInterval) * this.snapPositionInterval;
				}
				Vector3 vector = this.dragAxisDirection * num;
				Vector3 a = this.dragAxisOrigin + vector;
				if ((a - this.dragPreviousPosition).magnitude > Mathf.Epsilon)
				{
					this.pivotPosition = a;
					TransformHandles.TranslatedAndRotatedEventHandler onTranslatedAndRotated = this.OnTranslatedAndRotated;
					if (onTranslatedAndRotated != null)
					{
						onTranslatedAndRotated(vector, Quaternion.identity, this.pivotPosition, false);
					}
					this.dragPreviousPosition = a;
					return;
				}
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_PLANE))
			{
				Plane plane = new Plane(this.dragPlaneNormal, this.dragPlaneOrigin);
				float d;
				if (plane.Raycast(mouseRay, out d))
				{
					Vector3 rhs = mouseRay.origin + mouseRay.direction * d - this.dragPlaneOrigin;
					float num2 = Vector3.Dot(this.dragPlaneAxis0, rhs) - this.dragPlaneInitialDistance0;
					float num3 = Vector3.Dot(this.dragPlaneAxis1, rhs) - this.dragPlaneInitialDistance1;
					if (this.wantsToSnap && this.snapPositionInterval > Mathf.Epsilon)
					{
						num2 = (float)Mathf.RoundToInt(num2 / this.snapPositionInterval) * this.snapPositionInterval;
						num3 = (float)Mathf.RoundToInt(num3 / this.snapPositionInterval) * this.snapPositionInterval;
					}
					Vector3 vector2 = this.dragPlaneAxis0 * num2 + this.dragPlaneAxis1 * num3;
					Vector3 a2 = this.dragPlaneOrigin + vector2;
					if ((a2 - this.dragPreviousPosition).magnitude > Mathf.Epsilon)
					{
						this.pivotPosition = a2;
						TransformHandles.TranslatedAndRotatedEventHandler onTranslatedAndRotated2 = this.OnTranslatedAndRotated;
						if (onTranslatedAndRotated2 != null)
						{
							onTranslatedAndRotated2(vector2, Quaternion.identity, this.pivotPosition, false);
						}
						this.dragPreviousPosition = a2;
						return;
					}
				}
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.ROTATION))
			{
				float num4 = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragRotationEdgePoint, this.dragRotationTangent) * 90f / this.viewScale;
				if (this.wantsToSnap && this.snapRotationIntervalDegrees > Mathf.Epsilon)
				{
					num4 = (float)Mathf.RoundToInt(num4 / this.snapRotationIntervalDegrees) * this.snapRotationIntervalDegrees;
				}
				if (Mathf.Abs(num4 - this.dragPreviousAngle) > Mathf.Epsilon)
				{
					Quaternion quaternion = Quaternion.AngleAxis(num4, this.dragRotationAxis);
					Quaternion quaternion2 = quaternion * this.dragRotationOrigin;
					this.pivotRotation = quaternion2;
					TransformHandles.TranslatedAndRotatedEventHandler onTranslatedAndRotated3 = this.OnTranslatedAndRotated;
					if (onTranslatedAndRotated3 != null)
					{
						onTranslatedAndRotated3(Vector3.zero, quaternion, this.pivotPosition, true);
					}
					this.dragPreviousAngle = num4;
					this.dragPreviousRotation = quaternion2;
					return;
				}
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.SCALE))
			{
				float num5 = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragScaleOrigin, this.dragScaleWorldDirection) - this.dragScaleInitialDistance;
				num5 /= this.viewScale;
				if (this.wantsToSnap && this.snapPositionInterval > Mathf.Epsilon)
				{
					num5 = (float)Mathf.RoundToInt(num5 / this.snapPositionInterval) * this.snapPositionInterval;
				}
				if (!MathfEx.IsNearlyEqual(num5, -1f, Mathf.Epsilon))
				{
					Vector3 vector3 = Vector3.one + this.dragScaleLocalDirection * num5;
					if ((vector3 - this.dragPreviousScale).magnitude > Mathf.Epsilon)
					{
						Matrix4x4 pivotToWorld = Matrix4x4.TRS(this.dragPreviousPosition, this.dragPreviousRotation, vector3);
						TransformHandles.TransformedEventHandler onTransformed = this.OnTransformed;
						if (onTransformed != null)
						{
							onTransformed(pivotToWorld);
						}
						this.dragPreviousScale = vector3;
						return;
					}
				}
			}
			else if (this.dragComponent.HasFlag(TransformHandles.EComponent.SCALE_BOUNDS))
			{
				float num6 = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragScaleOrigin, this.dragScaleWorldDirection) - this.dragScaleInitialDistance;
				if (this.wantsToSnap && this.snapPositionInterval > Mathf.Epsilon)
				{
					num6 = (float)Mathf.RoundToInt(num6 / this.snapPositionInterval) * this.snapPositionInterval;
				}
				Vector3 vector4 = this.dragScaleBoundsCenter + this.dragScaleWorldDirection * num6 * 0.5f;
				Vector3 vector5 = Vector3.one + this.dragScaleLocalDirection * (num6 / this.dragScaleBounds);
				if ((vector4 - this.dragPreviousPosition).magnitude > Mathf.Epsilon && (vector5 - this.dragPreviousScale).magnitude > Mathf.Epsilon)
				{
					this.pivotPosition = this.dragScaleOrigin + this.dragScaleWorldDirection * num6 * 0.5f;
					this.pivotBounds.size = this.dragScaleBoundsSize + this.dragScaleLocalDirection * num6;
					Matrix4x4 pivotToWorld2 = Matrix4x4.TRS(vector4, this.dragPreviousRotation, vector5);
					TransformHandles.TransformedEventHandler onTransformed2 = this.OnTransformed;
					if (onTransformed2 != null)
					{
						onTransformed2(pivotToWorld2);
					}
					this.dragPreviousPosition = vector4;
					this.dragPreviousScale = vector5;
				}
			}
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x00077D12 File Offset: 0x00075F12
		public void MouseUp()
		{
			this.dragComponent = TransformHandles.EComponent.NONE;
			this.wantsToSnap = false;
			this.SyncMode();
			this.SyncPivot();
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x00077D30 File Offset: 0x00075F30
		public void Render(Ray mouseRay)
		{
			this.UpdateViewProperties();
			if ((this.mode == TransformHandles.EMode.PositionBounds || this.mode == TransformHandles.EMode.ScaleBounds) && this.hasPivotBounds)
			{
				Color yellow = Color.yellow;
				yellow.a = 0.25f;
				RuntimeGizmos.Get().Box(Matrix4x4.TRS(this.pivotPosition, this.pivotRotation, Vector3.one), this.pivotBounds.center, this.pivotBounds.size, yellow, 0f, EGizmoLayer.Foreground);
			}
			if (this.mode == TransformHandles.EMode.Position)
			{
				if (this.wantsToSnap && this.snapPositionInterval > Mathf.Epsilon)
				{
					if (this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_AXIS))
					{
						this.DrawPositionAxisSnap(mouseRay);
					}
					else if (this.dragComponent.HasFlag(TransformHandles.EComponent.POSITION_PLANE))
					{
						this.DrawPositionPlaneSnap(mouseRay);
					}
				}
				this.DrawPositionPlane(this.pivotRotation * Vector3.up * this.viewAxisFlip.y, this.pivotRotation * Vector3.forward * this.viewAxisFlip.z, Color.red, TransformHandles.EComponent.POSITION_PLANE_X);
				this.DrawPositionPlane(this.pivotRotation * Vector3.right * this.viewAxisFlip.x, this.pivotRotation * Vector3.forward * this.viewAxisFlip.z, Color.green, TransformHandles.EComponent.POSITION_PLANE_Y);
				this.DrawPositionPlane(this.pivotRotation * Vector3.right * this.viewAxisFlip.x, this.pivotRotation * Vector3.up * this.viewAxisFlip.y, Color.blue, TransformHandles.EComponent.POSITION_PLANE_Z);
				this.DrawPositionAxis(this.pivotRotation * Vector3.right * this.viewAxisFlip.x, Color.red, TransformHandles.EComponent.POSITION_AXIS_X);
				this.DrawPositionAxis(this.pivotRotation * Vector3.up * this.viewAxisFlip.y, Color.green, TransformHandles.EComponent.POSITION_AXIS_Y);
				this.DrawPositionAxis(this.pivotRotation * Vector3.forward * this.viewAxisFlip.z, Color.blue, TransformHandles.EComponent.POSITION_AXIS_Z);
				return;
			}
			if (this.mode == TransformHandles.EMode.Rotation)
			{
				if (this.dragComponent == TransformHandles.EComponent.NONE)
				{
					this.DrawRotationCircle(this.pivotRotation * Vector3.up, this.pivotRotation * Vector3.forward, TransformHandles.EComponent.ROTATION_X, Color.red);
					this.DrawRotationCircle(this.pivotRotation * Vector3.right, this.pivotRotation * Vector3.forward, TransformHandles.EComponent.ROTATION_Y, Color.green);
					this.DrawRotationCircle(this.pivotRotation * Vector3.right, this.pivotRotation * Vector3.up, TransformHandles.EComponent.ROTATION_Z, Color.blue);
					return;
				}
				this.DrawDragCircle();
				return;
			}
			else
			{
				if (this.mode == TransformHandles.EMode.Scale)
				{
					Color color = (this.dragComponent == TransformHandles.EComponent.SCALE_UNIFORM) ? Color.white : ((this.hoverComponent == TransformHandles.EComponent.SCALE_UNIFORM) ? Color.yellow : Color.gray);
					RuntimeGizmos.Get().Circle(this.pivotPosition, this.viewRight, this.viewUp, 0.25f * this.viewScale, color, 0f, 16, EGizmoLayer.Foreground);
					this.DrawScaleAxis(this.pivotRotation * Vector3.right * this.viewAxisFlip.x, Color.red, TransformHandles.EComponent.SCALE_AXIS_X);
					this.DrawScaleAxis(this.pivotRotation * Vector3.up * this.viewAxisFlip.y, Color.green, TransformHandles.EComponent.SCALE_AXIS_Y);
					this.DrawScaleAxis(this.pivotRotation * Vector3.forward * this.viewAxisFlip.z, Color.blue, TransformHandles.EComponent.SCALE_AXIS_Z);
					return;
				}
				if (this.mode == TransformHandles.EMode.PositionBounds)
				{
					if (this.wantsToSnap && this.snapPositionInterval > Mathf.Epsilon)
					{
						this.DrawPositionAxisSnap(mouseRay);
					}
					Vector3 min = this.pivotBounds.min;
					Vector3 max = this.pivotBounds.max;
					this.DrawPositionBoundsAxis(this.pivotRotation * -Vector3.right, -min.x, Color.red, TransformHandles.EComponent.POSITION_BOUNDS_NEGATIVE_X);
					this.DrawPositionBoundsAxis(this.pivotRotation * -Vector3.up, -min.y, Color.green, TransformHandles.EComponent.POSITION_BOUNDS_NEGATIVE_Y);
					this.DrawPositionBoundsAxis(this.pivotRotation * -Vector3.forward, -min.z, Color.blue, TransformHandles.EComponent.POSITION_BOUNDS_NEGATIVE_Z);
					this.DrawPositionBoundsAxis(this.pivotRotation * Vector3.right, max.x, Color.red, TransformHandles.EComponent.POSITION_BOUNDS_POSITIVE_X);
					this.DrawPositionBoundsAxis(this.pivotRotation * Vector3.up, max.y, Color.green, TransformHandles.EComponent.POSITION_BOUNDS_POSITIVE_Y);
					this.DrawPositionBoundsAxis(this.pivotRotation * Vector3.forward, max.z, Color.blue, TransformHandles.EComponent.POSITION_BOUNDS_POSITIVE_Z);
					return;
				}
				if (this.mode == TransformHandles.EMode.ScaleBounds)
				{
					if (this.wantsToSnap && this.snapPositionInterval > Mathf.Epsilon)
					{
						this.DrawPositionAxisSnap(mouseRay);
					}
					Vector3 min2 = this.pivotBounds.min;
					Vector3 max2 = this.pivotBounds.max;
					this.DrawScaleBoundsAxis(this.pivotRotation * -Vector3.right, -min2.x, Color.red, TransformHandles.EComponent.SCALE_BOUNDS_NEGATIVE_X);
					this.DrawScaleBoundsAxis(this.pivotRotation * -Vector3.up, -min2.y, Color.green, TransformHandles.EComponent.SCALE_BOUNDS_NEGATIVE_Y);
					this.DrawScaleBoundsAxis(this.pivotRotation * -Vector3.forward, -min2.z, Color.blue, TransformHandles.EComponent.SCALE_BOUNDS_NEGATIVE_Z);
					this.DrawScaleBoundsAxis(this.pivotRotation * Vector3.right, max2.x, Color.red, TransformHandles.EComponent.SCALE_BOUNDS_POSITIVE_X);
					this.DrawScaleBoundsAxis(this.pivotRotation * Vector3.up, max2.y, Color.green, TransformHandles.EComponent.SCALE_BOUNDS_POSITIVE_Y);
					this.DrawScaleBoundsAxis(this.pivotRotation * Vector3.forward, max2.z, Color.blue, TransformHandles.EComponent.SCALE_BOUNDS_POSITIVE_Z);
				}
				return;
			}
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x00078384 File Offset: 0x00076584
		public void UpdateBoundsFromSelection(IEnumerable<GameObject> selection)
		{
			TransformHandles.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (this.dragComponent != TransformHandles.EComponent.NONE)
			{
				return;
			}
			this.pivotBounds = default(Bounds);
			this.hasPivotBounds = false;
			CS$<>8__locals1.worldToPivot = Matrix4x4.TRS(this.pivotPosition, this.pivotRotation, Vector3.one).inverse;
			foreach (GameObject gameObject in selection)
			{
				gameObject.GetComponentsInChildren<Component>(TransformHandles.workingComponentList);
				foreach (Component component in TransformHandles.workingComponentList)
				{
					MeshFilter meshFilter = component as MeshFilter;
					if (meshFilter != null)
					{
						if (meshFilter.sharedMesh != null)
						{
							Bounds bounds = meshFilter.sharedMesh.bounds;
							this.<UpdateBoundsFromSelection>g__EncapsuleBounds|27_0(component.transform, bounds.center, bounds.extents, ref CS$<>8__locals1);
							this.hasPivotBounds = true;
						}
					}
					else
					{
						BoxCollider boxCollider = component as BoxCollider;
						if (boxCollider != null)
						{
							this.<UpdateBoundsFromSelection>g__EncapsuleBounds|27_0(component.transform, boxCollider.center, boxCollider.size * 0.5f, ref CS$<>8__locals1);
							this.hasPivotBounds = true;
						}
						else
						{
							SphereCollider sphereCollider = component as SphereCollider;
							if (sphereCollider != null)
							{
								float radius = sphereCollider.radius;
								Vector3 extents = new Vector3(radius, radius, radius);
								this.<UpdateBoundsFromSelection>g__EncapsuleBounds|27_0(component.transform, sphereCollider.center, extents, ref CS$<>8__locals1);
								this.hasPivotBounds = true;
							}
						}
					}
				}
				TransformHandles.workingComponentList.Clear();
			}
			this.SyncMode();
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x00078558 File Offset: 0x00076758
		private bool RaycastPositionAxis(Ray mouseRay, Vector3 axisDirection)
		{
			float num = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.pivotPosition, axisDirection);
			float num2 = MathfEx.DistanceBetweenRays(mouseRay.origin, mouseRay.direction, this.pivotPosition, axisDirection);
			return num > 0f && num < this.viewScale && num2 < this.viewScale * 0.1f;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000785C0 File Offset: 0x000767C0
		private bool RaycastPositionPlane(Ray mouseRay, Vector3 axis0, Vector3 axis1, Vector3 planeNormal)
		{
			Plane plane = new Plane(planeNormal, this.pivotPosition);
			float d;
			if (!plane.Raycast(mouseRay, out d))
			{
				return false;
			}
			Vector3 rhs = mouseRay.origin + mouseRay.direction * d - this.pivotPosition;
			float num = Vector3.Dot(axis0, rhs);
			float num2 = Vector3.Dot(axis1, rhs);
			return num > 0f && num < this.viewScale * 0.25f && num2 > 0f && num2 < this.viewScale * 0.25f;
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x00078654 File Offset: 0x00076854
		private bool RaycastRotationPlane(Ray mouseRay, Vector3 planeNormal, out float hitDistance)
		{
			Plane plane = new Plane(planeNormal, this.pivotPosition);
			if (!plane.Raycast(mouseRay, out hitDistance))
			{
				return false;
			}
			Vector3 vector = mouseRay.origin + mouseRay.direction * hitDistance - this.pivotPosition;
			float num = MathfEx.Square(this.viewScale * 0.9f);
			float num2 = MathfEx.Square(this.viewScale * 1.1f);
			float sqrMagnitude = vector.sqrMagnitude;
			return sqrMagnitude > num && sqrMagnitude < num2;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000786DC File Offset: 0x000768DC
		private bool RaycastPositionBoundsAxis(Ray mouseRay, Vector3 axisDirection, float offset)
		{
			Vector3 origin = this.pivotPosition + axisDirection * offset;
			float num = MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, origin, axisDirection);
			float num2 = MathfEx.DistanceBetweenRays(mouseRay.origin, mouseRay.direction, origin, axisDirection);
			return num > 0f && num < this.viewScale && num2 < this.viewScale * 0.1f;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0007874C File Offset: 0x0007694C
		private bool RaycastSphere(Ray mouseRay)
		{
			Vector3 a = this.pivotPosition;
			float num = this.viewScale * 0.25f;
			float num2 = num * num;
			float num3 = Vector3.Dot(a - mouseRay.origin, mouseRay.direction);
			if (num3 < 0f)
			{
				return false;
			}
			Vector3 b = mouseRay.origin + mouseRay.direction * num3;
			return (a - b).sqrMagnitude <= num2;
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x000787C4 File Offset: 0x000769C4
		private void DrawPositionAxisSnap(Ray mouseRay)
		{
			Color color = new Color(0f, 0f, 0f, 0.5f);
			Vector3 b = Vector3.Cross(this.dragAxisDirection, this.viewDirection).normalized * 0.1f * this.viewScale;
			float d = (float)Mathf.RoundToInt((MathfEx.ProjectRayOntoRay(mouseRay.origin, mouseRay.direction, this.dragAxisOrigin, this.dragAxisDirection) - this.dragAxisInitialDistance) / this.snapPositionInterval) * this.snapPositionInterval + this.dragAxisInitialDistance;
			Vector3 a = this.dragAxisOrigin + this.dragAxisDirection * d;
			for (int i = -10; i <= 10; i++)
			{
				Vector3 a2 = a + this.dragAxisDirection * this.snapPositionInterval * (float)i;
				RuntimeGizmos.Get().Line(a2 - b, a2 + b, color, 0f, EGizmoLayer.Foreground);
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000788CC File Offset: 0x00076ACC
		private void DrawPositionPlaneSnap(Ray mouseRay)
		{
			Plane plane = new Plane(this.dragPlaneNormal, this.dragPlaneOrigin);
			float d;
			if (!plane.Raycast(mouseRay, out d))
			{
				return;
			}
			Vector3 rhs = mouseRay.origin + mouseRay.direction * d - this.dragPlaneOrigin;
			float d2 = (float)Mathf.RoundToInt((Vector3.Dot(this.dragPlaneAxis0, rhs) - this.dragPlaneInitialDistance0) / this.snapPositionInterval) * this.snapPositionInterval + this.dragPlaneInitialDistance0;
			float d3 = (float)Mathf.RoundToInt((Vector3.Dot(this.dragPlaneAxis1, rhs) - this.dragPlaneInitialDistance1) / this.snapPositionInterval) * this.snapPositionInterval + this.dragPlaneInitialDistance1;
			Vector3 a = this.dragPlaneOrigin + this.dragPlaneAxis0 * d2 + this.dragPlaneAxis1 * d3;
			Color color = new Color(0f, 0f, 0f, 0.5f);
			Vector3 b = this.dragPlaneAxis0 * this.snapPositionInterval * 10f;
			Vector3 b2 = this.dragPlaneAxis1 * this.snapPositionInterval * 10f;
			for (int i = -10; i <= 10; i++)
			{
				Vector3 a2 = a + this.dragPlaneAxis0 * this.snapPositionInterval * (float)i;
				RuntimeGizmos.Get().Line(a2 - b2, a2 + b2, color, 0f, EGizmoLayer.Foreground);
			}
			for (int j = -10; j <= 10; j++)
			{
				Vector3 a3 = a + this.dragPlaneAxis1 * this.snapPositionInterval * (float)j;
				RuntimeGizmos.Get().Line(a3 - b, a3 + b, color, 0f, EGizmoLayer.Foreground);
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x00078AB0 File Offset: 0x00076CB0
		private void DrawPositionAxis(Vector3 direction, Color color, TransformHandles.EComponent component)
		{
			Color color2 = (this.dragComponent == component) ? Color.white : ((this.hoverComponent == component) ? Color.yellow : color);
			RuntimeGizmos.Get().Arrow(this.pivotPosition, direction, this.viewScale, color2, 0f, EGizmoLayer.Foreground);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x00078B00 File Offset: 0x00076D00
		private void DrawPositionPlane(Vector3 axis0, Vector3 axis1, Color color, TransformHandles.EComponent component)
		{
			Color color2 = (this.dragComponent == component) ? Color.white : ((this.hoverComponent == component) ? Color.yellow : color);
			Vector3 b = axis0 * 0.25f * this.viewScale;
			Vector3 b2 = axis1 * 0.25f * this.viewScale;
			Vector3 end = this.pivotPosition + b + b2;
			RuntimeGizmos.Get().Line(this.pivotPosition + b, end, color2, 0f, EGizmoLayer.Foreground);
			RuntimeGizmos.Get().Line(this.pivotPosition + b2, end, color2, 0f, EGizmoLayer.Foreground);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x00078BB0 File Offset: 0x00076DB0
		private void DrawRotationCircle(Vector3 axis0, Vector3 axis1, TransformHandles.EComponent component, Color color)
		{
			Color color2 = (this.hoverComponent == component) ? Color.yellow : color;
			RuntimeGizmos.Get().Circle(this.pivotPosition, axis0, axis1, this.viewScale, color2, 0f, 32, EGizmoLayer.Foreground);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x00078BF4 File Offset: 0x00076DF4
		private void DrawDragCircle()
		{
			if (this.wantsToSnap)
			{
				Color color = new Color(0f, 0f, 0f, 0.5f);
				float num = 0.017453292f * this.dragPreviousAngle;
				float num2 = 0.017453292f * this.snapRotationIntervalDegrees;
				int num3 = Mathf.Max(1, Mathf.CeilToInt(1.5707964f / num2));
				for (int i = -num3; i <= num3; i++)
				{
					if (i != 0)
					{
						float f = num + (float)i * num2;
						float d = Mathf.Cos(f);
						float d2 = Mathf.Sin(f);
						Vector3 a = this.dragRotationOutwardDirection * d + this.dragRotationTangent * d2;
						Vector3 begin = this.pivotPosition + a * this.viewScale * 0.9f;
						Vector3 end = this.pivotPosition + a * this.viewScale * 1.1f;
						RuntimeGizmos.Get().Line(begin, end, color, 0f, EGizmoLayer.Foreground);
					}
				}
			}
			Color white = Color.white;
			RuntimeGizmos.Get().Circle(this.pivotPosition, this.dragRotationOutwardDirection, this.dragRotationTangent, this.viewScale, white, 0f, 32, EGizmoLayer.Foreground);
			float f2 = 0.017453292f * this.dragPreviousAngle;
			float d3 = Mathf.Cos(f2);
			float d4 = Mathf.Sin(f2);
			Vector3 a2 = this.dragRotationOutwardDirection * d3 + this.dragRotationTangent * d4;
			RuntimeGizmos.Get().Line(this.pivotPosition, this.pivotPosition + a2 * this.viewScale * 1.1f, white, 0f, EGizmoLayer.Foreground);
			white.a = 0.5f;
			Vector3 vector = this.pivotPosition + this.dragRotationOutwardDirection * this.viewScale;
			Vector3 b = this.dragRotationTangent * 0.5f * this.viewScale;
			RuntimeGizmos.Get().Line(this.pivotPosition, vector, white, 0f, EGizmoLayer.Foreground);
			RuntimeGizmos.Get().Line(vector, vector - b, white, 0f, EGizmoLayer.Foreground);
			RuntimeGizmos.Get().Line(vector, vector + b, white, 0f, EGizmoLayer.Foreground);
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x00078E48 File Offset: 0x00077048
		private void DrawScaleAxis(Vector3 direction, Color color, TransformHandles.EComponent component)
		{
			Vector3 b = Vector3.Cross(direction, this.viewDirection).normalized * 0.1f * this.viewScale;
			Color color2 = (this.dragComponent == component) ? Color.white : ((this.hoverComponent == component) ? Color.yellow : color);
			Vector3 vector = this.pivotPosition + direction * this.viewScale;
			RuntimeGizmos.Get().Line(this.pivotPosition, vector, color2, 0f, EGizmoLayer.Foreground);
			RuntimeGizmos.Get().Line(vector - b, vector + b, color2, 0f, EGizmoLayer.Foreground);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x00078EF0 File Offset: 0x000770F0
		private void DrawPositionBoundsAxis(Vector3 direction, float offset, Color color, TransformHandles.EComponent component)
		{
			Vector3 b = Vector3.Cross(direction, this.viewDirection).normalized * 0.1f * this.viewScale;
			Color color2 = (this.dragComponent == component) ? Color.white : ((this.hoverComponent == component) ? Color.yellow : color);
			Vector3 vector = this.pivotPosition + direction * offset;
			Vector3 end = vector + direction * this.viewScale;
			Vector3 a = vector + direction * 0.75f * this.viewScale;
			RuntimeGizmos.Get().Line(vector, end, color2, 0f, EGizmoLayer.Foreground);
			RuntimeGizmos.Get().Line(a - b, end, color2, 0f, EGizmoLayer.Foreground);
			RuntimeGizmos.Get().Line(a + b, end, color2, 0f, EGizmoLayer.Foreground);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x00078FD8 File Offset: 0x000771D8
		private void DrawScaleBoundsAxis(Vector3 direction, float offset, Color color, TransformHandles.EComponent component)
		{
			Vector3 b = Vector3.Cross(direction, this.viewDirection).normalized * 0.1f * this.viewScale;
			Color color2 = (this.dragComponent == component) ? Color.white : ((this.hoverComponent == component) ? Color.yellow : color);
			Vector3 vector = this.pivotPosition + direction * offset;
			Vector3 vector2 = vector + direction * this.viewScale;
			RuntimeGizmos.Get().Line(vector, vector2, color2, 0f, EGizmoLayer.Foreground);
			RuntimeGizmos.Get().Line(vector2 - b, vector2 + b, color2, 0f, EGizmoLayer.Foreground);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0007908C File Offset: 0x0007728C
		private void SyncMode()
		{
			if (this.dragComponent != TransformHandles.EComponent.NONE)
			{
				return;
			}
			if (this.preferredMode == TransformHandles.EMode.PositionBounds && !this.hasPivotBounds)
			{
				this.mode = TransformHandles.EMode.Position;
				return;
			}
			if (this.preferredMode == TransformHandles.EMode.ScaleBounds && !this.hasPivotBounds)
			{
				this.mode = TransformHandles.EMode.Scale;
				return;
			}
			this.mode = this.preferredMode;
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000790E0 File Offset: 0x000772E0
		private void SyncPivot()
		{
			if (this.dragComponent != TransformHandles.EComponent.NONE)
			{
				return;
			}
			this.pivotPosition = this.preferredPivotPosition;
			this.pivotRotation = this.preferredPivotRotation;
		}

		/// <summary>
		/// Update properties that depend on the transform of the camera relative to our handles.
		/// </summary>
		// Token: 0x06001F51 RID: 8017 RVA: 0x00079104 File Offset: 0x00077304
		private void UpdateViewProperties()
		{
			if (MainCamera.instance == null)
			{
				this.viewDirection = Vector3.forward;
				this.viewScale = 1f;
				this.viewAxisFlip = Vector3.one;
				this.cameraForward = Vector3.forward;
				return;
			}
			this.cameraForward = MainCamera.instance.transform.forward;
			Vector3 position = MainCamera.instance.transform.position;
			Vector3 a = this.pivotPosition - position;
			float magnitude = a.magnitude;
			if (magnitude < 0.001f)
			{
				this.viewDirection = Vector3.forward;
				this.viewScale = 1f;
				this.viewAxisFlip = Vector3.one;
				return;
			}
			this.viewDirection = a / magnitude;
			this.viewRight = Vector3.Cross(this.viewDirection, Vector3.up).normalized;
			this.viewUp = Vector3.Cross(this.viewDirection, this.viewRight).normalized;
			this.viewScale = magnitude * 0.5f;
			this.viewAxisFlip.x = (float)((Vector3.Dot(this.viewDirection, this.pivotRotation * Vector3.right) < 0f) ? 1 : -1);
			this.viewAxisFlip.y = (float)((Vector3.Dot(this.viewDirection, this.pivotRotation * Vector3.up) < 0f) ? 1 : -1);
			this.viewAxisFlip.z = (float)((Vector3.Dot(this.viewDirection, this.pivotRotation * Vector3.forward) < 0f) ? 1 : -1);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000792E8 File Offset: 0x000774E8
		[CompilerGenerated]
		private void <UpdateBoundsFromSelection>g__EncapsuleBounds|27_0(Transform transform, Vector3 center, Vector3 extents, ref TransformHandles.<>c__DisplayClass27_0 A_4)
		{
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(-extents.x, -extents.y, -extents.z))));
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(-extents.x, -extents.y, extents.z))));
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(-extents.x, extents.y, -extents.z))));
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(-extents.x, extents.y, extents.z))));
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(extents.x, -extents.y, -extents.z))));
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(extents.x, -extents.y, extents.z))));
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(extents.x, extents.y, -extents.z))));
			this.pivotBounds.Encapsulate(A_4.worldToPivot.MultiplyPoint3x4(transform.TransformPoint(center + new Vector3(extents.x, extents.y, extents.z))));
		}

		// Token: 0x04000F70 RID: 3952
		public bool wantsToSnap;

		// Token: 0x04000F71 RID: 3953
		public float snapPositionInterval = 1f;

		// Token: 0x04000F72 RID: 3954
		public float snapRotationIntervalDegrees = 15f;

		// Token: 0x04000F73 RID: 3955
		private static List<Component> workingComponentList = new List<Component>();

		// Token: 0x04000F74 RID: 3956
		private TransformHandles.EMode preferredMode;

		// Token: 0x04000F75 RID: 3957
		private TransformHandles.EMode mode;

		/// <summary>
		/// Center of handle.
		/// </summary>
		// Token: 0x04000F76 RID: 3958
		private Vector3 pivotPosition;

		/// <summary>
		/// Rotation of handle.
		/// </summary>
		// Token: 0x04000F77 RID: 3959
		private Quaternion pivotRotation = Quaternion.identity;

		// Token: 0x04000F78 RID: 3960
		private Vector3 preferredPivotPosition;

		// Token: 0x04000F79 RID: 3961
		private Quaternion preferredPivotRotation;

		// Token: 0x04000F7A RID: 3962
		private Bounds pivotBounds;

		/// <summary>
		/// True if pivotBounds is non-zero.
		/// </summary>
		// Token: 0x04000F7B RID: 3963
		private bool hasPivotBounds;

		/// <summary>
		/// Mouse currently over this handle.
		/// </summary>
		// Token: 0x04000F7C RID: 3964
		private TransformHandles.EComponent hoverComponent;

		/// <summary>
		/// Mouse currently dragging this handle.
		/// </summary>
		// Token: 0x04000F7D RID: 3965
		private TransformHandles.EComponent dragComponent;

		/// <summary>
		/// Direction from camera toward pivot.
		/// </summary>
		// Token: 0x04000F7E RID: 3966
		private Vector3 viewDirection;

		// Token: 0x04000F7F RID: 3967
		private Vector3 viewRight;

		// Token: 0x04000F80 RID: 3968
		private Vector3 viewUp;

		// Token: 0x04000F81 RID: 3969
		private Vector3 cameraForward;

		/// <summary>
		/// Multiplier according to distance between camera and pivot to keep handles a constant on-screen size.
		/// </summary>
		// Token: 0x04000F82 RID: 3970
		private float viewScale = 1f;

		/// <summary>
		/// Multiplier to flip axis handles according to which side the camera is on.
		/// </summary>
		// Token: 0x04000F83 RID: 3971
		private Vector3 viewAxisFlip = Vector3.one;

		// Token: 0x04000F84 RID: 3972
		private Vector3 dragPreviousPosition;

		// Token: 0x04000F85 RID: 3973
		private Quaternion dragPreviousRotation;

		// Token: 0x04000F86 RID: 3974
		private float dragPreviousAngle;

		// Token: 0x04000F87 RID: 3975
		private Vector3 dragPreviousScale;

		// Token: 0x04000F88 RID: 3976
		private Vector3 dragAxisOrigin;

		// Token: 0x04000F89 RID: 3977
		private Vector3 dragAxisDirection;

		// Token: 0x04000F8A RID: 3978
		private float dragAxisInitialDistance;

		// Token: 0x04000F8B RID: 3979
		private Vector3 dragPlaneOrigin;

		// Token: 0x04000F8C RID: 3980
		private Vector3 dragPlaneAxis0;

		// Token: 0x04000F8D RID: 3981
		private Vector3 dragPlaneAxis1;

		// Token: 0x04000F8E RID: 3982
		private Vector3 dragPlaneNormal;

		// Token: 0x04000F8F RID: 3983
		private float dragPlaneInitialDistance0;

		// Token: 0x04000F90 RID: 3984
		private float dragPlaneInitialDistance1;

		/// <summary>
		/// Pivot rotation when rotation drag started.
		/// </summary>
		// Token: 0x04000F91 RID: 3985
		private Quaternion dragRotationOrigin;

		/// <summary>
		/// Rotating around this axis.
		/// </summary>
		// Token: 0x04000F92 RID: 3986
		private Vector3 dragRotationAxis;

		/// <summary>
		/// Direction from circle center to edge point.
		/// </summary>
		// Token: 0x04000F93 RID: 3987
		private Vector3 dragRotationOutwardDirection;

		/// <summary>
		/// Point on the edge of the circle.
		/// </summary>
		// Token: 0x04000F94 RID: 3988
		private Vector3 dragRotationEdgePoint;

		/// <summary>
		/// Drag along this tangent to the circle.
		/// </summary>
		// Token: 0x04000F95 RID: 3989
		private Vector3 dragRotationTangent;

		// Token: 0x04000F96 RID: 3990
		private Vector3 dragScaleOrigin;

		// Token: 0x04000F97 RID: 3991
		private Vector3 dragScaleLocalDirection;

		// Token: 0x04000F98 RID: 3992
		private Vector3 dragScaleWorldDirection;

		// Token: 0x04000F99 RID: 3993
		private float dragScaleInitialDistance;

		// Token: 0x04000F9A RID: 3994
		private Vector3 dragScaleBoundsCenter;

		// Token: 0x04000F9B RID: 3995
		private Vector3 dragScaleBoundsSize;

		// Token: 0x04000F9C RID: 3996
		private float dragScaleBounds;

		// Token: 0x0200093D RID: 2365
		public enum EMode
		{
			/// <summary>
			/// Position and plane handles for each axis.
			/// </summary>
			// Token: 0x040032CB RID: 13003
			Position,
			/// <summary>
			/// Disc handles for each axis.
			/// </summary>
			// Token: 0x040032CC RID: 13004
			Rotation,
			/// <summary>
			/// Scale handles for each axis.
			/// </summary>
			// Token: 0x040032CD RID: 13005
			Scale,
			/// <summary>
			/// Position handles on each side of box.
			/// </summary>
			// Token: 0x040032CE RID: 13006
			PositionBounds,
			/// <summary>
			/// Scale handles on each side of box which both move and resize the box.
			/// </summary>
			// Token: 0x040032CF RID: 13007
			ScaleBounds
		}

		// Token: 0x0200093E RID: 2366
		// (Invoke) Token: 0x06004AB0 RID: 19120
		public delegate void PreTransformEventHandler(Matrix4x4 worldToPivot);

		// Token: 0x0200093F RID: 2367
		// (Invoke) Token: 0x06004AB4 RID: 19124
		public delegate void TranslatedAndRotatedEventHandler(Vector3 worldPositionDelta, Quaternion worldRotationDelta, Vector3 pivotPosition, bool modifyRotation);

		// Token: 0x02000940 RID: 2368
		// (Invoke) Token: 0x06004AB8 RID: 19128
		public delegate void TransformedEventHandler(Matrix4x4 pivotToWorld);

		// Token: 0x02000941 RID: 2369
		[Flags]
		private enum EComponent
		{
			// Token: 0x040032D1 RID: 13009
			NONE = 0,
			// Token: 0x040032D2 RID: 13010
			X = 1,
			// Token: 0x040032D3 RID: 13011
			Y = 2,
			// Token: 0x040032D4 RID: 13012
			Z = 4,
			// Token: 0x040032D5 RID: 13013
			POSITION_AXIS = 8,
			// Token: 0x040032D6 RID: 13014
			POSITION_PLANE = 16,
			// Token: 0x040032D7 RID: 13015
			ROTATION = 32,
			// Token: 0x040032D8 RID: 13016
			SCALE = 64,
			// Token: 0x040032D9 RID: 13017
			POSITION_BOUNDS = 128,
			// Token: 0x040032DA RID: 13018
			NEGATIVE = 256,
			// Token: 0x040032DB RID: 13019
			POSITIVE = 512,
			// Token: 0x040032DC RID: 13020
			SCALE_BOUNDS = 1024,
			// Token: 0x040032DD RID: 13021
			POSITION_AXIS_X = 9,
			// Token: 0x040032DE RID: 13022
			POSITION_AXIS_Y = 10,
			// Token: 0x040032DF RID: 13023
			POSITION_AXIS_Z = 12,
			// Token: 0x040032E0 RID: 13024
			POSITION_PLANE_X = 17,
			// Token: 0x040032E1 RID: 13025
			POSITION_PLANE_Y = 18,
			// Token: 0x040032E2 RID: 13026
			POSITION_PLANE_Z = 20,
			// Token: 0x040032E3 RID: 13027
			ROTATION_X = 33,
			// Token: 0x040032E4 RID: 13028
			ROTATION_Y = 34,
			// Token: 0x040032E5 RID: 13029
			ROTATION_Z = 36,
			// Token: 0x040032E6 RID: 13030
			SCALE_AXIS_X = 65,
			// Token: 0x040032E7 RID: 13031
			SCALE_AXIS_Y = 66,
			// Token: 0x040032E8 RID: 13032
			SCALE_AXIS_Z = 68,
			// Token: 0x040032E9 RID: 13033
			SCALE_UNIFORM = 71,
			// Token: 0x040032EA RID: 13034
			POSITION_BOUNDS_NEGATIVE_X = 385,
			// Token: 0x040032EB RID: 13035
			POSITION_BOUNDS_POSITIVE_X = 641,
			// Token: 0x040032EC RID: 13036
			POSITION_BOUNDS_NEGATIVE_Y = 386,
			// Token: 0x040032ED RID: 13037
			POSITION_BOUNDS_POSITIVE_Y = 642,
			// Token: 0x040032EE RID: 13038
			POSITION_BOUNDS_NEGATIVE_Z = 388,
			// Token: 0x040032EF RID: 13039
			POSITION_BOUNDS_POSITIVE_Z = 644,
			// Token: 0x040032F0 RID: 13040
			SCALE_BOUNDS_NEGATIVE_X = 1281,
			// Token: 0x040032F1 RID: 13041
			SCALE_BOUNDS_POSITIVE_X = 1537,
			// Token: 0x040032F2 RID: 13042
			SCALE_BOUNDS_NEGATIVE_Y = 1282,
			// Token: 0x040032F3 RID: 13043
			SCALE_BOUNDS_POSITIVE_Y = 1538,
			// Token: 0x040032F4 RID: 13044
			SCALE_BOUNDS_NEGATIVE_Z = 1284,
			// Token: 0x040032F5 RID: 13045
			SCALE_BOUNDS_POSITIVE_Z = 1540
		}
	}
}
