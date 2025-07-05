using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservices.Identity.Web.Host.Components.Pages.Auth
{
    public partial class SignUp
    {
        private EditContext _editContext = default!;

        [Inject]    
        private NavigationManager NavigationManager { get; set; } = default!;

        [SupplyParameterFromForm]
        private InputModel Model { get; set; } = new();

        [SupplyParameterFromQuery]
        private string ReturnUrl { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _editContext = new EditContext(Model);
        }

        private string GetInvalidClass(string fieldName)
        {
            var fieldIdentifier = new FieldIdentifier(Model, fieldName);
            return _editContext.IsValid(fieldIdentifier) ? string.Empty : "is-invalid";
        }

        private void HandleValidSubmit()
        {
            // Simulate successful sign-up
            NavigationManager.NavigateTo(ReturnUrl, true);
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Invalid email address format.")]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string? DisplayName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }
        }
    }
}
