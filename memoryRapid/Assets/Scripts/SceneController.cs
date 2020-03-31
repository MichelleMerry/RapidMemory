using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 4f;
    public const float offsetY = 5f;


    [SerializeField] private MainCard orignalCard;
    [SerializeField] private Sprite[]images;

    private void Start()
    {
        Vector3 startPos = orignalCard.transform.position; 

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        numbers = ShuffleArray(numbers);


        for(int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MainCard card;
                if (i == 0 && j == 0)
                {
                    card = orignalCard;
                }
                else
                {
                    card = Instantiate(orignalCard) as MainCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.ChangeSprite(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);

            }
        }

    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;

        }
        return newArray;
    }
    //.................................

    private MainCard _FirstReveal;
    private MainCard _SecondReveal;

    private int _score = 0;
    [SerializeField] private TextMesh ScoreText;

    public bool canreveal
    { 
        get { return _SecondReveal == null; }
    }

    public void Cardrevealed(MainCard card) 
    {
    if(_FirstReveal == null)
        {
            _FirstReveal = card;
        }
    else
        {
            _SecondReveal = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (_FirstReveal.id == _SecondReveal.id)
        {
            _score ++;
            ScoreText.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            _FirstReveal.Unreveal();
            _SecondReveal.Unreveal();
        }

        _FirstReveal = null;
        _SecondReveal = null;
    }

    public void Restart ()
    {
        SceneManager.LoadScene("MainGame");
    }
}
