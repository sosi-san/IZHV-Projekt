using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Woska.Core;

namespace Woska.Bakalarka
{
    public class App : Singleton<App>
    {
        public NetworkController NetworkController { get; private set; }
        public GameManager GameManager { get; private set; }

        private void Awake()
        {
            FindBaseScripts();
            this.MyCoroutine(LoadMainMenu());
        }
        private void FindBaseScripts()
        {
            NetworkController = gameObject.GetOrAddComponent<NetworkController>();
            GameManager = gameObject.GetOrAddComponent<GameManager>();
        }
        IEnumerator LoadMainMenu()
        {
            yield return SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive);
            NetworkController.FindStatusFields();
        }

        public void QuitGame()
        {
            AppHelper.Quit();
        }
    }
    public enum SceneIndexes
    {
        _BOOT,
        MAIN_MENU,
        MINIGAME,

        NUMBER_OF_SCENES
    }
}