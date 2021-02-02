using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkDetectManager : MonoBehaviour
{
    private List<Enemys> enemys = new List<Enemys>();
    private Vector2 dstA, dstB;
    private float dst;
    private void Awake()
    {
        enemys = SpawnManager.spawnManager.enemys;
        dstB = new Vector2(this.transform.position.x, this.transform.position.z);
        //Enemys.Add(GameObject.FindWithTag("Enemy").GetComponent<Enemys>());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Enemys enemy in enemys)
        {
            dstA = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
            dst = Vector2.Distance(dstA, dstB);
            if(dst<=2.0f)
            {
                Debug.Log("Enemy is in");
            }
        }
    }
}
