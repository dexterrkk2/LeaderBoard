using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody rig;
    float startTime;
    float timeTaken;
    public int startingLives;
    int lives;
    float score;
    int collectabledPicked;
    public int maxCollectables = 10;
    bool isPlaying;
    public GameObject playButton;
    public TextMeshProUGUI curTimeText;
    public GameObject[] collectables;
    public Transform startPosition;
    int collectablesTotal;
    int beginTimes;
    // Start is called before the first frame update
    void Awake()
    {
        lives = startingLives;
        rig = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) {
            return;
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);
        curTimeText.text = (Time.time - startTime).ToString("F2");
        if (lives <= 0)
        {
            End();
        }
    }
    public void Begin()
    {
        lives = startingLives;
        collectabledPicked = 0;
        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].SetActive(true);
        }
        if (beginTimes == 0)
        {
            startTime = Time.time;
        }
        transform.position = startPosition.position;
        isPlaying = true;
        playButton.SetActive(false);
        beginTimes++;
    }
    public void TakeDamage(){
        lives -= 1;
    }
    void End()
    {
        timeTaken = Time.time - startTime;
        score = (collectablesTotal * 100);
        Debug.Log(score);
        isPlaying = false;
        playButton.SetActive(true);
        LeaderBoard.instance.SetLeaderboardEntry(Mathf.RoundToInt(score));
        beginTimes = 0;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("collectable"))
        {
            collectabledPicked++;
            collectablesTotal++;
            other.gameObject.SetActive(false);
            if(collectabledPicked == maxCollectables)
            {
                Begin();
            }
        }
    }
    public void PlayButtonReset()
    {
        collectablesTotal = 0;
    }
}
