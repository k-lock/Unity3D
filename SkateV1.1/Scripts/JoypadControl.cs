using UnityEngine;
using System.Collections;

public class JoypadControl : MonoBehaviour {

	
	void Update () {
		
/*		// GamePad Stick Left
		
		if(Input.GetAxis("Stick_Left_X") != 0)  print("S_L [ left <-> right ] :"+ Input.GetAxis("Stick_Left_X"));
		if(Input.GetAxis("Stick_Left_Y") != 0)  print("S_L [  up  <-> down  ] :"+ Input.GetAxis("Stick_Left_Y"));
		
		// GamePad Stick Right
		
		if(Input.GetAxis("Stick_Right_Y") != 0) print("S_R [ up <-> down ] :"+ Input.GetAxis("Stick_Right_Y"));
		
		// GamePad Buttons
		
		if (Input.GetKeyDown (KeyCode.JoystickButton0)) print("JOY 1");
		if (Input.GetKeyDown (KeyCode.JoystickButton1)) print("JOY 2");
		if (Input.GetKeyDown (KeyCode.JoystickButton2)) print("JOY 3");
		if (Input.GetKeyDown (KeyCode.JoystickButton3)) print("JOY 4");
		if (Input.GetKeyDown (KeyCode.JoystickButton4)) print("JOY 5");
		if (Input.GetKeyDown (KeyCode.JoystickButton5)) print("JOY 6");
		if (Input.GetKeyDown (KeyCode.JoystickButton6)) print("JOY 7");
		if (Input.GetKeyDown (KeyCode.JoystickButton7)) print("JOY 8");
*/			
		
		if(Input.GetAxis("Stick_Right_X") != 0) print("S_R [ left <-> right ] :"+ Input.GetAxis("Stick_Right_X"));
		if(Input.GetAxis("Stick_Right_Y") != 0) print("S_R [ up   <-> down  ] :"+ Input.GetAxis("Stick_Right_Y"));
	
		
/*		if(Input.GetKeyDown (KeyCode.JoystickButton7))
		{
			
			print("PRESS : JOY 7 + Stick L" + Input.GetAxis("Stick_Left_X") + " " + Input.GetAxis("Stick_Left_Y"));
			
			if(Input.GetAxis("Stick_Left_X")       == -1 && Input.GetAxis("Stick_Left_Y") ==  0){
				print("LEFT");
			}else if(Input.GetAxis("Stick_Left_X") < -.4 && Input.GetAxis("Stick_Left_Y") < -.4){
				print("LEFT - UP");
			}else if(Input.GetAxis("Stick_Left_X") < -.4 && Input.GetAxis("Stick_Left_Y") >  .4){
				print("LEFT -DOWN");
			}else if(Input.GetAxis("Stick_Left_X")  == 1 && Input.GetAxis("Stick_Left_Y") ==  0){
				print("RIGHT");
			}else if(Input.GetAxis("Stick_Left_X")  > .4 && Input.GetAxis("Stick_Left_Y") < -.4){
				print("RIGHT - UP");
			}else if(Input.GetAxis("Stick_Left_X")  > .4 && Input.GetAxis("Stick_Left_Y") >  .4){
				print("RIGHT -DOWN");
			}else if(Input.GetAxis("Stick_Left_X")  == 0 && Input.GetAxis("Stick_Left_Y") == -1){
				print("UP");
			}else if(Input.GetAxis("Stick_Left_X")  == 0 && Input.GetAxis("Stick_Left_Y") ==  1){
				print("DOWN");
			}
			
			
		}*/
	}
	
	
	
}
