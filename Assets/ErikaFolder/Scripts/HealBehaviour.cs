using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBehaviour : GenericBehaviour
{
    public string healButton = "Heal";

    private int healBool;
    private bool heal = false;

    public bool healTrigger;

    void Start()
    {
        healBool = Animator.StringToHash("Heal");

        behaviourManager.SubscribeBehaviour(this);
    }

    void Update()
    {
        // Toggle fly by input, only if there is no overriding state or temporary transitions.
        if ((Input.GetButtonDown(healButton) || healTrigger == true) && !behaviourManager.IsOverriding()
            && !behaviourManager.GetTempLockStatus(behaviourManager.GetDefaultBehaviour))
        {
            heal = !heal;
            healTrigger = false;

            behaviourManager.UnlockTempBehaviour(behaviourManager.GetDefaultBehaviour);

            // Player is Attacking
            if (heal)
            {
                // Register this behaviour.
                behaviourManager.RegisterBehaviour(this.behaviourCode);
            }
            else
            {
                // Set camera default offset.
                behaviourManager.GetCamScript.ResetTargetOffsets();

                // Unregister this behaviour and set current behaviour to the default one.
                behaviourManager.UnregisterBehaviour(this.behaviourCode);
            }
        }

        heal = heal && behaviourManager.IsCurrentBehaviour(this.behaviourCode);

        behaviourManager.GetAnim.SetBool(healBool, heal);
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
            heal = false;
            behaviourManager.GetAnim.SetBool(healBool, heal);
            behaviourManager.UnregisterBehaviour(this.behaviourCode);
        }
    }
}
