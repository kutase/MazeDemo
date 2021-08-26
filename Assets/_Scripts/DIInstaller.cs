using UnityEngine;
using Zenject;

namespace MazeDemo
{
    public class DIInstaller : MonoInstaller
    {
        [SerializeField]
        private GameManager gameManager;

        public override void InstallBindings()
        {
            Container.Bind<MazeRenderer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PathRenderer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Character>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IMazeProvider>().FromInstance(gameManager).AsSingle();
        }
    }
}
