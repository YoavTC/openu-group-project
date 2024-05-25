using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class FadeIntro : MonoBehaviour
{
    [SerializeField] private Image fade;

    private void Start()
    {
        fade.DOFade(0, 5f).SetDelay(1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }
}
