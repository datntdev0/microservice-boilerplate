using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Identity.Web.Host.Components.Pages.Auth
{
    public partial class SignIn
    {
        private EditContext _editContext = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [SupplyParameterFromForm]
        private InputModel InputModel { get; set; } = new();

        [SupplyParameterFromQuery]
        private string ReturnUrl { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _editContext = new EditContext(InputModel);
        }

        private string GetInvalidClass(string fieldName)
        {
            var fieldIdentifier = new FieldIdentifier(InputModel, fieldName);
            return _editContext.IsValid(fieldIdentifier) ? string.Empty : "is-invalid";
        }

        private void HandleValidSubmit()
        {
            string defaultEmail = "admin@datntdev.com";
            string defaultPassword = "Admin@1234";

            // Simulate successful sign-in
            if (InputModel.Email == defaultEmail && InputModel.Password == defaultPassword)
            {
                NavigationManager.NavigateTo(ReturnUrl, true);
            }
        }
    }

    public class InputModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
