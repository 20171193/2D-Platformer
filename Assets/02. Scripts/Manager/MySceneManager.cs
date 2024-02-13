using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadType
{
    Normal = 0,
    Fade
}

public class MySceneManager : MonoBehaviour
{
    [Header("로드할 씬의 로드 타입")]
    [SerializeField]
    LoadType curSceneLoadType;
    public LoadType CurSceneLoadType { get { return curSceneLoadType; } }

    public void SceneLoad(string name, LoadType loadType)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        StartCoroutine(LoadProcess(asyncOperation));
        asyncOperation.allowSceneActivation = false;


        SceneManager.LoadScene(name);
    }
    IEnumerator LoadProcess(AsyncOperation asyncOperation)
    {
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }


    public void SceneLoad(int index, LoadType loadType)
    {
        SceneManager.LoadScene(index);
    }
}
