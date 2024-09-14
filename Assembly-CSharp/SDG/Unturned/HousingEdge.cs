using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Initially these were structs so that they would be adjacent in memory and therefore faster to iterate lots of them,
	/// but making them classes lets them reference each other which significantly simplifies finding adjactent housing parts.
	/// </summary>
	// Token: 0x0200055A RID: 1370
	internal class HousingEdge
	{
		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x000BB3EC File Offset: 0x000B95EC
		public bool ShouldBeRemoved
		{
			get
			{
				return this.backwardFloors.IsEmpty<StructureDrop>() && this.forwardFloors.IsEmpty<StructureDrop>() && this.walls.IsEmpty<StructureDrop>() && (this.lowerEdge == null || this.lowerEdge.walls.IsEmpty<StructureDrop>());
			}
		}

		/// <summary>
		/// Is there a wall in this slot, and is it full height (not rampart)?
		/// </summary>
		// Token: 0x06002BD8 RID: 11224 RVA: 0x000BB43C File Offset: 0x000B963C
		public bool HasFullHeightWall()
		{
			using (List<StructureDrop>.Enumerator enumerator = this.walls.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.asset.construct == EConstruct.WALL)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// This check prevents placing roof onto the upper edge of a rampart because ramparts
		/// create an edge at full wall height even though they are short.
		///
		/// Ideally in the future wall height will become configurable and remove
		/// the need for this check.
		///
		/// See public issue #3590.
		/// </summary>
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x000BB49C File Offset: 0x000B969C
		public bool CanAttachRoof
		{
			get
			{
				return this.forwardFloors.Count + this.backwardFloors.Count + this.walls.Count > 0 || (this.lowerEdge != null && this.lowerEdge.HasFullHeightWall()) || (this.vertex0 != null && this.vertex1 != null && this.vertex0.lowerVertex != null && this.vertex1.lowerVertex != null && this.vertex0.lowerVertex.HasFullHeightPillar() && this.vertex1.lowerVertex.HasFullHeightPillar());
			}
		}

		// Token: 0x0400175D RID: 5981
		public Vector3 position;

		// Token: 0x0400175E RID: 5982
		public Vector3 direction;

		// Token: 0x0400175F RID: 5983
		public float rotation;

		/// <summary>
		/// Item along positive direction.
		/// Can be multiple on existing saves or if players found an exploit.
		/// </summary>
		// Token: 0x04001760 RID: 5984
		public List<StructureDrop> forwardFloors;

		/// <summary>
		/// Item along negative direction.
		/// Can be multiple on existing saves or if players found an exploit.
		/// </summary>
		// Token: 0x04001761 RID: 5985
		public List<StructureDrop> backwardFloors;

		/// <summary>
		/// Item between floors.
		/// Can be multiple on existing saves or if players found an exploit.
		/// </summary>
		// Token: 0x04001762 RID: 5986
		public List<StructureDrop> walls;

		// Token: 0x04001763 RID: 5987
		public HousingVertex vertex0;

		// Token: 0x04001764 RID: 5988
		public HousingVertex vertex1;

		// Token: 0x04001765 RID: 5989
		public HousingEdge upperEdge;

		// Token: 0x04001766 RID: 5990
		public HousingEdge lowerEdge;
	}
}
