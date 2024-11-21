using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    public List<TextMeshProUGUI> names;
    [SerializeField]
    public List<TextMeshProUGUI> scores;
    public TMP_InputField name;
    public TextMeshProUGUI score;
    

    // UpdateScore is called when the button is pressed
    public void UpdateScore()
    {
            for (int i=0;i<names.Count;i++)
            {
                if (name.text.Equals(names[i].text))
                {
                    scores[i].text=score.text;
                    break;
                }
                else if (string.IsNullOrEmpty(names[i].text))
                {
                    names[i].text=name.text;
                    scores[i].text=score.text;
                    break;
                }
            }
              
        }
}

