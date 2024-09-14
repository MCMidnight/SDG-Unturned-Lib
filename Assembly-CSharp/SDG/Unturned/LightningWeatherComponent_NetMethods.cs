using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001F0 RID: 496
	[NetInvokableGeneratedClass(typeof(LightningWeatherComponent))]
	public static class LightningWeatherComponent_NetMethods
	{
		// Token: 0x06000EF0 RID: 3824 RVA: 0x00033D5C File Offset: 0x00031F5C
		[NetInvokableGeneratedMethod("ReceiveLightningStrike", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLightningStrike_Read(in ClientInvocationContext context)
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
			LightningWeatherComponent lightningWeatherComponent = obj as LightningWeatherComponent;
			if (lightningWeatherComponent == null)
			{
				return;
			}
			Vector3 hitPosition;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref hitPosition, 13, 7);
			lightningWeatherComponent.ReceiveLightningStrike(hitPosition);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00033DAB File Offset: 0x00031FAB
		[NetInvokableGeneratedMethod("ReceiveLightningStrike", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLightningStrike_Write(NetPakWriter writer, Vector3 hitPosition)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, hitPosition, 13, 7);
		}
	}
}
