using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private GameObject dia;

    [SerializeField] private float speed = 0f;
    [SerializeField] private InputActionReference act;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference Roll;

    [SerializeField] private Sprite[] diceSides;
    [SerializeField] private TMP_Text Rolls;
    [SerializeField] private TMP_Text Score;
    [SerializeField] private TMP_Text timer;

    public GameObject Collectable;

    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;

    private int index;
    private float movementX;
    private float movementY;
    private int num = 5;
    private int scorenum = 0;
    private int finalSide = 0;
    private float time = 20f;
    private bool tim = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rend = GameObject.Find("dice").GetComponent<SpriteRenderer>();
        dia = GameObject.Find("Dialogue");

        dia.SetActive(false);

        act.action.performed += Say;
        moveInput.action.performed += move;
        Roll.action.performed += SliceTheDice;

        Roll.action.Enable();
        moveInput.action.Enable();
        rend.gameObject.SetActive(false);
    }

    private void SliceTheDice(InputAction.CallbackContext obj)
    {
        StartCoroutine("RollTheDice");
    }

    private void move(InputAction.CallbackContext obj)
    {
        Vector2 move = obj.ReadValue<Vector2>();

        movementX = move.x;
        movementY = move.y;
    }

    void FixedUpdate()
    {
        int x = 0;
        Movement();

        if (num == 0) 
        {
            SceneManager.LoadScene("GameOver");
        }
        else if (scorenum == 50) 
        {
            SceneManager.LoadScene("Win");
        }
        else if (timer.text == x.ToString("F2"))
        {
            tim = false;
            var clones = GameObject.FindGameObjectsWithTag("col");
            foreach (var clone in clones)
            {
                Destroy(clone);
            }
            finalSide = 0;
            time = 20f;
            timer.text = time.ToString("F2");
            timer.gameObject.SetActive(false);
        }
        else if (tim && finalSide == 3 || finalSide == 6)
        {
            time -= Time.deltaTime;
            timer.text = time.ToString("F2");
        }
    }
    private IEnumerator Spawn()
    {
        int counter = 0;
        int rand = Random.Range(1, 20);
        int x = 0;
        
        while (tim)
        {
            if (timer.text == x.ToString("F2"))
            {
                break;
            }
            Instantiate(Collectable, new Vector2(Random.Range(-5, 7), Random.Range(-3, 2)), Quaternion.identity);

            if (counter == rand)
            {
                var clones = GameObject.FindGameObjectsWithTag("col");
                foreach (var clone in clones)
                {
                    Destroy(clone);
                }
                yield return new WaitForSeconds(1f);
                rand = Random.Range(1, 20);
                counter = 0;
            }
            
            yield return new WaitForSeconds(.5f);
            counter++;
        }
    }
    private void Say(InputAction.CallbackContext obj) 
    {
        dia.SetActive(true);
        if (textComponent.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopCoroutine(TypeLine());
            textComponent.text = lines[index];

            rend.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "col")
        {
            scorenum += 1;
            Score.text = scorenum.ToString();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Table") 
        {
            act.action.Enable();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Table")
        {
            act.action.Disable();
        }
    }

    

    private void Movement()
    {
        Vector2 movement = new Vector2(movementX, movementY);
        
        movement.Normalize();

        anim.SetFloat("X", movementX);
        anim.SetFloat("Y", movementY);
        anim.SetFloat("speed", movement.sqrMagnitude);

        rb.AddForce(movement * speed);

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

        switch (finalSide)
        {
            case 1:
                num += 4;
                Rolls.text = num.ToString();
                break;
            case 2:
                num += 2;
                Rolls.text = num.ToString();
                break;
            case 4:
                num -= 3;
                Rolls.text = num.ToString();
                break;
            case 5:
                num -= 1;
                Rolls.text = num.ToString();
                break;
            case 3:
            case 6:
                tim = true;
                StartCoroutine(Spawn());
                break;
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dia.SetActive(false);
        }
    }
}
