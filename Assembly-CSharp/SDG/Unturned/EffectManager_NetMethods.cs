using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001D7 RID: 471
	[NetInvokableGeneratedClass(typeof(EffectManager))]
	public static class EffectManager_NetMethods
	{
		// Token: 0x06000E3D RID: 3645 RVA: 0x00031A38 File Offset: 0x0002FC38
		[NetInvokableGeneratedMethod("ReceiveEffectClearById", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectClearById_Read(in ClientInvocationContext context)
		{
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref id);
			EffectManager.ReceiveEffectClearById(id);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00031A59 File Offset: 0x0002FC59
		[NetInvokableGeneratedMethod("ReceiveEffectClearById", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectClearById_Write(NetPakWriter writer, ushort id)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00031A64 File Offset: 0x0002FC64
		[NetInvokableGeneratedMethod("ReceiveEffectClearByGuid", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectClearByGuid_Read(in ClientInvocationContext context)
		{
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(context.reader, ref assetGuid);
			EffectManager.ReceiveEffectClearByGuid(assetGuid);
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00031A85 File Offset: 0x0002FC85
		[NetInvokableGeneratedMethod("ReceiveEffectClearByGuid", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectClearByGuid_Write(NetPakWriter writer, Guid assetGuid)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00031A8F File Offset: 0x0002FC8F
		[NetInvokableGeneratedMethod("ReceiveEffectClearAll", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectClearAll_Read(in ClientInvocationContext context)
		{
			EffectManager.ReceiveEffectClearAll();
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00031A96 File Offset: 0x0002FC96
		[NetInvokableGeneratedMethod("ReceiveEffectClearAll", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectClearAll_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00031A98 File Offset: 0x0002FC98
		[NetInvokableGeneratedMethod("ReceiveEffectPointNormal_NonUniformScale", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPointNormal_NonUniformScale_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			Vector3 normal;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref normal, 9);
			Vector3 scale;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref scale, 13, 7);
			EffectManager.ReceiveEffectPointNormal_NonUniformScale(assetGuid, point, normal, scale);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00031ADF File Offset: 0x0002FCDF
		[NetInvokableGeneratedMethod("ReceiveEffectPointNormal_NonUniformScale", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPointNormal_NonUniformScale_Write(NetPakWriter writer, Guid assetGuid, Vector3 point, Vector3 normal, Vector3 scale)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, normal, 9);
			UnityNetPakWriterEx.WriteClampedVector3(writer, scale, 13, 7);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00031B0C File Offset: 0x0002FD0C
		[NetInvokableGeneratedMethod("ReceiveEffectPointNormal_UniformScale", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPointNormal_UniformScale_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			Vector3 normal;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref normal, 9);
			float uniformScale;
			SystemNetPakReaderEx.ReadFloat(reader, ref uniformScale);
			EffectManager.ReceiveEffectPointNormal_UniformScale(assetGuid, point, normal, uniformScale);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00031B50 File Offset: 0x0002FD50
		[NetInvokableGeneratedMethod("ReceiveEffectPointNormal_UniformScale", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPointNormal_UniformScale_Write(NetPakWriter writer, Guid assetGuid, Vector3 point, Vector3 normal, float uniformScale)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, normal, 9);
			SystemNetPakWriterEx.WriteFloat(writer, uniformScale);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00031B78 File Offset: 0x0002FD78
		[NetInvokableGeneratedMethod("ReceiveEffectPointNormal", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPointNormal_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			Vector3 normal;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref normal, 9);
			EffectManager.ReceiveEffectPointNormal(assetGuid, point, normal);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00031BB2 File Offset: 0x0002FDB2
		[NetInvokableGeneratedMethod("ReceiveEffectPointNormal", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPointNormal_Write(NetPakWriter writer, Guid assetGuid, Vector3 point, Vector3 normal)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, normal, 9);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00031BD4 File Offset: 0x0002FDD4
		[NetInvokableGeneratedMethod("ReceiveEffectPoint_NonUniformScale", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPoint_NonUniformScale_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			Vector3 scale;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref scale, 13, 7);
			EffectManager.ReceiveEffectPoint_NonUniformScale(assetGuid, point, scale);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00031C0F File Offset: 0x0002FE0F
		[NetInvokableGeneratedMethod("ReceiveEffectPoint_NonUniformScale", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPoint_NonUniformScale_Write(NetPakWriter writer, Guid assetGuid, Vector3 point, Vector3 scale)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
			UnityNetPakWriterEx.WriteClampedVector3(writer, scale, 13, 7);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00031C30 File Offset: 0x0002FE30
		[NetInvokableGeneratedMethod("ReceiveEffectPoint_UniformScale", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPoint_UniformScale_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			float uniformScale;
			SystemNetPakReaderEx.ReadFloat(reader, ref uniformScale);
			EffectManager.ReceiveEffectPoint_UniformScale(assetGuid, point, uniformScale);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00031C68 File Offset: 0x0002FE68
		[NetInvokableGeneratedMethod("ReceiveEffectPoint_UniformScale", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPoint_UniformScale_Write(NetPakWriter writer, Guid assetGuid, Vector3 point, float uniformScale)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
			SystemNetPakWriterEx.WriteFloat(writer, uniformScale);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00031C88 File Offset: 0x0002FE88
		[NetInvokableGeneratedMethod("ReceiveEffectPoint", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPoint_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			EffectManager.ReceiveEffectPoint(assetGuid, point);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00031CB6 File Offset: 0x0002FEB6
		[NetInvokableGeneratedMethod("ReceiveEffectPoint", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPoint_Write(NetPakWriter writer, Guid assetGuid, Vector3 point)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00031CCC File Offset: 0x0002FECC
		[NetInvokableGeneratedMethod("ReceiveEffectPositionRotation_NonUniformScale", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPositionRotation_NonUniformScale_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref rotation, 9);
			Vector3 scale;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref scale, 13, 7);
			EffectManager.ReceiveEffectPositionRotation_NonUniformScale(assetGuid, position, rotation, scale);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00031D13 File Offset: 0x0002FF13
		[NetInvokableGeneratedMethod("ReceiveEffectPositionRotation_NonUniformScale", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPositionRotation_NonUniformScale_Write(NetPakWriter writer, Guid assetGuid, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			UnityNetPakWriterEx.WriteQuaternion(writer, rotation, 9);
			UnityNetPakWriterEx.WriteClampedVector3(writer, scale, 13, 7);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00031D40 File Offset: 0x0002FF40
		[NetInvokableGeneratedMethod("ReceiveEffectPositionRotation_UniformScale", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPositionRotation_UniformScale_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref rotation, 9);
			float uniformScale;
			SystemNetPakReaderEx.ReadFloat(reader, ref uniformScale);
			EffectManager.ReceiveEffectPositionRotation_UniformScale(assetGuid, position, rotation, uniformScale);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00031D84 File Offset: 0x0002FF84
		[NetInvokableGeneratedMethod("ReceiveEffectPositionRotation_UniformScale", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPositionRotation_UniformScale_Write(NetPakWriter writer, Guid assetGuid, Vector3 position, Quaternion rotation, float uniformScale)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			UnityNetPakWriterEx.WriteQuaternion(writer, rotation, 9);
			SystemNetPakWriterEx.WriteFloat(writer, uniformScale);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00031DAC File Offset: 0x0002FFAC
		[NetInvokableGeneratedMethod("ReceiveEffectPositionRotation", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectPositionRotation_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref rotation, 9);
			EffectManager.ReceiveEffectPositionRotation(assetGuid, position, rotation);
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00031DE6 File Offset: 0x0002FFE6
		[NetInvokableGeneratedMethod("ReceiveEffectPositionRotation", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectPositionRotation_Write(NetPakWriter writer, Guid assetGuid, Vector3 position, Quaternion rotation)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			UnityNetPakWriterEx.WriteQuaternion(writer, rotation, 9);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00031E08 File Offset: 0x00030008
		[NetInvokableGeneratedMethod("ReceiveUIEffect", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffect_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			EffectManager.ReceiveUIEffect(id, key);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00031E33 File Offset: 0x00030033
		[NetInvokableGeneratedMethod("ReceiveUIEffect", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffect_Write(NetPakWriter writer, ushort id, short key)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteInt16(writer, key);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00031E48 File Offset: 0x00030048
		[NetInvokableGeneratedMethod("ReceiveUIEffect1Arg", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffect1Arg_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			string arg;
			SystemNetPakReaderEx.ReadString(reader, ref arg, 11);
			EffectManager.ReceiveUIEffect1Arg(id, key, arg);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00031E7F File Offset: 0x0003007F
		[NetInvokableGeneratedMethod("ReceiveUIEffect1Arg", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffect1Arg_Write(NetPakWriter writer, ushort id, short key, string arg0)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteInt16(writer, key);
			SystemNetPakWriterEx.WriteString(writer, arg0, 11);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00031E9C File Offset: 0x0003009C
		[NetInvokableGeneratedMethod("ReceiveUIEffect2Args", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffect2Args_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			string arg;
			SystemNetPakReaderEx.ReadString(reader, ref arg, 11);
			string arg2;
			SystemNetPakReaderEx.ReadString(reader, ref arg2, 11);
			EffectManager.ReceiveUIEffect2Args(id, key, arg, arg2);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00031EDF File Offset: 0x000300DF
		[NetInvokableGeneratedMethod("ReceiveUIEffect2Args", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffect2Args_Write(NetPakWriter writer, ushort id, short key, string arg0, string arg1)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteInt16(writer, key);
			SystemNetPakWriterEx.WriteString(writer, arg0, 11);
			SystemNetPakWriterEx.WriteString(writer, arg1, 11);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00031F08 File Offset: 0x00030108
		[NetInvokableGeneratedMethod("ReceiveUIEffect3Args", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffect3Args_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			string arg;
			SystemNetPakReaderEx.ReadString(reader, ref arg, 11);
			string arg2;
			SystemNetPakReaderEx.ReadString(reader, ref arg2, 11);
			string arg3;
			SystemNetPakReaderEx.ReadString(reader, ref arg3, 11);
			EffectManager.ReceiveUIEffect3Args(id, key, arg, arg2, arg3);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00031F58 File Offset: 0x00030158
		[NetInvokableGeneratedMethod("ReceiveUIEffect3Args", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffect3Args_Write(NetPakWriter writer, ushort id, short key, string arg0, string arg1, string arg2)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteInt16(writer, key);
			SystemNetPakWriterEx.WriteString(writer, arg0, 11);
			SystemNetPakWriterEx.WriteString(writer, arg1, 11);
			SystemNetPakWriterEx.WriteString(writer, arg2, 11);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00031F8C File Offset: 0x0003018C
		[NetInvokableGeneratedMethod("ReceiveUIEffect4Args", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffect4Args_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			string arg;
			SystemNetPakReaderEx.ReadString(reader, ref arg, 11);
			string arg2;
			SystemNetPakReaderEx.ReadString(reader, ref arg2, 11);
			string arg3;
			SystemNetPakReaderEx.ReadString(reader, ref arg3, 11);
			string arg4;
			SystemNetPakReaderEx.ReadString(reader, ref arg4, 11);
			EffectManager.ReceiveUIEffect4Args(id, key, arg, arg2, arg3, arg4);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00031FE9 File Offset: 0x000301E9
		[NetInvokableGeneratedMethod("ReceiveUIEffect4Args", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffect4Args_Write(NetPakWriter writer, ushort id, short key, string arg0, string arg1, string arg2, string arg3)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteInt16(writer, key);
			SystemNetPakWriterEx.WriteString(writer, arg0, 11);
			SystemNetPakWriterEx.WriteString(writer, arg1, 11);
			SystemNetPakWriterEx.WriteString(writer, arg2, 11);
			SystemNetPakWriterEx.WriteString(writer, arg3, 11);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00032028 File Offset: 0x00030228
		[NetInvokableGeneratedMethod("ReceiveUIEffectVisibility", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffectVisibility_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			string childNameOrPath;
			SystemNetPakReaderEx.ReadString(reader, ref childNameOrPath, 11);
			bool visible;
			reader.ReadBit(ref visible);
			EffectManager.ReceiveUIEffectVisibility(key, childNameOrPath, visible);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0003205F File Offset: 0x0003025F
		[NetInvokableGeneratedMethod("ReceiveUIEffectVisibility", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffectVisibility_Write(NetPakWriter writer, short key, string childNameOrPath, bool visible)
		{
			SystemNetPakWriterEx.WriteInt16(writer, key);
			SystemNetPakWriterEx.WriteString(writer, childNameOrPath, 11);
			writer.WriteBit(visible);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0003207C File Offset: 0x0003027C
		[NetInvokableGeneratedMethod("ReceiveUIEffectText", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffectText_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			string childNameOrPath;
			SystemNetPakReaderEx.ReadString(reader, ref childNameOrPath, 11);
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			EffectManager.ReceiveUIEffectText(key, childNameOrPath, text);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x000320B5 File Offset: 0x000302B5
		[NetInvokableGeneratedMethod("ReceiveUIEffectText", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffectText_Write(NetPakWriter writer, short key, string childNameOrPath, string text)
		{
			SystemNetPakWriterEx.WriteInt16(writer, key);
			SystemNetPakWriterEx.WriteString(writer, childNameOrPath, 11);
			SystemNetPakWriterEx.WriteString(writer, text, 11);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000320D4 File Offset: 0x000302D4
		[NetInvokableGeneratedMethod("ReceiveUIEffectImageURL", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIEffectImageURL_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			short key;
			SystemNetPakReaderEx.ReadInt16(reader, ref key);
			string childNameOrPath;
			SystemNetPakReaderEx.ReadString(reader, ref childNameOrPath, 11);
			string url;
			SystemNetPakReaderEx.ReadString(reader, ref url, 11);
			bool shouldCache;
			reader.ReadBit(ref shouldCache);
			bool forceRefresh;
			reader.ReadBit(ref forceRefresh);
			EffectManager.ReceiveUIEffectImageURL(key, childNameOrPath, url, shouldCache, forceRefresh);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00032122 File Offset: 0x00030322
		[NetInvokableGeneratedMethod("ReceiveUIEffectImageURL", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIEffectImageURL_Write(NetPakWriter writer, short key, string childNameOrPath, string url, bool shouldCache, bool forceRefresh)
		{
			SystemNetPakWriterEx.WriteInt16(writer, key);
			SystemNetPakWriterEx.WriteString(writer, childNameOrPath, 11);
			SystemNetPakWriterEx.WriteString(writer, url, 11);
			writer.WriteBit(shouldCache);
			writer.WriteBit(forceRefresh);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00032154 File Offset: 0x00030354
		[NetInvokableGeneratedMethod("ReceiveEffectClicked", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectClicked_Read(in ServerInvocationContext context)
		{
			string buttonName;
			SystemNetPakReaderEx.ReadString(context.reader, ref buttonName, 11);
			EffectManager.ReceiveEffectClicked(context, buttonName);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00032178 File Offset: 0x00030378
		[NetInvokableGeneratedMethod("ReceiveEffectClicked", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectClicked_Write(NetPakWriter writer, string buttonName)
		{
			SystemNetPakWriterEx.WriteString(writer, buttonName, 11);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00032184 File Offset: 0x00030384
		[NetInvokableGeneratedMethod("ReceiveEffectTextCommitted", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEffectTextCommitted_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			string inputFieldName;
			SystemNetPakReaderEx.ReadString(reader, ref inputFieldName, 11);
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			EffectManager.ReceiveEffectTextCommitted(context, inputFieldName, text);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000321B4 File Offset: 0x000303B4
		[NetInvokableGeneratedMethod("ReceiveEffectTextCommitted", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEffectTextCommitted_Write(NetPakWriter writer, string inputFieldName, string text)
		{
			SystemNetPakWriterEx.WriteString(writer, inputFieldName, 11);
			SystemNetPakWriterEx.WriteString(writer, text, 11);
		}
	}
}
