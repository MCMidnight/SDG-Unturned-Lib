using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000271 RID: 625
	internal static class ClientMessageHandler_QueuePositionChanged
	{
		// Token: 0x06001272 RID: 4722 RVA: 0x0003FECC File Offset: 0x0003E0CC
		internal static void ReadMessage(NetPakReader reader)
		{
			if (Provider.isWaitingForConnectResponse)
			{
				Provider.isWaitingForConnectResponse = false;
				UnturnedLog.info("Connection pending verification");
			}
			byte queuePosition = Provider.queuePosition;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			Provider._queuePosition = b;
			if (queuePosition != b)
			{
				UnturnedLog.info("Queue position: {0}", new object[]
				{
					Provider.queuePosition
				});
			}
			Provider.QueuePositionUpdated onQueuePositionUpdated = Provider.onQueuePositionUpdated;
			if (onQueuePositionUpdated == null)
			{
				return;
			}
			onQueuePositionUpdated();
		}
	}
}
