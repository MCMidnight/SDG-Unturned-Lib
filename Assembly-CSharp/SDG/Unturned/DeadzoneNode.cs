using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004B6 RID: 1206
	public class DeadzoneNode : Node, IDeadzoneNode
	{
		/// <summary>
		/// This value is confusing because in the level editor it is the normalized radius, but in-game it is the square radius.
		/// </summary>
		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x000941A8 File Offset: 0x000923A8
		// (set) Token: 0x0600252A RID: 9514 RVA: 0x000941C8 File Offset: 0x000923C8
		public float radius
		{
			get
			{
				if (Level.isEditor)
				{
					return this._normalizedRadius;
				}
				return MathfEx.Square(DeadzoneNode.CalculateRadiusFromNormalizedRadius(this._normalizedRadius));
			}
			set
			{
				this._normalizedRadius = value;
			}
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000941D1 File Offset: 0x000923D1
		public static float CalculateRadiusFromNormalizedRadius(float normalizedRadius)
		{
			return Mathf.Lerp(DeadzoneNode.MIN_SIZE, DeadzoneNode.MAX_SIZE, normalizedRadius) * 0.5f;
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000941E9 File Offset: 0x000923E9
		public static float CalculateNormalizedRadiusFromRadius(float radius)
		{
			return Mathf.InverseLerp(DeadzoneNode.MIN_SIZE, DeadzoneNode.MAX_SIZE, radius * 2f);
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600252D RID: 9517 RVA: 0x00094201 File Offset: 0x00092401
		// (set) Token: 0x0600252E RID: 9518 RVA: 0x00094209 File Offset: 0x00092409
		public EDeadzoneType DeadzoneType
		{
			get
			{
				return this._deadzoneType;
			}
			set
			{
				this._deadzoneType = value;
			}
		}

		/// <summary>
		/// Nelson 2024-06-10: Added this property after nodes were converted to volumes. i.e., only old levels from
		/// before this property were added still have nodes, so it's expected that they won't deal damage over time.
		/// </summary>
		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x00094212 File Offset: 0x00092412
		public float UnprotectedDamagePerSecond
		{
			get
			{
				return 0f;
			}
		}

		/// <summary>
		/// Same description as <see cref="P:SDG.Unturned.DeadzoneNode.UnprotectedDamagePerSecond" />.
		/// </summary>
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002530 RID: 9520 RVA: 0x00094219 File Offset: 0x00092419
		public float ProtectedDamagePerSecond
		{
			get
			{
				return 0f;
			}
		}

		/// <summary>
		/// Same description as <see cref="P:SDG.Unturned.DeadzoneNode.UnprotectedDamagePerSecond" />.
		/// </summary>
		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x00094220 File Offset: 0x00092420
		public float UnprotectedRadiationPerSecond
		{
			get
			{
				return 6.25f;
			}
		}

		/// <summary>
		/// Same description as <see cref="P:SDG.Unturned.DeadzoneNode.UnprotectedDamagePerSecond" />.
		/// </summary>
		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x00094227 File Offset: 0x00092427
		public float MaskFilterDamagePerSecond
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x0009422E File Offset: 0x0009242E
		public DeadzoneNode(Vector3 newPoint) : this(newPoint, 0f, EDeadzoneType.DefaultRadiation)
		{
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x0009423D File Offset: 0x0009243D
		public DeadzoneNode(Vector3 newPoint, float newRadius, EDeadzoneType newDeadzoneType)
		{
			this._point = newPoint;
			this._deadzoneType = newDeadzoneType;
			this._normalizedRadius = newRadius;
			this._type = ENodeType.DEADZONE;
		}

		// Token: 0x040012DD RID: 4829
		public static readonly float MIN_SIZE = 32f;

		// Token: 0x040012DE RID: 4830
		public static readonly float MAX_SIZE = 1024f;

		// Token: 0x040012DF RID: 4831
		internal float _normalizedRadius;

		// Token: 0x040012E0 RID: 4832
		private EDeadzoneType _deadzoneType;
	}
}
