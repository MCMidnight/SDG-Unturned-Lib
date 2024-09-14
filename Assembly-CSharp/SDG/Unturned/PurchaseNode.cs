using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000502 RID: 1282
	public class PurchaseNode : Node
	{
		/// <summary>
		/// This value is confusing because in the level editor it is the normalized radius, but in-game it is the square radius.
		/// </summary>
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000A9B8A File Offset: 0x000A7D8A
		// (set) Token: 0x06002830 RID: 10288 RVA: 0x000A9BAA File Offset: 0x000A7DAA
		public float radius
		{
			get
			{
				if (Level.isEditor)
				{
					return this._normalizedRadius;
				}
				return MathfEx.Square(PurchaseNode.CalculateRadiusFromNormalizedRadius(this._normalizedRadius));
			}
			set
			{
				this._normalizedRadius = value;
			}
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000A9BB3 File Offset: 0x000A7DB3
		public static float CalculateRadiusFromNormalizedRadius(float normalizedRadius)
		{
			return Mathf.Lerp(PurchaseNode.MIN_SIZE, PurchaseNode.MAX_SIZE, normalizedRadius) * 0.5f;
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000A9BCB File Offset: 0x000A7DCB
		public static float CalculateNormalizedRadiusFromRadius(float radius)
		{
			return Mathf.InverseLerp(PurchaseNode.MIN_SIZE, PurchaseNode.MAX_SIZE, radius * 2f);
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000A9BE3 File Offset: 0x000A7DE3
		public PurchaseNode(Vector3 newPoint) : this(newPoint, 0f, 0, 0U)
		{
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000A9BF3 File Offset: 0x000A7DF3
		public PurchaseNode(Vector3 newPoint, float newRadius, ushort newID, uint newCost)
		{
			this._point = newPoint;
			this._normalizedRadius = newRadius;
			this.id = newID;
			this.cost = newCost;
			this._type = ENodeType.PURCHASE;
		}

		// Token: 0x04001549 RID: 5449
		public static readonly float MIN_SIZE = 2f;

		// Token: 0x0400154A RID: 5450
		public static readonly float MAX_SIZE = 16f;

		// Token: 0x0400154B RID: 5451
		internal float _normalizedRadius;

		// Token: 0x0400154C RID: 5452
		public ushort id;

		// Token: 0x0400154D RID: 5453
		public uint cost;
	}
}
