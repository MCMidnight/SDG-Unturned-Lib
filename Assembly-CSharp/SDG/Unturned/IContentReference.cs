using System;

namespace SDG.Unturned
{
	// Token: 0x0200029F RID: 671
	public interface IContentReference
	{
		/// <summary>
		/// Name of the asset bundle.
		/// </summary>
		/// <example>core.content</example>
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x0600142A RID: 5162
		// (set) Token: 0x0600142B RID: 5163
		string name { get; set; }

		/// <summary>
		/// Path within the asset bundle.
		/// </summary>
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600142C RID: 5164
		// (set) Token: 0x0600142D RID: 5165
		string path { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600142E RID: 5166
		bool isValid { get; }
	}
}
