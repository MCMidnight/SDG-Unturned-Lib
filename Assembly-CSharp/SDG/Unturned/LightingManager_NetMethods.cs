using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001EF RID: 495
	[NetInvokableGeneratedClass(typeof(LightingManager))]
	public static class LightingManager_NetMethods
	{
		// Token: 0x06000EE4 RID: 3812 RVA: 0x00033B88 File Offset: 0x00031D88
		[NetInvokableGeneratedMethod("ReceiveInitialLightingState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveInitialLightingState_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint serverTime;
			SystemNetPakReaderEx.ReadUInt32(reader, ref serverTime);
			uint newCycle;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newCycle);
			uint newOffset;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newOffset);
			byte moon;
			SystemNetPakReaderEx.ReadUInt8(reader, ref moon);
			byte wind;
			SystemNetPakReaderEx.ReadUInt8(reader, ref wind);
			Guid activeWeatherGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref activeWeatherGuid);
			float activeWeatherBlendAlpha;
			SystemNetPakReaderEx.ReadFloat(reader, ref activeWeatherBlendAlpha);
			NetId activeWeatherNetId;
			reader.ReadNetId(out activeWeatherNetId);
			int newDateCounter;
			SystemNetPakReaderEx.ReadInt32(reader, ref newDateCounter);
			LightingManager.ReceiveInitialLightingState(serverTime, newCycle, newOffset, moon, wind, activeWeatherGuid, activeWeatherBlendAlpha, activeWeatherNetId, newDateCounter);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00033C00 File Offset: 0x00031E00
		[NetInvokableGeneratedMethod("ReceiveInitialLightingState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveInitialLightingState_Write(NetPakWriter writer, uint serverTime, uint newCycle, uint newOffset, byte moon, byte wind, Guid activeWeatherGuid, float activeWeatherBlendAlpha, NetId activeWeatherNetId, int newDateCounter)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, serverTime);
			SystemNetPakWriterEx.WriteUInt32(writer, newCycle);
			SystemNetPakWriterEx.WriteUInt32(writer, newOffset);
			SystemNetPakWriterEx.WriteUInt8(writer, moon);
			SystemNetPakWriterEx.WriteUInt8(writer, wind);
			SystemNetPakWriterEx.WriteGuid(writer, activeWeatherGuid);
			SystemNetPakWriterEx.WriteFloat(writer, activeWeatherBlendAlpha);
			writer.WriteNetId(activeWeatherNetId);
			SystemNetPakWriterEx.WriteInt32(writer, newDateCounter);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00033C5C File Offset: 0x00031E5C
		[NetInvokableGeneratedMethod("ReceiveLightingCycle", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLightingCycle_Read(in ClientInvocationContext context)
		{
			uint newScale;
			SystemNetPakReaderEx.ReadUInt32(context.reader, ref newScale);
			LightingManager.ReceiveLightingCycle(newScale);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00033C7D File Offset: 0x00031E7D
		[NetInvokableGeneratedMethod("ReceiveLightingCycle", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLightingCycle_Write(NetPakWriter writer, uint newScale)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newScale);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00033C88 File Offset: 0x00031E88
		[NetInvokableGeneratedMethod("ReceiveLightingOffset", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLightingOffset_Read(in ClientInvocationContext context)
		{
			uint newOffset;
			SystemNetPakReaderEx.ReadUInt32(context.reader, ref newOffset);
			LightingManager.ReceiveLightingOffset(newOffset);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00033CA9 File Offset: 0x00031EA9
		[NetInvokableGeneratedMethod("ReceiveLightingOffset", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLightingOffset_Write(NetPakWriter writer, uint newOffset)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newOffset);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00033CB4 File Offset: 0x00031EB4
		[NetInvokableGeneratedMethod("ReceiveLightingWind", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLightingWind_Read(in ClientInvocationContext context)
		{
			byte newWind;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref newWind);
			LightingManager.ReceiveLightingWind(newWind);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00033CD5 File Offset: 0x00031ED5
		[NetInvokableGeneratedMethod("ReceiveLightingWind", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLightingWind_Write(NetPakWriter writer, byte newWind)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newWind);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00033CE0 File Offset: 0x00031EE0
		[NetInvokableGeneratedMethod("ReceiveDateCounter", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDateCounter_Read(in ClientInvocationContext context)
		{
			long newValue;
			SystemNetPakReaderEx.ReadInt64(context.reader, ref newValue);
			LightingManager.ReceiveDateCounter(newValue);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00033D01 File Offset: 0x00031F01
		[NetInvokableGeneratedMethod("ReceiveDateCounter", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDateCounter_Write(NetPakWriter writer, long newValue)
		{
			SystemNetPakWriterEx.WriteInt64(writer, newValue);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00033D0C File Offset: 0x00031F0C
		[NetInvokableGeneratedMethod("ReceiveLightingActiveWeather", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLightingActiveWeather_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			float blendAlpha;
			SystemNetPakReaderEx.ReadFloat(reader, ref blendAlpha);
			NetId netId;
			reader.ReadNetId(out netId);
			LightingManager.ReceiveLightingActiveWeather(assetGuid, blendAlpha, netId);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00033D41 File Offset: 0x00031F41
		[NetInvokableGeneratedMethod("ReceiveLightingActiveWeather", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLightingActiveWeather_Write(NetPakWriter writer, Guid assetGuid, float blendAlpha, NetId netId)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			SystemNetPakWriterEx.WriteFloat(writer, blendAlpha);
			writer.WriteNetId(netId);
		}
	}
}
