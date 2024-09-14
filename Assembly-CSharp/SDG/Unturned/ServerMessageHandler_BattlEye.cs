using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000279 RID: 633
	internal static class ServerMessageHandler_BattlEye
	{
		// Token: 0x06001283 RID: 4739 RVA: 0x00040CCC File Offset: 0x0003EECC
		internal unsafe static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			if (Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnReceivedPacket != null)
			{
				SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
				if (steamPlayer != null)
				{
					uint num;
					reader.ReadBits(Provider.battlEyeBufferSize.bitCount, ref num);
					byte[] array;
					int num2;
					if (num > 0U && reader.ReadBytesPtr((int)num, ref array, ref num2))
					{
						byte[] array2;
						byte* ptr;
						if ((array2 = array) == null || array2.Length == 0)
						{
							ptr = null;
						}
						else
						{
							ptr = &array2[0];
						}
						IntPtr intPtr;
						intPtr..ctor((void*)(ptr + num2));
						Provider.battlEyeServerRunData.pfnReceivedPacket.Invoke(steamPlayer.battlEyeId, intPtr, (int)num);
						array2 = null;
						return;
					}
					UnturnedLog.warn("Received empty BattlEye payload from {0}, so we're refusing them", new object[]
					{
						transportConnection
					});
					Provider.refuseGarbageConnection(transportConnection, "sv empty BE payload");
					return;
				}
				else if (NetMessages.shouldLogBadMessages)
				{
					UnturnedLog.info(string.Format("Ignoring BattlEye message from {0} because there is no associated player", transportConnection));
					return;
				}
			}
			else if (NetMessages.shouldLogBadMessages)
			{
				UnturnedLog.info(string.Format("Ignoring BattlEye message from {0} because BattlEye is not running", transportConnection));
			}
		}
	}
}
