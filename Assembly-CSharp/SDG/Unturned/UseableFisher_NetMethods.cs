using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000211 RID: 529
	[NetInvokableGeneratedClass(typeof(UseableFisher))]
	public static class UseableFisher_NetMethods
	{
		// Token: 0x06001040 RID: 4160 RVA: 0x00038AE0 File Offset: 0x00036CE0
		[NetInvokableGeneratedMethod("ReceiveCatch", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveCatch_Read(in ServerInvocationContext context)
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
			UseableFisher useableFisher = obj as UseableFisher;
			if (useableFisher == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableFisher.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableFisher));
				return;
			}
			useableFisher.ReceiveCatch();
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00038B3F File Offset: 0x00036D3F
		[NetInvokableGeneratedMethod("ReceiveCatch", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveCatch_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00038B44 File Offset: 0x00036D44
		[NetInvokableGeneratedMethod("ReceiveLuckTime", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLuckTime_Read(in ClientInvocationContext context)
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
			UseableFisher useableFisher = obj as UseableFisher;
			if (useableFisher == null)
			{
				return;
			}
			float newLuckTime;
			SystemNetPakReaderEx.ReadFloat(reader, ref newLuckTime);
			useableFisher.ReceiveLuckTime(newLuckTime);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00038B90 File Offset: 0x00036D90
		[NetInvokableGeneratedMethod("ReceiveLuckTime", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLuckTime_Write(NetPakWriter writer, float NewLuckTime)
		{
			SystemNetPakWriterEx.WriteFloat(writer, NewLuckTime);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00038B9C File Offset: 0x00036D9C
		[NetInvokableGeneratedMethod("ReceivePlayReel", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayReel_Read(in ClientInvocationContext context)
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
			UseableFisher useableFisher = obj as UseableFisher;
			if (useableFisher == null)
			{
				return;
			}
			useableFisher.ReceivePlayReel();
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00038BDB File Offset: 0x00036DDB
		[NetInvokableGeneratedMethod("ReceivePlayReel", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayReel_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00038BE0 File Offset: 0x00036DE0
		[NetInvokableGeneratedMethod("ReceivePlayCast", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayCast_Read(in ClientInvocationContext context)
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
			UseableFisher useableFisher = obj as UseableFisher;
			if (useableFisher == null)
			{
				return;
			}
			useableFisher.ReceivePlayCast();
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00038C1F File Offset: 0x00036E1F
		[NetInvokableGeneratedMethod("ReceivePlayCast", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayCast_Write(NetPakWriter writer)
		{
		}
	}
}
