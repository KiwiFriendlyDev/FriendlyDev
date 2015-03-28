using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float m_Deadzone = 0.3f;
	public uint m_PlayerNumber = 1;
	public float m_MoveForce = 2500.0f;
	public float m_JumpForce = 10000.0f;
	public Vector3 m_GrabDir = Vector3.zero;
	
	private Rigidbody m_rb;
	private LineRenderer m_lr;

	void Start ()
	{
		m_rb = gameObject.GetComponent<Rigidbody> ();
		m_lr = gameObject.AddComponent<LineRenderer> ();
		m_lr.material = new Material(Shader.Find("Particles/Additve"));
		m_lr.SetColors (Color.yellow, Color.blue);
		m_lr.SetWidth (0.1f, 0.1f);
		m_lr.SetVertexCount (2);
	}

	//-------------------------------------//
	// Update()
	//
	//  Update is called every frame
	//-------------------------------------//
	void Update()
	{
		GetGrabDirection( ref m_GrabDir );

		// DEBUG DRAW
	    m_lr.SetPosition(0, this.transform.position );
		m_lr.SetPosition(1, this.transform.position + m_GrabDir * 10.0f );
	}

	//-------------------------------------//
	// FixedUpdate()
	//
	// This function is called every fixed framerate frame
	// FixedUpdate should be used instead of Update when dealing with Rigidbody. 
	// For example when adding a force to a rigidbody, you have to apply the force every fixed frame inside FixedUpdate instead of every frame inside Update.
	//-------------------------------------//
	void FixedUpdate ()
	{
		Vector3 movementForce = Vector3.zero;
		float joystickX = GetJoystickX ();
		if (System.Math.Abs (joystickX) > Mathf.Epsilon) 
		{
			movementForce.x = joystickX * m_MoveForce * Time.fixedDeltaTime;
		}

		if (Input.GetButton ("A_" + m_PlayerNumber) && IsGrounded ()) 
		{
			m_rb.velocity = new Vector3 (m_rb.velocity.x, 0.0f, m_rb.velocity.z);
			movementForce.y = m_JumpForce;
		}

		m_rb.AddForce(movementForce);
	}
	
	bool IsGrounded()
	{
		float colliderHeight = gameObject.GetComponent<Collider> ().bounds.extents.y;
		bool isOnGround = Physics.Raycast (transform.position, -Vector3.up, colliderHeight + 0.15f);
		bool movingUp = m_rb.velocity.y > 0.1f;
		return isOnGround && !movingUp;
	}

	float GetJoystickX()
	{
		float xInputRaw = Input.GetAxisRaw ("L_XAxis_" + m_PlayerNumber);
		if(System.Math.Abs(xInputRaw) <= m_Deadzone)
		{
			return 0.0f;
		}
		else if(xInputRaw < 0.0f)
		{
			return (xInputRaw + m_Deadzone) / (1.0f - m_Deadzone);
		}
		else
		{
			return (xInputRaw - m_Deadzone) / (1.0f - m_Deadzone);
		}
	}

	// GetGrabDirection(ref Vector2 outVector )
	//
	// Compute a 3D vectore based on the right analog stick
	// and put it in outVector
	void GetGrabDirection(ref Vector3 outVector )
	{
		float xInputRaw = Input.GetAxisRaw ("R_XAxis_" + m_PlayerNumber);
		float yInputRaw = Input.GetAxisRaw ("R_YAxis_" + m_PlayerNumber);
		if(System.Math.Abs( xInputRaw ) + System.Math.Abs( yInputRaw ) > m_Deadzone )
		{
			outVector.x = xInputRaw;
			outVector.y = -yInputRaw;
			outVector.z = 0.0f;
			outVector.Normalize();
		}
	}
}
