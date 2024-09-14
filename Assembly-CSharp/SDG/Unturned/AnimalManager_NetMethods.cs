using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001D1 RID: 465
	[NetInvokableGeneratedClass(typeof(AnimalManager))]
	public static class AnimalManager_NetMethods
	{
		// Token: 0x06000E06 RID: 3590 RVA: 0x00030FCC File Offset: 0x0002F1CC
		[NetInvokableGeneratedMethod("ReceiveAnimalAlive", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAnimalAlive_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			Vector3 newPosition;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPosition, 13, 7);
			byte newAngle;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newAngle);
			AnimalManager.ReceiveAnimalAlive(index, newPosition, newAngle);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00031004 File Offset: 0x0002F204
		[NetInvokableGeneratedMethod("ReceiveAnimalAlive", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAnimalAlive_Write(NetPakWriter writer, ushort index, Vector3 newPosition, byte newAngle)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, index);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newPosition, 13, 7);
			SystemNetPakWriterEx.WriteUInt8(writer, newAngle);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00031024 File Offset: 0x0002F224
		[NetInvokableGeneratedMethod("ReceiveAnimalDead", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAnimalDead_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			Vector3 newRagdoll;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newRagdoll, 13, 7);
			ERagdollEffect newRagdollEffect;
			reader.ReadEnum(out newRagdollEffect);
			AnimalManager.ReceiveAnimalDead(index, newRagdoll, newRagdollEffect);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0003105C File Offset: 0x0002F25C
		[NetInvokableGeneratedMethod("ReceiveAnimalDead", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAnimalDead_Write(NetPakWriter writer, ushort index, Vector3 newRagdoll, ERagdollEffect newRagdollEffect)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, index);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newRagdoll, 13, 7);
			writer.WriteEnum(newRagdollEffect);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0003107C File Offset: 0x0002F27C
		[NetInvokableGeneratedMethod("ReceiveAnimalStartle", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAnimalStartle_Read(in ClientInvocationContext context)
		{
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref index);
			AnimalManager.ReceiveAnimalStartle(index);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0003109D File Offset: 0x0002F29D
		[NetInvokableGeneratedMethod("ReceiveAnimalStartle", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAnimalStartle_Write(NetPakWriter writer, ushort index)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, index);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000310A8 File Offset: 0x0002F2A8
		[NetInvokableGeneratedMethod("ReceiveAnimalAttack", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAnimalAttack_Read(in ClientInvocationContext context)
		{
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref index);
			AnimalManager.ReceiveAnimalAttack(index);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x000310C9 File Offset: 0x0002F2C9
		[NetInvokableGeneratedMethod("ReceiveAnimalAttack", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAnimalAttack_Write(NetPakWriter writer, ushort index)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, index);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000310D4 File Offset: 0x0002F2D4
		[NetInvokableGeneratedMethod("ReceiveAnimalPanic", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAnimalPanic_Read(in ClientInvocationContext context)
		{
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref index);
			AnimalManager.ReceiveAnimalPanic(index);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000310F5 File Offset: 0x0002F2F5
		[NetInvokableGeneratedMethod("ReceiveAnimalPanic", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAnimalPanic_Write(NetPakWriter writer, ushort index)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, index);
		}
	}
}
