using UnityEngine;
using System.Collections;

public class Explossion : MonoBehaviour
{
	SphereCollider m_SphereCollider;
	float m_CurrentTime;

	void Start()
	{
		m_SphereCollider=GetComponent<SphereCollider>();
		m_SphereCollider.radius=0.0f;
		m_CurrentTime=0.0f;
	}
	void Update()
	{
		m_CurrentTime+=Time.deltaTime;
		float l_Pct=Mathf.Min(1.0f, m_CurrentTime/2.0f);
		m_SphereCollider.radius=l_Pct*4.0f;
	}
}
