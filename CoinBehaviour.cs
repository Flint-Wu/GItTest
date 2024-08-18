using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public enum CoinState
    {
        Idle,
        Move,
    }
    float lifeTime =0f;
    public CoinState coinState = CoinState.Idle;
    public Transform target;
    public AudioClip[] getCoinSound;
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        switch (coinState)
        {
            case CoinState.Idle:
                if (lifeTime > 1f)
                {
                    StartChase();
                }
                break;
            case CoinState.Move:
                ChaseTarget();
                JudgeState();
                break;
        }
    }
    void StartChase()
    {
        coinState = CoinState.Move;
    }
    void ChaseTarget()
    {
        Vector3 dir = target.position+Vector3.up - transform.position;
        transform.Translate(dir.normalized * lifeTime * Time.deltaTime*3);
    }

    void JudgeState()
    {
        if (Vector3.Distance(target.position+Vector3.up,transform.position) < 0.2f)
        {
            if (getCoinSound != null)
            {
                AudioSource.PlayClipAtPoint(getCoinSound[Random.Range(0, getCoinSound.Length)], transform.position, 0.5f);
            }
            AddCoin();
            Destroy(this.gameObject);
        }
    }

    void AddCoin()
    {
        
    }
}
