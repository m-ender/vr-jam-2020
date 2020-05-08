using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace VRJam2020
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyHealth : Health
    {
        [SerializeField] private Renderer modelRenderer = null;

        [SerializeField] private Color damageHighlightColor = Color.red;
        [SerializeField] private float damageHighlightTime = 0.2f;
        [SerializeField] private float particleEffectTime = 5f;
        [SerializeField] private ParticleSystem emberEffect = null;
        [SerializeField] private Material dissolveMaterial = null;
        [SerializeField] private Material fireDissolveMaterial = null;

        private Enemy enemy;
        private NavMeshAgent navMeshAgent;
        private Animator animator;

        private ParticleSystem ps;

        private int shaderProperty;

        protected override void Die()
        {
            StartCoroutine(DestroyColliderNextFrame());
            if (oneHitKill)
                AnimateOneHitDeath();
            else
                AnimateDeath();
        }

        private IEnumerator DestroyColliderNextFrame()
        {
            yield return null;
            Destroy(GetComponent<Collider>());
        }

        private void AnimateDeath()
        {
            emberEffect.gameObject.SetActive(false);
            enemy = GetComponent<Enemy>(); navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            enemy.enabled = false; navMeshAgent.enabled = false; animator.enabled = false;
            modelRenderer.material = dissolveMaterial;
            shaderProperty = Shader.PropertyToID("_cutoff");
            ps = GetComponentInChildren<ParticleSystem>();
            
            var main = ps.main;
            main.duration = particleEffectTime;

            ps.Play();

            Sequence s = DOTween.Sequence();

            s.Append(modelRenderer.material.DOFloat(1, shaderProperty, particleEffectTime));
            s.AppendCallback(() => Destroy(gameObject));
        }

        private void AnimateOneHitDeath()
        {
            enemy = GetComponent<Enemy>(); navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            enemy.enabled = false; navMeshAgent.enabled = false; animator.enabled = false;
            modelRenderer.material = fireDissolveMaterial;
            shaderProperty = Shader.PropertyToID("_cutoff");
            ps = GetComponentInChildren<ParticleSystem>();

            var main = ps.main;
            main.duration = particleEffectTime;

            ps.Play();

            Sequence s = DOTween.Sequence();

            s.Append(modelRenderer.material.DOFloat(1, shaderProperty, particleEffectTime));
            s.AppendCallback(() => Destroy(gameObject));
        }

        protected override void ShowDamageFeedback()
        {
            DOTween.Sequence()
                .Append(modelRenderer.material.DOColor(damageHighlightColor, damageHighlightTime / 2))
                .Append(modelRenderer.material.DOColor(Color.white, damageHighlightTime / 2));
        }
    }
}