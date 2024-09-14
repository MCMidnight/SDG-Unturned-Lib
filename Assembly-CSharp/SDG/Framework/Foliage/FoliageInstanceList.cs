using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000EF RID: 239
	public class FoliageInstanceList : IPoolable
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x000162AF File Offset: 0x000144AF
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x000162B7 File Offset: 0x000144B7
		public List<List<Matrix4x4>> matrices { get; protected set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x000162C0 File Offset: 0x000144C0
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x000162C8 File Offset: 0x000144C8
		public List<List<bool>> clearWhenBaked { get; protected set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x000162D1 File Offset: 0x000144D1
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x000162D9 File Offset: 0x000144D9
		public bool isAssetLoaded { get; protected set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x000162E2 File Offset: 0x000144E2
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x000162EA File Offset: 0x000144EA
		public Mesh mesh { get; protected set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x000162F3 File Offset: 0x000144F3
		// (set) Token: 0x060005DF RID: 1503 RVA: 0x000162FB File Offset: 0x000144FB
		public Material material { get; protected set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00016304 File Offset: 0x00014504
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0001630C File Offset: 0x0001450C
		public bool castShadows { get; protected set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00016315 File Offset: 0x00014515
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x0001631D File Offset: 0x0001451D
		public bool tileDither { get; protected set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00016326 File Offset: 0x00014526
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x0001632E File Offset: 0x0001452E
		public int sqrDrawDistance { get; protected set; }

		// Token: 0x060005E6 RID: 1510 RVA: 0x00016337 File Offset: 0x00014537
		public virtual void poolClaim()
		{
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001633C File Offset: 0x0001453C
		public virtual void poolRelease()
		{
			this.assetReference = AssetReference<FoliageInstancedMeshInfoAsset>.invalid;
			foreach (List<Matrix4x4> list in this.matrices)
			{
				ListPool<Matrix4x4>.release(list);
			}
			this.matrices.Clear();
			foreach (List<bool> list2 in this.clearWhenBaked)
			{
				ListPool<bool>.release(list2);
			}
			this.clearWhenBaked.Clear();
			this.isAssetLoaded = false;
			this.mesh = null;
			this.material = null;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00016404 File Offset: 0x00014604
		public bool IsListEmpty()
		{
			using (List<List<Matrix4x4>>.Enumerator enumerator = this.matrices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Count > 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00016460 File Offset: 0x00014660
		public virtual void clearGeneratedInstances()
		{
			for (int i = 0; i < this.matrices.Count; i++)
			{
				List<Matrix4x4> list = this.matrices[i];
				List<bool> list2 = this.clearWhenBaked[i];
				for (int j = list.Count - 1; j >= 0; j--)
				{
					if (list2[j])
					{
						list.RemoveAt(j);
						list2.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000164C8 File Offset: 0x000146C8
		public virtual void applyScale()
		{
			FoliageInstancedMeshInfoAsset foliageInstancedMeshInfoAsset = Assets.find<FoliageInstancedMeshInfoAsset>(this.assetReference);
			if (foliageInstancedMeshInfoAsset == null)
			{
				return;
			}
			for (int i = 0; i < this.matrices.Count; i++)
			{
				List<Matrix4x4> list = this.matrices[i];
				List<bool> list2 = this.clearWhenBaked[i];
				for (int j = list.Count - 1; j >= 0; j--)
				{
					Matrix4x4 matrix4x = list[j];
					Vector3 position = matrix4x.GetPosition();
					Quaternion rotation = matrix4x.GetRotation();
					Vector3 randomScale = foliageInstancedMeshInfoAsset.randomScale;
					matrix4x = Matrix4x4.TRS(position, rotation, randomScale);
					list[j] = matrix4x;
				}
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00016560 File Offset: 0x00014760
		protected virtual void getOrAddLists(out List<Matrix4x4> matrixList, out List<bool> clearWhenBakedList)
		{
			matrixList = null;
			foreach (List<Matrix4x4> list in this.matrices)
			{
				if (list.Count < 1023)
				{
					matrixList = list;
					break;
				}
			}
			if (matrixList == null)
			{
				matrixList = ListPool<Matrix4x4>.claim();
				this.matrices.Add(matrixList);
			}
			clearWhenBakedList = null;
			foreach (List<bool> list2 in this.clearWhenBaked)
			{
				if (list2.Count < 1023)
				{
					clearWhenBakedList = list2;
					break;
				}
			}
			if (clearWhenBakedList == null)
			{
				clearWhenBakedList = ListPool<bool>.claim();
				this.clearWhenBaked.Add(clearWhenBakedList);
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00016644 File Offset: 0x00014844
		public virtual void addInstanceRandom(FoliageInstanceGroup group)
		{
			List<Matrix4x4> list;
			List<bool> list2;
			this.getOrAddLists(out list, out list2);
			int num = Random.Range(0, list.Count);
			list.Insert(num, group.matrix);
			list2.Insert(num, group.clearWhenBaked);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00016684 File Offset: 0x00014884
		public virtual void addInstanceAppend(FoliageInstanceGroup group)
		{
			List<Matrix4x4> list;
			List<bool> list2;
			this.getOrAddLists(out list, out list2);
			list.Add(group.matrix);
			list2.Add(group.clearWhenBaked);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000166B4 File Offset: 0x000148B4
		public virtual void removeInstance(int matricesIndex, int matrixIndex)
		{
			List<Matrix4x4> list = this.matrices[matricesIndex];
			List<bool> list2 = this.clearWhenBaked[matricesIndex];
			list.RemoveAt(matrixIndex);
			list2.RemoveAt(matrixIndex);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000166E8 File Offset: 0x000148E8
		public virtual void loadAsset()
		{
			if (this.isAssetLoaded)
			{
				return;
			}
			this.isAssetLoaded = true;
			FoliageInstancedMeshInfoAsset foliageInstancedMeshInfoAsset = this.assetReference.Find();
			ClientAssetIntegrity.QueueRequest(this.assetReference.GUID, foliageInstancedMeshInfoAsset, "Foliage");
			if (foliageInstancedMeshInfoAsset == null)
			{
				return;
			}
			if (Level.shouldUseHolidayRedirects)
			{
				AssetReference<FoliageInstancedMeshInfoAsset>? holidayRedirect = foliageInstancedMeshInfoAsset.getHolidayRedirect();
				if (holidayRedirect != null)
				{
					this.assetReference = holidayRedirect.Value;
					foliageInstancedMeshInfoAsset = this.assetReference.Find();
					if (foliageInstancedMeshInfoAsset == null)
					{
						return;
					}
				}
			}
			this.mesh = Assets.load<Mesh>(foliageInstancedMeshInfoAsset.mesh);
			this.material = Assets.load<Material>(foliageInstancedMeshInfoAsset.material);
			if (this.material != null && !this.material.enableInstancing)
			{
				this.material.enableInstancing = true;
			}
			this.castShadows = foliageInstancedMeshInfoAsset.castShadows;
			this.tileDither = foliageInstancedMeshInfoAsset.tileDither;
			if (foliageInstancedMeshInfoAsset.drawDistance == -1)
			{
				this.sqrDrawDistance = -1;
				return;
			}
			this.sqrDrawDistance = foliageInstancedMeshInfoAsset.drawDistance * foliageInstancedMeshInfoAsset.drawDistance;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000167E4 File Offset: 0x000149E4
		public FoliageInstanceList()
		{
			this.matrices = new List<List<Matrix4x4>>(1);
			this.clearWhenBaked = new List<List<bool>>(1);
		}

		// Token: 0x04000229 RID: 553
		public AssetReference<FoliageInstancedMeshInfoAsset> assetReference;
	}
}
