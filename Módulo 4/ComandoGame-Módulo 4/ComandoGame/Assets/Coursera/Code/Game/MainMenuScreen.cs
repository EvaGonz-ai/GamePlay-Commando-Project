using UnityEngine;
using System.Collections;
using CourseraUtils;

public class MainMenuScreen : MonoBehaviour
{
	public Texture2D m_LogoTexture;
	public GUISkin m_Skin;
	public AudioClip m_FXSound;

	void Start()
	{
	}
	void Update()
	{
		if(Input.GetButtonUp("Fire1"))
		{
			audio.PlayOneShot(m_FXSound);
			Application.LoadLevel("GameScene");
		}
	}
	void OnGUI()
	{
		GUI.skin=m_Skin;
		GUI.DrawTexture(GUIController.GetRectangleGUI(0.0f, 0.0f, 0.18984375f, 0.4472222222222222f), m_LogoTexture);
		GUI.Label(GUIController.GetRectangleGUI(0.2f, 0.8f, 0.6f, 0.1f), "- Press button to start game -", "MainMenuLabelStyle");
	}
}
