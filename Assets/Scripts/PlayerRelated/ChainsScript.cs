using UnityEngine;
using System.Collections.Generic;

public class ChainsScript : MonoBehaviour
{
    public List<GameObject> PossibleTargets = new List<GameObject>(5);
    private int targetsListed = 0;
    private bool AnalysingTargets = false;
    private bool isTracking = false;
    public GameObject ActualTarget;


    private Player pRef;

    void Start()
    {
        pRef = GameController.controller.playerRef;

        /* //para garantir o sistema de escolha dos alvos, colocamos um valor grande para começar as comparações
        ActualTarget.transform.position = new Vector3(200f, 0f, 0f); */
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Detectei inimigos!");

            PossibleTargets.Add(other.gameObject);

            if (targetsListed <= 0)
            {
                ActualTarget = other.gameObject;
            }

            AnalysingTargets = true;

            Debug.Log(PossibleTargets[0].name + " foi adicionado!");
        }
    }

    void Update()
    {
        targetsListed = PossibleTargets.Count;
        RemoveBehindPlayer();

        if (ActualTarget == null)
        {
            SelectNewTarget();
            TargetTracking();
        }

    }
    void RemoveBehindPlayer()
    {
        for (int i = targetsListed - 1; i >= 0; i--)
        {
            if (PossibleTargets[i].transform.position.x - pRef.transform.position.x <= 5f)
            {
                if (ActualTarget == PossibleTargets[i])
                {
                    ActualTarget = null;
                    PossibleTargets.Remove(PossibleTargets[i]);

                    Debug.Log(PossibleTargets[i].name + " foi deletado do alvo atual e lista");
                    //já que removemos o alvo atual, procure outro
                }
                else
                {
                    PossibleTargets.Remove(PossibleTargets[i]);
                    Debug.Log(PossibleTargets[i].name + " foi deletado da lista");
                }
            }

        }
    }

    void SelectNewTarget()
    {
        if (ActualTarget == null && PossibleTargets != null)
        {
            GameObject BestTarget = PossibleTargets[0];

            for (int i = 0; i < targetsListed; i++)
            {
                if (PossibleTargets[i].transform.position.x < BestTarget.transform.position.x)
                {
                    BestTarget = PossibleTargets[i];
                }
                //eita
            }
            ActualTarget = BestTarget;
        }
    }

    void TargetTracking()
    {
        if (isTracking)
        {
            Debug.Log(ActualTarget.name + "está no " + ActualTarget.transform.position.x + "em X");
        }
    }
}