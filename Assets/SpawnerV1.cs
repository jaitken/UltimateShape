using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerV1 : MonoBehaviour
{
    public GameObject cubePrefab;
    public float spawnTime = 1.0f;
    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(cubeWave());

    }
    private void spawnCube()
    {
        GameObject c = Instantiate(cubePrefab) as GameObject;
        c.transform.position = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y*2, 1f);
    }
    
    IEnumerator cubeWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
           spawnCube();
        }
    }
}
