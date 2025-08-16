using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView arrowView;

    [SerializeField] private LayerMask targetLayerMask;

    [SerializeField] private LayerMask cardLayerMask;

    public void StartTageting(Vector3 startPosion)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetupArrow(startPosion);
    }

    public EnemyView EndTargeting(Vector3 endPosion)
    {
        arrowView.gameObject.SetActive(false);
        if(Physics.Raycast(endPosion,Vector3.forward,out RaycastHit hit,10f,targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }
        return null;
    }

    //¿¨ÅÆºÏ³É»¥¶¯
    public CardView EndCardTargeting(Vector3 endPosion)
    {
        arrowView.gameObject.SetActive(false);
        if (Physics.Raycast(endPosion, Vector3.forward, out RaycastHit hit, 10f, cardLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out CardView cardView))
        {
            return cardView;
        }
        return null;
    }
}
