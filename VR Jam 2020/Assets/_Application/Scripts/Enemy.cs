using UnityEngine;
using UnityEngine.AI;

namespace VRJam2020
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        protected enum EnemyState
        {
            Idle,
            Following,
            Attacking,
        }

        [SerializeField] private Animator animator = null;

        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float navGoalRefreshInterval = 0.5f;
        [SerializeField] private float attackInterval = 3f;

        private Transform goal;
        private NavMeshAgent navMeshAgent;

        private EnemyState currentState;

        private float lastNavGoalRefresh;
        private float lastAttack;

        private LayerMask playerMask; // <_<

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            currentState = EnemyState.Idle;

            playerMask = LayerMask.GetMask("Player");
        }

        private void Update()
        {
            switch (currentState)
            {
            case EnemyState.Idle:
                break;
            case EnemyState.Following:
                UpdateWhileFollowing();
                break;
            case EnemyState.Attacking:
                UpdateWhileAttacking();
                break;
            }
        }

        private void UpdateWhileFollowing()
        {
            if (ReachedGoal())
            {
                animator.SetBool(AnimatorParameters.IsMoving, false);
                BeginAttack();
            }
            else
            {
                if (Time.time > lastNavGoalRefresh + navGoalRefreshInterval)
                    RefreshNavGoal();
            }
        }

        private void UpdateWhileAttacking()
        {
            RotateTowardsPlayer();

            if (Time.time > lastAttack + attackInterval)
                DetermineNextState();
        }

        public void DiscoverPlayer(Transform player)
        {
            goal = player;
            DetermineNextState();
        }

        public void TryToHitPlayer()
        {
            RaycastHit[] hits = Physics.RaycastAll(
                transform.position + Vector3.up,
                transform.forward,
                attackRange,
                playerMask);

            foreach (RaycastHit hit in hits)
            {
                var playerHealth = hit.collider.GetComponent<PlayerHealth>();
                if (playerHealth)
                    playerHealth.TakeDamage(1);
            }
        }

        private void DetermineNextState()
        {
            if ((goal.position - transform.position).magnitude < attackRange)
            {
                BeginAttack();
            }
            else
            {
                BeginFollowing();
            }
        }

        private void BeginFollowing()
        {
            currentState = EnemyState.Following;
            animator.SetBool(AnimatorParameters.IsMoving, true);

            RefreshNavGoal();
        }

        private void RefreshNavGoal()
        {
            navMeshAgent.destination = goal.position;
            lastNavGoalRefresh = Time.time;
        }

        private void BeginAttack()
        {
            currentState = EnemyState.Attacking;
            Attack();
        }

        private void Attack()
        {
            animator.SetTrigger(AnimatorParameters.IsAttacking);
            lastAttack = Time.time;
        }

        private bool ReachedGoal()
        {
            return !navMeshAgent.pathPending
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);
        }

        private void RotateTowardsPlayer()
        {
            Vector3 direction = (goal.position - transform.position).normalized;
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);
            float angle = Vector3.Angle(horizontalDirection, transform.forward);
            Quaternion lookRotation = Quaternion.LookRotation(horizontalDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, (Time.deltaTime * navMeshAgent.angularSpeed) / angle);
        }
    }
}
