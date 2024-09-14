using System;
using System.IO;
using SDG.Provider.Services.Community;

namespace SDG.Provider.Services.Multiplayer.Client
{
	// Token: 0x02000059 RID: 89
	public interface IClientMultiplayerService : IService
	{
		/// <summary>
		/// Information about currently connected server.
		/// </summary>
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000217 RID: 535
		IServerInfo serverInfo { get; }

		/// <summary>
		/// Whether a server is currently connected to.
		/// </summary>
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000218 RID: 536
		bool isConnected { get; }

		/// <summary>
		/// Whether connection attempts are being made.
		/// </summary>
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000219 RID: 537
		bool isAttempting { get; }

		/// <summary>
		/// Network buffer memory stream.
		/// </summary>
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600021A RID: 538
		MemoryStream stream { get; }

		/// <summary>
		/// Network buffer memory stream reader.
		/// </summary>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600021B RID: 539
		BinaryReader reader { get; }

		/// <summary>
		/// Network buffer memory stream writer.
		/// </summary>
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600021C RID: 540
		BinaryWriter writer { get; }

		/// <summary>
		/// Connect to a server.
		/// </summary>
		/// <param name="newServerInfo">Server to join.</param>
		// Token: 0x0600021D RID: 541
		void connect(IServerInfo newServerInfo);

		/// <summary>
		/// Disconnect from current server.
		/// </summary>
		// Token: 0x0600021E RID: 542
		void disconnect();

		/// <summary>
		/// Receive a packet from an entity across the network.
		/// </summary>
		/// <param name="entity">Sender of data.</param>
		/// <param name="data"></param>
		/// <param name="length"></param>
		/// <returns>Whether any data was read.</returns>
		// Token: 0x0600021F RID: 543
		bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel);

		/// <summary>
		/// Send a packet to an entity across the network.
		/// </summary>
		/// <param name="entity">Recipient of data.</param>
		/// <param name="data">Packet to send.</param>
		/// <param name="length">Size of data in array.</param>
		// Token: 0x06000220 RID: 544
		void write(ICommunityEntity entity, byte[] data, ulong length);

		/// <summary>
		/// Send a packet to an entity across the network.
		/// </summary>
		/// <param name="entity">Recipient of data.</param>
		/// <param name="data">Packet to send.</param>
		/// <param name="length">Size of data in array.</param>
		/// <param name="method">Type of send to use.</param>
		// Token: 0x06000221 RID: 545
		[Obsolete("Used by old multiplayer code, please use send without method instead.")]
		void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel);
	}
}
