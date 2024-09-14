using System;

namespace SDG.Unturned
{
	// Token: 0x020004A6 RID: 1190
	public class AnimalSpawn
	{
		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x0009321C File Offset: 0x0009141C
		public ushort animal
		{
			get
			{
				return this._animal;
			}
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00093224 File Offset: 0x00091424
		public AnimalSpawn(ushort newAnimal)
		{
			this._animal = newAnimal;
		}

		// Token: 0x040012C4 RID: 4804
		private ushort _animal;
	}
}
