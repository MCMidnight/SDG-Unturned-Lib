using System;

namespace SDG.Framework.Debug
{
	// Token: 0x02000150 RID: 336
	public interface IInspectableList
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000860 RID: 2144
		// (remove) Token: 0x06000861 RID: 2145
		event InspectableListAddedHandler inspectorAdded;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000862 RID: 2146
		// (remove) Token: 0x06000863 RID: 2147
		event InspectableListRemovedHandler inspectorRemoved;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06000864 RID: 2148
		// (remove) Token: 0x06000865 RID: 2149
		event InspectableListChangedHandler inspectorChanged;

		/// <summary>
		/// Called when the inspector adds an element.
		/// </summary>
		// Token: 0x06000866 RID: 2150
		void inspectorAdd(object instance);

		/// <summary>
		/// Called when the inspector removes an element.
		/// </summary>
		// Token: 0x06000867 RID: 2151
		void inspectorRemove(object instance);

		/// <summary>
		/// Called when the inspector sets an element to a different value.
		/// </summary>
		// Token: 0x06000868 RID: 2152
		void inspectorSet(int index);

		/// <summary>
		/// Whether add can be called from the inspector.
		/// </summary>
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000869 RID: 2153
		// (set) Token: 0x0600086A RID: 2154
		bool canInspectorAdd { get; set; }

		/// <summary>
		/// Whether remove can be called from the inspector.
		/// </summary>
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600086B RID: 2155
		// (set) Token: 0x0600086C RID: 2156
		bool canInspectorRemove { get; set; }
	}
}
