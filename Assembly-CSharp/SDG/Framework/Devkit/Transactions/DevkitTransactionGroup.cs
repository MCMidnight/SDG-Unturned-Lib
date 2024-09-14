using System;
using System.Collections.Generic;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x02000135 RID: 309
	public class DevkitTransactionGroup
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0001C7E1 File Offset: 0x0001A9E1
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0001C7E9 File Offset: 0x0001A9E9
		public string name { get; protected set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0001C7F2 File Offset: 0x0001A9F2
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0001C7FA File Offset: 0x0001A9FA
		public List<IDevkitTransaction> transactions { get; protected set; }

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001C803 File Offset: 0x0001AA03
		public void record(IDevkitTransaction transaction)
		{
			transaction.begin();
			this.transactions.Add(transaction);
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001C818 File Offset: 0x0001AA18
		public bool delta
		{
			get
			{
				for (int i = this.transactions.Count - 1; i >= 0; i--)
				{
					if (!this.transactions[i].delta)
					{
						this.transactions.RemoveAt(i);
					}
				}
				return this.transactions.Count > 0;
			}
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001C86C File Offset: 0x0001AA6C
		public void undo()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].undo();
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
		public void redo()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].redo();
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
		public void end()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].end();
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001C908 File Offset: 0x0001AB08
		public void forget()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].forget();
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001C93C File Offset: 0x0001AB3C
		public DevkitTransactionGroup(string newName)
		{
			this.name = newName;
			this.transactions = new List<IDevkitTransaction>();
		}
	}
}
