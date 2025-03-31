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
    public List<Material> Materials;
    [SerializeField] private TMP_Text countText;
    private int score;
    public float spawnInterval = 2.5f; 
    private bool gameActive = true;
    public GameObject gameOverPanel;
    private float timeLeft = 60f;
    public TMP_Text timerText;
    public TMP_Text Rank;
    public ARPlaneManager arPlaneManager;
    public GameObject ParticleEffect;
    private Vector2 touchPos;
    private RaycastHit hit;
    public Camera cam;
    public TMP_Text scoret;
    public AudioSource audioSource;
    public AudioClip destroySound;

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

        GameObject newCube = Instantiate(PrefabToInstantiate, spawnPosition, Quaternion.identity);
        if (Materials.Count > 0)
        {
            Material randomMaterial = Materials[Random.Range(0, Materials.Count)];
            newCube.GetComponent<Renderer>().material = randomMaterial;
        }
    }



    void Start()
    {
        gameActive = true;
        touchPressAction = PlayerInput.actions["TouchPress"];
        touchPosAction = PlayerInput.actions["TouchPos"];
        score = 0;
        StartCoroutine(SpawnCubes());
    }


    void Update()
    {
        

        if (touchPressAction.WasPerformedThisFrame())
        {
            touchPos = touchPosAction.ReadValue<Vector2>();
            Ray ray = cam.ScreenPointToRay(touchPos);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.tag == "Cube")
                {
                    var clone = Instantiate(ParticleEffect, hitObj.transform.position, Quaternion.identity);
                    clone.transform.localScale = hitObj.transform.localScale;
                    if (audioSource != null && destroySound != null)
                    {
                        audioSource.PlayOneShot(destroySound);
                    }
                    Destroy(hitObj);
                    score = score + 10;
                    countText.text = "Score: " + score;
                }
            }
        }


        if (gameActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
               
            }
            else
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
        else if (score >50  && score <= 100)
        {
            Rank.text = "not bad";
        }
        else if (score > 100 && score <= 150)
        {
            Rank.text = "youre good!";
        }
        else if (score > 150 && score < 200)
        {
            Rank.text = "Amazing !!!";
        }
        else
        {
            Rank.text = "You are THE block destroyer.";
        }
        scoret.text = "You're Score : " + score;
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
