using System;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200021D RID: 541
	[NetInvokableGeneratedClass(typeof(VehicleManager))]
	public static class VehicleManager_NetMethods
	{
		// Token: 0x06001084 RID: 4228 RVA: 0x00039968 File Offset: 0x00037B68
		[NetInvokableGeneratedMethod("ReceiveVehicleLockState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleLockState_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			CSteamID owner;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref owner);
			CSteamID group;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref group);
			bool locked;
			reader.ReadBit(ref locked);
			VehicleManager.ReceiveVehicleLockState(instanceID, owner, group, locked);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x000399A7 File Offset: 0x00037BA7
		[NetInvokableGeneratedMethod("ReceiveVehicleLockState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleLockState_Write(NetPakWriter writer, uint instanceID, CSteamID owner, CSteamID group, bool locked)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SteamworksNetPakWriterEx.WriteSteamID(writer, owner);
			SteamworksNetPakWriterEx.WriteSteamID(writer, group);
			writer.WriteBit(locked);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x000399CC File Offset: 0x00037BCC
		[NetInvokableGeneratedMethod("ReceiveVehicleSkin", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleSkin_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			ushort skinID;
			SystemNetPakReaderEx.ReadUInt16(reader, ref skinID);
			ushort mythicID;
			SystemNetPakReaderEx.ReadUInt16(reader, ref mythicID);
			VehicleManager.ReceiveVehicleSkin(instanceID, skinID, mythicID);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00039A01 File Offset: 0x00037C01
		[NetInvokableGeneratedMethod("ReceiveVehicleSkin", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleSkin_Write(NetPakWriter writer, uint instanceID, ushort skinID, ushort mythicID)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt16(writer, skinID);
			SystemNetPakWriterEx.WriteUInt16(writer, mythicID);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00039A1C File Offset: 0x00037C1C
		[NetInvokableGeneratedMethod("ReceiveVehicleSirens", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleSirens_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			bool on;
			reader.ReadBit(ref on);
			VehicleManager.ReceiveVehicleSirens(instanceID, on);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00039A47 File Offset: 0x00037C47
		[NetInvokableGeneratedMethod("ReceiveVehicleSirens", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleSirens_Write(NetPakWriter writer, uint instanceID, bool on)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			writer.WriteBit(on);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00039A5C File Offset: 0x00037C5C
		[NetInvokableGeneratedMethod("ReceiveVehicleBlimp", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleBlimp_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			bool on;
			reader.ReadBit(ref on);
			VehicleManager.ReceiveVehicleBlimp(instanceID, on);
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00039A87 File Offset: 0x00037C87
		[NetInvokableGeneratedMethod("ReceiveVehicleBlimp", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleBlimp_Write(NetPakWriter writer, uint instanceID, bool on)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			writer.WriteBit(on);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00039A9C File Offset: 0x00037C9C
		[NetInvokableGeneratedMethod("ReceiveVehicleHeadlights", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleHeadlights_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			bool on;
			reader.ReadBit(ref on);
			VehicleManager.ReceiveVehicleHeadlights(instanceID, on);
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00039AC7 File Offset: 0x00037CC7
		[NetInvokableGeneratedMethod("ReceiveVehicleHeadlights", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleHeadlights_Write(NetPakWriter writer, uint instanceID, bool on)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			writer.WriteBit(on);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00039ADC File Offset: 0x00037CDC
		[NetInvokableGeneratedMethod("ReceiveVehicleHorn", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleHorn_Read(in ClientInvocationContext context)
		{
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(context.reader, ref instanceID);
			VehicleManager.ReceiveVehicleHorn(instanceID);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00039AFD File Offset: 0x00037CFD
		[NetInvokableGeneratedMethod("ReceiveVehicleHorn", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleHorn_Write(NetPakWriter writer, uint instanceID)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00039B08 File Offset: 0x00037D08
		[NetInvokableGeneratedMethod("ReceiveVehicleFuel", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleFuel_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			ushort newFuel;
			SystemNetPakReaderEx.ReadUInt16(reader, ref newFuel);
			VehicleManager.ReceiveVehicleFuel(instanceID, newFuel);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00039B33 File Offset: 0x00037D33
		[NetInvokableGeneratedMethod("ReceiveVehicleFuel", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleFuel_Write(NetPakWriter writer, uint instanceID, ushort newFuel)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt16(writer, newFuel);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00039B48 File Offset: 0x00037D48
		[NetInvokableGeneratedMethod("ReceiveVehicleBatteryCharge", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleBatteryCharge_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			ushort newBatteryCharge;
			SystemNetPakReaderEx.ReadUInt16(reader, ref newBatteryCharge);
			VehicleManager.ReceiveVehicleBatteryCharge(instanceID, newBatteryCharge);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00039B73 File Offset: 0x00037D73
		[NetInvokableGeneratedMethod("ReceiveVehicleBatteryCharge", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleBatteryCharge_Write(NetPakWriter writer, uint instanceID, ushort newBatteryCharge)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt16(writer, newBatteryCharge);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00039B88 File Offset: 0x00037D88
		[NetInvokableGeneratedMethod("ReceiveVehicleTireAliveMask", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleTireAliveMask_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			byte newTireAliveMask;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newTireAliveMask);
			VehicleManager.ReceiveVehicleTireAliveMask(instanceID, newTireAliveMask);
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00039BB3 File Offset: 0x00037DB3
		[NetInvokableGeneratedMethod("ReceiveVehicleTireAliveMask", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleTireAliveMask_Write(NetPakWriter writer, uint instanceID, byte newTireAliveMask)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt8(writer, newTireAliveMask);
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00039BC8 File Offset: 0x00037DC8
		[NetInvokableGeneratedMethod("ReceiveVehicleExploded", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleExploded_Read(in ClientInvocationContext context)
		{
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(context.reader, ref instanceID);
			VehicleManager.ReceiveVehicleExploded(instanceID);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00039BE9 File Offset: 0x00037DE9
		[NetInvokableGeneratedMethod("ReceiveVehicleExploded", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleExploded_Write(NetPakWriter writer, uint instanceID)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00039BF4 File Offset: 0x00037DF4
		[NetInvokableGeneratedMethod("ReceiveVehicleHealth", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleHealth_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			ushort newHealth;
			SystemNetPakReaderEx.ReadUInt16(reader, ref newHealth);
			VehicleManager.ReceiveVehicleHealth(instanceID, newHealth);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00039C1F File Offset: 0x00037E1F
		[NetInvokableGeneratedMethod("ReceiveVehicleHealth", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleHealth_Write(NetPakWriter writer, uint instanceID, ushort newHealth)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt16(writer, newHealth);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00039C34 File Offset: 0x00037E34
		[NetInvokableGeneratedMethod("ReceiveVehicleRecov", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVehicleRecov_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			Vector3 newPosition;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPosition, 13, 7);
			int newRecov;
			SystemNetPakReaderEx.ReadInt32(reader, ref newRecov);
			VehicleManager.ReceiveVehicleRecov(instanceID, newPosition, newRecov);
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00039C6C File Offset: 0x00037E6C
		[NetInvokableGeneratedMethod("ReceiveVehicleRecov", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVehicleRecov_Write(NetPakWriter writer, uint instanceID, Vector3 newPosition, int newRecov)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newPosition, 13, 7);
			SystemNetPakWriterEx.WriteInt32(writer, newRecov);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00039C8C File Offset: 0x00037E8C
		[NetInvokableGeneratedMethod("ReceiveDestroySingleVehicle", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDestroySingleVehicle_Read(in ClientInvocationContext context)
		{
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(context.reader, ref instanceID);
			VehicleManager.ReceiveDestroySingleVehicle(instanceID);
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00039CAD File Offset: 0x00037EAD
		[NetInvokableGeneratedMethod("ReceiveDestroySingleVehicle", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDestroySingleVehicle_Write(NetPakWriter writer, uint instanceID)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00039CB7 File Offset: 0x00037EB7
		[NetInvokableGeneratedMethod("ReceiveDestroyAllVehicles", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDestroyAllVehicles_Read(in ClientInvocationContext context)
		{
			VehicleManager.ReceiveDestroyAllVehicles();
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00039CBE File Offset: 0x00037EBE
		[NetInvokableGeneratedMethod("ReceiveDestroyAllVehicles", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDestroyAllVehicles_Write(NetPakWriter writer)
		{
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00039CC0 File Offset: 0x00037EC0
		[NetInvokableGeneratedMethod("ReceiveEnterVehicle", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEnterVehicle_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			byte seat;
			SystemNetPakReaderEx.ReadUInt8(reader, ref seat);
			CSteamID player;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref player);
			VehicleManager.ReceiveEnterVehicle(instanceID, seat, player);
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00039CF5 File Offset: 0x00037EF5
		[NetInvokableGeneratedMethod("ReceiveEnterVehicle", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEnterVehicle_Write(NetPakWriter writer, uint instanceID, byte seat, CSteamID player)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt8(writer, seat);
			SteamworksNetPakWriterEx.WriteSteamID(writer, player);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00039D10 File Offset: 0x00037F10
		[NetInvokableGeneratedMethod("ReceiveExitVehicle", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveExitVehicle_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			byte seat;
			SystemNetPakReaderEx.ReadUInt8(reader, ref seat);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			byte angle;
			SystemNetPakReaderEx.ReadUInt8(reader, ref angle);
			bool forceUpdate;
			reader.ReadBit(ref forceUpdate);
			VehicleManager.ReceiveExitVehicle(instanceID, seat, point, angle, forceUpdate);
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00039D5D File Offset: 0x00037F5D
		[NetInvokableGeneratedMethod("ReceiveExitVehicle", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveExitVehicle_Write(NetPakWriter writer, uint instanceID, byte seat, Vector3 point, byte angle, bool forceUpdate)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt8(writer, seat);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
			SystemNetPakWriterEx.WriteUInt8(writer, angle);
			writer.WriteBit(forceUpdate);
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00039D8C File Offset: 0x00037F8C
		[NetInvokableGeneratedMethod("ReceiveSwapVehicleSeats", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapVehicleSeats_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			byte fromSeat;
			SystemNetPakReaderEx.ReadUInt8(reader, ref fromSeat);
			byte toSeat;
			SystemNetPakReaderEx.ReadUInt8(reader, ref toSeat);
			VehicleManager.ReceiveSwapVehicleSeats(instanceID, fromSeat, toSeat);
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00039DC1 File Offset: 0x00037FC1
		[NetInvokableGeneratedMethod("ReceiveSwapVehicleSeats", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapVehicleSeats_Write(NetPakWriter writer, uint instanceID, byte fromSeat, byte toSeat)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt8(writer, fromSeat);
			SystemNetPakWriterEx.WriteUInt8(writer, toSeat);
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00039DDC File Offset: 0x00037FDC
		[NetInvokableGeneratedMethod("ReceiveToggleVehicleHeadlights", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveToggleVehicleHeadlights_Read(in ServerInvocationContext context)
		{
			bool wantsHeadlightsOn;
			context.reader.ReadBit(ref wantsHeadlightsOn);
			VehicleManager.ReceiveToggleVehicleHeadlights(context, wantsHeadlightsOn);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00039DFE File Offset: 0x00037FFE
		[NetInvokableGeneratedMethod("ReceiveToggleVehicleHeadlights", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleVehicleHeadlights_Write(NetPakWriter writer, bool wantsHeadlightsOn)
		{
			writer.WriteBit(wantsHeadlightsOn);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00039E08 File Offset: 0x00038008
		[NetInvokableGeneratedMethod("ReceiveUseVehicleBonus", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUseVehicleBonus_Read(in ServerInvocationContext context)
		{
			byte bonusType;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref bonusType);
			VehicleManager.ReceiveUseVehicleBonus(context, bonusType);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00039E2A File Offset: 0x0003802A
		[NetInvokableGeneratedMethod("ReceiveUseVehicleBonus", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUseVehicleBonus_Write(NetPakWriter writer, byte bonusType)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, bonusType);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00039E34 File Offset: 0x00038034
		[NetInvokableGeneratedMethod("ReceiveEnterVehicleRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEnterVehicleRequest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			byte b2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b2);
			byte[] array2 = new byte[(int)b2];
			reader.ReadBytes(array2);
			byte engine;
			SystemNetPakReaderEx.ReadUInt8(reader, ref engine);
			VehicleManager.ReceiveEnterVehicleRequest(context, instanceID, array, array2, engine);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00039E94 File Offset: 0x00038094
		[NetInvokableGeneratedMethod("ReceiveEnterVehicleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEnterVehicleRequest_Write(NetPakWriter writer, uint instanceID, byte[] hash, byte[] physicsProfileHash, byte engine)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			byte b = (byte)hash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(hash, (int)b);
			byte b2 = (byte)physicsProfileHash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b2);
			writer.WriteBytes(physicsProfileHash, (int)b2);
			SystemNetPakWriterEx.WriteUInt8(writer, engine);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00039EE0 File Offset: 0x000380E0
		[NetInvokableGeneratedMethod("ReceiveExitVehicleRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveExitVehicleRequest_Read(in ServerInvocationContext context)
		{
			Vector3 velocity;
			UnityNetPakReaderEx.ReadClampedVector3(context.reader, ref velocity, 13, 7);
			VehicleManager.ReceiveExitVehicleRequest(context, velocity);
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00039F05 File Offset: 0x00038105
		[NetInvokableGeneratedMethod("ReceiveExitVehicleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveExitVehicleRequest_Write(NetPakWriter writer, Vector3 velocity)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, velocity, 13, 7);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00039F14 File Offset: 0x00038114
		[NetInvokableGeneratedMethod("ReceiveSwapVehicleRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapVehicleRequest_Read(in ServerInvocationContext context)
		{
			byte toSeat;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref toSeat);
			VehicleManager.ReceiveSwapVehicleRequest(context, toSeat);
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00039F36 File Offset: 0x00038136
		[NetInvokableGeneratedMethod("ReceiveSwapVehicleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapVehicleRequest_Write(NetPakWriter writer, byte toSeat)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, toSeat);
		}
	}
}
