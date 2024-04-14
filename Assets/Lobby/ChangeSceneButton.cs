using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private Scenes sceneToChangeTo;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneChanger.ChangeSceneTo(sceneToChangeTo);
        });
    }
}
