using HeroScripts.Infrastructure.Services;
using UnityEngine;

namespace HeroScripts.Infrastructure.AssetManagement
{
	public interface IAssetsProvider : IService
	{
		GameObject Instantiate(string path, Vector3 at);
		GameObject Instantiate(string path);
	}
}
