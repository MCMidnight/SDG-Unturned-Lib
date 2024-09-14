using System;

namespace SDG.Unturned
{
	// Token: 0x020002AA RID: 682
	public class DialoguePage
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x0004CBE9 File Offset: 0x0004ADE9
		// (set) Token: 0x06001495 RID: 5269 RVA: 0x0004CBF1 File Offset: 0x0004ADF1
		public string text { get; protected set; }

		// Token: 0x06001496 RID: 5270 RVA: 0x0004CBFA File Offset: 0x0004ADFA
		public DialoguePage(string newText)
		{
			this.text = newText;
		}
	}
}
