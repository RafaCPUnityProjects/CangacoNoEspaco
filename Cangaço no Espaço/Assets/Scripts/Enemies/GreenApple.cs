using UnityEngine;
using System.Collections;

public class GreenApple : MonoBehaviour
{

	public GameObject dropAcid;
	public float timedDrop = 0.5f;
	public float increaseScale = 0.1f;

	float dropT;

	Vector3 lastPos;
	GameObject lastAcidDrop;

    float timeStopped = 0f;
    //Vector3 expandPos;
    

    void Start()
	{
		//InvokeRepeating("SpawnAcid", 1f, timedDrop);

		dropT = timedDrop;
	}


	void Update()
	{
        
        if (lastAcidDrop == null || lastPos != transform.position)
		{
            //DROP
			lastPos = transform.position;
			dropT -= Time.deltaTime;
			if (dropT <= 0f)
			{
				dropT = timedDrop;
				lastAcidDrop = Instantiate(dropAcid, transform.position, transform.rotation) as GameObject;
			}
		}
		else
		{
            //Scale acid
            //Vector3 originalSize = lastAcidDrop.transform.localScale;
            //lastAcidDrop.transform.localScale = originalSize + Vector3.one * increaseScale * Time.deltaTime;

            if((timeStopped > 2.5f) && lastPos == transform.position)
            {
                timeStopped = 0f;
                //expandPos = transform.position;

                //DROP
                dropT = timedDrop;
                lastAcidDrop = Instantiate(dropAcid, transform.position, transform.rotation) as GameObject;
                lastAcidDrop.GetComponent<Animator>().SetTrigger("expand");

            }
                

        }

        if (lastPos.magnitude - transform.position.magnitude < 0.01f)
            timeStopped += Time.deltaTime;


    }

    
    void OnDestroy()
    {
        //Destroy all acid, warning! also acid from other green apples
        GameObject[] acids = GameObject.FindGameObjectsWithTag("Acid");

        foreach (GameObject a in acids)
        {
            a.GetComponent<Animator>().SetTrigger("death");
            //Destroy(a);
        }
    }

}
