using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200059D RID: 1437
	public class StructureRegion
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x000C8B6C File Offset: 0x000C6D6C
		public List<StructureDrop> drops
		{
			get
			{
				return this._drops;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x000C8B74 File Offset: 0x000C6D74
		[Obsolete("Maintaining two separate lists was error prone, but still kept for backwards compat")]
		public List<StructureData> structures
		{
			get
			{
				return this._structures;
			}
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000C8B7C File Offset: 0x000C6D7C
		public StructureDrop FindStructureByRootTransform(Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			StructureRefComponent component = transform.GetComponent<StructureRefComponent>();
			if (component == null)
			{
				return null;
			}
			return component.tempNotSureIfStructureShouldBeAComponentYet;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000C8B94 File Offset: 0x000C6D94
		[Obsolete("Dead code, please contact if you need this and we will make a plan")]
		public StructureData findStructureByInstanceID(uint instanceID)
		{
			foreach (StructureData structureData in this.structures)
			{
				if (structureData.instanceID == instanceID)
				{
					return structureData;
				}
			}
			return null;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000C8BF0 File Offset: 0x000C6DF0
		[Obsolete("Renamed to DestroyAll")]
		public void destroy()
		{
			this.DestroyAll();
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000C8BF8 File Offset: 0x000C6DF8
		internal void DestroyTail()
		{
			StructureDrop andRemoveTail = this._drops.GetAndRemoveTail<StructureDrop>();
			try
			{
				andRemoveTail.ReleaseNetId();
				StructureManager.instance.DestroyOrReleaseStructure(andRemoveTail);
				andRemoveTail.model.position = Vector3.zero;
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception destroying structure:");
			}
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000C8C50 File Offset: 0x000C6E50
		internal void DestroyAll()
		{
			foreach (StructureDrop structureDrop in this._drops)
			{
				try
				{
					structureDrop.ReleaseNetId();
					StructureManager.instance.DestroyOrReleaseStructure(structureDrop);
					structureDrop.model.position = Vector3.zero;
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Exception destroying structure:");
				}
			}
			this.drops.Clear();
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000C8CE4 File Offset: 0x000C6EE4
		public StructureRegion()
		{
			this._drops = new List<StructureDrop>();
			this._structures = new List<StructureData>();
			this.isNetworked = false;
			this.isPendingDestroy = false;
		}

		// Token: 0x040018CC RID: 6348
		private List<StructureDrop> _drops;

		// Token: 0x040018CD RID: 6349
		private List<StructureData> _structures;

		// Token: 0x040018CE RID: 6350
		public bool isNetworked;

		// Token: 0x040018CF RID: 6351
		internal bool isPendingDestroy;
	}
}
