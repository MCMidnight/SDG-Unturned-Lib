using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200020F RID: 527
	[NetInvokableGeneratedClass(typeof(UseableDetonator))]
	public static class UseableDetonator_NetMethods
	{
		// Token: 0x0600103C RID: 4156 RVA: 0x00038A58 File Offset: 0x00036C58
		[NetInvokableGeneratedMethod("ReceivePlayPlunge", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayPlunge_Read(in ClientInvocationContext context)
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
			UseableDetonator useableDetonator = obj as UseableDetonator;
			if (useableDetonator == null)
			{
				return;
			}
			useableDetonator.ReceivePlayPlunge();
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00038A97 File Offset: 0x00036C97
		[NetInvokableGeneratedMethod("ReceivePlayPlunge", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayPlunge_Write(NetPakWriter writer)
		{
		}
	}
}
