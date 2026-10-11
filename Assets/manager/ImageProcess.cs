using UnityEngine;

public class ImageProcess : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(5f * Time.deltaTime, 0.0f, 0.0f);
        
        if (transform.position.x > 25f)
            Destroy(gameObject);
    }

    
}
