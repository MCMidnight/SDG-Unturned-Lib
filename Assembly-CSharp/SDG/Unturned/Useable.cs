using System;

namespace SDG.Unturned
{
	// Token: 0x020007D6 RID: 2006
	public class Useable : PlayerCaller
	{
		/// <returns>True if primary action was started and stopPrimary should be called in the future.
		/// Useful to allow input to be held until action executes.</returns>
		// Token: 0x06004413 RID: 17427 RVA: 0x0018779A File Offset: 0x0018599A
		public virtual bool startPrimary()
		{
			return false;
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x0018779D File Offset: 0x0018599D
		public virtual void stopPrimary()
		{
		}

		/// <returns>True if secondary action was started and stopSecondary should be called in the future.
		/// Useful to allow input to be held until action executes.</returns>
		// Token: 0x06004415 RID: 17429 RVA: 0x0018779F File Offset: 0x0018599F
		public virtual bool startSecondary()
		{
			return false;
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x001877A2 File Offset: 0x001859A2
		public virtual void stopSecondary()
		{
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06004417 RID: 17431 RVA: 0x001877A4 File Offset: 0x001859A4
		public virtual bool canInspect
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Does useable have a menu open?
		/// If so pause menu, dashboard, and other menus cannot be opened.
		/// </summary>
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06004418 RID: 17432 RVA: 0x001877A7 File Offset: 0x001859A7
		public virtual bool isUseableShowingMenu
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x001877AA File Offset: 0x001859AA
		public virtual void equip()
		{
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x001877AC File Offset: 0x001859AC
		public virtual void dequip()
		{
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x001877AE File Offset: 0x001859AE
		public virtual void tick()
		{
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x001877B0 File Offset: 0x001859B0
		public virtual void simulate(uint simulation, bool inputSteady)
		{
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x001877B2 File Offset: 0x001859B2
		public virtual void tock(uint clock)
		{
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x001877B4 File Offset: 0x001859B4
		public virtual void updateState(byte[] newState)
		{
		}

		// Token: 0x04002D98 RID: 11672
		internal float movementSpeedMultiplier = 1f;
	}
}
