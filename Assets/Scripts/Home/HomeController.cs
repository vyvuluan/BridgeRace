using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    [SerializeField] private List<PlayerHome> players;
    private int characterIndex = 0;

    public void NextCharacter()
    {
        characterIndex++;
        characterIndex = characterIndex > 3 ? 0 : characterIndex;
        EnablePlayer(characterIndex);
    }
    public void PreCharacter()
    {
        characterIndex--;
        characterIndex = characterIndex < 0 ? 3 : characterIndex;
        EnablePlayer(characterIndex);
    }
    public void StartGame()
    {
        GameObject go = new(Constants.ParamTag)
        {
            tag = Constants.ParamTag
        };
        Parameter parameter = go.AddComponent<Parameter>();
        parameter.color = players[characterIndex].BrickColor;
        DontDestroyOnLoad(parameter);
        SceneManager.LoadScene(Constants.GameplayScene);
    }
    public void EnablePlayer(int index)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (i == index)
            {
                players[i].gameObject.SetActive(true);
            }
            else players[i].gameObject.SetActive(false);
        }
    }

}
