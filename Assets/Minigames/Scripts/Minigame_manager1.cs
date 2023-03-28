using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame_manager1 : MonoBehaviour
{
    bool running = false;

    bool game_started = false;
    bool game_over = false;
    
    public GameObject camera;

    public GameObject player;
    public Animator player_animator;

    public GameObject toy;
    public Animator toy_animator;

    public GameObject laser;
    public ParticleSystem blood_splash;
    public GameObject blood;

    AudioSource source;
    public AudioClip step;
    public AudioClip shooting;
    public AudioClip hit;
    public AudioClip fall;

    float steps_counter;
    public GameObject ui_start;


    void Start()
    {
        blood.SetActive(false);
        source = GetComponent<AudioSource>();
        ui_start.SetActive(true); 
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            player.transform.position -= new Vector3(0.5f * Time.deltaTime, 0, 0);
            camera.transform.position -= new Vector3(0.5f * Time.deltaTime, 0, 0);

            steps_counter += Time.deltaTime;
            if (steps_counter  > .25f)
            {
                steps_counter = 0;
                source.PlayOneShot(step);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !game_started)
        {
            running = true;
            game_started = true;
            ui_start.SetActive(false);
            player_animator.SetTrigger("run");
            StartCoroutine(Sing());
        }
        if (Input.GetKeyDown(KeyCode.Space) && game_over)
        {
            SceneManager.LoadScene("MiniGame");
         
        }

        if (Input.GetKey(KeyCode.W))
        {
            running = false;
            player_animator.speed = 0;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            running = true;
            player_animator.speed = 1;
        }
    }



    IEnumerator Sing()
    {
        toy.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(4.5f);
        toy_animator.SetTrigger("look");
        //animator -> look
        yield return new WaitForSeconds(2);
        //check player move
        if (running)
        {
            Debug.Log("shoot");
            GameObject new_laser = Instantiate(laser);
            new_laser.transform.position = toy.transform.GetChild(0).transform.position;
            game_over = true;
            source.PlayOneShot(shooting);
        }

        yield return new WaitForSeconds(2);
        toy_animator.SetTrigger("idle");
        yield return new WaitForSeconds(1);
        toy.GetComponent<AudioSource>().Stop();
        if (!game_over) StartCoroutine(Sing());


    }

    public void HitPlayer()
    {
        running = false;
        player_animator.SetTrigger("idle");
        player.GetComponent<Rigidbody>().velocity = new Vector3(2, 0, 0);
        player.GetComponent<Rigidbody>().angularVelocity = new Vector3(3, 0, 0);
        camera.GetComponent<Animator>().Play("camera_lose");
        blood_splash.Play();
        StartCoroutine(ShowBlood());
        source.PlayOneShot(hit);
    }
    IEnumerator ShowBlood()
    {
      
        yield return new WaitForSeconds(1f);
        source.PlayOneShot(fall);
        blood.SetActive(true);
        blood.transform.position = new Vector3(player.transform.position.x + 0.2f, player.transform.position.y, player.transform.position.z );

    }
}
