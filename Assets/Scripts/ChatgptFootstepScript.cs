using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
  public AudioClip walkClip;
  public AudioClip runClip;
  public float stepIntervalWalk = 0.5f;
  public float stepIntervalRun = 0.3f;

  private AudioSource audioSource;
  private CharacterController characterController; // Or your movement reference
  private float stepTimer;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    characterController = GetComponent<CharacterController>();
  }

  void Update()
  {
    bool isMoving = characterController.velocity.magnitude > 0.1f && characterController.isGrounded;
    bool isRunning = Input.GetKey(KeyCode.LeftShift); // Customize for your game

    // Debug.Log($"Moving: {characterController.velocity.magnitude:F2} | Running: {Input.GetKey(KeyCode.LeftShift)}");

    if (isMoving)
    {
      stepTimer -= Time.deltaTime;

      if (stepTimer <= 0f)
      {
        PlayFootstep(isRunning);
        stepTimer = isRunning ? stepIntervalRun : stepIntervalWalk;
      }
    }
    else
    {
      stepTimer = 0f; // Reset when not moving
      audioSource.Stop();
    }
  }
  void PlayFootstep(bool running)
  {
    AudioClip clipToPlay = running ? runClip : walkClip;
    Debug.Log($"ClipToPlay: {clipToPlay.name} | Running: {running}");
    Debug.Log($"AudioSource isPlaying: {audioSource.isPlaying} | Clip: {audioSource.clip?.name}");

    if (audioSource.isPlaying)
    {
      if (audioSource.clip == clipToPlay)
      {
        return; // Prevent overlapping sounds
      }
      audioSource.Stop(); // Stop current sound if different
    }

    audioSource.clip = clipToPlay;
    audioSource.PlayOneShot(clipToPlay);
  }
}