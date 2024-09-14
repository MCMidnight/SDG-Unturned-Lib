using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject to receive weather events for a specific custom weather asset.
	/// </summary>
	// Token: 0x020005C6 RID: 1478
	[AddComponentMenu("Unturned/Custom Weather Event Hook")]
	public class CustomWeatherEventHook : MonoBehaviour
	{
		// Token: 0x06002FE0 RID: 12256 RVA: 0x000D3B04 File Offset: 0x000D1D04
		protected void OnEnable()
		{
			if (Guid.TryParse(this.WeatherAssetGuid, ref this.parsedGuid))
			{
				WeatherEventListenerManager.AddComponentListener(this.parsedGuid, this);
				return;
			}
			this.parsedGuid = Guid.Empty;
			UnturnedLog.warn("{0} unable to parse weather asset guid", new object[]
			{
				base.transform.GetSceneHierarchyPath()
			});
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000D3B5A File Offset: 0x000D1D5A
		protected void OnDisable()
		{
			if (!this.parsedGuid.Equals(Guid.Empty))
			{
				WeatherEventListenerManager.RemoveComponentListener(this.parsedGuid, this);
			}
		}

		/// <summary>
		/// GUID of custom weather asset to listen for.
		/// </summary>
		// Token: 0x040019DA RID: 6618
		public string WeatherAssetGuid;

		/// <summary>
		/// Invoked when custom weather is activated, or immediately if weather is fading in when registered.
		/// </summary>
		// Token: 0x040019DB RID: 6619
		public UnityEvent OnWeatherBeginTransitionIn;

		/// <summary>
		/// Invoked when custom weather finishes fading in, or immediately if weather is already fully active when registered.
		/// </summary>
		// Token: 0x040019DC RID: 6620
		public UnityEvent OnWeatherEndTransitionIn;

		/// <summary>
		/// Invoked when custom weather is deactivated and begins fading out.
		/// </summary>
		// Token: 0x040019DD RID: 6621
		public UnityEvent OnWeatherBeginTransitionOut;

		/// <summary>
		/// Invoked when custom weather finishes fading out and is destroyed.
		/// </summary>
		// Token: 0x040019DE RID: 6622
		public UnityEvent OnWeatherEndTransitionOut;

		/// <summary>
		/// GUID parsed from WeatherAssetGuid parameter.
		/// </summary>
		// Token: 0x040019DF RID: 6623
		protected Guid parsedGuid;
	}
}
