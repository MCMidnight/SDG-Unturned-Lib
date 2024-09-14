using System;

namespace SDG.Unturned
{
	// Token: 0x020005DC RID: 1500
	public interface ISnapshotInfo<T>
	{
		// Token: 0x0600302A RID: 12330
		void lerp(T target, float delta, out T result);
	}
}
