﻿using datntdev.Microservices.Identity.Application.Identity;
using datntdev.Microservices.Identity.Application.Identity.Models;
using datntdev.Microservices.Identity.Web.Host.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservices.Identity.Web.Host.Components.Pages.Auth
{
    public partial class SignIn
    {
        private readonly SweetAlertModel _sweetAlertOptions = new();

        private EditContext _editContext = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IdentityManager IdentityManager { get; set; } = default!;

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
            var loginResult = await IdentityManager.SignInWithPassword(
                Model.Email!, Model.Password!, HttpContext.RequestAborted);

            if (loginResult.Status == IdentityResultStatus.Success)
            {
                NavigationManager.NavigateTo(ReturnUrl, forceLoad: true);
            }
            else
            {
                _sweetAlertOptions.Title = "Login Failed";
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
