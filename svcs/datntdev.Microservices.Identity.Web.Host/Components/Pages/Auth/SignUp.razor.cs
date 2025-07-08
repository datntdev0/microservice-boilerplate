using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Web.Host.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservices.Identity.Web.Host.Components.Pages.Auth
{
    public partial class SignUp
    {
        private readonly SweetAlertModel _sweetAlertOptions = new();
        
        private EditContext _editContext = default!;

        [Inject]    
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private UserManager<AppUserEntity> UserManager { get; set; } = default!;

        [SupplyParameterFromForm]
        private InputModel Model { get; set; } = new();

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

        private async Task HandleValidSubmit()
        {
            var newUser = new AppUserEntity
            {
                UserName = Model.Email,
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
            };
            var result = await UserManager.CreateAsync(newUser, Model.Password!);

            if (result.Succeeded)
            {
                NavigationManager.NavigateTo("/auth/signin", forceLoad: true);
            }
            else
            {
                _sweetAlertOptions.Title = "Sign Up Failed";
                _sweetAlertOptions.Text = string.Join(", ", result.Errors.Select(e => e.Description));
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
            [DataType(DataType.Text)]
            public string? FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string? LastName { get; set; }

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
