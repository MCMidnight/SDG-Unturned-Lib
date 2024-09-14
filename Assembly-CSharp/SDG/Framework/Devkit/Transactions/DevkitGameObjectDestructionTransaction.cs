using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x02000132 RID: 306
	public class DevkitGameObjectDestructionTransaction : IDevkitTransaction
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0001C380 File Offset: 0x0001A580
		public bool delta
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001C383 File Offset: 0x0001A583
		public void undo()
		{
			if (this.go != null)
			{
				this.go.SetActive(true);
			}
			this.isActive = true;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001C3A6 File Offset: 0x0001A5A6
		public void redo()
		{
			if (this.go != null)
			{
				this.go.SetActive(false);
			}
			this.isActive = false;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001C3C9 File Offset: 0x0001A5C9
		public void begin()
		{
			if (this.go != null)
			{
				this.go.SetActive(false);
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001C3E5 File Offset: 0x0001A5E5
		public void end()
		{
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001C3E7 File Offset: 0x0001A5E7
		public void forget()
		{
			if (this.go != null && !this.isActive)
			{
				Object.Destroy(this.go);
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001C40A File Offset: 0x0001A60A
		public DevkitGameObjectDestructionTransaction(GameObject newGO)
		{
			this.go = newGO;
			this.isActive = false;
		}

		// Token: 0x040002CD RID: 717
		protected GameObject go;

		// Token: 0x040002CE RID: 718
		protected bool isActive;
	}
}
