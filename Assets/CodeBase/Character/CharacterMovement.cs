using CodeBase.Food;
using CodeBase.Infrastructure.Events;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Character
{
    public class CharacterMovement
    {
        private readonly ICharacterPool charactersPool;
        private readonly Transform characterMovement;

        [Inject]
        public CharacterMovement(ICharacterPool charactersPool, EventReferer eventReferer,
            [Inject(Id = "CharacterMovementPosition")] Transform characterMovement)
        {
            this.charactersPool = charactersPool;
            this.characterMovement = characterMovement;
        }

        public async UniTask<bool> MoveCharacter(GameObject character, Transform parent = null)
        {
            character.GetComponent<CharacterAnimations>().PlayAnimation(isWalking: true);
            var tween = character.transform.DOLocalMoveX(0, 1.5f).SetEase(Ease.Linear);
            await UniTask.Delay(2000);
            return true;
        }

        public void IdleCharacter(GameObject character) => 
            character.GetComponent<CharacterAnimations>().PlayAnimation(isWalking: false);
    }
}