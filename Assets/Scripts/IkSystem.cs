using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkSystem : MonoBehaviour
{
    [SerializeField] Transform[] Points;
    [SerializeField] Transform[] Targets;
    [SerializeField] RaycastHit[] Hits;


    private int stepIndex;
    private float stepDuration = .1f;
    private bool isStepping = false;

    void Start()
    {
        Hits = new RaycastHit[Targets.Length];  
        StartCoroutine(StepCycel());
    }

    void Update()
    {
        for (int i = 0; i < Targets.Length; i++)
        {
            if(Physics.Raycast(Points[i].position, Vector3.down, out Hits[i])){
                if(Vector3.Distance(Hits[i].point, Targets[i].position) > .5f && ShouldWalkLegs(i))
                {
                    Targets[i].position = Hits[i].point;
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

    bool ShouldWalkLegs(int legIndex){
        int[] [] legPairs = new int[] []{
            new int[] {0, 4 },
            new int[] {1 , 5},
            new int[] {2, 3}
        };

        return legPairs[stepIndex] [0] == legIndex || legPairs[stepIndex][1] ==legIndex;
    }

}
