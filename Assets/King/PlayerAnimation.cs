using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody2D), typeof(Animator), typeof(PlayerMovement) )]
public class PlayerAnimation : MonoBehaviour
{
    #region Attributes
    private Rigidbody2D _rb;
    private Animator _anim;
    #endregion

    private void Start()
	{
        _anim = GetComponent<Animator>();
        var player = GetComponent<PlayerMovement>();

        player.playerMoved      +=  horz => _anim.SetFloat("speed", Mathf.Abs(horz));
        player.playerJumped     +=    () => _anim.SetTrigger("jump");
        player.playerJumped     +=    () => _anim.SetBool("jumping", true);
        player.playerIsFalling  +=    () => _anim.SetTrigger("fall");
        player.playerIsGrounded +=    () => _anim.SetBool("jumping", false);
        player.playerIsGrounded +=    () => _anim.SetTrigger("grounded");
    }
}
