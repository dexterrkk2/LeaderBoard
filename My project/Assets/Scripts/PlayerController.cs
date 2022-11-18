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
    float timeCheck;
    public float invincibleDelay;
    float lastDamageTime;
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
    public bool playButtonPressed;
    public TextMeshProUGUI LivesText;
    public TextMeshProUGUI scoreText;
    public MeshRenderer mesh;
    public GameObject gfx;
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
        LivesText.text = "Lives:" + lives;
        transform.rotation = Quaternion.LookRotation(rig.velocity);
    }
    public void Begin()
    {
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
        lives += 2;
    }
    public void TakeDamage(){
        timeCheck = Time.time - startTime;
        if (timeCheck-lastDamageTime> invincibleDelay) {
            lives -= 1;
            lastDamageTime = Time.time - startTime;
            Debug.Log(lives);
            FlashDamage();
        }
    }
    void FlashDamage()
    {
        StartCoroutine(DamageFlashCoRoutine());
        IEnumerator DamageFlashCoRoutine()
        {

            Color defaultColor = mesh.material.color;
            mesh.material.color = Color.black;
            yield return new WaitForSeconds(invincibleDelay);
            mesh.material.color = defaultColor;
        }
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
            scoreText.text = "Score:" + collectablesTotal * 100;
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
        lives = startingLives;
        timeCheck = 0;
        lastDamageTime = 0;
    }
}
