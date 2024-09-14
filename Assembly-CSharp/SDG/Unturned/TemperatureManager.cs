using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200059F RID: 1439
	public class TemperatureManager : MonoBehaviour
	{
		// Token: 0x06002E09 RID: 11785 RVA: 0x000C8D30 File Offset: 0x000C6F30
		public static EPlayerTemperature checkPointTemperature(Vector3 point, bool proofFire)
		{
			EPlayerTemperature eplayerTemperature = EPlayerTemperature.NONE;
			for (int i = 0; i < TemperatureManager.bubbles.Count; i++)
			{
				TemperatureBubble temperatureBubble = TemperatureManager.bubbles[i];
				if (!(temperatureBubble.origin == null) && (!proofFire || temperatureBubble.temperature != EPlayerTemperature.BURNING) && (temperatureBubble.origin.position - point).sqrMagnitude < temperatureBubble.sqrRadius)
				{
					if (temperatureBubble.temperature == EPlayerTemperature.ACID)
					{
						return temperatureBubble.temperature;
					}
					if (temperatureBubble.temperature == EPlayerTemperature.BURNING)
					{
						eplayerTemperature = temperatureBubble.temperature;
					}
					else if (eplayerTemperature != EPlayerTemperature.BURNING)
					{
						eplayerTemperature = temperatureBubble.temperature;
					}
				}
			}
			return eplayerTemperature;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000C8DCC File Offset: 0x000C6FCC
		public static TemperatureBubble registerBubble(Transform origin, float radius, EPlayerTemperature temperature)
		{
			TemperatureBubble temperatureBubble = new TemperatureBubble(origin, radius * radius, temperature);
			TemperatureManager.bubbles.Add(temperatureBubble);
			return temperatureBubble;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000C8DF0 File Offset: 0x000C6FF0
		public static void deregisterBubble(TemperatureBubble bubble)
		{
			TemperatureManager.bubbles.Remove(bubble);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000C8DFE File Offset: 0x000C6FFE
		private void onLevelLoaded(int level)
		{
			TemperatureManager.bubbles = new List<TemperatureBubble>();
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000C8E0A File Offset: 0x000C700A
		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x040018D3 RID: 6355
		private static List<TemperatureBubble> bubbles;
	}
}
