using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200050C RID: 1292
	public class SafezoneNode : Node
	{
		/// <summary>
		/// This value is confusing because in the level editor it is the normalized radius, but in-game it is the square radius.
		/// </summary>
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x000ACCAA File Offset: 0x000AAEAA
		// (set) Token: 0x0600288D RID: 10381 RVA: 0x000ACCCA File Offset: 0x000AAECA
		public float radius
		{
			get
			{
				if (Level.isEditor)
				{
					return this._normalizedRadius;
				}
				return MathfEx.Square(SafezoneNode.CalculateRadiusFromNormalizedRadius(this._normalizedRadius));
			}
			set
			{
				this._normalizedRadius = value;
			}
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000ACCD3 File Offset: 0x000AAED3
		public static float CalculateRadiusFromNormalizedRadius(float normalizedRadius)
		{
			return Mathf.Lerp(SafezoneNode.MIN_SIZE, SafezoneNode.MAX_SIZE, normalizedRadius) * 0.5f;
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000ACCEB File Offset: 0x000AAEEB
		public static float CalculateNormalizedRadiusFromRadius(float radius)
		{
			return Mathf.InverseLerp(SafezoneNode.MIN_SIZE, SafezoneNode.MAX_SIZE, radius * 2f);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000ACD03 File Offset: 0x000AAF03
		public SafezoneNode(Vector3 newPoint) : this(newPoint, 0f, false, true, true)
		{
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000ACD14 File Offset: 0x000AAF14
		public SafezoneNode(Vector3 newPoint, float newRadius, bool newHeight, bool newNoWeapons, bool newNoBuildables)
		{
			this._point = newPoint;
			this._normalizedRadius = newRadius;
			this.isHeight = newHeight;
			this.noWeapons = newNoWeapons;
			this.noBuildables = newNoBuildables;
			this._type = ENodeType.SAFEZONE;
		}

		// Token: 0x04001592 RID: 5522
		public static readonly float MIN_SIZE = 32f;

		// Token: 0x04001593 RID: 5523
		public static readonly float MAX_SIZE = 1024f;

		// Token: 0x04001594 RID: 5524
		internal float _normalizedRadius;

		// Token: 0x04001595 RID: 5525
		public bool isHeight;

		// Token: 0x04001596 RID: 5526
		public bool noWeapons;

		// Token: 0x04001597 RID: 5527
		public bool noBuildables;
	}
}
