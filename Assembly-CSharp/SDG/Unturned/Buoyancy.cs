using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000444 RID: 1092
	public class Buoyancy : MonoBehaviour
	{
		// Token: 0x060020D4 RID: 8404 RVA: 0x0007ECE0 File Offset: 0x0007CEE0
		private void FixedUpdate()
		{
			for (int i = 0; i < this.voxels.Count; i++)
			{
				Vector3 vector = base.transform.TransformPoint(this.voxels[i]);
				bool flag;
				float num;
				if (this.overrideSurfaceElevation < 0f)
				{
					WaterUtility.getUnderwaterInfo(vector, out flag, out num);
				}
				else
				{
					flag = (vector.y < this.overrideSurfaceElevation);
					num = this.overrideSurfaceElevation;
				}
				if (flag && vector.y - this.voxelHalfHeight < num)
				{
					Vector3 force = -this.rootRigidbody.GetPointVelocity(vector) * Buoyancy.DAMPER * this.rootRigidbody.mass + Mathf.Sqrt(Mathf.Clamp01((num - vector.y) / (2f * this.voxelHalfHeight) + 0.5f)) * this.localArchimedesForce;
					this.rootRigidbody.AddForceAtPosition(force, vector);
				}
			}
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x0007EDD8 File Offset: 0x0007CFD8
		private void Start()
		{
			this.rootRigidbody = base.gameObject.GetComponentInParent<Rigidbody>();
			this.volumeCollider = base.GetComponent<Collider>();
			BoxCollider boxCollider = this.volumeCollider as BoxCollider;
			if (!boxCollider)
			{
				UnturnedLog.warn("Unknown volume collider for buoyancy simulation: {0}", new object[]
				{
					this.volumeCollider
				});
				return;
			}
			Vector3 size = boxCollider.size;
			Vector3 vector = size / -2f;
			Vector3 vector2 = size / (float)this.slicesPerAxis;
			this.voxelHalfHeight = Mathf.Min(vector2.x, Mathf.Min(vector2.y, vector2.z)) / 2f;
			this.voxels = new List<Vector3>(this.slicesPerAxis * this.slicesPerAxis * this.slicesPerAxis);
			for (int i = 0; i < this.slicesPerAxis; i++)
			{
				for (int j = 0; j < this.slicesPerAxis; j++)
				{
					for (int k = 0; k < this.slicesPerAxis; k++)
					{
						float x = vector.x + vector2.x * (0.5f + (float)i);
						float y = vector.y + vector2.y * (0.5f + (float)j);
						float z = vector.z + vector2.z * (0.5f + (float)k);
						Vector3 vector3 = new Vector3(x, y, z);
						this.voxels.Add(vector3);
					}
				}
			}
			if (this.voxels.Count == 0)
			{
				this.voxels.Add(boxCollider.center);
			}
			float num = this.rootRigidbody.mass / this.density;
			float y2 = Buoyancy.WATER_DENSITY * Mathf.Abs(Physics.gravity.y) * num;
			this.localArchimedesForce = new Vector3(0f, y2, 0f) / (float)this.voxels.Count;
		}

		// Token: 0x04001014 RID: 4116
		private static readonly float DAMPER = 0.1f;

		// Token: 0x04001015 RID: 4117
		private static readonly float WATER_DENSITY = 1000f;

		// Token: 0x04001016 RID: 4118
		public float density = 500f;

		// Token: 0x04001017 RID: 4119
		public int slicesPerAxis = 2;

		// Token: 0x04001018 RID: 4120
		private float voxelHalfHeight;

		// Token: 0x04001019 RID: 4121
		private Vector3 localArchimedesForce;

		// Token: 0x0400101A RID: 4122
		private List<Vector3> voxels;

		// Token: 0x0400101B RID: 4123
		private Rigidbody rootRigidbody;

		// Token: 0x0400101C RID: 4124
		private Collider volumeCollider;

		// Token: 0x0400101D RID: 4125
		public float overrideSurfaceElevation = -1f;
	}
}
