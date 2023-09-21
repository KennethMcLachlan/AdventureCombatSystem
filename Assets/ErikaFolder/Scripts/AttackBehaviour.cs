using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class AttackBehaviour : GenericBehaviour
{
    //Set up core parts for generic behaviour
    //Set up the key
    //Set up animation
    //Correctly overriding and giving back when its completed

    public string attackButton = "Attack";

    private int attackBool;
    private bool attack = false;
    private bool _isAttacking = false;

    public GameObject particleAttack = null;

    public GameObject socket = null;

    public float createTime1 = 0.1f;
    public float createTime2 = 0.1f;
    public float createTime3 = 0.1f;

    public float attackTime1 = 0.8f;
    public float attackTime2 = 0.5f;
    public float attackTime3 = 1.0f;

    void Start()
    {
        attackBool = Animator.StringToHash("Attack");

        behaviourManager.SubscribeBehaviour(this);

        _isAttacking = false;
    }

    void Update()
    {
        // Toggle fly by input, only if there is no overriding state or temporary transitions.
        if (Input.GetButtonDown(attackButton) && !behaviourManager.IsOverriding()
            && !behaviourManager.GetTempLockStatus(behaviourManager.GetDefaultBehaviour))
        {
            attack = !attack;

            // Force end jump transition.
            behaviourManager.UnlockTempBehaviour(behaviourManager.GetDefaultBehaviour);

            // Player is Attacking
            if (attack)
            {
                behaviourManager.RegisterBehaviour(this.behaviourCode);
                StartCoroutine(ParticleAttackRoutine());
            }
            else
            {
                // Set camera default offset.
                behaviourManager.GetCamScript.ResetTargetOffsets();

                // Unregister this behaviour and set current behaviour to the default one.
                behaviourManager.UnregisterBehaviour(this.behaviourCode);
            }
        }

        attack = attack && behaviourManager.IsCurrentBehaviour(this.behaviourCode);

        behaviourManager.GetAnim.SetBool(attackBool, attack);
    }

    // This function is called when another behaviour overrides the current one.
    public override void OnOverride()
    {
    }

    public override void LocalFixedUpdate()
    {
        AttackManagement(behaviourManager.GetH, behaviourManager.GetV);
    }
    private void AttackManagement(float horizontal, float vertical)
    {
        if (behaviourManager.GetAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 &&
            !behaviourManager.GetAnim.IsInTransition(0))
        {
            attack = false;
            behaviourManager.GetAnim.SetBool(attackBool, attack);
            behaviourManager.UnregisterBehaviour(this.behaviourCode);
        }
    }

    IEnumerator ParticleAttackRoutine()
    {
        if (_isAttacking == false)
        {
            _isAttacking = true;

            yield return new WaitForSeconds(createTime1);
            GameObject particle1 = CreateParticleEffect();
            yield return new WaitForSeconds(attackTime1);
            MoveParticleEffect(particle1);

            yield return new WaitForSeconds(createTime2);
            GameObject particle2 = CreateParticleEffect();
            yield return new WaitForSeconds(attackTime2);
            MoveParticleEffect(particle2);

            yield return new WaitForSeconds(createTime3);
            GameObject particle3 = CreateParticleEffect();
            yield return new WaitForSeconds(attackTime3);
            MoveParticleEffect(particle3);

            _isAttacking = false;
        }

    }

    GameObject CreateParticleEffect()
    {
        GameObject obj = Instantiate(particleAttack);
        obj.transform.position = socket.transform.position;
        //obj.transform.parent = socket.transform;
        return obj;
    }

    void MoveParticleEffect (GameObject obj)
    {
        obj.GetComponent<ProjectileMovement>().enabled = true;
        //obj.transform.parent = null;
        obj.transform.localRotation = this.transform.localRotation;
    }
}
