using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLaptopController : MenuController
{
    public bool IsActive;

    [SerializeField] private TMP_Text startGameText;
    [SerializeField] private TMP_Text[] menuText;

    public void NewGame()
    {
        if (menuText[0].GetComponentInChildren<TMP_InputField>().text == "yes")
        {
            menuText[0].gameObject.SetActive(false);
            startGameText.gameObject.SetActive(true);
        }

        SetText(0);
    }
    public void StartGame()
    {
        if (startGameText.GetComponentInChildren<TMP_InputField>().text == "black")
        {
            //begin game from black
            SceneManager.LoadScene(1);
        }
        if (startGameText.GetComponentInChildren<TMP_InputField>().text == "white")
        {
            //begin game from white
            SceneManager.LoadScene(1);
        }

        startGameText.GetComponentInChildren<TMP_InputField>().text = "";
        /*startGameText.GetComponentInChildren<TMP_InputField>()
                    .placeholder.GetComponent<TMP_Text>().text = "¬вед≥ть \"black\" або \"white\"";*/
    }
    public void LoadGame()
    {
        if (menuText[1].GetComponentInChildren<TMP_InputField>().text == "yes")
        {
            menuText[0].gameObject.SetActive(false);
            SceneManager.LoadScene(1);
            //startGameText.gameObject.SetActive(true);
        }

        SetText(1);
    }

    private void OnMouseDown()
    {
        Camera.main.GetComponent<Animator>().Play("GoToLaptop");
        StartCoroutine(Culldown(menuText[0].gameObject));
    }

    public void CloseMenu()
    {
        if (menuText[2].GetComponentInChildren<TMP_InputField>().text == "yes")
        {
            Camera.main.GetComponent<Animator>().Play("GoFromLaptop");
            menuText[0].gameObject.SetActive(false);
        }

        SetText(2);
    }

    private IEnumerator Culldown(GameObject activeGO)
    {
        yield return new WaitForSeconds(2f);
        activeGO.SetActive(true);
    }
    private void SetText(int index)
    {
        menuText[index].GetComponentInChildren<TMP_InputField>().text = "";
        /*menuText[index].GetComponentInChildren<TMP_InputField>()
                    .placeholder.GetComponent<TMP_Text>().text = "¬вед≥ть \"yes\" €кщо так";*/
    }
}
