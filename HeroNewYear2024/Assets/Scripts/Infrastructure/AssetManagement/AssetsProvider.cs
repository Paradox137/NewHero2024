using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HeroScripts.Infrastructure.AssetManagement
{
	public class AssetsProvider : IAssetsProvider
	{
		private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
		private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

		public void Initialize()
		{
			Addressables.InitializeAsync();
		}

		public GameObject Instantiate(string path, Vector3 at)
		{
			var prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab, at, Quaternion.identity);
		}

		public GameObject Instantiate(string path)
		{
			var prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab);
		}
		public Task<T> Load<T>(string address) where T : class
		{
			throw new System.NotImplementedException();
		}
		public async Task<T> Load<T>(AssetReference assetReference) where T : class
		{
			if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
				return completedHandle.Result as T;

			return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID);
		}
		private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
		{
			handle.Completed += completeHandle =>
			{
				_completedCache[cacheKey] = completeHandle;
			};

			AddHandle<T>(cacheKey, handle);

			return await handle.Task;
		}
		private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : class
		{
			if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
			{
				resourceHandles = new List<AsyncOperationHandle>();
				_handles[key] = resourceHandles;
			}

			resourceHandles.Add(handle);
		}
		public void Cleanup()
		{
			foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
				foreach (AsyncOperationHandle handle in resourceHandles)
					Addressables.Release(handle);
			
			_completedCache.Clear();
			_handles.Clear();
		}
	}
}
