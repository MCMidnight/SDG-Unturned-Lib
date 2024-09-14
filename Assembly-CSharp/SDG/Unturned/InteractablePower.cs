using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000468 RID: 1128
	public class InteractablePower : Interactable
	{
		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002260 RID: 8800 RVA: 0x000850EF File Offset: 0x000832EF
		public bool isWired
		{
			get
			{
				return this._isWired;
			}
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000850F7 File Offset: 0x000832F7
		protected virtual void updateWired()
		{
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000850F9 File Offset: 0x000832F9
		public void updateWired(bool newWired)
		{
			if (newWired == this.isWired)
			{
				return;
			}
			this._isWired = newWired;
			this.updateWired();
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x00085114 File Offset: 0x00083314
		private bool CalculateIsConnectedToPower()
		{
			if (Level.isEditor)
			{
				return true;
			}
			if (Level.info != null && Level.info.configData != null && Level.info.configData.Has_Global_Electricity)
			{
				return true;
			}
			if (base.IsChildOfVehicle)
			{
				ushort maxValue = ushort.MaxValue;
				byte b;
				byte b2;
				BarricadeRegion barricadeRegion;
				BarricadeManager.tryGetPlant(base.transform.parent, out b, out b2, out maxValue, out barricadeRegion);
				List<InteractableGenerator> list = PowerTool.checkGenerators(base.transform.position, PowerTool.MAX_POWER_RANGE, maxValue);
				for (int i = 0; i < list.Count; i++)
				{
					InteractableGenerator interactableGenerator = list[i];
					if (interactableGenerator.isPowered && interactableGenerator.fuel > 0 && (interactableGenerator.transform.position - base.transform.position).sqrMagnitude < interactableGenerator.sqrWirerange)
					{
						return true;
					}
				}
				return false;
			}
			return InteractableGenerator.IsWorldPositionPowered(base.transform.position);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x00085208 File Offset: 0x00083408
		internal void RefreshIsConnectedToPower()
		{
			this.updateWired(this.CalculateIsConnectedToPower());
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00085216 File Offset: 0x00083416
		internal void RefreshIsConnectedToPowerWithoutNotify()
		{
			this._isWired = this.CalculateIsConnectedToPower();
		}

		// Token: 0x040010EB RID: 4331
		protected bool _isWired;
	}
}
