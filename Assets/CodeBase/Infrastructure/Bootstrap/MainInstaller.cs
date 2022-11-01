using CodeBase.Blender;
using CodeBase.Character;
using CodeBase.Food;
using CodeBase.Infrastructure.Common.Factory;
using CodeBase.Infrastructure.Common.StateMachine;
using CodeBase.Infrastructure.Data;
using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.GameStateMachine;
using CodeBase.Infrastructure.GameStateMachine.States;
using CodeBase.MixingLogic;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Bootstrap
{
    public class MainInstaller : MonoInstaller
    {
        private const string LevelColorDataPath = "LevelColorData";

        [SerializeField] private Transform foodContainer;
        [SerializeField] private Transform foodPosition;
        [SerializeField] private Transform foodInBlender;
        [SerializeField] private Transform waterParent;
        [SerializeField] private Transform charactersContainer;
        [SerializeField] private Transform characterMovement;
        [SerializeField] private Transform buttonsContainer;

        [SerializeField] private GameObject drinkBubble;
        [SerializeField] private GameObject mixPanel;
        [SerializeField] private GameObject colliderCurtain;
        [SerializeField] private GameObject blenderHead;
        [SerializeField] private GameObject blender;

        public override void InstallBindings()
        {
            BindServices();
            BindContainers();
            BindStateMachine();
            BindResources();
            BindGameObjects();
        }

        private void BindGameObjects()
        {
            Container.Bind<GameObject>().WithId("DrinkBubble").FromInstance(drinkBubble);
            Container.Bind<GameObject>().WithId("MixPanel").FromInstance(mixPanel);
            Container.Bind<GameObject>().WithId("ColliderCurtain").FromInstance(colliderCurtain);
            Container.Bind<GameObject>().WithId("BlenderHead").FromInstance(blenderHead);
            Container.Bind<GameObject>().WithId("Blender").FromInstance(blender);
        }

        private void BindResources()
        {
            Container.Bind<LevelColorData>().FromResources(LevelColorDataPath);
        }

        private void BindServices()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<ICharacterPool>().To<CharacterPool>().AsSingle();
            Container.Bind<IFoodPool>().To<FoodPool>().AsSingle();
            Container.Bind<EventReferer>().AsSingle();
            Container.Bind<MoveFoodToBlender>().AsSingle();
            Container.Bind<CopyClickedFood>().AsSingle();
            Container.Bind<Blending>().AsSingle();
            Container.Bind<LevelManager.LevelManager>().AsSingle();
            Container.Bind<CharacterMovement>().AsSingle();
            Container.Bind<MixPanelColorSet>().AsSingle();
            Container.Bind<HeadOpener>().AsSingle();
            Container.Bind<BlenderShaking>().AsSingle();
            Container.Bind<FoodSetUp>().AsSingle();
            Container.Bind<ButtonActivator>().AsSingle();
        }

        private void BindContainers()
        {
            Container.Bind<Transform>().WithId("FoodContainer").FromInstance(foodContainer);
            Container.Bind<Transform>().WithId("FoodPosition").FromInstance(foodPosition);
            Container.Bind<Transform>().WithId("FoodInBlender").FromInstance(foodInBlender);
            Container.Bind<Transform>().WithId("WaterParent").FromInstance(waterParent);
            Container.Bind<Transform>().WithId("CharacterContainer").FromInstance(charactersContainer);
            Container.Bind<Transform>().WithId("CharacterMovementPosition").FromInstance(characterMovement);
            Container.Bind<Transform>().WithId("ButtonsContainer").FromInstance(buttonsContainer);
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            BindStates();
        }

        private void BindStates()
        {
            Container.Bind<IState>().WithId("InitializeState").To<InitializeState>().AsSingle();
            Container.Bind<IState>().WithId("StartGameState").To<StartGameState>().AsSingle();
            Container.Bind<IState>().WithId("GamePlayState").To<GamePlayState>().AsSingle();
            Container.Bind<IState>().WithId("NextLevelState").To<NextLevelState>().AsSingle();
        }
    }
}