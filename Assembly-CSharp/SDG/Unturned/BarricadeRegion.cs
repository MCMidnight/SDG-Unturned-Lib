using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000540 RID: 1344
	public class BarricadeRegion
	{
		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000B6E83 File Offset: 0x000B5083
		public List<BarricadeDrop> drops
		{
			get
			{
				return this._drops;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x000B6E8B File Offset: 0x000B508B
		[Obsolete("Maintaining two separate lists was error prone, but still kept for backwards compat")]
		public List<BarricadeData> barricades
		{
			get
			{
				return this._barricades;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000B6E93 File Offset: 0x000B5093
		public Transform parent
		{
			get
			{
				return this._parent;
			}
		}

		/// <summary>
		/// New code should not use this. Only intended for backwards compatibility.
		/// </summary>
		// Token: 0x06002AB6 RID: 10934 RVA: 0x000B6E9C File Offset: 0x000B509C
		public int IndexOfBarricadeByRootTransform(Transform rootTransform)
		{
			for (int i = 0; i < this._drops.Count; i++)
			{
				if (this._drops[i].model == rootTransform)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000B6EDB File Offset: 0x000B50DB
		public BarricadeDrop FindBarricadeByRootTransform(Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			BarricadeRefComponent component = transform.GetComponent<BarricadeRefComponent>();
			if (component == null)
			{
				return null;
			}
			return component.tempNotSureIfBarricadeShouldBeAComponentYet;
		}

		/// <summary>
		/// Ideally the interactable components should have a reference to their barricade, but that will maybe happen
		/// after the NetId rewrites. For the meantime this is to avoid calling FindBarricadeByRootTransform. If we go
		/// the component route then FindBarricadeByRootTransform will do the same as this method.
		/// </summary>
		// Token: 0x06002AB8 RID: 10936 RVA: 0x000B6EF3 File Offset: 0x000B50F3
		internal BarricadeDrop FindBarricadeByRootFast(Transform rootTransform)
		{
			return rootTransform.GetComponent<BarricadeRefComponent>().tempNotSureIfBarricadeShouldBeAComponentYet;
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000B6F00 File Offset: 0x000B5100
		[Obsolete("Dead code, please contact if you need this and we will make a plan")]
		public BarricadeData findBarricadeByInstanceID(uint instanceID)
		{
			foreach (BarricadeData barricadeData in this.barricades)
			{
				if (barricadeData.instanceID == instanceID)
				{
					return barricadeData;
				}
			}
			return null;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000B6F5C File Offset: 0x000B515C
		[Obsolete("Renamed to DestroyAll")]
		public void destroy()
		{
			this.DestroyAll();
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000B6F64 File Offset: 0x000B5164
		internal void DestroyTail()
		{
			this._drops.GetAndRemoveTail<BarricadeDrop>().CustomDestroy();
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000B6F78 File Offset: 0x000B5178
		internal void DestroyAll()
		{
			foreach (BarricadeDrop barricadeDrop in this._drops)
			{
				barricadeDrop.CustomDestroy();
			}
			this.drops.Clear();
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000B6FD4 File Offset: 0x000B51D4
		public BarricadeRegion(Transform newParent)
		{
			this._drops = new List<BarricadeDrop>();
			this._barricades = new List<BarricadeData>();
			this._parent = newParent;
			this.isNetworked = false;
			this.isPendingDestroy = false;
		}

		// Token: 0x040016C1 RID: 5825
		private List<BarricadeDrop> _drops;

		// Token: 0x040016C2 RID: 5826
		private List<BarricadeData> _barricades;

		// Token: 0x040016C3 RID: 5827
		private Transform _parent;

		// Token: 0x040016C4 RID: 5828
		public bool isNetworked;

		// Token: 0x040016C5 RID: 5829
		internal bool isPendingDestroy;
	}
}
