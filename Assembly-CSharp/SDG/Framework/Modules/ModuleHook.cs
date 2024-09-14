using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SDG.Framework.IO;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Framework.Modules
{
	/// <summary>
	/// Runs before everything else to find and load modules.
	/// </summary>
	// Token: 0x02000098 RID: 152
	public class ModuleHook : MonoBehaviour
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0000DCBF File Offset: 0x0000BEBF
		// (set) Token: 0x06000398 RID: 920 RVA: 0x0000DCC6 File Offset: 0x0000BEC6
		public static List<Module> modules { get; protected set; }

		/// <summary>
		/// Temporarily contains Unturned's code untils it's moved into modules.
		/// </summary>
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000DCCE File Offset: 0x0000BECE
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0000DCD5 File Offset: 0x0000BED5
		public static Assembly coreAssembly { get; protected set; }

		/// <summary>
		/// Temporarily contains <see cref="P:SDG.Framework.Modules.ModuleHook.coreAssembly" /> types.
		/// </summary>
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0000DCDD File Offset: 0x0000BEDD
		// (set) Token: 0x0600039C RID: 924 RVA: 0x0000DCE4 File Offset: 0x0000BEE4
		public static Type[] coreTypes { get; protected set; }

		/// <summary>
		/// Should module assemblies be loaded?
		/// </summary>
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000DCEC File Offset: 0x0000BEEC
		private static bool shouldLoadModules
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Called once after all startup enabled modules are loaded. Not called when modules are initialized due to enabling/disabling.
		/// </summary>
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600039E RID: 926 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		// (remove) Token: 0x0600039F RID: 927 RVA: 0x0000DD24 File Offset: 0x0000BF24
		public static event ModulesInitializedHandler onModulesInitialized;

		/// <summary>
		/// Called once after all modules are shutdown. Not called when modules are shutdown due to enabling/disabling.
		/// </summary>
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060003A0 RID: 928 RVA: 0x0000DD58 File Offset: 0x0000BF58
		// (remove) Token: 0x060003A1 RID: 929 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		public static event ModulesShutdownHandler onModulesShutdown;

		/// <summary>
		/// Find modules containing an assembly with the Both_Required role.
		/// </summary>
		/// <param name="result">Modules to append to.</param>
		// Token: 0x060003A2 RID: 930 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
		public static void getRequiredModules(List<Module> result)
		{
			if (ModuleHook.modules == null || result == null)
			{
				return;
			}
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				if (module != null)
				{
					ModuleConfig config = module.config;
					if (config != null)
					{
						for (int j = 0; j < config.Assemblies.Count; j++)
						{
							ModuleAssembly moduleAssembly = config.Assemblies[j];
							if (moduleAssembly != null && moduleAssembly.Role == EModuleRole.Both_Required)
							{
								result.Add(module);
								break;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Find module using dependency name.
		/// </summary>
		/// <returns></returns>
		// Token: 0x060003A3 RID: 931 RVA: 0x0000DE44 File Offset: 0x0000C044
		public static Module getModuleByName(string name)
		{
			if (ModuleHook.modules == null)
			{
				return null;
			}
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				if (module != null && module.config != null && module.config.Name == name)
				{
					return module;
				}
			}
			return null;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000DE9C File Offset: 0x0000C09C
		public static void toggleModuleEnabled(int index)
		{
			if (index < 0 || index >= ModuleHook.modules.Count)
			{
				return;
			}
			Module module = ModuleHook.modules[index];
			ModuleConfig config = module.config;
			config.IsEnabled = !config.IsEnabled;
			IOUtility.jsonSerializer.serialize<ModuleConfig>(module.config, config.FilePath, true);
			ModuleHook.updateModuleEnabled(index, config.IsEnabled);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000DF00 File Offset: 0x0000C100
		public static void registerAssemblyPath(string path)
		{
			ModuleHook.registerAssemblyPath(path, false);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000DF0C File Offset: 0x0000C10C
		public static void registerAssemblyPath(string path, bool loadAsByteArray)
		{
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(path);
			if (!ModuleHook.nameToPath.ContainsKey(assemblyName.FullName))
			{
				ModuleHook.AssemblyFileSettings assemblyFileSettings = new ModuleHook.AssemblyFileSettings();
				assemblyFileSettings.absolutePath = path;
				assemblyFileSettings.loadAsByteArray = loadAsByteArray;
				ModuleHook.nameToPath.Add(assemblyName.FullName, assemblyFileSettings);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000DF58 File Offset: 0x0000C158
		public static Assembly resolveAssemblyName(string name)
		{
			Assembly assembly;
			if (ModuleHook.nameToAssembly.TryGetValue(name, ref assembly))
			{
				return assembly;
			}
			ModuleHook.AssemblyFileSettings assemblyFileSettings;
			if (ModuleHook.nameToPath.TryGetValue(name, ref assemblyFileSettings))
			{
				if (assemblyFileSettings.loadAsByteArray)
				{
					assembly = Assembly.Load(File.ReadAllBytes(assemblyFileSettings.absolutePath));
				}
				else
				{
					assembly = Assembly.LoadFile(assemblyFileSettings.absolutePath);
				}
				ModuleHook.nameToAssembly.Add(name, assembly);
				return assembly;
			}
			return null;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000DFBC File Offset: 0x0000C1BC
		private static Assembly LoadAssemblyFromDiscoveredPaths(AssemblyName loadAssemblyName)
		{
			Assembly assembly = null;
			try
			{
				foreach (KeyValuePair<AssemblyName, string> keyValuePair in ModuleHook.discoveredNameToPath)
				{
					AssemblyName key = keyValuePair.Key;
					string value = keyValuePair.Value;
					if (string.Equals(key.Name, loadAssemblyName.Name) && key.Version >= loadAssemblyName.Version)
					{
						if (ModuleHook.shouldLogAssemblyResolve)
						{
							UnturnedLog.info(string.Format("Using discovered assembly for \"{0}\" at \"{1}\"", loadAssemblyName, value));
						}
						if (ModuleHook.nameToAssembly.TryGetValue(key.Name, ref assembly))
						{
							break;
						}
						assembly = Assembly.Load(File.ReadAllBytes(value));
						if (assembly != null)
						{
							ModuleHook.nameToAssembly.Add(key.Name, assembly);
							break;
						}
						break;
					}
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, string.Format("Caught exception loading assembly for \"{0}\" from discovered paths:", loadAssemblyName));
			}
			return assembly;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000E0C4 File Offset: 0x0000C2C4
		public static Assembly resolveAssemblyPath(string path)
		{
			return ModuleHook.resolveAssemblyName(AssemblyName.GetAssemblyName(path).FullName);
		}

		/// <summary>
		/// Event for plugin frameworks (e.g., Rocket) to override AssemblyResolve handling.
		/// </summary>
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060003AA RID: 938 RVA: 0x0000E0D8 File Offset: 0x0000C2D8
		// (remove) Token: 0x060003AB RID: 939 RVA: 0x0000E10C File Offset: 0x0000C30C
		public static event ResolveEventHandler PreVanillaAssemblyResolve;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060003AC RID: 940 RVA: 0x0000E140 File Offset: 0x0000C340
		// (remove) Token: 0x060003AD RID: 941 RVA: 0x0000E174 File Offset: 0x0000C374
		public static event ResolveEventHandler PreVanillaAssemblyResolvePostRedirects;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060003AE RID: 942 RVA: 0x0000E1A8 File Offset: 0x0000C3A8
		// (remove) Token: 0x060003AF RID: 943 RVA: 0x0000E1DC File Offset: 0x0000C3DC
		public static event ResolveEventHandler PostVanillaAssemblyResolve;

		// Token: 0x060003B0 RID: 944 RVA: 0x0000E210 File Offset: 0x0000C410
		protected Assembly handleAssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (ModuleHook.PreVanillaAssemblyResolve != null)
			{
				Assembly assembly = ModuleHook.PreVanillaAssemblyResolve.Invoke(sender, args);
				if (ModuleHook.shouldLogAssemblyResolve)
				{
					if (assembly != null)
					{
						UnturnedLog.info(string.Format("PreVanillaAssemblyResolve found \"{0}\" for \"{1}\"", assembly.FullName, args.RequestingAssembly));
					}
					else
					{
						UnturnedLog.info(string.Format("PreVanillaAssemblyResolve is bound but unable to find \"{0}\" for \"{1}\"", args.Name, args.RequestingAssembly));
					}
				}
				if (assembly != null)
				{
					return assembly;
				}
			}
			if (string.Equals(args.Name, "Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"))
			{
				if (ModuleHook.shouldLogAssemblyResolve)
				{
					UnturnedLog.info("Redirecting Assembly-CSharp-firstpass to com.rlabrecque.steamworks.net for {0}", new object[]
					{
						args.RequestingAssembly
					});
				}
				return typeof(SteamAPI).Assembly;
			}
			if (string.Equals(args.Name, "Steamworks.NET, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"))
			{
				if (ModuleHook.shouldLogAssemblyResolve)
				{
					UnturnedLog.info("Redirecting Steamworks.NET to com.rlabrecque.steamworks.net for {0}", new object[]
					{
						args.RequestingAssembly
					});
				}
				return typeof(SteamAPI).Assembly;
			}
			if (ModuleHook.PreVanillaAssemblyResolvePostRedirects != null)
			{
				Assembly assembly2 = ModuleHook.PreVanillaAssemblyResolvePostRedirects.Invoke(sender, args);
				if (ModuleHook.shouldLogAssemblyResolve)
				{
					if (assembly2 != null)
					{
						UnturnedLog.info(string.Format("PreVanillaAssemblyResolvePostRedirects found \"{0}\" for \"{1}\"", assembly2.FullName, args.RequestingAssembly));
					}
					else
					{
						UnturnedLog.info(string.Format("PreVanillaAssemblyResolvePostRedirects is bound but unable to find \"{0}\" for \"{1}\"", args.Name, args.RequestingAssembly));
					}
				}
				if (assembly2 != null)
				{
					return assembly2;
				}
			}
			Assembly assembly3 = ModuleHook.resolveAssemblyName(args.Name);
			if (assembly3 != null)
			{
				return assembly3;
			}
			if (ModuleHook.shouldSearchModulesForDLLs)
			{
				assembly3 = ModuleHook.LoadAssemblyFromDiscoveredPaths(new AssemblyName(args.Name));
				if (assembly3 != null)
				{
					return assembly3;
				}
			}
			if (ModuleHook.shouldLogAssemblyResolve)
			{
				UnturnedLog.error("Vanilla unable to resolve dependency \"" + args.Name + "\"! Please include it in one of your module assembly lists.");
			}
			if (ModuleHook.PostVanillaAssemblyResolve != null)
			{
				Assembly assembly4 = ModuleHook.PostVanillaAssemblyResolve.Invoke(sender, args);
				if (ModuleHook.shouldLogAssemblyResolve)
				{
					if (assembly4 != null)
					{
						UnturnedLog.info(string.Format("PostVanillaAssemblyResolve found \"{0}\" for \"{1}\"", assembly4.FullName, args.RequestingAssembly));
					}
					else
					{
						UnturnedLog.info(string.Format("PostVanillaAssemblyResolve is bound but unable to find \"{0}\" for \"{1}\"", args.Name, args.RequestingAssembly));
					}
				}
				if (assembly4 != null)
				{
					return assembly4;
				}
			}
			return null;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000E458 File Offset: 0x0000C658
		protected Assembly OnTypeResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("SDG.NetTransport."))
			{
				UnturnedLog.info("Redirecting type \"{0}\" assembly for {1}", new object[]
				{
					args.Name,
					args.RequestingAssembly
				});
				return typeof(ITransportConnection).Assembly;
			}
			UnturnedLog.info("Unable to resolve type \"{0}\" for {1}", new object[]
			{
				args.Name,
				args.RequestingAssembly
			});
			return null;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000E4CC File Offset: 0x0000C6CC
		private static bool areModuleDependenciesEnabled(int moduleIndex)
		{
			ModuleConfig config = ModuleHook.modules[moduleIndex].config;
			for (int i = 0; i < config.Dependencies.Count; i++)
			{
				ModuleDependency moduleDependency = config.Dependencies[i];
				int num = moduleIndex - 1;
				while (moduleIndex >= 0)
				{
					if (ModuleHook.modules[num].config.Name == moduleDependency.Name && !ModuleHook.modules[num].isEnabled)
					{
						return false;
					}
					moduleIndex--;
				}
			}
			return true;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000E554 File Offset: 0x0000C754
		private static void updateModuleEnabled(int index, bool enable)
		{
			if (enable)
			{
				if (ModuleHook.modules[index].config.IsEnabled && ModuleHook.areModuleDependenciesEnabled(index) && !ModuleHook.isModuleDisabledByCommandLine(ModuleHook.modules[index].config.Name))
				{
					ModuleHook.modules[index].isEnabled = true;
					for (int i = index + 1; i < ModuleHook.modules.Count; i++)
					{
						for (int j = 0; j < ModuleHook.modules[i].config.Dependencies.Count; j++)
						{
							if (ModuleHook.modules[i].config.Dependencies[j].Name == ModuleHook.modules[index].config.Name)
							{
								ModuleHook.updateModuleEnabled(i, true);
								break;
							}
						}
					}
					return;
				}
			}
			else
			{
				for (int k = ModuleHook.modules.Count - 1; k > index; k--)
				{
					for (int l = 0; l < ModuleHook.modules[k].config.Dependencies.Count; l++)
					{
						if (ModuleHook.modules[k].config.Dependencies[l].Name == ModuleHook.modules[index].config.Name)
						{
							ModuleHook.updateModuleEnabled(k, false);
							break;
						}
					}
				}
				ModuleHook.modules[index].isEnabled = false;
			}
		}

		/// <summary>
		/// Depending on the platform, assemblies are found in different directories.
		/// </summary>
		/// <returns>Root folder for modules.</returns>
		// Token: 0x060003B4 RID: 948 RVA: 0x0000E6D0 File Offset: 0x0000C8D0
		private string getModulesRootPath()
		{
			string text = ReadWrite.PATH;
			text += "/Modules";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		/// <summary>
		/// Search Modules directory for .dll files and save their AssemblyName to discoveredNameToPath.
		/// </summary>
		// Token: 0x060003B5 RID: 949 RVA: 0x0000E700 File Offset: 0x0000C900
		private void DiscoverAssemblies()
		{
			try
			{
				foreach (string text in Directory.GetFiles(this.getModulesRootPath(), "*.dll", 1))
				{
					try
					{
						AssemblyName assemblyName = AssemblyName.GetAssemblyName(text);
						UnturnedLog.info(string.Format("Discovered assembly \"{0}\" at \"{1}\"", assemblyName, text));
						string text2;
						if (!ModuleHook.discoveredNameToPath.TryGetValue(assemblyName, ref text2))
						{
							ModuleHook.discoveredNameToPath.Add(assemblyName, text);
							UnturnedLog.info(string.Format("Discovered assembly \"{0}\" at \"{1}\"", assemblyName, text));
						}
						else
						{
							UnturnedLog.info(string.Format("Discovered duplicate of assembly \"{0}\" at \"{1}\" (first found at \"{2}\")", assemblyName, text, text2));
						}
					}
					catch (Exception ex)
					{
						UnturnedLog.info(string.Concat(new string[]
						{
							"Caught exception trying to determine AssemblyName for dll \"",
							text,
							"\": \"",
							ex.Message,
							"\""
						}));
					}
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception discovering assemblies in Modules folder:");
			}
		}

		/// <summary>
		/// Search Modules directory for .module files and load them.
		/// </summary>
		// Token: 0x060003B6 RID: 950 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		private List<ModuleConfig> findModules()
		{
			List<ModuleConfig> list = new List<ModuleConfig>();
			string modulesRootPath = this.getModulesRootPath();
			this.findModules(modulesRootPath, list);
			return list;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000E81C File Offset: 0x0000CA1C
		private void findModules(string path, List<ModuleConfig> configs)
		{
			foreach (string text in Directory.GetFiles(path, "*.module"))
			{
				try
				{
					ModuleConfig moduleConfig = IOUtility.jsonDeserializer.deserialize<ModuleConfig>(text);
					if (moduleConfig == null)
					{
						UnturnedLog.warn("Unable to parse module config file: " + text);
					}
					else
					{
						moduleConfig.DirectoryPath = path;
						moduleConfig.FilePath = text;
						moduleConfig.Version_Internal = Parser.getUInt32FromIP(moduleConfig.Version);
						for (int j = moduleConfig.Dependencies.Count - 1; j >= 0; j--)
						{
							ModuleDependency moduleDependency = moduleConfig.Dependencies[j];
							if (moduleDependency.Name == "Framework" || moduleDependency.Name == "Unturned")
							{
								moduleConfig.Dependencies.RemoveAtFast(j);
							}
							else
							{
								moduleDependency.Version_Internal = Parser.getUInt32FromIP(moduleDependency.Version);
							}
						}
						configs.Add(moduleConfig);
					}
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Caught exception parsing .module file: " + text);
				}
			}
			foreach (string path2 in Directory.GetDirectories(path))
			{
				this.findModules(path2, configs);
			}
		}

		/// <summary>
		/// Orders configs by dependency and removes those that are missing files.
		/// </summary>
		// Token: 0x060003B8 RID: 952 RVA: 0x0000E960 File Offset: 0x0000CB60
		private void sortModules(List<ModuleConfig> configs)
		{
			ModuleComparer moduleComparer = new ModuleComparer();
			configs.Sort(moduleComparer);
			for (int i = 0; i < configs.Count; i++)
			{
				ModuleConfig moduleConfig = configs[i];
				bool flag = true;
				for (int j = moduleConfig.Assemblies.Count - 1; j >= 0; j--)
				{
					ModuleAssembly moduleAssembly = moduleConfig.Assemblies[j];
					if (moduleAssembly.Role == EModuleRole.Client)
					{
						moduleConfig.Assemblies.RemoveAt(j);
					}
					else
					{
						EModuleRole role = moduleAssembly.Role;
						bool flag2 = false;
						for (int k = 1; k < moduleAssembly.Path.Length; k++)
						{
							if (moduleAssembly.Path.get_Chars(k) == '.' && moduleAssembly.Path.get_Chars(k - 1) == '.')
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							flag = false;
							break;
						}
						string text = moduleConfig.DirectoryPath + moduleAssembly.Path;
						if (!File.Exists(text))
						{
							flag = false;
							UnturnedLog.warn("Module \"" + moduleConfig.Name + "\" missing assembly: " + text);
							break;
						}
					}
				}
				if (!flag || moduleConfig.Assemblies.Count == 0)
				{
					configs.RemoveAt(i);
					i--;
					UnturnedLog.info("Discard module \"" + moduleConfig.Name + "\" because it has no assemblies");
				}
				else
				{
					for (int l = 0; l < moduleConfig.Dependencies.Count; l++)
					{
						ModuleDependency moduleDependency = moduleConfig.Dependencies[l];
						bool flag3 = false;
						int m = i - 1;
						while (m >= 0)
						{
							if (configs[m].Name == moduleDependency.Name)
							{
								if (configs[m].Version_Internal >= moduleDependency.Version_Internal)
								{
									flag3 = true;
									break;
								}
								break;
							}
							else
							{
								m--;
							}
						}
						if (!flag3)
						{
							configs.RemoveAtFast(i);
							i--;
							UnturnedLog.warn(string.Concat(new string[]
							{
								"Discard module \"",
								moduleConfig.Name,
								"\" because dependency \"",
								moduleDependency.Name,
								"\" wasn't met"
							}));
							break;
						}
					}
				}
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000EB80 File Offset: 0x0000CD80
		private void loadModules()
		{
			ModuleHook.modules = new List<Module>();
			ModuleHook.nameToPath = new Dictionary<string, ModuleHook.AssemblyFileSettings>();
			ModuleHook.discoveredNameToPath = new Dictionary<AssemblyName, string>();
			ModuleHook.nameToAssembly = new Dictionary<string, Assembly>();
			if (ModuleHook.shouldLoadModules)
			{
				if (ModuleHook.shouldSearchModulesForDLLs)
				{
					this.DiscoverAssemblies();
				}
				List<ModuleConfig> list = this.findModules();
				this.sortModules(list);
				if (list.Count > 0)
				{
					UnturnedLog.info(string.Format("Found {0} module(s):", list.Count));
				}
				for (int i = 0; i < list.Count; i++)
				{
					ModuleConfig moduleConfig = list[i];
					if (moduleConfig != null)
					{
						UnturnedLog.info(string.Format("{0}: \"{1}\"", i, moduleConfig.Name));
						Module module = new Module(moduleConfig);
						ModuleHook.modules.Add(module);
					}
				}
				return;
			}
			UnturnedLog.info("Disabling module loading because BattlEye is enabled");
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000EC58 File Offset: 0x0000CE58
		private static bool isModuleDisabledByCommandLine(string moduleName)
		{
			string commandLine = Environment.CommandLine;
			int num = commandLine.IndexOf(moduleName, 5);
			if (num == -1)
			{
				return false;
			}
			string text = "-disableModule/";
			int num2 = num - text.Length;
			return num2 >= 0 && commandLine.Substring(num2, text.Length) == text;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000ECA8 File Offset: 0x0000CEA8
		private void initializeModules()
		{
			if (ModuleHook.modules == null)
			{
				return;
			}
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				ModuleConfig config = module.config;
				bool isEnabled;
				if (!config.IsEnabled)
				{
					isEnabled = false;
					UnturnedLog.info("Disabling module \"" + config.Name + "\" as requested by config");
				}
				else if (!ModuleHook.areModuleDependenciesEnabled(i))
				{
					isEnabled = false;
					UnturnedLog.info("Disabling module \"" + config.Name + "\" because dependencies are disabled");
				}
				else if (ModuleHook.isModuleDisabledByCommandLine(config.Name))
				{
					isEnabled = false;
					UnturnedLog.info("Disabling module \"" + config.Name + "\" as requested by command-line");
				}
				else
				{
					isEnabled = true;
				}
				module.isEnabled = isEnabled;
			}
			ModulesInitializedHandler modulesInitializedHandler = ModuleHook.onModulesInitialized;
			if (modulesInitializedHandler == null)
			{
				return;
			}
			modulesInitializedHandler();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000ED78 File Offset: 0x0000CF78
		private void shutdownModules()
		{
			if (ModuleHook.modules == null)
			{
				return;
			}
			for (int i = ModuleHook.modules.Count - 1; i >= 0; i--)
			{
				Module module = ModuleHook.modules[i];
				if (module != null)
				{
					module.isEnabled = false;
				}
			}
			ModulesShutdownHandler modulesShutdownHandler = ModuleHook.onModulesShutdown;
			if (modulesShutdownHandler == null)
			{
				return;
			}
			modulesShutdownHandler();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000EDCC File Offset: 0x0000CFCC
		public void awake()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.handleAssemblyResolve);
			AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(this.OnTypeResolve);
			ModuleHook.coreAssembly = Assembly.GetExecutingAssembly();
			try
			{
				ModuleHook.coreTypes = ModuleHook.coreAssembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				ModuleHook.coreTypes = ex.Types;
			}
			this.loadModules();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000EE44 File Offset: 0x0000D044
		public void start()
		{
			ModuleHook.coreNexii = new List<IModuleNexus>();
			ModuleHook.coreNexii.Clear();
			Type typeFromHandle = typeof(IModuleNexus);
			for (int i = 0; i < ModuleHook.coreTypes.Length; i++)
			{
				Type type = ModuleHook.coreTypes[i];
				if (!type.IsAbstract && TypeEx.TryIsAssignableFrom(typeFromHandle, type))
				{
					IModuleNexus moduleNexus = Activator.CreateInstance(type) as IModuleNexus;
					try
					{
						moduleNexus.initialize();
					}
					catch (Exception e)
					{
						UnturnedLog.error("Failed to initialize nexus!");
						UnturnedLog.exception(e);
					}
					ModuleHook.coreNexii.Add(moduleNexus);
				}
			}
			this.initializeModules();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000EEE4 File Offset: 0x0000D0E4
		private void OnDestroy()
		{
			this.shutdownModules();
			for (int i = 0; i < ModuleHook.coreNexii.Count; i++)
			{
				try
				{
					ModuleHook.coreNexii[i].shutdown();
				}
				catch (Exception e)
				{
					UnturnedLog.error("Failed to shutdown nexus!");
					UnturnedLog.exception(e);
				}
			}
			ModuleHook.coreNexii.Clear();
			AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(this.handleAssemblyResolve);
			AppDomain.CurrentDomain.TypeResolve -= new ResolveEventHandler(this.OnTypeResolve);
		}

		// Token: 0x04000191 RID: 401
		private static List<IModuleNexus> coreNexii;

		// Token: 0x04000194 RID: 404
		protected static Dictionary<string, ModuleHook.AssemblyFileSettings> nameToPath;

		/// <summary>
		/// These are *.dll files discovered in the modules folder.
		/// </summary>
		// Token: 0x04000195 RID: 405
		protected static Dictionary<AssemblyName, string> discoveredNameToPath;

		// Token: 0x04000196 RID: 406
		protected static Dictionary<string, Assembly> nameToAssembly;

		/// <summary>
		/// Should missing DLLs be logged?
		/// Opt-in because RocketMod has its own handler.
		/// </summary>
		// Token: 0x04000197 RID: 407
		private static CommandLineFlag shouldLogAssemblyResolve = new CommandLineFlag(false, "-LogAssemblyResolve");

		/// <summary>
		/// Should vanilla search for *.dll files?
		/// Can be turned off in case it conflicts with third-party search mechanism.
		/// </summary>
		// Token: 0x04000198 RID: 408
		private static CommandLineFlag shouldSearchModulesForDLLs = new CommandLineFlag(true, "-NoVanillaAssemblySearch");

		// Token: 0x02000858 RID: 2136
		protected class AssemblyFileSettings
		{
			// Token: 0x04003152 RID: 12626
			public string absolutePath;

			// Token: 0x04003153 RID: 12627
			public bool loadAsByteArray;
		}
	}
}
