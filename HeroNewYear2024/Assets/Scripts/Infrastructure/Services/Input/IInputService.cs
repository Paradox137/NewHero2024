using HeroScripts.Infrastructure.Services;
using UnityEngine;

namespace HeroScripts.Services.Input
{
	public interface IInputService : IService	
	{
		Vector2 Axis { get; }
	}
}
