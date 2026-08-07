using UnityEngine;

public class VignetteView : MonoBehaviour
{
    /// <summary>
    /// 1=Construction 2=Damaged
    /// </summary>
    [SerializeField] private Sprite[] vignettes;
    public void Init(int id)
    {
        GetComponent<SpriteRenderer>().sprite = vignettes[id];
    }
}
