using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200021A RID: 538
	[NetInvokableGeneratedClass(typeof(UseableTire))]
	public static class UseableTire_NetMethods
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x0003989C File Offset: 0x00037A9C
		[NetInvokableGeneratedMethod("ReceivePlayAttach", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayAttach_Read(in ClientInvocationContext context)
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
			UseableTire useableTire = obj as UseableTire;
			if (useableTire == null)
			{
				return;
			}
			useableTire.ReceivePlayAttach();
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x000398DB File Offset: 0x00037ADB
		[NetInvokableGeneratedMethod("ReceivePlayAttach", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayAttach_Write(NetPakWriter writer)
		{
		}
	}
}
