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
            ReparentCharacter(character);
            await characterMovement.MoveCharacter(character);
            CharacterStop(character).OnComplete(
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

        private Tween CharacterStop(GameObject character)
        {
            characterMovement.IdleCharacter(character);
            return character.transform.DOBlendableLocalRotateBy(new Vector3(0, 90, 0), 1f);
        }

        private void ReparentCharacter(GameObject character)
        {
            character.transform.SetParent(characterMovementPosition.GetChild(0));
            character.transform.localPosition = Vector3.zero;
            character.transform.SetParent(characterMovementPosition.GetChild(1));
        }

        public void Exit()
        {
        }
    }
}