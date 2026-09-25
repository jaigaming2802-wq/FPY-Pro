using UnityEngine; 
 
public class DeathState : PlayerState 
{ 
    [SerializeField] private float respawnDelay = 0.8f; 
 
    private bool animationStarted; 
    private bool respawned; 
    private float respawnTimer; 
 
    public DeathState( 
        PlayerMovement player, 
        PlayerStateMachine stateMachine) 
        : base(player, stateMachine) 
    { 
    } 
 
    public override void Enter() 
    { 
        player.Stop(); 
        player.SetAnimationSpeed(0f); 
 
        animationStarted = false; 
        respawned = false; 
        respawnTimer = 0f; 
 
        player.anim.ResetTrigger("Death"); 
        player.anim.SetTrigger("Death"); 
    } 
 
    public override void Update() 
    { 
        AnimatorStateInfo state = 
            player.anim.GetCurrentAnimatorStateInfo(0); 
 
        if (state.IsName("Death")) 
        { 
            animationStarted = true; 
        } 
 
        if (animationStarted && 
            state.IsName("Death") && 
            state.normalizedTime >= 1f && 
            !respawned) 
        { 
            respawnTimer += Time.deltaTime; 
 
            if (respawnTimer >= respawnDelay) 
            { 
                respawned = true; 
 
                Respawn(); 
            } 
        } 
    } 
 
    private void Respawn() 
    { 
        Vector3 respawnPosition = 
            CheckpointManager.Instance.GetCheckpointPosition(); 
 
        Quaternion respawnRotation = 
            CheckpointManager.Instance.GetCheckpointRotation(); 
 
        player.transform.position = respawnPosition; 
        player.transform.rotation = respawnRotation; 
 
        PlayerHealth health = 
            player.GetComponent<PlayerHealth>(); 
 
        if (health != null) 
        { 
            health.ResetHealth(); 
        } 
 
        player.anim.ResetTrigger("Death"); 
        player.anim.Play("Idle", 0, 0f); 
 
        EnemyManager.Instance.ResetAllEnemies(); 
 
        player.StateMachine.ChangeState( 
            new IdleState( 
                player, 
                player.StateMachine)); 
    } 
 
    public override void FixedUpdate() 
    { 
        player.Stop(); 
    } 
 
    public override void Exit() 
    { 
    } 
} 