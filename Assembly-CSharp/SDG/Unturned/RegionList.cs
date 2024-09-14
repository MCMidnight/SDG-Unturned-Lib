using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006BD RID: 1725
	internal class RegionList<T>
	{
		// Token: 0x06003995 RID: 14741 RVA: 0x0010E0F8 File Offset: 0x0010C2F8
		public void Add(Vector3 position, T item)
		{
			int cellIndex = this.GetCellIndex(position.x);
			int cellIndex2 = this.GetCellIndex(position.z);
			this.GetOrAddList(cellIndex, cellIndex2).Add(item);
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0010E130 File Offset: 0x0010C330
		public bool RemoveFast(Vector3 position, T item, float tolerance)
		{
			using (IEnumerator<List<T>> enumerator = this.EnumerateListsInSquare(position, tolerance).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.RemoveFast(item))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Can be null if nothing has been added at position.
		/// </summary>
		// Token: 0x06003997 RID: 14743 RVA: 0x0010E188 File Offset: 0x0010C388
		public List<T> GetList(Vector3 position)
		{
			return this.grid[this.GetCellIndex(position.x), this.GetCellIndex(position.z)];
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x0010E1AD File Offset: 0x0010C3AD
		public IEnumerable<T> EnumerateAllItems()
		{
			List<T>[,] array = this.grid;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int i = array.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
				{
					List<T> list = array[i, j];
					if (list != null)
					{
						foreach (T t in list)
						{
							yield return t;
						}
						List<T>.Enumerator enumerator = default(List<T>.Enumerator);
					}
				}
			}
			array = null;
			yield break;
			yield break;
		}

		/// <summary>
		/// Does not add new lists to empty cells.
		/// </summary>
		// Token: 0x06003999 RID: 14745 RVA: 0x0010E1BD File Offset: 0x0010C3BD
		public IEnumerable<List<T>> EnumerateListsInSquare(Vector3 position, float radius)
		{
			int cellIndex = this.GetCellIndex(position.x - radius);
			int max_x = this.GetCellIndex(position.x + radius);
			int min_z = this.GetCellIndex(position.z - radius);
			int max_z = this.GetCellIndex(position.z + radius);
			int num;
			for (int x = cellIndex; x <= max_x; x = num)
			{
				for (int z = min_z; z <= max_z; z = num)
				{
					if (this.grid[x, z] != null)
					{
						yield return this.grid[x, z];
					}
					num = z + 1;
				}
				num = x + 1;
			}
			yield break;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x0010E1DB File Offset: 0x0010C3DB
		public IEnumerable<T> EnumerateItemsInSquare(Vector3 position, float radius)
		{
			int cellIndex = this.GetCellIndex(position.x - radius);
			int max_x = this.GetCellIndex(position.x + radius);
			int min_z = this.GetCellIndex(position.z - radius);
			int max_z = this.GetCellIndex(position.z + radius);
			int num;
			for (int x = cellIndex; x <= max_x; x = num)
			{
				for (int z = min_z; z <= max_z; z = num)
				{
					List<T> list = this.grid[x, z];
					if (list != null)
					{
						foreach (T t in list)
						{
							yield return t;
						}
						List<T>.Enumerator enumerator = default(List<T>.Enumerator);
					}
					num = z + 1;
				}
				num = x + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x0010E1FC File Offset: 0x0010C3FC
		public void DrawGrid(Vector3 cameraPosition, Color color)
		{
			cameraPosition.x = (float)(this.GetCellIndex(cameraPosition.x) * 16) - 4096f + 8f;
			cameraPosition.y = (float)Mathf.FloorToInt(cameraPosition.y * 0.1f) * 10f - 5f;
			cameraPosition.z = (float)(this.GetCellIndex(cameraPosition.z) * 16) - 4096f + 8f;
			RuntimeGizmos.Get().GridXZ(cameraPosition, 80f, 5, color, 0f, EGizmoLayer.World);
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0010E28C File Offset: 0x0010C48C
		public RegionList()
		{
			this.grid = new List<T>[512, 512];
			this.listPool = new List<List<T>>(1024);
			for (int i = 0; i < 1024; i++)
			{
				this.listPool.Add(new List<T>());
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0010E2E4 File Offset: 0x0010C4E4
		private List<T> GetOrAddList(int x, int z)
		{
			List<T> list = this.grid[x, z];
			if (list != null)
			{
				return list;
			}
			if (this.listPool.IsEmpty<List<T>>())
			{
				for (int i = 0; i < 1024; i++)
				{
					this.listPool.Add(new List<T>());
				}
			}
			list = this.listPool.GetAndRemoveTail<List<T>>();
			this.grid[x, z] = list;
			return list;
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x0010E34C File Offset: 0x0010C54C
		private int GetCellIndex(float position)
		{
			if (position <= -4096f)
			{
				return 0;
			}
			if (position >= 4096f)
			{
				return 511;
			}
			return Mathf.FloorToInt((position + 4096f) / 16f);
		}

		// Token: 0x0400223F RID: 8767
		private List<T>[,] grid;

		// Token: 0x04002240 RID: 8768
		private List<List<T>> listPool;

		// Token: 0x04002241 RID: 8769
		private const int GRID_SIZE = 512;

		// Token: 0x04002242 RID: 8770
		private const int CELL_SIZE = 16;

		/// <summary>
		/// Number of Lists to preallocate in batches.
		/// (GRID_SIZE * GRID_SIZE) % LIST_POOL_SIZE should be zero leftover.
		/// Reduces constructor performance cost. (public issue #4209)
		/// </summary>
		// Token: 0x04002243 RID: 8771
		private const int LIST_POOL_SIZE = 1024;
	}
}
