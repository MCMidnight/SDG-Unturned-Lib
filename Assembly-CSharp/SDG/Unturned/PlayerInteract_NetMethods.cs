using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001F9 RID: 505
	[NetInvokableGeneratedClass(typeof(PlayerInteract))]
	public static class PlayerInteract_NetMethods
	{
		// Token: 0x06000F58 RID: 3928 RVA: 0x00035784 File Offset: 0x00033984
		[NetInvokableGeneratedMethod("ReceiveSalvageTimeOverride", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSalvageTimeOverride_Read(in ClientInvocationContext context)
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
			PlayerInteract playerInteract = obj as PlayerInteract;
			if (playerInteract == null)
			{
				return;
			}
			float overrideValue;
			SystemNetPakReaderEx.ReadFloat(reader, ref overrideValue);
			playerInteract.ReceiveSalvageTimeOverride(overrideValue);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x000357D0 File Offset: 0x000339D0
		[NetInvokableGeneratedMethod("ReceiveSalvageTimeOverride", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSalvageTimeOverride_Write(NetPakWriter writer, float overrideValue)
		{
			SystemNetPakWriterEx.WriteFloat(writer, overrideValue);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x000357DC File Offset: 0x000339DC
		[NetInvokableGeneratedMethod("ReceiveInspectRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveInspectRequest_Read(in ServerInvocationContext context)
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
			PlayerInteract playerInteract = obj as PlayerInteract;
			if (playerInteract == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerInteract.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerInteract));
				return;
			}
			playerInteract.ReceiveInspectRequest();
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0003583B File Offset: 0x00033A3B
		[NetInvokableGeneratedMethod("ReceiveInspectRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveInspectRequest_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00035840 File Offset: 0x00033A40
		[NetInvokableGeneratedMethod("ReceivePlayInspect", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayInspect_Read(in ClientInvocationContext context)
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
			PlayerInteract playerInteract = obj as PlayerInteract;
			if (playerInteract == null)
			{
				return;
			}
			playerInteract.ReceivePlayInspect();
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0003587F File Offset: 0x00033A7F
		[NetInvokableGeneratedMethod("ReceivePlayInspect", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayInspect_Write(NetPakWriter writer)
		{
		}
	}
}
