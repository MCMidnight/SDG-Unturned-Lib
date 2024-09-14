using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000208 RID: 520
	[NetInvokableGeneratedClass(typeof(UseableArrestEnd))]
	public static class UseableArrestEnd_NetMethods
	{
		// Token: 0x0600102A RID: 4138 RVA: 0x000386E0 File Offset: 0x000368E0
		[NetInvokableGeneratedMethod("ReceivePlayArrest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayArrest_Read(in ClientInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableArrestEnd useableArrestEnd = obj as UseableArrestEnd;
			if (useableArrestEnd == null)
			{
				return;
			}
			useableArrestEnd.ReceivePlayArrest();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0003871F File Offset: 0x0003691F
		[NetInvokableGeneratedMethod("ReceivePlayArrest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayArrest_Write(NetPakWriter writer)
		{
		}
	}
}
