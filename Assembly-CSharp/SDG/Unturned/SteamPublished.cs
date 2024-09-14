using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000696 RID: 1686
	public class SteamPublished
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x0600390A RID: 14602 RVA: 0x0010CB1B File Offset: 0x0010AD1B
		public string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x0600390B RID: 14603 RVA: 0x0010CB23 File Offset: 0x0010AD23
		public PublishedFileId_t id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0010CB2B File Offset: 0x0010AD2B
		public SteamPublished(string newName, PublishedFileId_t newID)
		{
			this._name = newName;
			this._id = newID;
		}

		// Token: 0x040021F5 RID: 8693
		private string _name;

		// Token: 0x040021F6 RID: 8694
		private PublishedFileId_t _id;
	}
}
