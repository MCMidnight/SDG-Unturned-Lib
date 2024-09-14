using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Unturned
{
	// Token: 0x020004E3 RID: 1251
	public class LevelInfo
	{
		/// <summary>
		/// Absolute path to the map folder.
		/// </summary>
		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x0009D616 File Offset: 0x0009B816
		// (set) Token: 0x06002685 RID: 9861 RVA: 0x0009D61E File Offset: 0x0009B81E
		public string path { get; protected set; }

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x0009D627 File Offset: 0x0009B827
		public string name
		{
			get
			{
				return this._name;
			}
		}

		/// <summary>
		/// Whether unity analytics should track this map's name. Don't want to burn all the analysis points!
		/// </summary>
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002687 RID: 9863 RVA: 0x0009D62F File Offset: 0x0009B82F
		public bool canAnalyticsTrack
		{
			get
			{
				return this.isSpecial;
			}
		}

		/// <summary>
		/// Maps included with the game only, separate from category because arena maps are misc.
		/// Category is set as part of the config file. This is only mainly used to enable unity analytics tracking for map name.
		/// </summary>
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002688 RID: 9864 RVA: 0x0009D638 File Offset: 0x0009B838
		public bool isSpecial
		{
			get
			{
				return this.name == "Alpha Valley" || this.name == "Monolith" || this.name == "Paintball_Arena_0" || this.name == "PEI" || this.name == "PEI Arena" || this.name == "Tutorial" || this.name == "Washington" || this.name == "Washington Arena" || this.name == "Yukon" || this.name == "Russia" || this.name == "Hawaii" || this.name == "Germany";
			}
		}

		/// <summary>
		/// Only used for play menu categories at the moment.
		/// </summary>
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002689 RID: 9865 RVA: 0x0009D729 File Offset: 0x0009B929
		// (set) Token: 0x0600268A RID: 9866 RVA: 0x0009D731 File Offset: 0x0009B931
		public bool isFromWorkshop { get; protected set; }

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x0009D73A File Offset: 0x0009B93A
		// (set) Token: 0x0600268C RID: 9868 RVA: 0x0009D742 File Offset: 0x0009B942
		public ulong publishedFileId { get; protected set; }

		/// <summary>
		/// SHA1 hash of the Level.dat file.
		/// </summary>
		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x0009D74B File Offset: 0x0009B94B
		// (set) Token: 0x0600268E RID: 9870 RVA: 0x0009D753 File Offset: 0x0009B953
		public byte[] hash { get; protected set; }

		/// <summary>
		/// Test whether this map's workshop file ID is in the curated maps list.
		/// </summary>
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x0600268F RID: 9871 RVA: 0x0009D75C File Offset: 0x0009B95C
		public bool isCurated
		{
			get
			{
				if (this.isFromWorkshop)
				{
					using (List<CuratedMapLink>.Enumerator enumerator = Provider.statusData.Maps.Curated_Map_Links.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.Workshop_File_Id == this.publishedFileId)
							{
								return true;
							}
						}
					}
					return false;
				}
				return this.name == "France" || this.name == "Canyon Arena";
			}
		}

		/// <summary>
		/// Web URL to map feedback discussions.
		/// </summary>
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002690 RID: 9872 RVA: 0x0009D7F4 File Offset: 0x0009B9F4
		public string feedbackUrl
		{
			get
			{
				if (this.configData != null && !string.IsNullOrEmpty(this.configData.Feedback))
				{
					return this.configData.Feedback;
				}
				if (this.isFromWorkshop)
				{
					return "https://steamcommunity.com/sharedfiles/filedetails/discussions/" + this.publishedFileId.ToString();
				}
				return null;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x0009D849 File Offset: 0x0009BA49
		public ELevelSize size
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002692 RID: 9874 RVA: 0x0009D851 File Offset: 0x0009BA51
		public ELevelType type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x0009D859 File Offset: 0x0009BA59
		public bool isEditable
		{
			get
			{
				return this._isEditable;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06002694 RID: 9876 RVA: 0x0009D861 File Offset: 0x0009BA61
		// (set) Token: 0x06002695 RID: 9877 RVA: 0x0009D869 File Offset: 0x0009BA69
		public LevelInfoConfigData configData { get; private set; }

		// Token: 0x06002696 RID: 9878 RVA: 0x0009D874 File Offset: 0x0009BA74
		public Local getLocalization()
		{
			if (this.cachedLocalization == null)
			{
				string path = this.path + "/" + Provider.language + ".dat";
				if (ReadWrite.fileExists(path, false, false))
				{
					this.cachedLocalization = new Local(ReadWrite.ReadDataWithoutHash(path));
				}
				else
				{
					string path2 = Provider.localizationRoot + "/Maps/" + this.name + ".dat";
					if (ReadWrite.fileExists(path2, false, false))
					{
						this.cachedLocalization = new Local(ReadWrite.ReadDataWithoutHash(path2));
					}
					else
					{
						string path3 = Provider.localizationRoot + "/Maps/" + this.name.Replace(' ', '_') + ".dat";
						if (ReadWrite.fileExists(path3, false, false))
						{
							this.cachedLocalization = new Local(ReadWrite.ReadDataWithoutHash(path3));
						}
					}
				}
				if (this.cachedLocalization == null)
				{
					string path4 = this.path + "/English.dat";
					if (ReadWrite.fileExists(path4, false, false))
					{
						this.cachedLocalization = new Local(ReadWrite.ReadDataWithoutHash(path4));
					}
					else
					{
						this.cachedLocalization = new Local();
					}
				}
			}
			return this.cachedLocalization;
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0009D984 File Offset: 0x0009BB84
		public string getLocalizedName()
		{
			Local localization = this.getLocalization();
			if (localization != null && localization.has("Name"))
			{
				return localization.format("Name");
			}
			return this.name;
		}

		/// <summary>
		/// Preview.png should be 320x180
		/// </summary>
		// Token: 0x06002698 RID: 9880 RVA: 0x0009D9BC File Offset: 0x0009BBBC
		public string GetPreviewImageFilePath()
		{
			string text = Path.Combine(this.path, "Preview.png");
			if (File.Exists(text))
			{
				return text;
			}
			return this.GetLoadingScreenImagePath();
		}

		/// <summary>
		/// Get a random file path in the /Screenshots folder, or fallback to Level.png if it exists.
		/// </summary>
		// Token: 0x06002699 RID: 9881 RVA: 0x0009D9EC File Offset: 0x0009BBEC
		public string GetLoadingScreenImagePath()
		{
			string randomScreenshotPath = this.GetRandomScreenshotPath();
			if (!string.IsNullOrEmpty(randomScreenshotPath))
			{
				return randomScreenshotPath;
			}
			string text = Path.Combine(this.path, "Level.png");
			if (File.Exists(text))
			{
				return text;
			}
			return null;
		}

		/// <summary>
		/// Get a random file path in the /Screenshots folder
		/// </summary>
		// Token: 0x0600269A RID: 9882 RVA: 0x0009DA28 File Offset: 0x0009BC28
		internal string GetRandomScreenshotPath()
		{
			string text = Path.Combine(this.path, "Screenshots");
			if (!Directory.Exists(text))
			{
				return null;
			}
			return LoadingUI.GetRandomImagePathInDirectory(text, false);
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0009DA58 File Offset: 0x0009BC58
		public LevelInfo(string newPath, string newName, ELevelSize newSize, ELevelType newType, bool newEditable, LevelInfoConfigData newConfigData, ulong publishedFileId, byte[] hash)
		{
			this.path = newPath;
			this._name = newName;
			this._size = newSize;
			this._type = newType;
			this._isEditable = newEditable;
			this.configData = newConfigData;
			this.isFromWorkshop = (publishedFileId > 0UL);
			this.publishedFileId = publishedFileId;
			this.hash = hash;
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0009DAB4 File Offset: 0x0009BCB4
		[Obsolete("Please use Level.getAsset instead. LevelInfo persists between loads now. (public issue #4273)")]
		public LevelAsset resolveAsset()
		{
			LevelAsset levelAsset = null;
			if (this.configData != null && this.configData.Asset.isValid)
			{
				levelAsset = Assets.find<LevelAsset>(this.configData.Asset);
			}
			if (levelAsset == null)
			{
				levelAsset = Assets.find<LevelAsset>(LevelAsset.defaultLevel);
			}
			return levelAsset;
		}

		// Token: 0x0400142E RID: 5166
		private string _name;

		// Token: 0x04001432 RID: 5170
		private ELevelSize _size;

		// Token: 0x04001433 RID: 5171
		private ELevelType _type;

		// Token: 0x04001434 RID: 5172
		private bool _isEditable;

		// Token: 0x04001436 RID: 5174
		private Local cachedLocalization;
	}
}
