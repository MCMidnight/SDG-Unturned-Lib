using System;
using System.Collections.Generic;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000115 RID: 277
	public class DirtyManager
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001A707 File Offset: 0x00018907
		public static List<IDirtyable> dirty
		{
			get
			{
				return DirtyManager._dirty;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001A70E File Offset: 0x0001890E
		public static HashSet<IDirtyable> notSaveable
		{
			get
			{
				return DirtyManager._notSaveable;
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000715 RID: 1813 RVA: 0x0001A718 File Offset: 0x00018918
		// (remove) Token: 0x06000716 RID: 1814 RVA: 0x0001A74C File Offset: 0x0001894C
		public static event MarkedDirtyHandler markedDirty;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000717 RID: 1815 RVA: 0x0001A780 File Offset: 0x00018980
		// (remove) Token: 0x06000718 RID: 1816 RVA: 0x0001A7B4 File Offset: 0x000189B4
		public static event MarkedCleanHandler markedClean;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000719 RID: 1817 RVA: 0x0001A7E8 File Offset: 0x000189E8
		// (remove) Token: 0x0600071A RID: 1818 RVA: 0x0001A81C File Offset: 0x00018A1C
		public static event SaveableChangedHandler saveableChanged;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600071B RID: 1819 RVA: 0x0001A850 File Offset: 0x00018A50
		// (remove) Token: 0x0600071C RID: 1820 RVA: 0x0001A884 File Offset: 0x00018A84
		public static event DirtySaved saved;

		// Token: 0x0600071D RID: 1821 RVA: 0x0001A8B7 File Offset: 0x00018AB7
		public static void markDirty(IDirtyable item)
		{
			DirtyManager.dirty.Add(item);
			DirtyManager.triggerMarkedDirty(item);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001A8CA File Offset: 0x00018ACA
		public static void markClean(IDirtyable item)
		{
			if (DirtyManager.isSaving)
			{
				return;
			}
			DirtyManager.dirty.Remove(item);
			DirtyManager.triggerMarkedClean(item);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001A8E6 File Offset: 0x00018AE6
		public static bool checkSaveable(IDirtyable item)
		{
			return !DirtyManager.notSaveable.Contains(item);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001A8F6 File Offset: 0x00018AF6
		public static void toggleSaveable(IDirtyable item)
		{
			if (!DirtyManager.notSaveable.Remove(item))
			{
				DirtyManager.notSaveable.Add(item);
				DirtyManager.triggerSaveableChanged(item, true);
				return;
			}
			DirtyManager.triggerSaveableChanged(item, false);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001A920 File Offset: 0x00018B20
		public static void save()
		{
			DirtyManager.isSaving = true;
			for (int i = DirtyManager.dirty.Count - 1; i >= 0; i--)
			{
				IDirtyable dirtyable = DirtyManager.dirty[i];
				if (!DirtyManager.notSaveable.Contains(dirtyable))
				{
					dirtyable.save();
					dirtyable.isDirty = false;
					DirtyManager.dirty.RemoveAt(i);
				}
			}
			DirtyManager.isSaving = false;
			DirtyManager.triggerSaved();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001A986 File Offset: 0x00018B86
		protected static void triggerMarkedDirty(IDirtyable item)
		{
			MarkedDirtyHandler markedDirtyHandler = DirtyManager.markedDirty;
			if (markedDirtyHandler == null)
			{
				return;
			}
			markedDirtyHandler(item);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001A998 File Offset: 0x00018B98
		protected static void triggerMarkedClean(IDirtyable item)
		{
			MarkedCleanHandler markedCleanHandler = DirtyManager.markedClean;
			if (markedCleanHandler == null)
			{
				return;
			}
			markedCleanHandler(item);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001A9AA File Offset: 0x00018BAA
		protected static void triggerSaveableChanged(IDirtyable item, bool isSaveable)
		{
			SaveableChangedHandler saveableChangedHandler = DirtyManager.saveableChanged;
			if (saveableChangedHandler == null)
			{
				return;
			}
			saveableChangedHandler(item, isSaveable);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001A9BD File Offset: 0x00018BBD
		protected static void triggerSaved()
		{
			DirtySaved dirtySaved = DirtyManager.saved;
			if (dirtySaved == null)
			{
				return;
			}
			dirtySaved();
		}

		// Token: 0x040002A6 RID: 678
		protected static List<IDirtyable> _dirty = new List<IDirtyable>();

		// Token: 0x040002A7 RID: 679
		public static HashSet<IDirtyable> _notSaveable = new HashSet<IDirtyable>();

		// Token: 0x040002AC RID: 684
		protected static bool isSaving;
	}
}
