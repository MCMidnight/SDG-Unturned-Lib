using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006F5 RID: 1781
	public class MapsStatusData
	{
		// Token: 0x06003B19 RID: 15129 RVA: 0x00114698 File Offset: 0x00112898
		public MapsStatusData()
		{
			this.Curated_Map_Links = new List<CuratedMapLink>();
			this.Auto_Subscribe = new List<AutoSubscribeMap>();
		}

		/// <summary>
		/// Maps not installed by default, but recommended from maps list.
		/// </summary>
		// Token: 0x04002502 RID: 9474
		public List<CuratedMapLink> Curated_Map_Links;

		/// <summary>
		/// Maps to install to automatically.
		/// Used early in startup to hopefully install before reaching main menu.
		/// </summary>
		// Token: 0x04002503 RID: 9475
		public List<AutoSubscribeMap> Auto_Subscribe;
	}
}
