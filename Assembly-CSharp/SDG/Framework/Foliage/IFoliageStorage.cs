using System;
using System.Collections.Generic;

namespace SDG.Framework.Foliage
{
	/// <summary>
	/// Responsible for reading and writing persistent foliage data.
	/// </summary>
	// Token: 0x02000103 RID: 259
	public interface IFoliageStorage
	{
		/// <summary>
		/// Called after creating instance for level, prior to any loading.
		/// Not called when creating the auto-upgrade instance for editorSaveAllTiles.
		/// </summary>
		// Token: 0x060006A3 RID: 1699
		void Initialize();

		/// <summary>
		/// Called prior to destroying instance.
		/// </summary>
		// Token: 0x060006A4 RID: 1700
		void Shutdown();

		/// <summary>
		/// Called when tile wants to be drawn.
		/// </summary>
		// Token: 0x060006A5 RID: 1701
		void TileBecameRelevantToViewer(FoliageTile tile);

		/// <summary>
		/// Called when tile no longer wants to be drawn.
		/// </summary>
		// Token: 0x060006A6 RID: 1702
		void TileNoLongerRelevantToViewer(FoliageTile tile);

		/// <summary>
		/// Called during Unity's Update loop.
		/// </summary>
		// Token: 0x060006A7 RID: 1703
		void Update();

		/// <summary>
		/// Load known tiles during level load.
		/// </summary>
		// Token: 0x060006A8 RID: 1704
		void EditorLoadAllTiles(IEnumerable<FoliageTile> tiles);

		/// <summary>
		/// Save tiles during level save. 
		/// </summary>
		// Token: 0x060006A9 RID: 1705
		void EditorSaveAllTiles(IEnumerable<FoliageTile> tiles);
	}
}
