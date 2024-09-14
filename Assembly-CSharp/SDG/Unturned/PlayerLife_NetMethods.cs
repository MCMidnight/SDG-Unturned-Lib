using System;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001FB RID: 507
	[NetInvokableGeneratedClass(typeof(PlayerLife))]
	public static class PlayerLife_NetMethods
	{
		// Token: 0x06000F72 RID: 3954 RVA: 0x00035F44 File Offset: 0x00034144
		[NetInvokableGeneratedMethod("ReceiveDeath", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDeath_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			EDeathCause newCause;
			reader.ReadEnum(out newCause);
			ELimb newLimb;
			reader.ReadEnum(out newLimb);
			CSteamID newKiller;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newKiller);
			playerLife.ReceiveDeath(newCause, newLimb, newKiller);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00035FA6 File Offset: 0x000341A6
		[NetInvokableGeneratedMethod("ReceiveDeath", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDeath_Write(NetPakWriter writer, EDeathCause newCause, ELimb newLimb, CSteamID newKiller)
		{
			writer.WriteEnum(newCause);
			writer.WriteEnum(newLimb);
			SteamworksNetPakWriterEx.WriteSteamID(writer, newKiller);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00035FC0 File Offset: 0x000341C0
		[NetInvokableGeneratedMethod("ReceiveDead", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDead_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			Vector3 newRagdoll;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newRagdoll, 13, 7);
			ERagdollEffect newRagdollEffect;
			reader.ReadEnum(out newRagdollEffect);
			playerLife.ReceiveDead(newRagdoll, newRagdollEffect);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0003601A File Offset: 0x0003421A
		[NetInvokableGeneratedMethod("ReceiveDead", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDead_Write(NetPakWriter writer, Vector3 newRagdoll, ERagdollEffect newRagdollEffect)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, newRagdoll, 13, 7);
			writer.WriteEnum(newRagdollEffect);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00036030 File Offset: 0x00034230
		[NetInvokableGeneratedMethod("ReceiveRevive", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRevive_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			byte angle;
			SystemNetPakReaderEx.ReadUInt8(reader, ref angle);
			playerLife.ReceiveRevive(position, angle);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0003608A File Offset: 0x0003428A
		[NetInvokableGeneratedMethod("ReceiveRevive", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRevive_Write(NetPakWriter writer, Vector3 position, byte angle)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			SystemNetPakWriterEx.WriteUInt8(writer, angle);
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000360A0 File Offset: 0x000342A0
		[NetInvokableGeneratedMethod("ReceiveLifeStats", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLifeStats_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			byte newHealth;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newHealth);
			byte newFood;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newFood);
			byte newWater;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newWater);
			byte newVirus;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newVirus);
			byte newOxygen;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newOxygen);
			bool newBleeding;
			reader.ReadBit(ref newBleeding);
			bool newBroken;
			reader.ReadBit(ref newBroken);
			playerLife.ReceiveLifeStats(newHealth, newFood, newWater, newVirus, newOxygen, newBleeding, newBroken);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0003612E File Offset: 0x0003432E
		[NetInvokableGeneratedMethod("ReceiveLifeStats", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLifeStats_Write(NetPakWriter writer, byte newHealth, byte newFood, byte newWater, byte newVirus, byte newOxygen, bool newBleeding, bool newBroken)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newHealth);
			SystemNetPakWriterEx.WriteUInt8(writer, newFood);
			SystemNetPakWriterEx.WriteUInt8(writer, newWater);
			SystemNetPakWriterEx.WriteUInt8(writer, newVirus);
			SystemNetPakWriterEx.WriteUInt8(writer, newOxygen);
			writer.WriteBit(newBleeding);
			writer.WriteBit(newBroken);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0003616C File Offset: 0x0003436C
		[NetInvokableGeneratedMethod("ReceiveHealth", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveHealth_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			byte newHealth;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newHealth);
			playerLife.ReceiveHealth(newHealth);
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000361B8 File Offset: 0x000343B8
		[NetInvokableGeneratedMethod("ReceiveHealth", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveHealth_Write(NetPakWriter writer, byte newHealth)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newHealth);
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000361C4 File Offset: 0x000343C4
		[NetInvokableGeneratedMethod("ReceiveDamagedEvent", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDamagedEvent_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			byte damageAmount;
			SystemNetPakReaderEx.ReadUInt8(reader, ref damageAmount);
			Vector3 damageDirection;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref damageDirection, 13, 7);
			playerLife.ReceiveDamagedEvent(damageAmount, damageDirection);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0003621E File Offset: 0x0003441E
		[NetInvokableGeneratedMethod("ReceiveDamagedEvent", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDamagedEvent_Write(NetPakWriter writer, byte damageAmount, Vector3 damageDirection)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, damageAmount);
			UnityNetPakWriterEx.WriteClampedVector3(writer, damageDirection, 13, 7);
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00036234 File Offset: 0x00034434
		[NetInvokableGeneratedMethod("ReceiveFood", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveFood_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			byte newFood;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newFood);
			playerLife.ReceiveFood(newFood);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00036280 File Offset: 0x00034480
		[NetInvokableGeneratedMethod("ReceiveFood", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveFood_Write(NetPakWriter writer, byte newFood)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newFood);
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0003628C File Offset: 0x0003448C
		[NetInvokableGeneratedMethod("ReceiveWater", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWater_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			byte newWater;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newWater);
			playerLife.ReceiveWater(newWater);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000362D8 File Offset: 0x000344D8
		[NetInvokableGeneratedMethod("ReceiveWater", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWater_Write(NetPakWriter writer, byte newWater)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newWater);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000362E4 File Offset: 0x000344E4
		[NetInvokableGeneratedMethod("ReceiveVirus", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVirus_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			byte newVirus;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newVirus);
			playerLife.ReceiveVirus(newVirus);
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00036330 File Offset: 0x00034530
		[NetInvokableGeneratedMethod("ReceiveVirus", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVirus_Write(NetPakWriter writer, byte newVirus)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newVirus);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0003633C File Offset: 0x0003453C
		[NetInvokableGeneratedMethod("ReceiveBleeding", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBleeding_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			bool newBleeding;
			reader.ReadBit(ref newBleeding);
			playerLife.ReceiveBleeding(newBleeding);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00036388 File Offset: 0x00034588
		[NetInvokableGeneratedMethod("ReceiveBleeding", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBleeding_Write(NetPakWriter writer, bool newBleeding)
		{
			writer.WriteBit(newBleeding);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00036394 File Offset: 0x00034594
		[NetInvokableGeneratedMethod("ReceiveBroken", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBroken_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			bool newBroken;
			reader.ReadBit(ref newBroken);
			playerLife.ReceiveBroken(newBroken);
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x000363E0 File Offset: 0x000345E0
		[NetInvokableGeneratedMethod("ReceiveBroken", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBroken_Write(NetPakWriter writer, bool newBroken)
		{
			writer.WriteBit(newBroken);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000363EC File Offset: 0x000345EC
		[NetInvokableGeneratedMethod("ReceiveModifyStamina", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveModifyStamina_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			short delta;
			SystemNetPakReaderEx.ReadInt16(reader, ref delta);
			playerLife.ReceiveModifyStamina(delta);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00036438 File Offset: 0x00034638
		[NetInvokableGeneratedMethod("ReceiveModifyStamina", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveModifyStamina_Write(NetPakWriter writer, short delta)
		{
			SystemNetPakWriterEx.WriteInt16(writer, delta);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00036444 File Offset: 0x00034644
		[NetInvokableGeneratedMethod("ReceiveModifyHallucination", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveModifyHallucination_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			short delta;
			SystemNetPakReaderEx.ReadInt16(reader, ref delta);
			playerLife.ReceiveModifyHallucination(delta);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00036490 File Offset: 0x00034690
		[NetInvokableGeneratedMethod("ReceiveModifyHallucination", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveModifyHallucination_Write(NetPakWriter writer, short delta)
		{
			SystemNetPakWriterEx.WriteInt16(writer, delta);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0003649C File Offset: 0x0003469C
		[NetInvokableGeneratedMethod("ReceiveModifyWarmth", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveModifyWarmth_Read(in ClientInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			short delta;
			SystemNetPakReaderEx.ReadInt16(reader, ref delta);
			playerLife.ReceiveModifyWarmth(delta);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x000364E8 File Offset: 0x000346E8
		[NetInvokableGeneratedMethod("ReceiveModifyWarmth", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveModifyWarmth_Write(NetPakWriter writer, short delta)
		{
			SystemNetPakWriterEx.WriteInt16(writer, delta);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000364F4 File Offset: 0x000346F4
		[NetInvokableGeneratedMethod("ReceiveRespawnRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRespawnRequest_Read(in ServerInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerLife.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerLife));
				return;
			}
			bool atHome;
			reader.ReadBit(ref atHome);
			playerLife.ReceiveRespawnRequest(atHome);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00036560 File Offset: 0x00034760
		[NetInvokableGeneratedMethod("ReceiveRespawnRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRespawnRequest_Write(NetPakWriter writer, bool atHome)
		{
			writer.WriteBit(atHome);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0003656C File Offset: 0x0003476C
		[NetInvokableGeneratedMethod("ReceiveSuicideRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSuicideRequest_Read(in ServerInvocationContext context)
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
			PlayerLife playerLife = obj as PlayerLife;
			if (playerLife == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerLife.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerLife));
				return;
			}
			playerLife.ReceiveSuicideRequest();
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x000365CB File Offset: 0x000347CB
		[NetInvokableGeneratedMethod("ReceiveSuicideRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSuicideRequest_Write(NetPakWriter writer)
		{
		}
	}
}
