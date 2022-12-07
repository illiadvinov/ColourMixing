using CodeBase.Character;
using CodeBase.Infrastructure.Common.StateMachine;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.GameStateMachine.States
{
    public class NextLevelState : IState
    {
        private readonly IStateMachine stateMachine;
        private readonly ICharacterPool charactersPool;
        private readonly CharacterMovement characterMovement;
        private readonly Transform characterMovementPosition;
        private readonly GameObject drinkBubble;

        [Inject]
        public NextLevelState(IStateMachine stateMachine, ICharacterPool charactersPool,
            CharacterMovement characterMovement,
            [Inject(Id = "CharacterMovementPosition")] Transform characterMovementPosition,
            [Inject(Id = "DrinkBubble")] GameObject drinkBubble)
        {
            this.stateMachine = stateMachine;
            this.charactersPool = charactersPool;
            this.characterMovement = characterMovement;
            this.characterMovementPosition = characterMovementPosition;
            this.drinkBubble = drinkBubble;
        }

        public async void Enter()
        {
            drinkBubble.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InCubic);
            GameObject character = charactersPool.GetActiveCharacter();
            character.transform.DOBlendableLocalRotateBy(new Vector3(0, -90, 0), 1f);

            character.transform.SetParent(characterMovementPosition.GetChild(2));
            await characterMovement.Move(character);
            
            charactersPool.Add(character);
            stateMachine.Enter<StartGameState>();
        }

        public void Exit()
        {
        }
    }
}