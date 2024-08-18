using System.Collections.Generic;
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
    public float StruggleTime = 0f;
    public List<float> StruggleTimeList = new List<float>();
    public Transform player;
    //public GameObject Flash;
    void Start()
    {
        fishState = FishState.Move;
        fishPool = this.transform.parent.GetComponentInChildren<FishPool>();
        TargetTransform = Instantiate(new GameObject()).transform;
        EscapeTransform = fishPool.transform.Find("EscapePoint");
        GenerateTarget();
        player = GameObject.FindWithTag("Player").transform;

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
    void DeadBehavior()
    {
        //this.transform.Translate(Vector3.down * 1f * Time.deltaTime);

        Vector3 dir = player.position+Vector3.up - transform.position;
        transform.Translate(dir.normalized * Time.deltaTime*3,Space.World);

        if (Vector3.Distance(player.position+Vector3.up,transform.position) < 0.2f)
        {
            Destroy(this.gameObject);
        }
    }
    void OnHooked()
    {
        //模拟鱼挣扎(下沉)
        transform.Translate(Vector3.down * 1f * (1-StruggleTime) * Time.deltaTime);
        StruggleTime += Time.deltaTime;//挣扎值逐渐增加
        //如果挣扎值大于1,则脱钩
        if (StruggleTime > StruggleTimeList[1])
        {
            fishState = FishState.Move;
            _GetHookTransform = Vector3.zero;
            fishPool.FishHookTransform.parent.GetComponent<FishingRod>().ReleaseFish();
            fishPool.ExitFish();
        }

    }
    public int GetHooked()//鱼上钩的判断
    {
        //1代表完美钓到鱼，2代表普通起鱼，3代表鱼逃跑
        //被钩住后,鱼钩位置跟随鱼的位置
        if(StruggleTime<StruggleTimeList[0])
        {
            fishState = FishState.Dead;
            fishPool.ExitFish();
            return 1;
        }
        else if(StruggleTime<StruggleTimeList[1])
        {
            fishState = FishState.Dead;
            fishPool.ExitFish();
            return 2;
        }
        else
        {
            return 3;
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
                fishPool.PlayFlash();
                // Flash.transform.position = transform.position+Vector3.up*2f;
                // Flash.gameObject.SetActive(true);

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
