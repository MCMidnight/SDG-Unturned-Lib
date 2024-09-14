using System;

namespace SDG.Unturned
{
	/// <summary>
	/// It is useful to be able to reference transforms generically over the network, for example to attach a bullet
	/// hole to a tree or vehicle without tagging it as a tree or vehicle, but most entities placed in the level do not
	/// have unique IDs. To work around this we count downward from uint.MaxValue for level objects to avoid conflicts
	/// with server-assigned net ids.
	/// </summary>
	// Token: 0x020004EC RID: 1260
	internal static class LevelNetIdRegistry
	{
		/// <summary>
		/// Each region can have ushort.MaxValue trees, and we reserve that entire block so that a region can be slightly
		/// modified on the client or server without breaking all netids in the level.
		/// </summary>
		// Token: 0x0600272F RID: 10031 RVA: 0x000A2D53 File Offset: 0x000A0F53
		public static NetId GetTreeNetId(byte regionX, byte regionY, ushort index)
		{
			return new NetId((uint)(int.MinValue | (int)regionX << 22 | (int)regionY << 16 | (int)index));
		}

		/// <summary>
		/// Each region can have ushort.MaxValue objects, and we reserve that entire block so that a region can be slightly
		/// modified on the client or server without breaking all netids in the level.
		/// </summary>
		// Token: 0x06002730 RID: 10032 RVA: 0x000A2D6B File Offset: 0x000A0F6B
		public static NetId GetRegularObjectNetId(byte regionX, byte regionY, ushort index)
		{
			return new NetId((uint)(1073741824 | (int)regionX << 22 | (int)regionY << 16 | (int)index));
		}

		/// <summary>
		/// Devkit instance IDs should already be fairly stable. There is no way any level is using more than 30 bits
		/// for the instance ID, so it should be safe to set those bits to prevent collisions with server net IDs.
		/// </summary>
		// Token: 0x06002731 RID: 10033 RVA: 0x000A2D83 File Offset: 0x000A0F83
		public static NetId GetDevkitObjectNetId(uint instanceId)
		{
			return new NetId(3221225472U | instanceId);
		}

		// Token: 0x040014C6 RID: 5318
		private const uint TREE_FLAG = 2147483648U;

		// Token: 0x040014C7 RID: 5319
		private const uint REGULAR_OBJECT_FLAG = 1073741824U;

		// Token: 0x040014C8 RID: 5320
		private const uint DEVKIT_OBJECT_FLAG = 3221225472U;
	}
}
