using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossTankController : MonoBehaviour
{
	public enum TState
	{
		IDLE=0,
		INTRO_CINEMATICS,
		LIGHT_ATTACK,
		HEAVY_ATTACK,
		MOVE_TO_POSITION,
		ROTATE,
		DEAD
	}
	public TState m_State=TState.IDLE;
	public TState m_NextState=TState.IDLE;
	public float m_Speed=8.0f;
	public Transform m_TargetIntroCinematicsTransform;
	public PlayerController m_Player;
	public GameObject m_Turret;
	public float m_LightAttackFirstShootTime=4.0f;
	public float m_LightAttackShootTime=4.0f;
	public float m_LightAttackShootRandomTime=2.0f;
	public int m_LightAttackCount=4;
	public GameObject m_LightShootParticles;
	int m_CurrentLightAttack=0;
	float m_NextShootTime;
	AmmoContainer m_AmmoContainer;
	public GameObject m_LightAmmoPrefab;
	public Transform m_OutputLightAmmo;
	public GameObject m_HeavyAmmoPrefab;
	public Transform m_OutputHeavyAmmo;
	public float m_HeavyAttackShootTime=6.0f;
	public float m_HeavyAttackShootRandomTime=3.0f;
	public GameObject m_HeavyShootParticles;
	public List<Transform> m_AttackPositions;
	int m_CurrentAttackPosition=0;
	float m_CurrentTime;
	Quaternion m_StartRotation;
	Quaternion m_DestinyRotation;
	public float m_Life=1.0f;
	public float m_HitTime=0.0f;
	public float m_HitValue=0.34f;
	public GameObject m_EndGameExplossionPrefab;
	public Color m_DeadColor=new Color(133.0f/255.0f, 133.0f/255.0f, 133.0f/255.0f);
	Vector3 m_StartGamePosition;
	Quaternion m_StartGameRotation;
	public float m_SpeedBackHeavyShoot=12.0f;
	public float m_TimeBackHeavyShoot=1.0f;
	float m_CurrentTimeBackHeavyShoot;
	bool m_MoveBackHeavyShoot=false;
	public GameController m_GameController;

	void Start()
	{
		m_AmmoContainer=GetComponent<AmmoContainer>();
		m_HeavyShootParticles.SetActive(false);
		m_LightShootParticles.SetActive(false);
		m_StartGamePosition=transform.position;
		m_StartGameRotation=transform.rotation;
		m_GameController=Camera.main.GetComponent<GameController>();
	}
	void Update()
	{
		m_HitTime-=Time.deltaTime;
		switch(m_State)
		{
			case TState.IDLE:
				FaceTurretToPlayer();
				break;
			case TState.INTRO_CINEMATICS:
				FaceTurretToPlayer();
				if(MoveToPosition(m_TargetIntroCinematicsTransform.position))
					SetLightAttackState();
				break;
			case TState.LIGHT_ATTACK:
				FaceTurretToPlayer();
				m_NextShootTime-=Time.deltaTime;
				if(m_NextShootTime<=0.0f)
				{
					ShootLightAttack();
					++m_CurrentLightAttack;
					if(m_CurrentLightAttack>=m_LightAttackCount)
						//SetHeavyAttackState();
						SetRotateState(TState.MOVE_TO_POSITION, m_AttackPositions[m_CurrentAttackPosition].position);
					else
						m_NextShootTime=m_LightAttackShootTime+m_LightAttackShootRandomTime*Random.value;
				}
				UpdateMoveBackShoot();
				break;
			case TState.HEAVY_ATTACK:
				FaceTurretToPlayer();
				m_NextShootTime-=Time.deltaTime;
				if(m_NextShootTime<=0.0f)
				{
					ShootHeavyAttack();
					SetLightAttackState();
				}
				break;
			case TState.MOVE_TO_POSITION:
				if(MoveToPosition(m_AttackPositions[m_CurrentAttackPosition].position))
				{
					SetRotateState(TState.HEAVY_ATTACK, transform.position+m_AttackPositions[m_CurrentAttackPosition].forward);
					m_CurrentAttackPosition=(m_CurrentAttackPosition+1)%m_AttackPositions.Count;
				}
				FaceTurretToPlayer();
				break;
			case TState.ROTATE:
				m_CurrentTime+=Time.deltaTime;
				float l_RotationWeight=Mathf.Min(1.0f, m_CurrentTime/2.0f);
				Quaternion l_Rotation=Quaternion.Lerp(m_StartRotation, m_DestinyRotation, l_RotationWeight);
				transform.rotation=l_Rotation;
				if(l_RotationWeight==1.0f)
				{
					if(m_NextState==TState.HEAVY_ATTACK)
						SetHeavyAttackState();
					else
						SetMoveToPositionState();
				}
				FaceTurretToPlayer();
				break;
			case TState.DEAD:
				break;
		}
		rigidbody.MovePosition(transform.position);
	}
	void SetLightAttackState()
	{
		m_LightShootParticles.SetActive(true);
		m_LightShootParticles.particleSystem.Stop();
		m_LightShootParticles.particleSystem.Play();
		m_CurrentLightAttack=0;
		m_State=TState.LIGHT_ATTACK;
		m_NextShootTime=m_LightAttackFirstShootTime;
	}
	public void SetIntroCinematicsState()
	{
		m_State=TState.INTRO_CINEMATICS;
	}
	public void FaceTurretToPlayer()
	{
		Vector3 l_Direction=m_Player.transform.position-m_Turret.transform.position;
		l_Direction.y=0.0f;
		l_Direction.Normalize();
		m_Turret.transform.LookAt(m_Turret.transform.position+l_Direction*5.0f);
	}
	void ShootLightAttack()
	{
		Vector3 l_AmmoDirection=(m_Player.transform.position+Vector3.up*1.6f)-m_OutputLightAmmo.position;
		l_AmmoDirection.Normalize();
		m_AmmoContainer.AddAmmo(m_LightAmmoPrefab, m_OutputLightAmmo.position, l_AmmoDirection);
	}
	void UpdateMoveBackShoot()
	{
		if(m_MoveBackHeavyShoot)
		{
			m_CurrentTimeBackHeavyShoot+=Time.deltaTime;
			transform.position=transform.position-m_Turret.transform.forward*m_SpeedBackHeavyShoot*Time.deltaTime*(1.0f-Mathf.Min(1.0f, m_CurrentTimeBackHeavyShoot/m_TimeBackHeavyShoot));
			if(m_CurrentTimeBackHeavyShoot>=m_TimeBackHeavyShoot)
				m_MoveBackHeavyShoot=false;
		}
	}
	void ShootHeavyAttack()
	{
		m_MoveBackHeavyShoot=true;
		m_CurrentTimeBackHeavyShoot=0.0f;

		Vector3 l_FeetPosition=m_Player.transform.position;
		l_FeetPosition.y=0.0f;
		Vector3 l_AmmoDirection=l_FeetPosition-m_OutputHeavyAmmo.position;
		l_AmmoDirection.Normalize();
		m_AmmoContainer.AddAmmo(m_HeavyAmmoPrefab, m_OutputHeavyAmmo.position, l_AmmoDirection);
	}
	void SetHeavyAttackState()
	{
		m_HeavyShootParticles.SetActive(true);
		m_HeavyShootParticles.particleSystem.Stop();
		m_HeavyShootParticles.particleSystem.Play();
		m_State=TState.HEAVY_ATTACK;
		m_NextShootTime=m_HeavyAttackShootTime+m_HeavyAttackShootRandomTime*Random.value;
	}
	void SetMoveToPositionState()
	{
		m_State=TState.MOVE_TO_POSITION;
	}
	bool MoveToPosition(Vector3 Position)
	{
		bool l_Done=false;
		float l_Movement=Time.deltaTime*m_Speed;
		Vector3 l_Direction=Position-transform.position;
		float l_Distance=l_Direction.magnitude;
		if(l_Distance==0.0f)
			l_Done=true;
		else
		{
			l_Direction/=l_Distance;
			if(l_Distance<l_Movement)
			{
				l_Movement=l_Distance;
				l_Done=true;
			}
			transform.position=transform.position+l_Direction*l_Movement;
		}
		return l_Done;
	}
	void SetRotateState(TState NextState, Vector3 LookAt)
	{
		m_CurrentTime=0.0f;
		m_State=TState.ROTATE;
		m_NextState=NextState;
		m_StartRotation=transform.rotation;
		Vector3 l_Direction=LookAt-transform.position;
		l_Direction.y=0.0f;
		l_Direction.Normalize();
		m_DestinyRotation.SetLookRotation(l_Direction);
	}
	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="Player" && (m_State==TState.ROTATE || m_State==TState.MOVE_TO_POSITION))
		{
			m_Player.Kill();
			SetIdleState();
		}
		else if(other.gameObject.tag=="Explossion" && CanBeHit())
			Hit();
	}
	void SetIdleState()
	{
		m_State=TState.IDLE;
		m_MoveBackHeavyShoot=false;
	}
	bool CanBeHit()
	{
		return m_HitTime<=0.0f;
	}
	void Hit()
	{
		m_HitTime=5.0f;
		m_Life=Mathf.Max(0.0f, m_Life-m_HitValue);
		if(m_Life<=0.0f)
			SetDeadState();
	}
	void SetDeadMaterial(Renderer _Renderer)
	{
		foreach(Material l_Material in _Renderer.materials)
			l_Material.color=m_DeadColor;
	}
	void SetDeadState()
	{
		m_State=TState.DEAD;
		GameObject.Instantiate(m_EndGameExplossionPrefab, transform.position, Quaternion.identity);
		Renderer[] l_Renderers=GetComponentsInChildren<Renderer>();
		foreach(Renderer l_Renderer in l_Renderers)
			SetDeadMaterial(l_Renderer);
		//m_Player.SetLevelAccomplished();
	}
	public void RestartGame()
	{
		m_CurrentAttackPosition=0;
		SetIdleState();
		m_Life=1.0f;
		m_HeavyShootParticles.SetActive(false);
		m_LightShootParticles.SetActive(false);
		transform.position=m_StartGamePosition;
		transform.rotation=m_StartGameRotation;
	}
}
