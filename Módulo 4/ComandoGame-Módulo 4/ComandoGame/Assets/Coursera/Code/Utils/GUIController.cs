using UnityEngine;
using System.Collections;

namespace CourseraUtils
{
	public class GUIController
	{
		public static Rect GetRectangleGUI (float x, float y, float Width, float Height)
		{
			return new Rect (Screen.width * x, Screen.height * y, Screen.width * Width, Screen.height * Height);
		}
		public static Rect GetRectangleGUICentered (float x, float y, float Width, float Height)
		{
			Rect l_Rect = GetRectangleGUI (x, y, Width, Height);
			l_Rect.x -= l_Rect.width * 0.5f;
			l_Rect.y -= l_Rect.height * 0.5f;
			return l_Rect;
		}
	}
}
