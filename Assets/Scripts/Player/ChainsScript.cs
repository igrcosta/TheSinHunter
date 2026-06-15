using UnityEngine;
using System.Collections.Generic;

public class ChainsScript : MonoBehaviour
{
    [Header("M.I.R.A")]
    public List<GameObject> PossibleTargets = new List<GameObject>();
    public GameObject ActualTarget;

    //Referencias e Variaveis privadas
    private int targetsListed = 0;
    private bool AnalysingTargets = false;
    private bool isTracking = false;
    private Vector3 TargetPosition;  //Usada para guardar as posições dos alvos
    private Player Pref;

    private void Start()
    {
        Pref = GameController.controller.playerRef;
   

    }
    void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Enemy") || other.CompareTag("Parry"))
        //{
        //    PossibleTargets.Add(other.gameObject);

        //    if (targetsListed <= 0)
        //    {
        //        ActualTarget = other.gameObject;
        //        isTracking = true;
        //        //Rastreia o primeiro inimigo que aparecer
        //    }
        //}
        if (other.CompareTag("Lever"))
        {
            PossibleTargets.Add(other.gameObject);

            if (targetsListed <= 0)
            {
                ActualTarget = other.gameObject;
                isTracking = true;
                //Rastreia o primeiro inimigo que aparecer
            }
        }
    }

    void Update()
    {

        if (GameController.controller.ChainsUnlocked)
        {

            if (ActualTarget != null && Pref.ActualWeapon != Player.WeaponTypes.LuxuryChains)
            {
                Debug.Log("Corrente");
                Pref.SetActualWeapon("LuxuryChains");
            }

            if (ActualTarget == null && Pref.ActualWeapon == Player.WeaponTypes.LuxuryChains)
            {
                Pref.SetActualWeapon("Default");
            }

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
            //Esse aqui só funciona quando tem alvos pra rastrear
        }
        else
        {
            GameController.controller.UIManager.DisableAim();
        }


    }
    void RemoveBehindPlayer()
    {
        for (int i = targetsListed - 1; i >= 0; i--)
        {
            if (PossibleTargets[i] == null)
            {
                PossibleTargets.Remove(PossibleTargets[i]);
            }
            if (PossibleTargets[i] != null && PossibleTargets[i].transform.position.x - Pref.transform.position.x <= 0.1f)
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
                //eita //safado
            }
            ActualTarget = BestTarget;
            GameController.controller.playerRef.TargetObject = ActualTarget;
            isTracking = true;
            //se definimos o alvo, devemos rastreá-lo
        }
    }

    void TargetTracking()
    {
        if (Pref.ChainsActive == true)
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
        else
        {

        }
    }
}