using System;

namespace SDG.Unturned
{
	// Token: 0x0200048A RID: 1162
	public class ActionBlueprint
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x00091627 File Offset: 0x0008F827
		public byte id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002458 RID: 9304 RVA: 0x0009162F File Offset: 0x0008F82F
		public bool isLink
		{
			get
			{
				return this._isLink;
			}
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x00091637 File Offset: 0x0008F837
		public ActionBlueprint(byte newID, bool newLink)
		{
			this._id = newID;
			this._isLink = newLink;
		}

		// Token: 0x04001253 RID: 4691
		private byte _id;

		// Token: 0x04001254 RID: 4692
		private bool _isLink;
	}
}
