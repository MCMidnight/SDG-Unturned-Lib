using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000414 RID: 1044
	public interface IReun
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001EB7 RID: 7863
		int step { get; }

		// Token: 0x06001EB8 RID: 7864
		Transform redo();

		// Token: 0x06001EB9 RID: 7865
		void undo();
	}
}
