using System;

namespace SDG.Unturned
{
	/// <summary>
	/// When adding or removing entries remember to update NetMessages size and regenerate NetCode!
	/// </summary>
	// Token: 0x02000277 RID: 631
	public enum EServerMessage
	{
		/// <summary>
		/// Client requesting workshop files to download.
		/// </summary>
		// Token: 0x040005DD RID: 1501
		GetWorkshopFiles,
		/// <summary>
		/// Client has loaded the level.
		/// </summary>
		// Token: 0x040005DE RID: 1502
		ReadyToConnect,
		/// <summary>
		/// Client providing Steam login token.
		/// </summary>
		// Token: 0x040005DF RID: 1503
		Authenticate,
		/// <summary>
		/// Client sending BattlEye payload to server.
		/// </summary>
		// Token: 0x040005E0 RID: 1504
		BattlEye,
		/// <summary>
		/// Client sent a ping.
		/// </summary>
		// Token: 0x040005E1 RID: 1505
		PingRequest,
		/// <summary>
		/// Client responded to our ping.
		/// </summary>
		// Token: 0x040005E2 RID: 1506
		PingResponse,
		/// <summary>
		/// Client calling an RPC.
		/// </summary>
		// Token: 0x040005E3 RID: 1507
		InvokeMethod,
		/// <summary>
		/// Client providing asset GUIDs with their file hashes to check integrity.
		/// </summary>
		// Token: 0x040005E4 RID: 1508
		ValidateAssets,
		/// <summary>
		/// Client intends to disconnect. It is fine if server does not receive this message
		/// because players are also removed for transport failure (e.g. timeout) and for expiry
		/// of Steam authentication ticket. This message is useful to know the client instigated
		/// the disconnection rather than an error.
		/// </summary>
		// Token: 0x040005E5 RID: 1509
		GracefullyDisconnect
	}
}
