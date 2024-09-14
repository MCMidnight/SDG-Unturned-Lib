using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SDG.Provider
{
	/// <summary>
	/// Response data from IInventoryService GetInventory web API.
	///
	/// One player's inventory became so large that the Steam client's built-in GetInventory fails,
	/// so as temporary fix we can send them a json file with their inventory.
	/// </summary>
	// Token: 0x0200002F RID: 47
	public class SteamGetInventoryResponse
	{
		/// <summary>
		/// Parse response from json file.
		/// </summary>
		// Token: 0x06000124 RID: 292 RVA: 0x00004988 File Offset: 0x00002B88
		public static List<SteamGetInventoryResponse.Item> parse(string path)
		{
			List<SteamGetInventoryResponse.Item> result;
			using (StreamReader streamReader = new StreamReader(path))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
				{
					result = JsonConvert.DeserializeObject<List<SteamGetInventoryResponse.Item>>(new JsonSerializer().Deserialize<SteamGetInventoryResponse>(jsonReader).response.item_json);
				}
			}
			return result;
		}

		// Token: 0x04000070 RID: 112
		public SteamGetInventoryResponse.InnerResponse response;

		// Token: 0x02000830 RID: 2096
		public class Item
		{
			// Token: 0x04003122 RID: 12578
			public ulong itemid;

			// Token: 0x04003123 RID: 12579
			public ushort quantity;

			// Token: 0x04003124 RID: 12580
			public int itemdefid;
		}

		// Token: 0x02000831 RID: 2097
		public class InnerResponse
		{
			/// <summary>
			/// Json string representation of the contained items.
			/// </summary>
			// Token: 0x04003125 RID: 12581
			public string item_json;
		}
	}
}
