using System;

namespace SDG.Unturned
{
	/// <summary>
	/// When adding or removing entries remember to update NetMessages size and regenerate NetCode!
	/// </summary>
	// Token: 0x02000265 RID: 613
	public enum EClientMessage
	{
		// Token: 0x040005BE RID: 1470
		UPDATE_RELIABLE_BUFFER,
		// Token: 0x040005BF RID: 1471
		UPDATE_UNRELIABLE_BUFFER,
		/// <summary>
		/// Server sent a ping.
		/// </summary>
		// Token: 0x040005C0 RID: 1472
		PingRequest,
		/// <summary>
		/// Server replying to our ping.
		/// </summary>
		// Token: 0x040005C1 RID: 1473
		PingResponse,
		/// <summary>
		/// Server is shutting down shortly.
		/// </summary>
		// Token: 0x040005C2 RID: 1474
		Shutdown,
		/// <summary>
		/// Create game object for player.
		/// </summary>
		// Token: 0x040005C3 RID: 1475
		PlayerConnected,
		/// <summary>
		/// Destroy game object for player.
		/// </summary>
		// Token: 0x040005C4 RID: 1476
		PlayerDisconnected,
		/// <summary>
		/// Download these files before loading the level.
		/// </summary>
		// Token: 0x040005C5 RID: 1477
		DownloadWorkshopFiles,
		/// <summary>
		/// Server wants additional info before accepting us.
		/// </summary>
		// Token: 0x040005C6 RID: 1478
		Verify,
		/// <summary>
		/// Server has accepted us and will create a player game object.
		/// </summary>
		// Token: 0x040005C7 RID: 1479
		Accepted,
		/// <summary>
		/// Server rejected us, we will go back to the menu.
		/// </summary>
		// Token: 0x040005C8 RID: 1480
		Rejected,
		/// <summary>
		/// Banned either during connect or gameplay.
		/// </summary>
		// Token: 0x040005C9 RID: 1481
		Banned,
		/// <summary>
		/// Kicked during gameplay.
		/// </summary>
		// Token: 0x040005CA RID: 1482
		Kicked,
		/// <summary>
		/// Should be converted to an RPC. Leftover from prior to net messaging code.
		/// </summary>
		// Token: 0x040005CB RID: 1483
		Admined,
		/// <summary>
		/// Should be converted to an RPC. Leftover from prior to net messaging code.
		/// </summary>
		// Token: 0x040005CC RID: 1484
		Unadmined,
		/// <summary>
		/// Server sending BattlEye payload to client.
		/// </summary>
		// Token: 0x040005CD RID: 1485
		BattlEye,
		/// <summary>
		/// Infrequent notification of queue position.
		/// </summary>
		// Token: 0x040005CE RID: 1486
		QueuePositionChanged,
		/// <summary>
		/// Server calling an RPC.
		/// </summary>
		// Token: 0x040005CF RID: 1487
		InvokeMethod
	}
}
