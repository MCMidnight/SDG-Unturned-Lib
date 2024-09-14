using System;
using SDG.Provider;
using TMPro;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000284 RID: 644
	public class StatTracker : MonoBehaviour
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001305 RID: 4869 RVA: 0x00045EB9 File Offset: 0x000440B9
		// (set) Token: 0x06001306 RID: 4870 RVA: 0x00045EC1 File Offset: 0x000440C1
		public TextMeshPro statTrackerText { get; protected set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x00045ECA File Offset: 0x000440CA
		// (set) Token: 0x06001308 RID: 4872 RVA: 0x00045ED2 File Offset: 0x000440D2
		public Transform statTrackerHook { get; protected set; }

		// Token: 0x06001309 RID: 4873 RVA: 0x00045EDC File Offset: 0x000440DC
		public void updateStatTracker(bool viewmodel)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(StatTracker.statTrackerRef);
			gameObject.transform.SetParent(this.statTrackerHook);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			this.statTrackerText = gameObject.GetComponentInChildren<TextMeshPro>();
			if (viewmodel)
			{
				Layerer.relayer(gameObject.transform, 11);
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00045F48 File Offset: 0x00044148
		protected void Update()
		{
			if (this.statTrackerCallback == null)
			{
				return;
			}
			EStatTrackerType type;
			int num;
			if (!this.statTrackerCallback(out type, out num))
			{
				return;
			}
			num %= 10000000;
			if (this.oldStatValue == num)
			{
				return;
			}
			this.oldStatValue = num;
			this.statTrackerText.color = Provider.provider.economyService.getStatTrackerColor(type);
			this.statTrackerText.text = num.ToString("D7");
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x00045FBB File Offset: 0x000441BB
		protected void Awake()
		{
			this.statTrackerHook = base.transform.Find("Stat_Tracker");
		}

		// Token: 0x04000655 RID: 1621
		public GetStatTrackerValueHandler statTrackerCallback;

		// Token: 0x04000656 RID: 1622
		protected int oldStatValue = -1;

		// Token: 0x04000657 RID: 1623
		private static StaticResourceRef<GameObject> statTrackerRef = new StaticResourceRef<GameObject>("Economy/Attachments/Stat_Tracker");
	}
}
