using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float m_Deadzone = 0.3f;
	public uint m_PlayerNumber = 1;
	public float m_MoveForce = 2500.0f;
	public float m_JumpForce = 10000.0f;
	
	private Rigidbody m_rb;
	private LineRenderer m_lr;

	void Start ()
	{
		m_rb = gameObject.GetComponent<Rigidbody> ();
		m_lr = gameObject.AddComponent<LineRenderer> ();
		m_lr.material = new Material(Shader.Find("Particles/Additve"));
		m_lr.SetColors (Color.yellow, Color.red);
		m_lr.SetWidth (0.1f, 0.1f);
		m_lr.SetVertexCount (2);
	}

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
		bool isOnGround = Physics.Raycast (transform.position, -Vector3.up, colliderHeight + 0.1f);
		bool movingUp = m_rb.velocity.y > Mathf.Epsilon;
		return isOnGround && !movingUp;
	}

	float GetJoystickX()
	{
		float xInputRaw = Input.GetAxis ("L_XAxis_" + m_PlayerNumber);
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
}
