using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.Modules;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000121 RID: 289
	public class LevelHierarchy : IModuleNexus, IDirtyable
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0001B18B File Offset: 0x0001938B
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0001B192 File Offset: 0x00019392
		public static LevelHierarchy instance { get; protected set; }

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x0600075D RID: 1885 RVA: 0x0001B19C File Offset: 0x0001939C
		// (remove) Token: 0x0600075E RID: 1886 RVA: 0x0001B1D0 File Offset: 0x000193D0
		public static event LevelHiearchyItemAdded itemAdded;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x0600075F RID: 1887 RVA: 0x0001B204 File Offset: 0x00019404
		// (remove) Token: 0x06000760 RID: 1888 RVA: 0x0001B238 File Offset: 0x00019438
		public static event LevelHierarchyItemRemoved itemRemoved;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000761 RID: 1889 RVA: 0x0001B26C File Offset: 0x0001946C
		// (remove) Token: 0x06000762 RID: 1890 RVA: 0x0001B2A0 File Offset: 0x000194A0
		public static event LevelHierarchyLoaded loaded;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000763 RID: 1891 RVA: 0x0001B2D4 File Offset: 0x000194D4
		// (remove) Token: 0x06000764 RID: 1892 RVA: 0x0001B308 File Offset: 0x00019508
		public static event LevelHierarchyReady ready;

		// Token: 0x06000765 RID: 1893 RVA: 0x0001B33B File Offset: 0x0001953B
		public static void MarkDirty()
		{
			if (LevelHierarchy.instance != null)
			{
				LevelHierarchy.instance.isDirty = true;
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001B34F File Offset: 0x0001954F
		public static uint generateUniqueInstanceID()
		{
			return LevelHierarchy.availableInstanceID++;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001B35E File Offset: 0x0001955E
		[Obsolete("Renamed to AssignInstanceIdAndMarkDirty")]
		public static void initItem(IDevkitHierarchyItem item)
		{
			LevelHierarchy.AssignInstanceIdAndMarkDirty(item);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001B366 File Offset: 0x00019566
		public static void AssignInstanceIdAndMarkDirty(IDevkitHierarchyItem item)
		{
			if (item.instanceID == 0U)
			{
				item.instanceID = LevelHierarchy.generateUniqueInstanceID();
			}
			if (LevelHierarchy.instance != null)
			{
				LevelHierarchy.instance.isDirty = true;
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001B38D File Offset: 0x0001958D
		public static void addItem(IDevkitHierarchyItem item)
		{
			LevelHierarchy.instance.items.Add(item);
			LevelHierarchy.triggerItemAdded(item);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001B3A5 File Offset: 0x000195A5
		public static void removeItem(IDevkitHierarchyItem item)
		{
			LevelHierarchy.instance.items.Remove(item);
			LevelHierarchy.triggerItemRemoved(item);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001B3BE File Offset: 0x000195BE
		protected static void triggerItemAdded(IDevkitHierarchyItem item)
		{
			LevelHiearchyItemAdded levelHiearchyItemAdded = LevelHierarchy.itemAdded;
			if (levelHiearchyItemAdded == null)
			{
				return;
			}
			levelHiearchyItemAdded(item);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001B3D0 File Offset: 0x000195D0
		protected static void triggerItemRemoved(IDevkitHierarchyItem item)
		{
			if (Level.isExiting)
			{
				return;
			}
			LevelHierarchyItemRemoved levelHierarchyItemRemoved = LevelHierarchy.itemRemoved;
			if (levelHierarchyItemRemoved == null)
			{
				return;
			}
			levelHierarchyItemRemoved(item);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001B3EA File Offset: 0x000195EA
		protected static void triggerLoaded()
		{
			LevelHierarchyLoaded levelHierarchyLoaded = LevelHierarchy.loaded;
			if (levelHierarchyLoaded == null)
			{
				return;
			}
			levelHierarchyLoaded();
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001B3FB File Offset: 0x000195FB
		protected static void triggerReady()
		{
			LevelHierarchyReady levelHierarchyReady = LevelHierarchy.ready;
			if (levelHierarchyReady == null)
			{
				return;
			}
			levelHierarchyReady();
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x0001B40C File Offset: 0x0001960C
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x0001B414 File Offset: 0x00019614
		public List<IDevkitHierarchyItem> items { get; protected set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0001B41D File Offset: 0x0001961D
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0001B425 File Offset: 0x00019625
		public bool isDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				if (this.isDirty == value)
				{
					return;
				}
				this._isDirty = value;
				if (this.isDirty)
				{
					DirtyManager.markDirty(this);
					return;
				}
				DirtyManager.markClean(this);
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001B450 File Offset: 0x00019650
		public void load()
		{
			this.loadedAnyDevkitObjects = false;
			string text = Level.info.path + "/Level.hierarchy";
			if (File.Exists(text))
			{
				using (FileStream fileStream = new FileStream(text, 3, 1, 3))
				{
					using (SHA1Stream sha1Stream = new SHA1Stream(fileStream))
					{
						using (StreamReader streamReader = new StreamReader(sha1Stream))
						{
							IFormattedFileReader reader = new KeyValueTableReader(streamReader);
							this.read(reader);
							byte[] hash = sha1Stream.Hash;
							Level.includeHash("Level.hierarchy", hash);
							goto IL_86;
						}
					}
				}
			}
			LevelHierarchy.availableInstanceID = 1U;
			IL_86:
			if (this.loadedAnyDevkitObjects)
			{
				UnturnedLog.info("Marking level dirty because devkit objects were converted");
				LevelHierarchy.MarkDirty();
			}
			LevelHierarchy.triggerLoaded();
			TimeUtility.updated += this.handleLoadUpdated;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001B538 File Offset: 0x00019738
		public void save()
		{
			string text = Level.info.path + "/Level.hierarchy";
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(text))
			{
				IFormattedFileWriter writer = new KeyValueTableWriter(streamWriter);
				this.write(writer);
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001B5A0 File Offset: 0x000197A0
		public virtual void read(IFormattedFileReader reader)
		{
			if (reader.containsKey("Available_Instance_ID"))
			{
				LevelHierarchy.availableInstanceID = reader.readValue<uint>("Available_Instance_ID");
			}
			else
			{
				LevelHierarchy.availableInstanceID = 1U;
			}
			int num = reader.readArrayLength("Items");
			for (int i = 0; i < num; i++)
			{
				IFormattedFileReader formattedFileReader = reader.readObject(i);
				Type type = formattedFileReader.readValue<Type>("Type");
				if (type == null)
				{
					UnturnedLog.error("Level hierarchy item index " + i.ToString() + " missing type: " + formattedFileReader.readValue("Type"));
				}
				else
				{
					IDevkitHierarchyItem devkitHierarchyItem;
					if (typeof(MonoBehaviour).IsAssignableFrom(type))
					{
						devkitHierarchyItem = (new GameObject
						{
							name = type.Name
						}.AddComponent(type) as IDevkitHierarchyItem);
					}
					else
					{
						devkitHierarchyItem = (Activator.CreateInstance(type) as IDevkitHierarchyItem);
					}
					if (devkitHierarchyItem != null)
					{
						if (formattedFileReader.containsKey("Instance_ID"))
						{
							devkitHierarchyItem.instanceID = formattedFileReader.readValue<uint>("Instance_ID");
						}
						if (devkitHierarchyItem.instanceID == 0U)
						{
							devkitHierarchyItem.instanceID = LevelHierarchy.generateUniqueInstanceID();
						}
						formattedFileReader.readKey("Item");
						devkitHierarchyItem.read(formattedFileReader);
					}
				}
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001B6C4 File Offset: 0x000198C4
		public virtual void write(IFormattedFileWriter writer)
		{
			writer.writeValue<uint>("Available_Instance_ID", LevelHierarchy.availableInstanceID);
			writer.beginArray("Items");
			for (int i = 0; i < this.items.Count; i++)
			{
				IDevkitHierarchyItem devkitHierarchyItem = this.items[i];
				if (devkitHierarchyItem.instanceID != 0U && devkitHierarchyItem.ShouldSave)
				{
					writer.beginObject();
					writer.writeValue<Type>("Type", devkitHierarchyItem.GetType());
					writer.writeValue<uint>("Instance_ID", devkitHierarchyItem.instanceID);
					writer.writeValue<IDevkitHierarchyItem>("Item", devkitHierarchyItem);
					writer.endObject();
				}
			}
			writer.endArray();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001B75F File Offset: 0x0001995F
		public void initialize()
		{
			LevelHierarchy.instance = this;
			this.items = new List<IDevkitHierarchyItem>();
			Level.loadingSteps += this.handleLoadingStep;
			DevkitTransactionManager.transactionsChanged += this.handleTransactionsChanged;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001B794 File Offset: 0x00019994
		public void shutdown()
		{
			Level.loadingSteps -= this.handleLoadingStep;
			DevkitTransactionManager.transactionsChanged -= this.handleTransactionsChanged;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001B7B8 File Offset: 0x000199B8
		protected void handleLoadingStep()
		{
			this.items.Clear();
			this.load();
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001B7CB File Offset: 0x000199CB
		protected void handleLoadUpdated()
		{
			TimeUtility.updated -= this.handleLoadUpdated;
			LevelHierarchy.triggerReady();
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001B7E3 File Offset: 0x000199E3
		protected void handleTransactionsChanged()
		{
			if (!Level.isEditor)
			{
				return;
			}
			this.isDirty = true;
		}

		// Token: 0x040002BE RID: 702
		private static uint availableInstanceID;

		// Token: 0x040002C0 RID: 704
		protected bool _isDirty;

		// Token: 0x040002C1 RID: 705
		internal bool loadedAnyDevkitObjects;
	}
}
