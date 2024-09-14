using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E6 RID: 486
	[NetInvokableGeneratedClass(typeof(InteractableSentry))]
	public static class InteractableSentry_NetMethods
	{
		// Token: 0x06000EAB RID: 3755 RVA: 0x00032EF4 File Offset: 0x000310F4
		[NetInvokableGeneratedMethod("ReceiveShoot", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveShoot_Read(in ClientInvocationContext context)
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
			InteractableSentry interactableSentry = obj as InteractableSentry;
			if (interactableSentry == null)
			{
				return;
			}
			interactableSentry.ReceiveShoot();
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00032F33 File Offset: 0x00031133
		[NetInvokableGeneratedMethod("ReceiveShoot", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveShoot_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00032F38 File Offset: 0x00031138
		[NetInvokableGeneratedMethod("ReceiveAlert", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAlert_Read(in ClientInvocationContext context)
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
			InteractableSentry interactableSentry = obj as InteractableSentry;
			if (interactableSentry == null)
			{
				return;
			}
			byte yaw;
			SystemNetPakReaderEx.ReadUInt8(reader, ref yaw);
			byte pitch;
			SystemNetPakReaderEx.ReadUInt8(reader, ref pitch);
			interactableSentry.ReceiveAlert(yaw, pitch);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00032F8F File Offset: 0x0003118F
		[NetInvokableGeneratedMethod("ReceiveAlert", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAlert_Write(NetPakWriter writer, byte yaw, byte pitch)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, yaw);
			SystemNetPakWriterEx.WriteUInt8(writer, pitch);
		}
	}
}
