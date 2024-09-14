using System;
using System.Reflection;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200068C RID: 1676
	public class SteamChannelMethod
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06003875 RID: 14453 RVA: 0x0010B503 File Offset: 0x00109703
		// (set) Token: 0x06003876 RID: 14454 RVA: 0x0010B50B File Offset: 0x0010970B
		public Component component { get; protected set; }

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003877 RID: 14455 RVA: 0x0010B514 File Offset: 0x00109714
		// (set) Token: 0x06003878 RID: 14456 RVA: 0x0010B51C File Offset: 0x0010971C
		public MethodInfo method { get; protected set; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06003879 RID: 14457 RVA: 0x0010B525 File Offset: 0x00109725
		// (set) Token: 0x0600387A RID: 14458 RVA: 0x0010B52D File Offset: 0x0010972D
		public string legacyMethodName { get; protected set; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x0010B536 File Offset: 0x00109736
		// (set) Token: 0x0600387C RID: 14460 RVA: 0x0010B53E File Offset: 0x0010973E
		public Type[] types { get; protected set; }

		/// <summary>
		/// Reflected attribute that was used to find this method.
		/// Contains extra information about how to call it.
		/// </summary>
		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x0600387D RID: 14461 RVA: 0x0010B547 File Offset: 0x00109747
		// (set) Token: 0x0600387E RID: 14462 RVA: 0x0010B54F File Offset: 0x0010974F
		public SteamCall attribute { get; protected set; }

		// Token: 0x0600387F RID: 14463 RVA: 0x0010B558 File Offset: 0x00109758
		public SteamChannelMethod(Component newComponent, MethodInfo newMethod, string legacyMethodName, Type[] newTypes, int typesReadOffset, SteamChannelMethod.EContextType contextType, int contextParameterIndex, SteamCall attribute)
		{
			this.component = newComponent;
			this.method = newMethod;
			this.legacyMethodName = legacyMethodName;
			this.types = newTypes;
			this.typesReadOffset = typesReadOffset;
			this.contextType = contextType;
			this.contextParameterIndex = contextParameterIndex;
			this.attribute = attribute;
		}

		// Token: 0x04002178 RID: 8568
		public int typesReadOffset;

		// Token: 0x04002179 RID: 8569
		public SteamChannelMethod.EContextType contextType;

		/// <summary>
		/// Index of the context parameter, if not None.
		/// </summary>
		// Token: 0x0400217A RID: 8570
		public int contextParameterIndex;

		// Token: 0x020009E4 RID: 2532
		public enum EContextType
		{
			// Token: 0x04003462 RID: 13410
			None,
			// Token: 0x04003463 RID: 13411
			Client,
			// Token: 0x04003464 RID: 13412
			Server
		}
	}
}
