using System;
using System.Diagnostics;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// Optional parameter for error logging and responding to the invoker.
	/// </summary>
	// Token: 0x02000258 RID: 600
	public readonly struct ServerInvocationContext
	{
		// Token: 0x0600121F RID: 4639 RVA: 0x0003E608 File Offset: 0x0003C808
		internal bool IsOwnerOf(SteamChannel legacyComponent)
		{
			return legacyComponent.owner != null && legacyComponent.owner == this.callingPlayer;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0003E622 File Offset: 0x0003C822
		public Player GetPlayer()
		{
			SteamPlayer steamPlayer = this.callingPlayer;
			if (steamPlayer == null)
			{
				return null;
			}
			return steamPlayer.player;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0003E635 File Offset: 0x0003C835
		public SteamPlayer GetCallingPlayer()
		{
			return this.callingPlayer;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0003E63D File Offset: 0x0003C83D
		public ITransportConnection GetTransportConnection()
		{
			return this.callingPlayer.transportConnection;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0003E64A File Offset: 0x0003C84A
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("DEBUG_NETINVOKABLES")]
		public void ReadParameterFailed(string parameterName)
		{
			CommandWindow.LogWarningFormat("{0} {1}: unable to read {2}", new object[]
			{
				this.GetTransportConnection(),
				this.serverMethodInfo,
				parameterName
			});
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0003E672 File Offset: 0x0003C872
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("DEBUG_NETINVOKABLES")]
		public void LogWarning(string message)
		{
			CommandWindow.LogWarningFormat("{0} {1}: {2}", new object[]
			{
				this.GetTransportConnection(),
				this.serverMethodInfo,
				message
			});
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0003E69A File Offset: 0x0003C89A
		public void Kick(string reason)
		{
			if (this.callingPlayer != null)
			{
				Provider.kick(this.callingPlayer.playerID.steamID, reason);
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0003E6BA File Offset: 0x0003C8BA
		[Obsolete("Only exists for plugins manually calling obsolete RPCs with steamID sender parameter. Do not use directly. Will remove.")]
		internal static ServerInvocationContext FromSteamIDForBackwardsCompatibility(CSteamID steamID)
		{
			return new ServerInvocationContext(steamID);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0003E6C2 File Offset: 0x0003C8C2
		internal ServerInvocationContext(ServerInvocationContext.EOrigin origin, SteamPlayer callingPlayer, NetPakReader reader, ServerMethodInfo serverMethodInfo)
		{
			this.origin = origin;
			this.callingPlayer = callingPlayer;
			this.reader = reader;
			this.serverMethodInfo = serverMethodInfo;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0003E6E1 File Offset: 0x0003C8E1
		[Obsolete("Only exists for plugins manually calling obsolete RPCs with steamID sender parameter.")]
		private ServerInvocationContext(CSteamID steamID)
		{
			this.origin = ServerInvocationContext.EOrigin.Obsolete;
			this.callingPlayer = PlayerTool.getSteamPlayer(steamID);
			this.reader = null;
			this.serverMethodInfo = null;
		}

		// Token: 0x040005AD RID: 1453
		public readonly ServerInvocationContext.EOrigin origin;

		// Token: 0x040005AE RID: 1454
		public readonly NetPakReader reader;

		// Token: 0x040005AF RID: 1455
		private readonly SteamPlayer callingPlayer;

		// Token: 0x040005B0 RID: 1456
		private readonly ServerMethodInfo serverMethodInfo;

		// Token: 0x020008E1 RID: 2273
		public enum EOrigin
		{
			// Token: 0x040031FE RID: 12798
			Remote,
			// Token: 0x040031FF RID: 12799
			Loopback,
			// Token: 0x04003200 RID: 12800
			Obsolete
		}
	}
}
