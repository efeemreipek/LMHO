using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PeopleState
{
    Laugh,
    Die,
    Dislike
};

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodEffect;
    [SerializeField] private GameObject head;

    public float laughMeter = 100f;
    public bool isMale;

    private Animator _animator;
    private AudioSource _audioSource;
    private Vector2Int laughBounds = new Vector2Int(100, 500);
    private PeopleState peopleState;

    private float audioVolume = 0.5f;
    public static int deathCount;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        SelectAttitude();
        PickRandomLaughMeter();

        deathCount = 0;
    }

    private void Update()
    {
        if (laughMeter < 0)
            laughMeter = 0;

        switch (deathCount)
        {
            case 0:
                GameManager.Instance.deathSlider.value = 0f;
                break;
            case 1:
                GameManager.Instance.deathSlider.value = 0.33f;
                break;
            case 2:
                GameManager.Instance.deathSlider.value = 0.66f;
                break;
            case 3:
                GameManager.Instance.deathSlider.value = 1f;
                break;
            default:
                break;
        }

        if(deathCount >= 3)
        {
            StartCoroutine(GameManager.Instance.EndGame());
        }

        if (GameManager.Instance.isGameOver)
            StartCoroutine(LowerVolume());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audioSource.volume = audioVolume;

            if(peopleState == PeopleState.Laugh && laughMeter > 0)
            {
                GameManager.Instance.canScore = true;

                if (isMale)
                {
                    _audioSource.clip = AudioManager.Instance.maleLaughs[Random.Range(0, AudioManager.Instance.maleLaughs.Count)];
                    _audioSource.Play();
                }
                else
                {
                    _audioSource.clip = AudioManager.Instance.femaleLaughs[Random.Range(0, AudioManager.Instance.femaleLaughs.Count)];
                    _audioSource.Play();
                }
                StartCoroutine(GameManager.Instance.AddScore());
            }
            else if (peopleState == PeopleState.Dislike)
            {
                _audioSource.clip = AudioManager.Instance.booEffect;
                _audioSource.Play();
            }    
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.LookAt(other.transform.position);

            if(peopleState == PeopleState.Dislike)
            {
                _animator.SetTrigger("No");

            }

            if(peopleState == PeopleState.Laugh)
            {
                if (laughMeter > 0)
                {
                    laughMeter--;
                    _animator.SetBool("IdleOrLaugh", false);
                }
                else
                {
                    peopleState = PeopleState.Die;
                    GameManager.Instance.canScore = false;
                    bloodEffect.Play();
                    head.SetActive(false);
                    _audioSource.clip = null;
                    _audioSource.PlayOneShot(AudioManager.Instance.deathEffect);
                    _animator.SetTrigger("Death");
                    deathCount++;
                    Destroy(gameObject, 3f);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(peopleState == PeopleState.Laugh && laughMeter > 0)
            {
                _animator.SetBool("IdleOrLaugh", true);
                GameManager.Instance.canScore = false;
                StopCoroutine(GameManager.Instance.AddScore());
            }
            if (_audioSource.isPlaying)
                StartCoroutine(LowerVolume());
        }
    }

    [ContextMenu("SelectAttitude")]
    private void SelectAttitude()
    {
        int dislikePercent = 30;
        int attitudePercent = Random.Range(0, 100);


        if (attitudePercent <= dislikePercent)
            peopleState = PeopleState.Dislike;
        else
            peopleState = PeopleState.Laugh;
    }

    [ContextMenu("PickRandomLaughMeter")]
    private void PickRandomLaughMeter()
    {
        if (peopleState == PeopleState.Laugh)
            laughMeter = Random.Range(laughBounds.x, laughBounds.y);
        else
            laughMeter = 0;
    }

    private IEnumerator LowerVolume()
    {
        for (float i = 0; i < 100; i++)
        {
            _audioSource.volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
