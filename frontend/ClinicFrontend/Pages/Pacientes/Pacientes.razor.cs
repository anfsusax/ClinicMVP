using ClinicFrontend.Models;
using ClinicFrontend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace ClinicFrontend.Pages.Pacientes;

public partial class Pacientes : ComponentBase
{
    [Inject] private ClinicApi Api { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Load();
    }

    // UI state
    protected bool loading;
    protected List<PacienteReadDto> items = new();

    protected string? filterNome;
    protected string? filterDocumento;

    protected bool showForm;
    protected int formId;
    protected string formNome = string.Empty;
    protected string? formDocumento;
    protected DateOnly? formDataNascimento;
    protected string? formTelefone;
    protected string? formEmail;

    protected object form => new { formNome, formDocumento, formDataNascimento, formTelefone, formEmail };

    protected async Task Load()
    {
        loading = true;
        items = await Api.GetPacientesAsync(filterNome, filterDocumento);
        loading = false;
        StateHasChanged();
    }

    protected void New()
    {
        formId = 0;
        formNome = string.Empty;
        formDocumento = null;
        formDataNascimento = null;
        formTelefone = null;
        formEmail = null;
        showForm = true;
    }

    protected void Edit(PacienteReadDto p)
    {
        formId = p.Id;
        formNome = p.Nome;
        formDocumento = p.Documento;
        formDataNascimento = p.DataNascimento;
        formTelefone = p.Telefone;
        formEmail = p.Email;
        showForm = true;
    }

    protected async Task Save()
    {
        if (string.IsNullOrWhiteSpace(formNome))
            return;

        if (formId == 0)
        {
            var dto = new PacienteCreateDto(formNome, formDocumento, formDataNascimento, formTelefone, formEmail);
            _ = await Api.CreatePacienteAsync(dto);
        }
        else
        {
            var dto = new PacienteUpdateDto(formNome, formDocumento, formDataNascimento, formTelefone, formEmail);
            _ = await Api.UpdatePacienteAsync(formId, dto);
        }
        showForm = false;
        await Load();
    }

    protected void Cancel()
    {
        showForm = false;
    }

    protected async Task Delete(PacienteReadDto p)
    {
        if (await JsConfirm($"Deseja excluir o paciente #{p.Id} - {p.Nome} ?"))
        {
            var ok = await Api.DeletePacienteAsync(p.Id);
            if (ok)
                await Load();
        }
    }

    private async Task<bool> JsConfirm(string message)
    {
        return await JS.InvokeAsync<bool>("confirm", message);
    }
}
