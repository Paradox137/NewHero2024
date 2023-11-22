using System;
using HeroScripts.CameraLogic;
using HeroScripts.Data;
using HeroScripts.Infrastructure;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HeroScripts.Hero
{
	public class HeroMove : MonoBehaviour, ISavedProgress
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private float _movementSpeed;

		private IInputService _inputService;
		private Camera _camera;

		private void Awake()
		{
			_inputService = AllServices.Container.Single<IInputService>();
		}

		private void Start()
		{
			_camera = Camera.main;
		}

		private void Update()
		{
			Vector3 movementVector = Vector3.zero;

			if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
			{
				movementVector = _camera.transform.TransformDirection(_inputService.Axis);
				movementVector.y = 0;
				movementVector.Normalize();
				transform.forward = movementVector;
			}

			movementVector += Physics.gravity;
			
			_characterController.Move(movementVector * (_movementSpeed * Time.deltaTime));
		}
		public void LoadProgress(PlayerProgress progress)
		{
			if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
			{
				Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
				
				if (savedPosition != null)
					DeformTransform(savedPosition);
			}
		}
		
		public void UpdateProgress(PlayerProgress progress)
		{
			progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(),transform.position.AsVector3Data());
		}
		
		private void DeformTransform(Vector3Data to)
		{
			_characterController.enabled = false;
			transform.position = to.AsUnityVector3();
			_characterController.enabled = true;
		}
		private static string CurrentLevel() => SceneManager.GetActiveScene().name;
	}
}
