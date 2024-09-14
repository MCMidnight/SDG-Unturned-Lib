using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200021C RID: 540
	[NetInvokableGeneratedClass(typeof(UseableVehiclePaint))]
	public static class UseableVehiclePaint_NetMethods
	{
		// Token: 0x06001082 RID: 4226 RVA: 0x00039924 File Offset: 0x00037B24
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
			UseableVehiclePaint useableVehiclePaint = obj as UseableVehiclePaint;
			if (useableVehiclePaint == null)
			{
				return;
			}
			useableVehiclePaint.ReceivePlayReplace();
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00039963 File Offset: 0x00037B63
		[NetInvokableGeneratedMethod("ReceivePlayReplace", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayReplace_Write(NetPakWriter writer)
		{
		}
	}
}
