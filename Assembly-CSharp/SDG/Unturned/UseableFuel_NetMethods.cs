using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000212 RID: 530
	[NetInvokableGeneratedClass(typeof(UseableFuel))]
	public static class UseableFuel_NetMethods
	{
		// Token: 0x06001048 RID: 4168 RVA: 0x00038C24 File Offset: 0x00036E24
		[NetInvokableGeneratedMethod("ReceivePlayGlug", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayGlug_Read(in ClientInvocationContext context)
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
			UseableFuel useableFuel = obj as UseableFuel;
			if (useableFuel == null)
			{
				return;
			}
			useableFuel.ReceivePlayGlug();
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00038C63 File Offset: 0x00036E63
		[NetInvokableGeneratedMethod("ReceivePlayGlug", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayGlug_Write(NetPakWriter writer)
		{
		}
	}
}
