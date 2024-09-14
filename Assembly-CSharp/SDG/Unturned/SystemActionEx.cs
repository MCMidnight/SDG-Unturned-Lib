using System;

namespace SDG.Unturned
{
	// Token: 0x0200081C RID: 2076
	public static class SystemActionEx
	{
		// Token: 0x060046E7 RID: 18151 RVA: 0x001A7C54 File Offset: 0x001A5E54
		public static void TryInvoke(this Action action, string debugName)
		{
			try
			{
				if (action != null)
				{
					action.Invoke();
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception invoking {0}:", new object[]
				{
					debugName
				});
			}
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x001A7C94 File Offset: 0x001A5E94
		public static void TryInvoke<T>(this Action<T> action, string debugName, T obj)
		{
			try
			{
				if (action != null)
				{
					action.Invoke(obj);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception invoking {0}({1}):", new object[]
				{
					debugName,
					obj
				});
			}
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x001A7CDC File Offset: 0x001A5EDC
		public static void TryInvoke<T1, T2>(this Action<T1, T2> action, string debugName, T1 arg1, T2 arg2)
		{
			try
			{
				if (action != null)
				{
					action.Invoke(arg1, arg2);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception invoking {0}({1}, {2}):", new object[]
				{
					debugName,
					arg1,
					arg2
				});
			}
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x001A7D30 File Offset: 0x001A5F30
		public static void TryInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, string debugName, T1 arg1, T2 arg2, T3 arg3)
		{
			try
			{
				if (action != null)
				{
					action.Invoke(arg1, arg2, arg3);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception invoking {0}({1}, {2}, {3}):", new object[]
				{
					debugName,
					arg1,
					arg2,
					arg3
				});
			}
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x001A7D90 File Offset: 0x001A5F90
		public static void TryInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, string debugName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			try
			{
				if (action != null)
				{
					action.Invoke(arg1, arg2, arg3, arg4);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception invoking {0}({1}, {2}, {3}, {4}):", new object[]
				{
					debugName,
					arg1,
					arg2,
					arg3,
					arg4
				});
			}
		}
	}
}
