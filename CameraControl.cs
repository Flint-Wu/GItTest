using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Material _originMaterial;
    public Material _transparentMaterial;
    public LayerMask _layerMask;
    private List<Material> materials = new List<Material>();
    private List<Material> _materialList = new List<Material>();
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckObstacle();
    }
    //检测player和摄像机中间是否有障碍物,如果有则设置其material的alpha值为0.5
    void CheckObstacle()
    {
        //来源：https://zhuanlan.zhihu.com/p/675702516
        RaycastHit hit;
        if (Physics.Linecast(transform.position, player.position, out hit, _layerMask))
        {

            var meshRenderers = hit.collider.GetComponentsInChildren<MeshRenderer>();
            foreach (var variable in meshRenderers)
            {
                materials.AddRange(variable.materials);
            }
            
        
            var transparentList = materials.Except(_materialList).ToList();
            var opaqueList = _materialList.Except(materials).ToList();
            foreach (var variable in transparentList)
            {
                MaterialTransparent.SetMaterialTransparent(true, variable, 0.22f);
            }

            foreach (var variable in opaqueList)
            {
                MaterialTransparent.SetMaterialTransparent(false, variable);
            }

            _materialList = materials;
            }
    }
}
