using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000218 RID: 536
	[NetInvokableGeneratedClass(typeof(UseableStructure))]
	public static class UseableStructure_NetMethods
	{
		// Token: 0x06001076 RID: 4214 RVA: 0x00039708 File Offset: 0x00037908
		[NetInvokableGeneratedMethod("ReceiveBuildStructure", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBuildStructure_Read(in ServerInvocationContext context)
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
			UseableStructure useableStructure = obj as UseableStructure;
			if (useableStructure == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableStructure.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableStructure));
				return;
			}
			Vector3 newPoint;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPoint, 13, 11);
			float newAngle;
			SystemNetPakReaderEx.ReadFloat(reader, ref newAngle);
			useableStructure.ReceiveBuildStructure(context, newPoint, newAngle);
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00039784 File Offset: 0x00037984
		[NetInvokableGeneratedMethod("ReceiveBuildStructure", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBuildStructure_Write(NetPakWriter writer, Vector3 newPoint, float newAngle)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, newPoint, 13, 11);
			SystemNetPakWriterEx.WriteFloat(writer, newAngle);
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0003979C File Offset: 0x0003799C
		[NetInvokableGeneratedMethod("ReceivePlayConstruct", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayConstruct_Read(in ClientInvocationContext context)
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
			UseableStructure useableStructure = obj as UseableStructure;
			if (useableStructure == null)
			{
				return;
			}
			useableStructure.ReceivePlayConstruct();
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x000397DB File Offset: 0x000379DB
		[NetInvokableGeneratedMethod("ReceivePlayConstruct", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayConstruct_Write(NetPakWriter writer)
		{
		}
	}
}
