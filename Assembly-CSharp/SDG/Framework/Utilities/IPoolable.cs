using System;

namespace SDG.Framework.Utilities
{
	/// <summary>
	/// For use with PoolablePool when no special construction is required.
	/// </summary>
	// Token: 0x0200007C RID: 124
	public interface IPoolable
	{
		/// <summary>
		/// Called when this instance is getting claimed.
		/// </summary>
		// Token: 0x06000307 RID: 775
		void poolClaim();

		/// <summary>
		/// Called when this instance is returned to the pool.
		/// </summary>
		// Token: 0x06000308 RID: 776
		void poolRelease();
	}
}
