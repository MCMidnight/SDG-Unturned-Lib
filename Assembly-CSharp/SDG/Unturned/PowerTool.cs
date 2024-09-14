using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000766 RID: 1894
	public class PowerTool
	{
		// Token: 0x06003DF2 RID: 15858 RVA: 0x0012C07C File Offset: 0x0012A27C
		public static void checkInteractables<T>(Vector3 point, float radius, ushort plant, List<T> interactablesInRadius) where T : Interactable
		{
			float sqrRadius = radius * radius;
			if (plant == 65535)
			{
				PowerTool.regionsInRadius.Clear();
				Regions.getRegionsInRadius(point, radius, PowerTool.regionsInRadius);
				PowerTool.barricadesInRadius.Clear();
				BarricadeManager.getBarricadesInRadius(point, sqrRadius, PowerTool.regionsInRadius, PowerTool.barricadesInRadius);
				ObjectManager.getObjectsInRadius(point, sqrRadius, PowerTool.regionsInRadius, PowerTool.barricadesInRadius);
			}
			else
			{
				PowerTool.barricadesInRadius.Clear();
				BarricadeManager.getBarricadesInRadius(point, sqrRadius, plant, PowerTool.barricadesInRadius);
			}
			for (int i = 0; i < PowerTool.barricadesInRadius.Count; i++)
			{
				T component = PowerTool.barricadesInRadius[i].GetComponent<T>();
				if (!(component == null))
				{
					interactablesInRadius.Add(component);
				}
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0012C12C File Offset: 0x0012A32C
		public static void checkInteractables<T>(Vector3 point, float radius, List<T> interactablesInRadius) where T : Interactable
		{
			float sqrRadius = radius * radius;
			PowerTool.regionsInRadius.Clear();
			Regions.getRegionsInRadius(point, radius, PowerTool.regionsInRadius);
			PowerTool.barricadesInRadius.Clear();
			BarricadeManager.getBarricadesInRadius(point, sqrRadius, PowerTool.regionsInRadius, PowerTool.barricadesInRadius);
			BarricadeManager.getBarricadesInRadius(point, sqrRadius, PowerTool.barricadesInRadius);
			for (int i = 0; i < PowerTool.barricadesInRadius.Count; i++)
			{
				T component = PowerTool.barricadesInRadius[i].GetComponent<T>();
				if (!(component == null))
				{
					interactablesInRadius.Add(component);
				}
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0012C1B8 File Offset: 0x0012A3B8
		public static bool checkFires(Vector3 point, float radius)
		{
			PowerTool.firesInRadius.Clear();
			PowerTool.checkInteractables<InteractableFire>(point, radius, PowerTool.firesInRadius);
			for (int i = 0; i < PowerTool.firesInRadius.Count; i++)
			{
				if (PowerTool.firesInRadius[i].isLit)
				{
					return true;
				}
			}
			PowerTool.ovensInRadius.Clear();
			PowerTool.checkInteractables<InteractableOven>(point, radius, PowerTool.ovensInRadius);
			for (int j = 0; j < PowerTool.ovensInRadius.Count; j++)
			{
				if (PowerTool.ovensInRadius[j].isWired && PowerTool.ovensInRadius[j].isLit)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x0012C256 File Offset: 0x0012A456
		public static List<InteractableGenerator> checkGenerators(Vector3 point, float radius, ushort plant)
		{
			PowerTool.generatorsInRadius.Clear();
			PowerTool.checkInteractables<InteractableGenerator>(point, radius, plant, PowerTool.generatorsInRadius);
			return PowerTool.generatorsInRadius;
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x0012C274 File Offset: 0x0012A474
		public static List<InteractablePower> checkPower(Vector3 point, float radius, ushort plant)
		{
			PowerTool.powerInRadius.Clear();
			PowerTool.checkInteractables<InteractablePower>(point, radius, plant, PowerTool.powerInRadius);
			return PowerTool.powerInRadius;
		}

		// Token: 0x040026E4 RID: 9956
		public static readonly float MAX_POWER_RANGE = 256f;

		// Token: 0x040026E5 RID: 9957
		private static List<RegionCoordinate> regionsInRadius = new List<RegionCoordinate>(4);

		// Token: 0x040026E6 RID: 9958
		private static List<Transform> barricadesInRadius = new List<Transform>();

		// Token: 0x040026E7 RID: 9959
		private static List<InteractableFire> firesInRadius = new List<InteractableFire>();

		// Token: 0x040026E8 RID: 9960
		private static List<InteractableOven> ovensInRadius = new List<InteractableOven>();

		// Token: 0x040026E9 RID: 9961
		private static List<InteractablePower> powerInRadius = new List<InteractablePower>();

		// Token: 0x040026EA RID: 9962
		private static List<InteractableGenerator> generatorsInRadius = new List<InteractableGenerator>();
	}
}
