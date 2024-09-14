using System;

namespace SDG.Unturned
{
	// Token: 0x0200037B RID: 891
	internal class RpmEngineSoundConfiguration : IDatParseable
	{
		// Token: 0x06001AB8 RID: 6840 RVA: 0x0006051C File Offset: 0x0005E71C
		public bool TryParse(IDatNode node)
		{
			DatDictionary datDictionary = node as DatDictionary;
			if (datDictionary != null)
			{
				this.idlePitch = datDictionary.ParseFloat("IdlePitch", 0f);
				this.idleVolume = datDictionary.ParseFloat("IdleVolume", 0f);
				this.maxPitch = datDictionary.ParseFloat("MaxPitch", 0f);
				this.maxVolume = datDictionary.ParseFloat("MaxVolume", 0f);
				return true;
			}
			return false;
		}

		// Token: 0x04000C70 RID: 3184
		public float idlePitch;

		// Token: 0x04000C71 RID: 3185
		public float idleVolume;

		// Token: 0x04000C72 RID: 3186
		public float maxPitch;

		// Token: 0x04000C73 RID: 3187
		public float maxVolume;
	}
}
