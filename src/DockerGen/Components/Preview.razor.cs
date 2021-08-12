using BlazorMonaco;
using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Components
{
	public partial class Preview : ComponentBase
	{
		private MonacoEditor _editor;
		[CascadingParameter]
		public Pages.Index ContainerEditor { get; set; }
		[Inject]
		private ILogger<Preview> _logger { get; set; }
		protected override void OnInitialized()
		{
			ContainerEditor.OnImageChanged += ContainerEditor_OnImageChanged;
			ContainerEditor.Container.OnImageChanged += ContainerChanged;
		}

		private void ContainerEditor_OnImageChanged(object sender, ContainerImageEventArgs e)
		{
			ContainerEditor.Container.OnImageChanged += ContainerChanged;
		}

		private async Task UpdateContainer(KeyboardEvent _)
		{
			var dockerfile = await _editor.GetValue();
			try
			{
				ContainerImage image = dockerfile;
				if (image != null)
				{
					ContainerEditor.Container = dockerfile;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to import docker instructions");
			}
		}

		private void ContainerChanged(object sender, Container.ContainerImageEventArgs e)
		{
			_editor.SetValue(GetCompiledDockerfile());
			StateHasChanged();
		}

		public string GetCompiledDockerfile()
		{
			var dockerFile = ContainerEditor.Container.Compile();
			if (string.IsNullOrEmpty(dockerFile))
			{
				return "# No instructions found - try adding one now or copy/paste an existing Dockerfile!";
			}

			return dockerFile;
		}

		private StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor _)
		{
			return new StandaloneEditorConstructionOptions
			{
				AutomaticLayout = true,
				FormatOnType = true,
				FormatOnPaste = true,
				Language = "dockerfile",
				Value = GetCompiledDockerfile(),
				Minimap = new EditorMinimapOptions { Enabled = false }
			};
		}
	}
}
