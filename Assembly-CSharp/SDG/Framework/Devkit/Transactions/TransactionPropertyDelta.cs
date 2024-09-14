using System;
using System.Reflection;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x0200013F RID: 319
	public struct TransactionPropertyDelta : ITransactionDelta
	{
		// Token: 0x0600081F RID: 2079 RVA: 0x0001CE60 File Offset: 0x0001B060
		public void undo(object instance)
		{
			this.property.SetValue(instance, this.before, null);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001CE75 File Offset: 0x0001B075
		public void redo(object instance)
		{
			this.property.SetValue(instance, this.after, null);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001CE8A File Offset: 0x0001B08A
		public TransactionPropertyDelta(PropertyInfo newProperty)
		{
			this = new TransactionPropertyDelta(newProperty, null, null);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001CE95 File Offset: 0x0001B095
		public TransactionPropertyDelta(PropertyInfo newProperty, object newBefore, object newAfter)
		{
			this.property = newProperty;
			this.before = newBefore;
			this.after = newAfter;
		}

		// Token: 0x040002E8 RID: 744
		public PropertyInfo property;

		// Token: 0x040002E9 RID: 745
		public object before;

		// Token: 0x040002EA RID: 746
		public object after;
	}
}
