using System;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	/// <summary>
	/// Hold onto collider and gameobject separately because collider isn't necessarily attached to gameobject.
	/// </summary>
	// Token: 0x0200010E RID: 270
	public class DevkitSelection : IEquatable<DevkitSelection>
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001A2ED File Offset: 0x000184ED
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0001A30A File Offset: 0x0001850A
		public Transform transform
		{
			get
			{
				if (!(this.gameObject != null))
				{
					return null;
				}
				return this.gameObject.transform;
			}
			set
			{
				this.gameObject = ((value != null) ? value.gameObject : null);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001A324 File Offset: 0x00018524
		public bool isValid
		{
			get
			{
				return this.gameObject != null && this.collider != null;
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001A342 File Offset: 0x00018542
		public bool Equals(DevkitSelection other)
		{
			return other != null && this.gameObject == other.gameObject;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001A35C File Offset: 0x0001855C
		public override bool Equals(object obj)
		{
			DevkitSelection other = obj as DevkitSelection;
			return this.Equals(other);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001A377 File Offset: 0x00018577
		public override int GetHashCode()
		{
			if (this.gameObject == null)
			{
				return -1;
			}
			return this.gameObject.GetHashCode();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001A394 File Offset: 0x00018594
		public DevkitSelection(GameObject newGameObject, Collider newCollider)
		{
			this.gameObject = newGameObject;
			this.collider = newCollider;
		}

		// Token: 0x04000299 RID: 665
		public static DevkitSelection invalid = new DevkitSelection(null, null);

		// Token: 0x0400029A RID: 666
		public GameObject gameObject;

		// Token: 0x0400029B RID: 667
		public Collider collider;

		// Token: 0x0400029C RID: 668
		public Vector3 preTransformPosition;

		// Token: 0x0400029D RID: 669
		public Quaternion preTransformRotation;

		// Token: 0x0400029E RID: 670
		public Vector3 preTransformLocalScale;

		// Token: 0x0400029F RID: 671
		public Matrix4x4 localToWorld;

		// Token: 0x040002A0 RID: 672
		public Matrix4x4 relativeToPivot;
	}
}
