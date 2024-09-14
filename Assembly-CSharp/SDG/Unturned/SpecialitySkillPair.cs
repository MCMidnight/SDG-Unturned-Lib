using System;

namespace SDG.Unturned
{
	// Token: 0x0200065B RID: 1627
	public class SpecialitySkillPair
	{
		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x060035C9 RID: 13769 RVA: 0x000F77EF File Offset: 0x000F59EF
		// (set) Token: 0x060035CA RID: 13770 RVA: 0x000F77F7 File Offset: 0x000F59F7
		public int speciality { get; private set; }

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x060035CB RID: 13771 RVA: 0x000F7800 File Offset: 0x000F5A00
		// (set) Token: 0x060035CC RID: 13772 RVA: 0x000F7808 File Offset: 0x000F5A08
		public int skill { get; private set; }

		// Token: 0x060035CD RID: 13773 RVA: 0x000F7811 File Offset: 0x000F5A11
		public SpecialitySkillPair(int newSpeciality, int newSkill)
		{
			this.speciality = newSpeciality;
			this.skill = newSkill;
		}
	}
}
