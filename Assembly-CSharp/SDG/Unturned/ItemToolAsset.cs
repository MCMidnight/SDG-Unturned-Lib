using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000308 RID: 776
	public class ItemToolAsset : ItemAsset
	{
		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x00055998 File Offset: 0x00053B98
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x000559A0 File Offset: 0x00053BA0
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return base.useableType != typeof(UseableWalkieTalkie);
			}
		}

		/// <summary>
		/// Tools like carjacks and tires can be used in safezone by admins for maintenance.
		/// </summary>
		// Token: 0x06001774 RID: 6004 RVA: 0x000559B7 File Offset: 0x00053BB7
		public override bool canBeUsedInSafezone(SafezoneNode safezone, bool byAdmin)
		{
			return byAdmin || base.useableType == typeof(UseableCarjack) || base.canBeUsedInSafezone(safezone, byAdmin);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x000559DF File Offset: 0x00053BDF
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = base.LoadRedirectableAsset<AudioClip>(bundle, "Use", data, "UseAudioClip");
		}

		// Token: 0x04000A68 RID: 2664
		protected AudioClip _use;
	}
}
