using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;



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
    private int cubeCount;



    private void OnTouch()
    {
        var touchPos = touchPosAction.ReadValue<Vector2>();
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        RaycastManager.Raycast(touchPos, hits, TypeToTrack);
        if (hits.Count > 0)
        {
            ARRaycastHit firstHit = hits[0];
            Instantiate(PrefabToInstantiate, firstHit.pose.position, firstHit.pose.rotation);
            GameObject cube = Instantiate(PrefabToInstantiate, firstHit.pose.position, firstHit.pose.rotation);
            instantiatedCubes.Add(cube);
            cubeCount += 1;
            countText.text = "Cubes: " + cubeCount;
        }
       

    }

    void Start()
    {
        touchPressAction = PlayerInput.actions["TouchPress"];
        touchPosAction = PlayerInput.actions["TouchPos"];
        instantiatedCubes = new List<GameObject>();
        touchPhaseAction = PlayerInput.actions["TouchPhase"];
        cubeCount = 0;
    }


    void Update()
    {
        if (touchPressAction.WasPerformedThisFrame())
        {
            OnTouch();
        }
        if (touchPressAction.WasPerformedThisFrame())
        {
            var touchPhase = touchPhaseAction.ReadValue<UnityEngine.InputSystem.TouchPhase>();
            if (touchPhase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                OnTouch();
            }
        }
    }
    public void ChangeColor()
    {
        foreach (GameObject cube in instantiatedCubes)
        {
            int randomIndex = Random.Range(0, Materials.Count);
            Material randomMaterial = Materials[randomIndex];
            cube.GetComponent<MeshRenderer>().material = randomMaterial;
        }
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
