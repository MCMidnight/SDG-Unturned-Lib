using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001F4 RID: 500
	[NetInvokableGeneratedClass(typeof(PlayerAnimator))]
	public static class PlayerAnimator_NetMethods
	{
		// Token: 0x06000F04 RID: 3844 RVA: 0x000340C4 File Offset: 0x000322C4
		[NetInvokableGeneratedMethod("ReceiveLean", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLean_Read(in ClientInvocationContext context)
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
			PlayerAnimator playerAnimator = obj as PlayerAnimator;
			if (playerAnimator == null)
			{
				return;
			}
			byte newLean;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newLean);
			playerAnimator.ReceiveLean(newLean);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00034110 File Offset: 0x00032310
		[NetInvokableGeneratedMethod("ReceiveLean", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLean_Write(NetPakWriter writer, byte newLean)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newLean);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0003411C File Offset: 0x0003231C
		[NetInvokableGeneratedMethod("ReceiveGesture", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveGesture_Read(in ClientInvocationContext context)
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
			PlayerAnimator playerAnimator = obj as PlayerAnimator;
			if (playerAnimator == null)
			{
				return;
			}
			EPlayerGesture newGesture;
			reader.ReadEnum(out newGesture);
			playerAnimator.ReceiveGesture(newGesture);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00034168 File Offset: 0x00032368
		[NetInvokableGeneratedMethod("ReceiveGesture", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveGesture_Write(NetPakWriter writer, EPlayerGesture newGesture)
		{
			writer.WriteEnum(newGesture);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00034174 File Offset: 0x00032374
		[NetInvokableGeneratedMethod("ReceiveGestureRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveGestureRequest_Read(in ServerInvocationContext context)
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
			PlayerAnimator playerAnimator = obj as PlayerAnimator;
			if (playerAnimator == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerAnimator.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerAnimator));
				return;
			}
			EPlayerGesture newGesture;
			reader.ReadEnum(out newGesture);
			playerAnimator.ReceiveGestureRequest(newGesture);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x000341E0 File Offset: 0x000323E0
		[NetInvokableGeneratedMethod("ReceiveGestureRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveGestureRequest_Write(NetPakWriter writer, EPlayerGesture newGesture)
		{
			writer.WriteEnum(newGesture);
		}
	}
}
