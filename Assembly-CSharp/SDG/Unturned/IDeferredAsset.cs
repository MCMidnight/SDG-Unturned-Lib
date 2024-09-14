using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Struct interface so that for transient asset bundles (older workshop mods) they can be preloaded
	/// and retrieved as-needed, but for master bundles the asset loading can be deferred until needed.
	/// </summary>
	// Token: 0x02000295 RID: 661
	public interface IDeferredAsset<T> where T : Object
	{
		// Token: 0x060013F5 RID: 5109
		T getOrLoad();
	}
}
