using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float m_Speed;
	CharacterController m_CharacterController;

	void Start()
	{
		m_CharacterController=GetComponent<CharacterController>();
	}
	void Update()
	{
		Vector3 l_Direction=Vector3.zero;
		float l_DesiredAngle=transform.rotation.eulerAngles.y;
		bool l_NewAngle=false;

		if(Input.GetKey(KeyCode.RightArrow))
		{
			l_NewAngle=true;
			l_DesiredAngle=90.0f;
			l_Direction.x=1.0f;
		}
		else if(Input.GetKey(KeyCode.LeftArrow))
		{
			l_NewAngle=true;
			l_Direction.x=-1.0f;
			l_DesiredAngle=-90.0f;
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			if(l_NewAngle)
			{
				if(l_DesiredAngle==90.0f)
					l_DesiredAngle=45.0f;
				if(l_DesiredAngle==-90.0f)
					l_DesiredAngle=-45.0f;
			}
			else
				l_DesiredAngle=0.0f;
			l_Direction.z=1.0f;
		}
		else if(Input.GetKey(KeyCode.DownArrow))
		{
			if(l_NewAngle)
			{
				if(l_DesiredAngle==-90.0f)
					l_DesiredAngle=225.0f;
				if(l_DesiredAngle==90.0f)
					l_DesiredAngle=135.0f;
			}
			else
				l_DesiredAngle=180.0f;
			l_Direction.z=-1.0f;
		}

		l_Direction.Normalize();

		Vector3 l_Movement=l_Direction*m_Speed*Time.deltaTime;
		m_CharacterController.Move(l_Movement);
		transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, l_DesiredAngle, 0.0f), Mathf.Min(1.0f, Time.deltaTime/0.4f));
	}
}
