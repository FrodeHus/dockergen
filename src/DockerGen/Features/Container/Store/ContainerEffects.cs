using Blazored.LocalStorage;
using DockerGen.Container;
using DockerGen.Infrastructure;
using Fluxor;

namespace DockerGen.Features.Container.Store
{
    public class ContainerEffects
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ApiService _apiClient;
        private const string ContainerStatePersistenceName = "DockerGen_ContainerState";
        public ContainerEffects(ILocalStorageService localStorageService, ApiService apiClient)
        {
            _localStorageService = localStorageService;
            _apiClient = apiClient;
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

        [EffectMethod]
        public async Task LoadQuickLink(ContainerLoadQuickLinkAction action, IDispatcher dispatcher)
        {
            try
            {
                var image = await _apiClient.LoadFromQuickLinkAsync(action.QuickLinkId);
                dispatcher.Dispatch(new ContainerSetStateAction(new ContainerState { Container = image }));
                dispatcher.Dispatch(new ContainerLoadQuickLinkSuccess());
            }
            catch (Exception)
            {
                dispatcher.Dispatch(new ContainerLoadQuickLinkFailed("Something went wrong when opening quick link"));
            }
        }
        [EffectMethod(typeof(LoadRecipesAction))]
        public async Task LoadRecipes(IDispatcher dispatcher)
		{
			try
			{
                var recipes = await _apiClient.LoadRecipesAsync();
                dispatcher.Dispatch(new SetRecipesAction(recipes));
			}
			catch
			{

			}
		}

        [EffectMethod]
        public Task LoadFromDockerfile(ContainerLoadDockerfileFromStringAction action, IDispatcher dispatcher)
        {
            try
            {
                var image = ContainerImage.ParseFromString(action.dockerfile);
                
                dispatcher.Dispatch(new ContainerSetStateAction(new ContainerState { Container = image }));
                dispatcher.Dispatch(new ContainerLoadDockerfileFromStringSuccessAction());
            }
            catch(ArgumentException ex)
            {
                dispatcher.Dispatch(new ContainerLoadDockerfileFromStringFailureAction(ex.Message));
            }
            catch 
            {
                dispatcher.Dispatch(new ContainerLoadDockerfileFromStringFailureAction("Unknown error while importing Dockerfile"));
            }
            return Task.CompletedTask;
        }
    }
}
