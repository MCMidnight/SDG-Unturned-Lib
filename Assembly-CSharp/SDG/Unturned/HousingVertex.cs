using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200055B RID: 1371
	internal class HousingVertex
	{
		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002BDB RID: 11227 RVA: 0x000BB544 File Offset: 0x000B9744
		public bool ShouldBeRemoved
		{
			get
			{
				return this.pillars.Count < 1 && this.floors.Count < 1 && this.edges.Count < 1 && (this.lowerVertex == null || this.lowerVertex.pillars.IsEmpty<StructureDrop>());
			}
		}

		/// <summary>
		/// Is there a pillar in this slot, and is it full height (not post)?
		/// </summary>
		// Token: 0x06002BDC RID: 11228 RVA: 0x000BB598 File Offset: 0x000B9798
		public bool HasFullHeightPillar()
		{
			using (List<StructureDrop>.Enumerator enumerator = this.pillars.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.asset.construct == EConstruct.PILLAR)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Position at the base of the pillar.
		/// </summary>
		// Token: 0x04001767 RID: 5991
		public Vector3 position;

		/// <summary>
		/// Yaw if placing pillar at this vertex.
		/// </summary>
		// Token: 0x04001768 RID: 5992
		public float rotation;

		/// <summary>
		/// Pillar or post currently occupying this slot.
		/// Can be multiple on existing saves or if players found an exploit.
		/// </summary>
		// Token: 0x04001769 RID: 5993
		public List<StructureDrop> pillars = new List<StructureDrop>(1);

		/// <summary>
		/// Can be zero if pillar is floating, or up to six in the center of a triangular circle.
		/// </summary>
		// Token: 0x0400176A RID: 5994
		public List<StructureDrop> floors = new List<StructureDrop>(4);

		// Token: 0x0400176B RID: 5995
		public List<HousingEdge> edges = new List<HousingEdge>(4);

		// Token: 0x0400176C RID: 5996
		public HousingVertex upperVertex;

		// Token: 0x0400176D RID: 5997
		public HousingVertex lowerVertex;
	}
}
