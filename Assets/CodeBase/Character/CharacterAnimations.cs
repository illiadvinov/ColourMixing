using System;
using UnityEngine;

namespace CodeBase.Character
{
    public class CharacterAnimations : MonoBehaviour
    {
        private Animator animator;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

        private void Awake() => 
            animator = GetComponent<Animator>();

        public void PlayAnimation(bool isWalking) => 
            animator.SetBool(IsWalking, isWalking);
    }
}