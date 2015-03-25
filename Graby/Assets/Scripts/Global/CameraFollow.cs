using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public GameObject m_ObjectToFollow;
	
	// Update is called once per frame
	void Update () 
	{
		if(m_ObjectToFollow)
		{
			Transform camera = GetComponent<Transform>();
			Vector3 currentPos = camera.position;
			currentPos.x = m_ObjectToFollow.transform.position.x;
			currentPos.y = m_ObjectToFollow.transform.position.y;
			camera.position = currentPos;
		}
	}
}
