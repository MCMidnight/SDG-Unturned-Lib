using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows foreach loop to iterate renderers defined in lod group.
	/// </summary>
	// Token: 0x02000808 RID: 2056
	public struct LodGroupEnumerator : IEnumerator<Renderer>, IEnumerator, IDisposable, IEnumerable<Renderer>, IEnumerable
	{
		// Token: 0x06004666 RID: 18022 RVA: 0x001A3E4E File Offset: 0x001A204E
		public LodGroupEnumerator(LODGroup lodGroup)
		{
			this.lods = lodGroup.GetLODs();
			this.lodIndex = 0;
			this.rendererIndex = -1;
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004667 RID: 18023 RVA: 0x001A3E6A File Offset: 0x001A206A
		public Renderer Current
		{
			get
			{
				return this.lods[this.lodIndex].renderers[this.rendererIndex];
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004668 RID: 18024 RVA: 0x001A3E89 File Offset: 0x001A2089
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x001A3E91 File Offset: 0x001A2091
		public void Dispose()
		{
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x001A3E94 File Offset: 0x001A2094
		private bool MoveRendererIndex()
		{
			Renderer[] renderers = this.lods[this.lodIndex].renderers;
			if (renderers == null || renderers.Length < 1)
			{
				return false;
			}
			do
			{
				int num = this.rendererIndex + 1;
				this.rendererIndex = num;
				if (num >= renderers.Length)
				{
					return false;
				}
			}
			while (renderers[this.rendererIndex] == null);
			return true;
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x001A3EEC File Offset: 0x001A20EC
		public bool MoveNext()
		{
			if (this.lods == null || this.lods.Length < 1)
			{
				return false;
			}
			if (this.MoveRendererIndex())
			{
				return true;
			}
			do
			{
				int num = this.lodIndex + 1;
				this.lodIndex = num;
				if (num >= this.lods.Length)
				{
					return false;
				}
				this.rendererIndex = -1;
			}
			while (!this.MoveRendererIndex());
			return true;
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x001A3F45 File Offset: 0x001A2145
		public void Reset()
		{
			this.lodIndex = 0;
			this.rendererIndex = -1;
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x001A3F55 File Offset: 0x001A2155
		IEnumerator<Renderer> IEnumerable<Renderer>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x001A3F62 File Offset: 0x001A2162
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x04002F89 RID: 12169
		private LOD[] lods;

		// Token: 0x04002F8A RID: 12170
		private int lodIndex;

		// Token: 0x04002F8B RID: 12171
		private int rendererIndex;
	}
}
