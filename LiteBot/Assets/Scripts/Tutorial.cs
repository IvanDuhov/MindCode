using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour 
{
	private Image[] tips = new Image[20];
	private Sprite[] bgtips = new Sprite[20];

	public byte clicker = 1;
	public Image background;

	public Image tip1; 
	public Image tip2; 
	public Image tip3; 
	public Image tip4; 
	public Image tip5; 
	public Image tip6; 

	public Sprite bgtip1; 
	public Sprite bgtip2; 
	public Sprite bgtip3; 
	public Sprite bgtip4; 
	public Sprite bgtip5; 
	public Sprite bgtip6; 

	public Image mt1; 
	public Image mt2; 
	public Image mt3; 

	public Text  bgmt1;
	public Text  bgmt2;
	public Text  bgmt3;

	public Text  engmt1;
	public Text  engmt2;
	public Text  engmt3;

	public Transform stars;

	private string lang;
	private bool bg;

	private bool activated = false;

	void Update()
	{
		if ((stars != null) && (stars.gameObject.activeSelf == true)) 
		{
			StartCoroutine (Timer (0f));
		}	
	}

	void Start()
	{
		tips [1] = tip1;
		tips [2] = tip2;
		tips [3] = tip3;
		tips [4] = tip4;
		tips [5] = tip5;
		tips [6] = tip6;

		bgtips [1] = bgtip1;
		bgtips [2] = bgtip2;
		bgtips [3] = bgtip3;
		bgtips [4] = bgtip4;
		bgtips [5] = bgtip5;
		bgtips [6] = bgtip6;

		lang = PlayerPrefs.GetString ("English");
		if (lang == "false")
			bg = true;
		
		if (bg) {
			tip1.sprite = bgtip1;
		}

	}

	public void Level1() // Also working for every level with 5 tips!
	{
		if (clicker == 1) {
			StartCoroutine (Timer (0.0f));
			background.gameObject.SetActive (true);
		}
		if (bg) {
			tip1.sprite = bgtip1;
			tip2.sprite = bgtip2;
			tip3.sprite = bgtip3;
			tip4.sprite = bgtip4;
			tip5.sprite = bgtip5;

			engmt1.gameObject.SetActive (false);
			engmt2.gameObject.SetActive (false);
			engmt3.gameObject.SetActive (false);
		} else {
			bgmt1.gameObject.SetActive (false);
			bgmt2.gameObject.SetActive (false);
			bgmt3.gameObject.SetActive (false);
		}

		if (clicker < 5) {
			tips [clicker].gameObject.SetActive (false);
			tips [clicker + 1].gameObject.SetActive (true);
			clicker++;
		} else {
			tips [clicker].gameObject.SetActive (false);
			background.gameObject.SetActive (false);
			mt1.gameObject.SetActive (true);
			mt2.gameObject.SetActive (true);
			mt3.gameObject.SetActive (true);
			StartCoroutine (Timer (7.5f));
			clicker = 1;
		}
	}

	private IEnumerator Timer(float sec)
	{
		yield return new WaitForSeconds (sec);
		mt1.gameObject.SetActive (false);
		mt2.gameObject.SetActive (false);
		if (mt3 != null)
			mt3.gameObject.SetActive (false);
	}

	public void Level2() // Also working for every level with 4 tips!
	{
		if (clicker == 1) {
			background.gameObject.SetActive (true);
		}
		if (bg) 
		{
			tip1.sprite = bgtip1;
			tip2.sprite = bgtip2;
			tip3.sprite = bgtip3;
			tip4.sprite = bgtip4;
		}

		if (clicker < 4) {
			tips [clicker].gameObject.SetActive (false);
			tips [clicker + 1].gameObject.SetActive (true);
			clicker++;
		} else {
			tips [clicker].gameObject.SetActive (false);
			background.gameObject.SetActive (false);
			clicker = 1;
		}
	}

	public void LevelP1()
	{
		if (clicker == 1) {
			StartCoroutine (Timer (0.0f));
			background.gameObject.SetActive (true);
		}
		if (bg) 
		{
			tip1.sprite = bgtip1;
			tip2.sprite = bgtip2;
			tip3.sprite = bgtip3;
			tip4.sprite = bgtip4;
			tip5.sprite = bgtip5;

			engmt1.gameObject.SetActive (false);
			engmt2.gameObject.SetActive (false);
		} else {
			bgmt1.gameObject.SetActive (false);
			bgmt2.gameObject.SetActive (false);
		}
		if (clicker < 5) {
			tips [clicker].gameObject.SetActive (false);
			tips [clicker + 1].gameObject.SetActive (true);
			clicker++;
		} else {
			tips [clicker].gameObject.SetActive (false);
			background.gameObject.SetActive (false);
			mt1.gameObject.SetActive (true);
			mt2.gameObject.SetActive (true);
			StartCoroutine (Timer (7.5f));
			clicker = 1;
		}
	}

	public void RegularHelp()
	{
		if (bg) 
		{
			tip1.sprite = bgtip1;
			tip2.sprite = bgtip2;
		}

		if ((clicker == 1) && !(activated)) {
			background.gameObject.SetActive (true);
			activated = true;
			tip1.gameObject.SetActive (true);
			return;
		}

		if (clicker < 2) {
			tips [clicker].gameObject.SetActive (false);
			tips [clicker + 1].gameObject.SetActive (true);
			clicker++;
		} else {
			tips [clicker].gameObject.SetActive (false);
			background.gameObject.SetActive (false);
			clicker = 1;
			activated = false;
		}
	}


}
