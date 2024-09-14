using System;

namespace SDG.Unturned
{
	// Token: 0x02000810 RID: 2064
	public class RayMasks
	{
		// Token: 0x04002FB4 RID: 12212
		public const int DEFAULT = 1;

		// Token: 0x04002FB5 RID: 12213
		public const int TRANSPARENT_FX = 2;

		// Token: 0x04002FB6 RID: 12214
		public const int IGNORE_RAYCAST = 4;

		// Token: 0x04002FB7 RID: 12215
		public const int WATER = 16;

		// Token: 0x04002FB8 RID: 12216
		public const int UI = 32;

		// Token: 0x04002FB9 RID: 12217
		public const int LOGIC = 256;

		// Token: 0x04002FBA RID: 12218
		public const int PLAYER = 512;

		// Token: 0x04002FBB RID: 12219
		public const int ENEMY = 1024;

		// Token: 0x04002FBC RID: 12220
		public const int VIEWMODEL = 2048;

		// Token: 0x04002FBD RID: 12221
		public const int DEBRIS = 4096;

		// Token: 0x04002FBE RID: 12222
		public const int ITEM = 8192;

		// Token: 0x04002FBF RID: 12223
		public const int RESOURCE = 16384;

		// Token: 0x04002FC0 RID: 12224
		public const int LARGE = 32768;

		// Token: 0x04002FC1 RID: 12225
		public const int MEDIUM = 65536;

		// Token: 0x04002FC2 RID: 12226
		public const int SMALL = 131072;

		// Token: 0x04002FC3 RID: 12227
		public const int SKY = 262144;

		// Token: 0x04002FC4 RID: 12228
		public const int ENVIRONMENT = 524288;

		// Token: 0x04002FC5 RID: 12229
		public const int GROUND = 1048576;

		// Token: 0x04002FC6 RID: 12230
		public const int CLIP = 2097152;

		// Token: 0x04002FC7 RID: 12231
		public const int NAVMESH = 4194304;

		// Token: 0x04002FC8 RID: 12232
		public const int ENTITY = 8388608;

		// Token: 0x04002FC9 RID: 12233
		public const int AGENT = 16777216;

		// Token: 0x04002FCA RID: 12234
		public const int LADDER = 33554432;

		// Token: 0x04002FCB RID: 12235
		public const int VEHICLE = 67108864;

		// Token: 0x04002FCC RID: 12236
		public const int BARRICADE = 134217728;

		// Token: 0x04002FCD RID: 12237
		public const int STRUCTURE = 268435456;

		// Token: 0x04002FCE RID: 12238
		public const int TIRE = 536870912;

		// Token: 0x04002FCF RID: 12239
		public const int TRAP = 1073741824;

		// Token: 0x04002FD0 RID: 12240
		public const int GROUND2 = -2147483648;

		// Token: 0x04002FD1 RID: 12241
		public static readonly int REFLECTION = 1687552;

		// Token: 0x04002FD2 RID: 12242
		public static readonly int CHART = 1687552;

		// Token: 0x04002FD3 RID: 12243
		public static readonly int FOLIAGE_FOCUS = -2146336768;

		// Token: 0x04002FD4 RID: 12244
		public static readonly int POWER_INTERACT = 134463488;

		// Token: 0x04002FD5 RID: 12245
		public static readonly int BARRICADE_INTERACT = 471449600;

		// Token: 0x04002FD6 RID: 12246
		public static readonly int STRUCTURE_INTERACT = 270123008;

		// Token: 0x04002FD7 RID: 12247
		public static readonly int ROOFS_INTERACT = -1877360640;

		// Token: 0x04002FD8 RID: 12248
		public static readonly int CORNERS_INTERACT = 270385152;

		// Token: 0x04002FD9 RID: 12249
		public static readonly int WALLS_INTERACT = 270123040;

		// Token: 0x04002FDA RID: 12250
		public static readonly int LADDERS_INTERACT = 337231874;

		// Token: 0x04002FDB RID: 12251
		public static readonly int SLOTS_INTERACT = 505004288;

		// Token: 0x04002FDC RID: 12252
		public static readonly int LADDER_INTERACT = 505004032;

		// Token: 0x04002FDD RID: 12253
		public static readonly int CLOTHING_INTERACT = 9728;

		// Token: 0x04002FDE RID: 12254
		public static readonly int PLAYER_INTERACT = 505144321;

		// Token: 0x04002FDF RID: 12255
		public static readonly int EDITOR_INTERACT = 402882560;

		// Token: 0x04002FE0 RID: 12256
		public static readonly int EDITOR_WORLD = -1743536128;

		// Token: 0x04002FE1 RID: 12257
		public static readonly int EDITOR_LOGIC = 262400;

		// Token: 0x04002FE2 RID: 12258
		public static readonly int EDITOR_VR = -2146320384;

		// Token: 0x04002FE3 RID: 12259
		public static readonly int EDITOR_BUILDABLE = 402669568;

		// Token: 0x04002FE4 RID: 12260
		public static readonly int BLOCK_LADDER = 473546752;

		// Token: 0x04002FE5 RID: 12261
		public static readonly int BLOCK_PICKUP = 402751488;

		// Token: 0x04002FE6 RID: 12262
		public static readonly int BLOCK_LASER = 479847424;

		// Token: 0x04002FE7 RID: 12263
		public static readonly int BLOCK_RESOURCE = 638976;

		// Token: 0x04002FE8 RID: 12264
		public static readonly int BLOCK_ITEM = 404340736;

		// Token: 0x04002FE9 RID: 12265
		public static readonly int BLOCK_VEHICLE = 1671168;

		/// <summary>
		/// Used to test whether player can fit in a space.
		/// Includes terrain because tested capsule could be slightly underground, and clip to prevent exploits at sky limit.
		/// </summary>
		// Token: 0x04002FEA RID: 12266
		public static readonly int BLOCK_STANCE = 473546752;

		// Token: 0x04002FEB RID: 12267
		public static readonly int BLOCK_NAVMESH = 4734976;

		// Token: 0x04002FEC RID: 12268
		public static readonly int BLOCK_KILLCAM = 471384064;

		// Token: 0x04002FED RID: 12269
		public static readonly int BLOCK_PLAYERCAM = 471384064;

		// Token: 0x04002FEE RID: 12270
		public static readonly int BLOCK_PLAYERCAM_1P = 470335488;

		/// <summary>
		/// Used for third-person camera in vehicle.
		/// Does not include resource layer because attached barricades are put on that layer.
		/// Barricades layer itself is included to prevent looking inside player bases.
		/// </summary>
		// Token: 0x04002FEF RID: 12271
		public static readonly int BLOCK_VEHICLECAM = 404258816;

		// Token: 0x04002FF0 RID: 12272
		public static readonly int BLOCK_VISION = 98304;

		// Token: 0x04002FF1 RID: 12273
		public static readonly int BLOCK_COLLISION = 473546752;

		// Token: 0x04002FF2 RID: 12274
		public static readonly int BLOCK_GRASS = 622592;

		// Token: 0x04002FF3 RID: 12275
		public static readonly int BLOCK_LEAN = RayMasks.BLOCK_STANCE;

		/// <summary>
		/// Used to test whether player can enter a vehicle.
		/// Does not include resource layer because attached barricades are put on that layer.
		/// </summary>
		// Token: 0x04002FF4 RID: 12276
		public static readonly int BLOCK_ENTRY = 405897216;

		// Token: 0x04002FF5 RID: 12277
		public static readonly int BLOCK_EXIT = 406437888;

		// Token: 0x04002FF6 RID: 12278
		public static readonly int BLOCK_EXIT_FIND_GROUND = 473546752;

		// Token: 0x04002FF7 RID: 12279
		public static readonly int BLOCK_BARRICADE_INTERACT_LOS = 268533760;

		// Token: 0x04002FF8 RID: 12280
		public static readonly int BLOCK_TIRE = 3784704;

		// Token: 0x04002FF9 RID: 12281
		public static readonly int BLOCK_BARRICADE = 403293696;

		// Token: 0x04002FFA RID: 12282
		public static readonly int BLOCK_DOOR_OPENING = 403293696;

		// Token: 0x04002FFB RID: 12283
		public static readonly int BLOCK_BED_LOS = RayMasks.BLOCK_ITEM;

		// Token: 0x04002FFC RID: 12284
		[Obsolete]
		public static readonly int BLOCK_STRUCTURE = 469763584;

		// Token: 0x04002FFD RID: 12285
		public static readonly int BLOCK_EXPLOSION = 403816448;

		// Token: 0x04002FFE RID: 12286
		public static readonly int BLOCK_EXPLOSION_PENETRATE_BUILDABLES = 1163264;

		// Token: 0x04002FFF RID: 12287
		public static readonly int BLOCK_WIND = 402685952;

		// Token: 0x04003000 RID: 12288
		public static readonly int BLOCK_FRAME = 134217732;

		// Token: 0x04003001 RID: 12289
		public static readonly int BLOCK_WINDOW = 134217732;

		// Token: 0x04003002 RID: 12290
		public static readonly int BLOCK_SENTRY = 471449600;

		// Token: 0x04003003 RID: 12291
		public static readonly int BLOCK_CHAR_BUILDABLE_OVERLAP = 512;

		// Token: 0x04003004 RID: 12292
		public static readonly int BLOCK_CHAR_BUILDABLE_OVERLAP_NOT_ON_VEHICLE = 67109376;

		// Token: 0x04003005 RID: 12293
		public static readonly int BLOCK_CHAR_HINGE_OVERLAP = 67109376;

		// Token: 0x04003006 RID: 12294
		public static readonly int BLOCK_CHAR_HINGE_OVERLAP_ON_VEHICLE = 512;

		// Token: 0x04003007 RID: 12295
		public static readonly int BLOCK_TRAIN = 469860352;

		// Token: 0x04003008 RID: 12296
		public static readonly int WAYPOINT = 404340736;

		// Token: 0x04003009 RID: 12297
		public static readonly int DAMAGE_PHYSICS = 471449600;

		// Token: 0x0400300A RID: 12298
		public static readonly int DAMAGE_CLIENT = 479970304;

		// Token: 0x0400300B RID: 12299
		public static readonly int DAMAGE_SERVER = 404340736;

		// Token: 0x0400300C RID: 12300
		public static readonly int DAMAGE_ZOMBIE = 469762048;

		// Token: 0x0400300D RID: 12301
		[Obsolete("Replaced by EFFECT_SPLATTER to make const")]
		public static readonly int SPLATTER = 1671168;

		/// <summary>
		/// 2023-02-02: adding more layers since splatter can be attached to them now.
		/// parent should only be set if that system also calls ClearAttachments, otherwise attachedEffects will leak memory.
		/// </summary>
		// Token: 0x0400300E RID: 12302
		public const int EFFECT_SPLATTER = 471433216;

		/// <summary>
		/// Layer mask for CharacterController overlap test.
		/// </summary>
		// Token: 0x0400300F RID: 12303
		public const int CHARACTER_CONTROLLER_MOVE = 406437888;

		/// <summary>
		/// Layer mask for CharacterController overlap test while inside landscape hole volume.
		/// </summary>
		// Token: 0x04003010 RID: 12304
		public const int CHARACTER_CONTROLLER_MOVE_IGNORE_GROUND = 405389312;

		/// <summary>
		/// Lightning strike raycasts from sky to ground using this layer mask.
		/// </summary>
		// Token: 0x04003011 RID: 12305
		public const int LIGHTNING = 471449600;
	}
}
