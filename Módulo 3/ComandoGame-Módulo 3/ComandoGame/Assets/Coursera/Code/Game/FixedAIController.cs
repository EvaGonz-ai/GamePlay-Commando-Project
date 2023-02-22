using UnityEngine;
using System.Collections;

public class FixedAIController : CAIController
{
	PlayerController m_PlayerController;
	public GameObject m_AmmoPrefab;
	AmmoContainer m_AmmoContainer;
	public float m_BaseTimeToShoot=5;
	public float m_RandomTimeToShoot=3;
	public Transform m_AmmoOutputTransform;
	float m_CurrentTime;
	Animator m_Animator;
	CharacterController m_CharacterController;

	void Start()
	{
		m_CharacterController=GetComponent<CharacterController>();
		m_AmmoContainer=GetComponent<AmmoContainer>();
		m_PlayerController=Camera.main.GetComponent<CameraController>().m_PlayerTransform.GetComponent<PlayerController>();
		m_Animator=GetComponent<Animator>();
		CalcNextShootTime();
	}
	void Update()
	{
		if(m_Animator.GetBool("Dead"))
			return;
		if(!IsOnScreen())
			return;

		m_CurrentTime-=Time.deltaTime;
		if(m_CurrentTime<=0.0f)
			Shoot();

		Vector3 l_Direction=m_PlayerController.transform.position-transform.position;
		l_Direction.y=0.0f;
		l_Direction.Normalize();
		transform.forward=l_Direction;
	}
	void CalcNextShootTime()
	{
		m_CurrentTime=m_BaseTimeToShoot+Random.value*m_RandomTimeToShoot;
	}
	void Shoot()
	{
		m_AmmoContainer.AddAmmo(m_AmmoPrefab, m_AmmoOutputTransform.position, transform.forward);
		CalcNextShootTime();
	}
	public override void Kill()
	{
		m_Animator.SetBool("Dead", true);
		m_CharacterController.enabled=false;
		m_PlayerController.AddScore(100);
	}
	bool IsOnScreen()
	{
		Vector3 l_Position=Camera.main.WorldToViewportPoint(transform.position);
		return l_Position.y>=0.0f && l_Position.y<=1.0f;
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag=="Explossion")
			Kill();
	}
}
