using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;



public class M : MonoBehaviour
{
    public ARRaycastManager RaycastManager;
    public TrackableType TypeToTrack = TrackableType.PlaneWithinBounds; 
    public GameObject PrefabToInstantiate;
    public PlayerInput PlayerInput;
    private InputAction touchPressAction;
    private InputAction touchPosAction;
    private List<GameObject> instantiatedCubes;
    public List<Material> Materials;
    private InputAction touchPhaseAction;
    [SerializeField] private TMP_Text countText;
    private int score;
    public float spawnInterval = 3f; 
    private bool gameActive = true;
    public GameObject gameOverPanel;
    private float timeLeft = 30f;
    public TMP_Text timerText;
    public TMP_Text Rank;
    public ARPlaneManager arPlaneManager;

    IEnumerator SpawnCubes()
    {
        while (gameActive)
        {
            SpawnCubeOnRandomPlane();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnCubeOnRandomPlane()
    {
        List<ARPlane> planes = new List<ARPlane>();
        foreach (var plane in arPlaneManager.trackables)
        {
            planes.Add(plane);
        }

        if (planes.Count == 0) return; 
        ARPlane randomPlane = planes[Random.Range(0, planes.Count)];
        Vector3 spawnPosition = randomPlane.transform.position;
        spawnPosition.y += 0.05f; 

        Instantiate(PrefabToInstantiate, spawnPosition, Quaternion.identity);
    }



    void Start()
    {
        gameActive = true;
        touchPressAction = PlayerInput.actions["TouchPress"];
        touchPosAction = PlayerInput.actions["TouchPos"];
        instantiatedCubes = new List<GameObject>();
        touchPhaseAction = PlayerInput.actions["TouchPhase"];
        score = 0;
        StartCoroutine(SpawnCubes());
    }


    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Cube")) 
                {
                    Destroy(hit.collider.gameObject);
                    score = score + 10;
                    countText.text = "Score: " + score;
                }
            }
        }

        if (gameActive)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);

            if (timeLeft <= 0)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        gameActive = false;
        if (score == 0)
        {
           Rank.text = "what.";
        }
        else if(score> 0 && score <= 50)
        {
            Rank.text = "youre pretty bad...";
        }
        else if (score >50  && score <= 70)
        {
            Rank.text = "not bad";
        }
        else if (score > 70 && score <= 90)
        {
            Rank.text = "youre good!";
        }
        else if (score > 90 && score < 100)
        {
            Rank.text = "Amazing !!!";
        }
        else
        {
            Rank.text = "You are THE block destroyer.";
        }
        gameOverPanel.SetActive(true);
    }

    
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("AR Scene 2");
    }



}
