using UnityEngine;
using System.Collections;

public class RobotTestScriptFree : MonoBehaviour {

	private Animator anim;
	private float jumpTimer = 0;

	private Character cha;

	void Start () {
	
		anim = this.gameObject.GetComponent<Animator> ();
		cha = this.gameObject.GetComponent<Character>();

	}
	
	// Update is called once per frame
	void Update () {
	
		//Controls the Input for running animations
		// 1: walk
		//2: Run
		//3: Jump
			
		if(Input.GetKey("2")) anim.SetInteger("Speed", 2);
			else if(Input.GetKey("1")) anim.SetInteger("Speed", 1);
				else anim.SetInteger("Speed", 0);

		if (Input.GetKey("3")) {

			jumpTimer = 0.8f;
			anim.SetBool ("Jumping", true);

		}

		if (cha.jumping) 
		{
			jumpTimer = 0.8f;
			anim.SetBool ("Jumping", true);
		}

		if (jumpTimer > 0.79) {
			jumpTimer -= Time.deltaTime;
		}
		else if (anim.GetBool ("Jumping") == true) 
		{
			anim.SetBool ("Jumping", false);
			anim.SetInteger ("Speed", 0);
		}

	}
}
