using System;
using SDG.Framework.Modules;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200052D RID: 1325
	public class Setup : MonoBehaviour
	{
		// Token: 0x06002978 RID: 10616 RVA: 0x000B092C File Offset: 0x000AEB2C
		private void Awake()
		{
			UnturnedPlayerLoop.initialize();
			ThreadUtil.setupGameThread();
			if (this.awakeDedicator)
			{
				base.GetComponent<Dedicator>().awake();
			}
			if (this.awakeLogs)
			{
				base.GetComponent<Logs>().awake();
			}
			if (this.awakeModuleHook)
			{
				base.GetComponent<ModuleHook>().awake();
			}
			if (this.awakeProvider)
			{
				base.GetComponent<Provider>().awake();
			}
			if (this.startModuleHook)
			{
				base.GetComponent<ModuleHook>().start();
			}
			if (this.startProvider)
			{
				base.GetComponent<Provider>().start();
			}
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000B09B5 File Offset: 0x000AEBB5
		private void Start()
		{
			this.postProcess.initialize();
		}

		// Token: 0x0400165B RID: 5723
		public UnturnedPostProcess postProcess;

		// Token: 0x0400165C RID: 5724
		public bool awakeDedicator = true;

		// Token: 0x0400165D RID: 5725
		public bool awakeLogs = true;

		// Token: 0x0400165E RID: 5726
		public bool awakeModuleHook = true;

		// Token: 0x0400165F RID: 5727
		public bool awakeProvider = true;

		// Token: 0x04001660 RID: 5728
		public bool startModuleHook = true;

		// Token: 0x04001661 RID: 5729
		public bool startProvider = true;
	}
}
