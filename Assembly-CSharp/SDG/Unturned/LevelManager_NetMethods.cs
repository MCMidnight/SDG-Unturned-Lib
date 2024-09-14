using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001EE RID: 494
	[NetInvokableGeneratedClass(typeof(LevelManager))]
	public static class LevelManager_NetMethods
	{
		// Token: 0x06000EDA RID: 3802 RVA: 0x000339B4 File Offset: 0x00031BB4
		[NetInvokableGeneratedMethod("ReceiveArenaOrigin", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveArenaOrigin_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Vector3 newArenaCurrentCenter;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newArenaCurrentCenter, 13, 7);
			float newArenaCurrentRadius;
			SystemNetPakReaderEx.ReadFloat(reader, ref newArenaCurrentRadius);
			Vector3 newArenaOriginCenter;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newArenaOriginCenter, 13, 7);
			float newArenaOriginRadius;
			SystemNetPakReaderEx.ReadFloat(reader, ref newArenaOriginRadius);
			Vector3 newArenaTargetCenter;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newArenaTargetCenter, 13, 7);
			float newArenaTargetRadius;
			SystemNetPakReaderEx.ReadFloat(reader, ref newArenaTargetRadius);
			float newArenaCompactorSpeed;
			SystemNetPakReaderEx.ReadFloat(reader, ref newArenaCompactorSpeed);
			byte delay;
			SystemNetPakReaderEx.ReadUInt8(reader, ref delay);
			LevelManager.ReceiveArenaOrigin(newArenaCurrentCenter, newArenaCurrentRadius, newArenaOriginCenter, newArenaOriginRadius, newArenaTargetCenter, newArenaTargetRadius, newArenaCompactorSpeed, delay);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00033A28 File Offset: 0x00031C28
		[NetInvokableGeneratedMethod("ReceiveArenaOrigin", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveArenaOrigin_Write(NetPakWriter writer, Vector3 newArenaCurrentCenter, float newArenaCurrentRadius, Vector3 newArenaOriginCenter, float newArenaOriginRadius, Vector3 newArenaTargetCenter, float newArenaTargetRadius, float newArenaCompactorSpeed, byte delay)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, newArenaCurrentCenter, 13, 7);
			SystemNetPakWriterEx.WriteFloat(writer, newArenaCurrentRadius);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newArenaOriginCenter, 13, 7);
			SystemNetPakWriterEx.WriteFloat(writer, newArenaOriginRadius);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newArenaTargetCenter, 13, 7);
			SystemNetPakWriterEx.WriteFloat(writer, newArenaTargetRadius);
			SystemNetPakWriterEx.WriteFloat(writer, newArenaCompactorSpeed);
			SystemNetPakWriterEx.WriteUInt8(writer, delay);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00033A84 File Offset: 0x00031C84
		[NetInvokableGeneratedMethod("ReceiveArenaMessage", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveArenaMessage_Read(in ClientInvocationContext context)
		{
			EArenaMessage newArenaMessage;
			context.reader.ReadEnum(out newArenaMessage);
			LevelManager.ReceiveArenaMessage(newArenaMessage);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00033AA5 File Offset: 0x00031CA5
		[NetInvokableGeneratedMethod("ReceiveArenaMessage", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveArenaMessage_Write(NetPakWriter writer, EArenaMessage newArenaMessage)
		{
			writer.WriteEnum(newArenaMessage);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00033AB0 File Offset: 0x00031CB0
		[NetInvokableGeneratedMethod("ReceiveLevelNumber", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLevelNumber_Read(in ClientInvocationContext context)
		{
			byte newLevelNumber;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref newLevelNumber);
			LevelManager.ReceiveLevelNumber(newLevelNumber);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00033AD1 File Offset: 0x00031CD1
		[NetInvokableGeneratedMethod("ReceiveLevelNumber", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLevelNumber_Write(NetPakWriter writer, byte newLevelNumber)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newLevelNumber);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00033ADC File Offset: 0x00031CDC
		[NetInvokableGeneratedMethod("ReceiveLevelTimer", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLevelTimer_Read(in ClientInvocationContext context)
		{
			byte newTimerCount;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref newTimerCount);
			LevelManager.ReceiveLevelTimer(newTimerCount);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00033AFD File Offset: 0x00031CFD
		[NetInvokableGeneratedMethod("ReceiveLevelTimer", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLevelTimer_Write(NetPakWriter writer, byte newTimerCount)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newTimerCount);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00033B08 File Offset: 0x00031D08
		[NetInvokableGeneratedMethod("ReceiveAirdropState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAirdropState_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Vector3 state;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref state, 13, 7);
			Vector3 direction;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref direction, 9);
			float speed;
			SystemNetPakReaderEx.ReadFloat(reader, ref speed);
			float force;
			SystemNetPakReaderEx.ReadFloat(reader, ref force);
			float delay;
			SystemNetPakReaderEx.ReadFloat(reader, ref delay);
			LevelManager.ReceiveAirdropState(state, direction, speed, force, delay);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00033B57 File Offset: 0x00031D57
		[NetInvokableGeneratedMethod("ReceiveAirdropState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAirdropState_Write(NetPakWriter writer, Vector3 state, Vector3 direction, float speed, float force, float delay)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, state, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, direction, 9);
			SystemNetPakWriterEx.WriteFloat(writer, speed);
			SystemNetPakWriterEx.WriteFloat(writer, force);
			SystemNetPakWriterEx.WriteFloat(writer, delay);
		}
	}
}
