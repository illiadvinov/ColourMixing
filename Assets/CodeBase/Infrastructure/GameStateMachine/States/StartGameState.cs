using CodeBase.Character;
using CodeBase.Drink;
using CodeBase.Food;
using CodeBase.Infrastructure.Common.StateMachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.GameStateMachine.States
{
    public class StartGameState : IState
    {
        private readonly IStateMachine stateMachine;
        private readonly ICharacterPool charactersPool;
        private readonly CharacterMovement characterMovement;
        private readonly LevelManager.LevelManager levelManager;
        private readonly Transform characterMovementPosition;
        private readonly GameObject drinkBubble;

        [Inject]
        public StartGameState(IStateMachine stateMachine, ICharacterPool charactersPool,
            CharacterMovement characterMovement,
            LevelManager.LevelManager levelManager,
            [Inject(Id = "CharacterMovementPosition")]
            Transform characterMovementPosition,
            [Inject(Id = "DrinkBubble")] GameObject drinkBubble)
        {
            this.stateMachine = stateMachine;
            this.charactersPool = charactersPool;
            this.characterMovement = characterMovement;
            this.levelManager = levelManager;
            this.characterMovementPosition = characterMovementPosition;
            this.drinkBubble = drinkBubble;
        }

        public async void Enter()
        {
            GameObject character = charactersPool.GetRandom();
            SetUpDrinkBubble(character);
            characterMovement.ReparentCharacter(character);
            await characterMovement.Move(character);
            characterMovement.Stop(character).OnComplete(
                () => drinkBubble.transform.DOScale(new Vector3(.35f, .35f, .35f), .5f)
                    .SetEase(Ease.OutCubic));

            stateMachine.Enter<GamePlayState>();
        }

        private void SetUpDrinkBubble(GameObject character)
        {
            drinkBubble.transform.SetParent(character.transform);
            drinkBubble.transform.localPosition = new Vector3(-.2f, 1.7f, .1f);
            drinkBubble.transform.localRotation = new Quaternion(0, -180, 0, 0);
            drinkBubble.transform.localScale = Vector3.zero;
            drinkBubble.GetComponent<DrinkMaterialContainer>().Renderer.material.color = levelManager.GetNextLevelColor();
        }

        public void Exit()
        {
        }
    }
}