using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Beacon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider other)
    {
        DOTween.To(()=> this.gameObject.GetComponentInParent<SkinnedMeshRenderer>().GetBlendShapeWeight(0), x=> this.gameObject.GetComponentInParent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,x),100f,0.5f);
    }
    void OnTriggerExit(Collider other)
    {
        DOTween.To(()=> this.gameObject.GetComponentInParent<SkinnedMeshRenderer>().GetBlendShapeWeight(0), x=> this.gameObject.GetComponentInParent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,x),0f,0.5f);
    }


}
