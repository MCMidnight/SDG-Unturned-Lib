using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.Framework.Utilities;
using SDG.Unturned;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x02000134 RID: 308
	public class DevkitObjectDeltaTransaction : IDevkitTransaction
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x0001C4C0 File Offset: 0x0001A6C0
		public bool delta
		{
			get
			{
				return this.deltas.Count > 0;
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001C4D0 File Offset: 0x0001A6D0
		public void undo()
		{
			for (int i = 0; i < this.deltas.Count; i++)
			{
				this.deltas[i].undo(this.instance);
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001C50C File Offset: 0x0001A70C
		public void redo()
		{
			for (int i = 0; i < this.deltas.Count; i++)
			{
				this.deltas[i].redo(this.instance);
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001C548 File Offset: 0x0001A748
		public void begin()
		{
			this.tempFields = ListPool<object>.claim();
			this.tempProperties = ListPool<object>.claim();
			Type type = this.instance.GetType();
			FieldInfo[] fields = type.GetFields(20);
			for (int i = 0; i < fields.Length; i++)
			{
				try
				{
					object value = fields[i].GetValue(this.instance);
					this.tempFields.Add(value);
				}
				catch
				{
					this.tempFields.Add(null);
				}
			}
			PropertyInfo[] properties = type.GetProperties(20);
			for (int j = 0; j < properties.Length; j++)
			{
				try
				{
					PropertyInfo propertyInfo = properties[j];
					if (propertyInfo.CanRead && propertyInfo.CanWrite)
					{
						object value2 = propertyInfo.GetValue(this.instance, null);
						this.tempProperties.Add(value2);
					}
					else
					{
						this.tempProperties.Add(null);
					}
				}
				catch
				{
					this.tempProperties.Add(null);
				}
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001C64C File Offset: 0x0001A84C
		public void end()
		{
			this.deltas = ListPool<ITransactionDelta>.claim();
			Type type = this.instance.GetType();
			FieldInfo[] fields = type.GetFields(20);
			for (int i = 0; i < fields.Length; i++)
			{
				try
				{
					FieldInfo fieldInfo = fields[i];
					object value = fieldInfo.GetValue(this.instance);
					if (this.changed(this.tempFields[i], value))
					{
						this.deltas.Add(new TransactionFieldDelta(fieldInfo, this.tempFields[i], value));
					}
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e);
				}
			}
			PropertyInfo[] properties = type.GetProperties(20);
			for (int j = 0; j < properties.Length; j++)
			{
				try
				{
					PropertyInfo propertyInfo = properties[j];
					if (propertyInfo.CanRead && propertyInfo.CanWrite)
					{
						object value2 = propertyInfo.GetValue(this.instance, null);
						if (this.changed(this.tempProperties[j], value2))
						{
							this.deltas.Add(new TransactionPropertyDelta(propertyInfo, this.tempProperties[j], value2));
						}
					}
				}
				catch (Exception e2)
				{
					UnturnedLog.exception(e2);
				}
			}
			ListPool<object>.release(this.tempFields);
			ListPool<object>.release(this.tempProperties);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001C79C File Offset: 0x0001A99C
		public void forget()
		{
			if (this.deltas != null)
			{
				ListPool<ITransactionDelta>.release(this.deltas);
				this.deltas = null;
			}
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001C7B8 File Offset: 0x0001A9B8
		protected bool changed(object before, object after)
		{
			if (before == null || after == null)
			{
				return before != after;
			}
			return !before.Equals(after);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001C7D2 File Offset: 0x0001A9D2
		public DevkitObjectDeltaTransaction(object newInstance)
		{
			this.instance = newInstance;
		}

		// Token: 0x040002D1 RID: 721
		protected object instance;

		// Token: 0x040002D2 RID: 722
		protected List<object> tempFields;

		// Token: 0x040002D3 RID: 723
		protected List<object> tempProperties;

		// Token: 0x040002D4 RID: 724
		protected List<ITransactionDelta> deltas;
	}
}
