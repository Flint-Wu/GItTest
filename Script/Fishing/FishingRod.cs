using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [System.Serializable]
    public class ConrolPoint
    {
        //用来储存控制点的偏移量
        public float DiifH;
        public float DiifV;
    }
    //鱼竿的开始位置和结束位置
    public Transform BeginTransform;
    public Transform EndTransform;
    public Transform HookTransform;
    public LineRenderer fishrodLine;
    public LineRenderer fishingLine;
    [Range(0, 1)]
    public float strengeth = 0.5f;
    [Tooltip("控制点的偏移量")]
    public List<ConrolPoint> DiffPoints = new List<ConrolPoint>();
    public bool isHooked = false;
    public FishAI hookedFish;
    // Start is called before the first frame update
    void Start()
    {
        fishrodLine = GetComponent<LineRenderer>();
        BeginTransform = transform.GetChild(0);
        EndTransform = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateLineStretch();
        DrawFishingRod();
        DrawFishingLine();
        if (isHooked)
        {
            //鱼上钩后,鱼钩位置跟随鱼的位置
            HookTransform.position = hookedFish.transform.position;
        }
        else
        {
            //鱼钩做正弦运动（y方向）
            HookTransform.position = new Vector3(HookTransform.position.x, HookTransform.position.y + Mathf.Sin(Time.time * 2) * 0.01f, HookTransform.position.z);
        }
        Test();
        
    }

    public void CatchFish(FishAI fishAI)
    {
        //捕获到鱼
        Debug.Log("CatchFish");
        isHooked = true;
        hookedFish = fishAI;
    }
    public void ReleaseFish()
    {
        //鱼脱勾了
        isHooked = false;
        hookedFish = null;
    }   
    void DrawFishingRod()
    {
        fishrodLine.positionCount = GetFishingRodPoints().Count;

        //根据控制点生成贝塞尔曲线绘制鱼竿sa
        //BezierHelper.Bezier(0.5f, ControlPoints);
        fishrodLine.SetPositions(GetFishingRodPoints().ToArray());
    }
    void DrawFishingLine()
    {
        fishingLine.positionCount = 2;
        fishingLine.SetPosition(0, HookTransform.position);
        fishingLine.SetPosition(1, EndTransform.position);
    }
    void CalculateLineStretch()
    {
        //计算鱼线的拉伸程度
        float distance = Vector3.Distance(HookTransform.position, EndTransform.position);
        strengeth = Mathf.Clamp(distance-4, 0, 1);
    }
    //获取鱼竿的起始点和结束点
    List<Vector3> GetFishingRodPoints()
    {
        //存储绘制所有point的位置
        List<Vector3> points = new List<Vector3>();
        
        List<Vector3> _controlPoints = new List<Vector3>();
        _controlPoints.Add(BeginTransform.position);
        //将控制点的位置添加到控制点列表中,并且加上偏移量,可以通过偏移量来控制鱼竿的弯曲程度
        for (int i = 0; i < 5; i++)
        {
            //start - end 水平方向的偏移量
            Vector3 _controlPoint = BeginTransform.position + (EndTransform.position - BeginTransform.position)*DiffPoints[i].DiifH ;
            //start - end 垂直方向的偏移量(yz平面)
            Vector2 _VerticalOffset = Vector2.Perpendicular(EndTransform.position - BeginTransform.position) * DiffPoints[i].DiifV*strengeth*-1;
            _controlPoint = _controlPoint + new Vector3(0,_VerticalOffset.x,_VerticalOffset.y);
            _controlPoints.Add(_controlPoint);
        }
        _controlPoints.Add(EndTransform.position);

        for (int i = 0; i < 100; i++)
        {
            points.Add(BezierHelper.Bezier(i / 100f, _controlPoints));
        }
        points.Add(EndTransform.position);
        return points;
    }

    void Test()
    {
        //按空格键,减少鱼的挣扎值
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isHooked)
            {
                hookedFish.ReduceStruggleRate(0.1f);
            }
        }
    }
}
