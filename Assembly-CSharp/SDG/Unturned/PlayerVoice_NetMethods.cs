using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000202 RID: 514
	[NetInvokableGeneratedClass(typeof(PlayerVoice))]
	public static class PlayerVoice_NetMethods
	{
		// Token: 0x06000FF0 RID: 4080 RVA: 0x00037A7C File Offset: 0x00035C7C
		[NetInvokableGeneratedMethod("ReceivePermissions", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePermissions_Read(in ClientInvocationContext context)
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
			PlayerVoice playerVoice = obj as PlayerVoice;
			if (playerVoice == null)
			{
				return;
			}
			bool allowTalkingWhileDead;
			reader.ReadBit(ref allowTalkingWhileDead);
			bool customAllowTalking;
			reader.ReadBit(ref customAllowTalking);
			playerVoice.ReceivePermissions(allowTalkingWhileDead, customAllowTalking);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00037AD3 File Offset: 0x00035CD3
		[NetInvokableGeneratedMethod("ReceivePermissions", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePermissions_Write(NetPakWriter writer, bool allowTalkingWhileDead, bool customAllowTalking)
		{
			writer.WriteBit(allowTalkingWhileDead);
			writer.WriteBit(customAllowTalking);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00037AE8 File Offset: 0x00035CE8
		[NetInvokableGeneratedMethod("ReceiveVoiceChatRelay", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVoiceChatRelay_Read(in ServerInvocationContext context)
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
			PlayerVoice playerVoice = obj as PlayerVoice;
			if (playerVoice == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerVoice.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerVoice));
				return;
			}
			playerVoice.ReceiveVoiceChatRelay(context);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00037B48 File Offset: 0x00035D48
		[NetInvokableGeneratedMethod("ReceivePlayVoiceChat", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayVoiceChat_Read(in ClientInvocationContext context)
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
			PlayerVoice playerVoice = obj as PlayerVoice;
			if (playerVoice == null)
			{
				return;
			}
			playerVoice.ReceivePlayVoiceChat(context);
		}
	}
}
