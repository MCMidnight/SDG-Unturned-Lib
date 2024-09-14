using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200020D RID: 525
	[NetInvokableGeneratedClass(typeof(UseableClothing))]
	public static class UseableClothing_NetMethods
	{
		// Token: 0x06001038 RID: 4152 RVA: 0x000389BC File Offset: 0x00036BBC
		[NetInvokableGeneratedMethod("ReceivePlayWear", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayWear_Read(in ClientInvocationContext context)
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
			UseableClothing useableClothing = obj as UseableClothing;
			if (useableClothing == null)
			{
				return;
			}
			useableClothing.ReceivePlayWear();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x000389FB File Offset: 0x00036BFB
		[NetInvokableGeneratedMethod("ReceivePlayWear", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayWear_Write(NetPakWriter writer)
		{
		}
	}
}
