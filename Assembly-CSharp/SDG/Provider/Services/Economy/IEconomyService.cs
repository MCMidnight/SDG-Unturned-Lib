using System;

namespace SDG.Provider.Services.Economy
{
	// Token: 0x02000062 RID: 98
	public interface IEconomyService : IService
	{
		/// <summary>
		/// Whether the user has their overlay enabled.
		/// </summary>
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000230 RID: 560
		bool canOpenInventory { get; }

		/// <summary>
		/// Requests the user's inventory.
		/// </summary>
		/// <param name="economyRequestReadyCallback">Called when the request is completed.</param>
		/// <returns>Handle for checking the owner of the callback.</returns>
		// Token: 0x06000231 RID: 561
		IEconomyRequestHandle requestInventory(EconomyRequestReadyCallback economyRequestReadyCallback);

		/// <summary>
		/// Requests a check for promotional items.
		/// </summary>
		/// <param name="economyRequestReadyCallback">Called when the request is completed.</param>
		/// <returns>Handle for checking the owner of the callback.</returns>
		// Token: 0x06000232 RID: 562
		IEconomyRequestHandle requestPromo(EconomyRequestReadyCallback economyRequestReadyCallback);

		/// <summary>
		/// Converts the input items into the output items.
		/// </summary>
		/// <param name="inputItemInstanceIDs">Items to be converted from.</param>
		/// <param name="inputItemQuantities">Item amounts to be consumed.</param>
		/// <param name="outputItemDefinitionIDs">Items to be converted to.</param>
		/// <param name="outputItemQuantities">Item amounts to be generated.</param>
		/// <param name="economyRequestReadyCallback">Called when the exchange is completed.</param>
		// Token: 0x06000233 RID: 563
		IEconomyRequestHandle exchangeItems(IEconomyItemInstance[] inputItemInstanceIDs, uint[] inputItemQuantities, IEconomyItemDefinition[] outputItemDefinitionIDs, uint[] outputItemQuantities, EconomyRequestReadyCallback economyRequestReadyCallback);

		// Token: 0x06000234 RID: 564
		void open(ulong id);
	}
}
