using System;
using System.IO;
using SDG.Provider.Services.Community;

namespace SDG.Provider.Services.Multiplayer.Server
{
	// Token: 0x02000058 RID: 88
	public interface IServerMultiplayerService : IService
	{
		/// <summary>
		/// Information about currently hosted server.
		/// </summary>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600020B RID: 523
		IServerInfo serverInfo { get; }

		/// <summary>
		/// Whether a server is open.
		/// </summary>
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600020C RID: 524
		bool isHosting { get; }

		/// <summary>
		/// Network buffer memory stream.
		/// </summary>
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600020D RID: 525
		MemoryStream stream { get; }

		/// <summary>
		/// Network buffer memory stream reader.
		/// </summary>
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600020E RID: 526
		BinaryReader reader { get; }

		/// <summary>
		/// Network buffer memory stream writer.
		/// </summary>
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600020F RID: 527
		BinaryWriter writer { get; }

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000210 RID: 528
		// (remove) Token: 0x06000211 RID: 529
		event ServerMultiplayerServiceReadyHandler ready;

		/// <summary>
		/// Open a new server.
		/// </summary>
		// Token: 0x06000212 RID: 530
		void open(uint ip, ushort port, ESecurityMode security);

		/// <summary>
		/// Close an existing server.
		/// </summary>
		// Token: 0x06000213 RID: 531
		void close();

		/// <summary>
		/// Receive a packet from an entity across the network.
		/// </summary>
		/// <param name="entity">Sender of data.</param>
		/// <param name="data"></param>
		/// <param name="length"></param>
		/// <returns>Whether any data was read.</returns>
		// Token: 0x06000214 RID: 532
		bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel);

		/// <summary>
		/// Send a packet to an entity across the network.
		/// </summary>
		/// <param name="entity">Recipient of data.</param>
		/// <param name="data">Packet to send.</param>
		/// <param name="length">Size of data in array.</param>
		// Token: 0x06000215 RID: 533
		void write(ICommunityEntity entity, byte[] data, ulong length);

		/// <summary>
		/// Send a packet to an entity across the network.
		/// </summary>
		/// <param name="entity">Recipient of data.</param>
		/// <param name="data">Packet to send.</param>
		/// <param name="length">Size of data in array.</param>
		/// <param name="method">Type of send to use.</param>
		// Token: 0x06000216 RID: 534
		[Obsolete("Used by old multiplayer code, please use send without method instead.")]
		void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel);
	}
}
