using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class QueueDemo : MonoBehaviour
{
    public float Spamfrequency;
    private float _lastSpamTime = 0f;
    private bool _isSpaming;
    private Queue<string> _queue = new();

    public Button Enqueue;
    public Button Swpan;
    public Button Pause;
    public InputField Name;
    public TextMeshProUGUI Notice;
    public Text CountText;
    public GameObject EnemyPrefab;
    public GameObject BossPrefab;
    private int _countEnemy = 0;
    private int _countBoss = 0;
    public RectTransform panelRectTransform;
    private float _minX, _maxX, _minY, _maxY;

    void Start()
    {
        Vector3[] corners = new Vector3[4];
        panelRectTransform.GetWorldCorners(corners);
        // Gán giá trị min/max
        _minX = corners[0].x; // Góc dưới-trái X
        _maxX = corners[2].x; // Góc trên-phải X
        _minY = corners[0].y; // Góc dưới-trái Y
        _maxY = corners[1].y; // Góc trên-trái Y

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isSpaming)
        {
            return;
        }

        if (Time.time > _lastSpamTime + Spamfrequency)
        {
            if (_queue.TryDequeue(out string result))
            {
                float randomX = Random.Range(_minX, _maxX);
                float randomY = Random.Range(_minY, _maxY);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

                GameObject prefabToSpawn = null;

                if (result == "Boss")
                {
                    prefabToSpawn = BossPrefab;
                    Debug.Log("Spawning a Boss");
                }
                else
                {
                    prefabToSpawn = EnemyPrefab;
                    Debug.Log("Spawning an Enemy");
                }

                if (prefabToSpawn != null)
                {
                    GameObject newObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                    newObject.transform.SetParent(panelRectTransform);
                }
            }
            else
            {
                Debug.Log("Hàng đợi trống, dừng spawn.");
                PauseSpawn();
            }

            _lastSpamTime = Time.time;
        }
    }

    public void AddNPC(string Name)
    {
        _clearNotice();
        if (Name.ToLower() == "boss")
        {
            _queue.Enqueue("Boss");
            _setNotice(Color.green, "Đã thêm Boss vào hàng chờ");
            _countBoss++;
        }
        else { 
            _queue.Enqueue("Enemy");
            _setNotice(Color.green, "Đã thêm Enemy vào hàng chờ");
            _countEnemy++;
        }
        _setCountText();
        _clearInput();
    }
    public void StartSpawning()
    {
        _clearNotice();
        _isSpaming = true;
        _setNotice(Color.green, "Bắt đầu chạy NPC");
    }
    public void PauseSpawn()
    {
        _clearNotice();
        _isSpaming = false;
        _setNotice(Color.green, "Kết thúc chạy NPC");
    }

    private void _setNotice(Color color, string message)
    {
        Notice.color = color;
        Notice.text = message;
    }

    private void _clearNotice()
    {
        Notice.text = "";
    }
    private void _clearInput()
    {
        Name.text = "";
    }

    public void ClickButtonEnqueue()
    {
       var _textName = Name.text; // lấy dữ liệu input đầu vào
       AddNPC(_textName); // thêm danh sách NPC
    }

    private void _setCountText()
    {
        CountText.text = "Enemy: "+ _countEnemy +"\nBoss: "+_countBoss;
    }


    public void ClickButtonSpawn()
    {
        StartSpawning();
    }

    public void ClickButtonPasue()
    {
        PauseSpawn();
    }
}
