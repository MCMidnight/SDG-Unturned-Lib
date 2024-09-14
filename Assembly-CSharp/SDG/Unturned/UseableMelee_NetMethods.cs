using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000216 RID: 534
	[NetInvokableGeneratedClass(typeof(UseableMelee))]
	public static class UseableMelee_NetMethods
	{
		// Token: 0x06001068 RID: 4200 RVA: 0x0003949C File Offset: 0x0003769C
		[NetInvokableGeneratedMethod("ReceiveSpawnMeleeImpact", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSpawnMeleeImpact_Read(in ClientInvocationContext context)
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
			UseableMelee useableMelee = obj as UseableMelee;
			if (useableMelee == null)
			{
				return;
			}
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			Vector3 normal;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref normal, 9);
			string materialName;
			SystemNetPakReaderEx.ReadString(reader, ref materialName, 11);
			Transform colliderTransform;
			reader.ReadTransform(out colliderTransform);
			useableMelee.ReceiveSpawnMeleeImpact(position, normal, materialName, colliderTransform);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00039510 File Offset: 0x00037710
		[NetInvokableGeneratedMethod("ReceiveSpawnMeleeImpact", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSpawnMeleeImpact_Write(NetPakWriter writer, Vector3 position, Vector3 normal, string materialName, Transform colliderTransform)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, normal, 9);
			SystemNetPakWriterEx.WriteString(writer, materialName, 11);
			writer.WriteTransform(colliderTransform);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0003953C File Offset: 0x0003773C
		[NetInvokableGeneratedMethod("ReceiveInteractMelee", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveInteractMelee_Read(in ServerInvocationContext context)
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
			UseableMelee useableMelee = obj as UseableMelee;
			if (useableMelee == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableMelee.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableMelee));
				return;
			}
			useableMelee.ReceiveInteractMelee();
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0003959B File Offset: 0x0003779B
		[NetInvokableGeneratedMethod("ReceiveInteractMelee", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveInteractMelee_Write(NetPakWriter writer)
		{
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x000395A0 File Offset: 0x000377A0
		[NetInvokableGeneratedMethod("ReceivePlaySwingStart", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlaySwingStart_Read(in ClientInvocationContext context)
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
			UseableMelee useableMelee = obj as UseableMelee;
			if (useableMelee == null)
			{
				return;
			}
			useableMelee.ReceivePlaySwingStart();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x000395DF File Offset: 0x000377DF
		[NetInvokableGeneratedMethod("ReceivePlaySwingStart", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlaySwingStart_Write(NetPakWriter writer)
		{
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x000395E4 File Offset: 0x000377E4
		[NetInvokableGeneratedMethod("ReceivePlaySwingStop", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlaySwingStop_Read(in ClientInvocationContext context)
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
			UseableMelee useableMelee = obj as UseableMelee;
			if (useableMelee == null)
			{
				return;
			}
			useableMelee.ReceivePlaySwingStop();
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00039623 File Offset: 0x00037823
		[NetInvokableGeneratedMethod("ReceivePlaySwingStop", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlaySwingStop_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00039628 File Offset: 0x00037828
		[NetInvokableGeneratedMethod("ReceivePlaySwing", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlaySwing_Read(in ClientInvocationContext context)
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
			UseableMelee useableMelee = obj as UseableMelee;
			if (useableMelee == null)
			{
				return;
			}
			ESwingMode mode;
			reader.ReadEnum(out mode);
			useableMelee.ReceivePlaySwing(mode);
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00039674 File Offset: 0x00037874
		[NetInvokableGeneratedMethod("ReceivePlaySwing", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlaySwing_Write(NetPakWriter writer, ESwingMode mode)
		{
			writer.WriteEnum(mode);
		}
	}
}
