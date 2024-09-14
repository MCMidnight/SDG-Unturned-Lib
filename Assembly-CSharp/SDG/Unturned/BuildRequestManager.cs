using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// When placing structures that snap to grid multiple requests can come
	/// in to the server at the same time, and checking overlaps against structures
	/// can be problematic, so as a backup we track pending build requests
	/// and cancel ones which conflict.
	/// </summary>
	// Token: 0x02000544 RID: 1348
	public class BuildRequestManager
	{
		/// <summary>
		/// Register a location as having something built there soon.
		/// </summary>
		/// <returns>Unique handle to later finish the request.</returns>
		// Token: 0x06002ACE RID: 10958 RVA: 0x000B71D4 File Offset: 0x000B53D4
		public static int registerPendingBuild(Vector3 location)
		{
			BuildRequestManager.PendingBuild pendingBuild = default(BuildRequestManager.PendingBuild);
			pendingBuild.handle = BuildRequestManager.getUniqueHandle();
			pendingBuild.location = location;
			BuildRequestManager.pendingBuilds.Add(pendingBuild);
			return pendingBuild.handle;
		}

		/// <summary>
		/// Is a location available to build at (i.e. no pending builds)?
		/// </summary>
		/// <returns>False if there are any outstanding build requests for given location.</returns>
		// Token: 0x06002ACF RID: 10959 RVA: 0x000B7210 File Offset: 0x000B5410
		public static bool canBuildAt(Vector3 location, int ignoreHandle)
		{
			foreach (BuildRequestManager.PendingBuild pendingBuild in BuildRequestManager.pendingBuilds)
			{
				if ((pendingBuild.location - location).sqrMagnitude < 0.01f && pendingBuild.handle != ignoreHandle)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Notify that a previously registered build has been completed.
		/// </summary>
		/// <param name="handle">Unique handle.</param>
		// Token: 0x06002AD0 RID: 10960 RVA: 0x000B7288 File Offset: 0x000B5488
		public static void finishPendingBuild(ref int handle)
		{
			if (!BuildRequestManager.isValidHandle(handle))
			{
				return;
			}
			int count = BuildRequestManager.pendingBuilds.Count;
			for (int i = 0; i < count; i++)
			{
				if (BuildRequestManager.pendingBuilds[i].handle == handle)
				{
					BuildRequestManager.pendingBuilds.RemoveAtFast(i);
					handle = -1;
					return;
				}
			}
			handle = -1;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000B72DC File Offset: 0x000B54DC
		public static bool isValidHandle(int handle)
		{
			return handle > 0;
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000B72E2 File Offset: 0x000B54E2
		private static int getUniqueHandle()
		{
			BuildRequestManager.highestHandleId++;
			return BuildRequestManager.highestHandleId;
		}

		// Token: 0x040016CB RID: 5835
		private static List<BuildRequestManager.PendingBuild> pendingBuilds = new List<BuildRequestManager.PendingBuild>();

		// Token: 0x040016CC RID: 5836
		private static int highestHandleId;

		// Token: 0x02000970 RID: 2416
		private struct PendingBuild
		{
			// Token: 0x0400336D RID: 13165
			public int handle;

			// Token: 0x0400336E RID: 13166
			public Vector3 location;
		}
	}
}
