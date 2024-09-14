using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001F1 RID: 497
	[NetInvokableGeneratedClass(typeof(NPCEventManager))]
	public static class NPCEventManager_NetMethods
	{
		// Token: 0x06000EF2 RID: 3826 RVA: 0x00033DB8 File Offset: 0x00031FB8
		[NetInvokableGeneratedMethod("ReceiveBroadcast", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBroadcast_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte channelId;
			SystemNetPakReaderEx.ReadUInt8(reader, ref channelId);
			string eventId;
			SystemNetPakReaderEx.ReadString(reader, ref eventId, 11);
			NPCEventManager.ReceiveBroadcast(channelId, eventId);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00033DE5 File Offset: 0x00031FE5
		[NetInvokableGeneratedMethod("ReceiveBroadcast", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBroadcast_Write(NetPakWriter writer, byte channelId, string eventId)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, channelId);
			SystemNetPakWriterEx.WriteString(writer, eventId, 11);
		}
	}
}
