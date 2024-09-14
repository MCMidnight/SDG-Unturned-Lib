using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000219 RID: 537
	[NetInvokableGeneratedClass(typeof(UseableThrowable))]
	public static class UseableThrowable_NetMethods
	{
		// Token: 0x0600107A RID: 4218 RVA: 0x000397E0 File Offset: 0x000379E0
		[NetInvokableGeneratedMethod("ReceiveToss", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveToss_Read(in ClientInvocationContext context)
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
			UseableThrowable useableThrowable = obj as UseableThrowable;
			if (useableThrowable == null)
			{
				return;
			}
			Vector3 origin;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref origin, 13, 7);
			Vector3 force;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref force, 13, 7);
			useableThrowable.ReceiveToss(origin, force);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0003983D File Offset: 0x00037A3D
		[NetInvokableGeneratedMethod("ReceiveToss", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToss_Write(NetPakWriter writer, Vector3 origin, Vector3 force)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, origin, 13, 7);
			UnityNetPakWriterEx.WriteClampedVector3(writer, force, 13, 7);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00039858 File Offset: 0x00037A58
		[NetInvokableGeneratedMethod("ReceivePlaySwing", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlaySwing_Read(in ClientInvocationContext context)
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
			UseableThrowable useableThrowable = obj as UseableThrowable;
			if (useableThrowable == null)
			{
				return;
			}
			useableThrowable.ReceivePlaySwing();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00039897 File Offset: 0x00037A97
		[NetInvokableGeneratedMethod("ReceivePlaySwing", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlaySwing_Write(NetPakWriter writer)
		{
		}
	}
}
