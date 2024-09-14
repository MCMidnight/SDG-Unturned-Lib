using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SDG.Unturned
{
	/// <summary>
	/// Responsible for loading asset definitions on a separate thread.
	/// </summary>
	// Token: 0x02000291 RID: 657
	internal class AssetsWorker
	{
		/// <summary>
		/// Used on main thread to determine when all queued tasks have finished.
		/// </summary>
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00049817 File Offset: 0x00047A17
		public bool IsWorking
		{
			get
			{
				return this.isWorking;
			}
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0004981F File Offset: 0x00047A1F
		public void Initialize()
		{
			this.language = Provider.language;
			this.languageIsEnglish = Provider.languageIsEnglish;
			this.shouldWorkerThreadsContinue = 1;
			this.foundMasterBundles = new ConcurrentQueue<AssetsWorker.MasterBundle>();
			this.foundAssetDefinitions = new ConcurrentQueue<AssetsWorker.AssetDefinition>();
			this.exceptions = new ConcurrentQueue<AssetsWorker.ExceptionDetails>();
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0004985F File Offset: 0x00047A5F
		public void Shutdown()
		{
			Interlocked.Exchange(ref this.shouldWorkerThreadsContinue, 0);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00049870 File Offset: 0x00047A70
		public void RequestSearch(string path, AssetOrigin origin)
		{
			this.totalSearchLocationRequests++;
			AssetsWorker.WorkerThreadState workerThreadState = new AssetsWorker.WorkerThreadState
			{
				owner = this,
				rootPath = path,
				searchPaths = new Queue<string>(),
				masterBundleFilePaths = new ConcurrentQueue<string>(),
				assetDefinitionFilePaths = new ConcurrentQueue<AssetsWorker.WorkerThreadState.AssetDefFilePath>(),
				datParser = new DatParser(),
				origin = origin
			};
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.SearcherThreadMain), workerThreadState);
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReaderThreadMain), workerThreadState);
			this.isWorking = true;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x000498FF File Offset: 0x00047AFF
		public bool TryDequeueMasterBundle(out AssetsWorker.MasterBundle result)
		{
			return this.foundMasterBundles.TryDequeue(ref result);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0004990D File Offset: 0x00047B0D
		public bool TryDequeueAssetDefinition(out AssetsWorker.AssetDefinition result)
		{
			return this.foundAssetDefinitions.TryDequeue(ref result);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0004991C File Offset: 0x00047B1C
		public void Update()
		{
			this.isWorking = (this.totalSearchLocationRequests > this.totalSearchLocationsFinishedReading || this.foundMasterBundles.Count > 0 || this.foundAssetDefinitions.Count > 0);
			AssetsWorker.ExceptionDetails exceptionDetails;
			while (this.exceptions.TryDequeue(ref exceptionDetails))
			{
				UnturnedLog.exception(exceptionDetails.exception, exceptionDetails.message);
			}
		}

		/// <summary>
		/// Loop searching directories recursively for asset bundle and asset definition files.
		/// </summary>
		// Token: 0x060013E1 RID: 5089 RVA: 0x00049980 File Offset: 0x00047B80
		private void SearcherThreadMain(object untypedState)
		{
			AssetsWorker.WorkerThreadState workerThreadState = (AssetsWorker.WorkerThreadState)untypedState;
			workerThreadState.searchPaths.Enqueue(workerThreadState.rootPath);
			while (this.shouldWorkerThreadsContinue > 0 && workerThreadState.searchPaths.Count > 0)
			{
				string text = workerThreadState.searchPaths.Dequeue();
				string text2 = Path.Combine(text, "MasterBundle.dat");
				try
				{
					if (File.Exists(text2))
					{
						Interlocked.Increment(ref this.totalMasterBundlesFound);
						workerThreadState.masterBundleFilePaths.Enqueue(text2);
					}
				}
				catch (Exception exception)
				{
					workerThreadState.AddException(exception, "Caught exception reading master bundle config at: \"" + text2 + "\"");
				}
				workerThreadState.FindAssets(text, workerThreadState.origin);
				try
				{
					foreach (string text3 in Directory.EnumerateDirectories(text))
					{
						workerThreadState.searchPaths.Enqueue(text3);
					}
				}
				catch (Exception exception2)
				{
					workerThreadState.AddException(exception2, "Caught exception finding asset subdirectories in: \"" + text + "\"");
				}
			}
			Interlocked.Exchange(ref workerThreadState.isFinishedSearching, 1);
			Interlocked.Increment(ref this.totalSearchLocationsFinishedSearching);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x00049AC0 File Offset: 0x00047CC0
		private void ReaderThreadMain(object untypedState)
		{
			AssetsWorker.<ReaderThreadMain>d__11 <ReaderThreadMain>d__;
			<ReaderThreadMain>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ReaderThreadMain>d__.<>4__this = this;
			<ReaderThreadMain>d__.untypedState = untypedState;
			<ReaderThreadMain>d__.<>1__state = -1;
			<ReaderThreadMain>d__.<>t__builder.Start<AssetsWorker.<ReaderThreadMain>d__11>(ref <ReaderThreadMain>d__);
		}

		// Token: 0x040006C8 RID: 1736
		private int shouldWorkerThreadsContinue;

		// Token: 0x040006C9 RID: 1737
		private ConcurrentQueue<AssetsWorker.MasterBundle> foundMasterBundles;

		// Token: 0x040006CA RID: 1738
		private ConcurrentQueue<AssetsWorker.AssetDefinition> foundAssetDefinitions;

		// Token: 0x040006CB RID: 1739
		internal int totalMasterBundlesFound;

		// Token: 0x040006CC RID: 1740
		internal int totalMasterBundlesRead;

		// Token: 0x040006CD RID: 1741
		internal int totalAssetDefinitionsFound;

		// Token: 0x040006CE RID: 1742
		internal int totalAssetDefinitionsRead;

		// Token: 0x040006CF RID: 1743
		internal int totalSearchLocationRequests;

		// Token: 0x040006D0 RID: 1744
		internal int totalSearchLocationsFinishedSearching;

		// Token: 0x040006D1 RID: 1745
		internal int totalSearchLocationsFinishedReading;

		// Token: 0x040006D2 RID: 1746
		private ConcurrentQueue<AssetsWorker.ExceptionDetails> exceptions;

		// Token: 0x040006D3 RID: 1747
		private bool isWorking;

		// Token: 0x040006D4 RID: 1748
		private string language;

		// Token: 0x040006D5 RID: 1749
		private bool languageIsEnglish;

		// Token: 0x02000913 RID: 2323
		public struct MasterBundle
		{
			// Token: 0x04003230 RID: 12848
			public MasterBundleConfig config;

			// Token: 0x04003231 RID: 12849
			public byte[] assetBundleData;

			// Token: 0x04003232 RID: 12850
			public byte[] hash;
		}

		// Token: 0x02000914 RID: 2324
		public struct AssetDefinition
		{
			// Token: 0x04003233 RID: 12851
			public string path;

			// Token: 0x04003234 RID: 12852
			public byte[] hash;

			// Token: 0x04003235 RID: 12853
			public DatDictionary assetData;

			// Token: 0x04003236 RID: 12854
			public DatDictionary translationData;

			// Token: 0x04003237 RID: 12855
			public DatDictionary fallbackTranslationData;

			/// <summary>
			/// Parser error messages, if any.
			/// </summary>
			// Token: 0x04003238 RID: 12856
			public string assetError;

			/// <summary>
			/// Warning: on worker thread this only acts as handle. Do not access.
			/// </summary>
			// Token: 0x04003239 RID: 12857
			public AssetOrigin origin;
		}

		// Token: 0x02000915 RID: 2325
		private class WorkerThreadState
		{
			// Token: 0x06004A65 RID: 19045 RVA: 0x001B0D38 File Offset: 0x001AEF38
			public DatDictionary ReadFileWithoutHash(string path)
			{
				DatDictionary result;
				using (FileStream fileStream = new FileStream(path, 3, 1, 1))
				{
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						result = this.datParser.Parse(streamReader);
					}
				}
				return result;
			}

			// Token: 0x06004A66 RID: 19046 RVA: 0x001B0D98 File Offset: 0x001AEF98
			public void FindAssets(string dirPath, AssetOrigin origin)
			{
				string fileName = Path.GetFileName(dirPath);
				string text = Path.Combine(dirPath, fileName + ".asset");
				try
				{
					if (File.Exists(text))
					{
						Interlocked.Increment(ref this.owner.totalAssetDefinitionsFound);
						this.assetDefinitionFilePaths.Enqueue(new AssetsWorker.WorkerThreadState.AssetDefFilePath
						{
							filePath = text,
							checkForTranslations = true
						});
						return;
					}
				}
				catch (Exception exception)
				{
					this.AddException(exception, "Caught exception checking for assets at: \"" + text + "\"");
					return;
				}
				text = Path.Combine(dirPath, fileName + ".dat");
				try
				{
					if (File.Exists(text))
					{
						Interlocked.Increment(ref this.owner.totalAssetDefinitionsFound);
						this.assetDefinitionFilePaths.Enqueue(new AssetsWorker.WorkerThreadState.AssetDefFilePath
						{
							filePath = text,
							checkForTranslations = true
						});
						return;
					}
				}
				catch (Exception exception2)
				{
					this.AddException(exception2, "Caught exception checking for assets at: \"" + text + "\"");
					return;
				}
				text = Path.Combine(dirPath, "Asset.dat");
				try
				{
					if (File.Exists(text))
					{
						Interlocked.Increment(ref this.owner.totalAssetDefinitionsFound);
						this.assetDefinitionFilePaths.Enqueue(new AssetsWorker.WorkerThreadState.AssetDefFilePath
						{
							filePath = text,
							checkForTranslations = true
						});
						return;
					}
				}
				catch (Exception exception3)
				{
					this.AddException(exception3, "Caught exception checking for assets at: \"" + text + "\"");
					return;
				}
				try
				{
					foreach (string filePath in Directory.EnumerateFiles(dirPath, "*.asset"))
					{
						Interlocked.Increment(ref this.owner.totalAssetDefinitionsFound);
						this.assetDefinitionFilePaths.Enqueue(new AssetsWorker.WorkerThreadState.AssetDefFilePath
						{
							filePath = filePath,
							checkForTranslations = true
						});
					}
				}
				catch (Exception exception4)
				{
					this.AddException(exception4, "Caught exception checking for assets in: \"" + dirPath + "\"");
				}
			}

			// Token: 0x06004A67 RID: 19047 RVA: 0x001B0FCC File Offset: 0x001AF1CC
			public void AddFoundAsset(string filePath, bool checkForTranslations)
			{
				string directoryName = Path.GetDirectoryName(filePath);
				using (FileStream fileStream = new FileStream(filePath, 3, 1, 1))
				{
					using (SHA1Stream sha1Stream = new SHA1Stream(fileStream))
					{
						using (StreamReader streamReader = new StreamReader(sha1Stream))
						{
							DatDictionary assetData = this.datParser.Parse(streamReader);
							string errorMessage = this.datParser.ErrorMessage;
							byte[] hash = sha1Stream.Hash;
							DatDictionary translationData = null;
							DatDictionary fallbackTranslationData = null;
							if (checkForTranslations)
							{
								string text = Path.Combine(directoryName, this.owner.language + ".dat");
								string text2 = Path.Combine(directoryName, "English.dat");
								if (File.Exists(text))
								{
									translationData = this.ReadFileWithoutHash(text);
									if (!this.owner.languageIsEnglish && File.Exists(text2))
									{
										fallbackTranslationData = this.ReadFileWithoutHash(text2);
									}
								}
								else if (File.Exists(text2))
								{
									translationData = this.ReadFileWithoutHash(text2);
								}
							}
							Interlocked.Increment(ref this.owner.totalAssetDefinitionsRead);
							this.owner.foundAssetDefinitions.Enqueue(new AssetsWorker.AssetDefinition
							{
								path = filePath,
								assetData = assetData,
								assetError = errorMessage,
								hash = hash,
								translationData = translationData,
								fallbackTranslationData = fallbackTranslationData,
								origin = this.origin
							});
						}
					}
				}
			}

			// Token: 0x06004A68 RID: 19048 RVA: 0x001B1174 File Offset: 0x001AF374
			public void AddException(Exception exception, string message)
			{
				this.owner.exceptions.Enqueue(new AssetsWorker.ExceptionDetails
				{
					message = message,
					exception = exception
				});
			}

			// Token: 0x0400323A RID: 12858
			public AssetsWorker owner;

			// Token: 0x0400323B RID: 12859
			public DatParser datParser;

			// Token: 0x0400323C RID: 12860
			public string rootPath;

			// Token: 0x0400323D RID: 12861
			public Queue<string> searchPaths;

			// Token: 0x0400323E RID: 12862
			public ConcurrentQueue<string> masterBundleFilePaths;

			// Token: 0x0400323F RID: 12863
			public ConcurrentQueue<AssetsWorker.WorkerThreadState.AssetDefFilePath> assetDefinitionFilePaths;

			// Token: 0x04003240 RID: 12864
			public int isFinishedSearching;

			/// <summary>
			/// Warning: on worker thread this only acts as handle. Do not access.
			/// </summary>
			// Token: 0x04003241 RID: 12865
			public AssetOrigin origin;

			// Token: 0x02000A34 RID: 2612
			public struct AssetDefFilePath
			{
				// Token: 0x04003559 RID: 13657
				public string filePath;

				// Token: 0x0400355A RID: 13658
				public bool checkForTranslations;
			}
		}

		// Token: 0x02000916 RID: 2326
		private struct ExceptionDetails
		{
			// Token: 0x04003242 RID: 12866
			public string message;

			// Token: 0x04003243 RID: 12867
			public Exception exception;
		}
	}
}
