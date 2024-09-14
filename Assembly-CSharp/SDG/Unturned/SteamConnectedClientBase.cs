using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Some new code common to SteamPending and SteamPlayer.
	/// </summary>
	// Token: 0x02000691 RID: 1681
	public class SteamConnectedClientBase
	{
		/// <summary>
		/// Number of ping requests the server has received from this client.
		/// </summary>
		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060038A8 RID: 14504 RVA: 0x0010B8D7 File Offset: 0x00109AD7
		// (set) Token: 0x060038A9 RID: 14505 RVA: 0x0010B8DF File Offset: 0x00109ADF
		public int numPingRequestsReceived { get; private set; }

		/// <summary>
		/// Called when a ping request is received from this client.
		/// </summary>
		// Token: 0x060038AA RID: 14506 RVA: 0x0010B8E8 File Offset: 0x00109AE8
		public void incrementNumPingRequestsReceived()
		{
			if (this.numPingRequestsReceived == 0)
			{
				this.firstPingRequestRealtime = Time.realtimeSinceStartup;
			}
			int numPingRequestsReceived = this.numPingRequestsReceived;
			this.numPingRequestsReceived = numPingRequestsReceived + 1;
		}

		/// <summary>
		/// Realtime passed since the first ping request was received from this client.
		/// </summary>
		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060038AB RID: 14507 RVA: 0x0010B918 File Offset: 0x00109B18
		public float realtimeSinceFirstPingRequest
		{
			get
			{
				return Time.realtimeSinceStartup - this.firstPingRequestRealtime;
			}
		}

		/// <summary>
		/// Average number of ping requests received from this client per second.
		/// Begins tracking 10 seconds after the first ping request was received, or -1 if average is unknown yet.
		/// </summary>
		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060038AC RID: 14508 RVA: 0x0010B928 File Offset: 0x00109B28
		public float averagePingRequestsReceivedPerSecond
		{
			get
			{
				if (this.numPingRequestsReceived < 1)
				{
					return -1f;
				}
				float realtimeSinceFirstPingRequest = this.realtimeSinceFirstPingRequest;
				if (realtimeSinceFirstPingRequest < 10f)
				{
					return -1f;
				}
				return (float)this.numPingRequestsReceived / realtimeSinceFirstPingRequest;
			}
		}

		/// <summary>
		/// Only set on server. Associates player with their connection.
		/// </summary>
		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060038AD RID: 14509 RVA: 0x0010B962 File Offset: 0x00109B62
		// (set) Token: 0x060038AE RID: 14510 RVA: 0x0010B96A File Offset: 0x00109B6A
		public ITransportConnection transportConnection { get; protected set; }

		/// <summary>
		/// Realtime the first ping request was received.
		/// </summary>
		// Token: 0x04002187 RID: 8583
		private float firstPingRequestRealtime;
	}
}
