using System;
using System.Collections.Generic;
using SDG.Unturned;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x02000138 RID: 312
	public class DevkitTransactionManager
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0001C956 File Offset: 0x0001AB56
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0001C960 File Offset: 0x0001AB60
		public static uint historyLength
		{
			get
			{
				return DevkitTransactionManager._historyLength;
			}
			set
			{
				DevkitTransactionManager._historyLength = value;
				UnturnedLog.info("Set history_length to: " + DevkitTransactionManager.historyLength.ToString());
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060007EA RID: 2026 RVA: 0x0001C990 File Offset: 0x0001AB90
		// (remove) Token: 0x060007EB RID: 2027 RVA: 0x0001C9C4 File Offset: 0x0001ABC4
		public static event DevkitTransactionPerformedHandler transactionPerformed;

		// Token: 0x060007EC RID: 2028 RVA: 0x0001C9F7 File Offset: 0x0001ABF7
		protected static void triggerTransactionPerformed(DevkitTransactionGroup group)
		{
			DevkitTransactionPerformedHandler devkitTransactionPerformedHandler = DevkitTransactionManager.transactionPerformed;
			if (devkitTransactionPerformedHandler == null)
			{
				return;
			}
			devkitTransactionPerformedHandler(group);
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060007ED RID: 2029 RVA: 0x0001CA0C File Offset: 0x0001AC0C
		// (remove) Token: 0x060007EE RID: 2030 RVA: 0x0001CA40 File Offset: 0x0001AC40
		public static event DevkitTransactionsChangedHandler transactionsChanged;

		// Token: 0x060007EF RID: 2031 RVA: 0x0001CA73 File Offset: 0x0001AC73
		protected static void triggerTransactionsChanged()
		{
			DevkitTransactionsChangedHandler devkitTransactionsChangedHandler = DevkitTransactionManager.transactionsChanged;
			if (devkitTransactionsChangedHandler == null)
			{
				return;
			}
			devkitTransactionsChangedHandler();
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0001CA84 File Offset: 0x0001AC84
		public static bool canUndo
		{
			get
			{
				return DevkitTransactionManager.undoable.Count > 0;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001CA93 File Offset: 0x0001AC93
		public static bool canRedo
		{
			get
			{
				return DevkitTransactionManager.redoable.Count > 0;
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001CAA2 File Offset: 0x0001ACA2
		public static IEnumerable<DevkitTransactionGroup> getUndoable()
		{
			return DevkitTransactionManager.undoable;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001CAA9 File Offset: 0x0001ACA9
		public static IEnumerable<DevkitTransactionGroup> getRedoable()
		{
			return DevkitTransactionManager.redoable;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001CAB0 File Offset: 0x0001ACB0
		public static DevkitTransactionGroup undo()
		{
			if (!DevkitTransactionManager.canUndo)
			{
				return null;
			}
			DevkitTransactionGroup devkitTransactionGroup = DevkitTransactionManager.popUndo();
			devkitTransactionGroup.undo();
			DevkitTransactionManager.pushRedo(devkitTransactionGroup);
			DevkitTransactionManager.triggerTransactionPerformed(devkitTransactionGroup);
			return devkitTransactionGroup;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001CAD2 File Offset: 0x0001ACD2
		public static DevkitTransactionGroup redo()
		{
			if (!DevkitTransactionManager.canRedo)
			{
				return null;
			}
			DevkitTransactionGroup devkitTransactionGroup = DevkitTransactionManager.popRedo();
			devkitTransactionGroup.redo();
			DevkitTransactionManager.pushUndo(devkitTransactionGroup);
			DevkitTransactionManager.triggerTransactionPerformed(devkitTransactionGroup);
			return devkitTransactionGroup;
		}

		/// <summary>
		/// Open a new transaction group which stores multiple undo/redoable actions, for example this would be called before moving an object.
		/// </summary>
		// Token: 0x060007F6 RID: 2038 RVA: 0x0001CAF4 File Offset: 0x0001ACF4
		public static void beginTransaction(string name)
		{
			if (DevkitTransactionManager.transactionDepth == 0)
			{
				DevkitTransactionManager.clearRedo();
				DevkitTransactionManager.pendingGroup = new DevkitTransactionGroup(name);
			}
			DevkitTransactionManager.transactionDepth++;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001CB19 File Offset: 0x0001AD19
		public static void recordTransaction(IDevkitTransaction transaction)
		{
			if (DevkitTransactionManager.pendingGroup == null)
			{
				return;
			}
			DevkitTransactionManager.pendingGroup.record(transaction);
		}

		/// <summary>
		/// Close the pending transaction and finalize any change checks.
		/// </summary>
		// Token: 0x060007F8 RID: 2040 RVA: 0x0001CB30 File Offset: 0x0001AD30
		public static void endTransaction()
		{
			if (DevkitTransactionManager.transactionDepth == 0)
			{
				return;
			}
			DevkitTransactionManager.transactionDepth--;
			if (DevkitTransactionManager.transactionDepth == 0)
			{
				DevkitTransactionManager.pendingGroup.end();
				if (DevkitTransactionManager.pendingGroup.delta)
				{
					DevkitTransactionManager.pushUndo(DevkitTransactionManager.pendingGroup);
				}
				else
				{
					DevkitTransactionManager.pendingGroup.forget();
				}
				DevkitTransactionManager.pendingGroup = null;
				DevkitTransactionManager.triggerTransactionsChanged();
			}
		}

		/// <summary>
		/// Clear the undo/redo queues.
		/// </summary>
		// Token: 0x060007F9 RID: 2041 RVA: 0x0001CB8F File Offset: 0x0001AD8F
		public static void resetTransactions()
		{
			DevkitTransactionManager.clearUndo();
			DevkitTransactionManager.clearRedo();
			DevkitTransactionManager.pendingGroup = null;
			DevkitTransactionManager.transactionDepth = 0;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001CBA7 File Offset: 0x0001ADA7
		protected static void pushUndo(DevkitTransactionGroup group)
		{
			if ((long)DevkitTransactionManager.undoable.Count >= (long)((ulong)DevkitTransactionManager.historyLength))
			{
				DevkitTransactionManager.undoable.First.Value.forget();
				DevkitTransactionManager.undoable.RemoveFirst();
			}
			DevkitTransactionManager.undoable.AddLast(group);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001CBE6 File Offset: 0x0001ADE6
		protected static DevkitTransactionGroup popUndo()
		{
			DevkitTransactionGroup value = DevkitTransactionManager.undoable.Last.Value;
			DevkitTransactionManager.undoable.RemoveLast();
			return value;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001CC01 File Offset: 0x0001AE01
		protected static void clearUndo()
		{
			while (DevkitTransactionManager.undoable.Count > 0)
			{
				DevkitTransactionGroup value = DevkitTransactionManager.undoable.Last.Value;
				DevkitTransactionManager.undoable.RemoveLast();
				value.forget();
			}
			DevkitTransactionManager.undoable.Clear();
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001CC3A File Offset: 0x0001AE3A
		protected static void pushRedo(DevkitTransactionGroup group)
		{
			DevkitTransactionManager.redoable.Push(group);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001CC47 File Offset: 0x0001AE47
		protected static DevkitTransactionGroup popRedo()
		{
			return DevkitTransactionManager.redoable.Pop();
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001CC53 File Offset: 0x0001AE53
		protected static void clearRedo()
		{
			while (DevkitTransactionManager.redoable.Count > 0)
			{
				DevkitTransactionManager.redoable.Pop().forget();
			}
			DevkitTransactionManager.redoable.Clear();
		}

		// Token: 0x040002D7 RID: 727
		private static uint _historyLength = 25U;

		// Token: 0x040002DA RID: 730
		protected static LinkedList<DevkitTransactionGroup> undoable = new LinkedList<DevkitTransactionGroup>();

		// Token: 0x040002DB RID: 731
		protected static Stack<DevkitTransactionGroup> redoable = new Stack<DevkitTransactionGroup>();

		// Token: 0x040002DC RID: 732
		protected static DevkitTransactionGroup pendingGroup;

		// Token: 0x040002DD RID: 733
		protected static int transactionDepth;
	}
}
