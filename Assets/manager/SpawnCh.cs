using UnityEngine;

public class SpawnCh : MonoBehaviour
{
    public GameObject Character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnFoo", 1f, 2f);
    }

    // Update is called once per frame
    void SpawnFoo()
    {
        Instantiate(Character, transform.position, transform.rotation);
    }

}
