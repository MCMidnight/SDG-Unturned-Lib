using System;

namespace SDG.Unturned
{
	// Token: 0x02000826 RID: 2086
	public static class ZombieSpecialityExtension
	{
		/// <summary>
		/// Is this one of the Dying Light volatile zombies? Only spawns during night. Explodes into fire at dawn.
		/// </summary>
		// Token: 0x0600470E RID: 18190 RVA: 0x001A830F File Offset: 0x001A650F
		public static bool IsDLVolatile(this EZombieSpeciality speciality)
		{
			return speciality == EZombieSpeciality.DL_RED_VOLATILE | speciality == EZombieSpeciality.DL_BLUE_VOLATILE;
		}

		/// <summary>
		/// Does this have the BOSS_* prefix?
		/// </summary>
		// Token: 0x0600470F RID: 18191 RVA: 0x001A831C File Offset: 0x001A651C
		public static bool IsBoss(this EZombieSpeciality speciality)
		{
			switch (speciality)
			{
			case EZombieSpeciality.BOSS_ELECTRIC:
			case EZombieSpeciality.BOSS_WIND:
			case EZombieSpeciality.BOSS_FIRE:
			case EZombieSpeciality.BOSS_MAGMA:
			case EZombieSpeciality.BOSS_SPIRIT:
			case EZombieSpeciality.BOSS_NUCLEAR:
			case EZombieSpeciality.BOSS_ELVER_STOMPER:
			case EZombieSpeciality.BOSS_KUWAIT:
			case EZombieSpeciality.BOSS_BUAK_ELECTRIC:
			case EZombieSpeciality.BOSS_BUAK_WIND:
			case EZombieSpeciality.BOSS_BUAK_FIRE:
			case EZombieSpeciality.BOSS_BUAK_FINAL:
				return true;
			}
			return false;
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x001A8377 File Offset: 0x001A6577
		public static bool IsFromBuakMap(this EZombieSpeciality speciality)
		{
			return speciality - EZombieSpeciality.BOSS_BUAK_ELECTRIC <= 3;
		}
	}
}
