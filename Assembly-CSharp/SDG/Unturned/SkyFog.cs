using System;
using UnityEngine.Rendering.PostProcessing;

namespace SDG.Unturned
{
	// Token: 0x02000155 RID: 341
	[PostProcess(typeof(SkyFogRenderer), 0, "Custom/SkyFog", true)]
	[Serializable]
	public sealed class SkyFog : PostProcessEffectSettings
	{
	}
}
