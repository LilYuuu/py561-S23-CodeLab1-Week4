using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;

    public int currentLevel = 0;

    public int targetScore = 5;

    public float currTime = 0.0f;

    private const string DIR_DATA = "/Data/";
    private const string FILE_BEST_RECORDS = "bestRecords.txt";
    private string PATH_BEST_RECORDS;
    
    // list of multiple high scores
    private List<float> bestRecords = new List<float>();

    public TextMeshProUGUI textMeshPro;

    public bool inGame = true;
    
    void Awake()    // Called when the script is being loaded
    {
        if (Instance == null)   // Instance has not been set
        {
            DontDestroyOnLoad(gameObject);  // Don't destroy
            Instance = this;    // Set Instance
        }
        else // Instance is set
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currTime = 0;

        PATH_BEST_RECORDS = Application.dataPath + DIR_DATA + FILE_BEST_RECORDS;

    }

    // Update is called once per frame
    void Update()
    {
        if (!inGame)
        {
            currTime = 0.0f;
        }
        
        if (currentLevel != 2 && currentLevel != -1)
        {
            // CurrTime = Time.time;
            currTime += Time.deltaTime;
        }

        if (inGame)     // to display the list of best records in the final scene
        {
            textMeshPro.text = 
                "Level: " + (currentLevel + 1) + "\n" + 
                "Time used: " + currTime.ToString("N1") + "\n";
        }
        
        
        // if (score == targetScore || Input.GetKey(KeyCode.Return))   // for debugging purpose
        if (score == targetScore) 
        {
            currentLevel++;
            SceneManager.LoadScene(currentLevel);
            score = 0;
        }
        
        // winner scene
        if (currentLevel == 2 && inGame)
        {
            inGame = false;
            UpdateBestRecords();
            WASDControl.Instance.rb2d.gameObject.SetActive(false);
        }
        
    }
    
    void UpdateBestRecords()
    {
        // if the list is empty
        if (bestRecords.Count == 0)
        {
            string fileContents = File.ReadAllText(PATH_BEST_RECORDS);
            string[] fileSplit = fileContents.Split("\n");
            for (int i = 1; i < fileSplit.Length - 1; i++) // skip the tile of "High score:" and the last empty string 
            {
                bestRecords.Add(float.Parse(fileSplit[i]));
            }
        }
        
        // remove if the list has more than 5 values
        if (bestRecords.Count > 5)
        {
            Debug.Log("remove!");
            bestRecords.RemoveRange(4, bestRecords.Count-5);    
        }
        

        for (int i = 0; i < bestRecords.Count; i++)
        {
            if (bestRecords[i] > currTime)
            {
                bestRecords.Insert(i, currTime);
                break;  // jump out of the loop
            }
        }

        string bestRecordsStr = "Best Records:\n";
        for (int i = 0; i < bestRecords.Count; i++)
        {
            Debug.Log(i);
            bestRecordsStr += bestRecords[i].ToString("N1") + "\n";
        }

        textMeshPro.text = bestRecordsStr;
        
        File.WriteAllText(PATH_BEST_RECORDS, bestRecordsStr);
    }
}
