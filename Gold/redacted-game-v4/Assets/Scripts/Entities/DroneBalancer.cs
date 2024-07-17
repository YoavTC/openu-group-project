using System;
using DG.Tweening;
using UnityEngine;

public class DroneBalancer : MonoBehaviour
{
    private bool flipping;
    
    private void Update()
    {
        if (flipping) return;
        
        float rotation = transform.eulerAngles.z;
        rotation = (rotation + 360) % 360;
        if (rotation > 60 && rotation < 120)
        {
            flipping = true;
            Flip();
        }
    }

    private void Flip()
    {
        transform.DORotate(new Vector3(0f, 0f, transform.rotation.z * -1), 2f).SetEase(Ease.InSine).OnComplete(() =>
        {
            flipping = false;
        });
    }
}
