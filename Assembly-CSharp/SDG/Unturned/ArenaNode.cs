using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004AC RID: 1196
	public class ArenaNode : Node
	{
		/// <summary>
		/// This value is confusing because in the level editor it is the normalized radius, but in-game it is the radius.
		/// </summary>
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x00093A55 File Offset: 0x00091C55
		// (set) Token: 0x0600250C RID: 9484 RVA: 0x00093A70 File Offset: 0x00091C70
		public float radius
		{
			get
			{
				if (Level.isEditor)
				{
					return this._normalizedRadius;
				}
				return ArenaNode.CalculateRadiusFromNormalizedRadius(this._normalizedRadius);
			}
			set
			{
				this._normalizedRadius = value;
			}
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x00093A79 File Offset: 0x00091C79
		public static float CalculateRadiusFromNormalizedRadius(float normalizedRadius)
		{
			return Mathf.Lerp(ArenaNode.MIN_SIZE, ArenaNode.MAX_SIZE, normalizedRadius) * 0.5f;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x00093A91 File Offset: 0x00091C91
		public static float CalculateNormalizedRadiusFromRadius(float radius)
		{
			return Mathf.InverseLerp(ArenaNode.MIN_SIZE, ArenaNode.MAX_SIZE, radius * 2f);
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x00093AA9 File Offset: 0x00091CA9
		public ArenaNode(Vector3 newPoint) : this(newPoint, 0f)
		{
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x00093AB7 File Offset: 0x00091CB7
		public ArenaNode(Vector3 newPoint, float newRadius)
		{
			this._point = newPoint;
			this._normalizedRadius = newRadius;
			this._type = ENodeType.ARENA;
		}

		// Token: 0x040012CF RID: 4815
		public static readonly float MIN_SIZE = 128f;

		// Token: 0x040012D0 RID: 4816
		public static readonly float MAX_SIZE = 8192f;

		// Token: 0x040012D1 RID: 4817
		internal float _normalizedRadius;
	}
}
