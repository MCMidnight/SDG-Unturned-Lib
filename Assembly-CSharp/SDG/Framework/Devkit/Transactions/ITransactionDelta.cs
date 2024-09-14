using System;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x0200013D RID: 317
	public interface ITransactionDelta
	{
		// Token: 0x06000819 RID: 2073
		void undo(object instance);

		// Token: 0x0600081A RID: 2074
		void redo(object instance);
	}
}
