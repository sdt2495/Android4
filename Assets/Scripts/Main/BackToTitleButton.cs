using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitleButton : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}