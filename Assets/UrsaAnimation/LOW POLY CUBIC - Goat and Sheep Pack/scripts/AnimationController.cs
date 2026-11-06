using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Ursaanimation.CubicFarmAnimals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimationController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator animator;

        public float wanderRadius = 20f;

        // Максимальная скорость, соответствующая Blend = 4 (Run)
        public float maxSpeed = 5f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            StartCoroutine(Wander());
        }

        void Update()
        {
            // Реальная скорость движения агента
            float speed = agent.velocity.magnitude;

            // Переводим скорость в диапазон 0–4
            float blend = Mathf.InverseLerp(0f, maxSpeed, speed) * 4f;

            animator.SetFloat("Blend", blend);
        }

        IEnumerator Wander()
        {
            while (true)
            {
                float roll = Random.value;

                if (roll < 0.7f)
                {
                    agent.speed = 1.5f; // Walk
                }
                else if (roll < 0.95f)
                {
                    agent.speed = 3f; // Trot
                }
                else
                {
                    agent.speed = 4f; // Run
                }

                Vector3 point = Random.insideUnitSphere * wanderRadius;
                point += transform.position;

                NavMeshHit hit;

                if (NavMesh.SamplePosition(point, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }

                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }
        }
    }
}