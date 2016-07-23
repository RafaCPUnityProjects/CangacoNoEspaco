using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    NimMove nimMove;
    Collider2D playerBodyCollider;
	Collider2D bossBodyCollider;
	Collider2D targetCollider;
    public float verticalOffset;
    public float lookAheadDstX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize;

    FocusArea focusArea;

    Vector2 input;
    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped;

    Transform myTransform;
	public Transform cameraTarget;

	
    void Awake()
    {
		GameplayEvents.nimOnStage.Subscribe (OnNimOnStage);
    }

	void Start(){
		myTransform = transform;
	}

	private void OnNimOnStage(NimHealth nimHealth){
		
		nimMove = nimHealth.GetComponent<NimMove> ();

		if (nimMove == null || cameraTarget == null)
			return;
		
		cameraTarget.parent = nimMove.transform;
		targetCollider = cameraTarget.GetComponent<Collider2D>();
		focusArea = new FocusArea(targetCollider.bounds, focusAreaSize);

		SetTarget ("Player");
	}

	public void SetTarget(string name){
		if (name == "Boss") {
			Transform bossT = GameObject.Find ("Enemy Boss").transform;
			cameraTarget.parent = bossT;
			cameraTarget.localPosition = Vector3.zero;
		} else {
			cameraTarget.parent = nimMove.transform;
			cameraTarget.localPosition = Vector3.zero;
		}
	}

    void LateUpdate()
    {
		if (nimMove == null)
			return;
		
		input = nimMove.GetInput ();

		focusArea.Update(targetCollider.bounds);

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        if(focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if(Mathf.Sign(input.x) == lookAheadDirX && input.x != 0)
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else
            {
                if(!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f; 
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(myTransform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;

        myTransform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;
        float left, right, top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);

            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
