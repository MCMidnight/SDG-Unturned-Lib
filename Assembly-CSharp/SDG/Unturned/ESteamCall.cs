using System;

namespace SDG.Unturned
{
	// Token: 0x02000669 RID: 1641
	public enum ESteamCall
	{
		/// <summary>
		/// Replaced by ServerMethodHandle.
		/// </summary>
		// Token: 0x04001FBA RID: 8122
		[Obsolete]
		SERVER,
		/// <summary>
		/// Replaced by ClientInstanceMethod.InvokeAndLoopback or ClientStaticMethod.InvokeAndLoopback.
		/// </summary>
		// Token: 0x04001FBB RID: 8123
		[Obsolete]
		ALL,
		/// <summary>
		/// Replaced by ClientMethodHandle invoked with Provider.EnumerateClients_Remote.
		/// Unlike ESteamCall.CLIENTS this is not loopback invoked.
		/// </summary>
		// Token: 0x04001FBC RID: 8124
		[Obsolete]
		OTHERS,
		/// <summary>
		/// Replaced by ClientMethodHandle invoked with SteamChannel.GetOwnerTransportConnection.
		/// </summary>
		// Token: 0x04001FBD RID: 8125
		[Obsolete]
		OWNER,
		/// <summary>
		/// Replaced by ClientMethodHandle invoked with SteamChannel.EnumerateClients_RemoteNotOwner.
		/// </summary>
		// Token: 0x04001FBE RID: 8126
		[Obsolete]
		NOT_OWNER,
		/// <summary>
		/// Replaced by ClientMethodHandle invoked with Provider.EnumerateClients.
		/// Unlike ESteamCall.OTHERS this will be loopback invoked in singleplayer or listen server.
		/// </summary>
		// Token: 0x04001FBF RID: 8127
		[Obsolete]
		CLIENTS,
		/// <summary>
		/// May have been used by voice in early versions, but has been completely removed.
		/// </summary>
		// Token: 0x04001FC0 RID: 8128
		[Obsolete]
		PEERS
	}
}
