using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001EC RID: 492
	[NetInvokableGeneratedClass(typeof(InteractableVehicle))]
	public static class InteractableVehicle_NetMethods
	{
		// Token: 0x06000ED0 RID: 3792 RVA: 0x0003371C File Offset: 0x0003191C
		[NetInvokableGeneratedMethod("ReceivePaintColor", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePaintColor_Read(in ClientInvocationContext context)
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
			InteractableVehicle interactableVehicle = obj as InteractableVehicle;
			if (interactableVehicle == null)
			{
				return;
			}
			Color32 newPaintColor;
			UnityNetPakReaderEx.ReadColor32RGBA(reader, ref newPaintColor);
			interactableVehicle.ReceivePaintColor(newPaintColor);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00033768 File Offset: 0x00031968
		[NetInvokableGeneratedMethod("ReceivePaintColor", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePaintColor_Write(NetPakWriter writer, Color32 newPaintColor)
		{
			UnityNetPakWriterEx.WriteColor32RGBA(writer, newPaintColor);
		}
	}
}
