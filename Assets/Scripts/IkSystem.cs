using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkSystem : MonoBehaviour
{
    [SerializeField] Transform[] Points;
    [SerializeField] Transform[] Targets;
    [SerializeField] RaycastHit[] Hits;


    private int stepIndex;
    [SerializeField] float stepDuration = 0.1f;
    [SerializeField] float stepDistance;
    [SerializeField] float stepThreshold;
    private bool isStepping = false;

    private PlayerMovement playerMovement;

    void Start()
    {
        Hits = new RaycastHit[Targets.Length];  
        StartCoroutine(StepCycel());

        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        for (int i = 0; i < Targets.Length; i++)
        {
            if(Physics.Raycast(Points[i].position, Vector3.down, out Hits[i])){

                float distance = Vector3.Distance(Hits[i].point, Targets[i].position);
                if(distance > stepThreshold && ShouldWalkLegs(i) && playerMovement.Grounded())
                {   
                    StartCoroutine(MoveLegs(i, Hits[i].point));
                }
            }
        }  
    }

    IEnumerator StepCycel(){
        while(true){
            isStepping = true;

            stepIndex = (stepIndex + 1) % 3;
            
            yield return new WaitForSeconds(stepDuration);
            isStepping = false;

            yield return new WaitForSeconds(stepDuration);
        }
    }

    IEnumerator MoveLegs(int legIndex, Vector3 targetPosition){
        float elapsedTime = 0f;
        Vector3 startPosition = Targets[legIndex].position;

        while (elapsedTime < stepDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / stepDuration;
            Targets[legIndex].position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        Targets[legIndex].position = targetPosition;
    }

    bool ShouldWalkLegs(int legIndex){
        int[] [] legPairs = new int[] []{
            new int[] {0, 4},
            new int[] {1, 5},
            new int[] {2, 3}
        };

        return legPairs[stepIndex] [0] == legIndex || legPairs[stepIndex][1] ==legIndex;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < Targets.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Points[i].position, Vector3.down * 10);
            Gizmos.DrawSphere(Targets[i].position, .1f);
        }  
    }

}
