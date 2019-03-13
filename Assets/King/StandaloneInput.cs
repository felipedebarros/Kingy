using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(PlayerMovement) )]
public class StandaloneInput : MonoBehaviour {

	private PlayerMovement _player;

	void Start () {
		_player = GetComponent<PlayerMovement>();
	}
	
	void Update () {
		
		_player.Move(Input.GetAxis("Horizontal"));

		if(Input.GetButtonDown("Jump"))
			_player.Jump();
		if(Input.GetButtonUp("Jump"))
			_player.ReleaseJump();

		// if(Input.GetKeyDown(KeyCode.Z))
		// 	_player.Interact();
	}
}
