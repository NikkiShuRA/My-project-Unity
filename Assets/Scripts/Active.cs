using UnityEngine;

public class Active : MonoBehaviour
{
    public CheckPointManager Manager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.PointActivated();
            Debug.Log("Игрок вошёл в триггер!");
        }
    }
}
