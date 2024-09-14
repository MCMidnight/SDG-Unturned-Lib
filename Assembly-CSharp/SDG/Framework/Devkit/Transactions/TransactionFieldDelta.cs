using System;
using System.Reflection;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x0200013E RID: 318
	public struct TransactionFieldDelta : ITransactionDelta
	{
		// Token: 0x0600081B RID: 2075 RVA: 0x0001CE16 File Offset: 0x0001B016
		public void undo(object instance)
		{
			this.field.SetValue(instance, this.before);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001CE2A File Offset: 0x0001B02A
		public void redo(object instance)
		{
			this.field.SetValue(instance, this.after);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001CE3E File Offset: 0x0001B03E
		public TransactionFieldDelta(FieldInfo newField)
		{
			this = new TransactionFieldDelta(newField, null, null);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001CE49 File Offset: 0x0001B049
		public TransactionFieldDelta(FieldInfo newField, object newBefore, object newAfter)
		{
			this.field = newField;
			this.before = newBefore;
			this.after = newAfter;
		}

		// Token: 0x040002E5 RID: 741
		public FieldInfo field;

		// Token: 0x040002E6 RID: 742
		public object before;

		// Token: 0x040002E7 RID: 743
		public object after;
	}
}
