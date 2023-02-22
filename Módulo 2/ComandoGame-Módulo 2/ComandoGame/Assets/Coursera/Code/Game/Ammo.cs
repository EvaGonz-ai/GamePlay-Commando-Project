using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour
{
	public enum TAmmoType 
	{
		PLAYER=0,
		ENEMY
	}
	public TAmmoType m_Type;
	public float m_Speed=10.0f;
	public float m_LifeTime=3.0f;
	public float m_CurrenTime=0.0f;

	void Start()
	{
		m_CurrenTime=0.0f;
	}
	void Update()
	{
		m_CurrenTime+=Time.deltaTime;
		if(m_CurrenTime>=m_LifeTime)
			GameObject.Destroy(this.gameObject);
	}
	public void Shoot(Vector3 Direction)
	{
		rigidbody.velocity=Direction*m_Speed;
	}
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag=="Player" && m_Type==TAmmoType.ENEMY)
			other.gameObject.GetComponent<PlayerController>().Kill();
	}
}
