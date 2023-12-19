using System;
using HeroScripts.Infrastructure;
using HeroScripts.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace HeroScripts.Enemy
{
	public class AgentMoveToHero : Follow
	{
		private const float MinimalDistance = 2f;
		public NavMeshAgent Agent;
		private Transform _heroTransform;

		private IGameFactory _gameFactory;

		public void Construct(Transform heroTransform)
		{
			_heroTransform = heroTransform;
		}
		private void Update()
		{
			if (_heroTransform && HeroNotReached())
				Agent.destination = _heroTransform.position;
		}
		private bool HeroNotReached() 
		{
			return Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
		}
	}
}
