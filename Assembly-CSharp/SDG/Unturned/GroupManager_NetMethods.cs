using System;
using SDG.NetPak;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020001D8 RID: 472
	[NetInvokableGeneratedClass(typeof(GroupManager))]
	public static class GroupManager_NetMethods
	{
		// Token: 0x06000E69 RID: 3689 RVA: 0x000321CC File Offset: 0x000303CC
		[NetInvokableGeneratedMethod("ReceiveGroupInfo", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveGroupInfo_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			CSteamID groupID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref groupID);
			string name;
			SystemNetPakReaderEx.ReadString(reader, ref name, 11);
			uint members;
			SystemNetPakReaderEx.ReadUInt32(reader, ref members);
			GroupManager.ReceiveGroupInfo(groupID, name, members);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00032203 File Offset: 0x00030403
		[NetInvokableGeneratedMethod("ReceiveGroupInfo", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveGroupInfo_Write(NetPakWriter writer, CSteamID groupID, string name, uint members)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, groupID);
			SystemNetPakWriterEx.WriteString(writer, name, 11);
			SystemNetPakWriterEx.WriteUInt32(writer, members);
		}
	}
}
