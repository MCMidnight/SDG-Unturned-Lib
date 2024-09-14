using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200036D RID: 877
	public class StereoSongAsset : Asset
	{
		/// <summary>
		/// Optional URL to open in web browser.
		/// </summary>
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x0005FEDA File Offset: 0x0005E0DA
		// (set) Token: 0x06001A8C RID: 6796 RVA: 0x0005FEE2 File Offset: 0x0005E0E2
		public string linkURL { get; protected set; }

		// Token: 0x06001A8D RID: 6797 RVA: 0x0005FEEC File Offset: 0x0005E0EC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (localization.has("Name"))
			{
				this.titleText = localization.read("Name");
			}
			if (string.IsNullOrEmpty(this.titleText))
			{
				this.titleText = data.GetString("Title", null);
			}
			this.songContentRef = data.ParseStruct<ContentReference<AudioClip>>("Song", default(ContentReference<AudioClip>));
			this.songMbRef = data.ParseStruct<MasterBundleReference<AudioClip>>("Song", default(MasterBundleReference<AudioClip>));
			this.linkURL = data.GetString("Link_URL", null);
			this.isLoop = data.ParseBool("Is_Loop", false);
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0005FF97 File Offset: 0x0005E197
		protected virtual void construct()
		{
			this.songContentRef = ContentReference<AudioClip>.invalid;
			this.songMbRef = MasterBundleReference<AudioClip>.invalid;
			this.linkURL = null;
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0005FFB6 File Offset: 0x0005E1B6
		public StereoSongAsset()
		{
			this.construct();
		}

		/// <summary>
		/// Text from *.dat localization file.
		/// </summary>
		// Token: 0x04000C3D RID: 3133
		public string titleText;

		/// <summary>
		/// Older *.content asset bundle reference. 
		/// </summary>
		// Token: 0x04000C3E RID: 3134
		public ContentReference<AudioClip> songContentRef;

		/// <summary>
		/// Newer *.masterbundle reference.
		/// </summary>
		// Token: 0x04000C3F RID: 3135
		public MasterBundleReference<AudioClip> songMbRef;

		/// <summary>
		/// Whether audio source should loop.
		/// </summary>
		// Token: 0x04000C41 RID: 3137
		public bool isLoop;
	}
}
