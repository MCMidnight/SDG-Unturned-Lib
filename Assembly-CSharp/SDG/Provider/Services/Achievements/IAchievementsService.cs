using System;

namespace SDG.Provider.Services.Achievements
{
	// Token: 0x02000067 RID: 103
	public interface IAchievementsService : IService
	{
		/// <summary>
		/// Checks whether the current user has an achievement with this name.
		/// </summary>
		/// <param name="name">The name of the achievement.</param>
		/// <param name="has">Whether the user has this achievement.</param>
		/// <returns>Whether the check succesfully executed.</returns>
		// Token: 0x06000243 RID: 579
		bool getAchievement(string name, out bool has);

		/// <summary>
		/// Assigns the current user an achievement with this name.
		/// </summary>
		/// <param name="name">The name of the achievement.</param>
		/// <returns>Whether the assignment succesfully executed.</returns>
		// Token: 0x06000244 RID: 580
		bool setAchievement(string name);
	}
}
