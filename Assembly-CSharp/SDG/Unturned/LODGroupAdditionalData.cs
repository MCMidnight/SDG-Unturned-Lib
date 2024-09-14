using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Optional Unturned extensions to the LOD Group component.
	/// </summary>
	// Token: 0x020005CD RID: 1485
	[AddComponentMenu("Unturned/LOD Group Additional Data")]
	[RequireComponent(typeof(LODGroup))]
	public class LODGroupAdditionalData : MonoBehaviour
	{
		// Token: 0x06002FF7 RID: 12279 RVA: 0x000D3FEC File Offset: 0x000D21EC
		private void Start()
		{
			LODGroupManager.Get().Register(this);
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000D3FF9 File Offset: 0x000D21F9
		private void OnDestroy()
		{
			LODGroupManager.Get().Unregister(this);
		}

		// Token: 0x040019F4 RID: 6644
		public LODGroupAdditionalData.ELODBiasOverride LODBiasOverride = LODGroupAdditionalData.ELODBiasOverride.IgnoreLODBias;

		/// <summary>
		/// Could be extended, e.g. to clamp cull size separately from the per-LOD sizes.
		/// </summary>
		// Token: 0x02000999 RID: 2457
		public enum ELODBiasOverride
		{
			// Token: 0x040033C8 RID: 13256
			None,
			/// <summary>
			/// Unturned will adjust per-LOD sizes to counteract LOD bias.
			/// Elver has carefully tuned LOD sizes for the interior of the mall, so LOD bias affecting them is undesirable.
			/// Note that due to a Unity bug only LOD0 can be greater than 100%.
			/// </summary>
			// Token: 0x040033C9 RID: 13257
			IgnoreLODBias
		}
	}
}
