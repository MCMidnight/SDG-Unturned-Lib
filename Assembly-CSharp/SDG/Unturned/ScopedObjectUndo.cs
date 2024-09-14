using System;
using SDG.Framework.Devkit.Transactions;

namespace SDG.Unturned
{
	// Token: 0x02000419 RID: 1049
	public class ScopedObjectUndo : IDisposable
	{
		// Token: 0x06001ED0 RID: 7888 RVA: 0x000727C4 File Offset: 0x000709C4
		public ScopedObjectUndo(object modifiedObject)
		{
			DevkitTransactionUtility.beginGenericTransaction();
			DevkitTransactionUtility.recordObjectDelta(modifiedObject);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000727D7 File Offset: 0x000709D7
		public void Dispose()
		{
			DevkitTransactionUtility.endGenericTransaction();
		}
	}
}
