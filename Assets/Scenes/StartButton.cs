using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button startBtn;
    public string sceneName;
    void Start()
    {
        startBtn = GetComponent<Button>();
        startBtn.onClick.AddListener(OnClickStart);
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene(sceneName);
    }
}
