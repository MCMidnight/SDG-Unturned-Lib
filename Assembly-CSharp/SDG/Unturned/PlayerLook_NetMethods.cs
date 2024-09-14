using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001FC RID: 508
	[NetInvokableGeneratedClass(typeof(PlayerLook))]
	public static class PlayerLook_NetMethods
	{
		// Token: 0x06000F92 RID: 3986 RVA: 0x000365D0 File Offset: 0x000347D0
		[NetInvokableGeneratedMethod("ReceiveFreecamAllowed", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveFreecamAllowed_Read(in ClientInvocationContext context)
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
			PlayerLook playerLook = obj as PlayerLook;
			if (playerLook == null)
			{
				return;
			}
			bool isAllowed;
			reader.ReadBit(ref isAllowed);
			playerLook.ReceiveFreecamAllowed(isAllowed);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0003661C File Offset: 0x0003481C
		[NetInvokableGeneratedMethod("ReceiveFreecamAllowed", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveFreecamAllowed_Write(NetPakWriter writer, bool isAllowed)
		{
			writer.WriteBit(isAllowed);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00036628 File Offset: 0x00034828
		[NetInvokableGeneratedMethod("ReceiveWorkzoneAllowed", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWorkzoneAllowed_Read(in ClientInvocationContext context)
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
			PlayerLook playerLook = obj as PlayerLook;
			if (playerLook == null)
			{
				return;
			}
			bool isAllowed;
			reader.ReadBit(ref isAllowed);
			playerLook.ReceiveWorkzoneAllowed(isAllowed);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00036674 File Offset: 0x00034874
		[NetInvokableGeneratedMethod("ReceiveWorkzoneAllowed", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWorkzoneAllowed_Write(NetPakWriter writer, bool isAllowed)
		{
			writer.WriteBit(isAllowed);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00036680 File Offset: 0x00034880
		[NetInvokableGeneratedMethod("ReceiveSpecStatsAllowed", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSpecStatsAllowed_Read(in ClientInvocationContext context)
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
			PlayerLook playerLook = obj as PlayerLook;
			if (playerLook == null)
			{
				return;
			}
			bool isAllowed;
			reader.ReadBit(ref isAllowed);
			playerLook.ReceiveSpecStatsAllowed(isAllowed);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x000366CC File Offset: 0x000348CC
		[NetInvokableGeneratedMethod("ReceiveSpecStatsAllowed", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSpecStatsAllowed_Write(NetPakWriter writer, bool isAllowed)
		{
			writer.WriteBit(isAllowed);
		}
	}
}
