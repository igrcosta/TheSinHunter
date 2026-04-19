using UnityEngine;
using System.Collections.Generic;

public class ChainsScript : MonoBehaviour
{
    public List<GameObject> PossibleTargets = new List<GameObject>(5);
    private int targetsListed = 0;
    private bool AnalysingTargets = false;
    private bool isTracking = false;
    public GameObject ActualTarget;

    private Vector3 TargetPosition;
    //essa var vai ser usada para guardar as posições dos alvos e não ficar repetindo textos enormes


    private Player pRef;

    void Start()
    {
        pRef = GameController.controller.playerRef;
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
                isTracking = true;
                //o primeiro inimigo que aparecer vai estar sendo rastreado
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
        }

        TargetTracking();
        //esse aqui só funciona quando tem alvos pra rastrear

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

                    isTracking = false;
                    //se removeu aquele alvo, só quando achar outro que deve ser true

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
            isTracking = true;
            //se definimos o alvo, devemos rastreá-lo
        }
    }

    void TargetTracking()
    {
        if (isTracking)
        {
            TargetPosition = ActualTarget.transform.position;
            //posição do alvo armazenada e atualizada em tempo real

            Debug.Log(ActualTarget.name + "está no " + ActualTarget.transform.position.x + "em X");

            UIController.UIcontroller.EnableAim();
            UIController.UIcontroller.SetAimPosition(TargetPosition);
            //ativar target sobre o inimigo
        }
        else
        {
            UIController.UIcontroller.DisableAim();
        }
    }
}