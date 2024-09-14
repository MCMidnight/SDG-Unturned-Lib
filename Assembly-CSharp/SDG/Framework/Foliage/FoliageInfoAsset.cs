using System;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000EB RID: 235
	public abstract class FoliageInfoAsset : Asset
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00015A88 File Offset: 0x00013C88
		public virtual float randomNormalPositionOffset
		{
			get
			{
				return Random.Range(this.minNormalPositionOffset, this.maxNormalPositionOffset);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00015A9C File Offset: 0x00013C9C
		public virtual Quaternion randomRotation
		{
			get
			{
				return Quaternion.Euler(new Vector3(Random.Range(this.minRotation.x, this.maxRotation.x), Random.Range(this.minRotation.y, this.maxRotation.y), Random.Range(this.minRotation.z, this.maxRotation.z)));
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00015B04 File Offset: 0x00013D04
		public virtual Vector3 randomScale
		{
			get
			{
				return new Vector3(Random.Range(this.minScale.x, this.maxScale.x), Random.Range(this.minScale.y, this.maxScale.y), Random.Range(this.minScale.z, this.maxScale.z));
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00015B67 File Offset: 0x00013D67
		public virtual void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!this.isSurfaceWeightValid(surfaceWeight))
			{
				return;
			}
			this.bakeFoliageSteps(surface, bounds, surfaceWeight, collectionWeight, new FoliageInfoAsset.BakeFoliageStepHandler(this.handleBakeFoliageStep));
		}

		/// <param name="followRules">Should angle limits and subtractive volumes be respected? Disabled when manually placing individually.</param>
		// Token: 0x060005BE RID: 1470 RVA: 0x00015B90 File Offset: 0x00013D90
		public virtual void addFoliageToSurface(Vector3 surfacePosition, Vector3 surfaceNormal, bool clearWhenBaked, bool followRules)
		{
			if (followRules && !this.isAngleValid(surfaceNormal))
			{
				return;
			}
			Vector3 position = surfacePosition + surfaceNormal * this.randomNormalPositionOffset;
			if (followRules && !this.isPositionValid(position))
			{
				return;
			}
			Quaternion quaternion = Quaternion.Lerp(MathUtility.IDENTITY_QUATERNION, Quaternion.FromToRotation(Vector3.up, surfaceNormal), this.normalRotationAlignment);
			quaternion *= Quaternion.Euler(this.normalRotationOffset);
			quaternion *= this.randomRotation;
			Vector3 randomScale = this.randomScale;
			this.addFoliage(position, quaternion, randomScale, clearWhenBaked);
		}

		// Token: 0x060005BF RID: 1471
		public abstract int getInstanceCountInVolume(IShapeVolume volume);

		// Token: 0x060005C0 RID: 1472
		protected abstract void addFoliage(Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked);

		// Token: 0x060005C1 RID: 1473 RVA: 0x00015C18 File Offset: 0x00013E18
		protected virtual void bakeFoliageSteps(IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight, FoliageInfoAsset.BakeFoliageStepHandler callback)
		{
			float num = surfaceWeight * collectionWeight;
			float num2 = bounds.size.x * bounds.size.z / this.density * num;
			int num3 = Mathf.FloorToInt(num2);
			if (Random.value < num2 - (float)num3)
			{
				num3++;
			}
			for (int i = 0; i < num3; i++)
			{
				callback(surface, bounds, surfaceWeight, collectionWeight);
			}
		}

		/// <summary>
		/// Pick a point inside the bounds to test for foliage placement. The base implementation is completely random, but a blue noise implementation could be very nice.
		/// </summary>
		// Token: 0x060005C2 RID: 1474 RVA: 0x00015C7C File Offset: 0x00013E7C
		protected virtual Vector3 getTestPosition(Bounds bounds)
		{
			float x = Random.Range(-1f, 1f) * bounds.extents.x;
			float z = Random.Range(-1f, 1f) * bounds.extents.z;
			return bounds.center + new Vector3(x, bounds.extents.y, z);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00015CE4 File Offset: 0x00013EE4
		protected virtual void handleBakeFoliageStep(IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			Vector3 testPosition = this.getTestPosition(bounds);
			Vector3 surfacePosition;
			Vector3 surfaceNormal;
			if (!surface.getFoliageSurfaceInfo(testPosition, out surfacePosition, out surfaceNormal))
			{
				return;
			}
			this.addFoliageToSurface(surfacePosition, surfaceNormal, true, true);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00015D14 File Offset: 0x00013F14
		protected virtual bool isAngleValid(Vector3 surfaceNormal)
		{
			float num = Vector3.Angle(Vector3.up, surfaceNormal);
			return num >= this.minSurfaceAngle && num <= this.maxSurfaceAngle;
		}

		// Token: 0x060005C5 RID: 1477
		protected abstract bool isPositionValid(Vector3 position);

		// Token: 0x060005C6 RID: 1478 RVA: 0x00015D44 File Offset: 0x00013F44
		protected virtual bool isSurfaceWeightValid(float surfaceWeight)
		{
			return surfaceWeight >= this.minSurfaceWeight && surfaceWeight <= this.maxSurfaceWeight;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00015D60 File Offset: 0x00013F60
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.density = data.ParseFloat("Density", 0f);
			this.minNormalPositionOffset = data.ParseFloat("Min_Normal_Position_Offset", 0f);
			this.maxNormalPositionOffset = data.ParseFloat("Max_Normal_Position_Offset", 0f);
			this.normalRotationOffset = data.ParseVector3("Normal_Rotation_Offset", default(Vector3));
			if (data.ContainsKey("Normal_Rotation_Alignment"))
			{
				this.normalRotationAlignment = data.ParseFloat("Normal_Rotation_Alignment", 0f);
			}
			else
			{
				this.normalRotationAlignment = 1f;
			}
			this.minSurfaceWeight = data.ParseFloat("Min_Weight", 0f);
			this.maxSurfaceWeight = data.ParseFloat("Max_Weight", 0f);
			this.minSurfaceAngle = data.ParseFloat("Min_Angle", 0f);
			this.maxSurfaceAngle = data.ParseFloat("Max_Angle", 0f);
			this.minRotation = data.ParseVector3("Min_Rotation", default(Vector3));
			this.maxRotation = data.ParseVector3("Max_Rotation", default(Vector3));
			this.minScale = data.ParseVector3("Min_Scale", default(Vector3));
			this.maxScale = data.ParseVector3("Max_Scale", default(Vector3));
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00015EC2 File Offset: 0x000140C2
		protected virtual void resetFoliageInfo()
		{
			this.normalRotationAlignment = 1f;
			this.maxSurfaceWeight = 1f;
			this.minScale = Vector3.one;
			this.maxScale = Vector3.one;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00015EF0 File Offset: 0x000140F0
		public FoliageInfoAsset()
		{
			this.resetFoliageInfo();
		}

		// Token: 0x04000211 RID: 529
		public float density;

		// Token: 0x04000212 RID: 530
		public float minNormalPositionOffset;

		// Token: 0x04000213 RID: 531
		public float maxNormalPositionOffset;

		// Token: 0x04000214 RID: 532
		public Vector3 normalRotationOffset;

		// Token: 0x04000215 RID: 533
		public float normalRotationAlignment;

		// Token: 0x04000216 RID: 534
		public float minSurfaceWeight;

		// Token: 0x04000217 RID: 535
		public float maxSurfaceWeight;

		// Token: 0x04000218 RID: 536
		public float minSurfaceAngle;

		// Token: 0x04000219 RID: 537
		public float maxSurfaceAngle;

		// Token: 0x0400021A RID: 538
		public Vector3 minRotation;

		// Token: 0x0400021B RID: 539
		public Vector3 maxRotation;

		// Token: 0x0400021C RID: 540
		public Vector3 minScale;

		// Token: 0x0400021D RID: 541
		public Vector3 maxScale;

		// Token: 0x02000863 RID: 2147
		// (Invoke) Token: 0x06004802 RID: 18434
		protected delegate void BakeFoliageStepHandler(IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight);
	}
}
