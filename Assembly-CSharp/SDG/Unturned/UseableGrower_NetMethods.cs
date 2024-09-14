using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000213 RID: 531
	[NetInvokableGeneratedClass(typeof(UseableGrower))]
	public static class UseableGrower_NetMethods
	{
		// Token: 0x0600104A RID: 4170 RVA: 0x00038C68 File Offset: 0x00036E68
		[NetInvokableGeneratedMethod("ReceivePlayGrow", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayGrow_Read(in ClientInvocationContext context)
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
			UseableGrower useableGrower = obj as UseableGrower;
			if (useableGrower == null)
			{
				return;
			}
			useableGrower.ReceivePlayGrow();
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x00038CA7 File Offset: 0x00036EA7
		[NetInvokableGeneratedMethod("ReceivePlayGrow", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayGrow_Write(NetPakWriter writer)
		{
		}
	}
}
