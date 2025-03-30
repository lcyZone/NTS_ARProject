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

    

    Vector3 GetRandomPlanePosition()
    {
        return new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
    }

    IEnumerator SpawnCubes()
    {
        while (gameActive)
        {
            Vector3 spawnPosition = GetRandomPlanePosition();
            Instantiate(PrefabToInstantiate, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }



    void Start()
    {
        gameActive = true;
        touchPressAction = PlayerInput.actions["TouchPress"];
        touchPosAction = PlayerInput.actions["TouchPos"];
        instantiatedCubes = new List<GameObject>();
        touchPhaseAction = PlayerInput.actions["TouchPhase"];
        score = 0;
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
                    score++;
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
