using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001D2 RID: 466
	[NetInvokableGeneratedClass(typeof(Assets))]
	public static class Assets_NetMethods
	{
		// Token: 0x06000E10 RID: 3600 RVA: 0x00031100 File Offset: 0x0002F300
		[NetInvokableGeneratedMethod("ReceiveKickForInvalidGuid", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveKickForInvalidGuid_Read(in ClientInvocationContext context)
		{
			Guid guid;
			SystemNetPakReaderEx.ReadGuid(context.reader, ref guid);
			Assets.ReceiveKickForInvalidGuid(guid);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00031121 File Offset: 0x0002F321
		[NetInvokableGeneratedMethod("ReceiveKickForInvalidGuid", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveKickForInvalidGuid_Write(NetPakWriter writer, Guid guid)
		{
			SystemNetPakWriterEx.WriteGuid(writer, guid);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0003112C File Offset: 0x0002F32C
		[NetInvokableGeneratedMethod("ReceiveKickForHashMismatch", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveKickForHashMismatch_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid guid;
			SystemNetPakReaderEx.ReadGuid(reader, ref guid);
			string serverName;
			SystemNetPakReaderEx.ReadString(reader, ref serverName, 11);
			string serverFriendlyName;
			SystemNetPakReaderEx.ReadString(reader, ref serverFriendlyName, 11);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			string serverAssetBundleNameWithoutExtension;
			SystemNetPakReaderEx.ReadString(reader, ref serverAssetBundleNameWithoutExtension, 11);
			string serverAssetOrigin;
			SystemNetPakReaderEx.ReadString(reader, ref serverAssetOrigin, 11);
			Assets.ReceiveKickForHashMismatch(guid, serverName, serverFriendlyName, array, serverAssetBundleNameWithoutExtension, serverAssetOrigin);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0003119C File Offset: 0x0002F39C
		[NetInvokableGeneratedMethod("ReceiveKickForHashMismatch", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveKickForHashMismatch_Write(NetPakWriter writer, Guid guid, string serverName, string serverFriendlyName, byte[] serverHash, string serverAssetBundleNameWithoutExtension, string serverAssetOrigin)
		{
			SystemNetPakWriterEx.WriteGuid(writer, guid);
			SystemNetPakWriterEx.WriteString(writer, serverName, 11);
			SystemNetPakWriterEx.WriteString(writer, serverFriendlyName, 11);
			byte b = (byte)serverHash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(serverHash, (int)b);
			SystemNetPakWriterEx.WriteString(writer, serverAssetBundleNameWithoutExtension, 11);
			SystemNetPakWriterEx.WriteString(writer, serverAssetOrigin, 11);
		}
	}
}
