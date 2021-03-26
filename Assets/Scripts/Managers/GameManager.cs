using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    public GameObject player;
    public GameObject playerInstance;

    public LevelManager currentLevel;

    public static bool IsInputEnabled = true;

    //private bool soundPlaying = false;

    public static GameManager instance
    {
        get { return _instance;  }
        set { _instance = value;  }
    }

    bool _win;

    public bool win
    {
        get { return win; }
        set 
        {
            _win = value;
            if (_win == true)
            {
                if (SceneManager.GetActiveScene().name == "Jungle_Hijinx")
                {
                    Debug.Log("should have won");
                    SceneManager.LoadScene("WinScreen");
                  
                }
            }
            else
            {

            }
        }
    }

    int _score;
    public int score
    {
        get { return _score; }
        set { _score = value;
            //Debug.Log("Current score is: " + _score);
        }
    }

    public int maxLives = 3;
    int _lives;

    public int lives
    {
        get { return _lives; }
        set
        {   
            if (_lives > value)
            {
                if (SceneManager.GetActiveScene().name == "Jungle_Hijinx")
                {
                    //player.transform.position = new Vector3(-8.36999989f, -3.14599991f, 0f);
                    //score = 0;
                    //player.GetComponent<PlayerMovement>().jumpForce = 350;
                    //SceneManager.LoadScene("Level");
                    //_lives = value;

                    //SpawnPlayer(currentLevel.spawnLocation);
                    //PlayerCollision playerCollider = playerInstance.GetComponent<PlayerCollision>();
                   // Debug.Log(playerInstance.GetComponent<PlayerCollision>().dieSource.isPlaying);
                   // Debug.Log(playerInstance.GetComponent<PlayerCollision>().playerBox.enabled);

                    if (playerInstance.GetComponent<PlayerCollision>().dieSource.isPlaying == false && playerInstance.GetComponent<PlayerCollision>().playerBox.enabled == false)
                    {
                        //Debug.Log("Should have spawned");
                        StartCoroutine(WaitCoroutine());
                        //Respawn();          
                    }
                    //Respawn();

                }
            }
            _lives = value;
            if(_lives > maxLives)
            {
                _lives = maxLives;
            }
            else if (_lives <= 0)
            {
                _lives = 0;
                if (SceneManager.GetActiveScene().name == "Jungle_Hijinx")
                {
                    //SceneManager.LoadScene("GameOver");
                    StartCoroutine(WaitCoroutine());
                }
            }
            //Debug.Log(_lives);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        

        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(win);

        if(player)

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Jungle_Hijinx")
            {
                SceneManager.LoadScene("TitleScreen");
            }
            else if (SceneManager.GetActiveScene().name == "TitleScreen")
            {
                SceneManager.LoadScene("Jungle_Hijinx");
            }
            else if (SceneManager.GetActiveScene().name == "GameOver")
            {
                SceneManager.LoadScene("TitleScreen");
                lives = 3;
                score = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }

    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        CameraFollow mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        EnemyTurret[] turretEnemy = FindObjectsOfType<EnemyTurret>();

        if (mainCamera)
        {
            mainCamera.player = Instantiate(player, spawnLocation.position, spawnLocation.rotation);
            playerInstance = mainCamera.player;

            for (int i = 0; i < turretEnemy.Length; i++)
            {
                turretEnemy[i].playerInstance = playerInstance;
            }

        }
        else
        {
            SpawnPlayer(spawnLocation);
        }
  
    }

    public void Respawn()
    {
        if(SceneManager.GetActiveScene().name == "Jungle_Hijinx")
        { 
            playerInstance.transform.position = currentLevel.spawnLocation.position;
            PlayerReset();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Jungle_Hijinx");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
          EditorApplication.isPlaying = false;
        #else
         Application.Quit();
        #endif
    }

    public void ReturnToMenu()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        score = 0;

        SceneManager.LoadScene("TitleScreen");
    }

    IEnumerator WaitCoroutine()
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);
        if(playerInstance.GetComponent<PlayerCollision>().doneDeath == true)
        {
            if(_lives > 0)
            {
                Respawn();
                IsInputEnabled = true;
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
         
        }

    }

    public void PlayerReset()
    {
        playerInstance.GetComponent<PlayerCollision>().rb.velocity = new Vector2(0, 0);
        playerInstance.GetComponent<PlayerMovement>().jumpForce = 200;
        playerInstance.GetComponent<PlayerFire>().isFiring = false;
        playerInstance.GetComponent<PlayerCollision>().playerBox.enabled = true;
        playerInstance.GetComponent<PlayerCollision>().rb.gravityScale = 1;
        playerInstance.GetComponent<PlayerCollision>().death = false;
        playerInstance.GetComponent<PlayerCollision>().doneDeath = false;
    }
}
