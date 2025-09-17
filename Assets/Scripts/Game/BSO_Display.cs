using UnityEngine;
using UnityEngine.UI;

public class BSO_Display : MonoBehaviour
{
    public Image[] strikeImages;
    public Image[] ballImages;
    public Image[] outImages;

    public Color strikeColor = Color.red;
    public Color ballColor = Color.green;
    public Color outColor = Color.yellow;
    public Color noCountColor = Color.gray;

    public void UpdateDisplay(int strikes, int balls, int outs)
    {
        for (int i = 0; i < strikeImages.Length; i++)
        {
            if (i < strikes)
            {
                strikeImages[i].color = strikeColor;
            }
            else
            {
                strikeImages[i].color = noCountColor;
            }
        }

        for (int i = 0; i < ballImages.Length; i++)
        {
            if (i < balls)
            {
                ballImages[i].color = ballColor;
            }
            else
            {
                ballImages[i].color = noCountColor;
            }
        }

        for (int i = 0; i < outImages.Length; i++)
        {
            if (i < outs)
            {
                outImages[i].color = outColor;
            }
            else
            {
                outImages[i].color = noCountColor;
            }
        }
    }
}