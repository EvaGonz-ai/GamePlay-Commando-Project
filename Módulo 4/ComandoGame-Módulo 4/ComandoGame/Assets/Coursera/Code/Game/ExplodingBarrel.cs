using UnityEngine;
using System.Collections;

public class ExplodingBarrel : MonoBehaviour
{
	enum TState
	{
		IDLE,
		EXPLODING
	}
	TState m_State;
	float m_CurrentTime;
	public GameObject m_ExplosionPrefab;
	public GameController m_GameController;

	void Start()
	{
		m_GameController=Camera.main.GetComponent<GameController>();
		SetIdleState();
	}
	void Update()
	{
		switch(m_State)
		{
			case TState.IDLE:
				break;
			case TState.EXPLODING:
				m_CurrentTime+=Time.deltaTime;
				if(m_CurrentTime>6.0f)
					SetIdleState();
				break;
		}
	}
	public void Explode()
	{
		if(m_State!=TState.IDLE)
			return;
		m_CurrentTime=0.0f;
		m_State=TState.EXPLODING;
		GameObject l_Explosion=(GameObject)GameObject.Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
		GameObject.Destroy(l_Explosion, 8.0f);
	}
	void SetIdleState()
	{
		m_State=TState.IDLE;
	}
	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="Explossion" && m_State==TState.IDLE)
			Explode();
	}
}
