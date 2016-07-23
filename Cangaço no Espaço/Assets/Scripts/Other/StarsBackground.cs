using UnityEngine;
using System.Collections;

public class StarsBackground : MonoBehaviour {

	ParticleSystem ps;

	public Color startColor;
	float bright = 0; //0: no green apple near, 1:apple near

	Transform nim;

	float T_CHECK = 1.3f;
	float timeToCheck;

	GreenApple[] greenApples;

    Transform appleTransform;
    Quaternion lookRotation;
    public float rotationSpeed = 1f;
    bool firstSetDir;
    
    void Awake(){
		GameplayEvents.nimOnStage.Subscribe (NimOnStage);
	}

	void Start () {
		ps = GetComponent<ParticleSystem> ();
		ps.startColor = startColor;
	}

	void NimOnStage(NimHealth nimHealth){
		nim = nimHealth.transform;
		greenApples = Transform.FindObjectsOfType<GreenApple>();
	}

	void SetShortestDistance(){
		float shortestDistance = 100f;
		float appleDistance;
		foreach(GreenApple apple in greenApples){
			if (apple != null) {
				appleDistance = Vector3.Distance (apple.transform.position, nim.position);
				if (appleDistance < shortestDistance)
                {
                    shortestDistance = appleDistance;
                    appleTransform = apple.transform;
                }
					
			}
		}

		SetStarBright(shortestDistance);
	}


    void SetStarDirection(Vector3 direction)
    {
        transform.rotation.SetLookRotation(direction);

        //create the rotation we need to be in to look at the target
        lookRotation = Quaternion.LookRotation(direction);
    }
    

    void SetStarBright(float realDistance){

		if (realDistance > 30f)
			bright = 0.2f;
		else if (realDistance > 20f)
			bright = 0.4f;
		else if (realDistance > 10f)
			bright = 0.6f;
		else
			bright = 1f;
	}

	void Update () {
		if (nim == null)
			return;

        if(appleTransform != null)
        {
            
            Vector3 direc = (new Vector3(nim.position.x, nim.position.y, 0f) - new Vector3(appleTransform.position.x, appleTransform.position.y, 0f)).normalized;
            Quaternion rotationTow = Quaternion.LookRotation(new Vector3(0,0,1), -direc);

            if (!firstSetDir)
            {
                transform.rotation = rotationTow;
                firstSetDir = true;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTow, Time.deltaTime * rotationSpeed);
            }               
            
        }

		timeToCheck -= Time.deltaTime;
		if (timeToCheck < 0f)
			timeToCheck = T_CHECK;
		else
			return;

		SetShortestDistance ();
		//Debug.Log (bright+" star bright");
		
		ps.startColor = new Color (startColor.r, startColor.g, startColor.b, bright);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
    }

}
