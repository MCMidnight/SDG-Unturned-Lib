using System;

namespace SDG.Unturned
{
	// Token: 0x0200036F RID: 879
	public interface ITypeReference
	{
		/// <summary>
		/// GUID of the asset this is referring to.
		/// </summary>
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001A95 RID: 6805
		// (set) Token: 0x06001A96 RID: 6806
		string assemblyQualifiedName { get; set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001A97 RID: 6807
		Type type { get; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001A98 RID: 6808
		bool isValid { get; }
	}
}
