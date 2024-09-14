using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x02000139 RID: 313
	public class DevkitTransactionUtility
	{
		// Token: 0x06000802 RID: 2050 RVA: 0x0001CCA2 File Offset: 0x0001AEA2
		public static void beginGenericTransaction()
		{
			DevkitTransactionManager.beginTransaction("Generic");
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001CCAE File Offset: 0x0001AEAE
		public static void endGenericTransaction()
		{
			DevkitTransactionManager.endTransaction();
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001CCB5 File Offset: 0x0001AEB5
		public static void recordInstantiation(GameObject go)
		{
			DevkitTransactionManager.recordTransaction(new DevkitGameObjectInstantiationTransaction(go));
		}

		/// <summary>
		/// Save the state of all the fields and properties on this object to the current transaction group so that they can be checked for changes once the transaction has ended.
		/// </summary>
		// Token: 0x06000805 RID: 2053 RVA: 0x0001CCC2 File Offset: 0x0001AEC2
		public static void recordObjectDelta(object instance)
		{
			DevkitTransactionManager.recordTransaction(new DevkitObjectDeltaTransaction(instance));
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001CCCF File Offset: 0x0001AECF
		public static void recordDestruction(GameObject go)
		{
			DevkitTransactionManager.recordTransaction(new DevkitGameObjectDestructionTransaction(go));
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001CCDC File Offset: 0x0001AEDC
		public static void recordTransformChangeParent(Transform transform, Transform parent)
		{
			DevkitTransactionManager.recordTransaction(new DevkitTransformChangeParentTransaction(transform, parent));
		}
	}
}
