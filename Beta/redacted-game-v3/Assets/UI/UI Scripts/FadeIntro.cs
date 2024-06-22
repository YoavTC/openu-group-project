using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Image = UnityEngine.UI.Image;

public class FadeIntro : MonoBehaviour
{
    [SerializeField] private Image fade;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        // fade.DOFade(0, 5f).SetDelay(1f).SetEase(Ease.InOutSine).OnComplete(() =>
        // {
        //     SceneManager.LoadScene(1);
        // });
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += source =>
        {
            fade.DOFade(0, 0.75f).SetDelay(1f).OnComplete(() =>
            {
                source.Play();
            });
        };
        videoPlayer.loopPointReached += source =>
        {
            fade.DOFade(1, 2f).SetDelay(1f).OnComplete(() =>
            {
                SceneManager.LoadScene(1);
            });
        };
    }
}
