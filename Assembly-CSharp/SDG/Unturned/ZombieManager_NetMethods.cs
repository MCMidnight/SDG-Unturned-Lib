using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200021E RID: 542
	[NetInvokableGeneratedClass(typeof(ZombieManager))]
	public static class ZombieManager_NetMethods
	{
		// Token: 0x060010B0 RID: 4272 RVA: 0x00039F40 File Offset: 0x00038140
		[NetInvokableGeneratedMethod("ReceiveBeacon", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBeacon_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			bool hasBeacon;
			reader.ReadBit(ref hasBeacon);
			ZombieManager.ReceiveBeacon(reference, hasBeacon);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00039F6B File Offset: 0x0003816B
		[NetInvokableGeneratedMethod("ReceiveBeacon", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBeacon_Write(NetPakWriter writer, byte reference, bool hasBeacon)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			writer.WriteBit(hasBeacon);
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00039F80 File Offset: 0x00038180
		[NetInvokableGeneratedMethod("ReceiveWave", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWave_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			bool newWaveReady;
			reader.ReadBit(ref newWaveReady);
			int newWave;
			SystemNetPakReaderEx.ReadInt32(reader, ref newWave);
			ZombieManager.ReceiveWave(newWaveReady, newWave);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00039FAB File Offset: 0x000381AB
		[NetInvokableGeneratedMethod("ReceiveWave", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWave_Write(NetPakWriter writer, bool newWaveReady, int newWave)
		{
			writer.WriteBit(newWaveReady);
			SystemNetPakWriterEx.WriteInt32(writer, newWave);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00039FC0 File Offset: 0x000381C0
		[NetInvokableGeneratedMethod("ReceiveZombieAlive", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieAlive_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte newType;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newType);
			byte newSpeciality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newSpeciality);
			byte newShirt;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newShirt);
			byte newPants;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newPants);
			byte newHat;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newHat);
			byte newGear;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newGear);
			Vector3 newPosition;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPosition, 13, 7);
			byte newAngle;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newAngle);
			ZombieManager.ReceiveZombieAlive(reference, id, newType, newSpeciality, newShirt, newPants, newHat, newGear, newPosition, newAngle);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0003A044 File Offset: 0x00038244
		[NetInvokableGeneratedMethod("ReceiveZombieAlive", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieAlive_Write(NetPakWriter writer, byte reference, ushort id, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, newType);
			SystemNetPakWriterEx.WriteUInt8(writer, newSpeciality);
			SystemNetPakWriterEx.WriteUInt8(writer, newShirt);
			SystemNetPakWriterEx.WriteUInt8(writer, newPants);
			SystemNetPakWriterEx.WriteUInt8(writer, newHat);
			SystemNetPakWriterEx.WriteUInt8(writer, newGear);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newPosition, 13, 7);
			SystemNetPakWriterEx.WriteUInt8(writer, newAngle);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0003A0AC File Offset: 0x000382AC
		[NetInvokableGeneratedMethod("ReceiveZombieDead", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieDead_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			Vector3 newRagdoll;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newRagdoll, 13, 7);
			ERagdollEffect newRagdollEffect;
			reader.ReadEnum(out newRagdollEffect);
			ZombieManager.ReceiveZombieDead(reference, id, newRagdoll, newRagdollEffect);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0003A0EE File Offset: 0x000382EE
		[NetInvokableGeneratedMethod("ReceiveZombieDead", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieDead_Write(NetPakWriter writer, byte reference, ushort id, Vector3 newRagdoll, ERagdollEffect newRagdollEffect)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newRagdoll, 13, 7);
			writer.WriteEnum(newRagdollEffect);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0003A114 File Offset: 0x00038314
		[NetInvokableGeneratedMethod("ReceiveZombieSpeciality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieSpeciality_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			EZombieSpeciality speciality;
			reader.ReadEnum(out speciality);
			ZombieManager.ReceiveZombieSpeciality(reference, id, speciality);
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0003A149 File Offset: 0x00038349
		[NetInvokableGeneratedMethod("ReceiveZombieSpeciality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieSpeciality_Write(NetPakWriter writer, byte reference, ushort id, EZombieSpeciality speciality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			writer.WriteEnum(speciality);
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0003A164 File Offset: 0x00038364
		[NetInvokableGeneratedMethod("ReceiveZombieThrow", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieThrow_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			ZombieManager.ReceiveZombieThrow(reference, id);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0003A18F File Offset: 0x0003838F
		[NetInvokableGeneratedMethod("ReceiveZombieThrow", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieThrow_Write(NetPakWriter writer, byte reference, ushort id)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0003A1A4 File Offset: 0x000383A4
		[NetInvokableGeneratedMethod("ReceiveZombieBoulder", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieBoulder_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			Vector3 origin;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref origin, 13, 7);
			Vector3 direction;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref direction, 9);
			ZombieManager.ReceiveZombieBoulder(reference, id, origin, direction);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0003A1E8 File Offset: 0x000383E8
		[NetInvokableGeneratedMethod("ReceiveZombieBoulder", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieBoulder_Write(NetPakWriter writer, byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			UnityNetPakWriterEx.WriteClampedVector3(writer, origin, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, direction, 9);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0003A210 File Offset: 0x00038410
		[NetInvokableGeneratedMethod("ReceiveZombieSpit", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieSpit_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			ZombieManager.ReceiveZombieSpit(reference, id);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0003A23B File Offset: 0x0003843B
		[NetInvokableGeneratedMethod("ReceiveZombieSpit", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieSpit_Write(NetPakWriter writer, byte reference, ushort id)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0003A250 File Offset: 0x00038450
		[NetInvokableGeneratedMethod("ReceiveZombieCharge", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieCharge_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			ZombieManager.ReceiveZombieCharge(reference, id);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0003A27B File Offset: 0x0003847B
		[NetInvokableGeneratedMethod("ReceiveZombieCharge", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieCharge_Write(NetPakWriter writer, byte reference, ushort id)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0003A290 File Offset: 0x00038490
		[NetInvokableGeneratedMethod("ReceiveZombieStomp", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieStomp_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			ZombieManager.ReceiveZombieStomp(reference, id);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0003A2BB File Offset: 0x000384BB
		[NetInvokableGeneratedMethod("ReceiveZombieStomp", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieStomp_Write(NetPakWriter writer, byte reference, ushort id)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0003A2D0 File Offset: 0x000384D0
		[NetInvokableGeneratedMethod("ReceiveZombieBreath", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieBreath_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			ZombieManager.ReceiveZombieBreath(reference, id);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0003A2FB File Offset: 0x000384FB
		[NetInvokableGeneratedMethod("ReceiveZombieBreath", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieBreath_Write(NetPakWriter writer, byte reference, ushort id)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0003A310 File Offset: 0x00038510
		[NetInvokableGeneratedMethod("ReceiveZombieAcid", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieAcid_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			Vector3 origin;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref origin, 13, 7);
			Vector3 direction;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref direction, 9);
			ZombieManager.ReceiveZombieAcid(reference, id, origin, direction);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0003A354 File Offset: 0x00038554
		[NetInvokableGeneratedMethod("ReceiveZombieAcid", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieAcid_Write(NetPakWriter writer, byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			UnityNetPakWriterEx.WriteClampedVector3(writer, origin, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, direction, 9);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0003A37C File Offset: 0x0003857C
		[NetInvokableGeneratedMethod("ReceiveZombieSpark", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieSpark_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			Vector3 target;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref target, 13, 7);
			ZombieManager.ReceiveZombieSpark(reference, id, target);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0003A3B4 File Offset: 0x000385B4
		[NetInvokableGeneratedMethod("ReceiveZombieSpark", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieSpark_Write(NetPakWriter writer, byte reference, ushort id, Vector3 target)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			UnityNetPakWriterEx.WriteClampedVector3(writer, target, 13, 7);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0003A3D4 File Offset: 0x000385D4
		[NetInvokableGeneratedMethod("ReceiveZombieAttack", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieAttack_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte attack;
			SystemNetPakReaderEx.ReadUInt8(reader, ref attack);
			ZombieManager.ReceiveZombieAttack(reference, id, attack);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0003A409 File Offset: 0x00038609
		[NetInvokableGeneratedMethod("ReceiveZombieAttack", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieAttack_Write(NetPakWriter writer, byte reference, ushort id, byte attack)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, attack);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0003A424 File Offset: 0x00038624
		[NetInvokableGeneratedMethod("ReceiveZombieStartle", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieStartle_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte startle;
			SystemNetPakReaderEx.ReadUInt8(reader, ref startle);
			ZombieManager.ReceiveZombieStartle(reference, id, startle);
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0003A459 File Offset: 0x00038659
		[NetInvokableGeneratedMethod("ReceiveZombieStartle", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieStartle_Write(NetPakWriter writer, byte reference, ushort id, byte startle)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, startle);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0003A474 File Offset: 0x00038674
		[NetInvokableGeneratedMethod("ReceiveZombieStun", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveZombieStun_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte reference;
			SystemNetPakReaderEx.ReadUInt8(reader, ref reference);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte stun;
			SystemNetPakReaderEx.ReadUInt8(reader, ref stun);
			ZombieManager.ReceiveZombieStun(reference, id, stun);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0003A4A9 File Offset: 0x000386A9
		[NetInvokableGeneratedMethod("ReceiveZombieStun", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveZombieStun_Write(NetPakWriter writer, byte reference, ushort id, byte stun)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, reference);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, stun);
		}
	}
}
