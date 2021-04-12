using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform teleportTarget;
    public Transform cameraTarget;
    public Vector3 cameraPosition;
    public GameObject player;
    public GameObject mainCamera;
    public float smoothness;
    public bool collided;
    public EnemiesDefeatedTrigger trigger;
    void Awake()
    {
        player = GameObject.Find("Fox");
        mainCamera = GameObject.Find("CameraHolder");
        cameraPosition = mainCamera.transform.position;
        smoothness = 200;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (trigger != null)
        {
            for (int i = 0; i < trigger.enemiesList.Count; i++)
            {
                trigger.enemiesList[i].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }
        }
        if (collider.gameObject.tag == "Player")
        {
            player.transform.position = teleportTarget.transform.position;
            player.GetComponent<PlayerController>().enabled = false;
            collided = true;
        }
    }
    void Update()
    {
        if (collided)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, cameraTarget.transform.position, Time.deltaTime * smoothness);
        }
        if (mainCamera.transform.position == cameraTarget.transform.position)
        {
            if (player.GetComponent<PlayerController>().enabled == false)
            {
                player.GetComponent<PlayerController>().enabled = true;
            }
            collided = false;
        }
    }
}
