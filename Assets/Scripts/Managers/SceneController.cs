using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
   public static SceneController SceneInstance;

    void Awake()
    {
        Singleton();
    }


    void Singleton()
    {
        if (SceneInstance == null)
        {
            SceneInstance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadSomeScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }


}
