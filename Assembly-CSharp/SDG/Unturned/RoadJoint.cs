using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000508 RID: 1288
	public class RoadJoint
	{
		// Token: 0x0600285C RID: 10332 RVA: 0x000AA75E File Offset: 0x000A895E
		public Vector3 getTangent(int index)
		{
			return this.tangents[index];
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000AA76C File Offset: 0x000A896C
		public void setTangent(int index, Vector3 tangent)
		{
			this.tangents[index] = tangent;
			if (this.mode == ERoadMode.MIRROR)
			{
				this.tangents[1 - index] = -tangent;
				return;
			}
			if (this.mode == ERoadMode.ALIGNED)
			{
				this.tangents[1 - index] = -tangent.normalized * this.tangents[1 - index].magnitude;
			}
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000AA7DE File Offset: 0x000A89DE
		public RoadJoint(Vector3 vertex)
		{
			this.vertex = vertex;
			this.tangents = new Vector3[2];
			this.mode = ERoadMode.MIRROR;
			this.offset = 0f;
			this.ignoreTerrain = false;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000AA812 File Offset: 0x000A8A12
		public RoadJoint(Vector3 vertex, Vector3[] tangents, ERoadMode mode, float offset, bool ignoreTerrain)
		{
			this.vertex = vertex;
			this.tangents = tangents;
			this.mode = mode;
			this.offset = offset;
			this.ignoreTerrain = ignoreTerrain;
		}

		// Token: 0x04001576 RID: 5494
		public Vector3 vertex;

		// Token: 0x04001577 RID: 5495
		private Vector3[] tangents;

		// Token: 0x04001578 RID: 5496
		public ERoadMode mode;

		// Token: 0x04001579 RID: 5497
		public float offset;

		// Token: 0x0400157A RID: 5498
		public bool ignoreTerrain;
	}
}
