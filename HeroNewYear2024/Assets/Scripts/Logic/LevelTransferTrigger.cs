using System;
using HeroScripts.Infrastructure;
using HeroScripts.Infrastructure.States;
using UnityEngine;

namespace HeroScripts.Logic
{
	public class LevelTransferTrigger : MonoBehaviour
	{
		private const string PlayerTag = "Player";
		public string TransferTo;
		private IGameStateMachine _stateMachine;
		private bool _triggered;

		public void Construct(IGameStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(_triggered)
				return;

			if (other.CompareTag(PlayerTag))
			{
				_stateMachine.Enter<LoadLevelState, string>(TransferTo);
				_triggered = true;
			}
		}
	}
}
