using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Properties common to asset and extensions. For example both can specify sounds.
	/// </summary>
	// Token: 0x02000359 RID: 857
	public class PhysicsMaterialAssetBase : Asset
	{
		// Token: 0x060019F0 RID: 6640 RVA: 0x0005D660 File Offset: 0x0005B860
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			DatDictionary dictionary = data.GetDictionary("AudioDefs");
			if (dictionary != null)
			{
				this.audioDefs = new Dictionary<string, MasterBundleReference<OneShotAudioDefinition>>();
				foreach (KeyValuePair<string, IDatNode> keyValuePair in dictionary)
				{
					this.audioDefs[keyValuePair.Key] = keyValuePair.Value.ParseStruct(default(MasterBundleReference<OneShotAudioDefinition>));
				}
			}
		}

		// Token: 0x04000BDF RID: 3039
		public Dictionary<string, MasterBundleReference<OneShotAudioDefinition>> audioDefs;
	}
}
