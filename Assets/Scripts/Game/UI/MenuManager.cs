using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class MenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Game"); // Название сцены игры
        }
    }
}

