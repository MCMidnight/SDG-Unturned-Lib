using System;

namespace SDG.Unturned
{
	// Token: 0x0200034E RID: 846
	public enum ENPCWeatherStatus
	{
		/// <summary>
		/// True while fading in or fully transitioned in. 
		/// </summary>
		// Token: 0x04000B4C RID: 2892
		Active,
		/// <summary>
		/// True while fading in, but not at full intensity.
		/// </summary>
		// Token: 0x04000B4D RID: 2893
		Transitioning_In,
		/// <summary>
		/// True while finished fading in.
		/// </summary>
		// Token: 0x04000B4E RID: 2894
		Fully_Transitioned_In,
		/// <summary>
		/// True while fading out, but not at zero intensity.
		/// </summary>
		// Token: 0x04000B4F RID: 2895
		Transitioning_Out,
		/// <summary>
		/// True while finished fading out.
		/// </summary>
		// Token: 0x04000B50 RID: 2896
		Fully_Transitioned_Out,
		/// <summary>
		/// True while fading in or out.
		/// </summary>
		// Token: 0x04000B51 RID: 2897
		Transitioning
	}
}
