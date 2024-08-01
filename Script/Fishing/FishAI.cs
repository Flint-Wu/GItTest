using Unity.VisualScripting;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public enum FishState
    {
        Move,
        Follow,//跟随鱼钩
        Hooked,//上钩
        Dead
    }
    public FishState fishState;
    public Transform TargetTransform;
    public Transform EscapeTransform;//逃跑点
    private Vector3 _GetHookTransform;//被钩住的位置
    public FishPool fishPool;
    public float MoveSpeed;
    public float FollowSpeed;
    public float stateTime;
    public float StruggleRate = 0.5f;
    void Start()
    {
        fishState = FishState.Move;
        fishPool = this.transform.parent.GetComponentInChildren<FishPool>();
        TargetTransform = Instantiate(new GameObject()).transform;
        EscapeTransform = fishPool.transform.parent.Find("EscapePoint");
        GenerateTarget();

    }

    // Update is called once per frame
    void Update()
    {
        //每隔3-8s切换state
        stateTime += Time.deltaTime;
        //每隔5s切换state
        switch (fishState)
        {
            case FishState.Move:
                Move(MoveSpeed);
                CheckReachTarget();
                break;
            case FishState.Follow:
                Move(FollowSpeed);
                CheckReachTarget();
                break;
            case FishState.Hooked:
                OnHooked();
                break;
            case FishState.Dead:
                DeadBehavior();
                break;
        }
    }
    /// <summary>
    /// 让鱼寻找目标
    /// </summary>
    public void FindTarget(){
        fishState = FishState.Follow;
        TargetTransform.position = fishPool.FishHookTransform.position;
    }

    void Move(float speed)
    {
        this.transform.forward = Vector3.Lerp(this.transform.forward, TargetTransform.position - transform.position, 0.05f);
        transform.position = Vector3.MoveTowards(transform.position, TargetTransform.position, speed * Time.deltaTime);
    }
    //减少挣扎值
    public void ReduceStruggleRate(float rate)
    {
        StruggleRate -= rate;
    }
    void DeadBehavior()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position,
                fishPool.FishHookTransform.parent.GetComponent<FishingRod>().EndTransform.position, MoveSpeed * Time.deltaTime);
    }
    void OnHooked()
    {
        //模拟鱼挣扎
        transform.position = Vector3.Lerp(_GetHookTransform, EscapeTransform.position, (StruggleRate-0.5f)/2);
        transform.Rotate(Vector3.up, Random.Range(-360,360) * Time.deltaTime);
        

        StruggleRate += Time.deltaTime*0.5F;//挣扎值逐渐增加
        if(StruggleRate < 0f)
        {
            fishState = FishState.Dead;
        }
        //如果挣扎值大于1,则脱钩
        else if (StruggleRate > 1f)
        {
            fishState = FishState.Move;
            _GetHookTransform = Vector3.zero;
            fishPool.FishHookTransform.parent.GetComponent<FishingRod>().ReleaseFish();
        }

    }
    void CheckReachTarget()
    {
        //到达目标点,如果是Move状态,则重新生成目标，如果是Follow状态，则切换到Hooked状态
        if (Vector3.Distance(transform.position, TargetTransform.position) < 0.2f)
        {
            if(FishState.Move == fishState)
            {   
                GenerateTarget();
            }
            else if (FishState.Follow == fishState)
            {
                stateTime = 0;
                fishState = FishState.Hooked;
                _GetHookTransform = TargetTransform.position;
                fishPool.FishHookTransform.parent.GetComponent<FishingRod>().CatchFish(this);

            }
        }
    }
    void GenerateTarget()
    {
        TargetTransform.position = new Vector3(
        Random.Range(fishPool.PoolRange.bounds.min.x, fishPool.PoolRange.bounds.max.x), 
        Random.Range(fishPool.PoolRange.bounds.min.y, fishPool.PoolRange.bounds.max.y), 
        Random.Range(fishPool.PoolRange.bounds.min.z, fishPool.PoolRange.bounds.max.z)); 
    }
}
