using System;

namespace SDG.Unturned
{
	// Token: 0x02000440 RID: 1088
	public enum EServerMonetizationTag
	{
		/// <summary>
		/// Host has not specified a value.
		/// </summary>
		// Token: 0x04001003 RID: 4099
		Unspecified,
		/// <summary>
		/// Not an actual tag. Used for filtering.
		/// </summary>
		// Token: 0x04001004 RID: 4100
		Any,
		/// <summary>
		/// Host has specified that the server does not sell anything for real money.
		/// </summary>
		// Token: 0x04001005 RID: 4101
		None,
		/// <summary>
		/// Host has specified that the server does have a real money shop, but does not sell anything which affects gameplay.
		/// </summary>
		// Token: 0x04001006 RID: 4102
		NonGameplay,
		/// <summary>
		/// Host has specified that the server does have a real money shop which sells benefits that affect gameplay.
		/// </summary>
		// Token: 0x04001007 RID: 4103
		Monetized
	}
}
