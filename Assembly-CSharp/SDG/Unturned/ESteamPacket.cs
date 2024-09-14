using System;

namespace SDG.Unturned
{
	// Token: 0x0200066B RID: 1643
	[Obsolete]
	public enum ESteamPacket
	{
		// Token: 0x0400200D RID: 8205
		[Obsolete]
		UPDATE_RELIABLE_BUFFER,
		// Token: 0x0400200E RID: 8206
		[Obsolete]
		UPDATE_UNRELIABLE_BUFFER,
		// Token: 0x0400200F RID: 8207
		[Obsolete]
		UPDATE_RELIABLE_CHUNK_BUFFER,
		// Token: 0x04002010 RID: 8208
		[Obsolete]
		UPDATE_UNRELIABLE_CHUNK_BUFFER,
		// Token: 0x04002011 RID: 8209
		[Obsolete]
		UPDATE_VOICE,
		// Token: 0x04002012 RID: 8210
		[Obsolete]
		SHUTDOWN,
		// Token: 0x04002013 RID: 8211
		[Obsolete]
		WORKSHOP,
		// Token: 0x04002014 RID: 8212
		[Obsolete]
		CONNECT,
		// Token: 0x04002015 RID: 8213
		[Obsolete]
		VERIFY,
		// Token: 0x04002016 RID: 8214
		[Obsolete]
		AUTHENTICATE,
		// Token: 0x04002017 RID: 8215
		[Obsolete]
		REJECTED,
		// Token: 0x04002018 RID: 8216
		[Obsolete]
		ACCEPTED,
		// Token: 0x04002019 RID: 8217
		[Obsolete]
		ADMINED,
		// Token: 0x0400201A RID: 8218
		[Obsolete]
		UNADMINED,
		// Token: 0x0400201B RID: 8219
		[Obsolete]
		BANNED,
		// Token: 0x0400201C RID: 8220
		[Obsolete]
		KICKED,
		// Token: 0x0400201D RID: 8221
		[Obsolete]
		CONNECTED,
		// Token: 0x0400201E RID: 8222
		[Obsolete]
		DISCONNECTED,
		// Token: 0x0400201F RID: 8223
		[Obsolete]
		PING_REQUEST,
		// Token: 0x04002020 RID: 8224
		[Obsolete]
		PING_RESPONSE,
		// Token: 0x04002021 RID: 8225
		[Obsolete("Unused and will kick sender.")]
		UPDATE_RELIABLE_INSTANT,
		// Token: 0x04002022 RID: 8226
		[Obsolete("Unused and will kick sender.")]
		UPDATE_UNRELIABLE_INSTANT,
		// Token: 0x04002023 RID: 8227
		[Obsolete("Unused and will kick sender.")]
		UPDATE_RELIABLE_CHUNK_INSTANT,
		// Token: 0x04002024 RID: 8228
		[Obsolete("Unused and will kick sender.")]
		UPDATE_UNRELIABLE_CHUNK_INSTANT,
		// Token: 0x04002025 RID: 8229
		[Obsolete]
		BATTLEYE,
		// Token: 0x04002026 RID: 8230
		[Obsolete]
		GUIDTABLE,
		/// <summary>
		/// Server response to a non-rejected CONNECT request. Notifies client they are in the queue.
		/// </summary>
		// Token: 0x04002027 RID: 8231
		[Obsolete]
		CLIENT_PENDING,
		// Token: 0x04002028 RID: 8232
		[Obsolete]
		MAX
	}
}
