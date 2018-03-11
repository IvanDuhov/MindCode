using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Character : MonoBehaviour 
{
	bool OnBlueTile; // shows if the character is on blue tile
	bool walking = false; // animations trigger

	public string level;
	public byte optimal = 3;

	public Vector3 startingPosition;
	public float startingRotation = 90;


	private Vector3 offset; // Offset to 0 as starting position. !!!Always the starting tile should be (0,0,0)!!! Equals to the opposite of the starting position.

	public bool jumping;

	private Animator anim; // animator
	private float jumpTimer;
	private bool suspend = false;

	public Image[] orders = new Image[13]; // An array with the order's images
	public Image[] procedure1Orders = new Image[9]; // An array with procedure1 orders
	public Image[] procedure2Orders = new Image[9]; // An Array with procedure 2 orders 

	public Image mainPanel;
	public Image Proc1Panel;
	public Image Proc2Panel;

	public Color32 activeColour;
	public Color32 offColour;

	private byte numOfOrders = 0; // Tracks the num of orders
	private byte numOfProcedure1Orders = 0; // Tracks the num of procedure orders
	private byte numOfProcedure2Orders = 0; 

	byte count = 0; // the engligthen blue tiles in the map

	public bool procedure1Selected = false;
	public bool procedure2Selected = false;

	public byte aob; // The amount of blue tiles in the map

	private Tile[] tiles; // An array with all the tiles
	private byte numOfTiles; // Number of tile

	public Material yellow;
	public Material blue;

	public Sprite arrow;
	public Sprite currentArrow;
	public Sprite wrongArrow;
	public Sprite bulb;
	public Sprite currentBulb;
	public Sprite leftSprite;
	public Sprite currentLeftSprite;
	public Sprite rightSprite;
	public Sprite currentRightSprite;
	public Sprite Jump1;
	public Sprite currentJump;
	public Sprite wrongJump;
	public Sprite none;

	public Sprite Proc1Sprite;
	public Sprite currentProc1;
	public Sprite Proc2Sprite;
	public Sprite currentProc2;

	public Button startButton;

	public Sprite startSprite;
	public Sprite stopSprite;
	public Sprite backwardSprite;
	public Sprite forwardBTNSprite;

	public Transform forwardButton; // The button taking you to the next level

	public Image tryagain; // If not successful its giving a tip!
	public Sprite tryagainbg;

	public Image forwardbg;
	public Sprite bgSprite;

	public Image background;
	public Button forwardBTN;
	public Image stars;
	public Image log;

	public Sprite zerostar;
	public Sprite onestar;
	public Sprite twostar;
	public Sprite threestar;
	public Sprite forwardOff;

	public Sprite bgtip22; // 3 star
	public Sprite tip22;

	public Sprite bgtip23; // 2 star
	public Sprite tip23;

	public Sprite bgtip24; // 1 star
	public Sprite tip24;

	public Sprite bgtip25; // 0 star
	public Sprite tip25;

	private string lang;

	void Start()
	{
		anim = this.gameObject.GetComponent<Animator> ();

		offset = -startingPosition;

		tiles = (Tile[]) GameObject.FindObjectsOfType (typeof(Tile)); // Adding all the tiles

		foreach (var tile in tiles) // Counting tiles
		{
			numOfTiles += 1;
		}

		lang = PlayerPrefs.GetString ("English");
		if (lang == "false") {
			tryagain.sprite = tryagainbg;
			forwardbg.sprite = bgSprite;
		}

	}

	void Update()
	{
		// Animation "magic"
		if (walking) 
		{
			anim.SetInteger("Speed", 1);
		}

		// Changing the color of the panels 
		ColorChangerMain ();
		ColorChangerProc1 ();
		ColorChangerProc2 ();
	}

	#region Tile Functions

	bool IsThereATile(float xpos, float height, float zpos)
	{
		foreach (var tile in tiles) 
		{
			if (Round( tile.xpos,2) == Round(xpos,2) && Round(tile.height,2)
				== Round(height,2) && Round(tile.zpos,2) == Round(zpos,2)) 
			{
				return true;  
			}
		}
		return false;
	}
	 
	bool CheckTile(Vector3 pos)
	{
		return IsThereATile (pos.x, pos.y, pos.z);
	}

	Tile GetTile(Vector3 pos)
	{
		foreach (var tile in tiles) 
		{
			if (Round( tile.xpos,2) == Round(pos.x,2) && Round(tile.height,2)
				== Round(pos.y,2) && Round(tile.zpos,2) == Round(pos.z,2)) 
			{
				return tile;
			}
		}
		return null;
	}

    float Round(float value, int digits)
	{
		float mult = Mathf.Pow(10.0f, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}

	#endregion

	#region Order Buttons

	public void AheadButton()
	{
		if (procedure1Selected) 
		{
			if (numOfProcedure1Orders < 8) 
			{
				procedure1Orders [numOfProcedure1Orders].color =  Color.white;
				procedure1Orders [numOfProcedure1Orders].sprite = arrow;
				numOfProcedure1Orders += 1;
			}
		}
		else if (procedure2Selected) 
		{
			if (numOfProcedure2Orders < 8) 
			{
				procedure2Orders [numOfProcedure2Orders].color =  Color.white;
				procedure2Orders [numOfProcedure2Orders].sprite = arrow;
				numOfProcedure2Orders += 1;
			}
		}
		else if (numOfOrders < 12) 
		{
			orders [numOfOrders].color =  Color.white;
			orders [numOfOrders].sprite = arrow;
			numOfOrders += 1;
		}
	}

	public void Light()
	{
		if (procedure1Selected) 
		{
			if (numOfProcedure1Orders < 8) 
			{
				procedure1Orders [numOfProcedure1Orders].color =  Color.white;
				procedure1Orders [numOfProcedure1Orders].sprite = bulb;
				numOfProcedure1Orders += 1;
			}
		}
		else if (procedure2Selected) 
		{
			if (numOfProcedure2Orders < 8) 
			{
				procedure2Orders [numOfProcedure2Orders].color =  Color.white;
				procedure2Orders [numOfProcedure2Orders].sprite = bulb;
				numOfProcedure2Orders += 1;
			}
		}
		else if (numOfOrders < 12) {
			orders [numOfOrders].color =  Color.white;
			orders [numOfOrders].sprite = bulb;
			numOfOrders += 1;
		}
	}

	public void Left()
	{
		if (procedure1Selected) 
		{
			if (numOfProcedure1Orders < 8) 
			{
				procedure1Orders [numOfProcedure1Orders].color =  Color.white;
				procedure1Orders [numOfProcedure1Orders].sprite = leftSprite;
				numOfProcedure1Orders += 1;
			}
		}
		else if (procedure2Selected) 
		{
			if (numOfProcedure2Orders < 8) 
			{
				procedure2Orders [numOfProcedure2Orders].color =  Color.white;
				procedure2Orders [numOfProcedure2Orders].sprite = leftSprite;
				numOfProcedure2Orders += 1;
			}
		}
		else if (numOfOrders < 12) 
		{
			orders [numOfOrders].color =  Color.white;
			orders [numOfOrders].sprite = leftSprite;
			numOfOrders += 1;
		}
	}

	public void Right()
	{
		if (procedure1Selected) 
		{
			if (numOfProcedure1Orders < 8) 
			{
				procedure1Orders [numOfProcedure1Orders].color =  Color.white;
				procedure1Orders [numOfProcedure1Orders].sprite = rightSprite;
				numOfProcedure1Orders += 1;
			}
		}
		else if (procedure2Selected) 
		{
			if (numOfProcedure2Orders < 8) 
			{
				procedure2Orders [numOfProcedure2Orders].color =  Color.white;
				procedure2Orders [numOfProcedure2Orders].sprite = rightSprite;
				numOfProcedure2Orders += 1;
			}
		}
		else if (numOfOrders < 12) 
		{
			orders [numOfOrders].color =  Color.white;
			orders [numOfOrders].sprite = rightSprite;
			numOfOrders += 1;
		}
	}

	public void Jump()
	{
		if (procedure1Selected) 
		{
			if (numOfProcedure1Orders < 8) 
			{
				procedure1Orders [numOfProcedure1Orders].color =  Color.white;
				procedure1Orders [numOfProcedure1Orders].sprite = Jump1;
				numOfProcedure1Orders += 1;
			}
		}
		else if (procedure2Selected) 
		{
			if (numOfProcedure2Orders < 8) 
			{
				procedure2Orders [numOfProcedure2Orders].color =  Color.white;
				procedure2Orders [numOfProcedure2Orders].sprite = Jump1;
				numOfProcedure2Orders += 1;
			}
		}
		else if (numOfOrders < 12) 
		{
			orders [numOfOrders].color =  Color.white;
			orders [numOfOrders].sprite = Jump1;
			numOfOrders += 1;
		}
	}

	public void Proc1()
	{
		if (procedure1Selected) 
		{
			if (numOfProcedure1Orders < 8) 
			{
				procedure1Orders [numOfProcedure1Orders].color =  Color.white;
				procedure1Orders [numOfProcedure1Orders].sprite = Proc1Sprite;
				numOfProcedure1Orders += 1;
			}
		}
		else if (procedure2Selected) 
		{
			if (numOfProcedure2Orders < 8) 
			{
				procedure2Orders [numOfProcedure2Orders].color =  Color.white;
				procedure2Orders [numOfProcedure2Orders].sprite = Proc1Sprite;
				numOfProcedure2Orders += 1;
			}
		}
		else if (numOfOrders < 12) 
		{
			orders [numOfOrders].color =  Color.white;
			orders [numOfOrders].sprite = Proc1Sprite;
			numOfOrders += 1;
		}
	}

	public void Proc2()
	{
		if (procedure1Selected) 
		{
			if (numOfProcedure1Orders < 8) 
			{
				procedure1Orders [numOfProcedure1Orders].color =  Color.white;
				procedure1Orders [numOfProcedure1Orders].sprite = Proc2Sprite;
				numOfProcedure1Orders += 1;
			}
		}
		else if (procedure2Selected) 
		{
			if (numOfProcedure2Orders < 8) 
			{
				procedure2Orders [numOfProcedure2Orders].color =  Color.white;
				procedure2Orders [numOfProcedure2Orders].sprite = Proc2Sprite;
				numOfProcedure2Orders += 1;
			}
		}
		else if (numOfOrders < 12) 
		{
			orders [numOfOrders].color =  Color.white;
			orders [numOfOrders].sprite = Proc2Sprite;
			numOfOrders += 1;
		}
	}

	#endregion

	public void Backward()
	{
		forwardButton.gameObject.SetActive (false);
		count = 0;
		for (int i = 0; i < numOfTiles; i++) 
		{
			if (tiles [i].colorGrid != null)
			if ((tiles [i].colour == "Blue") || (tiles [i].colour == "lighted"))
			{
				tiles [i].colorGrid.material = blue;
				tiles [i].colour = "Blue";
			}
		}

		this.transform.position = startingPosition;
		this.transform.rotation = Quaternion.Euler (0, startingRotation, 0);;
		startButton.image.sprite = startSprite;

		if (background != null) {
			background.gameObject.SetActive (false);
			tryagain.gameObject.SetActive (false);
			forwardBTN.enabled = true;
			forwardBTN.image.sprite = forwardBTNSprite;
		}
	}

	public void GameStart()
	{
		tryagain.gameObject.SetActive (false);
		if (startButton.image.sprite == startSprite) 
		{
			forwardButton.gameObject.SetActive (false);
			StartCoroutine (Ticker ());
		} 
		else if (startButton.image.sprite == backwardSprite) 
		{
			Backward ();
		}
		else if (startButton.image.sprite == stopSprite) 
		{
			count = 0;
			for (int i = 0; i < numOfTiles; i++) 
			{
				if (tiles [i].colorGrid != null)
				if ((tiles [i].colour == "Blue") || (tiles [i].colour == "lighted")) 
				{
					tiles [i].colorGrid.material = blue;
					tiles [i].colour = "Blue";
				}
			}
			suspend = true;
		}
	}
		
	void Enlightening(Image[] orderS, int currentIteration) // Enlightens the tile
	{
		if (count < aob) 
		{
			Tile currentTile = GetTile (transform.position + offset);

			if ((currentTile.colorGrid != null) && (currentTile.colour == "Blue"))
			{
				currentTile.colorGrid.material = yellow;
				currentTile.colour = "lighted";
				Debug.Log ("Englight the tile!");
				count++;
			}
		} 
	}  // Enlightens the tile

	void TurningLeft() // Turning Left
	{
		if (this.transform.rotation.eulerAngles.y == 0) {
			this.transform.rotation = Quaternion.Euler (0, 270, 0);
		} else if (this.transform.eulerAngles.y == 270) {
			this.transform.rotation = Quaternion.Euler (0, 180, 0);
		} else if (this.transform.eulerAngles.y == 180) {
			this.transform.rotation = Quaternion.Euler (0, 90, 0);
		} else if (this.transform.eulerAngles.y == 90) {
			this.transform.rotation = Quaternion.Euler (0, 0, 0);
		}
	}  // Turning Left

	void TurningRight() // Turrning Right
	{ 
		if (this.transform.eulerAngles.y == 0) {
			this.transform.rotation = Quaternion.Euler (0, 90, 0);
		} else if (this.transform.eulerAngles.y == 90) {
			this.transform.rotation = Quaternion.Euler (0, 180, 0);
		} else if (this.transform.eulerAngles.y == 180) {
			this.transform.rotation = Quaternion.Euler (0, 270, 0);
		} else if (this.transform.eulerAngles.y == 270) {
			this.transform.rotation = Quaternion.Euler (0, 0, 0);
		}
	}  // Turning Right


	public IEnumerator Ticker()
	{
		startButton.image.sprite = stopSprite;
		bool trigger = true;


		bool procesing = false;

		for (int i = 0; i < numOfOrders; i++) {

			if (orders [i].sprite == Jump1) {  // Jumping
				
				bool proceeded = false;
				orders [i].sprite = currentJump;
				jumping = true;


				if ((CheckTile (transform.position + offset + new Vector3 (0, 0.38f, 0) + transform.forward)) &&
				    (!CheckTile (transform.position + offset + new Vector3 (0, 0.76f, 0) + transform.forward))) 
				{
					var startPosition = transform.position;
					var endPosition = transform.position + transform.forward + Vector3.up * 0.38f;
					float startTime = Time.time;

					while ((transform.position - endPosition).sqrMagnitude > 0) {
						transform.position = Vector3.Lerp (startPosition, endPosition, Mathf.Clamp01 (Time.time - startTime));
						yield return null;
					}
					transform.position = endPosition;
					proceeded = true;

				} 
				else if ((CheckTile (transform.position + offset + transform.forward + new Vector3 (0, -0.38f, 0))) &&
				           (!CheckTile (transform.position + offset + transform.forward)))				{
					var startPosition = transform.position;
					var endPosition = transform.position + transform.forward + Vector3.down * 0.38f;
					float startTime = Time.time;

					while ((transform.position - endPosition).sqrMagnitude > 0) {
						transform.position = Vector3.Lerp (startPosition, endPosition, Mathf.Clamp01 (Time.time - startTime));
						yield return null;
					}
					transform.position = endPosition;
					proceeded = true;
				}

				if (!proceeded) {
					orders [i].sprite = wrongJump;
					yield return new WaitForSeconds (0.6f);
					jumping = false;
					yield return new WaitForSeconds (0.7f);
				} else {
					jumping = false;
					yield return new WaitForSeconds (0.65f);
				}

				orders [i].sprite = Jump1;

			}  // Jumping

			if (orders [i].sprite == arrow) { // Going ahead

				walking = true;
				anim.SetInteger ("Speed", 1);
				orders [i].sprite = currentArrow;

				if (CheckTile (transform.position + offset + transform.forward + new Vector3 (0, 0.38f, 0))) {
					Debug.Log ("OK");
				} else if (CheckTile (transform.position + offset + transform.forward)) {
					procesing = true;

					var startPosition = transform.position;
					var endPosition = transform.position + transform.forward;
					float startTime = Time.time;

					while ((transform.position - endPosition).sqrMagnitude > 0) {
						transform.position = Vector3.Lerp (startPosition, endPosition, (Time.time - startTime));
						yield return null;
					}

					if (Round (transform.forward.z, 2) == 0) {
						transform.position = new Vector3 (endPosition.x, endPosition.y, startPosition.z);
					} else {
						transform.position = endPosition;
					}
				}

				if (!procesing) {
					orders [i].sprite = wrongArrow;
					yield return new WaitForSeconds (0.8f);
					walking = false;
					orders [i].sprite = arrow;
				} else {
					orders [i].sprite = arrow;
					walking = false;
				}

				anim.SetInteger ("Speed", 0);
				procesing = false;

			}  // Going Ahead

			if (orders [i].sprite == bulb) { // Enlightening the tile
				
				orders [i].sprite = currentBulb;
				Enlightening (orders, i);
				yield return new WaitForSeconds (0.5f);
				orders [i].sprite = bulb;

			}  // Enlightening the tile

			if (orders [i].sprite == leftSprite) { // Turning Left
				
				orders [i].sprite = currentLeftSprite;

				TurningLeft ();
				yield return new WaitForSeconds (0.35f);

				orders [i].sprite = leftSprite;

			}  // Turning Left

			if (orders [i].sprite == rightSprite) { // Turning Right
				
				orders [i].sprite = currentRightSprite;

				TurningRight ();
				yield return new WaitForSeconds (0.35f);

				orders [i].sprite = rightSprite;
			}  // Turning Right

			if (orders [i].sprite == Proc1Sprite) { // Procedure 1
				
				orders [i].sprite = currentProc1;

				for (int k = 0; k < numOfProcedure1Orders; k++) { 
					if (procedure1Orders [k].sprite == Jump1) { // Jumping
						bool proceeded = false;
						procedure1Orders [k].sprite = currentJump;
						jumping = true;


						if ((CheckTile (transform.position + offset + new Vector3 (0, 0.38f, 0) + transform.forward)) &&
						    (!CheckTile (transform.position + offset + new Vector3 (0, 0.76f, 0) + transform.forward))) {
							var startPosition = transform.position;
							var endPosition = transform.position + transform.forward + Vector3.up * 0.38f;
							float startTime = Time.time;

							while ((transform.position - endPosition).sqrMagnitude > 0) {
								transform.position = Vector3.Lerp (startPosition, endPosition, Mathf.Clamp01 (Time.time - startTime));
								yield return null;
							}
							transform.position = endPosition;
							proceeded = true;
						} else if ((CheckTile (transform.position + offset + transform.forward + new Vector3 (0, -0.38f, 0))) &&
						           (!CheckTile (transform.position + offset + transform.forward))) {
							var startPosition = transform.position;
							var endPosition = transform.position + transform.forward + Vector3.down * 0.38f;
							float startTime = Time.time;

							while ((transform.position - endPosition).sqrMagnitude > 0) {
								transform.position = Vector3.Lerp (startPosition, endPosition, Mathf.Clamp01 (Time.time - startTime));
								yield return null;
							}
							transform.position = endPosition;
							proceeded = true;
						}

						if (!proceeded) {
							procedure1Orders [k].sprite = wrongJump;
							yield return new WaitForSeconds (0.6f);
							jumping = false;
							yield return new WaitForSeconds (0.7f);
						} else {
							jumping = false;
							yield return new WaitForSeconds (0.65f);
						}

						procedure1Orders [k].sprite = Jump1;
					} // Jumping

					if (procedure1Orders [k].sprite == arrow) { // Going ahead

						walking = true;
						anim.SetInteger ("Speed", 1);
						procedure1Orders [k].sprite = currentArrow;

						if (CheckTile (transform.position + offset + transform.forward + new Vector3 (0, 0.38f, 0))) {
							Debug.Log ("OK");
						} else if (CheckTile (transform.position + offset + transform.forward)) {
							procesing = true;

							var startPosition = transform.position;
							var endPosition = transform.position + transform.forward;
							float startTime = Time.time;

							while ((transform.position - endPosition).sqrMagnitude > 0) {
								transform.position = Vector3.Lerp (startPosition, endPosition, (Time.time - startTime));
								yield return null;
							}

							if (Round (transform.forward.z, 2) == 0) {
								transform.position = new Vector3 (endPosition.x, endPosition.y, startPosition.z);
							} else {
								transform.position = endPosition;
							}
						}

						if (!procesing) {
							procedure1Orders [k].sprite = wrongArrow;
							yield return new WaitForSeconds (0.8f);
							walking = false;
							procedure1Orders [k].sprite = arrow;
						} else {
							procedure1Orders [k].sprite = arrow;
							walking = false;
						}

						anim.SetInteger ("Speed", 0);
						procesing = false;

					}  // Going Ahead

					if (procedure1Orders [k].sprite == bulb) { // Enlightening the tile
						
						procedure1Orders [k].sprite = currentBulb;

						Enlightening (procedure1Orders, k);
						yield return new WaitForSeconds (0.5f);

						procedure1Orders [k].sprite = bulb;

					} // Enlightening the tile

					if (procedure1Orders [k].sprite == leftSprite) { // Turning Left
						
						procedure1Orders [k].sprite = currentLeftSprite;

						TurningLeft ();
						yield return new WaitForSeconds (0.35f);

						procedure1Orders [k].sprite = leftSprite;

					} // Turning Left

					if (procedure1Orders [k].sprite == rightSprite) { // Turning Right
						
						procedure1Orders [k].sprite = currentRightSprite;

						TurningRight ();
						yield return new WaitForSeconds (0.35f);

						procedure1Orders [k].sprite = rightSprite;

					} // Turning Right

					if (procedure1Orders [k].sprite == Proc2Sprite) { // GOTO procedure 2

						procedure1Orders [k].sprite = currentProc2;
						procedure1Orders [k].sprite = Proc2Sprite;

					} // Turning Right
				}

				orders [i].sprite = Proc1Sprite;

			}  // Procedure 1
						
//			if (orders [i].sprite == Proc2Sprite) { // Procedure 2
//				orders [i].sprite = currentProc2;
//
//				for (int k = 0; k < numOfProcedure2Orders; k++) { 
//					
//					if (procedure2Orders [k].sprite == Jump1) { // Jumping
//						bool proceeded = false;
//						procedure2Orders [k].sprite = currentJump;
//						jumping = true;
//
//
//						if ((CheckTile (transform.position + offset + new Vector3 (0, 0.38f, 0) + transform.forward)) &&
//						    (!CheckTile (transform.position + offset + new Vector3 (0, 0.76f, 0) + transform.forward))) {
//							var startPosition = transform.position;
//							var endPosition = transform.position + transform.forward + Vector3.up * 0.38f;
//							float startTime = Time.time;
//
//							while ((transform.position - endPosition).sqrMagnitude > 0) {
//								transform.position = Vector3.Lerp (startPosition, endPosition, Mathf.Clamp01 (Time.time - startTime));
//								yield return null;
//							}
//							transform.position = endPosition;
//							proceeded = true;
//						} else if ((CheckTile (transform.position + offset + transform.forward + new Vector3 (0, -0.38f, 0))) &&
//						           (!CheckTile (transform.position + offset + transform.forward))) {
//							var startPosition = transform.position;
//							var endPosition = transform.position + transform.forward + Vector3.down * 0.38f;
//							float startTime = Time.time;
//
//							while ((transform.position - endPosition).sqrMagnitude > 0) {
//								transform.position = Vector3.Lerp (startPosition, endPosition, Mathf.Clamp01 (Time.time - startTime));
//								yield return null;
//							}
//							transform.position = endPosition;
//							proceeded = true;
//						}
//
//						if (!proceeded) {
//							yield return new WaitForSeconds (0.6f);
//							jumping = false;
//							yield return new WaitForSeconds (0.7f);
//						} else {
//							jumping = false;
//							yield return new WaitForSeconds (0.65f);
//						}
//
//						procedure2Orders [k].sprite = Jump1;
//					} // Jumping
//
//					if (procedure2Orders [k].sprite == arrow) { // Going ahead
//
//						walking = true;
//						anim.SetInteger ("Speed", 1);
//						procedure2Orders [k].sprite = currentArrow;
//
//						if (CheckTile (transform.position + offset + transform.forward + new Vector3 (0, 0.38f, 0))) {
//							Debug.Log ("OK");
//						} else if (CheckTile (transform.position + offset + transform.forward)) {
//							procesing = true;
//
//							var startPosition = transform.position;
//							var endPosition = transform.position + transform.forward;
//							float startTime = Time.time;
//
//							while ((transform.position - endPosition).sqrMagnitude > 0) {
//								transform.position = Vector3.Lerp (startPosition, endPosition, (Time.time - startTime));
//								yield return null;
//							}
//
//							if (Round (transform.forward.z, 2) == 0) {
//								transform.position = new Vector3 (endPosition.x, endPosition.y, startPosition.z);
//							} else {
//								transform.position = endPosition;
//							}
//						}
//
//						if (!procesing) {
//							yield return new WaitForSeconds (0.8f);
//							walking = false;
//							procedure2Orders [k].sprite = arrow;
//						} else {
//							procedure2Orders [k].sprite = arrow;
//							walking = false;
//						}
//
//						anim.SetInteger ("Speed", 0);
//						procesing = false;
//
//					}  // Going Ahead
//
//					if (procedure2Orders [k].sprite == bulb) { // Enlightening the tile
//
//						procedure2Orders [k].sprite = currentBulb;
//
//						Enlightening (procedure1Orders, k);
//						yield return new WaitForSeconds (0.5f);
//
//						procedure2Orders [k].sprite = bulb;
//
//					} // Enlightening the tile
//
//					if (procedure2Orders [k].sprite == leftSprite) { // Turning Left
//
//						procedure2Orders [k].sprite = currentLeftSprite;
//
//						TurningLeft ();
//						yield return new WaitForSeconds (0.35f);
//
//						procedure2Orders [k].sprite = leftSprite;
//
//					} // Turning Left
//
//					if (procedure2Orders [k].sprite == rightSprite) { // Turning Right
//
//						procedure2Orders [k].sprite = currentRightSprite;
//
//						TurningRight ();
//						yield return new WaitForSeconds (0.35f);
//
//						procedure2Orders [k].sprite = rightSprite;
//
//					} // Turning Right
//				}
//
//				orders [i].sprite = Proc2Sprite;
//
//			} // Procedure 2

			yield return new WaitForSeconds (0.01f);

			if (count == aob) 
			{
				if (background != null) 
				{
					background.gameObject.SetActive (true);

					bool passed3 = true;
					bool passed2 = true;
					bool passed1 = true;
					string temp = PlayerPrefs.GetString (level + "S");

					if (temp != "3")
						passed3 = false;
					if (temp != "2")
						passed2 = false;
					if (temp != "1")
						passed1 = false;
					
					if (numOfOrders == optimal) 
					{
						stars.sprite = threestar;

						if (lang == "false")
							log.sprite = bgtip22;
						else
							log.sprite = tip22;

						if (!passed3)
							PlayerPrefs.SetString (level + "S", "3");
					} 
					else if ((numOfOrders + 1 == optimal) || (numOfOrders - 1 == optimal)) 
					{
						stars.sprite = twostar;

						if (lang == "false")
							log.sprite = bgtip23;
						else
							log.sprite = tip23;

						if ((!passed3) && (!passed2))
							PlayerPrefs.SetString (level + "S", "2");
					} 
					else 
					{
						stars.sprite = onestar;

						if (lang == "false")
							log.sprite = bgtip24;
						else
							log.sprite = tip24;

						if (((!passed3) && (!passed2) && (!passed1)))
							PlayerPrefs.SetString (level + "S", "1");
					}

					forwardBTN.enabled = true;
					forwardBTN.image.sprite = forwardBTNSprite;
					PlayerPrefs.SetString (level.ToString(), "true");
				}
				else 
				{
					PlayerPrefs.SetString (level.ToString(), "true");
					forwardButton.gameObject.SetActive (true);
					Debug.Log ("You win!");
				}

				trigger = false;
				break;
			}

			if (suspend) 
			{
				suspend = false;
				trigger = false;
				this.transform.position = startingPosition;
				this.transform.rotation = Quaternion.Euler (0, startingRotation, 0);
				startButton.image.sprite = startSprite;
				break;
			}

		}

		if (trigger) 
		{
			if (background != null) {
				background.gameObject.SetActive (true);
				stars.sprite = zerostar;
				forwardBTN.enabled = false;
				forwardBTN.image.sprite = forwardOff;

				if (lang == "false")
					log.sprite = bgtip25;
				else
					log.sprite = tip25;

			} else 
			{	
				startButton.image.sprite = backwardSprite;
				if (forwardButton.gameObject.activeSelf != true)
					tryagain.gameObject.SetActive (true);
				
			}

		}
	}

	void ColorChangerMain()
	{
		for (int i = 0; i < 13; i++) 
		{
			if (orders[i].sprite == null) 
			{
				orders [i].color = new Color32 (255, 255, 255, 0);
			} 
		}
	}

	void ColorChangerProc1()
	{
		if (Proc1Panel != null) {
			for (int i = 0; i < 8; i++) {
				if (procedure1Orders != null) {
					if (procedure1Orders [i].sprite == null) {
						procedure1Orders [i].color = new Color32 (255, 255, 255, 0);
					} 
				}
			}
		}
	}

	void ColorChangerProc2()
	{
		if (Proc2Panel != null) {
			for (int i = 0; i < 8; i++) {
				if (procedure2Orders != null) {
					if (procedure2Orders [i].sprite == null) {
						procedure2Orders [i].color = new Color32 (255, 255, 255, 0);
					} 
				}
			}
		}
	}

	#region Remove Main Orders
	public void RemoveOrder0()
	{
		byte currentOrder = 0;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
					if (Proc2Panel != null) 
					{
						Proc2Panel.color = offColour;
						procedure2Selected = false;
					}
				}
			}
		}
	}

	public void RemoveOrder1()
	{
		byte currentOrder = 1;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder2()
	{
		byte currentOrder = 2;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder3()
	{
		byte currentOrder = 3;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder4()
	{
		byte currentOrder = 4;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder5()
	{
		byte currentOrder = 5;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}
		
	public void RemoveOrder6()
	{
		byte currentOrder = 6;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder7()
	{
		byte currentOrder = 7;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder8()
	{
		byte currentOrder = 8;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder9()
	{
		byte currentOrder = 9;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder10()
	{
		byte currentOrder = 10;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}

	public void RemoveOrder11()
	{
		byte currentOrder = 11;
		bool trigger = false;
		if (numOfOrders > currentOrder) {
			for (int k = currentOrder; k < numOfOrders; k++) {
				orders [k].sprite = orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfOrders -= 1;
				if (Proc1Panel != null) 
				{
					procedure1Selected = false;
					Proc1Panel.color =  offColour;
					mainPanel.color = activeColour;
				}
				if (Proc2Panel != null) 
				{
					Proc2Panel.color = offColour;
					procedure2Selected = false;
				}
			}
		}
	}
	#endregion

	#region Remove Procedure1 Orders

	public void RemoveProcedure1Order0()
	{
		byte currentOrder = 0;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) 
			{
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order1()
	{
		byte currentOrder = 1;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order2()
	{
		byte currentOrder = 2;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order3()
	{
		byte currentOrder = 3;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order4()
	{
		byte currentOrder = 4;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order5()
	{
		byte currentOrder = 5;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order6()
	{
		byte currentOrder = 6;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order7()
	{
		byte currentOrder = 7;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure1Order8()
	{
		byte currentOrder = 8;
		bool trigger = false;
		if (numOfProcedure1Orders > currentOrder) {
			for (int k = currentOrder; k < numOfProcedure1Orders; k++) {
				procedure1Orders [k].sprite = procedure1Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) {
				numOfProcedure1Orders -= 1;
				procedure1Selected = true;
				mainPanel.color = offColour;
				Proc1Panel.color = activeColour;
			}
		}
	}

	#endregion

	#region Remove Procedure2 Orders

	public void RemoveProcedure2Order0()
	{
		byte currentOrder = 0;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order1()
	{
		byte currentOrder = 1;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order2()
	{
		byte currentOrder = 2;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order3()
	{
		byte currentOrder = 3;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order4()
	{
		byte currentOrder = 4;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order5()
	{
		byte currentOrder = 5;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order6()
	{
		byte currentOrder = 6;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order7()
	{
		byte currentOrder = 7;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	public void RemoveProcedure2Order8()
	{
		byte currentOrder = 8;
		bool trigger = false;
		if (numOfProcedure2Orders > currentOrder) 
		{
			for (int k = currentOrder; k < numOfProcedure2Orders; k++) 
			{
				procedure2Orders [k].sprite = procedure2Orders [k + 1].sprite;
				trigger	= true;
			}
			if (trigger) 
			{
				numOfProcedure2Orders -= 1;
				procedure2Selected = true;
				procedure1Selected = false;
				mainPanel.color = offColour;
				Proc1Panel.color = offColour;
				Proc2Panel.color = activeColour;
			}
		}
	}

	#endregion


}
