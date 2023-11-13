using System;
using HeroScripts.CameraLogic;
using HeroScripts.Infrastructure;
using HeroScripts.Services.Input;
using UnityEngine;

namespace HeroScripts.Hero
{
	public class HeroMove : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private float _movementSpeed;

		private IInputService _inputService;
		private Camera _camera;

		private void Awake()
		{
			_inputService = Game.InputService;
		}

		private void Start()
		{
			_camera = Camera.main;

			CameraFollow();
		}

		private void Update()
		{
			//RotateIsometric();
			
			Vector3 movementVector = Vector3.zero;

			if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
			{
				movementVector = _camera.transform.TransformDirection(_inputService.Axis);
				movementVector.y = 0;
				movementVector.ToIso();
				movementVector.Normalize();
				
				transform.forward = movementVector;
			}

			movementVector += Physics.gravity;
			
			_characterController.Move(movementVector * (_movementSpeed * Time.deltaTime));
		}


		private void RotateIsometric()
		{
			Vector3  relative = (transform.position + _camera.transform.TransformDirection(_inputService.Axis).ToIso())
				- transform.position;
		
			var rot = Quaternion.LookRotation(relative, Vector3.up);

			transform.rotation = rot;
		}
		private void CameraFollow() => _camera.GetComponentInParent<CameraFollow>().Follow(gameObject);
	}
}
