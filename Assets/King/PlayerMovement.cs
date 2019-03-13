using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent( typeof(Rigidbody2D), typeof(BoxCollider2D) )]
public class PlayerMovement : MonoBehaviour
{
	public event Action<float> playerMoved;
	public event Action playerJumped;
	public event Action playerIsFalling;
	public event Action playerIsGrounded;

    #region Attributes
    private Rigidbody2D _rb;

	[Header("Movement")]
	public float footSpeed;
    [SerializeField]
    private float _airSpeed;

    private bool _facingRight = true;

    [Header("Ground Checking")]
	[SerializeField]
	private Transform _feet;
	[ SerializeField ]
	private float _groundDist;
	[ SerializeField ]
	private LayerMask _groundLayer;
	[SerializeField]
	private int _mercyFrames;

	private float _feetLength;
	private bool _isGrounded;

	[Header("Jump")]
	public float jmpHeight;
	public float jmpReach;
	[Range(0.1f, 0.9f)]
	public float jmpPeak = 0.5f;

	bool _isJumping = false;
	bool _isFalling = false;

	[Header("Variable Height Jump")]
	public bool varHJmp;
	public float vhjGScale;

    #endregion

    private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		var coll = GetComponent<BoxCollider2D>();
		_feetLength = coll.bounds.size.x * Mathf.Abs(transform.localScale.x);
	}
    private void Update()
    {
        if(CheckGrounded()) {
			_isJumping = false;
			_isFalling = false;
		}

		if(_rb.velocity.y <= 0f) Fall();
    }

	#region Movement
    public void Move(float horz)
	{
        Flip(horz);
		horz *= footSpeed;           

		var vel = _rb.velocity;
		vel.x = horz;
		_rb.velocity = vel;
		if(playerMoved != null) playerMoved(horz);

	}
    private void Flip(float horz)
    {
        if( (_facingRight && horz < 0) ||
            (!_facingRight && horz > 0) )
        {
            Vector3 s = transform.localScale;
            s.x *= -1;
            transform.localScale = s;

            _facingRight = !_facingRight;
        }
    }
	#endregion

	#region Jump
	public void Jump()
	{
		if(!_isGrounded) return;

		_isGrounded = false;
		_isJumping = true;
		_isFalling = false;
		if(playerJumped != null) playerJumped();

		var d = jmpReach * jmpPeak;
		var th = d / footSpeed;
		var vy = 2 * jmpHeight / th;
		var g = footSpeed * vy / d;
		var gScale = -g / Physics2D.gravity.y;

		_rb.gravityScale = gScale;

		var vel = _rb.velocity;
		vel.y = vy;
		_rb.velocity = vel;
	}
	private void Fall()
	{
		_isFalling = true;
		_isJumping = false;
		if(playerIsFalling != null) playerIsFalling();

		var d = jmpReach * (1 - jmpPeak);
		var th = d / footSpeed;
		var g = footSpeed * jmpHeight / (th * th);
		var gScale = -g / Physics2D.gravity.y;

		_rb.gravityScale = gScale;
	}
	public void ReleaseJump()
	{
		if(!varHJmp || !_isJumping || _isFalling) return;
		_rb.gravityScale *= vhjGScale;
	}
	#endregion

    #region Ground Checking
    private bool CheckGrounded()
	{
		var hit = Physics2D.BoxCast(_feet.position, new Vector2(_feetLength, _groundDist), 0f, Vector3.forward, Mathf.Infinity, _groundLayer);
		if(!hit) StartCoroutine(MercyFrames(_mercyFrames));
		else if(!_isJumping) {
			if(!_isGrounded && playerIsGrounded != null) playerIsGrounded();
			_isGrounded = true;
		}

		return _isGrounded;
	}

    private IEnumerator MercyFrames(int n)
	{
		if(_isGrounded) {
			for(int i = 0; i < n; i++)
				yield return new WaitForFixedUpdate();
			_isGrounded = false;
		}
	}
    #endregion
}
