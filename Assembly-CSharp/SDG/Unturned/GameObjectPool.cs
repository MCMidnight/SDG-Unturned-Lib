using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000753 RID: 1875
	public class GameObjectPool
	{
		// Token: 0x06003D5D RID: 15709 RVA: 0x001268FA File Offset: 0x00124AFA
		public PoolReference Instantiate()
		{
			return this.Instantiate(Vector3.zero, Quaternion.identity);
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x0012690C File Offset: 0x00124B0C
		public PoolReference Instantiate(Vector3 position, Quaternion rotation)
		{
			while (this.pool.Count > 0)
			{
				GameObject gameObject = this.pool.Pop();
				if (!(gameObject == null))
				{
					gameObject.transform.parent = null;
					gameObject.transform.position = position;
					gameObject.transform.rotation = rotation;
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(true);
					PoolReference component = gameObject.GetComponent<PoolReference>();
					component.inPool = false;
					component.excludeFromDestroyAll = false;
					this.active.Add(component);
					return component;
				}
			}
			PoolReference poolReference = Object.Instantiate<GameObject>(this.prefab, position, rotation).AddComponent<PoolReference>();
			poolReference.pool = this;
			poolReference.inPool = false;
			this.active.Add(poolReference);
			return poolReference;
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x001269D0 File Offset: 0x00124BD0
		public void Destroy(PoolReference reference)
		{
			if (reference == null || reference.inPool || reference.pool != this)
			{
				return;
			}
			reference.CancelDestroyTimer();
			GameObject gameObject = reference.gameObject;
			gameObject.SetActive(false);
			if (gameObject.transform.parent != null)
			{
				EffectManager.UnregisterAttachment(gameObject);
				gameObject.transform.parent = null;
			}
			this.pool.Push(gameObject);
			this.active.RemoveFast(reference);
			reference.inPool = true;
			reference.excludeFromDestroyAll = false;
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x00126A58 File Offset: 0x00124C58
		public void DestroyAll()
		{
			for (int i = this.active.Count - 1; i >= 0; i--)
			{
				PoolReference poolReference = this.active[i];
				if (poolReference == null || poolReference.gameObject == null)
				{
					this.active.RemoveAtFast(i);
				}
				else if (!poolReference.excludeFromDestroyAll)
				{
					poolReference.CancelDestroyTimer();
					GameObject gameObject = poolReference.gameObject;
					gameObject.SetActive(false);
					if (gameObject.transform.parent != null)
					{
						EffectManager.UnregisterAttachment(gameObject);
						gameObject.transform.parent = null;
					}
					this.pool.Push(gameObject);
					this.active.RemoveAtFast(i);
					poolReference.inPool = true;
				}
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00126B15 File Offset: 0x00124D15
		public GameObjectPool(GameObject prefab) : this(prefab, 1)
		{
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x00126B1F File Offset: 0x00124D1F
		public GameObjectPool(GameObject prefab, int count)
		{
			this.prefab = prefab;
			this.pool = new Stack<GameObject>(count);
			this.active = new List<PoolReference>(count);
		}

		// Token: 0x040026A3 RID: 9891
		private GameObject prefab;

		// Token: 0x040026A4 RID: 9892
		internal Stack<GameObject> pool;

		// Token: 0x040026A5 RID: 9893
		internal List<PoolReference> active;
	}
}
