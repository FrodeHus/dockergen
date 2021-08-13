using Blazored.LocalStorage;
using Fluxor;

namespace DockerGen.Features.Container.Store
{
    public class ContainerEffects
    {
        private readonly ILocalStorageService _localStorageService;
        private const string ContainerStatePersistenceName = "DockerGen_ContainerState";
        public ContainerEffects(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        [EffectMethod]
        public async Task PersistState(ContainerPersistStateAction action, IDispatcher dispatcher)
        {
            try
            {
                await _localStorageService.SetItemAsync(ContainerStatePersistenceName, action.State);
            }
            catch (Exception ex)
            {

                dispatcher.Dispatch(new ContainerPersistStateFailureAction(ex.Message));
            }
        }

        [EffectMethod(typeof(ContainerLoadStateAction))]
        public async Task LoadState(IDispatcher dispatcher)
        {
            try
            {
                var state = await _localStorageService.GetItemAsync<ContainerState>(ContainerStatePersistenceName);
                if (state is not null)
                {
                    dispatcher.Dispatch(new ContainerSetStateAction(state));
                    dispatcher.Dispatch(new ContainerLoadStateSuccessAction());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                dispatcher.Dispatch(new ContainerLoadStateFailureAction(ex.Message));
            }
        }

        [EffectMethod(typeof(ContainerClearStateAction))]
        public async Task ClearState(IDispatcher dispatcher)
        {
            try
            {
                await _localStorageService.RemoveItemAsync(ContainerStatePersistenceName);
                dispatcher.Dispatch(new ContainerSetStateAction(new ContainerState { Container = new DockerGen.Container.ContainerImage() }));
                dispatcher.Dispatch(new ContainerClearStateSuccessAction());
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new ContainerClearStateFailureAction(ex.Message));
            }
        }
    }
}
