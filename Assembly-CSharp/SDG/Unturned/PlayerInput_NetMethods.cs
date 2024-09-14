using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001F8 RID: 504
	[NetInvokableGeneratedClass(typeof(PlayerInput))]
	public static class PlayerInput_NetMethods
	{
		// Token: 0x06000F53 RID: 3923 RVA: 0x000355E8 File Offset: 0x000337E8
		[NetInvokableGeneratedMethod("ReceiveSimulateMispredictedInputs", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSimulateMispredictedInputs_Read(in ClientInvocationContext context)
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
			PlayerInput playerInput = obj as PlayerInput;
			if (playerInput == null)
			{
				return;
			}
			uint frameNumber;
			SystemNetPakReaderEx.ReadUInt32(reader, ref frameNumber);
			EPlayerStance stance;
			reader.ReadEnum(out stance);
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			Vector3 velocity;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref velocity, 13, 7);
			byte stamina;
			SystemNetPakReaderEx.ReadUInt8(reader, ref stamina);
			int lastTireOffset;
			SystemNetPakReaderEx.ReadInt32(reader, ref lastTireOffset);
			int lastRestOffset;
			SystemNetPakReaderEx.ReadInt32(reader, ref lastRestOffset);
			playerInput.ReceiveSimulateMispredictedInputs(frameNumber, stance, position, velocity, stamina, lastTireOffset, lastRestOffset);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0003567C File Offset: 0x0003387C
		[NetInvokableGeneratedMethod("ReceiveSimulateMispredictedInputs", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSimulateMispredictedInputs_Write(NetPakWriter writer, uint frameNumber, EPlayerStance stance, Vector3 position, Vector3 velocity, byte stamina, int lastTireOffset, int lastRestOffset)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, frameNumber);
			writer.WriteEnum(stance);
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			UnityNetPakWriterEx.WriteClampedVector3(writer, velocity, 13, 7);
			SystemNetPakWriterEx.WriteUInt8(writer, stamina);
			SystemNetPakWriterEx.WriteInt32(writer, lastTireOffset);
			SystemNetPakWriterEx.WriteInt32(writer, lastRestOffset);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x000356CC File Offset: 0x000338CC
		[NetInvokableGeneratedMethod("ReceiveAckGoodInputs", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAckGoodInputs_Read(in ClientInvocationContext context)
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
			PlayerInput playerInput = obj as PlayerInput;
			if (playerInput == null)
			{
				return;
			}
			uint frameNumber;
			SystemNetPakReaderEx.ReadUInt32(reader, ref frameNumber);
			playerInput.ReceiveAckGoodInputs(frameNumber);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00035718 File Offset: 0x00033918
		[NetInvokableGeneratedMethod("ReceiveAckGoodInputs", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAckGoodInputs_Write(NetPakWriter writer, uint frameNumber)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, frameNumber);
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00035724 File Offset: 0x00033924
		[NetInvokableGeneratedMethod("ReceiveInputs", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveInputs_Read(in ServerInvocationContext context)
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
			PlayerInput playerInput = obj as PlayerInput;
			if (playerInput == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerInput.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerInput));
				return;
			}
			playerInput.ReceiveInputs(context);
		}
	}
}
