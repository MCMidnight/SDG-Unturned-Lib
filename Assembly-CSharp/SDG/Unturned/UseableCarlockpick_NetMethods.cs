using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200020C RID: 524
	[NetInvokableGeneratedClass(typeof(UseableCarlockpick))]
	public static class UseableCarlockpick_NetMethods
	{
		// Token: 0x06001036 RID: 4150 RVA: 0x00038978 File Offset: 0x00036B78
		[NetInvokableGeneratedMethod("ReceivePlayJimmy", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayJimmy_Read(in ClientInvocationContext context)
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
			UseableCarlockpick useableCarlockpick = obj as UseableCarlockpick;
			if (useableCarlockpick == null)
			{
				return;
			}
			useableCarlockpick.ReceivePlayJimmy();
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x000389B7 File Offset: 0x00036BB7
		[NetInvokableGeneratedMethod("ReceivePlayJimmy", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayJimmy_Write(NetPakWriter writer)
		{
		}
	}
}
