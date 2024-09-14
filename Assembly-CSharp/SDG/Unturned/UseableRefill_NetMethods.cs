using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000217 RID: 535
	[NetInvokableGeneratedClass(typeof(UseableRefill))]
	public static class UseableRefill_NetMethods
	{
		// Token: 0x06001072 RID: 4210 RVA: 0x00039680 File Offset: 0x00037880
		[NetInvokableGeneratedMethod("ReceivePlayUse", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayUse_Read(in ClientInvocationContext context)
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
			UseableRefill useableRefill = obj as UseableRefill;
			if (useableRefill == null)
			{
				return;
			}
			useableRefill.ReceivePlayUse();
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x000396BF File Offset: 0x000378BF
		[NetInvokableGeneratedMethod("ReceivePlayUse", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayUse_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x000396C4 File Offset: 0x000378C4
		[NetInvokableGeneratedMethod("ReceivePlayRefill", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayRefill_Read(in ClientInvocationContext context)
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
			UseableRefill useableRefill = obj as UseableRefill;
			if (useableRefill == null)
			{
				return;
			}
			useableRefill.ReceivePlayRefill();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00039703 File Offset: 0x00037903
		[NetInvokableGeneratedMethod("ReceivePlayRefill", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayRefill_Write(NetPakWriter writer)
		{
		}
	}
}
