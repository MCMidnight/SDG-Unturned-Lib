using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// HUD with projected labels for teammates.
	/// </summary>
	// Token: 0x020007C7 RID: 1991
	internal class PlayerGroupUI : SleekWrapper
	{
		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x0600436C RID: 17260 RVA: 0x0017CC2D File Offset: 0x0017AE2D
		public List<ISleekLabel> groups
		{
			get
			{
				return this._groups;
			}
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0017CC38 File Offset: 0x0017AE38
		private void addGroup(SteamPlayer player)
		{
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = -100f;
			sleekLabel.PositionOffset_Y = -15f;
			sleekLabel.SizeOffset_X = 200f;
			sleekLabel.SizeOffset_Y = 30f;
			sleekLabel.TextContrastContext = 2;
			sleekLabel.TextColor = new SleekColor(3, 0.5f);
			base.AddChild(sleekLabel);
			sleekLabel.IsVisible = false;
			this.groups.Add(sleekLabel);
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0017CCAE File Offset: 0x0017AEAE
		private void onEnemyConnected(SteamPlayer player)
		{
			this.addGroup(player);
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0017CCB8 File Offset: 0x0017AEB8
		private void onEnemyDisconnected(SteamPlayer player)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i] == player)
				{
					base.RemoveChild(this.groups[i]);
					this.groups.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0017CD08 File Offset: 0x0017AF08
		public override void OnDestroy()
		{
			Provider.onEnemyConnected = (Provider.EnemyConnected)Delegate.Remove(Provider.onEnemyConnected, new Provider.EnemyConnected(this.onEnemyConnected));
			Provider.onEnemyDisconnected = (Provider.EnemyDisconnected)Delegate.Remove(Provider.onEnemyDisconnected, new Provider.EnemyDisconnected(this.onEnemyDisconnected));
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0017CD58 File Offset: 0x0017AF58
		public PlayerGroupUI()
		{
			this._groups = new List<ISleekLabel>();
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				this.addGroup(Provider.clients[i]);
			}
			Provider.onEnemyConnected = (Provider.EnemyConnected)Delegate.Combine(Provider.onEnemyConnected, new Provider.EnemyConnected(this.onEnemyConnected));
			Provider.onEnemyDisconnected = (Provider.EnemyDisconnected)Delegate.Combine(Provider.onEnemyDisconnected, new Provider.EnemyDisconnected(this.onEnemyDisconnected));
		}

		// Token: 0x04002CA3 RID: 11427
		private List<ISleekLabel> _groups;
	}
}
