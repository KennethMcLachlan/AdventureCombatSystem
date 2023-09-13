using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntBehaviour : GenericBehaviour
{
    public string tauntButton = "Taunt";

    private int tauntBool;
    private bool taunt = false;

    void Start()
    {
        tauntBool = Animator.StringToHash("Taunt");

        behaviourManager.SubscribeBehaviour(this);
    }

    void Update()
    {
        // Toggle fly by input, only if there is no overriding state or temporary transitions.
        if (Input.GetButtonDown(tauntButton) && !behaviourManager.IsOverriding()
            && !behaviourManager.GetTempLockStatus(behaviourManager.GetDefaultBehaviour))
        {
            taunt = !taunt;

            // Force end jump transition.
            behaviourManager.UnlockTempBehaviour(behaviourManager.GetDefaultBehaviour);

            // Player is Attacking
            if (taunt)
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

        taunt = taunt && behaviourManager.IsCurrentBehaviour(this.behaviourCode);

        behaviourManager.GetAnim.SetBool(tauntBool, taunt);
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
            taunt = false;
            behaviourManager.GetAnim.SetBool(tauntBool, taunt);
            behaviourManager.UnregisterBehaviour(this.behaviourCode);
        }
    }
}
