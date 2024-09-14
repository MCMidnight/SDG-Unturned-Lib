using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000215 RID: 533
	[NetInvokableGeneratedClass(typeof(UseableHousingPlanner))]
	public static class UseableHousingPlanner_NetMethods
	{
		// Token: 0x06001064 RID: 4196 RVA: 0x0003939C File Offset: 0x0003759C
		[NetInvokableGeneratedMethod("ReceivePlaceHousingItemResult", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlaceHousingItemResult_Read(in ClientInvocationContext context)
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
			UseableHousingPlanner useableHousingPlanner = obj as UseableHousingPlanner;
			if (useableHousingPlanner == null)
			{
				return;
			}
			bool success;
			reader.ReadBit(ref success);
			useableHousingPlanner.ReceivePlaceHousingItemResult(success);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000393E8 File Offset: 0x000375E8
		[NetInvokableGeneratedMethod("ReceivePlaceHousingItemResult", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlaceHousingItemResult_Write(NetPakWriter writer, bool success)
		{
			writer.WriteBit(success);
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000393F4 File Offset: 0x000375F4
		[NetInvokableGeneratedMethod("ReceivePlaceHousingItem", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlaceHousingItem_Read(in ServerInvocationContext context)
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
			UseableHousingPlanner useableHousingPlanner = obj as UseableHousingPlanner;
			if (useableHousingPlanner == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableHousingPlanner.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableHousingPlanner));
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 11);
			float yaw;
			SystemNetPakReaderEx.ReadFloat(reader, ref yaw);
			useableHousingPlanner.ReceivePlaceHousingItem(context, assetGuid, position, yaw);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0003947B File Offset: 0x0003767B
		[NetInvokableGeneratedMethod("ReceivePlaceHousingItem", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlaceHousingItem_Write(NetPakWriter writer, Guid assetGuid, Vector3 position, float yaw)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 11);
			SystemNetPakWriterEx.WriteFloat(writer, yaw);
		}
	}
}
