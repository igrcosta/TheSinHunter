using UnityEngine;
using System.Collections.Generic;

public class ChainsScript : MonoBehaviour
{
    public List<GameObject> PossibleTargets = new List<GameObject>();
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
        if (other.CompareTag("Enemy") || other.CompareTag("Parry"))
        {
            //Debug.Log("Detectei alvos!");

            PossibleTargets.Add(other.gameObject);

            if (targetsListed <= 0)
            {
                ActualTarget = other.gameObject;
                isTracking = true;
                //o primeiro inimigo que aparecer vai estar sendo rastreado
            }

            //Debug.Log(PossibleTargets[0].name + " foi adicionado!");
        }
    }

    void Update()
    {
        targetsListed = PossibleTargets.Count;
        RemoveBehindPlayer();

        if (ActualTarget == null && PossibleTargets != null)
        {
            SelectNewTarget();
        }
        else
        {
            GameController.controller.UIManager.DisableAim();
        }

        TargetTracking();
        //esse aqui só funciona quando tem alvos pra rastrear

    }
    void RemoveBehindPlayer()
    {
        for (int i = targetsListed - 1; i >= 0; i--)
        {
            if (PossibleTargets[i] == null)
            {
                PossibleTargets.Remove(PossibleTargets[i]);
            }
            if (PossibleTargets[i] != null && PossibleTargets[i].transform.position.x - pRef.transform.position.x <= 0.1f)
            {
                if (ActualTarget == PossibleTargets[i])
                {
                    ActualTarget = null;
                    GameController.controller.playerRef.TargetObject = ActualTarget;
                    PossibleTargets.Remove(PossibleTargets[i]);

                    //já que removemos o alvo atual, procure outro
                }
                else
                {
                    PossibleTargets.Remove(PossibleTargets[i]);
                    //Debug.Log(PossibleTargets[i].name + " foi deletado da lista");
                }
            }

        }

    }

    public void SelectNewTarget()
    {
        if (PossibleTargets.Count != 0)
        {
            if (PossibleTargets[0] == null)
            {
                return;
            }

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
            GameController.controller.playerRef.TargetObject = ActualTarget;
            isTracking = true;
            //se definimos o alvo, devemos rastreá-lo
        }
    }

    void TargetTracking()
    {
        if (targetsListed == 0)
        {
            GameController.controller.UIManager.DisableAim();
            return;

        }
        if (isTracking && targetsListed != 0)
        {
            if (ActualTarget == null) return;
            TargetPosition = ActualTarget.transform.position;
            Debug.Log("POSIÇÃO DE " + TargetPosition.y);
            //posição do alvo armazenada e atualizada em tempo real

            //Debug.Log(ActualTarget.name + "está no " + ActualTarget.transform.position.x + "em X");

            GameController.controller.UIManager.EnableAim();
            GameController.controller.UIManager.SetAimPosition(TargetPosition);
            GameController.controller.playerRef.TargetObject = ActualTarget;
            //ativar target sobre o inimigo
            //enviar valor pro game controller, assim o player pode acessar
        }
        else if (ActualTarget == null && PossibleTargets == null)
        {
            GameController.controller.UIManager.DisableAim();
        }
    }
}