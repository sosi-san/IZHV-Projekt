using UnityEngine;
using UnityEngine.SceneManagement;

namespace Woska.Core
{
    /*
     * Tahle třída se zavolá před načtení první scény.
     * Zaručuje to aby se jako první scéna načetla scéna s indexem 0.
     * Což by měla být _Bootstrap scéna.
     */
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    break;
            
                default:
                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                    SceneManager.LoadScene(0, LoadSceneMode.Single);
                    break;
            }
        }
    }
}