using System;

namespace SDG.Unturned
{
	// Token: 0x0200066A RID: 1642
	public enum ESteamConnectionFailureInfo
	{
		// Token: 0x04001FC2 RID: 8130
		NONE,
		// Token: 0x04001FC3 RID: 8131
		SHUTDOWN,
		// Token: 0x04001FC4 RID: 8132
		MAP,
		// Token: 0x04001FC5 RID: 8133
		BANNED,
		// Token: 0x04001FC6 RID: 8134
		KICKED,
		// Token: 0x04001FC7 RID: 8135
		WHITELISTED,
		// Token: 0x04001FC8 RID: 8136
		PASSWORD,
		// Token: 0x04001FC9 RID: 8137
		FULL,
		// Token: 0x04001FCA RID: 8138
		HASH_LEVEL,
		// Token: 0x04001FCB RID: 8139
		HASH_ASSEMBLY,
		// Token: 0x04001FCC RID: 8140
		VERSION,
		// Token: 0x04001FCD RID: 8141
		PRO_SERVER,
		// Token: 0x04001FCE RID: 8142
		PRO_CHARACTER,
		// Token: 0x04001FCF RID: 8143
		PRO_DESYNC,
		// Token: 0x04001FD0 RID: 8144
		PRO_APPEARANCE,
		// Token: 0x04001FD1 RID: 8145
		AUTH_VERIFICATION,
		// Token: 0x04001FD2 RID: 8146
		AUTH_NO_STEAM,
		// Token: 0x04001FD3 RID: 8147
		AUTH_LICENSE_EXPIRED,
		// Token: 0x04001FD4 RID: 8148
		AUTH_VAC_BAN,
		// Token: 0x04001FD5 RID: 8149
		AUTH_ELSEWHERE,
		// Token: 0x04001FD6 RID: 8150
		AUTH_TIMED_OUT,
		// Token: 0x04001FD7 RID: 8151
		AUTH_USED,
		// Token: 0x04001FD8 RID: 8152
		AUTH_NO_USER,
		// Token: 0x04001FD9 RID: 8153
		AUTH_PUB_BAN,
		// Token: 0x04001FDA RID: 8154
		AUTH_ECON_SERIALIZE,
		// Token: 0x04001FDB RID: 8155
		AUTH_ECON_DESERIALIZE,
		// Token: 0x04001FDC RID: 8156
		AUTH_ECON_VERIFY,
		// Token: 0x04001FDD RID: 8157
		AUTH_EMPTY,
		// Token: 0x04001FDE RID: 8158
		ALREADY_CONNECTED,
		// Token: 0x04001FDF RID: 8159
		ALREADY_PENDING,
		// Token: 0x04001FE0 RID: 8160
		LATE_PENDING,
		// Token: 0x04001FE1 RID: 8161
		NOT_PENDING,
		// Token: 0x04001FE2 RID: 8162
		NAME_PLAYER_SHORT,
		// Token: 0x04001FE3 RID: 8163
		NAME_PLAYER_LONG,
		// Token: 0x04001FE4 RID: 8164
		NAME_PLAYER_INVALID,
		// Token: 0x04001FE5 RID: 8165
		NAME_PLAYER_NUMBER,
		// Token: 0x04001FE6 RID: 8166
		NAME_CHARACTER_SHORT,
		// Token: 0x04001FE7 RID: 8167
		NAME_CHARACTER_LONG,
		// Token: 0x04001FE8 RID: 8168
		NAME_CHARACTER_INVALID,
		// Token: 0x04001FE9 RID: 8169
		NAME_CHARACTER_NUMBER,
		// Token: 0x04001FEA RID: 8170
		TIMED_OUT,
		// Token: 0x04001FEB RID: 8171
		PING,
		// Token: 0x04001FEC RID: 8172
		PLUGIN,
		// Token: 0x04001FED RID: 8173
		BARRICADE,
		// Token: 0x04001FEE RID: 8174
		STRUCTURE,
		// Token: 0x04001FEF RID: 8175
		VEHICLE,
		// Token: 0x04001FF0 RID: 8176
		CLIENT_MODULE_DESYNC,
		// Token: 0x04001FF1 RID: 8177
		SERVER_MODULE_DESYNC,
		// Token: 0x04001FF2 RID: 8178
		BATTLEYE_BROKEN,
		// Token: 0x04001FF3 RID: 8179
		BATTLEYE_UPDATE,
		// Token: 0x04001FF4 RID: 8180
		BATTLEYE_UNKNOWN,
		// Token: 0x04001FF5 RID: 8181
		LEVEL_VERSION,
		/// <summary>
		/// EconInfo.json hash does not match.
		/// </summary>
		// Token: 0x04001FF6 RID: 8182
		ECON_HASH,
		/// <summary>
		/// Master bundle hashes do not match.
		/// </summary>
		// Token: 0x04001FF7 RID: 8183
		HASH_MASTER_BUNDLE,
		// Token: 0x04001FF8 RID: 8184
		REJECT_UNKNOWN,
		// Token: 0x04001FF9 RID: 8185
		WORKSHOP_DOWNLOAD_RESTRICTION,
		/// <summary>
		/// Workshop usage advertised on server list does not match during connect.
		/// </summary>
		// Token: 0x04001FFA RID: 8186
		WORKSHOP_ADVERTISEMENT_MISMATCH,
		/// <summary>
		/// Used by client transport to show a custom localized message.
		/// </summary>
		// Token: 0x04001FFB RID: 8187
		CUSTOM,
		/// <summary>
		/// Server has not received an auth session response from Steam yet.
		/// </summary>
		// Token: 0x04001FFC RID: 8188
		LATE_PENDING_STEAM_AUTH,
		/// <summary>
		/// Server has not received an economy response from Steam yet.
		/// </summary>
		// Token: 0x04001FFD RID: 8189
		LATE_PENDING_STEAM_ECON,
		/// <summary>
		/// Server has not received a groups response from Steam yet.
		/// </summary>
		// Token: 0x04001FFE RID: 8190
		LATE_PENDING_STEAM_GROUPS,
		/// <summary>
		/// Player nickname exceeds limit.
		/// </summary>
		// Token: 0x04001FFF RID: 8191
		NAME_PRIVATE_LONG,
		/// <summary>
		/// Player nickname contains invalid characters.
		/// </summary>
		// Token: 0x04002000 RID: 8192
		NAME_PRIVATE_INVALID,
		/// <summary>
		/// Player nickname should not be a number.
		/// </summary>
		// Token: 0x04002001 RID: 8193
		NAME_PRIVATE_NUMBER,
		/// <summary>
		/// Server did not respond to EServerMessage.Authenticate
		/// </summary>
		// Token: 0x04002002 RID: 8194
		TIMED_OUT_LOGIN,
		/// <summary>
		/// Player resources folders don't match.
		/// </summary>
		// Token: 0x04002003 RID: 8195
		HASH_RESOURCES,
		/// <summary>
		/// The network identity in the ticket does not match the server authenticating the ticket.
		/// This can happen if server's Steam ID has changed from what the client thinks it is.
		/// For example, joining a stale entry in the server list. (public issue #4101)
		/// </summary>
		// Token: 0x04002004 RID: 8196
		AUTH_NETWORK_IDENTITY_FAILURE,
		/// <summary>
		/// Level name advertised on server list does not match during connect.
		/// </summary>
		// Token: 0x04002005 RID: 8197
		SERVER_MAP_ADVERTISEMENT_MISMATCH,
		/// <summary>
		/// VAC status advertised on server list does not match during connect.
		/// </summary>
		// Token: 0x04002006 RID: 8198
		SERVER_VAC_ADVERTISEMENT_MISMATCH,
		/// <summary>
		/// BattlEye status advertised on server list does not match during connect.
		/// </summary>
		// Token: 0x04002007 RID: 8199
		SERVER_BATTLEYE_ADVERTISEMENT_MISMATCH,
		/// <summary>
		/// Max players advertised on server list does not match during connect.
		/// </summary>
		// Token: 0x04002008 RID: 8200
		SERVER_MAXPLAYERS_ADVERTISEMENT_MISMATCH,
		/// <summary>
		/// Camera mode advertised on server list does not match during connect.
		/// </summary>
		// Token: 0x04002009 RID: 8201
		SERVER_CAMERAMODE_ADVERTISEMENT_MISMATCH,
		/// <summary>
		/// Combat mode advertised on server list does not match during connect.
		/// </summary>
		// Token: 0x0400200A RID: 8202
		SERVER_PVP_ADVERTISEMENT_MISMATCH,
		/// <summary>
		/// Player's skin color is too similar to one of <see cref="F:SDG.Unturned.LevelAsset.terrainColorRules" />.
		/// </summary>
		// Token: 0x0400200B RID: 8203
		SKIN_COLOR_WITHIN_THRESHOLD_OF_TERRAIN_COLOR
	}
}
