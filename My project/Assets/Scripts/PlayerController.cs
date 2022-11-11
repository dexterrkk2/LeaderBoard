using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;
    float startTime;
    float timeTaken;

    int collectabledPicked;
    public int maxCollectables = 10;
    bool isPlaying;
    public GameObject playButton;
    public TextMeshProUGUI curTimeText;
    public GameObject[] collectables;
    public Transform startPosition;
    // Start is called before the first frame update
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlaying){
            return;
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);
        curTimeText.text = (Time.time - startTime).ToString("F2");
    }
    public void Begin()
    {
        collectabledPicked = 0;
        for(int i = 0; i< collectables.Length; i++)
        {
            collectables[i].SetActive(true);
        }
        startTime = Time.time;
        transform.position = startPosition.position;
        isPlaying = true;
        playButton.SetActive(false);
    }
    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        playButton.SetActive(true);
        LeaderBoard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("collectable"))
        {
            collectabledPicked++;
            other.gameObject.SetActive(false);
            if(collectabledPicked == maxCollectables)
            {
                End();
            }
        }
    }
}
