using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;

namespace PixelCrew.Components.LevelManegement
{
    public class RestartLevelComponent : MonoBehaviour
    {
        public void Restart()
        {
            var session = FindObjectOfType<GameSession>();
            session.LoadLastSave();

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
