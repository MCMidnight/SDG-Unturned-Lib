using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200020E RID: 526
	[NetInvokableGeneratedClass(typeof(UseableConsumeable))]
	public static class UseableConsumeable_NetMethods
	{
		// Token: 0x0600103A RID: 4154 RVA: 0x00038A00 File Offset: 0x00036C00
		[NetInvokableGeneratedMethod("ReceivePlayConsume", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayConsume_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableConsumeable useableConsumeable = obj as UseableConsumeable;
			if (useableConsumeable == null)
			{
				return;
			}
			EConsumeMode mode;
			reader.ReadEnum(out mode);
			useableConsumeable.ReceivePlayConsume(mode);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00038A4C File Offset: 0x00036C4C
		[NetInvokableGeneratedMethod("ReceivePlayConsume", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayConsume_Write(NetPakWriter writer, EConsumeMode mode)
		{
			writer.WriteEnum(mode);
		}
	}
}
