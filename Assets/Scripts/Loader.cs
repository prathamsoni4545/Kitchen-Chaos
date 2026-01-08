using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{

    public enum Scene
    {
       MainMenuscene,
        GameScene,
        LoadingScene
    }


    private static Scene tragetscene;



    public static void Load(Scene targetScene)
    {
        Loader.tragetscene = targetScene;


        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
        public static void LoaderCallback()
    { 
        SceneManager.LoadScene(tragetscene.ToString());
    }
}
