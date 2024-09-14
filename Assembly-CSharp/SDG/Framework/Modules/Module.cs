using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.Unturned;
using Unturned.SystemEx;

namespace SDG.Framework.Modules
{
	/// <summary>
	/// Wraps module assembly and handles initialization.
	/// </summary>
	// Token: 0x02000091 RID: 145
	public class Module
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000D5E8 File Offset: 0x0000B7E8
		public bool isEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this.isEnabled == value)
				{
					return;
				}
				this._isEnabled = value;
				if (this.isEnabled)
				{
					this.load();
					this.initialize();
					return;
				}
				this.shutdown();
			}
		}

		/// <summary>
		/// Metadata.
		/// </summary>
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000D616 File Offset: 0x0000B816
		// (set) Token: 0x06000378 RID: 888 RVA: 0x0000D61E File Offset: 0x0000B81E
		public ModuleConfig config { get; protected set; }

		/// <summary>
		/// Assembly files loaded.
		/// </summary>
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000D627 File Offset: 0x0000B827
		// (set) Token: 0x0600037A RID: 890 RVA: 0x0000D62F File Offset: 0x0000B82F
		public Assembly[] assemblies { get; protected set; }

		/// <summary>
		/// Types in the assemblies of this module. Refer to this for types rather than the assemblies to avoid exception and garbage.
		/// </summary>
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000D638 File Offset: 0x0000B838
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0000D640 File Offset: 0x0000B840
		public Type[] types { get; protected set; }

		/// <summary>
		/// How far along the initialization to shutdown lifecycle this module is.
		/// </summary>
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0000D649 File Offset: 0x0000B849
		// (set) Token: 0x0600037E RID: 894 RVA: 0x0000D651 File Offset: 0x0000B851
		public EModuleStatus status { get; protected set; }

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600037F RID: 895 RVA: 0x0000D65C File Offset: 0x0000B85C
		// (remove) Token: 0x06000380 RID: 896 RVA: 0x0000D694 File Offset: 0x0000B894
		public event ModuleLoaded onModuleLoaded;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000381 RID: 897 RVA: 0x0000D6CC File Offset: 0x0000B8CC
		// (remove) Token: 0x06000382 RID: 898 RVA: 0x0000D704 File Offset: 0x0000B904
		public event ModuleInitialized onModuleInitialized;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000383 RID: 899 RVA: 0x0000D73C File Offset: 0x0000B93C
		// (remove) Token: 0x06000384 RID: 900 RVA: 0x0000D774 File Offset: 0x0000B974
		public event ModuleShutdown onModuleShutdown;

		// Token: 0x06000385 RID: 901 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		protected void register()
		{
			if (this.config == null)
			{
				return;
			}
			foreach (ModuleAssembly moduleAssembly in this.config.Assemblies)
			{
				ModuleHook.registerAssemblyPath(this.config.DirectoryPath + moduleAssembly.Path, moduleAssembly.Load_As_Byte_Array);
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000D828 File Offset: 0x0000BA28
		protected void load()
		{
			if (this.config == null || this.assemblies != null)
			{
				return;
			}
			if (!this.config.IsEnabled)
			{
				return;
			}
			List<Type> list = new List<Type>();
			this.assemblies = new Assembly[this.config.Assemblies.Count];
			for (int i = 0; i < this.config.Assemblies.Count; i++)
			{
				Assembly assembly = ModuleHook.resolveAssemblyPath(this.config.DirectoryPath + this.config.Assemblies[i].Path);
				this.assemblies[i] = assembly;
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					types = ex.Types;
				}
				if (types != null)
				{
					for (int j = 0; j < types.Length; j++)
					{
						if (!(types[j] == null))
						{
							list.Add(types[j]);
						}
					}
				}
			}
			this.types = list.ToArray();
			ModuleLoaded moduleLoaded = this.onModuleLoaded;
			if (moduleLoaded == null)
			{
				return;
			}
			moduleLoaded(this);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000D934 File Offset: 0x0000BB34
		protected void initialize()
		{
			if (this.config == null || this.assemblies == null)
			{
				return;
			}
			if (this.status != EModuleStatus.None && this.status != EModuleStatus.Shutdown)
			{
				return;
			}
			this.nexii.Clear();
			Type typeFromHandle = typeof(IModuleNexus);
			for (int i = 0; i < this.types.Length; i++)
			{
				Type type = this.types[i];
				try
				{
					if (!type.IsAbstract && TypeEx.TryIsAssignableFrom(typeFromHandle, type))
					{
						IModuleNexus moduleNexus = Activator.CreateInstance(type) as IModuleNexus;
						try
						{
							moduleNexus.initialize();
						}
						catch (Exception e)
						{
							UnturnedLog.error(string.Concat(new string[]
							{
								"Caught exception while initializing module \"",
								this.config.Name,
								"\" entry point \"",
								type.Name,
								"\":"
							}));
							UnturnedLog.exception(e);
						}
						this.nexii.Add(moduleNexus);
					}
				}
				catch (Exception e2)
				{
					UnturnedLog.exception(e2, string.Concat(new string[]
					{
						"Caught exception while searching for entry points in module \"",
						this.config.Name,
						"\" type \"",
						type.Name,
						"\""
					}));
				}
			}
			this.status = EModuleStatus.Initialized;
			UnturnedLog.info("Initialized module \"" + this.config.Name + "\"");
			ModuleInitialized moduleInitialized = this.onModuleInitialized;
			if (moduleInitialized == null)
			{
				return;
			}
			moduleInitialized(this);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000DAB0 File Offset: 0x0000BCB0
		protected void shutdown()
		{
			if (this.config == null || this.assemblies == null)
			{
				return;
			}
			if (this.status != EModuleStatus.Initialized)
			{
				return;
			}
			for (int i = 0; i < this.nexii.Count; i++)
			{
				try
				{
					this.nexii[i].shutdown();
				}
				catch (Exception e)
				{
					UnturnedLog.error("Caught exception while shutting down module \"" + this.config.Name + "\":");
					UnturnedLog.exception(e);
				}
			}
			this.nexii.Clear();
			this.status = EModuleStatus.Shutdown;
			UnturnedLog.info("Shutdown module \"" + this.config.Name + "\"");
			ModuleShutdown moduleShutdown = this.onModuleShutdown;
			if (moduleShutdown == null)
			{
				return;
			}
			moduleShutdown(this);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000DB7C File Offset: 0x0000BD7C
		public Module(ModuleConfig newConfig)
		{
			this.config = newConfig;
			this.isEnabled = false;
			this.status = EModuleStatus.None;
			this.nexii = new List<IModuleNexus>();
			this.register();
		}

		/// <summary>
		/// True when config is enabled and dependencies are enabled.
		/// </summary>
		// Token: 0x04000177 RID: 375
		protected bool _isEnabled;

		// Token: 0x0400017C RID: 380
		private List<IModuleNexus> nexii;
	}
}
