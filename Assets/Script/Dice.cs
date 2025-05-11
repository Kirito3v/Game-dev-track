using System.Collections;
using UnityEngine;
using TMPro;

public class Dice : MonoBehaviour {

    [SerializeField] private Sprite[] diceSides;
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private TMP_Text Rolls;
    [SerializeField] private TMP_Text BD;
    
    private int num = 5;
    private int finalSide = 0;

    private void Start () 
    {
        rend = GetComponent<SpriteRenderer>();
	}

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine("RollTheDice");
        }
    }

    private IEnumerator RollTheDice()
    {
        num -= 1;
        Rolls.text = num.ToString();

        int randomDiceSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);

            rend.sprite = diceSides[randomDiceSide];

            yield return new WaitForSeconds(0.05f);
        }

        finalSide = randomDiceSide + 1;

        if (finalSide == 1 || finalSide == 2 || finalSide == 3) 
        {
            BD.text = "buff";
        }
        else 
        {
            BD.text = "debuff";
        }
    }
}
