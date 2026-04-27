using UnityEngine;

public class Auth_CivChoice : MonoBehaviour
{
    public enum civList { Terrakins=0,Ascend=1,Wrecks=2}
    civList civDisplayed=civList.Terrakins;

    [SerializeField] private Transform[] content;

    private void DisplaySelectedCiv(int index)
    {
        foreach (var item in content) { item.gameObject.SetActive(false); }

        civDisplayed = (civList)index;
        content[index].gameObject.SetActive(true);
    }
    public void OnMoveLeft()
    {
        Debug.Log("Test left");
        int index = (int)civDisplayed;
        index-=1;
        if (index < 0) index = 2;

        DisplaySelectedCiv(index);
    }
    public void OnMoveRight()
    {
        int index = (int)civDisplayed;
        index++;
        if (index > 2) index = 0;

        DisplaySelectedCiv(index);
    }
}
