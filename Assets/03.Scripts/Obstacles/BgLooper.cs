using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCount = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BackGround"))
        {

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
