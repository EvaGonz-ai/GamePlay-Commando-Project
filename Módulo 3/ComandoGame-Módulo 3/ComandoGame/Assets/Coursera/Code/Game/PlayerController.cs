using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	CharacterController m_CharacterController;
	Animator m_Animator;
	public int m_Score=0;
	public int m_Grenades=2;
	public int m_Lifes=3;
	public GameObject m_AmmoPrefab;
	public float m_ShootRate=1.0f;
	float m_CurrentShootTime=0.0f;
	public AmmoContainer m_AmmoContainer;
	public Transform m_OutputAmmo;
	public GameObject m_GrenadePrefab;
	GameObject m_CurrentGrenade;
	public Transform m_OutputGrenadeTransform;
	public float m_GrenadeSpeedXZ;
	public float m_GrenadeSpeedY;
	int l_ThrowGrenadeIdAnimation=Animator.StringToHash("Base Layer.ThrowGrenade");
	bool m_CanShootGrenade;

	void Start()
	{
		m_CharacterController=GetComponent<CharacterController>();
		m_Animator=GetComponent<Animator>();
		m_CurrentShootTime=0.0f;
		m_CanShootGrenade=true;
		m_CurrentGrenade=null;
	}
	void Update()
	{
		if(m_Animator.GetBool("Dead"))
			return;

		Vector3 l_Direction=Vector3.zero;
		float l_DesiredAngle=transform.rotation.eulerAngles.y;
		bool l_NewAngle=false;
		float l_HorizontalAxis=Input.GetAxis("Horizontal");
		float l_VerticalAxis=Input.GetAxis("Vertical");
		bool l_FirePressed=Input.GetButton("Fire1");
		bool l_ThrowGenadePressed=Input.GetButton("Fire2");

		m_Animator.SetBool("Shoot", l_FirePressed);


		m_CurrentShootTime-=Time.deltaTime;

		if(l_FirePressed && CanShoot()) 
			Shoot(transform.forward);
		if(l_HorizontalAxis>0.5f)
		{
			l_NewAngle=true;
			l_DesiredAngle=90.0f;
			l_Direction.x=1.0f;
		}
		else if(l_HorizontalAxis<-0.5f)
		{
			l_NewAngle=true;
			l_Direction.x=-1.0f;
			l_DesiredAngle=-90.0f;
		}
		if(l_VerticalAxis>0.5f)
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
		else if(l_VerticalAxis<-0.5f)
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
		m_Animator.SetFloat("Movement", l_Direction.magnitude);
		AnimatorStateInfo l_AnimatorStateInfo=m_Animator.GetCurrentAnimatorStateInfo(0);
		if(m_CanShootGrenade && l_AnimatorStateInfo.nameHash!=l_ThrowGrenadeIdAnimation && m_Grenades>0 && l_ThrowGenadePressed)
		{
			m_Animator.SetBool("ThrowGrenade", l_ThrowGenadePressed);
			ThrowGrenade();
		}

		if(m_CurrentGrenade!=null && l_AnimatorStateInfo.nameHash==l_ThrowGrenadeIdAnimation)
		{
			if(l_AnimatorStateInfo.normalizedTime>=0.5f)
			{
				m_CurrentGrenade.rigidbody.isKinematic=false;
				Vector3 l_ForwardXZ=transform.forward;
				l_ForwardXZ.y=0.0f;
				m_CurrentGrenade.rigidbody.velocity=m_GrenadeSpeedXZ*transform.forward+Vector3.up*m_GrenadeSpeedY;
				m_CurrentGrenade.GetComponent<GrenadeController>().enabled=true;
				m_CurrentGrenade=null;
				m_Animator.SetBool("ThrowGrenade", false);
			}
			else
				m_CurrentGrenade.transform.position=m_OutputGrenadeTransform.transform.position;
		}
		if(l_AnimatorStateInfo.nameHash!=l_ThrowGrenadeIdAnimation && m_CurrentGrenade==null && !m_CanShootGrenade)
			m_CanShootGrenade=true;

		m_CharacterController.Move(Physics.gravity*Time.deltaTime);
		transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, l_DesiredAngle, 0.0f), Mathf.Min(1.0f, Time.deltaTime/0.4f));
	}
	public void AddGrenade()
	{
		++m_Grenades;
	}
	public void AddLife()
	{
		++m_Lifes;
		Debug.Log("add life");
	}
	public void AddScore(int Amount)
	{
		m_Score+=Amount;
	}
	public void Kill()
	{
		Debug.Log("Kill player");
		m_Animator.SetBool("Dead", true);
		--m_Lifes;
		Restart();
	}
	void Restart()
	{
	}
	bool CanShoot()
	{
		return m_CurrentShootTime<=0.0f;
	}
	void Shoot(Vector3 Direction)
	{
		m_CurrentShootTime=m_ShootRate;
		m_AmmoContainer.AddAmmo(m_AmmoPrefab, m_OutputAmmo.position, Direction);
	}
	void ThrowGrenade()
	{
		--m_Grenades;
		m_CurrentGrenade=(GameObject)GameObject.Instantiate(m_GrenadePrefab, m_OutputGrenadeTransform.position, Quaternion.identity);
		m_CurrentGrenade.rigidbody.isKinematic=true;
		m_CurrentGrenade.GetComponent<GrenadeController>().enabled=false;
		m_CanShootGrenade=false;
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag=="Explossion")
			Kill();
	}
}
