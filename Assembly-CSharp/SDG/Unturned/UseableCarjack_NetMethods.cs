using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200020B RID: 523
	[NetInvokableGeneratedClass(typeof(UseableCarjack))]
	public static class UseableCarjack_NetMethods
	{
		// Token: 0x06001034 RID: 4148 RVA: 0x00038934 File Offset: 0x00036B34
		[NetInvokableGeneratedMethod("ReceivePlayPull", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayPull_Read(in ClientInvocationContext context)
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
			UseableCarjack useableCarjack = obj as UseableCarjack;
			if (useableCarjack == null)
			{
				return;
			}
			useableCarjack.ReceivePlayPull();
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00038973 File Offset: 0x00036B73
		[NetInvokableGeneratedMethod("ReceivePlayPull", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayPull_Write(NetPakWriter writer)
		{
		}
	}
}
