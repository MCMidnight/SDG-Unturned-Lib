using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows Unity events to print messages to the log file for debugging.
	/// </summary>
	// Token: 0x020005CE RID: 1486
	[AddComponentMenu("Unturned/Log Messenger")]
	public class LogMessengerComponent : MonoBehaviour
	{
		// Token: 0x06002FFA RID: 12282 RVA: 0x000D4015 File Offset: 0x000D2215
		public void PrintInfo(string text)
		{
			UnturnedLog.info(base.transform.GetSceneHierarchyPath() + ": \"" + text + "\"");
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000D4037 File Offset: 0x000D2237
		public void PrintDefaultInfo()
		{
			this.PrintInfo(this.DefaultText);
		}

		/// <summary>
		/// Text to use when PrintInfo is invoked.
		/// </summary>
		// Token: 0x040019F5 RID: 6645
		public string DefaultText;
	}
}
