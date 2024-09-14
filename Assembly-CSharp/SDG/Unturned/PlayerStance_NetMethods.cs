using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000201 RID: 513
	[NetInvokableGeneratedClass(typeof(PlayerStance))]
	public static class PlayerStance_NetMethods
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x000379A8 File Offset: 0x00035BA8
		[NetInvokableGeneratedMethod("ReceiveClimbRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClimbRequest_Read(in ServerInvocationContext context)
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
			PlayerStance playerStance = obj as PlayerStance;
			if (playerStance == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerStance.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerStance));
				return;
			}
			Vector3 direction;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref direction, 9);
			playerStance.ReceiveClimbRequest(context, direction);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00037A17 File Offset: 0x00035C17
		[NetInvokableGeneratedMethod("ReceiveClimbRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveClimbRequest_Write(NetPakWriter writer, Vector3 direction)
		{
			UnityNetPakWriterEx.WriteNormalVector3(writer, direction, 9);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00037A24 File Offset: 0x00035C24
		[NetInvokableGeneratedMethod("ReceiveStance", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveStance_Read(in ClientInvocationContext context)
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
			PlayerStance playerStance = obj as PlayerStance;
			if (playerStance == null)
			{
				return;
			}
			EPlayerStance newStance;
			reader.ReadEnum(out newStance);
			playerStance.ReceiveStance(newStance);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00037A70 File Offset: 0x00035C70
		[NetInvokableGeneratedMethod("ReceiveStance", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveStance_Write(NetPakWriter writer, EPlayerStance newStance)
		{
			writer.WriteEnum(newStance);
		}
	}
}
