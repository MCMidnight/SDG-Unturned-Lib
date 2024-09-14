using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x02000133 RID: 307
	public class DevkitGameObjectInstantiationTransaction : IDevkitTransaction
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001C420 File Offset: 0x0001A620
		public bool delta
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001C423 File Offset: 0x0001A623
		public void undo()
		{
			if (this.go != null)
			{
				this.go.SetActive(false);
			}
			this.isActive = false;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001C446 File Offset: 0x0001A646
		public void redo()
		{
			if (this.go != null)
			{
				this.go.SetActive(true);
			}
			this.isActive = true;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001C469 File Offset: 0x0001A669
		public void begin()
		{
			if (this.go != null)
			{
				this.go.SetActive(true);
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001C485 File Offset: 0x0001A685
		public void end()
		{
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001C487 File Offset: 0x0001A687
		public void forget()
		{
			if (this.go != null && !this.isActive)
			{
				Object.Destroy(this.go);
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001C4AA File Offset: 0x0001A6AA
		public DevkitGameObjectInstantiationTransaction(GameObject newGO)
		{
			this.go = newGO;
			this.isActive = true;
		}

		// Token: 0x040002CF RID: 719
		protected GameObject go;

		// Token: 0x040002D0 RID: 720
		protected bool isActive;
	}
}
