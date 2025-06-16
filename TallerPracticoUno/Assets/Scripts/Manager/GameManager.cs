using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private InputReader input;
    //[SerializeField] private GameObject pauseMenu;
    void Start()
    {
        UIManager.Instance.Show<FlameHUDController>(ScreenType.FlameHUD);
        //input.PauseEvent += HandlePause;
        //input.ResumeEvent += HandleResume;
    }
    /*private void HandlePause()
    {
        pauseMenu.SetActive(true);
    }
    private void HandleResume()
    {
        pauseMenu.SetActive(false);
    }*/
}
