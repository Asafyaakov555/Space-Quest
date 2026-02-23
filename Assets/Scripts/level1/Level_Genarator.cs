using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Level_Genarator : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject[] floorPreFabs;
    [SerializeField] GameObject[] Coins;
    [SerializeField] GameObject Monster;
    [SerializeField] GameObject player;
    [SerializeField] GameObject trampoline;
    List<GameObject> appleAndTramps = new List<GameObject>();
    [SerializeField] GameObject apple;

    [Header("transform")]
    [SerializeField] Transform ArenaParent;
    [SerializeField] Transform CoinsParent;

    Dictionary<Vector3, GameObject> floors = new Dictionary<Vector3, GameObject>();
    List<Vector3> floorKeysList = new List<Vector3>();

    Vector3[] FirstFloorPositions =
    {
        new Vector3(0, 0, 0),
        new Vector3(10.5f, 0, 0),
        new Vector3(-10.5f, 0, 0),
        new Vector3(0, 0, 10.5f),
        new Vector3(-10.5f, 0, 10.5f),
        new Vector3(10.5f, 0, 10.5f),
    };

    private struct MazeRequest
    {
        public Vector3 originPos;
        public Vector3 oldFloorPos;

        public MazeRequest(Vector3 origin, Vector3 old)
        {
            originPos = origin;
            oldFloorPos = old;
        }
    }

    private Queue<MazeRequest> spawnQueue = new Queue<MazeRequest>();

    [Header("Settings")]
    public LayerMask obstacleLayer;
    public float checkRadius = 1f;

    float nextAppleTime = 0f;
    float delayApple = 11;
    float nexttrampTime = 0;
    float delaytramp = 12;

    [Header("bool values")]
    public bool loseBool = false;
    private bool isProcessing = false;
    public bool GodMode = false;

    void Update()
    {
        if (loseBool) return;

        spawnApple();
        spawntramp();
    }

    public void DestroyAllItems()
    {
        foreach (var item in appleAndTramps)
        {
            Destroy(item);
        }
    }

    void spawnApple()
    {
        if (GodMode) return;
        if (floors.Count == 0) return;
        if (Time.time < nextAppleTime) return;
        float randomNum = Random.Range(0f, 1f);
        if(randomNum < 0.3f)
        return;
        int floorPos = Random.Range(0, floorKeysList.Count);
        Vector3 randomPos = floorKeysList[floorPos];  
        GameObject selectedFloor = floors[randomPos];
        if (selectedFloor.transform.position == Vector3.zero) return;
        foreach (Transform child in selectedFloor.transform)//we dont want two apples on the same floor
        {
            if (child.CompareTag("Apple")) return;
        }
        if (selectedFloor.name.Contains("rollFloor"))//specip place for this floor
        {
            Vector3 spawnPosition = randomPos + new Vector3(0, 0.5f, 0);
            GameObject appleREF = Instantiate(
                apple,
                spawnPosition, 
                Quaternion.identity,
                selectedFloor.transform
            );
            
            appleAndTramps.Add(appleREF); 
            nextAppleTime = Time.time + delayApple;
            return;

        } 
        Vector3 offset = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 2f),
            Random.Range(-1f, 1f)
        );
        Vector3 targetPosition = randomPos + offset;
        bool isBlocked = Physics.CheckSphere(targetPosition, checkRadius, obstacleLayer);
        if (!isBlocked)
        {
            
            Vector3 spawnPosition = targetPosition + new Vector3(0, 0.36f, 0);

            GameObject appleREF = Instantiate(
                apple,
                spawnPosition, 
                Quaternion.identity,
                selectedFloor.transform
            );
            
            appleAndTramps.Add(appleREF); 
            nextAppleTime = Time.time + delayApple;
        }
        else
        {
            //Debug.Log("Position blocked, skipping spawn this time. -apple");
            // if dont find good place to instantiate skip but dont add delay to next apple
        }
    }
    void spawntramp()
    {
        if (GodMode) return;
        if (floors.Count == 0) return;
        if (Time.time < nexttrampTime) return;
        float randomNum = Random.Range(0f, 1f);
        if(randomNum < 0.3f)
        return;
        int floorPos = Random.Range(0, floorKeysList.Count);
        Vector3 randomPos = floorKeysList[floorPos]; 
        GameObject selectedFloor = floors[randomPos];
        if (selectedFloor.transform.position == Vector3.zero) return;
        if (selectedFloor.name.Contains("rollFloor")||selectedFloor.name.Contains("more")) return;// to prevent over calculate to find good place to spawn tramp 
        foreach (Transform child in selectedFloor.transform)//we dont want two tramp on the same floor
        {
            if (child.CompareTag("trampoline")) return;
        }

        
        Vector3 offset = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        );
        Vector3 targetPosition = randomPos + offset;

        
        bool isBlocked = Physics.CheckSphere(targetPosition, checkRadius, obstacleLayer, QueryTriggerInteraction.Collide);
        
        if (!isBlocked)
        {
            
            Vector3 spawnPosition = targetPosition + new Vector3(0, 0.36f, 0);

            GameObject trampREF = Instantiate(
                trampoline,
                spawnPosition, 
                Quaternion.identity,
                selectedFloor.transform
            );
            
            appleAndTramps.Add(trampREF); 
            nexttrampTime = Time.time + delaytramp;
        }
        else
        {
            //Debug.Log("Position blocked, skipping spawn this time. -tramp");
            // if dont find good place to instantiate skip but dont add delay to next tramp
        }
    }

    void SpawnCoin(Vector3 vec, Transform parent)
    {
        int NumberOfNewCoins = Random.Range(3, 6);
        int typeOfCoin = Random.Range(0, 3);
        GameObject NewCoin = Coins[typeOfCoin];

        Vector3 offset = Vector3.zero;
        float value = Random.value;
        Vector3 decVec;

        if (value >= 0.5)
        {
            offset += new Vector3(1.5f, 0, 0);
            decVec = new Vector3(-4f, 0, 0);
        }
        else
        {
            offset += new Vector3(0, 0, 1.5f);
            decVec = new Vector3(0, 0, -4f);
        }

        vec += decVec;

        for (int i = 0; i < NumberOfNewCoins; i++)
        {
            bool isBlocked = Physics.CheckSphere(vec, checkRadius, obstacleLayer);
            //Collider[] hitColliders = Physics.OverlapSphere(vec, checkRadius, obstacleLayer);
            if (!isBlocked)
            {
                Instantiate(NewCoin, vec, Quaternion.identity, parent);
            }
            else
            {
                /*
                foreach (var hit in hitColliders)
                {
                    Debug.LogError($"BLOCKED BY: {hit.name} | Layer: {LayerMask.LayerToName(hit.gameObject.layer)}");
                }
                Debug.Log("Position blocked, skipping spawn this time. -coin ");
                */
            }

            vec += offset;
        }
    }

    public void SpawnStartingFloor()
    {
        for (int i = 0; i < 6; i++)
        {
            Spawnfirst(i);
        }

        NavMeshSurface surface = ArenaParent.GetComponent<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }

    void Spawnfirst(int i)
    {
        GameObject FloorToSpawn = floorPreFabs[0];
        Vector3 FloorPos = FirstFloorPositions[i];
        GameObject newFloor = Instantiate(FloorToSpawn, FloorPos, Quaternion.identity, ArenaParent);
        floors.Add(FloorPos, newFloor);
        floorKeysList.Add(FloorPos);
    }

    public void deleteFloor(Vector3 oldFloorPos)
    {
        if (floors.ContainsKey(oldFloorPos))
        {
            floors.Remove(oldFloorPos);
            floorKeysList.Remove(oldFloorPos);
        }
    }

    public void SpawnMaze(Vector3 originPos, Vector3 oldFloorPos)
    {
        spawnQueue.Enqueue(new MazeRequest(originPos, oldFloorPos));

        if (!isProcessing)
        {
            StartCoroutine(ProcessSpawnQueue());
        }
    }

    IEnumerator ProcessSpawnQueue()
    {
        isProcessing = true;
        while (spawnQueue.Count > 0)
        {
            MazeRequest currentRequest = spawnQueue.Dequeue();//to prevent the situation the process twice for the same floor
            GenerateMazeLogic(currentRequest.originPos, currentRequest.oldFloorPos);
            yield return null;
        }

        isProcessing = false;
    }

    public void GenerateMazeLogic(Vector3 originPos, Vector3 oldFloorPos)
    {
        Vector3[] directions =
        {
            new Vector3(10.5f, 0, 0),
            new Vector3(-10.5f, 0, 0),
            new Vector3(0, 0, 10.5f),
            new Vector3(0, 0, -10.5f)
        };

        List<Vector3> validPositions = new List<Vector3>();

        foreach (Vector3 dir in directions)
        {
            Vector3 checkPos = originPos + dir;
            if (!floors.ContainsKey(checkPos))
            {
                validPositions.Add(checkPos);
            }
        }

        deleteFloor(oldFloorPos);

        int wantedFloors = Random.Range(1, 4);
        int amountToSpawn = Mathf.Min(wantedFloors, validPositions.Count);

        for (int i = 0; i < amountToSpawn; i++)
        {
            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3 spawnPos = validPositions[randomIndex];
            validPositions.RemoveAt(randomIndex);

            GameObject floorPrefab = floorPreFabs[Random.Range(0, floorPreFabs.Length)];
            GameObject newFloor = Instantiate(floorPrefab, spawnPos, Quaternion.identity, ArenaParent);
            floors.Add(spawnPos, newFloor);
            floorKeysList.Add(spawnPos);

            if (Random.value >= 0.5f)
            {
                SpawnCoin(spawnPos, newFloor.transform);
            }
        }

        if (ArenaParent != null)
        {
            NavMeshSurface surface = ArenaParent.GetComponent<NavMeshSurface>();
            if (surface != null)
            {
                StartCoroutine(UpdateNavMeshAsync(surface));
            }
        }
    }

    private IEnumerator UpdateNavMeshAsync(NavMeshSurface surface)// update the maze for ai agent
    {
        yield return new WaitForEndOfFrame();

        if (surface.navMeshData != null)
        {
            surface.UpdateNavMesh(surface.navMeshData);
        }
        else
        {
            surface.BuildNavMesh();
        }
    }
}
