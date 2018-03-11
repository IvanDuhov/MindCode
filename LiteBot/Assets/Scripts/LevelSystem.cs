using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour 
{
	public string BorP;

	string b1;
	string b2;
	string b3;
	string b4;
	string b5;
	string b6;
	string b7;
	string b8;

	string p1;
	string p2;
	string p3;

	string b1s;
	string b2s;
	string b3s;
	string b4s;
	string b5s;
	string b6s;
	string b7s;
	string b8s;

	string P1s;
	string P2s;
	string P3s;

	public Image S1;
	public Image S2;
	public Image S3;
	public Image S4;
	public Image S5;
	public Image S6;
	public Image S7;
	public Image S8;

	public Image P1;
	public Image P2;
	public Image P3;

	public Image l2;
	public Image l3;
	public Image l4;
	public Image l5;
	public Image l6;
	public Image l7;
	public Image l8;

	public Button btn2;
	public Button btn3;
	public Button btn4;
	public Button btn5;
	public Button btn6;
	public Button btn7;
	public Button btn8;

	public Button btns1;

	public Sprite s2;
	public Sprite s3;
	public Sprite s4;
	public Sprite s5;
	public Sprite s6;
	public Sprite s7;
	public Sprite s8;

	public Sprite star0;
	public Sprite star1;
	public Sprite star2;
	public Sprite star3;

	public Image basic;
	public Image procedures;

	public Sprite basicbg;
	public Sprite procedurelockBg;
	public Sprite procedureBG;
	public Sprite procedurenolock;

	private string lang;
	private bool bg;

	void Start()
	{
		lang = PlayerPrefs.GetString ("English");
		if (lang == "false")
			bg = true;

		b1 = PlayerPrefs.GetString ("B1");
		b2 = PlayerPrefs.GetString ("B2");
		b3 = PlayerPrefs.GetString ("B3");
		b4 = PlayerPrefs.GetString ("B4");
		b5 = PlayerPrefs.GetString ("B5");
		b6 = PlayerPrefs.GetString ("B6");
		b7 = PlayerPrefs.GetString ("B7");
		b8 = PlayerPrefs.GetString ("B8");

		p1 = PlayerPrefs.GetString ("P1");
		p2 = PlayerPrefs.GetString ("P2");
		p3 = PlayerPrefs.GetString ("P3");

		b1s = PlayerPrefs.GetString ("B1S");
		b2s = PlayerPrefs.GetString ("B2S");
		b3s = PlayerPrefs.GetString ("B3S");
		b4s = PlayerPrefs.GetString ("B4S");
		b5s = PlayerPrefs.GetString ("B5S");
		b6s = PlayerPrefs.GetString ("B6S");
		b7s = PlayerPrefs.GetString ("B7S");
		b8s = PlayerPrefs.GetString ("B8S");

		P1s = PlayerPrefs.GetString ("P1S");
		P2s = PlayerPrefs.GetString ("P2S");
		P3s = PlayerPrefs.GetString ("P3S");

		if (BorP == "B")
		{
			#region Basic
			if (b1 == "true") {
				l2.sprite = s2;
				btn2.enabled = true;
			} else
				btn2.enabled = false;

			if (b2 == "true") {
				l3.sprite = s3;
				btn3.enabled = true;
			} else
				btn3.enabled = false;

			if (b3 == "true") {
				l4.sprite = s4;
				btn4.enabled = true;
			} else
				btn4.enabled = false;

			if (b4 == "true") {
				l5.sprite = s5;
				btn5.enabled = true;
			} else
				btn5.enabled = false;

			if (b5 == "true") {
				l6.sprite = s6;
				btn6.enabled = true;
			} else
				btn6.enabled = false;

			if (b6 == "true") {
				l7.sprite = s7;
				btn7.enabled = true;
			} else
				btn7.enabled = false;

			if (b7 == "true") {
				l8.sprite = s8;
				btn8.enabled = true;
			} else
				btn8.enabled = false;
			
			// 	  Matching the stars  //
			// ---------------------- //

			switch (b1s) 
			{
			case "3": S1.sprite = star3;
				break;
			case "2": S1.sprite = star2;
				break;
			case "1": S1.sprite = star1;
				break;
			default: S1.sprite = star0;
				break;
			}
			switch (b2s) 
			{
			case "3": S2.sprite = star3;
				break;
			case "2": S2.sprite = star2;
				break;
			case "1": S2.sprite = star1;
				break;
			default: S2.sprite = star0;
				break;
			}
			switch (b3s) 
			{
			case "3": S3.sprite = star3;
				break;
			case "2": S3.sprite = star2;
				break;
			case "1": S3.sprite = star1;
				break;
			default: S3.sprite = star0;
				break;
			}
			switch (b4s) 
			{
			case "3": S4.sprite = star3;
				break;
			case "2": S4.sprite = star2;
				break;
			case "1": S4.sprite = star1;
				break;
			default: S4.sprite = star0;
				break;
			}
			switch (b5s) 
			{
			case "3": S5.sprite = star3;
				break;
			case "2": S5.sprite = star2;
				break;
			case "1": S5.sprite = star1;
				break;
			default: S5.sprite = star0;
				break;
			}
			switch (b6s) 
			{
			case "3": S6.sprite = star3;
				break;
			case "2": S6.sprite = star2;
				break;
			case "1": S6.sprite = star1;
				break;
			default: S6.sprite = star0;
				break;
			}
			switch (b7s) 
			{
			case "3": S7.sprite = star3;
				break;
			case "2": S7.sprite = star2;
				break;
			case "1": S7.sprite = star1;
				break;
			default: S7.sprite = star0;
				break;
			}
			switch (b8s) 
			{
			case "3": S8.sprite = star3;
				break;
			case "2": S8.sprite = star2;
				break;
			case "1": S8.sprite = star1;
				break;
			default: S8.sprite = star0;
				break;
			}



			#endregion
		} 
		else if (BorP == "S")
		{
			if (b8 == "true") 
			{
				if (bg == true) {
					basic.sprite = basicbg;
					procedures.sprite = procedureBG;
					btns1.enabled = true;
				} else 
				{
					btns1.enabled = true;
					procedures.sprite = procedurenolock;
				}
			} 
			else 
			{
				if (bg == true) {
					basic.sprite = basicbg;
					btns1.enabled = false;
					procedures.sprite = procedurelockBg;
				} else 
					btns1.enabled = false;

				
			}

		}
		else 
		{
			#region Procedures
			if (p1 == "true") {
				l2.sprite = s2;
				btn2.enabled = true;
			} else 
				btn2.enabled = false;

			if (p2 == "true") {
				l3.sprite = s3;
				btn2.enabled = true;
			} else 
				btn3.enabled = false;

			switch (P1s) 
			{
			case "3": P1.sprite = star3;
				break;
			case "2": P1.sprite = star2;
				break;
			case "1": P1.sprite = star1;
				break;
			default: P1.sprite = star0;
				break;
			}
			switch (P2s) 
			{
			case "3": P2.sprite = star3;
				break;
			case "2": P2.sprite = star2;
				break;
			case "1": P2.sprite = star1;
				break;
			default: P2.sprite = star0;
				break;
			}
			switch (P3s) 
			{
			case "3": P3.sprite = star3;
				break;
			case "2": P3.sprite = star2;
				break;
			case "1": P3.sprite = star1;
				break;
			default: P3.sprite = star0;
				break;
			}

			#endregion
		}

	
	}

}
