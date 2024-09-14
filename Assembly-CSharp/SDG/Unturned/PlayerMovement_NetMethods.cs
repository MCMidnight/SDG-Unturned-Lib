using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001FE RID: 510
	[NetInvokableGeneratedClass(typeof(PlayerMovement))]
	public static class PlayerMovement_NetMethods
	{
		// Token: 0x06000F98 RID: 3992 RVA: 0x000366D8 File Offset: 0x000348D8
		[NetInvokableGeneratedMethod("ReceivePluginGravityMultiplier", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePluginGravityMultiplier_Read(in ClientInvocationContext context)
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
			PlayerMovement playerMovement = obj as PlayerMovement;
			if (playerMovement == null)
			{
				return;
			}
			float newPluginGravityMultiplier;
			SystemNetPakReaderEx.ReadFloat(reader, ref newPluginGravityMultiplier);
			playerMovement.ReceivePluginGravityMultiplier(newPluginGravityMultiplier);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00036724 File Offset: 0x00034924
		[NetInvokableGeneratedMethod("ReceivePluginGravityMultiplier", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePluginGravityMultiplier_Write(NetPakWriter writer, float newPluginGravityMultiplier)
		{
			SystemNetPakWriterEx.WriteFloat(writer, newPluginGravityMultiplier);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00036730 File Offset: 0x00034930
		[NetInvokableGeneratedMethod("ReceivePluginJumpMultiplier", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePluginJumpMultiplier_Read(in ClientInvocationContext context)
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
			PlayerMovement playerMovement = obj as PlayerMovement;
			if (playerMovement == null)
			{
				return;
			}
			float newPluginJumpMultiplier;
			SystemNetPakReaderEx.ReadFloat(reader, ref newPluginJumpMultiplier);
			playerMovement.ReceivePluginJumpMultiplier(newPluginJumpMultiplier);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0003677C File Offset: 0x0003497C
		[NetInvokableGeneratedMethod("ReceivePluginJumpMultiplier", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePluginJumpMultiplier_Write(NetPakWriter writer, float newPluginJumpMultiplier)
		{
			SystemNetPakWriterEx.WriteFloat(writer, newPluginJumpMultiplier);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00036788 File Offset: 0x00034988
		[NetInvokableGeneratedMethod("ReceivePluginSpeedMultiplier", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePluginSpeedMultiplier_Read(in ClientInvocationContext context)
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
			PlayerMovement playerMovement = obj as PlayerMovement;
			if (playerMovement == null)
			{
				return;
			}
			float newPluginSpeedMultiplier;
			SystemNetPakReaderEx.ReadFloat(reader, ref newPluginSpeedMultiplier);
			playerMovement.ReceivePluginSpeedMultiplier(newPluginSpeedMultiplier);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x000367D4 File Offset: 0x000349D4
		[NetInvokableGeneratedMethod("ReceivePluginSpeedMultiplier", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePluginSpeedMultiplier_Write(NetPakWriter writer, float newPluginSpeedMultiplier)
		{
			SystemNetPakWriterEx.WriteFloat(writer, newPluginSpeedMultiplier);
		}
	}
}
