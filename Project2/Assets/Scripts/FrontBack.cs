using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontBack : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] bool[] targetPoint;

    [SerializeField] float moveSpeed;

    [SerializeField] GameObject movingObstacle;

    private bool goingForward;
    private bool goingBackward;

    // Start is called before the first frame update
    void Start()
    {
        //movingObstacle.transform.position = points[1].transform.position;
        movingObstacle.transform.position = new Vector3(points[0].position.x, points[0].position.y, points[0].position.z);
        goingForward = true;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < points.Length; i++) { //for each point in points array, check to see if the obstacle is at one of them
            if(movingObstacle.transform.position == points[i].position) { //if it is at one of them, then set all points but the next one false and set the next one true
                for(int j = 0; j < targetPoint.Length; j++) {
                    targetPoint[j] = false;
                }

                if((i == targetPoint.Length - 1 && goingForward) || (i == 0 && goingBackward)) {
                    SwitchDirection();
                }
                if(goingForward) {
                    targetPoint[i + 1] = true;
                } else if(goingBackward) {
                    targetPoint[i - 1] = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        //for each bool check if true, move towards
        for(int i = 0; i < targetPoint.Length; i++) {
            if(targetPoint[i] == true) {
                movingObstacle.transform.position = Vector3.MoveTowards(movingObstacle.transform.position, points[i].position, moveSpeed * Time.deltaTime);
            }
        }
    }

    void SwitchDirection() {
        if(goingForward) {
            goingBackward = true;
            goingForward = false;
        } else if(goingBackward) {
            goingForward = true;
            goingBackward = false;
        }
    }

    void OnDrawGizmosSelected() {
        foreach(Transform t in points) {
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }
    }
}
