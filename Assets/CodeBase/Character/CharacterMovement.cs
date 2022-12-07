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
        private readonly Transform characterMovementPosition;
        private readonly float tweenDuration = 1.5f;

        [Inject]
        public CharacterMovement(ICharacterPool charactersPool, EventReferer eventReferer,
            [Inject(Id = "CharacterMovementPosition")]
            Transform characterMovementPosition)
        {
            this.charactersPool = charactersPool;
            this.characterMovementPosition = characterMovementPosition;
        }

        public async UniTask<bool> Move(GameObject character, Transform parent = null)
        {
            character.GetComponent<CharacterAnimations>().PlayAnimation(isWalking: true);
            character.transform.DOLocalMoveX(0, tweenDuration).SetEase(Ease.Linear);
            await UniTask.Delay((int) tweenDuration * 1100);
            return true;
        }

        public Tween Stop(GameObject character)
        {
            IdleCharacter(character);
            return character.transform.DOBlendableLocalRotateBy(new Vector3(0, 90, 0), 1f);
        }

        public void ReparentCharacter(GameObject character)
        {
            character.transform.SetParent(characterMovementPosition.GetChild(0));
            character.transform.localPosition = Vector3.zero;
            character.transform.SetParent(characterMovementPosition.GetChild(1));
        }

        private void IdleCharacter(GameObject character) =>
            character.GetComponent<CharacterAnimations>().PlayAnimation(isWalking: false);
    }
}