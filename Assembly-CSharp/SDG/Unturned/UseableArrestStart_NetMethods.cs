using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000209 RID: 521
	[NetInvokableGeneratedClass(typeof(UseableArrestStart))]
	public static class UseableArrestStart_NetMethods
	{
		// Token: 0x0600102C RID: 4140 RVA: 0x00038724 File Offset: 0x00036924
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
			UseableArrestStart useableArrestStart = obj as UseableArrestStart;
			if (useableArrestStart == null)
			{
				return;
			}
			useableArrestStart.ReceivePlayArrest();
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00038763 File Offset: 0x00036963
		[NetInvokableGeneratedMethod("ReceivePlayArrest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayArrest_Write(NetPakWriter writer)
		{
		}
	}
}
