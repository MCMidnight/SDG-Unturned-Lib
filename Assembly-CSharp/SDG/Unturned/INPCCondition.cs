using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002C9 RID: 713
	public class INPCCondition
	{
		// Token: 0x060014CD RID: 5325 RVA: 0x0004D38A File Offset: 0x0004B58A
		public virtual bool isConditionMet(Player player)
		{
			return false;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004D38D File Offset: 0x0004B58D
		public virtual void ApplyCondition(Player player)
		{
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004D38F File Offset: 0x0004B58F
		public virtual string formatCondition(Player player)
		{
			if (!string.IsNullOrEmpty(this.text))
			{
				return this.text;
			}
			return null;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004D3A8 File Offset: 0x0004B5A8
		public virtual ISleekElement createUI(Player player, Texture2D icon)
		{
			string text = this.formatCondition(player);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.SizeOffset_Y = 30f;
			sleekBox.SizeScale_X = 1f;
			if (icon != null)
			{
				ISleekImage sleekImage = Glazier.Get().CreateImage(icon);
				sleekImage.PositionOffset_X = 5f;
				sleekImage.PositionOffset_Y = 5f;
				sleekImage.SizeOffset_X = 20f;
				sleekImage.SizeOffset_Y = 20f;
				sleekBox.AddChild(sleekImage);
			}
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			if (icon != null)
			{
				sleekLabel.PositionOffset_X = 30f;
				sleekLabel.SizeOffset_X = -35f;
			}
			else
			{
				sleekLabel.PositionOffset_X = 5f;
				sleekLabel.SizeOffset_X = -10f;
			}
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeScale_Y = 1f;
			sleekLabel.TextAlignment = 3;
			sleekLabel.TextColor = 4;
			sleekLabel.TextContrastContext = 1;
			sleekLabel.AllowRichText = true;
			sleekLabel.Text = text;
			sleekBox.AddChild(sleekLabel);
			return sleekBox;
		}

		/// <summary>
		/// Is this condition influenced by a given quest flag?
		/// Used by level objects to determine if local player's flag change may affect visibility.
		/// </summary>
		// Token: 0x060014D1 RID: 5329 RVA: 0x0004D4B8 File Offset: 0x0004B6B8
		public virtual bool isAssociatedWithFlag(ushort flagID)
		{
			return false;
		}

		/// <summary>
		/// Replacement for isAssociatedWithFlag to fix quest conditions and somewhat improve perf.
		/// </summary>
		// Token: 0x060014D2 RID: 5330 RVA: 0x0004D4BB File Offset: 0x0004B6BB
		internal virtual void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004D4C0 File Offset: 0x0004B6C0
		public bool AreUIRequirementsMet(List<bool> areConditionsMet)
		{
			if (this.uiRequirementIndices == null || this.uiRequirementIndices.Count < 1)
			{
				return true;
			}
			foreach (int num in this.uiRequirementIndices)
			{
				if (num >= 0 && num < areConditionsMet.Count && !areConditionsMet[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0004D544 File Offset: 0x0004B744
		public INPCCondition(string newText, bool newShouldReset)
		{
			this.text = newText;
			this.shouldReset = newShouldReset;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0004D55A File Offset: 0x0004B75A
		[Obsolete("Removed shouldSend parameter because ApplyCondition is only called on the server now")]
		public virtual void applyCondition(Player player, bool shouldSend)
		{
			this.ApplyCondition(player);
		}

		// Token: 0x04000859 RID: 2137
		protected string text;

		// Token: 0x0400085A RID: 2138
		protected bool shouldReset;

		/// <summary>
		/// If set, only show this condition in the UI when conditions with these indices are met.
		/// For example don't show "arrest the criminal (name)" until "investigate crime" is completed.
		/// </summary>
		// Token: 0x0400085B RID: 2139
		internal List<int> uiRequirementIndices;
	}
}
