using datntdev.Microservices.Identity.Contracts;
using datntdev.Microservices.Identity.Web.Host.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace datntdev.Microservices.Identity.Web.Host.Components.Pages.Auth
{
    public partial class SignIn
    {
        private readonly SweetAlertModel _sweetAlertOptions = new();

        private EditContext _editContext = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

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

        private async Task HandleValidSubmitAsync()
        {
            if (Model.Password == Constants.AdminPassword)
            {
                var claims = new Claim[] { new(ClaimTypes.Name, Model.Email!) };
                var claimsIdentity = new ClaimsIdentity(claims, Constants.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                NavigationManager.NavigateTo(ReturnUrl, forceLoad: true);
            }
            else
            {
                _sweetAlertOptions.Title = "Sign In Failed";
                _sweetAlertOptions.Text = "Invalid login attempt. Please try again.";
                _sweetAlertOptions.Icon = "error";
            }
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Invalid email address format.")]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }
        }
    }
}
