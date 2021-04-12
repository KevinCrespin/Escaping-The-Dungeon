using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomTrigger : MonoBehaviour
{
    public GameObject lantern;
    private ItemController itemController;
    void Awake()
    {
        itemController = lantern.GetComponent<ItemController>();
    }
    void Update()
    {
        if (itemController.itemPicked)
        {
            Destroy(gameObject);
        }
    }
}
