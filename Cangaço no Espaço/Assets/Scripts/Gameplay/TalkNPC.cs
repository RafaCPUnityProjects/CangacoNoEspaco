using UnityEngine;
using System.Collections;
using Fungus;

public class TalkNPC : MonoBehaviour {

	bool nextToNPC = false;
	string npcName;
	public Flowchart questFlowchart;

	void Start(){
		if (questFlowchart == null) {
			questFlowchart = GameObject.FindObjectOfType<Flowchart> ();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("NPC")) {
			nextToNPC = true;
			npcName = other.name;
			SetActiveIconDialog (other.transform, true);
		}


		if (other.CompareTag ("QuestItem")) {
			//if(questFlowchart != null)
			//	questFlowchart.SetBooleanVariable (other.GetComponent<QuestItem>().itemID, true);
			
			GameController.instance.PickBit ();
			Destroy (other.gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("NPC")) {
			nextToNPC = false;
			npcName = "";
			SetActiveIconDialog (other.transform, false);
		}
	}

	void Update(){
		if (questFlowchart == null)
			return;

		if (Input.GetKeyDown (KeyCode.E) && nextToNPC) {
			questFlowchart.SendFungusMessage (npcName);
		}
	}

	void SetActiveIconDialog(Transform parent, bool actived){
		foreach(Transform child in parent){
			if (child.name == "IconOpenDialog") {
				child.gameObject.SetActive (actived);
			}
		}
	}
}
