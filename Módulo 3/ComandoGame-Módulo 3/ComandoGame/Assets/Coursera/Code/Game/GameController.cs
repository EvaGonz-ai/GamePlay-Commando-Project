using UnityEngine;
using System.Collections;
using CourseraUtils;

public class GameController : MonoBehaviour
{
	public Texture2D m_GrenadeTexture;
	public Texture2D m_LifeTexture;
	public PlayerController m_PlayerController;
	public GUISkin m_Skin;
	float m_CurrenTime;

	void Start()
	{
		m_CurrenTime=0.0f;
	}
	void Update()
	{
		m_CurrenTime+=Time.deltaTime;
	}
	void OnGUI()
	{
		GUI.skin=m_Skin;
		GUI.Label(GUIController.GetRectangleGUI(0.01f, 0.01f, 0.2f, 0.1f), "SCORE:", "HeaderStyle");
		GUI.Label(GUIController.GetRectangleGUI(0.5f, 0.01f, 0.2f, 0.1f), "HISCORE:", "HeaderStyle");
		GUI.Label(GUIController.GetRectangleGUI(0.85f, 0.01f, 0.2f, 0.1f), "TIME:", "HeaderStyle");
		GUI.Label(GUIController.GetRectangleGUI(0.01f, 0.08f, 0.2f, 0.1f), m_PlayerController.m_Score.ToString(), "ValueStyle");
		GUI.Label(GUIController.GetRectangleGUI(0.5f, 0.08f, 0.2f, 0.1f), "20000", "ValueStyle");
		GUI.Label(GUIController.GetRectangleGUI(0.85f, 0.08f, 0.2f, 0.1f), m_CurrenTime.ToString("f0"), "ValueStyle");
		for(int i=0;i<m_PlayerController.m_Grenades; ++i)
			GUI.DrawTexture(GUIController.GetRectangleGUI(0.9f-i*0.04f, 0.85f, 0.02421875f, 0.0777777777777778f), m_GrenadeTexture);
		for(int i=0;i<m_PlayerController.m_Lifes; ++i)
			GUI.DrawTexture(GUIController.GetRectangleGUI(0.05f+i*0.042f, 0.85f, 0.04140625f, 0.0666666666666667f), m_LifeTexture);
	}
}
