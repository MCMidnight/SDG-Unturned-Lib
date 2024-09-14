using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000205 RID: 517
	[NetInvokableGeneratedClass(typeof(SteamPlayer))]
	public static class SteamPlayer_NetMethods
	{
		// Token: 0x06001016 RID: 4118 RVA: 0x0003824C File Offset: 0x0003644C
		[NetInvokableGeneratedMethod("ReceiveGetSteamAuthTicketForWebApiRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveGetSteamAuthTicketForWebApiRequest_Read(in ClientInvocationContext context)
		{
			string identity;
			SystemNetPakReaderEx.ReadString(context.reader, ref identity, 11);
			SteamPlayer.ReceiveGetSteamAuthTicketForWebApiRequest(identity);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003826F File Offset: 0x0003646F
		[NetInvokableGeneratedMethod("ReceiveGetSteamAuthTicketForWebApiRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveGetSteamAuthTicketForWebApiRequest_Write(NetPakWriter writer, string identity)
		{
			SystemNetPakWriterEx.WriteString(writer, identity, 11);
		}
	}
}
