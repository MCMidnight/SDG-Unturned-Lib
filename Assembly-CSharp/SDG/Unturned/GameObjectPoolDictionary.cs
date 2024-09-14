using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000754 RID: 1876
	public class GameObjectPoolDictionary
	{
		// Token: 0x06003D63 RID: 15715 RVA: 0x00126B46 File Offset: 0x00124D46
		public PoolReference Instantiate(GameObject prefab)
		{
			return this.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x00126B5C File Offset: 0x00124D5C
		public PoolReference Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			GameObjectPool gameObjectPool;
			if (!this.pools.TryGetValue(prefab, ref gameObjectPool))
			{
				gameObjectPool = new GameObjectPool(prefab);
				this.pools.Add(prefab, gameObjectPool);
			}
			return gameObjectPool.Instantiate(position, rotation);
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x00126B98 File Offset: 0x00124D98
		public void Instantiate(GameObject prefab, string name, int count)
		{
			GameObjectPool gameObjectPool;
			if (!this.pools.TryGetValue(prefab, ref gameObjectPool))
			{
				gameObjectPool = new GameObjectPool(prefab, count);
				this.pools.Add(prefab, gameObjectPool);
			}
			GameObject[] array = new GameObject[count];
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = gameObjectPool.Instantiate().gameObject;
				gameObject.name = name;
				array[i] = gameObject;
			}
			for (int j = 0; j < count; j++)
			{
				gameObjectPool.Destroy(array[j].GetComponent<PoolReference>());
			}
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00126C14 File Offset: 0x00124E14
		public void Destroy(GameObject element)
		{
			if (element == null)
			{
				return;
			}
			PoolReference component = element.GetComponent<PoolReference>();
			if (component == null || component.pool == null)
			{
				if (element.transform.parent != null)
				{
					EffectManager.UnregisterAttachment(element);
					element.transform.parent = null;
				}
				Object.Destroy(element);
				return;
			}
			component.pool.Destroy(component);
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x00126C7C File Offset: 0x00124E7C
		public void Destroy(GameObject element, float t)
		{
			if (element == null)
			{
				return;
			}
			PoolReference component = element.GetComponent<PoolReference>();
			if (component == null || component.pool == null)
			{
				if (element.transform.parent != null)
				{
					EffectManager.UnregisterAttachment(element);
					element.transform.parent = null;
				}
				Object.Destroy(element);
				return;
			}
			component.DestroyIntoPool(t);
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x00126CE0 File Offset: 0x00124EE0
		public void DestroyAll()
		{
			foreach (GameObjectPool gameObjectPool in this.pools.Values)
			{
				gameObjectPool.DestroyAll();
			}
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x00126D38 File Offset: 0x00124F38
		public void DestroyAllMatchingPrefab(GameObject prefab)
		{
			GameObjectPool gameObjectPool;
			if (this.pools.TryGetValue(prefab, ref gameObjectPool))
			{
				gameObjectPool.DestroyAll();
			}
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x00126D5B File Offset: 0x00124F5B
		public GameObjectPoolDictionary()
		{
			this.pools = new Dictionary<GameObject, GameObjectPool>();
		}

		// Token: 0x040026A6 RID: 9894
		internal Dictionary<GameObject, GameObjectPool> pools;
	}
}
