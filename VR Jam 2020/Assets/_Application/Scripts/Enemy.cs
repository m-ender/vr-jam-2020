using DG.Tweening;
using System;
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

        private Transform goal;
        private NavMeshAgent navMeshAgent;

        private EnemyState currentState;

        private float lastNavGoalRefresh;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            currentState = EnemyState.Idle;
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

        }

        private void UpdateWhileAttacking()
        {
            throw new NotImplementedException();
        }

        public void DiscoverPlayer(Transform player)
        {
            goal = player;
            DetermineNextState();
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
            animator.SetTrigger(AnimatorParameters.IsAttacking);
        }

        private void Start()
        {
            DOTween.Sequence()
                .AppendCallback(() => navMeshAgent.destination = goal.position)
                .AppendInterval(0.5f)
                .SetLoops(-1);
        }
    }
}
