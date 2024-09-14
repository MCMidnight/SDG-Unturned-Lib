using System;
using System.Collections.Generic;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006C1 RID: 1729
	public class Regions
	{
		// Token: 0x060039A9 RID: 14761 RVA: 0x0010E3D4 File Offset: 0x0010C5D4
		public static void getRegionsInRadius(Vector3 center, float radius, List<RegionCoordinate> result)
		{
			Vector3 point = new Vector3(center.x - radius, center.y, center.z - radius);
			Vector3 point2 = new Vector3(center.x + radius, center.y, center.z + radius);
			int num;
			int num2;
			Regions.getUnsafeCoordinates(point, out num, out num2);
			int num3;
			int num4;
			Regions.getUnsafeCoordinates(point2, out num3, out num4);
			if (num >= (int)Regions.WORLD_SIZE || num2 >= (int)Regions.WORLD_SIZE || num3 < 0 || num4 < 0)
			{
				return;
			}
			num = Mathf.Max(num, 0);
			num3 = Mathf.Min(num3, (int)(Regions.WORLD_SIZE - 1));
			num2 = Mathf.Max(num2, 0);
			num4 = Mathf.Min(num4, (int)(Regions.WORLD_SIZE - 1));
			for (int i = num; i <= num3; i++)
			{
				for (int j = num2; j <= num4; j++)
				{
					result.Add(new RegionCoordinate((byte)i, (byte)j));
				}
			}
		}

		/// <summary>
		/// Convert world-space point into region coordinates that may be out of bounds.
		/// </summary>
		// Token: 0x060039AA RID: 14762 RVA: 0x0010E4A6 File Offset: 0x0010C6A6
		private static void getUnsafeCoordinates(Vector3 point, out int x, out int y)
		{
			x = Mathf.FloorToInt((point.x + 4096f) / (float)Regions.REGION_SIZE);
			y = Mathf.FloorToInt((point.z + 4096f) / (float)Regions.REGION_SIZE);
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0010E4DC File Offset: 0x0010C6DC
		public static bool tryGetCoordinate(Vector3 point, out byte x, out byte y)
		{
			x = byte.MaxValue;
			y = byte.MaxValue;
			if (Regions.checkSafe(point))
			{
				x = (byte)((point.x + 4096f) / (float)Regions.REGION_SIZE);
				y = (byte)((point.z + 4096f) / (float)Regions.REGION_SIZE);
				return true;
			}
			return false;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x0010E530 File Offset: 0x0010C730
		public static bool tryGetPoint(int x, int y, out Vector3 point)
		{
			point = Vector3.zero;
			if (Regions.checkSafe(x, y))
			{
				point.x = (float)(x * (int)Regions.REGION_SIZE - 4096);
				point.z = (float)(y * (int)Regions.REGION_SIZE - 4096);
				return true;
			}
			return false;
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x0010E57C File Offset: 0x0010C77C
		internal static float HorizontalDistanceFromCenterSquared(int x, int y, Vector3 position)
		{
			return MathfEx.HorizontalDistanceSquared(new Vector3((float)(x * (int)Regions.REGION_SIZE) + -4032f, 0f, (float)(y * (int)Regions.REGION_SIZE) + -4032f), position);
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0010E5AA File Offset: 0x0010C7AA
		public static bool checkSafe(Vector3 point)
		{
			return point.x >= -4096f && point.z >= -4096f && point.x < 4096f && point.z < 4096f;
		}

		/// <summary>
		/// Clamp position into the maximum bounds expected by the game, not necessarily the level bounds.
		/// </summary>
		/// <returns>True if position was modified.</returns>
		// Token: 0x060039AF RID: 14767 RVA: 0x0010E5E4 File Offset: 0x0010C7E4
		public static bool clampPositionIntoBounds(ref Vector3 position)
		{
			bool result = false;
			if (position.x < -4095.9f)
			{
				position.x = -4095.9f;
				result = true;
			}
			else if (position.x > 4095.9f)
			{
				position.x = 4095.9f;
				result = true;
			}
			if (position.y < -1023.9f)
			{
				position.y = -1023.9f;
				result = true;
			}
			else if (position.y > 1023.9f)
			{
				position.y = 1023.9f;
				result = true;
			}
			if (position.z < -4095.9f)
			{
				position.z = -4095.9f;
				result = true;
			}
			else if (position.z > 4095.9f)
			{
				position.z = 4095.9f;
				result = true;
			}
			return result;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x0010E696 File Offset: 0x0010C896
		public static bool checkSafe(int x, int y)
		{
			return x >= 0 && y >= 0 && x < (int)Regions.WORLD_SIZE && y < (int)Regions.WORLD_SIZE;
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x0010E6B3 File Offset: 0x0010C8B3
		public static bool checkArea(byte x_0, byte y_0, byte x_1, byte y_1, byte area)
		{
			return x_0 >= x_1 - area && y_0 >= y_1 - area && x_0 <= x_1 + area && y_0 <= y_1 + area;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0010E6D8 File Offset: 0x0010C8D8
		public static PooledTransportConnectionList GatherClientConnections(byte x, byte y, byte distance)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.player == null) && Regions.checkArea(x, y, steamPlayer.player.movement.region_x, steamPlayer.player.movement.region_y, distance))
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x0010E770 File Offset: 0x0010C970
		[Obsolete("Replaced by GatherClientConnections")]
		public static IEnumerable<ITransportConnection> EnumerateClients(byte x, byte y, byte distance)
		{
			return Regions.GatherClientConnections(x, y, distance);
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x0010E77C File Offset: 0x0010C97C
		public static PooledTransportConnectionList GatherRemoteClientConnections(byte x, byte y, byte distance)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.player == null) && Regions.checkArea(x, y, steamPlayer.player.movement.region_x, steamPlayer.player.movement.region_y, distance))
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x0010E814 File Offset: 0x0010CA14
		[Obsolete("Replaced by GatherRemoteClientConnections")]
		public static IEnumerable<ITransportConnection> EnumerateClients_Remote(byte x, byte y, byte distance)
		{
			return Regions.GatherRemoteClientConnections(x, y, distance);
		}

		// Token: 0x04002246 RID: 8774
		internal const byte CONST_WORLD_SIZE = 64;

		// Token: 0x04002247 RID: 8775
		public static readonly byte WORLD_SIZE = 64;

		// Token: 0x04002248 RID: 8776
		internal const byte CONST_REGION_SIZE = 128;

		// Token: 0x04002249 RID: 8777
		public static readonly byte REGION_SIZE = 128;
	}
}
