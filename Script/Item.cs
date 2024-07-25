using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject gameObject = collision.gameObject;

        if (collision.gameObject.tag == "Player")
        {
            SaveSelf(gameObject);
            Destroy(this.gameObject);
        }
    }

    private void SaveSelf(GameObject gameObject)
    {
        EventManager.CallItemSave(gameObject);
    }
}
