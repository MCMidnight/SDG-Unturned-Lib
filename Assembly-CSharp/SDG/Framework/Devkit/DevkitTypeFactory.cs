using System;
using SDG.Framework.Devkit.Transactions;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000110 RID: 272
	public class DevkitTypeFactory
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x0001A660 File Offset: 0x00018860
		public static void instantiate(Type type, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			if (!Level.isEditor)
			{
				return;
			}
			DevkitTransactionManager.beginTransaction("Spawn " + type.Name);
			IDevkitHierarchyItem devkitHierarchyItem;
			if (typeof(MonoBehaviour).IsAssignableFrom(type))
			{
				GameObject gameObject = new GameObject();
				gameObject.name = type.Name;
				gameObject.transform.position = position;
				gameObject.transform.rotation = rotation;
				gameObject.transform.localScale = scale;
				DevkitTransactionUtility.recordInstantiation(gameObject);
				devkitHierarchyItem = (gameObject.AddComponent(type) as IDevkitHierarchyItem);
			}
			else
			{
				devkitHierarchyItem = (Activator.CreateInstance(type) as IDevkitHierarchyItem);
			}
			if (devkitHierarchyItem != null)
			{
				LevelHierarchy.AssignInstanceIdAndMarkDirty(devkitHierarchyItem);
			}
			DevkitTransactionManager.endTransaction();
		}
	}
}
