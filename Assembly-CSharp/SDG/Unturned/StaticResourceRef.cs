using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Assets cannot be loaded from Resources during static initialization, so this reference defers the load until
	/// the first time user tries to use it.
	/// </summary>
	// Token: 0x0200036C RID: 876
	public class StaticResourceRef<T> where T : Object
	{
		// Token: 0x06001A88 RID: 6792 RVA: 0x0005FE44 File Offset: 0x0005E044
		public T GetOrLoad()
		{
			if (this.needsLoad)
			{
				this.needsLoad = false;
				this.asset = Resources.Load<T>(this.path);
				if (this.asset == null)
				{
					UnturnedLog.error("Missing resource {0} ({1})", new object[]
					{
						this.path,
						typeof(T)
					});
				}
			}
			return this.asset;
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x0005FEB0 File Offset: 0x0005E0B0
		public StaticResourceRef(string path)
		{
			this.path = path;
			this.asset = default(T);
			this.needsLoad = true;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0005FED2 File Offset: 0x0005E0D2
		public static implicit operator T(StaticResourceRef<T> resource)
		{
			return resource.GetOrLoad();
		}

		// Token: 0x04000C3A RID: 3130
		private string path;

		// Token: 0x04000C3B RID: 3131
		private T asset;

		// Token: 0x04000C3C RID: 3132
		private bool needsLoad;
	}
}
