using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ButtonsManager : MonoBehaviour
{
    public Image imageToScale;
    private bool isZoomOut = false;
    private bool isThere = true;
    private bool isFaded = false;
    private Vector3 initialPosition;
    private Vector2 initialSize;
    private Color initialColor;
    private float bounceHeight = 5f;
    private float bounceSpeed = 5.0f;
    private float bounceDuration = 2.0f;

    private void Awake()
    {
       // initialSize = imageToScale.rectTransform.sizeDelta;
       initialSize = imageToScale.transform.localScale;
        initialPosition = imageToScale.rectTransform.position;
        initialColor = imageToScale.color;

    }

    public void Zoom()
    {
        CheckIsThere("zoom");
        float zoomVal = 0;
        float targetScale = isZoomOut ? 1.0f : zoomVal;
        imageToScale.transform.DOScale(targetScale, 0.25f);
        isZoomOut = !isZoomOut;
        isThere = !isThere;
       
    }

    public void CheckIsThere(string except) // in case an effect button is clicked while the image has not been returned 
                                            // like clicking zoom button only once
                                              // aka for when leaf is not visible
    {
        if (isThere == false)
        {
            if (except == "zoom")
            {
                if (isFaded)
                {
                    imageToScale.DOFade(1f, 0f);
                    imageToScale.transform.DOScale(0, 0f);
                    isZoomOut = true;
                    isFaded = false;
                }

            }

            if (except == "fade")
            {
                if (isZoomOut)
                {
                    imageToScale.DOFade(0f, 0f);
                    imageToScale.transform.DOScale(1.0f, 0f);
                    isZoomOut = false;
                    isFaded = true;

                }

            }
            
            if (except == "none")
            {
                    imageToScale.transform.DOScale(1.0f, 0f);
                    isZoomOut = false;
                    imageToScale.DOFade(1f, 0f);
                    isFaded = false;
                    isThere = true;


            }

        }

        else
        {
            return;
        }
    }

    public void Fade()
    {

        CheckIsThere("fade");

        if (!isFaded) {
            imageToScale.DOFade(0f, 1.5f);
            isFaded = true;
            isThere = false;
        }

        else
        {
            imageToScale.DOFade(1f, 1.5f);
            isFaded = false;
            isThere = true;
        }

    }

    public void Flash()
    {
        CheckIsThere("none");
        Sequence flashSequence = DOTween.Sequence();
        flashSequence.Append(imageToScale.DOColor(new Color(initialColor.r, initialColor.g, initialColor.b, 0.1f), 0.2f));
        flashSequence.Append(imageToScale.DOColor(initialColor, 0.2f));
        flashSequence.SetLoops(2, LoopType.Restart);
    }


    public void Tada()
    {
        CheckIsThere("none");
        Sequence tadaSequence = DOTween.Sequence();
  
        tadaSequence.Append(imageToScale.transform.DORotate(new Vector3(0f, 0f, 5f), 0.1f));
        tadaSequence.Append(imageToScale.transform.DORotate(new Vector3(0f, 0f, -5f), 0.1f));
        tadaSequence.SetLoops(5, LoopType.Restart);


    }

    public void Shake()
    {
        CheckIsThere("none");
        Sequence shakeSequence = DOTween.Sequence();

        shakeSequence.Append(imageToScale.transform.DOLocalMoveX(initialPosition.x - 10f, 0.1f));
        shakeSequence.Append(imageToScale.transform.DOLocalMoveX(initialPosition.x + 10f, 0.1f));
        shakeSequence.SetLoops(5, LoopType.Restart);
    }

    public void Pulse()
    {
        CheckIsThere("none");

        Sequence pulseSequence = DOTween.Sequence();
        pulseSequence.Append(imageToScale.transform.DOScale(initialSize * 0.9f, 0.5f));
        pulseSequence.Join(imageToScale.DOColor(new Color(initialColor.r, initialColor.g, initialColor.b, 0.7f), 0.5f));

        pulseSequence.Append(imageToScale.transform.DOScale(initialSize, 0.3f));
        pulseSequence.Join(imageToScale.DOColor(initialColor, 0.3f));
    }

    public void Bounce()
    {
        CheckIsThere("none");
        StartCoroutine(BounceAnimation());
    }

    private System.Collections.IEnumerator BounceAnimation()
    {
        float startTime = Time.time;
        initialPosition = imageToScale.rectTransform.position;

        while (Time.time - startTime < bounceDuration)
        {
         
            float t = (Time.time - startTime) / bounceDuration;
            float yPos = initialPosition.y + Mathf.Sin(t * Mathf.PI * bounceSpeed) * bounceHeight;
            imageToScale.rectTransform.position = new Vector3(imageToScale.rectTransform.position.x, yPos, imageToScale.rectTransform.position.z);

            yield return null;
        }

        imageToScale.rectTransform.position = initialPosition;
    }

}
