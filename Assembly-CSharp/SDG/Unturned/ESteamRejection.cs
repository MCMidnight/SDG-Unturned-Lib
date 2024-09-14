using System;

namespace SDG.Unturned
{
	// Token: 0x0200066C RID: 1644
	public enum ESteamRejection
	{
		// Token: 0x0400202A RID: 8234
		SERVER_FULL,
		// Token: 0x0400202B RID: 8235
		WRONG_HASH_LEVEL,
		// Token: 0x0400202C RID: 8236
		WRONG_HASH_ASSEMBLY,
		// Token: 0x0400202D RID: 8237
		WRONG_VERSION,
		// Token: 0x0400202E RID: 8238
		WRONG_PASSWORD,
		// Token: 0x0400202F RID: 8239
		NAME_PLAYER_SHORT,
		// Token: 0x04002030 RID: 8240
		NAME_PLAYER_LONG,
		/// <summary>
		/// If modifying usage please update support article:
		/// https://support.smartlydressedgames.com/hc/en-us/articles/13452208765716
		/// </summary>
		// Token: 0x04002031 RID: 8241
		NAME_PLAYER_INVALID,
		// Token: 0x04002032 RID: 8242
		NAME_PLAYER_NUMBER,
		// Token: 0x04002033 RID: 8243
		NAME_CHARACTER_SHORT,
		// Token: 0x04002034 RID: 8244
		NAME_CHARACTER_LONG,
		/// <summary>
		/// If modifying usage please update support article:
		/// https://support.smartlydressedgames.com/hc/en-us/articles/13452208765716
		/// </summary>
		// Token: 0x04002035 RID: 8245
		NAME_CHARACTER_INVALID,
		// Token: 0x04002036 RID: 8246
		NAME_CHARACTER_NUMBER,
		// Token: 0x04002037 RID: 8247
		PRO_SERVER,
		// Token: 0x04002038 RID: 8248
		PRO_CHARACTER,
		// Token: 0x04002039 RID: 8249
		PRO_DESYNC,
		// Token: 0x0400203A RID: 8250
		PRO_APPEARANCE,
		// Token: 0x0400203B RID: 8251
		ALREADY_PENDING,
		// Token: 0x0400203C RID: 8252
		ALREADY_CONNECTED,
		// Token: 0x0400203D RID: 8253
		NOT_PENDING,
		// Token: 0x0400203E RID: 8254
		LATE_PENDING,
		// Token: 0x0400203F RID: 8255
		WHITELISTED,
		// Token: 0x04002040 RID: 8256
		AUTH_VERIFICATION,
		// Token: 0x04002041 RID: 8257
		AUTH_NO_STEAM,
		// Token: 0x04002042 RID: 8258
		AUTH_LICENSE_EXPIRED,
		// Token: 0x04002043 RID: 8259
		AUTH_VAC_BAN,
		// Token: 0x04002044 RID: 8260
		AUTH_ELSEWHERE,
		// Token: 0x04002045 RID: 8261
		AUTH_TIMED_OUT,
		// Token: 0x04002046 RID: 8262
		AUTH_USED,
		// Token: 0x04002047 RID: 8263
		AUTH_NO_USER,
		// Token: 0x04002048 RID: 8264
		AUTH_PUB_BAN,
		// Token: 0x04002049 RID: 8265
		AUTH_ECON_DESERIALIZE,
		// Token: 0x0400204A RID: 8266
		AUTH_ECON_VERIFY,
		// Token: 0x0400204B RID: 8267
		PING,
		// Token: 0x0400204C RID: 8268
		PLUGIN,
		/// <summary>
		/// Client has a critical module the server doesn't.
		/// </summary>
		// Token: 0x0400204D RID: 8269
		CLIENT_MODULE_DESYNC,
		/// <summary>
		/// Server has a critical module the client doesn't.
		/// </summary>
		// Token: 0x0400204E RID: 8270
		SERVER_MODULE_DESYNC,
		/// <summary>
		/// Level config's version number does not match.
		/// </summary>
		// Token: 0x0400204F RID: 8271
		WRONG_LEVEL_VERSION,
		/// <summary>
		/// EconInfo.json hash does not match.
		/// </summary>
		// Token: 0x04002050 RID: 8272
		WRONG_HASH_ECON,
		/// <summary>
		/// Master bundle hashes do not match.
		/// </summary>
		// Token: 0x04002051 RID: 8273
		WRONG_HASH_MASTER_BUNDLE,
		/// <summary>
		/// Server has not received an auth session response from Steam yet.
		/// </summary>
		// Token: 0x04002052 RID: 8274
		LATE_PENDING_STEAM_AUTH,
		/// <summary>
		/// Server has not received an economy response from Steam yet.
		/// </summary>
		// Token: 0x04002053 RID: 8275
		LATE_PENDING_STEAM_ECON,
		/// <summary>
		/// Server has not received a groups response from Steam yet.
		/// </summary>
		// Token: 0x04002054 RID: 8276
		LATE_PENDING_STEAM_GROUPS,
		/// <summary>
		/// Player nickname exceeds limit.
		/// </summary>
		// Token: 0x04002055 RID: 8277
		NAME_PRIVATE_LONG,
		/// <summary>
		/// Player nickname contains invalid characters.
		///
		/// If modifying usage please update support article:
		/// https://support.smartlydressedgames.com/hc/en-us/articles/13452208765716
		/// </summary>
		// Token: 0x04002056 RID: 8278
		NAME_PRIVATE_INVALID,
		/// <summary>
		/// Player nickname should not be a number.
		/// </summary>
		// Token: 0x04002057 RID: 8279
		NAME_PRIVATE_NUMBER,
		/// <summary>
		/// Player resources folders don't match.
		/// </summary>
		// Token: 0x04002058 RID: 8280
		WRONG_HASH_RESOURCES,
		/// <summary>
		/// The network identity in the ticket does not match the server authenticating the ticket.
		/// This can happen if server's Steam ID has changed from what the client thinks it is.
		/// For example, joining a stale entry in the server list. (public issue #4101)
		/// </summary>
		// Token: 0x04002059 RID: 8281
		AUTH_NETWORK_IDENTITY_FAILURE,
		/// <summary>
		/// Player's skin color is too similar to one of <see cref="F:SDG.Unturned.LevelAsset.terrainColorRules" />.
		/// </summary>
		// Token: 0x0400205A RID: 8282
		SKIN_COLOR_WITHIN_THRESHOLD_OF_TERRAIN_COLOR
	}
}
