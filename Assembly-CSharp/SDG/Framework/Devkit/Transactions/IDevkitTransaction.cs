using System;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x0200013C RID: 316
	public interface IDevkitTransaction
	{
		/// <summary>
		/// If false this transaction is ignored. If there were no changes at all in the group it's discarded.
		/// </summary>
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000813 RID: 2067
		bool delta { get; }

		// Token: 0x06000814 RID: 2068
		void undo();

		// Token: 0x06000815 RID: 2069
		void redo();

		// Token: 0x06000816 RID: 2070
		void begin();

		// Token: 0x06000817 RID: 2071
		void end();

		/// <summary>
		/// Called when history buffer is too long so this transaction is discarded.
		/// </summary>
		// Token: 0x06000818 RID: 2072
		void forget();
	}
}
