using System;
using System.Collections.Generic;

namespace SDG.Framework.Debug
{
	// Token: 0x02000154 RID: 340
	public class InspectableList<T> : List<T>, IInspectableList
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x0600087A RID: 2170 RVA: 0x0001DDDC File Offset: 0x0001BFDC
		// (remove) Token: 0x0600087B RID: 2171 RVA: 0x0001DE14 File Offset: 0x0001C014
		public event InspectableListAddedHandler inspectorAdded;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600087C RID: 2172 RVA: 0x0001DE4C File Offset: 0x0001C04C
		// (remove) Token: 0x0600087D RID: 2173 RVA: 0x0001DE84 File Offset: 0x0001C084
		public event InspectableListRemovedHandler inspectorRemoved;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x0600087E RID: 2174 RVA: 0x0001DEBC File Offset: 0x0001C0BC
		// (remove) Token: 0x0600087F RID: 2175 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
		public event InspectableListChangedHandler inspectorChanged;

		// Token: 0x06000880 RID: 2176 RVA: 0x0001DF29 File Offset: 0x0001C129
		public void Add(T item)
		{
			base.Add(item);
			this.triggerChanged();
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001DF38 File Offset: 0x0001C138
		public bool Remove(T item)
		{
			bool result = base.Remove(item);
			this.triggerChanged();
			return result;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001DF47 File Offset: 0x0001C147
		public void RemoveAt(int index)
		{
			base.RemoveAt(index);
			this.triggerChanged();
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001DF56 File Offset: 0x0001C156
		public virtual void inspectorAdd(object instance)
		{
			this.triggerAdded(instance);
			this.triggerChanged();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001DF65 File Offset: 0x0001C165
		public virtual void inspectorRemove(object instance)
		{
			this.triggerRemoved(instance);
			this.triggerChanged();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001DF74 File Offset: 0x0001C174
		public virtual void inspectorSet(int index)
		{
			this.triggerChanged();
		}

		/// <summary>
		/// Whether add can be called from the inspector.
		/// </summary>
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0001DF7C File Offset: 0x0001C17C
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x0001DF84 File Offset: 0x0001C184
		public virtual bool canInspectorAdd { get; set; }

		/// <summary>
		/// Whether remove can be called from the inspector.
		/// </summary>
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0001DF8D File Offset: 0x0001C18D
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0001DF95 File Offset: 0x0001C195
		public virtual bool canInspectorRemove { get; set; }

		// Token: 0x0600088A RID: 2186 RVA: 0x0001DF9E File Offset: 0x0001C19E
		protected virtual void triggerAdded(object instance)
		{
			InspectableListAddedHandler inspectableListAddedHandler = this.inspectorAdded;
			if (inspectableListAddedHandler == null)
			{
				return;
			}
			inspectableListAddedHandler(this, instance);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001DFB2 File Offset: 0x0001C1B2
		protected virtual void triggerRemoved(object instance)
		{
			InspectableListRemovedHandler inspectableListRemovedHandler = this.inspectorRemoved;
			if (inspectableListRemovedHandler == null)
			{
				return;
			}
			inspectableListRemovedHandler(this, instance);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001DFC6 File Offset: 0x0001C1C6
		protected virtual void triggerChanged()
		{
			InspectableListChangedHandler inspectableListChangedHandler = this.inspectorChanged;
			if (inspectableListChangedHandler == null)
			{
				return;
			}
			inspectableListChangedHandler(this);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001DFD9 File Offset: 0x0001C1D9
		public InspectableList()
		{
			this.canInspectorAdd = true;
			this.canInspectorRemove = true;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001DFEF File Offset: 0x0001C1EF
		public InspectableList(int capacity) : base(capacity)
		{
			this.canInspectorAdd = false;
			this.canInspectorRemove = false;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001E006 File Offset: 0x0001C206
		public InspectableList(IEnumerable<T> collection) : base(collection)
		{
			this.canInspectorAdd = true;
			this.canInspectorRemove = true;
		}
	}
}
