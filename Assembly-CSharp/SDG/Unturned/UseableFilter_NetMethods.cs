using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000210 RID: 528
	[NetInvokableGeneratedClass(typeof(UseableFilter))]
	public static class UseableFilter_NetMethods
	{
		// Token: 0x0600103E RID: 4158 RVA: 0x00038A9C File Offset: 0x00036C9C
		[NetInvokableGeneratedMethod("ReceivePlayFilter", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayFilter_Read(in ClientInvocationContext context)
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
			UseableFilter useableFilter = obj as UseableFilter;
			if (useableFilter == null)
			{
				return;
			}
			useableFilter.ReceivePlayFilter();
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x00038ADB File Offset: 0x00036CDB
		[NetInvokableGeneratedMethod("ReceivePlayFilter", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayFilter_Write(NetPakWriter writer)
		{
		}
	}
}
