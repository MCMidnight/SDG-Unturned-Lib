using System;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Button in the list of levels for the map editor.
	/// </summary>
	// Token: 0x02000725 RID: 1829
	public class SleekEditorLevel : SleekLevel
	{
		// Token: 0x06003C51 RID: 15441 RVA: 0x0011C060 File Offset: 0x0011A260
		public SleekEditorLevel(LevelInfo level) : base(level)
		{
			if (!level.isEditable)
			{
				this.button.OnClicked -= new ClickedButton(base.onClickedButton);
				Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshopEditor/MenuWorkshopEditor.unity3d");
				ISleekImage sleekImage = Glazier.Get().CreateImage();
				sleekImage.PositionOffset_X = 20f;
				sleekImage.PositionOffset_Y = -20f;
				sleekImage.PositionScale_Y = 0.5f;
				sleekImage.SizeOffset_X = 40f;
				sleekImage.SizeOffset_Y = 40f;
				sleekImage.Texture = bundle.load<Texture2D>("Lock");
				sleekImage.TintColor = 2;
				this.button.AddChild(sleekImage);
				bundle.unload();
			}
			if (!level.isFromWorkshop)
			{
				this.button.TooltipText = level.path;
				return;
			}
			CachedUGCDetails cachedUGCDetails;
			if (TempSteamworksWorkshop.getCachedDetails(new PublishedFileId_t(level.publishedFileId), out cachedUGCDetails))
			{
				this.button.TooltipText = cachedUGCDetails.GetTitle();
				return;
			}
			this.button.TooltipText = level.publishedFileId.ToString();
		}
	}
}
