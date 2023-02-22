using UnityEngine;
using System.Collections;

public class GrenadeController : MonoBehaviour
{
	public GameObject m_ExplossionPrefab;
	float m_CurrentTime=0.0f;
	public float m_TimeToExplode=0.5f;

	void Start()
	{
	}
	void Update()
	{
		m_CurrentTime+=Time.deltaTime;
	}
	void OnCollisionEnter(Collision Other)
	{
		if(m_CurrentTime>=m_TimeToExplode)
		{
			GameObject l_Explossion=(GameObject)GameObject.Instantiate(m_ExplossionPrefab, transform.position, Quaternion.identity);
			GameObject.Destroy(this.gameObject);
			GameObject.Destroy(l_Explossion, 3.0f);
		}
	}
}
