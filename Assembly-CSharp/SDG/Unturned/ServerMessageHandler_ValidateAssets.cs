using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows file name to be included in kick message that client would otherwise not know.
	/// </summary>
	// Token: 0x02000280 RID: 640
	internal static class ServerMessageHandler_ValidateAssets
	{
		// Token: 0x06001290 RID: 4752 RVA: 0x00041DBC File Offset: 0x0003FFBC
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
			if (steamPlayer == null)
			{
				if (NetMessages.shouldLogBadMessages)
				{
					UnturnedLog.info(string.Format("Ignoring ValidateAssets message from {0} because there is no associated player", transportConnection));
				}
				return;
			}
			uint num;
			if (!reader.ReadBits(ServerMessageHandler_ValidateAssets.MAX_ASSETS.bitCount, ref num))
			{
				Provider.kick(steamPlayer.playerID.steamID, "ValidateAssets unable to read itemCountBits");
				return;
			}
			int num2 = (int)num;
			if ((long)num2 > (long)((ulong)ServerMessageHandler_ValidateAssets.MAX_ASSETS.value))
			{
				Provider.kick(steamPlayer.playerID.steamID, "ValidateAssets invalid itemCount");
				return;
			}
			num2++;
			uint num3;
			if (!reader.ReadBits(num2, ref num3))
			{
				Provider.kick(steamPlayer.playerID.steamID, "ValidateAssets unable to read hasHashFlags");
				return;
			}
			for (int i = 0; i < num2; i++)
			{
				Guid guid;
				if (!SystemNetPakReaderEx.ReadGuid(reader, ref guid))
				{
					Provider.kick(steamPlayer.playerID.steamID, "ValidateAssets unable to read guid");
					return;
				}
				if (guid == Guid.Empty)
				{
					Provider.kick(steamPlayer.playerID.steamID, "ValidateAssets empty guid");
					return;
				}
				if (!steamPlayer.validatedGuids.Add(guid))
				{
					Provider.kick(steamPlayer.playerID.steamID, "ValidateAssets duplicate guid");
					return;
				}
				bool flag = (num3 & 1U << i) > 0U;
				if (flag && !reader.ReadBytes(ServerMessageHandler_ValidateAssets.clientHash))
				{
					Provider.kick(steamPlayer.playerID.steamID, "ValidateAssets unable to read clientHash");
					return;
				}
				if (!ClientAssetIntegrity.serverKnownMissingGuids.Contains(guid))
				{
					Asset asset = Assets.find(guid);
					if (asset == null)
					{
						if (Assets.shouldLoadAnyAssets)
						{
							UnturnedLog.info(string.Format("Kicking {0} for invalid file integrity request guid: {1:N}", transportConnection, guid));
							Assets.SendKickForInvalidGuid.Invoke(ENetReliability.Reliable, transportConnection, guid);
							Provider.dismiss(steamPlayer.playerID.steamID);
						}
						return;
					}
					if (asset.ShouldVerifyHash)
					{
						if (flag)
						{
							byte[] array = asset.hash;
							if (asset.originMasterBundle != null && asset.originMasterBundle.serverHashes != null)
							{
								byte[] platformHash = asset.originMasterBundle.serverHashes.GetPlatformHash(steamPlayer.clientPlatform);
								if (platformHash != null)
								{
									array = Hash.combine(new byte[][]
									{
										array,
										platformHash
									});
								}
							}
							if (!Hash.verifyHash(ServerMessageHandler_ValidateAssets.clientHash, array))
							{
								AssetOrigin origin = asset.origin;
								string text = (origin != null) ? origin.name : null;
								if (string.IsNullOrEmpty(text))
								{
									text = "Unknown";
								}
								UnturnedLog.info(string.Format("Kicking {0} for asset hash mismatch: \"{1}\" Type: {2} File: \"{3}\" Id: {4:N} Client: {5} Server: {6}", new object[]
								{
									transportConnection,
									asset.FriendlyName,
									asset.GetTypeFriendlyName(),
									asset.name,
									guid,
									Hash.toString(ServerMessageHandler_ValidateAssets.clientHash),
									Hash.toString(array)
								}));
								ClientStaticMethod<Guid, string, string, byte[], string, string> sendKickForHashMismatch = Assets.SendKickForHashMismatch;
								ENetReliability reliability = ENetReliability.Reliable;
								Guid arg = guid;
								string name = asset.name;
								string friendlyName = asset.FriendlyName;
								byte[] arg2 = array;
								MasterBundleConfig originMasterBundle = asset.originMasterBundle;
								sendKickForHashMismatch.Invoke(reliability, transportConnection, arg, name, friendlyName, arg2, (originMasterBundle != null) ? originMasterBundle.assetBundleNameWithoutExtension : null, text);
								Provider.dismiss(steamPlayer.playerID.steamID);
								return;
							}
						}
						else if (asset.hash != null && asset.hash.Length == 20)
						{
							Provider.kick(steamPlayer.playerID.steamID, string.Format("missing asset: \"{0}\" File: \"{1}\" Id: {2:N}", asset.FriendlyName, asset.name, guid));
							return;
						}
					}
				}
			}
		}

		/// <summary>
		/// Actual max value is plus one because message never contains zero items.
		/// </summary>
		// Token: 0x040005E8 RID: 1512
		internal static readonly NetLength MAX_ASSETS = new NetLength(7U);

		// Token: 0x040005E9 RID: 1513
		private static byte[] clientHash = new byte[20];
	}
}
