using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private Animator animator;
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.OnPlayerKnockdown += OnPlayerKnockdown; //subscribe
        animator = GetComponentInChildren<Animator>();
    }

    private void OnPlayerKnockdown(object sender, PlayerHealth.OnPlayerKnockdownEventArgs e)
    {
        bool test = e.isKnockedDown;
        animator.SetBool("Knockdown", true);

    }
}
