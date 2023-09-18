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

    [SerializeField]
    private GameObject _particleAttack;

    [SerializeField]
    private GameObject _socket;
    
    void Start()
    {
        attackBool = Animator.StringToHash("Attack");

        behaviourManager.SubscribeBehaviour(this);
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
    void AttackManagement(float horizontal, float vertical)
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
        yield return new WaitForSeconds(0.25f);
        GameObject obj = Instantiate(_particleAttack);
        obj.transform.position = _socket.transform.position;    
    }
}
