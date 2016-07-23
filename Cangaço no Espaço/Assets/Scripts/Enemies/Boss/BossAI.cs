using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {

	//FollowTarget followTarget;
	WalkImpulseTarget followTarget;
	MoveRandom moveRandom;
	public GunWeapon gun;

	public Animator aniBoss;
	public SpriteRenderer bossSprite;
	int speedState = 0;

    float spinTime = 2f;
    float timeChangeAttack = 6f;

	Vector3 tempPlayerPosition;

    public enum BossState
	{
		Spin,
        AttackFollow,
        AttackJump,
        AttackShoot,
        AttackBomb
	}

	private BossState curState = BossState.Spin;

	void Awake(){
		//followTarget = GetComponent<FollowTarget> ();
		followTarget = GetComponent<WalkImpulseTarget> ();
		moveRandom = GetComponent<MoveRandom> ();
		//aniBoss = GetComponent<Animator> ();
	}

	void Start(){
		InvokeRepeating ("Spin", timeChangeAttack, timeChangeAttack);
	}

	void Update(){

		switch (curState)
		{
		case BossState.Spin:
            HandleSpinState();
            break;

        case BossState.AttackFollow:
			HandleFollowingState();
			break;

        case BossState.AttackJump:
            HandleJumpState();
            break;

        case BossState.AttackShoot:
			HandleShootState();
			break;

		case BossState.AttackBomb:
			HandleBombState();
			break;
		}
	}


    void HandleSpinState()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        followTarget.enabled = false;
        moveRandom.enabled = false;
        gun.enabled = false;
    }

    void HandleFollowingState()
    {
        followTarget.enabled = true;
        moveRandom.enabled = false;
        gun.enabled = false;
    }

    void HandleJumpState(){

		followTarget.enabled = false;
		moveRandom.enabled = true;
		gun.enabled = false;
	}
    
    void HandleShootState()
    {
        followTarget.enabled = false;
        moveRandom.enabled = false;
        gun.enabled = true;
    }

    void HandleBombState()
    {
        followTarget.enabled = false;
        moveRandom.enabled = false;
        gun.enabled = false;
    }

    
	void Spin(){

        
        curState = BossState.Spin;

        float rnd = Random.value;

        if (rnd > 0.75f)
        {
            aniBoss.SetTrigger("spinApple");
            Invoke("AppleAttack", spinTime);
            Debug.Log("Spin to AppleAttack");
        }
        else if (rnd > 0.5f)
        {
            aniBoss.SetTrigger("spinPineapple");
            Invoke("PineappleAttack", spinTime);
            Debug.Log("Spin to PineappleAttack");
        }
        else if (rnd > 0.25f)
        {
            aniBoss.SetTrigger("spinBanana");
            Invoke("BananaAttack", spinTime);
            Debug.Log("Spin to BananaAttack");
        }
        else {
            aniBoss.SetTrigger("spinWatermelon");
            Invoke("WatermelonAttack", spinTime);
            Debug.Log("Spin to WatermelonAttack");
        }  



	}

    void AppleAttack()
    {
		GetComponent<Animator> ().SetTrigger ("jump");
		tempPlayerPosition = NimMove.instance.transform.position;

		curState = BossState.AttackJump;
    }

    void PineappleAttack()
    {
		GetComponent<Animator> ().SetTrigger ("bomb");
		aniBoss.SetTrigger ("bomb");

        curState = BossState.AttackBomb;
    }

    void BananaAttack()
    {
        curState = BossState.AttackFollow;
    }

    void WatermelonAttack()
    {
        curState = BossState.AttackShoot;
    }



    public void TriggerHealthChanged(float healthPercentage){
		Debug.Log ("%"+healthPercentage);
		if (healthPercentage <= 0.5 && speedState == 0) {
			IncreaseSpeed ();
		}
	}

	void IncreaseSpeed(){
		speedState++;
		//bossSprite.color = Color.red;
		//followTarget.walkForce += 1;
		//moveRandom.force += 1;
	}

	public void MoveToPlayer(){
		transform.position = tempPlayerPosition;
	}
}
