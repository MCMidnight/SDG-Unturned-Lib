using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004BA RID: 1210
	public class EffectNode : Node, IAmbianceNode
	{
		/// <summary>
		/// This value is confusing because in the level editor it is the normalized radius, but in-game it is the square radius.
		/// </summary>
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x0009467C File Offset: 0x0009287C
		// (set) Token: 0x06002549 RID: 9545 RVA: 0x0009469C File Offset: 0x0009289C
		public float radius
		{
			get
			{
				if (Level.isEditor)
				{
					return this._normalizedRadius;
				}
				return MathfEx.Square(EffectNode.CalculateRadiusFromNormalizedRadius(this._normalizedRadius));
			}
			set
			{
				this._normalizedRadius = value;
			}
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000946A5 File Offset: 0x000928A5
		public static float CalculateRadiusFromNormalizedRadius(float normalizedRadius)
		{
			return Mathf.Lerp(EffectNode.MIN_SIZE, EffectNode.MAX_SIZE, normalizedRadius) * 0.5f;
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000946BD File Offset: 0x000928BD
		public static float CalculateNormalizedRadiusFromRadius(float radius)
		{
			return Mathf.InverseLerp(EffectNode.MIN_SIZE, EffectNode.MAX_SIZE, radius * 2f);
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x000946D5 File Offset: 0x000928D5
		public float editorRadius
		{
			get
			{
				return MathfEx.Square(EffectNode.CalculateRadiusFromNormalizedRadius(this._normalizedRadius));
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x000946E7 File Offset: 0x000928E7
		// (set) Token: 0x0600254E RID: 9550 RVA: 0x000946EF File Offset: 0x000928EF
		public Vector3 bounds
		{
			get
			{
				return this._bounds;
			}
			set
			{
				this._bounds = value;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x000946F8 File Offset: 0x000928F8
		// (set) Token: 0x06002550 RID: 9552 RVA: 0x00094700 File Offset: 0x00092900
		public ENodeShape shape
		{
			get
			{
				return this._shape;
			}
			set
			{
				this._shape = value;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x00094709 File Offset: 0x00092909
		// (set) Token: 0x06002552 RID: 9554 RVA: 0x00094711 File Offset: 0x00092911
		public ushort id { get; set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x0009471A File Offset: 0x0009291A
		// (set) Token: 0x06002554 RID: 9556 RVA: 0x00094722 File Offset: 0x00092922
		public bool noWater { get; set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x0009472B File Offset: 0x0009292B
		// (set) Token: 0x06002556 RID: 9558 RVA: 0x00094733 File Offset: 0x00092933
		public bool noLighting { get; set; }

		// Token: 0x06002557 RID: 9559 RVA: 0x0009473C File Offset: 0x0009293C
		public EffectAsset GetEffectAsset()
		{
			return Assets.find(EAssetType.EFFECT, this.id) as EffectAsset;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x0009474F File Offset: 0x0009294F
		public EffectNode(Vector3 newPoint) : this(newPoint, ENodeShape.SPHERE, 0f, Vector3.one, 0, false, false)
		{
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x00094768 File Offset: 0x00092968
		public EffectNode(Vector3 newPoint, ENodeShape newShape, float newRadius, Vector3 newBounds, ushort newID, bool newNoWater, bool newNoLighting)
		{
			this._point = newPoint;
			this.shape = newShape;
			this._normalizedRadius = newRadius;
			this.bounds = newBounds;
			this.id = newID;
			this.noWater = newNoWater;
			this.noLighting = newNoLighting;
			this._type = ENodeType.EFFECT;
		}

		// Token: 0x040012E6 RID: 4838
		public static readonly float MIN_SIZE = 8f;

		// Token: 0x040012E7 RID: 4839
		public static readonly float MAX_SIZE = 256f;

		// Token: 0x040012E8 RID: 4840
		internal float _normalizedRadius;

		// Token: 0x040012E9 RID: 4841
		private Vector3 _bounds;

		// Token: 0x040012EA RID: 4842
		private ENodeShape _shape;
	}
}
