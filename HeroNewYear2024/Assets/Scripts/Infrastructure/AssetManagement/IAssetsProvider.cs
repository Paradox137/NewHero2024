using System.Threading.Tasks;
using HeroScripts.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HeroScripts.Infrastructure.AssetManagement
{
	public interface IAssetsProvider : IService
	{
		Task<GameObject> Instantiate(string path, Vector3 at);
		Task<GameObject> Instantiate(string path);
		Task<T> Load<T>(AssetReference assetReference) where T : class;
		void Cleanup();
		Task<T> Load<T>(string address) where T : class;
		void Initialize();
	}
}
