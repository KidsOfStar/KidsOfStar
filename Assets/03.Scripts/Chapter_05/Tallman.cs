using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tallman : MonoBehaviour
{

    [SerializeField] private GameObject bubbleTextPrefab;   // 말풍선 프리팹
    private GameObject bubbleTextInstance;                  // 생성된 프리팹 인스턴스

    private Coroutine warningCoroutine;                     // 경고 코루틴


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
