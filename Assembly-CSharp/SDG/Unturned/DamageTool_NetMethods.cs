using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001D6 RID: 470
	[NetInvokableGeneratedClass(typeof(DamageTool))]
	public static class DamageTool_NetMethods
	{
		// Token: 0x06000E39 RID: 3641 RVA: 0x00031944 File Offset: 0x0002FB44
		[NetInvokableGeneratedMethod("ReceiveSpawnBulletImpact", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSpawnBulletImpact_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			Vector3 normal;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref normal, 9);
			string materialName;
			SystemNetPakReaderEx.ReadString(reader, ref materialName, 11);
			Transform colliderTransform;
			reader.ReadTransform(out colliderTransform);
			NetId instigatorNetId;
			reader.ReadNetId(out instigatorNetId);
			DamageTool.ReceiveSpawnBulletImpact(position, normal, materialName, colliderTransform, instigatorNetId);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00031995 File Offset: 0x0002FB95
		[NetInvokableGeneratedMethod("ReceiveSpawnBulletImpact", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSpawnBulletImpact_Write(NetPakWriter writer, Vector3 position, Vector3 normal, string materialName, Transform colliderTransform, NetId instigatorNetId)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, normal, 9);
			SystemNetPakWriterEx.WriteString(writer, materialName, 11);
			writer.WriteTransform(colliderTransform);
			writer.WriteNetId(instigatorNetId);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x000319C8 File Offset: 0x0002FBC8
		[NetInvokableGeneratedMethod("ReceiveSpawnLegacyImpact", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSpawnLegacyImpact_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			Vector3 normal;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref normal, 9);
			string materialName;
			SystemNetPakReaderEx.ReadString(reader, ref materialName, 11);
			Transform colliderTransform;
			reader.ReadTransform(out colliderTransform);
			DamageTool.ReceiveSpawnLegacyImpact(position, normal, materialName, colliderTransform);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00031A0E File Offset: 0x0002FC0E
		[NetInvokableGeneratedMethod("ReceiveSpawnLegacyImpact", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSpawnLegacyImpact_Write(NetPakWriter writer, Vector3 position, Vector3 normal, string materialName, Transform colliderTransform)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, normal, 9);
			SystemNetPakWriterEx.WriteString(writer, materialName, 11);
			writer.WriteTransform(colliderTransform);
		}
	}
}
