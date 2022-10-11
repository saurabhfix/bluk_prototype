using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGeneration : MonoBehaviour
{
    public GameObject cubePrefab;
    public int totalCubeToGenerate;
    public Transform spawnPos;
    public float xT;
    public List<GameObject> generatedCubes;
    int num;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < totalCubeToGenerate; i++)
        {
            var cube = Instantiate(cubePrefab, spawnPos.position, Quaternion.identity);
            spawnPos.position = new Vector3(spawnPos.position.x+xT, Random.Range(-2, 3), 0);
            generatedCubes.Add(cube);
        }
    }

    public void NextCube()
    {
        generatedCubes[num].transform.position = spawnPos.position;
        spawnPos.position = new Vector3(spawnPos.position.x+xT, Random.Range(-2, 3), 0);
        print(num);
        num = num == generatedCubes.Count ? 0 : ++num;
    }


}
