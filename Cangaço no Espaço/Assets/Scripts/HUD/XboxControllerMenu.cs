using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XboxControllerMenu : MonoBehaviour {

	//http://answers.unity3d.com/questions/543403/taking-steps-to-all-gamepad-control.html

	public GameObject[] menu;
	public int selectionNum = 0;
	GameObject selection;

	float lastVerticalAxis = 0;

	void Start(){
		selection = menu[selectionNum];
		ChangeSelectButton (0);
	}

	void Update(){
		
		if(GetControllerDown()){
			ChangeSelectButton (1);
		}
		if(GetControllerUp()){
			ChangeSelectButton (-1);
		}
		if (Input.GetButtonDown ("Fire1")) {
			selection.GetComponent<Button>().onClick.Invoke();
		}

		lastVerticalAxis = Input.GetAxisRaw ("Vertical");
	}

	void ChangeSelectButton(int nextSelectionIndex){
		
		//check to make sure the selection is not outside of the array
		if(InsideArray(nextSelectionIndex)){

			//change color back
			selection.GetComponent<Animator>().SetTrigger("Normal");

			//change selectionNum
			selectionNum = selectionNum + nextSelectionIndex;

			selection = menu[selectionNum];
			//change color of selection
			selection.GetComponent<Animator>().SetTrigger("Highlighted");
		}
	}

	bool InsideArray(int i){
		if (i > 0) {
			return (selectionNum + i < menu.Length);
		} else if(i < 0){
			return (selectionNum + i >= 0);
		}else{
			return true;
		}
	}

	bool GetControllerDown(){
		if (Input.GetAxisRaw ("Vertical") < -0.2f && lastVerticalAxis >= -0.2f) {
			return true;
		} else {
			return false;
		}
	}

	bool GetControllerUp(){
		if (Input.GetAxisRaw ("Vertical") > 0.2f && lastVerticalAxis <= 0.2f) {
			return true;
		} else {
			return false;
		}
	}
}
