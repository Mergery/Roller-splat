using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public AudioClip levelUpSound;
    private AudioSource levelAudio;

    public ParticleSystem levelUpParticles;

    private GroundPiece[] allGroundPieces; // to hold all ground pieces and enable us to trace how they are discolored 
    // Start is called before the first frame update
    void Start()
    {
        levelAudio= GetComponent<AudioSource>();
        // set up our level though we must have ground pieces 
        SetUpNewLevel();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetUpNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if(singleton == null)
        {
            singleton= this;
        }
        else if (singleton!= this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevel();
    }

    public void  CheckComplete()
    {
        bool isFinished = true;
        for(int i =0 ; i<allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished= false;
                break;

            }
        }
        if(isFinished)
        {
            levelUpParticles.Play();
            levelAudio.PlayOneShot(levelUpSound, 1.0f);
            
            NextLevel();
            
        }
    }
    private void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex== 4)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // whta loads the scene 
        }
       
    }
}
