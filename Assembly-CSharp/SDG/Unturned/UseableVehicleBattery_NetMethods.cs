using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200021B RID: 539
	[NetInvokableGeneratedClass(typeof(UseableVehicleBattery))]
	public static class UseableVehicleBattery_NetMethods
	{
		// Token: 0x06001080 RID: 4224 RVA: 0x000398E0 File Offset: 0x00037AE0
		[NetInvokableGeneratedMethod("ReceivePlayReplace", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayReplace_Read(in ClientInvocationContext context)
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
			UseableVehicleBattery useableVehicleBattery = obj as UseableVehicleBattery;
			if (useableVehicleBattery == null)
			{
				return;
			}
			useableVehicleBattery.ReceivePlayReplace();
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0003991F File Offset: 0x00037B1F
		[NetInvokableGeneratedMethod("ReceivePlayReplace", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayReplace_Write(NetPakWriter writer)
		{
		}
	}
}
