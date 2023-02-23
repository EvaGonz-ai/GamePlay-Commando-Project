using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoContainer : MonoBehaviour
{
	public GameObject m_DestroyObjects;
	List<Ammo> m_Ammo;

	void Start()
	{
		m_Ammo=new List<Ammo>();
	}
	public void AddAmmo(GameObject AmmoPrefab, Vector3 Position, Vector3 Direction)
	{
		GameObject l_AmmoGO=(GameObject)GameObject.Instantiate(AmmoPrefab, Position, Quaternion.identity);
		l_AmmoGO.transform.parent=m_DestroyObjects.transform;
		Ammo l_Ammo=l_AmmoGO.GetComponent<Ammo>();
		l_Ammo.Shoot(Direction);
		m_Ammo.Add(l_Ammo);
	}
	public void Restart()
	{
		foreach(Ammo l_Ammo in m_Ammo)
		{
			if(l_Ammo!=null)
				GameObject.Destroy(l_Ammo.gameObject);
		}
		m_Ammo=new List<Ammo>();
	}
}
