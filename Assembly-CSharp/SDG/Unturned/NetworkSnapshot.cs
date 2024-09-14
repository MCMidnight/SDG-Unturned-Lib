using System;

namespace SDG.Unturned
{
	// Token: 0x020005E0 RID: 1504
	public struct NetworkSnapshot<T> where T : ISnapshotInfo<T>
	{
		// Token: 0x04001A44 RID: 6724
		public T info;

		// Token: 0x04001A45 RID: 6725
		public float timestamp;
	}
}
