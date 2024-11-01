using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  public CharacterController controller;

  public float speed = 8f;
  public float gravity = -9.81f;
  public bool chaveUm = false;

  Vector3 velocity;

  public Transform GroundCheck;
  public float groundDistance = 0.4f;
  public LayerMask groundMask;

  public int hp = 3;
  public float stunDuration = 0.2f;
  public float deathDuration = 5f;
  public GameObject hitParticle;

  private bool locked = false;
  private bool dead = false;

  public Image[] hpImages;

  bool isGrounded;

  void Update()
  {
    isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance);

    if(isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 move = transform.right*x + transform.forward*z;

    controller.Move(move * speed*Time.deltaTime);

    velocity.y += gravity*Time.deltaTime;

    controller.Move(velocity*Time.deltaTime);
  }

  public void GetHit(int damage, Vector3 particlePos)
  {
    if (!dead)
    {
      locked = true;
      hp -= damage;
      Instantiate(hitParticle, particlePos, Quaternion.identity);
      CancelInvoke("DealDamage");
      CancelInvoke("Unlock");

      for(int i = 0 ; i < hpImages.Length; i++) hpImages[i].color = Color.black;
      for(int i = 0 ; i < hp; i++) hpImages[i].color = Color.white;

      if (hp <= 0)
      {
        //rig.constraints = RigidBodyContraints.Nome;
        //rig.AddTorque(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
        dead = true;
        Invoke("Reload", deathDuration);
      }
      else
      {
        Invoke("Unlock", stunDuration);
      }
    }
  }

  void Unlock()
  {
    locked = false;
  }

  void Reload()
  {
    SceneManager.LoadScene(0);
  }
}
