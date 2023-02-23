using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform m_PlayerTransform;
	Vector3 m_OffsetCamera;
	public float m_XDivider=1.0f;
	float m_MinZCamera;
	Vector3 m_StartPosition;
	float m_StartMinZCamera;
	bool m_BossState=false;
	public Transform m_BossTransformCamera;

	void Start()
	{
		m_OffsetCamera=transform.position-m_PlayerTransform.position;
		m_MinZCamera=transform.position.z;
		m_StartPosition=transform.position;
		m_StartMinZCamera=m_MinZCamera;
	}
	void Update()
	{
		if(m_BossState)
		{
			transform.position=m_BossTransformCamera.position;
			transform.rotation=m_BossTransformCamera.rotation;
		}
		else
		{
			Vector3 l_CameraPosition=m_PlayerTransform.position+m_OffsetCamera;
			l_CameraPosition.x/=m_XDivider;
			l_CameraPosition.z=Mathf.Max(m_MinZCamera, l_CameraPosition.z);
			m_MinZCamera=l_CameraPosition.z;
			transform.position=l_CameraPosition;
		}
	}
	public void RestartGame()
	{
		transform.position=m_StartPosition;
		m_MinZCamera=m_StartMinZCamera;
	}
	public void SetBossState()
	{
		m_BossState=true;
	}
}
