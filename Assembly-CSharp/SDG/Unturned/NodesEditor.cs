using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000415 RID: 1045
	public class NodesEditor : SelectionTool
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x00072405 File Offset: 0x00070605
		// (set) Token: 0x06001EBB RID: 7867 RVA: 0x0007240D File Offset: 0x0007060D
		public TempNodeSystemBase activeNodeSystem
		{
			get
			{
				return this._activeNodeSystem;
			}
			set
			{
				DevkitSelectionManager.clear();
				this._activeNodeSystem = value;
			}
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0007241C File Offset: 0x0007061C
		protected override bool RaycastSelectableObjects(Ray ray, out RaycastHit hitInfo)
		{
			RaycastHit raycastHit;
			if (this.activeNodeSystem != null && Physics.Raycast(ray, out raycastHit))
			{
				GameObject gameObject = raycastHit.transform.gameObject;
				if (gameObject != null && gameObject.GetComponent(this.activeNodeSystem.GetComponentType()) != null)
				{
					hitInfo = raycastHit;
					return true;
				}
			}
			hitInfo = default(RaycastHit);
			return false;
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0007247B File Offset: 0x0007067B
		protected override void RequestInstantiation(Vector3 position)
		{
			if (this.activeNodeSystem != null)
			{
				this.activeNodeSystem.Instantiate(position);
			}
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x00072491 File Offset: 0x00070691
		protected override bool HasBoxSelectableObjects()
		{
			return this.activeNodeSystem != null;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0007249C File Offset: 0x0007069C
		protected override IEnumerable<GameObject> EnumerateBoxSelectableObjects()
		{
			TempNodeSystemBase activeNodeSystem = this.activeNodeSystem;
			if (activeNodeSystem == null)
			{
				return null;
			}
			return activeNodeSystem.EnumerateGameObjects();
		}

		// Token: 0x04000F20 RID: 3872
		private TempNodeSystemBase _activeNodeSystem;
	}
}
