using System.Collections;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator coroutine);
	}
}
